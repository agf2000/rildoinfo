
$(function () {

    my.personId = my.getParameterByName('personId');
    my.estimateId = my.getParameterByName('estimateId');
    my.new = my.getTopParameterByName('new');
    my.hasFocus = true;

    my.viewModel();

    my.vm.viewPrice(JSON.parse(amplify.store.sessionStorage('showEstimatePrice').toLowerCase()));

    $('.icon-exclamation-sign').popover({
        delay: {
            show: 500,
            hide: 100
        },
        placement: 'top',
        trigger: 'hover'
    });

    $('#viewPriceCheckBox').bootstrapSwitch();
    $('#lockedCheckBox').bootstrapSwitch();

    $('[data-provider]').popover({
        delay: {
            show: 500,
            hide: 100
        },
        placement: 'top',
        trigger: 'hover'
    });

    $.valHooks.textarea = {
        get: function (elem) {
            return elem.value.replace(/\r?\n/g, "\r\n");
        }
    };

    window.onfocus = window.onblur = window.onpageshow = window.onpagehide = function (e) {
        if (e.type === 'focus') {
            //console.log("visible");
            my.hasFocus = true;
        } else {
            //console.log("hidden");
            my.hasFocus = false;
        }
    };

    $("input[type=text]").on('focusin', function () {
        var saveThis = $(this);
        window.setTimeout(function () {
            saveThis.select();
        }, 100);
    });

    my.hub = $.connection.estimatesHub;

    my.hub.client.pushEstimate = function () {
        my.cancelAmounts();
        my.estimateItemsData.read();
        my.loadEstimate();
        my.cancelAmounts();
        setAmounts();
        my.sendNotification('/desktopmodules/rildoinfo/webapi/content/images/ri-logo.png', siteName, 'Orcamento atualizado.', 6000, !my.hasFocus);
    };

    my.hub.client.pushEstimateTitle = function (title) {
        $('#estimateTitleLabel').html(title);
        my.sendNotification('/desktopmodules/rildoinfo/webapi/content/images/ri-logo.png', siteName, 'Forma de pagamento atualizada.', 6000, !my.hasFocus);
    };

    my.hub.client.pushPayCondition = function () {
        my.loadEstimate();
        my.sendNotification('/desktopmodules/rildoinfo/webapi/content/images/ri-logo.png', siteName, 'Forma de pagamento atualizada.', 6000, !my.hasFocus);
    };

    my.hub.client.closeEstimate = function (estimateId) {
        if (parent.$('#window')) {
            parent.$('#window').data("kendoWindow").close();
        }
        //my.sendNotification('/desktopmodules/rildoinfo/webapi/content/images/ri-logo.png', siteName, 'Orcamento "' + estimateId.toUpperCase() + '" concluido.', 6000, !my.hasFocus);
    };

    my.hub.client.pushCancelPayCondition = function () {
        my.vm.selectedPayFormId(null);
        $('#divPayFormCond').fadeOut();
        $('#divChosenPayCond').fadeOut();
        $('#divPayIn').fadeOut();
        $('#btnSavePayFormCond').fadeOut();
        $('#divPayForms').fadeIn();
        my.sendNotification('/desktopmodules/rildoinfo/webapi/content/images/ri-logo.png', siteName, 'Forma de pagamento atualizada.', 6000, !my.hasFocus);
    };

    my.hub.client.pushMessageComment = function (item, index) {
        my.vm.filteredMessages()[index].messageComments.unshift(new my.MessageComment(item));
        my.sendNotification('/desktopmodules/rildoinfo/webapi/content/images/ri-logo.png', siteName, 'Mensagem atualizada.', 6000, !my.hasFocus);
    };

    my.hub.client.pushMessage = function (item) {
        my.vm.estimateMessages.unshift(new my.EstimateMessage(item));
        my.sendNotification('/desktopmodules/rildoinfo/webapi/content/images/ri-logo.png', siteName, 'Mensagem atualizada.', 6000, !my.hasFocus);
    };

    my.hub.client.pushHistoryComment = function (item, index) {
        my.vm.filteredHistories()[index].historyComments.unshift(new my.HistoryComment(item));
        my.sendNotification('/desktopmodules/rildoinfo/webapi/content/images/ri-logo.png', siteName, 'Historico atualizado.', 6000, !my.hasFocus);
    };

    my.hub.client.pushHistory = function (item) {
        my.vm.estimateHistories.unshift(new my.EstimateHistory(item));
        my.sendNotification('/desktopmodules/rildoinfo/webapi/content/images/ri-logo.png', siteName, 'Historico atualizado.', 6000, !my.hasFocus);
    };

    my.hub.client.connectedUser = function () {
        my.hub.server.join({ UserId: userID, PortalId: portalID, GroupName: portalID.toString() + '_' + my.estimateId.toString() });
    };

    my.hub.client.pushNewTerm = function (term) {
        $('#estimateTermTextarea').val(toMarkdown(amplify.store.sessionStorage('estimateTerm')));
    };

    //my.hub.client.pushConfig = function () {
    //    my.cancelAmounts();
    //    my.estimateItemsData.read();
    //    my.loadEstimate();
    //    my.cancelAmounts();
    //    setAmounts();
    //    my.sendNotification('/desktopmodules/rildoinfo/webapi/content/images/ri-logo.png', siteName, 'O website ' + siteURL + ' foi atualizado.', 6000, !my.hasFocus);
    //};

    //my.hub.client.reconnectUser = function () {
    //    //my.hub.server.getUserInfo();
    //    if (userID === 2) {
    //        my.vm.clientConnected(true);
    //    }
    //};

    //my.hub.client.disconnectedUser = function () {
    //    // to do
    //};

    //my.hub.client.showUsersOnLine = function (user) {
    //    my.vm.connectedUsers.push(new my.User(user));
    //};

    my.hub.client.showConnected = function (users) {
        my.vm.connectedUsers.removeAll();
        $.each(users, function (index, user) {
            my.vm.connectedUsers.push(new my.User(user));
        });
    };

    $.connection.hub.start().done(function () {
        my.hub.server.clientsJoin(portalID.toString() + '_' + my.estimateId.toString());
    });

    $("#ddlPayForms").data('kendoDropDownList').text(' Selecionar ');

    $('#estimateTabs').kendoTabStrip({
        animation: {
            // fade-out current tab over 1000 milliseconds
            close: {
                effects: "fadeOut"
            },
            // fade-in new tab over 500 milliseconds
            open: {
                effects: "fadeIn"
            }
        },
        select: function (e) {
            if (e.item.id === 'tab_2') {
                $.ajax({
                    url: '/desktopmodules/riw/api/estimates/getMessages?estimateId=' + my.estimateId
                }).done(function (data) {
                    if (data) {
                        if (data.length > 0) {
                            var mappedPosts = $.map(data, function (item) { return new my.EstimateMessage(item); });
                            my.vm.estimateMessages(mappedPosts);
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
            if (e.item.id === 'tab_3') {
                $.ajax({
                    url: '/desktopmodules/riw/api/estimates/getHistories?estimateId=' + my.estimateId
                }).done(function (data) {
                    if (data) {
                        if (data.length > 0) {
                            var mappedPosts = $.map(data, function (item) { return new my.EstimateHistory(item); });
                            my.vm.estimateHistories(mappedPosts);
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
        }
    });

    $('#qTyTextBox').kendoNumericTextBox({
        placeholder: 'Qde',
        value: qTy,
        spinners: false,
        min: 1
    });

    $('#kddlSalesGroup').kendoDropDownList({
        autoBind: false,
        dataTextField: 'DisplayName',
        dataValueField: 'UserId',
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/people/GetUsersByRoleGroup?portalId=' + portalID + '&roleGroupName=Departamentos'
                }
            }
        }
    });

    $('#kddlStatuses').kendoDropDownList({
        autoBind: false,
        dataTextField: "StatusTitle",
        dataValueField: "StatusId",
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/statuses/getStatuses?portalId=' + portalID + '&isDeleted=False'
                }
            }
        },
        dataBound: function (e) {
            var dropDown = this;
            $.each(dropDown.dataSource.data(), function (i, status) {
                if (status)
                    var itemToRemove = 0;
                if (status !== undefined) {
                    switch (true) {
                        case (status.StatusId === 5):
                            itemToRemove = dropDown.dataSource.at(i);
                            dropDown.dataSource.remove(itemToRemove);
                            break;
                        case (status.StatusId === 10):
                            itemToRemove = dropDown.dataSource.at(i);
                            dropDown.dataSource.remove(itemToRemove);
                            break;
                        default:

                    }
                }
            });
        }
    });

    $("#productSearch").select2({
        placeholder: "Busque produtos por nome, *ref. ou #c&#243;d. de barra.",
        allowClear: true,
        minimumInputLength: 2,
        id: function (data) {
            return {
                ProductId: data.ProductId,
                ProductName: data.ProductName,
                Summary: data.Suammry,
                ProductRef: data.ProductRef,
                Barcode: data.Barcode,
                UnitValue: data.UnitValue,
                ProductsRelatedCount: data.ProductsRelatedCount,
                ProductImageId: data.ProductImageId,
                Extension: data.Extension,
                CategoriesNames: data.CategoriesNames,
                ProductUnit: data.ProductUnit,
                QtyStockSet: data.QtyStockSet,
                Finan_Cost: data.Finan_Cost,
                Finan_Rep: data.Finan_Rep,
                Finan_SalesPerson: data.Finan_SalesPerson,
                Finan_Tech: data.Finan_Tech,
                Finan_Telemarketing: data.Finan_Telemarketing,
                Finan_Manager: data.Finan_Manager
            };
        },
        ajax: {
            url: "/desktopmodules/riw/api/products/getproducts",
            quietMillis: 100,
            data: function (term, page) { // page is the one-based page number tracked by Select2

                var fieldName = term;

                return {
                    portalId: portalID,
                    searchField: fieldName.charAt(0) === '*' ? 'ProductRef' : fieldName.charAt(0) === '#' ? 'BarCode' : 'ProductName',
                    searchString: fieldName.charAt(0) === '*' || fieldName.charAt(0) === '#' ? fieldName.slice(1) : fieldName,
                    pageIndex: page,
                    pageSize: 10,
                    orderBy: 'ProductName',
                    orderDesc: 'ASC'
                };
            },
            results: function (data, page) {
                var more = (page * 10) < data.total; // whether or not there are more results available

                // notice we return the value of more so Select2 knows if more results can be loaded
                return { results: data.data, more: more };
            }
        },
        formatResult: productFormatResult, // omitted for brevity, see the source of this page
        formatSelection: productFormatSelection, // omitted for brevity, see the source of this page
        dropdownCssClass: "bigdrop", // apply css that makes the dropdown taller
        escapeMarkup: function (m) { return m; } // we do not want to escape markup since we are displaying html in results
    });

    $('#productSearch').on("select2-selecting", function (e) {
        if (e.val.ProductUnit === 1) {
            $('#qTyTextBox').data('kendoNumericTextBox').options.format = '';
            setTimeout(function () {
                $('#btnAddSelectedProduct').focus();
            }, 100);
        }
        $('#qTyTextBox').data('kendoNumericTextBox').value(1);
    });

    //$('#messageBody').markdown({
    //    autofocus: true,
    //    savable: false,
    //    disableButtons: 'image'
    //});
    //$('#messageBody').css({ 'min-width': '90% !important', 'height': '150px' }).attr({ 'cols': '10', 'rows': '7' });
    //$('#estimateHistory').css({ 'min-width': '90% !important', 'height': '70px' }).attr({ 'cols': '10', 'rows': '3' });

    my.loadEstimate = function () {
        $.ajax({
            url: '/desktopmodules/riw/api/estimates/getEstimate?estimateId=' + my.estimateId + '&portalId=' + portalID + '&userId=' + userID + '&getAll=' + (authorized > 2 ? 'true' : 'false'),
            cache: false
        }).done(function (data) {
            if (data.Result.indexOf('success') !== -1) {

                $('#preEmailSubjectTextBox').val(estimateEmailSubject);

                $('#preEmailMessageBody').val(toMarkdown(estimateBody));

                $('#notifySubjectTextBox').val(estimateNotifySubject);

                $('#notifyMessageBody').val(toMarkdown(estimateNotify));

                var subject = estimateEmailSubject.replace('[WEBSITELINK]', siteName).replace('[ID]', my.estimateId);

                var estimateLink = '[link](' + estimateURL + '?estimateId=' + my.estimateId + ')' // '[link] (' + estimateURL + '?eid=' + my.eId + ' "Clique para acessar o orçamento")';
                var body = estimateBody.replace('[ORCAMENTOLINK]', estimateLink);
                body = body.replace('[CLIENTE]', data.Estimate.ClientDisplayName);
                body = body.replace('[ID]', my.estimateId);

                $('#emailSubjectTextBox').val(subject);
                //$('.markdown-editor').text(my.vm.message());

                $('#messageBody').val(toMarkdown(body));

                //if (authorized > 1) {
                //    $('#fromNameTextBox').val(data.SalesRepName);
                //} else {
                //    $('#fromNameTextBox').val(data.ClientDisplayName);
                //}
                $('#toEmailTextBox').val(data.Estimate.ClientEmail);

                my.vm.salesPersonId(data.Estimate.SalesRep);
                my.vm.createdOnDate(data.Estimate.CreatedOnDate);
                my.vm.salesRepName(data.Estimate.SalesRepName);
                my.vm.salesRepEmail(data.Estimate.SalesRepEmail);
                my.vm.salesRepPhone(data.Estimate.SalesRepPhone);
                my.vm.statusTitle(data.Estimate.StatusTitle);
                my.vm.clientDisplayName(data.Estimate.ClientDisplayName);
                my.vm.clientPhone(data.Estimate.ClientTelephone);
                my.vm.clientEmail(data.Estimate.ClientEmail);
                my.vm.clientUserId(data.Estimate.UserId);
                if (data.Estimate.EstimateTitle) {
                    if (data.Estimate.EstimateTitle.length > 2) {
                        $('#estimateTitleLabel').text(data.Estimate.EstimateTitle);
                    } else {
                        if ((estimateLocked()) && (authorized === 1)) {
                            $('#divEstimateTitle').hide();
                        } else {
                            $('#divEstimateTitle').show();
                        }
                    }
                } else {
                    if ((estimateLocked()) && (authorized === 1)) {
                        $('#divEstimateTitle').hide();
                    } else {
                        $('#divEstimateTitle').show();
                    }
                }
                if (authorized > 1) {
                    $('#spanClientInfo').html('<div class="pull-left"><ul>'
                        + '<li><strong>Cliente:</strong> ' + data.Estimate.ClientDisplayName + '</li>'
                        + (data.Estimate.ClientEmail ? ('<li><strong>Email:</strong> ' + data.Estimate.ClientEmail + '</li>') : '')
                        + (data.Estimate.ClientTelephone ? ('<li><span><strong>Telefone:</strong></span> ' + my.formatPhone(data.Estimate.ClientTelephone) + '</li>') : '')
                        + (data.Estimate.ClientCell ? ('<li><span><strong>Celular:</strong></span> ' + my.formatPhone(data.Estimate.ClientCell) + '</li>') : '')
                        + (data.Estimate.ClientFax ? ('<li><span><strong>Fax:</strong></span> ' + my.formatPhone(data.Estimate.ClientFax) + '</li>') : '')
                        + (data.Estimate.ClientZero800s ? ('<li><span><strong>0800:</strong></span> ' + my.formatPhone(data.Estimate.ClientZero800s) + '</li></ul></div>') : ''));
                    //if (data.Estimate.ClientAddress) {
                    //    $('#spanClientInfo').html($('#spanClientInfo').html() + '<div class="offset1 pull-left"><address>'
                    //    + (data.Estimate.ClientAddress + ' N&#186; ' + data.Estimate.ClientUnit + '<br />')
                    //    + (data.Estimate.ClientComplement ? (data.Estimate.ClientComplement + '<br />') : '')
                    //    + (data.Estimate.ClientDistrict ? ('<strong>Bairro:</strong> ' + data.Estimate.ClientDistrict + '<br />') : '')
                    //    + (data.Estimate.ClientCity ? (data.Estimate.ClientCity + ', ') : '')
                    //    + (data.Estimate.ClientRegion ? (data.Estimate.ClientRegion + (data.Estimate.ClientPostalCode ? (' <strong>CEP:</strong> ' + my.formatPostalcode(data.Estimate.ClientPostalCode)) : '') + '<br />') : '')
                    //    + (data.Estimate.ClientCountry ? (data.Estimate.ClientCountry + '</address></div>') : ''));
                    //}
                }
                $('#spanClientInfo').html($('#spanClientInfo').html() + '<div class="clearfix"></div>');

                $('#divSalesPerson').html('<address>'
                        + '<strong>Vendedor:</strong> ' + data.Estimate.SalesRepName + '<br />'
                        //+ (data.Estimate.ClientEmail ? ('<dt>Email:</dt><dd>' + data.Estimate.ClientEmail + '</dd>') : '')
                        + '<strong>Telefone:</strong> ' + my.formatPhone(data.Estimate.SalesRepPhone));

                if (data.Estimate.PayCondType !== '') {
                    my.vm.selectedPayForm(data.Estimate.PayCondType);
                    my.vm.conditionPayIn(kendo.parseFloat(data.Estimate.PayCondIn));
                    my.vm.conditionPayInDays(data.Estimate.PayInDays);
                    my.vm.payInMin(data.Estimate.PayCondIn);
                    my.vm.conditionNumberPayments(data.Estimate.PayCondN);
                    my.vm.conditionInterest(data.Estimate.PayCondPerc);
                    $('#interval').text(data.Estimate.PayCondInterval < 0 ? function () { $('#payIn').attr({ 'readonly': true, 'title': 'Desativado' }); my.vm.conditionInterval(data.Estimate.PayCondInterval); return 'A Vista'; } : data.Estimate.PayCondInterval > 0 ? data.Estimate.PayCondInterval : 'Mensal');
                    $('#divPayForms').hide();
                    $('#divPayFormCond').hide();
                    $('#divChosenPayCond').show();
                    if (data.Estimate.PayCondIn > 0) {
                        $('#divPayIn').show();
                        $('#btnSavePayFormCond').show();
                    }
                }

                $('#estimateDiscount').data('kendoNumericTextBox').value(data.Estimate.Discount);
                $('#viewPriceCheckBox').bootstrapSwitch('setState', data.Estimate.ViewPrice);
                $('#lockedCheckBox').bootstrapSwitch('setState', data.Estimate.Locked);

                my.vm.viewPrice(data.Estimate.ViewPrice);
                my.vm.estimateLocked(data.Estimate.Locked);

                if (my.vm.viewPrice() == false) {
                    var $dialog = $('<div></div>')
                        .html('<p class="confirmDialog"><p>Caro(a) ' + my.vm.clientDisplayName() + ',</p>Este or&#231;amento est&#225; sendo analizado por <strong>' + my.vm.salesRepName() + '</strong>. Em alguns instantes lhe ser&#225; permitido ver os pre&#231;os.<p>Por favor, aguarde!</p></p>')
                        .dialog({
                            autoOpen: false,
                            modal: true,
                            resizable: false,
                            dialogClass: 'dnnFormPopup',
                            title: 'Mensagem',
                            width: 360,
                            buttons: {
                                'ok': {
                                    text: 'Ok',
                                    //priority: 'primary',
                                    "class": 'btn btn-primary',
                                    click: function (e) {
                                        // on ok, close dialog
                                        $dialog.dialog('close');
                                        $dialog.dialog('destroy');
                                    }
                                }
                            }
                        });

                    $dialog.dialog('open');
                }

                if (data.Estimate.Inst) {
                    $('#estimateTermTextarea').val(toMarkdown(data.Estimate.Inst));
                } else {
                    $('#estimateTermTextarea').val(toMarkdown(amplify.store.sessionStorage('estimateTerm')));
                }

                $('#kddlSalesGroup').data('kendoDropDownList').value(data.Estimate.SalesRep);
                $('#kddlStatuses').data('kendoDropDownList').value(data.Estimate.StatusId);
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast: ', data.Result);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
    };

    my.loadEstimate();

    my.setAmounts = function () {
        var productDiscountValue = my.vm.productGrossSale() - my.vm.productNetSale();

        $('#ddEstimateDiscountValue').text(kendo.toString(my.vm.productNetSale() / 100 * $('#estimateDiscount').data('kendoNumericTextBox').value(), 'c'));

        if ($('#estimateDiscount').data('kendoNumericTextBox').value() > 0) {
            my.vm.productNetSale(my.vm.productNetSale() - my.vm.productNetSale() / 100 * $('#estimateDiscount').data('kendoNumericTextBox').value());
        }

        //my.total = my.productNetSale;

        my.vm.discountTotalPerc((my.vm.productGrossSale() - my.vm.productNetSale()) / my.vm.productGrossSale());

        var markUpValue = my.vm.productNetSale() - my.vm.productTotalPaid();
        var markUpPerc = ((my.vm.productNetSale() - my.vm.productTotalPaid()) / my.vm.productTotalPaid()) * 100;
        if (!my.vm.productTotalPaid() > 0) {
            markUpPerc = 100;
        }

        $('#ddOriginalAmount').text(kendo.toString(my.vm.productGrossSale(), 'c'));

        $('#ddProductDiscountValue').text(kendo.toString(productDiscountValue, 'c'));

        if (my.vm.discountTotalPerc() > 0) {
            $('#ddTotalDiscountPerc').text(kendo.toString(my.vm.discountTotalPerc(), 'p'));
        } else {
            //discountTotalPerc = 0;
            $('#ddTotalDiscountPerc').text(kendo.format("{0:p}", my.vm.discountTotalPerc() / 100));
        }

        my.vm.discountTotalValue(productDiscountValue + (my.vm.productGrossSale() / 100 * $('#estimateDiscount').data('kendoNumericTextBox').value()));

        $('#ddTotalDiscountValue').text(kendo.toString(my.vm.discountTotalValue(), 'c'));

        $('#ddTotal').text(kendo.toString(my.vm.productNetSale(), 'c'));

        $('#ddMarkupAmount').text(kendo.toString(markUpValue, 'c'));

        $('#ddMarkupPerc').text(kendo.format("{0:p}", markUpPerc / 100));

        var profitAmount = markUpValue - my.vm.paidComission();
        $('#ddProfitAmount').text(kendo.toString(profitAmount, 'c'));

        $('#ddCommsAmount').text(kendo.toString(my.vm.paidComission(), 'c'));
        $('#ddCommsPerc').text(kendo.format('{0:p}', ((my.vm.paidComission() / my.vm.productNetSale()) * 100) / 100));

        var profitPerc = 0;
        if (profitAmount) {
            profitPerc = (profitAmount / my.vm.productNetSale()) * 100.0;
        }
        $('#ddProfitPerc').text(kendo.format("{0:p}", profitPerc / 100));

        my.hiddenTotal = my.vm.productNetSale();

        kendo.ui.progress($("#estimateAmount"), false);
        kendo.ui.progress($("#adminAmount"), false);
    };

    //my.loadClient = function () {
    //    $.ajax({
    //        url: '/desktopmodules/riw/api/people/getPerson?personId=' + my.pId + '&lang=pt-BR'
    //    }).done(function (data) {
    //        if (data) {
    //            my.vm.clientDisplayName(data.DisplayName);
    //            my.vm.clientPhone(data.Telephone);
    //            my.vm.clientEmail(data.Email);
    //            $('#spanClientInfo').html('<div class="pull-left"><dl class="dl-horizontal">'
    //                + '<dt>Cliente:</dt><dd>' + data.DisplayName + '</dd>'
    //                + (data.Email ? ('<dt>Email:</dt><dd>' + data.Email + '</dd>') : '')
    //                + (data.Telephone ? ('<dt>Telefone:</dt><dd>' + my.formatPhone(data.Telephone) + '</dd>') : '')
    //                + (data.Cell ? ('<dt>Celular:</dt><dd>' + my.formatPhone(data.Cell) + '</dd>') : '')
    //                + (data.Fax ? ('<dt>Fax:</dt><dd>' + my.formatPhone(data.Fax) + '</dd>') : '')
    //                + (data.Zero800s ? ('<dt>0800:</dt><dd>' + my.formatPhone(data.Zero800s) + '</dd></dl></div>') : ''));
    //            if (data.Street) {
    //                $('#spanClientInfo').html($('#spanClientInfo').html() + '<div class="pull-left"><dl class="dl-horizontal">'
    //                + '<dt>Rua:</dt><dd>' + data.Street + ' N&#186; ' + data.Unit + '</dd>'
    //                + (data.Complement ? ('<dt>&nbsp;</dt><dd>' + data.Complement + '</dd>') : '')
    //                + (data.District ? ('<dt>Bairro:</dt><dd>' + data.District + '</dd>') : '')
    //                + (data.City ? ('<dt>Cidade:</dt><dd>' + data.City + '</dd>') : '')
    //                + (data.Region ? ('<dt>Estado:</dt><dd>' + data.Region + (data.PostalCode ? (' ' + data.PostalCode ) : '') + '</dd>') : '')
    //                + (data.Country ? ('<dt>Pa&#237;s:</dt><dd>' + data.Country + '</dd></dl></div>') : ''));
    //            }
    //            $('#spanClientInfo').html($('#spanClientInfo').html() + '<div class="clearfix"></div>');
    //        } else {
    //            $().toastmessage('showErrorToast: ', data.Result);
    //        }
    //    }).fail(function (jqXHR, textStatus) {
    //        console.log(jqXHR.responseText);
    //    });
    //};

    function detailInit(e) {
        var detailRow = e.detailRow;

        var lvImages = detailRow.find('.imagesListView');
        $(lvImages).kendoListView({
            dataSource: new kendo.data.DataSource({
                transport: {
                    read: {
                        url: '/desktopmodules/riw/api/products/GetProductImages?productId=' + e.data.ProductId
                    }
                }
            }),
            template: kendo.template($("#tmplProductImages").html()),
            dataBound: function (e) {
                var lv = this;
                if (lv.dataSource.data().length > 1) {
                    $(lvImages).show();
                } else {
                    $(lvImages).hide();
                }

                // initiate colobox jquery plugin
                $(".photo").colorbox();
            }
        });
    }

    // create kendo dataSource transport to get products
    my.estimateItemsTransport = {
        read: {
            url: '/desktopmodules/riw/api/estimates/getEstimateItems'
        },
        parameterMap: function (data, type) {
            return {
                portalId: portalID,
                estimateId: my.estimateId,
                lang: 'pt-BR'
            };
        }
    };

    // create kendo dataSource for getting products transport
    my.estimateItemsData = new kendo.data.DataSource({
        transport: my.estimateItemsTransport,
        schema: {
            model: {
                id: 'EstimateItemId',
                fields: {
                    EstimateItemId: {
                        type: 'number'
                    },
                    ProductEstimateOriginalPrice: {
                        editable: true, nullable: false
                    },
                    ProductEstimatePrice: {
                        editable: false, nullable: false
                    },
                    ProductQty: { type: 'number', validation: { required: { message: 'Campo obrgat&#243;rio' } } },
                    ProductDiscount: { type: "number", validation: { min: 0, required: { message: 'Campo obrgat&#243;rio' } } },
                    ExtendedAmount: {
                        editable: false, nullable: false
                    },
                    selected: {
                        editable: false, nullable: false
                    }
                }
            },
            data: 'Items'
        },
        pageSize: 10
    });

    $('#estimateItemsGrid').kendoGrid({
        dataSource: my.estimateItemsData,
        toolbar: kendo.template($("#tmplToolbar").html()),
        columns: [
            //define template column with checkbox and attach click event handler
            {
                title: ' ',
                template: "<input type='checkbox' class='checkbox' />",
                width: 35,
                sortable: false
            },
            {
                title: 'C&#243;digo',
                template: '# if (Barcode) { # <strong>CB: </strong> #= Barcode # # } else if (ProductRef) { # <strong>REF: </strong> #= ProductRef # # } #',
                width: 130
            },
            {
                title: 'Produto',
                template: '#= ProductName #'
            },
            {
                title: 'Quantidade',
                field: 'ProductQty',
                width: 80,
                editor: editNumber
            },
            {
                title: 'Pre&#231;o Original',
                field: 'ProductEstimateOriginalPrice',
                format: '{0:n}',
                //template: '#= kendo.toString(ProductEstimateOriginalPrice, "n") #',
                attributes: { class: 'text-right' },
                width: 100,
                editor: editNumber
            },
            {
                title: 'Desc. %',
                field: 'ProductDiscount',
                //format: '{0:p}',
                template: '#= kendo.format("{0:p}", (ProductDiscount / 100)) #',
                attributes: { class: 'text-right' },
                width: 70,
                editor: editNumber
            },
            {
                title: 'Pre&#231;o',
                field: 'ProductEstimatePrice',
                format: '{0:n}',
                //template: '#= kendo.toString((ProductEstimatePrice * ProductQty), "n") #',
                attributes: { class: 'text-right' },
                width: 100
            },
            {
                title: 'Total',
                field: 'ExtendedAmount',
                format: '{0:n}',
                //template: '#= kendo.toString(ExtendedAmount, "n") #',
                attributes: { class: 'text-right' },
                width: 100
            }
        ],
        //pageable: {
        //    pageSizes: [20, 40, 60],
        //    refresh: true,
        //    numeric: false,
        //    input: true,
        //    messages: {
        //        display: "{0} - {1} de {2} Itens",
        //        empty: "Sem Registro.",
        //        page: "P&#225;gina",
        //        of: "de {0}",
        //        itemsPerPage: "Itens por p&#225;gina",
        //        first: "Ir para primeira p&#225;gina",
        //        previous: "Ir para p&#225;gina anterior",
        //        next: "Ir para pr&#243;xima p&#225;gina",
        //        last: "Ir para &#250;ltima p&#225;gina",
        //        refresh: "Recarregar"
        //    }
        //},
        sortable: true,
        reorderable: true,
        resizable: true,
        scrollable: true,
        editable: true,
        dataBound: function (e) {
            var grid = this;
            if (grid.dataSource.data().length) {
                $.each(grid.dataSource.data(), function (i, item) {
                    my.vm.productGrossSale(my.vm.productGrossSale() + (item.ProductEstimateOriginalPrice * item.ProductQty));
                    my.vm.productDiscount(my.vm.productDiscount() + item.ProductDiscount);
                    my.vm.productTotalPaid(my.vm.productTotalPaid() + (item.Finan_Cost * item.ProductQty));
                    my.vm.productNetSale(my.vm.productNetSale() + ((item.ProductEstimateOriginalPrice - (item.ProductEstimateOriginalPrice / 100 * item.ProductDiscount)) * item.ProductQty));
                    var comissions = item.Finan_Rep + item.Finan_SalesPerson + item.Finan_Tech + item.Finan_Telemarketing + item.Finan_Manager;
                    my.vm.paidComission(my.vm.paidComission() + ((my.vm.paidComission() + my.vm.productNetSale()) * comissions / 100));

                    var rowSelector = ">tr:nth-child(" + (i + 1) + ")";

                    //Grab a reference to the corrosponding data row
                    var row = grid.tbody.find(rowSelector);
                    if (item.IsDeleted) {
                        row.addClass('isDeleted');
                    }
                    if (item.QtyStockSet <= 0) {
                        row.addClass('negativeStock');
                    }

                    if (my.checkedIds[item.EstimateItemId]) {
                        //grid.tbody.find("tr[data-uid='" + item.uid + "']")
                        row.addClass("k-state-selected")
                            .find(".checkbox")
                            .attr('checked', 'checked');
                    }
                });

                if (authorized === 1) {
                    if (my.vm.estimateLocked()) grid.hideColumn(0);
                    grid.hideColumn(4);
                    grid.hideColumn(5);
                    if (!$('#viewPriceCheckBox').is(':checked')) {
                        grid.hideColumn(6);
                        grid.hideColumn(7);
                        grid.hideColumn(8);
                    }
                }
            }
            my.appLocked = false;

            $("input[type=text]").on('focusin', function () {
                var saveThis = $(this);
                window.setTimeout(function () {
                    saveThis.select();
                }, 100);
            });

            this.element.find('tr.k-master-row').each(function () {
                var row = $(this);
                var data = grid.dataSource.getByUid(row.data('uid'));
                // this example will work if ReportId is null or 0 (if the row has no details)
                if (!data.get('Summary') || !data.get('ProductImageId')) {
                    row.find('.k-hierarchy-cell a').css({ opacity: 0.3, cursor: 'default' }).click(function (e) { e.stopImmediatePropagation(); return false; });
                }
            });
        },
        detailTemplate: kendo.template($("#tmplProductDetail").html()),
        detailInit: detailInit
    });

    //bind click event to the checkbox
    $('#estimateItemsGrid').data('kendoGrid').table.on("click", ".checkbox", selectRow);

    my.checkedIds = {};

    //on click of the checkbox:
    function selectRow() {
        var checked = this.checked,
            row = $(this).closest("tr"),
            grid = $('#estimateItemsGrid').data('kendoGrid'),
            dataItem = grid.dataItem(row);

        if (checked) {
            my.checkedIds[dataItem.id] = {
                ProductId: dataItem.ProductId,
                ProductEstimateOriginalPrice: dataItem.ProductEstimateOriginalPrice,
                ProductEstimatePrice: dataItem.ProductEstimatePrice,
                ProductDiscount: dataItem.ProductDiscount,
                ProductName: dataItem.ProductName,
                ProductQty: dataItem.ProductQty,
                ProductDiscount: dataItem.ProductDiscount,
                UnitValue: dataItem.UnitValue
            };

            //-select the row
            row.addClass("k-state-selected");
        } else {
            //-remove selection
            row.removeClass("k-state-selected");
            delete my.checkedIds[dataItem.id];
        }
    }

    my.appLocked = true;
    kendo.ui.progress($("#estimateAmount"), true);
    kendo.ui.progress($("#adminAmount"), true);
    var counter = 0;
    var setAmounts = function () {
        setTimeout(function () {
            counter = counter + 1;
            if (my.appLocked) {
                if (counter !== 20) {
                    setAmounts();
                }
            } else {
                my.setAmounts();
            }
        }, 1000);
    };

    setAmounts();

    addEstimateItems = function () {

        var addedProducts = [];

        $.each(my.vm.selectedProducts(), function (i, item) {
            addedProducts.push({
                //PortalId: portalID,
                //PersonId: my.personId,
                //EstimateId: my.estimateId,
                //Barcode: item.barcode(),
                //ProductRef: item.productRef(),
                ProductId: item.productId(),
                //ProductName: item.productName(),
                ProductQty: item.itemQty(),
                ProductEstimateOriginalPrice: item.unitValue(),
                ProductEstimatePrice: item.unitValue(),
                TotalAmount: item.totalValue(),
                //ProductImageId: item.productImageId(),
                //Extension: item.extension(),
                //Summary: item.summary(),
                //CategoriesNames: item.categoriesNames(),
                //QtyStockSet: item.qTyStockSet(),
                //Finan_Rep: item.finan_Rep(),
                //Finan_SalesPerson: item.finan_SalesPerson(),
                //Finan_Tech: item.finan_Tech(),
                //Finan_Telemarketing: item.finan_Telemarketing(),
                //Finan_Manager: item.finan_Manager(),
                //Finan_Cost: item.finan_Cost(),
                CreatedByUser: userID,
                CreatedOnDate: moment().format()
            });
        });

        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/estimates/addEstimateItem',
            data: JSON.stringify({
                EstimateItems: addedProducts,
                PortalId: portalID,
                EstimateId: my.estimateId,
                PersonId: my.personId,
                UserId: -1,
                //PersonId: my.vm.personId() > 0 ? my.vm.personId() : defaultClient,
                //Guid: my.Right(my.generateUUID(), 8),
                //SalesRep: kendo.parseInt(authorized === 2 ? userID : parseInt(amplify.store.sessionStorage('salesPerson'))),
                //ViewPrice: viewPrice,
                TotalAmount: my.vm.productNetSale(),
                ModifiedByUser: userID,
                ModifiedOnDate: moment().format(),
                ConnId: my.hub.connection.id
            })
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                //$().toastmessage('showSuccessToast', 'Item "<strong>' + $('#productSearch').select2('data').ProductName + '</strong>"<br />inserido com sucesso!');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Item "<strong>' + $('#productSearch').select2('data').ProductName + '</strong>"<br />inserido.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });

                var grid = $('#estimateItemsGrid').data('kendoGrid');

                $.each(data.Estimate.EstimateItems, function (i, estimateItem) {
                    my.estimateItemsData.add({
                        EstimateItemId: estimateItem.EstimateId,
                        ProductId: estimateItem.ProductId,
                        ProductImageId: estimateItem.ProductImageId,
                        Extension: estimateItem.Extension,
                        Summary: estimateItem.Summary,
                        CategoriesNames: estimateItem.CategoriesNames,
                        Barcode: estimateItem.Barcode,
                        ProductRef: estimateItem.ProductRef,
                        ProductName: estimateItem.ProductName,
                        ProductQty: estimateItem.ProductQty,
                        ProductEstimateOriginalPrice: estimateItem.ProductEstimateOriginalPrice,
                        ProductEstimatePrice: estimateItem.ProductEstimatePrice,
                        ProductDiscount: 0,
                        QtyStockSet: estimateItem.QtyStockSet,
                        Finan_Rep: estimateItem.Finan_Rep,
                        Finan_SalesPerson: estimateItem.Finan_SalesPerson,
                        Finan_Tech: estimateItem.Finan_Tech,
                        Finan_Telemarketing: estimateItem.Finan_Telemarketing,
                        Finan_Manager: estimateItem.Finan_Manager,
                        Finan_Cost: estimateItem.Finan_Cost,
                        ExtendedAmount: estimateItem.TotalAmount
                    });
                });

                my.setAmounts();

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
            kendo.ui.progress($("#estimateAmount"), false);
            kendo.ui.progress($("#adminAmount"), false);
            $('#productSearch').select2('val', '');
            $('#qTyTextBox').data('kendoNumericTextBox').value(null);
        });
    };

    $('#btnAddSelectedProduct').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        kendo.ui.progress($("#estimateAmount"), true);
        kendo.ui.progress($("#adminAmount"), true);

        if ($('#productSearch').select2('data').ProductsRelatedCount > 0) {

            // get related products
            $.ajax({
                url: '/desktopmodules/riw/api/products/getProductsRelated?portalId=' + portalID + '&productId=' + $('#productSearch').select2('data').ProductId + '&lang=pt-BR&relatedType=2&getAll=true'
            }).done(function (data) {
                if (data) {
                    $.each(data, function (index, item) {
                        var itemQty = kendo.parseFloat($('#qTyTextBox').data('kendoNumericTextBox').value());

                        my.vm.selectedProducts.push(new my.Product()
                            .barcode(item.RelatedBarcode.length > 0 ? item.RelatedBarcode : '')
                            .productRef(item.RelatedProductRef.length > 0 ? item.RelatedProductRef : '')
                            .productId(item.RelatedProductId)
                            .productName(item.RelatedProductName)
                            .itemQty(itemQty > 0 ? (itemQty * item.ProductQty) : item.ProductQty)
                            .unitValue(item.RelatedUnitValue)
                            .totalValue((itemQty * item.ProductQty) * item.RelatedUnitValue)
                            .productImageId(item.RelatedProductImageId)
                            .extension(item.RelatedExtension)
                            .summary(item.RelatedSummary)
                            .categoriesNames(item.RelatedCategoriesNames)
                            .qTyStockSet(item.RelatedQtyStockSet)
                            .finan_Rep(item.RelatedFinan_Rep)
                            .finan_SalesPerson(item.RelatedFinan_SalesPerson)
                            .finan_Tech(item.RelatedFinan_Tech)
                            .finan_Telemarketing(item.RelatedFinan_Telemarketing)
                            .finan_Manager(item.RelatedFinan_Manager)
                            .finan_Cost(item.RelatedFinan_Cost)
                            .createdByUser(userID)
                            .createdOnDate(moment().format()));
                    });

                    addEstimateItems();
                    $this.button('reset');
                }
            });
        } else {
            my.vm.selectedProducts.push(new my.Product()
                .barcode($('#productSearch').select2('data').Barcode.length > 0 ? $('#productSearch').select2('data').Barcode : '')
                .productRef($('#productSearch').select2('data').ProductRef.length > 0 ? $('#productSearch').select2('data').ProductRef : '')
                .productId($('#productSearch').select2('data').ProductId)
                .productName($('#productSearch').select2('data').ProductName)
                .itemQty(kendo.parseFloat($('#qTyTextBox').data('kendoNumericTextBox').value()))
                .unitValue($('#productSearch').select2('data').UnitValue)
                .totalValue(kendo.parseFloat($('#qTyTextBox').data('kendoNumericTextBox').value() * $('#productSearch').select2('data').UnitValue))
                .productImageId($('#productSearch').select2('data').ProductImageId)
                .extension($('#productSearch').select2('data').Extension)
                .summary($('#productSearch').select2('data').Summary)
                .categoriesNames($('#productSearch').select2('data').CategoriesNames)
                .qtyStockSet($('#productSearch').select2('data').QtyStockSet)
                .finan_Rep($('#productSearch').select2('data').Finan_Rep)
                .finan_SalesPerson($('#productSearch').select2('data').Finan_SalesPerson)
                .finan_Tech($('#productSearch').select2('data').Finan_Tech)
                .finan_Telemarketing($('#productSearch').select2('data').Finan_Telemarketing)
                .finan_Manager($('#productSearch').select2('data').Finan_Manager)
                .finan_Cost($('#productSearch').select2('data').Finan_Cost)
                .createdByUser(userID)
                .createdOnDate(moment().format()));

            addEstimateItems();
            $this.button('reset');
        }
    });

    my.vm.loadPayForms();

    my.updateEstimateItems = function (sender) {
        var $this = $(sender);
        $this.button('loading');
        //$(sender).html('<i class="fa fa-spinner fa-spin"></i> Um momento...').attr({ 'disabled': true });

        my.cancelAmounts();

        var grid = $('#estimateItemsGrid').data('kendoGrid');
        grid.refresh();

        my.setAmounts();

        var estimateItems = [];
        $.each(grid.dataSource.data(), function (i, item) {
            if (item.dirty) {
                estimateItems.push({
                    EstimateId: my.estimateId,
                    EstimateItemId: item.EstimateItemId,
                    ProductName: item.ProductName,
                    ProductQty: item.ProductQty,
                    ProductDiscount: item.ProductDiscount,
                    ProductEstimateOriginalPrice: item.ProductEstimateOriginalPrice,
                    ProductEstimatePrice: (item.ProductEstimateOriginalPrice - (item.ProductEstimateOriginalPrice / 100 * item.ProductDiscount)),
                    ModifiedByUser: userID,
                    ModifiedOnDate: moment().format()
                });
            }
        });

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/estimates/updateEstimateItems',
            data: JSON.stringify({
                EstimateItems: estimateItems,
                EstimateId: my.estimateId,
                EstimateTitle: $('#estimateTitleTextBox').val(),
                PortalId: portalID,
                Discount: $('#estimateDiscount').data('kendoNumericTextBox').value(),
                TotalAmount: my.vm.productNetSale(),
                ModifiedByUser: userID,
                ModifiedOnDate: moment().format(),
                ConnId: my.hub.connection.id
            })
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                //$().toastmessage('showSuccessToast', 'Item(ns) atualizados com sucesso!');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Item(ns) atualizados',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });

                my.vm.productNetSale(0);
                my.vm.conditionNPayments('');
                my.vm.conditionPayment(0);
                my.vm.conditionPayments(0);
                my.vm.conditionInterest(0);
                my.vm.conditionInterval(0);
                my.vm.conditionTotalPay(0);

                grid.dataSource.read();
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
            //$(sender).html('<i class="icon-ok icon-white"></i> Atualizar Or&#231;amento').attr({ 'disabled': false });
        });
    };

    my.syncEstimateItems = function (sender) {
        var $this = $(sender);

        if (!$.isEmptyObject(my.checkedIds)) {
            var $dialog = $('<div></div>')
                .html('<p class="confirmDialog">Tem certeza que deseja sincronizar pre&#231;o(s) do(s) produto(s) selecionado(s)? Esta a&#231;&#227;o n&#227;o pode ser revertida!</p>')
                .dialog({
                    autoOpen: false,
                    modal: true,
                    resizable: false,
                    dialogClass: 'dnnFormPopup',
                    open: function () {
                        $(".ui-dialog-title").append('Aviso de Exclus&#227;o');
                    },
                    buttons: {
                        'ok': {
                            text: 'Sim',
                            //priority: 'primary',
                            "class": 'btn btn-primary',
                            click: function () {
                                $this.button('loading');

                                //$(sender).html('<i class="fa fa-spinner fa-spin"></i> Um momento...').attr({ 'disabled': true });

                                var estimateItems = [];
                                $.each(my.checkedIds, function (i, item) {
                                    estimateItems.push({
                                        EstimateId: my.estimateId,
                                        EstimateItemId: kendo.parseInt(i),
                                        ProductName: item.ProductName,
                                        ProductQty: item.ProductQty,
                                        ProductEstimatePrice: item.UnitValue,
                                        ProductEstimateOriginalPrice: item.UnitValue,
                                        ProductDiscount: item.ProductDiscount,
                                        ModifiedByUser: userID,
                                        ModifiedOnDate: moment().format()
                                    });
                                });

                                $.ajax({
                                    type: 'PUT',
                                    contentType: 'application/json; charset=utf-8',
                                    url: '/desktopmodules/riw/api/estimates/SyncEstimateItems',
                                    data: JSON.stringify({
                                        EstimateItems: estimateItems,
                                        EstimateId: my.estimateId,
                                        PortalId: portalID,
                                        Discount: $('#estimateDiscount').data('kendoNumericTextBox').value(),
                                        TotalAmount: my.vm.productNetSale(),
                                        ModifiedByUser: userID,
                                        ModifiedOnDate: moment().format(),
                                        ConnId: my.hub.connection.id
                                    })
                                }).done(function (data) {
                                    if (data.Result.indexOf("success") !== -1) {
                                        //$().toastmessage('showSuccessToast', 'Item(ns) removidos com sucesso!');
                                        $.pnotify({
                                            title: 'Sucesso!',
                                            text: 'Item(ns) sincronizado(s).',
                                            type: 'success',
                                            icon: 'fa fa-check fa-lg',
                                            addclass: "stack-bottomright",
                                            stack: my.stack_bottomright
                                        });

                                        my.cancelAmounts();

                                        var grid = $('#estimateItemsGrid').data('kendoGrid');
                                        grid.dataSource.read();

                                        my.appLocked = true;
                                        setAmounts();
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
                                    //$(sender).html('<i class="icon-remove"></i> Remover Selecionado(s)').attr({ 'disabled': false });
                                    $dialog.dialog('close');
                                    $dialog.dialog('destroy');
                                });
                            }
                        },
                        'cancel': {
                            html: 'Cancelar',
                            //priority: 'secondary',
                            "class": 'btn',
                            click: function () {
                                $dialog.dialog('close');
                                $dialog.dialog('destroy');
                            }
                        }
                    }
                });

            $dialog.dialog('open');
        }
    };

    my.removeEstimateItem = function (sender) {
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
                                    click: function () {

                                        var $this = $(sender);
                                        $this.button('loading');
                                        //$(sender).html('<i class="fa fa-spinner fa-spin"></i> Um momento...').attr({ 'disabled': true });
                                        var newTotalAmount = 0;

                                        var estimateItems = [];
                                        $.each(my.checkedIds, function (i, item) {
                                            estimateItems.push({
                                                EstimateId: my.estimateId,
                                                EstimateItemId: kendo.parseInt(i),
                                                ProductEstimateOriginalPrice: item.ProductEstimateOriginalPrice,
                                                ProductEstimatePrice: item.ProductEstimatePrice,
                                                RemoveReasonId: $('#selectDeleteReason').data('kendoDropDownList').value(),
                                                ProductId: item.ProductId,
                                                ProductName: item.ProductName,
                                                ProductQty: item.ProductQty,
                                                CreatedByUser: userID,
                                                CreatedOnDate: moment().format(),
                                                ModifiedByUser: userID,
                                                ModifiedOnDate: moment().format()
                                            });
                                            newTotalAmount = newTotalAmount + item.UnitValue;
                                        });

                                        $.ajax({
                                            type: 'POST',
                                            contentType: 'application/json; charset=utf-8',
                                            url: '/desktopmodules/riw/api/estimates/removeEstimateItems',
                                            data: JSON.stringify({
                                                EstimateItemsRemoved: estimateItems,
                                                EstimateId: my.estimateId,
                                                PortalId: portalID,
                                                Discount: $('#estimateDiscount').data('kendoNumericTextBox').value(),
                                                TotalAmount: newTotalAmount,
                                                CreatedByUser: userID,
                                                CreatedOnDate: moment().format(),
                                                ModifiedByUser: userID,
                                                ModifiedOnDate: moment().format(),
                                                ConnId: my.hub.connection.id
                                            })
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                //$().toastmessage('showSuccessToast', 'Item(ns) removidos com sucesso!');

                                                my.cancelAmounts();

                                                var grid = $('#estimateItemsGrid').data('kendoGrid');
                                                grid.dataSource.read();

                                                my.appLocked = true;
                                                setAmounts();

                                                if ($('#selectDeleteReason').data('kendoDropDownList').value() === '3') {
                                                    $.ajax({
                                                        type: 'POST',
                                                        contentType: 'application/json; charset=utf-8',
                                                        url: '/desktopmodules/riw/api/estimates/addEstimateItem',
                                                        data: JSON.stringify({
                                                            EstimateItems: estimateItems,
                                                            PortalId: portalID,
                                                            EstimateId: 0,
                                                            EstimateTitle: '',
                                                            PersonId: my.personId,
                                                            UserId: my.vm.clientUserId(),
                                                            Guid: my.Right(my.generateUUID(), 8),
                                                            SalesRep: my.vm.salesPersonId(),
                                                            ViewPrice: my.vm.viewPrice(),
                                                            TotalAmount: newTotalAmount,
                                                            CreatedByUser: userID,
                                                            CreatedOnDate: moment().format(),
                                                            ModifiedByUser: userID,
                                                            ModifiedOnDate: moment().format()
                                                        })
                                                    }).done(function (data) {
                                                        if (data.Result.indexOf("success") !== -1) {
                                                            $.pnotify({
                                                                title: 'Sucesso!',
                                                                text: 'Novo or&#231;amento gerado!',
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

                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Item(ns) removidos.',
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
                                        }).always(function () {
                                            $this.button('reset');
                                            //$(sender).html('<i class="icon-remove"></i> Remover Selecionado(s)').attr({ 'disabled': false });
                                            $dialog.dialog('close');
                                            $dialog.dialog('destroy');
                                        });
                                    }
                                },
                                'cancel': {
                                    html: 'Cancelar',
                                    //priority: 'secondary',
                                    "class": 'btn',
                                    click: function () {
                                        $dialog.dialog('close');
                                        $dialog.dialog('destroy');
                                    }
                                }
                            }
                        });

        $dialog.dialog('open');
    };

    $('#selectDeleteReason').kendoDropDownList();

    $('#btnPrintPdf').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');
        //$(this).html('<i class="icon-spinner icon-spin"></i> Um momento...').attr({ 'disabled': true });

        var newWindow = window.open('', '_blank');

        var strSubHeader = 'Status: ' + my.vm.statusTitle() + ' - ' + moment(my.vm.createdOnDate()).format('L') + '[NEWLINE]';
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
            url: '/desktopmodules/riw/api/estimates/downloadEstimatePdf?customerLogo=' + amplify.store.sessionStorage('customerLogo') + '&pageHeader=Orcamento ID: ' + my.estimateId + '&pageSubHeader=' + strSubHeader + '&footerText=' + amplify.store.sessionStorage('footerText') + '&customerFooter=' + amplify.store.sessionStorage('my.vm.pdfFooter'),
            data: JSON.stringify({
                Columns: columns,
                EstimateId: my.estimateId,
                PortalId: portalID,
                Lang: 'pt-BR',
                ProductDiscountValue: my.vm.productDiscount(),
                ColumnsCount: 7,
                ProductOriginalAmount: my.vm.productGrossSale(),
                EstimateDiscountValue: $('#estimateDiscount').data('kendoNumericTextBox').value(),
                TotalDiscountPerc: my.vm.discountTotalPerc(),
                TotalDiscountValue: my.vm.discountTotalValue(),
                EstimateTotalAmount: my.vm.productNetSale(),
                ModifiedOnDate: moment().format(),
                Watermark: amplify.store.sessionStorage('watermark'),
                Expand: ($('#btnExpandCollapse').val() === '1' ? true : false),
                Conditions: $('#chkBoxPrintConds').is(':checked')
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

    $("#btnEmailEstimate").click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var kendoWindow = $('#divEmail').kendoWindow({
            actions: ["Maximize", "Close"],
            title: 'Enviando arquivo pdf via email',
            modal: true,
            width: '90%',
            height: '80%',
            close: function (e) {
                $("html, body").css("overflow", "");
                if (parent.$('#window').data('kendoWindow')) {
                    parent.$('#window').data('kendoWindow').toggleMaximization();
                }
            },
            open: function () {
                $("html, body").css("overflow", "hidden");
                if (parent.$('#window').data('kendoWindow')) {
                    parent.$('#window').data('kendoWindow').maximize();
                }
            }
        });

        kendoWindow.data("kendoWindow").center().open();
    });

    $('#btnEmail').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var strSubHeader = 'Status: ' + my.vm.statusTitle() + ' - ' + moment(my.vm.createdOnDate()).format('L') + '[NEWLINE]';
        strSubHeader += 'Vendedor: ' + my.vm.salesRepName() + '[NEWLINE]';
        if (my.salesRepPhone !== '') {
            strSubHeader += 'Telefone: ' + my.formatPhone(my.vm.salesRepPhone()) + '[NEWLINE]';
        }
        strSubHeader += 'Email: ' + my.vm.salesRepEmail();

        var columns = ['Cod.', 'Produto', 'Qde', 'Valor Original', 'Desconto', 'Valor Atual', 'Total'];

        var emailHtmlContent = my.converter.makeHtml($('#messageBody').val().trim());

        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/estimates/sendEstimatePdf?customerLogo=' + amplify.store.sessionStorage('customerLogo') + '&pageHeader=Orcamento ID: ' + my.estimateId + '&pageSubHeader=Email: ' + strSubHeader + '&footerText=' + amplify.store.sessionStorage('footerText') + '&customerFooter=' + amplify.store.sessionStorage('pdfFooter'),
            data: JSON.stringify({
                Columns: columns,
                EstimateId: my.estimateId,
                PortalId: portalID,
                Lang: 'pt-BR',
                ProductDiscountValue: my.vm.productDiscount(),
                ColumnsCount: 7,
                ProductOriginalAmount: my.vm.productGrossSale(),
                EstimateDiscountValue: $('#estimateDiscount').data('kendoNumericTextBox').value(),
                TotalDiscountPerc: my.vm.discountTotalPerc(),
                TotalDiscountValue: my.vm.discountTotalValue(),
                EstimateTotalAmount: my.vm.productNetSale(),
                ToEmail: $('#toEmailTextBox').val().trim(),
                Subject: $('#emailSubjectTextBox').val().trim(),
                MessageBody: emailHtmlContent,
                SalesPersonId: my.vm.salesPersonId(),
                PersonId: my.personId,
                Expand: $('#expandCheckbox').is(':checked'),
                ModifiedOnDate: moment().format()
            }),
            beforeSend: function () {
                var $this = $(this);
                $this.button('loading');
            }
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
                //    $.pnotify({
                //        title: 'Erro!',
                //        text: data.Result,
                //        type: 'error',
                //        icon: 'fa fa-times-circle fa-lg'
                //    });
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
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#divEmail').data("kendoWindow").close();
    });

    $('#btnNotifyClient').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        //$(this).html('<i class="icon-spinner icon-spin"></i> Um momento...').attr({ 'disabled': true });

        var subject = estimateNotifySubject.replace('[WEBSITELINK]', siteName).replace('[ID]', my.estimateId);

        var estimateLink = '[link](' + estimateURL + '?estimateId=' + my.estimateId + ')' // '[link] (' + estimateURL + '?eid=' + my.eId + ' "Clique para acessar o orçamento")';
        var body = estimateNotify.replace('[ORCAMENTOLINK]', estimateLink);
        body = body.replace('[CLIENTE]', my.vm.clientDisplayName());
        body = body.replace('[ID]', my.estimateId);

        var emailHtmlContent = my.converter.makeHtml(body.trim());

        var params = {
            PortalId: portalID,
            ToEmail: my.vm.clientEmail(),
            Subject: subject.trim(),
            MessageBody: emailHtmlContent,
            SalesPersonId: my.vm.salesPersonId(),
            PersonId: my.personId,
            UserId: -1,
            EstimateId: my.estimateId,
            EstimateLink: my.converter.makeHtml(estimateLink),
            ModifiedOnDate: moment().format(),
            ConnId: my.hub.connection.id,
            Watermark: amplify.store.sessionStorage('watermark')
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/estimates/sendClientEstimateNotification',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                //$().toastmessage('showSuccessToast', 'A notifica&#231;&#227;o foi transmitida com sucesso!');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'A notifica&#231;&#227;o foi transmitida.',
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
        }).always(function () {
            $this.button('reset');
        });
    });

    $('#btnGenerateSale').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var grid = $('#estimateItemsGrid').data('kendoGrid');

        var products = [];
        $.each(grid.dataSource.data(), function (i, item) {
            products.push({
                ProductId: item.ProductId,
                QtyStockSet: item.ProductQty,
                ModifiedByUser: userID,
                ModifiedOnDate: moment().format()
            });
        });

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/estimates/closeEstimate',
            data: JSON.stringify({
                Products: products,
                PortalId: portalID,
                EstimateId: my.estimateId,
                ModifiedByUser: userID,
                ModifiedOnDate: moment().format(),
                ConnId: my.hub.connection.id
            })
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {

                if (!amplify.store(siteURL + 'generateSaleMsg')) {
                    var $dialog = $('<div></div>')
                                    .html('<p class="confirmDialog">Or&#231;amento "Fechado". Redirecione ao caixa para recebimento e finaliza&#231;&#227;o.</p><label class="checkbox"><input type="checkbox" />N&#227;o mostrar mais esta mensagem.</label>')
                                    .dialog({
                                        autoOpen: false,
                                        modal: true,
                                        resizable: false,
                                        dialogClass: 'dnnFormPopup',
                                        open: function () {
                                            $(".ui-dialog-title").append('Aten&#231;&#227;o');
                                        },
                                        buttons: {
                                            'ok': {
                                                text: 'Continuar',
                                                //priority: 'primary',
                                                "class": 'btn btn-primary',
                                                click: function () {

                                                    $dialog.dialog('close');
                                                    $dialog.dialog('destroy');

                                                    if (parent.$('#window')) {
                                                        parent.$('#window').data("kendoWindow").close();
                                                    }

                                                }
                                            }
                                        }
                                    });

                    //$dialog.bind('dialogclose', function (event) {
                    //    if (parent.$('#window')) {
                    //        parent.$('#window').data("kendoWindow").close();
                    //    }
                    //});

                    $dialog.dialog('open');
                } else {
                    //if (parent.$('#window')) {
                    //    parent.$('#window').data("kendoWindow").close();
                    //}

                    //$().toastmessage('showSuccessToast', 'Configuração atualizada com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Configura&#231;&#227;o atualizada.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });

                }

                $('#generateSaleMsg input[type="checkbox"]').click(function (e) {
                    //if (my.storage) amplify.store.sessionStorage(siteURL + 'expandEstimate', true);
                    if (my.storage) amplify.store(siteURL + 'generateSaleMsg', true);
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

    //$('#btnDeleteEstimate').click(function (e) {
    //    if (e.clientX === 0) {
    //        return false;
    //    }
    //    e.preventDefault();

    //    var $this = $(this);

    //    var $dialog = $('<div></div>')
    //                    .html('<div class="confirmDialog">Tem Certeza?</div>')
    //                    .dialog({
    //                        autoOpen: false,
    //                        modal: true,
    //                        resizable: false,
    //                        dialogClass: 'dnnFormPopup',
    //                        title: 'Aviso',
    //                        buttons: {
    //                            'ok': {
    //                                text: 'Sim',
    //                                //priority: 'primary',
    //                                "class": 'btn btn-primary',
    //                                click: function () {
    //                                    $this.button('loading');

    //                                    $dialog.dialog('close');
    //                                    $dialog.dialog('destroy');

    //                                    var $this = $(this);
    //                                    $this.button('loading');

    //                                    var params = {
    //                                        PortalId: portalID,
    //                                        EstimateId: my.estimateId,
    //                                        IsDeleted: true,
    //                                        ModifiedByUser: userID,
    //                                        ModifiedOnDate: moment().format()
    //                                    };

    //                                    $.ajax({
    //                                        type: 'PUT',
    //                                        url: '/desktopmodules/riw/api/estimates/deleteEstimate',
    //                                        data: params
    //                                    }).done(function (data) {
    //                                        if (data.Result.indexOf("success") !== -1) {
    //                                            //$().toastmessage('showSuccessToast', 'O or&#231;amento foi inativado com sucesso');
    //                                            $.pnotify({
    //                                                title: 'Sucesso!',
    //                                                text: 'O or&#231;amento foi inativado.',
    //                                                type: 'success',
    //                                                icon: 'fa fa-check fa-lg',
    //                                                addclass: "stack-bottomright",
    //                                                stack: my.stack_bottomright
    //                                            });
    //                                            setTimeout(function () {
    //                                                if (parent.$('#window').data('kendoWindow')) {
    //                                                    parent.$('#window').data('kendoWindow').close();
    //                                                }
    //                                            }, 2000);
    //                                        } else {
    //                                            $.pnotify({
    //                                                title: 'Erro!',
    //                                                text: data.Result,
    //                                                type: 'error',
    //                                                icon: 'fa fa-times-circle fa-lg',
    //                                                addclass: "stack-bottomright",
    //                                                stack: my.stack_bottomright
    //                                            });
    //                                            //$().toastmessage('showErrorToast', data.Result);
    //                                        }
    //                                    }).fail(function (jqXHR, textStatus) {
    //                                        console.log(jqXHR.responseText);
    //                                    }).always(function () {
    //                                        $this.button('reset');
    //                                    });
    //                                }
    //                            },
    //                            'cancel': {
    //                                html: 'N&#227;o',
    //                                //priority: 'secondary',
    //                                "class": 'dnnSecondaryAction',
    //                                click: function () {
    //                                    $dialog.dialog('close');
    //                                    $dialog.dialog('destroy');
    //                                }
    //                            }
    //                        }
    //                    });

    //    $dialog.dialog('open');
    //});

    $('#btnAddMessage').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var messageHtmlContent = my.converter.makeHtml($('#estimateMessageTextarea').val().trim());

        var params = {
            PortalId: portalID,
            EstimateId: my.estimateId,
            MessageText: messageHtmlContent,
            Allowed: $('#managerOnlyCheckbox').is(':checked'),
            CreatedByUser: userID,
            CreatedOnDate: moment().format(),
            ConnId: my.hub.connection.id
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/estimates/updateMessage',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $('#estimateMessageTextarea').val('');
                //$().toastmessage('showSuccessToast', 'Texto adicionado ao hist&#243;rico com successo!');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Texto adicionado ao hist&#243;rico.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //my.vm.estimateMessages.unshift(new my.EstimateMessage(data.EstimateMessage));
                params.Avatar = avatar; // ? '/portals/0/' + avatar + '?w=45&h=45&mode=crop' : '/desktopmodules/rildoinfo/webapi/content/images/user.png?w=45&h=45';
                params.DisplayName = displayName;
                my.vm.estimateMessages.unshift(new my.EstimateMessage(params));
                //    .messageByAvatar = avatar ? '/portals/0/' + avatar + '?w=45&h=45&mode=crop' : '/desktopmodules/rildoinfo/webapi/content/images/user.png?w=45&h=45'
                //    .messageByName = displayName
                //    .messageId = data.EstimateMessageId
                //    .messageText = params.MessageText
                //    .createdOnDate = my.getTimeAgo(new Date())
                //    .allowed = $('#managerOnlyCheckbox').is(':checked'));
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

    $('#btnAddHistory').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var historyHtmlContent = my.converter.makeHtml($('#estimateHistoryTextarea').val().trim());

        var params = {
            PortalId: portalID,
            EstimateId: my.estimateId,
            HistoryText: historyHtmlContent,
            Locked: true,
            CreatedByUser: userID,
            CreatedOnDate: moment().format(),
            ConnId: my.hub.connection.id
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/estimates/addHistory',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $('#estimateHistoryTextarea').val('');
                //$().toastmessage('showSuccessToast', 'Texto adicionado ao hist&#243;rico com successo!');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Texto adicionado ao hist&#243;rico.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                params.Avatar = avatar; // ? '/portals/0/' + avatar + '?w=45&h=45&mode=crop' : '/desktopmodules/rildoinfo/webapi/content/images/user.png?w=45&h=45';
                params.DisplayName = displayName;
                my.vm.estimateHistories.unshift(new my.EstimateHistory(params));
                //.historyByAvatar(avatar ? '/portals/0/' + avatar + '?w=45&h=45&mode=crop' : '/desktopmodules/rildoinfo/webapi/content/images/user.png?w=45&h=45')
                //.historyByName(displayName)
                //.historyId(data.EstimateHistoryId)
                //.historyText(params.HistoryText)
                //.createdOnDate(moment())
                //.locked(false));
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

    my.startPayCondGrid = function (pfId) {
        $("#payCondGrid").kendoGrid({
            height: 150,
            columns: [
                {
                    command: [
                      {
                          name: "select",
                          text: " ",
                          imageClass: "fa fa-check",
                          click: function (e) {
                              e.preventDefault();
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
                                  my.vm.conditionPayInDays(dataItem.PayInDays);
                                  my.vm.payInMin(dataItem.PayIn);
                                  my.vm.conditionNumberPayments(dataItem.PayCondN);
                                  my.vm.conditionInterest(dataItem.PayCondPerc);
                                  my.vm.conditionInterval(dataItem.Intervalo);
                                  $('#interval').text(dataItem.Intervalo);
                              }

                              var payConds = {
                                  PortalId: portalID,
                                  EstimateId: my.estimateId,
                                  PayCondType: my.vm.selectedPayForm(),
                                  PayCondN: my.vm.conditionNPayments().replace(/[^\.\d]/g, ""),
                                  PayCondPerc: my.vm.conditionInterest(),
                                  PayCondIn: my.vm.conditionPayIn(),
                                  PayInDays: my.vm.conditionPayInDays(),
                                  PayCondInst: parseFloat(conditionPayment()),
                                  PayCondInterval: my.vm.conditionInterval() === 'A Vista' ? -1 : my.vm.conditionInterval() === 'Mensal' ? 0 : my.vm.conditionInterval(),
                                  TotalPayments: my.vm.conditionPayments(),
                                  TotalPayCond: my.vm.conditionTotalPay(),
                                  ModifiedByUser: userID,
                                  ModifiedOnDate: moment().format(),
                                  ConnId: my.hub.connection.id
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
                    width: 45
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
            //change: function () {
            //    var row = this.select();
            //    var id = row.data("uid");
            //    myUid = id;
            //},
            selectable: true,
            navigatable: true,
            dataSource: {
                type: 'json',
                transport: {
                    read: "/desktopmodules/riw/api/payconditions/getPayConds?portalId=" + portalID + "&pcType=" + pfId + "&pcStart=" + my.vm.productNetSale(),
                    cache: false
                }
            },
            resizable: true,
            dataBound: function () {
                var grid = this;
                if (this.dataSource.view().length !== 0) {
                    $('#payCondMsg').html('');
                    $('#divPayFormCond').show();
                    //$('#payCondMsg').html('');

                    my.vm.payFormTitle($('#ddlPayForms').data('kendoDropDownList').text());
                    $.each(grid.dataSource.data(), function (i, item) {
                        item.set('TotalPayCond', (item.TotalParcelado + item.PayIn));
                    });
                }
                else {
                    $('#payCondMsg').html('<span class="NormalRed">N&#227;o h&#225; condi&#231;&#245;es configuradas para forma de pagamento escolhida!</span>');
                }
            },
        });
    };

    $('#btnCancelPayFormCond').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        my.vm.selectedPayFormId(null);
        $('#divPayFormCond').fadeOut();
        $('#divChosenPayCond').fadeOut();
        $('#divPayIn').fadeOut();
        $('#btnSavePayFormCond').fadeOut();
        $('#divPayForms').fadeIn();
        //$('#divPayFormCond').fadeIn();
        //$('#divPayFormCond').kendoAnimate({ effects: 'slide:up fade:out', hide: true });
        //$('#divChosenPayCond').kendoAnimate({ effects: 'slide:up fade:out', hide: true });
        //$('#divPayForms').kendoAnimate({ effects: 'slide:down fade:in', show: true });
        //$('#divPayFormCond').kendoAnimate({ effects: 'slide:down fade:in', show: true });

        var payConds = {
            PortalId: portalID,
            EstimateId: my.estimateId,
            PayCondType: null,
            PayCondN: null,
            PayCondPerc: null,
            PayCondIn: null,
            PayCondInst: null,
            PayCondInterval: null,
            TotalPayments: null,
            TotalPayCond: null,
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format(),
            ConnId: my.hub.connection.id
        };

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
            PayCondInst: parseFloat(conditionPayment()),
            PayCondInterval: my.vm.conditionInterval() === 'A Vista' ? -1 : my.vm.conditionInterval() === 'Mensal' ? 0 : my.vm.conditionInterval(),
            TotalPayments: my.vm.conditionPayments(),
            TotalPayCond: my.vm.conditionTotalPay(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format(),
            ConnId: my.hub.connection.id
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

    $('#btnUpdateTerm').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        //var converter = new Markdown.getSanitizingConverter();
        //var termHtmlContent = converter.makeHtml($('#estimateTermTextarea').val().trim());

        var params = {
            PortalId: portalID,
            EstimateId: my.estimateId,
            Inst: $('#estimateTermTextarea').val().trim(), // termHtmlContent,
            ModifieByUser: userID,
            ModifiedOnDate: moment().format()
        };

        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/estimates/updateEstimateTerm',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                //$().toastmessage('showSuccessToast', 'Termo do or&#231;mento atualizado com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Termo do or&#231;mento atualizado.',
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
        }).always(function () {
            $this.button('reset');
        });
    });

    $('#btnUpdateConfig').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            PortalId: portalID,
            EstimateId: my.estimateId,
            SalesRep: $('#kddlSalesGroup').data('kendoDropDownList').value(),
            //Discount: $('#estimateDiscount').data('kendoNumericTextBox').value(),
            StatusId: $('#kddlStatuses').data('kendoDropDownList').value(),
            ViewPrice: $('#viewPriceCheckBox').is(':checked'),
            Locked: $('#lockedCheckBox').is(':checked'),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };

        my.vm.estimateLocked($('#lockedCheckBox').is(':checked'));

        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/estimates/updateEstimateConfig',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                //$().toastmessage('showSuccessToast', 'Configuração atualizada com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configura&#231;&#227;o atualizada.',
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
        }).always(function () {
            $this.button('reset');
        });
    });

    $('#btnReloadEstimate').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        my.loadHistory();

        setAmounts();
    });

    $('#btnClientReplace').click(function (e) {
        e.preventDefault();
        $(this).hide();
        $('#spanClientInfo').fadeOut();
        $('#divClientSearch').delay(200).fadeIn();
        $('#btnCancelClientReplace').delay(200).fadeIn();
        setTimeout(function () {
            $('#clientSearch').select2("open");
        }, 500);
    });

    $('#btnCancelClientReplace').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#btnClientReplace').fadeIn();
        $('#divClientSearch').fadeOut();
        $(this).fadeOut();
        $('#spanClientInfo').delay(200).fadeIn()
    });

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
        $('#spanClientInfo').html('<div class="pull-left"><ul>'
                    + '<li><strong>Cliente:</strong> ' + e.val.DisplayName + '</li>'
                    + (e.val.Email ? ('<li><span><strong>Email:</strong></span> ' + e.val.Email + '</li>') : '')
                    + (e.val.Telephone ? ('<li><span><strong>Telefone:</strong></span> ' + my.formatPhone(e.val.Telephone) + '</li>') : '')
                    + (e.val.Cell ? ('<li><span><strong>Celular:</strong></span> ' + my.formatPhone(e.val.Cell) + '</li>') : '')
                    + (e.val.Fax ? ('<li><span><strong>Fax:</strong></span> ' + my.formatPhone(e.val.Fax) + '</li>') : '')
                    + (e.val.Zero800s ? ('<li><span><strong>0800:</strong></span> ' + my.formatPhone(e.val.Zero800s) + '</li></ul></div>') : ''));
        //if (e.val.Street) {
        //    $('#spanClientInfo').html($('#spanClientInfo').html() + '<div class="span6"><address>'
        //    + (e.val.Street + ' N&#186; ' + e.val.Unit + '<br />')
        //    + (e.val.Complement ? (e.val.Complement + '<br />') : '')
        //    + (e.val.District ? ('<strong>Bairro:</strong>' + e.val.District + '<br />') : '')
        //    + (e.val.City ? (e.val.City + ', ') : '')
        //    + (e.val.Region ? (e.val.Region + (e.val.PostalCode ? (' <strong>CEP:</strong> ' + my.formatPostalcode(e.val.PostalCode)) : '') + '<br />') : '')
        //    + (e.val.Country ? (e.val.Country + '</address></div>') : ''));
        //}
        $('#spanClientInfo').html($('#spanClientInfo').html() + '<div class="clearfix"></div>');
        $('#btnClientReplace').fadeIn();
        $('#divClientSearch').fadeOut();
        $('#btnCancelClientReplace').fadeOut();
        $('#spanClientInfo').delay(200).fadeIn();

        var params = {
            EstimateId: my.estimateId,
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
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Cliente atualizado.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                parent.$('#window').data('kendoWindow').title('Or&#231;amento: (ID: ' + my.estimateId + ') ' + e.val.DisplayName + '(ID: ' + e.val.PersonId + ')')
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

    });

    if (my.new > 0 && my.viewPrice() === false) {
        var $dialog = $('#dialog-message')
            .dialog({
                autoOpen: false,
                modal: true,
                resizable: false,
                dialogClass: 'dnnFormPopup',
                title: 'Mensagem',
                width: 360,
                buttons: {
                    'ok': {
                        text: 'Ok',
                        //priority: 'primary',
                        "class": 'btn btn-primary',
                        click: function (e) {
                            // on ok, close dialog
                            $dialog.dialog('close');
                            $dialog.dialog('destroy');
                        }
                    }
                }
            });

        $dialog.dialog('open');
    }
    parent.history.pushState("", parent.document.title, parent.window.location.pathname);

    $('#estimateTitleLabel').click(function () {
        if ((!estimateLocked()) || (authorized !== 1)) {
            $('#estimateTitleTextBox').val($(this).text());
            $(this).hide();
            $('#divEstimateTitle').show();
            $('#btnUpdateCancelTitle').show();
        }
    });

    $('#btnUpdateCancelTitle').click(function () {
        $('#divEstimateTitle').hide();
        $('#estimateTitleLabel').show();
    });

    $('#btnUpdateTitle').click(function () {
        var params = {
            PortalId: portalID,
            EstimateId: my.estimateId,
            EstimateTitle: $('#estimateTitleTextBox').val(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format(),
            ConnId: my.hub.connection.id
        };

        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/estimates/updateTitle',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'T&#237;tulo do or&#231;amento atualizado.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });

                $('#divEstimateTitle').hide();
                $('#btnUpdateCancelTitle').hide();
                $('#estimateTitleLabel').text($('#estimateTitleTextBox').val());
                $('#estimateTitleLabel').show();
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

        return false;
    });

    $('.markdown-editor').css({ 'min-width': '90%', 'height': '80px', 'margin-bottom': '5px' }).attr({ 'cols': '30', 'rows': '2' });

    //$('.msgHolder').css({ 'max-height': kendo.parseInt($('#estimateTabs-1').height() / 1.5) });

    $('.markdown-editor').autogrow();
    $('.markdown-editor').css('overflow', 'hidden').autogrow();

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

    $('#btnUpdateEmail').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = [
            {
                'id': tModuleID,
                'name': 'RIW_EstimateEmailSubject',
                'value': $('#preEmailSubjectTextBox').val()
            },
            {
                'id': tModuleID,
                'name': 'RIW_EstimateEmailBody',
                'value': $('#preEmailMessageBody').val()
            },
            {
                'id': tModuleID,
                'name': 'RIW_EstimateNotifySubject',
                'value': $('#notifySubjectTextBox').val()
            },
            {
                'id': tModuleID,
                'name': 'RIW_EstimateEmailNotify',
                'value': $('#notifyMessageBody').val()
            }
        ];

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateTabModuleSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configura&#231;&#245;es atualizadas.',
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
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
    });

    $('.btnReturn').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        if (parent.$('#window')) {
            parent.$('#window').data("kendoWindow").close();
        }
    });

    function ToggleAllKendoGridDetailRows(direction) {
        //Get a collection of all rows in the grid
        var grid = $('#estimateItemsGrid').data('kendoGrid');
        var allMasterRows = grid.tbody.find('>tr.k-master-row');

        //Loop through each row and collapse or expand the row depending on set deriction
        if (direction === 'collapse') {
            $(".toggleDetail").attr("onclick", "ToggleAllKendoGridDetailRows('expand')");
            $(".toggleDetail").text("Expand all rows");
            for (var i = 0; i < allMasterRows.length; i++) {
                grid.collapseRow(allMasterRows.eq(i));
            }
        } else if (direction === 'expand') {
            $(".toggleDetail").attr("onclick", "ToggleAllKendoGridDetailRows('collapse')");
            $(".toggleDetail").text("Collapse all rows");
            for (var j = 0; j < allMasterRows.length; j++) {
                grid.expandRow(allMasterRows.eq(j));
            }
        }
    }

    $('#btnExpandCollapse').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        if (this.value === '1') {
            ToggleAllKendoGridDetailRows('collapse');
            $('#btnExpandCollapse').html('<i class="fa fa-plus-square"></i>&nbsp; Expandir Or&#231;amento');
            this.value = '0';
        } else {
            ToggleAllKendoGridDetailRows('expand');
            $('#btnExpandCollapse').html('<i class="fa fa-minus-square"></i>&nbsp; Retrair Or&#231;amento');
            this.value = '1';
        }
    });

    $('#clickExtraInfo').click(function () {
        if ($('#divExtraInfo').is(':hidden')) {
            $('#divExtraInfo').slideDown();
            $('#divExtraInfo').show();
            $('#clickExtraInfo').html('Menos Informa&#231;&#227;o');
        } else {
            $('#divExtraInfo').slideUp();
            $('#clickExtraInfo').html('Mais Informa&#231;&#227;o');
        }
    })

});

