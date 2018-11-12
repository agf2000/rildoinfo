
$(function () {

    my.productId = my.getParameterByName('itemId');
    my.onSale = false;
    $('#saleStartDate').kendoDatePicker();
    $('#saleEndDate').kendoDatePicker();

    // knockout js view model
    my.vm = function () {
        // this is knockout view model
        var self = this;

        self.finan_Paid = ko.observable(0),
        self.finan_DiffICMS = ko.observable(0),
        self.finan_Paid_Discount = ko.observable(0),
        self.finan_Freight = ko.observable(0),
        self.finan_IPI = ko.observable(0),
        self.finan_TribSubICMS = ko.observable(0),
        self.finan_ISS = ko.observable(0),
        self.finan_MarkUp = ko.observable(0),
        self.finan_OtherExpenses = ko.observable(0),
        self.finan_Sale_Price = ko.observable(0),
        self.finan_Special_Price = ko.observable(0),
        self.finan_Dealer_Price = ko.observable(0),
        self.finan_Rep = ko.observable(0),
        self.finan_Manager = ko.observable(0),
        self.finan_SalesPerson = ko.observable(0),
        self.finan_Tech = ko.observable(0),
        self.finan_Telemarketing = ko.observable(0),
        self.finan_OtherTaxes = ko.observable(0),
        self.finan_ICMSFreight = ko.observable(0),
        self.finan_Select = ko.observable(false),
        self.finan_PaidDiscountV = ko.observable(0),
        self.finan_FreightV = ko.observable(0),
        self.finan_ICMSFreightV = ko.observable(0),
        self.finan_OtherExpensesV = ko.observable(0),
        self.finan_IPIV = ko.observable(0),
        self.finan_DiffICMSV = ko.observable(0),
        self.finan_TribSubICMSV = ko.observable(0),
        self.finan_ISSV = ko.observable(0),
        self.finan_OtherTaxesV = ko.observable(0),
        self.netCost = ko.observable(0),
        self.profit = ko.observable(0),
        self.profitV = ko.observable(0),
        self.managerCommV = ko.observable(0),
        self.sellerCommV = ko.observable(0),
        self.repCommV = ko.observable(0),
        self.teleCommV = ko.observable(0),
        self.techCommV = ko.observable(0);

        // make view models available for apps
        return {
            finan_Paid: finan_Paid,
            finan_DiffICMS: finan_DiffICMS,
            finan_Paid_Discount: finan_Paid_Discount,
            finan_Freight: finan_Freight,
            finan_IPI: finan_IPI,
            finan_TribSubICMS: finan_TribSubICMS,
            finan_ISS: finan_ISS,
            finan_MarkUp: finan_MarkUp,
            finan_OtherExpenses: finan_OtherExpenses,
            finan_Sale_Price: finan_Sale_Price,
            finan_Special_Price: finan_Special_Price,
            finan_Dealer_Price: finan_Dealer_Price,
            finan_Rep: finan_Rep,
            finan_Manager: finan_Manager,
            finan_SalesPerson: finan_SalesPerson,
            finan_Tech: finan_Tech,
            finan_Telemarketing: finan_Telemarketing,
            finan_OtherTaxes: finan_OtherTaxes,
            finan_ICMSFreight: finan_ICMSFreight,
            finan_Select: finan_Select,
            finan_PaidDiscountV: finan_PaidDiscountV,
            finan_FreightV: finan_FreightV,
            finan_ICMSFreightV: finan_ICMSFreightV,
            finan_OtherExpensesV: finan_OtherExpensesV,
            finan_OtherTaxesV: finan_OtherTaxesV,
            finan_IPIV: finan_IPIV,
            finan_DiffICMSV: finan_DiffICMSV,
            finan_TribSubICMSV: finan_TribSubICMSV,
            finan_ISSV: finan_ISSV,
            netCost: netCost,
            profit: profit,
            profitV: profitV,
            managerCommV: managerCommV,
            sellerCommV: sellerCommV,
            repCommV: repCommV,
            teleCommV: teleCommV,
            techCommV: techCommV
        };

    }();

    // apply ko bindings
    ko.applyBindings(my.vm);

    $('#productMenu').show();

    // product finance
    $('#divFinanceTabs').kendoTabStrip({
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

    $('#defaultBarCode').kendoDropDownList();
    //$('#defaultBarCode').data('kendoDropDownList').list.width(600);
    $('#tribSitType').kendoDropDownList();
    //$('#tribSitType').data('kendoDropDownList').list.width('auto');
    $('#pisTribSit').kendoDropDownList();
    $('#pisTribSit').data('kendoDropDownList').list.width(600);
    $('#cofinsTribSit').kendoDropDownList();
    $('#cofinsTribSit').data('kendoDropDownList').list.width(600);
    $('#ipiTribSit').kendoDropDownList();
    $('#ipiTribSit').data('kendoDropDownList').list.width(250);

    my.loadProductFinance = function (e) {

        $.ajax({
            url: '/desktopmodules/riw/api/products/GetProductFinance?productId=' + my.productId
        }).done(function (data) {
            if (data) {

                $('#cstTextBox').data('kendoNumericTextBox').value(data.Finan_CST);
                $('#icmsTextBox').data('kendoNumericTextBox').value(data.Finan_ICMS);
                $('#cfopTextBox').data('kendoNumericTextBox').value(data.Finan_CFOP);
                $('#ncmTextBox').data('kendoNumericTextBox').value(data.Finan_NCM);
                $('#defaultBarCode').data('kendoDropDownList').value(data.Finan_DefaultBarCode);
                $('#tribSitType').data('kendoDropDownList').value(data.Finan_TributeSituationType);
                $('#pisTribSit').data('kendoDropDownList').value(data.Finan_PISTributeSituation);
                $('#cofinsTribSit').data('kendoDropDownList').value(data.Finan_COFINSTributeSituation);
                $('#ipiTribSit').data('kendoDropDownList').value(data.Finan_IPITributeSituation);
                $('#pisBaseTextBox').data('kendoNumericTextBox').value(data.Finan_PISBase);
                $('#pisTextBox').data('kendoNumericTextBox').value(data.Finan_PIS);
                $('#pisTribSubBaseTextBox').data('kendoNumericTextBox').value(data.Finan_PISTributeSubBase);
                $('#pisTribSubTextBox').data('kendoNumericTextBox').value(data.Finan_PISTributeSub);
                $('#cofinsTribSubBaseTextBox').data('kendoNumericTextBox').value(data.Finan_COFINSTributeSubBase);
                $('#cofinsTribSubTextBox').data('kendoNumericTextBox').value(data.Finan_COFINSTributeSub);
                $('#cofinsBaseTextBox').data('kendoNumericTextBox').value(data.Finan_COFINSBase);
                $('#cofinsTextBox').data('kendoNumericTextBox').value(data.Finan_COFINS);

                my.vm.finan_Paid(data.Finan_Paid);
                my.vm.finan_DiffICMS(data.Finan_DiffICMS);
                my.vm.finan_Paid_Discount(data.Finan_Paid_Discount);
                my.vm.finan_Freight(data.Finan_Freight);
                my.vm.finan_IPI(data.Finan_IPI);
                my.vm.finan_TribSubICMS(data.Finan_TributeSubICMS);
                my.vm.finan_ISS(data.Finan_ISS);
                my.vm.finan_MarkUp(data.Finan_MarkUp);
                my.vm.finan_OtherExpenses(data.Finan_OtherExpenses);
                my.vm.finan_Sale_Price(data.Finan_Sale_Price);
                my.vm.finan_Special_Price(data.Finan_Special_Price);
                my.vm.finan_Dealer_Price(data.Finan_Dealer_Price);
                my.vm.finan_Rep(data.Finan_Rep);
                my.vm.finan_Manager(data.Finan_Manager);
                my.vm.finan_SalesPerson(data.Finan_SalesPerson);
                my.vm.finan_Tech(data.Finan_Tech);
                my.vm.finan_Telemarketing(data.Finan_Telemarketing);
                my.vm.finan_OtherTaxes(data.Finan_OtherTaxes);
                my.vm.finan_ICMSFreight(data.Finan_ICMSFreight);
                switch (data.Finan_Select) {
                    case "0":
                        $('#markupCheckbox').attr({ 'checked': true });
                        $('#salePriceCheckbox').attr({ 'checked': false });
                        $('#specialPriceCheckbox').attr({ 'checked': false });
                        break;
                    case "1":
                        $('#salePriceCheckbox').attr({ 'checked': true });
                        $('#markupCheckbox').attr({ 'checked': false });
                        $('#specialPriceCheckbox').attr({ 'checked': false });
                        break;
                    default:
                        my.onSale = true;
                        $('#specialPriceCheckbox').attr({ 'checked': true });
                        $('#markupCheckbox').attr({ 'checked': false });
                        $('#salePriceCheckbox').attr({ 'checked': false });
                }

                my.vm.finan_PaidDiscountV(my.vm.finan_Paid() * my.vm.finan_Paid_Discount() / 100);
                my.vm.netCost(my.vm.finan_Paid() - my.vm.finan_PaidDiscountV());
                my.vm.finan_FreightV(my.vm.netCost() * my.vm.finan_Freight() / 100);
                my.vm.finan_ICMSFreightV(my.vm.finan_ICMSFreight() * my.vm.finan_FreightV() / 100);
                my.vm.finan_OtherTaxesV(my.vm.netCost() * my.vm.finan_OtherTaxes() / 100);
                my.vm.finan_IPIV(my.vm.netCost() * my.vm.finan_IPI() / 100);
                my.vm.finan_DiffICMSV(my.vm.netCost() * my.vm.finan_DiffICMS() / 100);
                my.vm.finan_TribSubICMSV(my.vm.netCost() * my.vm.finan_TribSubICMS() / 100);
                my.vm.finan_ISSV(my.vm.netCost() * my.vm.finan_ISS() / 100);
                my.vm.finan_OtherExpensesV(my.vm.netCost() * my.vm.finan_OtherExpenses() / 100);

                var totalCost = my.vm.netCost() +
                    my.vm.finan_IPIV() +
                    my.vm.finan_FreightV() +
                    my.vm.finan_OtherExpensesV() +
                    my.vm.finan_IPIV() +
                    my.vm.finan_DiffICMSV() +
                    my.vm.finan_TribSubICMSV() +
                    my.vm.finan_ISSV() +
                    my.vm.finan_OtherTaxesV();

                var profit = 0;
                var comms = 0;
                var taxes = my.vm.finan_IPIV() +
                    my.vm.finan_OtherTaxesV() +
                    my.vm.finan_DiffICMSV() +
                    my.vm.finan_TribSubICMSV() +
                    my.vm.finan_ISSV();

                if ($('#salePriceCheckbox').is(':checked')) {

                    if (totalCost > 0) {
                        my.vm.finan_MarkUp((my.vm.finan_Sale_Price() / totalCost - 1) * 100);
                    }

                    my.vm.managerCommV(my.vm.finan_Sale_Price() * my.vm.finan_Manager() / 100);
                    my.vm.sellerCommV(my.vm.finan_Sale_Price() * my.vm.finan_SalesPerson() / 100);
                    my.vm.repCommV(my.vm.finan_Sale_Price() * my.vm.finan_Rep() / 100);
                    my.vm.teleCommV(my.vm.finan_Sale_Price() * my.vm.finan_Telemarketing() / 100);
                    my.vm.techCommV(my.vm.finan_Sale_Price() * my.vm.finan_Tech() / 100);

                    comms = my.vm.managerCommV() +
                        my.vm.sellerCommV() +
                        my.vm.repCommV() +
                        my.vm.teleCommV() +
                        my.vm.techCommV();

                    profit = my.vm.finan_Sale_Price() - totalCost;
                    //my.vm.finan_MarkUp(profit - comms);

                } else if ($('#markupCheckbox').is(':checked')) {

                    var costPlusMarkUpPerc = totalCost * my.vm.finan_MarkUp() / 100;
                    var salePrice = totalCost + costPlusMarkUpPerc;
                    my.vm.finan_Sale_Price(salePrice);

                    my.vm.managerCommV(salePrice * my.vm.finan_Manager() / 100);
                    my.vm.sellerCommV(salePrice * my.vm.finan_SalesPerson() / 100);
                    my.vm.repCommV(salePrice * my.vm.finan_Rep() / 100);
                    my.vm.teleCommV(salePrice * my.vm.finan_Telemarketing() / 100);
                    my.vm.techCommV(salePrice * my.vm.finan_Tech() / 100);

                    comms = my.vm.managerCommV() +
                        my.vm.sellerCommV() +
                        my.vm.repCommV() +
                        my.vm.teleCommV() +
                        my.vm.techCommV();

                    profit = my.vm.finan_Sale_Price() - totalCost;
                    //my.vm.finan_MarkUp(profit - comms);

                } else {

                    if (totalCost > 0) {
                        my.vm.finan_MarkUp((my.vm.finan_Special_Price() / totalCost - 1) * 100);
                    }

                    my.vm.managerCommV(my.vm.finan_Special_Price() * my.vm.finan_Manager() / 100);
                    my.vm.sellerCommV(my.vm.finan_Special_Price() * my.vm.finan_SalesPerson() / 100);
                    my.vm.repCommV(my.vm.finan_Special_Price() * my.vm.finan_Rep() / 100);
                    my.vm.teleCommV(my.vm.finan_Special_Price() * my.vm.finan_Telemarketing() / 100);
                    my.vm.techCommV(my.vm.finan_Special_Price() * my.vm.finan_Tech() / 100);

                    comms = my.vm.managerCommV() +
                        my.vm.sellerCommV() +
                        my.vm.repCommV() +
                        my.vm.teleCommV() +
                        my.vm.techCommV();

                    profit = my.vm.finan_Special_Price() - totalCost;
                    //my.vm.finan_MarkUp(profit - comms);
                }

                my.vm.profitV(kendo.toString((profit - comms), 'c'));
                var profitPerc = (profit - comms);
                my.vm.profit(' (' + (profitPerc ? kendo.toString(((profit - comms) / my.vm.finan_Sale_Price() * 100), "0.00 '%'") : "0.00 '%'") + ')');

                my.vm.netCost(my.vm.netCost() + my.vm.finan_FreightV() + my.vm.finan_ICMSFreightV() + my.vm.finan_OtherExpensesV());

                $('#costDescLabel').text(kendo.toString(my.vm.netCost(), 'n'));
                $('#netCostLabel').text(kendo.toString(totalCost, 'n'));
                $('#taxesLabel').text(kendo.toString(taxes, 'n'));
                $('#commLabel').text(kendo.toString(comms, 'n'));

            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                //$().toastmessage('showErrorToast', data.Result);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
    };

    my.loadProductFinance();

    $('#markupCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#salePriceCheckbox").attr({ 'checked': false });
            $("#specialPriceCheckbox").attr({ 'checked': false });
            $("#markupTextBox").attr({ 'disabled': false });
            $("#salePriceTextBox").attr({ 'disabled': true });
            $("#specialPriceTextBox").attr({ 'disabled': true });

            if (my.onSale) {
                var params = {};
                params.ProductId = my.productId;
                params.Lang = 'pt-BR';
                params.Finan_Select = '0';
                params.Finan_Special_Price = my.vm.finan_Special_Price();
                params.SaleStartDate = moment(new Date('1900-01-01 00:00:00')).format();
                params.SaleEndDate = moment(new Date('1900-01-01 00:00:00')).format();
                params.ModifiedByUser = userID;
                params.ModifiedOnDate = moment().format();

                $.ajax({
                    type: 'PUT',
                    url: '/desktopmodules/riw/api/products/updateProductSpecialOffer',
                    data: params
                }).done(function (data) {
                    if (data.Result.indexOf("success") !== -1) {
                        //$().toastmessage('showSuccessToast', 'Dados da oferta anulados<br />com sucesso!');
                        $.pnotify({
                            title: 'Sucesso!',
                            text: 'Dados de oferta anulados.',
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
                        //$().toastmessage('showErrorToast', data.Result);
                    }
                    onSale = false;
                }).fail(function (jqXHR, textStatus) {
                    console.log(jqXHR.responseText);
                });
            }
        } else {
            $(this).attr({ 'checked': true });
        }
    });

    $('#salePriceCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#markupCheckbox").attr({ 'checked': false });
            $("#specialPriceCheckbox").attr({ 'checked': false });
            $("#markupTextBox").attr({ 'disabled': true });
            $("#salePriceTextBox").attr({ 'disabled': false });
            $("#specialPriceTextBox").attr({ 'disabled': true });

            if (my.onSale) {
                var params = {};

                params.ProductId = my.productId;
                params.Lang = 'pt-BR';
                params.Finan_Select = '1';
                params.Finan_Special_Price = my.vm.finan_Special_Price();
                params.SaleStartDate = moment(new Date('1900-01-01 00:00:00')).format();
                params.SaleEndDate = moment(new Date('1900-01-01 00:00:00')).format();
                params.ModifiedByUser = userID;
                params.ModifiedOnDate = moment().format();

                $.ajax({
                    type: 'PUT',
                    url: '/desktopmodules/riw/api/products/updateProductSpecialOffer',
                    data: params
                }).done(function (data) {
                    if (data.Result.indexOf("success") !== -1) {
                        //$().toastmessage('showSuccessToast', 'Dados da oferta anulados<br />com sucesso!');
                        $.pnotify({
                            title: 'Sucesso!',
                            text: 'Dados de oferta anulados.',
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
                        //$().toastmessage('showErrorToast', data.Result);
                    }
                    onSale = false;
                }).fail(function (jqXHR, textStatus) {
                    console.log(jqXHR.responseText);
                });
            }
        } else {
            $(this).attr({ 'checked': true });
        }
    });

    $('#divSpecialPriceDates').dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        dialogClass: 'dnnFormPopup',
        title: 'Dados da Oferta',
        width: 360,
        buttons: {
            'ok': {
                text: 'Salvar',
                //priority: 'primary',
                "class": 'dnnPrimaryAction',
                click: function () {
                    $.getJSON('/desktopmodules/riw/api/products/GetProduct?productId=' + my.productId + '&lang=pt-BR', function (data) {
                        if (data.SaleStartDate > new Date('1900-01-01 00:00:00')) {
                            $('#saleStartDate').data('kendoDatePicker').value(new Date(data.SaleStartDate));
                            $('#saleEndDate').data('kendoDatePicker').value(new Date(data.SaleEndDate));
                        }
                    });
                    $("#markupCheckbox").attr({ 'checked': false });
                    $("#salePriceCheckbox").attr({ 'checked': false });
                    $("#markupTextBox").attr({ 'disabled': true });
                    $("#salePriceTextBox").attr({ 'disabled': true });
                    $("#specialPriceTextBox").attr({ 'disabled': false });

                    var params = {};
                    params.ProductId = my.productId;
                    params.Lang = 'pt-BR';
                    params.Finan_Select = '2';
                    params.Finan_Special_Price = my.vm.finan_Special_Price();
                    params.SaleStartDate = moment(($('#saleStartDate').data('kendoDatePicker').value() ? $('#saleStartDate').data('kendoDatePicker').value() : new Date('1900-01-01 00:00:00'))).format();
                    params.SaleEndDate = moment(($('#saleEndDate').data('kendoDatePicker').value() ? $('#saleEndDate').data('kendoDatePicker').value() : new Date('1900-01-01 00:00:00'))).format();
                    params.ModifiedByUser = userID;
                    params.ModifiedOnDate = moment().format();

                    $.ajax({
                        type: 'PUT',
                        url: '/desktopmodules/riw/api/products/updateProductSpecialOffer',
                        data: params
                    }).done(function (data) {
                        if (data.Result.indexOf("success") !== -1) {
                            //$().toastmessage('showSuccessToast', 'Dados da oferta atualizados<br />com sucesso!');
                            $.pnotify({
                                title: 'Sucesso!',
                                text: 'Dados de oferta atualizados.',
                                type: 'success',
                                icon: 'fa fa-check fa-lg',
                                addclass: "stack-bottomright",
                                stack: my.stack_bottomright
                            });
                            $('#divSpecialPriceDates').dialog('close');
                        } else {
                            $.pnotify({
                                title: 'Erro!',
                                text: data.Result,
                                type: 'error',
                                icon: 'fa fa-times-circle fa-lg',
                                addclass: "stack-bottomright",
                                stack: my.stack_bottomright
                            });
                            //$().toastmessage('showErrorToast', data.Result);
                        }
                    }).fail(function (jqXHR, textStatus) {
                        console.log(jqXHR.responseText);
                    });
                }
            },
            'cancel': {
                html: 'Cancelar',
                //priority: 'secondary',
                "class": 'dnnSecondaryAction',
                click: function () {
                    $("#markupCheckbox").attr({ 'checked': false });
                    $("#specialPriceCheckbox").attr({ 'checked': false });
                    $("#markupTextBox").attr({ 'disabled': true });
                    $("#salePriceTextBox").attr({ 'disabled': false });
                    $("#specialPriceTextBox").attr({ 'disabled': true });
                    $('#salePriceCheckbox').attr({ 'checked': true });
                    $('#divSpecialPriceDates').dialog('close');
                }
            }
        }
    });

    $('#specialPriceCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $('#divSpecialPriceDates').dialog('open');
            $("#preSpecialPriceTextBox").focus();
        } else {
            $(this).attr({ 'checked': false });
        }
    });

    $('#btnCalc').click(function (e) {
        e.preventDefault();

        var netCostResult = 0;
        if ($("#discountCheckbox").is(':checked')) {
            netCostResult = my.vm.finan_Paid() * my.vm.finan_Paid_Discount() / 100;
            my.vm.finan_PaidDiscountV(netCostResult);
            my.vm.netCost(my.vm.finan_Paid() - netCostResult);
        }
        else {
            netCostResult = my.vm.finan_PaidDiscountV() / my.vm.finan_Paid() * 100;
            my.vm.finan_Paid_Discount(netCostResult);
            my.vm.netCost(my.vm.finan_Paid() - my.vm.finan_PaidDiscountV());
        }

        var freightResult = null;
        if ($("#freightCheckbox").is(':checked')) {
            freightResult = my.vm.netCost() * my.vm.finan_Freight() / 100;
            my.vm.finan_FreightV(freightResult);
        }
        else {
            freightResult = my.vm.finan_FreightV() / my.vm.netCost() * 100;
            my.vm.finan_Freight(freightResult);
        }

        var taxResultExpenses = null;
        if ($("#otherExpensesCheckbox").is(':checked')) {
            taxResultExpenses = my.vm.netCost() * my.vm.finan_OtherExpenses() / 100;
            my.vm.finan_OtherExpensesV(taxResultExpenses);
        }
        else {
            taxResultExpenses = my.vm.finan_OtherExpensesV() / my.vm.netCost() * 100;
            my.vm.finan_OtherExpenses(taxResultExpenses);
        }

        var taxFreightResult = null;
        if ($("#icmsFreightCheckbox").is(':checked')) {
            taxFreightResult = my.vm.finan_ICMSFreight() * my.vm.finan_FreightV() / 100;
            my.vm.finan_ICMSFreightV(taxFreightResult);
        }
        else {
            taxFreightResult = my.vm.finan_Freight() / my.vm.finan_ICMSFreightV() * 100;
            my.vm.finan_ICMSFreightV(taxFreightResult);
        }

        var taxIpiResult = null;
        taxIpiResult = my.vm.netCost() * my.vm.finan_IPI() / 100;
        my.vm.finan_IPIV(taxIpiResult);

        var taxResultDiffICMS = null;
        taxResultDiffICMS = my.vm.netCost() * my.vm.finan_DiffICMS() / 100;
        my.vm.finan_DiffICMSV(taxResultDiffICMS);

        var taxResultTribSubICMS = null;
        taxResultTribSubICMS = my.vm.netCost() * my.vm.finan_TribSubICMS() / 100;
        my.vm.finan_TribSubICMSV(taxResultTribSubICMS);

        var taxResultISS = null;
        taxResultISS = my.vm.netCost() * my.vm.finan_ISS() / 100;
        my.vm.finan_ISSV(taxResultISS);

        var otherTaxesResult = null;
        otherTaxesResult = my.vm.netCost() * my.vm.finan_OtherTaxes() / 100;
        my.vm.finan_OtherTaxesV(otherTaxesResult);

        var totalCost = my.vm.netCost() +
                    my.vm.finan_IPIV() +
                    my.vm.finan_FreightV() +
                    my.vm.finan_OtherExpensesV() +
                    my.vm.finan_IPIV() +
                    my.vm.finan_DiffICMSV() +
                    my.vm.finan_TribSubICMSV() +
                    my.vm.finan_ISSV() +
                    my.vm.finan_OtherTaxesV();

        var profit = 0;
        var comms = 0;
        var taxes = my.vm.finan_IPIV() +
            my.vm.finan_OtherTaxesV() +
            my.vm.finan_DiffICMSV() +
            my.vm.finan_TribSubICMSV() +
            my.vm.finan_ISSV();

        if ($('#salePriceCheckbox').is(':checked')) {

            if (totalCost > 0) {
                my.vm.finan_MarkUp((my.vm.finan_Sale_Price() / totalCost - 1) * 100);
            }

            my.vm.managerCommV(my.vm.finan_Sale_Price() * my.vm.finan_Manager() / 100);
            my.vm.sellerCommV(my.vm.finan_Sale_Price() * my.vm.finan_SalesPerson() / 100);
            my.vm.repCommV(my.vm.finan_Sale_Price() * my.vm.finan_Rep() / 100);
            my.vm.teleCommV(my.vm.finan_Sale_Price() * my.vm.finan_Telemarketing() / 100);
            my.vm.techCommV(my.vm.finan_Sale_Price() * my.vm.finan_Tech() / 100);

            comms = my.vm.managerCommV() +
                my.vm.sellerCommV() +
                my.vm.repCommV() +
                my.vm.teleCommV() +
                my.vm.techCommV();

            profit = my.vm.finan_Sale_Price() - totalCost;
            //my.vm.finan_MarkUp(profit - comms);

        } else if ($('#markupCheckbox').is(':checked')) {

            var costPlusMarkUpPerc = totalCost * my.vm.finan_MarkUp() / 100;
            var salePrice = totalCost + costPlusMarkUpPerc;
            my.vm.finan_Sale_Price(salePrice);

            my.vm.managerCommV(salePrice * my.vm.finan_Manager() / 100);
            my.vm.sellerCommV(salePrice * my.vm.finan_SalesPerson() / 100);
            my.vm.repCommV(salePrice * my.vm.finan_Rep() / 100);
            my.vm.teleCommV(salePrice * my.vm.finan_Telemarketing() / 100);
            my.vm.techCommV(salePrice * my.vm.finan_Tech() / 100);

            comms = my.vm.managerCommV() +
                my.vm.sellerCommV() +
                my.vm.repCommV() +
                my.vm.teleCommV() +
                my.vm.techCommV();

            profit = my.vm.finan_Sale_Price() - totalCost;
            // my.vm.finan_MarkUp(profit - comms);

        } else {

            if (totalCost > 0) {
                my.vm.finan_MarkUp((my.vm.finan_Special_Price() / totalCost - 1) * 100);
            }

            my.vm.managerCommV(my.vm.finan_Special_Price() * my.vm.finan_Manager() / 100);
            my.vm.sellerCommV(my.vm.finan_Special_Price() * my.vm.finan_SalesPerson() / 100);
            my.vm.repCommV(my.vm.finan_Special_Price() * my.vm.finan_Rep() / 100);
            my.vm.teleCommV(my.vm.finan_Special_Price() * my.vm.finan_Telemarketing() / 100);
            my.vm.techCommV(my.vm.finan_Special_Price() * my.vm.finan_Tech() / 100);

            comms = my.vm.managerCommV() +
                my.vm.sellerCommV() +
                my.vm.repCommV() +
                my.vm.teleCommV() +
                my.vm.techCommV();

            profit = my.vm.finan_Special_Price() - totalCost;
            //my.vm.finan_MarkUp(profit - comms);
        }

        my.vm.profitV(kendo.toString((profit - comms), 'c'));
        //var profitPerc = mkUp / comms;
        my.vm.profit(' (' + kendo.toString(((profit - comms) / my.vm.finan_Sale_Price() * 100), "0.00 '%'") + ')');

        my.vm.netCost(my.vm.netCost() + my.vm.finan_FreightV() + my.vm.finan_ICMSFreightV() + my.vm.finan_OtherExpensesV());

        $('#costDescLabel').text(kendo.toString(my.vm.netCost(), 'n'));
        $('#netCostLabel').text(kendo.toString(totalCost, 'n'));
        $('#taxesLabel').text(kendo.toString(taxes, 'n'));
        $('#commLabel').text(kendo.toString(comms, 'n'));

        $('#btnSaveFinan').attr({ 'disabled': false, 'title': 'Clique aqui para salvar as altera&#231;&#245;es' });

        //alert("Lembre-se de salvar as alterações");
        $.pnotify({
            title: 'Aten&#231;&#227;o!',
            text: 'Lembre-se de salvar as altera&#231;&#245;es.',
            type: 'warning',
            icon: 'fa fa-warning fa-lg',
            addclass: "stack-bottomright",
            stack: my.stack_bottomright
        });

    });

    $('#vDiscountCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#vDiscountTextBox").attr({ 'disabled': false });
            $("#discountCheckbox").attr({ 'checked': false });
            $("#discountTextBox").attr({ 'disabled': true });
        } else {
            $("#vDiscountTextBox").attr({ 'disabled': true });
            $("#discountCheckbox").attr({ 'checked': true });
            $("#discountTextBox").attr({ 'disabled': false });
        }
    });

    $('#discountCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#discountCheckbox").attr({ 'disabled': false });
            $("#vDiscountCheckbox").attr({ 'checked': false });
            $("#vDiscountTextBox").attr({ 'disabled': true });
        } else {
            $("#discountCheckbox").attr({ 'disabled': true });
            $("#vDiscountCheckbox").attr({ 'checked': true });
            $("#vDiscountTextBox").attr({ 'disabled': false });
        }
    });

    $('#vFreightCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#vFreightCheckbox").attr({ 'disabled': false });
            $("#freightCheckbox").attr({ 'checked': false });
            $("#freightTextBox").attr({ 'disabled': true });
        } else {
            $("#vFreightCheckbox").attr({ 'disabled': true });
            $("#freightCheckbox").attr({ 'checked': true });
            $("#freightTextBox").attr({ 'disabled': false });
        }
    });

    $('#freightCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#freightTextBox").attr({ 'disabled': false });
            $("#vFreightCheckbox").attr({ 'checked': false });
            $("#vFreightTextBox").attr({ 'disabled': true });
        } else {
            $("#freightTextBox").attr({ 'disabled': true });
            $("#vFreightCheckbox").attr({ 'checked': true });
            $("#vFreightTextBox").attr({ 'disabled': false });
        }
    });

    $('#vICMSFreightCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#vICMSFreightTextBox").attr({ 'disabled': false });
            $("#icmsFreightCheckbox").attr({ 'checked': false });
            $("#icmsFreightTextBox").attr({ 'disabled': true });
        } else {
            $("#vICMSFreightTextBox").attr({ 'disabled': true });
            $("#icmsFreightCheckbox").attr({ 'checked': true });
            $("#icmsFreightTextBox").attr({ 'disabled': false });
        }
    });

    $('#icmsFreightCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#icmsFreightTextBox").attr({ 'disabled': false });
            $("#vICMSFreightCheckbox").attr({ 'checked': false });
            $("#vICMSFreightTextBox").attr({ 'disabled': true });
        } else {
            $("#icmsFreightTextBox").attr({ 'disabled': true });
            $("#vICMSFreightCheckbox").attr({ 'checked': true });
            $("#vICMSFreightTextBox").attr({ 'disabled': false });
        }
    });

    $('#vOtherExpensesCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#otherExpensesCheckbox").attr({ 'checked': false });
            $("#vOtherExpensesTextBox").attr({ 'disabled': false });
            $("#otherExpensesTextBox").attr({ 'disabled': true });
        } else {
            $("#otherExpensesCheckbox").attr({ 'checked': true });
            $("#vOtherExpensesTextBox").attr({ 'disabled': true });
            $("#otherExpensesTextBox").attr({ 'disabled': false });
        }
    });

    $('#otherExpensesCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#vOtherExpensesCheckbox").attr({ 'checked': false });
            $("#otherExpensesTextBox").attr({ 'disabled': false });
            $("#vOtherExpensesTextBox").attr({ 'disabled': true });
        } else {
            $("#vOtherExpensesCheckbox").attr({ 'checked': true });
            $("#otherExpensesTextBox").attr({ 'disabled': true });
            $("#vOtherExpensesTextBox").attr({ 'disabled': false });
        }
    });

    $('#managerCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#vManagerCheckbox").attr({ 'checked': false });
            $("#managerTextBox").attr({ 'disabled': false });
            $("#vManagerTextBox").attr({ 'disabled': true });
        } else {
            $("#vManagerCheckbox").attr({ 'checked': true });
            $("#managerTextBox").attr({ 'disabled': true });
            $("#vManagerTextBox").attr({ 'disabled': false });
        }
    });

    $('#vManagerCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#managerCheckbox").attr({ 'checked': false });
            $("#vManagerTextBox").attr({ 'disabled': false });
            $("#managerTextBox").attr({ 'disabled': true });
        } else {
            $("#managerCheckbox").attr({ 'checked': true });
            $("#vManagerTextBox").attr({ 'disabled': true });
            $("#managerTextBox").attr({ 'disabled': false });
        }
    });

    $('#sellerCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#vSellerCheckbox").attr({ 'checked': false });
            $("#sellerTextBox").attr({ 'disabled': false });
            $("#vSellerTextBox").attr({ 'disabled': true });
        } else {
            $("#vSellerCheckbox").attr({ 'checked': true });
            $("#sellerTextBox").attr({ 'disabled': true });
            $("#vSellerTextBox").attr({ 'disabled': false });
        }
    });

    $('#vSellerCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#sellerCheckbox").attr({ 'checked': false });
            $("#vSellerTextBox").attr({ 'disabled': false });
            $("#sellerTextBox").attr({ 'disabled': true });
        } else {
            $("#sellerCheckbox").attr({ 'checked': true });
            $("#vSellerTextBox").attr({ 'disabled': true });
            $("#sellerTextBox").attr({ 'disabled': false });
        }
    });

    $('#repCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#vRepCheckbox").attr({ 'checked': false });
            $("#repTextBox").attr({ 'disabled': false });
            $("#vRepTextBox").attr({ 'disabled': true });
        } else {
            $("#vRepCheckbox").attr({ 'checked': true });
            $("#repTextBox").attr({ 'disabled': true });
            $("#vRepTextBox").attr({ 'disabled': false });
        }
    });

    $('#vRepCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#repCheckbox").attr({ 'checked': false });
            $("#vRepTextBox").attr({ 'disabled': false });
            $("#repTextBox").attr({ 'disabled': true });
        } else {
            $("#repCheckbox").attr({ 'checked': true });
            $("#vRepTextBox").attr({ 'disabled': true });
            $("#repTextBox").attr({ 'disabled': false });
        }
    });

    $('#telemarketCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#vTelemarketCheckbox").attr({ 'checked': false });
            $("#telemarketTextBox").attr({ 'disabled': false });
            $("#vTelemarketTextBox").attr({ 'disabled': true });
        } else {
            $("#vTelemarketCheckbox").attr({ 'checked': true });
            $("#telemarketTextBox").attr({ 'disabled': true });
            $("#vTelemarketTextBox").attr({ 'disabled': false });
        }
    });

    $('#vTelemarketCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#telemarketCheckbox").attr({ 'checked': false });
            $("#vTelemarketTextBox").attr({ 'disabled': false });
            $("#telemarketTextBox").attr({ 'disabled': true });
        } else {
            $("#telemarketCheckbox").attr({ 'checked': true });
            $("#vTelemarketTextBox").attr({ 'disabled': true });
            $("#telemarketTextBox").attr({ 'disabled': false });
        }
    });

    $('#techCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#vTechCheckbox").attr({ 'checked': false });
            $("#techTextBox").attr({ 'disabled': false });
            $("#vTechTextBox").attr({ 'disabled': true });
        } else {
            $("#vTechCheckbox").attr({ 'checked': true });
            $("#techTextBox").attr({ 'disabled': true });
            $("#vTechTextBox").attr({ 'disabled': false });
        }
    });

    $('#vTechCheckbox').on('change', function () {
        if ($(this).is(':checked')) {
            $("#techCheckbox").attr({ 'checked': false });
            $("#vTechTextBox").attr({ 'disabled': false });
            $("#techTextBox").attr({ 'disabled': true });
        } else {
            $("#techCheckbox").attr({ 'checked': true });
            $("#vTechTextBox").attr({ 'disabled': true });
            $("#techTextBox").attr({ 'disabled': false });
        }
    });

    $('#btnSaveFinan').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $(this).html('<i class="fa fa-spinner fa-spin"></i> Um momento...').attr({ 'disabled': true });

        var params = {
            PortalId: portalID,
            ProductId: my.productId,
            Finan_Paid: my.vm.finan_Paid(),
            Finan_Paid_Discount: my.vm.finan_Paid_Discount(),
            Finan_IPI: my.vm.finan_IPI(),
            Finan_Freight: my.vm.finan_Freight(),
            Finan_ICMSFreight: my.vm.finan_ICMSFreight(),
            Finan_OtherExpenses: my.vm.finan_OtherExpenses(),
            Finan_DiffICMS: my.vm.finan_DiffICMS(),
            Finan_TributeSubICMS: my.vm.finan_TribSubICMS(),
            Finan_ISS: my.vm.finan_ISS(),
            Finan_OtherTaxes: my.vm.finan_OtherTaxes(),
            Finan_ICMS: $('#icmsTextBox').val(),
            Finan_CFOP: $('#cfopTextBox').val(),
            Finan_CST: $('#cstTextBox').val(),
            Finan_MarkUp: my.vm.finan_MarkUp(),
            Finan_Sale_Price: my.vm.finan_Sale_Price(),
            Finan_Special_Price: my.vm.finan_Special_Price(),
            Finan_Dealer_Price: my.vm.finan_Dealer_Price(),
            Finan_Rep: my.vm.finan_Rep(),
            Finan_Manager: my.vm.finan_Manager(),
            Finan_SalesPerson: my.vm.finan_SalesPerson(),
            Finan_Telemarketing: my.vm.finan_Telemarketing(),
            Finan_Tech: my.vm.finan_Tech(),
            Finan_Cost: kendo.toString(Number($('#netCostLabel').text().replace('.', '').replace(',', '.').replace(/[^0-9\.-]+/g, ""))),
            Finan_COFINS: $('#cofinsTextBox').val(),
            Finan_COFINSBase: $('#cofinsBaseTextBox').val(),
            Finan_COFINSTributeSituation: $('#cofinsTribSit').data('kendoDropDownList').value(),
            Finan_COFINSTributeSub: $('#cofinsTribSubTextBox').val(),
            Finan_COFINSTributeSubBase: $('#cofinsTribSubBaseTextBox').val(),
            Finan_DefaultBarCode: $('#defaultBarCode').data('kendoDropDownList').value(),
            Finan_IPITributeSituation: $('#ipiTribSit').data('kendoDropDownList').value(),
            Finan_NCM: $('#ncmTextBox').val(),
            Finan_PIS: $('#pisTextBox').val(),
            Finan_PISBase: $('#pisBaseTextBox').val(),
            Finan_PISTributeSituation: $('#pisTribSit').data('kendoDropDownList').value(),
            Finan_PISTributeSub: $('#pisTribSubTextBox').val(),
            Finan_PISTributeSubBase: $('#pisTribSubBaseTextBox').val(),
            Finan_TributeSituationType: $('#tribSitType').data('kendoDropDownList').value(),
            SyncEnabled: amplify.store.sessionStorage('syncEnabled').toLowerCase()
        };

        switch (true) {
            case $('#markupCheckbox').is(':checked'):
                params.Finan_Select = '0';
                break;
            case $('#specialPriceCheckbox').is(':checked'):
                params.Finan_Select = '2';
                break;
            default:
                params.Finan_Select = '1';
        }

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/products/UpdateProductFinance',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                //parent.$("#productWindow").append("<div id='window'></div>");
                //var kendoWindow = parent.$('#window').kendoWindow({
                //    title: "Baixar Arquivo",
                //    resizable: false,
                //    modal: true,
                //    width: 550,
                //    height: 300,
                //    deactivate: function () {
                //        this.destroy();
                //    }
                //}).data("kendoWindow")
                //    .content('<h2>Arquivo disponível para exportação.</h2><h5><a href="/portals/0/products/E' + my.padLeft(my.pId, 8) + '.txt">E' + my.padLeft(my.pId, 8) + '.txt</a></h5><h6>Clique com o botão da direita do mouse e escolha a opção salvar link.</h6>')
                //    .center().open();

                //$('#btnSaveFinan').fadeOut();
                //$().toastmessage('showSuccessToast', 'Financeiro autalizado com<br />sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Financeiro autalizado.',
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
                //$().toastmessage('showErrorToast', data.Result);
            }

            $('#btnSaveFinan').html('<i class="icon-ok icon-white"></i> Atualizar').attr({ 'disabled': true });
        }).fail(function (jqXHR, textStatus) {
            $('#btnSaveFinan').html('<i class="icon-ok icon-white"></i> Atualizar').attr({ 'disabled': true });
            console.log(jqXHR.responseText);
        });
    });

    $('#productMenu').kendoMenu({
        select: function (e) {
            switch ($(e.item).attr('id')) {
                case 'menu_2':
                    e.preventDefault();
                    document.location.href = editItemURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_3':
                    e.preventDefault();
                    document.location.href = productImagesURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_4':
                    e.preventDefault();
                    document.location.href = productDescURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_5':
                    e.preventDefault();
                    document.location.href = productVideosURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_6':
                    e.preventDefault();
                    document.location.href = relatedProductsURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_7':
                    e.preventDefault();
                    document.location.href = productAttributesURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_8':
                    e.preventDefault();
                    document.location.href = productShippingURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
                case 'menu_9':
                    e.preventDefault();
                    document.location.href = productSEOURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemId/' + my.productId;
                    break;
            }
        }
    });

    $('input.enterastab, select.enterastab, textarea.enterastab').live('keydown', function (e) {
        if (e.keyCode === 13) {
            var focusable = $('input,a,select,button,textarea').filter(':visible');
            focusable.eq(focusable.index(this) + 1).focus();
            return false;
        }
    });

    $('.btnReturn').click(function (e) {
        e.preventDefault();

        $(this).html('<i class="fa fa-spinner fa-spin"></i>Um momento...').attr({ 'disabled': true });
        var urlAddress = '';
        if (my.retSel > 0) {
            switch (my.retSel) {
                case 7:
                    urlAddress = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.productId + '/sel/7';
                    break;
                case 8:
                    urlAddress = historyURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.productId + '/sel/8';
                    break;
                case 9:
                    urlAddress = finanURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.productId + '/sel/9';
                    break;
                case 10:
                    urlAddress = assistURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.productId + '/sel/10';
                    break;
                case 11:
                    urlAddress = commURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.productId + '/sel/11';
                    break;
            }
            document.location.href = urlAddress;
        } else {
            if (parent.$('#window')) {
                parent.$('#window').data("kendoWindow").close();
            }
        }
    });

});
