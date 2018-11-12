$(function () {

    my.viewModel();

    $('#moduleTitleSkinObject').html(moduleTitle);

    my.reportId = 0; // my.getQuerystring('rId', my.getParameterByName('rId'));
    my.personId = 0; // my.getQuerystring('cId', my.getParameterByName('cId'));
    my.statusIds = ''; // my.getQuerystring('sId', my.getParameterByName('sId'));
    my.salesRepId = 0; // my.getQuerystring('sRepId', my.getParameterByName('sRepId'));
    my.productId = 0; // my.getQuerystring('pId', my.getParameterByName('pId'));
    my.date1 = 0; // my.getQuerystring('date1', my.getParameterByName('date1'));
    my.date2 = 0; // my.getQuerystring('date2', my.getParameterByName('date2'));

    var date = new Date(), y = date.getFullYear(), m = date.getMonth();
    var firstDay = new Date(y, m, 1);
    var lastDay = new Date(y, m + 1, 0);

    //$('#kddlReports').kendoDropDownList({583
    //    autoBind: false,
    //    optionLabel: " Selecionar ",
    //    //headerTemplate: '<div class="dropdown-header"><span class="k-widget k-header">Relatórios</span></div>',
    //    dataTextField: "ReportName",
    //    datValueField: "ReportId",
    //    //valueTemplate: '<span>#: ReportName #</span>',
    //    //template: '<div class="k-state-default" title=\"#: ReportDescription #"\>#: ReportName #</div>',
    //    dataSource: {
    //        transport: {
    //            read: {
    //                url: '/desktopmodules/riw/api/reports/GetReports?portalId=' + portalID
    //            }
    //        }
    //    },
    //    select: function (e) {
    //        var dataItem = this.dataItem(e.item);
    //        my.reportId = dataItem.ReportId;
    //    }
    //});

    $("#chkBoxCredit").prop("indeterminate", true).prop('readonly', true);

    $("#chkBoxDone").prop("indeterminate", true).prop('readonly', true);

    var menu = $('#kmReports').kendoMenu({
        openOnClick: true,
        select: function (e) {            
            if (e.item.id.toString().indexOf('menu_') !== -1) {
                my.reportId = e.item.id.toString().replace('menu_', '');
                $('#reportName').show().html('Um momento...');
                getReport(my.reportId, portalID);
                $(e.item).addClass("k-state-selected");
            }
        }
    }).data('kendoMenu');

    getReport = function (reportId, portalId) {
        $('#tblReport').show();
        $('#reportName').html('Um momento...');
        $.getJSON('/desktopmodules/riw/api/reports/GetReport?reportId=' + reportId + '&portalId=' + portalId, function (data) {
            if (data) {
                $('#reportName').html('<span style="color: #0866C6;">' + data.ReportName + '</span>');
                $('#reportDescription').html(data.ReportDescription);
                //my.vm.sql(data.ReportSql);
                my.vm.sqlDetail(data.ReportSqlDetail);
            }
        });
    };

    $('#btnFilter').kendoButton();
    $('#btnPrint').kendoButton();
    $('#btnExcel').kendoButton();

    $('#kddlSalesPerson').kendoDropDownList({
        autoBind: false,
        optionLabel: "Todos Vendedores",
        dataTextField: 'DisplayName',
        dataValueField: 'UserId',
        //select: my.clients.read(),
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/people/GetUsersByRoleGroup?portalId=' + portalID + '&roleGroupName=Departamentos&roleName=Vendedores'
                }
            }
        },
        select: function (e) {
            var dataItem = this.dataItem(e.item);
            my.salesRepId = dataItem.UserId;
        }
    });

    $('#kmsStatuses').kendoMultiSelect({
        autoBind: false,
        autoClose: false,
        placeholder: "Selecionar Status",
        dataTextField: "StatusTitle",
        dataValueField: "StatusId",
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/statuses/GetStatuses?portalId=' + portalID + '&isDeleted=False'
                }
            }
        }
        //select: function (e) {
        //    var dataItem = this.dataItem(e.item);
        //    my.statusId = dataItem.StatusId;
        //}
    });

    $('#clientSearchBox').select2({
        placeholder: "Busque por clientes...",
        //allowClear: true,
        minimumInputLength: 3,
        id: function (data) {
            return {
                PersonId: data.PersonId,
                DisplayName: data.DisplayName,
                Email: data.Email,
                Telephone: data.Telephone,
                Cell: data.Cell,
                Fax: data.Fax,
                Zero800s: data.Zero800s,
                Street: data.Street,
                Complement: data.Complement,
                District: data.District,
                City: data.City,
                Region: data.Region,
                Country: data.Country,
                PostalCode: data.PostalCode,
                Unit: data.Unit,
                SalesRepName: data.SalesRepName,
                SalesRepEmail: data.SalesRepEmail,
                SalesRepPhone: data.SalesRepPhone,
                Blocked: data.Blocked,
                ReasonBlocked: data.ReasonBlocked
            };
        },
        ajax: {
            url: "/desktopmodules/riw/api/people/getpeople",
            quietMillis: 100,
            data: function (term, page) { // page is the one-based page number tracked by Select2
                return {
                    portalId: portalID,
                    registerType: clientRoleId,
                    isDeleted: false,
                    searchField: $('#kddlProviderFields').data('kendoDropDownList').value(),
                    sTerm: term,
                    pageIndex: page,
                    pageSize: 10,
                    orderBy: 'DisplayName',
                    orderDesc: 'ASC'
                };
            },
            results: function (data, page) {
                var more = (page * 10) < data.total; // whether or not there are more results available

                // notice we return the value of more so Select2 knows if more results can be loaded
                return { results: data.data, more: more };
            }
        },
        formatResult: clientFormatResult, // omitted for brevity, see the source of this page
        formatSelection: clientFormatSelection, // omitted for brevity, see the source of this page
        dropdownCssClass: "bigdrop", // apply css that makes the dropdown taller
        escapeMarkup: function (m) { return m; }, // we do not want to escape markup since we are displaying html in results
        initSelection: function (element, callback) {
            var text = element.val();
            var data = { DisplayName: text };
            callback(data);
        }
    });

    $('#clientSearchBox').on("select2-selecting", function (e) {
        my.personId = e.val.PersonId;
    });

    $('#providerSearchBox').select2({
        placeholder: "Busque por fornecedores...",
        //allowClear: true,
        minimumInputLength: 3,
        id: function (data) {
            return {
                PersonId: data.PersonId,
                DisplayName: data.DisplayName,
                Email: data.Email,
                Telephone: data.Telephone,
                Cell: data.Cell,
                Fax: data.Fax,
                Zero800s: data.Zero800s,
                Street: data.Street,
                Complement: data.Complement,
                District: data.District,
                City: data.City,
                Region: data.Region,
                Country: data.Country,
                PostalCode: data.PostalCode,
                Unit: data.Unit,
                SalesRepName: data.SalesRepName,
                SalesRepEmail: data.SalesRepEmail,
                SalesRepPhone: data.SalesRepPhone,
                Blocked: data.Blocked,
                ReasonBlocked: data.ReasonBlocked
            };
        },
        ajax: {
            url: "/desktopmodules/riw/api/people/getpeople",
            quietMillis: 100,
            data: function (term, page) { // page is the one-based page number tracked by Select2
                return {
                    portalId: portalID,
                    registerType: providerRoleId,
                    isDeleted: false,
                    searchField: $('#kddlProviderFields').data('kendoDropDownList').value(),
                    sTerm: term,
                    pageIndex: page,
                    pageSize: 10,
                    orderBy: 'DisplayName',
                    orderDesc: 'ASC'
                };
            },
            results: function (data, page) {
                var more = (page * 10) < data.total; // whether or not there are more results available

                // notice we return the value of more so Select2 knows if more results can be loaded
                return { results: data.data, more: more };
            }
        },
        formatResult: clientFormatResult, // omitted for brevity, see the source of this page
        formatSelection: clientFormatSelection, // omitted for brevity, see the source of this page
        dropdownCssClass: "bigdrop", // apply css that makes the dropdown taller
        escapeMarkup: function (m) { return m; }, // we do not want to escape markup since we are displaying html in results
        initSelection: function (element, callback) {
            var text = element.val();
            var data = { DisplayName: text };
            callback(data);
        }
    });

    $('#providerSearchBox').on("select2-selecting", function (e) {
        my.personId = e.val.PersonId;
    });

    $('#productSearchBox').select2({
        placeholder: "Busque por produtos...",
        //allowClear: true,
        minimumInputLength: 3,
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
                    portalId: portalID,
                    searchField: $('#kddlProductFields').data('kendoDropDownList').value(), // fieldName.charAt(0) === '*' ? 'ProductRef' : fieldName.charAt(0) === '#' ? 'BarCode' : 'ProductName',
                    searchString: fieldName.charAt(0) === '*' || fieldName.charAt(0) === '#' ? fieldName.slice(1) : fieldName,
                    pageIndex: page,
                    pageSize: 10,
                    orderBy: $('#kddlProductFields').data('kendoDropDownList').value(),
                    orderDesc: 'ASC'
                };
            },
            results: function (data, page) {
                var more = (page * 10) < data.total; // whether or not there are more results available

                // notice we return the value of more so Select2 knows if more results can be loaded
                return { results: data.data, more: more };
            }
        },
        formatResult: productFormatResult, // omitted for brevity, see the source of this page
        formatSelection: productFormatSelection, // omitted for brevity, see the source of this page
        dropdownCssClass: "bigdrop", // apply css that makes the dropdown taller
        escapeMarkup: function (m) { return m; } // we do not want to escape markup since we are displaying html in results
        //initSelection: function (element, callback) {
        //    var text = element.val();
        //    var data = { DisplayName: text };
        //    callback(data);
        //}
    });

    $('#productSearchBox').on("select2-selecting", function (e) {
        my.productId = e.val.ProductId;
    });

    $('#kdpStartDate').kendoDatePicker({
        value: firstDay,
        change: function () {
            var value = this.value();
            $('#spanStartDate').text(moment(value).format('ll'));
            my.date1 = $('#spanStartDate').text();
        }
    });
    $('#kdpStartDate').text(moment($('#kdpStartDate').data('kendoDatePicker').value()).format('ll'));

    if (my.date1 === 0) {
        my.date1 = moment(new Date($('#kdpStartDate').data('kendoDatePicker').value())).format('YYYY/MM/DD');
    };

    $('#kdpEndDate').kendoDatePicker({
        value: lastDay,
        change: function () {
            var value = this.value();
            $('#spanEndDate').text(moment(value).format('ll'));
            my.date2 = $('#spanStartDate').text();
        }
    });
    $('#kdpEndDate').text(moment($('#kdpEndDate').data('kendoDatePicker').value()).format('ll'));

    if (my.date2 === 0) {
        my.date2 = moment(new Date($('#kdpEndDate').data('kendoDatePicker').value())).format('YYYY/MM/DD');
    }

    my.reportsTransport = {
        read: {
            url: '/desktopmodules/riw/api/reports/RunReport'
        },
        parameterMap: function (data, type) {
            return {
                portalId: portalID,
                reportId: my.reportId,
                personId: my.personId,
                productId: my.productId,
                statusIds: $('#kmsStatuses').data('kendoMultiSelect').value().toString(), // my.statusId,
                salesRepId: my.salesRepId,
                date1: moment(new Date($('#kdpStartDate').data('kendoDatePicker').value())).format(), // moment(new Date(my.date1)).format() || 
                date2: moment(new Date($('#kdpEndDate').data('kendoDatePicker').value())).format(), // moment(new Date(my.date2)).format() || 
                credit: $('#chkBoxCredit').prop('readonly') ? null : $('#chkBoxCredit').prop('checked'),
                done: $('#chkBoxDone').prop('readonly') ? null : $('#chkBoxDone').prop('checked'),
                datefield: $('#kddlDates').data('kendoDropDownList').value()
            };
        }
    };

    my.reportsDataSource = new kendo.data.DataSource({
        transport: my.reportsTransport,
        //sort: { field: "Id", dir: "DESC" },
        pageSize: 10,
        schema: {
            model: {
                Codigo: 'Id',
                CreatedOnDate: {
                    type: "date", format: "{0:dd/MM/yyyy}"
                },
                ModifiedOnDate: {
                    type: "date", format: "{0:dd/MM/yyyy}"
                },
                DueDate: {
                    type: "date", format: "{0:dd/MM/yyyy}"
                },
                TransDate: {
                    type: "date", format: "{0:dd/MM/yyyy}"
                }
            },
            data: 'data',
            total: 'total'
        },
        aggregate: [
            {
                field: "Id", aggregate: "count"
            },
            {
                field: "Codigo", aggregate: "count"
            },
            {
                field: "ProductId", aggregate: "count"
            },
            {
                field: "QtyStockSet", aggregate: "sum"
            },
            {
                field: "QtyStockOther", aggregate: "sum"
            },
            {
                field: "Comission", aggregate: "sum"
            },
            {
                field: "Cost", aggregate: "sum"
            },
            {
                field: "TotalAmount", aggregate: "sum"
            },
            {
                field: "Debit", aggregate: "sum"
            },
            {
                field: "Credit", aggregate: "sum"
            },
            {
                field: "Profit", aggregate: "sum"
            },
            {
                field: "AmountValue", aggregate: "sum"
            },
            {
                field: "EstimateInfo", aggregate: "sum"
            }
        ]
    });

    runGrid = function () {

        if ($('#reportGrid').data('kendoGrid') !== undefined) {
            $('#reportGrid').data('kendoGrid').destroy();
        }

        $('#reportGrid').kendoGrid({
            autoBind: false,
            dataSource: my.reportsDataSource,
            selectable: "row",
            sortable: true,
            reorderable: true,
            resizable: true,
            scrollable: false,
            pageable: false,
            groupable: true,
            excel: {
                allPages: true
            },
            dataBound: function (e) {
                var grid = this;

                if (this.dataSource.view().length > 0) {
                    $.each(grid.dataSource.data(), function (i, estimate) {

                        var payType = '';

                        if (estimate.CashAmount) {
                            if (payType !== '') {
                                payType += ', DIN'
                            } else {
                                payType += 'DIN'
                            }
                        }

                        if (estimate.CheckAmount) {
                            if (payType !== '') {
                                payType += ', CHQ'
                            } else {
                                payType += 'CHQ'
                            }
                        }

                        if (estimate.CardAmount) {
                            if (payType !== '') {
                                payType += ', CAR'
                            } else {
                                payType += 'CAR'
                            }
                        }

                        if (estimate.BankAmount) {
                            if (payType !== '') {
                                payType += ', BOL'
                            } else {
                                payType += 'BOL'
                            }
                        }

                        if (estimate.DebitAmount) {
                            if (payType !== '') {
                                payType += ', DEB'
                            } else {
                                payType += 'DEB'
                            }
                        }

                        //if ((estimate.BankAmount + estimate.CardAmount + (estimate.CheckAmount || 0) + estimate.CashAmount) == 0) {
                        //    if (payType !== '') {
                        //        payType += ', DEB'
                        //    } else {
                        //        payType += 'DEB'
                        //    }
                        //}

                        estimate.set('PayTypes', payType)
                    })
                }
            }
        });
    };

    SetColumnsConfig = function () {
        var columnsConfig = [];

        switch (kendo.parseInt(my.reportId)) {
            case 11:
                columnsConfig.push(
                    {
                        field: 'ProductId', title: 'Código', width: 100, aggregates: ["count"], footerTemplate: "Total: #= kendo.toString(count, 'n0') #"
                    },
                    {
                        field: 'ProductName', title: 'Produto'
                    },
                    {
                        field: 'ProductRef', title: 'Referencia'
                    },
                    {
                        field: 'UnitTypeAbbv', title: 'UNIDADE'
                    },
                    {
                        field: 'UnitValue', title: 'Preco'
                    },
                    {
                        field: 'Finan_Cfop', title: 'CFOP'
                    },
                    {
                        field: 'Finan_Cofins', title: 'COFINS'
                    },
                    {
                        field: 'Finan_Cst', title: 'CST'
                    },
                    {
                        field: 'Finan_Cost', title: 'Custo'
                    },
                    {
                        field: 'Finan_Icms', title: 'ICMS'
                    },
                    {
                        field: 'Finan_Ipi', title: 'IPI'
                    },
                    {
                        field: 'Finan_Ncm', title: 'NCM'
                    },
                    {
                        field: 'Finan_Cest', title: 'Cest'
                    },
                    {
                        field: 'Finan_Pis', title: 'PIS'
                    }
                );
                break;
            case 10:
                columnsConfig.push(
                    {
                        field: 'Id', title: 'Nota Fis.', width: 100, aggregates: ["count"], footerTemplate: "#= kendo.toString(count, 'n0') # itens"
                    },
                    {
                        field: 'Client', title: 'Cliente'
                    },
                    {
                        field: 'Provider', title: 'Fornecedor'
                    },
                    {
                        field: 'Comment', title: 'Descrição'
                    },
                    {
                        field: 'Origin', title: 'Motivo'
                    },
                    {
                        field: 'CreatedOnDate', title: 'Data', width: 70, type: 'date', template: "#= kendo.toString(kendo.parseDate(CreatedOnDate, 'yyyy-MM-dd'),'dd/MM/yyyy') #", groupHeaderTemplate: "#= kendo.toString(kendo.parseDate(value, 'yyyy-MM-dd'),'dd/MM/yyyy') #"
                    },
                    {
                        field: 'Debit', title: 'Débito', width: 90, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                    },
                    {
                        field: 'Credit', title: 'Crédito', width: 90, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                    },
                    {
                        field: 'Done', title: 'Quitado', width: 80, template: '#= Done ? "Quitado" : "Em Aberto" #'
                    },
                    {
                        field: 'DueDate', title: 'Vencimento', width: 70, type: 'date', template: "#= kendo.toString(kendo.parseDate(DueDate, 'yyyy-MM-dd'),'dd/MM/yyyy') #", groupHeaderTemplate: "#= kendo.toString(kendo.parseDate(value, 'yyyy-MM-dd'),'dd/MM/yyyy') #"
                    }
                );
                break;
            case 9:
                columnsConfig.push(
                    {
                        field: 'Id', title: 'Código', width: 100, aggregates: ["count"], footerTemplate: "Total: #= kendo.toString(count, 'n0') #"
                    },
                    {
                        field: 'DisplayName', title: 'Cliente'
                    },
                    {
                        field: 'ModifiedOnDate', title: 'Data', width: 70, type: 'date', template: "#= kendo.toString(kendo.parseDate(ModifiedOnDate, 'yyyy-MM-dd'),'dd/MM/yyyy') #", groupHeaderTemplate: "#= kendo.toString(kendo.parseDate(value, 'yyyy-MM-dd'),'dd/MM/yyyy') #"
                    },
                    {
                        field: 'SalesRepName', title: 'Vendedor', width: 120
                    },
                    {
                        field: 'StatusTitle', title: 'Status', width: 80
                    },
                    {
                        field: 'TotalAmount', title: 'Total', width: 90, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                    },
                    {
                        field: 'PayTypes', title: 'Finalizador', width: 80
                    },
                    {
                        field: 'PayCond', title: 'Cond. Pag.', width: 80, template: '#= PayCond === "cash" ? "À Vista" : PayCond #'
                    },
                    {
                        field: 'Comission', title: 'Comissão', width: 80, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                    },
                    {
                        field: 'Cost', title: 'Custo', width: 80, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                    },
                    {
                        field: 'Profit', title: 'Lucro', width: 120, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                    }
                );
                break;
            case 2:
                columnsConfig.push(
                    {
                        field: 'Id', title: 'Código', width: 100, aggregates: ["count"], footerTemplate: "Total: #= kendo.toString(count, 'n0') #"
                    },
                    {
                        field: 'DisplayName', title: 'Cliente'
                    },
                    {
                        field: 'CreatedOnDate', title: 'Criado Em', type: "date", width: 70, template: "#= kendo.toString(kendo.parseDate(CreatedOnDate, 'yyyy-MM-dd'),'dd/MM/yyyy') #", groupHeaderTemplate: $('#kddlDates').data('kendoDropDownList').text() + " #= kendo.toString(kendo.parseDate(value, 'yyyy-MM-dd'),'dd/MM/yyyy') #"
                    },
                    {
                        field: 'SalesRepName', title: 'Vendedor', width: 120
                    },
                    {
                        field: 'StatusTitle', title: 'Status', width: 80
                    },
                    {
                        field: 'Telephone', title: 'Telefone', width: 80
                    },
                    {
                        field: 'Email', title: 'Email'
                    }
                );
                break;
            case 4:
                columnsConfig.push(
                    {
                        field: 'Id', title: 'Código', width: 100, aggregates: ["count"], footerTemplate: "Total: #= kendo.toString(count, 'n0') #", groupFooterTemplate: "#= kendo.toString(count, 'n0')  #"
                    },
                    {
                        field: 'ProductRef', title: 'Ref', width: 110
                    },
                    {
                        field: 'Barcode', title: 'Cod. Barra', width: 90
                    },
                    {
                        field: 'ProductName', title: 'Produto'
                    },
                    {
                        title: 'Estoque', headerAttributes: { style: 'text-align: center' },
                        columns: [{
                            field: 'QtyStockSet', title: 'Estoque 1', format: '{0:n}', width: 120, attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                        },
                        {
                            field: 'QtyStockOther', title: 'Estoque 2', format: '{0:n}', width: 120, attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                        }]
                    },
                    {
                        field: 'Price', title: 'Preço', format: '{0:n}', attributes: { style: 'text-align: right' }
                    }
                );
                break;
            case 1:
                columnsConfig.push(
                    {
                        field: 'Id', title: 'Código', width: 100, aggregates: ["count"], footerTemplate: "Total: #= kendo.toString(count, 'n0') #", groupFooterTemplate: "#= kendo.toString(count, 'n0')  #"
                    },
                    {
                        field: 'DisplayName', title: 'Cliente'
                    },
                    {
                        field: 'ModifiedOnDate', title: 'Data', width: 70, type: 'date', template: "#= kendo.toString(kendo.parseDate(ModifiedOnDate, 'yyyy-MM-dd'),'dd/MM/yyyy') #", groupHeaderTemplate: "#= kendo.toString(kendo.parseDate(value, 'yyyy-MM-dd'),'dd/MM/yyyy') #"
                    },
                    {
                        field: 'SalesRepName', title: 'Vendedor', width: 120
                    },
                    {
                        field: 'StatusTitle', title: 'Status', width: 80
                    },
                    {
                        field: 'TotalAmount', title: 'Total', width: 90, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                    },
                    {
                        field: 'PayForm', title: 'Finalizador', width: 80
                    },
                    {
                        field: 'PayCond', title: 'Cond. Pag.', width: 80, template: '#= PayCond === "cash" ? "À Vista" : PayCond #'
                    },
                    {
                        field: 'Comission', title: 'Comissão', width: 80, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                    },
                    {
                        field: 'Cost', title: 'Custo', width: 80, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                    },
                    {
                        field: 'Profit', title: 'Lucro', width: 120, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                    }
                );
                break;
            case 5:
                columnsConfig.push(
                    {
                        field: 'Id', title: 'Código', width: 100, aggregates: ["count"], footerTemplate: "Total: #= kendo.toString(count, 'n0') #", groupFooterTemplate: "#= kendo.toString(count, 'n0')  #"
                    },
                    {
                        field: 'DisplayName', title: 'Cliente'
                    },
                    {
                        field: 'ModifiedOnDate', title: 'Data', width: 70, type: 'date', template: "#= kendo.toString(kendo.parseDate(ModifiedOnDate, 'yyyy-MM-dd'),'dd/MM/yyyy') #", groupHeaderTemplate: "#= kendo.toString(kendo.parseDate(value, 'yyyy-MM-dd'),'dd/MM/yyyy') #"
                    },
                    {
                        field: 'SalesRepName', title: 'Vendedor', width: 120
                    },
                    {
                        field: 'StatusTitle', title: 'Status', width: 80
                    },
                    {
                        field: 'TotalAmount', title: 'Total', width: 90, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                    },
                    {
                        field: 'PayTypes', title: 'Finalizador', width: 80
                    },
                    {
                        field: 'PayCond', title: 'Cond. Pag.', width: 80, template: '#= PayCond === "cash" ? "À Vista" : PayCond #'
                    },
                    {
                        field: 'Comission', title: 'Comissão', width: 80, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                    },
                    {
                        field: 'Cost', title: 'Custo', width: 80, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                    },
                    {
                        field: 'Profit', title: 'Lucro', width: 120, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                    }
                );
                break;
            case 6:
                columnsConfig.push(
                    {
                        field: 'Id', title: 'Código', width: 100, aggregates: ["count"], footerTemplate: "Total: #= kendo.toString(count, 'n0') #", groupFooterTemplate: "#= kendo.toString(count, 'n0')  #"
                    },
                    {
                        field: 'DisplayName', title: 'Fornecedor'
                    },
                    {
                        field: 'ContactName', title: 'Contato'
                    },
                    {
                        field: 'Telephone', title: 'Telefone', width: 80
                    },
                    {
                        field: 'Email', title: 'Email'
                    }
                );
                break;
            case 8:
                columnsConfig.push(
                    {
                        field: 'Id', title: 'Código', width: 100, aggregates: ["count"], footerTemplate: "Total: #= kendo.toString(count, 'n0') #", groupFooterTemplate: "#= kendo.toString(count, 'n0')  #"
                    },
                    {
                        field: 'DisplayName', title: 'Fornecedor'
                    },
                    {
                        field: 'ContactName', title: 'Contato'
                    },
                    {
                        field: 'Telephone', title: 'Telefone', width: 80
                    },
                    {
                        field: 'Email', title: 'Email'
                    },
                    {
                        field: 'ProductName', title: 'Produto', hidden: true
                    }
                );

                //my.reportsDataSource.group({
                //    field: 'ProductName'
                //});
                break;
            case 7:
                columnsConfig.push(
                    {
                        field: 'Id', title: 'Código', width: 100, aggregates: ["count"], footerTemplate: "Total: #= kendo.toString(count, 'n0') #", groupFooterTemplate: "#= kendo.toString(count, 'n0')  #"
                    },
                    {
                        field: 'ProductRef', title: 'Ref', width: 110
                    },
                    {
                        field: 'Barcode', title: 'Cod. Barra', width: 90
                    },
                    {
                        field: 'ProductName', title: 'Produto'
                    },
                    {
                        title: 'Estoque', headerAttributes: { style: 'text-align: center' },
                        columns: [{
                            field: 'QtyStockSet', title: 'Estoque 1', format: '{0:n}', width: 120, attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                        },
                        {
                            field: 'QtyStockOther', title: 'Estoque 2', format: '{0:n}', width: 120, attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                        }]
                    },
                    {
                        field: 'ContactName', title: 'Fornecedor', hidden: true
                    }
                );

                //my.reportsDataSource.group({
                //    field: 'ContactName'
                //});
                break;
            case 3:
                columnsConfig.push(
                    {
                        field: 'Id', title: 'Código'
                    },
                    {
                        field: 'ProductName', title: 'Produto'
                    },
                    {
                        field: 'Qty', title: 'Qde', width: 100, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }
                    },
                    {
                        field: 'Cost', title: 'Custo', width: 100, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                    },
                    {
                        field: 'AmountValue', title: 'Valor', width: 100, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }, aggregates: ["sum"], footerTemplate: "#= kendo.toString(sum, 'n') #", footerAttributes: { style: 'text-align: right' }, groupFooterTemplate: "#= kendo.toString(sum, 'n')  #"
                    },
                    {
                        field: 'EstimateInfo', title: 'Orçamento', width: 80, hidden: true
                    }
                );

                //my.reportsDataSource.group({
                //    field: 'EstimateInfo'
                //});
                break;
            default:

        }

        $('#reportGrid').data('kendoGrid').setOptions({
            columns: columnsConfig
        });

        if ((my.vm.sqlDetail() !== null) && (my.vm.sqlDetail() !== '')) {
            $('#reportGrid').data('kendoGrid').setOptions({
                dataBound: function(e) {
                    this.expandRow(this.tbody.find("tr.k-master-row"));
                },
                detailTemplate: '<div class="details"></div>',
                detailInit: detailInit
            });
        }

        $("#reportGrid thead [data-field=ModifiedOnDate] .k-link").html($('#kddlDates').data('kendoDropDownList').text());
        $("#reportGrid thead [data-field=CreatedOnDate] .k-link").html($('#kddlDates').data('kendoDropDownList').text());
    }

    function detailInit(e) {
        var grid = $('#reportGrid').data('kendoGrid'),
            detailRow = e.detailRow;

        if (e.data.Items !== undefined) {
            var detailGrid = detailRow.find('.details').kendoGrid({
                //height: 375,
                dataSource: e.data.Items,
                sortable: true,
                reorderable: true,
                resizable: true,
                scrollable: false,
                aggregate: [
                    {
                        field: "Id", aggregate: "count"
                    },
                    {
                        field: "Codigo", aggregate: "count"
                    },
                    {
                        field: "ProductId", aggregate: "count"
                    },
                    {
                        field: "QtyStockSet", aggregate: "sum"
                    },
                    {
                        field: "QtyStockOther", aggregate: "sum"
                    },
                    {
                        field: "Comission", aggregate: "sum"
                    },
                    {
                        field: "Cost", aggregate: "sum"
                    },
                    {
                        field: "TotalAmount", aggregate: "sum"
                    },
                    {
                        field: "Profit", aggregate: "sum"
                    },
                    {
                        field: "AmountValue", aggregate: "sum"
                    }
                ]
            }).data('kendoGrid');

            function SetSubColumnsConfig() {
                var columnsConfig = [];

                switch (kendo.parseInt(my.reportId)) {
                    case 2:
                        columnsConfig.push(
                            {
                                field: 'Id', title: 'Código'
                            },
                            {
                                field: 'History', title: 'Histórico'
                            }
                        );
                        break;
                    case 4:
                        columnsConfig.push(
                            {
                                field: 'Id', title: 'Código', width: 100, aggregates: ["count"], footerTemplate: "Total: #= kendo.toString(count, 'n') #"
                            },
                            {
                                field: 'Ref', title: 'Ref', width: 110
                            },
                            {
                                field: 'Cod_Barra', title: 'Cod. Barra', width: 90
                            },
                            {
                                field: 'Produto', title: 'Produto'
                            },
                            {
                                field: 'Estoque', title: 'Estoque', width: 120, aggregates: ["sum"], footerTemplate: "Total: #= kendo.toString(sum, 'n') #"
                            }
                        );
                        break;
                    case 1:
                        columnsConfig.push(
                            {
                                field: 'ProductName', title: 'Produto'
                            },
                            {
                                field: 'Qty', title: 'Qde', width: 100, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }
                            },
                            {
                                field: 'Cost', title: 'Custo', width: 100, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }
                            },
                            {
                                field: 'AmountValue', title: 'Valor', width: 100, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }
                            }
                        );
                        break;
                    case 5:
                        columnsConfig.push(
                            {
                                field: 'Id', title: 'Código', width: 100
                            },
                            {
                                field: 'ProductName', title: 'Produto'
                            },
                            {
                                field: 'Qty', title: 'Qde', width: 100, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }
                            },
                            {
                                field: 'Cost', title: 'Custo', width: 100, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }
                            },
                            {
                                field: 'AmountValue', title: 'Valor', width: 100, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }
                            },
                            {
                                field: 'Comission', title: 'Comis', width: 100, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }
                            },
                            {
                                field: 'Profit', title: 'Lucro', width: 100, format: '{0:n}', attributes: { style: 'text-align: right' }, headerAttributes: { style: 'text-align: right' }
                            }
                        );
                        break;
                    default:

                }

                detailGrid.setOptions({
                    columns: columnsConfig
                });
            }

            SetSubColumnsConfig();

        }
    };

    if (my.reportId > 0) {
        getReport(my.reportId, portalID);
    }
    
    $('#btnFilter').click(function (e) {
        e.preventDefault();

        //document.location.hash = my.reportId ? '#rId/' + my.reportId : '';
        //document.location.hash += my.personId ? '&cId=' + my.personId : '';
        //document.location.hash += my.saleRepId ? '&sRepId=' + my.salesRepId : '';
        //document.location.hash += my.productId ? '&pId=' + my.productId : '';
        //document.location.hash += my.statusId ? '&sId=' + my.statusId : '';
        //document.location.hash += my.date1 ? '&date1=' + moment(new Date(my.date1)).format('YYYY/MM/DD') : '&date1=' + moment(new Date($('#kdpStartDate').data('kendoDatePicker').value())).format('YYYY/MM/DD');
        //document.location.hash += my.date2 ? '&date2=' + moment(new Date(my.date2)).format('YYYY/MM/DD') : '&date2=' + moment(new Date($('#kdpEndDate').data('kendoDatePicker').value())).format('YYYY/MM/DD');

        //$('#reportGrid').data('kendoGrid').refresh();

        runGrid();

        SetColumnsConfig();

        my.reportsDataSource.read();
    });

    function printGrid(grid, btn, dlg) {
        
        dlg.dialog('close');
        dlg.dialog('destroy');

        var gridElement = $('#' + grid);
        var dataSource = $('#' + grid).data("kendoGrid").dataSource;

        setTimeout(function () {
            dataSource.pageSize(dataSource.total());

            var printableContent = '',
                win = window.open('', '', 'width=800, height=500'),
                doc = win.document.open();

            var htmlStart =
                    '<!DOCTYPE html>' +
                    '<html>' +
                    '<head>' +
                    '<meta charset="utf-8" />' +
                    '<title>' + $('#reportName').text() + '</title>' +
                    '<script src="/Resources/libraries/jQuery/01_09_01/jquery.js" type="text/javascript"></script> ' +
                    '<script src="/DesktopModules/rildoinfo/webapi/scripts/kendo/2015.3.930/kendo.all.min.js" type="text/javascript"></script> ' +
                    '<link href="http://cdn.kendostatic.com/2015.3.930/styles/kendo.common.min.css" rel="stylesheet" /> ' +
                    '<link href="http://cdn.kendostatic.com/2015.3.930/styles/kendo.uniform.min.css" rel="stylesheet" /> ' +
                    '<style>' +
                    'html { font: 9pt sans-serif; }' +
                    '.k-grid { border-top-width: 0; }' +
                    '.k-grid, .k-grid-content { height: auto !important; }' +
                    '.k-grid-content { overflow: visible !important; }' +
                    '.k-grid .k-grid-header th { border-top: 1px solid; }' +
                    '.k-grid-toolbar, .k-grid-pager, .k-grouping-header { display: none; }' +
                    '.k-hierarchy-cell, .k-hierarchy-col { display: none; }' +
                    '.k-grid-footer { padding-right: 0px !important; }' +
                    'table td { padding: 0 !important; padding-left: 5px !important; padding-right: 3px !important; }' +
                    '</style>' +
                    '</head>' +
                    '<body>' +
                    '<h3> ' + $('#reportName').html() + '</h3>';

            var htmlEnd =
                    '</body>' +
                    '</html>';

            var gridHeader = gridElement.children('.k-grid-header');
            if (gridHeader[0]) {
                var thead = gridHeader.find('thead').clone().addClass('k-grid-header');
                printableContent = gridElement
                    .clone()
                        .children('.k-grid-header').remove()
                    .end()
                        .children('.k-grid-content')
                            .find('table')
                                .first()
                                    .children('tbody').before(thead)
                                .end()
                            .end()
                        .end()
                    .end()[0].outerHTML;
            } else {
                printableContent = gridElement.clone()[0].outerHTML;
            }

            doc.write(htmlStart + printableContent + htmlEnd);
            doc.close();
            btn.button('reset');

            setTimeout(function () {
                win.print();
                //gridElement.data('kendoGrid').dataSource.pageSize(10);
            }, 2000);

            dataSource.pageSize(10);
        }, 1000);
    }

    $('#btnPrint').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        
        var $dialog = $('<div></div>')
                        .html('<div class="confirmDialog">Dependendo da quantidade informações a ser impressa, seu navegador pode parar de responder por um longo período. Por favor espere até que seja prontificado para escolher uma impressora.<br />Deseja Continuar?</div>')
                        .dialog({
                            autoOpen: false,
                            modal: true,
                            resizable: false,
                            dialogClass: 'dnnFormPopup',
                            open: function () {
                                $(".ui-dialog-title").append('Aviso!');
                            },
                            buttons: {
                                'ok': {
                                    text: 'Sim',
                                    //priority: 'primary',
                                    "class": 'dnnPrimaryAction',
                                    click: function () {
                                        $this.button('loading');
                                        printGrid('reportGrid', $this, $dialog);
                                    }
                                },
                                'cancel': {
                                    html: 'N&#227;o',
                                    //priority: 'secondary',
                                    "class": 'dnnSecondaryAction',
                                    click: function () {
                                        $this.button('reset');
                                        $dialog.dialog('close');
                                        $dialog.dialog('destroy');
                                    }
                                }
                            }
                        });

        $dialog.dialog('open');

        //$("#reportGrid").getKendoGrid().saveAsPDF();
    }); $('#btnExcel').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $("#reportGrid").getKendoGrid().saveAsExcel();
    });

    $('#chkBoxCredit').change(function () {
        if (this.readOnly === true) {
            this.checked = false;
            this.readOnly = false;
        }
        else if (this.checked === false) {
            this.indeterminate = true;
            this.readOnly = true;
        }
    });

    $('#chkBoxDone').change(function () {
        if (this.readOnly === true) {
            this.checked = false;
            this.readOnly = false;
        }
        else if (this.checked === false) {
            this.indeterminate = true;
            this.readOnly = true;
        }
    });

});

function clientFormatResult(data) {
    return '<strong>Cliente: </strong><span>' + data.FirstName + ' ' + data.LastName + ' / ' + data.DisplayName + '</span>'
        + '<br /><strong>Email: </strong><span>' + data.Email + '</span>'
        + '<br /><strong>Telefone: </strong><span>' + data.Telephone + '</span>'
        + '<br /><strong>Endere&#231;o: </strong><span>' + data.Street + ' ' + data.Unit + '</span>';
}

function clientFormatSelection(data) {
    return data.DisplayName;
}

function productFormatResult(data) {
    var markup = '<table class="product-result">';
    markup += '<tr><td class="product-info"><div class="product-title">' + data.ProductName + '</div></td></tr>';
    if (data.ProductRef) {
        markup += '<tr><td class="product-info"><div class="product-title">Ref.: ' + data.ProductRef + '</div></td></tr>';
    }
    if (data.Barcode) {
        markup += '<tr><td class="product-info"><div class="product-title">Cod. Barra: ' + data.Barcode + '</div></td></tr>';
    }
    markup += "</table>"
    return markup;
}

function productFormatSelection(data) {
    return data.ProductName;
}
