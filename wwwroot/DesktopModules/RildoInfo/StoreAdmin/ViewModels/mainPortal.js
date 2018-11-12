
$(function () {

    my.viewModel();

    $('#moduleTitleSkinObject').html(moduleTitle);

    var buttons = '<ul class="inline">';
    buttons += '<li><a href="' + addressURL + '" class="btn btn-primary btn-medium" title="Endereço"><i class="fa fa-book fa-lg"></i></a></li>';
    buttons += '<li><a href="' + registrationURL + '" class="btn btn-primary btn-medium" title="Cadastro"><i class="fa fa-edit fa-lg"></i></a></li>';
    buttons += '<li><a href="' + payCondsURL + '" class="btn btn-primary btn-medium" title="Formas e Condições de Pagamento"><i class="fa fa-credit-card fa-lg"></i></a></li>';
    buttons += '<li><a href="' + syncURL + '" class="btn btn-primary btn-medium" title="Orçamento"><i class="fa fa-refresh fa-lg"></i></a></li>';
    buttons += '<li><a href="' + estimateURL + '" class="btn btn-primary btn-medium" title="Orçamento"><i class="fa fa-usd fa-lg"></i></a></li>';
    buttons += '<li><a href="' + smtpURL + '" class="btn btn-primary btn-medium" title="SMTP"><i class="fa fa-envelope-o fa-lg"></i></a></li>';
    buttons += '<li><a href="' + statusesManagerURL + '" class="btn btn-primary btn-medium" title="Status"><i class="fa fa-check-circle fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + templatesManagerURL + '" class="btn btn-primary btn-medium" title="Templates"><i class="fa fa-puzzle-piece fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + davReturnsURL + '" class="btn btn-primary btn-medium" title="DAVs"><i class="fa fa-briefcase fa-lg"></i></a></li>';
    buttons += '</ul>';
    $('#buttons').html(buttons);

    $('.icon-info-sign').popover({
        placement: 'top',
        trigger: 'hover'
    });

    my.vm.loadCountries();
    my.vm.loadRegions();

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
            this.text(logoFile);
        },
        select: function (e) {
            var dataItem = this.dataItem(e.item.index());
            $('#logoImage').attr({ 'src': '/portals/' + portalID + '/' + dataItem.RelativePath });
        }
    });

    if (logoFile !== '') {
        if (logoURL.toLowerCase().indexOf('portals') !== -1) {
            $('#logoImage').attr({ 'src': logoURL });
        } else {
            $('#logoImage').attr({ 'src': '/portals/' + portalID + '/' + logoURL });
        }
    }

    $('#rootBWImages').kendoDropDownList({
        dataSource: my.files,
        dataTextField: 'FileName',
        dataValueField: 'FileId',
        dataBound: function (e) {
            this.text(bwLogoFile);
        },
        select: function (e) {
            var dataItem = this.dataItem(e.item.index());
            $('#logoBWImage').attr({ 'src': '/portals/' + portalID + '/' + dataItem.RelativePath });
        }
    });

    if (bwLogoFile !== '') {
        if (bwLogoURL.toLowerCase().indexOf('portals') !== -1) {
            $('#logoBWImage').attr({ 'src': bwLogoURL });
        } else {
            $('#logoBWImage').attr({ 'src': '/portals/' + portalID + '/' + bwLogoURL });
        }
    }

    $('#btnUpdatePortal').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();
        $('#mainPortal').wrap('<form id="temp_form_id" />');
        if (!validator.validate('#temp_form_id')) {
            $.pnotify({
                title: 'Atenção!',
                text: 'Favor preenchar todos os campos obrigatórios.',
                type: 'warning',
                icon: 'fa fa-warning fa-lg',
                addclass: "stack-bottomright",
                stack: my.stack_bottomright
            });
            $('#mainPortal').unwrap();
        } else {

            var $this = $(this);
            $this.button('loading');

            var params = [
                {
                    'PortalId': portalID,
                    'SettingName': 'bW_Logo',
                    'SettingValue': $('#rootBWImages').data('kendoDropDownList').text()
                }
            ];

            $.ajax({
                type: 'PUT',
                contentType: 'application/json; charset=utf-8',
                url: '/desktopmodules/riw/api/store/updateAppSetting',
                data: JSON.stringify(params)
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    var portalInfo = {
                        PortalId: portalID,
                        LogoFile: $('#rootImages').data('kendoDropDownList').text(),
                        PortalName: my.vm.siteName(),
                        Description: my.vm.description(),
                        Keywords: my.vm.keywords()
                    };

                    $.ajax({
                        type: 'PUT',
                        url: '/desktopmodules/riw/api/store/updatePortal',
                        data: portalInfo
                    }).done(function (data) {
                        if (data.Result.indexOf("success") !== -1) {
                            // ok
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
                    });
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

        }
    });

    var validator = new ValidationUtility();

    parent.$('#imgWindow').kendoWindow({
        position: {
            top: 10,
            left: 10
        },
        visible: false
    });

    $('.fileManager').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $("#filesWindow").append("<div id='window'></div>");
        var sContent = fileManagerURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer',
            kendoWindow = $('#window').kendoWindow({
                actions: ["Maximize", "Close"],
                title: 'Gerenciador de Arquivos',
                modal: true,
                width: '90%',
                height: '80%',
                content: sContent,
                close: function (e) {
                    $("html, body").css("overflow", "");
                    my.files.read();
                },
                open: function () {
                    $("html, body").css("overflow", "hidden");
                    $('#window').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
                },
                deactivate: function () {
                    this.destroy();
                }
            });

        kendoWindow.data("kendoWindow").center().open();

        $.ajax({
            url: sContent, success: function (data) {
                kendoWindow.data("kendoWindow").refresh();
            }
        });
    });
    
    $('.markdown-editor').css({ 'min-width': '90%', 'height': '80px', 'margin-bottom': '5px' }).attr({ 'cols': '30', 'rows': '2' });

    $('.markdown-editor').autogrow();
    $('.markdown-editor').css('overflow', 'hidden').autogrow();

});
