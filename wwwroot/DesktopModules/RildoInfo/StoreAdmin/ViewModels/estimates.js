
$(function () {

    $.valHooks.textarea = {
        get: function (elem) {
            return elem.value.replace(/\r?\n/g, "\r\n");
        }
    };

    my.viewModel();

    var buttons = '<ul class="inline">';
    buttons += '<li><a href="' + addressURL + '" class="btn btn-primary btn-medium" title="Endereço"><i class="fa fa-book fa-lg"></i></a></li>';
    buttons += '<li><a href="' + registrationURL + '" class="btn btn-primary btn-medium" title="Cadastro"><i class="fa fa-edit fa-lg"></i></a></li>';
    buttons += '<li><a href="' + payCondsURL + '" class="btn btn-primary btn-medium" title="Formas e Condições de Pagamento"><i class="fa fa-credit-card fa-lg"></i></a></li>';
    buttons += '<li><a href="' + syncURL + '" class="btn btn-primary btn-medium" title="Orçamento"><i class="fa fa-refresh fa-lg"></i></a></li>';
    buttons += '<li><a href="' + smtpURL + '" class="btn btn-primary btn-medium" title="SMTP"><i class="fa fa-envelope-o fa-lg"></i></a></li>';
    buttons += '<li><a href="' + statusesManagerURL + '" class="btn btn-primary btn-medium" title="Status"><i class="fa fa-check-circle fa-lg"></i></a></li>';
    buttons += '<li><a href="' + websiteManagerURL + '" class="btn btn-primary btn-medium" title="Website"><i class="fa fa-bookmark fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + templatesManagerURL + '" class="btn btn-primary btn-medium" title="Templates"><i class="fa fa-puzzle-piece fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + davReturnsURL + '" class="btn btn-primary btn-medium" title="DAVs"><i class="fa fa-briefcase fa-lg"></i></a></li>';
    buttons += '</ul>';
    $('#buttons').html(buttons);

    $('#moduleTitleSkinObject').html(moduleTitle);

    $('.icon-info-sign').popover({
        placement: 'top',
        trigger: 'hover'
    });

    //my.vm.loadSaleReps();

    $('#viewPriceCheckBox').bootstrapSwitch();
    $('#noStockAllowedCheckBox').bootstrapSwitch();

    $('#estimateTermsTextArea').val(toMarkdown(estimateTerm));

    $('#kddlSalesRep').kendoDropDownList({
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/people/GetUsersByRoleName?portalId=' + portalID + '&roleName=Vendedores'
                }
            }
        },
        dataTextField: 'DisplayName', 
        dataValueField: 'MemberId', 
        value: salesPerson
    });

    $('#discountGroupsGrid').kendoGrid({
        dataSource: new kendo.data.DataSource({
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/store/getRolesByRoleGroup?portalId=' + portalID + '&roleGroupName=Descontos'
                }
            },
            sort: {
                field: "RoleName",
                dir: "asc"
            },
            schema: {
                model: {
                    id: 'RoleId'
                }
            },
            pageSize: 10
        }),
        toolbar: kendo.template($("#ulToolbar").html()),
        columns: [
            {
                title: 'Nome',
                field: 'RoleName'
            },
            {
                title: 'Descrição',
                field: 'Description'
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
                                    RoleId: dataItem.RoleId,
                                    RoleGroup: 'Descontos',
                                    RoleName: dataItem.RoleName,
                                    RoleDescription: dataItem.Description
                                };

                                $.ajax({
                                    type: 'POST',
                                    url: '/desktopmodules/riw/api/store/updateRole',
                                    data: params
                                }).done(function (data) {
                                    if (data.Result.indexOf("success") !== -1) {
                                        $.pnotify({
                                            title: 'Sucesso!',
                                            text: 'Grupo atualizado.',
                                            type: 'success',
                                            icon: 'fa fa-check fa-lg',
                                            addclass: "stack-bottomright",
                                            stack: my.stack_bottomright
                                        });
                                        $('#discountGroupsGrid').data('kendoGrid').refresh();
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
                                                        url: '/desktopmodules/riw/api/store/removeRole?roleId=' + dataItem.RoleId + '&roleGroup=' + dataItem.RoleGroupId
                                                    }).done(function (data) {
                                                        if (data.Result.indexOf("success") !== -1) {
                                                            //$().toastmessage('showSuccessToast', 'Item excluido com sucesso!');
                                                            $.pnotify({
                                                                title: 'Sucesso!',
                                                                text: 'Grupo excluido.',
                                                                type: 'success',
                                                                icon: 'fa fa-check fa-lg',
                                                                addclass: "stack-bottomright",
                                                                stack: my.stack_bottomright
                                                            });
                                                            $('#discountGroupsGrid').data('kendoGrid').dataSource.remove(dataItem);
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
        sortable: true
    });

    $("#ddlMainConsumer").select2({
        placeholder: "Busque por clientes...",
        //allowClear: true,
        minimumInputLength: 3,
        id: function (data) {
            return {
                PersonId: data.PersonId,
                DisplayName: data.DisplayName
            };
        },
        ajax: {
            url: "/desktopmodules/riw/api/people/getpeople",
            quietMillis: 100,
            data: function (term, page) { // page is the one-based page number tracked by Select2
                return {
                    portalId: portalID,
                    registerType: clientRoleId,
                    isDeleted: false,
                    searchField: 'FirstName',
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
        initSelection: function (item, callback) {
            var id = item.val();
            var text = item.data('option');
            var data = { PersonId: id, DisplayName: text };
            callback(data);
        }
    });

    $('#ddlMainConsumer').select2('data', { PersonId: mainConsumerId, DisplayName: mainConsumer });

    $('#kddlDefaultUnit').kendoDropDownList({
        dataTextField: "UnitTypeTitle",
        dataValueField: "UnitTypeId",
        template: '<span>(#= UnitTypeAbbv #)</span> <span> #= UnitTypeTitle #</span>',
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/unittypes/GetUnitTypes'
                }
            }
        },
        valueTemplate: '<span>(#= UnitTypeAbbv #)</span> <span> #= UnitTypeTitle #</span>',
        value: defaultUnit
    });

    $('#btnUpdateEstimate').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = [
            {
                'PortalId': portalID,
                'SettingName': 'salesPerson',
                'SettingValue': $('#kddlSalesRep').data('kendoDropDownList').value()
            },
            {
                'PortalId': portalID,
                'SettingName': 'mainConsumerId',
                'SettingValue': parseInt($('#ddlMainConsumer').select2('data').PersonId)
            },
            {
                'PortalId': portalID,
                'SettingName': 'mainConsumer',
                'SettingValue': $('#ddlMainConsumer').select2('data').DisplayName
            },
            {
                'PortalId': portalID,
                'SettingName': 'estimateMaxDiscount',
                'SettingValue': my.vm.estimateMaxDiscount()
            },
            {
                'PortalId': portalID,
                'SettingName': 'showEstimatePrice',
                'SettingValue': $('#viewPriceCheckBox').is(':checked')
            },
            {
                'PortalId': portalID,
                'SettingName': 'estimateMaxDuration',
                'SettingValue': my.vm.estimateMaxDuration()
            },
            {
                'PortalId': portalID,
                'SettingName': 'noStockAllowed',
                'SettingValue': $('#noStockAllowedCheckBox').is(':checked')
            },
            {
                'PortalId': portalID,
                'SettingName': 'estimateTerm',
                'SettingValue': $('#estimateTermsTextArea').val()
            },
            {
                'PortalId': portalID,
                'SettingName': 'defaultUnit',
                'SettingValue': $('#kddlDefaultUnit').data('kendoDropDownList').value()
            }
        ];

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/UpdateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Informações atualizadas.',
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

    my.addGroup = function (value) {
        my.vm.payCondType(value);

        kendoWindow = $('#newGroup').kendoWindow({
            title: 'Novo Grupo',
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

    $('#btnAddNewRole').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var params = {
            RoleGroup: 'Descontos',
            RoleName: $('#roleNameTextBox').val(),
            RoleDescription: $('#descriptionTextArea').val()
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/store/addRole',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Grupo inserido.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                $('#newGroup input').val(null);
                $('#discountGroupsGrid').data('kendoGrid').dataSource.read();
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
            $('#newGroup').data("kendoWindow").close();
        });
    });

    $('.markdown-editor').css({ 'min-width': '90%', 'height': '80px', 'margin-bottom': '5px' }).attr({ 'cols': '30', 'rows': '2' });

    $('.markdown-editor').autogrow();
    $('.markdown-editor').css('overflow', 'hidden').autogrow();

    $('.togglePreview').click(function (e) {
        e.preventDefault();
        var $this = $(this);

        var ele = $($this).data('provider');

        var $dialog = $('<div></div>')
            .html(my.converter.makeHtml($('#' + ele).val().trim()))
            .dialog({
                autoOpen: false,
                open: function () {
                    $(".ui-dialog-title").append('Termo');
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
    });

    $('#btnReturn').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#newGroup').data("kendoWindow").close();
    });

    $('#viewPriceCheckBox').bootstrapSwitch('setState', JSON.parse(showEstimatePrice.toLowerCase()));
    $('#noStockAllowedCheckBox').bootstrapSwitch('setState', JSON.parse(noStockAllowed.toLowerCase()));

});

function clientFormatResult(data) {
    return '<strong>Cliente: </strong><span>' + data.DisplayName + '</span><br /><strong>Email: </strong><span>' + data.Email + '</span><br /><strong>Telefone: </strong><span>' + data.Telephone + '</span>';
}

function clientFormatSelection(data) {
    return data.DisplayName;
}
