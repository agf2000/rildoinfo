
$(function () {

    // allocate memory for selected grid item's uid
    my.uId = null;
    //my.deleted = false;
    my.cats = null;

    // knockout js view model
    my.vm = function () {
        // this is knockout view model
        var self = this;

        // view models
        self.categoryId = ko.observable(),
        self.filter = ko.observable(''),
        self.deleteds = ko.observable();

        // make view models available for apps
        return {
            filter: filter,
            categoryId: categoryId,
            deleteds: deleteds
        };

    }();

    my.htmlConverter = new Showdown.converter();

    // apply ko bindings
    ko.applyBindings(my.vm);

    $('#moduleTitleSkinObject').html(moduleTitle);

    var buttons = '<ul class="inline">';
    buttons += '<li><a href="' + categoriesManagerURL + '" class="btn btn-primary btn-medium" title="Categorias"><i class="fa fa-sitemap fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + productsManagerURL + '" class="btn btn-primary btn-medium" title="Produtos"><i class="fa fa-barcode fa-lg"></i></a></li>';
    buttons += '<li><a href="' + peopleManagerURL + '" class="btn btn-primary btn-medium" title="Entidades"><i class="fa fa-group fa-lg"></i></a></li>';
    buttons += '<li><a href="' + usersManagerURL + '" class="btn btn-primary btn-medium" title="Colaboradores"><i class="fa fa-suitcase fa-lg"></i></a></li>';
    buttons += '<li><a href="' + invoicesManagerURL + '" class="btn btn-primary btn-medium" title="Lançamentos"><i class="fa fa-money fa-lg"></i></a></li>';
    buttons += '<li><a href="' + accountsManagerURL + '" class="btn btn-primary btn-medium" title="Contas"><i class="fa fa-book fa-lg"></i></a></li>';
    buttons += '<li><a href="' + agendaURL + '" class="btn btn-primary btn-medium" title="Agenda"><i class="fa fa-calendar fa-lg"></i></a></li>';
    buttons += '<li><a href="' + orURL + '" class="btn btn-primary btn-medium" title="Website"><i class="fa fa-shopping-cart fa-lg"></i></a></li>';
    buttons += '<li><a href="' + reportsManagerURL + '" class="btn btn-primary btn-medium" title="Relatórios"><i class="fa fa-bar-chart-o fa-lg"></i></a></li>';
    buttons += '<li><a href="' + storeManagerURL + '" class="btn btn-primary btn-medium" title="Loja"><i class="fa fa-cogs fa-lg"></i></a></li>';
    buttons += '</ul>';
    $('#buttons').html(buttons);

    function getDeleteds() {
        $.get('/desktopmodules/riw/api/products/GetDeletedProductsCount?portalId=' + portalID, function (data) {
            setTimeout(function () {
                my.vm.deleteds(data.productCount);
            }, 200);
        });
    };

    getDeleteds();

    my.loadCategories = function () {
        if (!my.cats) {
            var result = null;
            $.ajax({
                url: '/desktopmodules/riw/api/categories/Categories?portalId=' + portalID + '&lang=pt-BR&includeArchived=true',
                async: false,
                success: function (data) {
                    result = data;
                }
            });
            my.cats = result;
        } else {
            return my.cats;
        }
    };

    my.loadCategories();
    my.buildMenudata = function () {
        var source = [];
        var categoryItems = [];
        for (i = 0; i < my.cats.length; i++) {
            var item = my.cats[i];
            var text = item.CategoryName;
            var id = item.CategoryId;
            var desc = item.CategoryDesc;
            var parentid = item.ParentCategoryId;
            var code = item.CategoryId;

            if (categoryItems[parentid]) {
                item = {
                    parentid: parentid,
                    text: text,
                    id: id,
                    desc: desc,
                    item: item
                };
                if (!categoryItems[parentid].items) {
                    categoryItems[parentid].items = [];
                }
                categoryItems[parentid].items[categoryItems[parentid].items.length] = item;
                categoryItems[code] = item;
            } else {
                categoryItems[code] = {
                    parentid: parentid,
                    text: text,
                    id: id,
                    desc: desc,
                    item: item
                };
                source[code] = categoryItems[code];
            }
        }
        return source;
    };
    my.sourceMenu = my.buildMenudata();

    my.buildMenuUL = function (parent, items) {
        $.each(items, function () {
            if (this.text) {
                // create LI element and append it to the parent element.
                var li = $("<li class='text-t-c' id='" + this.item.CategoryId + "'>" + this.text + "</li>");
                li.appendTo(parent);
                // if there are sub items, call the buildUL function.
                if (this.items && this.items.length > 0) {
                    var ul = $("<ul></ul>");
                    ul.appendTo(li);
                    my.buildMenuUL(ul, this.items);
                }
            }
        });
    };
    my.ulMenu = $("<ul></ul>");

    my.ulMenu.appendTo("#tvCategories");
    my.buildMenuUL(my.ulMenu, my.sourceMenu);

    $('#tvCategories').jqxTree();
    $('#tvCategories').on('select', function (event) {
        var args = event.args;
        var item = $('#tvCategories').jqxTree('getItem', args.element);
        if ($('#selectedCats li[id=' + item.id + ']').length === 0) {
            $('#selectedCats').append('<li class="select2-search-choice text-t-c" id="' + item.id + '"><div>' + item.label + '</div class="text-t-c"><a id="' + item.id + '" href="#" onclick="my.removeCat(this); return false;" class="select2-search-choice-close" tabindex="-1"></a></li>');
            $('#tvCategories').slideUp();
        } else {
            $('#tvCategories').slideUp();
        }
    });

    var hiddenContent = $("#tvCategories");
    $("#selectedCats").click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var pos = $('#selectedCats').position();
        hiddenContent.css({
            top: (pos.top + 28) + 'px',
            left: pos.left + 'px'
        });

        // Check to see if the content is visible.
        if (hiddenContent.is(":visible")) {
            // Hide it slowly.
            hiddenContent.slideUp();
        } else {
            // Show it slowly.
            hiddenContent.slideDown();
        }
    });

    //$('#selectedCats').on('click', function (obj) {
    //    if (obj.target.className !== "select2-search-choice-close") {
    //        $('#tvCategories').toggle(function () {

    //        });
    //    }
    //});

    $('#tvCategories').on('focusout', function () {
        $('#tvCategories').slideUp();
    });

    my.removeCat = function (obj) {
        $('#selectedCats li[id=' + obj.id + ']').fadeOut();
        $('#selectedCats li[id=' + obj.id + ']').remove();
        return false;
    };

    $('#categoriesTextBox').on('keypress', function () {
        $('#tvCategories').slideUp();
    });

    //my.vendorsDataSource = new kendo.data.DataSource({
    //    transport: {
    //        read: {
    //            url: '/desktopmodules/riw/api/people/getPeople',
    //        },
    //        parameterMap: function (data, type) {
    //            return {
    //                portalId: portalID,
    //                registerType: 8,
    //                isDeleted: false,
    //                sTerm: data.filter ? data.filter.filters[0].value : '',
    //                pageIndex: data.page,
    //                pageSize: data.pageSize,
    //                searchDesc: my.convertSortingParameters(data.sort)
    //            };
    //        }
    //    },
    //    pageSize: 1000,
    //    serverPaging: true,
    //    serverSorting: true,
    //    serverFiltering: true,
    //    sort: {
    //        field: "DisplayName",
    //        dir: "ASC"
    //    },
    //    schema: {
    //        model: {
    //            id: 'PersonId'
    //        },
    //        data: 'data',
    //        total: 'total'
    //    }
    //});

    $('#selectVendors').select2({
        placeholder: "Busque por fornecedores...",
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
                    registerType: providerRoleId,
                    isDeleted: false,
                    searchField: 'CompanyName',
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

    $('#kdpStartDate').kendoDatePicker();
    $('#kdpEndDate').kendoDatePicker();

    $('#btnIsDeleted').kendoButton();
    $('#btnSyncProducts').kendoButton();

    //$('#clientSearchBox').on("select2-selecting", function (e) {
    //    my.personId = e.val.PersonId;
    //});

    //$('#selectVendors').kendoDropDownList({
    //    dataSource: my.vendorsDataSource,
    //    //minLength: 1,
    //    //filter: "startswith",
    //    autoBind: false,
    //    dataTextField: 'DisplayName',
    //    dataValueField: 'PersonId',
    //    optionLabel: 'Todos Fornecedores...',
    //    dataBound: function () {
    //        if (this.dataSource.total() === 0) {
    //            //if (!$('.toast-item-wrapper').is(":visible")) $().toastmessage('showWarningToast', 'Sua busca n&#227;o trouxe resultado algum!');
    //            if (!$('.ui-pnotify').is(":visible")) {
    //                $.pnotify({
    //                    title: 'Aten&#231;&#227;o!',
    //                    text: 'N&#227;o existem fornecedores cadastrados.',
    //                    type: 'warning',
    //                    icon: 'fa fa-warning fa-lg',
    //                    addclass: "stack-bottomright",
    //                    stack: my.stack_bottomright
    //                });
    //            }
    //        }
    //    }
    //});

    function detailInit(e) {
        var detailRow = e.detailRow;
        var prodId = e.data.ProductId;

        var lvImages = detailRow.find('.imagesListView');
        $(lvImages).kendoListView({
            dataSource: new kendo.data.DataSource({
                transport: {
                    read: {
                        url: '/desktopmodules/riw/api/products/getProductImages?productId=' + e.data.ProductId
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
                $('.group_' + prodId).colorbox({ rel: 'group_' + prodId });
            }
        });

        $('.photo').colorbox();
    }

    // create kendo dataSource transport to get products
    my.productsTransport = {
        read: {
            url: '/desktopmodules/riw/api/products/getProducts'
        },
        parameterMap: function (data, type) {
            var categories = '';
            $('#selectedCats li').each(function (i) {
                categories += kendo.toString($('#selectedCats li')[i].id + ',');
            });
            categories = categories.substring(0, categories.length - 1);

            return {
                portalId: portalID,
                categoryId: my.vm.categoryId(),
                searchField: my.vm.filter().length ? $('#kddlFields').data('kendoDropDownList').value() : 'ALL',
                searchString: my.vm.filter().length ? my.vm.filter() : '',
                getDeleted: $('#chkDeleted').is(':checked'),
                getArchived: 'True',
                providerList: $('#selectVendors').select2('val'),
                categoryList: categories,
                sDate: $('#kdpStartDate').val().length > 0 ? moment(new Date($('#kdpStartDate').data('kendoDatePicker').value())).format() : '',
                eDate: $('#kdpEndDate').val().length > 0 ? moment(new Date($('#kdpEndDate').data('kendoDatePicker').value())).format() : '',
                filterDate: $('#ddlDates').data('kendoDropDownList').value(),
                pageIndex: data.page,
                pageSize: data.pageSize,
                orderBy: data.sort[0] ? data.sort[0].field : 'ProductName',
                orderDesc: data.sort[0] ? data.sort[0].dir : ''
            };
        }
    };

    // create kendo dataSource for getting products transport
    my.products = new kendo.data.DataSource({
        transport: my.productsTransport,
        pageSize: 10,
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
                    ProductId: {
                        editable: false, nullable: false
                    },
                    UnitValue: {
                        type: 'number'
                    },
                    QtyStockSet: {
                        editable: false, nullable: false
                    },
                    NewStock: {
                        type: 'number'
                    },
                    QtyStockOther: {
                        editable: false, nullable: false
                    }
                    //ModifiedDate: {
                    //    type: "date", format: "{0:dd/MM/yyyy}", editable: false, nullable: false
                    //}
                }
            },
            data: 'data',
            total: 'total'
        }
    });

    $("#productsGrid").kendoGrid({
        dataSource: my.products,
        //height: 380,
        selectable: "row",
        change: function (e) {
            var row = this.select();
            var id = row.data("uid");
            my.uId = id;
            var dataItem = this.dataItem(row);
            if (dataItem) {
                $('#btnRemoveSelected').hide();
                $('#btnDeleteSelected').hide();
                $('#btnRestoreSelected').hide();
                $('#btnEditSelected').attr({ 'disabled': false });
                $('#btnExportProducts').attr({ 'disabled': false });
                $('#btnExportProduct').attr({ 'disabled': false });

                if (dataItem.Locked) {
                    if (dataItem.IsDeleted) {
                        $('#btnRestoreSelected').show();
                        $('#btnRemoveSelected').hide();
                    } else {
                        if (authorized > 2) {
                            $('#btnDeleteSelected').show();
                            $('#btnRemoveSelected').hide();
                            //} else {
                            //    $('#btnRemoveSelected').hide();
                        }
                    }
                } else {
                    if (dataItem.IsDeleted) {
                        $('#btnRestoreSelected').show();
                        $('#btnRemoveSelected').show();
                    } else {
                        if (authorized > 2) {
                            $('#btnRemoveSelected').show();
                            $('#btnDeleteSelected').show();
                        }
                    }
                }

                if (authorized < 3) {
                    $('#btnAddNewProduct').hide();
                    $('#btnEditSelected').hide();
                    $('#btnExportProduct').hide();
                    $('#btnUpdateProducts').hide();
                }
            }
        },
        toolbar: kendo.template($("#tmplToolbar").html()),
        columns: [
            {
                field: "ProductId", title: "ID", width: 50 //, template: kendo.template($("#productLinkTMPL").html())
            },
            //{ field: "Name", title: "Categoria(a)", width: 120, template: '#=  my.htmlEncode(Name) #' },
            {
                field: "Barcode", title: "Cod. de Barra", width: 105
            },
            {
                field: "ProductRef", title: "Referência", width: 85
            },
            {
                field: "ProductName", title: "Produto"
            },
            {
                field: "UnitValue", title: "Preço", width: 90, format: '{0:n}', attributes: { class: 'text-right' }, editor: editNumber
            },
            //{ field: "Finan_Special_Price", title: "Especial", width: 75, template: '#= kendo.toString(Finan_Special_Price, "n") #' },
            {
                title: 'Estoque',
                columns: [{
                    field: "QtyStockSet", title: "Real", width: 80, attributes: { class: 'text-right' }
                },
                {
                    field: "NewStock", title: "Novo", width: 80, editor: editNumber, sortable: false
                },
                {
                    field: "QtyStockOther", title: "Fiscal", width: 80
                }]
            }
            //{ field: "ModifiedOnDate", title: "Data", type: "date", format: "{0:g}", width: 130 }
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
                display: "{0} - {1} de {2} Produtos",
                empty: "Sem Registro.",
                page: "Página",
                of: "de {0}",
                itemsPerPage: "Produtos por página",
                first: "Ir para primeira página",
                previous: "Ir para página anterior",
                next: "Ir para próxima página",
                last: "Ir para última página",
                refresh: "Recarregar"
            }
        },
        dataBound: function () {
            $('#btnRemoveSelected').hide();
            $('#btnDeleteSelected').hide();
            $('#btnRestoreSelected').hide();
            $('#btnEditSelected').attr({ 'disabled': true });
            $('#btnUpdateProducts').attr({ 'disabled': true });
            $('#btnExportProduct').attr({ 'disabled': true });
            if (this.dataSource.view().length > 0) {
                var grid = this;
                for (var i = 0; i < grid.dataSource.view().length ; i++) {
                    var dataItem = grid.dataSource.view()[i];
                    var rowSelector = ">tr:nth-child(" + (i + 1) + ")";
                    //Grab a reference to the corrosponding data row
                    var row = grid.tbody.find(rowSelector);
                    if (dataItem.QtyStockSet <= 0) {
                        row.addClass('negativeStock');
                    }
                }
            }

            if (authorized < 3) {
                $('#btnAddNewProduct').hide();
                $('#btnEditSelected').hide();
                $('#btnUpdateProducts').hide();
            }

            $("input[type=text]").on('focusin', function () {
                var saveThis = $(this);
                window.setTimeout(function () {
                    saveThis.select();
                }, 100);
            });
        },
        editable: true,
        detailTemplate: kendo.template($("#tmplProductDetail").html()),
        detailInit: detailInit
    });

    $('#productsGrid').data().kendoGrid.dataSource.bind('change', function (e) {
        //the event argument here will indicate what action just happned
        // console.log(e.action)// could be => "itemchange","add" or "remove" if you made any changes to the items
        if (e.action === "itemchange") {
            $('#btnUpdateProducts').attr({ 'disabled': false });
        }
    });

    $("#productsGrid").delegate("tbody > tr", "dblclick", function () {
        $('#btnEditSelected').click();
    });

    // search for product and fill grid from new datasource
    $('#btnSearch').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        my.products.read();
    });

    $("#btnAddNewProduct").click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();
        //document.location.href = _editItemURL;

        $("#productWindow").append("<div id='window'></div>");
        var sContent = editItemURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer',
            kendoWindow = $('#window').kendoWindow({
                actions: ["Maximize", "Close"],
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
                    my.products.read();
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

    $('#btnEditSelected').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var grid = $('#productsGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        $("#productWindow").append("<div id='window'></div>");
        var sContent = editItemURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + dataItem.ProductId + '/sel/1',
            kendoWindow = $('#window').kendoWindow({
                actions: ["Maximize", "Close"],
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
                    getDeleteds();
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
    });

    $('#btnUpdateProducts').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var grid = $('#productsGrid').data("kendoGrid");
        //var dataItem = grid.dataSource.getByUid(my.uId);

        var params = [];
        $.each(grid.dataSource.data(), function (i, product) {
            if (product.dirty) {
                params.push({
                    ProductId: product.ProductId,
                    ProductRef: product.ProductRef,
                    Barcode: product.Barcode,
                    ProductName: product.ProductName,
                    Finan_Sale_Price: parseFloat(product.UnitValue),
                    Lang: 'pt-BR',
                    QtyStockSet: product.NewStock,
                    ModifiedByUser: userID,
                    ModifiedOnDate: moment().format(),
                    SyncEnabled: JSON.parse(amplify.store.sessionStorage('syncEnabled').toLowerCase())
                });
            }
        });

        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/products/updateProductStock',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.each(grid.dataSource.data(), function (i, item) {
                    if (item.dirty) {
                        item.set('QtyStockSet', item.QtyStockSet + item.NewStock)
                        item.set('ProductRef', item.ProductRef)
                        item.set('Barcode', item.Barcode)
                        item.set('ProductName', item.ProductName)
                        item.set('Finan_Sale_Price', item.UnitValue)
                        item.set('NewStock', '')
                    }
                });
                grid.refresh();
                //$('#btnDeleteSelected').hide();
                //$('#btnRemoveSelected').hide();
                //$('#btnRestoreSelected').hide();
                //$().toastmessage('showSuccessToast', 'Produto atualizado com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Produto atualizado com sucesso.',
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

    $('#btnSyncProducts').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#SyncMsg').text('');

        var $this = $(this);
        $this.button('loading');

        var theDate = new Date();

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/products/SyncProducts?sDate=' + moment(new Date(2017, theDate.getMonth(), theDate.getDate() - 7)).format('YYYY-MM-DD') + '&eDate=' + moment(new Date()).add(1, 'days').format('YYYY-MM-DD')
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Sincronização completa.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                $('#SyncMsg').html('Inseridos <strong>' + data.Added.toString() + '</strong> - Atualizados <strong>' + data.Updated.toString() + '</strong>.')
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

    $('#btnExportAll').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        //var newWindow = window.open('', '_blank');

        var $this = $(this);
        $this.button('loading');

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/products/ExportAllProducts?lang=pt-BR&portalId=' + portalID
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                var kendoWindow = $("<div />").kendoWindow({
                    title: "Baixar Arquivo",
                    resizable: false,
                    modal: true,
                    width: 550
                });

                kendoWindow.data("kendoWindow")
                    .content('<h3 class="DNNAligncenter">Produtos</h3><h5 class="DNNAligncenter"><a href="/portals/0/products/Produtos.txt">Produtos.txt</a></h5><h6 class="DNNAligncenter">Clique com o botão da direita do mouse no link e escolha a opção "salvar link".</h6>')
                    .center().open();
                //newWindow.location = 'http://' + _siteURL + '/portals/0/Products/produtos.TXT';
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

    $('#btnExportProduct').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        //var newWindow = window.open('', '_blank');

        //var $this = $(this);
        //$this.button('loading');

        var grid = $('#productsGrid').data("kendoGrid");

        var dataItem = grid.dataSource.getByUid(my.uId);

        var kendoWindow = $("<div />").kendoWindow({
            title: "Baixar Arquivo",
            resizable: false,
            modal: true,
            width: 550
        });

        kendoWindow.data("kendoWindow")
            .content('<h3 class="DNNAligncenter">' + dataItem.ProductName + '</h3><h5 class="DNNAligncenter"><a href="/portals/0/products/' + dataItem.ProductId.toString() + '.txt">' + dataItem.ProductId.toString() + '.txt</a></h5><h6 class="DNNAligncenter">Clique com o botão da direita do mouse no link e escolha a opção "salvar link".</h6>')
            .center().open();

        //$.ajax({
        //    type: 'POST',
        //    contentType: 'application/json; charset=utf-8',
        //    beforeSend: function (xhr) {
        //        xhr.setRequestHeader("Accept", "application/txt");
        //    },
        //    url: '/desktopmodules/riw/api/products/ExportProduct?productId=' + dataItem.ProductId + '&lang=pt-BR&portalId=' + _portalID
        //}).done(function (data) {
        //    if (data.Result.indexOf("success") !== -1) {
        //        //$("#tmpFrame").attr('src', '/portals/0/Products/E' + data.FileName + '.txt');
        //        newWindow.location = 'http://' + _siteURL + '/portals/0/Products/E' + data.FileName + '.txt';
        //        //$.fileDownload('http://' + _siteURL + '/portals/0/Products/E' + data.FileName + '.txt');
        //    } else {
        //        $.pnotify({
        //            title: 'Erro!',
        //            text: data.Result,
        //            type: 'error',
        //            icon: 'fa fa-times-circle fa-lg',
        //            addclass: "stack-bottomright",
        //            stack: my.stack_bottomright
        //        });
        //    }
        //}).fail(function (jqXHR, textStatus) {
        //    console.log(jqXHR.responseText);
        //}).always(function () {
        //    $this.button('reset');
        //});
    });

    $('#btnRemoveSelected').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var grid = $('#productsGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

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
                                        $this.button('loading');
                                        $.ajax({
                                            type: 'DELETE',
                                            url: '/desktopmodules/riw/api/products/removeProduct?productId=' + dataItem.ProductId + '&lang=pt-BR'
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                my.products.read();
                                                $('#btnRemoveSelected').hide();
                                                $('#btnDeleteSelected').hide();
                                                $('#btnRestoreSelected').hide();
                                                //$().toastmessage('showSuccessToast', 'Produto excluido com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Item excluido.',
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
                                            $dialog.dialog('close');
                                            $dialog.dialog('destroy');
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
    });

    $('#btnDeleteSelected').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var grid = $('#productsGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        var $dialog = $('<div></div>')
                        .html('<div class="confirmDialog">Tem Certeza?</div>')
                        .dialog({
                            autoOpen: false,
                            modal: true,
                            resizable: false,
                            dialogClass: 'dnnFormPopup',
                            open: function () {
                                $(".ui-dialog-title").append('Aviso de Desativa&#231;&#227;o');
                            },
                            buttons: {
                                'ok': {
                                    text: 'Sim',
                                    //priority: 'primary',
                                    "class": 'dnnPrimaryAction',
                                    click: function () {
                                        $this.button('loading');
                                        var params = {
                                            ProductId: dataItem.ProductId,
                                            Lang: 'pt-BR',
                                            ModifiedByUser: userID,
                                            ModifiedOnDate: moment().format()
                                        };

                                        $.ajax({
                                            type: 'PUT',
                                            url: '/desktopmodules/riw/api/products/deleteProduct',
                                            data: params
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                getDeleteds();
                                                my.products.read();
                                                $('#btnRemoveSelected').hide();
                                                $('#btnDeleteSelected').hide();
                                                $('#btnRestoreSelected').hide();
                                                //$().toastmessage('showSuccessToast', 'Produto desativado com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Item desativado.',
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
                                            $this.button('loading');
                                            $dialog.dialog('close');
                                            $dialog.dialog('destroy');
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
    });

    $('#btnRestoreSelected').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var grid = $('#productsGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        var params = {
            ProductId: dataItem.ProductId,
            Lang: 'pt-BR',
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };

        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/products/restoreProduct',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $('#chkDeleted').click();
                if (my.storage) amplify.store.sessionStorage(siteURL + '_chkDeleted', false);
                $('#btnIsDeleted').text('Mostrar Desativados');
                getDeleteds();
                my.products.read();
                $('#btnRemoveSelected').hide();
                $('#btnDeleteSelected').hide();
                $('#btnRestoreSelected').hide();
                //$().toastmessage('showSuccessToast', 'Produto ativado com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Item ativado.',
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
                //$().toastmessage('showErrorToast', data.Msg);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
    });

    // open product details in kendo window
    //my.showItem = function (uid) {
    //    var dataItem = $('#productsGrid').data("kendoGrid").dataSource.getByUid(uid);

    //    var kendoWindow = $('<div />').kendoWindow({
    //        actions: ["Maximize", "Close"],
    //        title: dataItem.ProdName,
    //        resizable: true,
    //        modal: true,
    //        width: '90%',
    //        height: '80%',
    //        open: function (e) {
    //            $("html, body").css("overflow", "hidden");
    //        },
    //        close: function (e) {
    //            $("html, body").css("overflow", "");
    //        },
    //        content: _detailURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#pid/' + dataItem.ProductId
    //        //deactivate: function () {
    //        //    this.destroy();
    //        //}
    //    });

    //    kendoWindow.css({
    //        'overflow': 'hidden'
    //    });
    //    kendoWindow.data('kendoWindow').center().open();
    //};

    $('#btnIsDeleted').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();
        $('#chkDeleted').click();
        if (my.storage) amplify.store.sessionStorage(siteURL + '_chkDeleted', $('#chkDeleted').is(':checked'));
        $('#chkDeleted').is(':checked') ? $('#btnIsDeleted').text('Esconder Desativados') : $('#btnIsDeleted').text('Mostrar Desativados (' + my.vm.deleteds() + ')');
        my.products.read();
    });

});

function editNumber(container, options) {
    $('<input data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoNumericTextBox({
            spinners: false
        });
}

function clientFormatResult(data) {
    return '<strong>Cliente: </strong><span>' + data.FirstName + ' ' + data.LastName + ' / ' + data.DisplayName + '</span>'
        + '<br /><strong>Email: </strong><span>' + data.Email + '</span>'
        + '<br /><strong>Telefone: </strong><span>' + data.Telephone + '</span>'
        + '<br /><strong>Endere&#231;o: </strong><span>' + data.Street + ' ' + data.Unit + '</span>';
}

function clientFormatSelection(data) {
    return data.DisplayName;
}