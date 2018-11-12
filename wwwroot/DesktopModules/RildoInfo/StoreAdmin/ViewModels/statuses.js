
$(function () {

    $('#moduleTitleSkinObject').html(moduleTitle);

    var buttons = '<ul class="inline">';
    buttons += '<li><a href="' + addressURL + '" class="btn btn-primary btn-medium" title="Endereço"><i class="fa fa-book fa-lg"></i></a></li>';
    buttons += '<li><a href="' + registrationURL + '" class="btn btn-primary btn-medium" title="Cadastro"><i class="fa fa-edit fa-lg"></i></a></li>';
    buttons += '<li><a href="' + payCondsURL + '" class="btn btn-primary btn-medium" title="Formas e Condições de Pagamento"><i class="fa fa-credit-card fa-lg"></i></a></li>';
    buttons += '<li><a href="' + syncURL + '" class="btn btn-primary btn-medium" title="Orçamento"><i class="fa fa-refresh fa-lg"></i></a></li>';
    buttons += '<li><a href="' + estimateURL + '" class="btn btn-primary btn-medium" title="Orçamento"><i class="fa fa-usd fa-lg"></i></a></li>';
    buttons += '<li><a href="' + smtpURL + '" class="btn btn-primary btn-medium" title="SMTP"><i class="fa fa-envelope-o fa-lg"></i></a></li>';
    buttons += '<li><a href="' + websiteManagerURL + '" class="btn btn-primary btn-medium" title="Website"><i class="fa fa-bookmark fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + templatesManagerURL + '" class="btn btn-primary btn-medium" title="Templates"><i class="fa fa-puzzle-piece fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + davReturnsURL + '" class="btn btn-primary btn-medium" title="DAVs"><i class="fa fa-briefcase fa-lg"></i></a></li>';
    buttons += '</ul>';
    $('#buttons').html(buttons);

    my.addNewStatus = function (e) {

        kendoWindow = $('#newStatus').kendoWindow({
            title: 'Novo Status',
            modal: true,
            width: '90%',
            height: '80%',
            close: function (e) {
                $("html, body").css("overflow", "");
            },
            open: function () {
                $("html, body").css("overflow", "hidden");
            },
            deactivate: function () {
                this.destroy();
            }
        });

        kendoWindow.data("kendoWindow").center().open();
    };

    $('#btnAddNewStatus').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var params = {
            PortalId: portalID,
            IsDeleted: false,
            IsReadOnly: false,
            StatusColor: $('#statusColorPallete').data('kendoColorPicker').value(),
            StatusTitle: $('#statusTitleTextBox').val(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format(),
            CreatedByUser: userID,
            CreatedOnDate: moment().format()
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/statuses/updateStatus',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Status adicionado.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                $('#newStatus input').val(null);
                $('#statusesGrid').data('kendoGrid').dataSource.read();
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
            $('#newStatus').data("kendoWindow").close();
        });
    });

    $('#statusesGrid').kendoGrid({
        dataSource: new kendo.data.DataSource({
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/statuses/getStatuses?portalId=' + portalID
                }
            },
            sort: {
                field: "StatusTitle",
                dir: "asc"
            },
            schema: {
                model: {
                    id: 'StatusId'
                }
            },
            pageSize: 10
        }),
        toolbar: kendo.template($("#tmplToolbar").html()),
        columns: [
            {
                title: 'Título',
                field: 'StatusTitle'
            },
            {
                title: 'Cor',
                field: 'StatusColor',
                template: '#=colorize(StatusColor)#',
                width: 160,
                editor: colorEditor
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
                                    StatusId: dataItem.StatusId,
                                    IsDeleted: dataItem.IsDeleted,
                                    IsReadOnly: dataItem.IsReadOnly,
                                    StatusColor: dataItem.StatusColor,
                                    StatusTitle: dataItem.StatusTitle,
                                    ModifiedByUser: userID,
                                    ModifiedOnDate: moment().format(),
                                    CreatedByUser: userID,
                                    CreatedOnDate: moment().format()
                                };

                                $.ajax({
                                    type: 'POST',
                                    url: '/desktopmodules/riw/api/statuses/updateStatus',
                                    data: params
                                }).done(function (data) {
                                    if (data.Result.indexOf("success") !== -1) {
                                        $.pnotify({
                                            title: 'Sucesso!',
                                            text: 'Status atualizado.',
                                            type: 'success',
                                            icon: 'fa fa-check fa-lg',
                                            addclass: "stack-bottomright",
                                            stack: my.stack_bottomright
                                        });
                                        $('#statusesGrid').data('kendoGrid').refresh();
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
                                    StatusId: dataItem.StatusId,
                                    IsDeleted: true,
                                    IsReadOnly: dataItem.IsReadOnly,
                                    StatusColor: dataItem.StatusColor,
                                    StatusTitle: dataItem.StatusTitle,
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
                                                        url: '/desktopmodules/riw/api/statuses/updateStatus',
                                                        data: params
                                                    }).done(function (data) {
                                                        if (data.Result.indexOf("success") !== -1) {
                                                            //$().toastmessage('showSuccessToast', 'Item excluido com sucesso!');
                                                            $.pnotify({
                                                                title: 'Sucesso!',
                                                                text: 'Status desativado.',
                                                                type: 'success',
                                                                icon: 'fa fa-check fa-lg',
                                                                addclass: "stack-bottomright",
                                                                stack: my.stack_bottomright
                                                            });
                                                            $('#statusesGrid').data('kendoGrid').dataSource.read();
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
                                    StatusId: dataItem.StatusId,
                                    IsDeleted: false,
                                    IsReadOnly: dataItem.IsReadOnly,
                                    StatusColor: dataItem.StatusColor,
                                    StatusTitle: dataItem.StatusTitle,
                                    ModifiedByUser: userID,
                                    ModifiedOnDate: moment().format(),
                                    CreatedByUser: userID,
                                    CreatedOnDate: moment().format()
                                };

                                $.ajax({
                                    type: 'POST',
                                    url: '/desktopmodules/riw/api/statuses/updateStatus',
                                    data: params
                                }).done(function (data) {
                                    if (data.Result.indexOf("success") !== -1) {
                                        $.pnotify({
                                            title: 'Sucesso!',
                                            text: 'Status restaurado.',
                                            type: 'success',
                                            icon: 'fa fa-check fa-lg',
                                            addclass: "stack-bottomright",
                                            stack: my.stack_bottomright
                                        });
                                        $('#statusesGrid').data('kendoGrid').dataSource.read();
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
                                                        url: '/desktopmodules/riw/api/statuses/removeStatus?statusId=' + dataItem.StatusId
                                                    }).done(function (data) {
                                                        if (data.Result.indexOf("success") !== -1) {
                                                            //$().toastmessage('showSuccessToast', 'Item excluido com sucesso!');
                                                            $.pnotify({
                                                                title: 'Sucesso!',
                                                                text: 'Status excluido.',
                                                                type: 'success',
                                                                icon: 'fa fa-check fa-lg',
                                                                addclass: "stack-bottomright",
                                                                stack: my.stack_bottomright
                                                            });
                                                            $('#statusesGrid').data('kendoGrid').dataSource.remove(dataItem);
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
                    if (gridData[i].IsReadOnly) {
                        //$('tr:nth-child(' + (i + 1) + ') td:nth-child(3)').find('a').hide();
                        $('tr:nth-child(' + (i + 1) + ') td:nth-child(6)').find('a').hide();
                        if (gridData[i].IsDeleted) {
                            currenRow.addClass('isDeleted');
                            $('tr:nth-child(' + (i + 1) + ') td:nth-child(3)').find('a').hide();
                            $('tr:nth-child(' + (i + 1) + ') td:nth-child(4)').find('a').hide();
                            $('tr:nth-child(' + (i + 1) + ') td:nth-child(6)').find('a').hide();
                        } else {
                            $('tr:nth-child(' + (i + 1) + ') td:nth-child(5)').find('a').hide();
                        }
                    } else {
                        if (gridData[i].IsDeleted) {
                            currenRow.addClass('isDeleted');
                            $('tr:nth-child(' + (i + 1) + ') td:nth-child(3)').find('a').hide();
                            $('tr:nth-child(' + (i + 1) + ') td:nth-child(4)').find('a').hide();
                            $('tr:nth-child(' + (i + 1) + ') td:nth-child(6)').find('a').hide();
                        } else {
                            $('tr:nth-child(' + (i + 1) + ') td:nth-child(5)').find('a').hide();
                        }
                    }
                }
            }
        }
    });

    $('#statusColorPallete').kendoColorPicker({
        palette: [
            "#FFFFFF", "#C5F7EB", "#C5CEF7", "#EEC5F7", "#F7D1C5", "#FFFF00", "#96C8AD", "#DDD2F2", "#D8D6A1", "#FFA633", "#23D3D1", "#BFFF1E", "#DBC8C2"
        ]
    });

    $('#btnReturn').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#newStatus').data("kendoWindow").close();
    });

});

function colorize(value) {
    return '<table><tr><td>' + value + '</td><td style="background: ' + value + '">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td></tr></table>';
}

function colorEditor(container, options) {
    $('<input class="input-small" data-bind="value:' + options.field + '" value="' + options.model.StatusColor + '" />')
        .appendTo(container).kendoColorPicker({
            palette: [
                "#FFFFFF", "#C5F7EB", "#C5CEF7", "#EEC5F7", "#F7D1C5", "#FFFF00", "#96C8AD", "#DDD2F2", "#D8D6A1", "#FFA633", "#CECBC2", "#BFFF1E", "#DBC8C2"
            ]
        });
}

function statusColorEditor(container, options) {
    $('<input type="text" class="color {pickerClosable:true}" data-bind="value: ' + options.field + '" value="' + options.model.StatusColor + '" />').appendTo(container);
    jscolor.init();
}
