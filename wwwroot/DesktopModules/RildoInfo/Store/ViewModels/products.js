
$(function () {

    // allocate memory for selected listview item's uid
    my.catId = my.getParameterByName('catId');
    my.uId = null;
    my.productsCount = 0;
    my.returnLimit = '';
    my.specials = my.getParameterByName('specials');

    // Check for adding command in url string
    my.itemAdd = my.getStringParameterByName('add');

    // check for item id in address bar and assign it to my.itemId variable
    my.itemId = my.getParameterByName('itemId');

    if (!ko.dataFor(document.getElementById('divEstimate'))) {
        my.viewModel();
    }

    $('#kddlOrder').kendoDropDownList({
        select: function (e) {
            setTimeout(function(){
                my.products.read();
            }, 500);
        }
    });

    my.vm.selectedCatId(my.catId);

    // function that checks for the selected category
    // set category name in status
    my.categoryName = function () {
        if (my.vm.selectedCatId() > 0) {            
            //my.vm.quick(2);
            $('#status').html('Filtrando por Categoria: <strong>' + $('li[id=' + my.vm.selectedCatId() + ']').text() + '</strong>');
            $('#clearFilter').show();
        }
    };

    my.categoryName();

    function catLink(obj) {
        var id = obj.split(':')[0];
        document.location.hash = 'catId/' + id;
        //my.vm.quick(2);
        my.vm.categoryId(id);
        my.loadProducts(id);

        if (my.getParameterByName('catId') > 0) {
            $('#status').html('Categoria Selecionada: <strong>' + obj.split(':')[1] + '</strong>');
            $('#clearFilter').show();
        }

        my.createListView();

        // create initial pager from datasource
        my.createPager();

        $('#productDetail').hide();
        $('.listView').show();
    }

    // create kendo pager
    my.createPager = function () {
        $("#pager").kendoPager({
            dataSource: my.products,
            autoBind: false,
            //pageSizes: [9, 18, 27],
            pageSizes: true,
            refresh: false,
            info: false,
            previousNext: true,
            numeric: false,
            messages: {
                display: "",
                empty: "",
                page: "P&#225;gina",
                of: "de {0}",
                itemsPerPage: "itens por p&#225;gina",
                first: "Ir para primeira p&#225;gina",
                previous: "",
                next: "Ir para pr&#243;xima p&#225;gina",
                last: "Ir para &#250;ltima p&#225;gina",
                refresh: "Recarregar"
            }
        });
    };

    // create kendo listview
    my.createListView = function () {

        // create kendo dataSource transport to get products
        my.productsTransport = {
            read: {
                url: '/desktopmodules/riw/api/products/getProducts'
            },
            parameterMap: function (data, type) {
                return {
                    portalId: portalID,
                    categoryId: my.vm.categoryId(),
                    searchString: my.vm.filterTerm(),
                    searchField: 'ProductName',
                    getArchived: my.vm.filterTerm().length > 0 ? 'True' : 'False',
                    getDeleted: 'False',
                    lang: 'pt-BR',
                    returnLimit: my.vm.filterTerm().length > 0 ? "" : (my.productsCount === 0 ? "10" : ""),
                    onSale: (my.specials > 0 && my.vm.filterTerm().length === 0) ? 'true' : '', // my.vm.filterTerm().length > 0 ? "" : my.productsCount === 0 ? "" : "True",
                    isDealer: isDealer,
                    pageIndex: data.page,
                    pageSize: data.pageSize,
                    orderBy: $('#kddlOrder').data('kendoDropDownList').value().split(' ')[0],
                    orderDesc: $('#kddlOrder').data('kendoDropDownList').value().split(' ')[1]
                };
            }
        };

        // create kendo dataSource for getting products transport
        my.products = new kendo.data.DataSource({
            transport: my.productsTransport,
            pageSize: pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            sort: {
                field: "ProductName",
                dir: "False"
            },
            schema: {
                model: {
                    id: 'ProductId',
                    fields: {
                        UnitValue: {
                            type: 'currency'
                        }
                    }
                },
                data: 'data',
                total: 'total'
            }
        });

        $("#listView").kendoListView({
            dataSource: my.products,
            template: kendo.template($("#templateEstimate").html()),
            dataBound: function (e) {

                $('.catLink').css({ 'cursor': 'pointer' }).on('click', function () {
                    document.location.hash = '#catId/' + this.id;
                    my.categoryName();
                    //my.vm.quick(2);
                    my.loadProducts(this.id);
                });

                // if total pages is not bigger than 1 then hide pager
                if (this.dataSource.totalPages() > 1) {
                    $('#pager').show();
                } else {
                    $('#pager').hide();
                }

                // check the total of listview items
                if (this.dataSource.total() > 0) {

                    $('.listView').show();

                    // check if search term is being used
                    if (my.vm.filterTerm().length > 0) {

                        // if true, set status's text to positive results
                        var results = this.dataSource.total() > 1 ? '<strong>' + this.dataSource.total() + '</strong> itens.' : '<strong>' + this.dataSource.total() + '</strong> item.';
                        $('#status').html('Sua busca com o termo "<strong>' + my.vm.filterTerm() + '</strong>" retornou com ' + results);
                        $('#clearFilter').show();
                    }

                    $.each(this.dataSource.view(), function (i, p) {
                        if (p.Archived === true) {
                            $('#btnEst_' + p.ProductId).hide();
                            $('#btnBuy_' + p.ProductId).hide();
                        }

                        if (p.UnitValue <= 0) {
                            $('#btnEst_' + p.ProductId).hide();
                            $('#btnBuy_' + p.ProductId).hide();
                        }
                    });
                } else {

                    // listview items is zero 
                    // check if search term is being used
                    if (my.vm.filterTerm().length > 0) {

                        // if true, set status's text to negative results
                        $('#status').html('Nada foi encontrado com o termo "<strong>' + my.vm.filterTerm() + '</strong>".');
                    }
                }

                // check if ko view model selectedProduct has anything
                //if (my.vm.selectedProducts().length > 0) {

                //    // if true, go thru each item and disabled these item in the listview
                //    $.each(this.dataSource.view(), function () {
                //        var item = this;
                //        var match = ko.utils.arrayFirst(my.vm.selectedProducts(), function (p) {
                //            return p.productId() === item.ProductId;
                //        });
                //        if (match) {
                //            $('#btnEst_' + item.ProductId).hide();
                //        }
                //    });
                //}

                // check if logged in user has credentials to see amounts, like managers and sales person
                //my.authorizationCheck();
                if (authorized < 2) {
                    //$('.columnValue').hide();
                    //$('.tFoot').hide();

                    // hide edit product pencil
                    $('.editItem').hide();
                }

                // transform each listview item with a class of k-numerictextbox to a kendo numeric text box
                $('.k-numerictextbox', this.element).each(function () {
                    var dom = $(this);
                    var ntb = dom.kendoNumericTextBox({
                        spinners: false,
                        format: ''
                    }).data('kendoNumericTextBox');
                    ntb.wrapper.width(80);
                });

                // select all text from an input and clicked in
                $("input[type=text]").on('focusin', function () {
                    var saveThis = $(this);
                    window.setTimeout(function () {
                        saveThis.select();
                    }, 100);
                });

                // listview total width
                //$('.productEstimate').css({
                //    'width': '650px'
                //});

                // initiate colobox jquery plugin
                $('.photo').colorbox();
                //$('.youtube').colorbox({ iframe: true, innerWidth: 640, innerHeight: 390 });
                $("#listView a[href*='vimeo.com']").each(function () {
                    $this = $(this);
                    var href = 'http://player.vimeo.com/video/' + $this.attr('href').split('/').pop() + '?autoplay=1';
                    $this.attr('href', href);
                    $this.colorbox({
                        iframe: true,
                        href: href,
                        innerWidth: 640,
                        innerHeight: 360
                    });
                });
                $("#listView a[href*='youtube.com']").each(function () {
                    $this = $(this);
                    var href = 'http://www.youtube.com/embed/' + $this.attr('href').split('/').pop() + '?autoplay=1';
                    $this.attr('href', href);
                    $this.colorbox({
                        iframe: true,
                        href: href,
                        innerWidth: 640,
                        innerHeight: 360
                    });
                });
                $("#listView a[href*='youtu.be']").each(function () {
                    $this = $(this);
                    var href = 'http://www.youtube.com/embed/' + $this.attr('href').split('/').pop() + '?autoplay=1';
                    $this.attr('href', href);
                    $this.colorbox({
                        iframe: true,
                        href: href,
                        innerWidth: 640,
                        innerHeight: 360
                    });
                    //var $this = $(this);
                    //var href = $this.attr('href');
                    //var youtubeId = href.split('=').pop();
                    //$this.colorbox({
                    //    html: function () {
                    //        var iframe = '<iframe width="853" height="480" src="http://www.youtube.com/embed/' + youtubeId + '?autoplay=1" frameborder="0" allowFullScreen></iframe>';
                    //        var output = "<div style='line-height: 0px; overflow: hidden;'>" + iframe + '</div>';
                    //        return output;
                    //    }
                    //});
                });

                // add each item ko view model selectedproducts
                $.each(my.vm.estimatedItems(), function (i, p) {
                    $('#btnEst_' + p.productId()).hide();
                });

                $('#btnEst_' + my.vm.removedItem()).show();

                if (my.vm.allowPurchase()) $('.purchase').removeClass('hidden');

                $('.noStock').popover({
                    delay: {
                        show: 500,
                        hide: 100
                    },
                    placement: 'top',
                    trigger: 'hover'
                });
            }
        });
    };

    my.formatCategoryLink = function (value) {
        var links = '';
        var category = value.toString().split(',');
        $.each(category, function (i, cat) {
            links = links + cat.toString().split(':')[1];
        });
        return links.substring(0, links.length - 1) + ' ';
    }

    my.editDetailItem = function () {

        $("#productWindow").append("<div id='window'></div>");
        var sContent = editItemURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.itemId + '/sel/1',
            kendoWindow = $('#window').kendoWindow({
                actions: ["Refresh", "Maximize", "Close"],
                title: 'ID: ' + my.itemId,
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
                    my.products.read();
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
    };

    my.editItem = function (uid) {

        var dataItem = $('#listView').data("kendoListView").dataSource.getByUid(uid);

        $("#productWindow").append("<div id='window'></div>");
        var sContent = editItemURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + dataItem.ProductId + '/sel/1',
            kendoWindow = $('#window').kendoWindow({
                actions: ["Refresh", "Maximize", "Close"],
                title: dataItem.ProductName + ' (ID: ' + dataItem.ProductId + ')',
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
                    my.products.read();
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
    };

    my.detailRelatedItem = function (id) {

        document.location.hash = '#itemId/' + id;

        my.itemId = id;

        my.loadItem();
    };

    my.detailItem = function (id) {

        $('#status').html('');

        $('#clearFilter').hide();

        document.location.hash = '#itemId/' + id;

        my.itemId = id;

        my.loadItem();
    };

    // add listview item to ko view model selectedProducts
    my.selectItem = function (uid) {

        // uid of the listview's selected item
        var dataItem = $('#listView').data("kendoListView").dataSource.getByUid(uid);

        if (dataItem.ProductOptionsCount > 0) {

            $('#status').html('');

            $('#clearFilter').hide();

            document.location.hash = '#itemId/' + dataItem.ProductId;

            my.itemId = dataItem.ProductId;

            my.loadItem();
        } else {

            if (dataItem.ProductsRelatedCount > 0) {

                // get relted products
                $.ajax({
                    url: '/desktopmodules/riw/api/products/getProductsRelated?portalId=' + portalID + '&productId=' + dataItem.ProductId + '&lang=pt-BR&relatedType=2&getAll=true'
                }).done(function (data) {
                    if (data) {
                        //my.vm.selectedProducts([]);

                        my.vm.estimatedItems.push(new my.Product().productId(dataItem.ProductId));

                        $.each(data, function (i, item) {
                            // push the new selected item to the view model selectedProducts
                            my.vm.selectedProducts.push(new my.Product()
                                .productId(item.RelatedProductId)
                                //.productRef(dataItem.ProductRef)
                                //.barcode(dataItem.Barcode)
                                .itemType(item.RelatedProductItemType)
                                .productCode(item.RelatedProductBarcode ? item.RelatedProductBarcode : (item.RelatedProductRef ? item.RelatedProductRef : ''))
                                .productName(item.RelatedProductName)
                                .summary(item.RelatedProductSummary)
                                .productUnit(item.RelatedProductUnit)
                                .unitValue(item.RelatedUnitValue)
                                .finan_Sale_Price(item.RelatedFinan_Sale_Price)
                                .finan_Special_Price(item.RelatedFinan_Special_Price)
                                .qTy(kendo.parseFloat($('#NumericTextBox_Qty_' + dataItem.ProductId).val() * item.ProductQty))
                                .qTyStockSet(item.RelatedQtyStockSet)
                                .showPrice(item.RelatedShowPrice)
                                .totalValue(kendo.parseFloat(($('#NumericTextBox_Qty_' + dataItem.ProductId).val() * item.ProductQty) * dataItem.UnitValue)));

                            my.vm.estimatedItems.push(new my.Product().productId(item.RelatedProductId));
                        });

                        // check for browser storage availability
                        if (my.storage) {

                            // if this checkbox is checked then add true to storage
                            if (my.storage) amplify.store.sessionStorage(siteURL + '_expandEstimate', true);

                            // convert view model selectedProducts to string and add to storage via amplify 
                            amplify.store.sessionStorage(siteURL + '_products', ko.toJSON(my.vm.selectedProducts()));

                            // convert view model estimatedItems to string and add to storage via amplify 
                            amplify.store.sessionStorage(siteURL + '_items', ko.toJSON(my.vm.estimatedItems()));

                            ko.utils.arrayForEach(my.vm.estimatedItems(), function (item) {
                                $('#btnEst_' + item.productId()).hide();
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
                    }
                }).fail(function (jqXHR, textStatus) {
                    console.log(jqXHR.responseText);
                });

                if (!amplify.store(siteURL + 'kitMsg')) {
                    // prompt user for redirection to estimate page
                    var $dialog = $('<div id="kitMsg"></div>')
                    .html('<p class="confirmDialog">O item escolhido conciste de um conjunto de itens em seu or&#231;amento.</p><label class="checkbox"><input type="checkbox" />N&#227;o mostrar mais esta mensagem.</label>')
                    .dialog({
                        autoOpen: false,
                        open: function () {
                            $(".ui-dialog-title").append('Aten&#231;&#227;o!');
                        },
                        modal: true,
                        resizable: true,
                        dialogClass: 'dnnFormPopup fixed-dialog',
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

                $('#kitMsg input').click(function (e) {
                    //if (my.storage) amplify.store.sessionStorage(_siteURL + '_expandEstimate', true);
                    if (my.storage) amplify.store(siteURL + 'kitMsg', true);
                });
            } else {

                // push the new selected item to the view model selectedProducts
                my.vm.selectedProducts.push(new my.Product()
                    .productId(dataItem.ProductId)
                    //.productRef(dataItem.ProductRef)
                    //.barcode(dataItem.Barcode)
                    .itemType(dataItem.ItemType)
                    .productCode(dataItem.Barcode ? '<strong>CB: </strong> ' + dataItem.Barcode : (dataItem.ProductRef ? '<strong>REF: </strong> ' + dataItem.ProductRef : ''))
                    .productName(dataItem.ProductName)
                    .summary(dataItem.Summary)
                    .productUnit(dataItem.ProductUnit)
                    .unitValue(dataItem.UnitValue)
                    .finan_Sale_Price(dataItem.Finan_Sale_Price)
                    .finan_Special_Price(dataItem.Finan_Special_Price)
                    .qTy(kendo.parseFloat($('#NumericTextBox_Qty_' + dataItem.ProductId).val()))
                    .qTyStockSet(dataItem.QtyStockSet)
                    .showPrice(dataItem.ShowPrice)
                    .totalValue(kendo.parseFloat($('#NumericTextBox_Qty_' + dataItem.ProductId).val()) * dataItem.UnitValue));

                my.vm.estimatedItems.push(new my.Product().productId(dataItem.ProductId));

                // check for browser storage availability
                if (my.storage) {

                    // if this checkbox is checked then add true to storage
                    if (my.storage) amplify.store.sessionStorage(siteURL + '_expandEstimate', true);

                    // convert view model selectedProducts to string and add to storage via amplify 
                    amplify.store.sessionStorage(siteURL + '_products', ko.toJSON(my.vm.selectedProducts()));

                    // convert view model estimatedItems to string and add to storage via amplify 
                    amplify.store.sessionStorage(siteURL + '_items', ko.toJSON(my.vm.estimatedItems()));

                    ko.utils.arrayForEach(my.vm.estimatedItems(), function (item) {
                        $('#btnEst_' + item.productId()).hide();
                    });
                }
            }

            $('.divButtons').fadeIn().css('display', 'inline-block');
            $('#divCheckExpand').fadeIn().css('display', 'inline-block');
            //$('#divCheckExpand input').attr({ 'checked': 'checked' });
            $('#divCheckExpand label span').html('Manter o or&#231;amento expandido.');

            //if (my.vm.selectedProducts().length === 1) {

            //    $.pnotify({
            //        title: 'Sucesso!',
            //        text: 'Or&#231;amento inicializado. ' + $('#NumericTextBox_Qty_' + dataItem.ProductId).data('kendoNumericTextBox').value() + ' do item <strong>' + dataItem.ProductName + '</strong> foi inserido no or&#231;amento.',
            //        type: 'success',
            //        icon: 'fa fa-check fa-lg',
            //        addclass: "stack-bottomright",
            //        stack: my.stack_bottomright
            //    });
            //} else {

            $.pnotify({
                title: 'Sucesso!',
                text: $('#NumericTextBox_Qty_' + dataItem.ProductId).data('kendoNumericTextBox').value() + ' do item <strong>' + dataItem.ProductName + '</strong> foi inserido no or&#231;amento.',
                type: 'success',
                icon: 'fa fa-check fa-lg',
                addclass: "stack-bottomright",
                stack: my.stack_bottomright
            });
            //}

            if (my.vm.selectedProducts().length === 1) {
                // check if my cart is on screen. if not, move the screeen to it
                //if (!$('#divMini_Estimate').is(':onScreen')) {
                $.scrollTo($('#configLink'), 600, {
                    easing: 'swing'
                });
                //}
            }

            // if cart item are hidden, show cart items
            if (!amplify.store.sessionStorage('estimateVisibility')) {

                // if false, show estimate area
                // show checkbox for keeping estimate area expanded
                $('#divCheckExpand').show();

                // expand estimate items
                $('#estimateItems').slideDown();

                // set expand cart items area's button text to hide cart and enable it
                $('#btnExpandCart').html('<span class="k-icon k-i-arrow-n"></span> Esconder Carrinho');

                //// apply effect to cart button's area
                //setTimeout(function () {
                //    $("#divMini_Estimate").effect("highlight", {
                //        color: 'blue'
                //    }, 2000);
                //}, 400);

                // check for browser storage availability 
                if (my.storage) {

                    // add estimateVisibility setting to storage and set it to true
                    amplify.store.sessionStorage('estimateVisibility', true);
                }
            }

            // check if logged in user has credentials to see amounts, like managers and sales person
            //my.authorizationCheck();
            if (authorized < 2) {
                //$('.columnValue').hide();
                //$('.tFoot').hide();

                // hide edit product pencil
                $('.editItem').hide();
            }

            $('.noStock').popover({
                delay: {
                    show: 500,
                    hide: 100
                },
                placement: 'top',
                trigger: 'hover'
            });

            // reset qty box to 1
            $('#NumericTextBox_Qty_' + dataItem.ProductId).data('kendoNumericTextBox').value('1');
        }
    };

    my.checkStorage = function () {

        // check for browser storage availability
        if (my.storage) {

            // check for items in storage via amplify
            if (amplify.store.sessionStorage(siteURL + '_products')) {

                // convert item from storage to ko view model
                var products = ko.utils.parseJson(amplify.store.sessionStorage(siteURL + '_products'));

                // add each item ko view model selectedproducts
                $.each(products, function (i, p) {
                    my.vm.selectedProducts.push(new my.Product()
                        .productId(p.productId)
                        //.productRef(p.productRef)
                        //.barcode(p.barcode)
                        .itemType(p.ItemType)
                        .productCode(p.productCode)
                        .productName(p.productName)
                        .summary(p.summary)
                        .productUnit(p.productUnit)
                        .unitValue(p.unitValue)
                        .finan_Sale_Price(p.finan_Sale_Price)
                        .finan_Special_Price(p.finan_Special_Price)
                        .qTy(p.qTy)
                        .qTyStockSet(p.qTyStockSet)
                        .showPrice(p.showPrice)
                        .totalValue(kendo.parseFloat(p.qTy) * p.unitValue));
                });

                //var match = ko.utils.arrayFirst(my.vm.selectedProducts(), function (p) {
                //    return p.productId() === my.itemId;
                //});
                //if (match) {
                //    $('#btnProductDetailEstimate').hide();
                //}

                // check if logged in user has credentials to see amounts, like managers and sales person
                //my.authorizationCheck();
                if (authorized < 2) {
                    //$('.columnValue').hide();
                    //$('.tFoot').hide();

                    // hide edit product pencil
                    $('.editItem').hide();
                }
            }

            // check for items in storage via amplify
            if (amplify.store.sessionStorage(siteURL + '_items')) {
                // convert item from storage to ko view model
                var items = ko.utils.parseJson(amplify.store.sessionStorage(siteURL + '_items'));

                // add each item ko view model selectedproducts
                $.each(items, function (i, p) {
                    my.vm.estimatedItems.push(new my.Product().productId(p.productId));
                    $('#btnEst_' + p.productId).hide();
                });

                var match = ko.utils.arrayFirst(my.vm.estimatedItems(), function (p) {
                    return p.productId() === my.itemId;
                });
                if (match) {
                    $('#btnProductDetailEstimate').hide();
                } else {
                    $('#btnProductDetailEstimate').show();
                }
            }
        }
    };

    my.checkStorage();

    my.loadItem = function () {

        var match = ko.utils.arrayFirst(my.vm.estimatedItems(), function (p) {
            return p.productId() === my.itemId;
        });
        if (match) {
            $('#btnProductDetailEstimate').hide();
        } else {
            $('#btnProductDetailEstimate').show();
        }

        //var match = ko.utils.arrayFirst(my.vm.selectedProducts(), function (p) {
        //    return p.productId() === kendo.parseInt(my.itemId);
        //});
        //if (match) {
        //    $('#btnProductDetailEstimate').hide();
        //}

        // get product
        $.ajax({
            url: '/desktopmodules/riw/api/products/getProduct?productId=' + my.itemId + '&lang=pt-BR'
        }).done(function (data) {
            if (data) {
                $('.listView').hide();
                $('#productDetail').show();
                my.vm.productId(my.itemId);
                my.vm.itemType(data.ItemType);
                my.vm.productImageExtension(data.Extension);
                my.vm.productImageId(data.ProductImageId);
                my.vm.productName(data.ProductName);
                my.vm.productUnit(data.ProductUnit);
                my.vm.qTyStockSet(data.QtyStockSet);
                my.vm.showPrice(data.ShowPrice);
                my.vm.unitTypeAbbv(data.UnitTypeAbbv);
                my.vm.productsRelatedCount(data.ProductsRelatedCount);
                my.vm.productCode(data.Barcode ? '<strong>CB: </strong> ' + data.Barcode : (data.ProductRef ? '<strong>REF: </strong> ' + data.ProductRef : ''));
                my.vm.unitValue(data.UnitValue);
                my.vm.finan_Sale_Price(data.Finan_Sale_Price);
                my.vm.finan_Special_Price(data.Finan_Special_Price);

                var converter = new Showdown.converter();
                my.vm.summary(converter.makeHtml(data.Summary));

                my.vm.description(converter.makeHtml(data.Description));

                $('#productDetail label').val('');
                $('#productDetail .form-horizontal').remove();

                my.vm.productOptions([]);

                if (data.ProductOptions) {
                    $.each(data.ProductOptions, function (i, option) {
                        my.vm.productOptions.push(option.OptionDesc);
                        var eleStr = '<div class="form-horizontal">'
                        eleStr += '<div class="control-group">'
                        eleStr += '<label id="label_' + option.OptionId + '" class="control-label for="option_' + option.OptionId + '"><strong>' + option.OptionDesc + ':</strong></label>'
                        eleStr += '<div class="controls">'
                        eleStr += '<select id="option_' + option.OptionId + '">'
                        eleStr += '</select>'
                        eleStr += '</div>'
                        eleStr += '</div>'
                        eleStr += '</div>'
                        $('#productOptions').append(eleStr);

                        var options = [];
                        $.each(data.ProductOptionValues, function (index, optionValue) {
                            if (option.OptionId === optionValue.OptionId) {
                                options.push($("<option/>", {
                                    value: optionValue.OptionValueId,
                                    text: optionValue.OptionValueDesc
                                }));
                            }
                        });
                        $('#option_' + option.OptionId).append(options);

                        $('#option_' + option.OptionId).kendoDropDownList();
                    });
                }

                $('#mt-thumbscroller').empty();
                my.vm.productImages([]);

                if (data.ProductImages) {
                    $.each(data.ProductImages, function (ind, img) {
                        my.vm.productImages.push(img.ProductImageId);
                        var imgUrl = '<li><a class="photo" title="' + my.vm.productName() + '" href="/databaseimages/' + img.ProductImageId + '.' + img.Extension + '?maxwidth=800&maxheight=600' + (watermark.length ? '&watermark=outglow&text=' + watermark : '') + '" style="cursor: url(/desktopmodules/rildoinfo/webapi/content/images/zoomin.cur), pointer;">'
                        imgUrl += '<img class="img-rounded thumbnail" src="/databaseimages/' + img.ProductImageId + '.' + img.Extension + '?w=35&h=45" /></a></li>';
                        $('#mt-thumbscroller').append(imgUrl);
                    });

                    if (data.ProductImages.length > 4) {
                        $('.thumbnailscroller').flexslider({
                            namespace: "thumbnail-",
                            animation: "slide",
                            animationLoop: true,
                            itemWidth: 40,
                            itemMargin: 10,
                            slideshow: false,
                            minItems: 4,
                            maxItems: 4,
                            directionNav: true,
                            controlNav: false,
                            animationSpeed: 500,
                            slideshowSpeed: 4000,
                            move: 1
                        });
                    } else {
                        $('#mt-thumbscroller').addClass('inline').css({ 'margin-bottom': '14px' });
                    }

                    // initiate colobox jquery plugin
                    //$('.photo_' + my.vm.productId()).colorbox();
                }

                $('#relatedProducts').empty();
                my.vm.productsRelated([]);

                if (data.ProductsRelated) {

                    $.each(data.ProductsRelated, function (i, rel) {
                        if (rel.RelatedType !== 2) {
                            my.vm.productsRelated.push(rel.RelatedProductId);
                            var imgUrl = '<li class="text-center"><h5>' + rel.RelatedProductName + '</h5><a class="photo" title="' + rel.RelatedProductName + '" href="/databaseimages/' + rel.RelatedProductImageId + '.' + rel.RelatedExtension + '?maxwidth=800&maxheight=600' + (watermark.length ? '&watermark=outglow&text=' + watermark : '') + '" style="cursor: url(/desktopmodules/rildoinfo/webapi/content/images/zoomin.cur), pointer;">'
                            imgUrl += '<img class="img-rounded thumbnail  slimmage" src="' + (rel.RelatedProductImageId > 0 ? ('/databaseimages/' + rel.RelatedProductImageId + '.' + rel.RelatedExtension + '?w=150&h=150') : '/desktopmodules/RildoInfo/Store/Content/Images/no-image.png') + '" /></a>';
                            imgUrl += '<h3><button class="btn btn-info" onclick="my.detailRelatedItem(' + rel.RelatedProductId + '); return false;">Detalhes</button></h3></li>';
                            $('#relatedProducts').append(imgUrl);
                        }
                    });
                }

                // initiate colobox jquery plugin
                $(".photo").colorbox();
            } else {
                $().toastmessage('showErrorToast', data.Result);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
    };

    if (my.itemId > 0) {

        my.loadItem();
    } else {
        $('#productDetail').hide();

        if (my.vm.selectedCatId() > 0) {
            $('#status').html('Categoria Selecionada: <strong>' + $('li[id=' + my.vm.selectedCatId() + ']').text() + '</strong>');
            $('#clearFilter').show();
        }

        // create initial list from datasource
        my.createListView();

        // create initial pager from datasource
        my.createPager();
    }

    // check if logged in user has credentials to see amounts, like managers and sales person
    my.authorizationCheck();

    $('#btnProductDetailEstimate').click(function (e) {
        e.preventDefault();

        var str = '';
        $.each($('#productOptions label'), function (o, ele) {
            str += ele.innerText + ' ';

            $.each($('#productOptions select'), function (s, sel) {
                if (ele.id.replace('label_', '') === sel.id.replace('option_', '')) {
                    str += $('#' + sel.id).data('kendoDropDownList').text() + ', ';
                }
            });
        });

        if (my.vm.productsRelatedCount() > 0) {

            // get relted products
            $.ajax({
                url: '/desktopmodules/riw/api/products/getProductsRelated?portalId=' + portalID + '&productId=' + my.vm.productId() + '&lang=pt-BR&relatedType=2&getAll=true'
            }).done(function (data) {
                if (data) {
                    //my.vm.selectedProducts([]);

                    my.vm.estimatedItems.push(new my.Product().productId(my.vm.productId()));

                    $.each(data, function (i, item) {
                        // push the new selected item to the view model selectedProducts
                        my.vm.selectedProducts.push(new my.Product()
                            .productId(item.RelatedProductId)
                            //.productRef(dataItem.ProductRef)
                            //.barcode(dataItem.Barcode)
                            .itemType(item.RelatedProductItemType)
                            .productCode(item.RelatedProductBarcode ? '<strong>CB: </strong> ' + item.RelatedProductBarcode : (item.RelatedProductRef ? '<strong>REF: </strong> ' + item.RelatedProductRef : ''))
                            .productName(item.RelatedProductName)
                            .summary(item.RelatedProductSummary)
                            .productUnit(item.RelatedProductUnit)
                            .unitValue(item.RelatedUnitValue)
                            .finan_Sale_Price(item.RelatedFinan_Sale_Price)
                            .finan_Special_Price(item.RelatedFinan_Special_Price)
                            .qTy(kendo.parseFloat(my.vm.productQty()))
                            .qTyStockSet(item.RelatedQTyStockSet)
                            .showPrice(item.RelatedShowPrice)
                            .totalValue(kendo.parseFloat(my.vm.productQty()) * item.RelatedUnitValue));

                        my.vm.estimatedItems.push(new my.Product().productId(item.RelatedProductId));
                    });

                    // check for browser storage availability
                    if (my.storage) {

                        // if this checkbox is checked then add true to storage
                        if (my.storage) amplify.store.sessionStorage(siteURL + '_expandEstimate', true);

                        // convert view model selectedProducts to string and add to storage via amplify 
                        amplify.store.sessionStorage(siteURL + '_products', ko.toJSON(my.vm.selectedProducts()));

                        // convert view model estimatedItems to string and add to storage via amplify 
                        amplify.store.sessionStorage(siteURL + '_items', ko.toJSON(my.vm.estimatedItems()));
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

            if (!amplify.store(siteURL + 'kitMsg')) {
                // prompt user for redirection to estimate page
                var $dialog = $('<div id="kitMsg"></div>')
                .html('<p class="confirmDialog">O item escolhido conciste de um conjunto de itens em seu or&#231;amento.</p><label class="checkbox"><input type="checkbox" />N&#227;o mostrar mais esta mensagem.</label>')
                .dialog({
                    autoOpen: false,
                    open: function () {
                        $(".ui-dialog-title").append('Aten&#231;&#227;o!');
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
            }

        } else {

            my.vm.estimatedItems.push(new my.Product().productId(my.vm.productId()));

            // push the new selected item to the view model selectedProducts
            my.vm.selectedProducts.push(new my.Product()
                .productId(my.itemId)
                .itemType(my.vm.itemType())
                .productCode(my.vm.productCode())
                .productName(my.vm.productName() + (str.length > 0 ? ' - ' + str : ''))
                .summary(my.vm.summary())
                .productUnit(my.vm.productUnit())
                .unitValue(my.vm.unitValue())
                .finan_Sale_Price(my.vm.finan_Sale_Price())
                .finan_Special_Price(my.vm.finan_Special_Price())
                .qTy(my.vm.productQty())
                .qTyStockSet(my.vm.qTyStockSet())
                .showPrice(my.vm.showPrice())
                .totalValue(kendo.parseFloat(my.vm.productQty()) * my.vm.unitValue()));

            // check for browser storage availability
            if (my.storage) {

                // if this checkbox is checked then add true to storage
                if (my.storage) amplify.store.sessionStorage(siteURL + '_expandEstimate', true);

                // convert view model selectedProducts to string and add to storage via amplify 
                amplify.store.sessionStorage(siteURL + '_products', ko.toJSON(my.vm.selectedProducts()));

                // convert view model estimatedItems to string and add to storage via amplify 
                amplify.store.sessionStorage(siteURL + '_items', ko.toJSON(my.vm.estimatedItems()));
            }

        }

        // disabled the selected item's button 
        $('#btnProductDetailEstimate').hide();

        $('.divButtons').show().css('display', 'inline-block');
        $('#divCheckExpand').show();
        $('#divCheckExpand input').attr({ 'checked': 'checked' });
        $('#divCheckExpand label span').html('Manter o or&#231;amento expandido.');

        //if (my.vm.selectedProducts().length === 1) {

        //    $.pnotify({
        //        title: 'Sucesso!',
        //        text: 'Or&#231;amento inicializado. ' + $('#productQtyTextBox').val() + ' do item <strong>' + my.vm.productName() + '</strong> foi inserido no or&#231;amento.',
        //        type: 'success',
        //        icon: 'fa fa-check fa-lg',
        //        addclass: "stack-bottomright",
        //        stack: my.stack_bottomright
        //    });
        //} else {

        $.pnotify({
            title: 'Sucesso!',
            text: $('#productQtyTextBox').val() + ' do item <strong>' + my.vm.productName() + '</strong> foi inserido no or&#231;amento.',
            type: 'success',
            icon: 'fa fa-check fa-lg',
            addclass: "stack-bottomright",
            stack: my.stack_bottomright
        });
        //}

        // check if my cart is on screen. if not, move the screeen to it
        //if (!$('#divMini_Estimate').is(':onScreen')) {
        //    $.scrollTo($('#divMini_Estimate'), 600, {
        //        easing: 'swing'
        //    });
        //}

        // if cart item are hidden, show cart items
        if (!amplify.store.sessionStorage('estimateVisibility')) {

            // if false, show estimate area
            // show checkbox for keeping estimate area expanded
            $('#divCheckExpand').show();

            // expand estimate items
            $('#estimateItems').slideDown();

            // set expand cart items area's button text to hide cart and enable it
            $('#btnExpandCart').html('<span class="k-icon k-i-arrow-n"></span> Esconder Carrinho');

            //// apply effect to cart button's area
            //setTimeout(function () {
            //    $("#divMini_Estimate").effect("highlight", {
            //        color: 'blue'
            //    }, 2000);
            //}, 400);

            // check for browser storage availability 
            if (my.storage) {

                // add estimateVisibility setting to storage and set it to true
                amplify.store.sessionStorage('estimateVisibility', true);
            }
        }

        // reset qty box to 1
        my.vm.productQty(1);

        // check if logged in user has credentials to see amounts, like managers and sales person
        //my.authorizationCheck();
        if (authorized < 2) {
            //$('.columnValue').hide();
            //$('.tFoot').hide();

            // hide edit product pencil
            $('.editItem').hide();
        }
    });

    // search for product and fill listview from new datasource
    $('#btnGetProducts').click(function (e) {
        e.preventDefault();

        if (my.vm.filterTerm().length > 0) {
            //my.vm.quick(2);
            //my.products.read();
            if (my.itemId > 0) {
                history.pushState("", document.title, window.location.pathname);
            }
            my.loadProducts();
        }
    });

    $('#clearFilter').click(function (e) {
        e.preventDefault();

        my.vm.categoryId(0);
        history.pushState("", document.title, window.location.pathname);
        $('#status').html('');
        $(this).hide();
        my.vm.filterTerm('');
        my.products.read();
    });

    $('#btnDetailReturn').click(function (e) {
        e.preventDefault();

        my.vm.categoryId(0);
        history.pushState("", document.title, window.location.pathname);
        $('#status').html('');
        //$(this).hide();
        my.vm.filterTerm('');
        $('#productDetail').hide();

        // create initial list from datasource
        my.createListView();

        // create initial pager from datasource
        my.createPager();
    });

});

// funtion to open an image in parent using colorbox
//This function only needs to be available in the parent window, but no harm in loading it for both. Notice this is NOT in the $(document).ready on purpose.
function showColorBox(imageURL) {
    $.fn.colorbox({
        opacity: 0.6,
        open: true,
        href: imageURL
    });
}