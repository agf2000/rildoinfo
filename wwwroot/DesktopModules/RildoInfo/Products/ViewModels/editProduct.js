
$(function () {

    // allocate memory for selected listview item's uid
    my.uId = null;
    my.productId = my.getParameterByName('itemId');
    my.cats = null;
    
    // provider view model
    my.Vendor = function () {
        this.personId = ko.observable();
        this.displayName = ko.observable();
    };

    // unit type view model
    my.UnitType = function () {
        this.unitId = ko.observable();
        this.unitName = ko.observable();
    };

    // knockout js view model
    my.vm = function () {
        // this is knockout view model
        var self = this;

        // view models
        self.itemType = ko.observable('1'),
        self.unitTypes = ko.observableArray([]),
        self.selectedUnit = ko.observable();
        self.brands = ko.observableArray([]),
        self.selectedBrandId = ko.observable(0),
        self.models = ko.observableArray([]),
        self.selectedModelId = ko.observable(0),
        self.unitTypeList = ko.observable(),
        
        //ajax function to get all unit types
        self.loadUnitTypes = function () {
            $.ajax({
                url: '/desktopmodules/riw/api/unittypes/GetUnitTypes?portalId=' + portalID
            }).done(function (data) {
                self.unitTypes.removeAll();
                $.each(data, function (i, u) {
                    self.unitTypes.push(new my.UnitType()
                        .unitId(u.UnitTypeId)
                        .unitName(u.UnitTypeAbbv + ' (' + u.UnitTypeTitle + ')'));
                });
                ko.utils.arrayForEach(self.unitTypes(), function (unit) {
                    if (unit.unitName() === 'Unidade') {
                        self.unitTypeList().value(unit.unitId());
                    }
                });
            });
        };

        // make view models available for apps
        return {
            itemType: itemType,
            unitTypes: unitTypes,
            loadUnitTypes: loadUnitTypes,
            brands: brands,
            selectedBrandId: selectedBrandId,
            models: models,
            selectedModelId: selectedModelId,
            unitTypeList: unitTypeList
        };

    }();

    // apply ko bindings
    ko.applyBindings(my.vm);

    $('#btnAddModel').attr({ 'disabled': true });
    $('#btnAddBrand').attr({ 'disabled': true });
    $('#showPriceCheckBox').bootstrapSwitch();

    // create kendo dataSource transport to get unit types
    my.unitTypesTransport = {
        read: {
            url: '/desktopmodules/riw/api/unittypes/GetUnitTypes?portalId=' + portalID
        }
    };

    // create kendo dataSource for getting unit types transport
    my.unitTypesData = new kendo.data.DataSource({
        transport: my.unitTypesTransport,
        sort: {
            field: "UnitTypeAbbv",
            dir: "False"
        },
        schema: {
            model: {
                id: 'UnitTypeId'
            }
        }
    });

    //$('#ddlUnitTypes').kendoDropDownList({
    //    //autoBind: false,
    //    dataSource: my.unitTypesData,
    //    dataTextField: 'UnitTypeTitle',
    //    dataValueField: 'UnitTypeId'
    //});

    //ko.bindingHandlers.kendoNumericTextBox.options = {
    //    min: 0,
    //    format: "0.00 '%'",
    //    decimal: 2
    //};

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

    $('#btnAddCat').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $(this).attr({ 'disabled': true });

        document.location.href = categoriesURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#itemId/' + my.productId + '/return/1';

        //$('body').append("<div id='window2'></div>");
        //var sContent = _categoriesURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#return/1',
        //    kendoWindow = $('#window2').kendoWindow({
        //        actions: ["Maximize", "Close"],
        //        title: 'Gerenciador de Categorias',
        //        resizable: true,
        //        modal: true,
        //        width: '90%',
        //        height: '80%',
        //        content: sContent,
        //        open: function (e) {
        //            $("html, body").css("overflow", "hidden");
        //            $('#window2').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
                    
        //            if (parent.$('#window2').data('kendoWindow')) {
        //                parent.$('#window2').data('kendoWindow').maximize();
        //            }
        //        },
        //        close: function (e) {
        //            $("html, body").css("overflow", "");
        //            my.loadCategories();
        //            if (parent.$('#window2').data('kendoWindow')) {
        //                parent.$('#window2').data('kendoWindow').toggleMaximization();
        //            }
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

    my.vendorsDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/desktopmodules/riw/api/people/getPeople',
            },
            parameterMap: function (data, type) {
                return {
                    portalId: portalID,
                    registerType: 9,
                    isDeleted: false,
                    sTerm: data.filter ? data.filter.filters[0].value : '',
                    pageIndex: data.page,
                    pageSize: data.pageSize,
                    searchDesc: my.convertSortingParameters(data.sort)
                };
            }
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
    });

    $('#selectVendors').kendoMultiSelect({
        dataSource: my.vendorsDataSource,
        minLength: 1,
        filter: "startswith",
        autoBind: false,
        dataTextField: 'DisplayName',
        dataValueField: 'PersonId',
        placeholder: 'Busque por fornecedores...',
        dataBound: function () {
            if (this.dataSource.total() === 0) {
                //if (!$('.toast-item-wrapper').is(":visible")) $().toastmessage('showWarningToast', 'Sua busca n&#227;o trouxe resultado algum!');
                if (!$('.ui-pnotify').is(":visible")) {
                    $.pnotify({
                        title: 'Aten&#231;&#227;o!',
                        text: 'Sua busca n&#227;o trouxe resultado algum.',
                        type: 'warning',
                        icon: 'fa fa-warning fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                }
            }
        }
    });
    //my.vm.loadVendors();

    my.vm.loadUnitTypes();

    my.brandsDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/desktopmodules/riw/api/brands/GetBrands',
            },
            parameterMap: function (data, type) {
                return {
                    portalId: portalID,
                    isDeleted: false
                };
            }
        },
        sort: {
            field: "BrandTitle",
            dir: "ASC"
        },
        schema: {
            model: {
                id: 'BrandId'
            }
        }
    });

    $('#ddlBrands').kendoComboBox({
        dataSource: my.brandsDataSource,
        autoBind: false,
        dataTextField: 'BrandTitle',
        dataValueField: 'BrandId',
        placeholder: ' - Selecione - ',
        select: function (e) {
            e.preventDefault();
            var dataItem = this.dataItem(e.item.index());
            if (dataItem) {
                my.vm.selectedBrandId(dataItem.BrandId);
                this.value(my.vm.selectedBrandId());
                $('#btnUpdateBrand').show();
                $('#btnRemoveBrand').show();
                $('#btnUpdateModel').hide();
                $('#btnRemoveModel').hide();
                $('#btnAddModel').attr({ 'disabled': true });
                $('#ddlModels').data("kendoComboBox").enable(true);
            }
        }
    });

    $('#ddlBrands').data('kendoComboBox').input.bind('keyup', function () {
        if (this.value.length === 0) {
            my.vm.selectedBrandId(0);
            $('#btnAddModel').attr({ 'disabled': true });
            $('#btnAddBrand').attr({ 'disabled': true });
            $('#ddlModels').data("kendoComboBox").enable(false);
        } else {
            $('#btnAddBrand').attr({ 'disabled': false });
        }
    });

    //$('#ddlBrands').data('kendoComboBox').bind('select', function (e) {
    //    e.preventDefault();
    //    var dataItem = this.dataItem(e.item.index());
    //    if (dataItem) {
    //        this.value(dataItem.BrandTitle);
    //        my.vm.selectedBrandId(dataItem.BrandId);
    //        $('#btnUpdateBrand').show();
    //        $('#btnRemoveBrand').show();
    //    }
    //});

    $('#btnAddBrand').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();
        
        if ($('#ddlBrands').data("kendoComboBox").text() !== "") {

            $(this).html('<i class="fa fa-spinner fa-spin"></i>').attr({ 'disabled': true });

            var params = {
                PortalId: portalID,
                BrandTitle: $('#ddlBrands').data('kendoComboBox').text(),
                CreatedByUser: userID,
                CreatedOnDate: moment().format()
            };

            $.ajax({
                type: 'POST',
                url: '/desktopmodules/riw/api/brands/UpdateBrand',
                dataType: 'json',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    if (data.BrandId) {
                        my.vm.selectedBrandId(data.BrandId);
                        my.brandsDataSource.read();
                        $('#ddlModels').data("kendoComboBox").enable();
                        $('#btnUpdateBrand').show();
                        $('#btnRemoveBrand').show();
                        //$().toastmessage('showSuccessToast', 'Marca inserida com sucesso.');
                        $.pnotify({
                            title: 'Sucesso!',
                            text: 'Marca inserida com sucesso.',
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

                    $('#btnAddBrand').html('<i class="icon-plus"></i>').attr({ 'disabled': false });
                } else {
                    $('#btnAddBrand').html('<i class="icon-plus"></i>').attr({ 'disabled': false });
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

    $('#btnUpdateBrand').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $(this).html('<i class="fa fa-spinner fa-spin"></i>').attr({ 'disabled': true });

        if (my.vm.selectedBrandId()) {
            var params = {
                PortalId: portalID,
                BrandTitle: $('#ddlBrands').data('kendoComboBox').text(),
                BrandId: my.vm.selectedBrandId(),
                CreatedByUser: userID,
                CreatedOnDate: moment().format()
            };

            $.ajax({
                type: 'POST',
                url: '/desktopmodules/riw/api/brands/UpdateBrand',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    my.brandsDataSource.read();
                    //my.vm.selectedBrandId(bId);
                    //$().toastmessage('showSuccessToast', 'Marca atualizada com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Marca atualizada com sucesso.',
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

                $('#btnUpdateBrand').html('<i class="icon-ok"></i>').attr({ 'disabled': false });
            }).fail(function (jqXHR, textStatus) {
                $('#btnUpdateBrand').html('<i class="icon-ok"></i>').attr({ 'disabled': false });
                console.log(jqXHR.responseText);
            });
        }
    });

    $('#btnRemoveBrand').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();
        
        $(this).html('<i class="fa fa-spinner fa-spin"></i>').attr({ 'disabled': true });

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
                                        $.ajax({
                                            type: 'DELETE',
                                            url: '/desktopmodules/riw/api/brands/RemoveBrand?brandId=' + my.vm.selectedBrandId()
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                my.vm.selectedBrandId(0);
                                                my.brandsDataSource.read();
                                                $('#btnUpdateBrand').hide();
                                                $('#btnRemoveBrand').hide();
                                                $('#ddlBrands').data("kendoComboBox").value(null);
                                                $('#ddlBrands').data("kendoComboBox").text("");
                                                $('#btnAddModel').attr({ 'disabled': true });
                                                $('#btnAddBrand').attr({ 'disabled': true });
                                                $('#ddlModels').data("kendoComboBox").enable(false);
                                                //$().toastmessage('showSuccessToast', 'Marca excluida com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Marca excluida com sucesso.',
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

                                            $dialog.dialog('close');
                                            $dialog.dialog('destroy');
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
    });

    my.brandModelsDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/desktopmodules/riw/api/brandmodels/GetBrandModels',
            },
            parameterMap: function (data, type) {
                return {
                    portalId: portalID,
                    brandId: my.vm.selectedBrandId()
                };
            }
        },
        sort: {
            field: "ModelTitle",
            dir: "ASC"
        },
        schema: {
            model: {
                id: 'ModelId'
            }
        }
    });

    $('#ddlModels').kendoComboBox({
        dataSource: my.brandModelsDataSource,
        dataTextField: 'ModelTitle',
        dataValueField: 'ModelId',
        placeholder: ' - Selecione - ',
        cascadeFrom: 'ddlBrands',
        select: function (e) {
            e.preventDefault();
            var dataItem = this.dataItem(e.item.index());
            if (dataItem) {
                my.vm.selectedModelId(dataItem.ModelId);
                this.value(my.vm.selectedModelId());
                $('#btnUpdateModel').show();
                $('#btnRemoveModel').show();
            }
        }
    });

    $('#ddlModels').data('kendoComboBox').input.bind('keyup', function () {
        if (this.value.length === 0) {
            my.vm.selectedModelId(0)
            $('#btnAddModel').attr({ 'disabled': true });
            $('#btnUpdateModel').hide();
            $('#btnRemoveModel').hide();
        } else {
            $('#btnAddModel').attr({ 'disabled': false });
        }
    });

    $('#btnAddModel').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        if ($('#ddlModels').data("kendoComboBox").text() !== "") {
            var params = {
                PortalId: portalID,
                BrandId: my.vm.selectedBrandId(),
                ModelTitle: $('#ddlModels').data('kendoComboBox').text(),
                CreatedByUser: userID,
                CreatedOnDate: moment().format()
            };

            $.ajax({
                type: 'POST',
                url: '/desktopmodules/riw/api/brandmodels/UpdateBrandModel',
                dataType: 'json',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    //$.getJSON('/desktopmodules/riw/api/brandmodels/GetBrandModels?brandId=' + my.vm.selectedBrandId() + '&portalId=' + _portalID, function (dataModel) {
                    //    my.vm.models.removeAll();
                    //    $.each(dataModel, function (i, mo) {
                    //        my.vm.models.push(ko.mapping.fromJS(mo));
                    //    });
                    //});
                    my.brandModelsDataSource.read();
                    my.vm.selectedModelId(data.ModelId);
                    $('#btnUpdateModel').show();
                    $('#btnRemoveModel').show();
                    //$().toastmessage('showSuccessToast', 'Lista de modelos atualizada com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Lista de modelos atualizada com sucesso.',
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

    $('#btnUpdateModel').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        if (my.vm.selectedBrandId()) {
            var params = {
                PortalId: portalID,
                BrandId: my.vm.selectedBrandId(),
                ModelId: my.vm.selectedModelId(),
                ModelTitle: $('#ddlModels').data('kendoComboBox').text(),
                CreatedByUser: userID,
                CreatedOnDate: moment().format()
            };

            $.ajax({
                type: 'POST',
                url: '/desktopmodules/riw/api/brandmodels/UpdateBrandModel',
                dataType: 'json',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    //$.getJSON('/desktopmodules/riw/api/brandmodels/GetBrandModels?brandId=' + my.vm.selectedBrandId() + '&portalId=' + _portalID, function (dataModel) {
                    //    my.vm.models.removeAll();
                    //    $.each(dataModel, function (i, mo) {
                    //        my.vm.models.push(ko.mapping.fromJS(mo));
                    //    });
                    //});
                    my.brandModelsDataSource.read();
                    $('#btnUpdateModel').show();
                    $('#btnRemoveModel').show();
                    //$().toastmessage('showSuccessToast', 'Modelo atualizado com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Modelo atualizado com sucesso.',
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

    $('#btnRemoveModel').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();
                
        $(this).html('<i class="fa fa-spinner fa-spin"></i>').attr({ 'disabled': true });

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
                                        $.ajax({
                                            type: 'DELETE',
                                            url: '/desktopmodules/riw/api/brandmodels/RemoveBrandModel?modelId=' + my.vm.selectedModelId() + '&brandId=' + my.vm.selectedBrandId()
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                //$.getJSON('/desktopmodules/riw/api/brandmodels/GetBrandModels?brandId=' + my.vm.selectedBrandId() + '&portalId=' + _portalID, function (dataModel) {
                                                //    my.vm.models.removeAll();
                                                //    $.each(dataModel, function (i, mo) {
                                                //        my.vm.models.push(ko.mapping.fromJS(mo));
                                                //    });
                                                //});
                                                my.vm.selectedModelId(0);
                                                my.brandModelsDataSource.read();
                                                $('#ddlModels').data("kendoComboBox").value(null);
                                                $('#ddlModels').data("kendoComboBox").text("");
                                                $('#btnUpdateModel').hide();
                                                $('#btnRemoveModel').hide();
                                                //$().toastmessage('showSuccessToast', 'Modelo excluido com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Modelo excluido com sucesso.',
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

                                            $dialog.dialog('close');
                                            $dialog.dialog('destroy');
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
    });

    my.setItemType = function () {
        if ($('#productType').is(':checked')) {
            $('#divUnitTypes').fadeIn();
            $('#divVendors').fadeIn();
            $('#divBarCode').fadeIn();
            $('#divBrand').fadeIn();
            $('#divModels').fadeIn();
            $('#divStockLabel').fadeIn();
            $('#divReorderPoint').fadeIn();
            $('#divStockTextBox').fadeIn();
        } else {
            $('#divUnitTypes').fadeOut();
            $('#divVendors').fadeOut();
            $('#divBarCode').fadeOut();
            $('#divBrand').fadeOut();
            $('#divModels').fadeOut();
            $('#divStockLabel').fadeOut();
            $('#divReorderPoint').fadeOut();
            $('#divStockTextBox').fadeOut();
        }
    };

    $('#productType').click(function (e) {
        my.setItemType();
    });

    $('#serviceType').click(function (e) {
        my.setItemType();
    });

    $('#btnUpdateProduct').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        //$(this).html('<i class="fa fa-spinner fa-spin"></i> Um momento...').attr({ 'disabled': true });

        var params = {
            PortalId: portalID,
            Lang: 'pt-BR',
            ProductId: my.productId,
            ItemType: $('#productType').is(':checked') ? 1 : 2,
            ProductRef: $('#prodRefTextBox').val().trim(),
            ProductName: $('#prodNameTextBox').val().trim(),
            Summary: $('#summaryTextArea').val().trim(),
            Brand: my.vm.selectedBrandId(),
            BrandModel: my.vm.selectedModelId(),
            Barcode: $('#prodBarCodeTextBox').val().trim(),
            ProductUnit: $('#ddlUnitTypes').data('kendoDropDownList').value(),
            SaleStartDate: moment('1900-01-01 00:00:00').format(),
            SaleEndDate: moment('1900-01-01 00:00:00').format(),
            QtyStockSet: $('#stockTextBox').data('kendoNumericTextBox').value(),
            ReorderPoint: $('#reorderPointTextBox').data('kendoNumericTextBox').value(),
            ShowPrice: $('#showPriceCheckBox').is(':checked'),
            Vendors: $('#selectVendors').data('kendoMultiSelect').value().toString(),
            Featured: $('#featuredCheckbox').is(':checked'),
            Archived: $('#archivedCheckbox').is(':checked'),
            DealerOnly: $('#dealerOnlyCheckbox').is(':checked'),
            IsHidden: $('#hiddenCheckbox').is(':checked'),
            ScaleProduct: $('#scaleCheckbox').is(':checked'),
            CreatedByUser: userID,
            CreatedOnDate: moment().format(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format(),
            SyncEnabled: amplify.store.sessionStorage('syncEnabled').toLowerCase()
        };

        var categories = '';
        $('#selectedCats li').each(function (i) {
            categories += kendo.toString($('#selectedCats li')[i].id + ',');

            params.Categories = categories;
        });

        //var kendoWindow = null;

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/products/UpdateProduct',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $('#productMenu').show();
                document.location.hash = '#itemId/' + my.productId;

                if (parent.$('#window').data('kendoWindow')) {
                    parent.$('#window').data('kendoWindow').title($('#prodNameTextBox').val() + ' (ID: ' + my.productId + ')');
                }
                if (params.ProductId === 0) {
                    document.location.hash = '#itemId/' + data.ProductId;

                    if (parent.$('#window').data('kendoWindow')) {
                        parent.$('#window').data('kendoWindow').title($('#prodNameTextBox').val() + ' (ID: ' + data.ProductId + ')');
                    }
                    my.productId = data.ProductId;

                    //parent.$("#productWindow").append("<div id='window2'></div>");
                    //kendoWindow = parent.$('#window2').kendoWindow({
                    //    title: "Baixar Arquivo",
                    //    resizable: false,
                    //    modal: false,
                    //    width: 550,
                    //    deactivate: function () {
                    //        this.destroy();
                    //    }
                    //}).data("kendoWindow")
                    //    .content('<h2>Arquivo disponível para exportação.</h2><h5><a href="/portals/0/products/E' + my.padLeft(data.ProdId, 8) + '.txt">E' + my.padLeft(data.ProdId, 8) + '.txt</a></h5><h6>Clique com o botão da direita do mouse e escolha a opção salvar link.</h6>')
                    //    .center().open();

                    //$().toastmessage('showSuccessToast', 'Item inserido com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Item inserido com sucesso.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });

                    // *ToDo*
                    // set buttons after product was added and send product id to url
                } else {
                    //parent.$("#productWindow").append("<div id='window2'></div>");
                    //kendoWindow = parent.$('#window2').kendoWindow({
                    //    title: "Baixar Arquivo",
                    //    resizable: false,
                    //    modal: false,
                    //    width: 550,
                    //    deactivate: function () {
                    //        this.destroy();
                    //    }
                    //}).data("kendoWindow")
                    //    .content('<h2>Arquivo disponível para exportação.</h2><h5><a href="/portals/0/products/E' + my.padLeft(my.pId, 8) + '.txt">E' + my.padLeft(my.pId, 8) + '.txt</a></h5><h6>Clique com o botão da direita do mouse e escolha a opção salvar link.</h6>')
                    //    .center().open();

                    //$().toastmessage('showSuccessToast', 'Item atualizado com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Item atualizado com sucesso.',
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
                //$().toastmessage('showErrorToast', data.Result);
            }

            $('#btnUpdateProduct').html('<i class="icon-ok icon-white"></i> Atualizar').attr({ 'disabled': false });
        }).fail(function (jqXHR, textStatus) {
            $('#btnUpdateProduct').html('<i class="icon-ok icon-white"></i> Atualizar').attr({ 'disabled': false });
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
    });

    $('#btnCopyProduct').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        //$(this).html('<i class="fa fa-spinner fa-spin"></i> Um momento...').attr({ 'disabled': true });

        var params = {
            PortalId: portalID,
            Lang: 'pt-BR',
            ProductId: 0,
            ItemType: $('#productType').is(':checked') ? 1 : 2,
            //ProductRef: $('#prodRefTextBox').val().trim() + '_Copia',
            ProductName: $('#prodNameTextBox').val().trim(),
            Summary: $('#summaryTextArea').val().trim(),
            Brand: my.vm.selectedBrandId(),
            BrandModel: my.vm.selectedModelId(),
            //Barcode: $('#prodBarCodeTextBox').val().trim() + '_Copia',
            ProductUnit: $('#ddlUnitTypes').data('kendoDropDownList').value(),
            SaleStartDate: moment('1900-01-01 00:00:00').format(),
            SaleEndDate: moment('1900-01-01 00:00:00').format(),
            QtyStockSet: $('#stockTextBox').data('kendoNumericTextBox').value(),
            ReorderPoint: $('#reorderPointTextBox').data('kendoNumericTextBox').value(),
            ShowPrice: $('#showPriceCheckBox').is(':checked'),
            Vendors: $('#selectVendors').data('kendoMultiSelect').value().toString(),
            Featured: $('#featuredCheckbox').is(':checked'),
            Archived: $('#archivedCheckbox').is(':checked'),
            DealerOnly: $('#dealerOnlyCheckbox').is(':checked'),
            IsHidden: $('#hiddenCheckbox').is(':checked'),
            ScaleProduct: $('#scaleCheckbox').is(':checked'),
            CreatedByUser: userID,
            CreatedOnDate: moment().format(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };

        var categories = '';
        $('#selectedCats li').each(function (i) {
            categories += kendo.toString($('#selectedCats li')[i].id + ',');

            params.Categories = categories;
        });

        var kendoWindow = null;

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/products/UpdateProduct',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $('#productMenu').show();
                if (parent.$('#window').data('kendoWindow')) {
                    parent.$('#window').data('kendoWindow').title($('#prodNameTextBox').val() + ' (ID: ' + data.ProductId + ')');
                }
                document.location.hash = '#itemId/' + data.ProductId;

                my.productId = data.ProductId;

                //$().toastmessage('showSuccessToast', 'Item inserido com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Item inserido com sucesso.',
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

    if (my.productId > 0) {
        //my.createListView();
        $('#btnCopyProduct').show();
        $('#btnUpdateProduct').html('<i class="icon-ok icon-white"></i> Atualizar');
        $.ajax({
            url: '/desktopmodules/riw/api/products/GetProduct?productId=' + my.productId + '&lang=pt-BR'
        }).done(function (data) {
            if (data) {
                $('#productMenu').show();
                $('#divStockLabel').show();

                if (data.ItemType === '1') {
                    $('#productType').attr({ 'checked': true });
                } else {
                    $('#serviceType').attr({ 'checked': true });
                }

                $('#prodNameTextBox').val(data.ProductName);
                $('#prodRefTextBox').val(data.ProductRef);
                $('#prodBarCodeTextBox').val(data.Barcode);
                $('#stockLabel').text(data.QtyStockSet);
                if (data.Stock <= 0) {
                    $('#stockLabel').addClass('NormalRed');
                }

                //$('#stockTextBox').val(data.QtyStockSet);
                $('#reorderPointTextBox').data('kendoNumericTextBox').value(data.ReorderPoint);
                $('#showPriceCheckBox').bootstrapSwitch('setState', data.ShowPrice);
                //$('#fShippingCheckBox').attr({ 'checked': data.FreeShipping });

                $('#summaryTextArea').val(data.Summary);

                $('#featuredCheckbox').attr({ 'checked': data.Featured });
                $('#archivedCheckbox').attr({ 'checked': data.Archived });
                $('#dealerOnlyCheckbox').attr({ 'checked': data.DealerOnly });
                $('#hiddenCheckbox').attr({ 'checked': data.IsHidden });
                $('#scaleCheckbox').attr({ 'checked': data.ScaleProduct });

                //$('#prodDescEditor').data('kendoEditor').value(data.Description);
                if (data.Brand > 0) {
                    my.vm.selectedBrandId(data.Brand);
                    $('#ddlBrands').data('kendoComboBox').value(data.Brand);
                }
                if (data.BrandModel > 0) {
                    my.vm.selectedModelId(data.BrandModel);
                    $('#ddlModels').data('kendoComboBox').value(data.BrandModel);
                }

                if (data.Vendors) {
                    if (data.Vendors.length > 0) {
                        $('#selectVendors').data('kendoMultiSelect').value(data.Vendors.split(','));
                    }
                }

                $.ajax({
                    url: '/desktopmodules/riw/api/categories/GetCategoriesProduct?productId=' + my.productId
                }).done(function (cats) {
                    if (cats) {
                        $('#selectedCats').text('');
                        $.each(cats, function (i, cat) {
                            $('#selectedCats').append('<li class="select2-search-choice text-t-c" id="' + cat.CategoryId + '"><div class="text-t-c">' + cat.CategoryName + '</div><a id="' + cat.CategoryId + '" href="#" onclick="my.removeCat(this); return false;" class="select2-search-choice-close" tabindex="-1"></a></li>');
                        });
                    }
                });

                if (data.Locked) {
                    if (data.IsDeleted) {
                        $('#btnRestoreProduct').show();
                        $('#btnDeleteProduct').hide();
                    } else {
                        if (authorized > 2) {
                            $('#btnRemoveProduct').hide();
                            $('#btnDeleteProduct').show();
                        }
                    }
                } else {
                    if (data.IsDeleted) {
                        $('#btnRestoreProduct').show();
                        $('#btnRemoveProduct').show();
                    } else {
                        if (authorized > 2) {
                            $('#btnRemoveProduct').show();
                            $('#btnDeleteProduct').show();
                        }
                    }
                }

                setTimeout(function () {
                    $('#ddlUnitTypes').data('kendoDropDownList').value(data.ProductUnit);
                }, 500);
            }
        });

        if (authorized < 3) {
            $('#liFinan').hide();
        }
    } else {
        $('#divStockTextBox').show();

        //var ddlUnitTypes = $('#ddlUnitTypes').data('kendoDropDownList');
        //ddlUnitTypes.select(function (dataItem) {
        //    return dataItem.UnitTypeTitle === 'Kilo';
        //});
    }

    $('#btnDeleteProduct').click(function (e) {
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

                                        $this.button('loading');
                                        $dialog.dialog('close');
                                        $dialog.dialog('destroy');

                                        //$(this).html('<i class="fa fa-spinner fa-spin"></i> Um momento...').attr({ 'disabled': true });

                                        var params = {
                                            ProductId: my.productId,
                                            Lang: 'pt-BR',
                                            ModifiedByUser: userID,
                                            ModifiedOnDate: moment().format()
                                        }

                                        $.ajax({
                                            type: 'PUT',
                                            url: '/desktopmodules/riw/api/products/deleteProduct',
                                            data: params
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                //$().toastmessage('showSuccessToast', 'Item desativado com sucesso!');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Item desativado com sucesso.',
                                                    type: 'success',
                                                    icon: 'fa fa-check fa-lg',
                                                    addclass: "stack-bottomright",
                                                    stack: my.stack_bottomright
                                                });
                                                $('#btnDeleteProduct').hide();
                                                $('#btnRestoreProduct').show();
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

                                            $dialog.dialog('close');
                                            $dialog.dialog('destroy');
                                            $('#btnDeleteProduct').html('<i class="icon-ban-circle"></i> Desativar').attr({ 'disabled': false });
                                        }).fail(function (jqXHR, textStatus) {
                                            $('#btnDeleteProduct').html('<i class="icon-ban-circle"></i> Desativar').attr({ 'disabled': false });
                                            console.log(jqXHR.responseText);
                                        }).always(function () {
                                            $this.button('reset');
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

    $('#btnRestoreProduct').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        //$(this).html('<i class="fa fa-spinner fa-spin"></i> Um momento...').attr({ 'disabled': true });

        var params = {
            ProductId: my.productId,
            Lang: 'pt-BR',
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        }

        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/products/restoreProduct',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $('#btnDeleteProduct').show();
                $('#btnRestoreProduct').hide();
                //$().toastmessage('showSuccessToast', 'Item restaurado com sucesso!');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Item restaurado com sucesso.',
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

            $('#btnRestoreProduct').html('<i class="icon-ban-circle"></i> Restaurar').attr({ 'disabled': false });
        }).fail(function (jqXHR, textStatus) {
            $('#btnRestoreProduct').html('<i class="icon-ban-circle"></i> Restaurar').attr({ 'disabled': false });
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
    });

    $('#btnRemoveProduct').click(function (e) {
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

                                        $this.button('loading');
                                        $dialog.dialog('close');
                                        $dialog.dialog('destroy');

                                        //$(this).html('<i class="fa fa-spinner fa-spin"></i> Um momento...').attr({ 'disabled': true });

                                        $.ajax({
                                            type: 'DELETE',
                                            url: '/desktopmodules/riw/api/products/removeProduct?productId=' + my.productId + '&lang=pt-BR'
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                //$().toastmessage('showSuccessToast', 'Item excluido com sucesso!');
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

                                            setTimeout(function () {
                                                if (parent.$('#window')) {
                                                    parent.$('#window').data("kendoWindow").close();
                                                }
                                            }, 2000);
                                        }).fail(function (jqXHR, textStatus) {
                                            console.log(jqXHR.responseText);
                                        }).always(function () {
                                            $this.button('reset');
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

    $('#btnAddProvider').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $(this).html('<i class="fa fa-spinner fa-spin"></i>').attr({ 'disabled': true });
        document.location.href = editClientURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#itemId/' + my.productId;
    });

    $('#productMenu').kendoMenu({
        select: function (e) {
            switch ($(e.item).attr('id')) {
                case 'menu_3':
                    document.location.href = productImagesURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_4':
                    document.location.href = productDescURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_5':
                    document.location.href = productVideosURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_6':
                    document.location.href = relatedProductsURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_7':
                    document.location.href = productAttributesURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_8':
                    document.location.href = productShippingURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_9':
                    document.location.href = productSEOURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_11':
                    document.location.href = productFinanceURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
            }
        }
    });
    
    $('input.enterastab, select.enterastab, textarea.enterastab').on('keydown', function (e) {
        if (e.keyCode === 13) {
            var focusable = $('input,a,select,textarea').filter(':visible');
            focusable.eq(focusable.index(this) + 2).focus().select();
            return false;
        }
    });

    $('#btnClose').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        parent.$('#window').data('kendoWindow').close();
    });

    my.countChar = function (val, max) {
        var len = val.value.length;

        if (len >= max) {
            val.value = val.value.substring(0, max);
            $('#counter span').text(0);
        } else {
            $('#counter span').text(max - len);
        }
    };

    my.countChar($('#summaryTextArea').get(0), 500);

    $('#summaryTextArea').keyup(function () {
        my.countChar(this, 500);
    });

    //YUI().use('aui-char-counter', function (Y) {
    //    new Y.CharCounter({
    //        counter: '#counter',
    //        input: '#summaryTextArea',
    //        maxLength: 500
    //    });
    //});

    //$(".markdown-input").markdown();
    //$('.markdown-editor').css({ 'width': '90%', 'height': '220px !important' }).addClass('historyTextArea');
    $('#summaryTextArea').css({ 'min-width': '90%', 'height': '80px' }).attr({ 'cols': '30', 'rows': '2' });

    $('.markdown-editor').autogrow();
    $('.markdown-editor').css('overflow', 'hidden').autogrow();

    $('#toggleSummaryPreview').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        if (this.value === 'preview') {
            var converter = new Showdown.converter();
            $('#markdownPreview').html(converter.makeHtml($('#summaryTextArea').val().trim()));
            var $dialog = $('#preview')
                .dialog({
                    autoOpen: false,
                    open: function() {
                        $(".ui-dialog-title").append('Introdu&#231;&#227;o');
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
    });

    $('.btnReturn').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        //$(this).html('<i class="fa fa-spinner fa-spin"></i>Um momento...').attr({ 'disabled': true });
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

    setTimeout(function () {
        $('#ddlUnitTypes').data('kendoDropDownList').value(amplify.store.sessionStorage('defaultUnit'));
    }, 1000);

});