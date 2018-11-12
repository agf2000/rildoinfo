
$(function () {

    my.invoiceId = my.getQuerystring('invoiceId', my.getParameterByName('invoiceId'));
    my.total = 0;

    my.viewModel();

    $('#moduleTitleSkinObject').html(moduleTitle);

    var buttons = '<ul class="inline">';
    buttons += '<li><a href="' + invoicesManagerURL + '" class="btn btn-primary btn-medium" title="Lançamentos"><i class="fa fa-money fa-lg"></i></a></li>';
    buttons += '<li><a href="' + accountsManagerURL + '" class="btn btn-primary btn-medium" title="Fluxo"><i class="fa fa-book fa-lg"></i></a></li>';
    buttons += '<li><a href="' + agendaURL + '" class="btn btn-primary btn-medium" title="Agenda"><i class="fa fa-calendar fa-lg"></i></a></li>';
    buttons += '</ul>';
    $('#buttons').html(buttons);

    $('#ntbInvoiceNumber').kendoNumericTextBox({
        min: 0,
        format: '#',
        decimal: 0,
        spinners: false
    });

    //$('#ntbInvoiceTotal').kendoNumericTextBox({
    //    min: 0,
    //    format: 'c',
    //    spinners: false
    //});

    $('#ntbPayQty').kendoNumericTextBox({
        min: 0,
        format: '#',
        decimals: 0,
        spinners: false
    });

    $('#ntbInterval').kendoNumericTextBox({
        min: 0,
        format: '#',
        decimals: 0,
        spinners: false
    });

    $('#ntbInterestRate').kendoNumericTextBox({
        min: 0,
        format: '##.00 "%"',
        spinners: false
    });

    $('#ntbQty').kendoNumericTextBox({
        min: 1,
        format: 'n',
        spinners: false
    });

    $('#ntbValue1').kendoNumericTextBox({
        min: 0,
        format: 'c',
        spinners: false
    });

    $('#ntbValue2').kendoNumericTextBox({
        min: 0,
        format: 'c',
        spinners: false
    });

    $('#ntbFreight').kendoNumericTextBox({
        min: 0,
        format: 'c',
        spinners: false
    });

    $('#ntbPayIn').kendoNumericTextBox({
        min: 0,
        format: 'c',
        spinners: false
    });

    $('#ntbDisc').kendoNumericTextBox({
        min: 0,
        format: 'c',
        spinners: false
    });

    $('#ntbPurchaseOrder').kendoNumericTextBox({
        min: 0,
        format: '#',
        decimal: 0,
        spinners: false
    });

    $('#emissionDate').kendoDatePicker({
        //change: function () {
        //    setTimeout(function () {
        //        //$('#dueDate').data('kendoDatePicker').open();
        //        $('#dueDate').focus();
        //    }, 500);
        //}
    });

    $('#dueDate').kendoDatePicker({
        //change: function () {
        //    setTimeout(function () {
        //        $('#ntbInvoiceTotal').data('kendoNumericTextBox').focus();
        //    }, 500);
        //}
    });

    $(':input[data-kendoDatePicker=true]').kendoDatePicker({
        format: "MM/dd/yyyy",
        format: "MM/dd/yyyy hh:mm:ss tt"
    });

    my.editVendor = function () {
        $('#divVendorTextBox').delay(400).fadeIn();
        $('#divVendorLabel').fadeOut();
        //$('#vendorSearchBox').data('kendoAutoComplete').value($('#labelVendorName').text());
    };

    $('#vendorSearchBox').select2({
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
        //setTimeout(function () {
        //    $("#clientSearchBox").select2('open');
        //}, 300);
        $('#invoiceCommentTextBox').val('Doc.: ' + $('#ntbInvoiceNumber').val() + (e.val.DisplayName.length ? ' - Fornecedor: ' + e.val.DisplayName : '') + (kendo.toString($('#clientSearchBox').select2('data') ? ' - Cliente: ' + $('#clientSearchBox').select2('data').DisplayName : '')));
        //if (my.vm.selectedVendorId()) {
        //    this.value(e.val.DisplayName);
        //} else {
        //    this.value('');
        //}
    });

    my.editClient = function () {
        $('#divClientTextBox').delay(400).fadeIn();
        $('#divClientLabel').fadeOut();
        //$('#clientSearchBox').data('kendoAutoComplete').value($('#labelClientName').text());
    };
    
    $('#clientSearchBox').select2({
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
        $('#invoiceCommentTextBox').val('Doc.: ' + $('#ntbInvoiceNumber').val() + (kendo.toString($('#clientSearchBox').select2('data') ? ' - Fornecedor: ' + $('#clientSearchBox').select2('data').DisplayName : '') + (e.val.DisplayName.length ? ' - Cliente: ' + e.val.DisplayName : '')));
        //if (my.vm.selectedClientId()) {
        //    this.value(e.val.DisplayName);
        //} else {
        //    this.value('');
        //}
    });

    $('#itemSearchBox').select2({
        placeholder: "Busque produtos por nome, *ref. ou #c&#243;d. de barra.",
        allowClear: true,
        //minimumInputLength: 2,
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

    $('#itemSearchBox').on("select2-selecting", function (e) {
        if (e.val.ProductUnit === 1) {
            $('#ntbQty').data('kendoNumericTextBox').options.format = '';
            setTimeout(function () {
                $('#btnAddItem').focus();
            }, 100);
        }
        my.vm.selectedProductId(e.val.ProductId);
        $('#ntbQty').data('kendoNumericTextBox').value(1);
        $('#ntbValue1').data('kendoNumericTextBox').value(e.val.Finan_Cost);
    });

    //$("#itemSearchBox").kendoAutoComplete({
    //    delay: 500,
    //    minLength: 5,
    //    dataTextField: 'ProductName',
    //    placeholder: "Insira Nome, Referência ou Código.",
    //    template: '<strong>Produto: </strong><span>${ data.ProductName }</span><br /><strong>Ref: </strong><span>${ data.ProductRef }</span> <div class="dnnRight"><strong>Estoque: </strong> <span>${ data.QtyStockSet }</span></div><strong>Cod. Barra: </strong><span>${ data.Barcode }</span>',
    //    dataSource: {
    //        transport: {
    //            read: '/desktopmodules/riw/api/products/getProducts?portalId=' + portalID
    //        },
    //        pageSize: 1000,
    //        serverPaging: true,
    //        serverSorting: true,
    //        serverFiltering: true,
    //        sort: {
    //            field: "ProductName",
    //            dir: "False"
    //        },
    //        schema: {
    //            model: {
    //                id: 'ProductId',
    //                fields: {
    //                    ProductName: {
    //                        type: 'string'
    //                    },
    //                    UnitValue: {
    //                        type: 'number'
    //                    }
    //                }
    //            },
    //            data: 'data',
    //            total: 'total'
    //        }
    //    },
    //    highlightFirst: true,
    //    filter: "contains",
    //    dataBound: function () {
    //        var autocomplete = this;
    //        switch (true) {
    //            case (this.dataSource.total() > 20):
    //                //if (!$('.toast-item-wrapper').length) $().toastmessage('showSuccessToast', 'Dezenas de items encontrados... refina sua busca.');
    //                $.pnotify({
    //                    title: 'Aten&#231;&#227;o!',
    //                    text: 'Dezenas de itens encontrados...<br />...refina sua busca.',
    //                    type: 'info',
    //                    icon: 'fa fa-exclamation-circle fa-lg',
    //                    addclass: "stack-bottomright",
    //                    stack: my.stack_bottomright
    //                });
    //                break;
    //            case (this.dataSource.total() === 0):
    //                autocomplete.value('');
    //                //if (!$('.toast-item-wrapper').length) $().toastmessage('showWarningToast', 'Sua busca não trouxe resultado algum.');
    //                $.pnotify({
    //                    title: 'Aten&#231;&#227;o!',
    //                    text: 'Sua busca n&#227;o trouxe resultado algum.',
    //                    type: 'warning',
    //                    icon: 'fa fa-warning fa-lg',
    //                    addclass: "stack-bottomright",
    //                    stack: my.stack_bottomright
    //                });
    //                break;
    //            default:
    //        }
    //    },
    //    select: function (e) {
    //        e.preventDefault();
    //        var autocom = this;
    //        var dataItem = autocom.dataItem(e.item.index());
    //        if (dataItem) {
    //            my.vm.selectedProductId(dataItem.ProductId);
    //            this.value(dataItem.ProductName);
    //            $('#unitType').text(dataItem.UnitTypeAbbv);
    //            setTimeout(function () {
    //                $('#ntbQty').data('kendoNumericTextBox').focus();
    //            }, 100);
    //        }
    //    }
    //});
    //$("#itemSearchBox").data('kendoAutoComplete').list.width(350);

    my.invoiceItems = new kendo.data.DataSource({
        sort: { field: "ProductName", dir: "ASC" },
        schema: {
            model: {
                id: 'InvoiceItemId'
            }
        },
        //aggregate: [
        //    {
        //        field: 'TotalValue',
        //        aggregate: "sum"
        //    }
        //]
    });

    $('#itemsGrid').kendoGrid({
        dataSource: my.invoiceItems,
        sortable: true,
        editable: {
            confirmation: "Tem certeza?"
        },
        //toolbar: kendo.template($("#tmplToolbar").html()),
        columns: [
            {
                field: 'ProductCode', title: 'Código', template: '<span>#= ProductCode #</span>', width: 130
            },
            {
                field: 'ProductName', title: 'Produto / Serviço'
            },
            //{
            //    field: 'Freight', title: 'Frete', width: 80, template: '<div class="text-right">#= kendo.toString(Freight, "n") #</div>', editor: editNumberWithoutSpinners
            //},
            //{
            //    field: 'FreightICMS', title: 'Frete ICMS', width: 80, template: '<div class="text-right">#= kendo.toString(FreightICMS, "p") #</div>', editor: editNumberWithoutSpinners
            //},
            {
                field: 'UnitTypeAbbv', title: 'Unid', width: 50
            },
            {
                field: 'Qty', title: 'Qde', width: 70, editor: editNumberWithoutSpinners
            },
            {
                field: 'UnitValue1', width: 80, headerTemplate: '<div class="text-right">Preço 1</div>', format: '{0:n}', template: '<span class="dnnRight">#= kendo.toString(UnitValue1, "n") #</span>', editor: editNumberWithoutSpinners
            },
            {
                field: 'UnitValue2', hidden: true, width: 80, headerTemplate: '<div class="text-right">Preço 2</div>', format: '{0:n}', template: '<span class="dnnRight">#= kendo.toString(UnitValue2, "n") #</span>', editor: editNumberWithoutSpinners
            },
            {
                field: 'Discount', hidden: true, width: 80, headerTemplate: '<div class="text-right">Desc.</div>', format: '{0:n}', template: '<span class="dnnRight">#= kendo.toString(Discount, "n") #</span>', editor: editNumberWithoutSpinners
            },
            {
                field: 'UpdateProduct', hidden: true, title: 'Atualizado', width: 80, template: '#= UpdateProduct ? "Sim" : "Não" #'
            },
            {
                field: 'TotalValue', width: 100, headerTemplate: '<div class="text-right">Total</div>', format: '{0:n}', template: '<span class="dnnRight">#= kendo.toString((UnitValue1 * Qty - Discount), "n") #</span>', aggregates: ["sum"] //, footerTemplate: '<div class="text-right">#= kendo.toString(sum, "c") #</div>'
            },
            {
                command: [
                    //{
                    //    name: 'update',
                    //    text: '',
                    //    //imageClass: 'fa fa-check',
                    //    click: function(e) {
                    //        var grid = this;
                    //        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

                    //        var params = {
                    //            InvoiceId: my.invoiceId,
                    //            InvoiceItemId: dataItem.InvoiceItemId,
                    //            ProductId: dataItem.ProductId,
                    //            Qty: dataItem.Qty,
                    //            UnitValue: dataItem.UnitValue,
                    //            Discount: dataItem.Discount,
                    //            ModifiedByUser: userID,
                    //            ModifiedOnDate: moment().format()
                    //        };

                    //        $.ajax({
                    //            type: 'POST',
                    //            url: '/desktopmodules/riw/api/invoices/updateInvoiceItem',
                    //            data: params
                    //        }).done(function (data) {
                    //            if (data.Result.indexOf("success") !== -1) {
                    //                grid.dataSource.sync();
                    //                //$().toastmessage('showSuccessToast', 'Item atualizado com sucesso!');
                    //                $.pnotify({
                    //                    title: 'Sucesso!',
                    //                    text: 'Item atualizado com sucesso!',
                    //                    type: 'success',
                    //                    icon: 'fa fa-check fa-lg',
                    //                    addclass: "stack-bottomright",
                    //                    stack: my.stack_bottomright
                    //                });
                    //                if (my.total !== $('#ntbInvoiceTotal').data('kendoNumericTextBox').value()) {
                    //                    //$().toastmessage('showWarningToast', 'Valor total da nota fiscal não coincide com o valor total dos itens inseridos!');
                    //                    $.pnotify({
                    //                        title: 'Aten&#231;&#227;o!',
                    //                        text: 'Valor total da nota fiscal não coincide com o valor total dos itens inseridos!',
                    //                        type: 'warning',
                    //                        icon: 'fa fa-warning fa-lg',
                    //                        addclass: "stack-bottomright",
                    //                        stack: my.stack_bottomright
                    //                    });
                    //                }
                    //            } else {
                    //                //$().toastmessage('showErrorToast', data.Result);
                    //                $.pnotify({
                    //                    title: 'Erro!',
                    //                    text: data.Result,
                    //                    type: 'error',
                    //                    icon: 'fa fa-times-circle fa-lg',
                    //                    addclass: "stack-bottomright",
                    //                    stack: my.stack_bottomright
                    //                });
                    //            }
                    //        }).fail(function (jqXHR, textStatus) {
                    //            console.log(jqXHR.responseText);
                    //        });
                    //    },
                    //},
                    {
                        name: "exclude",
                        text: '<span class="fa fa-times"></span>',
                        //imageClass: "k-i-close",
                        click: function (e) {
                            var grid = this;
                            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

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

                                                var params = {
                                                    InvoiceId: my.invoiceId,
                                                    InvoiceItemId: dataItem.InvoiceItemId,
                                                    ProductId: dataItem.ProductId,
                                                    Qty: dataItem.Qty,
                                                    ModifiedByUser: userID,
                                                    ModifiedOnDate: moment().format()
                                                };

                                                $dialog.dialog('close');
                                                $dialog.dialog('destroy');

                                                $.ajax({
                                                    type: 'POST',
                                                    url: '/desktopmodules/riw/api/invoices/removeInvoiceItem',
                                                    data: params
                                                }).done(function (data) {
                                                    if (data.Result.indexOf("success") !== -1) {
                                                        grid.dataSource.remove(dataItem);
                                                        grid.dataSource.sync();
                                                        //if (my.total !== $('#ntbInvoiceTotal').data('kendoNumericTextBox').value()) {
                                                        //    //$().toastmessage('showToast', { text: 'Valor total da nota fiscal não coincide com o valor total dos itens inseridos!', stayTime: 10000, type: 'warning' });
                                                        //    $.pnotify({
                                                        //        title: 'Aten&#231;&#227;o!',
                                                        //        text: 'Valor total da nota fiscal não coincide com o valor total dos itens inseridos!',
                                                        //        type: 'warning',
                                                        //        icon: 'fa fa-warning fa-lg',
                                                        //        addclass: "stack-bottomright",
                                                        //        stack: my.stack_bottomright,
                                                        //        delay: 2000
                                                        //    });
                                                        //}
                                                        //$().toastmessage('showSuccessToast', 'Item removido e estoque atualizado!');
                                                        $.pnotify({
                                                            title: 'Sucesso!',
                                                            text: 'Item atualizado com sucesso!',
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
                        }
                    }
                ],
                title: "Excluir",
                width: 55
            }
        ],
        dataBound: function (e) {
            var ds = this.dataSource;
            if (this.dataSource.view().length > 0) {
                $('#itemsGrid').show();

                //my.total = ds.aggregates().TotalValue.sum;

                //$("input").focus(function () {
                //    var input = $(this);
                //    setTimeout(function () {
                //        input.select();
                //    });
                //});
            } else {
                $('#itemsGrid').hide();
            }
        }
    });

    $('#btnUpdateInvoice').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            PortalId: portalID,
            InvoiceId: my.invoiceId,
            InvoiceNumber: $('#ntbInvoiceNumber').data('kendoNumericTextBox').value(),
            ProviderId: my.vm.selectedVendorId(),
            ClientId: my.vm.selectedClientId(),
            EmissionDate: moment($('#emissionDate').data('kendoDatePicker').value()).format(),
            DueDate: moment(new Date('1900-01-01 00:00')).format(),
            Comment: $('#invoiceCommentTextBox').val(),
            Purchase: $('#chkBoxPurchase').is(':checked'),
            CreatedByUser: userID,
            CreatedOnDate: moment().format(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/invoices/updateInvoice',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                if (my.invoiceId) {
                    //$().toastmessage('showSuccessToast', 'Notal Fiscal atualizada com sucesso!');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Lançamento atualizado.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                } else {
                    if (data.Invoice) {
                        my.invoiceId = data.Invoice.InvoiceId;
                        my.vm.locked(false);
                        document.location.hash = 'invoiceId/' + my.invoiceId
                        if (parent.$('#window').data('kendoWindow')) {
                            parent.$('#window').data('kendoWindow').title('Lançamento: ' + data.Invoice.InvoiceNumber.toString() + ' (' + $('#vendorSearchBox').val() + ')');
                        } else {
                            $('#moduleTitleSkinObject').append(' ' + data.Invoice.InvoiceNumber);
                        }
                        $('#newInvoiceItem').fadeIn();
                        $('#itemSearchBox').select2('val', null);
                        // $('#itemSearchBox').data('kendoAutoComplete').focus();

                        //$().toastmessage('showSuccessToast', 'Notal Fiscal inicializada com sucesso!');
                        $.pnotify({
                            title: 'Sucesso!',
                            text: 'Lançamento inicializado.',
                            type: 'success',
                            icon: 'fa fa-check fa-lg',
                            addclass: "stack-bottomright",
                            stack: my.stack_bottomright
                        });
                    }
                    //if (my.total !== $('#ntbInvoiceTotal').data('kendoNumericTextBox').value()) {
                    //    //$().toastmessage('showToast', { text: 'Valor total da nota fiscal não coincide com o valor total dos itens inseridos!', stayTime: 10000, type: 'warning' });
                    //    $.pnotify({
                    //        title: 'Aten&#231;&#227;o!',
                    //        text: 'Valor total da nota fiscal não coincide com o valor total dos itens inseridos!',
                    //        type: 'warning',
                    //        icon: 'fa fa-warning fa-lg',
                    //        addclass: "stack-bottomright",
                    //        stack: my.stack_bottomright,
                    //        delay: 2000
                    //    });
                    //}
                }
                $this.button('reset');
                $this.html('<i class="fa fa-check"></i>&nbsp; Atualizar');
            } else {
                //$().toastmessage('showToast', { text: data.Result, type: 'error', stayTyime: 10000 });
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                $this.button('reset');
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
    });

    $('#btnAddItem').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            InvoiceId: my.invoiceId,
            InvoiceItemId: 0,
            ProductId: my.vm.selectedProductId(),
            Qty: $('#ntbQty').data('kendoNumericTextBox').value(),
            UnitValue1: $('#ntbValue1').data('kendoNumericTextBox').value(),
            UnitValue2: $('#ntbValue2').data('kendoNumericTextBox').value(),
            Discount: (($('#ntbDisc').data('kendoNumericTextBox').value() / $('#ntbValue2').data('kendoNumericTextBox').value())) || 0,
            UpdateProduct: $('#costCheckBox').is(':checked'),
            Purchase: $('#chkBoxPurchase').is(':checked'),
            //freightItem: $('#ntbItemFreight').val(),
            //freightICMS: $('#ntbItemFreightICMS').val(),
            CreatedByUser: userID,
            CreatedOnDate: moment().format(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/invoices/addInvoiceItem',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                if (data.InvoiceItem) {

                    my.invoiceItems.add({
                        InvoiceItemId: data.InvoiceItem.InvoiceItemId,
                        ProductId: my.vm.selectedProductId(),
                        ProductName: $('#itemSearchBox').select2('data').ProductName, // $('#itemSearchBox').val(),
                        ProductCode: data.InvoiceItem.Barcode ? '<strong>CB: </strong>' + data.InvoiceItem.Barcode : (data.InvoiceItem.ProductRef ? '<strong>REF: </strong>' + data.InvoiceItem.ProductRef : ''),
                        UnitTypeAbbv: $('#unitType').text(),
                        Qty: $('#ntbQty').data('kendoNumericTextBox').value(),
                        Discount: $('#ntbDisc').data('kendoNumericTextBox').value(),
                        UnitValue1: $('#ntbValue1').data('kendoNumericTextBox').value(),
                        UnitValue2: $('#ntbValue2').data('kendoNumericTextBox').value(),
                        UpdateProduct: $('#costCheckBox').is(':checked'),
                        TotalValue: ($('#ntbValue2').data('kendoNumericTextBox').value() * $('#ntbQty').data('kendoNumericTextBox').value()),
                        ModifedOnDate: kendo.toString(my.setHours(new Date(), 0, 14, 0, 0), 'g')
                    });

                    my.vm.invoiceTotal(my.vm.invoiceTotal() + ($('#ntbQty').data('kendoNumericTextBox').value() * $('#ntbValue1').data('kendoNumericTextBox').value()));

                    $('#totalLabel').text(kendo.toString((my.vm.invoiceTotal() + $('#ntbFreight').data('kendoNumericTextBox').value()), 'c'));

                    // my.invoiceItems.sort({ field: 'ModifiedDate', dir: 'desc' });

                    my.vm.selectedProductId(null);
                    $('#ntbQty').data('kendoNumericTextBox').value(0);
                    $('#ntbValue1').data('kendoNumericTextBox').value(0);
                    $('#ntbValue2').data('kendoNumericTextBox').value(0);
                    $('#ntbDisc').data('kendoNumericTextBox').value(0);
                    $('#costCheckBox').val(null);
                    $('#itemSearchBox').select2('val', null);
                    //$('#itemSearchBox').data('kendoAutoComplete').focus();
                    //$().toastmessage('showSuccessToast', 'Item inserido com sucesso!');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Item inserido.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    //if (my.total !== $('#ntbInvoiceTotal').data('kendoNumericTextBox').value()) {
                    //    //$().toastmessage('showWarningToast', 'Valor total da nota fiscal não coincide com o valor total dos itens inseridos!');
                    //    $.pnotify({
                    //        title: 'Aten&#231;&#227;o!',
                    //        text: 'Valor total da nota fiscal não coincide com o valor total dos itens inseridos!',
                    //        type: 'warning',
                    //        icon: 'fa fa-warning fa-lg',
                    //        addclass: "stack-bottomright",
                    //        stack: my.stack_bottomright,
                    //        delay: 2000
                    //    });
                    //}
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
        }).always(function () {
            $this.button('reset');
        });
    });

    $('#btnUpdateItems').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var grid = $('#itemsGrid').data("kendoGrid");

        var params = [];
        $.each(grid.dataSource.data(), function (i, product) {
            if (product.dirty) {
                params.push({
                    InvoiceId: my.invoiceId,
                    InvoiceItemId: product.InvoiceItemId,
                    ProductId: product.ProductId,
                    Qty: product.Qty,
                    UnitValue1: product.UnitValue1,
                    UnitValue2: product.UnitValue2,
                    Discount: product.Discount,
                    UpdateProduct: product.UpdateProduct,
                    ModifiedByUser: userID,
                    ModifiedOnDate: moment().format()
                });
            }
        });

        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/invoices/updateInvoiceItem',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.each(grid.dataSource.data(), function (i, item) {
                    if (item.dirty) {
                        item.set('Qty', item.Qty)
                        item.set('Discount', item.Discount)
                        item.set('UnitValue1', item.UnitValue2)
                        item.set('UnitValue2', item.UnitValue2)
                        item.set('UpdateProduct', item.UpdateProduct)
                        item.set('TotalValue', item.UnitValue2 * item.Qty)
                    }
                });
                grid.refresh();
                //$('#aggregate').html(my.invoiceItems.aggregates().TotalValue.sum);
                //$().toastmessage('showSuccessToast', 'Item atualizado com sucesso!');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Item atualizado com sucesso!',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //if (my.total !== $('#ntbInvoiceTotal').data('kendoNumericTextBox').value()) {
                //    //$().toastmessage('showWarningToast', 'Valor total da nota fiscal não coincide com o valor total dos itens inseridos!');
                //    $.pnotify({
                //        title: 'Aten&#231;&#227;o!',
                //        text: 'Valor total da nota fiscal não coincide com o valor total dos itens inseridos!',
                //        type: 'warning',
                //        icon: 'fa fa-warning fa-lg',
                //        addclass: "stack-bottomright",
                //        stack: my.stack_bottomright,
                //        delay: 2000
                //    });
                //}
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
            $this.button('reset');
        });
    });

    $('#btnUpdatePayment').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            PortalId: portalID,
            InvoiceId: my.invoiceId,
            //InvoiceNumber: $('#ntbInvoiceNumber').data('kendoNumericTextBox').value(),
            InvoiceAmount: parseFloat($('#ntbInvoiceTotal').val().replace(',', '.')),
            PayIn: $('#ntbPayIn').val().length ? $('#ntbPayIn').data('kendoNumericTextBox').value() : 0,
            Freight: $('#ntbFreight').val().length ? $('#ntbFreight').data('kendoNumericTextBox').value() : 0,
            ProviderId: my.vm.selectedVendorId(),
            ClientId: my.vm.selectedClientId(),
            //EstimateId: my.vm.selectedEstimateId(),
            DueDate: $('#dueDate').val() ? moment($('#dueDate').data('kendoDatePicker').value()).format() : moment($('#emissionDate').data('kendoDatePicker').value()).format(),
            //EmissionDate: moment($('#emissionDate').data('kendoDatePicker').value()).format(),
            PayQty: $('#ntbPayQty').data('kendoNumericTextBox').value(),
            Interval: $('#ntbInterval').val().length ? $('#ntbInterval').data('kendoNumericTextBox').value() : 30,
            InterestRate: $('#ntbInterestRate').data('kendoNumericTextBox').value(),
            Comment: $('#invoiceCommentTextBox').val(),
            CreditDebit: $('#creDebRadio1').is(':checked'),
            //TransDate: moment(new Date('1900-01-01 00:00:00')).format(),
            CreatedByUser: userID,
            CreatedOnDate: moment().format(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/invoices/updateInvoicePayment',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                //if (data.Invoice) {
                //my.invoiceId = data.InvoiceId;
                //my.vm.locked(false);
                //document.location.hash = 'invoiceId/' + my.invoiceId
                //if (parent.$('#window').data('kendoWindow')) {
                //    parent.$('#window').data('kendoWindow').title('Nota Fiscal ' + data.InvoiceId.toString() + ' (' + $('#vendorSearchBox').val() + ')');
                //} else {
                //    $('#moduleTitleSkinObject').html($('#moduleTitleSkinObject').html() + ': ' + data.InvoiceId);
                //}
                //$('#newInvoiceItem').fadeIn();
                //$('#itemSearchBox').data('kendoAutoComplete').value('');
                //$('#itemSearchBox').data('kendoAutoComplete').focus();
                //$().toastmessage('showSuccessToast', 'Notal Fiscal inicializada com sucesso!');
                //    $.pnotify({
                //        title: 'Sucesso!',
                //        text: 'Nota Fiscal inicializada.',
                //        type: 'success',
                //        icon: 'fa fa-check fa-lg',
                //        addclass: "stack-bottomright",
                //        stack: my.stack_bottomright
                //    });
                //} else {
                //$().toastmessage('showSuccessToast', 'Notal Fiscal atualizada com sucesso!');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Financeiro atualizado.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //if (my.total !== $('#ntbInvoiceTotal').data('kendoNumericTextBox').value()) {
                //    //$().toastmessage('showToast', { text: 'Valor total da nota fiscal não coincide com o valor total dos itens inseridos!', stayTime: 10000, type: 'warning' });
                //    $.pnotify({
                //        title: 'Aten&#231;&#227;o!',
                //        text: 'Valor total da nota fiscal não coincide com o valor total dos itens inseridos!',
                //        type: 'warning',
                //        icon: 'fa fa-warning fa-lg',
                //        addclass: "stack-bottomright",
                //        stack: my.stack_bottomright,
                //        delay: 2000
                //    });
                //}
                //}
                $this.button('reset');
                $this.html('<i class="fa fa-check"></i>&nbsp; Atualizar Financeiro');
            } else {
                //$().toastmessage('showToast', { text: data.Result, type: 'error', stayTyime: 10000 });
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                $this.button('reset');
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
    });

    if (my.invoiceId > 0) {
        $.ajax({
            url: '/desktopmodules/riw/api/invoices/getInvoice?invoiceId=' + my.invoiceId
        }).done(function (data) {
            if (data.Result.indexOf('success') !== -1) {
                if (data.Invoice) {

                    $('#moduleTitleSkinObject').append(' ' + data.Invoice.InvoiceNumber.toString());
                    $('#ntbInvoiceNumber').data('kendoNumericTextBox').value(data.Invoice.InvoiceNumber);
                    my.vm.selectedVendorId(data.Invoice.ProviderId);
                    $('#divVendorLabel').show();
                    $('#labelVendorName').text(data.Invoice.VendorName);

                    if (data.Invoice.CreditDebit) {
                        $('#creDebRadio1').prop('checked', false);
                        $('#creDebRadio2').prop('checked', true);
                    } else {
                        $('#creDebRadio1').prop('checked', true);
                        $('#creDebRadio2').prop('checked', false);
                    }

                    if (data.Invoice.ClientId) {
                        my.vm.selectedClientId(data.Invoice.ClientId);
                        $('#divClientLabel').show();
                        $('#labelClientName').text(data.Invoice.ClientName);

                        if (data.Invoice.EstimateId > 0) {
                            my.estimatesData.read();
                            setTimeout(function () {
                                $('#ddlEstimates').data('kendoComboBox').value(data.Invoice.EstimateId);
                            }, 100);
                        }
                    } else {
                        $('#divClientTextBox').show();
                    }

                    $('#ntbInterestRate').data('kendoNumericTextBox').value(data.Invoice.InterestRate);
                    if (data.Invoice.Interval < 30 || data.Invoice.Interval > 30) {
                        $('#ntbInterval').data('kendoNumericTextBox').value(data.Invoice.Interval);
                    }
                    $('#ntbPayQty').data('kendoNumericTextBox').value(data.Invoice.PayQty);
                    $('#emissionDate').data('kendoDatePicker').value(new Date(data.Invoice.EmissionDate));
                    $('#ntbPayIn').data('kendoNumericTextBox').value(data.Invoice.PayIn);
                    $('#ntbInvoiceTotal').data('kendoNumericTextBox').value(data.Invoice.InvoiceAmount);
                    $('#dueDate').data('kendoDatePicker').value(new Date(data.Invoice.DueDate));
                    $('#invoiceCommentTextBox').val(data.Invoice.Comment);
                    $('#chkBoxPurchase').attr('checked', data.Invoice.Purchase);
                    my.vm.locked(data.Invoice.Locked);
                    $('#newInvoiceItem').fadeIn();
                    $('#btnUpdateInvoice').html('<span class="fa fa-check"></span>&nbsp; Atualizar');
                    //$('#itemSearchBox').data('kendoAutoComplete').focus();

                    $.each(data.Invoice.InvoiceItems, function (i, item) {
                        my.invoiceItems.add({
                            InvoiceItemId: item.InvoiceItemId,
                            ProductId: item.ProductId,
                            ProductCode: item.Barcode ? '<strong>CB: </strong>' + item.Barcode : (item.ProductRef ? '<strong>REF: </strong>' + item.ProductRef : ''),
                            ProductName: item.ProductName,
                            UnitTypeAbbv: item.UnitTypeAbbv,
                            Qty: item.Qty,
                            Discount: (item.UnitValue2 * item.Discount),
                            UnitValue1: item.UnitValue1,
                            UnitValue2: item.UnitValue2,
                            UpdateProduct: item.UpdateProduct,
                            TotalValue: (item.UnitValue2 * item.Qty)
                        });

                        my.vm.invoiceTotal((my.vm.invoiceTotal() + (item.UnitValue2 * item.Qty)) - (item.UnitValue2 * item.Discount));

                        $('#totalLabel').text(kendo.toString((my.vm.invoiceTotal() + $('#ntbFreight').data('kendoNumericTextBox').value()), 'c'));
                    });

                    //if (my.total !== $('#ntbInvoiceTotal').data('kendoNumericTextBox').value()) {
                    //    //$().toastmessage('showToast', { text: 'Valor total da nota fiscal não coincide com o valor total dos itens inseridos!', stayTime: 10000, type: 'warning' });
                    //    $.pnotify({
                    //        title: 'Aten&#231;&#227;o!',
                    //        text: 'Valor total da nota fiscal não coincide com o valor total dos itens inseridos!',
                    //        type: 'warning',
                    //        icon: 'fa fa-warning fa-lg',
                    //        addclass: "stack-bottomright",
                    //        stack: my.stack_bottomright,
                    //        delay: 2000
                    //    });
                    //}
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
    } else {
        $('#divVendorTextBox').show();
        $('#divClientTextBox').show();
        $('#ntbInvoiceNumber').data('kendoNumericTextBox').focus();
    }

    $('#btnAddVendor').click(function (e) {
        var btn = this;
        e.preventDefault();
        if (e.clientX === 0) {
            return false;
        }

        $("#invoiceWindow").append("<div id='window'></div>");
        var sContent = clientURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&vendor=1',
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
                    //$('vendorSearchBox').data('kendoComboBox').dataSource.read();
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

        $("#invoiceWindow").append("<div id='window'></div>");
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
                    //$('clientSearchBox').data('kendoComboBox').dataSource.read();
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

    $("#btnAddNewProduct").click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();
        //document.location.href = editItemURL;

        $("#invoiceWindow").append("<div id='window'></div>");
        var sContent = editItemURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer',
            kendoWindow = $('#window').kendoWindow({
                actions: ["Refresh", "Maximize", "Close"],
                title: 'Novo Item',
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

    $('#ntbFreight').keyup(function () {
        $('#totalLabel').text(kendo.toString((my.vm.invoiceTotal() + kendo.parseFloat(this.value)), 'c'));
    });

    $('input.enterastab, select.enterastab, textarea.enterastab').on('keydown', function (e) {
        if (e.keyCode === 13) {
            var focusable = $('input,a,select,textarea').filter(':visible');
            if (this.id === 'payForm') {
                //focusable.eq(focusable.index(this) + 1).focus().select();
                $('#emissionDate').data('kendoDatePicker').open();
            } else {
                focusable.eq(focusable.index(this) + 1).focus().select();
            }
            return false;
        }
    });

    $("input").focus(function () {
        var input = $(this);
        var kntb = input.closest('span');
        if (kntb.hasClass('k-numeric-wrap')) {
            setTimeout(function () {
                input.select();
            });
        }
    });

});

function editNumberWithoutSpinners(container, options) {
    var fld = $('<input data-text-field="' + options.field + '" ' + 'data-value-field="' + options.field + '" ' + 'data-bind="value:' + options.field + '" ' + 'data-format="' + options.format + '"  />');
    fld.focus(function () {
        this.select();
    });
    fld.appendTo(container)
    fld.kendoNumericTextBox({
        spinners: false
    });
}

function peopleFormatResult(data) {
    return data.PersonId + ' ' + data.DisplayName
        + (data.Email.length > 1 ? '<br />' + data.Email : '')
        + (data.Telephone.length > 1 ? '<br />' + data.Telephone : '');
}

function peopleFormatSelection(data) {
    return data.PersonId + ' ' + data.DisplayName;
}

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
}

function productFormatSelection(data) {
    return data.ProductName;
}