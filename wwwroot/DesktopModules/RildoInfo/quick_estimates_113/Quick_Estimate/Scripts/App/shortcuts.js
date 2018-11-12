/// <reference path="../_references.js" />

shortcut.add('ESC', function () {
    parent.$(".k-window-content").data("kendoWindow").close();
});

shortcut.add('Ctrl+1', function () {
    if ((my.vm.estimateId() > 0 && authorized === 2 && kendo.parseInt(selectedStatusId()) === 5 && estimatedByUser() === userID) || (my.vm.estimateId() > 0 && kendo.parseInt(selectedStatusId()) === 5 && authorized > 1)) { // && kendo.parseInt(my.vm.selectedStatusId()) === 5) {
        $('#btnPrintPdf').click();
    };
});

shortcut.add('Ctrl+2', function () {
    if ((my.vm.estimateId() > 0 && authorized === 2 && estimatedByUser() === userID) || (estimateId() > 0 && authorized === 2 && kendo.parseInt(selectedStatusId()) === 5) || (authorized === 3 && estimateId() > 0)) {
        $('#btnRegister').click();
    };
});

shortcut.add('Ctrl+3', function () {
    if (my.vm.email().length > 0) {
        $('#btnEmailEstimate').click();
    };
});

shortcut.add('Ctrl+4', function () {
    if (my.vm.estimateId() > 0) $('#btnPlans').click();
});

//shortcut.add('Ctrl+5', function () {
//    $('#btnDiscount').click();
//});

shortcut.add('Ctrl+6', function () {
    history.pushState("", document.title, window.location.pathname);
    my.estimateId = null;
    my.personId = null;
    my.lineTotal = 0;
    my.lineTotalDiscount = 0;
    my.vm.subTotal(0);
    my.vm.grandTotal(0);
    $('#divImage').off('click');
    $('#divImage').html(function () { if (qeImage !== '') $('#divImage').html('<img alt="" src="/portals/' + portalID + '/images/' + qeImage + '?maxwidth=390&maxheight=250" />'); });
    my.vm.estimateId(0);
    my.vm.estimatedByUser(0);
    my.vm.personId(0);
    my.vm.clientUserId(0);
    my.vm.displayName('');
    my.vm.telephone('');
    my.vm.email('');
    my.vm.street('');
    my.vm.unit('');
    my.vm.complement('');
    my.vm.district('');
    my.vm.city('');
    my.vm.region('');
    //my.vm.emailTo('');
    my.vm.productName('Rildo Inform&#225;tica Ltda (31) 3037-0551');
    my.vm.productCode('0000000000000');
    my.vm.itemQty('');
    my.vm.unitValue(0);
    my.vm.displayTotal('TOTAL = 0,00');
    my.vm.prodTotal('0 x 0,00 = 0,00');
    my.vm.createdOnDate(new Date());
    my.vm.hasLogin(false);
    my.vm.selectedPayForm('');
    my.vm.conditionPayIn(0);
    my.vm.conditionNumberPayments(0);
    my.vm.conditionInterest(0);
    my.vm.conditionInterval(0);
    my.vm.salesRepName('');
    my.vm.salesRepEmail('');
    my.vm.salesRepPhone('');
    my.vm.bankIn(0, 00);
    my.vm.checkIn(0, 00);
    my.vm.cardIn(0, 00);
    my.vm.cashIn(0, 00);
    my.vm.items.removeAll();
    my.loadEstimate();
    my.loadClient();
    setTimeout(function () {
        $('#getEstimateTextBox').focus();
    }, 400);
});

shortcut.add('Ctrl+7', function () {
    var ddlPF = $('#ddlPayForms');
    if (userID !== my.vm.estimatedByUser()) {
        if (authorized !== 3) {
            ddlPF.data("kendoDropDownList").enable(false);
        } else {
            var ddlPF = $('#ddlPayForms');
            ddlPF.data("kendoDropDownList").open();
            ddlPF.data("kendoDropDownList").focus();
        };
    } else {
        var ddlPF = $('#ddlPayForms');
        ddlPF.data("kendoDropDownList").open();
        setTimeout(function () {
            ddlPF.data("kendoDropDownList").focus();
        }, 100);
    };
});

