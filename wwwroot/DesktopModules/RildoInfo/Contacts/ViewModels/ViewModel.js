
my.viewModel = function () {

    my.Country = function (data) {
        this.CountryCode = ko.observable(data.CountryCode);
        this.CountryName = ko.observable(data.CountryName);
    };

    my.Region = function (data) {
        this.RegionCode = ko.observable(data.RegionCode);
        this.RegionName = ko.observable(data.RegionName);
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
        self.selectedRegion = ko.observable('Minas Gerais'),
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
                }
            });
            // $('#firstNameTextBox').focus();
        }),

        self.smtpConnection = ko.observable(_smtpConnection),
        self.methodType = ko.observable(),
        self.reqMethod = ko.observable(_reqMethod),
        self.reqAddress = ko.observable(_reqAddress);
        
        // make view models available for apps
        return {
            countries: countries,
            selectedCountry: selectedCountry,
            loadCountries: loadCountries,
            regions: regions,
            selectedRegion: selectedRegion,
            loadRegions: loadRegions
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