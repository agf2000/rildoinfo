
$(function () {

    my.viewModel();

    my.personId = my.getParameterByName('personId');
    my.selectedPropagandaId = my.getParameterByName('pId');

    my.loadPropaganda = function (value) {
        if (value !== 0) {            
            $.ajax({
                url: '/desktopmodules/riw/api/htmlcontents/GetHtmlContent?portalId=' + portalID + '&contentId=' + value
            }).done(function (data) {
                if (data) {
                    $('#propagandaTextArea').data('kendoEditor').exec("inserthtml", { value: data.HtmlContent });
                    $('#propagandaName').val(data.ContentTitle);
                    $('#btnRemovePropaganda').show().click(function (e) {
                        e.preventDefault();

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
                                            url: '/desktopmodules/riw/api/htmlcontents/RemoveHtmlContent?contentId=' + value
                                        }).done(function (msg) {
                                            if (msg.Result.indexOf("success") !== -1) {
                                                $('#btnRemovePropaganda').hide();
                                                $('#propagandaTextArea').data('kendoEditor').value('');
                                                $('#propagandaName').val('');
                                                //$('#propagandaEditorWindow').data("kendoWindow").toFront();
                                                $().toastmessage('showSuccessToast', 'Propaganda removida com sucesso.');
                                            } else {
                                                $().toastmessage('showErrorToast', data.Msg);
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
                    });
                }
            }).fail(function (jqXHR, textStatus) {
                console.log(jqXHR.responseText);
            });
        } else {
            $('#btnRemovePropaganda').hide();
            $('#propagandaTextArea').data('kendoEditor').value('');
            $('#propagandaName').val('');
        }
    };

    parent.$('#imgWindow').kendoWindow({
        position: {
            top: 10,
            left: 10
        },
        visible: false
    });

    $('#files').kendoUpload({
        async: {
            saveUrl: "/desktopmodules/riw/api/store/postFile",
            removeUrl: "remove",
            autoUpload: true,
            multiple: true
        },
        //showFileList: false,
        localization: {
            cancel: 'Cancelar',
            dropFilesHere: 'Arraste o arquivos aqui para envia-los',
            remove: 'Remover',
            select: 'Novo Upload',
            statusUploading: 'Enviando Arquivo',
            uploadSelectedFiles: 'Enviar',
            headerStatusUploaded: 'Completo',
            //headerStatusUploading: "customHeaderStatusUploading",
            retry: "Tente Novamente",
            statusFailed: "Falha no Envio",
            statusUploaded: "statusUploaded"
        },
        select: function (e) {
            $.each(e.files, function (index, value) {
                if (value.extension.toUpperCase() !== '.JPG' && value.extension.toUpperCase() !== '.PNG' && value.extension.toUpperCase() !== '.DOC' && value.extension.toUpperCase() !== '.PDF' && value.extension.toUpperCase() !== '.DOCX' && value.extension.toUpperCase() !== '.GIF' && value.extension.toUpperCase() !== '.ZIP' && value.extension.toUpperCase() !== '.RAR') {
                    e.preventDefault();
                    $().toastmessage('showWarningToast', 'É permitido enviar somente arquivos com formato jpg e png.');
                }
            });
        },
        upload: function (e) {
            e.data = {
                portalId: portalID,
                folderPath: $('#folderPathTextBox').val(),
                maxWidth: 0,
                maxHeight: 0,
            };
        },
        success: function (e) {
            var _fileInfo = e.response;
            //$.each(e.files, function (index, value) {
            my.vm.portalFiles.unshift(new my.PortalFile()
                .fileId(_fileInfo.FileId)
                .fileName(_fileInfo.FileName)
                .contenType(_fileInfo.ContentType)
                .extension(_fileInfo.Extension)
                .fileSize('Tamanho: ' + my.size_format(_fileInfo.FileSize))
                .height(_fileInfo.Height)
                .relativePath(_fileInfo.Extension === 'jpg' || _fileInfo.Extension === 'png' || _fileInfo.Extension === 'gif' ? '/portals/' + _portalID + '/' + _fileInfo.RelativePath : '/desktopmodules/rildoinfo/webapi/content/images/spacer.gif')
                .width(_fileInfo.Width));
            //});
            //});
            $(".k-widget.k-upload").find("ul").remove();
        },
        remove: function (e) {

        },
        error: function (e) {
            $(".k-widget.k-upload").find("ul").remove();
            $().toastmessage('showErrorToast', 'Não foi possível o envio do arquivo.');
        }
    });

    $('#propagandaTextArea').kendoEditor({
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
            "insertHtml",
            "formatting",
            {
                name: 'addFile',
                tooltip: 'Gerenciador de Arquivos',
                exec: function (e) {
                    e.preventDefault();

                    if (parent.$('#window').data('kendoWindow')) {
                        parent.$('#window').data('kendoWindow').maximize();
                    }

                    var kendoWindow = $("#image-browser").kendoWindow({
                        actions: ["Maximize", "Close"],
                        title: 'Gerenciador de Arquivos',
                        resizable: true,
                        modal: true,
                        width: '80%',
                        height: '75%'
                    });

                    my.openImage = function (e) {
                        parent.$('#imgWindow').data('kendoWindow').content('<img src=' + e.name + ' />').open();
                    };

                    var folderPath = $('#folderPathTextBox').kendoAutoComplete({
                        dataSource: new kendo.data.DataSource({
                            transport: {
                                read: {
                                    url: '/desktopmodules/riw/api/store/GetPortalFolders?portalId=' + portalID + '&permissions=READ&uId=' + userID
                                }
                            }
                        }),
                        value: my.vm.selectedFolderPath(),
                        dataTextField: 'FolderPath',
                        dataValueField: 'FolderID',
                        select: function (e) {
                            e.preventDefault();
                            var dataItem = this.dataItem(e.item.index());
                            if (dataItem) {
                                my.vm.selectedFolderPath(dataItem.FolderPath);
                                this.value(dataItem.FolderPath);
                                my.vm.loadPortalFiles();
                            }
                        }
                    });
                    folderPath.attr({ 'value': my.vm.selectedFolderPath() });

                    my.vm.loadPortalFiles();

                    my.selectFile = function (e) {
                        $('#fileUrlAddress').val('http://' + siteURL + '/portals/' + portalID + '/' + my.vm.selectedFolderPath() + e.name);
                        $('#fileUrlAddressTitle').val(e.name);
                        $('.liFile').removeClass('selected');
                        $(e).addClass('selected');
                        my.vm.selectedFileId(e.id);
                    };

                    my.goUp = function () {
                        my.vm.selectedFolderPath('');
                        $('#fileUrlAddress').val('');
                        $('#fileUrlAddressTitle').val('');
                        $('#folderPathTextBox').data('kendoAutoComplete').value('');
                        $('#folderPathTextBox').attr({ 'placeholder': '' });
                        my.vm.loadPortalFiles();
                    };

                    my.addPortalFolder = function () {
                        $.ajax({
                            type: 'POST',
                            url: '/desktopmodules/riw/api/store/AddPortalFolders?portalId=' + portalID + '&folder=' + $('#folderPathTextBox').data('kendoAutoComplete').value()
                        }).done(function (data) {
                            if (data.Result.indexOf("success") !== -1) {
                                $('#fileUrlAddress').val('');
                                $('#fileUrlAddressTitle').val('');
                                my.vm.selectedFolderPath($('#folderPathTextBox').data('kendoAutoComplete').value());
                                my.vm.loadPortalFiles();
                            } else {
                                $().toastmessage('showErrorToast', data.Msg);
                            }
                        }).fail(function (jqXHR, textStatus) {
                            console.log(jqXHR.responseText);
                        });
                    };

                    $('#btnDeleteFile').click(function (e) {
                        if (e.clientX === 0) {
                            return false;
                        }
                        e.preventDefault();

                        var $this = $(this);

                        var $dialog = $('<div></div>')
                            .html('<div class="confirmDialog">Tem certeza?</div>')
                            .dialog({
                                autoOpen: false,
                                open: function () {
                                    $(".ui-dialog-title").append('Aten&#231;&#227;o');
                                },
                                autoOpen: false,
                                modal: true,
                                resizable: false,
                                dialogClass: 'dnnFormPopup',
                                buttons: {
                                    'ok': {
                                        text: 'Ok',
                                        //priority: 'primary',
                                        "class": 'btn btn-primary',
                                        click: function (e) {

                                            $this.button('loading');

                                            $.ajax({
                                                type: 'POST',
                                                url: '/desktopmodules/riw/api/store/RemovePortalFile?portalId=' + portalID + '&fileName=&fileId=' + my.vm.selectedFileId() + '&folderPath='
                                            }).done(function (data) {
                                                if (data.Result.indexOf("success") !== -1) {
                                                    $('#fileUrlAddress').val('');
                                                    $('#fileUrlAddressTitle').val('');
                                                    my.vm.loadPortalFiles();
                                                } else {
                                                    $().toastmessage('showErrorToast', data.Msg);
                                                }
                                            }).fail(function (jqXHR, textStatus) {
                                                console.log(jqXHR.responseText);
                                            }).always(function (e) {
                                                $this.button('reset');
                                            });

                                            $dialog.dialog('close');
                                            $dialog.dialog('destroy');
                                        }
                                    },
                                    'Cancel': {
                                        text: 'Cancelar',
                                        //priority: 'primary',
                                        "class": 'btn btn-secondary',
                                        click: function (e) {
                                            $dialog.dialog('close');
                                            $dialog.dialog('destroy');
                                        }
                                    }
                                }
                            });

                        $dialog.dialog('open');
                    });

                    my.removePortalFolder = function (e) {
                        var kendoConfirmWindow = $("<div />").kendoWindow({
                            title: "Aviso!",
                            resizable: false,
                            modal: true,
                            width: 200
                        });

                        kendoConfirmWindow.data("kendoWindow")
                        .content($("#delete-confirmation").html())
                        .center().open();

                        kendoConfirmWindow.find(".delete-confirm,.delete-cancel").click(function () {
                            if ($(this).hasClass("delete-confirm")) {
                                $.ajax({
                                    type: 'DELETE',
                                    url: '/desktopmodules/riw/api/store/RemovePortalFolders?portalId=' + portalID + '&folder=' + $('#folderPathTextBox').data('kendoAutoComplete').value()
                                }).done(function (data) {
                                    if (data.Result.indexOf("success") !== -1) {
                                        my.vm.selectedFolderPath('');
                                        $('#fileUrlAddress').val('');
                                        $('#fileUrlAddressTitle').val('');
                                        $('#folderPathTextBox').data('kendoAutoComplete').value('');
                                        $('#folderPathTextBox').attr({ 'placeholder': '' });
                                        my.vm.loadPortalFiles();
                                    } else {
                                        $().toastmessage('showErrorToast', data.Msg);
                                    }
                                }).fail(function (jqXHR, textStatus) {
                                    console.log(jqXHR.responseText);
                                });
                            }
                            kendoConfirmWindow.data("kendoWindow").close();
                        }).end();
                    };

                    my.closeImageBrowser = function () {
                        kendoWindow.close();
                    };

                    $('#btnInsert').click(function (e) {
                        var editor = $('#propagandaTextArea').data("kendoEditor");
                        switch (true) {
                            case (my.endsWith($('#fileUrlAddress').val().toLowerCase(), '.bmp') || my.endsWith($('#fileUrlAddress').val().toLowerCase(), '.gif') || my.endsWith($('#fileUrlAddress').val().toLowerCase(), '.png') || my.endsWith($('#fileUrlAddress').val().toLowerCase(), '.jpg')):
                                if ($('#addLink').is(':checked')) {
                                    editor.exec("inserthtml", { value: '<a title="' + $('#fileUrlAddressTitle').val() + '" href="' + $('#fileUrlAddress').val() + '">' + $('#fileUrlAddressTitle').val() + '</a>' });
                                } else {
                                    editor.exec("inserthtml", { value: '<img alt="' + $('#fileUrlAddressTitle').val() + '" src="' + $('#fileUrlAddress').val() + '" />' });
                                }
                                break;
                            case (my.endsWith($('#fileUrlAddress').val().toLowerCase(), '.pdf') || my.endsWith($('#fileUrlAddress').val().toLowerCase(), '.doc') || my.endsWith($('#fileUrlAddress').val().toLowerCase(), '.docx')):
                                editor.exec("inserthtml", { value: '<a title="' + $('#fileUrlAddressTitle').val() + '" href="' + $('#fileUrlAddress').val() + '">' + $('#fileUrlAddressTitle').val() + '</a>' });
                                break;
                            case (my.endsWith($('#fileUrlAddress').val().toLowerCase(), '.htm') || my.endsWith($('#fileUrlAddress').val().toLowerCase(), '.html')):
                                $.get($('#fileUrlAddress').val(), function (data) {
                                    editor.exec("inserthtml", { value: data });
                                });
                                break;
                            default:
                                editor.exec("inserthtml", { value: '<a title="' + $('#fileUrlAddressTitle').val() + '" href="' + $('#fileUrlAddress').val() + '">' + $('#fileUrlAddressTitle').val() + '</a>' });
                        }
                        if (parent.$('#window').data('kendoWindow')) {
                            parent.$('#window').data('kendoWindow').toggleMaximization();
                        }
                        //if (my.endsWith($('#fileUrlAddress').val(), '.jpg') || my.endsWith($('#fileUrlAddress').val(), '.png') || my.endsWith($('#fileUrlAddress').val(), '.gif')) {
                        //    editor.exec("inserthtml", { value: '<img alt="' + $('#fileUrlAddressTitle').val() + '" src="' + $('#fileUrlAddress').val() + '" />' });
                        //} else {
                        //}
                        $('#addLink').attr({ 'checked': false });
                        $('.liFile').removeClass('selected');
                        $('#fileUrlAddress').val('');
                        $('#fileUrlAddressTitle').val('');
                        $("#image-browser").data("kendoWindow").close();
                    });

                    kendoWindow.data("kendoWindow").center().open();

                    //kendoConfirmWindow.data("kendoWindow")
                    //.content($("#image-browser").html())
                    //.center().open();
                }
            }
        ],
        insertHtml: [
            { text: "Nome do Site", value: "<strong>" + siteName + "</strong>" },
            { text: "Website URL", value: "<a href='" + siteURL + "'>" + siteURL + "</a>" },
            { text: "Endereço Físico", value: "<span>" + my.vm.siteAddress() + '</span>' }
        ],
        // value: _msgContent.replace('[CLIENTE]', my.vm.firstName() + ' ' + my.vm.lastName()).replace('[WEBSITE1]', _siteName).replace('[HTTPURL]', _siteURL).replace('[URL]', _siteURL).replace('[WEBSITE2]', _siteName).replace('[LOGIN]', my.vm.username()),
    });

    //$('#propagandaEditorWindow').kendoWindow({
    //    title: 'Propaganda',
    //    modal: false,
    //    width: 950,
    //    height: 580
    //    //visible: false
    //    //close: emptyEditor
    //});

    $('#btnSavePropaganda').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            PortalId: portalID,
            ContentId: my.selectedPropagandaId,
            ContentTitle: $('#propagandaName').val(),
            HtmlContent: $('#propagandaTextArea').data('kendoEditor').value(),
            CreatedByUser: userID,
            CreatedOnDate: moment().format()
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/htmlcontents/UpdateHtmlContent',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                //var ddlPropagandas = parent.$('#clientAssistWindow').find($('#ddlPropagandas').data('kendoDropDownList'));
                //ddlPropagandas.dataSource.read();
                //ddlPropagandas.value(data.fileId);
                //$("#propagandaEditorWindow").data("kendoWindow").close();
                var sContent = assistURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/10/pId/' + data.ContentId;
                document.location.href = sContent;
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

    if (my.selectedPropagandaId > 0) {
        my.loadPropaganda(my.selectedPropagandaId);
    }

    //$('#propagandaName').on('keyup', function (e) {
    //    if ($(this).val().length > 1) $('#btnSavePropaganda').attr({ 'disabled': false, 'class': 'k-button' });
    //});

    $('#btnReturn').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var sContent = assistURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/10';
        document.location.href = sContent;
    });

});
