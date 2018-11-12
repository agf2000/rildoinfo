
my.viewModel = function () {

    // category view model
    my.Category = function () {
        this.categoryId = ko.observable();
        this.categoryName = ko.observable();
    };

    // product view model
    my.Product = function () {
        var self = this;

        self.productId = ko.observable();
        //self.productRef = ko.observable();
        //self.barcode = ko.observable();
        self.itemType = ko.observable();
        self.productCode = ko.observable();
        self.productName = ko.observable();
        self.summary = ko.observable();
        self.productUnit = ko.observable();
        self.unitValue = ko.observable();
        self.finan_Sale_Price = ko.observable();
        self.finan_Special_Price = ko.observable();
        self.qTy = ko.observable();
        self.qTyStockSet = ko.observable();
        self.showPrice = ko.observable();
        self.totalValue = ko.observable();
        self.visible = ko.observable(true);
        self.increaseQuantity = function (quantity) {
            self.qTy(self.qTy() + quantity);
        };
    };

    // role view model
    my.Role = function () {
        this.roleId = ko.observable();
        this.roleName = ko.observable();
    };

    // permission view model
    my.Permission = function () {
        this.permissionId = ko.observable();
        this.roleId = ko.observable();
        this.categoryId = ko.observable();
    };

    // knockout js view model
    my.vm = function () {
        // this is knockout view model
        var self = this;

        // view models
        self.removedItem = ko.observable(0),
        self.estimatedItems = ko.observableArray([]),
        self.categories = ko.observableArray([]),
        self.categoryId = ko.observable(),
        self.selectedCategories = ko.observableArray([]),
        self.selectedCatId = ko.observable(0),
        //self.products = ko.observableArray([]),
        self.selectedProducts = ko.observableArray([]),
        self.displayName = ko.observable(displayName),
        //self.itemType = ko.observable(1),
        self.filterTerm = ko.observable(''),
        self.roles = ko.observableArray([]),
        //self.roleId = ko.observable(9999),
        self.permissions = ko.observableArray([]),
        //self.quick = ko.observable(1),
        self.emailTo = ko.observable(userInfoEmail),
        //self.displayName = ko.observable(displayName),
        //self.unitTypes = ko.observableArray([]),
        //self.selectedUnit = ko.observable();
        //self.vendors = ko.observableArray([]),
        //self.selectedVendors = ko.observableArray([]),
        //self.vendorSearch = ko.observable();
        //self.brands = ko.observableArray([]),
        //self.selectedBrandId = ko.observable(),
        //self.models = ko.observableArray([]),
        //self.selectedModelId = ko.observable(0),
        //self.selectedProdImage = ko.observable(),
        //self.prodImages = ko.observableArray([]),
        //self.selectedParentId = ko.observable(0),
        //self.prodVideos = ko.observableArray([]),
        self.productId = ko.observable(),
        self.itemType = ko.observable(),
        //self.productRef = ko.observable();
        //self.barcode = ko.observable();
        self.productCode = ko.observable(),
        self.productName = ko.observable(),
        self.summary = ko.observable(),
        self.productUnit = ko.observable(),
        self.unitTypeAbbv = ko.observable(),
        self.unitValue = ko.observable(),
        self.finan_Sale_Price = ko.observable(),
        self.finan_Special_Price = ko.observable(),
        self.productQty = ko.observable(),
        self.qTyStockSet = ko.observable(),
        self.productImageId = ko.observable(0),
        self.productImageExtension = ko.observable(),
        self.productImages = ko.observableArray([]),
        self.productOptions = ko.observableArray([]),
        self.productsRelated = ko.observableArray([]),
        self.productsRelatedCount = ko.observable(),
        self.description = ko.observable(),
        self.showPrice = ko.observable(),
        self.sendNewEstimateTo = ko.observable(),
        self.allowPurchase = ko.observable(JSON.parse(allowPurchase.toLowerCase())),

        // function to update a cart item's quantity
        self.updateItem = function (item) {

            item.qTy(kendo.parseFloat(item.qTy()));
            item.totalValue(kendo.parseFloat(item.qTy()) * item.unitValue());

            // check for browser storage availability
            if (my.storage) {
                sessionStorage.clear();
                amplify.store.sessionStorage(siteURL + '_products', ko.toJSON(my.vm.selectedProducts()));
            }

            // check if logged in user has credentials to see amounts, like managers and sales person
            //if (_authorized < 2) {
            //    $('.columnValue').hide();
            //    $('.tFoot').hide();
            //}

            // show cart item's updated message
            //$().toastmessage('showSuccessToast', 'Item atualizado com sucesso.');
            $.pnotify({
                title: 'Sucesso!',
                text: 'Item atualizado.',
                type: 'success',
                icon: 'fa fa-check fa-lg',
                addclass: "stack-bottomright",
                stack: my.stack_bottomright
            });
        },

        // function to remove a cart item
        self.removeItem = function () {

            self.removedItem(this.productId());

            // remove the cart item
            self.selectedProducts.remove(this);

            ko.utils.arrayFirst(my.vm.estimatedItems(), function (item) {
                if (item) {
                    if (item.productId() === self.removedItem()) self.estimatedItems.remove(item);
                }
            });

            // empty storage session
            sessionStorage.clear();

            if (self.selectedProducts().length > 0) {
                // convert view model selectedProducts to string and add to storage via amplify 
                amplify.store.sessionStorage(siteURL + '_products', ko.toJSON(my.vm.selectedProducts()));
            }

            // if there are no more items left on ko view model selectedProducts
            if (my.vm.selectedProducts().length === 0) {

                $('.divButtons').fadeOut();

                my.vm.estimatedItems([]);

                my.loadProducts(0);

                // collapse cart items table
                $('#estimateItems').slideUp();

                // set expand cart button's text to empty cart and disabled the button
                //$('#btnExpandCart').html('<span class="k-icon k-i-cancel"></span> Carrinho Vazio').addClass('k-state-disabled');

                // uncheckbox checkbox for keeping cart expanded
                $('#divCheckExpand input').removeAttr('checked');

                // hide checkbox for keeping cart expanded
                //$('#divCheckExpand').delay(400).hide();
            }

            // if there are items left in cart, check if logged in user has credentials to see amounts, like managers and sales person
            //if (_authorized < 2) {
            //    $('.columnValue').hide();
            //    $('.tFoot').hide();
            //}

            //setTimeout(function () { 
            // show the selected item's button 
            $('#btnEst_' + self.removedItem()).show();
            //}, 2000);
        },

        // format message to managers and sales person
        //self.estimateMsg = ko.computed(function () {
        //    // return _userID > 0 ? 'Caro(a) ' + my.vm.displayName() + ', nós não fazemos venda direta pelo webite. Adicione os produtos desejados ao seu carrinho para gerar um orçamento instantânio.' :
        //    // 'Caro(a) visitante, nós não fazemos venda direta pelo webite. Adicione os produtos desejados ao seu carrinho para gerar um orçamento instantânio. Lembre-se que é necessário criar uma conta no site e estar logado.'

        //    // add message variable to memory
        //    var result = null;

        //    // check if a user is logged in
        //    if (userID > 0) {

        //        // check if logged in user has credentials to see amounts, like managers and sales person
        //        if (authorized > 1) {

        //            // logged in user can see amounts but can not request estimate
        //            result = '<span style="color: red;">O grupo a qual pertence não lhe permite requisitar orçamentos.</span>';
        //        }
        //    }

        //    // return messsage from memory
        //    return result; // _userID > 0 ? _authorized > 0 ? '<span style="color: red;">O grupo a qual pertence não lhe permite finalizar orçamento</span>' : _estimateIntroMsg.replace('[CLIENTE]', self.displayName()) : _visitorIntroMsg;
        //}, self),

        self.incomplete = ko.observable(false);

        // make view models available for apps
        return {
            estimatedItems: estimatedItems,
            categories: categories,
            categoryId: categoryId,
            selectedCategories: selectedCategories,
            selectedCatId: selectedCatId,
            selectedProducts: selectedProducts,
            displayName: displayName,
            filterTerm: filterTerm,
            roles: roles,
            permissions: permissions,
            emailTo: emailTo,
            productId: productId,
            itemType: itemType,
            productCode: productCode,
            productName: productName,
            productsRelatedCount: productsRelatedCount,
            summary: summary,
            productUnit: productUnit,
            unitTypeAbbv: unitTypeAbbv,
            unitValue: unitValue,
            finan_Special_Price: finan_Special_Price,
            finan_Sale_Price: finan_Sale_Price,
            productQty: productQty,
            qTyStockSet: qTyStockSet,
            productImageId: productImageId,
            productImageExtension: productImageExtension,
            productImages: productImages,
            productOptions: productOptions,
            productsRelated: productsRelated,
            description: description,
            updateItemupdateItem: updateItem,
            removeItem: removeItem,
            //estimateMsg: estimateMsg,
            allowPurchase: allowPurchase,
            showPrice: showPrice,
            removedItem: removedItem,
            incomplete: incomplete
        };
    }();

    // compute and format cart's footer with total amounts
    my.vm.extendedPrice = ko.computed(function () {

        // add total variable to memory
        var total = 0;

        var complete = false
        
        // go thru each item in ko view model selectedProducts
        $.each(this.selectedProducts(), function (i, p) {
            
            if (p.showPrice() || authorized > 1) {

                // add up item's amount to total
                total += kendo.parseFloat(p.totalValue());

            } else if (!p.showPrice()) {
                if (!complete) {
                    my.vm.incomplete(true);
                    complete = true;
                }
            }
        });

        // return grand total from memory
        return total;
    }, my.vm);

    // apply ko bindings
    ko.applyBindings(my.vm);

    // authorization check function
    my.authorizationCheck = function () {
        if (authorized > 1) {
            $('#btnSaveEstimate').html('<span class="fa fa-chevron-down"></span>&nbsp; Requisitar Orçamento').attr({ 'disabled': true });
        //} else {
        //    $('.columnValue').hide();
        //    $('.tFoot').hide();
        } else if (authorized === 3) {
            $('#configLink').attr({ 'href': configURL }).html('Configurações');
        }
    };
    
    // load products into list view from datasource
    my.loadProducts = function (id) {
        my.vm.selectedCatId(id);
        //my.vm.quick(2);

        my.createListView();

        // create initial pager from datasource
        my.createPager();
        if ($('#pager').data('kendoPager')) {
            $('#productDetail').hide();
            $('.listView').show();
            $('#pager').data('kendoPager').dataSource.page(1);
            my.products.read();
            $("#listView").data('kendoListView').dataSource.page(1);
        }
        //$("#listView").data('kendoListView').dataSource.read();
    };

    my.converter = new Showdown.converter();

};