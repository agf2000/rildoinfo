
$(function () {

    $('#moduleTitleSkinObject').html(moduleTitle);

    var buttons = '<ul class="inline">';
    buttons += '<li><a href="' + addressURL + '" class="btn btn-primary btn-medium" title="Endereço"><i class="fa fa-book fa-lg"></i></a></li>';
    buttons += '<li><a href="' + payCondsURL + '" class="btn btn-primary btn-medium" title="Formas e Condições de Pagamento"><i class="fa fa-credit-card fa-lg"></i></a></li>';
    buttons += '<li><a href="' + syncURL + '" class="btn btn-primary btn-medium" title="Orçamento"><i class="fa fa-refresh fa-lg"></i></a></li>';
    buttons += '<li><a href="' + estimateURL + '" class="btn btn-primary btn-medium" title="Orçamento"><i class="fa fa-usd fa-lg"></i></a></li>';
    buttons += '<li><a href="' + smtpURL + '" class="btn btn-primary btn-medium" title="SMTP"><i class="fa fa-envelope-o fa-lg"></i></a></li>';
    buttons += '<li><a href="' + statusesManagerURL + '" class="btn btn-primary btn-medium" title="Status"><i class="fa fa-check-circle fa-lg"></i></a></li>';
    buttons += '<li><a href="' + websiteManagerURL + '" class="btn btn-primary btn-medium" title="Website"><i class="fa fa-bookmark fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + templatesManagerURL + '" class="btn btn-primary btn-medium" title="Templates"><i class="fa fa-puzzle-piece fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + davReturnsURL + '" class="btn btn-primary btn-medium" title="DAVs"><i class="fa fa-briefcase fa-lg"></i></a></li>';
    buttons += '</ul>';
    $('#buttons').html(buttons);

    $('#companyShow').attr({ 'checked': JSON.parse(askCompany.toLowerCase()) });
    $('#companyReq').attr({ 'checked': JSON.parse(reqCompany.toLowerCase()) });

    $('#lastNameShow').attr({ 'checked': JSON.parse(askLastName.toLowerCase()) });
    $('#lastNameReq').attr({ 'checked': JSON.parse(reqLastName.toLowerCase()) });

    $('#industryShow').attr({ 'checked': JSON.parse(askIndustry.toLowerCase()) });
    $('#industryReq').attr({ 'checked': JSON.parse(reqIndustry.toLowerCase()) });

    $('#phoneShow').attr({ 'checked': JSON.parse(askTelephone.toLowerCase()) });
    $('#phoneReq').attr({ 'checked': JSON.parse(reqTelephone.toLowerCase()) });

    $('#einShow').attr({ 'checked': JSON.parse(askEIN.toLowerCase()) });
    $('#einReq').attr({ 'checked': JSON.parse(reqEIN.toLowerCase()) });

    $('#stateTaxShow').attr({ 'checked': JSON.parse(askST.toLowerCase()) });
    $('#stateTaxReq').attr({ 'checked': JSON.parse(reqST.toLowerCase()) });

    $('#cityTaxShow').attr({ 'checked': JSON.parse(askCT.toLowerCase()) });
    $('#cityTaxReq').attr({ 'checked': JSON.parse(reqCT.toLowerCase()) });

    $('#ssnShow').attr({ 'checked': JSON.parse(askSSN.toLowerCase()) });
    $('#ssnReq').attr({ 'checked': JSON.parse(reqSSN.toLowerCase()) });

    $('#identShow').attr({ 'checked': JSON.parse(askIdent.toLowerCase()) });
    $('#identReq').attr({ 'checked': JSON.parse(reqIdent.toLowerCase()) });

    $('#websiteShow').attr({ 'checked': JSON.parse(askWebsite.toLowerCase()) });
    $('#websiteReq').attr({ 'checked': JSON.parse(reqWebsite.toLowerCase()) });

    $('#addressShow').attr({ 'checked': JSON.parse(askAddress.toLowerCase()) });
    $('#addressReq').attr({ 'checked': JSON.parse(reqAddress.toLowerCase()) });

    my.addNewIndustry = function (e) {

        kendoWindow = $('#newIndustry').kendoWindow({
            title: 'Novo Ramo de Atividade',
            modal: true,
            width: '90%',
            height: '80%',
            close: function (e) {
                $("html, body").css("overflow", "");
            },
            open: function () {
                $("html, body").css("overflow", "hidden");
            }
        });

        kendoWindow.data("kendoWindow").center().open();
    };

    $('#btnAddNewIndustry').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var params = {
            PortalId: portalID,
            IndustryId: 0,
            IndustryTitle: $('#industryTitleTextBox').val(),
            IsDeleted: false,
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format(),
            CreatedByUser: userID,
            CreatedOnDate: moment().format()
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/industries/updateIndustry',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Ramo adicionado.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                $('#newIndustry input').val(null);
                $('#industriesGrid').data('kendoGrid').dataSource.read();
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
            $('#newIndustry').data("kendoWindow").close();
        });
    });

    $('#industriesGrid').kendoGrid({
        dataSource: new kendo.data.DataSource({
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/industries/getIndustries?portalId=' + portalID
                }
            },
            sort: {
                field: "IndustryTitle",
                dir: "asc"
            },
            schema: {
                model: {
                    id: 'IndustryId'
                }
            },
            pageSize: 10
        }),
        toolbar: kendo.template($("#tmplToolbar").html()),
        pageable: {
            input: true,
            numeric: false,
            messages: {
                display: "{0} - {1} de {2} Itens",
                empty: "Sem Registro.",
                page: "Página",
                of: "de {0}",
                itemsPerPage: "Itens por página",
                first: "Ir para primeira página",
                previous: "Ir para página anterior",
                next: "Ir para próxima página",
                last: "Ir para última página",
                refresh: "Recarregar"
            }
        },
        columns: [
            {
                title: 'Título',
                field: 'IndustryTitle'
            },
            {
                command: [
                    {
                        name: "Update",
                        text: '',
                        imageClass: "fa fa-check",
                        click: function (e) {
                            e.preventDefault();

                            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                            if (dataItem) {

                                var params = {
                                    PortalId: portalID,
                                    IndustryId: dataItem.IndustryId,
                                    IndustryTitle: dataItem.IndustryTitle,
                                    IsDeleted: dataItem.IsDeleted,
                                    ModifiedByUser: userID,
                                    ModifiedOnDate: moment().format(),
                                    CreatedByUser: userID,
                                    CreatedOnDate: moment().format()
                                };

                                $.ajax({
                                    type: 'POST',
                                    url: '/desktopmodules/riw/api/industries/updateIndustry',
                                    data: params
                                }).done(function (data) {
                                    if (data.Result.indexOf("success") !== -1) {
                                        $.pnotify({
                                            title: 'Sucesso!',
                                            text: 'Ramo atualizado.',
                                            type: 'success',
                                            icon: 'fa fa-check fa-lg',
                                            addclass: "stack-bottomright",
                                            stack: my.stack_bottomright
                                        });
                                        $('#industriesGrid').data('kendoGrid').refresh();
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
                            }
                        }
                    }
                ],
                title: 'Atualizar',
                width: 65,
                attributes: { class: 'text-center' }
            },
            {
                command: [
                    {
                        name: "Delete",
                        text: '',
                        imageClass: "icon icon-ban-circle",
                        click: function (e) {
                            e.preventDefault();

                            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                            if (dataItem) {

                                var params = {
                                    PortalId: portalID,
                                    IndustryId: dataItem.IndustryId,
                                    IndustryTitle: dataItem.IndustryTitle,
                                    IsDeleted: true,
                                    ModifiedByUser: userID,
                                    ModifiedOnDate: moment().format(),
                                    CreatedByUser: userID,
                                    CreatedOnDate: moment().format()
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
                                                click: function () {

                                                    $dialog.dialog('close');
                                                    $dialog.dialog('destroy');

                                                    $.ajax({
                                                        type: 'POST',
                                                        url: '/desktopmodules/riw/api/industries/updateIndustry',
                                                        data: params
                                                    }).done(function (data) {
                                                        if (data.Result.indexOf("success") !== -1) {
                                                            //$().toastmessage('showSuccessToast', 'Item excluido com sucesso!');
                                                            $.pnotify({
                                                                title: 'Sucesso!',
                                                                text: 'Ramo desativado.',
                                                                type: 'success',
                                                                icon: 'fa fa-check fa-lg',
                                                                addclass: "stack-bottomright",
                                                                stack: my.stack_bottomright
                                                            });
                                                            $('#industriesGrid').data('kendoGrid').dataSource.read();
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
                            }
                        }
                    }
                ],
                title: 'Desativar',
                width: 70,
                attributes: { class: 'text-center' }
            },
            {
                command: [
                    {
                        name: "Restore",
                        text: '',
                        imageClass: "icon icon-refresh",
                        click: function (e) {
                            e.preventDefault();

                            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                            if (dataItem) {

                                var params = {
                                    PortalId: portalID,
                                    IndustryId: dataItem.IndustryId,
                                    IndustryTitle: dataItem.IndustryTitle,
                                    IsDeleted: false,
                                    ModifiedByUser: userID,
                                    ModifiedOnDate: moment().format(),
                                    CreatedByUser: userID,
                                    CreatedOnDate: moment().format()
                                };

                                $.ajax({
                                    type: 'POST',
                                    url: '/desktopmodules/riw/api/industries/updateIndustry',
                                    data: params
                                }).done(function (data) {
                                    if (data.Result.indexOf("success") !== -1) {
                                        $.pnotify({
                                            title: 'Sucesso!',
                                            text: 'Ramo restaurado.',
                                            type: 'success',
                                            icon: 'fa fa-check fa-lg',
                                            addclass: "stack-bottomright",
                                            stack: my.stack_bottomright
                                        });
                                        $('#industriesGrid').data('kendoGrid').dataSource.read();
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
                            }
                        }
                    }
                ],
                title: 'Restaurar',
                width: 70,
                attributes: { class: 'text-center' }
            },
            {
                command: [
                    {
                        name: "Exclude",
                        text: '',
                        imageClass: "fa fa-times",
                        click: function (e) {
                            e.preventDefault();

                            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                            if (dataItem) {

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

                                                    $dialog.dialog('close');
                                                    $dialog.dialog('destroy');

                                                    $.ajax({
                                                        type: 'DELETE',
                                                        url: '/desktopmodules/riw/api/industries/removeIndustry?industryId=' + dataItem.IndustryId + '&portalId=' + dataItem.PortalId
                                                    }).done(function (data) {
                                                        if (data.Result.indexOf("success") !== -1) {
                                                            //$().toastmessage('showSuccessToast', 'Item excluido com sucesso!');
                                                            $.pnotify({
                                                                title: 'Sucesso!',
                                                                text: 'Ramo excluido.',
                                                                type: 'success',
                                                                icon: 'fa fa-check fa-lg',
                                                                addclass: "stack-bottomright",
                                                                stack: my.stack_bottomright
                                                            });
                                                            $('#industriesGrid').data('kendoGrid').dataSource.remove(dataItem);
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
                            }
                        }
                    }
                ],
                title: 'Excluir',
                width: 65,
                attributes: { class: 'text-center' }
            }
        ],
        editable: true,
        sortable: true,
        //columnMenu: {
        //    messages: {
        //        columns: "Escolher colunas",
        //        filter: "Aplicar filtro",
        //        sortAscending: "Ordenar (a-z)",
        //        sortDescending: "Ordenar (z-a)"
        //    }
        //},
        dataBound: function (e) {
            var grid = this;
            if (grid.dataSource.data().length) {
                var gridData = grid.dataSource.view();

                for (var i = 0; i < gridData.length; i++) {
                    //get the item uid
                    var currentUid = gridData[i].uid;
                    var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
                    //if the record fits the custom condition
                    if (gridData[i].IsDeleted) {
                        currenRow.addClass('isDeleted');
                        $('tr:nth-child(' + (i + 1) + ') td:nth-child(2)').find('a').hide();
                        $('tr:nth-child(' + (i + 1) + ') td:nth-child(3)').find('a').hide();
                        $('tr:nth-child(' + (i + 1) + ') td:nth-child(5)').find('a').hide();
                    } else {
                        $('tr:nth-child(' + (i + 1) + ') td:nth-child(4)').find('a').hide();
                    }
                }
            }
        }
    });

    my.groupsTransport = {
        read: {
            url: '/desktopmodules/riw/api/store/getRolesByRoleGroup'
        },
        update: {
            url: '/desktopmodules/riw/api/store/updateRole',
            type: 'POST',
            complete: function (jqXhr, textStatus) {
                var obj = JSON.parse(jqXhr.responseText);
                if (!document.getElementById('popupNotification')) {
                    parent.$('#groupsGrid').append('<span id="popupNotification"></span>');
                }
                if (obj.Result.indexOf("success") !== -1) {
                    var grid = $("#groupsGrid").data("kendoGrid");
                    grid.dataSource.read();
                    $("#popupNotification").kendoNotification().data("kendoNotification").show(' Grupo atualizado com sucesso! ', 'success');
                } else {
                    $("#popupNotification").kendoNotification({
                        autoHideAfter: 6000
                    }).data("kendoNotification").show(' Erro ao tentar atualizar o grupo! <br /> Favor entrar em contato com o administrador do sistema. ', 'error');
                    console.log(jqXhr.responseText);
                }
            }
        },
        create: {
            url: '/desktopmodules/riw/api/store/updateRole',
            type: 'POST',
            complete: function (jqXhr, textStatus) {
                var obj = JSON.parse(jqXhr.responseText);
                if (!document.getElementById('popupNotification')) {
                    parent.$('#groupsGrid').append('<span id="popupNotification"></span>');
                }
                if (obj.Result.indexOf("success") !== -1) {
                    var grid = $("#groupsGrid").data("kendoGrid");
                    grid.dataSource.page(grid.dataSource.totalPages());
                    $("#popupNotification").kendoNotification().data("kendoNotification").show(' Grupo inserido com sucesso! ', 'success');
                } else {
                    $("#popupNotification").kendoNotification({
                        autoHideAfter: 6000
                    }).data("kendoNotification").show(' Erro ao tentar inserir o novo grupo! <br /> Favor entrar em contato com o administrador do sistema. ', 'error');
                    console.log(jqXhr.responseText);
                }
            }
        },
        destroy: {
            url: '/desktopmodules/riw/api/store/removeRole',
            type: 'DELETE',
            complete: function (jqXhr, textStatus) {
                var obj = JSON.parse(jqXhr.responseText);
                if (!document.getElementById('popupNotification')) {
                    parent.$('#groupsGrid').append('<span id="popupNotification"></span>');
                }
                if (obj.Result.indexOf("success") !== -1) {
                    $("#popupNotification").kendoNotification().data("kendoNotification").show(' Grupo excluido com sucesso! ', 'success');
                } else {
                    $("#popupNotification").kendoNotification({
                        autoHideAfter: 6000
                    }).data("kendoNotification").show(' Erro ao tentar excluir o grupo! <br /> Favor entrar em contato com o administrador do sistema. ', 'error');
                    console.log(jqXhr.responseText);
                }
            }
        },
        parameterMap: function (options, operation) {
            switch (operation) {
                case 'create':
                    return {
                        RoleGroup: 'Entidades',
                        RoleName: options.RoleName,
                        Description: options.Description
                    };
                case 'update':
                    return {
                        RoleId: options.RoleId,
                        RoleGroup: 'Entidades',
                        RoleName: options.RoleName,
                        Description: options.Description
                    };
                case 'destroy':
                    return {
                        RoleId: options.RoleId,
                        RoleGroupId: options.RoleGroupId
                    };
                default:
                    return {
                        portalId: portalID,
                        roleGroupName: 'Entidades'
                    };
            }

        }
    };

    my.groupsData = new kendo.data.DataSource({
        transport: my.groupsTransport,
        pageSize: 10,
        serverPaging: true,
        serverSorting: true,
        serverFiltering: true,
        sort: { field: "RoleName", dir: "ASC" },
        schema: {
            model: {
                id: 'RoleId',
                fields: {
                    RoleId: { editable: false, nullable: true },
                    RoleName: { validation: { required: true } },
                    Description: { validation: { required: true } }
                }
            }
        }
    });

    $('#groupsGrid').kendoGrid({
        dataSource: my.groupsData,
        toolbar: kendo.template($("#tmplGroupsToolbar").html()),
        columns: [
            //{
            //    title: 'ID',
            //    field: 'RoleId',
            //    width: 60,
            //    hidden: true
            //},
            {
                title: 'Nome',
                field: 'RoleName',
                width: '25%'
            },
            {
                title: 'Descrição',
                field: 'Description',
                width: '37%'
            },
            {
                command: [
                    {
                        name: "edit",
                        text: {
                            edit: "Editar",
                            update: "Salvar",
                            cancel: "Cancelar"
                        }
                    },
                    {
                        name: "Delete",
                        text: '&nbsp; Excluir',
                        imageClass: "fa fa-times",
                        click: function (e) {
                            e.preventDefault();

                            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

                            var params = {
                                roleId: dataItem.RoleId,
                                roleGroup: 'Entidades'
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
                                            click: function () {

                                                $.ajax({
                                                    type: 'DELETE',
                                                    url: '/desktopmodules/riw/api/store/removeRole',
                                                    data: params
                                                }).done(function (jqXhr) {
                                                    if (jqXhr.Result.indexOf("success") !== -1) {
                                                        $.pnotify({
                                                            title: 'Sucesso!',
                                                            text: 'Grupo excluido com sucesso!',
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
                                                }).fail(function (jqXhr, textStatus) {
                                                    console.log(jqXhr.responseText);
                                                });

                                                $dialog.dialog('close');
                                                $dialog.dialog('destroy');
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
                        }
                    }
                ],
                title: "&nbsp;",
                width: '38%'
            }
        ],
        selectable: "row",
        sortable: true,
        editable: {
            mode: 'inline'
        }
    });

    $('#companyShow').change(function () {
            
        var params = [
            {
                'PortalId': portalID,
                'SettingName': 'askCompany',
                'SettingValue': $(this).is(':checked')
            }
        ]

        kendo.ui.progress($("#checkboxes"), true);

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#lastNameShow').change(function () {

        var params = [
            {
                'PortalId': portalID,
                'SettingName': 'askLastName',
                'SettingValue': $(this).is(':checked')
            }
        ]

        kendo.ui.progress($("#checkboxes"), true);

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#industryShow').change(function () {

        var params = [
            {
                'PortalId': portalID,
                'SettingName': 'askIndustry',
                'SettingValue': $(this).is(':checked')
            }
        ]

        kendo.ui.progress($("#checkboxes"), true);

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#phoneShow').change(function () {

        var params = [
            {
                'PortalId': portalID,
                'SettingName': 'askTelephone',
                'SettingValue': $(this).is(':checked')
            }
        ]

        kendo.ui.progress($("#checkboxes"), true);

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#einShow').change(function () {

        var params = [
            {
                'PortalId': portalID,
                'SettingName': 'askEIN',
                'SettingValue': $(this).is(':checked')
            }
        ]

        kendo.ui.progress($("#checkboxes"), true);

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#stateTaxShow').change(function () {

        var params = [
            {
                'PortalId': portalID,
                'SettingName': 'askST',
                'SettingValue': $(this).is(':checked')
            }
        ]

        kendo.ui.progress($("#checkboxes"), true);

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#cityTaxShow').change(function () {

        var params = [
            {
                'PortalId': portalID,
                'SettingName': 'askCT',
                'SettingValue': $(this).is(':checked')
            }
        ]

        kendo.ui.progress($("#checkboxes"), true);

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#ssnShow').change(function () {

        var params = [
            {
                'PortalId': portalID,
                'SettingName': 'askSSN',
                'SettingValue': $(this).is(':checked')
            }
        ]

        kendo.ui.progress($("#checkboxes"), true);

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#identShow').change(function () {

        var params = [
            {
                'PortalId': portalID,
                'SettingName': 'askIdent',
                'SettingValue': $(this).is(':checked')
            }
        ]

        kendo.ui.progress($("#checkboxes"), true);

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#websiteShow').change(function () {

        var params = [
            {
                'PortalId': portalID,
                'SettingName': 'askWebsite',
                'SettingValue': $(this).is(':checked')
            }
        ]

        kendo.ui.progress($("#checkboxes"), true);

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#addressShow').change(function () {

        var params = [
            {
                'PortalId': portalID,
                'SettingName': 'askAddress',
                'SettingValue': $(this).is(':checked')
            }
        ]

        kendo.ui.progress($("#checkboxes"), true);

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#companyReq').change(function () {

        kendo.ui.progress($("#checkboxes"), true);

        var params = [];
        if ($(this).is(':checked')) {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqCompany',
                'SettingValue': true
            },
            {
                'PortalId': portalID,
                'SettingName': 'askCompany',
                'SettingValue': true
            });
            $('#companyShow').attr({ 'checked': true });
        } else {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqCompany',
                'SettingValue': false
            },
            {
                'PortalId': portalID,
                'SettingName': 'askCompany',
                'SettingValue': false
            });
            $('#companyShow').attr({ 'checked': false });
        }

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#lastNameReq').change(function () {

        kendo.ui.progress($("#checkboxes"), true);

        var params = [];
        if ($(this).is(':checked')) {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqLastName',
                'SettingValue': true
            },
            {
                'PortalId': portalID,
                'SettingName': 'askLastName',
                'SettingValue': true
            });
            $('#lastNameShow').attr({ 'checked': true });
        } else {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqLastName',
                'SettingValue': false
            },
            {
                'PortalId': portalID,
                'SettingName': 'askLastName',
                'SettingValue': false
            });
            $('#lastNameShow').attr({ 'checked': false });
        }

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#industryReq').change(function () {

        kendo.ui.progress($("#checkboxes"), true);

        var params = [];
        if ($(this).is(':checked')) {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqIndustry',
                'SettingValue': true
            },
            {
                'PortalId': portalID,
                'SettingName': 'askIndustry',
                'SettingValue': true
            });
            $('#industryShow').attr({ 'checked': true });
        } else {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqIndustry',
                'SettingValue': false
            },
            {
                'PortalId': portalID,
                'SettingName': 'askIndustry',
                'SettingValue': false
            });
            $('#industryShow').attr({ 'checked': false });
        }

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#phoneReq').change(function () {

        kendo.ui.progress($("#checkboxes"), true);

        var params = [];
        if ($(this).is(':checked')) {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqTelephone',
                'SettingValue': true
            },
            {
                'PortalId': portalID,
                'SettingName': 'askTelephone',
                'SettingValue': true
            });
            $('#phoneShow').attr({ 'checked': true });
        } else {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqTelephone',
                'SettingValue': false
            },
            {
                'PortalId': portalID,
                'SettingName': 'askTelephone',
                'SettingValue': false
            });
            $('#phoneShow').attr({ 'checked': false });
        }

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#einReq').change(function () {

        kendo.ui.progress($("#checkboxes"), true);

        var params = [];
        if ($(this).is(':checked')) {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqEIN',
                'SettingValue': true
            },
            {
                'PortalId': portalID,
                'SettingName': 'askEIN',
                'SettingValue': true
            });
            $('#einShow').attr({ 'checked': true });
        } else {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqEIN',
                'SettingValue': false
            },
            {
                'PortalId': portalID,
                'SettingName': 'askEIN',
                'SettingValue': false
            });
            $('#einShow').attr({ 'checked': false });
        }

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#stateTaxReq').change(function () {

        kendo.ui.progress($("#checkboxes"), true);

        var params = [];
        if ($(this).is(':checked')) {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqST',
                'SettingValue': true
            },
            {
                'PortalId': portalID,
                'SettingName': 'askST',
                'SettingValue': true
            });
            $('#stateTaxShow').attr({ 'checked': true });
        } else {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqST',
                'SettingValue': false
            },
            {
                'PortalId': portalID,
                'SettingName': 'askST',
                'SettingValue': false
            });
            $('#stateTaxShow').attr({ 'checked': false });
        }

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#cityTaxReq').change(function () {

        kendo.ui.progress($("#checkboxes"), true);

        var params = [];
        if ($(this).is(':checked')) {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqCT',
                'SettingValue': true
            },
            {
                'PortalId': portalID,
                'SettingName': 'askCT',
                'SettingValue': true
            });
            $('#cityTaxShow').attr({ 'checked': true });
        } else {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqCT',
                'SettingValue': false
            },
            {
                'PortalId': portalID,
                'SettingName': 'askCT',
                'SettingValue': false
            });
            $('#cityTaxShow').attr({ 'checked': false });
        }

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#ssnReq').change(function () {

        kendo.ui.progress($("#checkboxes"), true);

        var params = [];
        if ($(this).is(':checked')) {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqSSN',
                'SettingValue': true
            },
            {
                'PortalId': portalID,
                'SettingName': 'askSSN',
                'SettingValue': true
            });
            $('#ssnShow').attr({ 'checked': true });
        } else {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqSSN',
                'SettingValue': false
            },
            {
                'PortalId': portalID,
                'SettingName': 'askSSN',
                'SettingValue': false
            });
            $('#ssnShow').attr({ 'checked': false });
        }

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#identReq').change(function () {

        kendo.ui.progress($("#checkboxes"), true);

        var params = [];
        if ($(this).is(':checked')) {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqIdent',
                'SettingValue': true
            },
            {
                'PortalId': portalID,
                'SettingName': 'askIdent',
                'SettingValue': true
            });
            $('#identShow').attr({ 'checked': true });
        } else {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqIdent',
                'SettingValue': false
            },
            {
                'PortalId': portalID,
                'SettingName': 'askIdent',
                'SettingValue': false
            });
            $('#identShow').attr({ 'checked': false });
        }

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#websiteReq').change(function () {

        kendo.ui.progress($("#checkboxes"), true);

        var params = [];
        if ($(this).is(':checked')) {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqWebsite',
                'SettingValue': true
            },
            {
                'PortalId': portalID,
                'SettingName': 'askWebsite',
                'SettingValue': true
            });
            $('#websiteShow').attr({ 'checked': true });
        } else {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqWebsite',
                'SettingValue': false
            },
            {
                'PortalId': portalID,
                'SettingName': 'askWebsite',
                'SettingValue': false
            });
            $('#websiteShow').attr({ 'checked': false });
        }

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#addressReq').change(function () {

        kendo.ui.progress($("#checkboxes"), true);

        var params = [];
        if ($(this).is(':checked')) {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqAddress',
                'SettingValue': true
            },
            {
                'PortalId': portalID,
                'SettingName': 'askAddress',
                'SettingValue': true
            });
            $('#addressShow').attr({ 'checked': true });
        } else {
            params.push({
                'PortalId': portalID,
                'SettingName': 'reqAddress',
                'SettingValue': false
            },
            {
                'PortalId': portalID,
                'SettingName': 'askAddress',
                'SettingValue': false
            });
            $('#addressShow').attr({ 'checked': false });
        }

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configuração atualizada.',
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
            kendo.ui.progress($("#checkboxes"), false);
        });
    });

    $('#btnReturn').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#newIndustry').data("kendoWindow").close();
    });

});
