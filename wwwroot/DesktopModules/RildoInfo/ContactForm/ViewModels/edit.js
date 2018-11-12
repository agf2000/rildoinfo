
$(function () {

    my.viewModel();

    $.valHooks.textarea = {
        get: function (elem) {
            return elem.value.replace(/\r?\n/g, "\r\n");
        }
    };

    $('#postOfficeMessageTextBox').val(toMarkdown(_postOfficeMessage));
    $('#emailMessageTextBox').val(toMarkdown(_emailMessage));
    $('#autoAnswerTextBox').val(toMarkdown(_autoAnswer));

    $('.fa-exclamation-circle').popover({
        placement: 'top',
        trigger: 'hover'
    });

    my.GetValue = function (value) {
        // console.log(value);
        switch (value.toLowerCase()) {
            case "png": return '<i class="fam-image" title="' + value.toUpperCase() + '"></i>'; break;
            case "jpg": return '<i class="fam-image" title="' + value.toUpperCase() + '"></i>'; break;
            case "svg": return '<i class="fam-image" title="' + value.toUpperCase() + '"></i>'; break;
            case "gif": return '<i class="fam-image" title="' + value.toUpperCase() + '"></i>'; break;
            case "ico": return '<i class="fam-image" title="' + value.toUpperCase() + '"></i>'; break;
            case "txt": return '<i class="fam-page-white-text" title="' + value.toUpperCase() + '"></i>'; break;
            case "log": return '<i class="fam-page-white-log" title="' + value.toUpperCase() + '"></i>'; break;
            case "htm": return '<i class="fam-page-white-green" title="' + value.toUpperCase() + '"></i>'; break;
            case "php": return '<i class="fam-page-white-php" title="' + value.toUpperCase() + '"></i>'; break;
            case "js": return '<i class="fam-page-white-js" title="' + value.toUpperCase() + '"></i>'; break;
            case "css": return '<i class="fam-page-white-css" title="' + value.toUpperCase() + '"></i>'; break;
            case "pdf": return '<i class="fam-page-white-acrobat" title="' + value.toUpperCase() + '"></i>'; break;
            case "zip": return '<i class="fam-page-white-compressed" title="' + value.toUpperCase() + '"></i>'; break;
            case "doc": return '<i class="fam-page-word" title="' + value.toUpperCase() + '"></i>'; break;
            case "docx": return '<i class="fam-page-white-word" title="' + value.toUpperCase() + '"></i>'; break;
            case "xls": return '<i class="fam-page-excel" title="' + value.toUpperCase() + '"></i>'; break;
            case "xlsx": return '<i class="fam-page-white-excel" title="' + value.toUpperCase() + '"></i>'; break;

            default: return '<i class="fam-page-white" title="' + value.toUpperCase() + '"></i>';
        }
        //if (value && value != null && value.indexOf("A") == 0)
        //    return "<b style='color:red'>" + value + "</b>";
        //else
        //    return "";
    };

    // create kendo dataSource transport to get files
    my.docsTransport = {
        read: {
            url: '/desktopmodules/riwcf/api/contactform/getDocs'
        },
        parameterMap: function (data, type) {
            return {
                portalId: _portalID
            };
        }
    };

    // create kendo dataSource for getting products transport
    my.docsData = new kendo.data.DataSource({
        transport: my.docsTransport,
        schema: {
            model: {
                id: 'DocId'
            }
        },
        pageSize: 10
    });

    $('#docsGrid').kendoGrid({
        dataSource: my.docsData,
        columns: [
            {
                title: 'Arquivo',
                field: 'DocName'
            },
            {
                title: 'Descrição',
                field: 'DocDesc'
            },
            {
                title: 'Tipo',
                field: 'Extension',
                template: '#= my.GetValue(Extension) #',
                width: 40
            },
            {
                command: [
                    {
                        name: "exclude",
                        text: " ",
                        imageClass: "fa fa-times",
                        click: function (e) {
                            e.preventDefault();

                            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                            if (dataItem) {

                                var $dialog = $('<div></div>')
                                    .html('<p class="confirmDialog">Tem certeza?</p>')
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
                                                "class": 'btn btn-primary',
                                                click: function () {

                                                    $.ajax({
                                                        type: 'DELETE',
                                                        url: '/desktopmodules/riwcf/api/contactform/removeDoc?portalId=' + _portalID + '&docId=' + dataItem.DocId
                                                    }).done(function (data) {
                                                        if (data.Result.indexOf("success") !== -1) {
                                                            $('#docsGrid').data('kendoGrid').dataSource.remove(dataItem);
                                                            //$().toastmessage('showSuccessToast', 'Escolha do plano salva com sucesso.');
                                                            $.pnotify({
                                                                title: 'Sucesso!',
                                                                text: 'Arquivo excluido.',
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
                                                            //$().toastmessage('showErrorToast', msg.Result);
                                                        }
                                                    }).fail(function (jqXHR, textStatus) {
                                                        console.log(jqXHR.responseText);
                                                    }).always(function () {
                                                        $dialog.dialog('close');
                                                        $dialog.dialog('destroy');
                                                    });
                                                }
                                            },
                                            'cancel': {
                                                html: 'Cancelar',
                                                //priority: 'secondary',
                                                "class": 'btn',
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
                //title: '',
                width: 45
                //headeTemplate: ''
            }
        ],
        pageable: {
            pageSizes: [20, 40, 60],
            refresh: true,
            numeric: false,
            input: true,
            messages: {
                display: "{0} - {1} de {2} Itens",
                empty: "Sem Registro.",
                page: "P&#225;gina",
                of: "de {0}",
                itemsPerPage: "Itens por p&#225;gina",
                first: "Ir para primeira p&#225;gina",
                previous: "Ir para p&#225;gina anterior",
                next: "Ir para pr&#243;xima p&#225;gina",
                last: "Ir para &#250;ltima p&#225;gina",
                refresh: "Recarregar"
            }
        },
        sortable: true,
        reorderable: true,
        resizable: true,
        scrollable: true
    });
    
    $('#files').kendoUpload({
        async: {
            saveUrl: '/desktopmodules/riwcf/api/contactform/postFile',
            removeUrl: "remove",
            autoUpload: false
        },
        multiple: false,
        //showFileList: false,
        localization: {
            cancel: 'Cancelar',
            dropFilesHere: 'Arraste o arquivo aqui para envia-lo',
            remove: 'Remover',
            select: 'Selecionar Arquivo',
            statusUploading: 'Enviando Arquivo',
            uploadSelectedFiles: 'Enviar',
            headerStatusUploaded: 'Completo',
            //headerStatusUploading: "customHeaderStatusUploading",
            retry: "Tente Novamente",
            statusFailed: "Falha no Envio",
            statusUploaded: "statusUploaded"
        },
        select: function (e) {
            if ($('#docNameTextBox').val().length === 0) {
                e.preventDefault();
                $('#docNameTextBox').focus();
                //$().toastmessage('showErrorToast', 'Favor inserir o nome do documento.');
                $.pnotify({
                    title: 'Erro!',
                    text: 'Favor inserir o nome do documento',
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
            }
            $.each(e.files, function (index, value) {
                if (value.extension.toUpperCase() !== '.JPG' &&
                    value.extension.toUpperCase() !== '.PNG' &&
                    value.extension.toUpperCase() !== '.ZIP' &&
                    value.extension.toUpperCase() !== '.DOC' &&
                    value.extension.toUpperCase() !== '.DOCX' &&
                    value.extension.toUpperCase() !== '.XLSX' &&
                    value.extension.toUpperCase() !== '.XLS' &&
                    value.extension.toUpperCase() !== '.PDF') {
                    e.preventDefault();
                    //$().toastmessage('showErrorToast', '&#201; permitido enviar somente arquivos com formato jpg, png, zip, doc, docx e pdf.');
                    $.pnotify({
                        title: 'Erro!',
                        text: '&#201; permitido enviar somente arquivos com formato jpg, png, zip, doc, docx e pdf.',
                        type: 'error',
                        icon: 'fa fa-times-circle fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                }
                //$('.k-upload-button').hide();
                //var fileSize = 0;
                //if (value.size > 1024 * 1024)
                //    fileSize = (Math.round(value.size * 100 / (1024 * 1024)) / 100).toString() + 'MB';
                //else
                //    fileSize = (Math.round(value.size * 100 / 1024) / 100).toString() + 'KB';

                //document.getElementById('fileName').innerHTML = 'Arquivo: ' + value.name;
                //document.getElementById('fileSize').innerHTML = 'Tamanho: ' + fileSize;
                //document.getElementById('fileType').innerHTML = 'Tipo: ' + value.rawFile.type;
            });
            setTimeout(function () {
                //$('.k-upload').css({ 'max-width': '80%' }); // ($('.k-filename').width() * 2) });
                $('.k-upload button').removeClass('k-button').addClass('btn btn-small btn-inverse k-upload-selected').css({ 'margin-bottom': '10px' });
            });
        },
        upload: function (e) {
            e.data = {
                PortalId: _portalID,
                ModuleId: _moduleID,
                DocName: $('#docNameTextBox').val(),
                DocDesc: $('#docDescTextArea').val(),
                CreatedByUser: _userID,
                CreatedOnDate: moment().format()
            };
        },
        success: function (e) {
            $.each(e.files, function (index, value) {
                $('#docsGrid').data('kendoGrid').dataSource.add({
                    DocId: e.response.DocId,
                    DocName: $('#docNameTextBox').val(),
                    DocDesc: $('#docDescTextArea').val(),
                    Extension: value.extension.replace('.', '')
                });
            });

            $('#docNameTextBox').val('');
            $('#docDescTextArea').val('');
            $('.k-upload-files').remove();
            //$().toastmessage('showSuccessToast', 'Arquivo enviado com sucesso.');
            $.pnotify({
                title: 'Sucesso!',
                text: 'Arquivo enviado.',
                type: 'success',
                icon: 'fa fa-check fa-lg',
                addclass: "stack-bottomright",
                stack: my.stack_bottomright
            });
            $('#docNameTextBox').focus();
        },
        remove: function (e) {
            //$('.k-upload-button').show();
        },
        error: function (e) {
            $.pnotify({
                title: 'Erro!',
                text: 'N&#227;o foi poss&#237;vel o envio do arquivo.',
                type: 'error',
                icon: 'fa fa-times-circle fa-lg',
                addclass: "stack-bottomright",
                stack: my.stack_bottomright
            });
            //$().toastmessage('showErrorToast', 'Não foi possível o envio do arquivo.');
        }
    });

    $('#btnSaveDefinition').click(function (e) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var converter = new Showdown.converter();

        var params = [
            {
                'id': _tModuleID,
                'name': 'RIW_ContactForm_SendTo',
                'value': my.vm.sendTo()
            },
            {
                'id': _tModuleID,
                'name': 'RIW_ContactForm_poMessage',
                'value': $('#postOfficeMessageTextBox').val().trim()
            },
            {
                'id': _tModuleID,
                'name': 'RIW_ContactForm_emailMessage',
                'value': $('#emailMessageTextBox').val().trim()
            },
            {
                'id': _tModuleID,
                'name': 'RIW_ContactForm_autoAnswer',
                'value': $('#autoAnswerTextBox').val().trim()
            },
            {
                'id': _tModuleID,
                'name': 'RIW_ContactForm_reqSend',
                'value': my.vm.reqSend()
            },
            {
                'id': _tModuleID,
                'name': 'RIW_ContactForm_reqAddress',
                'value': my.vm.reqAddress()
            },
            {
                'id': _tModuleID,
                'name': 'RIW_ContactForm_reqTelephone',
                'value': my.vm.reqTelephone()
            }
        ];

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riwcf/api/contactform/updateTabModuleSettings',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: '',
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
    
    $('#btnSaveSMTP').click(function (e) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = [
            {
                'id': _tModuleID,
                'name': 'RIW_ContactForm_smtpServer',
                'value': my.vm.smtpServer()
            },
            {
                'id': _tModuleID,
                'name': 'RIW_ContactForm_smtpPort',
                'value': my.vm.smtpPort()
            },
            {
                'id': _tModuleID,
                'name': 'RIW_ContactForm_smtpLogin',
                'value': my.vm.smtpLogin()
            },
            {
                'id': _tModuleID,
                'name': 'RIW_ContactForm_smtpPassword',
                'value': my.vm.smtpPassword()
            },
            {
                'id': _tModuleID,
                'name': 'RIW_ContactForm_smtpConnection',
                'value': my.vm.smtpConnection()
            }
        ];

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riwcf/api/contactform/updateTabModuleSettings',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: '',
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

    $('.return').click(function (e) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        document.location.href = 'http://' + _siteURL;
    });

    $('.markdown-editor').css({ 'min-width': '90%', 'height': '80px', 'margin-bottom': '5px' }).attr({ 'cols': '30', 'rows': '2' });

    $('.markdown-editor').autogrow();
    $('.markdown-editor').css('overflow', 'hidden').autogrow();

    $('.togglePreview').click(function (e) {
        e.preventDefault();
        var $this = $(this);

        var converter = new Showdown.converter();

        var ele = $($this).data('provider');

        var $dialog = $('<div></div>')
            .html(converter.makeHtml($('#' + ele).val().trim()))
            .dialog({
                autoOpen: false,
                open: function () {
                    $(".ui-dialog-title").append('Texto');
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

});
