$(function () {

    my.record = 0;

    my.viewModel();

    $('#qTyTextBox').kendoNumericTextBox({
        placeholder: 'Qde',
        value: 1,
        spinners: false,
        min: 1
    });

    $("#productSearch").select2({
        placeholder: "Busque produtos por nome, *ref. ou #c&#243;d. de barra.",
        allowClear: true,
        minimumInputLength: 2,
        id: function (data) {
            return {
                ProductId: data.ProductId,
                ProductName: data.ProductName,
                Summary: data.Suammry,
                ProductRef: data.ProductRef,
                Barcode: data.Barcode,
                UnitValue: data.UnitValue,
                ProductsRelatedCount: data.ProductsRelatedCount,
                ProductImageId: data.ProductImageId,
                Extension: data.Extension,
                CategoriesNames: data.CategoriesNames,
                ProductUnit: data.ProductUnit,
                QtyStockSet: data.QtyStockSet,
                Finan_Cost: data.Finan_Cost,
                Finan_Rep: data.Finan_Rep,
                Finan_SalesPerson: data.Finan_SalesPerson,
                Finan_Tech: data.Finan_Tech,
                Finan_Telemarketing: data.Finan_Telemarketing,
                Finan_Manager: data.Finan_Manager
            };
        },
        ajax: {
            url: "/desktopmodules/riw/api/products/getproducts",
            quietMillis: 100,
            data: function (term, page) { // page is the one-based page number tracked by Select2

                var fieldName = term;

                return {
                    portalId: 0,
                    searchField: fieldName.charAt(0) === '*' ? 'ProductRef' : fieldName.charAt(0) === '#' ? 'BarCode' : 'ProductName',
                    searchString: fieldName.charAt(0) === '*' || fieldName.charAt(0) === '#' ? fieldName.slice(1) : fieldName,
                    pageIndex: page,
                    pageSize: 10,
                    orderBy: 'ProductName',
                    orderDesc: 'ASC'
                };
            },
            results: function (data, page) {
                var more = (page * 10) < data.total; 
                return { results: data.data, more: more };
            }
        },
        formatResult: productFormatResult, 
        formatSelection: productFormatSelection, 
        //dropdownCssClass: "bigdrop", 
        escapeMarkup: function (m) { return m; } 
    });

    $('#productSearch').on("select2-selecting", function (e) {
        if (e.val.ProductUnit === 1) {
            $('#qTyTextBox').data('kendoNumericTextBox').options.format = '';
            setTimeout(function () {
                $('#btnAddSelectedProduct').focus();
            }, 100);
        }
        $('#qTyTextBox').data('kendoNumericTextBox').value(1);
    });

    $('#btnAddSelectedProduct').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        my.vm.addItem(1, $('#productSearch').select2('data'));
    });

});

function productFormatResult(data) {
    var markup = '<table class="product-result Normal"><tr>';
    if (data.ProductImageId > 0) {
        markup += '<td class="product-image"><img src="/databaseimages/' + data.ProductImageId + '.' + data.Extension + '?maxwidth=60&maxheight=60&s.roundcorners=10" /></td>';
    } else {
        markup += '<td class="product-image"><img class="img-rounded" src="/portals/0/images/No-Image.jpg?maxwidth=60&maxheight=60&s.roundcorners=10" /></td>';
    }
    markup += "<td class='product-info'><div class='product-title'>" + data.ProductName + "</div>";
    if (data.Barcode) {
        markup += "<div><strong>CB: </strong>" + data.Barcode + "</div>";
    } else if (data.ProductRef) {
        markup += "<div><strong>REF: </strong>" + data.ProductRef + "</div>";
    }
    markup += "</td><td class='product-price'> " + kendo.toString(data.UnitValue, 'c');
    markup += "</td></tr></table>"
    return markup;
}

function productFormatSelection(data) {
    return data.ProductName;
}