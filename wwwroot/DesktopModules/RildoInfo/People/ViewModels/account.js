
$(function () {

    var status = $(".status");

    ko.subscribable.fn.trackDirtyFlag = function () {
        var original = this();

        this.isDirty = ko.computed(function () {
            if (this() !== original) {
                $(window).on('beforeunload', function () {
                    return 'Dados foram alterados, mas nada ainda foi salvo!';
                });
            }
            else {
                $(window).off('beforeunload');
            }
            return this() !== original;
        }, this);

        return this;
    };

    my.Country = function (data) {
        this.CountryCode = ko.observable(data.Value);
        this.CountryName = ko.observable(data.Text);
    };

    my.Region = function (data) {
        this.RegionCode = ko.observable(data.Value);
        this.RegionName = ko.observable(data.Text);
    };

    my.SalesPerson = function (data) {
        this.DisplayName = ko.observable(data.DisplayName);
        this.UserID = ko.observable(data.UserID);
    };

    // VIEW MODEL
    my.vm = function () {
        var self = this;

        self.sortJsonName = function (field, reverse, primer) {
            var key = primer ? function (x) { return primer(x[field]); } : function (x) { return x[field]; };
            return function (b, a) {
                var A = key(a), B = key(b);
                return ((A < B) ? -1 : (A > B) ? +1 : 0) * [-1, 1][+!!reverse];
            };
        },

        self.siteAddress = ko.computed(function () {
            return storeAddress + (storeUnit.length > 0 ? ' Nº ' + storeUnit : '') + '<br />' + storeComplement + '<br />' + storeCity + ' - ' + my.formatPostalcode(storePostalCode) + '<br />' + storeRegion + ' - ' + storeCountry;
        }),
        self.personType = ko.observable('1'),
        self.firstName = ko.observable(),
        self.lastName = ko.observable(''),
        self.displayName = ko.observable(),
        self.personId = ko.observable(0),
        self.personFullName = ko.computed(function () {
            return firstName() + ' ' + lastName();
        }, self),
        //self.personUserId = ko.observable(0),
        self.username = ko.observable(''),
        self.originalUsername = ko.observable(''),
        self.password = ko.observable(''),
        self.cpf = ko.observable(''),
        self.ident = ko.observable(''),
        self.telephone = ko.observable(''),
        self.cell = ko.observable(''),
        self.fax = ko.observable(''),
        self.zero800 = ko.observable(''),
        self.originalEmail = ko.observable(''),
        self.email = ko.observable(''),
        self.companyName = ko.observable(''),
        self.ein = ko.observable(''),
        self.stateTax = ko.observable(''),
        self.cityTax = ko.observable(''),
        self.dateFound = ko.observable(),
        self.dateRegistered = ko.observable(),
        self.statusId = ko.observable(0),
        self.postalCode = ko.observable(''),
        self.street = ko.observable(''),
        self.unit = ko.observable(''),
        self.complement = ko.observable(''),
        self.district = ko.observable(''),
        self.city = ko.observable(''),
        self.hasRegions = ko.observable(false),
        self.website = ko.observable(''),
        self.comments = ko.observable(''),
        self.sent = ko.observable(0),
        self.bio = ko.observable(''),
        self.income = ko.observable(0),
        self.credit = ko.observable(0),
        self.locked = ko.observable(false),
        self.finanAddress = ko.observable(''),
        self.selectedFinanAddress = ko.observable(0),
        self.createdByUser = ko.observable(0),
        self.createdOnDate = ko.observable(new Date()),

        self.countries = ko.observableArray([]),
        self.selectedCountry = ko.observable('BR'),
        self.loadCountries = function () {
            $.getJSON('/desktopmodules/riw/api/lists/countries', function (data) {
                data.sort(self.sortJsonName('Text', false, function (a) { return a.toUpperCase(); }));
                var mappedCountries = $.map(data, function (item) { return new my.Country(item); });
                self.countries(mappedCountries);
                //self.hasRegions(true);
            });
        },

        self.regions = ko.observableArray([]),
        self.selectedRegion = ko.observable(),
        self.loadRegions = ko.computed(function () {
            if (!self.selectedCountry()) {
                return null;
            }
            $.getJSON('/desktopmodules/riw/api/lists/regions?code=' + self.selectedCountry(), function (data) {
                if (data.length) {
                    //data.sort(self.sortJsonName('CountryName', false, function (a) { return a }));
                    self.regions.removeAll();
                    var mappedRegions = $.map(data, function (item) { return new my.Region(item); });
                    self.regions(mappedRegions);
                    self.hasRegions(true);
                    self.selectedRegion('MG')
                } else {
                    self.selectedRegion('Minas Gerais')
                }
            });
            // $('#firstNameTextBox').focus();
        }),

        self.saleReps = ko.observableArray([]),
        self.selectedSalesRepId = ko.observable(salesPerson),
        self.loadSaleReps = function () {
            $.getJSON('/desktopmodules/riw/api/people/GetUsersByRoleName?portalId=' + portalID + '&roleName=Vendedores', function (data) {
                if (data) {
                    self.saleReps.removeAll();
                    var mappedSaleReps = $.map(data, function (item) { return new my.SalesPerson(item); });
                    self.saleReps(mappedSaleReps);
                    //$.each(data, function (i, sr) {
                    //    self.saleReps.push(ko.mapping.fromJS(sr));
                    //});
                    //self.hasRegions(true);
                    //} else {
                    //    self.hasRegions(false);
                }
            });
        },

        self.industries = ko.observableArray([]),
        self.selectedIndustries = ko.observableArray([]),
        self.loadIndustries = function () {
            $.ajax({
                url: '/desktopmodules/riw/api/industries/GetIndustries?portalId=' + portalID + '&isDeleted=False',
                async: false
            }).done(function (data) {
                self.industries.removeAll();
                $.each(data, function (i, ind) {
                    self.industries.push(ko.mapping.fromJS(ind));
                });
            });
        },

        // Asked
        self.askLastName = ko.observable(JSON.parse(askLastName.toLowerCase())),
        self.askIndustry = ko.observable(JSON.parse(askIndustry.toLowerCase())),
        self.askTelephone = ko.observable(JSON.parse(askTelephone.toLowerCase())),
        self.askEIN = ko.observable(JSON.parse(askEIN.toLowerCase())),
        self.askCompany = ko.observable(JSON.parse(askCompany.toLowerCase())),
        self.askST = ko.observable(JSON.parse(askST.toLowerCase())),
        self.askCT = ko.observable(JSON.parse(askCT.toLowerCase())),
        self.askWebsite = ko.observable(JSON.parse(askWebsite.toLowerCase())),
        self.askSSN = ko.observable(JSON.parse(askSSN.toLowerCase())),
        self.askIdent = ko.observable(JSON.parse(askIdent.toLowerCase())),
        self.askAddress = ko.observable(JSON.parse(askAddress.toLowerCase())),

        // required
        self.reqEmail = ko.observable(false),
        self.reqLastName = ko.observable(JSON.parse(reqLastName.toLowerCase())),
        self.reqSSN = ko.observable(JSON.parse(reqSSN.toLowerCase())),
        self.reqIdent = ko.observable(JSON.parse(reqIdent.toLowerCase())),
        self.reqTelephone = ko.observable(JSON.parse(reqTelephone.toLowerCase())),
        self.reqWebsite = ko.observable(JSON.parse(reqWebsite.toLowerCase())),
        self.reqCompanyName = ko.observable(false),
        self.reqEIN = ko.observable(JSON.parse(reqEIN.toLowerCase())),
        self.reqST = ko.observable(JSON.parse(reqST.toLowerCase())),
        self.reqCT = ko.observable(JSON.parse(reqCT.toLowerCase())),
        self.reqIndustry = ko.observable(JSON.parse(reqIndustry.toLowerCase())),
        self.reqAddress = ko.observable(JSON.parse(reqAddress.toLowerCase())),

        self.formatResult = function (data) {
            return "<div class='select2-user-result'>" + data.IndustryTitle + "</div>";
        },

        self.formatSelection = function (data) {
            return data.IndustryId;
        },

        self.addresses = ko.observableArray([]),
        self.selectedContactAddress = ko.observable(0);
        self.loadAddresses = function () {
            $.ajax({
                url: '/desktopmodules/riw/api/people/GetPersonAddresses?personId=' + self.personId(),
                async: false
            }).done(function (data) {
                self.addresses.removeAll();
                $.each(data, function (i, a) {
                    self.addresses.push(ko.mapping.fromJS(a));
                });
            });
        },

        self.passwordSubject = ko.computed(function () {
            var result = passwordSubject.replace('[WEBSITE]', siteName);
            return result;
        }, self),

        self.passwordBody = ko.computed(function () {
            var result = passwordMessage.replace('[CLIENTE]', firstName() + ' ' + lastName()).replace('[WEBSITE1]', siteName).replace('[HTTPURL]', siteURL).replace('[URL]', siteURL).replace('[WEBSITE2]', siteName);
            return result;
        }, self),

        self.personAddressId = ko.observable(0),
        self.clientPRId = ko.observable(0),
        self.clientBRId = ko.observable(0),
        self.clientCRId = ko.observable(0),
        self.clientPartnerId = ko.observable(0),
        self.clientPBRId = ko.observable(0),
        self.clientISId = ko.observable(0),
            
        self.formatResult = function (data) {
            //return '<div class="select2-user-result">' + data.contactName + '</div>';
            return '<span title="' + data.id + '">' + data.text + '</span>';
        },
        self.formatSelection = function (data) {
            return data.text;
            //return '<div class="select2-user-result">' + data.text + '</div>';
        },

        //self.registerTypes = ko.observableArray([]);
        self.selectedType = ko.observable(),
        //self.loadEntitiesRoles = function () {
        //    $.ajax({
        //        type: 'GET',
        //        url: '/desktopmodules/riw/api/store/getRolesByRoleGroup?portalId=' + portalID + '&roleGroupName=Entidades'
        //    }).done(function (data) {
        //        if (data) {
        //            $.each(data, function (i, r) {
        //                self.registerTypes.push(ko.mapping.fromJS(r));
        //            });
        //            self.selectedType(5);
        //        } else {
        //            $.pnotify({
        //                title: 'Erro!',
        //                text: data.Result,
        //                type: 'error',
        //                icon: 'fa fa-times-circle fa-lg',
        //                addclass: "stack-bottomright",
        //                stack: my.stack_bottomright
        //            });
        //        }
        //    }).fail(function (jqXHR, textStatus) {
        //        console.log(jqXHR.responseText);
        //    });
        //},

        self.userAvatar = ko.observable('');

        return {
            sortJsonName: sortJsonName,
            siteAddress: siteAddress,
            personType: personType,
            firstName: firstName,
            lastName: lastName,
            displayName: displayName,
            personId: personId,
            personFullName: personFullName,
            //personUserId: personUserId,
            username: username,
            originalUsername: originalUsername,
            password: password,
            cpf: cpf,
            ident: ident,
            telephone: telephone,
            cell: cell,
            fax: fax,
            zero800: zero800,
            originalEmail: originalEmail,
            email: email,
            companyName: companyName,
            ein: ein,
            stateTax: stateTax,
            cityTax: cityTax,
            dateFound: dateFound,
            dateRegistered: dateRegistered,
            statusId: statusId,
            postalCode: postalCode,
            street: street,
            unit: unit,
            complement: complement,
            district: district,
            city: city,
            hasRegions: hasRegions,
            website: website,
            comments: comments,
            bio: bio,
            income: income,
            locked: locked,
            finanAddress: finanAddress,
            selectedFinanAddress: selectedFinanAddress,
            createdByUser: createdByUser,
            createdOnDate: createdOnDate,
            countries: countries,
            selectedCountry: selectedCountry,
            loadCountries: loadCountries,
            regions: regions,
            selectedRegion: selectedRegion,
            loadRegions: loadRegions,
            saleReps: saleReps,
            selectedSalesRepId: selectedSalesRepId,
            loadSaleReps: loadSaleReps,
            industries: industries,
            selectedIndustries: selectedIndustries,
            loadIndustries: loadIndustries,
            askLastName: askLastName,
            askIndustry: askIndustry,
            askTelephone: askTelephone,
            askEIN: askEIN,
            askCompany: askCompany,
            askST: askST,
            askCT: askCT,
            askWebsite: askWebsite,
            askSSN: askSSN,
            askIdent: askIdent,
            askAddress: askAddress,
            reqEmail: reqEmail,
            reqLastName: reqLastName,
            reqSSN: reqSSN,
            reqIdent: reqIdent,
            reqTelephone: reqTelephone,
            reqWebsite: reqWebsite,
            reqCompanyName: reqCompanyName,
            reqEIN: reqEIN,
            reqST: reqST,
            reqCT: reqCT,
            reqIndustry: reqIndustry,
            reqAddress: reqAddress,
            formatResult: formatResult,
            formatSelection: formatSelection,
            addresses: addresses,
            loadAddresses: loadAddresses,
            passwordSubject: passwordSubject,
            passwordBody: passwordBody,
            personAddressId: personAddressId,
            clientPRId: clientPRId,
            clientBRId: clientBRId,
            clientCRId: clientCRId,
            clientPartnerId: clientPartnerId,
            clientPBRId: clientPBRId,
            clientISId: clientISId,
            registerTypes: registerTypes,
            selectedType: selectedType,
            //loadEntitiesRoles: loadEntitiesRoles,
            userAvatar: userAvatar
        };

    }();

    ko.applyBindings(my.vm);

    //my.vm.loadEntitiesRoles();

    $('#selectRegisterTypes').kendoMultiSelect({
        autoBind: false,
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/store/getRolesByRoleGroup?portalId=' + portalID + '&roleGroupName=Entidades,Descontos'
                }
            }
        },
        dataTextField: 'RoleName',
        dataValueField: 'RoleId'
    });

    $('.icon-exclamation-sign').popover({
        delay: {
            show: 500,
            hide: 100
        },
        placement: 'top',
        trigger: 'hover'
    });

    my.getPerson = function () {
        
        $.ajax({
            url: '/desktopmodules/riw/api/people/getPerson?userId=' + userID,
            async: false
        }).done(function (data) {
            if (data) {
                my.vm.personId(data.PersonId);

                if (data.PersonType) {
                    my.vm.personType('1');
                } else {
                    my.vm.personType('0');
                }

                my.vm.displayName(data.DisplayName);
                my.vm.firstName(data.FirstName);
                my.vm.lastName(data.LastName);
                my.vm.cpf(data.Cpf);
                my.vm.ident(data.Ident);
                my.vm.telephone(data.Telephone);
                my.vm.cell(data.Cell);
                my.vm.fax(data.Fax);
                my.vm.originalEmail(data.Email);
                my.vm.email(data.Email);
                my.vm.companyName(data.CompanyName);
                my.vm.zero800(data.Zero800s);
                my.vm.dateFound(data.DateFound);
                my.vm.dateRegistered(data.DateRegistered);
                my.vm.ein(data.Cnpj);
                my.vm.stateTax(data.StateTax);
                my.vm.cityTax(data.CityTax);
                my.vm.comments(data.Comments);
                my.vm.bio(data.Biography);
                my.vm.locked(data.Locked);
                if (data.MonthlyIncome > 0) my.vm.income(data.MonthlyIncome);
                my.vm.selectedFinanAddress(data.PersonAddressId);
                my.vm.website(data.Website);
                my.vm.selectedSalesRepId(data.SalesRep);
                my.vm.createdByUser(data.CreatedByUser);
                my.vm.createdOnDate(data.CreatedOnDate);

                //$('#postalCodeTextBox').val(data.PostalCode);
                //$('#streetTextBox').val(data.Street);
                //$('#unitTextBox').val(data.Unit);
                //$('#complementTextBox').val(data.Complement);
                //$('#districtTextBox').val(data.District);
                //$('#cityTextBox').val(data.City);
                my.vm.selectedRegion(data.Region);
                my.vm.selectedCountry(data.Country);

                var _date = kendo.parseDate(lastPasswordChangeDate);
                $('#lastPasswordChangedDate').text(kendo.toString(_date, 'F')).css({ 'text-transform': 'capitalize' });
                $('#loginTextBox').val(my.userName);
                //my.vm.username(data.Username);
                //my.vm.originalUsername(_userName);

                $('#moduleTitleSkinObject').html('Perfil: ' + $('#firstNameTextBox').val() + ' ' + $('#lastNameTextBox').val() + ' ID: ' + userID);

                if (amplify.store.sessionStorage('avatar')) {
                    // $('#files').data('kendoUpload').disabled();
                    $("#divPhotos").hide();
                    my.vm.userAvatar(amplify.store.sessionStorage('avatar').replace(userFolder, ""));
                    var photoUrl = ('/portals/' + portalID + '/' + amplify.store.sessionStorage('avatar')).trim();
                    $('#aImg').html('<img alt="" src="' + photoUrl + '?maxwidth=100" />');
                    //$('#btnRemoveAvatar').html('&times; Remover');
                    $('#divAvatar').show();
                } else {
                    //$('#btnRemoveAvatar').html('');
                    $('#divAvatar').hide();
                }
                
                $('#btnDeletePerson').show();

                setTimeout(function () {
                    $.ajax({
                        url: '/desktopmodules/riw/api/people/GetPersonIndustries?personId=' + my.vm.personId(),
                        async: false
                    }).done(function (data) {
                        if (data) {
                            my.vm.selectedIndustries.removeAll();
                            $.each(data, function (i, item) {
                                my.vm.selectedIndustries.push(item.IndustryId);
                            });
                        }
                    });
                });
            }
        });        
    };

    if (userID !== 0) {
        my.getPerson();
    }

    $('#personMenu').show();
    $('#personMenu').jqxMenu({
        //width: '120',
        mode: 'vertical'
    });

    $("#bDayTextBox").kendoDatePicker({
        format: "m",
        parseFormats: ["m"]
    });

    $('#bDayTextBox').prop('placeholder', 'ex.: ' + kendo.toString(new Date(), 'm'));

    my.editPersonAddress = function (value) {
        $.ajax({
            url: '/desktopmodules/riw/api/people/GetPersonAddress?personAddressId=' + value + '&personId=' + my.vm.personId()
        }).done(function (data) {
            my.vm.personAddressId(data.PersonAddressId);
            $('#addressNameTextBox').val(data.AddressName);
            $('#addressPhoneTextBox').val(data.Telephone);
            $('#addressCellTextBox').val(data.Cell);
            $('#addressFaxTextBox').val(data.Fax);
            $('#addressViewOrderTextBox').val(data.ViewOrder);
            $('#addressPostalCodeTextBox').val(data.PostalCode);
            $('#addressStreetTextBox').val(data.Street);
            $('#addressUnitTextBox').val(data.Unit);
            $('#addressComplementTextBox').val(data.Complement);
            $('#addressDistrictTextBox').val(data.District);
            $('#addressCityTextBox').val(data.City);
            my.vm.selectedRegion(data.Region);
            $('#addressCommentTextArea').text(data.Comment);
            $("#addressPostalCodeTextBox").inputmask("99.999-999");
            $("#addressPhoneTextBox").inputmask("(99) 9999-9999");
            $("#addressCellTextBox").inputmask("(99) 9999-9999");
            $("#addressFaxTextBox").inputmask("(99) 9999-9999");
            $('#btnUpdatePersonAddress').html('<i class="fa fa-check"></i>&nbsp; Atualizar Endere&ccedil;o');
            if (data.IsDeleted) {
                $('#btnRestorePersonAddress').show();
                $('#btnDeletePersonAddress').hide();
            } else {
                $('#btnRestorePersonAddress').hide();
                $('#btnDeletePersonAddress').show();
            }
            $('#collapseAddress').addClass('in');
            $.scrollTo($('#collapseAddress'), 1000, { easing: 'swing' });
        });
        return false;
    };

    $('#btnUpdatePersonAddress').click(function (e) {
        e.preventDefault();
        $('#personAddresses').wrap('<form id="temp_form_id" />');
        if (!validator.validate('#temp_form_id')) {
            $.pnotify({
                title: 'Aten&#231;&#227;o!',
                text: 'Favor preenchar todos os campos obrigat&#243;rios.',
                type: 'warning',
                icon: 'fa fa-warning fa-lg',
                addclass: "stack-bottomright",
                stack: my.stack_bottomright
            });
            $('#personAddresses').unwrap();
        } else {

            var $this = $(this);
            $this.button('loading');

            var params = {
                PersonAddressId: my.vm.personAddressId(),
                PersonId: my.vm.personId(),
                AddressName: $('#addressNameTextBox').val().trim(),
                Street: $('#addressStreetTextBox').val().trim(),
                Unit: $('#addressUnitTextBox').val().trim(),
                Complement: $('#addressComplementTextBox').val().trim(),
                District: $('#addressDistrictTextBox').val().trim(),
                City: $('#addressCityTextBox').val().trim(),
                Region: my.vm.selectedRegion(),
                Country: my.vm.selectedCountry(),
                PostalCode: $('#addressPostalCodeTextBox').val(),
                Telephone: $('#addressPhoneTextBox').val().replace(/\D/g, ''),
                Cell: $('#addressCellTextBox').val().replace(/\D/g, ''),
                Fax: $('#addressFaxTextBox').val().replace(/\D/g, ''),
                AddressViewOrder: $('#addressViewOrderTextBox').val(),
                CreatedByUser: userID,
                CreatedOnDate: moment().format()
            };

            $.ajax({
                type: 'POST',
                url: '/desktopmodules/riw/api/people/UpdatePersonAddress',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    my.vm.personAddressId(0);
                    my.personAddressesData.read();
                    $('#personAddresses input').val('');
                    $('#addressViewOrderTextBox').val(1);
                    $('#btnDeletePersonAddress').hide();
                    $('#btnUpdatePersonAddress').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Endere&ccedil;o').attr({ 'disabled': false });
                    //$().toastmessage('showSuccessToast', 'Endere&#231;o gravado com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Endere&#231;o salvo.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    $('#collapseAddress').addClass('toggle');
                    $.scrollTo($('#personAddresses'), 1000, { easing: 'swing' });
                } else {
                    $.pnotify({
                        title: 'Erro!',
                        text: data.Result,
                        type: 'error',
                        icon: 'fa fa-times-circle fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    //$().toastmessage('showErrorToast', data.Meg);
                }
            }).fail(function (jqXHR, textStatus) {
                console.log(jqXHR.responseText);
            });
        }
    });

    $('#btnDeletePersonAddress').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var params = {
            PersonId: my.vm.personId(),
            PersonAddressId: my.vm.personAddressId(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
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
                            if (my.vm.personAddressId() !== 0) {

                                $this.button('loading');

                                $.ajax({
                                    type: 'PUT',
                                    url: '/desktopmodules/riw/api/people/DeletePersonAddress',
                                    data: params
                                }).done(function (data) {
                                    if (data.Result.indexOf("success") !== -1) {
                                        my.vm.personAddressId(0);
                                        my.personAddressesData.read();
                                        $('#btnUpdatePersonAddress').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Endere&ccedil;o').attr({ 'disabled': false });
                                        $('#personAddresses input').val('');
                                        $('#btnDeletePersonAddress').hide();
                                        //$().toastmessage('showSuccessToast', 'Endere&ccedil;o desativado com sucesso.');
                                        $.pnotify({
                                            title: 'Sucesso!',
                                            text: 'Endere&ccedil;o desativado.',
                                            type: 'success',
                                            icon: 'fa fa-check fa-lg',
                                            addclass: "stack-bottomright",
                                            stack: my.stack_bottomright
                                        });
                                        $('#collapseAddress').addClass('toggle');
                                    } else {
                                        $.pnotify({
                                            title: 'Erro!',
                                            text: data.Result,
                                            type: 'error',
                                            icon: 'fa fa-times-circle fa-lg',
                                            addclass: "stack-bottomright",
                                            stack: my.stack_bottomright
                                        });
                                        //$().toastmessage('showErrorToast', data.Meg);
                                    }
                                }).fail(function (jqXHR, textStatus) {
                                    console.log(jqXHR.responseText);
                                }).always(function () {
                                    $this.button('reset');
                                });

                                $dialog.dialog('close');
                                $dialog.dialog('destroy');
                            }
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

    $('#btnRestorePersonAddress').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            PersonId: my.vm.personId(),
            PersonAddressId: my.vm.personAddressId(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };
        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/people/RestorePersonAddress',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.vm.personAddressId(0);
                my.personAddressesData.read();
                $('#btnUpdatePersonAddress').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Endere&ccedil;o').attr({ 'disabled': false });
                $('#personAddresses input').val('');
                $('#btnRestorePersonAddress').hide();
                $('#btnDeletePersonAddress').hide();
                //$().toastmessage('showSuccessToast', 'Endere&ccedil;o ativado com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Endere&ccedil;o ativado.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                $.scrollTo($('#personAddresses'), 1000, { easing: 'swing' });
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

    $('#btnCancelAddress').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#btnUpdateAddress').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Endere&ccedil;o').attr({ 'disabled': false });
        $('#personAddresses input').val('');
        $('#btnDeletePersonAddress').hide();
        $('#collapseAddress').collapse('toggle');
        my.vm.personAddressId(null);
    });

    my.personAddressesTransport = {
        read: {
            url: '/desktopmodules/riw/api/people/getPersonAddresses?personId=' + my.vm.personId()
        }
    };

    my.personAddressesData = new kendo.data.DataSource({
        transport: my.personAddressesTransport,
        sort: { field: "AddressName", dir: "ASC" },
        schema: {
            model: {
                id: 'PersonAddressId'
            }
        }
    });

    my.createAddressesLV = function () {
        $("#lvPersonAddresses").kendoListView({
            dataSource: my.personAddressesData,
            template: kendo.template($("#tmplPersonAddresses").html()),
            dataBound: function () {
                if (this.dataSource.view().length === 0) $('#collapseAddress').addClass('in');
            }
        });
    };
    
    $("#addressPostalCodeTextBox").inputmask("99-999-999");
    $("#addressPhoneTextBox").inputmask("(99) 9999-9999");
    $("#addressCellTextBox").inputmask("(99) 9999-9999");
    $("#addressFaxTextBox").inputmask("(99) 9999-9999");
    var ddlCountries = $('#ddlAddressCountries').data('kendoDropDownList');
    ddlCountries.wrapper.find(".k-dropdown-wrap").css({ 'width': '160px' });
    ddlCountries.list.width(180);
    var ddlRegions = $('#ddlAddressRegions').data('kendoDropDownList');
    ddlRegions.wrapper.find(".k-dropdown-wrap").css({ 'width': '160px' });
    ddlRegions.list.width(180);

    my.personDocsTransport = {
        read: {
            url: '/desktopmodules/riw/api/people/getPersonDocs?personId=' + my.vm.personId()
        }
    };

    my.personDocsData = new kendo.data.DataSource({
        transport: my.personDocsTransport,
        sort: { field: "DocName", dir: "ASC" },
        schema: {
            model: {
                id: 'PersonDocsId',
                fields: {
                    //DateBirth: { field: "DateBirth", type: "date", format: "{0:m}" }
                }
            }
        }
    });

    my.createDocsLV = function () {
        $("#lvPersonDocs").kendoListView({
            dataSource: my.personDocsData,
            template: kendo.template($("#tmplPersonDocs").html()),
            change: function () {
                var row = this.select();
                var id = row.data("uid");
                my.uId = id;
            },
            dataBound: function () {
                if (this.dataSource.view().length === 0) $('#collapseDocument').addClass('in');
            }
        });
    };

    $('#files').kendoUpload({
        async: {
            saveUrl: '/desktopmodules/riw/api/people/PostFile',
            removeUrl: "remove",
            autoUpload: false
        },
        multiple: false,
        //showFileList: false,
        localization: {
            cancel: 'Cancelar',
            dropFilesHere: 'Arraste o arquivo aqui para envia-lo',
            remove: 'Remover',
            select: 'Selecionar Arquivo',
            statusUploading: 'Enviando Arquivo',
            uploadSelectedFiles: 'Enviar',
            headerStatusUploaded: 'Completo',
            //headerStatusUploading: "customHeaderStatusUploading",
            retry: "Tente Novamente",
            statusFailed: "Falha no Envio",
            statusUploaded: "statusUploaded"
        },
        select: function (e) {
            if ($('#docNameTextBox').val().length === 0) {
                e.preventDefault();
                $('#docNameTextBox').focus();
                //$().toastmessage('showErrorToast', 'Favor inserir o nome do documento.');
                $.pnotify({
                    title: 'Erro!',
                    text: 'Favor inserir o nome do documento',
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
            }
            $.each(e.files, function (index, value) {
                if (value.extension.toUpperCase() !== '.JPG' && value.extension.toUpperCase() !== '.PNG' && value.extension.toUpperCase() !== '.ZIP' && value.extension.toUpperCase() !== '.DOC' && value.extension.toUpperCase() !== '.DOCX' && value.extension.toUpperCase() !== '.PDF') {
                    e.preventDefault();
                    //$().toastmessage('showErrorToast', '&#201; permitido enviar somente arquivos com formato jpg, png, zip, doc, docx e pdf.');
                    $.pnotify({
                        title: 'Erro!',
                        text: '&#201; permitido enviar somente arquivos com formato jpg, png, zip, doc, docx e pdf.',
                        type: 'error',
                        icon: 'fa fa-times-circle fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                }
            });
            setTimeout(function () {
                $('.k-upload').css({ 'max-width': '80%' }); // ($('.k-filename').width() * 2) });
            });
        },
        upload: function (e) {
            e.data = {
                PortalId: portalID,
                PersonId: my.vm.personId(),
                DocName: $('#docNameTextBox').val(),
                DocDesc: $('#docDescTextArea').val(),
                DocUrl: '',
                //MaxWidth: 0,
                //MaxHeight: 0,
                FolderPath: 'Docs/' + userID,
                CreatedByUser: userID,
                CreatedOnDate: moment().format()
            };
        },
        success: function (e) {
            my.personDocsData.read();
            $('#personDocuments input').val('');
            $('#personDocuments textarea').val('');
            $('.k-upload-files').remove();
            //$().toastmessage('showSuccessToast', 'Arquivo enviado com sucesso.');
            $.pnotify({
                title: 'Sucesso!',
                text: 'Arquivo enviado.',
                type: 'success',
                icon: 'fa fa-check fa-lg',
                addclass: "stack-bottomright",
                stack: my.stack_bottomright
            });
            $('#docNameTextBox').focus();
        },
        remove: function (e) {
            //$('.k-upload-button').show();
        },
        error: function (e) {
            $.pnotify({
                title: 'Erro!',
                text: 'N&#227;o foi poss&#237;vel o envio do arquivo.',
                type: 'error',
                icon: 'fa fa-times-circle fa-lg',
                addclass: "stack-bottomright",
                stack: my.stack_bottomright
            });
            //$().toastmessage('showErrorToast', 'N&#227;o foi possível o envio do arquivo.');
        }
    });

    my.removeClientDoc = function (value) {
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
                            $.ajax({
                                type: 'DELETE',
                                url: '/desktopmodules/riw/api/people/RemovePersonDoc?personDocId=' + value + '&personId=' + my.vm.personId()
                            }).done(function (data) {
                                if (data.Result.indexOf("success") !== -1) {
                                    //my.vm.personContactId(0);
                                    my.personDocsData.read();
                                    //$().toastmessage('showSuccessToast', 'Documento excluido com sucesso.');
                                    $.pnotify({
                                        title: 'Sucesso!',
                                        text: 'Documento excluido.',
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
    };

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
            PersonId: my.vm.personId(),
            UserId: userID,
            Email: $('#loginTextBox').val(),
            //oldUsername: my.vm.originalUsername(),
            Subject: my.vm.passwordSubject(),
            MessageBody: my.vm.passwordBody(),
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

    $('#confirmPasswordTextBox').on('keyup', function (e) {

        if ($('#newPasswordTextBox').val() != $('#confirmPasswordTextBox').val()) {
            $('#passConfirm').removeClass().addClass('alert alert-error').html('Senhas N&#227;o Coincidem!');
        } else {
            $('#passConfirm').removeClass().addClass('alert alert-success').html('Senhas Coincidem!');
        }

        return false;

    });

    $('#newPasswordTextBox').on('keyup', function (e) {

        if ($('#newPasswordTextBox').val() == '') {
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
                Subject: my.vm.passwordSubject(),
                MessageBody: my.vm.passwordBody()
            };
            $.ajax({
                type: 'PUT',
                url: '/desktopmodules/riw/api/people/updateUserPassword',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
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
                my.vm.userAvatar(e.response.fileName);
                $('#dnn_dnnUser_avatar').find('img').attr({ 'src': '/portals/' + portalID + '/' + e.response.filePath + '?h=32&w=32' });
                $('#aImg').html('<img alt="" src="/portals/' + portalID + '/' + e.response.filePath + '?maxwidth=120" />');
                //$('#btnRemoveAvatar').html('&times; Remover');
                $("#divPhotos").hide();
                $('#divAvatar').show();
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

        var params = {
            portalId: portalID,
            userId: userID,
            fileName: my.vm.userAvatar()
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
                                url: '/desktopmodules/riw/api/people/RemoveUserPhoto',
                                data: params
                            }).done(function (data) {
                                if (data.Result.indexOf("success") !== -1) {
                                    $('#aImg').val('');
                                    //$('#files').data('kendoUpload').enable();
                                    $("#divPhotos").show();
                                    $('#divAvatar').hide();
                                    $('#dnn_dnnUser_avatar').find('img').attr({ 'src': '/images/no_avatar.gif?h=32&w=32' });
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

    $('#radioPerson').click(function (e) {
        my.setPerson();
    });

    $('#radioBusiness').click(function (e) {
        my.setPerson();
    });

    $('#btnUpdatePerson').click(function (e) {
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
                PersonId: my.vm.personId(),
                PortalId: portalID,
                PersonType: $('#radioPerson').is(':checked'),
                CompanyName: my.vm.companyName().trim(),
                FirstName: my.vm.firstName().trim(),
                LastName: my.vm.lastName().trim(),
                Telephone: my.vm.telephone().replace(/\D/g, ''),
                Cell: my.vm.cell().replace(/\D/g, ''),
                Fax: my.vm.fax().replace(/\D/g, ''),
                Zero800s: my.vm.zero800().replace(/\D/g, ''),
                Email: my.vm.email().trim(),
                Website: my.vm.website().trim(),
                RegisterTypes: JSON.stringify(my.vm.selectedType()), // JSON.stringify($("input[name='rTypes']:checked").getCheckboxVal()),
                DateFounded: moment(new Date('01/01/1900 00:00:00')).format(),
                DateRegistered: moment(new Date('01/01/1900 00:00:00')).format(),
                EIN: my.vm.ein(),
                CPF: my.vm.cpf(),
                Ident: my.vm.ident().trim(),
                StateTax: my.vm.stateTax().trim(),
                CityTax: my.vm.cityTax().trim(),
                Comments: my.vm.comments().trim(),
                Biography: my.vm.bio().trim(),
                SalesRep: selectedSalesRepId(),
                Industries: JSON.stringify(my.vm.selectedIndustries()),
                PostalCode: my.vm.postalCode().replace(/\D/g, ''),
                Street: my.vm.street().trim(),
                Unit: my.vm.unit().trim(),
                Complement: my.vm.complement().trim(),
                District: my.vm.district().trim(),
                City: my.vm.city().trim(),
                Region: my.vm.selectedRegion(),
                Country: my.vm.selectedCountry(),
                CreateLogin: false,
                CreatedByUser: userID,
                CreatedOnDate: moment().format(),
                ModifiedByUser: userID,
                ModifiedOnDate: moment().format()
            };

            $.ajax({
                type: 'POST',
                url: '/desktopmodules/riw/api/people/UpdatePerson',
                data: params
            }).done(function (data) {
                if (data.PersonId) {

                    $('#dnn_dnnUser_enhancedRegisterLink').text(data.DisplayName)

                    //$().toastmessage('showSuccessToast', 'Cadastro gravado com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Cadastro salvo.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
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

    $('#btnDeletePerson').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var params = {
            PersonId: my.vm.personId(),
            PortalId: portalID,
            UserId: userID,
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
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
                                url: '/desktopmodules/riw/api/people/DeletePerson',
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

    $("#postalCodeTextBox").inputmask("99-999-999");
    $("#phoneTextBox").inputmask("(99) 9999-9999");
    $("#cellTextBox").inputmask("(99) 9999-9999");
    $("#faxTextBox").inputmask("(99) 9999-9999");
    $("#cpfTextBox").inputmask("999.999.999-99");
    $("#einTextBox").inputmask("99.999.999/9999-99");
    $('#personForm .pull-left').css({ 'width': '48%' });
    $('#personForm .pull-right').css({ 'width': '52%' });

    $('#firstNameTextBox').focus();

    my.clientPartnerBankRefsTransport = {
        read: {
            url: '/desktopmodules/riw/api/people/GetClientPartnerBankRefs?personId=' + my.vm.personId()
        }
    };

    my.clientPartnerBankRefsData = new kendo.data.DataSource({
        transport: my.clientPartnerBankRefsTransport,
        sort: { field: "BankRef", dir: "ASC" },
        schema: {
            model: {
                id: 'ClientPartnerBankRefId',
                fields: {
                    //DateBirth: { field: "DateBirth", type: "date", format: "{0:m}" }
                }
            }
        }
    });

    my.createPartnerBankRefsLV = function () {
        $("#lvPartnerBankRefs").kendoListView({
            dataSource: my.clientPartnerBankRefsData,
            template: kendo.template($("#tmplClientPartnerBankRefs").html()),
            dataBound: function () {
                //if (this.dataSource.total() === 0) $('#collapseBankPartnerRefForm').collapse('toggle');
            }
        });
    };

    my.clientPartnersTransport = {
        read: {
            url: '/desktopmodules/riw/api/people/GetClientPartners?personId=' + my.vm.personId()
        }
    };

    my.clientPartnersData = new kendo.data.DataSource({
        transport: my.clientPartnersTransport,
        sort: { field: "PartnerName", dir: "ASC" },
        schema: {
            model: {
                id: 'ClientPartnerId'
            }
        }
    });

    my.createPartnersLV = function () {
        $("#lvClientPartners").kendoListView({
            dataSource: my.clientPartnersData,
            template: kendo.template($("#tmplClientPartners").html()),
            dataBound: function () {
                //if (this.dataSource.total() === 0) $('#collapsePartnerForm').collapse('toggle');
            }
        });
    };

    my.clientCommRefsTransport = {
        read: {
            url: '/desktopmodules/riw/api/people/GetClientCommRefs?personId=' + my.vm.personId()
        }
    };

    my.clientCommRefsData = new kendo.data.DataSource({
        transport: my.clientCommRefsTransport,
        sort: { field: "CommRef", dir: "ASC" },
        schema: {
            model: {
                id: 'ClientCommRefId',
                fields: {
                    //DateBirth: { field: "DateBirth", type: "date", format: "{0:m}" }
                }
            }
        }
    });

    my.createCommRefsLV = function () {
        $("#lvCommRefs").kendoListView({
            dataSource: my.clientCommRefsData,
            template: kendo.template($("#tmplClientCommRefs").html()),
            dataBound: function () {
                //if (this.dataSource.total() === 0) $('#collapseCommRefForm').collapse('toggle');
            }
        });
    };

    my.clientBankRefsTransport = {
        read: {
            url: '/desktopmodules/riw/api/people/GetClientBankRefs?personId=' + my.vm.personId()
        }
    };

    my.clientBankRefsData = new kendo.data.DataSource({
        transport: my.clientBankRefsTransport,
        sort: { field: "BankRef", dir: "ASC" },
        schema: {
            model: {
                id: 'ClientBankRefId'
            }
        }
    });

    my.createBankRefsLV = function () {
        $("#lvBankRefs").kendoListView({
            dataSource: my.clientBankRefsData,
            template: kendo.template($("#tmplClientBankRefs").html()),
            dataBound: function () {
                //if (this.dataSource.total() === 0) $('#collapseBankRefForm').collapse('toggle');
            }
        });
    };

    my.clientPersonalRefsTransport = {
        read: {
            url: '/desktopmodules/riw/api/people/GetClientPersonalRefs?personId=' + my.vm.personId()
        }
    };

    my.clientPersonalRefsData = new kendo.data.DataSource({
        transport: my.clientPersonalRefsTransport,
        sort: { field: "PRName", dir: "ASC" },
        schema: {
            model: {
                id: 'ClientPersonalRefId'
            }
        }
    });

    my.createPersonalRefsLV = function () {
        $("#lvPersonalRefs").kendoListView({
            dataSource: my.clientPersonalRefsData,
            template: kendo.template($("#tmplClientPersonalRefs").html()),
            dataBound: function () {
                //if (this.dataSource.total() === 0) $('#collapsePersonalRefForm').collapse('toggle');
            }
        });
    };

    my.clientIncomeSourcesTransport = {
        read: {
            url: '/desktopmodules/riw/api/people/GetClientIncomeSources?personId=' + my.vm.personId()
        }
    };

    my.clientIncomeSourcesData = new kendo.data.DataSource({
        transport: my.clientIncomeSourcesTransport,
        sort: { field: "ISName", dir: "ASC" },
        schema: {
            model: {
                id: 'clientISId',
                fields: {
                    //DateBirth: { field: "DateBirth", type: "date", format: "{0:m}" }
                }
            }
        }
    });

    my.createIncomeSourcesLV = function () {
        $("#lvIncomeSources").kendoListView({
            dataSource: my.clientIncomeSourcesData,
            template: kendo.template($("#tmplClientIncomeSources").html()),
            dataBound: function () {
                // if (this.dataSource.view().length === 0) $('#collapseIncomeSourceForm').collapse('toggle');
            }
        });
    };

    my.initializer = function () {
        
        if (my.vm.personType() === '1') {
            if (my.vm.askLastName()) {
                $('#lastNameTextBox').show();
                if (my.vm.reqLastName()) {
                    $('#lastNameTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                }
            }
            if (my.vm.askTelephone()) {
                $('#liPhone').show();
                $('#liCell').show();
                if (my.vm.reqTelephone()) {
                    $('#phoneTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                }
            }
            if (my.vm.askSSN()) {
                $('#liCPF').show();
                if (my.vm.reqSSN()) {
                    $('#cpfTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                }
            }
            if (my.vm.askIdent()) {
                $('#liIdent').show();
                if (my.vm.reqIdent()) {
                    $('#identTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                }
            }
        } else {
            $('#liCompanyName').show();
            $('#companyTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
            my.vm.reqCompanyName(true);
            if (my.vm.askTelephone()) {
                $('#liPhone').show();
                $('#liFax').show();
                $('#liZero0800').show();
                if (my.vm.reqTelephone()) {
                    $('#phoneTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                }
            }
            if (my.vm.askWebsite()) {
                $('#liWebsite').show();
                if (my.vm.reqWebsite()) {
                    $('#websiteTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                }
            }
            if (my.vm.askEIN()) {
                $('#liEIN').show();
                if (my.vm.reqEIN()) {
                    $('#einTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                }
            }
            if (my.vm.askST()) {
                $('#liST').show();
                if (my.vm.reqST()) {
                    $('#stateTaxTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                }
            }
            if (my.vm.askCT()) {
                $('#liCT').show();
                if (my.vm.reqCT()) {
                    $('#cityTaxTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                }
            }
            if (my.vm.askIndustry()) {
                my.vm.loadIndustries();
                $('#liIndustries').show();
                if (my.vm.reqIndustry()) {
                    $('#s2id_ddlIndustries').attr({ 'required': 'required', 'data-role': 'validate' });
                }
            }
        } //var validator = new ValidationUtility();

        switch (my.getParameterByName('sel')) {
            case 2: // Person Addresses

                my.createAddressesLV();

                $('#collapseAddress .pull-left').css({ 'width': '45%' });
                $('#collapseAddress .pull-right').css({ 'width': '55%' });

                $('#personAddresses').delay(100).show();

                break;
            case 3:  // Person Docs

                my.createDocsLV();

                $('#personDocuments').delay(100).show();

                setTimeout(function () {
                    $('#docNameTextBox').focus();
                });

                break;
            case 4: // Edit Login

                $('#editLogin').delay(100).show();

                break;
            case 5:

                $('#userPhoto .pull-left').css({ 'width': '50%' });
                $('#userPhoto .pull-right').css({ 'width': '50%' });

                $('#userPhoto').delay(100).show();

                break;
            case 6:

                if (my.vm.personType()) {
                    $('#liPartners').hide();
                    $('#liPartnerBanks').hide();
                    $('#liIncomes').show();
                    $('#liRefs').show();
                    $('#liComRefs').show();
                    $('#divBusinessName').hide();
                    $('#divCPF2').show();
                    $('#divIdent2').show();
                    $('#divEIN2').hide();
                    $('#divST2').hide();
                    $('#divIM2').hide();
                } else {
                    $('#liPartners').show();
                    $('#liPartnerBanks').show();
                    $('#liPersonalRefs').hide();
                    $('#liIncomes').hide();
                    $('#liRefs').hide();
                    $('#liComRefs').hide();
                    $('#divClientName').hide();
                    $('#divBusinessName').show();
                    $('#divEIN2').show();
                    $('#divST2').show();
                    $('#divIM2').show();
                    $('#divCPF2').hide();
                    $('#divIdent2').hide();
                }

                my.createPartnerBankRefsLV();
                my.createPartnersLV();
                my.createCommRefsLV();
                my.createBankRefsLV();
                my.createPersonalRefsLV();
                my.createIncomeSourcesLV();

                $('#personFinance').delay(100).show();

                break;
            default:

                //if (my.vm.personId() === 1) {
                //    $('#btnUpdateLogin').hide();
                //    $('#btnUpdatePassword').hide();
                //    $('#btnRandomPassword').hide();
                //}

                //my.vm.loadEntitiesRoles();

                if (my.vm.askLastName()) {
                    $('#lastNameTextBox').show();
                    if (my.vm.reqLastName()) {
                        $('#lastNameTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                    }
                }

                if (my.vm.askTelephone()) {
                    $('#liPhone').show();
                    $('#liCell').show();
                    if (my.vm.reqTelephone()) {
                        $('#phoneTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                    }
                }

                if (my.vm.askSSN()) {
                    $('#liCPF').show();
                    if (my.vm.reqSSN()) {
                        $('#cpfTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                    }
                }

                if (my.vm.askIdent()) {
                    $('#liIdent').show();
                    if (my.vm.reqIdent()) {
                        $('#identTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                    }
                }

                my.vm.loadCountries();
                my.vm.loadRegions();

                $('#personForm').show();
        }
    };

    my.setPerson = function () {
        if ($('#radioPerson').is(':checked')) {
            $('#liIndustries').fadeOut();
            $('#liCompanyName').fadeOut();
            $('#liFax').fadeOut();
            $('#liEIN').fadeOut();
            $('#liST').fadeOut();
            $('#liCT').fadeOut();
            $('#liZero0800').fadeOut();
            $('#liWebsite').fadeOut();
            if (my.vm.askLastName()) $('#lastNameTextBox').fadeIn();
            if (my.vm.reqLastName() && my.vm.askLastName()) $('#lastNameTextBox').fadeIn();
            if (my.vm.askTelephone()) {
                $('#liPhone').fadeIn();
                $('#liCell').fadeIn();
                if (my.vm.reqTelephone()) {
                    $('#phoneTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                }
            }
            if (my.vm.askSSN()) $('#liCPF').fadeIn();
            if (my.vm.reqSSN() && my.vm.askSSN()) $('#cpfTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
            if (my.vm.askIdent()) $('#liIdent').fadeIn();
            if (my.vm.reqIdent() && my.vm.askIdent()) $('#identTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
            $('#companyTextBox').removeClass('required').removeAttr('required');
            $('#companyTextBox').removeClass('required').removeAttr('data-role');
            $('#einTextBox').removeClass('required').removeAttr('required');
            $('#einTextBox').removeClass('required').removeAttr('data-role');
            $('#stateTaxTextBox').removeClass('required').removeAttr('required');
            $('#stateTaxTextBox').removeClass('required').removeAttr('data-role');
            $('#cityTaxTextBox').removeClass('required').removeAttr('required');
            $('#cityTaxTextBox').removeClass('required').removeAttr('data-role');
            $('#ddlIndustries').removeClass('required').removeAttr('required');
            $('#ddlIndustries').removeClass('required').removeAttr('data-role');
        } else {
            $('#cpfTextBox').removeClass('required').removeAttr('required');
            $('#cpfTextBox').removeClass('required').removeAttr('data-role');
            $('#identTextBox').removeClass('required').removeAttr('required');
            $('#identTextBox').removeClass('required').removeAttr('data-role');
            if (my.vm.askIndustry()) {
                my.vm.loadIndustries();
                $('#liIndustries').fadeIn();
            }
            $('#liCompanyName').fadeIn();
            $('#companyTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
            if (my.vm.askEIN()) $('#liEIN').fadeIn();
            if (my.vm.askST()) $('#liST').fadeIn();
            if (my.vm.askCT()) $('#liCT').fadeIn();
            if (my.vm.askTelephone()) {
                $('#liZero0800').fadeIn();
                $('#liFax').fadeIn();
                $('#liPhone').show();
                if (my.vm.reqTelephone()) {
                    $('#phoneTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                }
            }
            $('#liCPF').fadeOut();
            $('#liIdent').fadeOut();
            $('#liCell').fadeOut();
            if (my.vm.askWebsite()) $('#liWebsite').fadeIn();
            if (my.vm.askWebsite() && my.vm.reqWebsite()) $('#websiteTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
            my.vm.reqCompanyName(true);
            if (my.vm.askEIN() && my.vm.reqEIN()) $('#einTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
            if (my.vm.askST() && my.vm.reqST()) $('#stateTaxTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
            if (my.vm.askCT() && my.vm.reqCT()) $('#cityTaxTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
            if (my.vm.askIndustry() && my.vm.reqIndustry()) $('#s2id_ddlIndustries').attr({ 'required': 'required', 'data-role': 'validate' });
        }
    };

    $('#clientFinanceTabs').kendoTabStrip({
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

    my.initializer();
    my.vm.loadRegions();

    $('#iSIncomeNumericTextBox').kendoNumericTextBox({
        value: 1,
        min: 0,
        format: 'c'
    });

    $("#iSPhoneTextBox").inputmask("(99) 9999-9999");
    $("#iSFaxTextBox").inputmask("(99) 9999-9999");
    $("#iSPostalCodeTextBox").inputmask("99-999-999");
    $("#iSEINTextBox").inputmask("99.999.999/9999-99");

    my.editClientIncomeSource = function (value) {
        $.ajax({
            url: '/desktopmodules/riw/api/people/GetClientIncomeSource?clientIncomeSourceId=' + value + '&personId=' + my.vm.personId()
        }).done(function (data) {
            my.vm.clientISId(data.ClientIncomeSourceId);
            $('#iSNameTextBox').val(data.ISName);
            $('#iSEINTextBox').val(data.ISEIN);
            $('#iSInsEstTextBox').val(data.ISST);
            $('#iSInsMunTextBox').val(data.ISCT);
            $('#iSPhoneTextBox').val(data.ISPhone);
            $('#iSFaxTextBox').val(data.ISFax);
            $('#iSIncomeNumericTextBox').data('kendoNumericTextBox').value(data.ISIncome);
            $('#iSPostalCodeTextBox').val(data.ISPostalCode);
            $('#iSStreetTextBox').val(data.ISAddress);
            $('#iSUnitTextBox').val(data.ISAddressUnit);
            $('#iSComplementTextBox').val(data.ISComplement);
            $('#iSDistrictTextBox').val(data.ISDistrict);
            $('#iSCityTextBox').val(data.ISCity);
            my.vm.selectedRegion(data.Region);
            my.vm.selectedCountry('BR');
            $('#iSProofCheckbox').attr({ 'checked': data.ISProof });
            $("#iSPostalCodeTextBox").inputmask("99-999-999");
            $("#iSPhoneTextBox").inputmask("(99) 9999-9999");
            $("#iSFaxTextBox").inputmask("(99) 9999-9999");
            $('#btnUpdateIncomeSource').html('<i class="fa fa-check"></i>&nbsp; Atualizar Renda');
            $('#btnRemoveIncomeSource').show();
            $('#btnCopyIncomeSource').show();
            if ($('#collapseIncomeSourceForm.in').length === 0) {
                $('#collapseIncomeSourceForm').collapse('toggle');
            }
            $.scrollTo($('#collapseIncomeSourceForm'), 1000, { easing: 'swing' });
        });
    };

    $('#btnUpdateIncomeSource').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        //if (clientFinancesValidator.validate()) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            ClientIncomeSourceId: my.vm.clientISId(),
            PersonId: my.vm.personId(),
            ISName: $('#iSNameTextBox').val().trim(),
            ISEIN: $('#iSEINTextBox').val().replace(/\D/g, ''),
            ISST: $('#iSInsEstTextBox').val().trim(),
            ISCT: $('#iSInsMunTextBox').val().trim(),
            ISPhone: $('#iSPhoneTextBox').val().replace(/\D/g, ''),
            ISFax: $('#iSFaxTextBox').val().replace(/\D/g, ''),
            ISIncome: $('#iSIncomeNumericTextBox').data('kendoNumericTextBox').value(),
            ISPostalCode: $('#iSPostalCodeTextBox').val().replace(/\D/g, ''),
            ISAddress: $('#iSStreetTextBox').val().trim(),
            ISAddressUnit: $('#iSUnitTextBox').val().trim(),
            ISComplement: $('#iSComplementTextBox').val().trim(),
            ISDistrict: $('#iSDistrictTextBox').val().trim(),
            ISCity: $('#iSCityTextBox').val().trim(),
            ISRegion: my.vm.selectedRegion(),
            iSCountry: my.vm.selectedCountry(),
            ISProof: $('#iSProofCheckbox').is(':checked')
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/people/updateClientIncomeSource',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.clientIncomeSourcesData.read();
                $('#editIncomes input').val('');
                $('#iSProofCheckbox').attr({ 'checked': false });
                $('#iSIncomeNumericTextBox').data('kendoNumericTextBox').value(1);
                $('#btnRemoveIncomeSource').hide();
                $('#btnCopyIncomeSource').hide();

                if (my.vm.clientISId()) {
                    //$().toastmessage('showSuccessToast', 'Renda atualizada com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Renda atualizada.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                } else {
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Renda inserida.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    //$().toastmessage('showSuccessToast', 'Renda inserida com sucesso.');
                }

                my.vm.clientISId(null);
                $('#collapseIncomeSourceForm').collapse('toggle');
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

    $('#btnCopyIncomeSource').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            ClientIncomeSourceId: 0,
            PersonId: my.vm.personId(),
            ISName: $('#iSNameTextBox').val().trim(),
            ISEIN: $('#iSEINTextBox').val().replace(/\D/g, ''),
            ISST: $('#iSInsEstTextBox').val().trim(),
            ISCT: $('#iSInsMunTextBox').val().trim(),
            ISPhone: $('#iSPhoneTextBox').val().replace(/\D/g, ''),
            ISFax: $('#iSFaxTextBox').val().replace(/\D/g, ''),
            ISIncome: $('#iSIncomeNumericTextBox').data('kendoNumericTextBox').value(),
            ISPostalCode: $('#iSPostalCodeTextBox').val().replace(/\D/g, ''),
            ISAddress: $('#iSStreetTextBox').val().trim(),
            ISAddressUnit: $('#iSUnitTextBox').val().trim(),
            ISComplement: $('#iSComplementTextBox').val().trim(),
            ISDistrict: $('#iSDistrictTextBox').val().trim(),
            ISCity: $('#iSCityTextBox').val().trim(),
            ISRegion: my.vm.selectedRegion(),
            iSCountry: my.vm.selectedCountry(),
            ISProof: $('#iSProofCheckbox').is(':checked')
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/people/updateClientIncomeSource',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.clientIncomeSourcesData.read();
                $('#editIncomes input').val('');
                $('#iSProofCheckbox').attr({ 'checked': false });
                $('#iSIncomeNumericTextBox').data('kendoNumericTextBox').value(1);
                $('#btnRemoveIncomeSource').hide();
                $('#btnCopyIncomeSource').hide();

                //$().toastmessage('showSuccessToast', 'Renda atualizada com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Renda duplicada.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });

                my.vm.clientISId(null);
                $('#collapseIncomeSourceForm').collapse('toggle');
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

    $('#btnRemoveIncomeSource').click(function (e) {
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
                                    if (my.vm.clientISId() !== 0) {

                                        $this.button('loading');

                                        $.ajax({
                                            type: 'DELETE',
                                            url: '/desktopmodules/riw/api/people/RemoveClientIncomeSource?clientIncomeSourceId=' + my.vm.clientISId() + '&personId=' + my.vm.personId()
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                my.clientIncomeSourcesData.read();
                                                $('#editIncomes input').val('');
                                                $('#iSProofCheckbox').attr({ 'checked': false });
                                                $('#iSIncomeNumericTextBox').data('kendoNumericTextBox').value(1);
                                                $('#btnRemoveIncomeSource').hide();
                                                $('#btnCopyIncomeSource').hide();
                                                my.vm.selectedRegion('MG');
                                                my.vm.selectedCountry('BR');
                                                //$().toastmessage('showSuccessToast', 'Renda excluida com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Renda excluida.',
                                                    type: 'success',
                                                    icon: 'fa fa-check fa-lg',
                                                    addclass: "stack-bottomright",
                                                    stack: my.stack_bottomright
                                                });
                                                $('#collapseIncomeSourceForm').collapse('toggle');
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

    $('#btnCancelIncomeSource').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#editIncomes input').val('');
        $('#iSProofCheckbox').attr({ 'checked': false });
        $('#iSIncomeNumericTextBox').data('kendoNumericTextBox').value(1);
        $('#btnRemoveIncomeSource').hide();
        my.vm.selectedRegion('MG');
        my.vm.selectedCountry('BR');
        $('#btnCopyIncomeSource').hide();
        my.vm.clientISId(null);
        $('#collapseIncomeSourceForm').collapse('toggle');
    });

    $("#personalRefPhoneTextBox").inputmask("(99) 9999-9999");

    my.editClientPersonalRef = function (value) {
        $.ajax({
            url: '/desktopmodules/riw/api/people/GetClientPersonalRef?clientPersonalRefId=' + value + '&personId=' + my.vm.personId()
        }).done(function (data) {
            my.vm.clientPRId(data.ClientPersonalRefId);
            $('#personalRefNameTextBox').val(data.PRName);
            $('#personalRefPhoneTextBox').val(data.PRPhone);
            $('#personalRefEmailTextBox').val(data.PREmail);
            $("#personalRefPhoneTextBox").inputmask("(99) 9999-9999");
            $('#btnUpdatePersonalRef').html('<i class="fa fa-check"></i>&nbsp; Atualizar Refer&#234;ncia');
            $('#btnRemovePersonalRef').show();
            if ($('#collapsePersonalRef.in').length === 0) {
                $('#collapsePersonalRef').collapse('toggle');
            }
            $.scrollTo($('#collapsePersonalRef'), 1000, { easing: 'swing' });
        });
    };

    $('#btnUpdatePersonalRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        //if (clientFinancesValidator.validate()) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            ClientPersonalRefId: my.vm.clientPRId(),
            PersonId: my.vm.personId(),
            PRName: $('#personalRefNameTextBox').val().trim(),
            PRPhone: $('#personalRefPhoneTextBox').val().trim().replace(/\D/g, ''),
            PREmail: $('#personalRefEmailTextBox').val().trim()
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/people/updateClientPersonalRef',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.clientPersonalRefsData.read();
                $('#editPersonalRefs input').val('');
                $('#btnRemovePersonalRef').hide();

                if (params.cPRId !== 0) {
                    //$().toastmessage('showSuccessToast', 'Refer&#234;ncia atualizada com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Refer&#234;ncia atualizada.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                } else {
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Refer&#234;ncia inserida.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    //$().toastmessage('showSuccessToast', 'Refer&#234;ncia inserida com sucesso.');
                }

                $('#collapsePersonalRef').collapse('toggle');
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

        //}
    });

    $('#btnRemovePersonalRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

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
                                    if (my.vm.clientPRId() !== 0) {
                                        $.ajax({
                                            type: 'DELETE',
                                            url: '/desktopmodules/riw/api/people/removeClientPersonalRef?clientPersonalRefId=' + my.vm.clientPRId() + '&personId=' + my.vm.personId()
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                my.clientPersonalRefsData.read();
                                                $('#editPersonalRefs input').val('');
                                                $('#btnRemovePersonalRef').hide();
                                                $('#btnUpdatePersonalRef').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Refer&#234;ncia').attr({ 'disabled': false });
                                                //$().toastmessage('showSuccessToast', 'Refer&#234;ncia excluida com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Refer&#234;ncia excluida.',
                                                    type: 'success',
                                                    icon: 'fa fa-check fa-lg',
                                                    addclass: "stack-bottomright",
                                                    stack: my.stack_bottomright
                                                });
                                                $('#collapsePersonalRef').collapse('toggle');
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

    $('#btnCancelPersonalRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#editPersonalRefs input').val('');
        $('#btnRemovePersonalRef').hide();
        $('#collapsePersonalRef').collapse('toggle');
        $('#btnUpdatePersonalRef').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Refer&#234;ncia').attr({ 'disabled': false });
        my.vm.clientPRId(null);
    });

    $('#partnerBankRefClientSinceTextBox').kendoDatePicker();

    $('#bankRefClientSinceTextBox').kendoDatePicker();

    $('#bankRefCreditLimitTextBox').kendoNumericTextBox({
        value: 0,
        min: 0,
        format: 'c'
    });

    $("#bankRefContactPhoneTextBox").inputmask("(99) 9999-9999");

    my.editClientBankRef = function (value) {
        $.ajax({
            url: '/desktopmodules/riw/api/people/GetClientBankRef?clientBankRefId=' + value + '&personId=' + my.vm.personId()
        }).done(function (data) {
            my.vm.clientBRId(data.ClientBankRefId);
            $('#bankRefTextBox').val(data.BankRef);
            $('#bankRefAgencyTextBox').val(data.BankRefAgency);
            $('#bankRefAccountTextBox').val(data.BankRefAccount);
            $('#bankRefClientSinceTextBox').data('kendoDatePicker').value(kendo.parseDate(data.BankRefClientSince));
            $('#bankRefContactNameTextBox').val(data.BankRefContact);
            $('#bankRefContactPhoneTextBox').val(data.BankRefPhone);
            $('#bankRefAccountTypeTextBox').val(data.BankRefAccountType);
            $('#bankRefCreditLimitTextBox').data('kendoNumericTextBox').value(data.BankRefCredit);
            $("#bankRefContactPhoneTextBox").inputmask("(99) 9999-9999");
            $('#btnUpdateBankRef').html('<i class="fa fa-ok"></i>&nbsp; Atualizar Refer&#234;ncia');
            $('#btnRemoveBankRef').show();
            if ($('#collapseBankRefForm.in').length === 0) {
                $('#collapseBankRefForm').collapse('toggle');
            }
            $.scrollTo($('#collapseBankRefForm'), 1000, { easing: 'swing' });
        });
    };

    $('#btnUpdateBankRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        //if (clientFinancesValidator.validate()) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            clientBankRefId: my.vm.clientBRId(),
            PersonId: my.vm.personId(),
            BankRef: $('#bankRefTextBox').val().trim(),
            BankRefAgency: $('#bankRefAgencyTextBox').val().trim(),
            BankRefAccount: $('#bankRefAccountTextBox').val().trim(),
            BankRefClientSince: moment(new Date($('#bankRefClientSinceTextBox').data('kendoDatePicker').value())).format(),
            BankRefContact: $('#bankRefContactNameTextBox').val().trim(),
            BankRefPhone: $('#bankRefContactPhoneTextBox').val().replace(/\D/g, ''),
            BankRefAccountType: $('#bankRefAccountTypeTextBox').val().trim(),
            BankRefCredit: kendo.parseInt($('#bankRefCreditLimitTextBox').val()) > 0 ? $('#bankRefCreditLimitTextBox').data('kendoNumericTextBox').value() : 0
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/people/updateClientBankRef',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.vm.clientBRId(0);
                my.clientBankRefsData.read();
                $('#editBankRefs input').val('');
                $('#bankRefCreditLimitTextBox').data('kendoNumericTextBox').value(0);
                $('#bankRefClientSinceTextBox').data('kendoDatePicker').value('');
                $('#btnRemoveBankRef').hide();

                if (params.clientBankRefId !== 0) {
                    //$().toastmessage('showSuccessToast', 'Refer&#234;ncia atualizada com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Refer&#234;ncia atualizada.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                } else {
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Refer&#234;ncia inserida.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    //$().toastmessage('showSuccessToast', 'Refer&#234;ncia inserida com sucesso.');
                }

                $('#collapseBankRefForm').collapse('toggle');
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
        //}
    });

    $('#btnRemoveBankRef').click(function (e) {
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
                                    if (my.vm.clientBRId() !== 0) {

                                        $this.button('loading');

                                        $.ajax({
                                            type: 'DELETE',
                                            url: '/desktopmodules/riw/api/people/removeClientBankReference?clientBankReferenceId=' + my.vm.clientBRId() + '&personId=' + my.vm.personId()
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                my.vm.clientBRId(0);
                                                my.clientBankRefsData.read();
                                                $('#editBankRefs input').val('');
                                                $('#bankRefCreditLimitTextBox').data('kendoNumericTextBox').value(0);
                                                $('#bankRefClientSinceTextBox').data('kendoDatePicker').value('');
                                                $('#btnRemoveBankRef').hide();
                                                $('#btnUpdateBankRef').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Refer&#234;ncia').attr({ 'disabled': false });
                                                //$().toastmessage('showSuccessToast', 'Referência excluida com sucesso.');
                                                $('#collapseBankRefForm').collapse('toggle');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Refer&#234;ncia excluida.',
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

    $('#btnCancelBankRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#editBankRefs input').val('');
        $('#bankRefCreditLimitTextBox').data('kendoNumericTextBox').value(0);
        $('#bankRefClientSinceTextBox').data('kendoDatePicker').value('');
        $('#btnRemoveBankRef').hide();
        $('#btnUpdateBankRef').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Refer&#234;ncia').attr({ 'disabled': false });
        $('#collapseBankRefForm').collapse('toggle');
        my.vm.clientBRId(null);
    });

    $('#commRefLastActivityTextBox').kendoDatePicker();

    $('#commRefCreditTextBox').kendoNumericTextBox({
        value: 0,
        min: 0,
        format: 'c'
    });

    $("#commRefPhoneTextBox").inputmask("(99) 9999-9999");

    my.editClientCommRef = function (value) {
        $.ajax({
            url: '/desktopmodules/riw/api/people/GetClientCommRef?clientCommRefId=' + value + '&personId=' + my.vm.personId()
        }).done(function (data) {
            my.vm.clientCRId(data.ClientCommRefId);
            $('#commRefNameTextBox').val(data.CommRefBusiness);
            $('#commRefContactTextBox').val(data.CommRefContact);
            $('#commRefPhoneTextBox').val(data.CommRefPhone);
            $('#commRefLastActivityTextBox').data('kendoDatePicker').value(kendo.parseDate(data.CommRefLastActivity));
            $('#commRefCreditTextBox').data('kendoNumericTextBox').value(data.CommRefCredit);
            $("#commRefPhoneTextBox").inputmask("(99) 9999-9999");
            $('#btnUpdateCommRef').html('<i class="fa fa-check"></i>&nbsp; Atualizar Refer&#234;ncia');
            $('#btnRemoveCommRef').show();
            if ($('#collapseCommRefForm.in').length === 0) {
                $('#collapseCommRefForm').collapse('toggle');
            }
            $.scrollTo($('#collapseCommRefForm'), 1000, { easing: 'swing' });
        });
    };

    $('#btnUpdateCommRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        //if (clientFinancesValidator.validate()) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            ClientCommRefId: my.vm.clientCRId(),
            PersonId: my.vm.personId(),
            CommRefBusiness: $('#commRefNameTextBox').val(),
            CommRefContact: $('#commRefContactTextBox').val(),
            CommRefPhone: $('#commRefPhoneTextBox').val().replace(/\D/g, ''),
            CommRefLastActivity: moment(new Date($('#commRefLastActivityTextBox').data('kendoDatePicker').value())).format(),
            CommRefCredit: $('#commRefCreditTextBox').data('kendoNumericTextBox').value()
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/people/updateClientCommRef',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.vm.clientCRId(0);
                my.clientCommRefsData.read();
                $('#editCommRefs input').val('');
                $('#commRefCreditTextBox').data('kendoNumericTextBox').value(0);
                $('#commRefLastActivityTextBox').data('kendoDatePicker').value('');
                $('#btnRemoveCommRef').hide();

                if (params.ClientCommRefId !== 0) {
                    //$().toastmessage('showSuccessToast', 'Refer&#234;ncia atualizada com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Refer&#234;ncia atualizada.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                } else {
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Refer&#234;ncia inserida.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    //$().toastmessage('showSuccessToast', 'Refer&#234;ncia inserida com sucesso.');
                }
                $('#collapseCommRefForm').collapse('toggle');
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
        }).always(function (e) {
            $this.button('reset');
        });

        //}
    });

    $('#btnRemoveCommRef').click(function (e) {
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
                                    if (my.vm.clientCRId() !== 0) {

                                        $this.button('loading');

                                        $.ajax({
                                            type: 'DELETE',
                                            url: '/desktopmodules/riw/api/people/removeClientCommRef?clientCommReferenceId=' + my.vm.clientCRId() + '&personId=' + my.vm.personId()
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                my.vm.clientCRId(0);
                                                my.clientCommRefsData.read();
                                                $('#editCommRefs input').val('');
                                                $('#commRefCreditTextBox').data('kendoNumericTextBox').value(0);
                                                $('#commRefLastActivityTextBox').data('kendoDatePicker').value('');
                                                $('#btnRemoveCommRef').hide();
                                                $('#btnUpdateCommRef').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Refer&#234;ncia').attr({ 'disabled': false });
                                                //$().toastmessage('showSuccessToast', 'Refer&#234;ncia excluida com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Refer&#234;ncia excluida.',
                                                    type: 'success',
                                                    icon: 'fa fa-check fa-lg',
                                                    addclass: "stack-bottomright",
                                                    stack: my.stack_bottomright
                                                });
                                                $('#collapseCommRefForm').collapse('toggle');
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

    $('#btnCancelCommRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#editCommRefs input').val('');
        $('#commRefCreditTextBox').data('kendoNumericTextBox').value(0);
        $('#commRefLastActivityTextBox').data('kendoDatePicker').value('');
        $('#btnRemoveCommRef').hide();
        $('#btnUpdateCommRef').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Refer&#234;ncia').attr({ 'disabled': false });
        $('#collapseCommRefForm').collapse('toggle');
        my.vm.clientCRId(null);
    });

    $('#partnerQuotaTextBox').kendoNumericTextBox({
        value: 0,
        format: "##.00 '%'"
    });

    $("#partnerCPFTextBox").inputmask("999999999-99");
    $("#partnerPostalCodeTextBox").inputmask("99-999-999");
    $("#partnerPhoneTextBox").inputmask("(99) 9999-9999");
    $("#partnerCellTextBox").inputmask("(99) 9999-9999");

    my.editClientPartner = function (value) {
        $.ajax({
            url: '/desktopmodules/riw/api/people/GetClientPartner?clientPartnerId=' + value + '&personId=' + my.vm.personId()
        }).done(function (data) {
            my.vm.clientPartnerId(data.ClientPartnerId);
            $('#partnerNameTextBox').val(data.PartnerName);
            $('#partnerCPFTextBox').val(data.PartnerCPF);
            $('#partnerIdentTextBox').val(data.PartnerIdentity);
            $('#partnerPhoneTextBox').val(data.PartnerPhone);
            $('#partnerCellTextBox').val(data.PartnerCell);
            $('#partnerEmailTextBox').val(data.PartnerEmail);
            $('#partnerQuotaTextBox').data('kendoNumericTextBox').value(data.PartnerQuota);
            $('#partnerPostalCodeTextBox').val(data.PartnerPostalCode);
            $('#partnerStreetTextBox').val(data.PartnerAddress);
            $('#partnerUnitTextBox').val(data.PartnerAddressUnit);
            $('#partnerComplementTextBox').val(data.PartnerComplement);
            $('#partnerDistrictTextBox').val(data.PartnerDistrict);
            $('#partnerCityTextBox').val(data.PartnerCity);
            my.vm.selectedRegion(data.PartnerRegion);
            my.vm.selectedCountry('BR');
            $("#partnerCPFTextBox").inputmask("999999999-99");
            $("#partnerPostalCodeTextBox").inputmask("99-999-999");
            $("#partnerPhoneTextBox").inputmask("(99) 9999-9999");
            $("#partnerCellTextBox").inputmask("(99) 9999-9999");
            $('#btnUpdatePartner').html('<i class="fa fa-check"></i>&nbsp; Atualizar S&#243;cio');
            $('#btnRemovePartner').show();
            if ($('#collapsePartnerForm.in').length === 0) {
                $('#collapsePartnerForm').collapse('toggle');
            }
            $.scrollTo($('#collapsePartnerForm'), 1000, { easing: 'swing' });
        });
        return false;
    };

    $('#btnUpdatePartner').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        //if (clientAddressesValidator.validate()) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            ClientPartnerId: my.vm.clientPartnerId(),
            PersonId: my.vm.personId(),
            PartnerName: $('#partnerNameTextBox').val(),
            PartnerCPF: $('#partnerCPFTextBox').val(),
            PartnerIdentity: $('#partnerIdentTextBox').val(),
            PartnerPhone: $('#partnerPhoneTextBox').val().replace(/\D/g, ''),
            PartnerCell: $('#partnerCellTextBox').val().replace(/\D/g, ''),
            PartnerEmail: $('#partnerEmailTextBox').val(),
            PartnerQuota: $('#partnerQuotaTextBox').data('kendoNumericTextBox').value(),
            PartnerPostalCode: $('#partnerPostalCodeTextBox').val().replace(/\D/g, ''),
            PartnerAddress: $('#partnerStreetTextBox').val(),
            PartnerAddressUnit: $('#partnerUnitTextBox').val(),
            PartnerComplement: $('#partnerComplementTextBox').val(),
            PartnerDistrict: $('#partnerDistrictTextBox').val(),
            Partnercity: $('#partnerCityTextBox').val(),
            PartnerRegion: my.vm.selectedRegion(),
            PartnerCountry: my.vm.selectedCountry()
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/people/updateClientPartner',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.vm.clientPartnerId(0);
                my.clientPartnersData.read();
                $('#editPartners input').val('');
                $('#btnRemovePartner').hide();
                $('#btnCopyPartner').hide();

                if (params.ClientPartnerId !== 0) {
                    //$().toastmessage('showSuccessToast', 'S&#243;cio atualizado com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Dados do s&#243;cio atualizado.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                } else {
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Dados do s&#243;cio inserido.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    //$().toastmessage('showSuccessToast', 'S&#243;cio inserido com sucesso.');
                }

                $('#collapsePartnerForm').collapse('toggle');
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

        //}
    });

    $('#btnCopyPartner').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        //if (clientAddressesValidator.validate()) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            ClientPartnerId: 0,
            PersonId: my.vm.personId(),
            PartnerName: $('#partnerNameTextBox').val(),
            PartnerCPF: $('#partnerCPFTextBox').val().replace(/\D/g, ''),
            PartnerIdentity: $('#partnerIdentTextBox').val(),
            PartnerPhone: $('#partnerPhoneTextBox').val().replace(/\D/g, ''),
            PartnerCell: $('#partnerCellTextBox').val().replace(/\D/g, ''),
            PartnerEmail: $('#partnerEmailTextBox').val(),
            PartnerQuota: $('#partnerQuotaTextBox').data('kendoNumericTextBox').value(),
            PartnerPostalCode: $('#partnerPostalCodeTextBox').val().replace(/\D/g, ''),
            PartnerAddress: $('#partnerStreetTextBox').val(),
            PartnerAddressUnit: $('#partnerUnitTextBox').val(),
            PartnerComplement: $('#partnerComplementTextBox').val(),
            PartnerDistrict: $('#partnerDistrictTextBox').val(),
            Partnercity: $('#partnerCityTextBox').val(),
            PartnerRegion: my.vm.selectedRegion(),
            PartnerCountry: my.vm.selectedCountry()
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/people/updateClientPartner',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.vm.clientPartnerId(0);
                my.clientPartnersData.read();
                $('#editPartners input').val('');
                $('#btnRemovePartner').hide();
                //$().toastmessage('showSuccessToast', 'S&#243;cio inserido com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Dados do s&#243;cio duplicado.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                $('#collapsePartnerForm').collapse('toggle');
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

            $('#btnUpdatePartner').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar S&#243;cio').attr({ 'disabled': false });

        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
        //}
    });

    $('#btnRemovePartner').click(function (e) {
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
                                    if (my.vm.clientPartnerId() !== 0) {

                                        $this.button('loading');

                                        $.ajax({
                                            type: 'DELETE',
                                            url: '/desktopmodules/riw/api/people/RemoveClientPartner?clientPartnerId=' + my.vm.clientPartnerId() + '&personId=' + my.vm.personId()
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                my.vm.clientPartnerId(0);
                                                my.clientPartnersData.read();
                                                $('#editPartners input').val('');
                                                $('#btnRemovePartner').hide();
                                                $('#btnUpdatePartner').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar S&#243;cio').attr({ 'disabled': false });
                                                //$().toastmessage('showSuccessToast', 'S&#243;cio excluido com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Dados do s&#243;cio excluido.',
                                                    type: 'success',
                                                    icon: 'fa fa-check fa-lg',
                                                    addclass: "stack-bottomright",
                                                    stack: my.stack_bottomright
                                                });
                                                $('#collapsePartnerForm').collapse('toggle');
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

    $('#btnCancelPartner').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#collapsePartnerForm input').val('');
        $('#partnerQuotaTextBox').data('kendoNumericTextBox').value(0);
        $('#btnRemovePartner').hide();
        $('#btnUpdatePartner').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar S&#243;cio').attr({ 'disabled': false });
        $('#collapsePartnerForm').collapse('toggle');
        my.vm.clientPartnerId(null);
    });

    $('#ddlClientPartners').kendoDropDownList({
        dataSource: my.clientPartnersData,
        dataTextField: 'PartnerName',
        dataValueField: 'ClientPartnerId'
    })

    $('#partnerBankRefCreditLimitTextBox').kendoNumericTextBox({
        value: 0,
        min: 0,
        format: 'c'
    });

    $("#partnerBankRefContactPhoneTextBox").inputmask("(99) 9999-9999");

    my.editClientPartnerBankRef = function (value) {
        $.ajax({
            url: '/desktopmodules/riw/api/people/GetClientPartnerBankRef?clientPartnerBankReferenceId=' + value + '&personId=' + my.vm.personId()
        }).done(function (data) {
            my.vm.clientPBRId(data.ClientPartnerBankRefId);
            $('#ddlClientPartners').data('kendoDropDownList').text(data.PartnerName);
            $('#partnerBankRefTextBox').val(data.BankRef);
            $('#partnerBankRefAgencyTextBox').val(data.BankRefAgency);
            $('#partnerBankRefAccountTextBox').val(data.BankRefAccount);
            $('#partnerBankRefClientSinceTextBox').data('kendoDatePicker').value(kendo.parseDate(data.BankRefClientSince));
            $('#partnerBankRefContactNameTextBox').val(data.BankRefContact);
            $('#partnerBankRefContactPhoneTextBox').val(data.BankRefPhone);
            $('#partnerBankRefAccountTypeTextBox').val(data.BankRefAccountType);
            $('#partnerBankRefCreditLimitTextBox').data('kendoNumericTextBox').value(data.BankRefCredit);
            $("#partnerBankRefContactPhoneTextBox").inputmask("(99) 9999-9999");
            $('#btnUpdatePartnerBankRef').html('<i class="fa fa-check"></i>&nbsp; Atualizar Refer&#234;ncia');
            $('#btnRemovePartnerBankRef').show();
            if ($('#collapseBankPartnerRefForm.in').length === 0) {
                $('#collapseBankPartnerRefForm').collapse('toggle');
            }
            $.scrollTo($('#collapseBankPartnerRefForm'), 1000, { easing: 'swing' });
        });
    };

    $('#btnUpdatePartnerBankRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        //if (clientFinancesValidator.validate()) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            ClientPartnerBankRefId: my.vm.clientPBRId(),
            PartnerName: $('#ddlClientPartners').data('kendoDropDownList').text(),
            PersonId: my.vm.personId(),
            BankRef: $('#partnerBankRefTextBox').val(),
            BankRefAgency: $('#partnerBankRefAgencyTextBox').val(),
            BankRefAccount: $('#partnerBankRefAccountTextBox').val(),
            BankRefClientSince: moment(new Date($('#partnerBankRefClientSinceTextBox').data('kendoDatePicker').value())).format(),
            BankRefContact: $('#partnerBankRefContactNameTextBox').val(),
            BankRefPhone: $('#partnerBankRefContactPhoneTextBox').val().replace(/\D/g, ''),
            BankRefAccountType: $('#partnerBankRefAccountTypeTextBox').val(),
            BankRefCredit: $('#partnerBankRefCreditLimitTextBox').data('kendoNumericTextBox').value()
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/people/updateClientpartnerBankRef',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                my.vm.clientPBRId(0);
                my.clientPartnerBankRefsData.read();
                $('#collapseBankPartnerRefForm input').val('');
                $('#partnerBankRefCreditLimitTextBox').data('kendoNumericTextBox').value(0);
                $('#partnerBankRefClientSinceTextBox').data('kendoDatePicker').value('');
                $('#btnRemovePartnerBankRef').hide();

                if (params.ClientPartnerBankRefId !== 0) {
                    //$().toastmessage('showSuccessToast', 'Refer&#234;ncia atualizada com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Refer&#234;ncia atualizada.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                } else {
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Refer&#234;ncia inserida.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    //$().toastmessage('showSuccessToast', 'Refer&#234;ncia inserida com sucesso.');
                }

                $('#collapseBankPartnerRefForm').collapse('toggle');
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

        //}
    });

    $('#btnRemovePartnerBankRef').click(function (e) {
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
                                    if (my.vm.clientPBRId() !== 0) {

                                        $this.button('loading');

                                        $.ajax({
                                            type: 'DELETE',
                                            url: '/desktopmodules/riw/api/people/removeClientPartnerBankReference?clientPartnerBankRefId=' + my.vm.clientPBRId() + '&personId=' + my.vm.personId()
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                my.vm.clientPBRId(0);
                                                my.clientPartnerBankRefsData.read();
                                                $('#collapseBankPartnerRefForm input').val('');
                                                $('#partnerBankRefCreditLimitTextBox').data('kendoNumericTextBox').value(0);
                                                $('#partnerBankRefClientSinceTextBox').data('kendoDatePicker').value('');
                                                $('#btnRemovePartnerBankRef').hide();
                                                $('#btnUpdatePartnerBankRef').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Refer&#234;ncia').attr({ 'disabled': false });
                                                //$().toastmessage('showSuccessToast', 'Referência excluida com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Refer&#234;ncia excluida.',
                                                    type: 'success',
                                                    icon: 'fa fa-check fa-lg',
                                                    addclass: "stack-bottomright",
                                                    stack: my.stack_bottomright
                                                });
                                                $('#collapseBankPartnerRefForm').collapse('toggle');
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

    $('#btnCancelPartnerBankRef').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#collapseBankPartnerRefForm input').val('');
        $('#partnerBankRefCreditLimitTextBox').data('kendoNumericTextBox').value(0);
        $('#partnerBankRefClientSinceTextBox').data('kendoDatePicker').value('');
        $('#btnRemovePartnerBankRef').hide();
        $('#btnUpdatePartnerBankRef').html('<i class="icon-plus icon-white"></i>&nbsp; Adicionar Refer&#234;ncia').attr({ 'disabled': false });
        $('#collapseBankPartnerRefForm').collapse('toggle');
        my.vm.clientPBRId(null);
    });

    if (my.getParameterByName('sel')) {
        $('#personMenu ul li[id=' + my.getParameterByName('sel') + ']').addClass('jqx-menu-item-selected');
    } else {
        $('#personMenu ul li[id=1]').addClass('jqx-menu-item-selected');
    }

    $('#personMenu').on('itemclick', function (event) {
        document.location.hash = 'sel/' + event.args.id;
        $('#personMenu ul li').removeClass('jqx-menu-item-selected');
        $('#personMenu ul li[id=' + event.args.id + ']').addClass('jqx-menu-item-selected');
        my.initializer();
        switch (event.args.id) {
            case '1':

                $('#personForm').hide();
                $('#personFinance').fadeOut();
                $('#personDocuments').fadeOut();
                //$('#editMsg').fadeOut();
                $('#userPhoto').fadeOut();
                $('#editLogin').fadeOut();
                $('#personAddresses').fadeOut();
                $('#clientFinances').fadeOut();
                $('#personForm').delay(300).fadeIn();

                break;
            case '2':

                $('#personAddresses').hide();
                $('#personFinance').fadeOut();
                $('#personDocuments').fadeOut();
                //$('#editMsg').fadeOut();
                $('#userPhoto').fadeOut();
                $('#editLogin').fadeOut();
                $('#clientFinances').fadeOut();
                $('#personForm').fadeOut();
                $('#personAddresses').delay(300).fadeIn();

                break;
            case '3':

                $('#personDocuments').hide();
                $('#personFinance').fadeOut();
                //$('#editMsg').fadeOut();
                $('#userPhoto').fadeOut();
                $('#editLogin').fadeOut();
                $('#clientFinances').fadeOut();
                $('#personForm').fadeOut();
                $('#personAddresses').fadeOut();
                $('#personDocuments').delay(300).fadeIn();

                break;
            case '4':

                $('#editLogin').hide();
                $('#personFinance').fadeOut();
                //$('#editMsg').fadeOut();
                $('#userPhoto').fadeOut();
                $('#personDocuments').fadeOut();
                $('#personForm').fadeOut();
                $('#personAddresses').fadeOut();
                $('#clientFinances').fadeOut();
                $('#editLogin').delay(300).fadeIn();

                break;
            case '5':

                $('#userPhoto').hide();
                $('#personFinance').fadeOut();
                //$('#editMsg').fadeOut();
                $('#editLogin').fadeOut();
                $('#personForm').fadeOut();
                $('#personAddresses').fadeOut();
                $('#clientFinances').fadeOut();
                $('#personDocuments').fadeOut();
                $('#userPhoto').delay(300).fadeIn();

                break;
            case '6':

                $('#personFinance').hide();
                $('#userPhoto').fadeOut();
                //$('#editMsg').fadeOut();
                $('#editLogin').fadeOut();
                $('#personForm').fadeOut();
                $('#personAddresses').fadeOut();
                $('#clientFinances').fadeOut();
                $('#personDocuments').fadeOut();
                $('#personFinance').delay(300).fadeIn();

                break;
            case '7':

        }
    });

    var validator = new ValidationUtility();

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

    $.fn.dnnFileInput = function (options) {
        
    };

});
