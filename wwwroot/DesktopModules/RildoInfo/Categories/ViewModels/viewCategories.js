
$(function () {

    // category view model
    my.Category = function () {
        this.categoryId = ko.observable();
        this.categoryName = ko.observable();
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
    my.categoryViewModel = function () {
        // this is knockout view model
        var self = this;

        // view models
        self.categories = ko.observableArray([]),
        self.selectedCategories = ko.observableArray([]),
        self.selectedCatId = ko.observable(0),
        self.roles = ko.observableArray([]),
        self.permissions = ko.observableArray([]);

        // make view models available for apps
        return {
            categories: categories,
            selectedCategories: selectedCategories,
            selectedCatId: selectedCatId,
            roles: roles,
            permissions: permissions,
        };

    }();

    // apply ko bindings
    ko.applyBindings(my.categoryViewModel);

    // ajax call to get logged in user's roles and add them to ko view model roles
    my.loadRoles = function () {
        $.ajax({
            url: '/desktopmodules/riw/api/ristore/GetRolesByUser?portalId=' + _portalID + '&uId=' + _userID,
            async: false
        }).done(function (data) {
            if (data) {
                my.categoryViewModel.roles.removeAll();
                //$.grep(data, function (role, i) {
                //    if (role.RoleName === 'Clientes') my.vm.roleId(role.RoleID);
                //    else if (role.RoleName === 'Gerentes') my.vm.roleId(role.RoleID);
                //    else if (role.RoleName === 'Vendedores') my.vm.roleId(role.RoleID);
                //});
                $.each(data, function (i, role) {
                    my.categoryViewModel.roles.push(new my.Role()
                        .roleId(role.RoleID)
                        .roleName(role.RoleName));
                });
            }
        });
    };

    // ajax call to get permission for the categories
    my.loadPermissions = function () {
        $.ajax({
            url: '/desktopmodules/riw/api/categories/GetCategoriesRolePermissions?roleId=9999',
            async: false
        }).done(function (data) {
            if (data) {
                my.categoryViewModel.permissions.removeAll();
                $.each(data, function (i, per) {
                    my.categoryViewModel.permissions.push(new my.Permission()
                        .permissionId(per.PermissionId)
                        .categoryId(per.CategoryId)
                        .roleId(per.RoleId));
                });
            }
        });
    };

    // ajax call to get categories
    my.loadCategories = function () {

        // check if categories haven't been loaded already
        if (my.categoryViewModel.categories().length === 0) {
            $.ajax({
                url: '/desktopmodules/riw/api/categories/Categories?portalId=' + _portalID + '&lang=pt-BR&includeArchived=false',
                async: false
            }).done(function (data) {
                // if categories return from ajax call
                if (data) {

                    // go thru each return category
                    $.grep(data, function (cat, ic) {

                        // users in management role can see all active categoires
                        if (_managerRole === 'True' && cat.Active === true) {

                            // fill ko view model categories
                            my.categoryViewModel.categories.push(cat);
                        } else {

                            // else if logged in user is just a client, go thru each permission
                            $.grep(my.categoryViewModel.permissions(), function (per, ip) {

                                // check for category's permissions
                                if (cat.CategoryId === per.categoryId()) {

                                    // check if user is logged in
                                    if (_userID < 0) {

                                        // if user is not logged in, check if category can be seing by anonymouse
                                        if (per.roleId() === 9999) {
                                            my.categoryViewModel.categories.push(cat);
                                        }
                                    } else {

                                        // add category to ko view model categories if any user can see the category
                                        if (per.roleId() === 9999) {
                                            my.categoryViewModel.categories.push(cat);
                                        } else {

                                            // else, go thru each role and add category to ko view model categories if user is in a role that can see the category
                                            $.grep(my.categoryViewModel.roles(), function (role, ir) {
                                                if (per.roleId() === role.roleId()) {
                                                    my.categoryViewModel.categories.push(cat);
                                                }
                                            });
                                        }
                                    }
                                }
                            });
                        }
                    });
                }
            });
        } else {

            // else just return the already loaded categories
            return my.categoryViewModel.categories();
        }
    };

    if (_userID) my.loadRoles();
    my.loadPermissions();
    my.loadCategories();

    my.buildMenudata = function () {
        var source = [];
        var items = [];
        for (i = 0; i < my.categoryViewModel.categories().length; i++) {
            var item = my.categoryViewModel.categories()[i];
            var text = item.Name;
            var parentid = item.ParentId;
            var id = item.CategoryId;
            var hasProducts = item.ProductsAssigned;

            if (items[parentid]) {
                item = {
                    parentid: parentid,
                    text: text,
                    item: item,
                    hasProducts: hasProducts
                };
                if (!items[parentid].items) {
                    items[parentid].items = [];
                }
                items[parentid].items[items[parentid].items.length] = item;
                items[id] = item;
            } else {
                items[id] = {
                    parentid: parentid,
                    text: text,
                    item: item,
                    hasProducts: hasProducts
                };
                source[id] = items[id];
            }
        }
        return source;
    };

    var sourceMenu = my.buildMenudata();
    var buildMenuUL = function (parent, items) {
        $.each(items, function () {
            if (this.text) {
                // create LI element and append it to the parent element.
                var li = null;
                if (this.hasProducts > 0) {
                    li = $('<li id="' + this.item.CategoryId + '" title="' + this.text + '">' + this.text + '</li>');
                } else {
                    li = $('<li id="' + this.item.CategoryId + '" title="Sem Produtos">' + this.text + '</li>');
                }
                li.appendTo(parent);
                // if there are sub items, call the buildUL function.
                if (this.items && this.items.length > 0) {
                    var ul = $("<ul></ul>");
                    ul.appendTo(li);
                    buildMenuUL(ul, this.items);
                }
            }
        });
    };
    var ulMenu = $("<ul></ul>");
    ulMenu.appendTo("#catMenu");
    buildMenuUL(ulMenu, sourceMenu);

    $('#catMenu').jqxMenu({
        width: '120',
        mode: 'vertical'
    });

    $('#catMenu').on('itemclick', function (event) {
        // get the clicked LI element.
        event.preventDefault();

        if (event.args.attributes.kit.value === 'true') {
            my.addKitItems(event.args.id);
        } else if (event.args.attributes.title.value !== 'Sem Produtos') {
            {
                if (window.location.pathname.indexOf('produtos') !== -1) {
                    document.location.hash = 'catId/' + event.args.id;
                    //my.categoryViewModel.quick(2);
                    my.loadProducts(event.args.id);

                    if (my.getParameterByName('catId') > 0) {
                        $('#status').html('Categoria Selecionada: <strong>' + event.args.textContent + '</strong>');
                    }
                } else {
                    document.location.href = '/produtos#catId/' + event.args.id;
                }
            }
        }
    });

    if (my.getParameterByName('catId') > 0) {
        $('#status').html('Categoria Selecionada: <strong>' + $('li[id=' + my.getParameterByName('catId') + ']').text() + '</strong>');
    }

});