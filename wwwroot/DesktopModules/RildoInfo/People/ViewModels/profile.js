
$(function () {

    my.userName = userName;
    my.fileName = '';
    var status = $(".status");

    //my.viewModel();

    $('.icon-exclamation-sign').popover({
        delay: {
            show: 500,
            hide: 100
        },
        placement: 'top',
        trigger: 'hover'
    });

    my.checkUserEmail = function (username) {
                
        $.ajax({
            async: false,
            url: '/desktopmodules/riw/api/people/validateUser?vTerm=' + username
        }).done(function (data) {
            if (data > 0) {
                return true;
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
        
        return false;
    };

    $('#personMenu').show();
    $('#personMenu').jqxMenu({
        width: '120',
        mode: 'vertical'
    });

    $('#editProfile div.pull-left:first-child()').css({ 'width': '14%' });

    $("#bDayTextBox").kendoDatePicker({
        format: "m",
        parseFormats: ["m"]
    });

    $('#bDayTextBox').prop('placeholder', 'ex.: ' + kendo.toString(new Date(), 'm'));

    $('#btnUpdateLogin').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            // portalId As Integer, ByVal personId As Integer, ByVal cUId As Integer, ByVal newUsername As String, ByVal subject As String, ByVal body As String, ByVal mUId As Integer, ByVal md
            PortalId: portalID,
            PersonId: -1,
            UserId: userID,
            Email: $('#loginTextBox').val(),
            Subject: amplify.store.sessionStorage('passwordSubject').replace('[WEBSITE]', siteName),
            MessageBody: amplify.store.sessionStorage('passwordMessage').replace('[CLIENTE]', $('#firstNameTextBox').val() + ' ' + $('#lastNameTextBox').val()).replace('[WEBSITE1]', siteName).replace('[HTTPURL]', siteURL).replace('[URL]', siteURL).replace('[WEBSITE2]', siteName),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };
        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/people/UpdatePersonUserLogin',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $('#usernameCheck').html('');
                //$('#btnUpdateLogin').html('Alterar Login').attr({ 'disabled': true });
                //$().toastmessage('showSuccessToast', 'Login alterado com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Login alterado.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                my.userName = $('#loginTextBox').val();
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

    //$('#newPasswordTextBox').keyup(function () {
    //    $('#passMetter').text(my.checkPassStrength($(this).val()));
    //});

    $('#confirmPasswordTextBox').on('keyup', function (e) {

        if ($('#newPasswordTextBox').val() !== $('#confirmPasswordTextBox').val()) {
            $('#passConfirm').removeClass().addClass('alert alert-error').html('Senhas N&#227;o Coincidem!');
        } else {
            $('#passConfirm').removeClass().addClass('alert alert-success').html('Senhas Coincidem!');
        }

        return false;

    });

    $('#newPasswordTextBox').on('keyup', function (e) {

        if ($('#newPasswordTextBox').val() === '') {
            $('#passMetter').removeClass().html('');

            return false;
        }

        // Must have capital letter, numbers and lowercase letters
        var strongRegex = new RegExp("^(?=.{8,})(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*\\W).*$", "g");

        // Must have either capitals and lowercase letters or lowercase and numbers
        var mediumRegex = new RegExp("^(?=.{7,})(((?=.*[A-Z])(?=.*[a-z]))|((?=.*[A-Z])(?=.*[0-9]))|((?=.*[a-z])(?=.*[0-9]))).*$", "g");

        // Must be at least 6 characters long
        var okRegex = new RegExp("(?=.{7,}).*", "g");

        if (okRegex.test($(this).val()) === false) {
            // If ok regex doesn't match the password
            $('#passMetter').removeClass().addClass('alert alert-error').html('Senha deve cont&#234;r no m&#237;nimo 7 caracteres.');

        } else if (strongRegex.test($(this).val())) {
            // If reg ex matches strong password
            $('#passMetter').removeClass().addClass('alert alert-success').html('Boa Senha!');
        } else if (mediumRegex.test($(this).val())) {
            // If medium password matches the reg ex
            $('#passMetter').removeClass().addClass('alert alert-info').html('Senha Ok! Mas sugerimos incluir letras mai&#250;sculas e min&#250sculas, n&#250;meros e pontua&#231;&#245;es!');
        } else {
            // If password is ok
            $('#passMetter').removeClass().addClass('alert alert-warning').html('Senha Ok! Recomendamos incluir letras mai&#250;sculas e min&#250;sculas, n&#250;meros e pontua&#231;&#245;es.');
        }

        return true;
    });

    $('#btnUpdatePassword').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        $('#editLogin').wrap('<form id="temp_form_id" />');
        if (!validator.validate('#temp_form_id')) {
            $.pnotify({
                title: 'Aten&#231;&#227;o!',
                text: 'Favor preenchar todos os campos obrigat&#243;rios.',
                type: 'warning',
                icon: 'fa fa-warning fa-lg',
                addclass: "stack-bottomright",
                stack: my.stack_bottomright
            });
            $('#editLogin').unwrap();
        } else {
            $('#editLogin').unwrap();

            $this.button('loading');

            var params = {
                PortalId: portalID,
                UserId: userID,
                CurrentPassword: $('#currentPasswordTextBox').val(),
                NewPassword: $('#newPasswordTextBox').val(),
                Subject: amplify.store.sessionStorage('passwordSubject').replace('[WEBSITE]', siteName),
                MessageBody: amplify.store.sessionStorage('passwordMessage').replace('[CLIENTE]', $('#firstNameTextBox').val() + ' ' + $('#lastNameTextBox').val()).replace('[WEBSITE1]', siteName).replace('[HTTPURL]', siteURL).replace('[URL]', siteURL).replace('[WEBSITE2]', siteName)
            };
            $.ajax({
                type: 'PUT',
                url: '/desktopmodules/riw/api/people/UpdateUserPassword',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    //$('#btnUpdatePassword').attr({ 'disabled': true });
                    $('#editLogin input').val('');
                    $('#passMetter').text('');
                    $('#passConfirm').text('');
                    //$().toastmessage('showSuccessToast', 'Senha alterada com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Senha alterada.',
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
        }
    });

    $('#btnRandomPassword').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var params = {
            PortalId: portalID,
            UserId: userID,
            Subject: amplify.store.sessionStorage('passwordSubject').replace('[WEBSITE]', siteName),
            MessageBody: amplify.store.sessionStorage('passwordMessage').replace('[CLIENTE]', $('#firstNameTextBox').val() + ' ' + $('#lastNameTextBox').val()).replace('[WEBSITE1]', siteName).replace('[HTTPURL]', siteURL).replace('[URL]', siteURL).replace('[WEBSITE2]', siteName)
        };

        var $dialog = $('<div></div>')
            .html('<div class="confirmDialog">Tem Certeza? Uma nova senha ser&#225; gerada e enviada para o email cadastrado!</div>')
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
                        "class": 'dnnPrimaryAction',
                        click: function () {

                            $this.button('loading');

                            $.ajax({
                                type: 'PUT',
                                url: '/desktopmodules/riw/api/people/GenerateUserPassword',
                                data: params
                            }).done(function (data) {
                                if (data.Result.indexOf("success") !== -1) {
                                    //$().toastmessage('showSuccessToast', 'Uma nova senha gerada e enviada com sucesso.');
                                    $.pnotify({
                                        title: 'Sucesso!',
                                        text: 'Uma nova senha gerada e trnasmitida para o email cadastrado.',
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

                            $dialog.dialog('close');
                            $dialog.dialog('destroy');
                        }
                    },
                    'cancel': {
                        html: 'N&#227;o',
                        //priority: 'secondary',
                        "class": 'dnnSecondaryAction',
                        click: function () {
                            $dialog.dialog('close');
                            $dialog.dialog('destroy');
                        }
                    }
                }
            });

        $dialog.dialog('open');
    });

    $('#btnCheckLogin').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        if ($('#loginTextBox').val() !== my.userName) {
            if ($('#loginTextBox').val() !== '') {
                if (my.checkUserEmail($('#loginTextBox').val())) {
                    $('#usernameCheck').html('<span class="NormalRed"><i title="Login existente!" class="fa fa-times-circle fa-lg" /> Login existente!</span>');
                    $('#btnUpdateLogin').attr({ 'disabled': true });
                }
                else {
                    $('#usernameCheck').html('<span class="NormalRed"><i title="Este email pode ser usado como login!" class="fa fa-check fa-lg" /> Este login pode ser usado como login!</span>');
                    $('#btnUpdateLogin').attr({ 'disabled': false });
                }
            }
        } else {
            $('#usernameCheck').html('<span class="NormalRed"><i title="Login existente!" class="fa fa-times-circle fa-lg" /> Login existente!</span>');
            $('#btnUpdateLogin').attr({ 'disabled': true });
            setTimeout(function () {
                $('#usernameCheck').html('');
            }, 3000);
        }

        $this.button('reset');

    });

    $('#photos').kendoUpload({
        async: {
            saveUrl: "/desktopmodules/riw/api/people/SaveUserPhoto",
            removeUrl: "remove",
            multiple: false
        },
        //showFileList: false,
        localization: {
            cancel: 'Cancelar',
            dropFilesHere: 'Arraste o arquivo aqui para envia-lo',
            remove: 'Remover',
            select: 'Selecionar',
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
                if (value.extension.toUpperCase() !== '.JPG' && value.extension.toUpperCase() !== '.PNG') {
                    e.preventDefault();
                    //$().toastmessage('showWarningToast', 'É permitido enviar somente arquivos com formato jpg e png.');
                    $.pnotify({
                        title: 'Aten&#231;&#227;o!',
                        text: '&#201; permitido enviar somente arquivos com formato jpg e png.',
                        type: 'erro',
                        icon: 'fa fa-times-circle fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                }
            });
        },
        upload: function (e) {
            e.data = {
                PortalId: portalID,
                UserId: userID
                //maxWidth: 0,
                //maxHeight: 0
            };
        },
        success: function (e) {
            $.each(e.files, function (index, value) {
                my.fileName = e.response.fileName;
                $('#aImg').html('<img alt="" src="/portals/' + portalID + '/' + e.response.filePath + '?maxwidth=120" />');
                //$('#btnRemoveAvatar').html('&times; Remover');
                $("#divPhotos").hide();
                $('#divAvatar').show();
                parent.$('#userPhotoSkinObject').attr({ 'src': '/portals/' + portalID + '/' + e.response.filePath + '?maxwidth=80' });
                //$().toastmessage('showSuccessToast', 'Arquivo enviado com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Arquivo enviado.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$(".k-widget.k-upload").find("ul").remove();
            });
        },
        remove: function (e) {
        },
        error: function (e) {
            //$().toastmessage('showErrorToast', 'N&#227;o foi possível o envio do arquivo.');
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

    $('#btnRemoveAvatar').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var $dialog = $('<div></div>')
            .html('<div class="confirmDialog">Tem Certeza?</div>')
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
                        "class": 'dnnPrimaryAction',
                        click: function () {

                            $this.button('loading');

                            var params = {
                                PortalId: portalID,
                                UserId: userID,
                                FileName: my.fileName
                            };

                            $.ajax({
                                type: 'PUT',
                                url: '/desktopmodules/riw/api/people/RemoveUserPhoto',
                                data: params
                            }).done(function (data) {
                                if (data.Result.indexOf("success") !== -1) {
                                    $('#aImg').val('');
                                    //$('#files').data('kendoUpload').enable();
                                    $("#divPhotos").show();
                                    $('#divAvatar').hide();
                                    parent.$('#userPhotoSkinObject').attr({ 'src': '/images/no_avatar.gif' });
                                    //$().toastmessage('showSuccessToast', 'Arquivo excluido com sucesso.');
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
                                    //$().toastmessage('showErrorToast', data.Msg);
                                }
                            }).fail(function (jqXHR, textStatus) {
                                console.log(jqXHR.responseText);
                            }).always(function () {
                                $this.button('reset');
                            });

                            $dialog.dialog('close');
                            $dialog.dialog('destroy');
                        }
                    },
                    'cancel': {
                        html: 'N&#227;o',
                        //priority: 'secondary',
                        "class": 'dnnSecondaryAction',
                        click: function () {
                            $dialog.dialog('close');
                            $dialog.dialog('destroy');
                        }
                    }
                }
            });

        $dialog.dialog('open');
    });

    $('#btnUpdateUser').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        $('#personForm').wrap('<form id="temp_form_id" />');
        if (!validator.validate('#temp_form_id')) {
            $.pnotify({
                title: 'Aten&#231;&#227;o!',
                text: 'Favor preenchar todos os campos obrigat&#243;rios.',
                type: 'warning',
                icon: 'fa fa-warning fa-lg',
                addclass: "stack-bottomright",
                stack: my.stack_bottomright
            });
            $('#personForm').unwrap();
        } else {

            $this.button('loading');

            var params = {
                UserId: userID,
                FirstName: $('#firstNameTextBox').val().trim(),
                LastName: $('#lastNameTextBox').val().trim(),
                Telephone: $('#phoneTextBox').val().replace(/\D/g, ''),
                Cell: $('#cellTextBox').val().replace(/\D/g, ''),
                Fax: $('#faxTextBox').val().replace(/\D/g, ''),
                //Email: my.vm.email().trim(),
                //Website: my.vm.website().trim(),
                //Comments: my.vm.comments().trim(),
                Biography: $('#bioTextArea').text().trim(),
                PostalCode: $('#postalCodeTextBox').val().replace(/\D/g, ''),
                Street: $('#streetTextBox').val().trim(),
                Unit: $('#unitTextBox').val().trim(),
                Complement: $('#complementTextBox').val().trim(),
                District: $('#districtTextBox').val().trim(),
                City: $('#cityTextBox').val().trim(),
                Country: $('#ddlCountries').data('kendoDropDownList').text()
            };

            if ($('#regionTextBox').val().length > 0) {
                params.Region = $('#regionTextBox').val();
            } else {
                params.Region = $('#ddlRegions').data('kendoDropDownList').text();
            }

            $.ajax({
                type: 'POST',
                url: '/desktopmodules/riw/api/people/UpdateUser',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    //$().toastmessage('showSuccessToast', 'Cadastro gravado com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Cadastro salvo.',
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
        }
    });

    my.initializer = function () {
        switch (my.getQuerystring('sel', my.getParameterByName('sel'))) {
            case 2: // Edit Login

                $('#editLogin').delay(100).show();

                break;
            case 3:

                //if (_userID === 1) {
                //    $('#btnUpdateLogin').hide();
                //    $('#btnUpdatePassword').hide();
                //    $('#btnRandomPassword').hide();
                //}

                $('#userPhoto .pull-left').css({ 'width': '50%' });
                $('#userPhoto .pull-right').css({ 'width': '50%' });

                $('#userPhoto').delay(100).show();

                break;
            default:

                $("#postalCodeTextBox").inputmask("99-999-999");
                $("#phoneTextBox").inputmask("(99) 9999-9999");
                $("#cellTextBox").inputmask("(99) 9999-9999");
                $("#faxTextBox").inputmask("(99) 9999-9999");
                $("#cpfTextBox").inputmask("999.999.999-99");
                $("#einTextBox").inputmask("99.999.999/9999-99");

                $('#personForm').show();
                $('#firstNameTextBox').focus();
        }
    };

    $('input.enterastab, select.enterastab, textarea.enterastab').on('keydown', function (e) {
        if (e.keyCode === 13) {
            var focusable = $('input,a,select,textarea').filter(':visible');
            if (this.name === 'streetTextBox') {
                focusable.eq(focusable.index(this) + 1).focus().select();
            } else {
                focusable.eq(focusable.index(this) + 2).focus().select();
            }
            return false;
        }
    });
    
    my.initializer();

    $('#ddlCountries').kendoDropDownList({
        dataSource: new kendo.data.DataSource({
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/lists/countries'
                }
            },
            serverFiltering: true
        }),
        optionLabel: "Selecionar...",
        dataTextField: 'Text',
        dataValueField: 'Value',
        value: 'BR',
        dataBound: function () {
            if (country.length > 0) {
                this.text(country);
            }
        }
    });

    $('#ddlRegions').kendoDropDownList({
        autoBind: false,
        dataSource: new kendo.data.DataSource({
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/lists/regions'
                },
                parameterMap: function (data, type) {
                    if (type == "read") {
                        return {
                            code: $('#ddlCountries').data('kendoDropDownList').value()
                        }
                    }
                }
            },
            serverFiltering: true
        }),
        optionLabel: "Selecionar...",
        dataTextField: 'Text',
        dataValueField: 'Value',
        value: 'AC',
        cascadeFrom: 'ddlCountries',
        dataBound: function () {
            if (this.dataSource.view().length > 0) {
                $('#divRegionsDDL').show();
                $('#divRegionTextBox').hide();
                if (region.length > 0) {
                    this.text(region)
                } else {
                    this.value(null);
                }
            } else {
                $('#divRegionsDDL').hide();
                $('#divRegionTextBox').show();
                $('#regionTextBox').val(region);
            }
        }
    });

    if (my.getQuerystring('sel', my.getParameterByName('sel'))) {
        //$('#personMenu ul li').removeClass('jqx-menu-item-selected');
        var sel = my.getQuerystring('sel', my.getParameterByName('sel'));
        $('#personMenu ul li[id=' + sel + ']').addClass('jqx-menu-item-selected');
    } else {
        $('#personMenu ul li:first-child').addClass('jqx-menu-item-selected');
    }

    $('#firstNameTextBox').val(firstName);
    $('#lastNameTextBox').val(lastName);
    $('#phoneTextBox').val(telephone);
    $('#cellTextBox').val(cell);
    $('#faxTextBox').val(fax);
    $('#bioTextArea').val(biography);
    $('#postalCodeTextBox').val(postalCode);
    $('#streetTextBox').val(street);
    $('#unitTextBox').val(unit);
    $('#complementTextBox').val(complement);
    $('#districtTextBox').val(district);
    $('#cityTextBox').val(city);

    var _date = kendo.parseDate(lastPasswordChangeDate);
    $('#lastPasswordChangedDate').text(kendo.toString(_date, 'F')).css({ 'text-transform': 'capitalize' });
    $('#loginTextBox').val(my.userName);
    //my.vm.username(data.Username);
    //my.vm.originalUsername(_userName);

    $('#moduleTitleSkinObject').html('Perfil: ' + $('#firstNameTextBox').val() + ' ' + $('#lastNameTextBox').val() + ' ID: ' + userID);

    amplify.store.sessionStorage('avatar', avatar);

    if (amplify.store.sessionStorage('avatar').length > 0) {
        // $('#files').data('kendoUpload').disabled();
        $("#divPhotos").hide();
        my.fileName = amplify.store.sessionStorage('avatar').replace(userFolder, "");
        var photoUrl = ('/portals/' + portalID + '/' + amplify.store.sessionStorage('avatar')).trim();
        $('#aImg').html('<img alt="" src="' + photoUrl + '?maxwidth=100" />');
        //$('#btnRemoveAvatar').html('&times; Remover');
        $('#divAvatar').show();
    } else {
        //$('#btnRemoveAvatar').html('');
        $('#divAvatar').hide();
    }

    $('#personMenu').on('itemclick', function (event) {
        document.location.hash = 'sel/' + event.args.id;
        $('#personMenu ul li').removeClass('jqx-menu-item-selected');
        $('#personMenu ul li[id=' + event.args.id + ']').addClass('jqx-menu-item-selected');
        //$('#actionsMenu ul li').removeClass('jqx-menu-item-selected');
        my.initializer();
        switch (event.args.id) {
            case '1':

                $('#personForm').hide();
                //$('#editMsg').fadeOut();
                $('#userPhoto').fadeOut();
                $('#editLogin').fadeOut();
                $('#personForm').delay(300).fadeIn();

                break;
            case '2':

                $('#editLogin').hide();
                //$('#editMsg').fadeOut();
                $('#userPhoto').fadeOut();
                $('#personForm').fadeOut();
                $('#editLogin').delay(300).fadeIn();

                break;
            case '3':

                $('#userPhoto').hide();
                //$('#editMsg').fadeOut();
                $('#editLogin').fadeOut();
                $('#personForm').fadeOut();
                $('#userPhoto').delay(300).fadeIn();

                break;
        }
    });

    var validator = new ValidationUtility();

    $.fn.dnnFileInput = function (options) {

    };

    $('.btnReturn').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        parent.$('#window').data("kendoWindow").close();
    });

});