my.cancelAmounts = function () {
    //my.vm.selectedPayForm('');
    //my.vm.conditionPayIn(0);
    //my.vm.payInMin(0);
    //my.vm.conditionNumberPayments(0);
    //my.vm.conditionInterest(0);
    $('#ddEstimateDiscountValue').text('');
    $('#ddOriginalAmount').text('');
    $('#ddProductDiscountValue').text('');
    $('#ddTotalDiscountPerc').text('');
    $('#ddTotalDiscountValue').text('');
    $('#ddTotal').text('');
    $('#ddMarkupAmount').text('');
    $('#ddMarkupPerc').text('');
    $('#ddProfitAmount').text('');
    $('#ddCommsAmount').text('');
    $('#ddCommsPerc').text('');
    $('#ddProfitPerc').text('');
    my.vm.chosenPayCond(0);
    my.vm.productNetSale(0);
    my.vm.productGrossSale(0);
    my.vm.productDiscount(0);
    my.vm.productTotalPaid(0);
    my.vm.paidComission(0);
};

function productFormatResult(data) {
    var markup = '<table class="product-result Normal"><tr>';
    if (data.ProductImageId > 0) {
        markup += '<td class="product-image"><img src="/databaseimages/' + data.ProductImageId + '.' + data.Extension + '?maxwidth=60&maxheight=60&s.roundcorners=10" /></td>';
    } else {
        markup += '<td class="product-image"><img class="img-rounded" src="/portals/0/images/No-Image.jpg?maxwidth=60&maxheight=60&s.roundcorners=10" /></td>';
    }
    markup += "<td class='product-info'><div class='product-title'>" + data.ProductName + "</div>";
    if (data.Barcode) {
        markup += "<div><strong>CB: </strong>" + data.Barcode + "</div>";
    } else if (data.ProductRef) {
        markup += "<div><strong>REF: </strong>" + data.ProductRef + "</div>";
    }
    markup += "</td><td class='product-price'> " + kendo.toString(data.UnitValue, 'c');
    markup += "</td></tr></table>"
    return markup;
    //var markup = '<table class="product-result"><tr>';
    //markup += '<td class="product-info"><div class="product-title">' + data.productName + '</div></td>';
    //if (data.productRef) {
    //    markup += '<td class="product-info"><div class="product-title">' + data.productRef + '</div></td>';
    //}
    //if (data.Barcode) {
    //    markup += '<td class="product-info"><div class="product-title">' + data.Barcode + '</div></td>';
    //}
    //markup += "</tr></table>"
    //return markup;
}

function productFormatSelection(data) {
    return data.ProductName;
}

function clientFormatResult(data) {
    return '<strong>Cliente: </strong><span>' + data.DisplayName + '</span><br /><strong>Email: </strong><span>' + data.Email + '</span><br /><strong>Telefone: </strong><span>' + data.Telephone + '</span>';
}

function clientFormatSelection(data) {
    return data.DisplayName;
}

function editNumber(container, options) {
    $('<input data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoNumericTextBox({
            spinners: false,
            max: function () {
                if (options.field === 'ProductDiscount') {
                    if (authorized === 2) {
                        return estimateMaxDiscount
                    }
                }
            },
            min: 0
        });
}