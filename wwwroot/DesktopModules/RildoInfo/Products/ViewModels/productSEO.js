
$(function () {

    my.personId = my.getParameterByName('itemId');

    //// knockout js view model
    //my.vm = function () {
    //    // this is knockout view model
    //    var self = this;

        

    //    // make view models available for apps
    //    return {
            
    //    };

    //}();

    //// apply ko bindings
    //ko.applyBindings(my.vm);

    $('#productMenu').show();

    $('#editProductSEO .icon-info-sign').popover({
        placement: 'right',
        trigger: 'hover'
    });

    $.ajax({
        url: '/desktopmodules/riw/api/products/getProduct?productId=' + my.personId + '&lang=pt-BR'
    }).done(function (data) {
        if (data) {
            $('#seoNameTextBox').val();
            $('#pageTitleTextBox').val();
            $('#seoSummaryTextArea').val();
            $('#keywordsTextArea').val();
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

    $('#productDescriptionTextarea').kendoEditor({
        messages: {
            bold: "Negrito",
            italic: "Itálico",
            underline: "Sublinhado",
            strikethrough: "Riscado",
            superscript: "Sobrescrito",
            subscript: "Subscrito",
            justifyCenter: "Centralizar texto",
            justifyLeft: "Alinhar texto para esquerda",
            justifyRight: "Alinhar texto para direita",
            justifyFull: "Justificar",
            insertUnorderedList: "Inserir lista sem ordem",
            insertOrderedList: "Inserir lista ordenada",
            indent: "Recuar Margem",
            outdent: "Avançar Magem",
            createLink: "Inserir link",
            unlink: "Remover link",
            insertImage: "Inserir imagem",
            insertHtml: "Inserir HTML",
            fontName: "Selecionar Letra",
            fontNameInherit: "(Letra)",
            fontSize: "Selecionar Tamanho da Letra",
            fontSizeInherit: "(tamanhos)",
            formatting: "Formatar",
            style: "Estilos",
            emptyFolder: "Pasta Vazia",
            uploadFile: "Upload",
            orderBy: "Ordenar por:",
            orderBySize: "Tamanho",
            orderByName: "Nome",
            invalidFileType: "O Arquivo selecionado \"{0}\" não é válido. Arquivos permitidos são {1}.",
            deleteFile: "Tem certeza que deseja excluir \"{0}\"?",
            overwriteFile: "Um Arquivo com o nome \"{0}\" já existe nesta pasta. Deseja continuar?",
            directoryNotFound: "Não foi encontrado nenhuma pasta com este nome.",
            imageWebAddress: "Endereço URL",
            imageAltText: "Texto Alternativo",
            linkWebAddress: "Endereço URL",
            linkText: "Texto",
            linkToolTip: "Dica",
            linkOpenInNewWindow: "Abrir link em nova janela",
            dialogUpdate: "Atualizar",
            dialogInsert: "Inserir",
            dialogButtonSeparator: "ou",
            dialogCancel: "Cancelar"
        },
        tools: [
            "bold",
            "italic",
            "underline",
            "separator",
            "strikethrough",
            "foreColor",
            "backColor",
            "justifyLeft",
            "justifyCenter",
            "justifyRight",
            "justifyFull",
            "insertUnorderedList",
            "insertOrderedList",
            "indent",
            "outdent",
            "createLink",
            "unlink",
            "viewHtml",
            {
                name: "formatting", items: [
                      { text: "Titulo", value: "h1" },
                      { text: "Sub-Titulo", value: "h2" },
                      { text: "Sub-Sub-Titulo", value: "h3" },
                      { text: "Paragrafo", value: "p" },
                      { text: "Bloco", value: "blockquote" }
                ]
            }
        ]
    });

    ////$('#productDescription').wysiwym(Wysiwym.Mediawiki, {
    ////    helpEnabled: true,
    ////    helpToggle: true
    ////});
    
    //// Setup the Live Preview
    //$('#productDescription').wysiwym(Wysiwym.Markdown, {});
    //$('.wysiwym-editor .btn').tooltip({ delay: { show: 200, hide: 50 } });
    //var showdown = new Showdown.converter();
    //var previousValue = null;
    //var previewTextarea = $('#productDescription');
    //var previewOutput = $('#livepreview');
    //var updateLivePreview = function () {
    //    var newValue = previewTextarea.val();
    //    if (newValue !== previousValue) {
    //        previousValue = newValue;
    //        var newHtml = $("<div>" + showdown.makeHtml(newValue) + "</div>");
    //        newHtml.find('pre,p code').addClass('prettyprint');
    //        newHtml.find('pre code').each(function () {
    //            $(this).html(prettyPrintOne($(this).html()));
    //        });
    //        previewOutput.html(newHtml);
    //    }
    //}
    //setInterval(updateLivePreview, 200);

    $('#btnUpdateDescription').click(function (e) {
        e.preventDefault();

        $(this).html('<i class="fa fa-spinner fa-spin"></i> Um momento...').attr({ 'disabled': true });

        var params = {
            ProductId: my.personId,
            Lang: 'pt-BR',
            Description: $('#productDescriptionEditor textarea').val().trim(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };
        
        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/products/updateProductDescription',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                //$().toastmessage('showSuccessToast', 'Descri&#231;&#227;o atualizada com<br />sucesso!');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Descri&#231;&#227;o atualizada.',
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

            $('#btnUpdateDescription').html('<i class="icon-ok icon-white"></i> Salvar').attr({ 'disabled': false });
        }).fail(function (jqXHR, textStatus) {
            $('#btnUpdateDescription').html('<i class="icon-ok icon-white"></i> Salvar').attr({ 'disabled': false });
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
                case 'menu_6':
                    e.preventDefault();
                    document.location.href = relatedProductsURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_5':
                    e.preventDefault();
                    document.location.href = relatedProductsURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_7':
                    e.preventDefault();
                    document.location.href = productAttributesURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_8':
                    e.preventDefault();
                    document.location.href = productShippingURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
                case 'menu_11':
                    e.preventDefault();
                    document.location.href = productFinanceURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.personId;
                    break;
            }
        }
    });

    $("input:radio[name=editors]").click(function () {
        if ($(this).val() === '0') {
            $('#productDescriptionTextarea').data('kendoEditor').value('');
            $('#productDescriptionTextarea').data('kendoEditor').value($('.markdown-preview').html());
            $('#divProductDescriptionTextarea').delay(400).fadeIn();
            $("#divProductDescriptionEditor").fadeOut();
        } else {
            $('.markdown-editor').val('');
            $('.markdown-editor').val($('#productDescriptionTextarea').data('kendoEditor').value());
            $('#divProductDescriptionTextarea').fadeOut();
            $("#divProductDescriptionEditor").delay(400).fadeIn();
        }
    });

    //$(".markdown-input").markdown();
    ////$('.markdown-editor').css({ 'width': '90%', 'height': '220px !important' }).addClass('historyTextArea');
    //$('.markdown-editor').css({ 'min-width': '99%', 'height': '260px' }).attr({ 'cols': '30', 'rows': '10', 'placeholder': 'Editor de texto com suporte para Markdown.' }).addClass('historyTextArea');

    //$('.markdown-editor').autogrow();
    //$('.markdown-editor').css('overflow', 'hidden').autogrow();

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