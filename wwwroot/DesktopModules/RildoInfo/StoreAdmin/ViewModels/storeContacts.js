
$(function () {

    my.viewModel();

    $('#moduleTitleSkinObject').html(moduleTitle);

    var buttons = '<ul class="inline">';
    buttons += '<li><a href="' + registrationURL + '" class="btn btn-primary btn-medium" title="Cadastro"><i class="fa fa-edit fa-lg"></i></a></li>';
    buttons += '<li><a href="' + payCondsURL + '" class="btn btn-primary btn-medium" title="Formas e Condições de Pagamento"><i class="fa fa-credit-card fa-lg"></i></a></li>';
    buttons += '<li><a href="' + syncURL + '" class="btn btn-primary btn-medium" title="Orçamento"><i class="fa fa-refresh fa-lg"></i></a></li>';
    buttons += '<li><a href="' + estimateURL + '" class="btn btn-primary btn-medium" title="Orçamento"><i class="fa fa-usd fa-lg"></i></a></li>';
    buttons += '<li><a href="' + smtpURL + '" class="btn btn-primary btn-medium" title="SMTP"><i class="fa fa-envelope-o fa-lg"></i></a></li>';
    buttons += '<li><a href="' + statusesManagerURL + '" class="btn btn-primary btn-medium" title="Status"><i class="fa fa-check-circle fa-lg"></i></a></li>';
    buttons += '<li><a href="' + websiteManagerURL + '" class="btn btn-primary btn-medium" title="Website"><i class="fa fa-bookmark fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + templatesManagerURL + '" class="btn btn-primary btn-medium" title="Templates"><i class="fa fa-puzzle-piece fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + davReturnsURL + '" class="btn btn-primary btn-medium" title="DAVs"><i class="fa fa-briefcase fa-lg"></i></a></li>';
    buttons += '</ul>';
    $('#buttons').html(buttons);

    $('#replyEmailTextBox').attr({ 'placeholder': $('#emailTextBox').val() });

    $('.icon-info-sign').popover({
        placement: 'top',
        trigger: 'hover'
    });

    my.vm.loadRegions();
    my.vm.loadCountries();

    $('#postalCodeTextBox').inputmask("99.999-999");

    $('#btnUpdateContacts').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = [
            {
                'PortalId': portalID,
                'SettingName': 'storePostalCode',
                'SettingValue': my.vm.postalCode()
            },
            {
                'PortalId': portalID,
                'SettingName': 'storeAddress',
                'SettingValue': my.vm.street()
            },
            {
                'PortalId': portalID,
                'SettingName': 'storeUnit',
                'SettingValue': my.vm.unit()
            },
            {
                'PortalId': portalID,
                'SettingName': 'storeComplement',
                'SettingValue': my.vm.complement()
            },
            {
                'PortalId': portalID,
                'SettingName': 'storeDistrict',
                'SettingValue': my.vm.district()
            },
            {
                'PortalId': portalID,
                'SettingName': 'storeCity',
                'SettingValue': my.vm.city()
            },
            {
                'PortalId': portalID,
                'SettingName': 'storeRegion',
                'SettingValue': my.vm.selectedRegion()
            },
            {
                'PortalId': portalID,
                'SettingName': 'storeCountry',
                'SettingValue': my.vm.selectedCountry()
            },
            {
                'PortalId': portalID,
                'SettingName': 'storeEmail',
                'SettingValue': my.vm.email()
            },
            {
                'PortalId': portalID,
                'SettingName': 'storeReplyEmail',
                'SettingValue': my.vm.replyEmail()
            },
            {
                'PortalId': portalID,
                'SettingName': 'storePhone1',
                'SettingValue': my.vm.phone1()
            },
            {
                'PortalId': portalID,
                'SettingName': 'storePhone2',
                'SettingValue': my.vm.phone2()
            }
        ];

        $.ajax({
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            url: '/desktopmodules/riw/api/store/updateAppSetting',
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
    
});
