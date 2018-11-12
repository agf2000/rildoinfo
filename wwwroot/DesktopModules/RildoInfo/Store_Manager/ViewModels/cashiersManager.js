
$(function () {
    
    my.viewModel();

    $('#moduleTitleSkinObject').html(moduleTitle);

    var buttons = '<ul class="inline">';
    buttons += '<li><a href="' + invoicesManagerURL + '" class="btn btn-primary btn-medium" title="Lançamentos"><i class="fa fa-money fa-lg"></i></a></li>';
    buttons += '<li><a href="' + accountsManagerURL + '" class="btn btn-primary btn-medium" title="Fluxo"><i class="fa fa-book fa-lg"></i></a></li>';
    buttons += '<li><a href="' + agendaURL + '" class="btn btn-primary btn-medium" title="Agenda"><i class="fa fa-calendar fa-lg"></i></a></li>';
    buttons += '</ul>';
    $('#buttons').html(buttons);

    $('.icon-exclamation-sign').popover({
        delay: {
            show: 500,
            hide: 100
        },
        placement: 'top',
        trigger: 'hover'
    });

    $('#kdpStartDate').kendoDatePicker();
    $('#kdpEndDate').kendoDatePicker();
    $('#kdpCloseDate').kendoDatePicker();

    // create kendo dataSource transport to get products
    my.cashiersTransport = {
        read: {
            url: '/desktopmodules/riw/api/cashiers/GetCashiers'
        },
        parameterMap: function (data, type) {
            return {
                portalId: portalID,
                sDate: $('#kdpStartDate').val().length > 0 ? moment(new Date($('#kdpStartDate').data('kendoDatePicker').value())).format() : '',
                eDate: $('#kdpEndDate').val().length > 0 ? moment(new Date($('#kdpEndDate').data('kendoDatePicker').value())).format() : '',
                pageNumber: data.page,
                pageSize: data.pageSize,
                orderBy: data.sort[0].field,
                orderDir: data.sort[0].dir
            };
        }
    };

    // create kendo dataSource for getting products transport
    my.cashiers = new kendo.data.DataSource({
        transport: my.cashiersTransport,
        pageSize: 10,
        serverPaging: true,
        serverSorting: true,
        serverFiltering: true,
        sort: {
            field: "CreatedOnDate",
            dir: "DESC"
        },
        schema: {
            model: {
                id: 'CashierId',
                fields: {
                    CreatedOnDate: {
                        type: "date", format: "{0:dd/MM/yyyy}"
                    },
                    ModifiedOnDate: {
                        type: "date", format: "{0:dd/MM/yyyy}"
                    }
                }
            },
            data: 'data',
            total: 'total'
        }
    });

    $("#cashiersGrid").kendoGrid({
        dataSource: my.cashiers,
        //toolbar: kendo.template($("#tmplToolbar").html()),
        columns: [
            { field: "CreatedOnDate", title: "Data", type: "date", format: "{0:d}" },
            { field: "TotalCash", title: "Dinheiro", format: '{0:n}' },
            { field: "TotalCard", title: "Cartão", format: '{0:n}' },
            { field: "TotalDebit", title: "Débito", format: '{0:n}' },
            { field: "TotalCheck", title: "Cheque", format: '{0:n}' },
            { field: "TotalBank", title: "Boleto", format: '{0:n}' },
            { title: "Total", template: '#: kendo.toString(TotalCash + TotalCard + TotalDebit + TotalCheck + TotalBank, "n") #' }
        ],
        sortable: true,
        reorderable: true,
        resizable: true,
        scrollable: true,
        pageable: {
            pageSizes: [20, 80, 1000],
            refresh: true,
            numeric: false,
            input: true,
            messages: {
                display: "{0} - {1} de {2} itens",
                empty: "Sem Registro.",
                page: "Página",
                of: "de {0}",
                itemsPerPage: "itens por página",
                first: "Ir para primeira página",
                previous: "Ir para página anterior",
                next: "Ir para próxima página",
                last: "Ir para última página",
                refresh: "Recarregar"
            }
        },
        dataBound: function () {
            //$('#btnEditSelected').attr({ 'disabled': true });
            //if (authorized < 3) {
            //    $('#btnAddNewInvoice').hide();
            //    $('#btnEditSelected').hide();
            //}
        }
    });

    $("#btnClose").click(function (e) {
        e.preventDefault();
        if (e.clientX === 0) {
            return false;
        }

        my.closeCashier(moment($('#kdpCloseDate').data('kendoDatePicker').value()).format(), userID);
    });

    my.closeCashier = function(theDate, userId) {
        $.ajax({
            type: "POST",
            url: '/desktopmodules/riw/api/cashiers/ProcessCashier?cashierDate=' + theDate + '&userId=' + userId,
        }).done(function (response) {
            if (response != null) {
                if (response.Result.indexOf('success') !== -1) {
                    my.cashiers.read();
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Caixa atualizado.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                }
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            // optional...            
        });
    };
    
    $('#btnSearch').click(function (e) {
        e.preventDefault();
        if (e.clientX === 0) {
            return false;
        }

        my.cashiers.read();
    });

    $('#btnRemoveFilter').click(function (e) {
        var btn = this;
        e.preventDefault();
        if (e.clientX === 0) {
            return false;
        }

        $('#kdpStartDate').data('kendoDatePicker').value(null);
        $('#kdpEndDate').data('kendoDatePicker').value(null);

        my.cashiers.read();
    });

});