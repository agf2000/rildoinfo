
$(function () {

    my.personId = my.getParameterByName('itemId');

    // product video view model
    my.ProductVideo = function () {
        this.VideoId = ko.observable();
        this.Alt = ko.observable();
        this.Src = ko.observable();
    };

    // knockout js view model
    my.vm = function () {
        // this is knockout view model
        var self = this;

        self.prodVideos = ko.observableArray([]),

        // ajax function to get product videos
        self.loadProdVideos = function () {
            $.ajax({
                url: '/desktopmodules/riw/api/products/getProductVideos?productId=' + my.personId
            }).done(function (data) {
                if (data.length > 0) {
                    self.prodVideos.removeAll();
                    $.each(data, function (i, pv) {
                        self.prodVideos.push(new my.ProductVideo()
                            .Alt(pv.Alt)
                            .Src(pv.Src)
                            .VideoId(pv.VideoId));
                    });

                    $(".yt").each(function () {
                        var ifr_source = $(this).attr('src');
                        var wmode = "wmode=transparent";
                        if (ifr_source.indexOf('?') !== -1) {
                            var getQString = ifr_source.split('?');
                            var oldString = getQString[1];
                            var newString = getQString[0];
                            $(this).attr('src', newString + '?' + wmode + '&' + oldString + '&origin=http://ristore.dnndev.me');
                        }
                        else $(this).attr('src', ifr_source + '?' + wmode + '&origin=http://ristore.dnndev.me');

                        $(this).attr('width', '232');
                        $(this).attr('height', '180');
                    });

                    // initiate colobox jquery plugin
                    //$(".photo").colorbox();
                    $('.photo').click(function (event) {
                        event.preventDefault(); // this just cancels the default link behavior.
                        parent.showColorBox($(this).attr("href")); //this makes the parent window load the showColorBox function, using the a.colorbox href value
                    });
                }
            }).fail(function (jqXHR, textStatus) {
                console.log(jqXHR.responseText);
            });
        };

        // make view models available for apps
        return {
            prodVideos: prodVideos,
            loadProdVideos: loadProdVideos
        };

    }();

    // apply ko bindings
    ko.applyBindings(my.vm);

    $('#productMenu').show();

    my.vm.loadProdVideos();

    $('#btnSaveVideo').click(function (e) {
        e.preventDefault();

        $(this).html('<i class="fa fa-spinner fa-spin"></i> Um momento...').attr({ 'disabled': true });

        var params = {
            ProductId: my.personId,
            Alt: $('#videoTitleTextBox').val(),
            Src: $('#videoURLTextBox').val()
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/products/addProductVideo',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.vm.prodVideos.push(new my.ProductVideo()
                    .Alt($('#videoTitleTextBox').val())
                    .Src($('#videoURLTextBox').val())
                    .VideoId(data.VideoId));

                $(".yt").each(function () {
                    var ifr_source = $(this).attr('src');
                    var wmode = "wmode=transparent";
                    if (ifr_source.indexOf('?') !== -1) {
                        var getQString = ifr_source.split('?');
                        var oldString = getQString[1];
                        var newString = getQString[0];
                        $(this).attr('src', newString + '?' + wmode + '&' + oldString);
                    }
                    else $(this).attr('src', ifr_source + '?' + wmode);

                    $(this).attr('width', '232');
                    $(this).attr('height', '180');
                });
                //$().toastmessage('showSuccessToast', 'V&#237;deo inserido com sucesso.');
                $('#editProductVideos input').val(null);
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'V&#237;deo inserido.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                $('#btnSaveVideo').html('<i class="icon-ok icon-white"></i> Salvar').attr({ 'disabled': false });
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                $('#btnSaveVideo').html('<i class="icon-ok icon-white"></i> Salvar').attr({ 'disabled': false });
                //$().toastmessage('showErrorToast', data.Result);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
    });

    my.removeVideo = function (value) {
        // initiate progress effect
        kendo.ui.progress($("#productVideos"), true);

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
                                            url: '/desktopmodules/riw/api/products/removeProductVideo?productVideoId=' + value
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {

                                                var item = ko.utils.arrayFirst(my.vm.prodVideos(), function (video) {
                                                    return video.VideoId() === kendo.parseInt(value);
                                                });

                                                if (item) {
                                                    my.vm.prodVideos.remove(item);
                                                }
                                                //$().toastmessage('showSuccessToast', 'V&#237;deo removido com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'V&#237;deo removido.',
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
                                            kendo.ui.progress($("#productVideos"), false);
                                        }).fail(function (jqXHR, textStatus) {
                                            kendo.ui.progress($("#productVideos"), false);
                                            console.log(jqXHR.responseText);
                                        });
                                    }
                                },
                                'cancel': {
                                    html: 'N&#227;o',
                                    //priority: 'secondary',
                                    "class": 'dnnSecondaryAction',
                                    click: function () {
                                        kendo.ui.progress($("#productVideos"), false);
                                        $dialog.dialog('close');
                                        $dialog.dialog('destroy');
                                    }
                                }
                            }
                        });

        $dialog.dialog('open');
    };

    $('#productMenu').kendoMenu({
        select: function (e) {
            switch ($(e.item).attr('id')) {
                case 'menu_2':
                    document.location.href = editItemURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_3':
                    document.location.href = productImagesURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_4':
                    document.location.href = productDescURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_6':
                    document.location.href = relatedProductsURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_7':
                    document.location.href = productAttributesURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_8':
                    document.location.href = productShippingURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_9':
                    document.location.href = productSEOURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_11':
                    document.location.href = productFinanceURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
            }
        }
    });

    $("input:radio[name=editors]").click(function () {
        if ($(this).val() === '0') {
            $('#divProductDescriptionTextarea').delay(400).fadeIn();
            $("#divProductDescriptionEditor").fadeOut();
        } else {
            $('#divProductDescriptionTextarea').fadeOut();
            $("#divProductDescriptionEditor").delay(400).fadeIn()
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