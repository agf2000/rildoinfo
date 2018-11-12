﻿
my.viewModel = function () {

    //ko.bindingHandlers.bootstrapSwitchOn = {
    //    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
    //        var $elem = $(element);
    //        $(element).bootstrapSwitch('setState', ko.utils.unwrapObservable(valueAccessor())); // Set intial state
    //        $elem.on('switch-change', function (e, data) {
    //            valueAccessor()(data.value);
    //        }); // Update the model when changed.
    //    },
    //    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
    //        var vStatus = $(element).bootstrapSwitch('status');
    //        var vmStatus = ko.utils.unwrapObservable(valueAccessor());
    //        if (vStatus !== vmStatus) {
    //            $(element).bootstrapSwitch('setState', vmStatus);
    //        }
    //    }
    //};

    my.permission = function () {
        this.CatPermissionId = ko.observable();
        this.CategoryId = ko.observable();
        this.PermissionId = ko.observable();
        this.RoleId = ko.observable();
        this.AllowAccess = ko.observable();
    };

    // knockout js view model
    my.vm = function () {
        // this is knockout view model
        var self = this;

        // view models
        self.categories = ko.observableArray([]),
        self.selectedCategories = ko.observableArray([]),
        self.selectedCatId = ko.observable(0),
        self.roles = ko.observableArray([]),
        self.permissions = ko.observableArray([]),
        
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
            var btnLabel = '<i class="icon-plus icon-white"></i>&nbsp; Inserir';
            if (self.categoryId()) {
                btnLabel = '<i class="fa fa-check"></i>&nbsp; Atualizar';
                $('#btnCancel').show();
                $('#btnRemove').show();
            }

            return btnLabel;
        });

        // make view models available for apps
        return {
            categories: categories,
            selectedCategories: selectedCategories,
            selectedCatId: selectedCatId,
            roles: roles,
            permissions: permissions,
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
};