
my.viewModel = function () {

    ko.bindingHandlers.select2 = {
        init: function (element, valueAccessor, allBindingsAccessor) {
            var obj = valueAccessor(),
                allBindings = allBindingsAccessor(),
                lookupKey = allBindings.lookupKey;
            $(element).select2(obj);
            if (lookupKey) {
                var value = ko.utils.unwrapObservable(allBindings.value);
                $(element).select2('data', ko.utils.arrayFirst(obj.data.results, function (item) {
                    return item[lookupKey] === value;
                }));
            }

            ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                $(element).select2('destroy');
            });
        },
        update: function (element) {
            $(element).trigger('change');
        }
    };

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

    my.PersonHistory = function (data) {
        //var self = this;
        //data = data || {};

        //this.historyByAvatar = data.CreatedByUser > 0 ? (data.Avatar ? '/portals/0/' + data.Avatar + '?w=45&h=45&mode=crop' : '/desktopmodules/rildoinfo/webapi/content/images/user.png?w=45&h=45') : '/desktopmodules/rildoinfo/webapi/content/images/ri-logo.png';
        //this.historyByName = data.DisplayName || 'Sistema';
        //this.historyId = data.HistoryId;
        //this.historyText = data.HistoryText;
        //this.createdOnDate = moment(data.CreatedOnDate).fromNow();
        //this.locked = data.Locked;
        var self = this;
        data = data || {};

        self.historyByAvatar = data.Avatar ? '/portals/0/' + data.Avatar + '?w=45&h=45&mode=crop' : '/desktopmodules/rildoinfo/webapi/content/images/user.png?w=45&h=45';
        self.historyByName = data.DisplayName || 'Sistema';
        self.historyId = data.PersonHistoryId;
        self.historyText = data.HistoryText;
        self.createdOnDate = moment(data.CreatedOnDate).fromNow();
        self.allowed = data.Allowed;
        self.historyComments = ko.observableArray([]);

        self.newCommentHistory = ko.observable();
        self.addHistoryComment = function (index, data, e) {

            e.currentTarget.value = 'Um momento...';
            e.currentTarget.disabled = true;

            var PersonHistoryComment = {
                PortalId: portalID,
                PersonHistoryId: self.historyId,
                CommentText: my.converter.makeHtml(self.newCommentHistory().trim()),
                Locked: false,
                CreatedByUser: userID,
                CreatedOnDate: moment().format()
            };

            $.ajax({
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                url: '/desktopmodules/riw/api/people/updateHistoryComment',
                data: JSON.stringify({
                    dto: PersonHistoryComment,
                    messageIndex: index,
                    connId: my.hub.connection.id
                })
            }).done(function (commentData) {
                if (commentData.Result.indexOf("success") !== -1) {
                    my.vm.filteredHistories()[index].historyComments.unshift(new my.HistoryComment(commentData.PersonHistoryComment));
                    self.newCommentHistory(null);
                    //$().toastmessage('showSuccessToast', 'Comentário adicionado &#226; mensagem com successo!');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Coment&#225;rio adicionado.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                } else {
                    //$().toastmessage('showErrorToast', data.Result);
                    $.pnotify({
                        title: 'Erro!',
                        text: commentData.Result,
                        type: 'error',
                        icon: 'fa fa-times-circle fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                }
            }).fail(function (jqXHR, textStatus) {
                console.log(jqXHR.responseText);
            }).always(function () {
                e.currentTarget.value = 'Comentar';
                e.currentTarget.disabled = false;
                //my.getMoreComments();
            });
        };
        if (data.HistoryComments) {
            var mappedPosts = $.map(data.HistoryComments, function (item) { return new my.HistoryComment(item); });
            self.historyComments(mappedPosts);
        }
        self.toggleComment = function (item, event) {
            $(event.target).next().find('.publishComment').toggle();
            $(event.target).next().find('.commentTextArea').focus();
        }
    };

    my.HistoryComment = function (data) {
        var self = this;
        data = data || {};

        self.commentedByAvatar = data.Avatar ? '/portals/0/' + data.Avatar + '?w=45&h=45&mode=crop' : '/desktopmodules/rildoinfo/webapi/content/images/user.png?w=45&h=45';
        self.commentedByName = data.DisplayName;
        self.historyId = data.PersonHistoryId;
        self.commentId = data.CommentId;
        self.commentText = data.CommentText;
        self.createdOnDate = moment(data.CreatedOnDate).fromNow();
    };

    my.PersonContact = function () {
        this.contactEmail = ko.observable();
        this.contactName = ko.observable();
        this.contactPhone = ko.observable();
    };

    //my.PortalFolder = function() {
    //    this.FolderID = ko.observable();
    //    this.FolderName = ko.observable();
    //    this.FolderPath = ko.observable();
    //};

    my.PortalFile = function () {
        this.fileId = ko.observable();
        this.fileName = ko.observable();
        this.fileSize = ko.observable();
        this.contenType = ko.observable();
        this.extension = ko.observable();
        this.relativePath = ko.observable();
        this.width = ko.observable();
        this.height = ko.observable();
    };

    my.Product = function () {
        this.prodId = ko.observable();
        this.prodRef = ko.observable();
        this.prodBarCode = ko.observable();
        this.prodName = ko.observable();
        this.displayName = ko.observable();
        this.prodIntro = ko.observable();
        this.prodImageId = ko.observable();
        this.extension = ko.observable();
        this.unitValue = ko.observable();
    };

    //my.Group = function () {
    //    this.value = ko.observable();
    //    this.text = ko.observable();
    //};

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

        self.personHistories = ko.observableArray([]),
        self.filterHistoryTerm = ko.observable(''),
        self.filteredHistories = ko.dependentObservable(function () {
            var filter = this.filterHistoryTerm().toLowerCase();
            if (!filter) {
                return self.personHistories();
            } else {
                return ko.utils.arrayFilter(self.personHistories(), function (item) {
                    return ko.utils.arrayFilter([item.historyText.toLowerCase()], function (str) {
                        return str.indexOf(filter) !== -1;
                    }).length > 0;
                });
            }
        }, self),

        self.pushHistory = function (item) {
            self.personHistories.unshift(new my.PersonHistory()
                .historyByAvatar = item.Avatar ? '/portals/0/' + item.Avatar + '?w=45&h=45&mode=crop' : '/desktopmodules/rildoinfo/webapi/content/images/user.png?w=45&h=45'
                .historyByName = item.DisplayName
                .historyId = item.PersonHistoryId
                .historyText = item.HistoryText
                .createdOnDate = item.CreatedOnDate
                .allowed = item.Allowed);
        },

        //self.personHistories = ko.observableArray([]),
        //self.filterHistoryTerm = ko.observable(""),
        //self.filteredHistories = ko.dependentObservable(function () {
        //    var filter = this.filterHistoryTerm().toLowerCase();
        //    if (!filter) {
        //        return self.personHistories();
        //    } else {
        //        return ko.utils.arrayFilter(self.personHistories(), function (item) {
        //            return ko.utils.arrayFilter([item.historyText.toLowerCase()], function (str) {
        //                return str.indexOf(filter) !== -1;
        //            }).length > 0;
        //        });
        //    }
        //}, self),

        self.filter = ko.observable(''),
        self.siteAddress = ko.computed(function () {
            return amplify.store.sessionStorage('storeAddress') + (amplify.store.sessionStorage('storeUnit').length > 0 ? ' Nº ' + amplify.store.sessionStorage('storeUnit') : '') + '<br />' + amplify.store.sessionStorage('storeComplement') + '<br />' + amplify.store.sessionStorage('storeCity') + ' - ' + my.formatPostalcode(amplify.store.sessionStorage('storePostalCode')) + '<br />' + amplify.store.sessionStorage('storeRegion') + ' - ' + amplify.store.sessionStorage('storeCountry');
        }),
        self.personType = ko.observable('1'),
        self.firstName = ko.observable(),
        self.lastName = ko.observable(''),
        self.displayName = ko.observable(''),
        self.personId = ko.observable(0),
        self.personFullName = ko.computed(function () {
            return firstName() + ' ' + lastName();
        }, self),
        self.personUserId = ko.observable(0),
        self.userName = ko.observable(''),
        self.originalUserName = ko.observable(''),
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
        self.reasonBlocked = ko.observable(''),
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
        self.selectedSalesRepId = ko.observable(),
        self.loadSaleReps = function () {
            $.getJSON('/desktopmodules/riw/api/people/GetUsersByRoleName?portalId=' + portalID + '&roleName=Vendedores', function (data) {
                if (data) {
                    self.saleReps.removeAll();
                    //var mappedSaleReps = $.map(data, function (item) { return new my.SalesPerson(item); });
                    //self.saleReps(mappedSaleReps);
                    $.each(data, function (i, sr) {
                        self.saleReps.push(ko.mapping.fromJS(sr));
                    });
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
        self.askLastName = ko.observable(JSON.parse(amplify.store.sessionStorage('askLastName').toLowerCase())),
        self.askIndustry = ko.observable(JSON.parse(amplify.store.sessionStorage('askIndustry').toLowerCase())),
        self.askTelephone = ko.observable(JSON.parse(amplify.store.sessionStorage('askTelephone').toLowerCase())),
        self.askEIN = ko.observable(JSON.parse(amplify.store.sessionStorage('askEIN').toLowerCase())),
        self.askCompany = ko.observable(JSON.parse(amplify.store.sessionStorage('askCompany').toLowerCase())),
        self.askST = ko.observable(JSON.parse(amplify.store.sessionStorage('askST').toLowerCase())),
        self.askCT = ko.observable(JSON.parse(amplify.store.sessionStorage('askCT').toLowerCase())),
        self.askWebsite = ko.observable(JSON.parse(amplify.store.sessionStorage('askWebsite').toLowerCase())),
        self.askSSN = ko.observable(JSON.parse(amplify.store.sessionStorage('askSSN').toLowerCase())),
        self.askIdent = ko.observable(JSON.parse(amplify.store.sessionStorage('askIdent').toLowerCase())),
        self.askAddress = ko.observable(JSON.parse(amplify.store.sessionStorage('askAddress').toLowerCase())),

        // required
        self.reqEmail = ko.observable(false),
        self.reqLastName = ko.observable(JSON.parse(amplify.store.sessionStorage('reqLastName').toLowerCase())),
        self.reqSSN = ko.observable(JSON.parse(amplify.store.sessionStorage('reqSSN').toLowerCase())),
        self.reqIdent = ko.observable(JSON.parse(amplify.store.sessionStorage('reqIdent').toLowerCase())),
        self.reqTelephone = ko.observable(JSON.parse(amplify.store.sessionStorage('reqTelephone').toLowerCase())),
        self.reqWebsite = ko.observable(JSON.parse(amplify.store.sessionStorage('reqWebsite').toLowerCase())),
        self.reqCompanyName = ko.observable(false),
        self.reqEIN = ko.observable(JSON.parse(amplify.store.sessionStorage('reqEIN').toLowerCase())),
        self.reqST = ko.observable(JSON.parse(amplify.store.sessionStorage('reqST').toLowerCase())),
        self.reqCT = ko.observable(JSON.parse(amplify.store.sessionStorage('reqCT').toLowerCase())),
        self.reqIndustry = ko.observable(JSON.parse(amplify.store.sessionStorage('reqIndustry').toLowerCase())),
        self.reqAddress = ko.observable(JSON.parse(amplify.store.sessionStorage('reqAddress').toLowerCase())),

        //self.formatResult = function (data) {
        //    return "<div class='select2-user-result'>" + data.IndustryTitle + "</div>";
        //},

        //self.formatSelection = function (data) {
        //    return data.IndustryId;
        //},

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
            var result = amplify.store.sessionStorage('passwordSubject').replace('[WEBSITE]', siteName);
            return result;
        }, self),

        self.passwordBody = ko.computed(function () {
            var result = amplify.store.sessionStorage('passwordMessage').replace('[CLIENTE]', firstName() + ' ' + lastName()).replace('[WEBSITE1]', siteName).replace('[HTTPURL]', siteURL).replace('[URL]', siteURL).replace('[WEBSITE2]', siteName);
            return result;
        }, self),

        self.personAddressId = ko.observable(0),
        self.personContactId = ko.observable(0),
        self.clientPRId = ko.observable(0),
        self.clientBRId = ko.observable(0),
        self.clientCRId = ko.observable(0),
        self.clientPartnerId = ko.observable(0),
        self.clientPBRId = ko.observable(0),
        self.clientISId = ko.observable(0),

        self.selectedProducts = ko.observableArray([]),

        self.personContactsData = ko.observableArray([]),
        self.selectedContacts = ko.observableArray([]),
        self.contacts = ko.observable(),
        self.loadContacts = function () {
            $.ajax({
                //async: false,
                url: '/desktopmodules/riw/api/people/GetPersonContacts?personId=' + self.personId()
            }).done(function (data) {
                self.personContactsData.removeAll();
                if (self.email().length > 0) {
                    self.personContactsData.push(new my.PersonContact().contactEmail(self.email()).contactName(self.displayName()).contactPhone(my.formatPhone(self.telephone())));
                }
                if (data.length > 0) {
                    //self.personContactsData.push(new my.PersonContact().contactEmail('Ninguem').contactName('Selecionar').contactPhone(''));
                    //if (self.email().length > 0) {
                    //    self.personContactsData.push(new my.PersonContact().contactEmail(self.email()).contactName(self.displayName()).contactPhone(my.formatPhone(self.telephone())));
                    //}
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].ContactEmail1.length > 0) {
                            self.personContactsData.push(new my.PersonContact().contactEmail(data[i].ContactEmail1).contactName(data[i].ContactName).contactPhone(my.formatPhone(data[i].ContactPhone1)));
                        }
                    }
                }
                if (self.personContactsData()[0]) {
                    self.selectedContacts.push(self.personContactsData()[0].contactEmail());
                }
            });
            //$.getJSON('/desktopmodules/riw/api/clients/GetPersonContacts?cId=' + my.vm.personId(), function (data) {
            //    if (data) {
            //        self.clientContacts.removeAll();
            //        self.clientContacts.push(new my.PersonContact().contactEmail(self.email()).contactName(self.displayName()).contactPhone(my.formatPhone(self.telephone())));
            //        for (var i = 0; i < data.length; i++) {
            //            self.clientContacts.push(new my.PersonContact().contactEmail(data[i].ContactEmail1).contactName(data[i].ContactName).contactPhone(my.formatPhone(data[i].ContactPhone1)));
            //        }
            //    }
            //});
        },

        self.formatResult = function (data) {
            //return '<div class="select2-user-result">' + data.contactName + '</div>';
            return '<span title="' + data.id +'">' + data.text + '</span>';
        },
        self.formatSelection = function (data) {
             return data.text;
            //return '<div class="select2-user-result">' + data.text + '</div>';
        },

        self.selectedFolderPath = ko.observable('Images/'),
        self.portalFiles = ko.observableArray([]),
        self.loadPortalFiles = function () {
            $.ajax({
                url: '/desktopmodules/riw/api/store/GetPortalFiles?portalId=' + portalID + '&folder=' + self.selectedFolderPath()
            }).done(function (data) {
                if (data) {
                    if (data.length > 0) {
                        self.portalFiles.removeAll();
                        $.each(data, function (i, f) {
                            self.portalFiles.push(new my.PortalFile()
                                .fileId(f.FileId)
                                .fileName(f.FileName)
                                .contenType(f.ContentType)
                                .extension(f.Extension)
                                .fileSize('Tamanho: ' + my.size_format(kendo.parseInt(f.FileSize)))
                                .height(kendo.parseInt(f.Height))
                                .relativePath(f.Extension.toLowerCase() === 'jpg' || f.Extension.toLowerCase() === 'png' || f.Extension.toLowerCase() === 'gif' || f.Extension.toLowerCase() === 'bmp' ? '/portals/' + portalID + '/' + f.RelativePath : '/desktopmodules/rildoinfo/webapi/content/images/spacer.gif')
                                .width(kendo.parseInt(f.Width)));
                        });
                    } else {
                        self.portalFiles.removeAll();
                        setTimeout(function () {
                            $.post('/desktopmodules/riw/api/store/SyncPortalFolders?portalId=' + portalID + '&folder=', function (data) { });
                        });
                    }
                }
            });
        },

        self.fileSearch = ko.observable(""),
        self.filteredPortalFiles = ko.dependentObservable(function () {
            var filter = this.fileSearch().toLowerCase();
            if (!filter) {
                return self.portalFiles();
            } else {
                return ko.utils.arrayFilter(self.portalFiles(), function (item) {
                    return ko.utils.arrayFilter([item.fileName().toLowerCase()], function (str) {
                        return str.indexOf(filter) !== -1;
                    }).length > 0;
                });
            }
        }, self),

        self.selectedFileId = ko.observable(),

        self.prodId = ko.observable(),
        self.prodRef = ko.observable(),
        self.prodBarCode = ko.observable(),
        self.prodName = ko.observable(),
        self.displayName = ko.observable(),
        self.prodIntro = ko.observable(),
        self.unitValue = ko.observable(),
        self.prodImageId = ko.observable(),
        self.selectedProductId = ko.observable(),
        self.extension = ko.observable(),

        self.sentProductsHistory = ko.computed(function () {
            var productList = '';
            $.each(selectedProducts(), function (i, p) {
                if (p.prodName() !== null)
                    my.productList += '<br />' + p.prodName() + ' <strong>CB:</strong> (' + p.prodBarCode() + ') <strong>REF:</strong> (' + p.prodRef() + ')';
            });
            return productList;
        }, self),

        self.sentProductsBody = ko.computed(function () {
            var productList = '';
            $.each(selectedProducts(), function (i, p) {
                if (p.prodName() !== null) {
                    var imagePath = p.prodImageId() > 0 ? '<img src="http://' + siteURL + '/databaseimages/' + p.prodImageId() + '.' + p.extension() + '?width=80&height=80" />' : '<img src="http://' + siteURL + '/desktopmodules/rildoinfo/webapi/content/images/No-Image.jpg?width=80&height=80" />';
                    productList += '<table style="padding: 3px; margin: 3px"><tr><td colspan="2"><h4><a href="' + productsURL + '#itemId/' + p.prodId() + '">' + p.prodName() + '</a></h4></td></tr><tr><td>' + imagePath + '</td><td>' + p.prodIntro() + '</td></tr></table>';
                }
            });
            return productList;
        }, self),

        self.productMessage = ko.computed(function() {
            'Caro(a) ' + self.personFullName() + ', requisitou este email de <strong>' + siteName + '</strong><br/><br />' +
            'Veja abaixo o(s) item(ns) requisitado(s).<br/><br />' + '{PRODUTOS}<br/><br />' +
            'Descubra mais sobre estes e outros produtos no website <a href="http://' + siteURL + '" title="' + siteName + '">' + siteURL + '</a><br/><br />' +
            'Obrigado pelo interesse em nossos produtos.';
        }),

        self.statuses = ko.observableArray([]),
        self.selectedStatusId = ko.observable(1),
        self.loadStatuses = function () {
            $.getJSON('/desktopmodules/riw/api/statuses/GetStatuses?portalId=' + portalID + '&isDeleted=False', function (data) {
                self.statuses.removeAll();
                $.each(data, function (i, st) {
                    self.statuses.push(ko.mapping.fromJS(st));
                    //self.statuses.push(st.StatusTitle);
                });
            });
        },

        self.groups = ko.observableArray([]),
        self.selectedGroup = ko.observable(),
        self.loadGroups = function () {
            $.ajax({
                type: 'GET',
                url: '/desktopmodules/riw/api/store/getRolesByRoleGroup?portalId=' + portalID + '&roleGroupName=Departamentos'
            }).done(function (data) {
                if (data) {
                    $.each(data, function (i, r) {
                        self.groups.push(ko.mapping.fromJS(r));
                    });
                    self.selectedGroup(kendo.parseInt(my.getQuerystring('group', my.getParameterByName('group'))));
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
        },

        //self.registerTypes = ko.observableArray([]);
        //self.selectedType = ko.observable(),
        //self.loadEntitiesRoles = function () {
        //    $.ajax({
        //        type: 'GET',
        //        url: '/desktopmodules/riw/api/store/getRolesByRoleGroup?portalId=' + portalID + '&roleGroupName=Entidades,Descontos'
        //    }).done(function (data) {
        //        if (data) {
        //            $.each(data, function (i, r) {
        //                self.registerTypes.push(ko.mapping.fromJS(r));
        //            });
        //            self.selectedType(self.selectedType());
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

        self.userAvatar = ko.observable(''),        
        self.deleteds = ko.observable(0);

        //ko.bindingHandlers.kendoGrid.options = {
        //    pageable: {
        //        messages: {
        //            display: '{0} - {1} de {2} itens',
        //            empty: 'Nenhum produto disponível.',
        //            page: 'Página',
        //            of: 'de {0}',
        //            itemsPerPage: 'itens por página',
        //            first: 'Ir para primeira página',
        //            previous: 'Ir para página anterior',
        //            next: 'Ir para próxima página',
        //            last: 'Ir para última página',
        //            refresh: 'Recarregar'
        //        }
        //    }
        //};

        //ko.bindingHandlers.kendoGrid.options = {
        //    sortable: true,
        //    pageable: false
        //};

        return {
            sortJsonName: sortJsonName,
            personHistories: personHistories,
            filterHistoryTerm: filterHistoryTerm,
            filteredHistories: filteredHistories,
            filter: filter,
            siteAddress: siteAddress,
            personType: personType,
            firstName: firstName,
            lastName: lastName,
            displayName: displayName,
            personId: personId,
            personFullName: personFullName,
            personUserId: personUserId,
            userName: userName,
            originalUserName: originalUserName,
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
            sent: sent,
            bio: bio,
            income: income,
            credit: credit,
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
            selectedContactAddress: selectedContactAddress,
            loadAddresses: loadAddresses,
            passwordSubject: passwordSubject,
            passwordBody: passwordBody,
            personAddressId: personAddressId,
            personContactId: personContactId,
            clientPRId: clientPRId,
            clientBRId: clientBRId,
            clientCRId: clientCRId,
            clientPartnerId: clientPartnerId,
            clientPBRId: clientPBRId,
            clientISId: clientISId,
            selectedProducts: selectedProducts,
            personContactsData: personContactsData,
            selectedContacts: selectedContacts,
            contacts: contacts,
            loadContacts: loadContacts,
            selectedFolderPath: selectedFolderPath,
            portalFiles: portalFiles,
            loadPortalFiles: loadPortalFiles,
            fileSearch: fileSearch,
            filteredPortalFiles: filteredPortalFiles,
            selectedFileId: selectedFileId,
            prodId: prodId,
            prodRef: prodRef,
            prodBarCode: prodBarCode,
            prodName: prodName,
            prodIntro: prodIntro,
            unitValue: unitValue,
            prodImageId: prodImageId,
            selectedProductId: selectedProductId,
            extension: extension,
            sentProductsHistory: sentProductsHistory,
            sentProductsBody: sentProductsBody,
            productMessage: productMessage,
            statuses: statuses,
            selectedStatusId: selectedStatusId,
            loadStatuses: loadStatuses,
            groups: groups,
            selectedGroup: selectedGroup,
            loadGroups: loadGroups,
            //registerTypes: registerTypes,
            //selectedType: selectedType,
            //loadEntitiesRoles: loadEntitiesRoles,
            userAvatar: userAvatar,
            deleteds: deleteds,
            reasonBlocked: reasonBlocked
        };

    }();

    //ko.bindingHandlers.kendoTabStrip.animation = true;

    //$('#clientFinanceTabs').kendoTabStrip({
    //    animation: {
    //        // fade-out current tab over 1000 milliseconds
    //        close: {
    //            effects: "fadeOut"
    //        },
    //        // fade-in new tab over 500 milliseconds
    //        open: {
    //            effects: "fadeIn"
    //        }
    //    }
    //});

    ko.bindingHandlers.kendoGrid.options = {
        dataBound: function () {
            if (this.dataSource.view().length > 0) $('#selectedProductsGrid').show();
        }
    };

    ko.applyBindings(my.vm);

};
