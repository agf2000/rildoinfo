$(function () {

    my.productId = my.getParameterByName('itemId');

    // product image view model
    my.ProductImage = function () {
        this.ProductImageId = ko.observable();
        this.ProductId = ko.observable();
        this.ProductImageUrl = ko.observable();
        this.ProductImageBinary = ko.observable();
        this.ContentLength = ko.observable();
        this.FileName = ko.observable();
        this.Extension = ko.observable();
        this.CreatedByUser = ko.observable();
        this.CreatedOnDate = ko.observable();
        this.ModifiedByUser = ko.observable();
        this.ModifiedOnDate = ko.observable();
        this.ProductName = ko.observable();
        this.ListOrder = ko.observable();
    };

    // knockout js view model
    my.vm = function () {
        // this is knockout view model
        var self = this;

        self.sortProducts = function () {
            self.prodImages.sort(function (left, right) {
                return (left.ListOrder() < right.ListOrder() ? -1 : 1);
            });
        },

        self.selectedProdImage = ko.observable(),
        self.prodImages = ko.observableArray([]),

        // ajax function to get product images
        self.loadProdImages = function () {
            $.ajax({
                url: '/desktopmodules/riw/api/products/GetProductImages?productId=' + my.productId
            }).done(function (data) {
                self.prodImages.removeAll();
                $.each(data, function (i, pi) {
                    self.prodImages.push(new my.ProductImage()
                        .ContentLength(pi.ContentLength)
                        .CreatedByUser(pi.CreatedByUser)
                        .CreatedOnDate(pi.CreatedOnDate)
                        .Extension(pi.Extension)
                        .FileName(pi.FileName)
                        .ModifiedByUser(pi.ModifiedByUser)
                        .ModifiedOnDate(pi.ModifiedOnDate)
                        .ProductId(pi.ProductId)
                        .ProductImageId(pi.ProductImageId)
                        .ProductImageUrl(pi.ProductImageUrl)
                        .ProductName(pi.ProductName)
                        .ListOrder(pi.ListOrder));

                    // initiate colobox jquery plugin
                    //$(".photo").colorbox();
                    $('#a_img_' + pi.ProductImageId).click(function (event) {
                        event.preventDefault(); // this just cancels the default link behavior.
                        $.colorbox({ href: $(this).attr("href"), width: '90%', height: '90%' }); //this makes the parent window load the showColorBox function, using the a.colorbox href value
                    });
                });

                self.sortProducts();
            });
        };

        // make view models available for apps
        return {
            prodImages: prodImages,
            loadProdImages: loadProdImages
        };

    }();

    // apply ko bindings
    ko.applyBindings(my.vm);

    $('#productMenu').show();

    $('#productImgs').kendoUpload({
        async: {
            saveUrl: "/desktopmodules/riw/api/products/postProductImage",
            removeUrl: "remove",
            autoUpload: true,
            multiple: true
        },
        //showFileList: false,
        localization: {
            cancel: 'Cancelar',
            dropFilesHere: 'Arraste o arquivo aqui para envia-lo',
            remove: 'Remover',
            select: 'Enviar Imagem(ns)',
            statusUploading: 'Enviando Arquivo(s)',
            uploadSelectedFiles: 'Enviar',
            headerStatusUploaded: 'Envio Completo',
            //headerStatusUploading: "customHeaderStatusUploading",
            retry: "Tente Novamente",
            statusFailed: "Falha no Envio",
            statusUploaded: "statusUploaded"
        },
        select: function (e) {
            $.each(e.files, function (index, value) {
                if (value.extension.toUpperCase() !== '.JPG' && value.extension.toUpperCase() !== '.PNG' && value.extension.toUpperCase() !== '.DOC' && value.extension.toUpperCase() !== '.PDF' && value.extension.toUpperCase() !== '.DOCX' && value.extension.toUpperCase() !== '.GIF' && value.extension.toUpperCase() !== '.ZIP' && value.extension.toUpperCase() !== '.RAR') {
                    e.preventDefault();
                    //$().toastmessage('showWarningToast', 'É permitido enviar somente arquivos com formato jpg e png.');
                    $.pnotify({
                        title: 'Aten&#231;&#227;o!',
                        text: '&#201; permitido enviar somente arquivos com formato jpg e png.',
                        type: 'info',
                        icon: 'fa fa-exclamation-circle fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                }
            });
        },
        upload: function (e) {
            e.data = {
                portalId: portalID,
                personId: my.productId,
                uId: userID,
                cd: moment().format(),
                maxWidth: 0,
                maxHeight: 0
            };
        },
        success: function (e) {
            //$.each(e.files, function (index, value) {                
            //});
        },
        complete: function () {
            // initiate progress effect
            kendo.ui.progress($("#productImages"), true);

            // afeter images are uploaded, run ajax call to get all product's images
            $.ajax({
                url: '/desktopmodules/riw/api/products/GetProductImages?productId=' + my.productId
            }).done(function (data) {
                my.vm.loadProdImages();
                //my.vm.prodImages.removeAll();
                //$.each(data, function (i, pi) {
                //    my.vm.prodImages.push(new my.ProductImage()
                //        .ContentLength(pi.ContentLength)
                //        .CreatedByUser(pi.CreatedByUser)
                //        .CreatedOnDate(pi.CreatedOnDate)
                //        .Extension(pi.Extension)
                //        .FileName(pi.FileName)
                //        .ModifiedByUser(pi.ModifiedByUser)
                //        .ModifiedOnDate(pi.ModifiedOnDate)
                //        .ProdId(pi.ProdId)
                //        .ProdImagesId(pi.ProdImagesId)
                //        .ProdImageUrl(pi.ProdImageUrl)
                //        .ProdName(pi.prodName));
                //});
            });

            $(".k-widget.k-upload").find("ul").remove();

            // end progress effect
            kendo.ui.progress($("#productImages"), false);
        },
        remove: function (e) {

        },
        error: function (e) {
            //$().toastmessage('showErrorToast', 'Não foi possível o envio do arquivo.');
            $.pnotify({
                title: 'Erro!',
                text: 'N&#227;o foi poss&#237;vel o envio do arquivo.',
                type: 'error',
                icon: 'fa fa-times-circle fa-lg',
                addclass: "stack-bottomright",
                stack: my.stack_bottomright
            });
        }
    });

    $('div.k-button').removeClass('k-button').addClass('btn btn-small btn-inverse');

    my.createImgThumbs = function () {
        $('#productImages').sortable({
            update: function () {

                var params = [];
                $.each($('#productImages li'), function (index) {
                    params.push({
                        'ProductImageId': kendo.parseInt($('#productImages li')[index].id),
                        'ListOrder': index
                    });
                });

                //var params = [                  
                //    {
                //        ProductImageId: 1,
                //        ListOrder: 0
                //    },
                //    {
                //        ProductImageId: 2,
                //        ListOrder: 1
                //    },
                //    {
                //        ProductImageId: 3,
                //        ListOrder: 2
                //    }
                //];

                $.ajax({
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    url: '/desktopmodules/riw/api/products/updateProductImageOrder',
                    data: JSON.stringify(params)
                }).done(function (data) {
                    if (data.Result.indexOf("success") !== -1) {
                        //$().toastmessage('showSuccessToast', 'Ordem das imagens atualizadas');
                        $.pnotify({
                            title: 'Sucesso!',
                            text: 'Ordem das imagens atualizadas.',
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
                }).fail(function (jqXHR, textStatus) {
                    console.log(jqXHR.responseText);
                });

            }
        });
        $('#productImages').disableSelection();
    };

    my.vm.loadProdImages();
    my.createImgThumbs();

    $('#productMenu').kendoMenu({
        select: function (e) {
            e.preventDefault();
            var urlAddress = '';
            switch ($(e.item).attr('id')) {
                case 'menu_2':
                    urlAddress = editItemURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_4':
                    urlAddress = productDescURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_6':
                    urlAddress = relatedProductsURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_7':
                    urlAddress = productAttributesURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_8':
                    urlAddress = productShippingURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_9':
                    urlAddress = productSEOURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_11':
                    urlAddress = productFinanceURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
            }
            document.location.href = urlAddress;
        }
    });

    my.removeImage = function (value) {
        // initiate progress effect
        kendo.ui.progress($("#productImages"), true);

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
                                            url: '/desktopmodules/riw/api/products/removeProductImage?productImageId=' + value
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                my.vm.loadProdImages();
                                                //$().toastmessage('showSuccessToast', 'Imagem excluida com sucesso!');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Imagem excluida!',
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
                                            kendo.ui.progress($("#productImages"), false);
                                        }).fail(function (jqXHR, textStatus) {
                                            kendo.ui.progress($("#productImages"), false);
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
    };
    
    $('.btnReturn').click(function (e) {
        e.preventDefault();

        $(this).html('<i class="fa fa-spinner fa-spin"></i>Um momento...').attr({ 'disabled': true });
        var urlAddress = '';
        if (my.retSel > 0) {
            switch (my.retSel) {
                case 7:
                    urlAddress = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.productId + '/sel/7';
                    break;
                case 8:
                    urlAddress = historyURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.productId + '/sel/8';
                    break;
                case 9:
                    urlAddress = finanURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.productId + '/sel/9';
                    break;
                case 10:
                    urlAddress = assistURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.productId + '/sel/10';
                    break;
                case 11:
                    urlAddress = commURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.productId + '/sel/11';
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