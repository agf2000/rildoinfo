
my.viewModel = function () {

    // product view model
    my.Product = function () {
        var self = this;

        self.productId = ko.observable();
        self.productRef = ko.observable();
        self.barcode = ko.observable();
        //self.productCode = ko.observable();
        self.productName = ko.observable();
        self.summary = ko.observable();
        self.productUnit = ko.observable();
        self.productImageId = ko.observable();
        self.extension = ko.observable();
        self.unitValue = ko.observable();
        self.finan_Sale_Price = ko.observable();
        self.finan_Special_Price = ko.observable();
        self.itemQty = ko.observable();
        self.qTyStockSet = ko.observable();
        self.totalValue = ko.observable();
        self.visible = ko.observable(true);
        self.increaseQuantity = function (quantity) {
            self.qTy(self.qTy() + quantity);
        };
        self.categoriesNames = ko.observable();
        self.finan_Rep = ko.observable();
        self.finan_SalesPerson = ko.observable();
        self.finan_Tech = ko.observable();
        self.finan_Telemarketing = ko.observable();
        self.finan_Manager = ko.observable();
        self.finan_Cost = ko.observable();
        self.categoriesNames = ko.observable();
        self.createdByUser = ko.observable();
        self.createdOnDate = ko.observable();        
    };

    // product image view model
    my.ProductImage = function () {
        this.ProductImageId = ko.observable();
        this.ProductId = ko.observable();
        this.ProductImageUrl = ko.observable();
        this.ContentLength = ko.observable();
        this.FileName = ko.observable();
        this.Extension = ko.observable();
        this.CreatedByUser = ko.observable();
        this.CreatedOnDate = ko.observable();
        this.ModifiedByUser = ko.observable();
        this.ModifiedOnDate = ko.observable();
        this.ProductName = ko.observable();
        this.ListOrder = ko.observable();
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

    my.User = function (data) {
        var self = this;
        data = data || {};

        self.userId = data.UserId;
        self.displayName = data.DisplayName;
        self.groupName = data.GroupName;
    };

    my.EstimateHistory = function (data) {
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
        self.historyId = data.EstimateHistoryId;
        self.historyText = data.HistoryText;
        self.createdOnDate = moment(data.CreatedOnDate).fromNow();
        self.allowed = data.Allowed;
        self.historyComments = ko.observableArray([]);

        self.newCommentHistory = ko.observable();
        self.addHistoryComment = function (index, data, e) {

            e.currentTarget.value = 'Um momento...';
            e.currentTarget.disabled = true;

            var EstimateHistoryComment = {
                PortalId: portalID,
                EstimateHistoryId: self.historyId,
                CommentText: my.converter.makeHtml(self.newCommentHistory().trim()),
                Locked: false,
                CreatedByUser: userID,
                CreatedOnDate: moment().format()
            };

            $.ajax({
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                url: '/desktopmodules/riw/api/estimates/updateHistoryComment',
                data: JSON.stringify({
                    dto: EstimateHistoryComment,
                    messageIndex: index,
                    connId: my.hub.connection.id
                })
            }).done(function (commentData) {
                if (commentData.Result.indexOf("success") !== -1) {
                    my.vm.filteredHistories()[index].historyComments.unshift(new my.HistoryComment(commentData.EstimateHistoryComment));
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
        self.historyId = data.EstimateHistoryId;
        self.commentId = data.CommentId;
        self.commentText = data.CommentText;
        self.createdOnDate = moment(data.CreatedOnDate).fromNow();
    };

    my.EstimateMessage = function (data) {
        var self = this;
        data = data || {};

        self.messageByAvatar = data.Avatar ? '/portals/0/' + data.Avatar + '?w=45&h=45&mode=crop' : '/desktopmodules/rildoinfo/webapi/content/images/user.png?w=45&h=45';
        self.messageByName = data.DisplayName;
        self.messageId = data.EstimateMessageId;
        self.messageText = data.MessageText;
        self.createdOnDate = moment(data.CreatedOnDate).fromNow();
        self.allowed = data.Allowed;
        self.messageComments = ko.observableArray([]);

        self.newCommentMessage = ko.observable();
        //self.pushComment = function (data) {
        //    self.messageComments.unshift(new my.MessageComment(data));
        //};
        self.addMessageComment = function (index, data, e) {
            //e.preventDefault();

            e.currentTarget.value = 'Um momento...';
            e.currentTarget.disabled = true;

            var EstimateMessageComment = {
                PortalId: portalID,
                EstimateMessageId: self.messageId,
                CommentText: my.converter.makeHtml(self.newCommentMessage().trim()),
                Locked: true,
                CreatedByUser: userID,
                CreatedOnDate: moment().format()
            };

            $.ajax({
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                url: '/desktopmodules/riw/api/estimates/updateMessageComment',
                data: JSON.stringify({
                    dto: EstimateMessageComment,
                    messageIndex: index,
                    connId: my.hub.connection.id
                })
            }).done(function (commentData) {
                if (commentData.Result.indexOf("success") !== -1) {
                    my.vm.filteredMessages()[index].messageComments.unshift(new my.MessageComment(commentData.EstimateMessageComment));
                    self.newCommentMessage(null);
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
        if (data.MessageComments) {
            var mappedPosts = $.map(data.MessageComments, function (item) { return new my.MessageComment(item); });
            self.messageComments(mappedPosts);
        }
        self.toggleComment = function (item, event) {
            $(event.target).next().find('.publishComment').toggle();
            $(event.target).next().find('.commentTextArea').focus();
        }
    };

    my.MessageComment = function (data) {
        var self = this;
        data = data || {};

        self.commentedByAvatar = data.Avatar ? '/portals/0/' + data.Avatar + '?w=45&h=45&mode=crop' : '/desktopmodules/rildoinfo/webapi/content/images/user.png?w=45&h=45';
        self.commentedByName = data.DisplayName;
        self.messageId = data.EstimateMessageId;
        self.commentId = data.CommentId;
        self.commentText = data.CommentText;
        self.createdOnDate = moment(data.CreatedOnDate).fromNow();
    };
    
    // knockout js view model
    my.vm = function () {
        // this is knockout view model
        var self = this;

        self.sortProducts = function () {
            self.prodImages.sort(function (left, right) {
                return (left.ListOrder() < right.ListOrder() ? -1 : 1);
            });
        },

        self.estimateHistories = ko.observableArray([]),
        self.filterHistoryTerm = ko.observable(''),
        self.filteredHistories = ko.dependentObservable(function () {
            var filter = this.filterHistoryTerm().toLowerCase();
            if (!filter) {
                return self.estimateHistories();
            } else {
                return ko.utils.arrayFilter(self.estimateHistories(), function (item) {
                    return ko.utils.arrayFilter([item.historyText.toLowerCase()], function (str) {
                        return str.indexOf(filter) !== -1;
                    }).length > 0;
                });
            }
        }, self),

        self.pushHistory = function (item) {
            self.estimateHistories.unshift(new my.EstimateHistory()
                .historyByAvatar = item.Avatar ? '/portals/0/' + item.Avatar + '?w=45&h=45&mode=crop' : '/desktopmodules/rildoinfo/webapi/content/images/user.png?w=45&h=45'
                .historyByName = item.DisplayName
                .historyId = item.EstimateHistoryId
                .historyText = item.HistoryText
                .createdOnDate = item.CreatedOnDate
                .allowed = item.Allowed);
        },

        self.estimateMessages = ko.observableArray([]),
        self.filterMessageTerm = ko.observable(''),
        self.filteredMessages = ko.dependentObservable(function () {
            var filter = this.filterMessageTerm().toLowerCase();
            if (!filter) {
                return self.estimateMessages();
            } else {
                return ko.utils.arrayFilter(self.estimateMessages(), function (item) {
                    return ko.utils.arrayFilter([item.messageText.toLowerCase()], function (str) {
                        return str.indexOf(filter) !== -1;
                    }).length > 0;
                });
            }
        }, self),

        self.pushMessage = function (item) {
            self.estimateMessages.unshift(new my.EstimateMessage()
                .messageByAvatar = item.Avatar ? '/portals/0/' + item.Avatar + '?w=45&h=45&mode=crop' : '/desktopmodules/rildoinfo/webapi/content/images/user.png?w=45&h=45'
                .messageByName = item.DisplayName
                .messageId = item.EstimateMessageId
                .messageText = item.MessageText
                .createdOnDate = item.CreatedOnDate
                .allowed = item.Allowed);
        },

        self.connectedUsers = ko.observableArray([]),
        self.productNetSale = ko.observable(0),
        self.filter = ko.observable(''),
        self.qTy = ko.observable(0),
        self.personId = ko.observable(),
        self.clientDisplayName = ko.observable(),
        self.clientEmail = ko.observable();
        self.clientPhone = ko.observable(),
        self.clientCell = ko.observable(),
        self.clientUserId = ko.observable(),
        self.clientAddress = ko.observable(),
        self.prodImages = ko.observableArray([]),
        self.subject = ko.observable(''),
        self.message = ko.observable(''),
        self.payFormTitle = ko.observable(),
        self.selectedPayFormId = ko.observable(),
        self.selectedPayForm = ko.observable(''),
        self.payInMin = ko.observable(),
        self.conditionPayIn = ko.observable(0),
        self.conditionPayInDays = ko.observable(0),
        self.conditionNumberPayments = ko.observable(0),
        self.conditionInterest = ko.observable(0),
        self.conditionInterval = ko.observable(0),
        self.clientConnected = ko.observable(false),

        self.conditionPaymentResult = function () {
            $('#payment').text(conditionPayment()).formatCurrency({ region: 'pt-BR' });
        },

        self.conditionPayment = ko.computed({
            read: function () {
                var result = 0.00;
                if (conditionNumberPayments() !== 0) {
                    var nPayments = (conditionPayIn() > 0 ? (conditionNumberPayments() - 1) : conditionNumberPayments());
                    var totalMinusPayIn = (productNetSale() - conditionPayIn());
                    result = (my.pmt(conditionInterest(), 1, nPayments, (-totalMinusPayIn), 0));
                }
                return result;
            },
            write: function () {
                var result = 0.00;
                if (conditionNumberPayments() !== 0) {
                    var nPayments = (conditionPayIn() > 0 ? (conditionNumberPayments() - 1) : conditionNumberPayments());
                    var totalMinusPayIn = (productNetSale() - conditionPayIn());
                    result = (my.pmt(conditionInterest(), 1, nPayments, (-totalMinusPayIn), 0));
                }
                return result;
            },
            owner: self
        }),

        self.conditionPayments = ko.computed({
            read: function () {
                var result = 0.00;
                var nPayments = (conditionPayIn() > 0 ? (conditionNumberPayments() - 1) : conditionNumberPayments());
                result = (conditionPayment() * nPayments);
                return result;
            },
            write: function () {
                var result = 0.00;
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
        //self.selectedStatusId = ko.observable(),
        //self.loadStatuses = function () {
        //    $.getJSON('/desktopmodules/rildoinfo/api/statuses/GetStatuses?portalId=' + portalID + '&isDeleted=False', function (data) {
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
            $.getJSON("/desktopmodules/riw/api/payconditions/getPayConds?portalId=" + portalID + "&pcType=all&pcStart=" + my.vm.productNetSale(), function (data) {
                $.each(data, function (i, pc) {
                    self.payConds.push(new my.PayCond()
                        .payCondTitle(pc.PayCondTitle)
                        .totalParcelado(pc.TotalParcelado)
                        .totalPayCond(pc.TotalPayCond)
                        .parcela(pc.Parcela)
                        .payCondN(pc.PayCondN)
                        .payCondPerc(pc.PayCondPerc)
                        .payIn(pc.PayIn)
                        .payInDays(pc.PayInDays)
                        .payCondDisc(pc.PayCondDisc)
                        .interval(pc.Intervalo));
                });
            });
        },

        self.payFormSelected = function (e) {
            if (e.item.index() > -1) {
                var dataItem = this.dataItem(e.item.index());
                $('#payCondMsg').html('<i class="fa fa-spinner fa-spin"></i>');
                my.startPayCondGrid(dataItem.PayFormId);
                $('#payCondGrid').data("kendoGrid").dataSource.read();
                setTimeout(function () {
                    var target = $('#payCondGrid');
                    target.data("kendoGrid").table.focus();
                    target.find("tbody tr:first").addClass('k-state-selected');
                }, 600);
            }
        },
        
        self.chosenPayCond = ko.observable(0),
        self.productGrossSale = ko.observable(0),
        self.productDiscount = ko.observable(0),
        self.productTotalPaid = ko.observable(0),
        self.paidComission = ko.observable(0),
        self.newTotal = ko.observable(0),
        self.salesPersonId = ko.observable(0),
        self.salesRepEmail = ko.observable(''),
        self.salesRepPhone = ko.observable(''),
        self.salesRepName = ko.observable(''),
        self.statusTitle = ko.observable(''),
        self.viewPrice = ko.observable(true),
        self.estimateLocked = ko.observable(false),
        self.discountTotalPerc = ko.observable(0),
        self.discountTotalValue = ko.observable(0),

        // ajax function to get product images
        self.loadProductImages = function () {
            $.ajax({
                url: '/desktopmodules/riw/api/products/GetProductImages?productId=' + my.personId,
                cache: false
            }).done(function (data) {
                self.prodImages.removeAll();
                $.each(data, function (i, pi) {
                    self.prodImages.push(new my.ProductImage()
                        .ContentLength(pi.ContentLength)
                        .CreatedByUser(pi.CreatedByUser)
                        .CreatedOnDate(pi.CreatedOnDate)
                        .Extension(pi.Extension)
                        .FileName(pi.FileName)
                        .ModifiedByUser(pi.ModifiedByUser)
                        .ModifiedOnDate(pi.ModifiedOnDate)
                        .ProductId(pi.ProductId)
                        .ProductImageId(pi.ProductImageId)
                        .ProductImageUrl(pi.ProductImageUrl)
                        .ProductName(pi.ProductName)
                        .ListOrder(pi.ListOrder));
                });

                // initiate colobox jquery plugin
                //$(".photo").colorbox();
                $('.photo').click(function (event) {
                    event.preventDefault(); // this just cancels the default link behavior.
                    parent.showColorBox($(this).attr("href")); //this makes the parent window load the showColorBox function, using the a.colorbox href value
                });

                self.sortProducts();
            });
        },

        self.productsToAdd = ko.observableArray([]),
        self.createdOnDate = ko.observable(),
        self.selectedProducts = ko.observableArray([]);
        
        // make view models available for apps
        return {
            sortProducts: sortProducts,
            estimateHistories: estimateHistories,
            filterHistoryTerm: filterHistoryTerm,
            filteredHistories: filteredHistories,
            estimateMessages: estimateMessages,
            filterMessageTerm: filterMessageTerm,
            filteredMessages: filteredMessages,
            pushMessage: pushMessage,
            connectedUsers: connectedUsers,
            productNetSale: productNetSale,
            filter: filter,
            qTy: qTy,
            personId: personId,
            clientDisplayName: clientDisplayName,
            clientEmail: clientEmail,
            clientPhone: clientPhone,
            clientCell: clientCell,
            clientUserId: clientUserId,
            clientAddress: clientAddress,
            prodImages: prodImages,
            subject: subject,
            message: message,
            payFormTitle: payFormTitle,
            selectedPayFormId: selectedPayFormId,
            selectedPayForm: selectedPayForm,
            payInMin: payInMin,
            conditionPayIn: conditionPayIn,
            conditionPayInDays: conditionPayInDays,
            conditionNumberPayments: conditionNumberPayments,
            conditionInterest: conditionInterest,
            conditionInterval: conditionInterval,
            clientConnected: clientConnected,
            conditionPaymentResult: conditionPaymentResult,
            conditionPayment: conditionPayment,
            conditionPayments: conditionPayments,
            conditionTotalPay: conditionTotalPay,
            conditionNPayments: conditionNPayments,
            //statuses: statuses,
            //selectedStatusId: selectedStatusId,
            //loadStatuses: loadStatuses,
            currentDate: currentDate,
            payForms: payForms,
            loadPayForms: loadPayForms,
            payConds: payConds,
            loadPayConds: loadPayConds,
            payFormSelected: payFormSelected,
            chosenPayCond: chosenPayCond,
            productGrossSale: productGrossSale,
            productDiscount: productDiscount,
            productTotalPaid: productTotalPaid,
            paidComission: paidComission,
            newTotal: newTotal,
            salesPersonId: salesPersonId,
            salesRepEmail: salesRepEmail,
            salesRepPhone: salesRepPhone,
            salesRepName: salesRepName,
            statusTitle: statusTitle,
            viewPrice: viewPrice,
            estimateLocked: estimateLocked,
            discountTotalPerc: discountTotalPerc,
            discountTotalValue: discountTotalValue,
            loadProductImages: loadProductImages,
            productsToAdd: productsToAdd,
            createdOnDate: createdOnDate,
            selectedProducts: selectedProducts
        };

    }();

    ////textarea autosize
    //ko.bindingHandlers.jqAutoresize = {
    //    init: function (element, valueAccessor, aBA, vm) {
    //        if (!$(element).hasClass('msgTextArea')) {
    //            $(element).css('height', '1em');
    //        }
    //        $(element).autosize();
    //    }
    //};

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