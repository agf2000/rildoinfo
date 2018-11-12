
my.viewModel = function () {

    // knockout js view model
    my.vm = function () {
        // this is knockout view model
        var self = this;

        self.selectedVendorId = ko.observable(-1),
        self.selectedClientId = ko.observable(-1),
        self.selectedEstimateId = ko.observable(-1),
        self.filter = ko.observable(''),
        self.selectedAccountId = ko.observable(-1),
        self.selectedAccountName = ko.observable(''),
        self.locked = ko.observable(true),
        self.selectedCategory = ko.observable(''),
        self.originalAmount = ko.observable(0),
        self.interestRate = ko.observable(0),
        self.fee = ko.observable(0),
        self.payAmount = ko.computed({
            read: function () {
                return self.originalAmount() + (self.originalAmount() / 100 * self.interestRate()) + self.fee();
            },
            write: function (value) {
                self.originalAmount(value);
                return value;
            },
            owner: this
        }),
        self.payAmountBox = ko.observable(false),
        self.selectedProductId = ko.observable(null),
        self.originalDueDate = ko.observable(),
        self.invoiceTotal = ko.observable(0),
        self.selectedOriginId = ko.observable(-1),
        self.selectedOriginName = ko.observable('');

        // make view models available for apps
        return {
            selectedVendorId: selectedVendorId,
            selectedClientId: selectedClientId,
            selectedEstimateId: selectedEstimateId,
            filter: filter,
            selectedAccountId: selectedAccountId,
            selectedAccountName: selectedAccountName,
            locked: locked,
            selectedCategory: selectedCategory,
            fee: fee,
            payAmount: payAmount,
            interestRate: interestRate,
            originalAmount: originalAmount,
            payAmountBox: payAmountBox,
            selectedProductId: selectedProductId,
            originalDueDate: originalDueDate,
            invoiceTotal: invoiceTotal,
            selectedOriginId: selectedOriginId,
            selectedOriginName: selectedOriginName
        };

    }();

    // apply ko bindings
    ko.applyBindings(my.vm);

};