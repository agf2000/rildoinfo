
$(function () {

    my.viewModel();

    $('.icon-info-sign').popover({
        placement: function (tip, element) {
            var $element, above, actualHeight, actualWidth, below, boundBottom, boundLeft, boundRight, boundTop, elementAbove, elementBelow, elementLeft, elementRight, isWithinBounds, left, pos, right;
            isWithinBounds = function (elementPosition) {
                return boundTop < elementPosition.top && boundLeft < elementPosition.left && boundRight > (elementPosition.left + actualWidth) && boundBottom > (elementPosition.top + actualHeight);
            };
            $element = $(element);
            pos = $.extend({}, $element.offset(), {
                width: element.offsetWidth,
                height: element.offsetHeight
            });
            actualWidth = 350;
            actualHeight = 200;
            boundTop = $(document).scrollTop();
            boundLeft = $(document).scrollLeft();
            boundRight = boundLeft + $(window).width();
            boundBottom = boundTop + $(window).height();
            elementAbove = {
                top: pos.top - actualHeight,
                left: pos.left + pos.width / 2 - actualWidth / 2
            };
            elementBelow = {
                top: pos.top + pos.height,
                left: pos.left + pos.width / 2 - actualWidth / 2
            };
            elementLeft = {
                top: pos.top + pos.height / 2 - actualHeight / 2,
                left: pos.left - actualWidth
            };
            elementRight = {
                top: pos.top + pos.height / 2 - actualHeight / 2,
                left: pos.left + pos.width
            };
            above = isWithinBounds(elementAbove);
            below = isWithinBounds(elementBelow);
            left = isWithinBounds(elementLeft);
            right = isWithinBounds(elementRight);
            if (above) {
                return "top";
            } else {
                if (below) {
                    return "bottom";
                } else {
                    if (left) {
                        return "left";
                    } else {
                        if (right) {
                            return "right";
                        } else {
                            return "right";
                        }
                    }
                }
            }
        },
        delay: {
            show: 100,
            hide: 1000
        },
        trigger: 'hover'
    });

    $('#connectionCheckBox').bootstrapSwitch();

    $('#moduleTitleSkinObject').html(moduleTitle);

    var buttons = '<ul class="inline">';
    buttons += '<li><a href="' + addressURL + '" class="btn btn-primary btn-medium" title="Endereço"><i class="fa fa-book fa-lg"></i></a></li>';
    buttons += '<li><a href="' + registrationURL + '" class="btn btn-primary btn-medium" title="Cadastro"><i class="fa fa-edit fa-lg"></i></a></li>';
    buttons += '<li><a href="' + payCondsURL + '" class="btn btn-primary btn-medium" title="Formas e Condições de Pagamento"><i class="fa fa-credit-card fa-lg"></i></a></li>';
    buttons += '<li><a href="' + syncURL + '" class="btn btn-primary btn-medium" title="Orçamento"><i class="fa fa-refresh fa-lg"></i></a></li>';
    buttons += '<li><a href="' + estimateURL + '" class="btn btn-primary btn-medium" title="Orçamento"><i class="fa fa-usd fa-lg"></i></a></li>';
    buttons += '<li><a href="' + statusesManagerURL + '" class="btn btn-primary btn-medium" title="Status"><i class="fa fa-check-circle fa-lg"></i></a></li>';
    buttons += '<li><a href="' + websiteManagerURL + '" class="btn btn-primary btn-medium" title="Website"><i class="fa fa-bookmark fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + templatesManagerURL + '" class="btn btn-primary btn-medium" title="Templates"><i class="fa fa-puzzle-piece fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + davReturnsURL + '" class="btn btn-primary btn-medium" title="DAVs"><i class="fa fa-briefcase fa-lg"></i></a></li>';
    buttons += '</ul>';
    $('#buttons').html(buttons);

    var params = {
        PortalId: portalID,
        CreatedByUser: userID,
        CreatedOnDate: moment().format()
    };

    $('#btnUpdateSmtp').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();
        $('#smtp').wrap('<form id="temp_form_id" />');
        if (!validator.validate('#temp_form_id')) {
            $.pnotify({
                title: 'Atenção!',
                text: 'Favor preenchar todos os campos obrigatórios.',
                type: 'warning',
                icon: 'fa fa-warning fa-lg',
                addclass: "stack-bottomright",
                stack: my.stack_bottomright
            });
            $('#smtp').unwrap();
        } else {

            var $this = $(this);
            $this.button('loading');

            var params = [
                {
                    'PortalId': portalID,
                    'SettingName': 'smtpServer',
                    'SettingValue': my.vm.server()
                },
                {
                    'PortalId': portalID,
                    'SettingName': 'smtpPort',
                    'SettingValue': my.vm.port()
                },
                {
                    'PortalId': portalID,
                    'SettingName': 'smtpLogin',
                    'SettingValue': my.vm.login()
                },
                {
                    'PortalId': portalID,
                    'SettingName': 'smtpPassword',
                    'SettingValue': my.vm.password()
                },
                {
                    'PortalId': portalID,
                    'SettingName': 'smtpConnection',
                    'SettingValue': my.vm.connection()
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
                        text: 'Configuração atualizada.',
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

        }
    });

    var validator = new ValidationUtility();

});
