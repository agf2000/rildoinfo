
$(function () {

    my.personId = my.getParameterByName('personId');

    var status = $(".status");
        //clientFinancesValidator = $("#clientFinances").kendoValidator().data("kendoValidator");

    my.viewModel();

    my.hub = $.connection.peopleHub;

    my.hub.client.pushHistoryComment = function (item, index) {
        my.vm.filteredHistories()[index].historyComments.unshift(new my.HistoryComment(item));
        my.sendNotification('/desktopmodules/rildoinfo/webapi/content/images/ri-logo.png', siteName, 'O website ' + siteURL + ' foi atualizado.', 6000, !my.hasFocus);
    };

    my.hub.client.pushHistory = function (item) {
        my.vm.personHistories.unshift(new my.PersonHistory(item));
        my.sendNotification('/desktopmodules/rildoinfo/webapi/content/images/ri-logo.png', siteName, 'O website ' + siteURL + ' foi atualizado.', 6000, !my.hasFocus);
    };

    $.connection.hub.start().done(function () {
        my.hub.server.clientsJoin(portalID.toString() + '_' + my.personId.toString());
    });

    $("#actionsMenu").jqxMenu();

    $('#clientFinanceTabs').kendoTabStrip({
        animation: {
            // fade-out current tab over 1000 milliseconds
            close: {
                effects: "fadeOut"
            },
            // fade-in new tab over 500 milliseconds
            open: {
                effects: "fadeIn"
            }
        }
    });

    my.getPerson = function () {
        if (my.personId !== 0) {
            $.ajax({
                url: '/desktopmodules/riw/api/people/GetPerson?personId=' + my.personId,
                async: false
            }).done(function (data) {
                if (data) {
                    $('#actionsMenu').show();

                    my.vm.personType(data.PersonType);
                    my.vm.personId(data.PersonId);
                    my.vm.personUserId(data.UserId);
                    my.vm.displayName(data.DisplayName);
                    my.vm.firstName(data.FirstName);
                    my.vm.lastName(data.LastName);
                    my.vm.cpf(data.Cpf);
                    my.vm.ident(data.Ident);
                    my.vm.telephone(data.Telephone);
                    my.vm.cell(data.Cell);
                    my.vm.fax(data.Fax);
                    my.vm.originalEmail(data.Email);
                    if (data.Email.length > 2) {
                        my.vm.email(data.Email);
                        $('#actionsMenu li:nth-child(5)').show();
                    }
                    my.vm.companyName(data.CompanyName);
                    my.vm.zero800(data.Zero800s);
                    my.vm.dateFound(data.DateFound);
                    my.vm.dateRegistered(data.DateRegistered);
                    my.vm.ein(data.Cnpj);
                    my.vm.stateTax(data.StateTax);
                    my.vm.cityTax(data.CityTax);
                    my.vm.comments(data.Comments);
                    my.vm.bio(data.Biography);
                    my.vm.locked(data.Locked);
                    if (data.MonthlyIncome > 0) my.vm.income(data.MonthlyIncome);
                    my.vm.selectedFinanAddress(data.PersonAddressId);
                    my.vm.website(data.Website);
                    my.vm.selectedSalesRepId(data.SalesRep);
                    my.vm.createdByUser(data.CreatedByUser);
                    my.vm.createdOnDate(data.CreatedOnDate);

                    //$('#moduleTitle').html('Cliente: ' + data.DisplayName + ' (ID: ' + data.ClientId + ')');

                    $.getJSON('/desktopmodules/riw/api/people/getHistory?personId=' + my.personId, function (data) {
                        if (data.length > 0) {
                            my.vm.personHistories.removeAll();
                            $.each(data, function (i, item) {
                                my.vm.personHistories.unshift(new my.PersonHistory(item));
                            });
                        }
                    });
                }
            });
        }
    };
    my.getPerson();

    if (my.vm.personType()) {
        $('#liPartners').hide();
        $('#liPartnerBanks').hide();
        $('#liIncomes').show();
        $('#liRefs').show();
        $('#liComRefs').show();
        $('#divBusinessName').hide();
        $('#divCPF2').show();
        $('#divIdent2').show();
        $('#divEIN2').hide();
        $('#divST2').hide();
        $('#divIM2').hide();
    } else {
        $('#liPartners').show();
        $('#liPartnerBanks').show();
        $('#liPersonalRefs').hide();
        $('#liIncomes').hide();
        $('#liRefs').hide();
        $('#liComRefs').hide();
        $('#divClientName').hide();
        $('#divBusinessName').show();
        $('#divEIN2').show();
        $('#divST2').show();
        $('#divIM2').show();
        $('#divCPF2').hide();
        $('#divIdent2').hide();
    }

    my.vm.loadAddresses();

    if (my.vm.selectedFinanAddress() > 0) {
        kendo.ui.progress($("#addressLiteral"), true);
        $.ajax({
            url: '/desktopmodules/riw/api/people/GetPersonAddress?personAddressId=' + my.vm.selectedFinanAddress() + '&personId=' + my.personId
        }).done(function (data) {
            if (data) {
                my.vm.selectedFinanAddress(data.PersonAddressId);
                var address = (data.Street ? data.Street : '') + (data.Unit ? ' N&#186;: ' + data.Unit : '') + (data.Complement ? ' ' + data.Complement : '') + (data.District ? '<br />' + data.District : '') + (data.City ? '<br />' + data.City : '') + (data.Region ? ' ' + data.Region : '') + (data.PostalCode ? '<br />CEP: ' + data.PostalCode : '') + (data.Telephone ? '<br />' + my.formatPhone(data.Telephone) : '');
                $('#addressLiteral').html(address);
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', data.Msg);
            }
            kendo.ui.progress($("#addressLiteral"), false);
        }).fail(function (jqXHR, textStatus) {
            kendo.ui.progress($("#addressLiteral"), false);
            console.log(jqXHR.responseText);
        });
    }

    $('#ddlFinanAddress').data('kendoDropDownList').bind('change', function (e) {
        e.preventDefault();
        $.ajax({
            url: '/desktopmodules/riw/api/people/GetPersonAddress?personAddressId=' + e.sender.value() + '&personId=' + my.personId
        }).done(function (data) {
            if (data) {
                my.vm.selectedFinanAddress(data.PersonAddressId);
                var address = (data.Street ? data.Street : '') + (data.Unit ? ' N&#186;: ' + data.Unit : '') + (data.Complement ? ' ' + data.Complement : '') + (data.District ? '<br />' + data.District : '') + (data.City ? '<br />' + data.City : '') + (data.Region ? ' ' + data.Region : '') + (data.PostalCode ? '<br />CEP: ' + data.PostalCode : '') + (data.Telephone ? '<br />' + my.formatPhone(data.Telephone) : '');
                $('#addressLiteral').html(address);
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', data.Msg);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
    });

    $('#btnUpdateClientFinan').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            PersonId: my.personId,
            PersonAddressId: my.vm.selectedFinanAddress(),
            MonthlyIncome: my.vm.income(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };

        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/people/updateClientFinance',
            data: params
        }).done(function (data) {

            if (data.Result.indexOf("success") !== -1) {
                //$().toastmessage('showSuccessToast', 'Cliente atualizado com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Cliente atualizado.',
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
                //$().toastmessage('showErrorToast', data.Msg);
            }

        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
    });

    my.clientIncomeSourcesTransport = {
        read: {
            url: '/desktopmodules/riw/api/people/GetClientIncomeSources?personId=' + my.personId
        }
    };

    my.clientIncomeSourcesData = new kendo.data.DataSource({
        transport: my.clientIncomeSourcesTransport,
        sort: { field: "ISName", dir: "ASC" },
        schema: {
            model: {
                id: 'clientISId',
                fields: {
                    //DateBirth: { field: "DateBirth", type: "date", format: "{0:m}" }
                }
            }
        }
    });

    my.createIncomeSourcesLV = function () {
        $("#lvIncomeSources").kendoListView({
            dataSource: my.clientIncomeSourcesData,
            template: kendo.template($("#tmplClientIncomeSources").html()),
            dataBound: function () {
                // if (this.dataSource.view().length === 0) $('#collapseIncomeSourceForm').collapse('toggle');
            }
        });
    };

    my.createIncomeSourcesLV();

    $('#iSIncomeNumericTextBox').kendoNumericTextBox({
        value: 1,
        min: 0,
        format: 'c'
    });

    $("#iSPhoneTextBox").inputmask("(99) 9999-9999");
    $("#iSFaxTextBox").inputmask("(99) 9999-9999");
    $("#iSPostalCodeTextBox").inputmask("99-999-999");
    $("#iSEINTextBox").inputmask("99.999.999/9999-99");

    my.editClientIncomeSource = function (value) {
        $.ajax({
            url: '/desktopmodules/riw/api/people/GetClientIncomeSource?clientIncomeSourceId=' + value + '&personId=' + my.personId
        }).done(function (data) {
            my.vm.clientISId(data.ClientIncomeSourceId);
            $('#iSNameTextBox').val(data.ISName);
            $('#iSEINTextBox').val(data.ISEIN);
            $('#iSInsEstTextBox').val(data.ISST);
            $('#iSInsMunTextBox').val(data.ISCT);
            $('#iSPhoneTextBox').val(data.ISPhone);
            $('#iSFaxTextBox').val(data.ISFax);
            $('#iSIncomeNumericTextBox').data('kendoNumericTextBox').value(data.ISIncome);
            $('#iSPostalCodeTextBox').val(data.ISPostalCode);
            $('#iSStreetTextBox').val(data.ISAddress);
            $('#iSUnitTextBox').val(data.ISAddressUnit);
            $('#iSComplementTextBox').val(data.ISComplement);
            $('#iSDistrictTextBox').val(data.ISDistrict);
            $('#iSCityTextBox').val(data.ISCity);
            my.vm.selectedRegion(data.Region);
            my.vm.selectedCountry('BR');
            $('#iSProofCheckbox').attr({ 'checked': data.ISProof });
            $("#iSPostalCodeTextBox").inputmask("99-999-999");
            $("#iSPhoneTextBox").inputmask("(99) 9999-9999");
            $("#iSFaxTextBox").inputmask("(99) 9999-9999");
            $('#btnUpdateIncomeSource').html('<i class="fa fa-check"></i>&nbsp; Atualizar Renda');
            $('#btnRemoveIncomeSource').show();
            $('#btnCopyIncomeSource').show();
            if ($('#collapseIncomeSourceForm.in').length === 0) {
                $('#collapseIncomeSourceForm').collapse('toggle');
            }
            $.scrollTo($('#collapseIncomeSourceForm'), 1000, { easing: 'swing' });
        });
    };

    $('#btnUpdateIncomeSource').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        //if (clientFinancesValidator.validate()) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            ClientIncomeSourceId: my.vm.clientISId(),
            PersonId: my.personId,
            ISName: $('#iSNameTextBox').val().trim(),
            ISEIN: $('#iSEINTextBox').val().replace(/\D/g, ''),
            ISST: $('#iSInsEstTextBox').val().trim(),
            ISCT: $('#iSInsMunTextBox').val().trim(),
            ISPhone: $('#iSPhoneTextBox').val().replace(/\D/g, ''),
            ISFax: $('#iSFaxTextBox').val().replace(/\D/g, ''),
            ISIncome: $('#iSIncomeNumericTextBox').data('kendoNumericTextBox').value(),
            ISPostalCode: $('#iSPostalCodeTextBox').val().replace(/\D/g, ''),
            ISAddress: $('#iSStreetTextBox').val().trim(),
            ISAddressUnit: $('#iSUnitTextBox').val().trim(),
            ISComplement: $('#iSComplementTextBox').val().trim(),
            ISDistrict: $('#iSDistrictTextBox').val().trim(),
            ISCity: $('#iSCityTextBox').val().trim(),
            ISRegion: my.vm.selectedRegion(),
            iSCountry: my.vm.selectedCountry(),
            ISProof: $('#iSProofCheckbox').is(':checked')
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/people/updateClientIncomeSource',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.clientIncomeSourcesData.read();
                $('#editIncomes input').val('');
                $('#iSProofCheckbox').attr({ 'checked': false });
                $('#iSIncomeNumericTextBox').data('kendoNumericTextBox').value(1);
                $('#btnRemoveIncomeSource').hide();
                $('#btnCopyIncomeSource').hide();

                if (my.vm.clientISId()) {
                    //$().toastmessage('showSuccessToast', 'Renda atualizada com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Renda atualizada.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                } else {
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Renda inserida.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    //$().toastmessage('showSuccessToast', 'Renda inserida com sucesso.');
                }

                my.vm.clientISId(null);
                $('#collapseIncomeSourceForm').collapse('toggle');
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', data.Msg);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
    });

    $('#btnCopyIncomeSource').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            ClientIncomeSourceId: 0,
            PersonId: my.personId,
            ISName: $('#iSNameTextBox').val().trim(),
            ISEIN: $('#iSEINTextBox').val().replace(/\D/g, ''),
            ISST: $('#iSInsEstTextBox').val().trim(),
            ISCT: $('#iSInsMunTextBox').val().trim(),
            ISPhone: $('#iSPhoneTextBox').val().replace(/\D/g, ''),
            ISFax: $('#iSFaxTextBox').val().replace(/\D/g, ''),
            ISIncome: $('#iSIncomeNumericTextBox').data('kendoNumericTextBox').value(),
            ISPostalCode: $('#iSPostalCodeTextBox').val().replace(/\D/g, ''),
            ISAddress: $('#iSStreetTextBox').val().trim(),
            ISAddressUnit: $('#iSUnitTextBox').val().trim(),
            ISComplement: $('#iSComplementTextBox').val().trim(),
            ISDistrict: $('#iSDistrictTextBox').val().trim(),
            ISCity: $('#iSCityTextBox').val().trim(),
            ISRegion: my.vm.selectedRegion(),
            iSCountry: my.vm.selectedCountry(),
            ISProof: $('#iSProofCheckbox').is(':checked')
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/people/updateClientIncomeSource',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.clientIncomeSourcesData.read();
                $('#editIncomes input').val('');
                $('#iSProofCheckbox').attr({ 'checked': false });
                $('#iSIncomeNumericTextBox').data('kendoNumericTextBox').value(1);
                $('#btnRemoveIncomeSource').hide();
                $('#btnCopyIncomeSource').hide();

                //$().toastmessage('showSuccessToast', 'Renda atualizada com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Renda duplicada.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });

                my.vm.clientISId(null);
                $('#collapseIncomeSourceForm').collapse('toggle');
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', data.Msg);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
    });

    $('#btnRemoveIncomeSource').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var $dialog = $('<div></div>')
                    .html('<div class="confirmDialog">Tem Certeza?</div>')
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
                                "class": 'dnnPrimaryAction',
                                click: function () {
                                    if (my.vm.clientISId() !== 0) {

                                        $this.button('loading');

                                        $.ajax({
                                            type: 'DELETE',
                                            url: '/desktopmodules/riw/api/people/RemoveClientIncomeSource?clientIncomeSourceId=' + my.vm.clientISId() + '&personId=' + my.personId
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                my.clientIncomeSourcesData.read();
                                                $('#editIncomes input').val('');
                                                $('#iSProofCheckbox').attr({ 'checked': false });
                                                $('#iSIncomeNumericTextBox').data('kendoNumericTextBox').value(1);
                                                $('#btnRemoveIncomeSource').hide();
                                                $('#btnCopyIncomeSource').hide();
                                                my.vm.selectedRegion('MG');
                                                my.vm.selectedCountry('BR');
                                                //$().toastmessage('showSuccessToast', 'Renda excluida com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Renda excluida.',
                                                    type: 'success',
                                                    icon: 'fa fa-check fa-lg',
                                                    addclass: "stack-bottomright",
                                                    stack: my.stack_bottomright
                                                });
                                                $('#collapseIncomeSourceForm').collapse('toggle');
                                            } else {
                                                $.pnotify({
                                                    title: 'Erro!',
                                                    text: data.Result,
                                                    type: 'error',
                                                    icon: 'fa fa-times-circle fa-lg',
                                                    addclass: "stack-bottomright",
                                                    stack: my.stack_bottomright
                                                });
                                                //$().toastmessage('showErrorToast', data.Msg);
                                            }
                                        }).fail(function (jqXHR, textStatus) {
                                            console.log(jqXHR.responseText);
                                        }).always(function () {
                                            $this.button('reset');
                                        });
                                    }

                                    $dialog.dialog('close');
                                    $dialog.dialog('destroy');
                                }
                            },
                            'cancel': {
                                html: 'N&#227;o',
                                //priority: 'secondary',
                                "class": 'dnnSecondaryAction',
                                click: function () {
                                    $dialog.dialog('close');
                                    $dialog.dialog('destroy');
                                }
                            }
                        }
                    });

        $dialog.dialog('open');
    });

    $('#btnCancelIncomeSource').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#editIncomes input').val('');
        $('#iSProofCheckbox').attr({ 'checked': false });
        $('#iSIncomeNumericTextBox').data('kendoNumericTextBox').value(1);
        $('#btnRemoveIncomeSource').hide();
        my.vm.selectedRegion('MG');
        my.vm.selectedCountry('BR');
        $('#btnCopyIncomeSource').hide();
        my.vm.clientISId(null);
        $('#collapseIncomeSourceForm').collapse('toggle');
    });

    my.clientPersonalRefsTransport = {
        read: {
            url: '/desktopmodules/riw/api/people/GetClientPersonalRefs?personId=' + my.personId
        }
    };

    my.clientPersonalRefsData = new kendo.data.DataSource({
        transport: my.clientPersonalRefsTransport,
        sort: { field: "PRName", dir: "ASC" },
        schema: {
            model: {
                id: 'ClientPersonalRefId'
            }
        }
    });

    my.createPersonalRefsLV = function () {
        $("#lvPersonalRefs").kendoListView({
            dataSource: my.clientPersonalRefsData,
            template: kendo.template($("#tmplClientPersonalRefs").html()),
            dataBound: function () {
                //if (this.dataSource.total() === 0) $('#collapsePersonalRefForm').collapse('toggle');
            }
        });
    };

    my.createPersonalRefsLV();

    $("#personalRefPhoneTextBox").inputmask("(99) 9999-9999");

    my.editClientPersonalRef = function (value) {
        $.ajax({
            url: '/desktopmodules/riw/api/people/GetClientPersonalRef?clientPersonalRefId=' + value + '&personId=' + my.personId
        }).done(function (data) {
            my.vm.clientPRId(data.ClientPersonalRefId);
            $('#personalRefNameTextBox').val(data.PRName);
            $('#personalRefPhoneTextBox').val(data.PRPhone);
            $('#personalRefEmailTextBox').val(data.PREmail);
            $("#personalRefPhoneTextBox").inputmask("(99) 9999-9999");
            $('#btnUpdatePersonalRef').html('<i class="fa fa-check"></i>&nbsp; Atualizar Refer&#234;ncia');
            $('#btnRemovePersonalRef').show();
            if ($('#collapsePersonalRef.in').length === 0) {
                $('#collapsePersonalRef').collapse('toggle');
            }
            $.scrollTo($('#collapsePersonalRef'), 1000, { easing: 'swing' });
        });
    };

    $('#btnUpdatePersonalRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        //if (clientFinancesValidator.validate()) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            ClientPersonalRefId: my.vm.clientPRId(),
            PersonId: my.personId,
            PRName: $('#personalRefNameTextBox').val().trim(),
            PRPhone: $('#personalRefPhoneTextBox').val().trim().replace(/\D/g, ''),
            PREmail: $('#personalRefEmailTextBox').val().trim()
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/people/updateClientPersonalRef',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.clientPersonalRefsData.read();
                $('#editPersonalRefs input').val('');
                $('#btnRemovePersonalRef').hide();

                if (params.cPRId !== 0) {
                    //$().toastmessage('showSuccessToast', 'Refer&#234;ncia atualizada com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Refer&#234;ncia atualizada.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                } else {
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Refer&#234;ncia inserida.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    //$().toastmessage('showSuccessToast', 'Refer&#234;ncia inserida com sucesso.');
                }

                $('#collapsePersonalRef').collapse('toggle');
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', data.Msg);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });

        //}
    });

    $('#btnRemovePersonalRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var $dialog = $('<div></div>')
                    .html('<div class="confirmDialog">Tem Certeza?</div>')
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
                                "class": 'dnnPrimaryAction',
                                click: function () {
                                    if (my.vm.clientPRId() !== 0) {
                                        $.ajax({
                                            type: 'DELETE',
                                            url: '/desktopmodules/riw/api/people/removeClientPersonalRef?clientPersonalRefId=' + my.vm.clientPRId() + '&personId=' + my.personId
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                my.clientPersonalRefsData.read();
                                                $('#editPersonalRefs input').val('');
                                                $('#btnRemovePersonalRef').hide();
                                                $('#btnUpdatePersonalRef').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Refer&#234;ncia').attr({ 'disabled': false });
                                                //$().toastmessage('showSuccessToast', 'Refer&#234;ncia excluida com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Refer&#234;ncia excluida.',
                                                    type: 'success',
                                                    icon: 'fa fa-check fa-lg',
                                                    addclass: "stack-bottomright",
                                                    stack: my.stack_bottomright
                                                });
                                                $('#collapsePersonalRef').collapse('toggle');
                                            } else {
                                                $.pnotify({
                                                    title: 'Erro!',
                                                    text: data.Result,
                                                    type: 'error',
                                                    icon: 'fa fa-times-circle fa-lg',
                                                    addclass: "stack-bottomright",
                                                    stack: my.stack_bottomright
                                                });
                                                //$().toastmessage('showErrorToast', data.Msg);
                                            }
                                        }).fail(function (jqXHR, textStatus) {
                                            console.log(jqXHR.responseText);
                                        }).always(function () {
                                            $this.button('reset');
                                        });
                                    }

                                    $dialog.dialog('close');
                                    $dialog.dialog('destroy');
                                }
                            },
                            'cancel': {
                                html: 'N&#227;o',
                                //priority: 'secondary',
                                "class": 'dnnSecondaryAction',
                                click: function () {
                                    $dialog.dialog('close');
                                    $dialog.dialog('destroy');
                                }
                            }
                        }
                    });

        $dialog.dialog('open');
    });

    $('#btnCancelPersonalRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#editPersonalRefs input').val('');
        $('#btnRemovePersonalRef').hide();
        $('#collapsePersonalRef').collapse('toggle');
        $('#btnUpdatePersonalRef').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Refer&#234;ncia').attr({ 'disabled': false });
        my.vm.clientPRId(null);
    });

    $('#partnerBankRefClientSinceTextBox').kendoDatePicker();
    $('#bankRefClientSinceTextBox').kendoDatePicker();

    my.clientBankRefsTransport = {
        read: {
            url: '/desktopmodules/riw/api/people/GetClientBankRefs?personId=' + my.personId
        }
    };

    my.clientBankRefsData = new kendo.data.DataSource({
        transport: my.clientBankRefsTransport,
        sort: { field: "BankRef", dir: "ASC" },
        schema: {
            model: {
                id: 'ClientBankRefId'
            }
        }
    });

    my.createBankRefsLV = function () {
        $("#lvBankRefs").kendoListView({
            dataSource: my.clientBankRefsData,
            template: kendo.template($("#tmplClientBankRefs").html()),
            dataBound: function () {
                //if (this.dataSource.total() === 0) $('#collapseBankRefForm').collapse('toggle');
            }
        });
    };

    my.createBankRefsLV();

    $('#bankRefCreditLimitTextBox').kendoNumericTextBox({
        value: 0,
        min: 0,
        format: 'c'
    });

    $("#bankRefContactPhoneTextBox").inputmask("(99) 9999-9999");

    my.editClientBankRef = function (value) {
        $.ajax({
            url: '/desktopmodules/riw/api/people/GetClientBankRef?clientBankRefId=' + value + '&personId=' + my.personId
        }).done(function (data) {
            my.vm.clientBRId(data.ClientBankRefId);
            $('#bankRefTextBox').val(data.BankRef);
            $('#bankRefAgencyTextBox').val(data.BankRefAgency);
            $('#bankRefAccountTextBox').val(data.BankRefAccount);
            $('#bankRefClientSinceTextBox').data('kendoDatePicker').value(kendo.parseDate(data.BankRefClientSince));
            $('#bankRefContactNameTextBox').val(data.BankRefContact);
            $('#bankRefContactPhoneTextBox').val(data.BankRefPhone);
            $('#bankRefAccountTypeTextBox').val(data.BankRefAccountType);
            $('#bankRefCreditLimitTextBox').data('kendoNumericTextBox').value(data.BankRefCredit);
            $("#bankRefContactPhoneTextBox").inputmask("(99) 9999-9999");
            $('#btnUpdateBankRef').html('<i class="fa fa-ok"></i>&nbsp; Atualizar Refer&#234;ncia');
            $('#btnRemoveBankRef').show();
            if ($('#collapseBankRefForm.in').length === 0) {
                $('#collapseBankRefForm').collapse('toggle');
            }
            $.scrollTo($('#collapseBankRefForm'), 1000, { easing: 'swing' });
        });
    };

    $('#btnUpdateBankRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        //if (clientFinancesValidator.validate()) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            clientBankRefId: my.vm.clientBRId(),
            PersonId: my.personId,
            BankRef: $('#bankRefTextBox').val().trim(),
            BankRefAgency: $('#bankRefAgencyTextBox').val().trim(),
            BankRefAccount: $('#bankRefAccountTextBox').val().trim(),
            BankRefClientSince: moment(new Date($('#bankRefClientSinceTextBox').data('kendoDatePicker').value())).format(),
            BankRefContact: $('#bankRefContactNameTextBox').val().trim(),
            BankRefPhone: $('#bankRefContactPhoneTextBox').val().replace(/\D/g, ''),
            BankRefAccountType: $('#bankRefAccountTypeTextBox').val().trim(),
            BankRefCredit: kendo.parseInt($('#bankRefCreditLimitTextBox').val()) > 0 ? $('#bankRefCreditLimitTextBox').data('kendoNumericTextBox').value() : 0
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/people/updateClientBankRef',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.vm.clientBRId(0);
                my.clientBankRefsData.read();
                $('#editBankRefs input').val('');
                $('#bankRefCreditLimitTextBox').data('kendoNumericTextBox').value(0);
                $('#bankRefClientSinceTextBox').data('kendoDatePicker').value('');
                $('#btnRemoveBankRef').hide();

                if (params.clientBankRefId !== 0) {
                    //$().toastmessage('showSuccessToast', 'Refer&#234;ncia atualizada com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Refer&#234;ncia atualizada.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                } else {
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Refer&#234;ncia inserida.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    //$().toastmessage('showSuccessToast', 'Refer&#234;ncia inserida com sucesso.');
                }

                $('#collapseBankRefForm').collapse('toggle');
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', data.Msg);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
        //}
    });

    $('#btnRemoveBankRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var $dialog = $('<div></div>')
                    .html('<div class="confirmDialog">Tem Certeza?</div>')
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
                                "class": 'dnnPrimaryAction',
                                click: function () {
                                    if (my.vm.clientBRId() !== 0) {

                                        $this.button('loading');

                                        $.ajax({
                                            type: 'DELETE',
                                            url: '/desktopmodules/riw/api/people/removeClientBankReference?clientBankReferenceId=' + my.vm.clientBRId() + '&personId=' + my.personId
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                my.vm.clientBRId(0);
                                                my.clientBankRefsData.read();
                                                $('#editBankRefs input').val('');
                                                $('#bankRefCreditLimitTextBox').data('kendoNumericTextBox').value(0);
                                                $('#bankRefClientSinceTextBox').data('kendoDatePicker').value('');
                                                $('#btnRemoveBankRef').hide();
                                                $('#btnUpdateBankRef').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Refer&#234;ncia').attr({ 'disabled': false });
                                                //$().toastmessage('showSuccessToast', 'Referência excluida com sucesso.');
                                                $('#collapseBankRefForm').collapse('toggle');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Refer&#234;ncia excluida.',
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
                                                //$().toastmessage('showErrorToast', data.Msg);
                                            }
                                        }).fail(function (jqXHR, textStatus) {
                                            console.log(jqXHR.responseText);
                                        }).always(function () {
                                            $this.button('reset');
                                        });
                                    }

                                    $dialog.dialog('close');
                                    $dialog.dialog('destroy');
                                }
                            },
                            'cancel': {
                                html: 'N&#227;o',
                                //priority: 'secondary',
                                "class": 'dnnSecondaryAction',
                                click: function () {
                                    $dialog.dialog('close');
                                    $dialog.dialog('destroy');
                                }
                            }
                        }
                    });

        $dialog.dialog('open');
    });

    $('#btnCancelBankRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#editBankRefs input').val('');
        $('#bankRefCreditLimitTextBox').data('kendoNumericTextBox').value(0);
        $('#bankRefClientSinceTextBox').data('kendoDatePicker').value('');
        $('#btnRemoveBankRef').hide();
        $('#btnUpdateBankRef').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Refer&#234;ncia').attr({ 'disabled': false });
        $('#collapseBankRefForm').collapse('toggle');
        my.vm.clientBRId(null);
    });

    $('#commRefLastActivityTextBox').kendoDatePicker();

    my.clientCommRefsTransport = {
        read: {
            url: '/desktopmodules/riw/api/people/GetClientCommRefs?personId=' + my.personId
        }
    };

    my.clientCommRefsData = new kendo.data.DataSource({
        transport: my.clientCommRefsTransport,
        sort: { field: "CommRef", dir: "ASC" },
        schema: {
            model: {
                id: 'ClientCommRefId',
                fields: {
                    //DateBirth: { field: "DateBirth", type: "date", format: "{0:m}" }
                }
            }
        }
    });

    my.createCommRefsLV = function () {
        $("#lvCommRefs").kendoListView({
            dataSource: my.clientCommRefsData,
            template: kendo.template($("#tmplClientCommRefs").html()),
            dataBound: function () {
                //if (this.dataSource.total() === 0) $('#collapseCommRefForm').collapse('toggle');
            }
        });
    };

    my.createCommRefsLV();

    $('#commRefCreditTextBox').kendoNumericTextBox({
        value: 0,
        min: 0,
        format: 'c'
    });

    $("#commRefPhoneTextBox").inputmask("(99) 9999-9999");

    my.editClientCommRef = function (value) {
        $.ajax({
            url: '/desktopmodules/riw/api/people/GetClientCommRef?clientCommRefId=' + value + '&personId=' + my.personId
        }).done(function (data) {
            my.vm.clientCRId(data.ClientCommRefId);
            $('#commRefNameTextBox').val(data.CommRefBusiness);
            $('#commRefContactTextBox').val(data.CommRefContact);
            $('#commRefPhoneTextBox').val(data.CommRefPhone);
            $('#commRefLastActivityTextBox').data('kendoDatePicker').value(kendo.parseDate(data.CommRefLastActivity));
            $('#commRefCreditTextBox').data('kendoNumericTextBox').value(data.CommRefCredit);
            $("#commRefPhoneTextBox").inputmask("(99) 9999-9999");
            $('#btnUpdateCommRef').html('<i class="fa fa-check"></i>&nbsp; Atualizar Refer&#234;ncia');
            $('#btnRemoveCommRef').show();
            if ($('#collapseCommRefForm.in').length === 0) {
                $('#collapseCommRefForm').collapse('toggle');
            }
            $.scrollTo($('#collapseCommRefForm'), 1000, { easing: 'swing' });
        });
    };

    $('#btnUpdateCommRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        //if (clientFinancesValidator.validate()) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            ClientCommRefId: my.vm.clientCRId(),
            PersonId: my.personId,
            CommRefBusiness: $('#commRefNameTextBox').val(),
            CommRefContact: $('#commRefContactTextBox').val(),
            CommRefPhone: $('#commRefPhoneTextBox').val().replace(/\D/g, ''),
            CommRefLastActivity: moment(new Date($('#commRefLastActivityTextBox').data('kendoDatePicker').value())).format(),
            CommRefCredit: $('#commRefCreditTextBox').data('kendoNumericTextBox').value()
        };
                
        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/people/updateClientCommRef',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.vm.clientCRId(0);
                my.clientCommRefsData.read();
                $('#editCommRefs input').val('');
                $('#commRefCreditTextBox').data('kendoNumericTextBox').value(0);
                $('#commRefLastActivityTextBox').data('kendoDatePicker').value('');
                $('#btnRemoveCommRef').hide();

                if (params.ClientCommRefId !== 0) {
                    //$().toastmessage('showSuccessToast', 'Refer&#234;ncia atualizada com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Refer&#234;ncia atualizada.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                } else {
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Refer&#234;ncia inserida.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    //$().toastmessage('showSuccessToast', 'Refer&#234;ncia inserida com sucesso.');
                }
                $('#collapseCommRefForm').collapse('toggle');
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', data.Msg);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function (e) {
            $this.button('reset');
        });
        
        //}
    });

    $('#btnRemoveCommRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var $dialog = $('<div></div>')
                    .html('<div class="confirmDialog">Tem Certeza?</div>')
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
                                "class": 'dnnPrimaryAction',
                                click: function () {
                                    if (my.vm.clientCRId() !== 0) {

                                        $this.button('loading');

                                        $.ajax({
                                            type: 'DELETE',
                                            url: '/desktopmodules/riw/api/people/removeClientCommRef?clientCommReferenceId=' + my.vm.clientCRId() + '&personId=' + my.personId
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                my.vm.clientCRId(0);
                                                my.clientCommRefsData.read();
                                                $('#editCommRefs input').val('');
                                                $('#commRefCreditTextBox').data('kendoNumericTextBox').value(0);
                                                $('#commRefLastActivityTextBox').data('kendoDatePicker').value('');
                                                $('#btnRemoveCommRef').hide();
                                                $('#btnUpdateCommRef').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Refer&#234;ncia').attr({ 'disabled': false });
                                                //$().toastmessage('showSuccessToast', 'Refer&#234;ncia excluida com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Refer&#234;ncia excluida.',
                                                    type: 'success',
                                                    icon: 'fa fa-check fa-lg',
                                                    addclass: "stack-bottomright",
                                                    stack: my.stack_bottomright
                                                });
                                                $('#collapseCommRefForm').collapse('toggle');
                                            } else {
                                                $.pnotify({
                                                    title: 'Erro!',
                                                    text: data.Result,
                                                    type: 'error',
                                                    icon: 'fa fa-times-circle fa-lg',
                                                    addclass: "stack-bottomright",
                                                    stack: my.stack_bottomright
                                                });
                                                //$().toastmessage('showErrorToast', data.Msg);
                                            }
                                        }).fail(function (jqXHR, textStatus) {
                                            console.log(jqXHR.responseText);
                                        }).always(function () {
                                            $this.button('reset');
                                        });
                                    }

                                    $dialog.dialog('close');
                                    $dialog.dialog('destroy');
                                }
                            },
                            'cancel': {
                                html: 'N&#227;o',
                                //priority: 'secondary',
                                "class": 'dnnSecondaryAction',
                                click: function () {
                                    $dialog.dialog('close');
                                    $dialog.dialog('destroy');
                                }
                            }
                        }
                    });

        $dialog.dialog('open');
    });

    $('#btnCancelCommRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#editCommRefs input').val('');
        $('#commRefCreditTextBox').data('kendoNumericTextBox').value(0);
        $('#commRefLastActivityTextBox').data('kendoDatePicker').value('');
        $('#btnRemoveCommRef').hide();
        $('#btnUpdateCommRef').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Refer&#234;ncia').attr({ 'disabled': false });
        $('#collapseCommRefForm').collapse('toggle');
        my.vm.clientCRId(null);
    });

    my.clientPartnersTransport = {
        read: {
            url: '/desktopmodules/riw/api/people/GetClientPartners?personId=' + my.personId
        }
    };

    my.clientPartnersData = new kendo.data.DataSource({
        transport: my.clientPartnersTransport,
        sort: { field: "PartnerName", dir: "ASC" },
        schema: {
            model: {
                id: 'ClientPartnerId'
            }
        }
    });

    my.createPartnersLV = function () {
        $("#lvClientPartners").kendoListView({
            dataSource: my.clientPartnersData,
            template: kendo.template($("#tmplClientPartners").html()),
            dataBound: function () {
                //if (this.dataSource.total() === 0) $('#collapsePartnerForm').collapse('toggle');
            }
        });
    };

    my.createPartnersLV();

    $('#partnerQuotaTextBox').kendoNumericTextBox({
        value: 0,
        format: "##.00 '%'"
    });

    $("#partnerCPFTextBox").inputmask("999999999-99");
    $("#partnerPostalCodeTextBox").inputmask("99-999-999");
    $("#partnerPhoneTextBox").inputmask("(99) 9999-9999");
    $("#partnerCellTextBox").inputmask("(99) 9999-9999");
    //$("#ddlAddressCountries").css('width', '200px');
    //$("#ddlAddressRegions").css('width', '200px');
    //var ddlPartnerCountries = $('#ddlPartnerCountries').data('kendoDropDownList');
    //ddlPartnerCountries.wrapper.find(".k-dropdown-wrap").css({ 'width': '160px' });
    //ddlPartnerCountries.list.width(180);
    //var ddlPartnerRegions = $('#ddlPartnerRegions').data('kendoDropDownList');
    //ddlPartnerRegions.wrapper.find(".k-dropdown-wrap").css({ 'width': '160px' });
    //ddlPartnerRegions.list.width(180);
    //$('#addressNameTextBox').attr({ 'required': true });
    //$('#contactNameTextBox').attr({ 'required': false });

    my.editClientPartner = function (value) {
        $.ajax({
            url: '/desktopmodules/riw/api/people/GetClientPartner?clientPartnerId=' + value + '&personId=' + my.personId
        }).done(function (data) {
            my.vm.clientPartnerId(data.ClientPartnerId);
            $('#partnerNameTextBox').val(data.PartnerName);
            $('#partnerCPFTextBox').val(data.PartnerCPF);
            $('#partnerIdentTextBox').val(data.PartnerIdentity);
            $('#partnerPhoneTextBox').val(data.PartnerPhone);
            $('#partnerCellTextBox').val(data.PartnerCell);
            $('#partnerEmailTextBox').val(data.PartnerEmail);
            $('#partnerQuotaTextBox').data('kendoNumericTextBox').value(data.PartnerQuota);
            $('#partnerPostalCodeTextBox').val(data.PartnerPostalCode);
            $('#partnerStreetTextBox').val(data.PartnerAddress);
            $('#partnerUnitTextBox').val(data.PartnerAddressUnit);
            $('#partnerComplementTextBox').val(data.PartnerComplement);
            $('#partnerDistrictTextBox').val(data.PartnerDistrict);
            $('#partnerCityTextBox').val(data.PartnerCity);
            my.vm.selectedRegion(data.PartnerRegion);
            my.vm.selectedCountry('BR');
            $("#partnerCPFTextBox").inputmask("999999999-99");
            $("#partnerPostalCodeTextBox").inputmask("99-999-999");
            $("#partnerPhoneTextBox").inputmask("(99) 9999-9999");
            $("#partnerCellTextBox").inputmask("(99) 9999-9999");
            $('#btnUpdatePartner').html('<i class="fa fa-check"></i>&nbsp; Atualizar S&#243;cio');
            $('#btnRemovePartner').show();
            if ($('#collapsePartnerForm.in').length === 0) {
                $('#collapsePartnerForm').collapse('toggle');
            }
            $.scrollTo($('#collapsePartnerForm'), 1000, { easing: 'swing' });
        });
        return false;
    };

    $('#btnUpdatePartner').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        //if (clientAddressesValidator.validate()) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            ClientPartnerId: my.vm.clientPartnerId(),
            PersonId: my.personId,
            PartnerName: $('#partnerNameTextBox').val(),
            PartnerCPF: $('#partnerCPFTextBox').val(),
            PartnerIdentity: $('#partnerIdentTextBox').val(),
            PartnerPhone: $('#partnerPhoneTextBox').val().replace(/\D/g, ''),
            PartnerCell: $('#partnerCellTextBox').val().replace(/\D/g, ''),
            PartnerEmail: $('#partnerEmailTextBox').val(),
            PartnerQuota: $('#partnerQuotaTextBox').data('kendoNumericTextBox').value(),
            PartnerPostalCode: $('#partnerPostalCodeTextBox').val().replace(/\D/g, ''),
            PartnerAddress: $('#partnerStreetTextBox').val(),
            PartnerAddressUnit: $('#partnerUnitTextBox').val(),
            PartnerComplement: $('#partnerComplementTextBox').val(),
            PartnerDistrict: $('#partnerDistrictTextBox').val(),
            Partnercity: $('#partnerCityTextBox').val(),
            PartnerRegion: my.vm.selectedRegion(),
            PartnerCountry: my.vm.selectedCountry()
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/people/updateClientPartner',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.vm.clientPartnerId(0);
                my.clientPartnersData.read();
                $('#editPartners input').val('');
                $('#btnRemovePartner').hide();
                $('#btnCopyPartner').hide();

                if (params.ClientPartnerId !== 0) {
                    //$().toastmessage('showSuccessToast', 'S&#243;cio atualizado com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Dados do s&#243;cio atualizado.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                } else {
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Dados do s&#243;cio inserido.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    //$().toastmessage('showSuccessToast', 'S&#243;cio inserido com sucesso.');
                }

                $('#collapsePartnerForm').collapse('toggle');
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', data.Msg);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
        
        //}
    });

    $('#btnCopyPartner').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        //if (clientAddressesValidator.validate()) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            ClientPartnerId: 0,
            PersonId: my.personId,
            PartnerName: $('#partnerNameTextBox').val(),
            PartnerCPF: $('#partnerCPFTextBox').val().replace(/\D/g, ''),
            PartnerIdentity: $('#partnerIdentTextBox').val(),
            PartnerPhone: $('#partnerPhoneTextBox').val().replace(/\D/g, ''),
            PartnerCell: $('#partnerCellTextBox').val().replace(/\D/g, ''),
            PartnerEmail: $('#partnerEmailTextBox').val(),
            PartnerQuota: $('#partnerQuotaTextBox').data('kendoNumericTextBox').value(),
            PartnerPostalCode: $('#partnerPostalCodeTextBox').val().replace(/\D/g, ''),
            PartnerAddress: $('#partnerStreetTextBox').val(),
            PartnerAddressUnit: $('#partnerUnitTextBox').val(),
            PartnerComplement: $('#partnerComplementTextBox').val(),
            PartnerDistrict: $('#partnerDistrictTextBox').val(),
            Partnercity: $('#partnerCityTextBox').val(),
            PartnerRegion: my.vm.selectedRegion(),
            PartnerCountry: my.vm.selectedCountry()
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/people/updateClientPartner',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.vm.clientPartnerId(0);
                my.clientPartnersData.read();
                $('#editPartners input').val('');
                $('#btnRemovePartner').hide();
                //$().toastmessage('showSuccessToast', 'S&#243;cio inserido com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Dados do s&#243;cio duplicado.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                $('#collapsePartnerForm').collapse('toggle');
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', data.Msg);
            }

            $('#btnUpdatePartner').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar S&#243;cio').attr({ 'disabled': false });
            
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
        //}
    });

    $('#btnRemovePartner').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var $dialog = $('<div></div>')
                    .html('<div class="confirmDialog">Tem Certeza?</div>')
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
                                "class": 'dnnPrimaryAction',
                                click: function () {
                                    if (my.vm.clientPartnerId() !== 0) {

                                        $this.button('loading');

                                        $.ajax({
                                            type: 'DELETE',
                                            url: '/desktopmodules/riw/api/people/RemoveClientPartner?clientPartnerId=' + my.vm.clientPartnerId() + '&personId=' + my.personId
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                my.vm.clientPartnerId(0);
                                                my.clientPartnersData.read();
                                                $('#editPartners input').val('');
                                                $('#btnRemovePartner').hide();
                                                $('#btnUpdatePartner').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar S&#243;cio').attr({ 'disabled': false });
                                                //$().toastmessage('showSuccessToast', 'S&#243;cio excluido com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Dados do s&#243;cio excluido.',
                                                    type: 'success',
                                                    icon: 'fa fa-check fa-lg',
                                                    addclass: "stack-bottomright",
                                                    stack: my.stack_bottomright
                                                });
                                                $('#collapsePartnerForm').collapse('toggle');
                                            } else {
                                                $.pnotify({
                                                    title: 'Erro!',
                                                    text: data.Result,
                                                    type: 'error',
                                                    icon: 'fa fa-times-circle fa-lg',
                                                    addclass: "stack-bottomright",
                                                    stack: my.stack_bottomright
                                                });
                                                //$().toastmessage('showErrorToast', data.Msg);
                                            }
                                        }).fail(function (jqXHR, textStatus) {
                                            console.log(jqXHR.responseText);
                                        }).always(function () {
                                            $this.button('reset');
                                        });
                                    }

                                    $dialog.dialog('close');
                                    $dialog.dialog('destroy');
                                }
                            },
                            'cancel': {
                                html: 'N&#227;o',
                                //priority: 'secondary',
                                "class": 'dnnSecondaryAction',
                                click: function () {
                                    $dialog.dialog('close');
                                    $dialog.dialog('destroy');
                                }
                            }
                        }
                    });

        $dialog.dialog('open');
    });

    $('#btnCancelPartner').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#collapsePartnerForm input').val('');
        $('#partnerQuotaTextBox').data('kendoNumericTextBox').value(0);
        $('#btnRemovePartner').hide();
        $('#btnUpdatePartner').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar S&#243;cio').attr({ 'disabled': false });
        $('#collapsePartnerForm').collapse('toggle');
        my.vm.clientPartnerId(null);
    });

    my.clientPartnerBankRefsTransport = {
        read: {
            url: '/desktopmodules/riw/api/people/GetClientPartnerBankRefs?personId=' + my.personId
        }
    };

    my.clientPartnerBankRefsData = new kendo.data.DataSource({
        transport: my.clientPartnerBankRefsTransport,
        sort: { field: "BankRef", dir: "ASC" },
        schema: {
            model: {
                id: 'ClientPartnerBankRefId',
                fields: {
                    //DateBirth: { field: "DateBirth", type: "date", format: "{0:m}" }
                }
            }
        }
    });

    my.createPartnerBankRefsLV = function () {
        $("#lvPartnerBankRefs").kendoListView({
            dataSource: my.clientPartnerBankRefsData,
            template: kendo.template($("#tmplClientPartnerBankRefs").html()),
            dataBound: function () {
                //if (this.dataSource.total() === 0) $('#collapseBankPartnerRefForm').collapse('toggle');
            }
        });
    };

    my.createPartnerBankRefsLV();

    $('#ddlClientPartners').kendoDropDownList({
        dataSource: my.clientPartnersData,
        dataTextField: 'PartnerName',
        dataValueField: 'ClientPartnerId'
    })
    
    $('#partnerBankRefCreditLimitTextBox').kendoNumericTextBox({
        value: 0,
        min: 0,
        format: 'c'
    });

    $("#partnerBankRefContactPhoneTextBox").inputmask("(99) 9999-9999");

    my.editClientPartnerBankRef = function (value) {
        $.ajax({
            url: '/desktopmodules/riw/api/people/GetClientPartnerBankRef?clientPartnerBankReferenceId=' + value + '&personId=' + my.personId
        }).done(function (data) {
            my.vm.clientPBRId(data.ClientPartnerBankRefId);
            $('#ddlClientPartners').data('kendoDropDownList').text(data.PartnerName);
            $('#partnerBankRefTextBox').val(data.BankRef);
            $('#partnerBankRefAgencyTextBox').val(data.BankRefAgency);
            $('#partnerBankRefAccountTextBox').val(data.BankRefAccount);
            $('#partnerBankRefClientSinceTextBox').data('kendoDatePicker').value(kendo.parseDate(data.BankRefClientSince));
            $('#partnerBankRefContactNameTextBox').val(data.BankRefContact);
            $('#partnerBankRefContactPhoneTextBox').val(data.BankRefPhone);
            $('#partnerBankRefAccountTypeTextBox').val(data.BankRefAccountType);
            $('#partnerBankRefCreditLimitTextBox').data('kendoNumericTextBox').value(data.BankRefCredit);
            $("#partnerBankRefContactPhoneTextBox").inputmask("(99) 9999-9999");
            $('#btnUpdatePartnerBankRef').html('<i class="fa fa-check"></i>&nbsp; Atualizar Refer&#234;ncia');
            $('#btnRemovePartnerBankRef').show();
            if ($('#collapseBankPartnerRefForm.in').length === 0) {
                $('#collapseBankPartnerRefForm').collapse('toggle');
            }
            $.scrollTo($('#collapseBankPartnerRefForm'), 1000, { easing: 'swing' });
        });
    };

    $('#btnUpdatePartnerBankRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        //if (clientFinancesValidator.validate()) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            ClientPartnerBankRefId: my.vm.clientPBRId(),
            PartnerName: $('#ddlClientPartners').data('kendoDropDownList').text(),
            PersonId: my.personId,
            BankRef: $('#partnerBankRefTextBox').val(),
            BankRefAgency: $('#partnerBankRefAgencyTextBox').val(),
            BankRefAccount: $('#partnerBankRefAccountTextBox').val(),
            BankRefClientSince: moment(new Date($('#partnerBankRefClientSinceTextBox').data('kendoDatePicker').value())).format(),
            BankRefContact: $('#partnerBankRefContactNameTextBox').val(),
            BankRefPhone: $('#partnerBankRefContactPhoneTextBox').val().replace(/\D/g, ''),
            BankRefAccountType: $('#partnerBankRefAccountTypeTextBox').val(),
            BankRefCredit: $('#partnerBankRefCreditLimitTextBox').data('kendoNumericTextBox').value()
        };
                
            $.ajax({
                type: 'POST',
                url: '/desktopmodules/riw/api/people/updateClientpartnerBankRef',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    my.vm.clientPBRId(0);
                    my.clientPartnerBankRefsData.read();
                    $('#collapseBankPartnerRefForm input').val('');
                    $('#partnerBankRefCreditLimitTextBox').data('kendoNumericTextBox').value(0);
                    $('#partnerBankRefClientSinceTextBox').data('kendoDatePicker').value('');
                    $('#btnRemovePartnerBankRef').hide();
                    
                    if (params.ClientPartnerBankRefId !== 0) {
                        //$().toastmessage('showSuccessToast', 'Refer&#234;ncia atualizada com sucesso.');
                        $.pnotify({
                            title: 'Sucesso!',
                            text: 'Refer&#234;ncia atualizada.',
                            type: 'success',
                            icon: 'fa fa-check fa-lg',
                            addclass: "stack-bottomright",
                            stack: my.stack_bottomright
                        });
                    } else {
                        $.pnotify({
                            title: 'Sucesso!',
                            text: 'Refer&#234;ncia inserida.',
                            type: 'success',
                            icon: 'fa fa-check fa-lg',
                            addclass: "stack-bottomright",
                            stack: my.stack_bottomright
                        });
                        //$().toastmessage('showSuccessToast', 'Refer&#234;ncia inserida com sucesso.');
                    }

                    $('#collapseBankPartnerRefForm').collapse('toggle');
                } else {
                    $.pnotify({
                        title: 'Erro!',
                        text: data.Result,
                        type: 'error',
                        icon: 'fa fa-times-circle fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    //$().toastmessage('showErrorToast', data.Msg);
                }
            }).fail(function (jqXHR, textStatus) {
                console.log(jqXHR.responseText);
            }).always(function () {
                $this.button('reset');
            });
        
        //}
    });

    $('#btnRemovePartnerBankRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var $dialog = $('<div></div>')
                    .html('<div class="confirmDialog">Tem Certeza?</div>')
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
                                "class": 'dnnPrimaryAction',
                                click: function () {
                                    if (my.vm.clientPBRId() !== 0) {

                                        $this.button('loading');

                                        $.ajax({
                                            type: 'DELETE',
                                            url: '/desktopmodules/riw/api/people/removeClientPartnerBankReference?clientPartnerBankRefId=' + my.vm.clientPBRId() + '&personId=' + my.personId
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                my.vm.clientPBRId(0);
                                                my.clientPartnerBankRefsData.read();
                                                $('#collapseBankPartnerRefForm input').val('');
                                                $('#partnerBankRefCreditLimitTextBox').data('kendoNumericTextBox').value(0);
                                                $('#partnerBankRefClientSinceTextBox').data('kendoDatePicker').value('');
                                                $('#btnRemovePartnerBankRef').hide();
                                                $('#btnUpdatePartnerBankRef').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Refer&#234;ncia').attr({ 'disabled': false });
                                                //$().toastmessage('showSuccessToast', 'Referência excluida com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Refer&#234;ncia excluida.',
                                                    type: 'success',
                                                    icon: 'fa fa-check fa-lg',
                                                    addclass: "stack-bottomright",
                                                    stack: my.stack_bottomright
                                                });
                                                $('#collapseBankPartnerRefForm').collapse('toggle');
                                            } else {
                                                $.pnotify({
                                                    title: 'Erro!',
                                                    text: data.Result,
                                                    type: 'error',
                                                    icon: 'fa fa-times-circle fa-lg',
                                                    addclass: "stack-bottomright",
                                                    stack: my.stack_bottomright
                                                });
                                                //$().toastmessage('showErrorToast', data.Msg);
                                            }
                                        }).fail(function (jqXHR, textStatus) {
                                            console.log(jqXHR.responseText);
                                        }).always(function () {
                                            $this.button('reset');
                                        });
                                    }

                                    $dialog.dialog('close');
                                    $dialog.dialog('destroy');
                                }
                            },
                            'cancel': {
                                html: 'N&#227;o',
                                //priority: 'secondary',
                                "class": 'dnnSecondaryAction',
                                click: function () {
                                    $dialog.dialog('close');
                                    $dialog.dialog('destroy');
                                }
                            }
                        }
                    });

        $dialog.dialog('open');
    });

    $('#btnCancelPartnerBankRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#collapseBankPartnerRefForm input').val('');
        $('#partnerBankRefCreditLimitTextBox').data('kendoNumericTextBox').value(0);
        $('#partnerBankRefClientSinceTextBox').data('kendoDatePicker').value('');
        $('#btnRemovePartnerBankRef').hide();
        $('#btnUpdatePartnerBankRef').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Refer&#234;ncia').attr({ 'disabled': false });
        $('#collapseBankPartnerRefForm').collapse('toggle');
        my.vm.clientPBRId(null);
    });

    $('#ddlFinanAddress').data('kendoDropDownList').bind('dataBound', function (e) {
        if (this.dataSource.total() === 0) {
            this.text('Sem Registro');
            this.enable(false);
        }
    });

    $('input.enterastab, select.enterastab, textarea.enterastab').on('keydown', function (e) {
        if (e.keyCode === 13) {
            var focusable = $('input,a,select,textarea').filter(':visible');
            if (this.name === 'streetTextBox') {
                focusable.eq(focusable.index(this) + 1).focus().select();
            } else {
                focusable.eq(focusable.index(this) + 2).focus().select();
            };
            return false;
        }
    });

    $('.btnAddHistory').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var ele = $(this).data('provider');

        var historyHtmlContent = my.converter.makeHtml($('#' + ele).val().trim());

        var params = {
            PortalId: portalID,
            PersonId: my.personId,
            HistoryText: historyHtmlContent,
            Locked: true,
            CreatedByUser: userID,
            CreatedOnDate: moment().format()
            //ConnId: my.hub.connection.id
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/people/updateHistory',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $('#' + ele).val('');
                //$().toastmessage('showSuccessToast', 'Texto adicionado ao hist&#243;rico com successo!');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Texto adicionado ao hist&#243;rico.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                params.Avatar = amplify.store.sessionStorage('avatar'); // ? '/portals/0/' + _avatar + '?w=45&h=45&mode=crop' : '/desktopmodules/rildoinfo/webapi/content/images/user.png?w=45&h=45';
                params.DisplayName = displayName;
                my.vm.personHistories.unshift(new my.PersonHistory(params));

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

    $('.markdown-editor').css({ 'min-width': '90%', 'height': '80px', 'margin-bottom': '5px' }).attr({ 'cols': '30', 'rows': '2' });

    $('.markdown-editor').autogrow();
    $('.markdown-editor').css('overflow', 'hidden').autogrow();

    $('.togglePreview').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();
        var $this = $(this);

        var content = $('#historyTextArea1').val();
        content += $('#historyTextArea2').val();
        content += $('#historyTextArea3').val();
        content += $('#historyTextArea4').val();
        content += $('#historyTextArea5').val();
        content += $('#historyTextArea6').val();
        content += $('#historyTextArea7').val();

        var $dialog = $('<div></div>')
            .html(my.converter.makeHtml(content.trim()))
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

    my.vm.loadRegions();

    if (my.getParameterByName('sel')) {
        var sel = my.getParameterByName('sel');
        $('#actionsMenu ul li[id=' + sel + ']').addClass('jqx-menu-item-selected');
        if (sel <= 7) {
            $('#personMenu ul li:first-child').addClass('jqx-menu-item-selected');
            //} else {
        }
    }

    $('#actionsMenu').on('itemclick', function (event) {
        //event.args.textContent = 'Um momento...';
        switch (event.args.id) {
            case '7':
                document.location.href = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/7';
                break;
            case '8':
                document.location.href = historyURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/8';
                break;
            case '9':
                document.location.href = finanURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/9';
                break;
            case '10':
                document.location.href = assistURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/10';
                break;
            case '11':
                document.location.href = commURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/11';
                break;
            default:
                var urlAddress = '';
                if (my.retSel > 0) {
                    switch (my.retSel) {
                        case 7:
                            urlAddress = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/7';
                            break;
                        case 8:
                            urlAddress = historyURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/8';
                            break;
                        case 9:
                            urlAddress = finanURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/9';
                            break;
                        case 10:
                            urlAddress = assistURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/10';
                            break;
                        case 11:
                            urlAddress = commURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/11';
                            break;
                    }
                    document.location.href = urlAddress;
                } else {
                    //parent.$(".k-window-content").each(function (i, win) {
                    parent.$('#window').data("kendoWindow").close();
                    //});
                    //$(".k-window .k-window-content").each(function (index, element) {
                    //    setTimeout(function () {
                    //        $(element).data('kendoWindow').close();
                    //    }, 1000);
                    //});
                    //if (parent.$('#clientFinanWindow').data('kendoWindow')) {
                    //    parent.$('#clientFinanWindow').data('kendoWindow').close();
                    //}
                    //if (parent.$('#clientEditWindow').data('kendoWindow')) {
                    //    parent.$('#clientEditWindow').data('kendoWindow').close();
                    //}
                    //if (parent.$('#clientAssistWindow').data('kendoWindow')) {
                    //    parent.$('#clientAssistWindow').data('kendoWindow').close();
                    //}
                    //if (parent.$('#clientCommWindow').data('kendoWindow')) {
                    //    parent.$('#clientCommWindow').data('kendoWindow').close();
                    //}
                    //if (parent.$('#clientHistoryWindow').data('kendoWindow')) {
                    //    parent.$('#clientHistoryWindow').data('kendoWindow').close();
                    //}
                }
        }
    });

    $('.btnReturn').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var urlAddress = '';
        if (my.retSel > 0) {
            switch (my.retSel) {
                case 7:
                    urlAddress = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/7';
                    break;
                case 8:
                    urlAddress = historyURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/8';
                    break;
                case 9:
                    urlAddress = finanURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/9';
                    break;
                case 10:
                    urlAddress = assistURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/10';
                    break;
                case 11:
                    urlAddress = commURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/11';
                    break;
            }
            document.location.href = urlAddress;
        } else {
            //parent.$(".k-window-content").each(function (i, win) {
            parent.$('#window').data("kendoWindow").close();
            //});
            //$(".k-window .k-window-content").each(function (index, element) {
            //    setTimeout(function () {
            //        $(element).data('kendoWindow').close();
            //    }, 1000);
            //});
            //if (parent.$('#clientFinanWindow').data('kendoWindow')) {
            //    parent.$('#clientFinanWindow').data('kendoWindow').close();
            //}
            //if (parent.$('#clientEditWindow').data('kendoWindow')) {
            //    parent.$('#clientEditWindow').data('kendoWindow').close();
            //}
            //if (parent.$('#clientAssistWindow').data('kendoWindow')) {
            //    parent.$('#clientAssistWindow').data('kendoWindow').close();
            //}
            //if (parent.$('#clientCommWindow').data('kendoWindow')) {
            //    parent.$('#clientCommWindow').data('kendoWindow').close();
            //}
            //if (parent.$('#clientHistoryWindow').data('kendoWindow')) {
            //    parent.$('#clientHistoryWindow').data('kendoWindow').close();
            //}
        }
    });

});