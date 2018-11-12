
$(function () {

    my.cats = null;

    ko.bindingHandlers.bootstrapSwitchOn = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var $elem = $(element);
            $(element).bootstrapSwitch('setState', ko.utils.unwrapObservable(valueAccessor())); // Set intial state
            $elem.on('switch-change', function (e, data) {
                valueAccessor()(data.value);
            }); // Update the model when changed.
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var vStatus = $(element).bootstrapSwitch('status');
            var vmStatus = ko.utils.unwrapObservable(valueAccessor());
            if (vStatus!==vmStatus) {
                $(element).bootstrapSwitch('setState', vmStatus);
            }
        }
    };

    my.permission = function () {
        this.CatPermissionId = ko.observable();
        this.CategoryId = ko.observable();
        this.PermissionId = ko.observable();
        this.RoleId = ko.observable();
        this.AllowAccess = ko.observable();
    };

    // VIEW MODEL
    my.vm = function () {
        var self = this;

        self.productId = ko.observable(0),
        self.productName = ko.observable(''),
        self.productRef = ko.observable(''),
        self.hidden = ko.observable(false),
        self.message = ko.observable(''),
        self.lang = ko.observable('pt-BR'),
        self.archived = ko.observable(false),
        self.listOrder = ko.observable(1),
        self.categoryId = ko.observable(0),
        self.categoryName = ko.observable(''),
        self.categoryDesc = ko.observable(''),
        self.parentId = ko.observable(0),
        self.seoName = ko.observable(''),
        self.seoPageTitle = ko.observable(''),
        self.metaDesc = ko.observable(''),
        self.metaKeywords = ko.observable(''),
        self.productCount = ko.observable(0),

        //self.selectedTitle = ko.computed(function () {
        //    return self.selectedCategoryId() ? 'Editando "' + self.selectedCategoryName() + '"' : 'Nova Categoria';
        //}),

        self.catPermissions = ko.observableArray([]),
        self.setButton = ko.computed(function () {
            var btnLabel = 'Inserir';
            if (self.categoryId()) {
                btnLabel = 'Atualizar';
                $('#btnCancel').show();
                $('#btnRemove').show();
            }

            return btnLabel;
        });

        return {
            productId: productId,
            productName: productName,
            productRef: productRef,
            hidden: hidden,
            message: message,
            lang: lang,
            archived: archived,
            listOrder: listOrder,
            categoryId: categoryId,
            categoryName: categoryName,
            categoryDesc: categoryDesc,
            parentId: parentId,
            seoName: seoName,
            seoPageTitle: seoPageTitle,
            metaDesc: metaDesc,
            metaKeywords: metaKeywords,
            productCount: productCount,
            catPermissions: catPermissions
        };
    }();

    ko.applyBindings(my.vm);

    my.loadCatPermissions = function () {
        $.getJSON('/desktopmodules/riw/api/categories/GetCategoryPermissions?catId=' + my.vm.categoryId(), function (data) {
            if (data) {
                my.vm.catPermissions.removeAll();
                $.each(data, function (i, per) {
                    my.vm.catPermissions.push(new my.permission()
                        .CatPermissionId(per.CatPermissionId)
                        .CategoryId(per.CategoryId)
                        .PermissionId(per.PermissionId)
                        .RoleId(per.RoleId)
                        .AllowAccess(per.AllowAccess));
                });
            }
        });
    };

    $('.dnnFormHelp').tooltip({ placement: 'right' });

    var menu = $('#editCategoryMenu').kendoMenu({
        select: function (e) {
            $("#editCategoryMenu").find(".k-state-selected").removeClass("k-state-selected");
            switch (e.item.id)
            {
                case 'menu_2':
                    $('#basicEdit').fadeOut();
                    $('#extraEdit').fadeOut();
                    $('#productsEdit').fadeOut();
                    $('#seoEdit').delay(400).fadeIn();
                    $('#permissionEdit').fadeOut();
                    $(e.item).addClass("k-state-selected");
            	    break;
                case 'menu_3':
                    $('#seoEdit').fadeOut();
                    $('#basicEdit').fadeOut();
                    $('#productsEdit').fadeOut();
                    $('#extraEdit').delay(400).fadeIn();
                    $('#permissionEdit').fadeOut();
                    $(e.item).addClass("k-state-selected");
                    break;
                case 'menu_4':
                    $('#seoEdit').fadeOut();
                    $('#basicEdit').fadeOut();
                    $('#extraEdit').fadeOut();
                    $('#permissionEdit').fadeOut();
                    $('#productsEdit').delay(400).fadeIn();
                    $(e.item).addClass("k-state-selected");
                    break;
                case 'menu_5':
                    $('#seoEdit').fadeOut();
                    $('#basicEdit').fadeOut();
                    $('#extraEdit').fadeOut();
                    $('#productsEdit').fadeOut();
                    $('#permissionEdit').delay(400).fadeIn();
                    break;
                default:
                    $('#seoEdit').fadeOut();
                    $('#extraEdit').fadeOut();
                    $('#productsEdit').fadeOut();
                    $('#permissionEdit').fadeOut();
                    $('#basicEdit').delay(400).fadeIn();
            }

        }
    }).data("kendoMenu");

    if (my.vm.categoryId() === 0) {
        var items = $('#editCategoryMenu').find(':not(.k-first)');
        menu.enable(items, items.hasClass('k-state-disabled'));
    }

    $('#orderTextBox').kendoNumericTextBox({
        format: '',
        value: 1
    });
    
    my.loadCategories = function () {
        if (!my.cats) {
            var result = null;
            $.ajax({
                url: '/desktopmodules/riw/api/categories/Categories?portalId=' + _portalID + '&lang=pt-BR&includeArchived=true',
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
        var items = [];
        for (i = 0; i < my.cats.length; i++) {
            var item = my.cats[i];
            var label = item.CategoryName;
            var code = item.CategoryId;
            var desc = item.CategoryDesc;
            var parentid = item.ParentCategoryId;
            var id = item.CategoryId;

            if (items[parentid]) {
                item = {
                    parentid: parentid,
                    label: label,
                    code: code,
                    desc: desc,
                    item: item
                };
                if (!items[parentid].items) {
                    items[parentid].items = [];
                }
                items[parentid].items[items[parentid].items.length] = item;
                items[id] = item;
            } else {
                items[id] = {
                    parentid: parentid,
                    label: label,
                    code: code,
                    desc: desc,
                    item: item
                };
                source[id] = items[id];
            }
        }
        return source;
    };
    my.sourceMenu = my.buildMenudata();

    my.buildMenuUL = function (parent, items) {
        var li = $('<li id="0999999999" style="display: none">hidden</li>');
        li.appendTo(parent);
        $.each(items, function () {
            if (this.label) {
                // create LI element and append it to the parent element.
                li = $("<li id='" + this.item.CategoryId + "'>" + this.label + "</li>");
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

    my.ulMenu.appendTo("#treeViewCategories");
    my.buildMenuUL(my.ulMenu, my.sourceMenu);

    //$('#treeViewCategories ul').kendoTreeView({
    //    select: function (e) {
    //        if (e.node.id !== '0999999999') {
    //            my.vm.categoryId(e.node.id);
    //            my.loadCatPermissions();
    //            $.getJSON('/desktopmodules/riw/api/categories/Category?catId=' + e.node.id + '&lang=pt-BR', function (data) {
    //                my.vm.hidden(data.Hidden);
    //                my.vm.message(data.Message);
    //                my.vm.lang(data.Lang);
    //                my.vm.archived(data.Archived);
    //                my.vm.listOrder(data.ListOrder);
    //                my.vm.categoryName(data.CategoryName);
    //                my.vm.categoryDesc(data.CategoryDesc);
    //                my.vm.seoName(data.SEOName);
    //                my.vm.seoPageTitle(data.SEOPageTitle);
    //                my.vm.metaDesc(data.MetaDescription);
    //                my.vm.metaKeywords(data.MetaKeywords);
    //                my.vm.productCount(data.ProductCount);
    //                if (data.ParentCategoryId > 0) {
    //                    $('.jqx-dropdownlist-content').removeClass('jqx-dropdownlist-state-normal-placeholder');
    //                    my.vm.parentId(data.ParentCategoryId);
    //                    $('#mainCategories').jqxTree('selectItem', $('#' + my.vm.parentId() + '')[0]);
    //                }
    //            });
    //            my.categoryProductsData.read();

    //            var menuItems = $('#editCategoryMenu').find('.k-state-disabled');
    //            menu.enable(menuItems, menuItems.hasClass('k-state-disabled'));

    //            $('#groupsGrid').data('kendoGrid').dataSource.read();
    //            //setTimeout(function () {
    //            $('#nameTextBox').focus();
    //            //}, 1000);
    //        }
    //    }
    //});

    $('#treeViewCategories').jqxTree();
    $('#treeViewCategories').on('select', function (event) {
        var args = event.args;
        var item = $('#treeViewCategories').jqxTree('getItem', args.element);
        if (item.id !== '0999999999') {
            my.vm.categoryId(item.id);
            my.loadCatPermissions();
            $.getJSON('/desktopmodules/riw/api/categories/Category?catId=' + item.id + '&lang=pt-BR', function (data) {
                my.vm.hidden(data.Hidden);
                my.vm.message(data.Message);
                my.vm.lang(data.Lang);
                my.vm.archived(data.Archived);
                my.vm.listOrder(data.ListOrder);
                my.vm.categoryName(data.CategoryName);
                my.vm.categoryDesc(data.CategoryDesc);
                my.vm.seoName(data.SEOName);
                my.vm.seoPageTitle(data.SEOPageTitle);
                my.vm.metaDesc(data.MetaDescription);
                my.vm.metaKeywords(data.MetaKeywords);
                my.vm.productCount(data.ProductCount);
                if (data.ParentCategoryId > 0) {
                    $('.jqx-dropdownlist-content').removeClass('jqx-dropdownlist-state-normal-placeholder');
                    my.vm.parentId(data.ParentCategoryId);
                    $('#mainCategories').jqxTree('selectItem', $('#' + my.vm.parentId() + '')[0]);
                } else {
                    my.vm.parentId(0);
                    my.dropDownContent = '<div style="position: relative; margin-left: 5px; margin-top: 6px; color: #999; font-style: italic;"> Selecionar</div>';
                    $('#availCategoriesButton').jqxDropDownButton('setContent', my.dropDownContent);
                }
            });
            my.categoryProductsData.read();

            var menuItems = $('#editCategoryMenu').find('.k-state-disabled');
            menu.enable(menuItems, menuItems.hasClass('k-state-disabled'));

            $('#groupsGrid').data('kendoGrid').dataSource.read();
            //setTimeout(function () {
                $('#nameTextBox').focus();
            //}, 1000);
        }
    });

    my.buildDDMenuUL = function (parent, items) {
        var li = $('<li>Nenhuma</li>');
        li.appendTo(parent);
        $.each(items, function () {
            if (this.label) {
                // create LI element and append it to the parent element.
                li = $("<li id='" + this.item.CategoryId + "'>" + this.label + "</li>");
                li.appendTo(parent);
                // if there are sub items, call the buildUL function.
                if (this.items && this.items.length > 0) {
                    var ul = $("<ul></ul>");
                    ul.appendTo(li);
                    my.buildDDMenuUL(ul, this.items);
                }
            }
        });
    };
    my.ulDDMenu = $("<ul></ul>");
    my.ulDDMenu.appendTo("#mainCategories");
    my.buildDDMenuUL(my.ulDDMenu, my.sourceMenu);

    $('#availCategoriesButton').jqxDropDownButton({
        height: 32
    });

    my.dropDownContent = '<div style="position: relative; margin-left: 5px; margin-top: 6px; color: #999; font-style: italic;"> Selecionar</div>';
    $('#availCategoriesButton').jqxDropDownButton('setContent', my.dropDownContent);

    $('#mainCategories').on('select', function (event) {
        var args = event.args;
        var item = $('#mainCategories').jqxTree('getItem', args.element);
        my.dropDownContent = '<div style="position: relative; margin-left: 5px; margin-top: 6px;">' + item.label + '</div>';
        $('#availCategoriesButton').jqxDropDownButton('setContent', my.dropDownContent);
        my.vm.parentId(item.id);
        $('#availCategoriesButton').jqxDropDownButton('close');
    });
    $('#mainCategories').jqxTree({
        width: $('#availCategoriesButton').width()
        //height: 300
    });
    $('.jqx-dropdownlist-content').addClass('jqx-dropdownlist-state-normal-placeholder');

    $('#files').kendoUpload({
        async: {
            saveUrl: "/desktopmodules/riw/api/ristore/SaveFile",
            removeUrl: "/desktopmodules/riw/api/ristore/RemovePortalFile",
            autoUpload: false,
            multiple: false
        },
        //showFileList: false,
        localization: {
            cancel: 'Cancelar',
            dropFilesHere: 'Arraste arquivos aqui para envia-los',
            remove: 'Remover',
            select: 'Anexar Imagem',
            statusUploading: 'Enviando Arquivo(s)',
            uploadSelectedFiles: 'Enviar Arquivo(s)'
        },
        select: function (e) {
            $.each(e.files, function (index, value) {
                if (value.extension.toUpperCase() !== '.JPG' && value.extension.toUpperCase() !== '.PNG' && value.extension.toUpperCase() !== '.DOC' && value.extension.toUpperCase() !== '.PDF' && value.extension.toUpperCase() !== '.DOCX' && value.extension.toUpperCase() !== '.GIF' && value.extension.toUpperCase() !== '.ZIP' && value.extension.toUpperCase() !== '.RAR') {
                    e.preventDefault();
                    $().toastmessage('showWarningToast', 'É permitido enviar somente arquivos com formato jpg e png.');
                }
            });
        },
        upload: function (e) {
            e.data = {
                portalId: _portalID,
                maxWidth: 0,
                maxHeight: 0,
                folderPath: $('#folderPathTextBox').val()
            };
        },
        success: function (e) {
            var _fileInfo = e.response.fileInfo;
            //$.each(e.files, function (index, value) {
            my.vm.portalFiles.unshift(new my.PortalFile()
                .fileId(_fileInfo.FileId)
                .fileName(_fileInfo.FileName)
                .contenType(_fileInfo.ContentType)
                .extension(_fileInfo.Extension)
                .fileSize('Tamanho: ' + my.size_format(_fileInfo.Size))
                .height(_fileInfo.Height)
                .relativePath(_fileInfo.Extension === 'jpg' || _fileInfo.Extension === 'png' || _fileInfo.Extension === 'gif' ? '/portals/' + _portalID + '/' + _fileInfo.RelativePath : '/desktopmodules/riw/ristoredataservices/content/images/spacer.gif')
                .width(_fileInfo.Width));
            //});
            //});
            $(".k-widget.k-upload").find("ul").remove();
        },
        remove: function (e) {

        },
        error: function (e) {
            $().toastmessage('showErrorToast', 'Não foi possível o envio do arquivo.');
        }
    });
    
    $('#editorTextArea').kendoEditor({
        messages: {
            bold: "Bold",
            italic: "Italic",
            underline: "Underline",
            strikethrough: "Strikethrough",
            superscript: "Superscript",
            subscript: "Subscript",
            justifyCenter: "Center text",
            justifyLeft: "Align text left",
            justifyRight: "Align text right",
            justifyFull: "Justify",
            insertUnorderedList: "Insert unordered list",
            insertOrderedList: "Insert ordered list",
            indent: "Indent",
            outdent: "Outdent",
            createLink: "Insert hyperlink",
            unlink: "Remove hyperlink",
            insertImage: "Insert image",
            insertHtml: "Insert HTML",
            fontName: "Select font family",
            fontNameInherit: "(inherited font)",
            fontSize: "Select font size",
            fontSizeInherit: "(inherited size)",
            formatBlock: "Format",
            style: "Styles",
            emptyFolder: "Empty Folder",
            uploadFile: "Upload",
            orderBy: "Arrange by:",
            orderBySize: "Size",
            orderByName: "Name",
            invalidFileType: "The selected file \"{0}\" is not valid. Supported file types are {1}.",
            deleteFile: "Are you sure you want to delete \"{0}\"?",
            overwriteFile: "A file with name \"{0}\" already exists in the current directory. Do you want to overwrite it?",
            directoryNotFound: "A directory with this name was not found.",
            imageWebAddress: "Web address",
            imageAltText: "Alternate text",
            linkWebAddress: "Web address",
            linkText: "Text",
            linkToolTip: "ToolTip",
            linkOpenInNewWindow: "Open link in new window",
            dialogInsert: "Insert",
            dialogButtonSeparator: "or",
            dialogCancel: "Cancel"
        },
        tools: [
            "bold",
            "italic",
            "underline",
            "separator",
            "strikethrough",
            "justifyLeft",
            "justifyCenter",
            "justifyRight",
            "justifyFull",
            "insertUnorderedList",
            "insertOrderedList",
            "indent",
            "outdent",
            "createLink",
            "unlink",
            "viewHtml"
        ]
    });

    $('#btnUpload').click(function (e) {
        e.preventDefault();

        $('#files').click();
    });

    $("#productSearch").kendoAutoComplete({
        delay: 500,
        minLength: 5,
        dataValueField: 'ProductId',
        dataTextField: 'ProductName',
        placeholder: "Insira Nome, Referência ou Código.",
        template: '<strong>Produto: </strong><span>${ data.ProductName }</span><br /><strong>Ref: </strong><span>${ data.ProductRef }</span>',
        dataSource: {
            transport: {
                read: '/desktopmodules/riw/api/products/getProducts'
            },
            serverFiltering: true,
            pageSize: 50,
            sort: {
                field: "",
                dir: "ASC"
            },
            schema: {
                model: {
                    id: 'ProductId'
                },
                data: 'data',
                total: 'total'
            }
        },
        highlightFirst: true,
        filter: "contains",
        dataBound: function () {
            var autocomplete = this;
            switch (true) {
                case (this.dataSource.total() > 20):
                    if (!$('.toast-item-wrapper').length) $().toastmessage('showNoticeToast', 'Dezenas de itens encontrados... refina sua busca.');
                    break;
                case (this.dataSource.total() === 0):
                    autocomplete.value('');
                    if (!$('.toast-item-wrapper').length) $().toastmessage('showWarningToast', 'Sua busca não trouxe resultado algum.');
                    break;
                default:
            }
        },
        select: function (e) {
            e.preventDefault();
            var dataItem = this.dataItem(e.item.index());
            if (dataItem) {
                my.vm.productId(dataItem.ProductId);
                this.value(dataItem.ProductName);
                my.vm.productName(dataItem.ProductName);
                my.vm.productRef(dataItem.ProductRef);
            }
        }
    });

    my.categoryProductsTransport = {
        read: {
            url: '/desktopmodules/riw/api/categories/GetProductsCategory'
        },
        parameterMap: function (data, type) {
            return {
                portalId: _portalID,
                categoryId: my.vm.categoryId(),
                pageNumber: data.page,
                pageSize: data.pageSize,
                orderBy: my.convertSortingParameters(data.sort)
            };
        }
    };

    my.categoryProductsData = new kendo.data.DataSource({
        transport: my.categoryProductsTransport,
        pageSize: 10,
        serverPaging: true,
        serverSorting: true,
        serverFiltering: true,
        sort: {
            field: "",
            dir: "ASC"
        },
        schema: {
            model: {
                id: 'ProdId'
            },
            data: 'data',
            total: 'total'
        }
    });

    $('#categoryProductsGrid').kendoGrid({
        dataSource: my.categoryProductsData,
        columns: [
            {
                field: 'ProductId',
                title: 'ID',
                width: 60
            },
            {
                field: 'ProductRef',
                title: 'Referência',
                width: 100
            },
            {
                field: 'ProductName',
                title: 'Nome'
            },
            {
                command: {
                    name: 'remove',
                    text: '',
                    imageClass: 'k-icon k-i-close',
                    click: function (e) {
                        e.preventDefault();
                        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                            
                        $.ajax({
                            type: 'DELETE',
                            url: '/desktopmodules/riw/api/products/removeProductCategory?productId=' + dataItem.ProductId
                        }).done(function (data) {
                            if (data.Result.indexOf("success") !== -1) {
                                my.categoryProductsData.remove(dataItem);
                                $().toastmessage('showSuccessToast', 'O item ' + dataItem.ProductName + ' foi removido da categoria!');
                            } else {
                                $().toastmessage('showErrorToast', data.Result);
                            }
                        }).fail(function (jqXHR, textStatus) {
                            console.log(jqXHR.responseText);
                        });
                    }
                },
                width: 50
            }
        ]
    });

    $("#groupsGrid").kendoGrid({
        dataSource: new kendo.data.DataSource({
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/ristore/GetPublicRoles?portalId=' + _portalID
                }
            },
            sort: {
                field: "RoleName",
                dir: "ASC"
            },
            schema: {
                model: {
                    id: 'RoleId'
                }
            }
        }),
        sortable: true,
        dataBinding: function () {
            my.record = 0;
        },
        columns: [{
            field: 'RoleName',
            title: 'Grupo',
            width: '79%'
        }, {
            field: 'View',
            title: 'Vêr',
            width: '10%',
            template: '<span id="spanView_#= RoleId #" style="cursor: default;"><input type="hidden" id="view_#= RoleId #" name="view_#= RoleId #" value="0" /></span>'
        }, {
            field: 'Edit',
            title: 'Editar',
            width: '10%',
            template: '<span id="spanEdit_#= RoleId #" style="cursor: default;"><input type="hidden" id="edit_#= RoleId #" name="edit_#= RoleId #" value="0" /></span>'
        }],
        dataBound: function () {
            if (this.dataSource.view().length > 0) {
                var grid = this;
                //my.pers = ko.utils.arrayFirst(my.vm.catPermissions(), function (item) {
                //    return item;
                //});
                $.each(grid.tbody.find('tr'), function () {
                    var model = grid.dataItem(this);
                    switch (model.RoleId) {
                        case 0:
                            $('[data-uid=' + model.uid + ']').css({ 'display': 'none' });
                            break;
                        case 1:
                            $('[data-uid=' + model.uid + ']').css({ 'display': 'none' });
                            break;
                        case 2:
                            $('[data-uid=' + model.uid + ']').css({ 'display': 'none' });
                            break;
                        case 4:
                            $('[data-uid=' + model.uid + ']').css({ 'display': 'none' });
                            break;
                    }
                    if (my.vm.catPermissions().length > 0) {
                        ko.utils.arrayFirst(my.vm.catPermissions(), function (item) {
                            if (item.RoleId() === model.RoleId) {
                                if (item.AllowAccess()) {
                                    $('#view_' + model.RoleId).val(item.PermissionId());
                                } else {
                                    $('#edit_' + model.RoleId).val(item.PermissionId());
                                }
                                if (model.RoleId === 9999) $('#edit_' + model.RoleId).val(0);
                            }
                        });
                    } else {
                        $('#view_9999').val(1);
                    }
                    if (model.RoleName === 'Gerentes') $('#edit_' + model.RoleId).val(1);
                    initTriStateCheckBox('spanView_' + model.RoleId, 'view_' + model.RoleId, true);
                    initTriStateCheckBox('spanEdit_' + model.RoleId, 'edit_' + model.RoleId, true);
                });
                //for (var i = 0; i < grid.dataSource.view().length; i++) {
                //    if (my.vm.catPermissions().length > 0) {
                //        ko.utils.arrayFirst(my.vm.catPermissions(), function (item) {
                //            if (item.RoleId() === grid.dataSource.view()[i].RoleId) {
                //                if (item.AllowAccess()) {
                //                    $('#view_' + grid.dataSource.view()[i].RoleId).val(item.PermissionId());
                //                } else {
                //                    $('#edit_' + grid.dataSource.view()[i].RoleId).val(item.PermissionId());
                //                }
                //                if (grid.dataSource.view()[i].RoleId === 9999) $('#edit_' + grid.dataSource.view()[i].RoleId).val(0);
                //            }
                //        });
                //    } else {
                //        $('#view_9999').val(1);
                //    }
                //    if (grid.dataSource.view()[i].RoleName === 'Gerentes') $('#edit_' + grid.dataSource.view()[i].RoleId).val(1);
                //    initTriStateCheckBox('spanView_' + grid.dataSource.view()[i].RoleId, 'view_' + grid.dataSource.view()[i].RoleId, true);
                //    initTriStateCheckBox('spanEdit_' + grid.dataSource.view()[i].RoleId, 'edit_' + grid.dataSource.view()[i].RoleId, true);
                //}
                $('#spanEdit_9999').css({
                    'visibility': 'hidden'
                });
            }
        }
    });

    $('#btnUpdateCategory').click(function (e) {
        e.preventDefault();

        var categoryData = {
            Lang: 'pt-BR',
            CategoryName: my.vm.categoryName(),
            CategoryId: kendo.parseInt(my.vm.categoryId()),
            ParentCategoryId: my.vm.parentId(),
            ProductTemplate: '',
            ListItemTemplate: '',
            ListAltItemTemplate: '',
            SEOPageTitle: my.vm.seoPageTitle(),
            SEOName: my.vm.seoName(),
            MetaDescription: my.vm.metaDesc(),
            MetaKeywords: my.vm.metaKeywords(),
            ListOrder: my.vm.listOrder(),
            CategoryDesc: my.vm.categoryDesc(),
            Message: my.vm.message(),
            Archived: my.vm.archived(),
            Hidden: my.vm.hidden(),
            PortalId: _portalID,
            CreatedByUser: _userID,
            CreatedOnDate: kendo.toString(new Date(), 'u')
        };
        
        $.ajax({
            dataType: 'json',
            type: 'POST',
            url: '/desktopmodules/riw/api/categories/UpdateCategory',
            data: categoryData
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                if (my.vm.categoryId()) {

                    var selectedNode = $('#mainCategories').find('#' + my.vm.categoryId())[0];
                    $('#mainCategories').jqxTree('updateItem', selectedNode, { label: $('#nameTextBox').val() });

                    var selectedElement = $("#treeViewCategories").jqxTree('getSelectedItem').element;
                    $('#treeViewCategories').jqxTree('updateItem', selectedElement, { label: $('#nameTextBox').val() });

                    setTimeout(function () {
                        selectedElement.scrollIntoView();
                    }, 500);

                    var addCategorySecurity = function (args) {
                        $.ajax({
                            type: 'POST',
                            url: '/desktopmodules/riw/api/categories/UpdateCategorySecurity',
                            data: args
                        }).fail(function (jqXHR, textStatus) {
                            console.log(jqXHR.responseText);
                        });
                    };

                    var categorySecurityData = {
                        CategoryId: my.vm.categoryId()
                    };

                    var grid = $('#groupsGrid').data('kendoGrid');
                    $.each(grid.dataSource.data(), function (i, item) {

                        if ($('#view_' + item.RoleId).val() !== '0') {

                            categorySecurityData.RoleId = item.RoleId;
                            categorySecurityData.PermissionId = $('#view_' + item.RoleId).val();
                            categorySecurityData.AllowAccess = true;

                            addCategorySecurity(categorySecurityData);
                        }

                        if ($('#edit_' + item.RoleId).val() !== '0') {

                            categorySecurityData.RoleId = item.RoleId;
                            categorySecurityData.PermissionId = $('#edit_' + item.RoleId).val();
                            categorySecurityData.AllowAccess = false;

                            addCategorySecurity(categorySecurityData);
                        }
                    });

                    $().toastmessage('showSuccessToast', 'Categoria ' + my.vm.categoryName() + ' atualizada com sucesso.');
                } else {
                    my.vm.categoryId(data.CategoryId);

                    var selectedItem = null;
                    if (my.vm.parentId()) {
                        selectedItem = $('#treeViewCategories').find('#' + my.vm.parentId() + '')[0];
                    }
                    if (selectedItem !== null) {

                        $('#treeViewCategories').jqxTree('addTo', {
                            label: my.vm.categoryName(),
                            id: my.vm.categoryId()
                        }, selectedItem, false);
                        
                        var selectedMainItem = null;
                        selectedMainItem = $('#mainCategories').find('#' + my.vm.parentId() + '')[0];
                        $('#mainCategories').jqxTree('addTo', {
                            label: my.vm.categoryName(),
                            id: my.vm.categoryId()
                        }, selectedMainItem, true);

                    } else {

                        $('#mainCategories').jqxTree('addTo', {
                            label: my.vm.categoryName(),
                            id: my.vm.categoryId()
                        });

                        $('#treeViewCategories').jqxTree('addTo', {
                            label: my.vm.categoryName(),
                            id: my.vm.categoryId()
                        });
                    }
                    
                    $('#treeViewCategories').jqxTree('selectItem', $('#' + my.vm.categoryId() + '')[0]);

                    $().toastmessage('showSuccessToast', 'Categoria ' + my.vm.categoryName() + ' inserida com sucesso.');
                }

                $('#treeViewCategories').jqxTree('render');

                var menuItems = $('#editCategoryMenu').find('.k-state-disabled');
                menu.enable(menuItems, menuItems.hasClass('k-state-disabled'));

            } else {
                $().toastmessage('showErrorToast', data.Result);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
    });

    $('#btnAdddProductCategory').click(function (e) {
        e.preventDefault();

        var params = {
            ProductId: my.vm.productId(),
            CategoryId: my.vm.categoryId(),
        };
        
        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/products/addProductCategory',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.categoryProductsData.add({ ProductId: params.ProductId, ProductName: my.vm.productName(), ProductRef: my.vm.productRef() });
                $().toastmessage('showSuccessToast', 'O item ' + my.vm.productName() + ' foi adicionado à categoria!');
            } else {
                $().toastmessage('showErrorToast', data.Result);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
    });

    $('#btnRemoveProductsCategory').click(function (e) {
        e.preventDefault();
                
        $.ajax({
            type: 'DELETE',
            url: '/desktopmodules/riw/api/products/removeProductsCategory?categoryId=' + my.vm.categoryId()
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.categoryProductsData.data([]);
                $().toastmessage('showSuccessToast', 'Não há mais item algum<br /> relacionado à esta categoria.');
            } else {
                $().toastmessage('showErrorToast', data.Result);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
    });

    $('#btnRemove').click(function (e) {
        e.preventDefault();

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
                    url: '/desktopmodules/riw/api/categories/removeCategory?catId=' + my.vm.categoryId()
                }).done(function (data) {
                    if (data.Result.indexOf("success") !== -1) {

                        var selectedElement = $("#treeViewCategories").jqxTree('getSelectedItem').element;
                        $('#treeViewCategories').jqxTree('removeItem', selectedElement);
                        
                        var selectedNode = $('#mainCategories').find('#' + my.vm.categoryId())[0];
                        $('#mainCategories').jqxTree('removeItem', selectedNode);

                        $('#btnCancel').click();

                        $().toastmessage('showSuccessToast', 'Categoria excluida com sucesso');
                    } else {
                        $().toastmessage('showErrorToast', data.Result);
                    }
                }).fail(function (jqXHR, textStatus) {
                    console.log(jqXHR.responseText);
                });
            }

            setTimeout(function () {
                kendoWindow.data("kendoWindow").close();
            }, 500);

        }).end();
    });

    $('#btnCancel').click(function (e) {
        e.preventDefault();

        $('#btnCancel').hide();
        $('#btnRemove').hide();
        my.vm.hidden(false);
        my.vm.message('');
        my.vm.lang('pt-BR');
        my.vm.archived(false);
        my.vm.listOrder(1);
        my.vm.categoryId(0);
        my.vm.categoryName('');
        my.vm.categoryDesc('');
        my.vm.parentId(0);
        my.vm.seoName('');
        my.vm.seoPageTitle('');
        my.vm.metaDesc('');
        my.vm.metaKeywords('');
        my.vm.productCount(0);
        my.vm.catPermissions.removeAll();
        my.dropDownContent = '<div style="position: relative; margin-left: 5px; margin-top: 6px; color: #999; font-style: italic;"> Selecionar</div>';
        $('#availCategoriesButton').jqxDropDownButton('setContent', my.dropDownContent);
        var items = $('#editCategoryMenu').find(':not(.k-first)');
        menu.enable(items, items.hasClass('k-state-disabled'));
        $('#treeViewCategories').jqxTree('selectItem', $('#tvCategories').find('#0999999999')[0]);
        $('#nameTextBox').focus();
    });

});
