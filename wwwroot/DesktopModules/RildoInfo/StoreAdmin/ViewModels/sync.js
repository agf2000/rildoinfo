
$(function () {

    $.valHooks.textarea = {
        get: function (elem) {
            return elem.value.replace(/\r?\n/g, "\r\n");
        }
    };

    my.viewModel();

    var buttons = '<ul class="inline">';
    buttons += '<li><a href="' + addressURL + '" class="btn btn-primary btn-medium" title="Endereço"><i class="fa fa-book fa-lg"></i></a></li>';
    buttons += '<li><a href="' + registrationURL + '" class="btn btn-primary btn-medium" title="Cadastro"><i class="fa fa-edit fa-lg"></i></a></li>';
    buttons += '<li><a href="' + payCondsURL + '" class="btn btn-primary btn-medium" title="Formas e Condições de Pagamento"><i class="fa fa-credit-card fa-lg"></i></a></li>';
    buttons += '<li><a href="' + estimateURL + '" class="btn btn-primary btn-medium" title="Orçamento"><i class="fa fa-usd fa-lg"></i></a></li>';
    buttons += '<li><a href="' + smtpURL + '" class="btn btn-primary btn-medium" title="SMTP"><i class="fa fa-envelope-o fa-lg"></i></a></li>';
    buttons += '<li><a href="' + statusesManagerURL + '" class="btn btn-primary btn-medium" title="Status"><i class="fa fa-check-circle fa-lg"></i></a></li>';
    buttons += '<li><a href="' + websiteManagerURL + '" class="btn btn-primary btn-medium" title="Website"><i class="fa fa-bookmark fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + templatesManagerURL + '" class="btn btn-primary btn-medium" title="Templates"><i class="fa fa-puzzle-piece fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + davReturnsURL + '" class="btn btn-primary btn-medium" title="DAVs"><i class="fa fa-briefcase fa-lg"></i></a></li>';
    buttons += '</ul>';
    $('#buttons').html(buttons);

    $('#moduleTitleSkinObject').html(moduleTitle);

    $('.icon-info-sign').popover({
        placement: 'top',
        trigger: 'hover'
    });

    $('#viewPriceCheckBox').bootstrapSwitch();

    $('#btnUpdateSync').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = [
            {
                'PortalId': portalID,
                'SettingName': 'syncEnabled',
                'SettingValue': $('#enableSync').is(':checked')
            }
        ];

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/UpdateAppSetting',
            data: JSON.stringify(params)
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Informações atualizadas.',
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

    $('#btnReturn').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#newGroup').data("kendoWindow").close();
    });

    $('#viewPriceCheckBox').bootstrapSwitch('setState', JSON.parse(syncEnabled.toLowerCase()));

});
