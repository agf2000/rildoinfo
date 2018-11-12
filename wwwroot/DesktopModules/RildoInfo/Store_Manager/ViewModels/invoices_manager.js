
$(function () {

    my.uId = null;

    my.viewModel();

    $('#moduleTitleSkinObject').html(_moduleTitle);

    var buttons = '<ul class="inline">';
    buttons += '<li><a href="' + _categoriesManagerURL + '" class="btn btn-primary btn-medium" title="Categorias"><i class="fa fa-sitemap fa-lg"></i></a></li>';
    buttons += '<li><a href="' + _productsManagerURL + '" class="btn btn-primary btn-medium" title="Produtos"><i class="fa fa-barcode fa-lg"></i></a></li>';
    buttons += '<li><a href="' + _peopleManagerURL + '" class="btn btn-primary btn-medium" title="Entidades"><i class="fa fa-group fa-lg"></i></a></li>';
    buttons += '<li><a href="' + _usersManagerURL + '" class="btn btn-primary btn-medium" title="Colaboradores"><i class="fa fa-suitcase fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + _invoicesManagerURL + '" class="btn btn-primary btn-medium" title="Lançamentos"><i class="fa fa-money fa-lg"></i></a></li>';
    buttons += '<li><a href="' + _accountsManagerURL + '" class="btn btn-primary btn-medium" title="Fluxo"><i class="fa fa-book fa-lg"></i></a></li>';
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

    $('#kdpStartDate').kendoDatePicker();
    $('#kdpEndDate').kendoDatePicker();

    $("#vendorSearchBox").kendoAutoComplete({
        minLength: 4,
        delay: 600,
        highlightFirst: true,
        dataTextField: 'DisplayName',
        placeholder: 'Nome próprio, Empresa, Etc.',
        template: '<strong>Cliente: </strong><span>${ data.DisplayName }</span><br /><strong>Email: </strong><span>${ data.Email }</span><br /><strong>Telefone: </strong><span>${ data.Telephone }</span>',
        dataSource: {
            transport: {
                read: '/desktopmodules/riw/api/people/getPeople?portalId=' + _portalID + '&registerType=' + _providerRoleId
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
                //        text: this.dataSource.total() + ' fornecedores encontrados.',
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
        placeholder: 'Nome próprio, Empresa, Etc.',
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

    // create kendo dataSource transport to get products
    my.invoicesTransport = {
        read: {
            url: '/desktopmodules/riw/api/invoices/GetInvoices'
        },
        parameterMap: function (data, type) {
            return {
                portalId: _portalID,
                providerId: my.vm.selectedVendorId(),
                clientId: my.vm.selectedClientId(),
                sDate: $('#kdpStartDate').val().length > 0 ? moment(new Date($('#kdpStartDate').data('kendoDatePicker').value())).format() : '',
                eDate: $('#kdpEndDate').val().length > 0 ? moment(new Date($('#kdpEndDate').data('kendoDatePicker').value())).format() : '',
                pageNumber: data.page,
                pageSize: data.pageSize,
                orderBy: my.convertSortingParameters(data.sort)
            };
        }
    };

    // create kendo dataSource for getting products transport
    my.invoices = new kendo.data.DataSource({
        transport: my.invoicesTransport,
        pageSize: 10,
        serverPaging: true,
        serverSorting: true,
        serverFiltering: true,
        sort: {
            field: "ModifiedDate",
            dir: "DESC"
        },
        schema: {
            model: {
                id: 'InvoiceId',
                fields: {
                    EmissionDate: {
                        type: "date", format: "{0:dd/MM/yyyy}", editable: false, nullable: false
                    },
                    CreatedDate: {
                        type: "date", format: "{0:dd/MM/yyyy}"
                    },
                    ModifiedDate: {
                        type: "date", format: "{0:dd/MM/yyyy}"
                    }
                }
            },
            data: 'data',
            total: 'total'
        }
    });

    $("#invoicesGrid").kendoGrid({
        dataSource: my.invoices,
        //height: 385,
        selectable: "row",
        change: function () {
            var row = this.select();
            var id = row.data("uid");
            my.uId = id;
            var dataItem = this.dataItem(row);
            if (dataItem) {
                $('#btnRemoveSelected').hide();
                $('#btnEditSelected').attr({ 'disabled': false });

                if (_authorized > 2) {
                    $('#btnRemoveSelected').show();
                } else {
                    $('#btnAddNewInvoice').hide();
                    $('#btnEditSelected').hide();
                }
            }
        },
        toolbar: kendo.template($("#tmplToolbar").html()),
        columns: [
            { field: "InvoiceNumber", title: "Número", width: 90, },
            { field: "DisplayName", title: "Fornecedor" },
            //{ field: "ClientName", title: "Cliente" },
            { field: "InvoiceAmount", title: "Total N. F.", width: 100, template: '#= kendo.toString(InvoiceAmount, "c") #' },
            { field: "EmissionDate", title: "Data da Emissão", type: "date", format: "{0:d}", width: 115 }
        ],
        sortable: true,
        reorderable: true,
        resizable: true,
        scrollable: true,
        pageable: {
            pageSizes: [20, 40, 60],
            refresh: true,
            numeric: false,
            input: true,
            messages: {
                display: "{0} - {1} de {2} Lançamentos",
                empty: "Sem Registro.",
                page: "Página",
                of: "de {0}",
                itemsPerPage: "Lançamentos por página",
                first: "Ir para primeira página",
                previous: "Ir para página anterior",
                next: "Ir para próxima página",
                last: "Ir para última página",
                refresh: "Recarregar"
            }
        },
        dataBound: function () {
            $('#btnEditSelected').attr({ 'disabled': true });
            if (_authorized < 3) {
                $('#btnAddNewInvoice').hide();
                $('#btnEditSelected').hide();
            }
        }
    });

    $("#invoicesGrid").delegate("tbody > tr", "dblclick", function () {
        $('#btnEditSelected').click();
    });

    //function onEndDateSelect(e) {
    //    if ($('#dpEndDate').val().length === 0) {
    //        $('#dpStartDate').val(null);
    //    }
    //    my.products.read();
    //}
    //$('#dpEndDate').data("kendoDatePicker").bind("change", onEndDateSelect);

    $("#btnAddNewInvoice").click(function (e) {
        e.preventDefault();

        document.location.href = _editInvoiceURL;

        //$("#invoiceWindow").append("<div id='window'></div>");
        //var sContent = _editInvoiceURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer',
        //    kendoWindow = $('#window').kendoWindow({
        //        actions: ["Maximize", "Close"],
        //        title: 'Lançamento de Nota Fiscal',
        //        resizable: true,
        //        modal: true,
        //        width: '90%',
        //        height: '80%',
        //        content: sContent,
        //        open: function (e) {
        //            $("html, body").css("overflow", "hidden");
        //            $('#window').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
        //        },
        //        close: function (e) {
        //            $("html, body").css("overflow", "");
        //            my.invoices.read();
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

        var grid = $('#invoicesGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        document.location.href = _editInvoiceURL + '?invoiceId=' + dataItem.InvoiceId;

        //$("#invoiceWindow").append("<div id='window'></div>");
        //var sContent = _editInvoiceURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&invoiceId/' + dataItem.InvoiceId,
        //    kendoWindow = $('#window').kendoWindow({
        //        actions: ["Refresh", "Maximize", "Close"],
        //        title: 'Nota Fiscal ' + dataItem.InvoiceNumber + ' (' + dataItem.DisplayName + ')',
        //        resizable: true,
        //        modal: true,
        //        width: 950,
        //        height: 580,
        //        content: sContent,
        //        open: function (e) {
        //            $("html, body").css("overflow", "hidden");
        //            $('#window').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
        //        },
        //        close: function (e) {
        //            $("html, body").css("overflow", "");
        //            my.invoices.read();
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

    my.removeInvoice = function () {
        var grid = $('#invoicesGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        var kendoWindow = $("<div />").kendoWindow({
            title: "Aviso",
            resizable: false,
            modal: true,
            width: 200
        });

        kendoWindow.data("kendoWindow")
            .content($("#delete-confirmation").html())
            .center().open();

        kendoWindow.find(".delete-confirm,.delete-cancel").click(function () {
            if ($(this).hasClass("delete-confirm")) {

                $.ajax({
                    type: 'DELETE',
                    url: '/desktopmodules/riw/api/invoices/RemoveInvoice?invoiceId=' + dataItem.InvoiceId
                }).done(function (data) {
                    if (data.Result.indexOf("success") !== -1) {
                        my.invoices.read();
                        $('#btnRemoveSelected').hide();
                        //$().toastmessage('showSuccessToast', 'Nota Fiscal excluida com sucesso.');
                        $.pnotify({
                            title: 'Sucesso!',
                            text: 'Nota Fiscal excluida.',
                            type: 'success',
                            icon: 'fa fa-check fa-lg',
                            addclass: "stack-bottomright",
                            stack: my.stack_bottomright
                        });
                    } else {
                        //$().toastmessage('showErrorToast', data.Msg);
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
            kendoWindow.data("kendoWindow").close();
        }).end();
    };

    $('#btnSearch').click(function (e) {
        e.preventDefault();

        my.invoices.read();
    });

    $('#btnRemoveFilter').click(function (e) {
        var btn = this;
        e.preventDefault();
        if (e.clientX === 0) {
            return false;
        }

        my.vm.selectedClientId(-1);
        my.vm.selectedVendorId(-1);
        $('#kdpStartDate').data('kendoDatePicker').value(null);
        $('#kdpEndDate').data('kendoDatePicker').value(null);

        my.invoices.read();
    });

});