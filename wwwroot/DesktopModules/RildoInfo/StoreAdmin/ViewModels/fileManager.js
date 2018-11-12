
$(function () {

    my.PortalFile = function () {
        this.fileId = ko.observable();
        this.fileName = ko.observable();
        this.fileSize = ko.observable();
        this.contenType = ko.observable();
        this.extension = ko.observable();
        this.relativePath = ko.observable();
        this.width = ko.observable();
        this.height = ko.observable();
    };

    // knockout js view model
    my.vm = function () {
        // this is knockout view model
        var self = this;

        self.sortJsonName = function (field, reverse, primer) {
            var key = primer ? function (x) { return primer(x[field]); } : function (x) { return x[field]; };
            return function (b, a) {
                var A = key(a), B = key(b);
                return ((A < B) ? -1 : (A > B) ? +1 : 0) * [-1, 1][+!!reverse];
            };
        },

        self.selectedFolderPath = ko.observable('Images/'),
        self.portalFiles = ko.observableArray([]),
        self.loadPortalFiles = function () {
            $.ajax({
                url: '/desktopmodules/riw/api/store/GetPortalFiles?portalId=' + portalID + '&folder=' + self.selectedFolderPath()
            }).done(function (data) {
                if (data) {
                    if (data.length > 0) {
                        self.portalFiles.removeAll();
                        $.each(data, function (i, f) {
                            self.portalFiles.push(new my.PortalFile()
                                .fileId(f.FileId)
                                .fileName(f.FileName)
                                .contenType(f.ContentType)
                                .extension(f.Extension)
                                .fileSize('Tamanho: ' + my.size_format(kendo.parseInt(f.FileSize)))
                                .height(kendo.parseInt(f.Height))
                                .relativePath(f.Extension.toLowerCase() === 'jpg' || f.Extension.toLowerCase() === 'png' || f.Extension.toLowerCase() === 'gif' || f.Extension.toLowerCase() === 'bmp' ? '/portals/' + portalID + '/' + f.RelativePath : '/desktopmodules/rildoinfo/webapi/content/images/spacer.gif')
                                .width(kendo.parseInt(f.Width)));
                        });
                    } else {
                        self.portalFiles.removeAll();
                        setTimeout(function () {
                            $.post('/desktopmodules/riw/api/store/SyncPortalFolders?portalId=' + portalID + '&folder=', function (data) { });
                        });
                    }
                }
            });
        },

        self.fileSearch = ko.observable(""),
        self.filteredPortalFiles = ko.dependentObservable(function () {
            var filter = this.fileSearch().toLowerCase();
            if (!filter) {
                return self.portalFiles();
            } else {
                return ko.utils.arrayFilter(self.portalFiles(), function (item) {
                    return ko.utils.arrayFilter([item.fileName().toLowerCase()], function (str) {
                        return str.indexOf(filter) !== -1;
                    }).length > 0;
                });
            }
        }, self),

        self.selectedFileId = ko.observable();

        // make view models available for apps
        return {
            fileSearch: fileSearch,
            filteredPortalFiles: filteredPortalFiles,
            selectedFolderPath: selectedFolderPath,
            portalFiles: portalFiles,
            loadPortalFiles: loadPortalFiles,
            selectedFileId: selectedFileId
        };

    }();

    ko.applyBindings(my.vm);

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
                //$().toastmessage('showErrorToast', data.Msg);
                $.pnotify({
                    title: 'Erro!',
                    text: data.Msg,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
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
                                    //$().toastmessage('showErrorToast', data.Msg);
                                    $.pnotify({
                                        title: 'Erro!',
                                        text: data.Msg,
                                        type: 'error',
                                        icon: 'fa fa-times-circle fa-lg',
                                        addclass: "stack-bottomright",
                                        stack: my.stack_bottomright
                                    });
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
                        //$().toastmessage('showErrorToast', data.Msg);
                        $.pnotify({
                            title: 'Erro!',
                            text: data.Msg,
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
            kendoConfirmWindow.data("kendoWindow").close();
        }).end();
    };

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
                if (allowedExtensions.indexOf(value.extension.toLowerCase()) === -1) {
                    e.preventDefault();
                    //$().toastmessage('showWarningToast', 'É permitido enviar somente arquivos com formato jpg e png.');
                    $.pnotify({
                        title: 'Atenção!',
                        text: 'É permitido enviar somente arquivos com formato jpg e png.',
                        type: 'warning',
                        icon: 'fa fa-warning fa-lg',
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
                .relativePath(_fileInfo.Extension === 'jpg' || _fileInfo.Extension === 'png' || _fileInfo.Extension === 'gif' ? '/portals/' + portalID + '/' + _fileInfo.RelativePath : '/desktopmodules/rildoinfo/webapi/content/images/spacer.gif')
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
                text: 'Não foi possível o envio do arquivo.',
                type: 'error',
                icon: 'fa fa-times-circle fa-lg',
                addclass: "stack-bottomright",
                stack: my.stack_bottomright
            });
        }
    });

    $('.btnReturn').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#window').data('kendoWindow').close();
    });

});
