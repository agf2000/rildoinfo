$(function () {

    if (!ko.dataFor(document.getElementById('divEstimate'))) {
        my.viewModel();
    }

    // ajax call to get logged in user's roles and add them to ko view model roles
    my.loadRoles = function () {
        $.ajax({
            url: '/desktopmodules/riw/api/store/GetRolesByUser?portalId=' + portalID + '&userId=' + userID,
            async: false
        }).done(function (data) {
            if (data) {
                my.vm.roles.removeAll();
                //$.grep(data, function (role, i) {
                //    if (role.RoleName === 'Clientes') my.vm.roleId(role.RoleID);
                //    else if (role.RoleName === 'Gerentes') my.vm.roleId(role.RoleID);
                //    else if (role.RoleName === 'Vendedores') my.vm.roleId(role.RoleID);
                //});
                $.each(data, function (i, role) {
                    my.vm.roles.push(new my.Role()
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
                my.vm.permissions.removeAll();
                $.each(data, function (i, per) {
                    my.vm.permissions.push(new my.Permission()
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
        if (my.vm.categories().length === 0) {
            $.ajax({
                url: '/desktopmodules/riw/api/categories/categories?portalId=' + portalID + '&lang=pt-BR',
                async: false
            }).done(function (data) {
                // if categories return from ajax call
                if (data) {

                    // go thru each return category
                    $.grep(data, function (cat, ic) {

                        // users in management role can see all active categoires
                        if (managerRole === 'True' && cat.Hidden !== true) {

                            // fill ko view model categories
                            my.vm.categories.push(cat);
                        } else {

                            // else if logged in user is just a client, go thru each permission
                            $.grep(my.vm.permissions(), function (per, ip) {

                                // check for category's permissions
                                if (cat.CategoryId === per.categoryId()) {

                                    // check if user is logged in
                                    if (userID < 0) {

                                        // if user is not logged in, check if category can be seing by anonymouse
                                        if (per.roleId() === 9999) {
                                            my.vm.categories.push(cat);
                                        }
                                    } else {

                                        // add category to ko view model categories if any user can see the category
                                        if (per.roleId() === 9999) {
                                            my.vm.categories.push(cat);
                                        } else {

                                            // else, go thru each role and add category to ko view model categories if user is in a role that can see the category
                                            $.grep(my.vm.roles(), function (role, ir) {
                                                if (per.roleId() === role.roleId()) {
                                                    my.vm.categories.push(cat);
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
            return my.vm.categories();
        }
    };

    if (userID) my.loadRoles();
    my.loadPermissions();
    my.loadCategories();

    my.buildMenudata = function () {
        var source = [];
        var categoryItems = [];
        for (i = 0; i < my.vm.categories().length; i++) {
            var item = my.vm.categories()[i];
            var text = item.CategoryName;
            var id = item.CategoryId;
            var desc = item.CategoryDesc;
            var parentid = item.ParentCategoryId;
            var code = item.CategoryId;
            var productsCount = item.ProductCount;

            if (categoryItems[parentid]) {
                item = {
                    parentid: parentid,
                    text: text,
                    id: id,
                    desc: desc,
                    productsCount: productsCount,
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
                    productsCount: productsCount,
                    item: item
                };
                source[code] = categoryItems[code];
            }
        }
        return source;
    };
    my.sourceMenu = my.buildMenudata();

    var countMenu = 1;
    my.buildMenuUL = function (parent, items) {
        var li = $('<li id="0999999999" style="display: none">hidden</li>');
        if (countMenu <= 1) {
            li.appendTo(parent);
            countMenu = countMenu + 1;
        }
        $.each(items, function () {
            if (this.text) {

                if (this.productsCount > 0) {
                    li = $('<li id="' + this.item.CategoryId + '" title="' + this.text + '">' + this.text + '</li>');
                } else {
                    li = $('<li id="' + this.item.CategoryId + '" title="Sem Produtos" disabled class="isDeleted">' + this.text + '</li>');
                }
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
    my.ulMenu.appendTo("#catMenu");
    my.buildMenuUL(my.ulMenu, my.sourceMenu);

    $('#catMenu').jqxMenu({
        width: '120',
        mode: 'vertical'
    });

    $('#catMenu').on('itemclick', function (event) {
        // get the clicked LI element.
        event.preventDefault();

        if (event.args.attributes.title.value !== 'Sem Produtos') {
            {
                if (window.location.pathname.toLowerCase().indexOf('produtos') !== -1) {

                    document.location.hash = 'catId/' + event.args.id;
                    //my.vm.quick(2);
                    my.vm.categoryId(event.args.id);
                    my.loadProducts(event.args.id);

                    if (my.getParameterByName('catId') > 0) {
                        $('#status').html('Categoria Selecionada: <strong>' + event.args.textContent + '</strong>');
                        $('#clearFilter').show();
                    }

                    my.createListView();

                    // create initial pager from datasource
                    my.createPager();

                    $('#productDetail').hide();
                    $('.listView').show();
                } else {
                    document.location.href = productsURL + '#catId/' + event.args.id;
                }
            }
        }
    });

    //$('.product').css({
    //    'width': '750px !important'
    //});

});