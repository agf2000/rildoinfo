
$(function () {

    $.valHooks.textarea = {
        get: function (elem) {
            return elem.value.replace(/\r?\n/g, "\r\n");
        }
    };

    $('#optionsTabs').kendoTabStrip({
        animation: {
            // fade-out current tab over 1000 milliseconds
            close: {
                effects: "fadeOut"
            },
            // fade-in new tab over 500 milliseconds
            open: {
                effects: "fadeIn"
            }
        }
    });

    $('#subjectTextBox').val(estimateSubject);
    $('#emailBody').val(toMarkdown(estimateBody));

    $('.icon-exclamation-sign').popover({
        delay: {
            show: 500,
            hide: 1000
        },
        placement: 'top',
        trigger: 'hover'
    });

    my.files = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/desktopmodules/riw/api/store/GetPortalFiles?portalId=' + portalID
            }
        }
    });

    $('#rootImages').kendoDropDownList({
        dataSource: my.files,
        dataTextField: 'FileName',
        dataValueField: 'FileId',
        dataBound: function (e) {
            this.text(orLogoFile);
        },
        select: function (e) {
            var dataItem = this.dataItem(e.item.index());
            $('#logoImage').attr({ 'src': '/portals/' + portalID + '/' + dataItem.RelativePath });
        }
    });

    if (orLogoFile !== '') {
        $('#logoImage').attr({ 'src': orLogoURL });
    }

    $('.btnReturn').click(function (e) {
        e.preventDefault();

        document.location.href = returnURL;
    });

    $('#btnUpdateOr').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = [
            {
                'id': tModuleID,
                'name': 'RIW_OR_Slogan',
                'value': $('#mainLineTextBox').val()
            },
            {
                'id': tModuleID,
                'name': 'RIW_OR_Logo',
                'value': $('#rootImages').data('kendoDropDownList').text()
            }
        ];

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateTabModuleSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configurações atualizadas.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                setTimeout(function () {
                    document.location.href = returnURL;
                }, 3000);
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

    $('#btnUpdate').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = [
            {
                'id': tModuleID,
                'name': 'RIW_EstimateEmailSubject',
                'value': $('#subjectTextBox').val()
            },
            {
                'id': tModuleID,
                'name': 'RIW_EstimateEmailBody',
                'value': $('#emailBody').val()
            }
        ];

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateTabModuleSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Configurações atualizadas.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                setTimeout(function () {
                    document.location.href = returnURL;
                }, 3000);
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
