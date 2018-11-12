
$(function () {

    my.payCondId = 0;

    $.valHooks.textarea = {
        get: function (elem) {
            return elem.value.replace(/\r?\n/g, "\r\n");
        }
    };

    my.viewModel();

    $('#moduleTitleSkinObject').html(moduleTitle);

    var buttons = '<ul class="inline">';
    buttons += '<li><a href="' + addressURL + '" class="btn btn-primary btn-medium" title="Endereço"><i class="fa fa-book fa-lg"></i></a></li>';
    buttons += '<li><a href="' + registrationURL + '" class="btn btn-primary btn-medium" title="Cadastro"><i class="fa fa-edit fa-lg"></i></a></li>';
    buttons += '<li><a href="' + syncURL + '" class="btn btn-primary btn-medium" title="Orçamento"><i class="fa fa-refresh fa-lg"></i></a></li>';
    buttons += '<li><a href="' + estimateURL + '" class="btn btn-primary btn-medium" title="Orçamento"><i class="fa fa-usd fa-lg"></i></a></li>';
    buttons += '<li><a href="' + smtpURL + '" class="btn btn-primary btn-medium" title="SMTP"><i class="fa fa-envelope-o fa-lg"></i></a></li>';
    buttons += '<li><a href="' + statusesManagerURL + '" class="btn btn-primary btn-medium" title="Status"><i class="fa fa-check-circle fa-lg"></i></a></li>';
    buttons += '<li><a href="' + websiteManagerURL + '" class="btn btn-primary btn-medium" title="Website"><i class="fa fa-bookmark fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + templatesManagerURL + '" class="btn btn-primary btn-medium" title="Templates"><i class="fa fa-puzzle-piece fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + davReturnsURL + '" class="btn btn-primary btn-medium" title="DAVs"><i class="fa fa-briefcase fa-lg"></i></a></li>';
    buttons += '</ul>';
    $('#buttons').html(buttons);

    $('.icon-info-sign').popover({
        placement: function (tip, element) {
            var $element, above, actualHeight, actualWidth, below, boundBottom, boundLeft, boundRight, boundTop, elementAbove, elementBelow, elementLeft, elementRight, isWithinBounds, left, pos, right;
            isWithinBounds = function (elementPosition) {
                return boundTop < elementPosition.top && boundLeft < elementPosition.left && boundRight > (elementPosition.left + actualWidth) && boundBottom > (elementPosition.top + actualHeight);
            };
            $element = $(element);
            pos = $.extend({}, $element.offset(), {
                width: element.offsetWidth,
                height: element.offsetHeight
            });
            actualWidth = 350;
            actualHeight = 200;
            boundTop = $(document).scrollTop();
            boundLeft = $(document).scrollLeft();
            boundRight = boundLeft + $(window).width();
            boundBottom = boundTop + $(window).height();
            elementAbove = {
                top: pos.top - actualHeight,
                left: pos.left + pos.width / 2 - actualWidth / 2
            };
            elementBelow = {
                top: pos.top + pos.height,
                left: pos.left + pos.width / 2 - actualWidth / 2
            };
            elementLeft = {
                top: pos.top + pos.height / 2 - actualHeight / 2,
                left: pos.left - actualWidth
            };
            elementRight = {
                top: pos.top + pos.height / 2 - actualHeight / 2,
                left: pos.left + pos.width
            };
            above = isWithinBounds(elementAbove);
            below = isWithinBounds(elementBelow);
            left = isWithinBounds(elementLeft);
            right = isWithinBounds(elementRight);
            if (above) {
                return "top";
            } else {
                if (below) {
                    return "bottom";
                } else {
                    if (left) {
                        return "left";
                    } else {
                        if (right) {
                            return "right";
                        } else {
                            return "right";
                        }
                    }
                }
            }
        },
        delay: {
            show: 100,
            hide: 1000
        },
        trigger: 'hover'
    });

    $('#payFormTabs').kendoTabStrip({
        animation: {
            // fade-out current tab over 1000 milliseconds
            close: {
                effects: "fadeOut"
            },
            // fade-in new tab over 500 milliseconds
            open: {
                effects: "fadeIn"
            }
        },
        select: function (e) {
            switch (e.item.id) {
                case 'tab_2':
                    loadCheckGrid();
                    my.vm.payCondType(5);
                    break;
                case 'tab_3':
                    loadBankGrid();
                    my.vm.payCondType(1);
                    break;
                case 'tab_4':
                    loadVisaGrid();
                    my.vm.payCondType(2);
                    break;
                case 'tab_5':
                    loadMCGrid();
                    my.vm.payCondType(3);
                    break;
                case 'tab_6':
                    loadDinersGrid();
                    my.vm.payCondType(6);
                    break;
                case 'tab_7':
                    loadAmexGrid();
                    my.vm.payCondType(4);
                    break;
                case 'tab_8':
                    loadDebitGrid();
                    my.vm.payCondType(7);
                    break;
                default:
                    $.ajax({
                        url: '/desktopmodules/riw/api/payconditions/getPayConds?portalId=' + portalID + '&pcType=0&pcStart=0'
                    }).done(function (data) {
                        if (data) {
                            $.each(data, function (i, item) {
                                $('#cashDiscount').data('kendoNumericTextBox').value(item.PayCondDisc);
                                $('#cashMessageTextArea').val(item.PayCondTitle);
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
                        }
                    }).fail(function (jqXHR, textStatus) {
                        console.log(jqXHR.responseText);
                    });
            }
        }
    });

    $('#cashDiscount').kendoNumericTextBox({
        min: 0,
        format: "##.00 '%'"
    });

    $.ajax({
        url: '/desktopmodules/riw/api/payconditions/getPayConds?portalId=' + portalID + '&pcType=0&pcStart=0'
    }).done(function (data) {
        if (data) {
            $.each(data, function (i, item) {
                my.payCondId = item.PayCondId;
                $('#cashDiscount').data('kendoNumericTextBox').value(item.PayCondDisc);
                $('#cashMessageTextArea').val(item.PayCondTitle);
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
        }
    }).fail(function (jqXHR, textStatus) {
        console.log(jqXHR.responseText);
    });

    loadCheckGrid = function () {
        $('#checkPreGrid').kendoGrid({
            dataSource: new kendo.data.DataSource({
                transport: {
                    read: {
                        url: '/desktopmodules/riw/api/payconditions/getPayConds?portalId=' + portalID + '&pcType=5&pcStart=-1'
                    }
                },
                schema: {
                    model: {
                        id: 'PayCondId'
                        //fields: {
                        //    ProductEstimateOriginalPrice: {
                        //        editable: false, nullable: false
                        //    },
                        //    ProductEstimatePrice: {
                        //        editable: false, nullable: false
                        //    },
                        //    ProductQty: { type: 'number', validation: { required: { message: 'Campo obrgat&#243;rio' } } },
                        //    ProductDiscount: { type: "number", validation: { min: 0, required: { message: 'Campo obrgat&#243;rio' } } },
                        //    ExtendedAmount: {
                        //        editable: false, nullable: false
                        //    },
                        //    selected: {
                        //        editable: false, nullable: false
                        //    }
                        //}
                    }
                },
            }),
            toolbar: kendo.template($("#tmplCheckPreToolbar").html()),
            columns: [
                {
                    title: 'Apartir de',
                    field: 'PayCondStart',
                    format: '{0:n}'
                },
                {
                    title: 'Núm. Parcelas',
                    field: 'PayCondN'
                },
                {
                    title: 'Juros (a.m.)',
                    field: 'PayCondPerc',
                    template: '#= kendo.format("{0:p}",(PayCondPerc/100)) #'
                },
                {
                    title: 'Entrada %',
                    field: 'PayCondIn',
                    template: '#= kendo.format("{0:p}",(PayCondIn/100)) #'
                },
                {
                    title: 'Dias p/ Entrada',
                    field: 'PayInDays'
                },
                {
                    title: 'Intervalo',
                    field: 'PayCondInterval'
                },
                {
                    title: 'Grupo',
                    field: 'DiscountGroupId',
                    editor: groupDropDownEditor
                },
                {
                    command: [
                        {
                            name: "Update",
                            text: " Atualizar",
                            imageClass: "fa fa-check",
                            click: function (e) {
                                e.preventDefault();

                                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                if (dataItem) {

                                    var params = {
                                        PayCondId: dataItem.PayCondId,
                                        DiscountGroupId: dataItem.DiscountGroupId,
                                        ModifiedByUser: userID,
                                        ModifiedOnDate: moment().format(),
                                        PayCondDisc: dataItem.PayCondDisc,
                                        PayCondIn: dataItem.PayCondIn,
                                        PayInDays: dataItem.PayInDays,
                                        PayCondInterval: dataItem.PayCondInterval,
                                        PayCondN: dataItem.PayCondN,
                                        PayCondPerc: dataItem.PayCondPerc,
                                        PayCondTitle: 'Cheque Pré',
                                        PayCondType: 5,
                                        PayCondStart: dataItem.PayCondStart,
                                        PortalId: portalID,
                                        CreatedByUser: userID,
                                        CreatedOnDate: moment().format()
                                    };

                                    $.ajax({
                                        type: 'POST',
                                        url: '/desktopmodules/riw/api/payconditions/updatePayCond',
                                        data: params
                                    }).done(function (data) {
                                        if (data.Result.indexOf("success") !== -1) {
                                            $.pnotify({
                                                title: 'Sucesso!',
                                                text: 'Condição atualizada.',
                                                type: 'success',
                                                icon: 'fa fa-check fa-lg',
                                                addclass: "stack-bottomright",
                                                stack: my.stack_bottomright
                                            });
                                            $('#checkPreGrid').data('kendoGrid').dataSource.read();
                                        } else {
                                            $.pnotify({
                                                title: 'Erro!',
                                                text: data.Result,
                                                type: 'error',
                                                icon: 'fa fa-times-circle fa-lg',
                                                addclass: "stack-bottomright",
                                                stack: my.stack_bottomright
                                            });
                                        }
                                    }).fail(function (jqXHR, textStatus) {
                                        console.log(jqXHR.responseText);
                                    });
                                }
                            }
                        }
                    ],
                    width: 100
                },
                {
                    command: [
                        {
                            name: "Delete",
                            text: " Excluir",
                            imageClass: "fa fa-times",
                            click: function (e) {
                                e.preventDefault();

                                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                if (dataItem) {

                                    var $dialog = $('<div></div>')
                                        .html('<div class="confirmDialog">Tem Certeza?</div>')
                                        .dialog({
                                            autoOpen: false,
                                            modal: true,
                                            resizable: false,
                                            dialogClass: 'dnnFormPopup',
                                            open: function () {
                                                $(".ui-dialog-title").append('Aviso de Exclus&#227;o');
                                            },
                                            buttons: {
                                                'ok': {
                                                    text: 'Sim',
                                                    //priority: 'primary',
                                                    "class": 'dnnPrimaryAction',
                                                    click: function () {

                                                        $dialog.dialog('close');
                                                        $dialog.dialog('destroy');

                                                        $.ajax({
                                                            type: 'DELETE',
                                                            url: '/desktopmodules/riw/api/payconditions/removePayCond?payCondId=' + dataItem.PayCondId
                                                        }).done(function (data) {
                                                            if (data.Result.indexOf("success") !== -1) {
                                                                //$().toastmessage('showSuccessToast', 'Item excluido com sucesso!');
                                                                $.pnotify({
                                                                    title: 'Sucesso!',
                                                                    text: 'Condição excluida.',
                                                                    type: 'success',
                                                                    icon: 'fa fa-check fa-lg',
                                                                    addclass: "stack-bottomright",
                                                                    stack: my.stack_bottomright
                                                                });
                                                                $('#checkPreGrid').data('kendoGrid').dataSource.remove(dataItem);
                                                            } else {
                                                                $.pnotify({
                                                                    title: 'Erro!',
                                                                    text: data.Result,
                                                                    type: 'error',
                                                                    icon: 'fa fa-times-circle fa-lg',
                                                                    addclass: "stack-bottomright",
                                                                    stack: my.stack_bottomright
                                                                });
                                                                //$().toastmessage('showErrorToast', data.Msg);
                                                            }
                                                        }).fail(function (jqXHR, textStatus) {
                                                            console.log(jqXHR.responseText);
                                                        });
                                                    }
                                                },
                                                'cancel': {
                                                    html: 'N&#227;o',
                                                    //priority: 'secondary',
                                                    "class": 'dnnSecondaryAction',
                                                    click: function () {
                                                        $dialog.dialog('close');
                                                        $dialog.dialog('destroy');
                                                    }
                                                }
                                            }
                                        });

                                    $dialog.dialog('open');
                                }
                            }
                        }
                    ],
                    width: 90
                }
            ],
            sortable: true,
            editable: true
        });
    };

    loadBankGrid = function () {
        $('#bankPayGrid').kendoGrid({
            dataSource: new kendo.data.DataSource({
                transport: {
                    read: {
                        url: '/desktopmodules/riw/api/payconditions/getPayConds?portalId=' + portalID + '&pcType=1&pcStart=-1'
                    }
                },
                schema: {
                    model: {
                        id: 'PayCondId'
                        //fields: {
                        //    ProductEstimateOriginalPrice: {
                        //        editable: false, nullable: false
                        //    },
                        //    ProductEstimatePrice: {
                        //        editable: false, nullable: false
                        //    },
                        //    ProductQty: { type: 'number', validation: { required: { message: 'Campo obrgat&#243;rio' } } },
                        //    ProductDiscount: { type: "number", validation: { min: 0, required: { message: 'Campo obrgat&#243;rio' } } },
                        //    ExtendedAmount: {
                        //        editable: false, nullable: false
                        //    },
                        //    selected: {
                        //        editable: false, nullable: false
                        //    }
                        //}
                    }
                },
            }),
            toolbar: kendo.template($("#tmplCheckPreToolbar").html()),
            columns: [
                {
                    title: 'Apartir de',
                    field: 'PayCondStart',
                    format: '{0:n}'
                },
                {
                    title: 'Núm. Parcelas',
                    field: 'PayCondN'
                },
                {
                    title: 'Juros (a.m.)',
                    field: 'PayCondPerc',
                    template: '#= kendo.format("{0:p}",(PayCondPerc/100)) #'
                },
                {
                    title: 'Entrada %',
                    field: 'PayCondIn',
                    template: '#= kendo.format("{0:p}",(PayCondIn/100)) #'
                },
                {
                    title: 'Dias p/ Entrada',
                    field: 'PayInDays'
                },
                {
                    title: 'Intervalo',
                    field: 'PayCondInterval'
                },
                {
                    title: 'Grupo',
                    field: 'DiscountGroupId',
                    editor: groupDropDownEditor
                },
                {
                    command: [
                        {
                            name: "Update",
                            text: " Atualizar",
                            imageClass: "fa fa-check",
                            click: function (e) {
                                e.preventDefault();

                                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                if (dataItem) {

                                    var params = {
                                        PayCondId: dataItem.PayCondId,
                                        DiscountGroupId: dataItem.DiscountGroupId,
                                        ModifiedByUser: userID,
                                        ModifiedOnDate: moment().format(),
                                        PayCondDisc: dataItem.PayCondDisc,
                                        PayCondIn: dataItem.PayCondIn,
                                        PayInDays: dataItem.PayInDays,
                                        PayCondInterval: dataItem.PayCondInterval,
                                        PayCondN: dataItem.PayCondN,
                                        PayCondPerc: dataItem.PayCondPerc,
                                        PayCondTitle: 'Boleto',
                                        PayCondType: 1,
                                        PayCondStart: dataItem.PayCondStart,
                                        PortalId: portalID,
                                        CreatedByUser: userID,
                                        CreatedOnDate: moment().format()
                                    };

                                    $.ajax({
                                        type: 'POST',
                                        url: '/desktopmodules/riw/api/payconditions/updatePayCond',
                                        data: params
                                    }).done(function (data) {
                                        if (data.Result.indexOf("success") !== -1) {
                                            $.pnotify({
                                                title: 'Sucesso!',
                                                text: 'Condição atualizada.',
                                                type: 'success',
                                                icon: 'fa fa-check fa-lg',
                                                addclass: "stack-bottomright",
                                                stack: my.stack_bottomright
                                            });
                                            $('#bankPayGrid').data('kendoGrid').dataSource.read();
                                        } else {
                                            $.pnotify({
                                                title: 'Erro!',
                                                text: data.Result,
                                                type: 'error',
                                                icon: 'fa fa-times-circle fa-lg',
                                                addclass: "stack-bottomright",
                                                stack: my.stack_bottomright
                                            });
                                        }
                                    }).fail(function (jqXHR, textStatus) {
                                        console.log(jqXHR.responseText);
                                    });
                                }
                            }
                        }
                    ],
                    width: 100
                },
                {
                    command: [
                        {
                            name: "Delete",
                            text: " Excluir",
                            imageClass: "fa fa-times",
                            click: function (e) {
                                e.preventDefault();

                                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                if (dataItem) {

                                    var $dialog = $('<div></div>')
                                        .html('<div class="confirmDialog">Tem Certeza?</div>')
                                        .dialog({
                                            autoOpen: false,
                                            modal: true,
                                            resizable: false,
                                            dialogClass: 'dnnFormPopup',
                                            open: function () {
                                                $(".ui-dialog-title").append('Aviso de Exclus&#227;o');
                                            },
                                            buttons: {
                                                'ok': {
                                                    text: 'Sim',
                                                    //priority: 'primary',
                                                    "class": 'dnnPrimaryAction',
                                                    click: function () {

                                                        $dialog.dialog('close');
                                                        $dialog.dialog('destroy');

                                                        $.ajax({
                                                            type: 'DELETE',
                                                            url: '/desktopmodules/riw/api/payconditions/removePayCond?payCondId=' + dataItem.PayCondId
                                                        }).done(function (data) {
                                                            if (data.Result.indexOf("success") !== -1) {
                                                                //$().toastmessage('showSuccessToast', 'Item excluido com sucesso!');
                                                                $.pnotify({
                                                                    title: 'Sucesso!',
                                                                    text: 'Condição excluida.',
                                                                    type: 'success',
                                                                    icon: 'fa fa-check fa-lg',
                                                                    addclass: "stack-bottomright",
                                                                    stack: my.stack_bottomright
                                                                });
                                                                $('#bankPayGrid').data('kendoGrid').dataSource.remove(dataItem);
                                                            } else {
                                                                $.pnotify({
                                                                    title: 'Erro!',
                                                                    text: data.Result,
                                                                    type: 'error',
                                                                    icon: 'fa fa-times-circle fa-lg',
                                                                    addclass: "stack-bottomright",
                                                                    stack: my.stack_bottomright
                                                                });
                                                                //$().toastmessage('showErrorToast', data.Msg);
                                                            }
                                                        }).fail(function (jqXHR, textStatus) {
                                                            console.log(jqXHR.responseText);
                                                        });
                                                    }
                                                },
                                                'cancel': {
                                                    html: 'N&#227;o',
                                                    //priority: 'secondary',
                                                    "class": 'dnnSecondaryAction',
                                                    click: function () {
                                                        $dialog.dialog('close');
                                                        $dialog.dialog('destroy');
                                                    }
                                                }
                                            }
                                        });

                                    $dialog.dialog('open');
                                }
                            }
                        }
                    ],
                    width: 90
                }
            ],
            editable: true
        });
    };

    loadVisaGrid = function () {
        $('#visaGrid').kendoGrid({
            dataSource: new kendo.data.DataSource({
                transport: {
                    read: {
                        url: '/desktopmodules/riw/api/payconditions/getPayConds?portalId=' + portalID + '&pcType=2&pcStart=-1'
                    }
                },
                schema: {
                    model: {
                        id: 'PayCondId'
                        //fields: {
                        //    ProductEstimateOriginalPrice: {
                        //        editable: false, nullable: false
                        //    },
                        //    ProductEstimatePrice: {
                        //        editable: false, nullable: false
                        //    },
                        //    ProductQty: { type: 'number', validation: { required: { message: 'Campo obrgat&#243;rio' } } },
                        //    ProductDiscount: { type: "number", validation: { min: 0, required: { message: 'Campo obrgat&#243;rio' } } },
                        //    ExtendedAmount: {
                        //        editable: false, nullable: false
                        //    },
                        //    selected: {
                        //        editable: false, nullable: false
                        //    }
                        //}
                    }
                },
            }),
            toolbar: kendo.template($("#tmplCheckPreToolbar").html()),
            columns: [
                {
                    title: 'Apartir de',
                    field: 'PayCondStart',
                    format: '{0:n}'
                },
                {
                    title: 'Núm. Parcelas',
                    field: 'PayCondN'
                },
                {
                    title: 'Juros (a.m.)',
                    field: 'PayCondPerc',
                    template: '#= kendo.format("{0:p}",(PayCondPerc/100)) #'
                },
                {
                    title: 'Entrada %',
                    field: 'PayCondIn',
                    template: '#= kendo.format("{0:p}",(PayCondIn/100)) #'
                },
                {
                    title: 'Dias p/ Entrada',
                    field: 'PayInDays'
                },
                {
                    title: 'Intervalo',
                    field: 'PayCondInterval'
                },
                {
                    title: 'Grupo',
                    field: 'DiscountGroupId',
                    editor: groupDropDownEditor
                },
                {
                    command: [
                        {
                            name: "Update",
                            text: " Atualizar",
                            imageClass: "fa fa-check",
                            click: function (e) {
                                e.preventDefault();

                                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                if (dataItem) {

                                    var params = {
                                        PayCondId: dataItem.PayCondId,
                                        DiscountGroupId: dataItem.DiscountGroupId,
                                        ModifiedByUser: userID,
                                        ModifiedOnDate: moment().format(),
                                        PayCondDisc: dataItem.PayCondDisc,
                                        PayCondIn: dataItem.PayCondIn,
                                        PayInDays: dataItem.PayInDays,
                                        PayCondInterval: dataItem.PayCondInterval,
                                        PayCondN: dataItem.PayCondN,
                                        PayCondPerc: dataItem.PayCondPerc,
                                        PayCondTitle: 'Visa',
                                        PayCondType: 2,
                                        PayCondStart: dataItem.PayCondStart,
                                        PortalId: portalID,
                                        CreatedByUser: userID,
                                        CreatedOnDate: moment().format()
                                    };

                                    $.ajax({
                                        type: 'POST',
                                        url: '/desktopmodules/riw/api/payconditions/updatePayCond',
                                        data: params
                                    }).done(function (data) {
                                        if (data.Result.indexOf("success") !== -1) {
                                            $.pnotify({
                                                title: 'Sucesso!',
                                                text: 'Condição atualizada.',
                                                type: 'success',
                                                icon: 'fa fa-check fa-lg',
                                                addclass: "stack-bottomright",
                                                stack: my.stack_bottomright
                                            });
                                            $('#visaGrid').data('kendoGrid').dataSource.read();
                                        } else {
                                            $.pnotify({
                                                title: 'Erro!',
                                                text: data.Result,
                                                type: 'error',
                                                icon: 'fa fa-times-circle fa-lg',
                                                addclass: "stack-bottomright",
                                                stack: my.stack_bottomright
                                            });
                                        }
                                    }).fail(function (jqXHR, textStatus) {
                                        console.log(jqXHR.responseText);
                                    });
                                }
                            }
                        }
                    ],
                    width: 100
                },
                {
                    command: [
                        {
                            name: "Delete",
                            text: " Excluir",
                            imageClass: "fa fa-times",
                            click: function (e) {
                                e.preventDefault();

                                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                if (dataItem) {

                                    var $dialog = $('<div></div>')
                                        .html('<div class="confirmDialog">Tem Certeza?</div>')
                                        .dialog({
                                            autoOpen: false,
                                            modal: true,
                                            resizable: false,
                                            dialogClass: 'dnnFormPopup',
                                            open: function () {
                                                $(".ui-dialog-title").append('Aviso de Exclus&#227;o');
                                            },
                                            buttons: {
                                                'ok': {
                                                    text: 'Sim',
                                                    //priority: 'primary',
                                                    "class": 'dnnPrimaryAction',
                                                    click: function () {

                                                        $dialog.dialog('close');
                                                        $dialog.dialog('destroy');

                                                        $.ajax({
                                                            type: 'DELETE',
                                                            url: '/desktopmodules/riw/api/payconditions/removePayCond?payCondId=' + dataItem.PayCondId
                                                        }).done(function (data) {
                                                            if (data.Result.indexOf("success") !== -1) {
                                                                //$().toastmessage('showSuccessToast', 'Item excluido com sucesso!');
                                                                $.pnotify({
                                                                    title: 'Sucesso!',
                                                                    text: 'Condição excluida.',
                                                                    type: 'success',
                                                                    icon: 'fa fa-check fa-lg',
                                                                    addclass: "stack-bottomright",
                                                                    stack: my.stack_bottomright
                                                                });
                                                                $('#visaGrid').data('kendoGrid').dataSource.remove(dataItem);
                                                            } else {
                                                                $.pnotify({
                                                                    title: 'Erro!',
                                                                    text: data.Result,
                                                                    type: 'error',
                                                                    icon: 'fa fa-times-circle fa-lg',
                                                                    addclass: "stack-bottomright",
                                                                    stack: my.stack_bottomright
                                                                });
                                                                //$().toastmessage('showErrorToast', data.Msg);
                                                            }
                                                        }).fail(function (jqXHR, textStatus) {
                                                            console.log(jqXHR.responseText);
                                                        });
                                                    }
                                                },
                                                'cancel': {
                                                    html: 'N&#227;o',
                                                    //priority: 'secondary',
                                                    "class": 'dnnSecondaryAction',
                                                    click: function () {
                                                        $dialog.dialog('close');
                                                        $dialog.dialog('destroy');
                                                    }
                                                }
                                            }
                                        });

                                    $dialog.dialog('open');
                                }
                            }
                        }
                    ],
                    width: 90
                }
            ],
            editable: true
        });
    };

    loadMCGrid = function () {
        $('#mcGrid').kendoGrid({
            dataSource: new kendo.data.DataSource({
                transport: {
                    read: {
                        url: '/desktopmodules/riw/api/payconditions/getPayConds?portalId=' + portalID + '&pcType=3&pcStart=-1'
                    }
                },
                schema: {
                    model: {
                        id: 'PayCondId'
                        //fields: {
                        //    ProductEstimateOriginalPrice: {
                        //        editable: false, nullable: false
                        //    },
                        //    ProductEstimatePrice: {
                        //        editable: false, nullable: false
                        //    },
                        //    ProductQty: { type: 'number', validation: { required: { message: 'Campo obrgat&#243;rio' } } },
                        //    ProductDiscount: { type: "number", validation: { min: 0, required: { message: 'Campo obrgat&#243;rio' } } },
                        //    ExtendedAmount: {
                        //        editable: false, nullable: false
                        //    },
                        //    selected: {
                        //        editable: false, nullable: false
                        //    }
                        //}
                    }
                },
            }),
            toolbar: kendo.template($("#tmplCheckPreToolbar").html()),
            columns: [
                {
                    title: 'Apartir de',
                    field: 'PayCondStart',
                    format: '{0:n}'
                },
                {
                    title: 'Núm. Parcelas',
                    field: 'PayCondN'
                },
                {
                    title: 'Juros (a.m.)',
                    field: 'PayCondPerc',
                    template: '#= kendo.format("{0:p}",(PayCondPerc/100)) #'
                },
                {
                    title: 'Entrada %',
                    field: 'PayCondIn',
                    template: '#= kendo.format("{0:p}",(PayCondIn/100)) #'
                },
                {
                    title: 'Dias p/ Entrada',
                    field: 'PayInDays'
                },
                {
                    title: 'Intervalo',
                    field: 'PayCondInterval'
                },
                {
                    title: 'Grupo',
                    field: 'DiscountGroupId',
                    editor: groupDropDownEditor
                },
                {
                    command: [
                        {
                            name: "Update",
                            text: " Atualizar",
                            imageClass: "fa fa-check",
                            click: function (e) {
                                e.preventDefault();

                                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                if (dataItem) {

                                    var params = {
                                        PayCondId: dataItem.PayCondId,
                                        DiscountGroupId: dataItem.DiscountGroupId,
                                        ModifiedByUser: userID,
                                        ModifiedOnDate: moment().format(),
                                        PayCondDisc: dataItem.PayCondDisc,
                                        PayCondIn: dataItem.PayCondIn,
                                        PayInDays: dataItem.PayInDays,
                                        PayCondInterval: dataItem.PayCondInterval,
                                        PayCondN: dataItem.PayCondN,
                                        PayCondPerc: dataItem.PayCondPerc,
                                        PayCondTitle: 'Master Card',
                                        PayCondType: 3,
                                        PayCondStart: dataItem.PayCondStart,
                                        PortalId: portalID,
                                        CreatedByUser: userID,
                                        CreatedOnDate: moment().format()
                                    };

                                    $.ajax({
                                        type: 'POST',
                                        url: '/desktopmodules/riw/api/payconditions/updatePayCond',
                                        data: params
                                    }).done(function (data) {
                                        if (data.Result.indexOf("success") !== -1) {
                                            $.pnotify({
                                                title: 'Sucesso!',
                                                text: 'Condição atualizada.',
                                                type: 'success',
                                                icon: 'fa fa-check fa-lg',
                                                addclass: "stack-bottomright",
                                                stack: my.stack_bottomright
                                            });
                                            $('#mcGrid').data('kendoGrid').dataSource.read();
                                        } else {
                                            $.pnotify({
                                                title: 'Erro!',
                                                text: data.Result,
                                                type: 'error',
                                                icon: 'fa fa-times-circle fa-lg',
                                                addclass: "stack-bottomright",
                                                stack: my.stack_bottomright
                                            });
                                        }
                                    }).fail(function (jqXHR, textStatus) {
                                        console.log(jqXHR.responseText);
                                    });
                                }
                            }
                        }
                    ],
                    width: 100
                },
                {
                    command: [
                        {
                            name: "Delete",
                            text: " Excluir",
                            imageClass: "fa fa-times",
                            click: function (e) {
                                e.preventDefault();

                                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                if (dataItem) {

                                    var $dialog = $('<div></div>')
                                        .html('<div class="confirmDialog">Tem Certeza?</div>')
                                        .dialog({
                                            autoOpen: false,
                                            modal: true,
                                            resizable: false,
                                            dialogClass: 'dnnFormPopup',
                                            open: function () {
                                                $(".ui-dialog-title").append('Aviso de Exclus&#227;o');
                                            },
                                            buttons: {
                                                'ok': {
                                                    text: 'Sim',
                                                    //priority: 'primary',
                                                    "class": 'dnnPrimaryAction',
                                                    click: function () {

                                                        $dialog.dialog('close');
                                                        $dialog.dialog('destroy');

                                                        $.ajax({
                                                            type: 'DELETE',
                                                            url: '/desktopmodules/riw/api/payconditions/removePayCond?payCondId=' + dataItem.PayCondId
                                                        }).done(function (data) {
                                                            if (data.Result.indexOf("success") !== -1) {
                                                                //$().toastmessage('showSuccessToast', 'Item excluido com sucesso!');
                                                                $.pnotify({
                                                                    title: 'Sucesso!',
                                                                    text: 'Condição excluida.',
                                                                    type: 'success',
                                                                    icon: 'fa fa-check fa-lg',
                                                                    addclass: "stack-bottomright",
                                                                    stack: my.stack_bottomright
                                                                });
                                                                $('#mcGrid').data('kendoGrid').dataSource.remove(dataItem);
                                                            } else {
                                                                $.pnotify({
                                                                    title: 'Erro!',
                                                                    text: data.Result,
                                                                    type: 'error',
                                                                    icon: 'fa fa-times-circle fa-lg',
                                                                    addclass: "stack-bottomright",
                                                                    stack: my.stack_bottomright
                                                                });
                                                                //$().toastmessage('showErrorToast', data.Msg);
                                                            }
                                                        }).fail(function (jqXHR, textStatus) {
                                                            console.log(jqXHR.responseText);
                                                        });
                                                    }
                                                },
                                                'cancel': {
                                                    html: 'N&#227;o',
                                                    //priority: 'secondary',
                                                    "class": 'dnnSecondaryAction',
                                                    click: function () {
                                                        $dialog.dialog('close');
                                                        $dialog.dialog('destroy');
                                                    }
                                                }
                                            }
                                        });

                                    $dialog.dialog('open');
                                }
                            }
                        }
                    ],
                    width: 90
                }
            ],
            editable: true
        });
    };

    loadDinersGrid = function () {
        $('#dinersGrid').kendoGrid({
            dataSource: new kendo.data.DataSource({
                transport: {
                    read: {
                        url: '/desktopmodules/riw/api/payconditions/getPayConds?portalId=' + portalID + '&pcType=5&pcStart=-1'
                    }
                },
                schema: {
                    model: {
                        id: 'PayCondId'
                        //fields: {
                        //    ProductEstimateOriginalPrice: {
                        //        editable: false, nullable: false
                        //    },
                        //    ProductEstimatePrice: {
                        //        editable: false, nullable: false
                        //    },
                        //    ProductQty: { type: 'number', validation: { required: { message: 'Campo obrgat&#243;rio' } } },
                        //    ProductDiscount: { type: "number", validation: { min: 0, required: { message: 'Campo obrgat&#243;rio' } } },
                        //    ExtendedAmount: {
                        //        editable: false, nullable: false
                        //    },
                        //    selected: {
                        //        editable: false, nullable: false
                        //    }
                        //}
                    }
                },
            }),
            toolbar: kendo.template($("#tmplCheckPreToolbar").html()),
            columns: [
                {
                    title: 'Apartir de',
                    field: 'PayCondStart',
                    format: '{0:n}'
                },
                {
                    title: 'Núm. Parcelas',
                    field: 'PayCondN'
                },
                {
                    title: 'Juros (a.m.)',
                    field: 'PayCondPerc',
                    template: '#= kendo.format("{0:p}",(PayCondPerc/100)) #'
                },
                {
                    title: 'Entrada %',
                    field: 'PayCondIn',
                    template: '#= kendo.format("{0:p}",(PayCondIn/100)) #'
                },
                {
                    title: 'Dias p/ Entrada',
                    field: 'PayInDays'
                },
                {
                    title: 'Intervalo',
                    field: 'PayCondInterval'
                },
                {
                    title: 'Grupo',
                    field: 'DiscountGroupId',
                    editor: groupDropDownEditor
                },
                {
                    command: [
                        {
                            name: "Update",
                            text: " Atualizar",
                            imageClass: "fa fa-check",
                            click: function (e) {
                                e.preventDefault();

                                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                if (dataItem) {

                                    var params = {
                                        PayCondId: dataItem.PayCondId,
                                        DiscountGroupId: dataItem.DiscountGroupId,
                                        ModifiedByUser: userID,
                                        ModifiedOnDate: moment().format(),
                                        PayCondDisc: dataItem.PayCondDisc,
                                        PayCondIn: dataItem.PayCondIn,
                                        PayInDays: dataItem.PayInDays,
                                        PayCondInterval: dataItem.PayCondInterval,
                                        PayCondN: dataItem.PayCondN,
                                        PayCondPerc: dataItem.PayCondPerc,
                                        PayCondTitle: 'Dinners Club',
                                        PayCondType: 6,
                                        PayCondStart: dataItem.PayCondStart,
                                        PortalId: portalID,
                                        CreatedByUser: userID,
                                        CreatedOnDate: moment().format()
                                    };

                                    $.ajax({
                                        type: 'POST',
                                        url: '/desktopmodules/riw/api/payconditions/updatePayCond',
                                        data: params
                                    }).done(function (data) {
                                        if (data.Result.indexOf("success") !== -1) {
                                            $.pnotify({
                                                title: 'Sucesso!',
                                                text: 'Condição atualizada.',
                                                type: 'success',
                                                icon: 'fa fa-check fa-lg',
                                                addclass: "stack-bottomright",
                                                stack: my.stack_bottomright
                                            });
                                            $('#dinersGrid').data('kendoGrid').dataSource.read();
                                        } else {
                                            $.pnotify({
                                                title: 'Erro!',
                                                text: data.Result,
                                                type: 'error',
                                                icon: 'fa fa-times-circle fa-lg',
                                                addclass: "stack-bottomright",
                                                stack: my.stack_bottomright
                                            });
                                        }
                                    }).fail(function (jqXHR, textStatus) {
                                        console.log(jqXHR.responseText);
                                    });
                                }
                            }
                        }
                    ],
                    width: 100
                },
                {
                    command: [
                        {
                            name: "Delete",
                            text: " Excluir",
                            imageClass: "fa fa-times",
                            click: function (e) {
                                e.preventDefault();

                                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                if (dataItem) {

                                    var $dialog = $('<div></div>')
                                        .html('<div class="confirmDialog">Tem Certeza?</div>')
                                        .dialog({
                                            autoOpen: false,
                                            modal: true,
                                            resizable: false,
                                            dialogClass: 'dnnFormPopup',
                                            open: function () {
                                                $(".ui-dialog-title").append('Aviso de Exclus&#227;o');
                                            },
                                            buttons: {
                                                'ok': {
                                                    text: 'Sim',
                                                    //priority: 'primary',
                                                    "class": 'dnnPrimaryAction',
                                                    click: function () {

                                                        $dialog.dialog('close');
                                                        $dialog.dialog('destroy');

                                                        $.ajax({
                                                            type: 'DELETE',
                                                            url: '/desktopmodules/riw/api/payconditions/removePayCond?payCondId=' + dataItem.PayCondId
                                                        }).done(function (data) {
                                                            if (data.Result.indexOf("success") !== -1) {
                                                                //$().toastmessage('showSuccessToast', 'Item excluido com sucesso!');
                                                                $.pnotify({
                                                                    title: 'Sucesso!',
                                                                    text: 'Condição excluida.',
                                                                    type: 'success',
                                                                    icon: 'fa fa-check fa-lg',
                                                                    addclass: "stack-bottomright",
                                                                    stack: my.stack_bottomright
                                                                });
                                                                $('#dinersGrid').data('kendoGrid').dataSource.remove(dataItem);
                                                            } else {
                                                                $.pnotify({
                                                                    title: 'Erro!',
                                                                    text: data.Result,
                                                                    type: 'error',
                                                                    icon: 'fa fa-times-circle fa-lg',
                                                                    addclass: "stack-bottomright",
                                                                    stack: my.stack_bottomright
                                                                });
                                                                //$().toastmessage('showErrorToast', data.Msg);
                                                            }
                                                        }).fail(function (jqXHR, textStatus) {
                                                            console.log(jqXHR.responseText);
                                                        });
                                                    }
                                                },
                                                'cancel': {
                                                    html: 'N&#227;o',
                                                    //priority: 'secondary',
                                                    "class": 'dnnSecondaryAction',
                                                    click: function () {
                                                        $dialog.dialog('close');
                                                        $dialog.dialog('destroy');
                                                    }
                                                }
                                            }
                                        });

                                    $dialog.dialog('open');
                                }
                            }
                        }
                    ],
                    width: 90
                }
            ],
            editable: true
        });
    };

    loadAmexGrid = function () {
        $('#amexGrid').kendoGrid({
            dataSource: new kendo.data.DataSource({
                transport: {
                    read: {
                        url: '/desktopmodules/riw/api/payconditions/getPayConds?portalId=' + portalID + '&pcType=4&pcStart=-1'
                    }
                },
                schema: {
                    model: {
                        id: 'PayCondId'
                        //fields: {
                        //    ProductEstimateOriginalPrice: {
                        //        editable: false, nullable: false
                        //    },
                        //    ProductEstimatePrice: {
                        //        editable: false, nullable: false
                        //    },
                        //    ProductQty: { type: 'number', validation: { required: { message: 'Campo obrgat&#243;rio' } } },
                        //    ProductDiscount: { type: "number", validation: { min: 0, required: { message: 'Campo obrgat&#243;rio' } } },
                        //    ExtendedAmount: {
                        //        editable: false, nullable: false
                        //    },
                        //    selected: {
                        //        editable: false, nullable: false
                        //    }
                        //}
                    }
                },
            }),
            toolbar: kendo.template($("#tmplCheckPreToolbar").html()),
            columns: [
                {
                    title: 'Apartir de',
                    field: 'PayCondStart',
                    format: '{0:n}'
                },
                {
                    title: 'Núm. Parcelas',
                    field: 'PayCondN'
                },
                {
                    title: 'Juros (a.m.)',
                    field: 'PayCondPerc',
                    template: '#= kendo.format("{0:p}",(PayCondPerc/100)) #'
                },
                {
                    title: 'Entrada %',
                    field: 'PayCondIn',
                    template: '#= kendo.format("{0:p}",(PayCondIn/100)) #'
                },
                {
                    title: 'Dias p/ Entrada',
                    field: 'PayInDays'
                },
                {
                    title: 'Intervalo',
                    field: 'PayCondInterval'
                },
                {
                    title: 'Grupo',
                    field: 'DiscountGroupId',
                    editor: groupDropDownEditor
                },
                {
                    command: [
                        {
                            name: "Update",
                            text: " Atualizar",
                            imageClass: "fa fa-check",
                            click: function (e) {
                                e.preventDefault();

                                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                if (dataItem) {

                                    var params = {
                                        PayCondId: dataItem.PayCondId,
                                        DiscountGroupId: dataItem.DiscountGroupId,
                                        ModifiedByUser: userID,
                                        ModifiedOnDate: moment().format(),
                                        PayCondDisc: dataItem.PayCondDisc,
                                        PayCondIn: dataItem.PayCondIn,
                                        PayInDays: dataIem.PayInDays,
                                        PayCondInterval: dataItem.PayCondInterval,
                                        PayCondN: dataItem.PayCondN,
                                        PayCondPerc: dataItem.PayCondPerc,
                                        PayCondTitle: 'American Express',
                                        PayCondType: 4,
                                        PayCondStart: dataItem.PayCondStart,
                                        PortalId: portalID,
                                        CreatedByUser: userID,
                                        CreatedOnDate: moment().format()
                                    };

                                    $.ajax({
                                        type: 'POST',
                                        url: '/desktopmodules/riw/api/payconditions/updatePayCond',
                                        data: params
                                    }).done(function (data) {
                                        if (data.Result.indexOf("success") !== -1) {
                                            $.pnotify({
                                                title: 'Sucesso!',
                                                text: 'Condição atualizada.',
                                                type: 'success',
                                                icon: 'fa fa-check fa-lg',
                                                addclass: "stack-bottomright",
                                                stack: my.stack_bottomright
                                            });
                                            $('#amexGrid').data('kendoGrid').dataSource.read();
                                        } else {
                                            $.pnotify({
                                                title: 'Erro!',
                                                text: data.Result,
                                                type: 'error',
                                                icon: 'fa fa-times-circle fa-lg',
                                                addclass: "stack-bottomright",
                                                stack: my.stack_bottomright
                                            });
                                        }
                                    }).fail(function (jqXHR, textStatus) {
                                        console.log(jqXHR.responseText);
                                    });
                                }
                            }
                        }
                    ],
                    width: 100
                },
                {
                    command: [
                        {
                            name: "Delete",
                            text: " Excluir",
                            imageClass: "fa fa-times",
                            click: function (e) {
                                e.preventDefault();

                                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                if (dataItem) {

                                    var $dialog = $('<div></div>')
                                        .html('<div class="confirmDialog">Tem Certeza?</div>')
                                        .dialog({
                                            autoOpen: false,
                                            modal: true,
                                            resizable: false,
                                            dialogClass: 'dnnFormPopup',
                                            open: function () {
                                                $(".ui-dialog-title").append('Aviso de Exclus&#227;o');
                                            },
                                            buttons: {
                                                'ok': {
                                                    text: 'Sim',
                                                    //priority: 'primary',
                                                    "class": 'dnnPrimaryAction',
                                                    click: function () {

                                                        $dialog.dialog('close');
                                                        $dialog.dialog('destroy');

                                                        $.ajax({
                                                            type: 'DELETE',
                                                            url: '/desktopmodules/riw/api/payconditions/removePayCond?payCondId=' + dataItem.PayCondId
                                                        }).done(function (data) {
                                                            if (data.Result.indexOf("success") !== -1) {
                                                                //$().toastmessage('showSuccessToast', 'Item excluido com sucesso!');
                                                                $.pnotify({
                                                                    title: 'Sucesso!',
                                                                    text: 'Condição excluida.',
                                                                    type: 'success',
                                                                    icon: 'fa fa-check fa-lg',
                                                                    addclass: "stack-bottomright",
                                                                    stack: my.stack_bottomright
                                                                });
                                                                $('#amexGrid').data('kendoGrid').dataSource.remove(dataItem);
                                                            } else {
                                                                $.pnotify({
                                                                    title: 'Erro!',
                                                                    text: data.Result,
                                                                    type: 'error',
                                                                    icon: 'fa fa-times-circle fa-lg',
                                                                    addclass: "stack-bottomright",
                                                                    stack: my.stack_bottomright
                                                                });
                                                                //$().toastmessage('showErrorToast', data.Msg);
                                                            }
                                                        }).fail(function (jqXHR, textStatus) {
                                                            console.log(jqXHR.responseText);
                                                        });
                                                    }
                                                },
                                                'cancel': {
                                                    html: 'N&#227;o',
                                                    //priority: 'secondary',
                                                    "class": 'dnnSecondaryAction',
                                                    click: function () {
                                                        $dialog.dialog('close');
                                                        $dialog.dialog('destroy');
                                                    }
                                                }
                                            }
                                        });

                                    $dialog.dialog('open');
                                }
                            }
                        }
                    ],
                    width: 90
                }
            ],
            editable: true
        });
    };

    loadDebitGrid = function () {
        $('#debitGrid').kendoGrid({
            dataSource: new kendo.data.DataSource({
                transport: {
                    read: {
                        url: '/desktopmodules/riw/api/payconditions/getPayConds?portalId=' + portalID + '&pcType=7&pcStart=-1'
                    }
                },
                schema: {
                    model: {
                        id: 'PayCondId'
                        //fields: {
                        //    ProductEstimateOriginalPrice: {
                        //        editable: false, nullable: false
                        //    },
                        //    ProductEstimatePrice: {
                        //        editable: false, nullable: false
                        //    },
                        //    ProductQty: { type: 'number', validation: { required: { message: 'Campo obrgat&#243;rio' } } },
                        //    ProductDiscount: { type: "number", validation: { min: 0, required: { message: 'Campo obrgat&#243;rio' } } },
                        //    ExtendedAmount: {
                        //        editable: false, nullable: false
                        //    },
                        //    selected: {
                        //        editable: false, nullable: false
                        //    }
                        //}
                    }
                },
            }),
            toolbar: kendo.template($("#tmplCheckPreToolbar").html()),
            columns: [
                {
                    title: 'Apartir de',
                    field: 'PayCondStart',
                    format: '{0:n}'
                },
                {
                    title: 'Núm. Parcelas',
                    field: 'PayCondN'
                },
                {
                    title: 'Juros (a.m.)',
                    field: 'PayCondPerc',
                    template: '#= kendo.format("{0:p}",(PayCondPerc/100)) #'
                },
                {
                    title: 'Entrada %',
                    field: 'PayCondIn',
                    template: '#= kendo.format("{0:p}",(PayCondIn/100)) #'
                },
                {
                    title: 'Dias p/ Entrada',
                    field: 'PayInDays'
                },
                {
                    title: 'Intervalo',
                    field: 'PayCondInterval'
                },
                {
                    title: 'Grupo',
                    field: 'DiscountGroupId',
                    editor: groupDropDownEditor
                },
                {
                    command: [
                        {
                            name: "Update",
                            text: " Atualizar",
                            imageClass: "fa fa-check",
                            click: function (e) {
                                e.preventDefault();

                                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                if (dataItem) {

                                    var params = {
                                        PayCondId: dataItem.PayCondId,
                                        DiscountGroupId: dataItem.DiscountGroupId,
                                        ModifiedByUser: userID,
                                        ModifiedOnDate: moment().format(),
                                        PayCondDisc: dataItem.PayCondDisc,
                                        PayCondIn: dataItem.PayCondIn,
                                        PayInDays: dataItem.PayInDays,
                                        PayCondInterval: dataItem.PayCondInterval,
                                        PayCondN: dataItem.PayCondN,
                                        PayCondPerc: dataItem.PayCondPerc,
                                        PayCondTitle: 'Cartão de Débito',
                                        PayCondType: 7,
                                        PayCondStart: dataItem.PayCondStart,
                                        PortalId: portalID,
                                        CreatedByUser: userID,
                                        CreatedOnDate: moment().format()
                                    };

                                    $.ajax({
                                        type: 'POST',
                                        url: '/desktopmodules/riw/api/payconditions/updatePayCond',
                                        data: params
                                    }).done(function (data) {
                                        if (data.Result.indexOf("success") !== -1) {
                                            $.pnotify({
                                                title: 'Sucesso!',
                                                text: 'Condição atualizada.',
                                                type: 'success',
                                                icon: 'fa fa-check fa-lg',
                                                addclass: "stack-bottomright",
                                                stack: my.stack_bottomright
                                            });
                                            $('#debitGrid').data('kendoGrid').dataSource.read();
                                        } else {
                                            $.pnotify({
                                                title: 'Erro!',
                                                text: data.Result,
                                                type: 'error',
                                                icon: 'fa fa-times-circle fa-lg',
                                                addclass: "stack-bottomright",
                                                stack: my.stack_bottomright
                                            });
                                        }
                                    }).fail(function (jqXHR, textStatus) {
                                        console.log(jqXHR.responseText);
                                    });
                                }
                            }
                        }
                    ],
                    width: 100
                },
                {
                    command: [
                        {
                            name: "Exclude",
                            text: " Excluir",
                            imageClass: "fa fa-times",
                            click: function (e) {
                                e.preventDefault();

                                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                if (dataItem) {

                                    var $dialog = $('<div></div>')
                                        .html('<div class="confirmDialog">Tem Certeza?</div>')
                                        .dialog({
                                            autoOpen: false,
                                            modal: true,
                                            resizable: false,
                                            dialogClass: 'dnnFormPopup',
                                            open: function () {
                                                $(".ui-dialog-title").append('Aviso de Exclus&#227;o');
                                            },
                                            buttons: {
                                                'ok': {
                                                    text: 'Sim',
                                                    //priority: 'primary',
                                                    "class": 'dnnPrimaryAction',
                                                    click: function () {

                                                        $dialog.dialog('close');
                                                        $dialog.dialog('destroy');

                                                        $.ajax({
                                                            type: 'DELETE',
                                                            url: '/desktopmodules/riw/api/payconditions/removePayCond?payCondId=' + dataItem.PayCondId
                                                        }).done(function (data) {
                                                            if (data.Result.indexOf("success") !== -1) {
                                                                //$().toastmessage('showSuccessToast', 'Item excluido com sucesso!');
                                                                $.pnotify({
                                                                    title: 'Sucesso!',
                                                                    text: 'Condição excluida.',
                                                                    type: 'success',
                                                                    icon: 'fa fa-check fa-lg',
                                                                    addclass: "stack-bottomright",
                                                                    stack: my.stack_bottomright
                                                                });
                                                                $('#debitGrid').data('kendoGrid').dataSource.remove(dataItem);
                                                            } else {
                                                                $.pnotify({
                                                                    title: 'Erro!',
                                                                    text: data.Result,
                                                                    type: 'error',
                                                                    icon: 'fa fa-times-circle fa-lg',
                                                                    addclass: "stack-bottomright",
                                                                    stack: my.stack_bottomright
                                                                });
                                                                //$().toastmessage('showErrorToast', data.Msg);
                                                            }
                                                        }).fail(function (jqXHR, textStatus) {
                                                            console.log(jqXHR.responseText);
                                                        });
                                                    }
                                                },
                                                'cancel': {
                                                    html: 'N&#227;o',
                                                    //priority: 'secondary',
                                                    "class": 'dnnSecondaryAction',
                                                    click: function () {
                                                        $dialog.dialog('close');
                                                        $dialog.dialog('destroy');
                                                    }
                                                }
                                            }
                                        });

                                    $dialog.dialog('open');
                                }
                            }
                        }
                    ],
                    width: 90
                }
            ],
            editable: true
        });
    };

    my.addPayCond = function () {        
        kendoWindow = $('#newPayCond').kendoWindow({
                title: 'Nova Condição de Pagamento',
                modal: true,
                width: '90%',
                height: '80%',
                close: function (e) {
                    $("html, body").css("overflow", "");
                    my.vm.payCondType(null);
                },
                open: function () {
                    $("html, body").css("overflow", "hidden");
                }
            });

        kendoWindow.data("kendoWindow").center().open();
    };

    $('#btnAddPayCond').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            DiscountGroupId: $('#kddlDiscountGroups').data('kendoDropDownList').value(),
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format(),
            PayCondIn: $('#payCondIn').data('kendoNumericTextBox').value(),
            PayInDays: $('#payInDays').data('kendoNumericTextBox').value(),
            PayCondInterval: $('#interval').data('kendoNumericTextBox').value(),
            PayCondN: $('#nPays').data('kendoNumericTextBox').value(),
            PayCondPerc: $('#interest').data('kendoNumericTextBox').value(),
            PayCondStart: $('#startAt').data('kendoNumericTextBox').value(),
            PayCondTitle: '',
            PortalId: portalID,
            CreatedByUser: userID,
            CreatedOnDate: moment().format()
        };

        switch (my.vm.payCondType()) {
            case 1:
                params.PayCondTitle = 'Boleto';
                my.vm.payCondType('bankPayGrid');
                params.PayCondType = 1;
                break;
            case 2:
                params.PayCondTitle = 'Cartão Visa';
                my.vm.payCondType('visaGrid');
                params.PayCondType = 2;
                break;
            case 3:
                params.PayCondTitle = 'Cartão Master Card';
                my.vm.payCondType('mcGrid');
                params.PayCondType = 3;
                break;
            case 4:
                params.PayCondTitle = 'Cartão American Express';
                my.vm.payCondType('amexGrid');
                params.PayCondType = 4;
                break;
            case 5:
                params.PayCondTitle = 'Cheque Pré';
                my.vm.payCondType('checkPreGrid');
                params.PayCondType = 5;
                break;
            case 6:
                params.PayCondTitle = 'Cartão Dinners Club';
                my.vm.payCondType('dinersGrid');
                params.PayCondType = 6;
                break;
            case 7:
                params.PayCondTitle = 'Cartão Débito';
                my.vm.payCondType('debitGrid');
                params.PayCondType = 7;
                break;
        }

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/payconditions/updatePayCond',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Condição atualizada.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                $('#startAt').data('kendoNumericTextBox').value(1);
                $('#nPays').data('kendoNumericTextBox').value(0);
                $('#interest').data('kendoNumericTextBox').value(0);
                $('#payCondIn').data('kendoNumericTextBox').value(0),
                $('#payInDays').data('kendoNumericTextBox').value(0);
                $('#interval').data('kendoNumericTextBox').value(1);
                $('#kddlDiscountGroups').data('kendoDropDownList').value(null);
                $('#' + my.vm.payCondType()).data('kendoGrid').dataSource.read();
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
            $('#newPayCond').data("kendoWindow").close();
        });
    });

    $('#btnUpdatePreDiscount').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            PayCondId: my.payCondId,
            PayCondTitle: $('#cashMessageTextArea').val(),
            PayCondType: 0,
            DiscountGroupId: 0,
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format(),
            PayCondIn: 0,
            PayInDays: 0,
            PayCondInterval: 0,
            PayCondN: 0,
            PayCondPerc: 0,
            PayCondStart: 0,
            PayCondDisc: $('#cashDiscount').data('kendoNumericTextBox').value(),
            PortalId: portalID,
            CreatedByUser: userID,
            CreatedOnDate: moment().format()
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/payconditions/updatePayCond',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Condição atualizada.',
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
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
    });

    $('#kddlDiscountGroups').kendoDropDownList({
        autoBind: false,
        optionLabel: ' Selecionar ',
        dataTextField: 'RoleName',
        dataValueField: 'RoleId',
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/store/getRolesByRoleGroup?portalId=' + portalID + '&roleGroupName=Descontos'
                }
            }
        }
    });

    //$('#kddlDiscountGroups').data('kendoDropDownList').text(' Selecionar ');

    $('.markdown-editor').css({ 'min-width': '90%', 'height': '80px', 'margin-bottom': '5px' }).attr({ 'cols': '30', 'rows': '2' });

    $('.markdown-editor').autogrow();
    $('.markdown-editor').css('overflow', 'hidden').autogrow();

    $('.togglePreview').click(function (e) {
        e.preventDefault();
        var $this = $(this);

        var ele = $($this).data('provider');

        var $dialog = $('<div></div>')
            .html(my.converter.makeHtml($('#' + ele).val().trim()))
            .dialog({
                autoOpen: false,
                open: function () {
                    $(".ui-dialog-title").append('Termo');
                },
                modal: true,
                resizable: true,
                dialogClass: 'dnnFormPopup',
                width: '50%',
                buttons: {
                    'ok': {
                        text: 'Ok',
                        //priority: 'primary',
                        "class": 'btn btn-primary',
                        click: function (e) {
                            $dialog.dialog('close');
                            $dialog.dialog('destroy');
                        }
                    }
                }
            });

        $dialog.dialog('open');
    });

    $('#btnReturn').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#newPayCond').data("kendoWindow").close();
    });

});

function groupDropDownEditor(container, options) {
    $('<input data-text-field="RoleName" data-value-field="RoleId" data-bind="value:' + options.RoleName + '"/>')
        .appendTo(container)
        .kendoDropDownList({
            autoBind: false,
            dataSource: {
                transport: {
                    read: {
                        url: '/desktopmodules/riw/api/store/getRolesByRoleGroup?portalId=' + portalID + '&roleGroupName=Descontos'
                    }
                }
            }
        });
}
