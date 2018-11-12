
my.viewModel = function () {

    // knockout js view model
    my.vm = function () {
        // this is knockout view model
        var self = this;

        self.sql = ko.observable(''),
        self.sqlDetail = ko.observable('');
        
        // make view models available for apps
        return {
            sql: sql,
            sqlDetail: sqlDetail
        };

    }();

    ko.applyBindings(my.vm);

};