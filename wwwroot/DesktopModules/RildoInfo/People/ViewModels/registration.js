
$(function () {

    my.returnUrl = decodeURIComponent(my.getQuerystring('returnurl'));
    if (!my.returnUrl) {
        my.returnUrl = returnURL;
    }
    var status = $(".status");
    my.regType = my.getParameterByName('regType');

    my.viewModel();

    $('.icon-exclamation-sign').popover({
        placement: 'top',
        trigger: 'hover'
    });

    my.checkPersonEmail = function (username) {
        var result = false;
        $.ajax({
            async: false,
            url: '/desktopmodules/riw/api/people/ValidatePerson?email=' + username
        }).done(function (data) {
            //if (data.Result.indexOf("success") !== -1) {
            if (data > 0) {
                result = true;
            }
            //} else {
            //    $.pnotify({
            //        title: 'Erro!',
            //        text: data.Result,
            //        type: 'error',
            //        icon: 'fa fa-times-circle fa-lg',
            //        addclass: "stack-bottomright",
            //        stack: my.stack_bottomright
            //    });
            //    //$().toastmessage('showErrorToast', data.Msg);
            //}
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
        return result;
    };

    $('#confirmPasswordTextBox').on('keyup', function (e) {

        if ($('#passwordTextBox').val() != $('#confirmPasswordTextBox').val()) {
            $('#passConfirm').removeClass().addClass('alert alert-error').html('Senhas N&#227;o Coincidem!');            
        } else {
            $('#passConfirm').removeClass().addClass('alert alert-success').html('Senhas Coincidem!');
        }

        return false;

    });

    $('#passwordTextBox').on('keyup', function (e) {
 
        if ($('#passwordTextBox').val() == '')
        {
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
            $('#passMetter').removeClass().addClass('alert alert-info').html('Senha Ok! Mas ainda pode melhora-la se incluir letras mai&#250;sculas e min&#250sculas, n&#250;meros e pontua&#231;&#245;es!');
        } else {
            // If password is ok
            $('#passMetter').removeClass().addClass('alert alert-warning').html('Senha Ok! Recomendamos incluir letras mai&#250;sculas e min&#250;sculas, n&#250;meros e pontua&#231;&#245;es.');
        }
        
        return true;
    });

    //$('#btnCheckLogin').click(function (e) {
    //    e.preventDefault();

    //    $(this).html('<i class="fa fa-spinner fa-spin"></i>Um momento...').attr({ 'disabled': true });

    //    if ($('#loginTextBox').val() !== my.vm.originalUsername()) {
    //        if ($('#loginTextBox').val() !== '') {
    //            if (my.isValidEmailAddress($('#loginTextBox').val())) {
    //                $('#usernameCheck').html('<img alt="" src="/desktopmodules/rildoinfo/webapi/Content/kendo/2013.2.918/Default/loading.gif" style="margin-left: 5px; vertical-align: middle;" />');
    //                if (my.checkPersonEmail($('#loginTextBox').val())) {
    //                    $('#usernameCheck').html('<img alt="Email existente!" src="/images/red-error_16px.gif" style="margin-left: 5px; vertical-align: middle;" />').attr({ 'title': 'Email existente!' });
    //                    $('#btnUpdateLogin').attr({ 'disabled': true });
    //                }
    //                else {
    //                    $('#usernameCheck').html('<img alt="Ok" src="/images/success-icn.png" style="margin-left: 5px; vertical-align: middle;" />').attr({ 'title': 'Este email pode ser usado!' });
    //                    $('#btnUpdateLogin').attr({ 'disabled': false });
    //                }
    //            } else {
    //                $('#usernameCheck').html('<img alt="Email existente!" src="/images/red-error_16px.gif" style="margin-left: 5px; vertical-align: middle;" />').attr({ 'title': 'Email com formato inválido!' });
    //                $('#btnUpdateLogin').attr({ 'disabled': true });
    //                setTimeout(function () {
    //                    $('#usernameCheck').html('');
    //                }, 2000);
    //            }
    //        } else {
    //            $('#usernameCheck').html('<img alt="Email existente!" src="/images/red-error_16px.gif" style="margin-left: 5px; vertical-align: middle;" />').attr({ 'title': 'Email com formato inválido!' });
    //            $('#btnUpdateLogin').attr({ 'disabled': true });
    //            setTimeout(function () {
    //                $('#usernameCheck').html('');
    //            }, 2000);
    //        }
    //    } else {
    //        $('#usernameCheck').html('<img alt="Email existente!" src="/images/red-error_16px.gif" style="margin-left: 5px; vertical-align: middle;" />').attr({ 'title': 'Email com formato inválido!' });
    //        $('#btnUpdateLogin').attr({ 'disabled': true });
    //        setTimeout(function () {
    //            $('#usernameCheck').html('');
    //        }, 2000);
    //    }
    //    $(this).text('Verificar').attr({ 'disabled': false });
    //});

    //$('#emailTextBox').on('paste', function () {
    //    $('#btnCheckEmail').show();
    //});

    $('#emailTextBox').on('blur', function () {
        var $this = this;
        if ($this.value.length > 11) {
            if (my.isValidEmailAddress($this.value)) {

                if (my.checkPersonEmail(this.value)) {
                    $('#emailResult').popover('destroy');

                    $('#btnUpdatePerson').attr({ 'disabled': true, 'title': 'Desativado' });
                    $('#emailResult').show().css({ 'font-size': '20px', 'vertical-align': 'middle', 'color': 'red' }).addClass('fa fa-exclamation-triangle').attr({ 'title': 'Email Existente' });
                    $('#emailResult').popover({
                        delay: {
                            hide: 55000
                        },
                        trigger: 'hover',
                        placement: 'top',
                        html: true,
                        content: 'Este email j&#225; se encontra em nosso sistema. Click <a href="javascript:dnnModal.show(' + "'login?returmUrl=" + my.returnUrl + "&popUp=true',false,300,650,false)" + '">aqui</a> para efetuar o login.'
                    });
                    setTimeout(function () {
                        $('#emailResult').popover('show');
                    }, 1000);
                    
                    setTimeout(function () {
                        $('#emailResult').popover('hide');
                    }, 5000);

                } else {
                    $('#emailResult').popover('destroy');
                    $('#emailResult').show().css({ 'font-size': '20px', 'vertical-align': 'middle', 'color': 'green' }).removeClass('fa fa-exclamation-triangle').addClass('fa fa-check').attr({ 'title': 'Email Ok!' });
                    $('#btnUpdatePerson').attr({ 'disabled': false, 'title': 'Clique para se cadastrar' });
                }
            }
        }
    });

    //$('#btnCheckEmail').click(function () {
    //    //e.preventDefault();

    //        if (my.vm.email() !== '' && my.isValidEmailAddress(my.vm.email())) {
    //            if (my.checkPersonEmail($('#emailTextBox').val())) {
    //                $('#btnCheckEmail').attr({ 'disabled': false });
    //                $('#btnUpdatePerson').attr({ 'disabled': true, 'title': 'Desativado' });
    //                $.dnnAlert({
    //                    title: 'Aviso',
    //                    okText: 'Ok',
    //                    text: 'Email existente! Insira um novo endere&#231;o de email ou n&#227;o insira email algum.'
    //                });
    //                $('#btnCheckEmail').html('<i class="icon-ok"></i>').attr({ 'title': 'Verificar!' }).fadeIn(1000);
    //            } else {
    //                $('#btnCheckEmail').html('<img alt="Ok" src="/images/success-icn.png" style="vertical-align: middle;" />').attr({ 'title': 'Email pode ser validado!' }).attr({ 'disabled': false });
    //                $('#btnUpdatePerson').attr({ 'disabled': false, 'title': 'Clique para adicionar um novo cadastro' });
    //                my.vm.reqEmail(true);
    //            }
    //        } else {
    //            //if (self.clientId() > 0)
    //            //    $('#emailTextBox').attr({ 'class': 'required', 'required': true });
    //            //$('#emailCheck').html('');
    //            $('#btnCheckEmail').attr({ 'disabled': false }).hide();
    //            $('#btnUpdatePerson').attr({ 'disabled': false, 'title': '' });
    //        }
    //});

    $('#btnUpdatePerson').click(function (e) {
        e.preventDefault();

        var $this = $(this);

        $('#peopleRegistration').wrap('<form id="temp_form_id" />');
        if (!validator.validate('#temp_form_id')) {
            $.pnotify({
                title: 'Aten&#231;&#227;o!',
                text: 'Favor preenchar todos os campos obrigat&#243;rios.',
                type: 'warning',
                icon: 'fa fa-warning fa-lg',
                addclass: "stack-bottomright",
                stack: my.stack_bottomright
            });
            $('#peopleRegistration').unwrap();
        } else {
            $('#peopleRegistration').unwrap();

            $this.button('loading');

            var params = {
                PersonId: 0,
                PortalId: portalID,
                PersonType: $('#radioPerson').is(':checked'),
                CompanyName: my.vm.companyName().trim(),
                FirstName: my.vm.firstName().trim(),
                LastName: my.vm.lastName().trim(),
                DisplayName: '',
                Telephone: my.vm.telephone().replace(/\D/g, ''),
                Cell: my.vm.cell().replace(/\D/g, ''),
                Fax: my.vm.fax().replace(/\D/g, ''),
                Zero800s: my.vm.zero800().replace(/\D/g, ''),
                Email: my.vm.email().trim(),
                Website: my.vm.website().trim(),
                RegisterTypes: clientRoleId,
                DateFounded: moment('01/01/1900 00:00:00').format(),
                DateRegistered: moment('01/01/1900 00:00:00').format(),
                EIN: my.vm.ein(),
                CPF: my.vm.cpf(),
                Ident: my.vm.ident().trim(),
                StateTax: my.vm.stateTax().trim(),
                CityTax: my.vm.cityTax().trim(),
                Comments: '',
                Biography: '',
                Password: $('#passwordTextBox').val(),
                SalesRep: salesPerson,
                Industries: JSON.stringify(my.vm.selectedIndustries()),
                PostalCode: my.vm.postalCode().replace(/\D/g, ''),
                Street: my.vm.street().trim(),
                Unit: my.vm.unit().trim(),
                Complement: my.vm.complement().trim(),
                District: my.vm.district().trim(),
                City: my.vm.city().trim(),
                Region: my.vm.selectedRegion(),
                Country: my.vm.selectedCountry(),
                Blocked: false,
                ReasonBlocked: '',
                CreateLogin: true,
                CreatedByUser: userID,
                CreatedOnDate: moment().format(),
                ModifiedByUser: userID,
                ModifiedOnDate: moment().format(),
                SyncEnabled: amplify.store.sessionStorage('syncEnabled').toLowerCase()
            };

            $.ajax({
                type: 'POST',
                url: '/desktopmodules/riw/api/people/updatePerson',
                data: params
            }).done(function (data) {
                if (data.PersonId) {
                    my.personId = data.PersonId;
                    my.vm.personId(data.PersonId);

                    if (data.UserId) {

                        var loginParams = {
                            portalId: portalID,
                            userName: $('#emailTextBox').val(),
                            password: $('#passwordTextBox').val()
                        };

                        $.ajax({
                            type: 'POST',
                            url: '/desktopmodules/riw/api/store/login',
                            data: loginParams
                        }).done(function (data) {
                            if (data.Result.indexOf("success") !== -1) {
                                document.location.href = my.returnUrl + '#new/1'; // 'http://' + _siteURL;
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
                        
                    } else {
                        //$().toastmessage('showErrorToast', data.Result);
                        $.pnotify({
                            title: 'Erro!',
                            text: data.Result,
                            type: 'error',
                            icon: 'fa fa-times-circle fa-lg',
                            addclass: "stack-bottomright",
                            stack: my.stack_bottomright
                        });
                    }
                }

                $this.button('reset');
            }).fail(function (jqXHR, textStatus) {
                console.log(jqXHR.responseText);
            //}).always(function () {
                
            });
        }
    });

    $('#radioPerson').click(function (e) {
        my.setPerson();
    });

    $('#radioBusiness').click(function (e) {
        my.setPerson();
    });

    //my.vm.loadSaleReps();
    my.vm.loadCountries();
    my.vm.loadRegions();

    if (my.vm.reqLastName()) {
        $('#lastNameTextBox').addClass('required').attr({ 'required': true });
    }
    
    if (my.vm.reqTelephone()) {
        $('#liPhone').show();
        $('#phoneTextBox').addClass('required').attr({ 'required': true });        
    }

    if (my.vm.reqSSN()) {
        $('#liCPF').show();
        $('#cpfTextBox').addClass('required').attr({ 'required': true });
    }
    
    if (my.vm.reqIdent()) {
        $('#liIdent').show();
        $('#identTextBox').addClass('required').attr({ 'required': true });
    }

    $("#postalCodeTextBox").inputmask("99-999-999");
    $("#phoneTextBox").inputmask("(99) 9999-9999");
    $("#cellTextBox").inputmask("(99) 9999-9999");
    $("#faxTextBox").inputmask("(99) 9999-9999");
    $("#cpfTextBox").inputmask("999.999.999-99");
    $("#einTextBox").inputmask("99.999.999/9999-99");

    $('#personForm .pull-left').css({ 'width': '48%' });
    $('#personForm .pull-right').css({ 'width': '52%' });

    $('#personForm').show();
    $('#emailTextBox').focus();

    my.setPerson = function () {
        if ($('#radioPerson').is(':checked')) {
            
            $('#liCompanyName').fadeOut();
            $('#liFax').fadeOut();
            $('#liEIN').fadeOut();
            $('#liST').fadeOut();
            $('#liCT').fadeOut();
            $('#liZero0800').fadeOut();
            $('#liWebsite').fadeOut();
            if (my.vm.askLastName()) $('#lastNameTextBox').fadeIn();
            if (my.vm.reqLastName() && my.vm.askLastName()) $('#lastNameTextBox').fadeIn();

            if (my.vm.reqTelephone()) {
                $('#liPhone').fadeIn();
                $('#phoneTextBox').addClass('required').attr({ 'required': true });

            }

            if (my.vm.reqSSN()) {
                $('#liCPF').fadeIn();
                $('#cpfTextBox').addClass('required').attr({ 'required': true });
            }

            if (my.vm.reqIdent()) {
                $('#liIdent').fadeIn();
                $('#identTextBox').addClass('required').attr({ 'required': true });
            }

            $('#companyTextBox').removeClass('required').attr({ 'required': false });
            $('#einTextBox').removeClass('required').attr({ 'required': false });
            $('#stateTaxTextBox').removeClass('required').attr({ 'required': false });
            $('#cityTaxTextBox').removeClass('required').attr({ 'required': false });
            
        } else {
            $('#cpfTextBox').removeClass('required').attr({ 'required': false });
            $('#identTextBox').removeClass('required').attr({ 'required': false });

            $('#liCompanyName').fadeIn();
            $('#companyTextBox').addClass('required').attr({ 'required': true });
            if (my.vm.askEIN()) $('#liEIN').fadeIn();
            if (my.vm.askST()) $('#liST').fadeIn();
            if (my.vm.askCT()) $('#liCT').fadeIn();
            
            if (my.vm.reqTelephone()) {
                $('#liZero0800').fadeIn();
                $('#liFax').fadeIn();
                $('#liPhone').show();
                $('#phoneTextBox').addClass('required').attr({ 'required': true });
            }
            
            $('#liCPF').fadeOut();
            $('#liIdent').fadeOut();
            $('#liCell').fadeOut();
            if (my.vm.reqWebsite()) {
                $('#liWebsite').fadeIn();
                $('#websiteTextBox').addClass('required').attr({ 'required': true });
            }

            my.vm.reqCompanyName(true);
            if (my.vm.askEIN() && my.vm.reqEIN()) $('#einTextBox').addClass('required').attr({ 'required': true });
            if (my.vm.askST() && my.vm.reqST()) $('#stateTaxTextBox').addClass('required').attr({ 'required': true });
            if (my.vm.askCT() && my.vm.reqCT()) $('#cityTaxTextBox').addClass('required').attr({ 'required': true });
            
        }

        var validator = new ValidationUtility();
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

    my.vm.loadRegions();

    if (my.regType > 0) {
        my.vm.registerTypes.push(my.regType.toString());
    }

    var validator = new ValidationUtility();

    $('.btnReturn').click(function (e) {
        e.preventDefault();

        //$(this).html('<i class="fa fa-spinner fa-spin"></i>&nbsp; Um momento...').attr({ 'disabled': true });
        //var urlAddress = '';
        //if (my.editProduct) {
        //    urlAddress = _editProductURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.editProduct;
        //    document.location.href = urlAddress;
        //} else {
        //    //parent.$(".k-window-content").each(function (i, win) {
        //    parent.$('#window').data("kendoWindow").close();
        //    //});
        //    //$(".k-window .k-window-content").each(function (index, element) {
        //    //    setTimeout(function () {
        //    //        $(element).data('kendoWindow').close();
        //    //    }, 1000);
        //    //});
        //    //if (parent.$('#clientFinanWindow').data('kendoWindow')) {
        //    //    parent.$('#clientFinanWindow').data('kendoWindow').close();
        //    //}
        //    //if (parent.$('#clientEditWindow').data('kendoWindow')) {
        //    //    parent.$('#clientEditWindow').data('kendoWindow').close();
        //    //}
        //    //if (parent.$('#clientAssistWindow').data('kendoWindow')) {
        //    //    parent.$('#clientAssistWindow').data('kendoWindow').close();
        //    //}
        //    //if (parent.$('#clientCommWindow').data('kendoWindow')) {
        //    //    parent.$('#clientCommWindow').data('kendoWindow').close();
        //    //}
        //    //if (parent.$('#clientHistoryWindow').data('kendoWindow')) {
        //    //    parent.$('#clientHistoryWindow').data('kendoWindow').close();
        //    //}
        //}
    });

});
