
$(function () {

    my.pmtId = my.getQuerystring('pmtId', my.getParameterByName('pmtId'));

    my.viewModel();

    $('#moduleTitleSkinObject').html(moduleTitle);

    var buttons = '<ul class="inline">';
    buttons += '<li><a href="' + invoicesManagerURL + '" class="btn btn-primary btn-medium" title="Lançamentos"><i class="fa fa-money fa-lg"></i></a></li>';
    buttons += '<li><a href="' + accountsManagerURL + '" class="btn btn-primary btn-medium" title="Fluxo"><i class="fa fa-book fa-lg"></i></a></li>';
    buttons += '<li><a href="' + agendaURL + '" class="btn btn-primary btn-medium" title="Agenda"><i class="fa fa-calendar fa-lg"></i></a></li>';
    buttons += '</ul>';
    $('#buttons').html(buttons);

    $('#doneCheckbox').bootstrapSwitch();
    $('#agendaCheckbox').bootstrapSwitch();

    $('#modifiedDateLabel').text(kendo.toString(new Date(), 'd'));

    my.accountsTransport = {
        read: {
            url: '/desktopmodules/riw/api/accounts/getAccounts?portalId=' + portalID
        }
    };

    my.accountsData = new kendo.data.DataSource({
        transport: my.accountsTransport,
        sort: {
            field: 'AccountName',
            dir: 'ASC'
        },
        schema: {
            model: {
                ud: 'AccountId'
            }
        }
    });

    $('#ddlAccounts').kendoComboBox({
        autoBind: false,
        minLength: 3,
        delay: 500,
        placeholder: ' Selecionar ',
        dataTextField: 'AccountName',
        dataValueField: 'AccountId',
        dataSource: my.accountsData,
        //dataBound: function () {
        //    var dataSource = this.dataSource;
        //    var data = dataSource.data();

        //    if (!this._adding) {
        //        this._adding = true;

        //        //if (this.dataSource.view().length > 0) {
        //        //    if (data[0].AccountName.indexOf('Selecionar') !== -1) {
        //        //        data.splice(0, 1);
        //        //    }
        //        //}

        //        //data.splice(0, 0, {
        //        //    "AccountName": " Selecionar ",
        //        //    "AccountId": " "
        //        //});

        //        //OR add it at the and  
        //        /*dataSource.add({
        //            "ProductName": "test",
        //            "ProductID": "10000"
        //        });*/

        //        this._adding = false;
        //    }
        //},
        select: function (e) {
            var dataItem = this.dataItem(e.item.index());
            if (dataItem) {
                if (dataItem.AccountId > 0) {
                    my.vm.selectedAccountId(dataItem.AccountId);
                    my.vm.selectedAccountName(dataItem.AccountName);
                    $('#btnAddAccount').hide();
                    $('#btnRemoveAccount').show();
                    $('#btnUpdateAccount').show();
                    setTimeout(function () {
                        //$('#emissionDate').data('kendoDatePicker').open();
                        $("#vendorSearchBox").select2('open');
                    }, 300);
                } else {
                    my.vm.selectedAccountId(-1);
                    $('#btnRemoveAccount').hide();
                    $('#btnUpdateAccount').hide();
                    $('#btnAddAccount').show();
                }
            }
        }
    });

    my.originsTransport = {
        read: {
            url: '/desktopmodules/riw/api/origins/getOrigins?portalId=' + portalID
        }
    };

    my.originsData = new kendo.data.DataSource({
        transport: my.originsTransport,
        sort: {
            field: 'OriginName',
            dir: 'ASC'
        },
        schema: {
            model: {
                ud: 'OriginId'
            }
        }
    });

    $('#ddlOrigins').kendoComboBox({
        autoBind: false,
        minLength: 3,
        delay: 500,
        placeholder: ' Selecionar ',
        dataTextField: 'OriginName',
        dataValueField: 'OriginId',
        dataSource: my.originsData,
        //dataBound: function () {
        //    var dataSource = this.dataSource;
        //    var data = dataSource.data();

        //    if (!this._adding) {
        //        this._adding = true;

        //        if (this.dataSource.view().length > 0) {
        //            if (data[0].OriginName.indexOf('Selecionar') !== -1) {
        //                data.splice(0, 1);
        //            }
        //        }

        //        data.splice(0, 0, {
        //            "OriginName": " Selecionar ",
        //            "OriginId": " "
        //        });

        //        //OR add it at the and  
        //        /*dataSource.add({
        //            "ProductName": "test",
        //            "ProductID": "10000"
        //        });*/

        //        this._adding = false;
        //    }
        //},
        select: function (e) {
            var dataItem = this.dataItem(e.item.index());
            if (dataItem) {
                if (dataItem.OriginId > 0) {
                    my.vm.selectedOriginId(dataItem.OriginId);
                    my.vm.selectedOriginName(dataItem.OriginName);
                    $('#btnAddOrigin').hide();
                    $('#btnRemoveOrigin').show();
                    $('#btnUpdateOrigin').show();
                } else {
                    $('#btnRemoveOrigin').hide();
                    $('#btnUpdateOrigin').hide();
                    $('#btnAddOrigin').show();
                    my.vm.selectedOriginId(-1);
                }
                setTimeout(function () {
                    //$('#emissionDate').data('kendoDatePicker').open();
                    $("#commentTextarea").focus();
                }, 100);
            }
        }
    });

    $('#ntbDocId').kendoNumericTextBox({
        min: 0,
        format: '#',
        decimal: 0,
        spinners: false
    });

    $('#ntbTransId').kendoNumericTextBox({
        min: 0,
        format: '#',
        decimal: 0,
        spinners: false
    });

    $('#kdpTransDate').kendoDatePicker();

    $('#kdpDueDate').kendoDatePicker();
    
    $("#vendorSearchBox").select2({
        placeholder: "Busque fornecedores por nome próprio ou empresa",
        id: function (data) {
            return {
                PersonId: data.PersonId,
                DisplayName: data.DisplayName
            };
        },
        ajax: {
            url: "/desktopmodules/riw/api/people/getpeople",
            quietMillis: 100,
            data: function (term, page) { // page is the one-based page number tracked by Select2
                return {
                    portalId: portalID,
                    registerType: providerRoleId,
                    isDeleted: false,
                    searchField: 'DisplayName',
                    sTerm: term,
                    pageIndex: page,
                    pageSize: 10,
                    orderBy: 'DisplayName',
                    orderDesc: 'ASC'
                };
            },
            results: function (data, page) {
                var more = (page * 30) < data.total; // whether or not there are more results available

                // notice we return the value of more so Select2 knows if more results can be loaded
                return { results: data.data, more: more };
            }
        },
        formatResult: peopleFormatResult, // omitted for brevity, see the source of this page
        formatSelection: peopleFormatSelection, // omitted for brevity, see the source of this page
        dropdownCssClass: "bigdrop", // apply css that makes the dropdown taller
        escapeMarkup: function (m) { return m; }, // we do not want to escape markup since we are displaying html in results
        initSelection: function (element, callback) {
            var text = element.val();
            var data = { DisplayName: text };
            callback(data);
        }
    });

    $('#vendorSearchBox').on("select2-selecting", function (e) {
        my.vm.selectedVendorId(e.val.PersonId);
    });

    //var vendorsDropDown = $("#vendorSearchBox").kendoComboBox({
    //    delay: 600,
    //    //minLength: 4,
    //    height: 290,
    //    autoBind: false,
    //    highlightFirst: true,
    //    dataTextField: 'DisplayName',
    //    dataValueField: 'PersonId',
    //    placeholder: 'Nome próprio, Empresa, Etc.',
    //    template: '${ data.PersonId } ${ data.DisplayName }',
    //    virtual: {
    //        itemHeight: 26,
    //        valueMapper: function (options) {
    //            $.ajax({
    //                url: "http://demos.telerik.com/kendo-ui/service/Orders/ValueMapper",
    //                type: "GET",
    //                dataType: "jsonp",
    //                data: convertValues(options.value),
    //                success: function (data) {
    //                    options.success(data);
    //                }
    //            })
    //        }
    //    },
    //    dataSource: {
    //        transport: {
    //            read: {
    //                url: '/desktopmodules/riw/api/people/getPeople'
    //            },
    //            parameterMap: function (data, type) {

    //                var fieldName = '';
    //                if (data.filter !== undefined) {
    //                    fieldName = data.filter.filters[0].value
    //                }

    //                return {
    //                    portalId: portalID,
    //                    registerType: providerRoleId,
    //                    sTerm: fieldName,
    //                    searchField: 'DisplayName',
    //                    pageIndex: data.page,
    //                    pageSize: data.pageSize
    //                    //orderBy: data.sort[0] ? data.sort[0].field : 'PersonId',
    //                    //orderDesc: data.sort[0] ? data.sort[0].dir : ''
    //                };
    //            }
    //        },
    //        pageSize: 10,
    //        serverPaging: true,
    //        serverFiltering: true,
    //        sort: {
    //            field: "DisplayName",
    //            dir: "ASC"
    //        },
    //        schema: {
    //            model: {
    //                id: 'PersonId'
    //            },
    //            data: 'data',
    //            total: 'total'
    //        }
    //    },
    //    filter: "startswith",
    //    select: function (e) {
    //        e.preventDefault();
    //        var dataItem = this.dataItem(e.item.index());
    //        if (dataItem) {
    //            this.value(dataItem.DisplayName);
    //            my.vm.selectedVendorId(dataItem.PersonId);
    //        }
    //    }
    //}).data('kendoComboBox');

    //vendorsDropDown.list.width(400);
    
    $("#clientSearchBox").select2({
        placeholder: "Busque clientes por nome próprio ou empresa",
        id: function (data) {
            return {
                PersonId: data.PersonId,
                DisplayName: data.DisplayName
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
                    searchField: 'DisplayName',
                    sTerm: term,
                    pageIndex: page,
                    pageSize: 10,
                    orderBy: 'DisplayName',
                    orderDesc: 'ASC'
                };
            },
            results: function (data, page) {
                var more = (page * 30) < data.total; // whether or not there are more results available

                // notice we return the value of more so Select2 knows if more results can be loaded
                return { results: data.data, more: more };
            }
        },
        formatResult: peopleFormatResult, // omitted for brevity, see the source of this page
        formatSelection: peopleFormatSelection, // omitted for brevity, see the source of this page
        dropdownCssClass: "bigdrop", // apply css that makes the dropdown taller
        escapeMarkup: function (m) { return m; }, // we do not want to escape markup since we are displaying html in results
        initSelection: function (element, callback) {
            var text = element.val();
            var data = { DisplayName: text };
            callback(data);
        }
    });

    $('#clientSearchBox').on("select2-selecting", function (e) {
        my.vm.selectedClientId(e.val.PersonId);
    });

    //var clientsDropDown = $("#clientSearchBox").kendoComboBox({
    //    delay: 600,
    //    //minLength: 4,
    //    autoBind: false,
    //    highlightFirst: true,
    //    dataTextField: 'DisplayName',
    //    dataVlaueField: 'PersonId',
    //    placeholder: 'Nome próprio, Empresa, Etc.',
    //    template: '${ data.PersonId } ${ data.DisplayName }',
    //    virtual: {
    //        itemHeight: 26
    //    },
    //    dataSource: {
    //        transport: {
    //            read: {
    //                url: '/desktopmodules/riw/api/people/getPeople'
    //            },
    //            parameterMap: function (data, type) {

    //                var fieldName = '';
    //                if (data.filter !== undefined) {
    //                    fieldName = data.filter.filters[0].value
    //                }

    //                return {
    //                    portalId: portalID,
    //                    registerType: clientRoleId,
    //                    sTerm: fieldName,
    //                    searchField: 'DisplayName',
    //                    pageIndex: data.page,
    //                    pageSize: data.pageSize
    //                    //orderBy: data.sort[0] ? data.sort[0].field : 'PersonId',
    //                    //orderDesc: data.sort[0] ? data.sort[0].dir : ''
    //                };
    //            }
    //        },
    //        pageSize: 10,
    //        serverPaging: true,
    //        serverFiltering: true,
    //        sort: {
    //            field: "DisplayName",
    //            dir: "ASC"
    //        },
    //        schema: {
    //            model: {
    //                id: 'PersonId'
    //            },
    //            data: 'data',
    //            total: 'total'
    //        }
    //    },
    //    filter: "startswith",
    //    select: function (e) {
    //        e.preventDefault();
    //        var dataItem = this.dataItem(e.item.index());
    //        if (dataItem) {
    //            this.value(dataItem.DisplayName);
    //            my.vm.selectedClientId(dataItem.PersonId);
    //        }
    //    }
    //}).data('kendoComboBox');

    //clientsDropDown.list.width(400);

    if (my.pmtId > 0) {
        $.ajax({
            url: '/desktopmodules/riw/api/invoices/getPayment?pmtId=' + my.pmtId + '&portalId=' + portalID
        }).done(function (pmtData) {
            if (pmtData) {

                $('#kdpTransDate').data('kendoDatePicker').value(new Date(pmtData.TransDate));
                $('#modifiedDateLabel').text(kendo.toString(pmtData.ModifiedDate, 'g'));
                if (pmtData.Debit) {
                    $('#creDebRadio1').prop('checked', false);
                    $('#creDebRadio2').prop('checked', true);
                    $('#ntbOriginalAmount').data('kendoNumericTextBox').enable(true);
                    $('#ntbInterestRate').data('kendoNumericTextBox').enable(true);
                    $('#ntbFee').data('kendoNumericTextBox').enable(true);
                    my.vm.payAmountBox(false);
                    $('#agendaCheckbox').bootstrapSwitch('setState', true);
                    $('#divAgendaCheckBox').show();
                    //$('#agendaCheckbox').nextAll('span').text('Não Gravar');
                    $('#divOriginalAmount').show();
                    $('#divInterestRate').show();
                    $('#divFee').show();
                    $('#divDoc').show();
                    //$('#divDueDate').show();
                    //$('#divDone').show();
                    if (my.vm.selectedVendorId() > 0) {
                        $('#divVendorLabel').show();
                    } else {
                        $('#divVendorTextBox').show();
                    }
                } else if (pmtData.Credit) {
                    $('#creDebRadio1').prop('checked', true);
                    $('#creDebRadio2').prop('checked', false);
                    $('#ntbOriginalAmount').data('kendoNumericTextBox').enable(false);
                    $('#ntbInterestRate').data('kendoNumericTextBox').enable(false);
                    $('#ntbFee').data('kendoNumericTextBox').enable(false);
                    my.vm.payAmountBox(true);
                    $('#agendaCheckbox').bootstrapSwitch('setState', false);
                    $('#divAgendaCheckBox').hide();
                    $('#divOriginalAmount').hide();
                    $('#divInterestRate').hide();
                    $('#divFee').hide();
                    $('#divDoc').hide();
                    //$('#divDueDate').hide();
                    //$('#divDone').hide();
                    $('#divVendorTextBox').hide();
                    $('#divVendorLabel').hide();
                }

                $('#ntbDocId').data('kendoNumericTextBox').value(pmtData.DocId);
                $('#ntbTransId').data('kendoNumericTextBox').value(pmtData.TransId);

                my.vm.selectedAccountId(pmtData.AccountId);
                if (my.vm.selectedAccountId() > 0) {
                    $('#ddlAccounts').data('kendoComboBox').value(my.vm.selectedAccountId());
                    $('#btnAddAccount').hide();
                    $('#btnRemoveAccount').show();
                    $('#btnUpdateAccount').show();
                }

                if (pmtData.ProviderId) {
                    my.vm.selectedVendorId(pmtData.ProviderId);
                    $('#divVendorLabel').show();
                    $('#divVendorTextBox').hide();
                    $('#labelVendorName').text(pmtData.ProviderName);
                } else {
                    $('#divVendorTextBox').show();
                    my.vm.selectedVendorId(null);
                    $('#divVendorLabel').hide();
                }

                if (pmtData.ClientId) {
                    my.vm.selectedClientId(pmtData.ClientId);
                    $('#divClientLabel').show();
                    $('#labelClientName').text(pmtData.ClientName);
                } else {
                    my.vm.selectedClientId(null);
                    $('#divClientTextBox').show();
                }

                my.vm.selectedOriginId(pmtData.OriginId);
                if (my.vm.selectedOriginId() > 0) {
                    $('#ddlOrigins').data('kendoComboBox').value(my.vm.selectedOriginId());
                    $('#btnAddOrigin').hide();
                    $('#btnRemoveOrigin').show();
                    $('#btnUpdateOrigin').show();
                }

                $('#commentTextarea').text(pmtData.Comment);
                $('#kdpDueDate').data('kendoDatePicker').value(new Date(pmtData.DueDate));
                my.vm.originalDueDate(moment(pmtData.DueDate).format())
                $('#doneCheckbox').bootstrapSwitch('setState', pmtData.Done);
                my.vm.interestRate(pmtData.InterestRate);
                my.vm.originalAmount(pmtData.Total);
                $('#agendaCheckbox').bootstrapSwitch('setState', false);
                
            } else {
                //$().toastmessage('showErrorToast', pmtData.Result);
                $.pnotify({
                    title: 'Erro!',
                    text: pmtData.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
    } else {
        $('#divOriginalAmount').show();
        $('#divClientTextBox').show();
        //$('#kdpTransDate').focus();
        $('#divAgendaCheckBox').show();
        $('#agendaCheckbox').bootstrapSwitch('setState', false);
        my.vm.selectedClientId(null);
        my.vm.selectedVendorId(null);
    }

    $('#btnUpdatePayment').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            PortalId: portalID,
            PaymentId: my.pmtId,
            OriginalDueDate: my.vm.originalDueDate(),
            TransId: $('#ntbTransId').data('kendoNumericTextBox').value(),
            DocId: $('#ntbDocId').data('kendoNumericTextBox').value(),
            TransDate: moment($('#kdpTransDate').data('kendoDatePicker').value()).format(),
            Done: $('#doneCheckbox').is(':checked'),
            AccountId: my.vm.selectedAccountId(),
            OriginId: my.vm.selectedOriginId(),
            ProviderId: my.vm.selectedVendorId(),
            ClientId: my.vm.selectedClientId(),
            Comment: $('#commentTextarea').val(),
            DueDate: $('#kdpDueDate').val().length > 0 ? moment($('#kdpDueDate').data('kendoDatePicker').value()).format() : moment($('#kdpTransDate').data('kendoDatePicker').value()).format(),
            Credit: $('#creDebRadio1').is(':checked') ? my.vm.originalAmount() : 0,
            Debit: $('#creDebRadio2').is(':checked') ? my.vm.originalAmount() : 0,
            Agenda: $('#agendaCheckbox').is(':checked'),
            InterestRate: my.vm.interestRate(),
            Fee: my.vm.fee(),
            CreatedByUser: userID,
            CreatedOnDate: moment().format(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/invoices/updatePayment',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                if (data.PaymentId) {
                    //parent.$('#window2').data('kendoWindow').close();

                    if (my.pmtId) {
                        //$().toastmessage('showSuccessToast', 'Notal Fiscal inserida com sucesso!');
                        $.pnotify({
                            title: 'Sucesso!',
                            text: 'Pagamento atualizado.',
                            type: 'success',
                            icon: 'fa fa-check fa-lg',
                            addclass: "stack-bottomright",
                            stack: my.stack_bottomright
                        });
                    } else {
                        //parent.$('#window2').data('kendoWindow').title('Conta ' + $('#ddlAccounts').data('kendoComboBox').text() + ' (ID: ' + my.pmtId + ')');
                        //$().toastmessage('showSuccessToast', 'Lançamento atualizado com sucesso!');
                        $.pnotify({
                            title: 'Sucesso!',
                            text: 'Pagamento inserido.',
                            type: 'success',
                            icon: 'fa fa-check fa-lg',
                            addclass: "stack-bottomright",
                            stack: my.stack_bottomright
                        });
                    }
                    setTimeout(function () {
                        document.location.href = paymentsURL;
                    }, 3000);
                }
            } else {
                //$(btn).removeClass('k-state-disabled').html('<span class="k-icon k-i-tick"></span> Lançar').attr({ 'disabled': false });
                //$().toastmessage('showToast', { text: data.Result, type: 'error', stayTyime: 10000 });
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
            //$(btn).removeClass('k-state-disabled').html('<span class="k-icon k-i-tick"></span> Lançar').attr({ 'disabled': false });
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
    });

    $('#btnAddAccount').click(function (e) {
        e.preventDefault();
        if (e.clientX === 0) {
            return false;
        }
        if ($('#ddlAccounts').data('kendoComboBox').text().indexOf('Selecionar') === -1) {
            var params = {
                PortalId: portalID,
                AccountId: 0,
                AccountName: $('#ddlAccounts').data('kendoComboBox').text(),
                CreatedByUser: userID,
                CreatedOnDate: moment().format(),
                ModifiedByUser: userID,
                ModifiedOnDate: moment().format()
            };

            $.ajax({
                type: 'POST',
                url: '/desktopmodules/riw/api/accounts/UpdateAccount',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    if (data.Account) {
                        my.vm.selectedAccountId(data.Account.AccountId);
                        my.vm.selectedAccountName($('#ddlAccounts').data('kendoComboBox').text());

                        $('#ddlAccounts').data('kendoComboBox').dataSource.read();
                        setTimeout(function () {
                            $('#ddlAccounts').data('kendoComboBox').text(my.vm.selectedAccountName());
                        });

                        //$().toastmessage('showSuccessToast', 'Nova conta inserida com sucesso!');
                        $.pnotify({
                            title: 'Sucesso!',
                            text: 'Nova Origem inserida.',
                            type: 'success',
                            icon: 'fa fa-check fa-lg',
                            addclass: "stack-bottomright",
                            stack: my.stack_bottomright
                        });

                        $('#btnAddAccount').hide();
                        $('#btnRemoveAccount').show();
                        $('#btnUpdateAccount').show();
                    }
                } else {
                    //$().toastmessage('showErrorToast', data.Result);
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

    $('#btnUpdateAccount').click(function (e) {
        e.preventDefault();
        if (e.clientX === 0) {
            return false;
        }
        if (my.vm.selectedAccountId() > 0) {
            var params = {
                PortalId: portalID,
                AccountId: my.vm.selectedAccountId(),
                AccountName: $('#ddlAccounts').data('kendoComboBox').text(),
                ModifiedByUser: userID,
                ModifiedOnDate: moment().format()
            };

            $.ajax({
                type: 'POST',
                url: '/desktopmodules/riw/api/accounts/updateAccount',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    my.vm.selectedAccountName($('#ddlAccounts').data('kendoComboBox').text());

                    $('#ddlAccounts').data('kendoComboBox').dataSource.read();
                    setTimeout(function () {
                        $('#ddlAccounts').data('kendoComboBox').text(my.vm.selectedAccountName());
                    });

                    //$().toastmessage('showSuccessToast', 'Conta atualizada com sucesso!');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Conta atualizada.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                } else {
                    //$().toastmessage('showErrorToast', data.Result);
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

    $('#btnRemoveAccount').click(function (e) {
        var btn = this;
        e.preventDefault();
        if (e.clientX === 0) {
            return false;
        }
        if (my.vm.selectedAccountId() > 0) {
            var $dialog = $('<div></div>')
                    .html('<p class="confirmDialog">Tem certeza que deseja que deseja excluir a conta ' + $('#ddlAccounts').data('kendoComboBox').text() + ' ?</p>')
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

                                    $.ajax({
                                        type: 'DELETE',
                                        url: '/desktopmodules/riw/api/accounts/RemoveAccount?accountId=' + my.vm.selectedAccountId() + '&portalId=' + portalID
                                    }).done(function (data) {
                                        if (data.Result.indexOf("success") !== -1) {
                                            my.vm.selectedAccountId(null);

                                            $('#ddlAccounts').data('kendoComboBox').value(null)
                                            $('#ddlAccounts').data('kendoComboBox').dataSource.read();

                                            $('#btnRemoveAccount').hide();
                                            $('#btnUpdateAccount').hide();
                                            $('#btnAddAccount').show();

                                            //$().toastmessage('showSuccessToast', 'Conta excluida com sucesso!');
                                            $.pnotify({
                                                title: 'Sucesso!',
                                                text: 'Conta excluida.',
                                                type: 'success',
                                                icon: 'fa fa-check fa-lg',
                                                addclass: "stack-bottomright",
                                                stack: my.stack_bottomright
                                            });
                                        } else {
                                            //$().toastmessage('showErrorToast', data.Result);
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
    });

    $('#btnAddVendor').click(function (e) {
        var btn = this;
        e.preventDefault();
        if (e.clientX === 0) {
            return false;
        }

        $("#clientWindow").append("<div id='window'></div>");
        var sContent = clientURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer',
            kendoWindow = parent.$('#window').kendoWindow({
                actions: ["Maximize", "Close"],
                title: 'Novo Cadastro',
                resizable: true,
                modal: true,
                width: '90%',
                height: '80%',
                content: sContent,
                open: function (e) {
                    $("html, body").css("overflow", "hidden");
                    $('#window').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
                },
                close: function (e) {
                    $("html, body").css("overflow", "");
                },
                deactivate: function () {
                    this.destroy();
                }
            });

        kendoWindow.data('kendoWindow').center().open();

        $.ajax({
            url: sContent, success: function (data) {
                kendoWindow.data("kendoWindow").refresh();
            }
        });
    });

    $('#btnAddClient').click(function (e) {
        var btn = this;
        e.preventDefault();
        if (e.clientX === 0) {
            return false;
        }

        $("#clientWindow").append("<div id='window'></div>");
        var sContent = clientURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer',
            kendoWindow = parent.$('#window').kendoWindow({
                actions: ["Maximize", "Close"],
                title: 'Novo Cadastro',
                resizable: true,
                modal: true,
                width: '90%',
                height: '80%',
                content: sContent,
                open: function (e) {
                    $("html, body").css("overflow", "hidden");
                    parent.$('#window').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
                },
                close: function (e) {
                    $("html, body").css("overflow", "");
                },
                deactivate: function () {
                    this.destroy();
                }
            });

        kendoWindow.data('kendoWindow').center().open();

        $.ajax({
            url: sContent, success: function (data) {
                kendoWindow.data("kendoWindow").refresh();
            }
        });
    });

    $('#btnAddOrigin').click(function (e) {
        e.preventDefault();
        if (e.clientX === 0) {
            return false;
        }
        if ($('#ddlOrigins').data('kendoComboBox').text().indexOf('Selecionar') === -1) {
            var params = {
                PortalId: portalID,
                OriginId: 0,
                OriginName: $('#ddlOrigins').data('kendoComboBox').text(),
                CreatedByUser: userID,
                CreatedOnDate: moment().format(),
                ModifiedByUser: userID,
                ModifiedOnDate: moment().format()
            };

            $.ajax({
                type: 'POST',
                url: '/desktopmodules/riw/api/origins/updateOrigin',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    if (data.Origin) {
                        my.vm.selectedOriginId(data.Origin.OriginId);
                        my.vm.selectedOriginName($('#ddlOrigins').data('kendoComboBox').text());

                        $('#ddlOrigins').data('kendoComboBox').dataSource.read();
                        setTimeout(function () {
                            $('#ddlOrigins').data('kendoComboBox').text(my.vm.selectedOriginName());
                        });

                        //$().toastmessage('showSuccessToast', 'Nova conta inserida com sucesso!');
                        $.pnotify({
                            title: 'Sucesso!',
                            text: 'Nova Origem inserida.',
                            type: 'success',
                            icon: 'fa fa-check fa-lg',
                            addclass: "stack-bottomright",
                            stack: my.stack_bottomright
                        });

                        $('#btnAddOrigin').hide();
                        $('#btnRemoveOrigin').show();
                        $('#btnUpdateOrigin').show();
                    }
                } else {
                    //$().toastmessage('showErrorToast', data.Result);
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

    $('#btnUpdateOrigin').click(function (e) {
        e.preventDefault();
        if (e.clientX === 0) {
            return false;
        }
        if (my.vm.selectedOriginId() > 0) {
            var params = {
                PortalId: portalID,
                OriginId: my.vm.selectedOriginId(),
                OriginName: $('#ddlOrigins').data('kendoComboBox').text(),
                ModifiedByUser: userID,
                ModifiedOnDate: moment().format()
            };

            $.ajax({
                type: 'POST',
                url: '/desktopmodules/riw/api/origins/updateOrigin',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    my.vm.selectedOriginName($('#ddlOrigins').data('kendoComboBox').text());

                    $('#ddlOrigins').data('kendoComboBox').dataSource.read();
                    setTimeout(function () {
                        $('#ddlOrigins').data('kendoComboBox').text(my.vm.selectedOriginName());
                    });

                    //$().toastmessage('showSuccessToast', 'Conta atualizada com sucesso!');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Origem atualizada.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                } else {
                    //$().toastmessage('showErrorToast', data.Result);
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

    $('#btnRemoveOrigin').click(function (e) {
        var btn = this;
        e.preventDefault();
        if (e.clientX === 0) {
            return false;
        }
        if (my.vm.selectedOriginId() > 0) {
            var $dialog = $('<div></div>')
                    .html('<p class="confirmDialog">Tem certeza que deseja que deseja excluir a origem ' + $('#ddlOrigins').data('kendoComboBox').text() + ' ?</p>')
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

                                    $.ajax({
                                        type: 'DELETE',
                                        url: '/desktopmodules/riw/api/origins/removeOrigin?originId=' + my.vm.selectedOriginId() + '&portalId=' + portalID
                                    }).done(function (data) {
                                        if (data.Result.indexOf("success") !== -1) {
                                            my.vm.selectedOriginId(null);

                                            $('#ddlOrigins').data('kendoComboBox').value(null)
                                            $('#ddlOrigins').data('kendoComboBox').dataSource.read();

                                            $('#btnRemoveOrigin').hide();
                                            $('#btnUpdateOrigin').hide();
                                            $('#btnAddOrigin').show();

                                            //$().toastmessage('showSuccessToast', 'Conta excluida com sucesso!');
                                            $.pnotify({
                                                title: 'Sucesso!',
                                                text: 'Origem excluida.',
                                                type: 'success',
                                                icon: 'fa fa-check fa-lg',
                                                addclass: "stack-bottomright",
                                                stack: my.stack_bottomright
                                            });
                                        } else {
                                            //$().toastmessage('showErrorToast', data.Result);
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
    });

    $("input").focus(function () {
        var input = $(this);
        setTimeout(function () {
            input.select();
        });
    });

    my.editVendor = function () {
        $('#divVendorTextBox').delay(400).fadeIn();
        $('#divVendorLabel').fadeOut();
        //$('#vendorSearchBox').data('kendoAutoComplete').value($('#labelVendorName').text());
    };

    my.editClient = function () {
        $('#divClientTextBox').delay(400).fadeIn();
        $('#divClientLabel').fadeOut();
        //$('#clientSearchBox').data('kendoAutoComplete').value($('#labelClientName').text());
    };

    $("input[name='optionsRadios']").change(function () {
        if ($(this).val() === '1') {
            $('#ntbOriginalAmount').data('kendoNumericTextBox').enable(false);
            $('#ntbInterestRate').data('kendoNumericTextBox').enable(false);
            $('#ntbFee').data('kendoNumericTextBox').enable(false);
            my.vm.payAmountBox(true);
            $('#agendaCheckbox').bootstrapSwitch('setState', false);
            $('#divAgendaCheckBox').fadeOut();
            $('#divOriginalAmount').fadeOut();
            $('#divInterestRate').fadeOut();
            $('#divFee').fadeOut();
            $('#divDoc').fadeOut();
            //$('#divDueDate').fadeOut();
            //$('#divDone').fadeOut();
            $('#divVendorTextBox').fadeOut();
            $('#divVendorLabel').fadeOut();
        } else if ($(this).val() === '2') {
            $('#ntbOriginalAmount').data('kendoNumericTextBox').enable(true);
            $('#ntbInterestRate').data('kendoNumericTextBox').enable(true);
            $('#ntbFee').data('kendoNumericTextBox').enable(true);
            my.vm.payAmountBox(false);
            $('#agendaCheckbox').bootstrapSwitch('setState', true);
            $('#divAgendaCheckBox').fadeIn();
            //$('#agendaCheckbox').nextAll('span').text('Não Gravar');
            $('#divOriginalAmount').fadeIn();
            $('#divInterestRate').fadeIn();
            $('#divFee').fadeIn();
            $('#divDoc').fadeIn();
            //$('#divDueDate').fadeIn();
            //$('#divDone').fadeIn();
            if (my.vm.selectedVendorId() > 0) {
                $('#divVendorLabel').fadeIn();
            } else {
                $('#divVendorTextBox').fadeIn();
            }
        }
    });

    //$('#agendaCheckbox').change(function () {
    //    if ($(this).is(':checked'))
    //        $(this).next('span').text('Gravar');
    //    else
    //        $(this).next('span').text('Não Gravar');
    //});
});

function peopleFormatResult(data) {
    return data.PersonId + ' ' + data.DisplayName
        + (data.Email.length > 1 ? '<br />' + data.Email : '')
        + (data.Telephone.length > 1 ? '<br />' + data.Telephone : '');
}

function peopleFormatSelection(data) {
    return data.PersonId + ' ' + data.DisplayName;
}