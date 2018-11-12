
$(function () {

    my.userId = my.getQuerystring('userId', my.getParameterByName('userId'));
    var status = $(".status");

    my.viewModel();
    
    $('.icon-exclamation-sign').popover({
        delay: {
            show: 500,
            hide: 100
        },
        placement: 'top',
        trigger: 'hover'
    });

    my.vm.loadGroups();

    //my.checkUserEmail = function (username) {
    //    var result = false;
    //    $.ajax({
    //        async: false,
    //        url: '/desktopmodules/riw/api/people/validateUser?vTerm=' + username
    //    }).done(function (data) {
    //        if (data > 0) {
    //            result = true;
    //        }
    //    }).fail(function (jqXHR, textStatus) {
    //        console.log(jqXHR.responseText);
    //    });
    //    return result;
    //};

    my.getUser = function () {
        if (my.userId !== 0) {
            $.ajax({
                url: '/desktopmodules/riw/api/people/getUser?portalId=' + portalID + '&userId=' + my.userId,
                async: false
            }).done(function (data) {
                if (data.length > 0) {
                    $.each(data, function (i, user) {
                        $('#btnUpdateUser').html('<i class="fa fa-check"></i>&nbsp; Atualizar');
                        $('#btnUpdateUser').show();

                        $('#liLogin').hide();

                        my.vm.displayName(user.DisplayName);
                        my.vm.firstName(user.FirstName);
                        my.vm.lastName(user.LastName);
                        my.vm.telephone(user.ProfileProperties.Telephone);
                        my.vm.cell(user.ProfileProperties.Cell);
                        my.vm.fax(user.ProfileProperties.Fax);
                        my.vm.originalEmail(user.Email);
                        if (user.Email) {
                            if (user.Email.length > 2) {
                                my.vm.email(user.Email);
                            }
                        }
                        my.vm.comments(user.ProfileProperties.Comments);
                        //my.vm.bio(user.ProfileProperties.Biography);
                        //my.vm.locked(user.Locked);

                        $('#postalCodeTextBox').val(user.ProfileProperties.PostalCode);
                        $('#streetTextBox').val(user.ProfileProperties.Street);
                        $('#unitTextBox').val(user.ProfileProperties.Unit);
                        $('#complementTextBox').val(user.ProfileProperties.IM);
                        $('#districtTextBox').val(user.ProfileProperties.LinkedIn);
                        $('#cityTextBox').val(user.ProfileProperties.City);
                        my.vm.selectedRegion(user.ProfileProperties.Region);
                        my.vm.selectedCountry(user.ProfileProperties.Country);

                        var _date = kendo.parseDate(user.LastPasswordChangeDate);
                        $('#lastPasswordChangedDate').text(kendo.toString(_date, 'F')).css({ 'text-transform': 'capitalize' });
                        $('#loginTextBox').val(user.UserName);
                        my.vm.userName(data.UserName);
                        my.vm.originalUserName(user.UserName);

                        if (user.IsDeleted) {
                            $('#btnRestoreUser').show();
                            if (!user.Locked) {
                                $('#btnRemoveUser').show();
                            } else {
                                $('#btnDeleteUser').show();
                            }
                        } else {
                            $('#btnRestoreUser').hide();
                            $('#btnDeleteUser').show();
                            if (user.Locked) {
                                $('#btnDeleteUser').show();
                            } else {
                                $('#btnRemoveUser').show();
                            }
                        }
                    });

                    if (authorized < 3) {
                        $('#btnRemoveUser').hide();
                    }

                    $('#personMenu').show();
                    $('#personMenu').jqxMenu({
                        width: '120',
                        mode: 'vertical'
                    });

                    $('#editUser .pull-left').css({ 'width': '14%' });
                    $('#editUser .pull-right').css({ 'width': '85%' });
                }
            });
        } else {
            $('#editUser .pull-right').css({ 'width': '100%' });
            $('#personForm .pull-left').css({ 'width': '50%' });
            $('#personForm .pull-right').css({ 'width': '50%' });
        }
    };
    my.getUser();

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
            PortalId: portalID,
            PersonId: -1,
            UserId: my.userId,
            UserName: $('#loginTextBox').val(),
            Email: my.vm.email(),
            //oldUsername: my.vm.originalUsername(),
            Subject: my.vm.passwordSubject(),
            MessageBody: my.vm.passwordBody(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };
        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/people/UpdateUserLogin',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
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
            $('#userNameCheck').html('');
            setTimeout(function () {
                $('#btnUpdateLogin').attr({ 'disabled': true });
            }, 100);
        });

    });

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
            $('#passMetter').removeClass().addClass('alert alert-warning').html('Senha Ok! Recomendariamos incluir letras mai&#250;sculas e min&#250;sculas, n&#250;meros e pontua&#231;&#245;es.');
        }

        return true;
    });

    $('#btnUpdatePassword').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        $('#collapsePassword').wrap('<form id="temp_form_id" />');
        if (!validator.validate('#temp_form_id')) {
            $.pnotify({
                title: 'Aten&#231;&#227;o!',
                text: 'Favor preenchar todos os campos obrigat&#243;rios.',
                type: 'warning',
                icon: 'fa fa-warning fa-lg',
                addclass: "stack-bottomright",
                stack: my.stack_bottomright
            });
            $('#collapsePassword').unwrap();
        } else {
            $('#collapsePassword').unwrap();

            $this.button('loading');

            var params = {
                PortalId: portalID,
                UserId: my.userId,
                CurrentPassword: $('#currentPasswordTextBox').val().length > 0 ? $('#currentPasswordTextBox').val() : '',
                NewPassword: $('#newPasswordTextBox').val(),
                Subject: my.vm.passwordSubject(),
                MessageBody: my.vm.passwordBody(),
                SendPassword: true
            };
            $.ajax({
                type: 'PUT',
                url: '/desktopmodules/riw/api/people/UpdateUserPassword',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    //$('#btnUpdatePassword').attr({ 'disabled': true });
                    $('#currentPasswordTextBox').val('');
                    $('#newPasswordTextBox').val('');
                    $('#confirmPasswordTextBox').val('');
                    $('#passMetter').html('');
                    $('#passConfirm').html('');
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
            UserId: my.userId,
            Subject: my.vm.passwordSubject(),
            MessageBody: my.vm.passwordBody()
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
                            $dialog.dialog('close');
                            $dialog.dialog('destroy');

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

        if ($('#loginTextBox').val() !== my.vm.originalUserName()) {
            $.ajax({
                url: '/desktopmodules/riw/api/people/validateUserName?vTerm=' + $('#loginTextBox').val()
            }).done(function (data) {
                if (data > 0) {
                    $('#userNameCheck').html('<span class="NormalRed"><i title="Login existente!" class="fa fa-times-circle fa-lg" /> Login existente!</span>');
                    $('#btnUpdateLogin').attr({ 'disabled': true });
                } else {
                    $('#userNameCheck').html('<span class="text-success"><i title="Este login est&#225; liberado!" class="fa fa-check fa-lg" /> Este login est&#225; liberado!</span>');
                    $('#btnUpdateLogin').attr({ 'disabled': false });
                }
            }).fail(function (jqXHR, textStatus) {
                console.log(jqXHR.responseText);
            });
        } else {
            $('#userNameCheck').html('<span class="NormalRed"><i title="Login existente!" class="fa fa-times-circle fa-lg" /> Login existente!</span>');
            $('#btnUpdateLogin').attr({ 'disabled': true });
            setTimeout(function () {
                $('#userameCheck').html('');
            }, 3000);
        }

        $this.button('reset');

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
                            $dialog.dialog('close');
                            $dialog.dialog('destroy');

                            var params = {
                                PortalId: portalID,
                                UserId: my.userId,
                                FileName: my.vm.userAvatar()
                            };

                            $.ajax({
                                type: 'PUT',
                                url: '/desktopmodules/riw/api/people/RemoveUserPhoto',
                                data: params
                            }).done(function (data) {
                                if (data.Result.indexOf("success") !== -1) {
                                    $('#aImg').html('');
                                    //$('#files').data('kendoUpload').enable();
                                    $(".k-widget.k-upload").show();
                                    //$('#btnRemoveAvatar').html('');
                                    $('#btnRemoveAvatar').hide();
                                    if (userID === my.userId) parent.$('#userPhotoSkinObject').attr({ 'src': '/images/no_avatar.gif' });
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

    $('#btnAddUserGroup').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            PortalId: portalID,
            UserId: my.userId,
            RoleId: $('#kddlRoles').data('kendoDropDownList').value(),
            Cancel: false
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/store/addUserRole',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {

                var grid = $('#userGroupsGrid').data('kendoGrid');
                grid.dataSource.add({
                    RoleId: data.RoleId,
                    RoleName: $('#kddlRoles').data('kendoDropDownList').text(),
                    EffectiveDate: $('#dpStartDate').data('kendoDatePicker').value(),
                    ExpiryDate: $('#dpEndDate').data('kendoDatePicker').value()
                });

                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Colaborador(a) adcionado(a) ao grupo.',
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

    $('#emailTextBox').on('paste', function () {
        $('#btnCheckEmail').show();
    });

    $('#emailTextBox').keyup(function () {
        if (this.value.length) {
            $('#btnCheckEmail').show();
            $('#btnUpdateUser').attr({ 'disabled': true, 'title': 'Desativado' });
        } else {
            $('#btnCheckEmail').hide();
            $('#btnUpdateUser').attr({ 'disabled': false, 'title': 'Clique para adicionar um novo cadastro' });
            $('#btnCheckEmail').button('reset');
        }
    });

    $('#btnCheckEmail').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        if (my.vm.email() !== my.vm.originalEmail()) {
            if (my.vm.email() !== '' && my.isValidEmailAddress(my.vm.email())) {
                $.ajax({
                    url: '/desktopmodules/riw/api/people/validateUserEmail?vTerm=' + my.vm.email()
                }).done(function (data) {
                    if (data > 0) {
                        //$('#btnCheckEmail').attr({ 'disabled': false });
                        $('#btnUpdateUser').attr({ 'disabled': true, 'title': 'Desativado' });
                        $.dnnAlert({
                            title: 'Aviso',
                            okText: 'Ok',
                            text: 'Email existente!'
                        });
                        //$('#btnCheckEmail').html('<i class="fa fa-check"></i>').attr({ 'title': 'Verificar!' }).fadeIn(1000);   
                        $this.button('reset');
                    } else {
                        $('#btnCheckEmail').html('<img alt="Ok" src="/images/success-icn.png" style="vertical-align: middle;" />').attr({ 'title': 'Email pode ser validado!' }).attr({ 'disabled': false });
                        $('#btnUpdateUser').attr({ 'disabled': false, 'title': 'Clique para adicionar um novo cadastro' });
                    }
                }).fail(function (jqXHR, textStatus) {
                    console.log(jqXHR.responseText);
                });
                //if (my.checkUserEmail($('#emailTextBox').val())) {
                //    $('#btnCheckEmail').attr({ 'disabled': false });
                //    $('#btnUpdateUser').attr({ 'disabled': true, 'title': 'Desativado' });
                //    $.dnnAlert({
                //        title: 'Aviso',
                //        okText: 'Ok',
                //        text: 'Email existente!'
                //    });
                //    $('#btnCheckEmail').html('<i class="fa fa-check"></i>').attr({ 'title': 'Verificar!' }).fadeIn(1000);
                //} else {
                //    $('#btnCheckEmail').html('<img alt="Ok" src="/images/success-icn.png" style="vertical-align: middle;" />').attr({ 'title': 'Email pode ser validado!' }).attr({ 'disabled': false });
                //    $('#btnUpdateUser').attr({ 'disabled': false, 'title': 'Clique para adicionar um novo cadastro' });
                //    my.vm.reqEmail(true);
                //}
            } else {
                //if (self.clientId() > 0)
                //    $('#emailTextBox').attr({ 'class': 'required', 'required': true });
                //$('#emailCheck').html('');
                $('#btnCheckEmail').attr({ 'disabled': false }).hide();
                $('#btnUpdateUser').attr({ 'disabled': false, 'title': '' });
            }
        } else {
            //$('#emailCheck').html('');
            $('#btnCheckEmail').attr({ 'disabled': false }).hide();
            $('#btnUpdateUser').attr({ 'disabled': false, 'title': '' });
        }
    });

    $('#btnUpdateUser').click(function (e) {
        e.preventDefault();
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

            var $this = $(this);
            $this.button('loading');

            var params = {
                UserId: my.userId,
                PortalId: portalID,
                FirstName: my.vm.firstName(),
                LastName: my.vm.lastName(),
                Telephone: my.vm.telephone() ? my.vm.telephone().replace(/\D/g, '') : '',
                Cell: my.vm.cell() ? my.vm.cell().replace(/\D/g, '') : '',
                Fax: my.vm.fax() ? my.vm.fax().replace(/\D/g, '') : '',
                Email: my.vm.email(),
                //Website: my.vm.website(),
                Groups: JSON.stringify(my.vm.selectedGroup()),
                Comments: my.vm.comments(),
                //Biography: my.vm.bio(),
                PostalCode: my.vm.postalCode() ? my.vm.postalCode().replace(/\D/g, '') : '',
                Street: my.vm.street(),
                Unit: my.vm.unit(),
                Complement: my.vm.complement(),
                District: my.vm.district(),
                City: my.vm.city(),
                Region: my.vm.selectedRegion(),
                Country: my.vm.selectedCountry()
            };

            $.ajax({
                type: 'POST',
                url: '/desktopmodules/riw/api/people/UpdateUser',
                data: params
            }).done(function (data) {
                if (data.UserId) {
                    my.userId = data.UserId;
                    document.location.hash = 'userId/' + data.UserId + '/sel/1';
                    //$('#liEmail').hide();
                    $('#liGroups').hide();
                    $('#personMenu').show();
                    $('#personMenu').jqxMenu({
                        width: '120',
                        mode: 'vertical'
                    });
                    $('#personMenu ul li[id=1]').addClass('jqx-menu-item-selected');
                    $('#editUser .pull-left').css({ 'width': '14%' });
                    $('#editUser .pull-right').css({ 'width': '86%' });
                    $('#personForm .pull-left').css({ 'width': '48%' });
                    $('#personForm .pull-right').css({ 'width': '52%' });
                    $('#btnDeleteUser').show();
                    $('#btnRemoveUser').show();
                    $('#btnCheckEmail').hide();
                    //$('#liAddress').hide();

                    if (parent.$('#window').data('kendoWindow')) {
                        parent.$('#window').data('kendoWindow').title(params.FirstName + ' ' + params.LastName + ' (ID: ' + data.UserId + ')');
                    }

                    $('#lastPasswordChangedDate').text(kendo.toString(new Date(), 'F')).css({ 'text-transform': 'capitalize' });
                    $('#loginTextBox').val(my.vm.userName());
                    my.vm.originalUserName(my.vm.userName());

                    $this.html('<i class="fa fa-check"></i>&nbsp; Atualizar');

                    my.roles.read();
                    //$('#userGroupsGrid').data('kendoGrid').dataSource.read();
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
                }
            }).fail(function (jqXHR, textStatus) {
                console.log(jqXHR.responseText);
            }).always(function () {
                $this.button('reset');
            });
        }
    });

    $('#btnDeleteUser').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var params = {
            PortalId: portalID,
            UserId: my.userId
        };

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

                            $.ajax({
                                type: 'PUT',
                                url: '/desktopmodules/riw/api/people/deleteUser',
                                data: params
                            }).done(function (data) {
                                if (data.Result.indexOf("success") !== -1) {
                                    parent.my.peopleData.read();
                                    //$().toastmessage('showSuccessToast', 'Conta desativada com sucesso.');
                                    $.pnotify({
                                        title: 'Sucesso!',
                                        text: 'Conta desativada.',
                                        type: 'success',
                                        icon: 'fa fa-check fa-lg',
                                        addclass: "stack-bottomright",
                                        stack: my.stack_bottomright
                                    });
                                    //setTimeout(function () {
                                    //    $('.btnReturn').click();
                                    //}, 2000);
                                    if (parent.$('#window').data('kendoWindow')) {
                                        parent.$('#window').data('kendoWindow').close();
                                    }
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

    $('#btnRestoreUser').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            PortalId: portalID,
            UserId: my.userId
        };

        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/people/restoreUser',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                parent.my.peopleData.read();
                $('#btnRestoreUser').hide();
                $('#btnDeleteUser').show();
                if (my.vm.locked) {
                    $('#btnDeleteUser').show();
                } else {
                    if (authorized > 2) {
                        $('#btnRemoveUser').show();
                    }
                }
                //$().toastmessage('showSuccessToast', 'Conta ativada com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Conta ativada.',
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

    $('#btnRemoveUser').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var $dialog = $('<div></div>')
            .html('<div class="confirmDialog">Tem Certeza? Todas as informa&#231;&#245;es referente &#224; esta conta ser&#227;o excluidas. Esta a&#231;&#227;o n&#227;o poder&#225; ser revertida.</div>')
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
                                type: 'DELETE',
                                url: '/desktopmodules/riw/api/people/removeUser?portalId=' + portalID + '&userId=' + my.userId
                            }).done(function (data) {
                                if (data.Result.indexOf("success") !== -1) {
                                    parent.my.peopleData.read();
                                    //$().toastmessage('showSuccessToast', 'Conta excluida com sucesso.');
                                    $.pnotify({
                                        title: 'Sucesso!',
                                        text: 'Conta excluida.',
                                        type: 'success',
                                        icon: 'fa fa-check fa-lg',
                                        addclass: "stack-bottomright",
                                        stack: my.stack_bottomright
                                    });
                                    //setTimeout(function () {
                                    //    $('.btnReturn').click();
                                    //}, 2000);
                                    if (parent.$('#window').data('kendoWindow')) {
                                        parent.$('#window').data('kendoWindow').close();
                                    }
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

    my.initializer = function () {
        switch (my.getParameterByName('sel')) {
            case 2: // Edit Login

                $('#editLogin').delay(100).show();

                break;
            case 3:

                //if (my.userId === 1) {
                //    $('#btnUpdateLogin').hide();
                //    $('#btnUpdatePassword').hide();
                //    $('#btnRandomPassword').hide();
                //}

                $('#userPhoto .pull-left').css({ 'width': '50%' });
                $('#userPhoto .pull-right').css({ 'width': '50%' });

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
                            UserId: my.userId
                            //maxWidth: 0,
                            //maxHeight: 0
                        };
                    },
                    success: function (e) {
                        //$('#btnRemoveAvatar').off('click');
                        $.each(e.files, function (index, value) {
                            my.vm.userAvatar(e.response.fileName);
                            $('#aImg').html('<img alt="" src="/portals/' + portalID + '/' + e.response.filePath + '?maxwidth=120" />');
                            //$('#btnRemoveAvatar').html('&times; Remover');
                            $('#btnRemoveAvatar').show();
                            $(".k-widget.k-upload").hide();
                            //$().toastmessage('showSuccessToast', 'Arquivo enviado com sucesso.');
                            if (userID === my.userId) parent.$('#userPhotoSkinObject').attr({ 'src': '/portals/' + portalID + '/' + e.response.filePath + '?maxwidth=80' });
                            $.pnotify({
                                title: 'Sucesso!',
                                text: 'Avatar configurado.',
                                type: 'success',
                                icon: 'fa fa-check fa-lg',
                                addclass: "stack-bottomright",
                                stack: my.stack_bottomright
                            });
                            $(".k-widget.k-upload").find("ul").remove();
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

                $.ajax({
                    async: false,
                    url: '/desktopmodules/riw/api/people/GetUserPhoto?portalId=' + portalID + '&userId=' + my.userId
                }).done(function (data) {
                    if (data.Result.indexOf("success") !== -1) {
                        // $('#files').data('kendoUpload').disabled();
                        $(".k-widget.k-upload").hide();
                        my.vm.userAvatar(data.fileName);
                        var photoUrl = ('/portals/' + portalID + '/' + data.filePath);
                        $('#aImg').html('<img alt="" src="' + photoUrl + '?maxwidth=100" />');
                        //$('#btnRemoveAvatar').html('&times; Remover');
                        $('#btnRemoveAvatar').show();
                    } else {
                        //$('#btnRemoveAvatar').html('');
                        $('#btnRemoveAvatar').hide();
                    }
                });

                $('#userPhoto').delay(100).show();

                break;
            case 5:

                my.roles.read();

                break;
            default:

                my.vm.loadCountries();

                $("#postalCodeTextBox").inputmask("99-999-999");
                $("#phoneTextBox").inputmask("(99) 9999-9999");
                $("#cellTextBox").inputmask("(99) 9999-9999");
                $("#faxTextBox").inputmask("(99) 9999-9999");
                $('#personForm .pull-left').css({ 'width': '48%' });
                $('#personForm .pull-right').css({ 'width': '52%' });

                $('#personForm').show();
                $('#emailTextBox').focus();
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
    my.vm.loadRegions();

    $('#dpStartDate').kendoDatePicker();
    $('#dpEndDate').kendoDatePicker();

    $('#kddlRoles').kendoDropDownList({
        autoBind: false,
        optionLabel: "Todos os Grupos",
        dataTextField: 'RoleName',
        dataValueField: 'RoleId',
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/store/GetPublicRoles?portalId=' + portalID
                }
            }
        },
        dataBound: function () {
            if (this.dataSource.view().length > 0) {
                var dropDown = this;
                $.each(dropDown.dataSource.data(), function (i, group) {
                    if (group) {
                        switch (true) {
                            case (group.RoleName.toLowerCase().indexOf('admin') !== -1):
                                var itemToRemove = dropDown.dataSource.at(i);
                                dropDown.dataSource.remove(itemToRemove);
                                break;
                            case (group.RoleName.toLowerCase().indexOf('users') !== -1):
                                itemToRemove = dropDown.dataSource.at(i);
                                dropDown.dataSource.remove(itemToRemove);
                            default:
                                if (group.RoleName.toLowerCase().indexOf('todos') !== -1) {
                                    itemToRemove = dropDown.dataSource.at(i);
                                    dropDown.dataSource.remove(itemToRemove);
                                }
                        }
                    }
                });
            }
        }
    });

    my.roles = new kendo.data.DataSource({
        transport: {
                read: {
                    url: '/desktopmodules/riw/api/store/getRolesByUser'
                },
                parameterMap: function (data, type) {
                    return {
                        portalId: portalID,
                        userId: my.userId
                    };
                }
            },
        schema: {
            model: {
                id: 'RoleId',
                fields: {
                    RoleId: { type: 'number' },
                    EffectiveDate: { type: "date", format: "{0:g}" },
                    ExpiryDate: { type: "date", format: "{0:g}" }
                }
            }
        }
    });

    $('#userGroupsGrid').kendoGrid({
        dataSource: my.roles,
        columns: [
            {
                title: 'Grupo',
                field: 'RoleName'
            },
            {
                title: 'Data Efetiva',
                field: 'EffectiveDate',
                type: "date",
                format: "{0:g}",
                width: 130
            },
            {
                title: 'Data de Validade',
                field: 'ExpiryDate',
                type: "date",
                format: "{0:g}",
                width: 130
            },
            {
                command: [
                    {
                        name: "Exclude",
                        text: '',
                        imageClass: "fa fa-times",
                        click: function (e) {
                            e.preventDefault();

                            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                            if (dataItem) {

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

                                                    $dialog.dialog('close');
                                                    $dialog.dialog('destroy');

                                                    var params = {
                                                        PortalId: portalID,
                                                        UserId: my.userId,
                                                        RoleId: dataItem.RoleId,
                                                        Cancel: true
                                                    };

                                                    $.ajax({
                                                        type: 'POST',
                                                        url: '/desktopmodules/riw/api/store/updateUserRole',
                                                        data: params
                                                    }).done(function (data) {
                                                        if (data.Result.indexOf("success") !== -1) {
                                                            $('#userGroupsGrid').data('kendoGrid').dataSource.remove(dataItem);
                                                            $.pnotify({
                                                                title: 'Sucesso!',
                                                                text: 'Colaborador(a) removido(a) do grupo.',
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
                            }
                        }
                    }
                ],
                title: 'Remover',
                width: 65,
                attributes: { class: 'text-center' }
            }
        ],
        dataBound: function (e) {
            if (this.dataSource.view().length > 0) {
                var grid = this;
                $.each(grid.dataSource.data(), function (i, group) {
                    if (group) {
                        if (group.EffectiveDate) {
                            if (group.EffectiveDate.toString().indexOf('Mon Jan 01 1') !== -1) {
                                group.set('EffectiveDate', null);
                            }
                        }
                        if (group.ExpiryDate) {
                            if (group.ExpiryDate.toString().indexOf('Mon Jan 01 1') !== -1) {
                                group.set('ExpiryDate', null);
                            }
                        }
                        if (group.RoleName) {
                            if (group.RoleName.toLowerCase().indexOf('users') !== -1) {
                                grid.dataSource.remove(group)
                            }
                        }
                    }
                });
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

    $('#personMenu').on('itemclick', function (event) {
        document.location.hash = 'userId/' + my.userId + '/sel/' + event.args.id;
        $('#personMenu ul li').removeClass('jqx-menu-item-selected');
        $('#personMenu ul li[id=' + event.args.id + ']').addClass('jqx-menu-item-selected');
        //$('#actionsMenu ul li').removeClass('jqx-menu-item-selected');
        my.initializer();
        switch (event.args.id) {
            case '2':

                $('#editLogin').hide();
                $('#userPhoto').fadeOut();
                $('#userGroups').fadeOut();
                $('#personForm').fadeOut();
                $('#editLogin').delay(300).fadeIn();

                break;
            case '3':

                $('#userPhoto').hide();
                $('#userGroups').fadeOut();
                $('#editLogin').fadeOut();
                $('#personForm').fadeOut();
                $('#userPhoto').delay(300).fadeIn();

                break;
            case '4':

                $('#userGroups').hide();
                $('#userPhoto').fadeOut();
                $('#editLogin').fadeOut();
                $('#personForm').fadeOut();
                $('#userGroups').delay(300).fadeIn();

                break;
            default:

                $('#personForm').hide();
                $('#userPhoto').fadeOut();
                $('#userGroups').fadeOut();
                $('#editLogin').fadeOut();
                $('#personForm').delay(300).fadeIn();
        }
    });

    var validator = new ValidationUtility();

    $('#enableLoginChk').click(function () {
        if ($(this).is(":checked")) {
            my.vm.reqEmail(true);
            $('#emailTextBox').attr({ 'required': true });
        } else {
            my.vm.reqEmail(false);
            $('#emailTextBox').removeAttr('required');
            $('#emailTextBox').removeAttr('data-role');
        }
    });

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
