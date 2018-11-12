
$(function () {

    my.uId = null;
    var date = new Date(), y = date.getFullYear(), m = date.getMonth();
    var firstDay = new Date(y, m, 1);
    var lastDay = new Date(y, m + 1, 0);
    //var filterDate = null;

    my.viewModel();

    $('#moduleTitleSkinObject').html(_moduleTitle);

    $('#includePaid').bootstrapSwitch();

    var buttons = '<ul class="list-nostyle list-inline">';
    buttons += '<li><a href="' + _categoriesManagerURL + '" class="btn btn-primary btn-medium" title="Categorias"><i class="fa fa-sitemap fa-lg"></i></a></li>';
    buttons += '<li><a href="' + _productsManagerURL + '" class="btn btn-primary btn-medium" title="Produtos"><i class="fa fa-barcode fa-lg"></i></a></li>';
    buttons += '<li><a href="' + _peopleManagerURL + '" class="btn btn-primary btn-medium" title="Entidades"><i class="fa fa-group fa-lg"></i></a></li>';
    buttons += '<li><a href="' + _usersManagerURL + '" class="btn btn-primary btn-medium" title="Colaboradores"><i class="fa fa-suitcase fa-lg"></i></a></li>';
    buttons += '<li><a href="' + _invoicesManagerURL + '" class="btn btn-primary btn-medium" title="Lançamentos"><i class="fa fa-money fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + _accountsManagerURL + '" class="btn btn-primary btn-medium" title="Fluxo"><i class="fa fa-book fa-lg"></i></a></li>';
    buttons += '<li><a href="' + _agendaURL + '" class="btn btn-primary btn-medium" title="Agenda"><i class="fa fa-calendar fa-lg"></i></a></li>';
    buttons += '<li><a href="' + _orURL + '" class="btn btn-primary btn-medium" title="Website"><i class="fa fa-shopping-cart fa-lg"></i></a></li>';
    buttons += '<li><a href="' + _reportsManagerURL + '" class="btn btn-primary btn-medium" title="Relatórios"><i class="fa fa-bar-chart-o fa-lg"></i></a></li>';
    buttons += '<li><a href="' + _storeManagerURL + '" class="btn btn-primary btn-medium" title="Loja"><i class="fa fa-cogs fa-lg"></i></a></li>';
    buttons += '</ul>';
    $('#buttons').html(buttons);

    $('.icon-exclamation-sign').popover({
        delay: {
            show: 500,
            hide: 100
        },
        placement: 'top',
        trigger: 'hover'
    });

    my.accountsTransport = {
        read: {
            url: '/desktopmodules/riw/api/accounts/getAccounts?portalId=' + _portalID
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
        placeholder: 'Ver todas as Contas',
        dataTextField: 'AccountName',
        dataValueField: 'AccountId',
        dataSource: my.accountsData,
        select: function (e) {
            var dataItem = this.dataItem(e.item.index());
            if (dataItem) {
                my.vm.selectedAccountId(dataItem.AccountId);
                my.vm.selectedAccountName(dataItem.AccountName);
                if (dataItem.Locked) {
                    $('#btnRemoveAccount').hide();
                } else {
                    $('#btnRemoveAccount').show();
                }
                $('#btnUpdateAccount').show();
            } else {
                my.vm.selectedAccountId(-1);
                my.vm.selectedAccountName('');
                $('#btnRemoveAccount').hide();
                $('#btnUpdateAccount').hide();
                $('#btnAddAccount').show();
            }
        }
        //open: function (e) {
        //    $.each(my.accountsData.data(), function (i, acc) {
        //        if (acc.AccountId === '') {
        //            my.accountsData.remove(acc);
        //            return false;
        //        }
        //    });
        //    my.accountsData.insert(0, { AccountId: '', AccountName: 'Vêr Todas a Contas' });
        //}
    });

    $('#kdpStartDate').kendoDatePicker({
        value: firstDay,
        change: function () {
            var value = this.value();
            $('#spanStartDate').text(value);
        }
    });
    $('#spanStartDate').text(moment($('#kdpStartDate').data('kendoDatePicker').value()).format('D MMM YYYY'));

    $('#kdpEndDate').kendoDatePicker({
        value: lastDay,
        change: function () {
            var value = this.value();
            $('#spanEndDate').text(value);
        }
    });
    $('#spanEndDate').text(moment($('#kdpEndDate').data('kendoDatePicker').value()).format('D MMM YYYY'));

    $('#ddlCategory').kendoDropDownList({
        //autoBind: false,
        //optionLabel: 'Vêr Créditos e Débitos',
        dataTextField: 'text',
        dataValueField: 'value',
        select: function (e) {
            var dataItem = this.dataItem(e.item.index());
            my.vm.selectedCategory(dataItem.value);
        }
    });

    $("#vendorSearchBox").kendoAutoComplete({
        delay: 600,
        minLength: 4,
        highlightFirst: true,
        dataTextField: 'DisplayName',
        placeholder: 'Nome próprio, Empresa, Etc.',
        template: '<strong>Cliente: </strong><span>${ data.DisplayName }</span><br /><strong>Email: </strong><span>${ data.Email }</span><br /><strong>Telefone: </strong><span>${ data.Telephone }</span>',
        dataSource: {
            transport: {
                read: '/desktopmodules/riw/api/people/getPeople?portalId=' + _portalID + '&registerType=' + _providerRoleId
            },
            serverFiltering: true
        },
        filter: "contains",
        dataBound: function () {
            switch (true) {
                case (this.dataSource.total() === 0):
                    //if (!$('.toast-item-wrapper').length) $().toastmessage('showWarningToast', 'Sua busca não trouxe resultado algum.');
                    $.pnotify({
                        title: 'Aten&#231;&#227;o!',
                        text: 'Sua busca n&#227;o trouxe resultado algum.',
                        type: 'warning',
                        icon: 'fa fa-warning fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    break;
                    //case (this.dataSource.total() > 1):
                    //    //if (!$('.toast-item-wrapper').length) $().toastmessage('showNoticeToast', this.dataSource.total() + ' clientes encontrados.');
                    //    $.pnotify({
                    //        title: 'Aten&#231;&#227;o!',
                    //        text: this.dataSource.total() + ' clientes encontrados.',
                    //        type: 'info',
                    //        icon: 'fa fa-exclamation-circle fa-lg',
                    //        addclass: "stack-bottomright",
                    //        stack: my.stack_bottomright
                    //    });
                    //    break;
                case (this.dataSource.total() > 20):
                    //if (!$('.toast-item-wrapper').length) $().toastmessage('showNoticeToast', 'Dezenas de clientes encontrados... refina sua busca');
                    $.pnotify({
                        title: 'Aten&#231;&#227;o!',
                        text: 'Dezenas de itens encontrados...<br />...refina sua busca.',
                        type: 'info',
                        icon: 'fa fa-exclamation-circle fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    break;
            }
        },
        select: function (e) {
            e.preventDefault();
            var dataItem = this.dataItem(e.item.index());
            if (dataItem) {
                this.value(dataItem.DisplayName);
                my.vm.selectedVendorId(dataItem.PersonId);
            }
        }
    });

    $("#clientSearchBox").kendoAutoComplete({
        minLength: 4,
        delay: 600,
        highlightFirst: true,
        dataTextField: 'DisplayName',
        dataVlaueField: 'PersonId',
        placeholder: 'Nome própio, Empresa, Etc.',
        template: '<strong>Cliente: </strong><span>${ data.DisplayName }</span><br /><strong>Email: </strong><span>${ data.Email }</span><br /><strong>Telefone: </strong><span>${ data.Telephone }</span>',
        dataSource: {
            transport: {
                read: '/desktopmodules/riw/api/people/getPeople?portalId=' + _portalID + '&registerType=' + _clientRoleId
            },
            pageSize: 1000,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            sort: {
                field: "DisplayName",
                dir: "ASC"
            },
            schema: {
                model: {
                    id: 'PersonId'
                },
                data: 'data',
                total: 'total'
            }
        },
        filter: "contains",
        dataBound: function () {
            switch (true) {
                case (this.dataSource.total() === 0):
                    //if (!$('.toast-item-wrapper').length) $().toastmessage('showWarningToast', 'Sua busca não trouxe resultado algum.');
                    $.pnotify({
                        title: 'Aten&#231;&#227;o!',
                        text: 'Sua busca n&#227;o trouxe resultado algum.',
                        type: 'warning',
                        icon: 'fa fa-warning fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    break;
                    //case (this.dataSource.total() > 1):
                    //    //if (!$('.toast-item-wrapper').length) $().toastmessage('showNoticeToast', this.dataSource.total() + ' clientes encontrados.');
                    //    $.pnotify({
                    //        title: 'Aten&#231;&#227;o!',
                    //        text: this.dataSource.total() + ' clientes encontrados.',
                    //        type: 'info',
                    //        icon: 'fa fa-exclamation-circle fa-lg',
                    //        addclass: "stack-bottomright",
                    //        stack: my.stack_bottomright
                    //    });
                    //    break;
                case (this.dataSource.total() > 20):
                    //if (!$('.toast-item-wrapper').length) $().toastmessage('showNoticeToast', 'Dezenas de clientes encontrados... refina sua busca');
                    $.pnotify({
                        title: 'Aten&#231;&#227;o!',
                        text: 'Dezenas de itens encontrados...<br />...refina sua busca.',
                        type: 'info',
                        icon: 'fa fa-exclamation-circle fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    break;
            }
        },
        select: function (e) {
            e.preventDefault();
            var dataItem = this.dataItem(e.item.index());
            if (dataItem) {
                this.value(dataItem.DisplayName);
                my.vm.selectedClientId(dataItem.PersonId);
            }
        }
    });

    $('#ddlDates').kendoDropDownList();

    // create kendo dataSource transport to get products
    my.paymentsTransport = {
        read: {
            url: '/desktopmodules/riw/api/invoices/GetPayments'
        },
        parameterMap: function (data, type) {

            if (amplify.store(_siteURL + '_filters')) {

                if (!$('#collapseFilter').hasClass('in')) {
                    $('#collapseFilter').addClass('in');
                }

                if (amplify.store(_siteURL + '_accountId')) {
                    $('#ddlAccounts').data('kendoComboBox').value(kendo.parseInt(amplify.store(_siteURL + '_accountId')));
                    my.vm.selectedAccountId(amplify.store(_siteURL + '_accountId'));
                }

                if (amplify.store(_siteURL + '_categoryId')) {
                    $('#ddlCategory').data('kendoDropDownList').value(kendo.parseInt(amplify.store(_siteURL + '_categoryId')));
                    my.vm.selectedCategory(amplify.store(_siteURL + '_categoryId'));
                }

                if (amplify.store(_siteURL + '_done')) {
                    $('#includePaid').bootstrapSwitch('setState', amplify.store(_siteURL + '_done'));
                }

                if (amplify.store(_siteURL + '_dateFilter')) {
                    $('#ddlDates').data('kendoDropDownList').value(amplify.store(_siteURL + '_dateFilter'));
                }

                if (amplify.store(_siteURL + '_iDate')) {
                    $('#kdpStartDate').data('kendoDatePicker').value(new Date(amplify.store(_siteURL + '_iDate')));
                }

                if (amplify.store(_siteURL + '_fDate')) {
                    $('#kdpEndDate').data('kendoDatePicker').value(new Date(amplify.store(_siteURL + '_fDate')));
                }

                if (amplify.store(_siteURL + '_providerId')) {
                    $("#vendorSearchBox").data('kendoAutoComplete').value(kendo.parseInt(amplify.store(_siteURL + '_providerId')));
                    my.vm.selectedVendorId(amplify.store(_siteURL + '_providerId'));
                }

                if (amplify.store(_siteURL + '_clientId')) {
                    $("#clientSearchBox").data('kendoAutoComplete').value(kendo.parseInt(amplify.store(_siteURL + '_clientId')));
                    my.vm.selectedClientId(amplify.store(_siteURL + '_clientId'));
                }

                if (amplify.store(_siteURL + '_keyword')) {
                    $("#txtBoxSearch").text(amplify.store(_siteURL + '_keyword'));
                    // my.vm.filter(amplify.store(_siteURL + '_keyword'));
                }
            }
            return {
                portalId: _portalID,
                accountId: amplify.store(_siteURL + '_accountId') || -1,
                originId: -1,
                providerId: amplify.store(_siteURL + '_providerId') || -1,
                clientId: amplify.store(_siteURL + '_clientId') || -1,
                category: amplify.store(_siteURL + '_categoryId') || '""',
                done: $('#includePaid').is(':checked') ? '""' : 'False',
                sTerm: $('#txtBoxSearch').text().length ? $('#txtBoxSearch').text() : '""', // my.vm.filter().length ? my.vm.filter() : '""',
                sDate: $('#kdpStartDate').val() ? moment($('#kdpStartDate').data('kendoDatePicker').value()).format() : null,
                eDate: $('#kdpEndDate').val() ? moment($('#kdpEndDate').data('kendoDatePicker').value().setHours(23, 59, 00, 0)).format() : null,
                filterDate: $('#ddlDates').data('kendoDropDownList').value(),
                pageNumber: data.page,
                pageSize: data.pageSize,
                orderBy: my.convertSortingParameters(data.sort)
            };
        }
    };

    // create kendo dataSource for getting products transport
    my.paymentsData = new kendo.data.DataSource({
        transport: my.paymentsTransport,
        //aggregate: [
        //    {
        //        field: "Credit", aggregate: "sum"
        //    },
        //    {
        //        field: "Debit", aggregate: "sum"
        //    },
        //    {
        //        field: "Balance", aggregate: "sum"
        //    }
        //],
        pageSize: 8,
        serverPaging: true,
        serverSorting: true,
        serverFiltering: true,
        sort: [
            {
                field: "ModifiedOnDate",
                dir: "ASC"
            }
        ],
        schema: {
            model: {
                id: 'PaymentId',
                fields: {
                    DueDate: {
                        type: "date", format: "{0:d}"
                    },
                    CreatedOnDate: {
                        type: "date", format: "{0:d}"
                    },
                    TransDate: {
                        type: "date", format: "{0:d}" //, editable: false, nullable: false
                    },
                    CreatedDate: {
                        type: "date", format: "{0:d}"
                    },
                    ModifiedDate: {
                        type: "date", format: "{0:d}"
                    },
                    Credit: {
                        type: "number"
                    },
                    Debit: {
                        type: "number"
                    },
                    Balance: {
                        type: "number"
                    }
                }
            },
            data: 'data',
            total: 'total'
        }
    });

    $("#paymentsGrid").kendoGrid({
        dataSource: my.paymentsData,
        selectable: "row",
        change: function () {
            var row = this.select();
            var id = row.data("uid");
            my.uId = id;
            var dataItem = this.dataItem(row);
            if (dataItem) {
                if (_authorized > 2) {
                    if (dataItem.IsDeleted) {
                        $('#btnDeleteSelected').hide();
                        $('#btnRestoreSelected').show();
                    } else {
                        $('#btnDeleteSelected').show();
                        $('#btnRestoreSelected').hide();
                    }
                } else {
                    $('#btnDeleteSelected').hide();
                    $('#btnRestoreSelected').hide();
                }
            }
        },
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
                field: 'Date', title: "Data", type: "date", format: "{0:d}", width: 80
            },
            //{
            //    field: "VendorName", title: "Fornecedor"
            //},
            //{
            //    field: "ClientName", title: "Cliente"
            //},
            {
                field: "Comment", title: "Descrição", template: '<div>#= Comment #</div>'
            },
            {
                field: "Credit", width: 100, headerTemplate: 'Crédito (R$)', 'class': 'right', template: '<div class="text-right">#= Credit > 0 ? kendo.toString(Credit, "n") : "" #</div>'
            },
            {
                field: "Debit", width: 100, headerTemplate: 'Débito (R$)', 'class': 'right', template: '<div class="text-right">#= Debit > 0 ? kendo.toString(Debit, "n") : "" #</div>'
            },
            //{
            //    field: "Balance", width: 90, headerTemplate: '<div class="text-right">Saldo (R$)</div>', template: '<div class="text-right">#= kendo.toString(Balance, "n") #</div>'
            //}
        ],
        sortable: true,
        //reorderable: true,
        //resizable: true,
        pageable: {
            pageSizes: [8, 20, 60, 100, 200],
            refresh: true,
            numeric: false,
            input: true,
            messages: {
                display: "{0} - {1} de {2} Itens",
                empty: "Sem Registro.",
                page: "Página",
                of: "de {0}",
                itemsPerPage: "Itens por página",
                first: "Ir para primeira página",
                previous: "Ir para página anterior",
                next: "Ir para próxima página",
                last: "Ir para última página",
                refresh: "Recarregar"
            }
        },
        dataBound: function () {
            var grid = this;
            if (_authorized > 2) {
                $('#btnAddNewPayment').show();
                //$('#btnEditSelected').show();
            }

            var totalBal = 0;
            //bal = 0;
            $.each(this.dataSource.view(), function (i, item) {
                //bal = totalBal + item.Credit;
                //if (item.Debit > 0 && item.Done === false) {
                //    bal = totalBal - item.Debit;

                if (item.IsDeleted) {
                    totalBal = totalBal + item.Credit;
                } else {
                    totalBal = totalBal + item.Credit - item.Debit;
                }
                //}
                item.set('Balance', totalBal);

                var rowSelector = ">tr:nth-child(" + (i + 1) + ")";
                //Grab a reference to the corrosponding data row
                var row = grid.tbody.find(rowSelector);
                if (item.Done) {
                    row.addClass('isDone');
                }

                if (!item.IsDeleted && !item.Done && item.Debit > 0) {
                    if (moment(item.DueDate).format() < moment(new Date()).format()) {
                        row.addClass('isPastDue');
                    }
                }

                if (item.IsDeleted) {
                    row.addClass('isCanceled');
                }

                switch ($('#ddlDates').data('kendoDropDownList').value()) {
                    case '2':
                        item.set('Date', item.ModifiedOnDate);
                        break;
                    case '3':
                        item.set('Date', item.CreatedOnDate);
                        break;
                    default:
                        item.set('Date', item.DueDate);
                        break;
                }
            });

            //$('#balanceLabel').html('<strong>Saldo:</strong> ' + '<span class="' + (totalBal < 0 ? 'NormalRed' : 'NormalBold') + '">' + kendo.toString(totalBal, 'c') + '</span>')
        }
    });

    $("#paymentsGrid").delegate("tbody > tr", "dblclick", function () {
        $('#btnEditSelected').click();
    });

    //bind click event to the checkbox
    $('#paymentsGrid').data('kendoGrid').table.on("click", ".checkbox", selectRow);

    my.checkedIds = {};

    //on click of the checkbox:
    function selectRow() {
        var checked = this.checked,
            row = $(this).closest("tr"),
            grid = $('#paymentsGrid').data('kendoGrid'),
            dataItem = grid.dataItem(row);

        if (checked) {
            my.checkedIds[dataItem.id] = {
                PaymentId: dataItem.PaymentId
            };

            //-select the row
            row.addClass("k-state-selected");
        } else {
            //-remove selection
            row.removeClass("k-state-selected");
            delete my.checkedIds[dataItem.id];
        }
    };

    $("#btnAddNewPayment").click(function (e) {
        e.preventDefault();

        document.location.href = _editPaymentURL;

        //$("#paymentWindow").append("<div id='window2'></div>");
        //var sContent = _editPaymentURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer',
        //    kendoWindow = $('#window2').kendoWindow({
        //        actions: ["Maximize", "Close"],
        //        title: 'Novo Lançamento de Conta a Pagar / Receber',
        //        resizable: true,
        //        modal: true,
        //        width: 950,
        //        height: 290,
        //        content: sContent,
        //        open: function (e) {
        //            $("html, body").css("overflow", "hidden");
        //            $('#window2').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
        //        },
        //        close: function (e) {
        //            $("html, body").css("overflow", "");
        //            my.getBalance();
        //            my.paymentsData.read();
        //        },
        //        deactivate: function () {
        //            this.destroy();
        //        }
        //    });

        //kendoWindow.data('kendoWindow').center().open();

        //$.ajax({
        //    url: sContent, success: function (data) {
        //        kendoWindow.data("kendoWindow").refresh();
        //    }
        //});
    });

    $('#btnEditSelected').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var grid = $('#paymentsGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        document.location.href = _editPaymentURL + '?pmtId=' + dataItem.PaymentId;

        //$("#paymentWindow").append("<div id='window2'></div>");
        //var sContent = _editPaymentURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&pmtId/' + dataItem.PaymentId,
        //    kendoWindow = $('#window2').kendoWindow({
        //        actions: ["Refresh", "Maximize", "Close"],
        //        title: 'ID: ' + dataItem.PaymentId + (dataItem.Credit ? ' (Conta a Receber)' : ' (Conta a Pagar via ' + dataItem.AccountName + ')'),
        //        resizable: true,
        //        modal: true,
        //        width: 950,
        //        height: 290,
        //        content: sContent,
        //        open: function (e) {
        //            $("html, body").css("overflow", "hidden");
        //            $('#window2').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
        //        },
        //        close: function (e) {
        //            $("html, body").css("overflow", "");
        //            my.getBalance();
        //            my.paymentsData.read();
        //        },
        //        deactivate: function () {
        //            this.destroy();
        //        }
        //    });

        //kendoWindow.data("kendoWindow").center().open();

        //$.ajax({
        //    url: sContent, success: function (data) {
        //        kendoWindow.data("kendoWindow").refresh();
        //    }
        //});
    });

    my.deletePayment = function () {
        var grid = $('#paymentsGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        var $dialog = $('<div></div>')
                        .html('<div class="confirmDialog">Tem Certeza?</div>')
                        .dialog({
                            autoOpen: false,
                            modal: true,
                            resizable: false,
                            dialogClass: 'dnnFormPopup',
                            open: function () {
                                $(".ui-dialog-title").append('Aviso de Cancelamento');
                            },
                            buttons: {
                                'ok': {
                                    text: 'Sim',
                                    //priority: 'primary',
                                    "class": 'dnnPrimaryAction',
                                    click: function () {

                                        $dialog.dialog('close');
                                        $dialog.dialog('destroy');

                                        var payments = [];
                                        $.each(my.checkedIds, function (i, item) {
                                            payments.push({
                                                PaymentId: kendo.parseInt(i)
                                            });
                                        });

                                        $.ajax({
                                            type: 'PUT',
                                            contentType: 'application/json; charset=utf-8',
                                            url: '/desktopmodules/riw/api/invoices/removePayments',
                                            data: JSON.stringify({
                                                Payments: payments,
                                                PortalId: _portalID
                                            })
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                my.paymentsData.read();
                                                $('#btnDeleteSelected').hide();
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Item(ns) cancelado.',
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
                                        });
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
    };

    my.restorePayment = function () {
        var grid = $('#paymentsGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        var params = {
            PortalId: _portalID,
            PaymentId: dataItem.PaymentId,
            ModifiedByUser: _userID,
            ModifiedOnDate: moment().format()
        };

        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/invoices/deletePayment',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.paymentsData.read();
                $('#btnDeleteSelected').hide();
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Item cancelado.',
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
        });
    };

    $('#btnSearch').click(function (e) {
        e.preventDefault();

        if (my.storage) {
            amplify.store(_siteURL + '_filters', true);
            amplify.store(_siteURL + '_accountId', $('#ddlAccounts').data('kendoComboBox').value());
            amplify.store(_siteURL + '_categoryId', $('#ddlCategory').data('kendoDropDownList').value() || "");
            amplify.store(_siteURL + '_done', $('#includePaid').is(':checked'));
            amplify.store(_siteURL + '_dateFilter', $('#ddlDates').data('kendoDropDownList').value());
            amplify.store(_siteURL + '_iDate', $('#kdpStartDate').data('kendoDatePicker').value());
            amplify.store(_siteURL + '_fDate', $('#kdpEndDate').data('kendoDatePicker').value());
            amplify.store(_siteURL + '_providerId', $("#vendorSearchBox").data('kendoAutoComplete').value());
            amplify.store(_siteURL + '_clientId', $("#clientSearchBox").data('kendoAutoComplete').value());
            amplify.store(_siteURL + '_keyword', $("#txtBoxSearch").text());
        }

        my.paymentsData.read();
        my.getBalance();
    });

    $('#btnCalendar').click(function (e) {
        e.preventDefault();

        $("#paymentWindow").append("<div id='window'></div>");
        var sContent = _calendarURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer',
            kendoWindow = $('#window').kendoWindow({
                actions: ["Refresh", "Maximize", "Close"],
                title: 'Calendário',
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

    $('#btnRemoveFilter').click(function (e) {
        var btn = this;
        e.preventDefault();
        if (e.clientX === 0) {
            return false;
        }

        $('#ddlCategory').data('kendoDropDownList').value(null);
        $('#ddlAccounts').data('kendoComboBox').value(null);

        my.vm.selectedAccountId(-1);
        my.vm.selectedAccountName('');
        my.vm.selectedCategory(null);
        my.vm.selectedClientId(-1);
        my.vm.selectedVendorId(-1);
        //my.vm.filter('');
        $('#txtBoxSearch').val('');
        $('#kdpStartDate').data('kendoDatePicker').value(firstDay);
        $('#kdpEndDate').data('kendoDatePicker').value(lastDay);
        $('#includePaid').attr({ 'checked': true });

        if (my.storage) {
            amplify.store(_siteURL + '_filters', false);
            amplify.store(_siteURL + '_accountId', null);
            amplify.store(_siteURL + '_categoryId', "");
            amplify.store(_siteURL + '_done', true);
            amplify.store(_siteURL + '_dateFilter', null);
            amplify.store(_siteURL + '_iDate', firstDay);
            amplify.store(_siteURL + '_fDate', lastDay);
            amplify.store(_siteURL + '_providerId', null);
            amplify.store(_siteURL + '_clientId', null);
            amplify.store(_siteURL + '_keyword', null);
        }

        my.paymentsData.read();
        my.getBalance();
    });

    my.getBalance = function () {
        var params = {
            portalId: _portalID,
            accountId: $('#ddlAccounts').data('kendoComboBox').value() || -1,
            sDate: moment($('#kdpStartDate').data('kendoDatePicker').value()).format(),
            eDate: moment($('#kdpEndDate').data('kendoDatePicker').value()).format(),
            filterDate: $('#ddlDates').data('kendoDropDownList').value()
        };

        $.getJSON('/desktopmodules/riw/api/accounts/getAccountsBalance', params, function (data) {
            // $('#balanceLabel').html('<strong>Saldo:</strong> ' + '<span class="' + (data.Balance < 0 ? 'NormalRed' : 'NormalBold') + '">' + kendo.toString(data.Balance, 'c') + '</span>');
            $('#balanceCreditLabel').html('<strong>Saldo:</strong> ' + '<span class="' + (data.Balance < 0 ? 'NormalRed' : 'NormalBold') + '">' + kendo.toString(data.Balance, 'c') + '</span>');
            $('#balanceDebitLabel').html('<strong>Débito:</strong> ' + '<span class="' + (data.Debit4Seen > 0 ? 'NormalRed' : 'NormalBold') + '">' + kendo.toString(data.Debit4Seen, 'c') + '</span>');
            $('#columnCredit').html('<strong>Crédito:</strong> ' + kendo.toString(data.CreditBalance, 'c'));
            $('#columnDebit').html('<strong>Débito:</strong> ' + kendo.toString(data.DebitBalance, 'c'));
            $('#columnBalance').html('<strong>Diferença:</strong> ' + '<span class="' + (data.Balance < 0 ? 'NormalRed' : '') + '">' + kendo.toString(data.TotalBalance, 'c') + '</span>');
            if (my.vm.selectedCategory() > 0) {
                if (my.vm.selectedCategory() === 1) {
                    $('#balanceCreditLabel').show();
                    $('#balanceDebitLabel').hide();
                    $('#columnCredit').show();
                    $('#columnDebit').hide();
                    $('#columnBalance').hide();
                } else {
                    $('#balanceDebitLabel').show();
                    $('#balanceCreditLabel').hide();
                    $('#columnCredit').hide();
                    $('#columnDebit').show();
                    $('#columnBalance').hide();
                }
            } else {
                $('#balanceDebitLabel').show();
                $('#balanceCreditLabel').show();
            }
        });
    };

    my.getBalance();

    //my.removePayment = function () {
    //    var grid = $('#paymentsGrid').data("kendoGrid");
    //    var dataItem = grid.dataSource.getByUid(my.uId);

    //    var kendoWindow = $("<div />").kendoWindow({
    //        title: "Aviso",
    //        resizable: false,
    //        modal: true,
    //        width: 200
    //    });

    //    kendoWindow.data("kendoWindow")
    //        .content($("#delete-confirmation").html())
    //        .center().open();

    //    kendoWindow.find(".delete-confirm,.delete-cancel").click(function () {
    //        if ($(this).hasClass("delete-confirm")) {

    //            $.ajax({
    //                type: 'DELETE',
    //                url: '/desktopmodules/riw/api/invoices/RemovePayment?pmtId=' + dataItem.PaymentId
    //            }).done(function (data) {
    //                if (data.Result.indexOf("success") !== -1) {
    //                    my.paymentsData.read();
    //                    $('#btnRemoveSelected').hide();
    //                    $().toastmessage('showSuccessToast', 'Nota Fiscal excluida com sucesso.');
    //                } else {
    //                    $().toastmessage('showErrorToast', data.Msg);
    //                }
    //            }).fail(function (jqXHR, textStatus) {
    //                console.log(jqXHR.responseText);
    //            });
    //        }
    //        kendoWindow.data("kendoWindow").close();
    //    }).end();
    //};

    //$('#btnAddAccount').click(function (e) {
    //    var btn = this;
    //    e.preventDefault();
    //    if (e.clientX === 0) {
    //        return false;
    //    }

    //    var params = {
    //        portalId: _portalID,
    //        accName: $('#ddlAccounts').data('kendoComboBox').text(),
    //        uId: _userID,
    //        cd: kendo.toString(new Date(), 'u')
    //    };

    //    $.ajax({
    //        type: 'POST',
    //        url: '/desktopmodules/riw/api/accounts/UpdateAccount',
    //        data: params
    //    }).done(function (data) {
    //        if (data.Result.indexOf("success") !== -1) {
    //            if (data.AccountId) {
    //                my.vm.selectedAccountId(data.AccountId);
    //                //$().toastmessage('showSuccessToast', 'Nova conta inserida com sucesso!');
    //                $.pnotify({
    //                    title: 'Sucesso!',
    //                    text: 'Nova Conta inserida.',
    //                    type: 'success',
    //                    icon: 'fa fa-check fa-lg',
    //                    addclass: "stack-bottomright",
    //                    stack: my.stack_bottomright
    //                });
    //            }
    //        } else {
    //            //$().toastmessage('showErrorToast', data.Result);
    //            $.pnotify({
    //                title: 'Erro!',
    //                text: data.Result,
    //                type: 'error',
    //                icon: 'fa fa-times-circle fa-lg',
    //                addclass: "stack-bottomright",
    //                stack: my.stack_bottomright
    //            });
    //        }
    //    }).fail(function (jqXHR, textStatus) {
    //        console.log(jqXHR.responseText);
    //    });
    //});

    //$('#btnUpdateAccount').click(function (e) {
    //    var btn = this;
    //    e.preventDefault();
    //    if (e.clientX === 0) {
    //        return false;
    //    }

    //    var params = {
    //        portalId: _portalID,
    //        accId: my.vm.selectedAccountId(),
    //        accName: $('#ddlAccounts').data('kendoComboBox').text(),
    //        uId: _userID,
    //        cd: kendo.toString(new Date(), 'u')
    //    };

    //    $.ajax({
    //        type: 'POST',
    //        url: '/desktopmodules/riw/api/accounts/UpdateAccount',
    //        data: params
    //    }).done(function (data) {
    //        if (data.Result.indexOf("success") !== -1) {
    //            //$().toastmessage('showSuccessToast', 'Conta atualizada com sucesso!');
    //            $.pnotify({
    //                title: 'Sucesso!',
    //                text: 'Conta atualizada.',
    //                type: 'success',
    //                icon: 'fa fa-check fa-lg',
    //                addclass: "stack-bottomright",
    //                stack: my.stack_bottomright
    //            });
    //        } else {
    //            //$().toastmessage('showErrorToast', data.Result);
    //            $.pnotify({
    //                title: 'Erro!',
    //                text: data.Result,
    //                type: 'error',
    //                icon: 'fa fa-times-circle fa-lg',
    //                addclass: "stack-bottomright",
    //                stack: my.stack_bottomright
    //            });
    //        }
    //    }).fail(function (jqXHR, textStatus) {
    //        console.log(jqXHR.responseText);
    //    });
    //});

    //$('#btnRemoveAccount').click(function (e) {
    //    var btn = this;
    //    e.preventDefault();
    //    if (e.clientX === 0) {
    //        return false;
    //    }

    //    var kendoWindow = $("<div />").kendoWindow({
    //        title: "Aviso",
    //        resizable: false,
    //        modal: true,
    //        width: 200
    //    });

    //    kendoWindow.data("kendoWindow")
    //        .content($("#delete-confirmation").html())
    //        .center().open();

    //    kendoWindow.find(".delete-confirm,.delete-cancel").click(function () {
    //        if ($(this).hasClass("delete-confirm")) {

    //            $.ajax({
    //                type: 'DELETE',
    //                url: '/desktopmodules/riw/api/accounts/RemoveAccount?accId=' + my.vm.selectedAccountId()
    //            }).done(function (data) {
    //                if (data.Result.indexOf("success") !== -1) {
    //                    my.vm.selectedAccountId(-1);
    //                    $('#ddlAccounts').data('kendoComboBox').value(null);
    //                    $('#btnRemoveAccount').hide();
    //                    $('#btnUpdateAccount').html('<span class="k-icon k-i-plus"></span>');
    //                    //$().toastmessage('showSuccessToast', 'Conta excluida com sucesso!');
    //                    $.pnotify({
    //                        title: 'Sucesso!',
    //                        text: 'Conta excluida.',
    //                        type: 'success',
    //                        icon: 'fa fa-check fa-lg',
    //                        addclass: "stack-bottomright",
    //                        stack: my.stack_bottomright
    //                    });
    //                } else {
    //                    //$().toastmessage('showErrorToast', data.Result);
    //                    $.pnotify({
    //                        title: 'Erro!',
    //                        text: data.Result,
    //                        type: 'error',
    //                        icon: 'fa fa-times-circle fa-lg',
    //                        addclass: "stack-bottomright",
    //                        stack: my.stack_bottomright
    //                    });
    //                }
    //            }).fail(function (jqXHR, textStatus) {
    //                console.log(jqXHR.responseText);
    //            });
    //        }
    //        kendoWindow.data("kendoWindow").close();
    //    }).end();
    //});

    //$('#btnAddVendor').click(function (e) {
    //    var btn = this;
    //    e.preventDefault();
    //    if (e.clientX === 0) {
    //        return false;
    //    }

    //    $("#paymentWindow").append("<div id='window'></div>");
    //    var sContent = _clientURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer',
    //        kendoWindow = $('#window').kendoWindow({
    //            actions: ["Maximize", "Close"],
    //            title: 'Novo Cadastro',
    //            resizable: true,
    //            modal: true,
    //            width: '90%',
    //            height: '80%',
    //            content: sContent,
    //            open: function (e) {
    //                $("html, body").css("overflow", "hidden");
    //                $('#window').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
    //            },
    //            close: function (e) {
    //                $("html, body").css("overflow", "");
    //            },
    //            deactivate: function () {
    //                this.destroy();
    //            }
    //        });

    //    kendoWindow.data('kendoWindow').center().open();

    //    $.ajax({
    //        url: sContent, success: function (data) {
    //            kendoWindow.data("kendoWindow").refresh();
    //        }
    //    });
    //});

    //$('#btnAddClient').click(function (e) {
    //    var btn = this;
    //    e.preventDefault();
    //    if (e.clientX === 0) {
    //        return false;
    //    }

    //    $("#paymentWindow").append("<div id='window'></div>");
    //    var sContent = _clientURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer',
    //        kendoWindow = $('#window').kendoWindow({
    //            actions: ["Maximize", "Close"],
    //            title: 'Novo Cadastro',
    //            resizable: true,
    //            modal: true,
    //            width: '90%',
    //            height: '80%',
    //            content: sContent,
    //            open: function (e) {
    //                $("html, body").css("overflow", "hidden");
    //                $('#window').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
    //            },
    //            close: function (e) {
    //                $("html, body").css("overflow", "");
    //            },
    //            deactivate: function () {
    //                this.destroy();
    //            }
    //        });

    //    kendoWindow.data('kendoWindow').center().open();

    //    $.ajax({
    //        url: sContent, success: function (data) {
    //            kendoWindow.data("kendoWindow").refresh();
    //        }
    //    });
    //});

});