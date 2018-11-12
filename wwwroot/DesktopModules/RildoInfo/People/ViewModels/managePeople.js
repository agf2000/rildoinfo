
$(function () {

    //kendo.culture("pt-BR");
    my.uId = 0;
    my.sRId = 0;
    my.sId = 0;
    my.rType = '';
    //my.sTerm = '';

    my.viewModel();

    $('#moduleTitleSkinObject').html(moduleTitle);

    var buttons = '<ul class="inline">';
    buttons += '<li><a href="' + categoriesManagerURL + '" class="btn btn-primary btn-medium" title="Categorias"><i class="fa fa-sitemap fa-lg"></i></a></li>';
    buttons += '<li><a href="' + productsManagerURL + '" class="btn btn-primary btn-medium" title="Produtos"><i class="fa fa-barcode fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + peopleManagerURL + '" class="btn btn-primary btn-medium" title="Entidades"><i class="fa fa-group fa-lg"></i></a></li>';
    buttons += '<li><a href="' + usersManagerURL + '" class="btn btn-primary btn-medium" title="Colaboradores"><i class="fa fa-suitcase fa-lg"></i></a></li>';
    buttons += '<li><a href="' + invoicesManagerURL + '" class="btn btn-primary btn-medium" title="Lançamentos"><i class="fa fa-money fa-lg"></i></a></li>';
    buttons += '<li><a href="' + accountsManagerURL + '" class="btn btn-primary btn-medium" title="Contas"><i class="fa fa-book fa-lg"></i></a></li>';
    buttons += '<li><a href="' + agendaURL + '" class="btn btn-primary btn-medium" title="Agenda"><i class="fa fa-calendar fa-lg"></i></a></li>';
    buttons += '<li><a href="' + orURL + '" class="btn btn-primary btn-medium" title="Website"><i class="fa fa-shopping-cart fa-lg"></i></a></li>';
    buttons += '<li><a href="' + reportsManagerURL + '" class="btn btn-primary btn-medium" title="Relatórios"><i class="fa fa-bar-chart-o fa-lg"></i></a></li>';
    buttons += '<li><a href="' + storeManagerURL + '" class="btn btn-primary btn-medium" title="Loja"><i class="fa fa-cogs fa-lg"></i></a></li>';
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

    if (!authorized >= 3) $('li:nth-child(5)').hide();

    $('#kdpStartDate').kendoDatePicker();
    $('#kdpEndDate').kendoDatePicker();
    $('#btnSyncPeople').kendoButton();

    $('#ddlStatuses').kendoDropDownList({
        autoBind: false,
        optionLabel: "Todos Status",
        dataTextField: "StatusTitle",
        datValueField: "StatusId",
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/statuses/GetStatuses?portalId=' + portalID + '&isDeleted=False'
                }
            }
        }
    });

    $('#kddlEntitiesRoles').kendoDropDownList({
        //autoBind: false,
        //optionLabel: "Todas Entidades",
        dataTextField: 'RoleName',
        dataValueField: 'RoleId',
        select: function (e) {
            var dataItem = this.dataItem(e.item.index());
            my.rType = dataItem.RoleId;
            my.peopleData.read();
        },
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/store/getRolesByRoleGroup?portalId=' + portalID + '&roleGroupName=Entidades'
                }
            }
        },
        text: 'Clientes',
        dataBound: function (e) {
            var data = this.dataSource.view();
 
            for (var idx = 0; idx < data.length; idx++) {
                if (data[idx]) {
                    this.value(data[idx].RoleId);
                    my.rType = data[idx].RoleId;
                    break;
                }
            }
        }
    });

    $('#kddlSalesGroup').kendoDropDownList({
        autoBind: false,
        optionLabel: "Todos Vendedores",
        dataTextField: 'DisplayName',
        dataValueField: 'UserId',
        //select: my.clients.read(),
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/people/GetUsersByRoleGroup?portalId=' + portalID + '&roleGroupName=Departamentos'
                }
            }
        }
    });

    if (authorized > 2) {
        $('#liSalesPersons').show();
    }

    if (my.storage) {
        if (amplify.store.sessionStorage(siteURL + '_chkDeleted')) {
            var _chkDeleted = amplify.store.sessionStorage(siteURL + '_chkDeleted');
            if (_chkDeleted) {
                $('#chkDeleted').prop('checked', true);
                $('#btnIsDeleted').text('Esconder Desativados');
            } else {
                $('#chkDeleted').prop('checked', false);
                $('#btnIsDeleted').text('Mostrar Desativados');
            }
        }
    }

    function onSalesPersonSelect(e) {
        var dataItem = this.dataItem(e.item.index());
        my.sRId = dataItem.UserId;
        my.peopleData.read();
    }

    $('#kddlSalesGroup').data("kendoDropDownList").bind("select", onSalesPersonSelect);

    function onStatusSelect(e) {
        var dataItem = this.dataItem(e.item.index());
        my.sId = dataItem.StatusId;
    }

    $('#ddlStatuses').data("kendoDropDownList").bind("select", onStatusSelect);

    $('#txtSearch').keydown(function (e) {
        if (e.keyCode === 13) {
            $('#btnSearch').click();
        }
    });

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        my.vm.filter($('#txtSearch').val());
        my.peopleData.read();
    });

    function detailInit(e) {
        var detailRow = e.detailRow,
            personId = e.data.PersonId;

        var tabStrip = detailRow.find('.personDetails');
        $(tabStrip).kendoTabStrip({
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
            //height: 170
        });

        //var contactsData = new kendo.data.DataSource({
        //    autoSync: true,
        //    transport: {
        //        read: {
        //            url: '/desktopmodules/riw/api/people/GetPersonContacts?personId=' + e.data.PersonId
        //        }
        //    },
        //    sort: { field: "ContactName", dir: "ASC" },
        //    schema: {
        //        model: {
        //            id: 'PersonContactId',
        //            fields: {
        //                DateBirth: { type: "date", format: "{0:m}" }
        //            }
        //        }
        //    }
        //});

        my.estimatesTransport = {
            read: {
                url: '/desktopmodules/riw/api/estimates/getEstimates'
            },
            parameterMap: function (data, type) {
                return {
                    portalId: portalID,
                    personId: personId,
                    salesRep: -1,
                    statusId: -1,
                    sDate: '',
                    eDate: '',
                    filter: '""',
                    isDeleted: 'False',
                    getAll: 'False',
                    pageIndex: data.page,
                    pageSize: data.pageSize,
                    orderBy: data.sort[0] ? data.sort[0].field : 'ModifiedOnDate',
                    orderDesc: data.sort[0] ? data.sort[0].dir : 'DESC'
                };
            }
        };

        my.estimatesData = new kendo.data.DataSource({
            transport: my.estimatesTransport,
            pageSize: 5,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            serverAggregates: true,
            sort: { field: "ModifiedOnDate", dir: "DESC" },
            schema: {
                model: {
                    id: 'EstimateId',
                    fields: {
                        ModifiedOnDate: { type: "date", format: "{0:g}" },
                        TotalAmount: { type: "number" }
                    }
                },
                data: 'data',
                total: 'total'
            }
        });

        detailRow.find('.estimates').kendoGrid({
            //height: 375,
            dataSource: my.estimatesData,
            columns: [
                {
                    title: 'ID',
                    field: 'EstimateId',
                    width: 80
                },
                {
                    title: 'Vendedor',
                    field: 'SalesRepName'
                },
                {
                    title: 'Status',
                    field: 'StatusTitle',
                    width: 110
                },
                {
                    title: 'Total',
                    field: 'TotalAmount',
                    width: 100,
                    format: '{0:n}',
                    attributes: { style: 'text-align: right' }
                },
                {
                    title: 'Data Mod.',
                    field: 'ModifiedOnDate',
                    format: '{0:g}',
                    //template: '#= kendo.toString(ModifiedOnDate, "g") #',
                    width: 120
                },
                {
                    command: [
                        {
                            name: "open",
                            text: " ",
                            imageClass: "icon icon-eye-open",
                            click: function (e) {
                                e.preventDefault();

                                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                if (dataItem) {

                                    $("#personWindow").append("<div id='window'></div>");
                                    var sContent = clientEstimateURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#estimateId/' + dataItem.EstimateId + '/personId/' + dataItem.PersonId,
                                        kendoWindow = $('#window').kendoWindow({
                                            actions: ["Refresh", "Maximize", "Close"],
                                            title: 'Orçamento: ' + (dataItem.EstimateTitle ? dataItem.EstimateTitle : '') + ' (ID: ' + dataItem.EstimateId + ') ' + (authorized === 1 ? (dataItem.EstimateTitle ? dataItem.EstimateTitle : '') : dataItem.ClientDisplayName + ' (ID: ' + dataItem.PersonId + ')'),
                                            modal: true,
                                            width: '99%',
                                            height: '80%',
                                            content: sContent,
                                            close: function (e) {
                                                $("html, body").css("overflow", "");
                                                my.estimatesData.read();
                                            },
                                            open: function () {
                                                $("html, body").css("overflow", "hidden");
                                                $('#window').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
                                            },
                                            deactivate: function () {
                                                this.destroy();
                                            }
                                        });

                                    kendoWindow.data("kendoWindow").center().open();

                                    $.ajax({
                                        url: sContent, success: function (data) {
                                            kendoWindow.data("kendoWindow").refresh();
                                        }
                                    });

                                }
                            }
                        }
                    ],
                    title: 'Abrir',
                    width: 55
                }
            ],
            dataBound: function (e) {
                if (this.dataSource.view().length > 0) {
                    $('#estimates_' + personId.toString()).html('Orçamentos: (' + this.dataSource.data()[0].TotalRows.toString() + ') ' + kendo.toString(this.dataSource.data()[0].TotalEstimates, 'c'));
                }
            },
            pageable: true,
            //pageable: {
            //    pageSizes: [5, 10, 20, 40, 60],
            //    refresh: true,
            //    numeric: false,
            //    input: true,
            //    messages: {
            //        display: "{0} - {1} de {2} orçamentos",
            //        empty: "Sem Registro.",
            //        page: "Página",
            //        of: "de {0}",
            //        itemsPerPage: "Orçamentos por página",
            //        first: "Ir para primeira página",
            //        previous: "Ir para página anterior",
            //        next: "Ir para próxima página",
            //        last: "Ir para última página",
            //        refresh: "Recarregar"
            //    }
            //},
            selectable: "row",
            sortable: true,
            reorderable: true,
            resizable: true
        });
        //};

        my.ordersTransport = {
            read: {
                url: '/desktopmodules/riw/api/estimates/getEstimates'
            },
            parameterMap: function (data, type) {
                return {
                    portalId: portalID,
                    personId: personId,
                    salesRep: -1,
                    statusId: 10,
                    sDate: '',
                    eDate: '',
                    filter: '""',
                    isDeleted: 'False',
                    getAll: 'True',
                    pageIndex: data.page,
                    pageSize: data.pageSize,
                    orderBy: data.sort[0] ? data.sort[0].field : 'ModifiedOnDate',
                    orderDesc: data.sort[0] ? data.sort[0].dir : 'DESC'
                };
            }
        };

        my.ordersData = new kendo.data.DataSource({
            transport: my.ordersTransport,
            pageSize: 5,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            //serverAggregates: true,
            sort: { field: "ModifiedOnDate", dir: "DESC" },
            schema: {
                model: {
                    id: 'EstimateId',
                    fields: {
                        ModifiedOnDate: { type: "date", format: "{0:g}" },
                        TotalAmount: { type: "number" }
                    }
                },
                data: 'data',
                total: 'total'
            }
        });

        detailRow.find('.orders').kendoGrid({
            //height: 375,
            dataSource: my.ordersData,
            columns: [
                {
                    title: 'ID',
                    field: 'EstimateId',
                    width: 80
                },
                {
                    title: 'Vendedor',
                    field: 'SalesRepName'
                },
                {
                    title: 'Total',
                    field: 'TotalAmount',
                    width: 100,
                    format: '{0:n}',
                    attributes: { class: 'text-right' }
                },
                {
                    title: 'Data Mod.',
                    field: 'ModifiedOnDate',
                    format: '{0:g}',
                    //template: '#= kendo.toString(ModifiedOnDate, "g") #',
                    width: 120
                },
                {
                    command: [
                        {
                            name: "open",
                            text: " ",
                            imageClass: "icon icon-eye-open",
                            click: function (e) {
                                e.preventDefault();

                                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                if (dataItem) {

                                    $("#personWindow").append("<div id='window'></div>");
                                    var sContent = clientEstimateURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#estimateId/' + dataItem.EstimateId + '/personId/' + dataItem.PersonId,
                                        kendoWindow = $('#window').kendoWindow({
                                            actions: ["Refresh", "Maximize", "Close"],
                                            title: 'Venda: ' + (dataItem.EstimateTitle ? dataItem.EstimateTitle : '') + ' (ID: ' + dataItem.EstimateId + ') ' + (authorized === 1 ? (dataItem.EstimateTitle ? dataItem.EstimateTitle : '') : dataItem.ClientDisplayName + ' (ID: ' + dataItem.PersonId + ')'),
                                            modal: true,
                                            width: '99%',
                                            height: '80%',
                                            content: sContent,
                                            close: function (e) {
                                                $("html, body").css("overflow", "");
                                                my.estimatesData.read();
                                            },
                                            open: function () {
                                                $("html, body").css("overflow", "hidden");
                                                $('#window').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
                                            },
                                            deactivate: function () {
                                                this.destroy();
                                            }
                                        });

                                    kendoWindow.data("kendoWindow").center().open();

                                    $.ajax({
                                        url: sContent, success: function (data) {
                                            kendoWindow.data("kendoWindow").refresh();
                                        }
                                    });

                                }
                            }
                        }
                    ],
                    title: 'Abrir',
                    width: 55
                }
            ],
            dataBound: function (e) {
                if (this.dataSource.view().length > 0) {
                    $('#sales_' + personId.toString()).html('Vendas: (' + this.dataSource.data()[0].TotalRows.toString() + ') ' + kendo.toString(this.dataSource.data()[0].TotalSales, 'c'));
                }
            },
            selectable: "row",
            pageable: true,
            //pageable: {
            //    pageSizes: [5, 10, 20, 40, 60],
            //    refresh: true,
            //    numeric: false,
            //    input: true,
            //    messages: {
            //        display: "{0} - {1} de {2} Vendas",
            //        empty: "Sem Registro.",
            //        page: "Página",
            //        of: "de {0}",
            //        itemsPerPage: "Vendas por página",
            //        first: "Ir para primeira página",
            //        previous: "Ir para página anterior",
            //        next: "Ir para próxima página",
            //        last: "Ir para última página",
            //        refresh: "Recarregar"
            //    }
            //},
            sortable: true,
            reorderable: true,
            resizable: true
        });

        var contactsLV = detailRow.find('.contacts').kendoGrid({
            dataSource: new kendo.data.DataSource({
                autoSync: true,
                transport: {
                    read: {
                        url: '/desktopmodules/riw/api/people/GetPersonContacts?personId=' + e.data.PersonId
                    }
                },
                sort: { field: "ContactName", dir: "ASC" },
                schema: {
                    model: {
                        id: 'PersonContactId',
                        fields: {
                            DateBirth: { type: "date", format: "{0:m}" }
                        }
                    }
                }
            }),
            columns: [
                { title: 'Contatos', template: kendo.template($("#tmplPersonContacts").html()) }
            ]
        });

        //contactsLV.data('kendoGrid').dataSource.sync();

        detailRow.find('.addresses').kendoGrid({
            dataSource: {
                transport: {
                    read: '/desktopmodules/riw/api/people/GetPersonAddresses?personId=' + e.data.PersonId
                },
                sort: { field: "AddressName", dir: "ASC" },
                schema: {
                    model: {
                        id: 'PersonAddressId'
                    }
                }
            },
            //height: 160,
            columns: [
                { title: 'Endereços', template: kendo.template($("#tmplPersonAddresses").html()) }
            ]
        });

        setTimeout(function () {
            contactsLV.data('kendoGrid').dataSource.add({ ContactName: e.data.DisplayName, PersonContactId: '0', ContactPhone1: e.data.Telephone, ContactEmail1: e.data.Email, ContactEmail2: null, PhoneExt1: null, ContactPhone2: null, PhoneExt2: null, Dept: null, DateBirth: null, AddressName: null, Comments: null });
        }, 1000);
    };

    my.vm.deleteds(my.totalDeletedClients);

    my.peopleTransport = {
        read: {
            url: '/desktopmodules/riw/api/people/GetPeople'
        },
        parameterMap: function (data, type) {
            return {
                portalId: portalID,
                isDeleted: $('#chkDeleted').is(':checked'),
                sRep: authorized > 2 ? my.sRId > 0 ? my.sRId : -1 : userID,
                registerType: my.rType || clientsRoleId,
                sTerm: my.vm.filter().length ? my.vm.filter() : '',
                sId: my.sId > 0 ? my.sId : -1,
                searchField: $('#kddlFields').data('kendoDropDownList').value(),
                sDate: $('#kdpStartDate').val().length > 0 ? moment(new Date($('#kdpStartDate').data('kendoDatePicker').value())).format() : '',
                eDate: $('#kdpEndDate').val().length > 0 ? moment(new Date($('#kdpEndDate').data('kendoDatePicker').value())).format() : '',
                filterDate: $('#ddlDates').data('kendoDropDownList').value(),
                pageIndex: data.page,
                pageSize: data.pageSize,
                orderBy: data.sort[0] ? data.sort[0].field : 'PersonId',
                orderDesc: data.sort[0] ? data.sort[0].dir : ''
            };
        }
    };

    my.peopleData = new kendo.data.DataSource({
        transport: my.peopleTransport,
        pageSize: 10,
        serverPaging: true,
        serverSorting: true,
        serverFiltering: true,
        sort: { field: "PersonId", dir: "DESC" },
        schema: {
            model: {
                id: 'PersonId',
                fields: {
                    PersonId: { type: 'number' },
                    CompanyName: { type: 'string' },
                    FirstName: { type: 'string' },
                    LastName: { type: 'string' },
                    StatusTitle: { type: 'string' },
                    HasLogin: { type: 'boolean' },
                    ModifiedOnDate: { type: "date", format: "{0:g}" }
                }
            },
            data: 'data',
            total: 'total'
        }
    });

    $("#peopleGrid").kendoGrid({
        dataSource: my.peopleData,
        //height: 550,
        toolbar: kendo.template($("#tmplToolbar").html()),
        columns: [
            { field: "PersonId", title: "ID", width: 50 },
            { field: "CompanyName", title: "Empresa" },
            { field: "FirstName", title: "Nome" },
            { field: "LastName", title: "Sobrenome" },
            { field: "DisplayName", title: "Apelido" },
            { field: "StatusTitle", title: "Status", width: 110 },
            //{ field: "ModifiedOnDate", title: "Data", type: "date", format: "{0:g}", width: 130 },
            { field: "HasLogin", title: "Login", width: 70, filterable: false, template: '#= HasLogin ? "Sim" : "Não" #' }
        ],
        selectable: "row",
        sortable: true,
        pageable: {
            pageSizes: [15, 30, 50, 80],
            refresh: true,
            numeric: false,
            input: true,
            messages: {
                display: "{0} - {1} de {2} Entidades",
                empty: "Sem Registro.",
                //page: "Página",
                //of: "de {0}",
                itemsPerPage: "Entidades por página"
                //first: "Ir para primeira página",
                //previous: "Ir para página anterior",
                //next: "Ir para próxima página",
                //last: "Ir para última página",
                //refresh: "Recarregar"
            }
        },
        change: function () {
            var row = this.select();
            var id = row.data("uid");
            my.uId = id;
            var dataItem = this.dataItem(row);
            if (dataItem) {
                $('#btnAssistSelected').attr({ 'disabled': false });
                $('#btnEditSelected').attr({ 'disabled': false });
                $('#btnExportSelected').attr({ 'disabled': false });
                $('#btnFinanSelected').attr({ 'disabled': false });
                $('#btnCommSelected').attr({ 'disabled': false });
                $('#btnHistorySelected').attr({ 'disabled': false });
                if (dataItem.Locked) {
                    if (authorized > 2) {
                        $('#btnDeleteSelected').show();
                    }
                    $('#btnRemoveSelected').hide();
                } else {
                    if (authorized > 2) {
                        $('#btnRemoveSelected').show();
                    } else {
                        $('#btnRemoveSelected').hide();
                    }
                    $('#btnDeleteSelected').show();
                }
                if (dataItem.IsDeleted) {
                    $('#btnRestoreSelected').show();
                    $('#btnDeleteSelected').hide();
                } else {
                    $('#btnRestoreSelected').hide();
                }
                if (dataItem.Email) {
                    $('#btnCommSelected').attr({ 'disabled': false });
                } else {
                    $('#btnCommSelected').attr({ 'disabled': true });
                }
                //if (dataItem.PersonId === 1) {
                //    $('#btnDeleteSelected').hide();
                //    $('#btnRemoveSelected').hide();
                //}
            }
        },
        dataBound: function () {
            $('#btnRestoreSelected').hide();
            $('#btnDeleteSelected').hide();
            $('#btnRemoveSelected').hide();
            $('#btnAssistSelected').attr({ 'disabled': true });
            $('#btnEditSelected').attr({ 'disabled': true });
            $('#btnExportSelected').attr({ 'disabled': true });
            $('#btnFinanSelected').attr({ 'disabled': true });
            $('#btnCommSelected').attr({ 'disabled': true });
            $('#btnHistorySelected').attr({ 'disabled': true });
            if (this.dataSource.view().length > 0) {
                var grid = this;
                //_chkDeleted = false;
                $.each(grid.dataSource.data(), function (i, person) {
                    var rowSelector = ">tr:nth-child(" + (i + 1) + ")";
                    var row = grid.tbody.find(rowSelector);
                    if (person.IsDeleted) {
                        row.addClass('isDeleted');
                        //if (!$('#chkDeleted').is(':checked')) {
                        //    row.hide();
                        //    if (!my.deleted) {
                        //        my.vm.deleteds.push(dataItem.ProductId);
                        //    }
                        //}
                    }
                    if (person.Blocked) {
                        row.addClass('invalid');
                    }
                });

                //for (var i = 0; i < grid.dataSource.view().length ; i++) {
                //    var dataItem = grid.dataSource.view()[i];
                //    var rowSelector = ">tr:nth-child(" + (i + 1) + ")";
                //    //Grab a reference to the corrosponding data row
                //    var row = grid.tbody.find(rowSelector);
                //    if (dataItem.IsDeleted) {
                //        row.addClass('isDeleted');
                //        //_chkDeleted = true;
                //        //return true;
                //    }
                //}
                //if (_chkDeleted) {
                //    $('#btnIsDeleted').show();
                //} else {
                //    $('#btnIsDeleted').hide();
                //}
            }
        },
        detailTemplate: kendo.template($("#tmplPersonDetail").html()),
        detailInit: detailInit
    });

    $("#peopleGrid").delegate("tbody > tr", "dblclick", function () {
        $('#btnEditSelected').click();
    });

    $('#btnEditSelected').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var grid = $('#peopleGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        $("#personWindow").append("<div id='window'></div>");
        var sContent = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + dataItem.PersonId + '/sel/7/subSel/1',
            kendoWindow = $('#window').kendoWindow({
                actions: ["Maximize", "Close"],
                title: dataItem.DisplayName + ' (ID: ' + dataItem.PersonId + ')',
                modal: true,
                width: '99%',
                height: '80%',
                content: sContent,
                close: function (e) {
                    $("html, body").css("overflow", "");
                    my.peopleData.read();
                },
                open: function () {
                    $("html, body").css("overflow", "hidden");
                    $('#window').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
                },
                deactivate: function () {
                    this.destroy();
                }
            });

        kendoWindow.data("kendoWindow").center().open();

        $.ajax({
            url: sContent, success: function (data) {
                kendoWindow.data("kendoWindow").refresh();
            }
        });
    });
        
    $('#btnExportSelected').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var grid = $('#peopleGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        var kendoWindow = $("<div />").kendoWindow({
            title: "Baixar Arquivo",
            resizable: false,
            modal: true,
            width: 550
        });

        kendoWindow.data("kendoWindow")
            .content('<h3 class="DNNAligncenter">' + dataItem.DisplayName + '</h3><h5 class="DNNAligncenter"><a href="/portals/0/people/' + dataItem.PersonId.toString() + '.txt">' + dataItem.PersonId.toString() + '.txt</a></h5><h6 class="DNNAligncenter">Clique com o botão da direita do mouse no link e escolha a opção "salvar link".</h6>')
            .center().open();
    });

    $('#btnHistorySelected').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var grid = $('#peopleGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        $("#personWindow").append("<div id='window'></div>");
        var sContent = assistURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + dataItem.PersonId + '/sel/10',
            kendoWindow = $('#window').kendoWindow({
                actions: ["Maximize", "Close"],
                title: dataItem.DisplayName + ' (ID: ' + dataItem.PersonId + ')',
                modal: true,
                width: '99%',
                height: '80%',
                content: sContent,
                close: function (e) {
                    $("html, body").css("overflow", "");
                },
                open: function () {
                    $("html, body").css("overflow", "hidden");
                    $('#window').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
                },
                deactivate: function () {
                    this.destroy();
                }
            });

        kendoWindow.data("kendoWindow").center().open();

        $.ajax({
            url: sContent, success: function (data) {
                kendoWindow.data("kendoWindow").refresh();
            }
        });
    });

    $('#btnFinanSelected').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var grid = $('#peopleGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        $("#personWindow").append("<div id='window'></div>");
        var sContent = finanURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + dataItem.PersonId + '/sel/9',
            kendoWindow = $('#window').kendoWindow({
                actions: ["Maximize", "Close"],
                title: dataItem.DisplayName + ' (ID: ' + dataItem.PersonId + ')',
                modal: true,
                width: '99%',
                height: '80%',
                content: sContent,
                close: function (e) {
                    $("html, body").css("overflow", "");
                },
                open: function () {
                    $("html, body").css("overflow", "hidden");
                    $('#window').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
                },
                deactivate: function () {
                    this.destroy();
                }
            });

        kendoWindow.data("kendoWindow").center().open();

        $.ajax({
            url: sContent, success: function (data) {
                kendoWindow.data("kendoWindow").refresh();
            }
        });
    });

    $('#btnCommSelected').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var grid = $('#peopleGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        $("#personWindow").append("<div id='window'></div>");
        var sContent = commURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + dataItem.PersonId + '/sel/11',
            kendoWindow = $('#window').kendoWindow({
                actions: ["Maximize", "Close"],
                title: dataItem.DisplayName + ' (ID: ' + dataItem.PersonId + ')',
                modal: true,
                width: '99%',
                height: '80%',
                content: sContent,
                close: function (e) {
                    $("html, body").css("overflow", "");
                },
                open: function () {
                    $("html, body").css("overflow", "hidden");
                    $('#window').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
                },
                deactivate: function () {
                    this.destroy();
                }
            });

        kendoWindow.data("kendoWindow").center().open();

        $.ajax({
            url: sContent, success: function (data) {
                kendoWindow.data("kendoWindow").refresh();
            }
        });
    });

    $('#btnAssistSelected').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var grid = $('#peopleGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        $("#personWindow").append("<div id='window'></div>");
        var sContent = historyURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + dataItem.PersonId + '/sel/8',
            kendoWindow = $('#window').kendoWindow({
                actions: ["Maximize", "Close"],
                title: dataItem.DisplayName + ' (ID: ' + dataItem.PersonId + ')',
                modal: true,
                width: '99%',
                height: '80%',
                content: sContent,
                close: function (e) {
                    $("html, body").css("overflow", "");
                },
                open: function () {
                    $("html, body").css("overflow", "hidden");
                    $('#window').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
                },
                deactivate: function () {
                    this.destroy();
                }
            });

        kendoWindow.data("kendoWindow").center().open();

        $.ajax({
            url: sContent, success: function (data) {
                kendoWindow.data("kendoWindow").refresh();
            }
        });
    });

    $('#btnDeleteSelected').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var grid = $('#peopleGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        var params = {
            PersonId: dataItem.PersonId,
            PortalId: portalID,
            UserId: dataItem.UserId,
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };

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
                                    click: function (e) {
                                        $.ajax({
                                            type: 'PUT',
                                            url: '/desktopmodules/riw/api/people/DeletePerson',
                                            data: params,
                                            beforeSend: function () {
                                                $this.button('loading');
                                                $dialog.dialog('close');
                                                $dialog.dialog('destroy');
                                            }
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                //grid.dataSource.read();
                                                grid.tbody.find(grid.select()).addClass('isDeleted');
                                                //grid.refresh();
                                                $('#btnDeleteSelected').hide();
                                                $('#btnRemoveSelected').hide();
                                                $('#btnRestoreSelected').show();
                                                //$().toastmessage('showSuccessToast', 'Conta desativada com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Conta desativada.',
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
                                                //$().toastmessage('showErrorToast', data.Msg);
                                            }
                                        }).fail(function (jqXHR, textStatus) {
                                            console.log(jqXHR.responseText);
                                        }).always(function () {
                                            $this.button('reset');
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
    });

    $('#btnRemoveSelected').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var grid = $('#peopleGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        var $dialog = $('<div></div>')
                        .html('<div class="confirmDialog">Tem Certeza? Todas as informa&#231;&#245;es referente &#224; esta conta ser&#227;o excluidas. Esta a&#231;&#227;o n&#227;o poder&#225; ser revertida.</div>')
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
                                    click: function (e) {
                                        $.ajax({
                                            type: 'DELETE',
                                            url: '/desktopmodules/riw/api/people/RemovePerson?personId=' + dataItem.PersonId + '&portalId=' + portalID + '&userId=' + dataItem.UserId,
                                            beforeSend: function () {
                                                $this.button('loading');
                                                $dialog.dialog('close');
                                                $dialog.dialog('destroy');
                                            }
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                //grid.dataSource.read();
                                                grid.dataSource.remove(dataItem);
                                                //grid.refresh();
                                                $('#btnDeleteSelected').hide();
                                                $('#btnRemoveSelected').hide();
                                                $('#btnRestoreSelected').hide();
                                                //$().toastmessage('showSuccessToast', 'Conta excluida com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Conta excluida.',
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
                                                //$().toastmessage('showErrorToast', data.Msg);
                                            }
                                        }).fail(function (jqXHR, textStatus) {
                                            console.log(jqXHR.responseText);
                                        }).always(function () {
                                            $this.button('reset');
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
    });

    $('#btnRestoreSelected').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var grid = $('#peopleGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        var params = {
            PersonId: dataItem.PersonId,
            PortalId: portalID,
            UserId: dataItem.UserId,
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };

        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/people/RestorePerson',
            data: params,
            beforeSend: function () {
                $this.button('loading');
            }
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                grid.tbody.find(grid.select()).removeClass('isDeleted');
                //grid.refresh();
                $('#btnDeleteSelected').show();
                $('#btnRemoveSelected').hide();
                $('#btnRestoreSelected').hide();
                //$().toastmessage('showSuccessToast', 'Conta ativada com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Conta ativada.',
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
                //$().toastmessage('showErrorToast', data.Msg);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
    });

    $("#btnAddNewPerson").click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $("#personWindow").append("<div id='window'></div>");
        var sContent = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer',
            kendoWindow = $('#window').kendoWindow({
                actions: ["Maximize", "Close"],
                title: 'Novo Cadastro',
                modal: true,
                width: '99%',
                height: '80%',
                content: sContent,
                close: function (e) {
                    $("html, body").css("overflow", "");
                    my.peopleData.read();
                },
                open: function () {
                    $("html, body").css("overflow", "hidden");
                    $('#window').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
                },
                deactivate: function () {
                    this.destroy();
                }
            });

        kendoWindow.data("kendoWindow").center().open();

        $.ajax({
            url: sContent, success: function (data) {
                kendoWindow.data("kendoWindow").refresh();
            }
        });
    });

    $('#btnSyncPeople').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        $('#SyncMsg').text('');

        var $this = $(this);
        $this.button('loading');

        var theDate = new Date();

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/people/SyncPeople?sDate=' + moment(new Date(2015, 5, 1)).format('YYYY-MM-DD') + '&eDate=' + moment(new Date()).add(1, 'days').format('YYYY-MM-DD')
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Sincronização completa.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                $('#SyncMsg').html('Inseridos ' + data.Added.toString() + ' / Atualizados ' + data.Updated.toString())
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

    $('#btnIsDeleted').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();
        $('#chkDeleted').click();
        if (my.storage) amplify.store.sessionStorage(siteURL + '_chkDeleted', $('#chkDeleted').is(':checked'));
        ($('#chkDeleted').is(':checked')) ? $('#btnIsDeleted').text('Esconder Desativados') : $('#btnIsDeleted').text('Mostrar Desativados (' + my.vm.deleteds() + ')');
        my.peopleData.read();
    });

    //$('#advancedSearch').click(function (e) {
    //    $('#divPeopleManagerButtons').show();
    //    //$('#advancedSearch').hide();
    //    //$('#simpleSearch').show();
    //    $('.dropdown-menu').dropdown('toggle')
    //    return false;
    //});

    //$('#simpleSearch').click(function (e) {
    //    $('#divPeopleManagerButtons').hide();
    //    //$('#advancedSearch').show();
    //    //$('#simpleSearch').hide();
    //    $('.dropdown-menu').dropdown('toggle')
    //    return false;
    //});

});
