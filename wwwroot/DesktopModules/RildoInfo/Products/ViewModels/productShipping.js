
$(function () {

    my.personId = my.getParameterByName('itemId');

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

    $.ajax({
        url: '/desktopmodules/riw/api/products/getProduct?productId=' + my.personId + '&lang=pt-BR'
    }).done(function (data) {
        if (data) {
            $('#weightTextBox').data('kendoNumericTextBox').value(data.Weight);
            $('#lengthTextBox').data('kendoNumericTextBox').value(data.Length);
            $('#heightTextBox').data('kendoNumericTextBox').value(data.Height);
            $('#widthTextBox').data('kendoNumericTextBox').value(data.Width);
            $('#diameterTextBox').data('kendoNumericTextBox').value(data.Diameter);
            $('#zipOriginTextBox').val(data.ZipOrigin);
            $('#cityOriginTextBox').val(data.CityOrigin);
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
    });

    $("#zipOriginTextBox").inputmask("99-999-999");

    if (authorized < 3) {
        $('#liFinan').hide();
    }

    $('#btnUpdateShipping').click(function (e) {
        e.preventDefault();

        $(this).html('<i class="fa fa-spinner fa-spin"></i> Um momento...').attr({ 'disabled': true });

        var params = {
            ProductId: my.personId,
            Lang: 'pt-BR',
            Weight: $('#weightTextBox').data('kendoNumericTextBox').value(),
            Length: $('#lengthTextBox').data('kendoNumericTextBox').value(),
            Height: $('#heightTextBox').data('kendoNumericTextBox').value(),
            Width: $('#widthTextBox').data('kendoNumericTextBox').value(),
            Diameter: $('#diameterTextBox').data('kendoNumericTextBox').value(),
            ZipOrigin: $('#zipOriginTextBox').val().replace(/\D/g, '').trim(),
            CityOrigin: $('#cityOriginTextBox').val().trim(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };

        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/products/updateProductShipping',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                //$().toastmessage('showSuccessToast', 'Informa&#231;&#227;o atualizada com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Informa&#231;&#227;o atualizada.',
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
                //$().toastmessage('showErrorToast', data.Result);
            }

            $('#btnUpdateShipping').html('<i class="icon-ok icon-white"></i> Atualizar').attr({ 'disabled': false });
        }).fail(function (jqXHR, textStatus) {
            $('#btnUpdateShipping').html('<i class="icon-ok icon-white"></i> Atualizar').attr({ 'disabled': false });
            console.log(jqXHR.responseText);
        });
    });

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
                case 'menu_5':
                    e.preventDefault();
                    document.location.href = productVideosURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_5':
                    e.preventDefault();
                    document.location.href = relatedProductsURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_6':
                    e.preventDefault();
                    document.location.href = relatedProductsURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_7':
                    e.preventDefault();
                    document.location.href = productAttributesURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
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
        e.preventDefault();

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