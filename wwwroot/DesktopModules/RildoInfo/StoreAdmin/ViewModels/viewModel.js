
my.viewModel = function () {

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
        this.MemberId = ko.observable(data.MemberId);
    };

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

    // knockout js view model
    my.vm = function () {
        // this is knockout view model
        var self = this;

        self.sortJsonName = function (field, reverse, primer) {
            var key = primer ? function (x) { return primer(x[field]); } : function (x) { return x[field]; };
            return function (b, a) {
                var A = key(a), B = key(b);
                return ((A < B) ? -1 : (A > B) ? +1 : 0) * [-1, 1][+!!reverse];
            };
        },

        self.siteName = ko.observable(siteName),
        self.description = ko.observable(description),
        self.keywords = ko.observable(keywords),
        self.pageSize = ko.observable(pageSize),
        self.watermark = ko.observable(watermark),
        self.postalCode = ko.observable(storePostalCode),
        self.street = ko.observable(storeAddress),
        self.unit = ko.observable(storeUnit),
        self.complement = ko.observable(storeComplement),
        self.district = ko.observable(storeDistrict),
        self.city = ko.observable(storeCity),
        self.country = ko.observable(storeCountry),
        self.email = ko.observable(storeEmail),
        self.replyEmail = ko.observable(storeReplyEmail),
        self.phone1 = ko.observable(storePhone1),
        self.phone2 = ko.observable(storePhone2),
        self.hasRegions = ko.observable(false),
        self.estimateMaxDiscount = ko.observable(estimateMaxDiscount),
        self.estimateMaxDuration = ko.observable(estimateMaxDuration),
        //self.term = ko.observable(estimateTerm),

        self.countries = ko.observableArray([]),
        self.selectedCountry = ko.observable(storeCountry),
        self.loadCountries = function () {
            $.getJSON('/desktopmodules/riw/api/lists/countries', function (data) {
                data.sort(self.sortJsonName('Text', false, function (a) { return a.toUpperCase(); }));
                var mappedCountries = $.map(data, function (item) { return new my.Country(item); });
                self.countries(mappedCountries);
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
                    var mappedRegions = $.map(data, function (item) {
                        return new my.Region(item);
                    });
                    self.regions(mappedRegions);
                    self.hasRegions(true);
                    if (storeRegion.length > 1) {
                        self.selectedRegion(storeRegion);
                    }
                } else {
                    self.selectedRegion('Minas Gerais')
                }
            });
        }),

        //self.saleReps = ko.observableArray([]),
        //self.selectedSalesRepId = ko.observable(parseInt(salesPerson)),
        //self.loadSaleReps = function () {
        //    $.getJSON('/desktopmodules/riw/api/people/GetUsersByRoleName?portalId=' + portalID + '&roleName=Vendedores', function (data) {
        //        if (data) {
        //            self.saleReps.removeAll();
        //            var mappedSaleReps = $.map(data, function (item) { return new my.SalesPerson(item); });
        //            self.saleReps(mappedSaleReps);
        //        }
        //    });
        //},

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

        self.payCondType = ko.observable(),
        self.server = ko.observable(smtpServer),
        self.port = ko.observable(smtpPort),
        self.login = ko.observable(smtpLogin),
        self.password = ko.observable(smtpPassword),
        self.connection = ko.observable(JSON.parse(smtpConnection.toLowerCase())),

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
        self.totalUnreadMessages = ko.observable(),
        self.totalUnreadNotifications = ko.observable(),
        self.totalEstimatesOpened = ko.observable(),
        self.totalClients = ko.observable(),
        self.totalSales = ko.observable(),

        // indicators
        self.debit4Seen = ko.observable(0),
        self.debitActual = ko.observable(0),
        self.debitBalance = ko.computed(function () {
            return self.debit4Seen() - self.debitActual();
        }),
        self.credit4Seen = ko.observable(0),
        self.creditActual = ko.observable(0),
        self.creditBalance = ko.computed(function () {
            var result = 0;
            if (self.credit4Seen() >= self.creditActual()) {
                result = self.credit4Seen() - self.creditActual();
            }
            return result;
        }),
        self.totalProductSales = ko.observable(0),
        self.totalServiceSales = ko.observable(0),
        self.totalSalesBalance = ko.computed(function () {
            return self.totalProductSales() + self.totalServiceSales();
        }),
        self.totalProductsEstimates = ko.observable(0),
        self.totalServicesEstimates = ko.observable(0),
        self.totalEstimatesBalance = ko.computed(function () {
            return self.totalProductsEstimates() + self.totalServicesEstimates();
        });

        // make view models available for apps
        return {
            pageSize: pageSize,
            siteName: siteName,
            description: description,
            keywords: keywords,
            watermark: watermark,
            postalCode: postalCode,
            street: street,
            unit: unit,
            complement: complement,
            district: district,
            city: city,
            country: country,
            email: email,
            replyEmail: replyEmail,
            phone1: phone1,
            phone2: phone2,
            hasRegions: hasRegions,
            estimateMaxDiscount: estimateMaxDiscount,
            estimateMaxDuration: estimateMaxDuration,
            //term: term,
            countries: countries,
            selectedCountry: selectedCountry,
            loadCountries: loadCountries,
            regions: regions,
            selectedRegion: selectedRegion,
            loadRegions: loadRegions,
            //saleReps: saleReps,
            //selectedSalesRepId: selectedSalesRepId,
            //loadSaleReps: loadSaleReps,
            industries: industries,
            selectedIndustries: selectedIndustries,
            loadIndustries: loadIndustries,
            payCondType: payCondType,
            server: server,
            port: port,
            login: login,
            password: password,
            connection: connection,
            fileSearch: fileSearch,
            filteredPortalFiles: filteredPortalFiles,
            selectedFolderPath: selectedFolderPath,
            portalFiles: portalFiles,
            loadPortalFiles: loadPortalFiles,
            selectedFileId: selectedFileId,
            totalUnreadMessages: totalUnreadMessages,
            totalUnreadNotifications: totalUnreadNotifications,
            totalEstimatesOpened: totalEstimatesOpened,
            totalClients: totalClients,
            totalSales: totalSales,
            debit4Seen: debit4Seen,
            debitActual: debitActual,
            debitBalance: debitBalance,
            credit4Seen: credit4Seen,
            creditActual: creditActual,
            creditBalance: creditBalance,
            totalProductSales: totalProductSales,
            totalServiceSales: totalServiceSales,
            totalSalesBalance: totalSalesBalance,
            totalProductsEstimates: totalProductsEstimates,
            totalServicesEstimates: totalServicesEstimates,
            totalEstimatesBalance: totalEstimatesBalance
        };

    }();

    //ko.bindingHandlers.bootstrapSwitchOn = {
    //    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
    //        var $elem = $(element);
    //        $(element).bootstrapSwitch('setState', ko.utils.unwrapObservable(valueAccessor())); // Set intial state
    //        $elem.on('switch-change', function (e, data) {
    //            valueAccessor()(data.value);
    //        }); // Update the model when changed.
    //    },
    //    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
    //        var vStatus = $(element).bootstrapSwitch('status');
    //        var vmStatus = ko.utils.unwrapObservable(valueAccessor());
    //        if (vStatus !== vmStatus) {
    //            $(element).bootstrapSwitch('setState', vmStatus);
    //        }
    //    }
    //};

    ko.applyBindings(my.vm);
};