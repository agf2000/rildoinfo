
$(function () {

    my.viewModel();

    $('#tabstrip').kendoTabStrip({
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

    $('#btnSaveDefinition').click(function (e) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = [
            {
                'id': _tModuleID,
                'name': 'RIS_EstimateEmailSubject',
                'value': my.vm.emailSubject()
            },
            {
                'id': _tModuleID,
                'name': 'RIS_EstimateEmailBody',
                'value': my.vm.emailBody()
            },
            {
                'id': _tModuleID,
                'name': 'RIS_EstimateIntroMsg',
                'value': my.vm.estimateIntroMsg()
            },
            {
                'id': _tModuleID,
                'name': 'RIS_VisitorIntroMsg',
                'value': my.vm.visitorIntroMsg()
            }
        ];

        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/contacts/updateTabModuleSetting',
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
