
my.viewModel = function () {

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

    ko.bindingHandlers.commaDecimalFormatterBank = {
        init: function (element, valueAccessor) {

            var observable = valueAccessor();

            var interceptor = ko.computed({
                read: function () {
                    return formatWithComma(observable());
                },
                write: function (newValue) {
                    observable(reverseFormat(newValue));
                }
            });

            if (element.id === 'bankInTextBox')
                ko.applyBindingsToNode(element, {
                    value: interceptor
                });
            else
                ko.applyBindingsToNode(element, {
                    text: interceptor
                });
        }
    };

    ko.bindingHandlers.commaDecimalFormatterCard = {
        init: function (element, valueAccessor) {

            var observable = valueAccessor();

            var interceptor = ko.computed({
                read: function () {
                    return formatWithComma(observable());
                },
                write: function (newValue) {
                    observable(reverseFormat(newValue));
                }
            });

            if (element.id === 'cardInTextBox')
                ko.applyBindingsToNode(element, {
                    value: interceptor
                });
            else
                ko.applyBindingsToNode(element, {
                    text: interceptor
                });

            if (element.id === 'debitInTextBox')
                ko.applyBindingsToNode(element, {
                    value: interceptor
                });
            else
                ko.applyBindingsToNode(element, {
                    text: interceptor
                });
        }
    };

    ko.bindingHandlers.commaDecimalFormatterCheck = {
        init: function (element, valueAccessor) {

            var observable = valueAccessor();

            var interceptor = ko.computed({
                read: function () {
                    return formatWithComma(observable());
                },
                write: function (newValue) {
                    observable(reverseFormat(newValue));
                }
            });

            if (element.id === 'checkInTextBox')
                ko.applyBindingsToNode(element, {
                    value: interceptor
                });
            else
                ko.applyBindingsToNode(element, {
                    text: interceptor
                });
        }
    };

    ko.bindingHandlers.commaDecimalFormatterCash = {
        init: function (element, valueAccessor) {

            var observable = valueAccessor();

            var interceptor = ko.computed({
                read: function () {
                    return formatWithComma(observable());
                },
                write: function (newValue) {
                    observable(reverseFormat(newValue));
                }
            });

            if (element.id === 'cashInTextBox')
                ko.applyBindingsToNode(element, {
                    value: interceptor
                });
            else
                ko.applyBindingsToNode(element, {
                    text: interceptor
                });
        }
    };

    // Formatting Functions
    function formatWithComma(x, precision, seperator) {
        var options = {
            precision: precision || 2,
            seperator: seperator || ','
        };
        var formatted = parseFloat(x, 10).toFixed(options.precision);
        var regex = new RegExp(
                '^(\\d+)[^\\d](\\d{' + options.precision + '})$');
        formatted = formatted.replace(
            regex, '$1' + options.seperator + '$2');
        return formatted;
    }

    function reverseFormat(x, precision, seperator) {
        var options = {
            precision: precision || 2,
            seperator: seperator || ','
        };
        var regex = new RegExp(
            '^(\\d+)[^\\d](\\d+)$');
        var formatted = x.replace(regex, '$1.$2');
        return parseFloat(formatted);
    }

    // product view model
    my.Product = function () {
        var self = this;

        self.productId = ko.observable();
        //self.productRef = ko.observable();
        //self.barcode = ko.observable();
        self.productCode = ko.observable();
        self.productName = ko.observable();
        self.summary = ko.observable();
        self.productUnit = ko.observable();
        self.prodImageId = ko.observable();
        self.unitValue = ko.observable();
        self.finan_Sale_Price = ko.observable();
        self.finan_Special_Price = ko.observable();
        self.itemQty = ko.observable('');
        self.qTyStockSet = ko.observable();
        self.totalValue = ko.observable();
        self.visible = ko.observable(true);
        self.increaseQuantity = function (quantity) {
            self.qTy(self.qTy() + quantity);
        };
    };

    my.PayForm = function (data) {
        this.PayFormId = ko.observable(data.PayFormId);
        this.PayFormTitle = ko.observable(data.PayFormTitle);
    };

    my.PayCond = function () {
        this.payCondTitle = ko.observable();
        this.totalParcelado = ko.observable();
        this.totalPayCond = ko.observable();
        this.parcela = ko.observable();
        this.payCondN = ko.observable();
        this.payCondPerc = ko.observable();
        this.payIn = ko.observable();
        this.payInDays = ko.observable();
        this.payCondDisc = ko.observable();
        this.interval = ko.observable();
    };

    // VIEW MODEL
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

        self.siteName = ko.observable(siteName),
        self.logoUrl = ko.observable(customerLogo),
        self.estimateId = ko.observable(my.estimateId),
        self.estimateItemId = ko.observable(0),
        self.estimatedByUser = ko.observable(0),
        self.estimateStatus = ko.observable(''),
        self.personId = ko.observable(0),
        self.clientUserId = ko.observable(0),
        self.displayName = ko.observable(''),
        self.firstName = ko.observable(''),
        self.lastName = ko.observable(''),
        self.telephone = ko.observable(''),
        self.email = ko.observable(''),
        self.street = ko.observable(''),
        self.unit = ko.observable(''),
        self.complement = ko.observable(''),
        self.district = ko.observable(''),
        self.city = ko.observable(''),
        self.region = ko.observable(''),
        self.myUid = ko.observable(),
        self.productId = ko.observable(),
        self.productName = ko.observable('Rildo Informática Ltda (31) 3037-0551'),
        self.summary = ko.observable(),
        self.productCode = ko.observable('0000000000000'),
        self.unitTitle = ko.observable(),
        self.prodImageId = ko.observable(),
        self.itemQty = ko.observable(''),
        self.stock = ko.observable(0),
        self.unitValue = ko.observable(0),
        self.itemTotal = ko.observable(),
        self.subTotal = ko.observable(),
        self.grandTotal = ko.observable(),
        self.displayTotal = ko.observable('TOTAL = 0,00'),
        self.prodTotal = ko.observable('0 x 0,00 = 0,00'),
        self.createdByUser = ko.observable(),
        self.createdOnDate = ko.observable(new Date()),
        self.hasLogin = ko.observable(false),
        self.items = ko.observableArray([]),
        self.selectedItemDiscount = ko.observable(),
        self.payFormTitle = ko.observable(),
        self.selectedPayFormId = ko.observable(0),
        self.selectedPayForm = ko.observable(''),
        self.payInMin = ko.observable(),
        self.conditionPayIn = ko.observable(0),
        self.conditionPayInDays = ko.observable(0),
        self.conditionNumberPayments = ko.observable(0),
        self.conditionInterest = ko.observable(0),
        self.conditionInterval = ko.observable(0),
        self.payCondDisc = ko.observable(0),
        self.blocked = ko.observable(false),
        self.reasonBlocked = ko.observable(''),
        self.conditionPaymentResult = function () {
            $('#payment').text(conditionPayment()).formatCurrency({ region: 'pt-BR' });
        },

        self.conditionPayment = ko.computed({
            read: function () {
                var result;
                if (conditionNumberPayments() !== 0) {
                    var nPayments = (conditionPayIn() > 0 ? (conditionNumberPayments() - 1) : conditionNumberPayments());
                    var totalMinusPayIn = (((my.lineTotal) - (my.lineTotal * couponDiscount() / 100)) - conditionPayIn());
                    result = (my.pmt(conditionInterest(), 1, nPayments, (-totalMinusPayIn), 0));
                }
                else {
                    result = 0.00;
                }
                return result;
            },
            write: function () {
                var result;
                if (conditionNumberPayments() !== 0) {
                    var nPayments = (conditionPayIn() > 0 ? (conditionNumberPayments() - 1) : conditionNumberPayments());
                    var totalMinusPayIn = (((my.lineTotal) - (my.lineTotal * couponDiscount() / 100)) - conditionPayIn());
                    result = (my.pmt(conditionInterest(), 1, nPayments, (-totalMinusPayIn), 0));
                }
                else {
                    result = 0.00;
                }
                return result;
            },
            owner: self
        }),

        self.conditionPayments = ko.computed({
            read: function () {
                var result = 0;
                var nPayments = (conditionPayIn() > 0 ? (conditionNumberPayments() - 1) : conditionNumberPayments());
                result = (conditionPayment() * nPayments);
                return result;
            },
            write: function () {
                var result = 0;
                var nPayments = (conditionPayIn() > 0 ? (conditionNumberPayments() - 1) : conditionNumberPayments());
                result = (conditionPayment() * nPayments);
                return result;
            },
            owner: self
        }),

        self.conditionTotalPay = ko.computed({
            read: function () {
                var result = conditionPayments() + conditionPayIn();
                return result;
            },
            write: function () {
                var result = conditionPayments() + conditionPayIn();
                return result;
            },
            owner: self
        }),

        self.conditionNPayments = ko.computed({
            read: function () {
                var paymentsStr = conditionInterest() > 0 ? 'x com juros' : 'x sem juros';
                return conditionNumberPayments() + paymentsStr;
            },
            write: function () {
                var paymentsStr = conditionInterest() > 0 ? 'x com juros' : 'x sem juros';
                return conditionNumberPayments() + paymentsStr;
            },
            owner: self
        }),

        //self.statuses = ko.observableArray([]),
        self.selectedStatusId = ko.observable(10),
        //self.loadStatuses = function () {
        //    $.getJSON('/desktopmodules/riw/api/statuses/GetStatuses?portalId=' + portalID + '&isDeleted=False', function (data) {
        //        self.statuses.removeAll();
        //        $.each(data, function (i, st) {
        //            self.statuses.push(ko.mapping.fromJS(st));
        //        });
        //    });
        //},

        self.currentDate = ko.computed(function () {
            var displayDate = kendo.format('{0:D}', new Date());
            $('[data-bind*="currentDate"]').css({ textTransform: 'capitalize' });
            return displayDate;
        }),

        self.salesRepName = ko.observable(''),
        self.salesRepEmail = ko.observable(''),
        self.salesRepPhone = ko.observable(''),
        self.subject = ko.observable(''),
        //self.subject = ko.computed(function () {
        //    var subject = estimateSubject.replace('[WEBSITE]', siteName);
        //    subject = subject.replace('[ID]', '(' + my.estimateId > 0 ? my.estimateId : self.estimateId());
        //    return subject;
        //}),
        //self.emailTo = ko.observable(''),
        self.message = ko.observable(''),
        //self.estimateLink = ko.computed(function () {
        //    return '<a href="' + estimateURL + '?eid=' + my.estimateId + '">link</a>';
        //}),
        self.siteAddress = ko.computed(function () {
            var address = '<br /><br />' + storeAddress + ' ' + storeUnit + ' ' + storeComplement + '<br /> Bairro: ' + storeDistrict + ' / ' + storeCity + '<br />' + storeRegion + ' ' + storePostalCode + '<br />';
            return address;
        }),

        //ko.bindingHandlers.kendoGrid.options = {
        //    height: 285,
        //    columns: [
        //        {
        //            title: " ",
        //            template: '<span style="width: 100px">#= Barcode #</span><span>#= ProductName #</span><br /><span style="width: 30px">#= ++my.record #</span><span style="width: 49px">#= ProductQty #</span><span style="width: 30px">#= UnitTypeTitle #</span><span style="width: 70px">#= kendo.toString(UnitValue, "n") #</span><span style="width: 75px">#= kendo.toString(ProductDiscount, "n") #</span><span>#= kendo.toString(ExtendedAmount, "n") #</span><a id="' + my.estimateId + '_' + '#= ProductId #" onclick="my.removeItem(); return false;" class="deleteItem"><span class="fa fa-times" title="Excluir Item"></span></a>'
        //        }
        //    ],
        //    selectable: true,
        //    navigatable: true,
        //    change: function () {
        //        var row = this.select();
        //        var id = row.data("uid");
        //        var dataItem = $('#couponGrid').data("kendoGrid").dataSource.getByUid(id);

        //        $('#divDiscount').kendoWindow({
        //            title: 'Descontos',
        //            modal: true,
        //            width: '80%',
        //            height: '70%',
        //            close: function (e) {
        //                self.estimateItemId(0);
        //                self.newItemDiscount(0);
        //                self.itemPrice(0);
        //                self.newItemQty(0);
        //                self.oldItemQty(1);
        //            },
        //            open: function (e) {
        //                self.estimateItemId(0);
        //                self.newItemDiscount(0);
        //                self.itemPrice(0);
        //                self.newItemQty(0);
        //                self.oldItemQty(1);
        //            }
        //        });

        //        $('#divDiscount').data("kendoWindow").center().open();

        //        self.productName(dataItem.ProductName);
        //        self.productCode(dataItem.Barcode.length > 0 ? dataItem.Barcode : dataItem.ProductRef.length > 0 ? dataItem.ProductRef : '0000000000000');
        //        self.productId(dataItem.ProductId);

        //        setTimeout(function () {
        //            self.estimateItemId(dataItem.EstimateItemId);
        //            self.newItemDiscount(dataItem.ProductDiscountPerc);
        //            self.itemPrice(dataItem.UnitValue);
        //            self.newItemQty(dataItem.ProductQty);
        //            self.oldItemQty(dataItem.ProductQty);
        //        }, 1000);

        //        $("input[type=text]").on('focusin', function () {
        //            var saveThis = $(this);
        //            window.setTimeout(function () {
        //                saveThis.select();
        //            }, 100);
        //        });
                
        //        $('#divImage').off('click');
        //        if (dataItem.Summary.length > 0) {
        //            $('#divImage').html(function () {
        //                if (dataItem.ProductImageId > 0) {
        //                    return '<div class="view view-first"><img alt="' + dataItem.ProductName + '" src="/databaseimages/' + dataItem.ProductImageId + '.jpg?maxwidth=' + $('#divImage').width() + '&maxheight=' + $('#divImage').height() + '" /></div>';
        //                } else {
        //                    return '<div class="view view-first fallBack">Imagem N&#227;o Dispon&#237;vel</div>';
        //                }
        //            }).on('click', function (e) {
        //                window.open('/produtos/detalhes?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer&itemid=' + dataItem.ProductId, 'PrintMe', 'toolbar=no,menubar=no,location=no,width=720,height=580');
        //                return false;
        //            });
        //        } else {
        //            $('#divImage').html(function () {
        //                if (dataItem.ProductImageId > 0) {
        //                    return '<div class="view view-first"><img alt="' + dataItem.ProductName + '" src="/databaseimages/' + dataItem.ProductImageId + '.jpg?maxwidth=' + $('#divImage').width() + '&maxheight=' + $('#divImage').height() + '" /></div>';
        //                } else {
        //                    return '<div class="view view-first fallBack">Imagem N&#227;o Dispon&#237;vel</div>';
        //                }
        //            });
        //        }
        //        self.prodTotal(dataItem.ProductQty + ' x ' + dataItem.UnitValue + ' = ' + kendo.toString(Math.floor((kendo.parseFloat(dataItem.ProductQty) * dataItem.UnitValue) * 100) / 100, 'n'));
        //    },
        //    dataBinding: function () {
        //        my.record = 0;
        //    },
        //    dataBound: function () {
        //        for (var i = 0; i < this.dataSource.view().length; i++) {
        //            if (this.dataSource.view()[i].Stock <= 0) {
        //                var uid = this.dataSource.view()[i].uid;
        //                $("#couponGrid tbody").find("tr[data-uid=" + uid + "]").addClass('isDeleted');
        //            }
        //        };
        //        this.content.scrollTop(this.tbody.height());
        //    }
        //},

        self.currentPlace = function (data, elem) {
            my.vm.productName(data.ProductName);
            my.vm.productCode(data.Barcode.length > 0 ? data.Barcode : data.ProductRef.length > 0 ? data.ProductRef : '0000000000000');
            my.vm.productId(data.ProductId);

            setTimeout(function () {
                my.vm.estimateItemId(data.EstimateItemId);
                my.vm.newItemDiscount(data.ProductDiscountPerc);
                my.vm.itemPrice(data.UnitValue);
                my.vm.newItemQty(data.ProductQty);
                my.vm.oldItemQty(data.ProductQty);
            }, 1000);

            $('.selectable').removeClass('highlight');
            $(elem.target.parentElement).addClass('highlight');
        },

        self.payForms = ko.observableArray([]),
        self.loadPayForms = function () {
            $.getJSON('/desktopmodules/riw/api/payforms/getPayForms?portalId=' + portalID + '&isDeleted=false', function (data) {
                self.payForms.removeAll();
                //data.sort(self.sortJsonName('CountryName', false, function (a) { return a.toUpperCase() }));
                var mappedPayForms = $.map(data, function (item) { return new my.PayForm(item); });
                self.payForms(mappedPayForms);
            });
        },

        self.payConds = ko.observableArray([]),
        self.loadPayConds = function () {
            $.getJSON("/desktopmodules/riw/api/payconditions/GetPayConds?portalId=" + portalID + "&pcType=0&isDeleted=" + false + "&pcStart=" + my.lineTotal, function (data) {
                $.each(data, function (i, pc) {
                    self.payConds.push(new my.PayCond()
                        .payCondTitle(pc.PayCondTitle)
                        .totalParcelado(pc.TotalParcelado)
                        .totalPayCond(pc.TotalPayCond)
                        .parcela(pc.Parcela)
                        .payCondN(pc.PayCondN)
                        .payCondPerc(pc.PayCondPerc)
                        .payIn(pc.PayIn)
                        .payCondDisc(pc.PayCondDisc)
                        .interval(pc.Intervalo));
                });
            });
        },

        self.payFormSelected = function (e) {
            if (e.item.index() > -1) {
                var dataItem = this.dataItem(e.item.index());
                //$('#payCondMsg').html('<i class="fa fa-spinner fa-spin fa-lg"></i>');
                my.startPayCondGrid(dataItem.PayFormId);
                $('#payCondGrid').data("kendoGrid").dataSource.read();
                setTimeout(function () {
                    if ($('#payCondGrid').data("kendoGrid").dataSource.data().length === 0) {
                        $('#ddlPayForms').data('kendoDropDownList').value(0);
                        alert('Valor insuficiente para a forma de pagamento escolhida!');
                    } else {
                        var target = $('#payCondGrid');
                        target.data("kendoGrid").table.focus();
                        target.find("tbody tr:first").addClass('k-state-selected');
                    }
                }, 600);
            }
        },

        self.bankIn = ko.observable(0),
        self.checkIn = ko.observable(0),
        self.cardIn = ko.observable(0),
        self.debitIn = ko.observable(0),
        self.cashIn = ko.observable(0),
        self.totalsIn = ko.computed(function () {
            return (self.cashIn() + self.checkIn() + self.cardIn() + self.debitIn() + self.bankIn());
        }),
        self.balance = ko.computed(function () {
            return self.grandTotal() < self.totalsIn() ? 'TROCO:' : 'RESTA:';
        }),
        self.cashBack = ko.computed(function () {
            if (self.totalsIn() > 0) {
                return kendo.format('{0:c}', (self.grandTotal() - (self.totalsIn())));
            }
            return self.grandTotal();
        }),

        self.couponDiscount = ko.observable(0),
        self.originalProdTotal = ko.observable(0),
        //self.estimateProdTotal = ko.observable(0),
        self.oldItemQty = ko.observable('0'),
        self.newItemQty = ko.observable('1'),
        self.newItemDiscount = ko.observable(0),
        self.itemPrice = ko.observable(0),
        //self.comments = ko.observable('Assinatura: X<br />Reconheco o debito indicado acima.<br />Obrigado pela referencia, volte sempre!'),
        self.selectedProducts = ko.observableArray([]);

        return {
            sortJsonName: sortJsonName,
            siteName: siteName,
            logoUrl: logoUrl,
            estimateId: estimateId,
            estimateItemId: estimateItemId,
            estimatedByUser: estimatedByUser,
            estimateStatus: estimateStatus,
            personId: personId,
            clientUserId: clientUserId,
            displayName: displayName,
            firstName: firstName,
            lastName: lastName,
            telephone: telephone,
            email: email,
            street: street,
            unit: unit,
            complement: complement,
            district: district,
            city: city,
            region: region,
            myUid: myUid,
            productId: productId,
            productName: productName,
            summary: summary,
            productCode: productCode,
            unitTitle: unitTitle,
            prodImageId: prodImageId,
            itemQty: itemQty,
            stock: stock,
            unitValue: unitValue,
            itemTotal: itemTotal,
            subTotal: subTotal,
            grandTotal: grandTotal,
            displayTotal: displayTotal,
            prodTotal: prodTotal,
            createdByUser: createdByUser,
            createdOnDate: createdOnDate,
            hasLogin: hasLogin,
            items: items,
            selectedItemDiscount: selectedItemDiscount,
            payFormTitle: payFormTitle,
            selectedPayFormId: selectedPayFormId,
            selectedPayForm: selectedPayForm,
            payInMin: payInMin,
            conditionPayIn: conditionPayIn,
            conditionPayInDays: conditionPayInDays,
            conditionNumberPayments: conditionNumberPayments,
            conditionInterest: conditionInterest,
            conditionInterval: conditionInterval,
            conditionPaymentResult: conditionPaymentResult,
            conditionPayment: conditionPayment,
            conditionPayments: conditionPayments,
            conditionTotalPay: conditionTotalPay,
            conditionNPayments: conditionNPayments,
            //statuses: statuses,
            selectedStatusId: selectedStatusId,
            //loadStatuses: loadStatuses,
            currentDate: currentDate,
            salesRepName: salesRepName,
            salesRepEmail: salesRepEmail,
            salesRepPhone: salesRepPhone,
            subject: subject,
            message: message,
            siteAddress: siteAddress,
            payForms: payForms,
            loadPayForms: loadPayForms,
            payConds: payConds,
            loadPayConds: loadPayConds,
            payFormSelected: payFormSelected,
            bankIn: bankIn,
            checkIn: checkIn,
            cardIn: cardIn,
            cashIn: cashIn,
            debitIn: debitIn,
            totalsIn: totalsIn,
            balance: balance,
            cashBack: cashBack,
            couponDiscount: couponDiscount,
            originalProdTotal: originalProdTotal,
            oldItemQty: oldItemQty,
            newItemQty: newItemQty,
            newItemDiscount: newItemDiscount,
            itemPrice: itemPrice,
            //comments: comments,
            selectedProducts: selectedProducts,
            payCondDisc: payCondDisc,
            blocked: blocked,
            reasonBlocked: reasonBlocked,
            currentPlace: currentPlace
        };

    }();

    ko.applyBindings(my.vm);

};