shortcut.add('Ctrl+8', function () {
    if (!$('#clientArea').is(':hidden')) {
        $('#clientArea').slideUp();
        $('#clientToggle').removeClass('k-i-arrow-n').addClass('k-i-arrow-s');
    } else {
        $('#clientArea').slideDown();
        $('#clientToggle').removeClass('k-i-arrow-s').addClass('k-i-arrow-n');
        $('#clientsSearchBox').focus();
    }
    //if (!$('#menuTop').is(':hidden')) {
    //    $('#menuTop').kendoAnimate({ effects: "slideOutUp:up fade:out", hide: true });
    //    $('#footer').kendoAnimate({ effects: "slide:up fade:out", hide: true });
    //    $('#Breadcrumb').kendoAnimate({ effects: "slide:up fade:out", hide: true });
    //} else {
    //    $('#menuTop').kendoAnimate({ effects: "slide:up fade:in" });
    //    $('#footer').kendoAnimate({ effects: "slide:up fade:in" });
    //    $('#Breadcrumb').kendoAnimate({ effects: "slide:up fade:in" });
    //}
});

shortcut.add('Ctrl+9', function () {
    $('#btnRestart').click();
});

shortcut.add('Ctrl+0', function () {
    $('#btnLogin').click();
});

shortcut.add('Alt+2', function () {
    
});

shortcut.add('Alt+C', function () {
    $('#clientArea').slideToggle();
});

shortcut.add('Alt+B', function () {
    $("#clientSearch").select2("open");
});

shortcut.add('Ctrl+A', function () {
    if (!$('#editPayFormCond').is(':hidden')) {
        //if (userID !== my.vm.estimatedByUser()) {
        if (authorized !== 2) {
            //    $('#ddlStatuses').data("kendoDropDownList").enable(false);
            //} else {
            var ddlSt = $('#ddlStatuses');
            ddlSt.focus();
            ddlSt.data("kendoDropDownList").open();
        }
        //} else {
        //    var ddlSt = $('#ddlStatuses');
        //    ddlSt.focus();
        //    ddlSt.data("kendoDropDownList").open();
        //}
    }
});

shortcut.add('Ctrl+B', function () {
    $('#btnAddClient').click();
});

shortcut.add('Ctrl+D', function () {
    my.removeItem();
});

shortcut.add('Ctrl+E', function () {
    $('#payIn').siblings("input:visible").focus().select();
    setTimeout(function () {
        $('#payIn').select();
    });
});

shortcut.add('Ctrl+F', function () {
    if (kendo.parseInt(cashBack()) <= 0) $('#btnFinalize').click();
});

shortcut.add('Ctrl+I', function () {
    //if ((my.vm.estimateId() > 0 && authorized === 2 && kendo.parseInt(selectedStatusId()) === 5 && estimatedByUser() === userID) || (my.vm.estimateId() > 0 && kendo.parseInt(selectedStatusId()) === 5 && authorized > 1)) { // && kendo.parseInt(my.vm.selectedStatusId()) === 5) {
    $('#btnPrintReceipt').click();
    //}
});

shortcut.add('Ctrl+J', function () {
    $('#payCondGrid').data("kendoGrid").table.focus();
});

shortcut.add('Ctrl+K', function () {
    $('#btnLeave').click();
});

shortcut.add('Ctrl+L', function () {
    $('#couponGrid').data("kendoGrid").table.focus();
});

shortcut.add('Ctrl+M', function () {
    if (!$('#divEmail').is(':hidden'))
        $('#btnSendEmail').click();
});

shortcut.add('Ctrl+P', function () {
    $('#itemsSearchBox').focus().select();
});

shortcut.add('Ctrl+Q', function () {
    $('#itemQtyBox').focus().select();
});

shortcut.add('Ctrl+R', function () {
    $('#btnCancelPayFormCond').click();
});

shortcut.add('Ctrl+S', function () {
    $('#btnSavePayFormCond').click();
});

shortcut.add('Ctrl+U', function () {
    $('#btnDoLogin').click();
});

shortcut.add('Ctrl+Z', function () {
    $('#btnEditClient').click();
});

shortcut.add('Shift+D', function () {
    my.removeItem();
});


//$('#quickEstimate input').hotnav({
//    trigger: function (e, options) {
//        //e.focus();
//        jQuery("." + options.classname).hide();
//    },
//    keysource: function (e) {
//        return e.attr("name");
//    },
//    insertkey: function (e, key, options) {
//        e.wrap("<span style='position:relative;'>").after("<span class='" + options.classname + "'>" + key.toUpperCase() + "</span>");
//    }
//});

//$('#quickEstimate button').hotnav({
//    trigger: function (e, options) {
//        //e.focus();
//        jQuery("." + options.classname).hide();
//    },
//    keysource: function (e) {
//        return e.attr("name");
//    },
//    insertkey: function (e, key, options) {
//        e.wrap("<span style='position:relative;'>").after("<span class='" + options.classname + "'>" + key.toUpperCase() + "</span>");
//    }
//});