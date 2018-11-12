
$(function () {

    my.personId = my.getParameterByName('personId');

    //var status = $(".status");

    my.viewModel();

    my.hub = $.connection.peopleHub;

    my.hub.client.pushHistoryComment = function (item, index) {
        my.vm.filteredHistories()[index].historyComments.unshift(new my.HistoryComment(item));
        my.sendNotification('/desktopmodules/rildoinfo/webapi/content/images/ri-logo.png', siteName, 'O website ' + siteURL + ' foi atualizado.', 6000, !my.hasFocus);
    };

    my.hub.client.pushHistory = function (item) {
        my.vm.personHistories.unshift(new my.PersonHistory(item));
        my.sendNotification('/desktopmodules/rildoinfo/webapi/content/images/ri-logo.png', siteName, 'O website ' + siteURL + ' foi atualizado.', 6000, !my.hasFocus);
    };

    $.connection.hub.start().done(function () {
        my.hub.server.clientsJoin(portalID.toString() + '_' + my.personId.toString());
    });
    
    $("#actionsMenu").jqxMenu();

    $('.icon-info-sign').tooltip();

    my.getPerson = function () {
        if (my.personId !== 0) {
            $.ajax({
                url: '/desktopmodules/riw/api/people/GetPerson?personId=' + my.personId,
                async: false
            }).done(function (data) {
                if (data) {
                    $('#actionsMenu').show();

                    my.vm.personType(data.PersonType);
                    my.vm.personId(data.PersonId);
                    my.vm.personUserId(data.UserId);
                    my.vm.displayName(data.DisplayName);
                    my.vm.firstName(data.FirstName);
                    my.vm.lastName(data.LastName);
                    my.vm.cpf(data.Cpf);
                    my.vm.ident(data.Ident);
                    my.vm.telephone(data.Telephone);
                    my.vm.cell(data.Cell);
                    my.vm.fax(data.Fax);
                    my.vm.originalEmail(data.Email);
                    if (data.Email.length > 2) {
                        my.vm.email(data.Email);
                        $('#actionsMenu li:nth-child(5)').show();
                    }
                    my.vm.companyName(data.CompanyName);
                    my.vm.zero800(data.Zero800s);
                    my.vm.dateFound(data.DateFound);
                    my.vm.dateRegistered(data.DateRegistered);
                    my.vm.statusId(data.StatusId);
                    my.vm.ein(data.Cnpj);
                    my.vm.stateTax(data.StateTax);
                    my.vm.cityTax(data.CityTax);
                    my.vm.comments(data.Comments);
                    my.vm.bio(data.Biography);
                    my.vm.locked(data.Locked);
                    if (data.MonthlyIncome > 0) my.vm.income(data.MonthlyIncome);
                    my.vm.selectedFinanAddress(data.PersonAddressId);
                    my.vm.website(data.Website);
                    if (data.Sent === true) {
                        $('#sentStatus').html('&#218;ltimo envio n&#227;o confirmado. Clique aqui para confirmar recebimento do &#250;ltimo envio.');
                        $('#sentConfirm').attr({ 'disabled': false });
                    } else {
                        $('#sentStatus').text('Nada enviado recentemente.');
                        $('#sentConfirm').attr({ 'disabled': true });
                    }
                    my.vm.selectedSalesRepId(data.SalesRep);
                    my.vm.createdByUser(data.CreatedByUser);
                    my.vm.createdOnDate(data.CreatedOnDate);                    

                    //$('#moduleTitle').text('Cliente: ' + data.DisplayName + ' (ID:' + data.ClientId + ')');

                    $.getJSON('/desktopmodules/riw/api/people/getHistory?personId=' + my.personId, function (data) {
                        if (data.length > 0) {
                            my.vm.personHistories.removeAll();
                            $.each(data, function (i, item) {
                                my.vm.personHistories.unshift(new my.PersonHistory(item));
                            });
                        }
                    });
                }
            });
        }
    };
    my.getPerson();

    my.vm.loadContacts();

    $('.editContacts').click(function (e) {
        e.preventDefault();

        $(this).html('<i class="fa fa-spinner fa-spin"></i>Um momento...').attr({ 'disabled': true })
        document.location.href = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/7/subSel/3/retSel/11';
    });

    //$('#ddlMsgContacts').data('kendoDropDownList').text(' Selecionar ');
    //$('#ddlMsgContacts').data('kendoDropDownList').bind('dataBound', function (e) {
    //    if (this.dataSource.total() === 0) {
    //        this.text('Sem Registro');
    //        this.enable(false);
    //    } else {
    //        this.text(' Selecionar ');
    //        this.enable(true);
    //    }
    //});

    if (my.vm.personUserId() > 0) {
        $.ajax({
            async: false,
            url: '/desktopmodules/riw/api/people/GetUser?portalId=' + portalID + '&userId=' + my.vm.personUserId()
        }).done(function (data) {
            if (data) {
                $.each(data, function (i, userInfo) {
                    my.vm.username(userInfo.UserName);
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
                //$().toastmessage('showErrorToast', data.Msg);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
    }

    $('#emailSubjectTextBox').attr({ 'placeholder': passwordSubject() });

    $('#emailBody').kendoEditor({
        messages: {
            bold: "Negrito",
            italic: "Italico",
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
            invalidFileType: "O Arquivo selecionado \"{0}\" invalido. Arquivos permitidos {1}.",
            deleteFile: "Tem certeza que deseja excluir \"{0}\"?",
            overwriteFile: "Um Arquivo com o nome \"{0}\" existe nesta pasta. Deseja continuar?",
            directoryNotFound: "Nada encontrado na pasta.",
            imageWebAddress: "URL",
            imageAltText: "Texto Alternativo",
            linkWebAddress: "URL",
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
            {
                name: 'addFile',
                tooltip: 'Gerenciador de Arquivos',
                exec: function (e) {
                    e.preventDefault();

                    parent.$('#window').data('kendoWindow').maximize();

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
                    };

                    my.removePortalFile = function (e) {
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
                                    type: 'POST',
                                    url: '/desktopmodules/riw/api/store/RemovePortalFile?portalId=' + portalID + '&fileName=&fileId=' + my.vm.selectedFileId() + '&folderPath='
                                }).done(function (data) {
                                    if (data.Result.indexOf("success") !== -1) {
                                        $('#fileUrlAddress').val('');
                                        $('#fileUrlAddressTitle').val('');
                                        my.vm.loadPortalFiles();
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
                            kendoConfirmWindow.data("kendoWindow").close();
                        }).end();
                    };

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
                            kendoConfirmWindow.data("kendoWindow").close();
                        }).end();
                    };

                    my.closeImageBrowser = function () {
                        kendoWindow.close();
                    };

                    $('#btnInsert').click(function (e) {
                        var editor = $('#emailBody').data("kendoEditor");
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
                }
            }
        ],
        insertHtml: [
            { text: "Nome do Site", value: "<strong>" + siteName + "</strong>" },
            { text: "Website URL", value: "<a href='" + siteURL + "'>" + siteURL + "</a>" },
            { text: "Endereco", value: "<span>" + my.vm.siteAddress() + '</span>' }
        ],
        
        value: (my.vm.personUserId() > 0 ? (msgContent.replace('[CLIENTE]', my.vm.firstName() + ' ' + my.vm.lastName()).replace('[WEBSITE1]', siteName).replace('[HTTPURL]', siteURL).replace('[URL]', siteURL).replace('[WEBSITE2]', siteName).replace('[LOGIN]', my.vm.username())) : '')
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
            select: 'Upload',
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
                    //$().toastmessage('showWarningToast', 'É permitido enviar somente arquivos com formato jpg e png.');
                    $.pnotify({
                        title: 'Erro!',
                        text: '&#201; permitido enviar somente arquivos com formato jpg e png.',
                        type: 'error',
                        icon: 'fa fa-times-circle fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
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

    $('#btnSendMsg').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        if (my.vm.selectedContacts().length > 0) {

            var params = {
                PortalId: portalID,
                PersonId: my.personId,
                PersonUserId: my.vm.personUserId(),
                UserId: userID,
                HistoryText: $('#addToHistory').is(':checked') ? $('#emailBody').data('kendoEditor').value() : null,
                Emails: JSON.stringify(my.vm.selectedContacts()),
                Subject: $('#emailSubjectTextBox').val().length > 0 ? $('#emailSubjectTextBox').val() : $('#emailSubjectTextBox').attr('placeholder'),
                MessageBody: $('#emailBody').data('kendoEditor').value(),
                CreatedByUser: userID,
                CreatedOnDate: moment().format()
            };

            $.ajax({
                type: 'POST',
                url: '/desktopmodules/riw/api/store/sendNewEmail',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    $('#emailBody').data('kendoEditor').value('');
                    //my.vm.personHistories.unshift(new my.PersonHistory().historyId(0).historyText(params.HistoryText));
                    if ($('#addToHistory').is(':checked')) {
                        params.Avatar = amplify.store.sessionStorage('avatar');
                        params.DisplayName = displayName;
                        my.vm.personHistories.unshift(new my.PersonHistory(params));
                    }
                    $('#emailSubjectTextBox').attr({ 'placeholder': my.vm.passwordSubject() });
                    if (my.vm.personUserId() > 0) {
                        $('#emailBody').data('kendoEditor').value(msgContent.replace('[CLIENTE]', my.vm.firstName() + ' ' + my.vm.lastName()).replace('[WEBSITE1]', siteName).replace('[HTTPURL]', siteURL).replace('[URL]', siteURL).replace('[WEBSITE2]', siteName).replace('[LOGIN]', my.vm.username()))
                    }
                    //$().toastmessage('showSuccessToast', 'Mensagem enviada com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Mensagem transmitida.',
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
                    //$().toastmessage('showErrorToast', data.Msg);
                };
            }).fail(function (jqXHR, textStatus) {
                console.log(jqXHR.responseText);
            }).always(function () {
                $this.button('reset');
            });
        }
    });

    if (my.getParameterByName('sel')) {
        var sel = my.getParameterByName('sel');
        $('#actionsMenu ul li[id=' + sel + ']').addClass('jqx-menu-item-selected');
        //if (sel <= 7) {
        //    $('#personMenu ul li:first-child').addClass('jqx-menu-item-selected');
        //    //} else {
        //}
        if (my.getParameterByName('subSel')) {
            //$('#personMenu ul li').removeClass('jqx-menu-item-selected');
            var subSel = my.getParameterByName('subSel');
            $('#personMenu ul li[id=' + subSel + ']').addClass('jqx-menu-item-selected');
        } else {
            $('#personMenu ul li:first-child').addClass('jqx-menu-item-selected');
        }
    }

    $('#btnAddHistory').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var historyHtmlContent = my.converter.makeHtml($('#historyTextarea').val().trim());

        var params = {
            PersonId: my.personId,
            StatusId: $('#ddlStatuses').data('kendoDropDownList').value(),
            HistoryText: historyHtmlContent,
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };
        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/people/UpdatePersonStatus',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $('#historyTextarea').val('');
                //$.getJSON('/desktopmodules/riw/api/clients/GetClientHistory?cId=' + my.vm.personId(), function (data) {
                //    if (data.length > 0) {
                //        // $('#history').html('');
                //        my.vm.clientHistories.removeAll();
                //        for (var i = 0; i < data.length; i++) {
                //            // $('#history').append('<span>' + data[i].HistoryText + '</span><br />')
                //            my.vm.clientHistories.unshift(new my.ClientHistory().historyId(data[i].HistoryId).historyText(data[i].HistoryText));
                //        }
                //    }
                //});
                //my.vm.personHistories.unshift(new my.PersonHistory().historyId(0).historyText(params.HistoryText));
                params.Avatar = amplify.store.sessionStorage('avatar'); // ? '/portals/0/' + _avatar + '?w=45&h=45&mode=crop' : '/desktopmodules/rildoinfo/webapi/content/images/user.png?w=45&h=45';
                params.DisplayName = displayName;
                my.vm.personHistories.unshift(new my.PersonHistory(params));
                //$().toastmessage('showSuccessToast', 'Hist&#243;rico atualizado com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Hist&#243;rico atualizado.',
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
                //$().toastmessage('showErrorToast', data.Msg);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
    });

    $('.markdown-editor').css({ 'min-width': '90%', 'height': '80px', 'margin-bottom': '5px' }).attr({ 'cols': '30', 'rows': '2' });

    $('.markdown-editor').autogrow();
    $('.markdown-editor').css('overflow', 'hidden').autogrow();

    $('.togglePreview').click(function (e) {
        e.preventDefault();
        var $this = $(this);

        var $dialog = $('<div></div>')
            .html(my.converter.makeHtml($('#historyTextArea').val().trim()))
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

    $('#actionsMenu').on('itemclick', function (event) {
        //event.args.textContent = 'Um momento...';
        switch (event.args.id) {
            case '7':
                document.location.href = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/7';
                break;
            case '8':
                document.location.href = historyURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/8';
                break;
            case '9':
                document.location.href = finanURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/9';
                break;
            case '10':
                document.location.href = assistURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/10';
                break;
            case '11':
                document.location.href = commURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/11';
                break;
            default:
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
                    //parent.$(".k-window-content").each(function (i, win) {
                    parent.$('#window').data("kendoWindow").close();
                    //});
                    //$(".k-window .k-window-content").each(function (index, element) {
                    //    setTimeout(function () {
                    //        $(element).data('kendoWindow').close();
                    //    }, 1000);
                    //});
                    //if (parent.$('#clientFinanWindow').data('kendoWindow')) {
                    //    parent.$('#clientFinanWindow').data('kendoWindow').close();
                    //}
                    //if (parent.$('#clientEditWindow').data('kendoWindow')) {
                    //    parent.$('#clientEditWindow').data('kendoWindow').close();
                    //}
                    //if (parent.$('#clientAssistWindow').data('kendoWindow')) {
                    //    parent.$('#clientAssistWindow').data('kendoWindow').close();
                    //}
                    //if (parent.$('#clientCommWindow').data('kendoWindow')) {
                    //    parent.$('#clientCommWindow').data('kendoWindow').close();
                    //}
                    //if (parent.$('#clientHistoryWindow').data('kendoWindow')) {
                    //    parent.$('#clientHistoryWindow').data('kendoWindow').close();
                    //}
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
            //parent.$(".k-window-content").each(function (i, win) {
            parent.$('#window').data("kendoWindow").close();
            //});
            //$(".k-window .k-window-content").each(function (index, element) {
            //    setTimeout(function () {
            //        $(element).data('kendoWindow').close();
            //    }, 1000);
            //});
            //if (parent.$('#clientFinanWindow').data('kendoWindow')) {
            //    parent.$('#clientFinanWindow').data('kendoWindow').close();
            //}
            //if (parent.$('#clientEditWindow').data('kendoWindow')) {
            //    parent.$('#clientEditWindow').data('kendoWindow').close();
            //}
            //if (parent.$('#clientAssistWindow').data('kendoWindow')) {
            //    parent.$('#clientAssistWindow').data('kendoWindow').close();
            //}
            //if (parent.$('#clientCommWindow').data('kendoWindow')) {
            //    parent.$('#clientCommWindow').data('kendoWindow').close();
            //}
            //if (parent.$('#clientHistoryWindow').data('kendoWindow')) {
            //    parent.$('#clientHistoryWindow').data('kendoWindow').close();
            //}
        }
    });
    
});