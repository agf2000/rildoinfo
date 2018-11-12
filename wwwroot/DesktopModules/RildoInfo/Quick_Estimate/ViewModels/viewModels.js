

my.viewModel = function () {

    my.Product = function () {
        var self = this;

        self.productId = ko.observable();
        self.productRef = ko.observable();
        self.barcode = ko.observable();
        self.productCode = ko.observable();
        self.productName = ko.observable();
        self.summary = ko.observable();
        self.productUnit = ko.observable();
        self.prodImageId = ko.observable();
        self.unitValue = ko.observable();
        self.productDiscount = ko.observable(0);
        self.unitTypeTitle = ko.observable();
        self.itemQty = ko.observable('');
        self.increaseQuantity = function (quantity) {
            self.itemQty(self.itemQty() + quantity);
        };
        self.extendedAmount = ko.computed(function () {
            return self.itemQty() * self.unitValue();
        });
    };

    my.vm = function () {

        // knockout js view model
        var self = this;

        self.sortJsonName = function (field, reverse, primer) {
            var key = primer ? function (x) { return primer(x[field]); } : function (x) { return x[field]; };
            return function (b, a) {
                var A = key(a), B = key(b);
                return ((A < B) ? -1 : (A > B) ? +1 : 0) * [-1, 1][+!!reverse];
            };
        },
        self.items = ko.observableArray([]),
        self.selectedProducts = ko.observableArray([]),
        self.addItem = function (qty, data) {
            self.items.push(new my.Product()
                .barcode(data.Barcode)
                .unitValue(data.UnitValue)
                .itemQty(qty)
                .productCode(data.Barcode)
                .productId(data.ProductId)
                .productName(data.ProductName)
                .productRef(data.ProductRef)
                .productUnit(data.ProductUnit))
        },
        self.removeItem = function () {
            self.items.remove(this);
        };

        return {
            sortJsonName: sortJsonName,
            items: items,
            selectedProducts: selectedProducts,
            addItem: addItem,
            removeItem: removeItem
        };

    }();

    ko.applyBindings(my.vm);

};