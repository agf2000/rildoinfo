
$(function () {

    my.productId = my.getParameterByName('itemId');
    //my.itemsSearchBox = $("#productSearch").kendoAutoComplete();
    my.record = 0;
    my.uId;
    my.total = 0;

    // knockout js view model
    my.vm = function () {
        // this is knockout view model
        var self = this;

        self.barcode = ko.observable(),
        self.productRef = ko.observable(),
        self.relatedId = ko.observable(),
        self.productId = ko.observable(),
        self.productName = ko.observable(),
        self.unitValue = ko.observable(),
        self.productQty = ko.observable(1),
        self.maxQty = ko.observable(0),
        self.total = ko.observable(0),
        self.selectedProductId = ko.observable()

        // make view models available for apps
        return {
            barcode: barcode,
            productRef: productRef,
            relatedId: relatedId,
            productQty: productQty,
            maxQty: maxQty,
            total: total,
            selectedProductId: selectedProductId,
            productId: productId,
            productName: productName,
            unitValue: unitValue
        };

    }();

    // apply ko bindings
    ko.applyBindings(my.vm);

    $('#productMenu').show();

    $('#relatedProductsGrid').kendoGrid({
        dataSource: new kendo.data.DataSource({
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/products/getProductsRelated?portalId=' + portalID + '&productId=' + my.productId + '&lang=pt-BR&relatedType=-1&getAll=true'
                }
            }
        }),
        change: function () {
            var row = this.select();
            var id = row.data("uid");
            my.uId = id;
        },
        sortable: true,
        toolbar: kendo.template($('#availProductsHeaderTemplate').html()),
        dataBinding: function () {
            my.record = 0;
        },
        columns: [
            {
                field: 'RelatedProductName', title: 'Produto'
            },
            {
                field: 'ProductQty', title: 'Qde', width: 50
            },
            {
                field: 'RelatedUnitValue', title: 'Valor Total', width: 100, template: '#= RelatedUnitValue * ProductQty #'
            },
            {
                title: ' ',
                template: "<input type='checkbox' class='checkbox' />",
                width: 35,
                sortable: false
            }
        ],
        dataBound: function (e) {
            my.vm.total(0);
            var grid = this;
            if (grid.dataSource.data().length > 0) {
                $.each(grid.dataSource.data(), function (i, p) {
                    my.vm.total(my.vm.total() + kendo.parseFloat(p.RelatedUnitValue * p.ProductQty));
                });
            }
        }
    });

    //my.vm.extendedPrice = ko.computed(function () {
    //    var total = 0;
    //    $.each($('#relatedProductsGrid').data('kendoGrid').dataSource.data(), function (i, p) {
    //        total += kendo.parseFloat(p.RelatedUnitValue);
    //    });
    //    return total;
    //}, my.vm);

    //bind click event to the checkbox
    $('#relatedProductsGrid').data('kendoGrid').table.on("click", ".checkbox", selectRow);

    my.checkedIds = {};

    //on click of the checkbox:
    function selectRow() {
        var checked = this.checked,
            row = $(this).closest("tr"),
            grid = $('#relatedProductsGrid').data('kendoGrid'),
            dataItem = grid.dataItem(row);

        my.checkedIds[dataItem.id] = {
            RelatedId: dataItem.RelatedId
        };

        if (checked) {
            //-select the row
            row.addClass("k-state-selected");
        } else {
            //-remove selection
            row.removeClass("k-state-selected");
        }
    }

    function selectAllSelectedRows() {
        my.kitItemsGrid.tbody.find("tr").each(function () {
            var $this = $(this),
                ckbox,
                state;
            if ($this.hasClass("k-state-selected")) {
                ckbox = $this.find("td:last input");
                state = ckbox.prop("checked");
                ckbox.prop("checked", !state);
            }
        })
    };

    $('#btnAddRelatedProduct').click(function (e) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            BiDirectional: false,
            Disabled: false,
            DiscountAmt: 0,
            DiscountPercent: 0,
            MaxQty: my.vm.maxQty(),
            NotAvailable: false,
            PortalId: portalID,
            ProductId: my.productId,
            ProductQty: my.vm.productQty(),
            RelatedProductId: my.vm.selectedProductId(),
            RelatedType: $('#mustIncludeCheckbox').is(':checked') ? 1 : 2
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/products/addProductRelated',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                //$().toastmessage('showSuccessToast', my.vm.productName() + ' adicionado com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: my.vm.productName() + ' adicionado.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });

                $('#relatedProductsGrid').data('kendoGrid').dataSource.add({
                    RelatedId: data.RelatedId,
                    ProductId: my.vm.productId(),
                    RelatedProductName: my.vm.productName(),
                    RelatedProductRef: my.vm.productRef(),
                    RelatedProductBarcode: my.vm.barcode(),
                    RelatedUnitValue: my.vm.unitValue(),
                    //RelatedProductImageId: my.vm.productImageId(),
                    //RelatedExtension: my.vm.extension(),
                    ProductQty: my.vm.productQty()
                    //ProductUnit: data.ProductUnit,
                    //Finan_Cost: data.Finan_Cost,
                    //Finan_Rep: data.Finan_Rep,
                    //Finan_SalesPerson: data.Finan_SalesPerson,
                    //Finan_Tech: data.Finan_Tech,
                    //Finan_Telemarketing: data.Finan_Telemarketing,
                    //Finan_Manager: data.Finan_Manager
                });
                my.vm.productQty(1);
                my.vm.maxQty(0);
                $("#productSearch").select2('val', '');
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$('#divMsg').freeow('Erro: ', data.Result, my.freewoWarning.opts);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
    });

    $("#btnRemoveSelectedItems").click(function (e) {
        e.preventDefault();

        var $this = $(this);
                
        //var msg = '';
        //switch (true) {
        //    case (my.checkedIds.length === 1):
        //        msg = 'Tem certeza que deseja excluir o item selecionado?';
        //        break;
        //    case (my.checkedIds.length > 1):
        //        msg = 'Tem certeza que deseja excluir os itens selecionados?';
        //        break;
        //    default:
        //        return false;
        //};

        var $dialog = $('<div></div>')
            .html('<div class="confirmDialog">Tem certeza?</div>')
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

                            $this.button('loading');

                            var params = [];
                            $.each(my.checkedIds, function (i, item) {

                                params.push({
                                    RelatedId: item.RelatedId
                                });
                            });

                            $.ajax({
                                type: 'DELETE',
                                contentType: 'application/json; charset=utf-8',
                                url: '/desktopmodules/riw/api/products/removeRelatedProduct',
                                data: JSON.stringify(params)
                            }).done(function (data) {
                                if (data.Result.indexOf("success") !== -1) {
                                    $.pnotify({
                                        title: 'Sucesso!',
                                        text: 'Item removido da lista.',
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
                                $dialog.dialog('close');
                                $dialog.dialog('destroy');
                            });

                            var grid = $('#relatedProductsGrid');
                            var rowsToDelete = grid.find('.k-state-selected')
                            $.each(rowsToDelete, function (index) {
                                $.each(rowsToDelete, function (index, row) { grid.data('kendoGrid').removeRow(row); });
                            });

                            //setTimeout(function () {
                            //    $('#selectedProductsGrid').data('kendoGrid').dataSource.sync();
                            //}, 400);
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
                return {
                    portalId: portalID,
                    searchString: term,
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

    //$("#productSearch").select2({
    //    placeholder: "Busque por produtos...",
    //    allowClear: true,
    //    minimumInputLength: 3,
    //    id: function (data) {
    //        return {
    //            ProductId: data.ProductId,
    //            ProductName: data.ProductName,
    //            ProductRef: data.ProductRef,
    //            Barcode: data.Barcode,
    //            UnitValue: data.UnitValue,
    //            ProductImageId: data.ProductImageId,
    //            Extension: data.Extension,
    //            ProductUnit: data.ProductUnit,
    //            Finan_Cost: data.Finan_Cost,
    //            Finan_Rep: data.Finan_Rep,
    //            Finan_SalesPerson: data.Finan_SalesPerson,
    //            Finan_Tech: data.Finan_Tech,
    //            Finan_Telemarketing: data.Finan_Telemarketing,
    //            Finan_Manager: data.Finan_Manager
    //        };
    //    },
    //    ajax: {
    //        url: "/desktopmodules/riw/api/products/getproducts",
    //        quietMillis: 100,
    //        data: function (term, page) { // page is the one-based page number tracked by Select2
    //            return {
    //                portalId: portalID,
    //                filter: term,
    //                pageIndex: page,
    //                pageSize: 10,
    //                orderBy: 'ProductName'
    //            };
    //        },
    //        results: function (data, page) {
    //            var more = (page * 10) < data.total; // whether or not there are more results available

    //            // notice we return the value of more so Select2 knows if more results can be loaded
    //            return { results: data.data, more: more };
    //        }
    //    },
    //    formatResult: productFormatResult, // omitted for brevity, see the source of this page
    //    formatSelection: productFormatSelection, // omitted for brevity, see the source of this page
    //    dropdownCssClass: "bigdrop", // apply css that makes the dropdown taller
    //    escapeMarkup: function (m) { return m; } // we do not want to escape markup since we are displaying html in results
    //});

    $('#productSearch').on("select2-selecting", function (e) {
        my.vm.barcode(e.val.Barcode);
        my.vm.selectedProductId(e.val.ProductId);
        my.vm.productId(e.val.productId);
        my.vm.productName(e.val.ProductName);
        my.vm.productRef(e.val.ProductRef);
        my.vm.unitValue(e.val.UnitValue);

        setTimeout(function () {
            $('#btnAddRelatedProduct').focus();
        }, 100);
    });

    $('#productMenu').kendoMenu({
        select: function (e) {
            switch ($(e.item).attr('id')) {
                case 'menu_2':
                    e.preventDefault();
                    document.location.href = editItemURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_3':
                    e.preventDefault();
                    document.location.href = productImagesURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_4':
                    e.preventDefault();
                    document.location.href = productDescURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_5':
                    e.preventDefault();
                    document.location.href = productVideosURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_7':
                    e.preventDefault();
                    document.location.href = productAttributesURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_8':
                    e.preventDefault();
                    document.location.href = productShippingURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_9':
                    e.preventDefault();
                    document.location.href = productSEOURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_11':
                    e.preventDefault();
                    document.location.href = productFinanceURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
            }
        }
    });

    $('.btnReturn').click(function (e) {
        e.preventDefault();

        $(this).html('<i class="fa fa-spinner fa-spin"></i>Um momento...').attr({ 'disabled': true });
        var urlAddress = '';
        if (my.retSel > 0) {
            switch (my.retSel) {
                case 7:
                    urlAddress = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.productId + '/sel/7';
                    break;
                case 8:
                    urlAddress = historyURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.productId + '/sel/8';
                    break;
                case 9:
                    urlAddress = finanURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.productId + '/sel/9';
                    break;
                case 10:
                    urlAddress = assistURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.productId + '/sel/10';
                    break;
                case 11:
                    urlAddress = commURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.productId + '/sel/11';
                    break;
            }
            document.location.href = urlAddress;
        } else {
            if (parent.$('#window')) {
                parent.$('#window').data("kendoWindow").close();
            }
        }
    });
});

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
