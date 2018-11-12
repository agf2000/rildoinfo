
$(function () {

    var date = new Date(),
        y = date.getFullYear(),
        m = date.getMonth();
    var firstDay = new Date(y, m, 1);
    var lastDay = new Date(y, m + 1, 0);

    my.viewModel();

    $('#moduleTitleSkinObject').html(moduleTitle);

    //var buttons = '<ul class="inline">';
    //buttons += '<li><a href="' + addressURL + '" class="btn btn-primary btn-medium" title="Endereço"><i class="fa fa-book fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + registrationURL + '" class="btn btn-primary btn-medium" title="Cadastro"><i class="fa fa-edit fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + payCondsURL + '" class="btn btn-primary btn-medium" title="Formas e Condições de Pagamento"><i class="fa fa-credit-card fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + syncURL + '" class="btn btn-primary btn-medium" title="Orçamento"><i class="fa fa-refresh fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + estimateURL + '" class="btn btn-primary btn-medium" title="Orçamento"><i class="fa fa-usd fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + smtpURL + '" class="btn btn-primary btn-medium" title="SMTP"><i class="fa fa-envelope-o fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + statusesManagerURL + '" class="btn btn-primary btn-medium" title="Status"><i class="fa fa-check-circle fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + websiteManagerURL + '" class="btn btn-primary btn-medium" title="Website"><i class="fa fa-bookmark fa-lg"></i></a></li>';
    ////buttons += '<li><a href="' + templatesManagerURL + '" class="btn btn-primary btn-medium" title="Templates"><i class="fa fa-puzzle-piece fa-lg"></i></a></li>';
    //buttons += '</ul>';
    //$('#buttons').html(buttons);

    $('#kdpStartingEstimate').kendoDatePicker({
        value: firstDay
    });

    $('#kdpEndingEstimate').kendoDatePicker({
        value: lastDay
    });

    $('#kdpStartingDebit').kendoDatePicker({
        value: firstDay
    });

    $('#kdpEndingDebit').kendoDatePicker({
        value: lastDay
    });

    $('#kdpStartingIncome').kendoDatePicker({
        value: firstDay
    });

    $('#kdpEndingIncome').kendoDatePicker({
        value: lastDay
    });

    $('#kdpStartingSale').kendoDatePicker({
        value: firstDay
    });

    $('#kdpEndingSale').kendoDatePicker({
        value: lastDay
    });

    var params = {
        sDate: moment($('#kdpStartingDebit').data('kendoDatePicker').value()).format(),
        eDate: moment($('#kdpEndingDebit').data('kendoDatePicker').value().setHours(23,59,00,0)).format(),
        filterDate: '1'
    };

    $.ajax({
        url: '/desktopmodules/riw/api/accounts/getAccountsBalance',
        data: params
    }).done(function (data) {
        if (data) {
            my.vm.debit4Seen(data.Debit4Seen);
            my.vm.debitActual(data.DebitActual);
            my.vm.credit4Seen(data.Credit4Seen + data.Sales4Seen + data.CreditActual);
            my.vm.creditActual(data.CreditActual + data.SalesActual);
            my.vm.totalProductSales(data.TotalProductSales);
            my.vm.totalServiceSales(data.TotalServiceSales);
            my.vm.totalProductsEstimates(data.TotalProductsEstimates);
            my.vm.totalServicesEstimates(data.TotalServicesEstimates);
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

    $('#btnDebit').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        //$this.button('loading');

        var params = {
            sDate: moment($('#kdpStartingDebit').data('kendoDatePicker').value()).format(),
            eDate: moment($('#kdpEndingDebit').data('kendoDatePicker').value()).format(),
            filterDate: '1'
        };

        $.ajax({
            url: '/desktopmodules/riw/api/accounts/getAccountsBalance',
            data: params
        }).done(function (data) {
            if (data) {
                my.vm.debit4Seen(data.Debit4Seen + data.DebitActual);
                my.vm.debitActual(data.DebitActual);
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

    $('#btnTotalSales').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        //$this.button('loading');

        var params = {
            sDate: moment($('#kdpStartingSale').data('kendoDatePicker').value()).format(),
            eDate: moment($('#kdpEndingSale').data('kendoDatePicker').value()).format(),
            filterDate: '1'
        };

        $.ajax({
            url: '/desktopmodules/riw/api/accounts/getAccountsBalance',
            data: params
        }).done(function (data) {
            if (data) {
                my.vm.totalProductSales(data.TotalProductSales);
                my.vm.totalServiceSales(data.TotalServiceSales);
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

    $('#btnTotalEstimates').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        //$this.button('loading');

        var params = {
            sDate: moment($('#kdpStartingEstimate').data('kendoDatePicker').value()).format(),
            eDate: moment($('#kdpEndingEstimate').data('kendoDatePicker').value()).format(),
            filterDate: '1'
        };

        $.ajax({
            url: '/desktopmodules/riw/api/accounts/getAccountsBalance',
            data: params
        }).done(function (data) {
            if (data) {
                my.vm.totalProductsEstimates(data.TotalProductsEstimates);
                my.vm.totalServicesEstimates(data.TotalServicesEstimates);
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

    $('#btnCredit').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        //$this.button('loading');

        var params = {
            sDate: moment($('#kdpStartingIncome').data('kendoDatePicker').value()).format(),
            eDate: moment($('#kdpEndingIncome').data('kendoDatePicker').value()).format(),
            filterDate: '1'
        };

        $.ajax({
            url: '/desktopmodules/riw/api/accounts/getAccountsBalance',
            data: params
        }).done(function (data) {
            if (data) {
                my.vm.credit4Seen(data.Credit4Seen + data.Sales4Seen + data.CreditActual);
                my.vm.creditActual(data.CreditActual + data.SalesActual);
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
});
