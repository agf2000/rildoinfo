
$(function () {

    my.personId = my.getParameterByName('itemId');
    my.optionId = 0;

    // knockout js view model
    my.vm = function () {
        // this is knockout view model
        var self = this;

        // make view models available for apps
        return {
            
        };

    }();

    // apply ko bindings
    ko.applyBindings(my.vm);

    $('#productMenu').show();

    $('#editProductAttributes .icon-info-sign').popover({
        placement: 'right',
        trigger: 'hover'
    });

    $('#btnAddAttr').click(function (e) {
        e.preventDefault();

        $(this).html('<i class="fa fa-spinner fa-spin"></i> Um momento...').attr({ 'disabled': true });

        var params = {
            ProductId: my.personId,
            Lang: 'pt-BR',
            ListOrder: 1,
            OptionDesc: $('#attributeTextBox').val()
        }

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/products/addProductOption',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $('#attributeTextBox').val('');
                my.productOptionsData.read();
                //$().toastmessage('showSuccessToast', 'Atributo inserido com<br />sucesso!');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Atributo inserido.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                $('#attributeTextBox').focus();
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

            $('#btnAddAttr').html('Adicionar').attr({ 'disabled': false });
        }).fail(function (jqXHR, textStatus) {
            $('#btnAddAttr').html('Adicionar').attr({ 'disabled': false });
            console.log(jqXHR.responseText);
        });
    });

    $('#btnAddAttrOption').click(function (e) {
        e.preventDefault();

        $(this).html('<i class="fa fa-spinner fa-spin"></i> Um momento...').attr({ 'disabled': true });

        var params = {
            OptionId: my.optionId,
            Lang: 'pt-BR',
            ListOrder: 1,
            OptionValueDesc: $('#attributeOptionTextBox').val(),
            AddedCost: 0,
            QtyStockSet: 0
        }

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/products/addProductOptionValue',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $('#attributeOptionTextBox').val('');
                my.productOptionValuesData.read();
                //$().toastmessage('showSuccessToast', 'Op&#231;&#227;o inserida com sucesso!');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Op&#231;&#227;o inserida.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                $('#attributeOptionTextBox').focus();
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

            $('#btnAddAttrOption').html('Adicionar').attr({ 'disabled': false });
        }).fail(function (jqXHR, textStatus) {
            $('#btnAddAttrOption').html('Adicionar').attr({ 'disabled': false });
            console.log(jqXHR.responseText);
        });
    });
    
    // create kendo dataSource transport to get product options
    my.productOptionsTransport = {
        read: {
            url: '/desktopmodules/riw/api/products/getProductOptions'
        },
        parameterMap: function (data, type) {
            return {
                productId: my.personId,
                lang: 'pt-BR'
            };
        }
    };

    // create kendo dataSource for getting products transport
    my.productOptionsData = new kendo.data.DataSource({
        transport: my.productOptionsTransport,
        sort: {
            field: "ListOrder",
            dir: "ASC"
        },
        schema: {
            model: {
                id: 'OptionId',
                fields: {
                    OptionId: { editable: false, nullable: true },
                    OptionDesc: { type: 'string', validation: { required: { message: 'Campo obrgat&#243;rio' } } },
                    ListOrder: { type: "number", validation: { min: 0, required: true } }
                }
            }
        }
    });

    $('#attributesGrid').kendoGrid({
        dataSource: my.productOptionsData,
        columns: [
            {
                command: [
                    {
                        name: 'exclude',
                        text: '',
                        imageClass: 'k-icon k-i-close',
                        click: function (e) {
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
                                                    $.ajax({
                                                        type: 'POST',
                                                        url: '/desktopmodules/riw/api/products/removeProductOption?productOptionId=' + dataItem.OptionId + '&lang=' + dataItem.Lang
                                                    }).done(function (data) {
                                                        if (data.Result.indexOf("success") !== -1) {
                                                            my.optionId = 0
                                                            my.productOptionsData.read();
                                                            //$().toastmessage('showSuccessToast', 'Atributo excluido com sucesso!');
                                                            $.pnotify({
                                                                title: 'Sucesso!',
                                                                text: 'Atributo excluido.',
                                                                type: 'success',
                                                                icon: 'fa fa-check fa-lg',
                                                                addclass: "stack-bottomright",
                                                                stack: my.stack_bottomright
                                                            });
                                                            $dialog.dialog('close');
                                                            $dialog.dialog('destroy');
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
                title: "&nbsp;",
                width: 50
            },
            {
                field: 'OptionDesc', title: 'Atributo'
            },
            {
                field: 'ListOrder', title: 'Ordem', width: 60
            },
            {
                command: [
                    {
                        name: 'config',
                        text: '',
                        imageClass: 'k-icon k-i-custom',
                        click: function (e) {
                            e.preventDefault();

                            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                            if (dataItem) {
                                my.optionId = dataItem.OptionId;
                                $('#divAttrOptions').fadeIn();
                                $('#chosenAttribute').text(dataItem.OptionDesc);
                                my.productOptionValuesData.read();
                            }
                        }
                    }
                ],
                title: '&nbsp;',
                width: 50
            }
        ],
        editable: true,
        dataBound: function (e) {
            var grid = this;
            if (grid.dataSource.data().length) {
                $('#attributesGrid').show();
            }
        }
    });

    // create kendo dataSource transport to get product options
    my.productOptionValuesTransport = {
        read: {
            url: '/desktopmodules/riw/api/products/getProductOptionValues'
        },
        parameterMap: function (data, type) {
            return {
                optionId: my.optionId,
                lang: 'pt-BR'
            };
        }
    };

    // create kendo dataSource for getting products transport
    my.productOptionValuesData = new kendo.data.DataSource({
        transport: my.productOptionValuesTransport,
        sort: {
            field: "ListOrder",
            dir: "ASC"
        },
        schema: {
            model: {
                id: 'OptionValueId',
                fields: {
                    OptionValueId: { editable: false, nullable: true },
                    OptionValueDesc: { type: 'string', validation: { required: { message: 'Campo obrgat&#243;rio' } } },
                    ListOrder: { type: "number", validation: { min: 0, required: true } },
                    AddedCost: { type: "number", validation: { min: 0, required: true } },
                    QtyStockSet: { type: "number", validation: { min: 0, required: true } }
                }
            }
        }
    });

    $('#attributeOptionsGrid').kendoGrid({
        dataSource: my.productOptionValuesData,
        columns: [
            {
                command: [
                    {
                        name: 'exclude',
                        text: '',
                        imageClass: 'k-icon k-i-close',
                        click: function (e) {
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
                                                    $.ajax({
                                                        type: 'DELETE',
                                                        url: '/desktopmodules/riw/api/products/removeProductOptionValue?productOptionValueId=' + dataItem.OptionValueId + '&lang=' + dataItem.Lang
                                                    }).done(function (data) {
                                                        if (data.Result.indexOf("success") !== -1) {
                                                            my.productOptionValuesData.read();
                                                            //$().toastmessage('showSuccessToast', 'Op&#231;&#227;o excluida com sucesso!');
                                                            $.pnotify({
                                                                title: 'Sucesso!',
                                                                text: 'Op&#231;&#227;o excluida.',
                                                                type: 'success',
                                                                icon: 'fa fa-check fa-lg',
                                                                addclass: "stack-bottomright",
                                                                stack: my.stack_bottomright
                                                            });
                                                            $dialog.dialog('close');
                                                            $dialog.dialog('destroy');
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
                title: "&nbsp;",
                width: 50
            },
            {
                field: 'OptionValueDesc', title: 'Op&#231;&#227;o'
            },
            {
                field: 'AddedCost', title: 'C. Adicional', width: 80, format: '{0:n}', attributes: { class: 'text-right' }
            },
            {
                field: 'QtyStockSet', title: 'Estoque', width: 70
            }, 
            {
                field: 'ListOrder', title: 'Ordem', width: 60
            }
        ],
        editable: true,
        dataBound: function (e) {
            var grid = this;
            if (grid.dataSource.data().length) {
                $('#attributeOptionsGrid').show();
            }
        }
    });

    $('#btnUpdateAttributes').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var gridAttr = $("#attributesGrid").data("kendoGrid");
        var gridAttrValue = $("#attributeOptionsGrid").data("kendoGrid");
        //if (!grid.editable) {

            if (doesDataSourceHaveChanges(gridAttr.dataSource)) {
                
                $('#btnUpdateAttributes').html('<i class="fa fa-spinner fa-spin"></i> Um momento...').attr({ 'disabled': true });

                attributes();

            }
                        
            if (my.optionId) {
                if (doesDataSourceHaveChanges(gridAttrValue.dataSource)) {

                    if (!$('#btnUpdateAttributes').is('disabled')) {
                        $('#btnUpdateAttributes').html('<i class="fa fa-spinner fa-spin"></i> Um momento...').attr({ 'disabled': true });
                    }
                    attrValues();
                }
            }      
        //}
    });

    function attributes() {

        var params = [];

        $.each(my.productOptionsData.data(), function (index, item) {
            params.push({
                'OptionId': item.OptionId,
                'ProductId': my.personId,
                'Lang': 'pt-BR',
                'ListOrder': item.ListOrder,
                'OptionDesc': item.OptionDesc
            });
        });

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/products/updateProductOption',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $('#attributesGrid').data('kendoGrid').refresh();
                $('#btnUpdateAttributes').html('<i class="icon-ok icon-white"></i>&nbsp; Atualizar').attr({ 'disabled': false });
                //$().toastmessage('showSuccessToast', 'Atributo(s) atualizado(s) com<br />sucesso!');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Atributo(s) atualizado(s).',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
            } else {
                $('#btnUpdateAttributes').html('<i class="icon-ok icon-white"></i>&nbsp; Atualizar').attr({ 'disabled': false });
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

    function attrValues() {

        var optionValues = [];

        $.each(my.productOptionValuesData.data(), function (index, item) {
            optionValues.push({
                'OptionValueId': item.OptionValueId,
                'OptionId': my.optionId,
                'Lang': 'pt-BR',
                'ListOrder': item.ListOrder,
                'OptionValueDesc': item.OptionValueDesc,
                'AddedCost': item.AddedCost,
                'QtyStockSet': item.QtyStockSet
            });
        });

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/products/updateProductOptionValue',
            data: JSON.stringify(optionValues)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $('#attributeOptionsGrid').data('kendoGrid').refresh();
                $('#btnUpdateAttributes').html('<i class="icon-ok icon-white"></i>&nbsp; Atualizar').attr({ 'disabled': false });
                //if (!$('.toast-item-wrapper').is(":visible")) $().toastmessage('showSuccessToast', 'Atributo(s) atualizado(s) com<br />sucesso!');
                if (!$('.ui-pnotify').is(":visible")) {
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Atributo(s) atualizado(s).',
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
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
    }

    $('#productMenu').kendoMenu({
        select: function (e) {
            switch ($(e.item).attr('id')) {
                case 'menu_2':
                    e.preventDefault();
                    document.location.href = editItemURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_3':
                    e.preventDefault();
                    document.location.href = productImagesURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_4':
                    e.preventDefault();
                    document.location.href = productDescURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_5':
                    e.preventDefault();
                    document.location.href = productVideosURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_6':
                    e.preventDefault();
                    document.location.href = relatedProductsURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_8':
                    e.preventDefault();
                    document.location.href = productShippingURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_9':
                    e.preventDefault();
                    document.location.href = productSEOURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_11':
                    e.preventDefault();
                    document.location.href = productFinanceURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
            }
        }
    });

    $('.btnReturn').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        $(this).html('<i class="fa fa-spinner fa-spin"></i>Um momento...').attr({ 'disabled': true });
        var urlAddress = '';
        if (my.retSel > 0) {
            switch (my.retSel) {
                case 7:
                    urlAddress = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/7';
                    break;
                case 8:
                    urlAddress = historyURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/8';
                    break;
                case 9:
                    urlAddress = finanURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/9';
                    break;
                case 10:
                    urlAddress = assistURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/10';
                    break;
                case 11:
                    urlAddress = commURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/11';
                    break;
            }
            document.location.href = urlAddress;
        } else {
            if (parent.$('#window')) {
                parent.$('#window').data("kendoWindow").close();
            }
        }
    });

});
