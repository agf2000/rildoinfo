
$(function () {

    my.davId = 0;
    my.estimateId = my.getParameterByName('estimateId');
    my.personId = my.getParameterByName('personId');
    my.itemsSearchBox = $("#itemsSearchBox").kendoAutoComplete();
    my.uId = null;
    my.lineTotal = 0;
    my.lineTotalDiscount = 0;
    my.record = 0;
    my.originalTotal = 0;
    my.unitValue = 0;

    var loginValidator = $("#divLogin").kendoValidator().data("kendoValidator");
    // emailValidator = $("#divEmail").kendoValidator().data("kendoValidator");

    $("input[type=text]").on('focusin', function () {
        var saveThis = $(this);
        window.setTimeout(function () {
            saveThis.select();
        }, 100);
    });

    $('textarea').autogrow();
    $('textarea').css('overflow', 'hidden').autogrow();

    //$('#messageBody').css({ 'width': '90% !important', 'height': '150px' }).attr({ 'cols': '10', 'rows': '7' });

    $('#selectDeleteReason').kendoDropDownList();

    my.viewModel();

    if (orLogoFile !== '') {
        $('#rildoLogo').attr({ 'src': orLogoURL });
    }

    //my.vm.loadStatuses();

    $('#btnSaveDav').kendoButton();

    my.loadEstimate = function () {
        my.estimateId = my.getParameterByName('estimateId');
        my.vm.selectedPayFormId(null);
        my.vm.displayTotal('TOTAL = 0,00');
        my.vm.prodTotal('0 x 0,00 = 0,00');
        $('[data-bind*="displayTotal"]').css({ color: '#FFF' });
        $('#divPayForms').show();
        $('#divPayFormCond').hide();
        $('#divChosenPayCond').hide();
        $('#payCondMsg').html('<small><i>Escolha uma outra forma de pagamento (Ctrl+7).</i></small>');
        if (my.estimateId > 0) {
            $.getJSON('/desktopmodules/riw/api/estimates/GetQuickEstimate?estimateId=' + my.estimateId + '&portalId=' + portalID, function (estimateData) {
                if (estimateData.Estimate) {
                    my.vm.estimateId(estimateData.Estimate.EstimateId);
                    my.vm.estimatedByUser(estimateData.Estimate.SalesRep);
                    my.vm.estimateStatus(estimateData.Estimate.StatusTitle);
                    my.vm.selectedStatusId(estimateData.Estimate.StatusId);
                    my.vm.personId(estimateData.Estimate.PersonId);
                    $("#clientSearch").select2("val", estimateData.Estimate.ClientDisplayName);
                    if (estimateData.Estimate.StatusId === 10) {
                        $("#newORClientWindow").append("<div id='divAlert'>Aten&#231;&#227;o, este or&#231;amento foi finalizado.</div>");
                        var kendoWindow = $('#divAlert').kendoWindow({
                            title: 'Alerta',
                            modal: true,
                            width: 200,
                            height: 100,
                            close: function (e) {
                                $("html, body").css("overflow", "");
                            },
                            deactivate: function () {
                                this.destroy();
                            }
                        });

                        kendoWindow.data("kendoWindow").center().open();
                    }
                    //my.vm.comments(estimateData.Estimate.Comment);

                    if (estimateData.Estimate.Comment !== '') {
                        $('#commentsTextArea').html(estimateData.Estimate.Comment);
                    } else {
                        $('#commentsTextArea').html('Assinatura: X<br />Reconheco o debito indicado acima.<br />Obrigado pela referencia, volte sempre!');
                    }

                    //$.getJSON('/desktopmodules/riw/api/estimates/getEstimateItems?portalId=' + portalID + '&estimateId=' + my.estimateId + '&lang=pt-BR', function (data) {
                    //    if (data.Result.indexOf('success') !== -1) {
                    my.vm.items.removeAll();
                    my.lineTotal = 0;
                    my.lineTotalDiscount = 0;
                    my.vm.subTotal(0);
                    my.vm.grandTotal(0);
                    //my.vm.originalProdTotal(0);
                    $.each(estimateData.Estimate.EstimateItems, function (i, item) {
                        my.vm.items.push({
                            EstimateItemId: item.EstimateItemId,
                            ProductId: item.ProductId,
                            Barcode: item.Barcode.length > 0 ? item.Barcode : item.ProductRef.length > 0 ? item.ProductRef : '0000000000000',
                            ProductName: item.ProductName.length > 29 ? item.ProductName.substr(0, 29) + ' ...' : item.ProductName,
                            ProductQty: item.ProductQty,
                            QtyStockSet: item.QtyStockSet,
                            UnitTypeTitle: item.UnitTypeAbbv,
                            UnitValue: item.UnitValue,
                            ProductDiscountPerc: item.ProductDiscount,
                            ProductDiscount: item.ProductDiscount > 0 ? (item.UnitValue * item.ProductQty) * (item.ProductDiscount) / 100 : 0,
                            ExtendedAmount: item.ExtendedAmount,
                            Summary: item.Summary,
                            //Description: item.Description,
                            ProductImageId: item.ProductImageId
                        });

                        my.vm.originalProdTotal(my.vm.originalProdTotal() + (kendo.parseFloat(item.ProductQty) * item.UnitValue));
                        my.lineTotal = my.lineTotal + item.ExtendedAmount;
                        my.lineTotalDiscount = my.lineTotalDiscount + (item.ProductDiscount > 0 ? (item.UnitValue * item.ProductQty) * (item.ProductDiscount) / 100 : 0);
                    });

                    if ((estimateData.Estimate.TotalPayCond !== 0) && (estimateData.Estimate.TotalPayCond >= my.lineTotal)) {
                        my.vm.displayTotal('TOTAL = ' + kendo.toString(estimateData.Estimate.TotalPayCond, "n"));
                        //$('[data-bind*="displayTotal"]').css({ color: 'lightgreen' });
                        my.vm.subTotal(estimateData.Estimate.TotalPayCond);
                        my.vm.grandTotal(estimateData.Estimate.TotalPayCond);
                    }
                    else {
                        my.vm.subTotal(my.lineTotal - (my.lineTotal * my.vm.couponDiscount() / 100));
                        my.vm.grandTotal(my.lineTotal - (my.lineTotal * my.vm.couponDiscount() / 100));
                        my.vm.displayTotal('TOTAL = ' + kendo.toString(my.lineTotal - (my.lineTotal * (estimateData.Estimate.Discount > 0 ? estimateData.Estimate.Discount / 100 : 0)), "n"));
                        my.vm.subTotal(my.lineTotal - (my.lineTotal * (estimateData.Estimate.Discount > 0 ? estimateData.Estimate.Discount / 100 : 0)));
                        my.vm.grandTotal((my.lineTotal) - (my.lineTotal * (estimateData.Estimate.Discount > 0 ? estimateData.Estimate.Discount / 100 : 0)));
                    }

                    if (estimateData.Estimate.PayCondId > 0) {
                        my.vm.selectedPayForm(estimateData.Estimate.PayCondType);
                        my.vm.conditionPayIn(kendo.parseFloat(estimateData.Estimate.PayCondIn));
                        my.vm.conditionPayInDays(estimateData.Estimate.PayInDays);
                        my.vm.payInMin(estimateData.Estimate.PayCondIn);
                        my.vm.conditionNumberPayments(estimateData.Estimate.PayCondN);
                        my.vm.conditionInterest(estimateData.Estimate.PayCondPerc);
                        $('#interval').text(estimateData.Estimate.PayCondInterval < 0 ? function () { $('#payIn').attr({ 'readonly': true, 'title': 'Desativado' }); my.vm.conditionInterval(estimateData.PayCondInterval); return 'A Vista'; } : estimateData.PayCondInterval > 0 ? estimateData.PayCondInterval : 'Mensal');
                        $('#divPayForms').hide();
                        $('#divPayFormCond').hide();
                        $('#divChosenPayCond').show();
                        if (estimateData.Estimate.PayCondIn > 0) {
                            $('#divPayIn').show();
                            $('#btnSavePayFormCond').show();
                        }
                        //my.vm.displayTotal('TOTAL = ' + kendo.toString(my.originalTotal, "n"));
                        //my.vm.subTotal(my.originalTotal);
                        //my.vm.grandTotal(my.originalTotal);
                    } else {
                        $.ajax({
                            url: '/desktopmodules/riw/api/payconditions/getPayConds?portalId=' + portalID + '&pcType=0&pcStart=0'
                        }).done(function (payCond) {
                            if (payCond) {
                                if (payCond.length > 0) {
                                    if (payCond[0].PayCondTitle) {
                                        if (payCond[0].PayCondTitle.length > 0) {
                                            $('#payCondMsg').html(payCond[0].PayCondTitle + ' ' + $('#payCondMsg').html());
                                        }
                                        if (payCond[0].PayCondDisc > 0) {
                                            my.vm.payCondDisc(payCond[0].PayCondDisc);
                                            my.originalTotal = my.lineTotal;
                                            my.vm.displayTotal('TOTAL = ' + kendo.toString(my.lineTotal - (my.lineTotal * payCond[0].PayCondDisc / 100), "n"));
                                            my.vm.subTotal(my.lineTotal - (my.lineTotal * payCond[0].PayCondDisc / 100));
                                            my.vm.grandTotal(my.lineTotal - ((my.lineTotal * payCond[0].PayCondDisc / 100)));
                                        }
                                    }
                                }
                            } else {
                                $.pnotify({
                                    title: 'Erro!',
                                    text: data.Result,
                                    type: 'error',
                                    icon: 'fa fa-times-circle fa-lg',
                                    addclass: "stack-bottomright",
                                    stack: my.stack_bottomright
                                });
                            }
                        }).fail(function (jqXHR, textStatus) {
                            console.log(jqXHR.responseText);
                        });
                    };

                    $('.selectable').on('dblclick', function () {
                        $('#btnDiscount').click();
                    });

                    my.vm.couponDiscount(estimateData.Estimate.Discount);
                    $('#couponDiscountTextBox').data("kendoNumericTextBox").value(my.vm.couponDiscount());
                    //if (my.vm.couponDiscount() > 0) {
                    //    if (my.vm.displayTotal().indexOf('Desconto') === -1) {
                    //        my.vm.displayTotal(my.vm.displayTotal() + '<span style="font-size: 14px; line-height: 14px; color: orange;"> Desconto </span>');
                    //        //} else {
                    //        //    $('[data-bind*="displayTotal"]').append('<span style="font-size: 14px; line-height: 14px; color: orange;">D</span>');
                    //    }
                    //}

                    //$('#btnRestart').attr({ 'title': 'Clique para iniciar um novo', 'disabled': false }).removeClass('k-state-disabled');
                    my.authorizationCheck();

                    //    }
                    //});
                }
            });
        }
    };

    my.loadClient = function () {
        my.ad = my.getParameterByName('personId');
        if (my.personId === 0) my.personId = my.vm.personId();
        if (my.personId > 0) {
            $.getJSON('/desktopmodules/riw/api/people/getPerson?personId=' + my.personId, function (clientData) {
                if (clientData) {
                    my.vm.personId(clientData.PersonId);
                    my.vm.displayName(clientData.DisplayName);
                    my.vm.firstName(clientData.FirstName);
                    my.vm.lastName(clientData.LastName);
                    my.vm.telephone(clientData.Telephone);
                    my.vm.email(clientData.Email);
                    my.vm.street(clientData.Street);
                    my.vm.unit(clientData.Unit);
                    my.vm.complement(clientData.Complement);
                    my.vm.district(clientData.District);
                    my.vm.city(clientData.City);
                    my.vm.region(clientData.Region);
                    my.vm.salesRepName(clientData.SalesRepName);
                    my.vm.salesRepEmail(clientData.SalesRepEmail);
                    my.vm.salesRepPhone(clientData.SalesRepPhone);
                    my.vm.blocked(clientData.Blocked);
                    my.vm.reasonBlocked(clientData.ReasonBlocked);

                    if (my.vm.blocked()) {
                        //$('#clientEdit span:first').html('<strong><span class="isDeleted">CR&#201;DITO BLOQUEADO</span></strong> &nbsp; ' + $('#clientEdit span:first').html());
                        if (my.vm.reasonBlocked() !== '') {
                            $("#newORClientWindow").append("<div id='divAlert'>" + my.vm.reasonBlocked() + "</div>");
                            var kendoWindow = $('#divAlert').kendoWindow({
                                title: 'Alerta',
                                modal: true,
                                width: 300,
                                height: 150,
                                close: function (e) {
                                    $("html, body").css("overflow", "");
                                },
                                deactivate: function () {
                                    this.destroy();
                                }
                            });

                            kendoWindow.data("kendoWindow").center().open();
                        }
                        $('#bankInTextBox').attr('readonly', true);
                        $('#bankInTextBox').addClass('input-disabled');
                        $('#checkInTextBox').attr('readonly', true);
                        $('#checkInTextBox').addClass('input-disabled');
                    } else {
                        $('#clientEdit span:first').html('Alt+C - <strong>Cliente: </strong>');
                    }

                    if (clientData.UserId > 0) {
                        $('[data-bind*="personId"]').css({ color: 'blue' });
                        my.vm.clientUserId(clientData.UserId);
                        my.vm.hasLogin(true);
                    } else {
                        $('[data-bind*="personId"]').css({ color: 'red' });
                    }

                    $('#spanClientInfo').html('<div class="span6"><dl class="dl-horizontal">'
                    + '<dt>Cliente:</dt><dd>' + clientData.DisplayName + '</dd>'
                    + (clientData.Email ? ('<dt>Email:</dt><dd>' + clientData.Email + '</dd>') : '')
                    + (clientData.Telephone ? ('<dt>Telefone:</dt><dd>' + my.formatPhone(clientData.Telephone) + '</dd>') : '')
                    + (clientData.Cell ? ('<dt>Celular:</dt><dd>' + my.formatPhone(clientData.Cell) + '</dd>') : '')
                    + (clientData.Fax ? ('<dt>Fax:</dt><dd>' + my.formatPhone(clientData.Fax) + '</dd>') : '')
                    + (clientData.Zero800s ? ('<dt>0800:</dt><dd>' + my.formatPhone(clientData.Zero800s) + '</dd></dl></div>') : ''));
                    if (clientData.Street) {
                        $('#spanClientInfo').html($('#spanClientInfo').html() + '<div class="span6"><address>'
                        + clientData.Street + ' N&#186; ' + clientData.Unit + '<br />'
                        + (clientData.Complement ? (clientData.Complement + '<br />') : '')
                        + (clientData.District ? ('<strong>Bairro:</strong> ' + clientData.District + '<br />') : '')
                        + (clientData.City ? (clientData.City + ', ') : '')
                        + (clientData.Region ? (clientData.Region + (clientData.PostalCode ? (' <strong>CEP:</strong> ' + clientData.PostalCode) : '') + '<br />') : '')
                        + (clientData.Country ? (clientData.Country + '</address></div>') : ''));
                    } else {
                        $('#spanClientInfo').html($('#spanClientInfo').html() + '<div class="span6"></div>');
                    }

                    $("#clientSearch").select2("val", clientData.DisplayName);

                    //history.pushState("", document.title, window.location.pathname);
                    //my.clientSearchBox();
                    //$('#searchArea').fadeIn();
                    //$('#clientInfo').show();
                    ////$('#clientArea').slideToggle();
                    //$(my.itemsSearchBox).focus();
                    //setTimeout(function () {
                    //    if (!$('#clientArea').is(':hidden')) $('#clientArea').kendoAnimate({ effects: "slide:up fade:out", hide: true });
                    //}, 3000);
                }
            });
        }
    };

    my.getClient = function () {
        $("html, body").css("overflow", "");
        my.loadClient();
    };

    $("#clientSearch").select2({
        placeholder: "Busque por clientes...",
        //allowClear: true,
        minimumInputLength: 3,
        id: function (data) {
            return {
                PersonId: data.PersonId,
                DisplayName: data.DisplayName,
                Email: data.Email,
                Telephone: data.Telephone,
                Cell: data.Cell,
                Fax: data.Fax,
                Zero800s: data.Zero800s,
                Street: data.Street,
                Complement: data.Complement,
                District: data.District,
                City: data.City,
                Region: data.Region,
                Country: data.Country,
                PostalCode: data.PostalCode,
                Unit: data.Unit,
                SalesRepName: data.SalesRepName,
                SalesRepEmail: data.SalesRepEmail,
                SalesRepPhone: data.SalesRepPhone,
                Blocked: data.Blocked,
                ReasonBlocked: data.ReasonBlocked
            };
        },
        ajax: {
            url: "/desktopmodules/riw/api/people/getpeople",
            quietMillis: 100,
            data: function (term, page) { // page is the one-based page number tracked by Select2
                return {
                    portalId: portalID,
                    registerType: clientRoleId,
                    isDeleted: false,
                    searchField: 'FirstName',
                    sTerm: term,
                    pageIndex: page,
                    pageSize: 10,
                    orderBy: 'FirstName',
                    orderDesc: 'ASC'
                };
            },
            results: function (data, page) {
                var more = (page * 10) < data.total; // whether or not there are more results available

                // notice we return the value of more so Select2 knows if more results can be loaded
                return { results: data.data, more: more };
            }
        },
        formatResult: clientFormatResult, // omitted for brevity, see the source of this page
        formatSelection: clientFormatSelection, // omitted for brevity, see the source of this page
        dropdownCssClass: "bigdrop", // apply css that makes the dropdown taller
        escapeMarkup: function (m) { return m; }, // we do not want to escape markup since we are displaying html in results
        initSelection: function (element, callback) {
            var text = element.val();
            var data = { DisplayName: text };
            callback(data);
        }
    });

    $('#clientSearch').on("select2-selecting", function (e) {
        my.vm.personId(e.val.PersonId);
        my.vm.displayName(e.val.DisplayName);
        my.vm.firstName(e.val.FirstName);
        my.vm.lastName(e.val.LastName);
        my.vm.telephone(e.val.Telephone);
        my.vm.email(e.val.Email);
        my.vm.street(e.val.Street);
        my.vm.unit(e.val.Unit);
        my.vm.complement(e.val.Complement);
        my.vm.district(e.val.District);
        my.vm.city(e.val.City);
        my.vm.region(e.val.Region);
        my.vm.salesRepName(e.val.SalesRepName);
        my.vm.salesRepEmail(e.val.SalesRepEmail);
        my.vm.salesRepPhone(e.val.SalesRepPhone);
        my.vm.blocked(e.val.Blocked);
        my.vm.reasonBlocked(e.val.ReasonBlocked);

        if (my.vm.blocked()) {
            $('#clientEdit span:first').html('<strong><span class="isDeleted">CR&#201;DITO BLOQUEADO</span></strong> &nbsp; ' + $('#clientEdit span:first').html());
            if (my.vm.reasonBlocked() !== '') {
                $("#newORClientWindow").append("<div id='divAlert'>" + my.vm.reasonBlocked() + "</div>");
                var kendoWindow = $('#divAlert').kendoWindow({
                    title: 'Alerta',
                    modal: true,
                    width: 300,
                    height: 150,
                    close: function (e) {
                        $("html, body").css("overflow", "");
                    },
                    deactivate: function () {
                        this.destroy();
                    }
                });

                kendoWindow.data("kendoWindow").center().open();
            }
        } else {
            $('#clientEdit span:first').html('Alt+C - <strong>Cliente: </strong>');
        }

        if (e.val.UserId > 0) {
            $('[data-bind*="personId"]').css({ color: 'blue' });
            my.vm.clientUserId(e.val.UserId);
            my.vm.hasLogin(true);
        } else {
            $('[data-bind*="personId"]').css({ color: 'red' });
        }

        $('#spanClientInfo').html('<div class="pull-left"><dl class="dl-horizontal">'
                    + '<dt>Cliente:</dt><dd>' + e.val.DisplayName + '</dd>'
                    + (e.val.Email ? ('<dt>Email:</dt><dd>' + e.val.Email + '</dd>') : '')
                    + (e.val.Telephone ? ('<dt>Telefone:</dt><dd>' + my.formatPhone(e.val.Telephone) + '</dd>') : '')
                    + (e.val.Cell ? ('<dt>Celular:</dt><dd>' + my.formatPhone(e.val.Cell) + '</dd>') : '')
                    + (e.val.Fax ? ('<dt>Fax:</dt><dd>' + my.formatPhone(e.val.Fax) + '</dd>') : '')
                    + (e.val.Zero800s ? ('<dt>0800:</dt><dd>' + my.formatPhone(e.val.Zero800s) + '</dd></dl></div>') : ''));
        if (e.val.Street) {
            $('#spanClientInfo').html($('#spanClientInfo').html() + '<div class="pull-left"><dl class="dl-horizontal">'
            + '<dt>Rua:</dt><dd>' + e.val.Street + ' N&#186; ' + e.val.Unit + '</dd>'
            + (e.val.Complement ? ('<dt>&nbsp;</dt><dd>' + e.val.Complement + '</dd>') : '')
            + (e.val.District ? ('<dt>Bairro:</dt><dd>' + e.val.District + '</dd>') : '')
            + (e.val.City ? ('<dt>Cidade:</dt><dd>' + e.val.City + '</dd>') : '')
            + (e.val.Region ? ('<dt>Estado:</dt><dd>' + e.val.Region + (e.val.PostalCode ? (' ' + e.val.PostalCode) : '') + '</dd>') : '')
            + (e.val.Country ? ('<dt>Pa&#237;s:</dt><dd>' + e.val.Country + '</dd></dl></div>') : ''));
        }
        $('#spanClientInfo').html($('#spanClientInfo').html() + '<div class="clearfix"></div>');
        //$('#divClientSearch').fadeOut();
        //$('#btnCancelClientReplace').fadeOut();
        //$('#spanClientInfo').delay(200).fadeIn()

        if (my.vm.estimateId() > 0) {
            var params = {
                EstimateId: my.vm.estimateId(),
                PersonId: e.val.PersonId,
                ModifieByUser: userID,
                ModifiedOnDate: moment().format()
            };

            $.ajax({
                type: 'PUT',
                url: '/desktopmodules/riw/api/estimates/updateEstimateClient',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    document.location.hash = 'estimateId/' + my.vm.estimateId() + '/personId/' + e.val.PersonId;
                    my.vm.personId(e.val.PersonId);
                    //$().toastmessage('showSuccessToast', 'Cliente do or&#231;amento definido com sucesso!');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Cliente do or&#231;amento definido.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                } else {
                    $.pnotify({
                        title: 'Erro!',
                        text: data.Result,
                        type: 'error',
                        icon: 'fa fa-times-circle fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    //$().toastmessage('showErrorToast', data.Result);
                }
            }).fail(function (jqXHR, textStatus) {
                console.log(jqXHR.responseText);
            });
        }
    });

    $('#btnAddClient').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $("#newORClientWindow").append("<div id='windowOR'></div>");
        var sContent = editClientURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer',
            kendoWindow = $('#windowOR').kendoWindow({
                title: 'Novo Cadastro',
                modal: true,
                width: '90%',
                height: '80%',
                content: sContent,
                close: function (e) {
                    $("html, body").css("overflow", "");
                    my.getClient;
                },
                open: function () {
                    //$("html, body").css("overflow", "hidden");
                    $('#windowOR').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
                },
                deactivate: function () {
                    this.destroy();
                }
            });

        kendoWindow.data("kendoWindow").center().open();

        $.ajax({
            url: sContent, success: function (data) {
                kendoWindow.data("kendoWindow").refresh();
            }
        });
    });

    $('#btnEditClient').click(function (e) {
        e.preventDefault();

        $("#newORClientWindow").append("<div id='windowOR'></div>");
        var sContent = editClientURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.vm.personId() + '/sel/7/subSel/1',
            kendoWindow = $('#windowOR').kendoWindow({
                actions: ["Maximize", "Close"],
                title: 'Editar Cliente: ' + my.vm.displayName() + ' (ID: ' + my.vm.personId() + ')',
                modal: true,
                width: '90%',
                height: '80%',
                content: sContent,
                close: function (e) {
                    $("html, body").css("overflow", "");
                },
                open: function () {
                    //$("html, body").css("overflow", "hidden");
                    $('#windowOR').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
                },
                deactivate: function () {
                    this.destroy();
                }
            });

        kendoWindow.data("kendoWindow").center().open();

        $.ajax({
            url: sContent, success: function (data) {
                kendoWindow.data("kendoWindow").refresh();
            }
        });
    });

    $('#clientEdit').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();
        if (!$('#clientArea').is(':hidden')) {
            $('#clientArea').slideUp();
            $('#clientToggle').removeClass('k-i-arrow-n').addClass('k-i-arrow-s');
        } else {
            $('#clientArea').slideDown();
            $('#clientToggle').removeClass('k-i-arrow-s').addClass('k-i-arrow-n');
            $('#clientsSearchBox').focus();
        }
        //if (!$('#menuTop').is(':hidden')) {
        //    $('#menuTop').kendoAnimate({ effects: "slideOutUp:up fade:out", hide: true });
        //    $('#footer').kendoAnimate({ effects: "slide:up fade:out", hide: true });
        //    $('#Breadcrumb').kendoAnimate({ effects: "slide:up fade:out", hide: true });
        //} else {
        //    $('#menuTop').kendoAnimate({ effects: "slide:up fade:in" });
        //    $('#footer').kendoAnimate({ effects: "slide:up fade:in" });
        //    $('#Breadcrumb').kendoAnimate({ effects: "slide:up fade:in" });
        //}
    });

    my.addEstimateItem = function () {
        var products = [];
        if (my.vm.selectedProducts().length > 0) {
            $.each(my.vm.selectedProducts(), function (i, p) {
                products.push({
                    ProductId: p.productId(),
                    ProductQty: typeof p.itemQty() === 'number' ? kendo.parseFloat(p.itemQty()) : kendo.parseFloat(p.itemQty().replace('.', ',')),
                    ProductName: p.productName(),
                    ProductEstimateOriginalPrice: p.unitValue(),
                    ProductEstimatePrice: p.unitValue(),
                    CreatedByUser: userID,
                    CreatedOnDate: moment().format(),
                    ModifiedByUser: userID,
                    ModifiedOnDate: moment().format()
                });
            });
        } else {
            products.push({
                ProductId: my.vm.productId(),
                ProductQty: my.vm.itemQty().length > 0 ? kendo.parseFloat(my.vm.itemQty().replace('.', ',')) : '1',
                ProductName: my.vm.productName(),
                ProductEstimateOriginalPrice: my.vm.unitValue(),
                ProductEstimatePrice: my.vm.unitValue(),
                CreatedByUser: userID,
                CreatedOnDate: moment().format(),
                ModifiedByUser: userID,
                ModifiedOnDate: moment().format()
            });
        }

        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/estimates/addEstimateItem',
            data: JSON.stringify({
                EstimateItems: products,
                PortalId: portalID,
                EstimateId: my.vm.estimateId(),
                EstimateTitle: '',
                PersonId: my.vm.personId() > 0 ? my.vm.personId() : mainConsumerId,
                UserId: -1,
                StatusId: authorized >= 2 ? 5 : 1,
                Guid: my.Right(my.generateUUID(), 8),
                SalesRep: kendo.parseInt(authorized === 2 ? userID : salesPerson),
                ViewPrice: showEstimatePrice,
                TotalAmount: my.lineTotal + ((my.vm.itemQty().length > 0 ? kendo.parseFloat(my.vm.itemQty()) : 1) * my.vm.unitValue()),
                Comment: $('#commentsTextArea').val(), // my.vm.comments(),
                CreatedByUser: userID,
                CreatedOnDate: moment().format(),
                ModifiedByUser: userID,
                ModifiedOnDate: moment().format()
            })
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                if (my.vm.conditionNumberPayments() > 0) {
                    $('#btnCancelPayFormCond').click();
                }

                if (my.vm.selectedStatusId() === 5 || my.vm.selectedStatusId() === 6) {

                    var prodParams = [];
                    if (my.vm.selectedProducts().length > 0) {
                        $.each(my.vm.selectedProducts(), function (i, p) {
                            prodParams.push({
                                ProductId: my.vm.productId(),
                                ProductQty: my.vm.itemQty().length > 0 ? my.vm.itemQty() : '1',
                                ModifiedByUser: userID,
                                ModifiedOnDate: moment().format()
                            });
                        });
                    }

                    $.ajax({
                        type: 'PUT',
                        contentType: 'application/json; charset=utf-8',
                        url: '/desktopmodules/riw/api/estimates/takeProductStock',
                        data: JSON.stringify(prodParams)
                    });
                }

                if (my.vm.estimateId() === 0) {
                    my.vm.estimateId(data.Estimate.EstimateId);
                    history.pushState("", document.title, window.location.pathname);
                    document.location.hash = 'estimateId/' + data.Estimate.EstimateId + '/personId/' + (my.vm.personId() > 0 ? my.vm.personId() : mainConsumerId);
                }

                my.loadEstimate();
                my.getClient();

                //if (my.vm.estimateStatus() === 'Fechado' || my.vm.estimateStatus() === 'Pendente') {                    
                //    updateStock(prodParams);
                //}

                //my.vm.estimateItemId(msg.edId);
                //my.vm.items.unshift({
                //    EstimateItemId: msg.edId,
                //    EstimateId: my.estimateId,
                //    ProductId: my.vm.productId(),
                //    Barcode: my.vm.productCode(),
                //    ProductName: my.vm.productName().length > 40 ? my.vm.productName().substr(0, 40) + ' ...' : my.vm.productName(),
                //    Qty: my.vm.itemQty() > 0 ? my.vm.itemQty() : 1,
                //    UnitTypeTitle: my.vm.unitTitle(),
                //    UnitValue: my.vm.unitValue(),
                //    Discount: 0,
                //    TotalAmount: (my.vm.itemQty() > 0 ? my.vm.itemQty() : 1) * my.vm.unitValue(),
                //    Summary: my.vm.summary(),
                //    Description: my.vm.prodDesc(),
                //    ProductImageId: my.vm.prodImageId()
                //});

                //my.vm.prodTotal((my.vm.itemQty().length > 0 ? my.vm.itemQty() : '1') + ' x ' + my.vm.unitValue() + ' = ' + accounting.formatMoney((my.vm.itemQty().length > 0 ? kendo.parseFloat(my.vm.itemQty().replace('.', ',')) : 1) * my.vm.unitValue(), '$R', 2, '.', ','));
                my.vm.prodTotal((my.vm.itemQty().length > 0 ? kendo.parseFloat(my.vm.itemQty()) : '1') + ' x ' + my.vm.unitValue() + ' = ' + kendo.toString(Math.floor((my.vm.itemQty().length > 0 ? kendo.parseFloat(my.vm.itemQty().replace('.', ',')) : 1) * my.vm.unitValue() * 100) / 100, 'n'));
                my.lineTotal = my.lineTotal + ((my.vm.itemQty() > 0 ? my.vm.itemQty() : 1) * my.vm.unitValue());
                my.vm.grandTotal(my.lineTotal);
                my.vm.displayTotal('TOTAL = ' + my.lineTotal.toFixed(2));

                //$.post('/desktopmodules/riw/api/estimate/UpdateAmount', { eId: my.vm.estimateId(), amt: my.lineTotal, uId: userID, md: moment().format() }, function (data) { });
                //$().toastmessage('showSuccessToast', 'Item adicionado com sucesso ao o&#231;amento de n&#250;mero ' + my.vm.estimateId() + '.');
                if ($('.ui-pnotify').length === 0) {
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Item adicionado com sucesso ao o&#231;amento de n&#250;mero ' + my.vm.estimateId() + '.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                }
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', msg.Result);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            my.vm.selectedProducts.removeAll();
            my.vm.itemQty('');
            my.playAudio();
        });
    };

    my.itemsSearchBox.keypress(function (e) {
        if (this.value.length)
            if (e.keyCode === 13) {
                setTimeout(function () {
                    return true;
                }, 500);
            }
    });

    my.itemsSearchBox.kendoAutoComplete({
        delay: 1000,
        minLength: 3,
        dataTextField: 'ProductName',
        //placeholder: "Ctrl+P, Insira Nome, Refer&#234;ncia ou C&#243;digo.",
        template: '<strong>Produto: </strong><span>${ data.ProductName }</span><br /><strong>Ref: </strong><span>${ data.ProductRef }</span> <div class="pull-right"><strong>Estoque: </strong> <span>${ data.QtyStockSet } / ${ data.QtyStockOther }</span></div><strong>Cod. Barra: </strong><span>${ data.Barcode }</span><br /><strong>Valor: </strong><span> #= kendo.toString(data.UnitValue, "c") #</span>',
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/products/GetProducts'
                },
                parameterMap: function (data, type) {

                    var fieldName = data.filter.filters[0].value;

                    my.unitValue = 0;

                    if (fieldName.charAt(0) == '2' && fieldName.length == 13) {
                        my.unitValue = parseFloat(fieldName.substring(7, 12));
                    }

                    var theSearchStr = '';

                    if (fieldName.charAt(0) == '*' || fieldName.charAt(0) == '#') {
                        theSearchStr = fieldName.slice(1);
                    } else {
                        if (fieldName.charAt(0) == '2' && fieldName.length == 13) {
                            theSearchStr = fieldName.substring(7, 1);
                        } else {
                           theSearchStr = fieldName;
                        }
                    }

                    return {
                        portalId: portalID,
                        //categoryId: my.vm.categoryId(),
                        searchField: fieldName.charAt(0) == '*' ? 'ProductRef' : fieldName.charAt(0) == '#' ? 'BarCode' : fieldName.charAt(0) == '2' ? 'ProductRef' : 'ProductName',
                        searchString: theSearchStr,
                        getDeleted: 'False',
                        getArchived: 'False',
                        //providerList: '""',
                        //categoryList: '""',
                        //sDate: '',
                        //eDate: '',
                        //filterDate: '',
                        pageIndex: data.page,
                        pageSize: data.pageSize,
                    };
                }
            },
            schema: {
                model: {
                    id: 'ProductId',
                    fields: {
                        ProductId: {
                            editable: false, nullable: false
                        }
                    }
                },
                data: 'data',
                total: 'total'
            },
            pageSize: 1000,
            serverPaging: true,
            serverFiltering: true
        },
        highlightFirst: true,
        valuePrimitive: true,
        index: 0,
        filter: "startswith",
        filtering: function(e) {
            var filter = e.filter;

            if (!filter.value) {
                //prevent filtering if the filter does not value
                e.preventDefault();
            }
        },
        dataBound: function () {
            var autocomplete = this;
            switch (true) {
                case (this.dataSource.total() > 20):
                    //if (!$('.toast-item-wrapper').length) $().toastmessage('showSuccessToast', 'Dezenas de items encontrados... refina sua busca.');
                    if (!$('.ui-pnotify').length) {
                        $.pnotify({
                            title: 'Aten&#231;&#227;o!',
                            text: 'Dezenas de items encontrados...<br />...refina sua busca.',
                            type: 'info',
                            icon: 'fa fa-exclamation-circle fa-lg',
                            addclass: "stack-bottomright",
                            stack: my.stack_bottomright
                        });
                    }
                    break;
                case (this.dataSource.total() === 0):
                    autocomplete.value('');
                    //if (!$('.toast-item-wrapper').length) $().toastmessage('showWarningToast', 'Sua busca não trouxe resultado algum.');
                    if (!$('.ui-pnotify').length) {
                        $.pnotify({
                            title: 'Aten&#231;&#227;o!',
                            text: 'Sua busca n&#227;o trouxe resultado algum.',
                            type: 'warning',
                            icon: 'fa fa-warning fa-lg',
                            addclass: "stack-bottomright",
                            stack: my.stack_bottomright
                        });
                    }
                    break;
                case (this.dataSource.total() === 1):
                    var autocom = this;

                    var dataItem = autocomplete.dataItem(0);
                    if (dataItem) {

                        if (dataItem.ProductsRelatedCount > 0) {

                            // get related products
                            $.ajax({
                                url: '/desktopmodules/riw/api/products/getProductsRelated?portalId=' + portalID + '&productId=' + dataItem.ProductId + '&lang=pt-BR&relatedType=2&getAll=true'
                            }).done(function (data) {
                                if (data) {

                                    $.each(data, function (i, item) {
                                        // push the new selected item to the view model selectedProducts
                                        my.vm.selectedProducts.push(new my.Product()
                                            .productId(item.RelatedProductId)
                                            .productCode(item.RelatedBarcode ? item.RelatedBarcode : (item.RelatedProductRef ? item.RelatedProductRef : ''))
                                            .productName(item.RelatedProductName)
                                            .summary(item.RelatedProductSummary)
                                            .productUnit(item.RelatedProductUnit)
                                            .unitValue(item.RelatedUnitValue)
                                            .prodImageId(item.RelatedProductImageId)
                                            .itemQty(kendo.parseFloat(my.vm.itemQty().length > 0 ? (my.vm.itemQty() * item.ProductQty) : item.ProductQty)));
                                    });

                                    my.addEstimateItem();
                                }
                            });

                        } else {

                            my.vm.productId(dataItem.ProductId);
                            if (dataItem.Barcode.length > 0) {
                                my.vm.productCode(dataItem.Barcode);
                            } else {
                                if (dataItem.ProductRef.length > 0) {
                                    my.vm.productCode(dataItem.ProductRef);
                                }
                            }
                            my.vm.productName(dataItem.ProductName);
                            my.vm.summary(dataItem.Summary);
                            my.vm.itemQty(parseFloat(my.vm.itemQty()) > 0 ? my.vm.itemQty() : '1');
                            my.vm.unitTitle(dataItem.UnitTypeAbbv);
                            my.vm.prodImageId(dataItem.ProductImageId);
                            my.vm.unitValue((my.unitValue > 0 ? parseFloat(Math.round(my.unitValue) / 100) : dataItem.UnitValue));
                            if (dataItem.Summary.length > 0) {
                                $('#divImage').html(function () {
                                    if (dataItem.ProductImageId > 0) {
                                        return '<div class="view view-first"><img alt="' + dataItem.ProductName + '" src="/databaseimages/' + dataItem.ProductImageId + '.' + dataItem.Extension + '?maxwidth=' + $('#divImage').width() + '&maxheight=' + $('#divImage').height() + '" /></div>';
                                    } else {
                                        return '<div class="view view-first fallBack">Imagem N&#227;o Dispon&#237;vel</div>';
                                    }
                                }).click(function (e) {
                                    window.open('/produtos/detalhes?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemid=' + dataItem.ProductId, 'PrintMe', 'toolbar=no,menubar=no,location=no,width=720,height=580');
                                    return false;
                                });
                            } else {
                                $('#divImage').html(function () {
                                    if (dataItem.ProductImageId > 0) {
                                        return '<div class="view view-first"><img alt="' + dataItem.ProductName + '" src="/databaseimages/' + dataItem.ProductImageId + '.' + dataItem.Extension + '?maxwidth=' + $('#divImage').width() + '&maxheight=' + $('#divImage').height() + '" /></div>';
                                    } else {
                                        return '<div class="view view-first fallBack">Imagem N&#227;o Dispon&#237;vel</div>';
                                    }
                                });
                            }

                            my.addEstimateItem();
                            
                        }

                        autocom.value('');
                        autocom.close();

                        $('#itemQtyBox').val(null);
                    }
                    break;
                default:
            }
        },
        select: function (e) {
            e.preventDefault();
            var autocom = this;
            //if (window.event.keyCode === 13) {
            //    setTimeout(function () {
            var dataItem = autocom.dataItem(e.item.index());
            if (dataItem) {

                if (dataItem.ProductsRelatedCount > 0) {

                    // get related products
                    $.ajax({
                        url: '/desktopmodules/riw/api/products/getProductsRelated?portalId=' + portalID + '&productId=' + dataItem.ProductId + '&lang=pt-BR&relatedType=2&getAll=true'
                    }).done(function (data) {
                        if (data) {
                            //my.vm.selectedProducts([]);

                            //$('#divImage').html('<img alt="" src="/portals/' + portalID + '/images/' + qeImage + '?maxwidth=' + $('#divImage').width() + '&maxheight=' + $('#divImage').height() + '" />');

                            $.each(data, function (i, item) {
                                // push the new selected item to the view model selectedProducts
                                my.vm.selectedProducts.push(new my.Product()
                                    .productId(item.RelatedProductId)
                                    .productCode(item.RelatedBarcode ? item.RelatedBarcode : (item.RelatedProductRef ? item.RelatedProductRef : ''))
                                    .productName(item.RelatedProductName)
                                    .summary(item.RelatedProductSummary)
                                    .productUnit(item.RelatedProductUnit)
                                    .unitValue(item.RelatedUnitValue)
                                    .prodImageId(item.RelatedProductImageId)
                                    //.finan_Sale_Price(item.RelatedFinan_Sale_Price)
                                    //.finan_Special_Price(item.RelatedFinan_Special_Price)
                                    .itemQty(kendo.parseFloat(my.vm.itemQty().length > 0 ? (my.vm.itemQty() * item.ProductQty) : item.ProductQty)));
                                //.qTyStockSet(item.RelatedQTyStockSet));
                                //.totalValue(kendo.parseFloat($('#NumericTextBox_Qty_' + dataItem.ProductId).val()) * dataItem.UnitValue));
                            });

                            //if (!my.vm.estimateId() > 0) {
                            //    my.startEstimate();
                            //} else {
                            my.addEstimateItem();
                            //}
                        }
                    });

                } else {

                    my.vm.productId(dataItem.ProductId);
                    if (dataItem.Barcode.length > 0) {
                        my.vm.productCode(dataItem.Barcode);
                    } else {
                        if (dataItem.ProductRef.length > 0) {
                            my.vm.productCode(dataItem.ProductRef);
                        }
                    }
                    my.vm.productName(dataItem.ProductName);
                    my.vm.summary(dataItem.Summary);
                    //my.vm.prodDesc(dataItem.Description);
                    my.vm.itemQty(parseFloat(my.vm.itemQty()) > 0 ? my.vm.itemQty() : '1');
                    my.vm.unitTitle(dataItem.UnitTypeAbbv);
                    my.vm.prodImageId(dataItem.ProductImageId);
                    my.vm.unitValue((my.unitValue > 0 ? parseFloat(Math.round(my.unitValue) / 100) : dataItem.UnitValue));
                    //my.vm.itemTotal(dataItem.Total);
                    //$('#divImage').html('<img alt="" src="/portals/' + portalID + '/images/' + qeImage + '?maxwidth=' + $('#divImage').width() + '&maxheight=' + $('#divImage').height() + '" />');
                    if (dataItem.Summary.length > 0) {
                        $('#divImage').html(function () {
                            if (dataItem.ProductImageId > 0) {
                                return '<div class="view view-first"><img alt="' + dataItem.ProductName + '" src="/databaseimages/' + dataItem.ProductImageId + '.' + dataItem.Extension + '?maxwidth=' + $('#divImage').width() + '&maxheight=' + $('#divImage').height() + '" /></div>';
                            } else {
                                return '<div class="view view-first fallBack">Imagem N&#227;o Dispon&#237;vel</div>';
                            }
                        }).click(function (e) {
                            window.open('/produtos/detalhes?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemid=' + dataItem.ProductId, 'PrintMe', 'toolbar=no,menubar=no,location=no,width=720,height=580');
                            return false;
                        });
                    } else {
                        $('#divImage').html(function () {
                            if (dataItem.ProductImageId > 0) {
                                return '<div class="view view-first"><img alt="' + dataItem.ProductName + '" src="/databaseimages/' + dataItem.ProductImageId + '.' + dataItem.Extension + '?maxwidth=' + $('#divImage').width() + '&maxheight=' + $('#divImage').height() + '" /></div>';
                            } else {
                                return '<div class="view view-first fallBack">Imagem N&#227;o Dispon&#237;vel</div>';
                            }
                        });
                    }

                    //if (!my.vm.estimateId() > 0) {
                    //    my.startEstimate();
                    //} else {
                    my.addEstimateItem();
                    //}
                }

                autocom.value('');
                autocom.close();

                $('#itemQtyBox').val(null);
            }
            //    }, 1000);
            //};
        }
    });

    my.removeItem = function (product, elem) {
        //var grid = $('#couponGrid').data("kendoGrid");

        if ((authorized > 2) || (my.vm.estimatedByUser() === userID)) {

            var $dialog = $('#divDeleteDialog')
                .dialog({
                    autoOpen: false,
                    modal: true,
                    resizable: false,
                    dialogClass: 'dnnFormPopup',
                    title: 'Confirmar',
                    width: 360,
                    buttons: {
                        'ok': {
                            text: 'Continuar',
                            //priority: 'primary',
                            "class": 'btn btn-primary',
                            click: function (e) {

                                //var selectedItem = grid.dataItem(grid.select());
                                my.vm.items.remove(function (item) { return item.EstimateItemId === product.EstimateItemId; });

                                var estimateItems = [];
                                estimateItems.push({
                                    EstimateId: my.vm.estimateId(),
                                    EstimateItemId: product.EstimateItemId,
                                    RemoveReasonId: $('#selectDeleteReason').data('kendoDropDownList').value(),
                                    ProductId: product.ProductId,
                                    ProductName: product.ProductName,
                                    ProductQty: product.ProductQty,
                                    CreatedByUser: userID,
                                    CreatedOnDate: moment().format()
                                });

                                $.ajax({
                                    type: 'POST',
                                    contentType: 'application/json; charset=utf-8',
                                    url: '/desktopmodules/riw/api/estimates/removeEstimateItems',
                                    data: JSON.stringify({
                                        EstimateItemsRemoved: estimateItems,
                                        EstimateId: my.vm.estimateId(),
                                        PortalId: portalID,
                                        Discount: my.vm.couponDiscount(),
                                        TotalAmount: my.lineTotal,
                                        ModifiedByUser: userID,
                                        ModifiedOnDate: moment().format()
                                    })
                                }).done(function (data) {
                                    if (data.Result.indexOf("success") !== -1) {
                                        //grid.dataSource.remove(selectedItem);

                                        if (my.vm.conditionNumberPayments() > 0) {
                                            $('#btnCancelPayFormCond').click();
                                        }

                                        if (my.vm.selectedStatusId() === 5 || my.vm.selectedStatusId() === 6) {

                                            var prodParams = [];
                                            prodParams.push({
                                                ProductId: product.ProductId,
                                                ProductQty: product.ProductQty,
                                                ModifiedByUser: userID,
                                                ModifiedOnDate: moment().format()
                                            });

                                            $.ajax({
                                                type: 'PUT',
                                                contentType: 'application/json; charset=utf-8',
                                                url: '/desktopmodules/riw/api/estimates/returnProductStock',
                                                data: JSON.stringify(prodParams)
                                            });
                                        }

                                        my.vm.productName(slogan);
                                        my.vm.prodTotal('0 x 0 = 0,00');
                                        my.lineTotal = my.lineTotal - (kendo.parseFloat(product.ProductQty) * product.UnitValue);
                                        my.vm.displayTotal('TOTAL = ' + kendo.toString(my.lineTotal, 'n') + (my.vm.couponDiscount() > 0 ? '<span style="font-size: 8px">D</span>' : ''));
                                        my.vm.subTotal(my.lineTotal);
                                        my.vm.grandTotal((my.lineTotal - 0));
                                        my.vm.loadPayConds();
                                        my.vm.items.remove(function (item) { return item.EstimateItemId === product.EstimateItemId; });
                                        // $.post('/desktopmodules/riw/api/estimates/UpdateEstimateAmount', { eId: my.vm.estimateId(), amt: my.lineTotal, uId: userID, md: kendo.toString(new Date(), "G") }, function (data) { });
                                        my.loadEstimate();
                                        $('[data-bind*="displayTotal"]').css({ color: '#FFF' });
                                    } else {
                                        $.pnotify({
                                            title: 'Erro!',
                                            text: data.Result,
                                            type: 'error',
                                            icon: 'fa fa-times-circle fa-lg',
                                            addclass: "stack-bottomright",
                                            stack: my.stack_bottomright
                                        });
                                        //$().toastmessage('showErrorToast', data.Result);
                                    }
                                }).fail(function (jqXHR, textStatus) {
                                    console.log(jqXHR.responseText);
                                }).always(function () {
                                    $('#divImage').text('');
                                    $('#divImage').off('click');
                                    my.vm.displayTotal('TOTAL = 0,00');
                                    //$this.button('reset');
                                    $dialog.dialog('close');
                                    $dialog.dialog('destroy');
                                });

                            }
                        },
                        'cancel': {
                            text: 'Cancelar',
                            //priority: 'secondary',
                            "class": 'btn',
                            click: function (e) {
                                $dialog.dialog('close');
                                $dialog.dialog('destroy');
                            }
                        }
                    }
                });

            $dialog.dialog('open');

        } else {

            var $dialog2 = $('<div></div>')
                    .html('<div class="confirmDialog">Não Autorizado!</div>')
                    .dialog({
                        autoOpen: false,
                        modal: true,
                        resizable: false,
                        dialogClass: 'dnnFormPopup',
                        title: 'Aviso',
                        buttons: {
                            'ok': {
                                text: 'Cancelar',
                                //priority: 'primary',
                                "class": 'btn btn-primary',
                                click: function () {
                                    $dialog.dialog('close');
                                    $dialog.dialog('destroy');
                                }
                            }
                        }
                    });

            $dialog2.dialog('open');
        }
    };

    $('#btnPlans').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();
        $('#payFormCondWindow').kendoWindow({
            title: 'Planos de Pagamento',
            modal: true,
            width: '90%',
            height: '80%'
        }).data("kendoWindow").center().open();
    });

    my.startPayCondGrid = function (pfId) {
        if (pfId > 0) {
            $("#payCondGrid").kendoGrid({
                height: 150,
                columns: [
                    {
                        command: [
                          {
                              name: "select",
                              text: "",
                              imageClass: "k-icon k-i-tick",
                              click: function (e) {
                                  e.preventDefault();
                                  my.vm.displayTotal('TOTAL = ' + kendo.toString(my.originalTotal, "n"));
                                  my.vm.subTotal(my.originalTotal);
                                  my.vm.grandTotal(my.originalTotal);
                                  $('#divPayForms').fadeOut(200);
                                  $('#divPayFormCond').fadeOut(200);
                                  $('#divChosenPayCond').delay(400).fadeIn();
                                  my.vm.selectedPayForm($('#ddlPayForms').data("kendoDropDownList").text());
                                  var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                  if (dataItem) {
                                      if (dataItem.PayIn > 0) {
                                          $('#divPayIn').show();
                                          $('#btnSavePayFormCond').show();
                                      }
                                      my.vm.conditionPayIn((dataItem.PayIn));
                                      my.vm.conditionPayInDays((dataItem.PayInDays));
                                      my.vm.payInMin(dataItem.PayIn);
                                      my.vm.conditionNumberPayments(dataItem.PayCondN);
                                      my.vm.conditionInterest(dataItem.PayCondPerc);
                                      my.vm.conditionInterval(dataItem.Intervalo);
                                      $('#interval').text(dataItem.Intervalo);
                                  }

                                  var payConds = {
                                      PortalId: portalID,
                                      EstimateId: my.vm.estimateId(),
                                      PayCondId: dataItem.PayCondId,
                                      PayCondType: my.vm.selectedPayForm(),
                                      PayCondN: my.vm.conditionNPayments().replace(/[^\.\d]/g, ""),
                                      PayCondPerc: my.vm.conditionInterest(),
                                      PayCondIn: my.vm.conditionPayIn(),
                                      PayInDays: my.vm.conditionPayInDays(),
                                      PayCondInst: kendo.parseFloat(conditionPayment()),
                                      PayCondInterval: my.vm.conditionInterval() === 'A Vista' ? -1 : my.vm.conditionInterval() === 'Mensal' ? 0 : my.vm.conditionInterval(),
                                      TotalPayments: my.vm.conditionPayments(),
                                      TotalPayCond: my.vm.conditionTotalPay(),
                                      ModifiedByUser: userID,
                                      ModifiedOnDate: moment().format()
                                  };

                                  $.ajax({
                                      type: 'PUT',
                                      url: '/desktopmodules/riw/api/estimates/updateEstimatePayCond',
                                      data: payConds
                                  }).done(function (msg) {
                                      if (msg.Result.indexOf("success") !== -1) {
                                          my.loadEstimate();
                                          //$().toastmessage('showSuccessToast', 'Escolha do plano salva com sucesso.');
                                          $.pnotify({
                                              title: 'Sucesso!',
                                              text: 'Escolha do plano salva.',
                                              type: 'success',
                                              icon: 'fa fa-check fa-lg',
                                              addclass: "stack-bottomright",
                                              stack: my.stack_bottomright
                                          });
                                      } else {
                                          $.pnotify({
                                              title: 'Erro!',
                                              text: data.Result,
                                              type: 'error',
                                              icon: 'fa fa-times-circle fa-lg',
                                              addclass: "stack-bottomright",
                                              stack: my.stack_bottomright
                                          });
                                          //$().toastmessage('showErrorToast', msg.Result);
                                      }
                                  }).fail(function (jqXHR, textStatus) {
                                      console.log(jqXHR.responseText);
                                  });
                              }
                          }
                        ],
                        //title: '',
                        width: 40
                        //headeTemplate: ''
                    },
                    { field: "PayCondN", title: "N&#250;m. de Parcelas", template: '# if (PayCondPerc > 0) { # #= PayCondN # x com juros # } else { # #= PayCondN #x sem juros # } #', width: 100 },
                    { field: "PayIn", title: "Entrada c/ Dias", attributes: { class: 'text-right' }, width: 100, template: '#= kendo.toString(PayIn, "n") + " / " + String(PayInDays) #' },
                    { field: "Parcela", title: "Valor p/ Parcela", format: '{0:n}', attributes: { class: 'text-right' }, width: 100 },
                    { field: "TotalParcelado", title: "Total Parcelado", format: '{0:n}', attributes: { class: 'text-right' }, width: 100 },
                    { field: "PayCondPerc", title: "Juros % (a.m.)", template: '#= kendo.format("{0:p}",(PayCondPerc/100)) #', attributes: { class: 'text-right' }, width: 90 },
                    { field: "Intervalo", title: "Intervalo (dias)", width: 90 },
                    { field: "TotalPayCond", title: "Total", format: '{0:n}', attributes: { class: 'text-right' }, width: 80 }
                ],
                editable: false,
                change: function () {
                    var row = this.select();
                    var id = row.data("uid");
                    my.uId = id;
                },
                selectable: true,
                navigatable: true,
                dataSource: {
                    transport: {
                        read: "/desktopmodules/riw/api/payconditions/getPayConds?portalId=" + portalID + "&pcType=" + pfId + "&pcStart=" + (my.lineTotal - (my.lineTotal * my.vm.couponDiscount() / 100)), // my.vm.subTotal(),
                        cache: false
                    }
                },
                dataBound: function () {
                    var grid = this;
                    if (this.dataSource.view().length !== 0) {
                        if (this.dataSource.view().length !== 0) {
                            $('#payCondMsg').html('');
                            $('#divPayFormCond').show();
                            //$('#payCondMsg').html('');

                            my.vm.payFormTitle($('#ddlPayForms').data('kendoDropDownList').text());
                            $.each(grid.dataSource.data(), function (i, item) {
                                item.set('TotalPayCond', (item.TotalParcelado + item.PayIn));
                            });
                        }
                    } else {
                        $('#divPayFormCond').hide();
                        //    $('#payCondMsg').html('<span class="text-error">N&#227;o h&#225; outras condi&#231;&#245;es configuradas para forma de pagamento escolhida!</span> <small><i>(Ctrl+7 para escolher)</i></small>');
                    }
                },
            });
        }
    };

    $('#btnCancelPayFormCond').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        //my.vm.selectedPayFormId(null);
        ////startPayCondGrid(0);
        $('#payCondMsg').html('<small><i>Escolha uma outra forma de pagamento (Ctrl+7).</i></small>');
        //$('#divPayFormCond').kendoAnimate({ effects: 'slide:up fade:out', hide: true });
        //$('#divChosenPayCond').kendoAnimate({ effects: 'slide:up fade:out', hide: true });
        //$('#divPayFormCond').kendoAnimate({ effects: 'slide:down fade:in', show: true });

        my.vm.selectedPayFormId(null);
        my.vm.payCondDisc(0);
        $('#divPayFormCond').fadeOut();
        $('#divChosenPayCond').fadeOut();
        $('#divPayIn').fadeOut();
        $('#btnSavePayFormCond').fadeOut();
        $('#divPayForms').fadeIn();

        var payConds = {
            PortalId: portalID,
            EstimateId: my.estimateId,
            PayCondId: null,
            PayCondType: null,
            PayCondN: null,
            PayCondPerc: null,
            PayCondIn: null,
            PayCondInst: null,
            PayCondInterval: null,
            TotalPayments: null,
            TotalPayCond: null,
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };

        my.startPayCondGrid(0);
        $('#payCondGrid').data("kendoGrid").dataSource.read();

        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/estimates/cancelEstimatePayCond',
            data: payConds
        }).done(function (msg) {
            if (msg.Result.indexOf("success") !== -1) {
                my.loadEstimate();
                //$().toastmessage('showSuccessToast', 'Escolha do plano e condi&#231;&#245;es de pagamento cancelado com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Escolha do plano e condi&#231;&#245;es de pagamento cancelado.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', msg.Result);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
    });

    $('#btnSavePayFormCond').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var payConds = {
            PortalId: portalID,
            EstimateId: my.estimateId,
            PayCondType: my.vm.selectedPayForm(),
            PayCondN: my.vm.conditionNPayments().replace(/[^\.\d]/g, ""),
            PayCondPerc: my.vm.conditionInterest(),
            PayCondIn: my.vm.conditionPayIn(),
            PayInDays: my.vm.conditionPayInDays(),
            PayCondInst: kendo.parseFloat(conditionPayment()),
            PayCondInterval: my.vm.conditionInterval() === 'A Vista' ? -1 : my.vm.conditionInterval() === 'Mensal' ? 0 : my.vm.conditionInterval(),
            TotalPayments: my.vm.conditionPayments(),
            TotalPayCond: my.vm.conditionTotalPay(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };

        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/estimates/updateEstimatePayCond',
            data: payConds
        }).done(function (msg) {
            if (msg.Result.indexOf("success") !== -1) {
                my.loadEstimate();
                //$().toastmessage('showSuccessToast', 'Alera&#231;&#245;es em condi&#231;&#245;es de pagamento salvas com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Alera&#231;&#245;es em condi&#231;&#245;es de pagamento salvas.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                parent.$(".k-window-content").each(function () {
                    parent.$(this).data("kendoWindow").close();
                });
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', msg.Result);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
    });

    $('#btnClosePayFormCond').click(function (e) {
        parent.$(".k-window-content").each(function () {
            parent.$(this).data("kendoWindow").close();
        });
    });

    $('#btnPrintPdf').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');
        //$(this).html('<i class="icon-spinner icon-spin"></i> Um momento...').attr({ 'disabled': true });

        var newWindow = window.open('', '_blank');

        var strSubHeader = 'Status: ' + my.vm.estimateStatus() + ' - ' + moment().format('L') + '[NEWLINE]';
        strSubHeader += 'Vendedor: ' + my.vm.salesRepName() + '[NEWLINE]';
        if (my.salesRepPhone !== '') {
            strSubHeader += 'Telefone: ' + my.formatPhone(my.vm.salesRepPhone()) + '[NEWLINE]';
        }
        strSubHeader += 'Email: ' + my.vm.salesRepEmail();

        var columns = ['Cod.', 'Produto', 'Qde', 'Valor Original', 'Desconto', 'Valor Atual', 'Total'];

        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Accept", "application/pdf");
            },
            url: '/desktopmodules/riw/api/estimates/downloadEstimatePdf?customerLogo=' + customerLogo + '&pageHeader=Orcamento ID: ' + my.estimateId + '&pageSubHeader=Email: ' + strSubHeader + '&footerText=' + footerText + '&customerFooter=' + pdfFooter,
            data: JSON.stringify({
                Columns: columns,
                EstimateId: my.estimateId,
                PortalId: portalID,
                Lang: 'pt-BR',
                ProductDiscountValue: my.lineTotalDiscount,
                ColumnsCount: 7,
                ProductOriginalAmount: my.vm.grandTotal(),
                EstimateDiscountValue: my.lineTotalDiscount + (my.vm.grandTotal() / 100 * my.vm.couponDiscount()),
                TotalDiscountPerc: (my.vm.grandTotal() - my.vm.subTotal()) / my.vm.grandTotal(), // my.vm.discountTotalPerc(),
                TotalDiscountValue: (my.vm.grandTotal() - my.vm.subTotal()),
                EstimateTotalAmount: my.vm.subTotal(),
                ModifiedOnDate: moment().format(),
                Watermark: watermark
            })
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                newWindow.location = 'http://' + siteURL + '/portals/0/downloads/' + data.FileName;
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', data.Result);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
            //$('#btnPrintPdf').html('Enviar').attr({ 'disabled': false });
        });
    });

    $('#btnPrintReceipt').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');
        //$(this).html('<i class="icon-spinner icon-spin"></i> Um momento...').attr({ 'disabled': true });

        var newWindow = window.open('', '_blank');
        var locationStr = 'http://' + siteURL + '/portals/0/downloads/recibo_' + my.estimateId + '_' + moment().format('DD-MM-YYYY HH').replace(':', '-').replace(' ', '_') + '.html';

        //var strSubHeader = 'Status: ' + my.vm.estimateStatus() + '[NEWLINE]';
        //strSubHeader += 'Vendedor: ' + my.vm.salesRepName() + '[NEWLINE]';
        //if (my.salesRepPhone !== '') {
        //    strSubHeader += 'Telefone: ' + my.formatPhone(my.vm.salesRepPhone()) + '[NEWLINE]';
        //}
        //strSubHeader += 'Email: ' + my.vm.salesRepEmail();

        //var columns = ['Cod.', 'Produto', 'Qde', 'Valor Original', 'Desconto', 'Valor Atual', 'Total'];

        $.ajax({
            type: 'POST',
            //contentType: 'application/json; charset=utf-8',
            //beforeSend: function (xhr) {
            //    xhr.setRequestHeader("Accept", "application/octet-stream");
            //},
            url: '/desktopmodules/riw/api/estimates/DownloadEstimateHtmlReceipt',
            data: {
                PortalId: portalID,
                EstimateId: my.estimateId,
                //PersonId: my.personId > 0 ? my.personId : my.getParameterByName('personId'),
                //TxtTitle: 'ORCAMENTO n [ORC]',
                //TxtHeader: '[DATA]',
                //TxtFooter: '[BR][BR][EMPRESA][BR][SITE][BR][RUA] [NUM][BR][BAIRRO][BR][CIDADE] / [ESTADO][BR]Tel: [FONE][BR][EMAIL]',
                //TxtSubHeader: '[BR][BR]ORCAMENTO n [ORC][BR][BR]',
                //TxtClientInfo: 'Cliente: [NOME][BR][RUA] [NUM][BR]Bairro: [BAIRRO][BR][CIDADE] / [ESTADO][BR]CEP: [CEP] / Telefone: [TELEFONE][BR][BR]',
                //TxtColumnHeader: '[SPACE]ITEM DESCRICAO<table style="font-size: 12px;"><tbody><tr><td style="width: 35px;">Qde</td><td style="width: 60px;">Valor U.</td><td>Total</td></tr><tr><td colspan="3">------------------------------</td></tr></tbody></table>',
                //TxtItemName: '[ITEMNAME:28]',
                //TxtItemRef: '[ITEMREF:01]',
                //TxtItemPrice: '[ITEMPRICE:11]',
                //TxtItemUni: '[ITEMUNI:05]',
                //TxtItemQty: '[ITEMQTY:06]',
                //TxtItemDisc: '[ITEMDISC:09]',
                //TxtDiscount: '[15:DISCOUNT:12]',
                //TxtSubTotal: '[15:SUBTOTAL:12]',
                //TxtTotal: '[15:TOTAL:12]',
                //TxtBankIn: my.vm.bankIn(),
                //TxtCheckIn: my.vm.checkIn(),
                //TxtCardIn: my.vm.cardIn(),
                //TxtCashIn: my.vm.cashIn(),
                //TxtCheck: '[Cheque:18]',
                //TxtConditionColumnHeader: 'Forma             Qde P.    Valor Parcela.[BR]Ent.         Jur. AM   Total         Inter. Dias[BR]------------------------------------------------',
                //TxtPayQty: '[PAYQTY:10]',
                //TxtPayments: '[PAYMENTS:14]',
                //TxtInitialPay: '[INITIALPAY:13]',
                //TxtInterest: '[INTEREST:10]',
                //TxtTotalPays: '[TOTALPAYS:14]'
                //TxtHeader: '[DATA][BR][BR][EMPRESA] / [SITE][BR][RUA] [NUM] Bairro: [BAIRRO] / [CIDADE] / [ESTADO][BR]',
                //TxtSubHeader: 'Telefone: [FONE][BR]Email: [EMAIL][BR][BR]ORCAMENTO n [ORC][BR][BR]',
                //TxtClientInfo: 'Cliente: [NOME][BR][RUA] [NUM][BR]Bairro: [BAIRRO][BR][CIDADE] / [ESTADO][BR]CEP: [CEP] / Telefone: [TELEFONE][BR][BR]',
                //TxtColumnHeader: 'ITEM   DESCRICAO                   CODIGO[BR]VL.UN      QTD   UN   DESC     TOTAL[BR]------------------------------------------------[BR]',
                //TxtItemName: '[ITEMNAME:28]',
                //TxtItemRef: '[ITEMREF:01]',
                //TxtItemPrice: '[ITEMPRICE:11]',
                //TxtItemUni: '[ITEMUNI:05]',
                //TxtItemQty: '[ITEMQTY:06]',
                //TxtItemDisc: '[ITEMDISC:09]',
                //TxtDiscount: '[15:DISCOUNT:12]',
                //TxtSubTotal: '[15:SUBTOTAL:12]',
                //TxtTotal: '[15:TOTAL:12]',
                //TxtBankIn: my.vm.bankIn(),
                //TxtCheckIn: my.vm.checkIn(),
                //TxtCardIn: my.vm.cardIn(),
                //TxtCashIn: my.vm.cashIn(),
                //TxtCheck: '[Cheque:18]',
                //TxtConditionColumnHeader: 'Forma             Qde P.    Valor Parcela.[BR]Ent.         Jur. AM   Total         Inter. Dias[BR]------------------------------------------------',
                //TxtPayQty: '[PAYQTY:10]',
                //TxtPayments: '[PAYMENTS:14]',
                //TxtInitialPay: '[INITIALPAY:13]',
                //TxtInterest: '[INTEREST:10]',
                //TxtTotalPays: '[TOTALPAYS:14]'
            },
            cache: false
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                setTimeout(function () {
                    newWindow.location = locationStr;
                }, 1000);
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', data.Result);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
            //$('#btnPrintPdf').html('Enviar').attr({ 'disabled': false });
        });
    });

    $('#btnPrintTxt').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');
        //$(this).html('<i class="icon-spinner icon-spin"></i> Um momento...').attr({ 'disabled': true });

        var newWindow = window.open('', '_blank');

        //var strSubHeader = 'Status: ' + my.vm.estimateStatus() + '[NEWLINE]';
        //strSubHeader += 'Vendedor: ' + my.vm.salesRepName() + '[NEWLINE]';
        //if (my.salesRepPhone !== '') {
        //    strSubHeader += 'Telefone: ' + my.formatPhone(my.vm.salesRepPhone()) + '[NEWLINE]';
        //}
        //strSubHeader += 'Email: ' + my.vm.salesRepEmail();

        //var columns = ['Cod.', 'Produto', 'Qde', 'Valor Original', 'Desconto', 'Valor Atual', 'Total'];

        $.ajax({
            type: 'POST',
            //contentType: 'application/json; charset=utf-8',
            //beforeSend: function (xhr) {
            //    xhr.setRequestHeader("Accept", "application/octet-stream");
            //},
            url: '/desktopmodules/riw/api/estimates/downloadEstimateHtml',
            data: {
                PortalId: portalID,
                EstimateId: my.estimateId,
                //PersonId: my.personId > 0 ? my.personId : my.getParameterByName('personId'),
                //TxtHeader: '[DATA][BR][BR][EMPRESA] / [SITE][BR][RUA] [NUM] Bairro: [BAIRRO] / [CIDADE] / [ESTADO][BR]',
                //TxtSubHeader: 'Telefone: [FONE][BR]Email: [EMAIL][BR][BR]ORCAMENTO n [ORC][BR][BR]',
                //TxtClientInfo: 'Cliente: [NOME][BR][RUA] [NUM][BR]Bairro: [BAIRRO] / [CIDADE] / [ESTADO][BR]CEP: [CEP] / Telefone: [TELEFONE][BR][BR]',
                //TxtColumnHeader: 'ITEM   DESCRICAO                   CODIGO[BR]VL.UN      QTD   UN   DESC     TOTAL[BR]------------------------------------------------[BR]',
                //TxtItemName: '[ITEMNAME:28]',
                //TxtItemRef: '[ITEMREF:01]',
                //TxtItemPrice: '[ITEMPRICE:11]',
                //TxtItemUni: '[ITEMUNI:05]',
                //TxtItemQty: '[ITEMQTY:06]',
                //TxtItemDisc: '[ITEMDISC:09]',
                //TxtDiscount: '[15:DISCOUNT:12]',
                //TxtSubTotal: '[15:SUBTOTAL:12]',
                //TxtTotal: '[15:TOTAL:12]',
                //TxtConditionColumnHeader: 'Forma        Qde P.    Valor Parcela.[BR]Ent.         Jur. AM   Total         Inter. Dias[BR]------------------------------------------------',
                //TxtCheck: '[Cheque:13]',
                //TxtBankPay: '[Boleto:13]',
                //TxtVisa: '[Visa:13]',
                //TxtMC: '[M. C.:13]',
                //TxtAmex: '[Amex:13]',
                //TxtDinners: '[Diners:13]',
                //TxtDebit: '[Debito:13]',
                //TxtPayQty: '[PAYQTY:10]',
                //TxtPayments: '[PAYMENTS:14]',
                //TxtInitialPay: '[INITIALPAY:13]',
                //TxtInterest: '[INTEREST:10]',
                //TxtTotalPays: '[TOTALPAYS:14]'
            },
            cache: false
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                setTimeout(function () {
                    // newWindow.location = 'http://' + siteURL + '/portals/0/downloads/orcamento.txt';
                    newWindow.location = 'http://' + siteURL + '/portals/0/downloads/orcamento_' + my.estimateId + '_' + moment().format('DD-MM-YYYY HH').replace(':', '-').replace(' ', '_') + '.html';
                    //sUrl = 'http://' + siteURL + '/portals/0/downloads/orcamento_' + my.estimateId + '_' + moment().format('DD-MM-YYYY HH').replace(':', '-').replace(' ', '_') + '.txt';
                    //$("body").append("<iframe src='" + sUrl + "' style='display: none;' ></iframe>");
                }, 1000);
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', data.Result);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
            //$('#btnPrintPdf').html('Enviar').attr({ 'disabled': false });
        });
    });

    $("#btnEmailEstimate").click(function (e) {
        e.preventDefault();

        var subject = estimateSubject.replace('[WEBSITELINK]', siteName).replace('[ID]', my.vm.estimateId());
        my.vm.subject(subject);

        var estimateLink = '[link](' + estimateURL + '#estimateId/' + my.vm.estimateId() + ')' // '[link] (' + estimateURL + '?eid=' + my.eId + ' "Clique para acessar o orçamento")';
        var body = estimateBody.replace('[ORCAMENTOLINK]', estimateLink);
        // result = result.replace(/BR/g, '\n')
        body = body.replace('[CLIENTE]', my.vm.displayName());
        body = body.replace('[ID]', my.vm.estimateId());
        my.vm.message(toMarkdown(body));

        $('#emailSubjectTextBox').val(my.vm.subject());
        $('#toEmailTextBox').val(my.vm.email());
        $('#messageBody').val(my.vm.message());

        var kendoWindow = $('#divEmail').kendoWindow({
            actions: ["Maximize", "Close"],
            title: 'Enviando arquivo pdf via email',
            modal: true,
            width: '80%',
            height: '50%',
            close: function (e) {
                $("html, body").css("overflow", "");
                if (parent.$('#window').data('kendoWindow')) {
                    parent.$('#window').data('kendoWindow').toggleMaximization();
                }
            },
            open: function () {
                //$("html, body").css("overflow", "hidden");
                if (parent.$('#window').data('kendoWindow')) {
                    parent.$('#window').data('kendoWindow').maximize();
                }
            }
        });

        kendoWindow.data("kendoWindow").center().open();
    });

    $('#btnEmail').click(function (e) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        //$(this).html('<i class="icon-spinner icon-spin"></i> Um momento...').attr({ 'disabled': true });

        var strSubHeader = 'Status: ' + my.vm.estimateStatus() + ' - ' + moment().format('L') + '[NEWLINE]';
        strSubHeader += 'Vendedor: ' + my.vm.salesRepName() + '[NEWLINE]';
        if (my.salesRepPhone !== '') {
            strSubHeader += 'Telefone: ' + my.formatPhone(my.vm.salesRepPhone()) + '[NEWLINE]';
        }
        strSubHeader += 'Email: ' + my.vm.salesRepEmail();

        var columns = ['Cod.', 'Produto', 'Qde', 'Valor Original', 'Desconto', 'Valor Atual', 'Total'];

        var converter = new Showdown.converter();
        var emailHtmlContent = converter.makeHtml($('#messageBody').val().trim());

        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/estimates/sendEstimatePdf?customerLogo=' + customerLogo + '&pageHeader=Orcamento ID: ' + my.estimateId + '&pageSubHeader=Email: ' + strSubHeader + '&footerText=' + footerText + '&customerFooter=' + pdfFooter,
            data: JSON.stringify({
                Columns: columns,
                EstimateId: my.vm.estimateId(),
                PortalId: portalID,
                Lang: 'pt-BR',
                ProductDiscountValue: my.lineTotalDiscount,
                ColumnsCount: 7,
                ProductOriginalAmount: my.vm.originalProdTotal(),
                EstimateDiscountValue: my.vm.couponDiscount(),
                TotalDiscountPerc: (my.vm.originalProdTotal() - my.lineTotal) / my.vm.originalProdTotal(),
                TotalDiscountValue: my.lineTotalDiscount + (my.vm.originalProdTotal() / 100 * my.vm.couponDiscount()),
                EstimateTotalAmount: my.lineTotal,
                ToEmail: $('#toEmailTextBox').val().trim(),
                Subject: $('#emailSubjectTextBox').val().trim(),
                MessageBody: emailHtmlContent,
                SalesPersonId: salesPerson,
                PersonId: my.personId,
                //Expand: $('#expandCheckbox').is(':checked'),
                ModifiedOnDate: moment().format()
            })
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                //if (data.SentEmail > 1) {
                //$().toastmessage('showSuccessToast', 'O envio do email foi transmitido com sucesso!');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Email transmitido.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //} else {
                //$().toastmessage('showSuccessToast', 'O envio de ' + data.SentEmail + ' emails for&#227;o transmitidos com sucesso!');
                //}
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', data.Result);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
            //$('#btnEmail').html('Enviar').attr({ 'disabled': false });
            $('#divEmail').data("kendoWindow").close();
        });
    });

    $('#btnCancelEmail').click(function (e) {
        e.preventDefault();

        $('#divEmail').data("kendoWindow").close();
    });

    $('.togglePreview').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var ele = $(this).data('provider');

        var $dialog = $('<div></div>')
            .html(my.converter.makeHtml($('#' + ele).val().trim()))
            .dialog({
                autoOpen: false,
                open: function () {
                    $(".ui-dialog-title").append('Texto');
                },
                modal: true,
                resizable: true,
                dialogClass: 'dnnFormPopup',
                width: '50%',
                buttons: {
                    'ok': {
                        text: 'Ok',
                        //priority: 'primary',
                        "class": 'btn btn-primary',
                        click: function (e) {
                            $dialog.dialog('close');
                            $dialog.dialog('destroy');
                        }
                    }
                }
            });

        $dialog.dialog('open');
    });

    //$('#ddlCouponProducts').data('kendoDropDownList').bind('select', function (e) {
    //    var dataItem = this.dataItem(e.item.index());
    //    if (dataItem) {
    //        my.vm.productId(dataItem.ProductId);
    //        my.vm.productName(dataItem.ProductName);
    //        my.vm.estimateItemId(dataItem.EstimateItemId);
    //        my.vm.newItemDiscount(dataItem.ProductDiscountPerc);
    //        my.vm.itemPrice(dataItem.UnitValue);
    //        my.vm.newItemQty(dataItem.ProductQty);
    //        my.vm.oldItemQty(dataItem.ProductQty);
    //        this.text(dataItem.ProductName);
    //        //$('#itemDiscountTextBox').kendoNumericTextBox({
    //        //    format: "##.00 '%'"
    //        //});
    //    }
    //});

    $('#btnMinMax').click(function (e) {
        e.preventDefault();
        if (e.clientX === 0) {
            return false;
        }
        //if (!$('.MenuArea').is(':hidden')) {
        //    $('.MenuArea').kendoAnimate({ effects: "slide:up fade:out", hide: true });
        //    $('#FooterWrapper').kendoAnimate({ effects: "slide:up fade:out", hide: true });
        //    $('#BelowFtrWrapper').kendoAnimate({ effects: "slide:up fade:out", hide: true });
        //} else {
        //    $('.MenuArea').kendoAnimate({ effects: "slide:down fade:in", show: true });
        //    $('#FooterWrapper').kendoAnimate({ effects: "slide:down fade:in", show: true });
        //    $('#BelowFtrWrapper').kendoAnimate({ effects: "slide:down fade:in", show: true });
        //};
        if (!$('.outerEstimate').is(':hidden')) {
            $('.outerEstimate').kendoAnimate({ effects: "slide:up fade:out", hide: true });
        } else {
            $('.outerEstimate').kendoAnimate({ effects: "slide:up fade:in" });
        }
    });

    $('#btnRestart').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();
        history.pushState("", document.title, window.location.pathname);
        $('#clientSearch').select2('val', '');
        my.estimateId = 0;
        my.personId = 0;
        my.lineTotal = 0;
        my.lineTotalDiscount = 0;
        my.vm.subTotal(0);
        my.vm.grandTotal(0);
        my.vm.couponDiscount(0);
        my.vm.payCondDisc(0);
        $('#divImage').off('click');
        //$('#divImage').html('<img alt="" src="/portals/' + portalID + '/images/' + qeImage + '?maxwidth=' + $('#divImage').width() + '&maxheight=' + $('#divImage').height() + '" />');
        $('#divImage').html('');
        my.vm.estimateId(0);
        my.vm.selectedStatusId(null);
        //my.vm.comments('');
        my.vm.estimatedByUser(0);
        my.vm.personId(0);
        my.vm.clientUserId(0);
        my.vm.displayName('');
        my.vm.telephone('');
        my.vm.email('');
        my.vm.street('');
        my.vm.unit('');
        my.vm.complement('');
        my.vm.district('');
        my.vm.city('');
        my.vm.region('');
        //my.vm.emailTo('');
        my.vm.productName(slogan);
        my.vm.productCode('0000000000000');
        my.vm.itemQty('');
        my.vm.unitValue(0);
        my.vm.createdOnDate(new Date());
        my.vm.hasLogin(false);
        my.vm.selectedPayForm('');
        my.vm.conditionPayIn(0);
        my.vm.conditionPayInDays(0);
        my.vm.conditionNumberPayments(0);
        my.vm.conditionInterest(0);
        my.vm.conditionInterval(0);
        my.vm.salesRepName('');
        my.vm.salesRepEmail('');
        my.vm.salesRepPhone('');
        my.vm.bankIn(0);
        my.vm.checkIn(0);
        my.vm.cardIn(0);
        my.vm.cashIn(0);
        my.vm.debitIn(0);
        my.vm.items.removeAll();
        my.vm.selectedProducts.removeAll();
        my.vm.productId(0);
        my.loadEstimate();
        my.loadClient();
        //$('#btnRegister').attr({ 'title': 'Desativado', 'disabled': true });
        //setTimeout(function () {
        my.itemsSearchBox.attr({ 'title': '', 'disabled': false });
        my.itemsSearchBox.focus();
        //my.vm.selectedPayForm(0);
        //}, 500);
    });

    $('#getEstimateTextBox').keypress(function (e) {
        var key = e.keyCode || e.which;
        var input = $(this);
        if (key === 13) {
            $.ajax({
                url: '/desktopmodules/riw/api/estimates/getEstimate?portalId=' + portalID + '&estimateId=' + $(input).val() + '&getAll=true'
            }).done(function (data) {
                if (data.Result.indexOf('success') !== -1) {
                    if (data.Estimate) {
                        my.estimateId = data.Estimate.EstimateId;
                        document.location.hash = 'estimateId/' + data.Estimate.EstimateId + '/personId/' + data.Estimate.PersonId;
                        my.loadEstimate();
                        my.loadClient();
                        //loadURL();
                        $(input).val(null)
                        setTimeout(function () {
                            my.itemsSearchBox.focus();
                        }, 500);
                        //setTimeout(function () {
                        //    $(input).attr({ 'title': 'Desativado', 'disabled': true });
                        //}, 400);
                    } else {
                        $.pnotify({
                            title: 'Aten&#231;&#227;o!',
                            text: 'Sua busca n&#227;o trouxe resultado algum.',
                            type: 'warning',
                            icon: 'fa fa-warning fa-lg',
                            addclass: "stack-bottomright",
                            stack: my.stack_bottomright
                        });
                    }
                } else {
                    $.pnotify({
                        title: 'Erro!',
                        text: data.Result,
                        type: 'error',
                        icon: 'fa fa-times-circle fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    //$().toastmessage('showErrorToast', data.Result);
                }
            }).fail(function (jqXHR, textStatus) {
                console.log(jqXHR.responseText);
            });
        }
    });

    //$('#getEstimateTextBox')
    //    .data('timeout', null)
    //    .keyup(function() {
    //        clearTimeout($(this).data('timeout'));
    //        var input = $(this);
    //        $(this).data('timeout', setTimeout(function () {
    //            $.ajax({
    //                url: '/desktopmodules/riw/api/estimates/getEstimate?portalId=' + portalID + '&estimateId=' + $(input).val()
    //            }).done(function (data) {
    //                if (data.Result.indexOf('success') !== -1) {
    //                    if (data.Estimate) {
    //                        my.estimateId = data.Estimate.EstimateId;
    //                        document.location.hash = 'estimateId/' + data.Estimate.EstimateId + '/personId/' + data.Estimate.PersonId;
    //                        my.loadEstimate();
    //                        my.loadClient();
    //                        //loadURL();
    //                        $(input).val(null)
    //                        setTimeout(function () {
    //                            my.itemsSearchBox.focus();
    //                        }, 500);
    //                        //setTimeout(function () {
    //                        //    $(input).attr({ 'title': 'Desativado', 'disabled': true });
    //                        //}, 400);
    //                    } else {
    //                        $.pnotify({
    //                            title: 'Aten&#231;&#227;o!',
    //                            text: 'Sua busca n&#227;o trouxe resultado algum.',
    //                            type: 'warning',
    //                            icon: 'fa fa-warning fa-lg',
    //                            addclass: "stack-bottomright",
    //                            stack: my.stack_bottomright
    //                        });
    //                    }
    //                } else {
    //                    $.pnotify({
    //                        title: 'Erro!',
    //                        text: data.Result,
    //                        type: 'error',
    //                        icon: 'fa fa-times-circle fa-lg',
    //                        addclass: "stack-bottomright",
    //                        stack: my.stack_bottomright
    //                    });
    //                    //$().toastmessage('showErrorToast', data.Result);
    //                }
    //            }).fail(function (jqXHR, textStatus) {
    //                console.log(jqXHR.responseText);
    //            });
    //        }, 500));
    //    });

    my.onRegisterClose = function () {
        my.vm.cashIn(0);
        my.vm.bankIn(0);
        my.vm.cardIn(0);
        my.vm.checkIn(0);
        my.vm.debitIn(0);
    };

    $('#btnRegister').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();
        $("input[type=text]").on('focusin', function () {
            var saveThis = $(this);
            window.setTimeout(function () {
                saveThis.select();
            }, 100);
        });
        $('#divReceipt').kendoWindow({
            title: 'Finalizar',
            modal: true,
            close: my.onRegisterClose
            //width: 550,
            //height: 420
        }).data("kendoWindow").center().open();
        setTimeout(function () {
            switch (true) {
                case (my.vm.selectedPayForm().toString().indexOf('Boleto') !== -1):
                    $('#bankInTextBox').focus().select();
                    break;
                case (my.vm.selectedPayForm().toString().indexOf('Cheque') !== -1):
                    $('#checkInTextBox').focus().select();
                    break;
                case (my.vm.selectedPayForm().toString().indexOf('Visa') !== -1):
                    $('#cardInTextBox').focus().select();
                    break;
                case (my.vm.selectedPayForm().toString().indexOf('Master Card') !== -1):
                    $('#cardInTextBox').focus().select();
                    break;
                case (my.vm.selectedPayForm().toString().indexOf('American Express') !== -1):
                    $('#cardInTextBox').focus().select();
                    break;
                case (my.vm.selectedPayForm().toString().indexOf('Dinners Club') !== -1):
                    $('#cardInTextBox').focus().select();
                    break;
                case (my.vm.selectedPayForm().toString().indexOf('Dinheiro') !== -1):
                    $('#cashInTextBox').focus().select();
                    break;
                case (my.vm.selectedPayForm().toString().indexOf('Debito') !== -1):
                    $('#debitInTextBox').focus().select();
                    break;
                default:
                    $('#cashInTextBox').focus().select();
            }
        }, 1000);
    });

    $('#btnFinalize').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            PortalId: portalID,
            EstimateId: my.estimateId,
            TotalCash: my.vm.cashIn(),
            TotalCheck: my.vm.checkIn(),
            TotalCard: my.vm.cardIn(),
            TotalDebit: my.vm.debitIn(),
            TotalBank: my.vm.bankIn(),
            CreatedByUser: userID,
            CreatedOnDate: moment().format(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };

        my.davId = my.estimateId;

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/estimates/addCashier',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {

                var prodParams = [];
                $.each(my.vm.items(), function (i, p) {
                    prodParams.push({
                        ProductId: p.ProductId,
                        QtyStockSet: p.ProductQty,
                        ModifiedByUser: userID,
                        ModifiedOnDate: moment().format()
                    });
                });
                $.ajax({
                    type: 'PUT',
                    contentType: 'application/json; charset=utf-8',
                    url: '/desktopmodules/riw/api/estimates/takeProductStock',
                    data: JSON.stringify(prodParams)
                });

                $('#btnPrintReceipt').click();
                //$().toastmessage('showSuccessToast', msg);
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Or&#231;amento Concluido.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                setTimeout(function () {
                    $("#divReceipt").data("kendoWindow").close();
                }, 1000);

                var kendoWindow = $('#davWindow').kendoWindow({
                    title: "Gera&#231;&#227;o de Documento Auxiliar de venda",
                    resizable: false,
                    modal: true,
                    width: 550
                });

                kendoWindow.data("kendoWindow").center().open();

                setTimeout(function () {
                    $('#btnRestart').click();
                }, 4000);
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', msg.Result);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
    });

    $('#btnSaveDav').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/estimates/saveDav?estimateId=' + my.davId
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'DAV salvo.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                my.davId = 0;

                setTimeout(function () {
                    $("#davWindow").data("kendoWindow").close();
                }, 1000);

            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
    });

    $('#btnLogin').click(function (e) {
        e.preventDefault();
        $('#divLogin').kendoWindow({
            title: 'Login',
            modal: true,
            width: '60%',
            height: '50%'
        }).data("kendoWindow").center().open();
        setTimeout(function () {
            $('#loginTextBox').focus();
        }, 1000);
        $('#loginTextBox').prop({ 'required': 'required' });
        $('#passwordTextBox').prop({ 'required': 'required' });
    });

    $('#passwordTextBox').on('keydown', function (e) {
        if (e.keyCode === 13) {
            $('#btnDoLogin').click();
        }
    });

    $('#btnDoLogin').click(function (e) {
        if (loginValidator.validate()) {
            e.preventDefault();
            var params = {
                portalId: portalID,
                userName: $('#loginTextBox').val(),
                password: $('#passwordTextBox').val()
            };

            $.ajax({
                type: 'POST',
                url: '/desktopmodules/riw/api/store/login',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    document.location.reload(true);
                } else {
                    $.pnotify({
                        title: 'Erro!',
                        text: data.Result,
                        type: 'error',
                        icon: 'fa fa-times-circle fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                }
            }).fail(function (jqXHR, textStatus) {
                console.log(jqXHR.responseText);
            });
        }
    });

    $('#btnLeave').click(function (e) {
        $.get('/desktopmodules/riw/api/store/ExpireTime', { portalId: portalID, userName: userName }, function (data) { });
        setTimeout(function () {
            location.href = returnURL;
        }, 500);
    });

    $('#btnDiscount').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        //$('#ddlCouponProducts').data("kendoDropDownList").value(0);
        //$('#ddlCouponProducts').data("kendoDropDownList").text(' Selecionar ');

        $('#divDiscount').kendoWindow({
            title: 'Descontos',
            modal: true,
            width: '80%',
            height: '70%',
            close: function (e) {
                my.vm.estimateItemId(0);
                my.vm.newItemDiscount(0);
                my.vm.itemPrice(0);
                my.vm.newItemQty(0);
                my.vm.oldItemQty(1);
            },
            open: function (e) {
                my.vm.estimateItemId(0);
                my.vm.newItemDiscount(0);
                my.vm.itemPrice(0);
                my.vm.newItemQty(0);
                my.vm.oldItemQty(1);
            }
        });

        $('#divDiscount').data("kendoWindow").center().open();
        
        $("input[type=text]").on('focusin', function () {
            var saveThis = $(this);
            window.setTimeout(function () {
                saveThis.select();
            }, 100);
        });
    });

    $('.btnReturn').click(function (e) {
        e.preventDefault();

        //$('#divDiscount').data("kendoWindow").close();
        //if (parent.$('#window')) {
        //    parent.$('#window').data("kendoWindow").close();
        //}
         parent.$(".k-window-content").data("kendoWindow").close();
    });

    $('#toggleCommentsTextAreaPreview').click(function (e) {
        e.preventDefault();
        var $this = $(this);

        if (this.value === 'preview') {
            var converter = new Showdown.converter();
            $('#preview').html(converter.makeHtml($('#commentsTextArea').val().trim()));
            var $dialog = $('#preview')
                .dialog({
                    autoOpen: false,
                    modal: true,
                    resizable: true,
                    dialogClass: 'dnnFormPopup',
                    title: 'Mensagem',
                    width: '50%',
                    buttons: {
                        'ok': {
                            text: 'Ok',
                            //priority: 'primary',
                            "class": 'btn btn-primary',
                            click: function (e) {
                                $dialog.dialog('close');
                                $dialog.dialog('destroy');
                            }
                        }
                    }
                });

            $dialog.dialog('open');
        }
    });

    //$("#couponGrid").delegate("tbody > tr", "dblclick", function () {
    //    $('#btnDiscount').click();
    //});

    //$("#couponGrid").keypress(function (e) {
    //    var unicode = e.keyCode ? e.keyCode : e.charCode;
    //    alert(unicode);
    //});

    $('#btnApplyDiscount').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        //var newItemPrice = 0;
        var originalAmount = 0;
        //for (var i = 0; i < $('#couponGrid').data("kendoGrid").dataSource.view().length; i++) {
        //    //var itemPrice = 0;
        //    if (my.vm.estimateItemId() === $('#couponGrid').data("kendoGrid").dataSource.view()[i].EstimateItemId) {
        //        newItemPrice = $('#couponGrid').data("kendoGrid").dataSource.view()[i].UnitValue - ($('#couponGrid').data("kendoGrid").dataSource.view()[i].UnitValue * my.vm.newItemDiscount() / 100);
        //        originalAmount = originalAmount + $('#couponGrid').data("kendoGrid").dataSource.view()[i].ExtendedAmount; // (newItemPrice * kendo.parseFloat(my.vm.newItemQty()));
        //        //} else {
        //        //    itemPrice = $('#couponGrid').data("kendoGrid").dataSource.view()[i].ExtendedAmount;
        //        //    originalAmount = originalAmount + itemPrice;
        //    }
        //}

        originalAmount = kendo.parseFloat(my.vm.itemPrice() - (my.vm.itemPrice() * my.vm.newItemDiscount() / 100));

        //my.vm.productId(dataItem.ProductId);
        //my.vm.productName(dataItem.ProductNam);
        //my.vm.estimateItemId(dataItem.EstimateItemId);
        //my.vm.newItemDiscount(dataItem.DiscountPerc);
        //my.vm.itemPrice(dataItem.UnitValue);
        //my.vm.newItemQty(dataItem.ProductQty);
        //my.vm.oldItemQty(dataItem.ProductQty);

        var estimateItems = [];
        estimateItems.push({
            EstimateId: my.estimateId,
            EstimateItemId: my.vm.estimateItemId(),
            ProductQty: my.vm.newItemQty(),
            ProductEstimatePrice: originalAmount, // my.vm.itemPrice()
            ProductDiscount: (my.vm.newItemDiscount() > 0 ? kendo.parseFloat(my.vm.newItemDiscount()) : 0), // $('#itemDiscountTextBox').data('kendoNumericTextBox').value(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        });

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/estimates/updateEstimateItems',
            data: JSON.stringify({
                EstimateItems: estimateItems,
                EstimateId: my.vm.estimateId(),
                PortalId: portalID,
                Discount: kendo.parseFloat(my.vm.couponDiscount() > 0 ? my.vm.couponDiscount() : 0),
                TotalAmount: kendo.parseFloat(originalAmount * my.vm.newItemQty()),
                ModifiedByUser: userID,
                ModifiedOnDate: moment().format(),
                Comment: $('#commentsTextArea').val()
            })
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                //if (kendo.parseInt(my.vm.selectedStatusId()) === 5 || kendo.parseInt(my.vm.selectedStatusId()) === 6) {

                //    var prodParams = [];
                //    prodParams.push({
                //        ProductId: my.vm.productId(),
                //        QtyStockSet: kendo.parseFloat(my.vm.oldItemQty()) - kendo.parseFloat(my.vm.newItemQty()),
                //        ModifiedByUser: userID,
                //        ModifiedOnDate: moment().format()
                //    });

                //    switch (true) {
                //        case (kendo.parseFloat(my.vm.oldItemQty()) > kendo.parseFloat(my.vm.newItemQty())):
                //            $.ajax({
                //                type: 'PUT',
                //                contentType: 'application/json; charset=utf-8',
                //                url: '/desktopmodules/riw/api/estimates/returnProductStock',
                //                data: JSON.stringify(prodParams)
                //            });
                //            break;
                //        case (kendo.parseFloat(my.vm.newItemQty()) > kendo.parseFloat(my.vm.oldItemQty())):
                //            $.ajax({
                //                type: 'PUT',
                //                contentType: 'application/json; charset=utf-8',
                //                url: '/desktopmodules/riw/api/estimates/takeProductStock',
                //                data: JSON.stringify(prodParams)
                //            });
                //            break;
                //    }
                //}
                if (my.vm.conditionNumberPayments() > 0) {
                    $('#btnCancelPayFormCond').click();
                }
                my.loadEstimate();
                my.startPayCondGrid(0);
                my.vm.productId(0);
                $('[data-bind*="displayTotal"]').css({ color: '' });
                $('#ddlCouponProducts').data("kendoDropDownList").value(0);
                $('#ddlCouponProducts').data('kendoDropDownList').text(' Selecionar ');
                //origProdTotal = 0;
                //discProd = 0;
                //newProdTotal = 0;
                //newSubTotal = 0;
                //newItemPrice = 0;
                originalAmount = 0;
                my.vm.estimateItemId(0);
                my.vm.newItemDiscount(0);
                my.vm.itemPrice(0);
                my.vm.newItemQty(1);
                my.vm.oldItemQty(0);
                //my.vm.originalProdTotal(0);
                //my.vm.estimateProdTotal(0);
                //$().toastmessage('showSuccessToast', 'Or&#231;amento atualizado com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Or&#231;amento atualizado.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                parent.$(".k-window-content").each(function () {
                    parent.$(this).data("kendoWindow").close();
                });
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', data.Result);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
    });

    my.statusDropDown = $('#ddlStatuses').kendoDropDownList({
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/statuses/GetStatuses?portalId=' + portalID + '&isDeleted=False',
                }
            }
        },
        optionsText: 'Normal',
        dataTextField: 'StatusTitle',
        dataValueField: 'StatusId',
        value: my.vm.selectedStatusId(),
        select: function (e) {
            var dataItem = this.dataItem(e.item.index());

            var params = {
                EstimateId: my.vm.estimateId(),
                StatusId: dataItem.StatusId,
                ModifiedByUser: userID,
                ModifiedOnDate: moment().format()
            };

            $.ajax({
                type: 'PUT',
                url: '/desktopmodules/riw/api/estimates/updateStatus',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {

                    var prodParams = [];
                    $.each(my.vm.items(), function (i, p) {
                        prodParams.push({
                            ProductId: p.ProductId,
                            QtyStockSet: p.ProductQty,
                            ModifiedByUser: userID,
                            ModifiedOnDate: moment().format()
                        });
                    });

                    switch (true) {
                        case ((dataItem.StatusId === 6) || (dataItem.StatusId === 5)):
                            $.ajax({
                                type: 'PUT',
                                contentType: 'application/json; charset=utf-8',
                                url: '/desktopmodules/riw/api/estimates/takeProductStock',
                                data: JSON.stringify(prodParams)
                            });
                            break;
                        case (dataItem.StatusId === 9):
                            $.ajax({
                                type: 'PUT',
                                contentType: 'application/json; charset=utf-8',
                                url: '/desktopmodules/riw/api/estimates/returnProductStock',
                                data: JSON.stringify(prodParams)
                            });
                            $('#btnRestart').click();
                            break;
                        default:
                            $.ajax({
                                type: 'PUT',
                                contentType: 'application/json; charset=utf-8',
                                url: '/desktopmodules/riw/api/estimates/returnProductStock',
                                data: JSON.stringify(prodParams)
                            });
                            $('#btnRestart').click();
                    }

                    my.vm.estimateStatus(dataItem.StatusTitle);
                    //$().toastmessage('showSuccessToast', 'Status do or&#231;amento atualizado com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Status do or&#231;amento atualizado.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    parent.$(".k-window-content").each(function () {
                        parent.$(this).data("kendoWindow").close();
                    });
                } else {
                    $.pnotify({
                        title: 'Erro!',
                        text: data.Result,
                        type: 'error',
                        icon: 'fa fa-times-circle fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    //$().toastmessage('showErrorToast', data.Result);
                }
            }).fail(function (jqXHR, textStatus) {
                console.log(jqXHR.responseText);
            });
        }
    }).data('kendoDropDownList');

    $('input[type=text].vertical').keydown(function (e) {
        //get the next index of text input element
        var next_idx = $('input[type=text].vertical').index(this) + 1;

        //get number of text input element in a html document
        var tot_idx = $('body').find('input[type=text].vertical').length;

        //enter button in ASCII code
        if (e.keyCode === 13) {
            if (tot_idx === next_idx) {
                if (this.id === 'cashInTextBox') {
                    $('input[type=text].vertical:eq(0)').focus();
                } else {
                    //go to the first text element if focused in the last text input element
                    $('input[type=text].vertical:eq(0)').focus();
                }
            } else {
                //go to the next text input element
                if (!$('input[type=text].vertical:eq(' + next_idx + ')').is('[readonly]')) {
                    $('input[type=text].vertical:eq(' + next_idx + ')').focus();
                }
            }
        }
    });

    //$('#bankInTextBox').kendoNumericTextBox({
    //    min: 0,
    //    spinners: false
    //});

    //$('#checkInTextBox').kendoNumericTextBox({
    //    min: 0,
    //    spinners: false
    //});

    //$('#cardInTextBox').kendoNumericTextBox({
    //    min: 0,
    //    spinners: false
    //});

    //$('#cashInTextBox').kendoNumericTextBox({
    //    min: 0,
    //    spinners: false
    //});

    //$('.decimals').keyup(function () {
    //    var value = $(this).val();
    //    if (isNaN(value)) {
    //        value = value.replace(/[^0-9\.]/g, '');
    //        if (value.split('.').length > 2) value = value.replace(/\.+$/, "");
    //    }
    //    $(this).val(value);
    //});

    $('input.enterastab').on('keydown', function (e) {
        if (e.keyCode === 13) {
            var focusable = $('input').filter(':visible');
            if (this.name === 'cashInTextBox') {
                focusable.eq(focusable.index(this) - 3).focus().select();
            } else {

                focusable.eq(focusable.index(this) + 1).focus().select();
            }

            //hfCashIn.value = my.vm.cashIn();
            //hfCardIn.value = my.vm.cardIn();
            //hfCheckIn.value = my.vm.checkIn();
            //hfBankIn.value = my.vm.bankIn();
            return false;
        }
    });

    my.vm.loadPayForms();

    if (my.estimateId > 0) {
        my.loadEstimate();
    }

    if (my.personId > 0) {
        my.getClient();
        if (my.estimateId === 0) {
        }
    }

    my.statusDropDown.text('Normal');

    //my.itemsSearchBox.focus();
    //$("#ddlPayForms").data('kendoDropDownList').text(' Selecionar ');

    $('#payCondMsg').html('<small><i>Escolha uma outra forma de pagamento (Ctrl+7).</i></small>');

    //$('#divImage').html('<img alt="" src="/portals/' + portalID + '/images/' + qeImage + '?maxwidth=' + $('#divImage').width() + '&maxheight=' + $('#divImage').height() + '" />');
    //$('#couponDiscountTextBox').kendoNumericTextBox({
    //    format: "##.00 '%'",
    //    spinners: false
    //});
    //$('#itemDiscountTextBox').kendoNumericTextBox({
    //    format: "##.00 '%'",
    //    spinners: false
    //});
    //loadURL();

    if (authorized === 3) $('#configLink').show().attr({ 'href': configURL });

    my.authorizationCheck = function () {
        switch (authorized) {
            case 1:
                if (my.vm.estimateId() > 0)
                    if (my.vm.estimatedByUser() !== userID) {
                        $('#btnRestart').click();
                    }
                break;
            case 2:
                my.statusDropDown.enable(false);
                if (my.vm.estimateId() > 0) {
                    if (userID !== my.vm.estimatedByUser()) {
                        my.itemsSearchBox.attr({ 'title': 'Desativado', 'disabled': true });
                        $('#clientsSearchBox').attr({ 'title': 'Desativado', 'disabled': true });
                        $('#btnSavePayFormCond').attr({ 'title': 'Desativado', 'disabled': true });
                        $('#btnCancelPayFormCond').attr({ 'title': 'Desativado', 'disabled': true });
                        $('#ddlPayForms').data("kendoDropDownList").enable(false);
                        $('#payIn').attr({ 'readonly': true, 'title': 'Desativado' });
                        $('#btnDiscount').attr({ 'title': 'Desativado', 'disabled': true });
                    }
                }
                break;
            case 3:
                $('#configLink').show().attr({ 'href': configURL });
        }
    };

    $.idleTimer(60000 * 15);

    $(document).on("idle.idleTimer", function (event, elem, obj) {
        // function you want to fire when the user goes idle
        $.get('/desktopmodules/riw/api/store/ExtendTime', function (data) {
            if (data) {
                $(document).idleTimer("reset")
            }
        });
    });

    if (Modernizr.audio) {
        if (Modernizr.audio.wav) {
            $("#audiofile").val("/desktopmodules/rildoinfo/webapi/content/sounds/sample.wav");
        }
        if (Modernizr.audio.mp3) {
            $("#audiofile").val("/desktopmodules/rildoinfo/webapi/content/sounds/bip2.mp3");
        }
    }
    else {
        $("#HTML5Audio").hide();
        $("#OldSound").html('<embed src="/desktopmodules/rildoinfo/webapi/content/sounds/sample.wav" autostart=false width=1 height=1 id="LegacySound" enablejavascript="true" >');
    }

    var currentFile = "";
    my.playAudio = function () {
        var oAudio = document.getElementById('myaudio');
        // See if we already loaded this audio file.
        if ($("#audiofile").val() !== currentFile) {
            oAudio.src = $("#audiofile").val();
            currentFile = $("#audiofile").val();
        }
        var test = $("#myaudio");
        test.src = $("#audiofile").val();
        oAudio.play();
    };

    setTimeout(function () {
        $('#itemQtyBox').focus();
    }, 1000);

});

function clientFormatResult(data) {
    return '<strong>Cliente: </strong><span>' + data.FirstName + ' ' + data.LastName + ' / ' + data.DisplayName + '</span>'
        + '<br /><strong>Email: </strong><span>' + data.Email + '</span>'
        + '<br /><strong>Telefone: </strong><span>' + data.Telephone + '</span>'
        + '<br /><strong>Endere&#231;o: </strong><span>' + data.Street + ' ' + data.Unit + '</span>';
}

function clientFormatSelection(data) {
    return data.DisplayName;
}
