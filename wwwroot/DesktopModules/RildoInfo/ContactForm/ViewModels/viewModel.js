
my.viewModel = function () {

    my.Country = function (data) {
        this.Value = ko.observable(data.CountryCode);
        this.Text = ko.observable(data.CountryName);
    };

    my.Region = function (data) {
        this.Value = ko.observable(data.RegionCode);
        this.Text = ko.observable(data.RegionName);
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

        self.hasRegions = ko.observable(false);
        self.regions = ko.observableArray([]),
        self.selectedRegion = ko.observable(0),
        self.loadRegions = ko.computed(function () {
            if (!self.selectedCountry()) {
                return null;
            }
            $.getJSON('/desktopmodules/riw/api/lists/regions?code=' + self.selectedCountry(), function (data) {
                if (data.length) {
                    self.regions.removeAll();
                    var mappedRegions = $.map(data, function (item) { return new my.Region(item); });
                    self.regions(mappedRegions);
                    if (self.regions().length > 0) {
                        self.hasRegions(true);
                        self.selectedRegion('MG');
                    } else {
                        self.selectedRegion('Minas Gerais');
                    }
                }
            });
        }),

        self.reqTelephone = ko.observable(JSON.parse(_reqTelephone.toLowerCase())),
        self.emailMethodType = ko.observable(false),
        self.pOMethodType = ko.observable(false),
        self.sendTo = ko.observable(_sendTo),
        self.reqSend = ko.observable(JSON.parse(_reqSend.toLowerCase())),
        self.reqAddress = ko.observable(JSON.parse(_reqAddress.toLowerCase())),
        self.smtpServer = ko.observable(_smtpServer),
        self.smtpPort = ko.observable(_smtpPort),
        self.smtpLogin = ko.observable(_smtpLogin)
        self.smtpPassword = ko.observable(_smtpPassword),
        self.smtpConnection = ko.observable(JSON.parse(_smtpConnection.toLowerCase()));

        // make view models available for apps
        return {
            reqTelephone: reqTelephone,
            countries: countries,
            selectedCountry: selectedCountry,
            loadCountries: loadCountries,
            regions: regions,
            hasRegions: hasRegions,
            selectedRegion: selectedRegion,
            loadRegions: loadRegions,
            sendTo: sendTo,
            emailMethodType: emailMethodType,
            pOMethodType: pOMethodType,
            reqSend: reqSend,
            reqAddress: reqAddress,
            smtpServer: smtpServer,
            smtpPort: smtpPort,
            smtpLogin: smtpLogin,
            smtpPassword: smtpPassword,
            smtpConnection: smtpConnection
        };

    }();

    ko.bindingHandlers.bootstrapSwitchOn = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var $elem = $(element);
            $(element).bootstrapSwitch('setState', ko.utils.unwrapObservable(valueAccessor())); // Set intial state
            $elem.on('switch-change', function (e, data) {
                valueAccessor()(data.value);
            }); // Update the model when changed.
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var vStatus = $(element).bootstrapSwitch('status');
            var vmStatus = ko.utils.unwrapObservable(valueAccessor());
            if (vStatus !== vmStatus) {
                $(element).bootstrapSwitch('setState', vmStatus);
            }
        }
    };

    ko.applyBindings(my.vm);
};