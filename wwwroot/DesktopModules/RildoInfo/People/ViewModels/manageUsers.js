
$(function () {

    my.group = 'Vendedores';

    my.viewModel();

    $('#moduleTitleSkinObject').html(moduleTitle);

    var buttons = '<ul class="inline">';
    buttons += '<li><a href="' + categoriesManagerURL + '" class="btn btn-primary btn-medium" title="Categorias"><i class="fa fa-sitemap fa-lg"></i></a></li>';
    buttons += '<li><a href="' + productsManagerURL + '" class="btn btn-primary btn-medium" title="Produtos"><i class="fa fa-barcode fa-lg"></i></a></li>';
    buttons += '<li><a href="' + peopleManagerURL + '" class="btn btn-primary btn-medium" title="Entidades"><i class="fa fa-group fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + usersManagerURL + '" class="btn btn-primary btn-medium" title="Colaboradores"><i class="fa fa-suitcase fa-lg"></i></a></li>';
    buttons += '<li><a href="' + invoicesManagerURL + '" class="btn btn-primary btn-medium" title="Lançamentos"><i class="fa fa-money fa-lg"></i></a></li>';
    buttons += '<li><a href="' + accountsManagerURL + '" class="btn btn-primary btn-medium" title="Contas"><i class="fa fa-book fa-lg"></i></a></li>';
    buttons += '<li><a href="' + agendaURL + '" class="btn btn-primary btn-medium" title="Agenda"><i class="fa fa-calendar fa-lg"></i></a></li>';
    buttons += '<li><a href="' + orURL + '" class="btn btn-primary btn-medium" title="Website"><i class="fa fa-shopping-cart fa-lg"></i></a></li>';
    buttons += '<li><a href="' + reportsManagerURL + '" class="btn btn-primary btn-medium" title="Relatórios"><i class="fa fa-bar-chart-o fa-lg"></i></a></li>';
    buttons += '<li><a href="' + storeManagerURL + '" class="btn btn-primary btn-medium" title="Loja"><i class="fa fa-cogs fa-lg"></i></a></li>';
    buttons += '</ul>';
    $('#buttons').html(buttons);

    $('.icon-exclamation-sign').popover({
        delay: {
            show: 500,
            hide: 100
        },
        placement: 'top',
        trigger: 'hover'
    });

    $('#kddlRoles').kendoDropDownList({
        //autoBind: false,
        optionLabel: "Todos os Grupos",
        dataTextField: 'RoleName',
        dataValueField: 'RoleId',
        select: function (e) {
            var dataItem = this.dataItem(e.item.index());
            if (dataItem.RoleId > 0) {
                my.group = dataItem.RoleName;
            } else {
                my.group = '';
            }
            my.peopleData.read();
        },
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/store/getRolesByRoleGroup?portalId=' + portalID + '&roleGroupName=Departamentos'
                }
            }
        },
        dataBound: function (e) {
            if (this.dataSource.view().length > 0) {
                this.text('Vendedores');
            }
        }
    });

    my.peopleTransport = {
        read: {
            url: '/desktopmodules/riw/api/people/getEmployees'
        },
        parameterMap: function (data, type) {
            return {
                portalId: portalID,
                roleGroupName: 'Departamentos',
                roleName: my.group,
                isDeleted: '',
                orderBy: my.convertSortingParameters(data.sort)
            };
        }
    };

    my.peopleData = new kendo.data.DataSource({
        transport: my.peopleTransport,
        pageSize: 20,
        //serverPaging: true,
        //serverSorting: true,
        //serverFiltering: true,
        sort: { field: "FirstName", dir: "ASC" },
        schema: {
            model: {
                id: 'UserId',
                fields: {
                    UserId: { type: 'number' },
                    FirstName: { type: 'string' },
                    LastName: { type: 'string' },
                    ModifiedOnDate: { type: "date", format: "{0:g}" }
                }
            }
            //data: 'data',
            //total: 'total'
        }
    });

    function detailInit(e) {
        var detailRow = e.detailRow;

        var tabStrip = detailRow.find('.personDetails');
        $(tabStrip).kendoTabStrip({
            animation: {
                // fade-out current tab over 1000 milliseconds
                close: {
                    effects: "fadeOut"
                },
                // fade-in new tab over 500 milliseconds
                open: {
                    effects: "fadeIn"
                }
            }
        });
    }

    $("#peopleGrid").kendoGrid({
        //height: 385,
        dataSource: my.peopleData,
        selectable: "row",
        change: function () {
            var row = this.select();
            var id = row.data("uid");
            my.uId = id;
            var dataItem = this.dataItem(row);
            if (dataItem) {
                $('#btnEditSelected').attr({ 'disabled': false });
                if (dataItem.Locked) {
                    if (authorized > 2) {
                        $('#btnDeleteSelected').show();
                    }
                    $('#btnRemoveSelected').hide();
                } else {
                    if (authorized > 2) {
                        $('#btnRemoveSelected').show();
                    } else {
                        $('#btnRemoveSelected').hide();
                    }
                    $('#btnDeleteSelected').show();
                }
                if (dataItem.IsDeleted) {
                    $('#btnRestoreSelected').show();
                    $('#btnDeleteSelected').hide();
                } else {
                    $('#btnRestoreSelected').hide();
                }

            }
        },
        toolbar: kendo.template($("#tmplToolbar").html()),
        columns: [
            { field: "UserId", title: "ID", width: 50 },
            { field: "FirstName", title: "Nome", width: 160 },
            { field: "LastName", title: "Sobrenome", width: 160 },
            { field: "ModifiedOnDate", title: "Data", type: "date", format: "{0:g}", width: 130 }
        ],
        sortable: true,
        pageable: {
            pageSizes: [20, 40, 60],
            refresh: true,
            numeric: false,
            input: true,
            messages: {
                display: "{0} - {1} de {2} Colaboradores(as)",
                empty: "Sem Registro.",
                page: "Página",
                of: "de {0}",
                itemsPerPage: "Colaboradores(as) por página",
                first: "Ir para primeira página",
                previous: "Ir para página anterior",
                next: "Ir para próxima página",
                last: "Ir para última página",
                refresh: "Recarregar"
            }
        },
        //scrollable: true,
        dataBound: function () {
            $('#btnRestoreSelected').hide();
            $('#btnDeleteSelected').hide();
            $('#btnRemoveSelected').hide();
            if (this.dataSource.view().length > 0) {
                var grid = this;
                //_chkDeleted = false;
                $.each(grid.dataSource.data(), function (i, person) {
                    var rowSelector = ">tr:nth-child(" + (i + 1) + ")";
                    var row = grid.tbody.find(rowSelector);
                    if (person.IsDeleted) {
                        row.addClass('isDeleted');
                    }
                });
            }
        }
        //detailTemplate: kendo.template($("#tmplPersonDetail").html()),
        //detailInit: detailInit
    });

    $("#peopleGrid").delegate("tbody > tr", "dblclick", function () {
        $('#btnEditSelected').click();
    });

    $('#btnEditSelected').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var grid = $('#peopleGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        $("#personWindow").append("<div id='window'></div>");
        var sContent = editUserURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#userId/' + dataItem.UserId + '/sel/1',
            kendoWindow = $('#window').kendoWindow({
                actions: ["Maximize", "Close"],
                title: dataItem.DisplayName + ' (ID: ' + dataItem.UserId + ')',
                modal: true,
                width: '90%',
                height: '80%',
                content: sContent,
                close: function (e) {
                    $("html, body").css("overflow", "");
                },
                open: function () {
                    $("html, body").css("overflow", "hidden");
                    $('#window').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
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

    $('#btnDeleteSelected').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var grid = $('#peopleGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        var params = {
            PortalId: portalID,
            UserId: dataItem.UserId
        };

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
                                    click: function (e) {
                                        $.ajax({
                                            type: 'PUT',
                                            url: '/desktopmodules/riw/api/people/DeleteUser',
                                            data: params,
                                            beforeSend: function () {
                                                $this.button('loading');
                                                $dialog.dialog('close');
                                                $dialog.dialog('destroy');
                                            }
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                //grid.dataSource.read();
                                                grid.tbody.find(grid.select()).addClass('isDeleted');
                                                //grid.refresh();
                                                $('#btnDeleteSelected').hide();
                                                $('#btnRemoveSelected').hide();
                                                $('#btnRestoreSelected').show();
                                                //$().toastmessage('showSuccessToast', 'Conta desativada com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Conta desativada.',
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

    $('#btnRemoveSelected').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var grid = $('#peopleGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        var $dialog = $('<div></div>')
                        .html('<div class="confirmDialog">Tem Certeza? Todas as informa&#231;&#245;es referente &#224; esta conta ser&#227;o excluidas. Esta a&#231;&#227;o n&#227;o poder&#225; ser revertida.</div>')
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
                                    click: function (e) {
                                        $.ajax({
                                            type: 'DELETE',
                                            url: '/desktopmodules/riw/api/people/RemoveUser?portalId=' + portalID + '&userId=' + dataItem.UserId,
                                            beforeSend: function () {
                                                $this.button('loading');
                                                $dialog.dialog('close');
                                                $dialog.dialog('destroy');
                                            }
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                //grid.dataSource.read();
                                                grid.dataSource.remove(dataItem);
                                                //grid.refresh();
                                                $('#btnDeleteSelected').hide();
                                                $('#btnRemoveSelected').hide();
                                                $('#btnRestoreSelected').hide();
                                                //$().toastmessage('showSuccessToast', 'Conta excluida com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Conta excluida.',
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

        var grid = $('#peopleGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        var params = {
            PortalId: portalID,
            UserId: dataItem.UserId
        };

        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/people/RestoreUser',
            data: params,
            beforeSend: function () {
                $this.button('loading');
            }
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                grid.tbody.find(grid.select()).removeClass('isDeleted');
                //grid.refresh();
                $('#btnDeleteSelected').show();
                $('#btnRemoveSelected').hide();
                $('#btnRestoreSelected').hide();
                //$().toastmessage('showSuccessToast', 'Conta ativada com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Conta ativada.',
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
        });
    });

    $("#btnAddNewUser").click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $("#personWindow").append("<div id='window'></div>");
        var sContent = editUserURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&group=' + $('#kddlRoles').data('kendoDropDownList').value(),
            kendoWindow = $('#window').kendoWindow({
                actions: ["Maximize", "Close"],
                title: 'Novo Colaborador(a)',
                modal: true,
                width: '90%',
                height: '80%',
                content: sContent,
                close: function (e) {
                    $("html, body").css("overflow", "");
                    my.peopleData.read();
                },
                open: function () {
                    $("html, body").css("overflow", "hidden");
                    $('#window').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
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

});
