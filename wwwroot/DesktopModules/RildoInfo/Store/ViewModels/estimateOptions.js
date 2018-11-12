
$(function () {

    $.valHooks.textarea = {
        get: function (elem) {
            return elem.value.replace(/\r?\n/g, "\r\n");
        }
    };

    $('#sendNewEstimateToTextBox').val(_sendNewEstimateTo);
    $('#subjectTextBox').val(_estimateSubject);
    $('#emailBody').val(toMarkdown(_estimateBody));

    $('.icon-exclamation-sign').popover({
        delay: {
            show: 500,
            hide: 1000
        },
        placement: 'top',
        trigger: 'hover'
    });

    $('#btnReturn').click(function (e) {
        e.preventDefault();

        document.location.href = _returnURL;
    });

    $('#btnUpdate').click(function (e) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = [
            {
                'id': _tModuleID,
                'name': 'RIW_EstimateEmailSubject',
                'value': $('#subjectTextBox').val()
            },
            {
                'id': _tModuleID,
                'name': 'RIW_EstimateEmailBody',
                'value': $('#emailBody').val()
            },
            {
                'id': _tModuleID,
                'name': 'RIW_SendNewEstimateTo',
                'value': $('#sendNewEstimateToTextBox').val()
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
                    document.location.href = _returnURL;
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
