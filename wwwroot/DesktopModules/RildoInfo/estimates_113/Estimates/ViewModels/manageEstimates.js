
$(function () {

    my.uId = 0;
    my.sRId = -1;
    my.personId = 0;
    my.hasFocus = true;
    my.estimateId = my.getQuerystring('estimateId', my.getParameterByName('estimateId'));
    my.new = my.getQuerystring('new', my.getTopParameterByName('new'));

    my.viewModel();

    $('#moduleTitleSkinObject').html(moduleTitle);

    var buttons = '<ul class="inline">';
    buttons += '<li><a href="' + categoriesManagerURL + '" class="btn btn-primary btn-medium" title="Categorias"><i class="fa fa-sitemap fa-lg"></i></a></li>';
    buttons += '<li><a href="' + productsManagerURL + '" class="btn btn-primary btn-medium" title="Produtos"><i class="fa fa-barcode fa-lg"></i></a></li>';
    buttons += '<li><a href="' + peopleManagerURL + '" class="btn btn-primary btn-medium" title="Entidades"><i class="fa fa-group fa-lg"></i></a></li>';
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

    var notification_support = function () {
        if (window.webkitNotifications && navigator.userAgent.indexOf("Chrome") > -1) {
            if (webkitNotifications.checkPermission() === 1) {
                $("#ask_permission").show();
                $("#ask_permission").on('click', function () {
                    //webkitNotifications.requestPermission();
                    Notification.requestPermission(function (perm) {
                        if (perm === 'granted') {
                            $("#ask_permission").hide();
                        }
                    });
                });
            } else {
                $("#ask_permission").hide();
            }
        } else if (window.Notification) {
            if (window.Notification.permission === 'default' || window.Notification.permission === 'denied') {
                $("#ask_permission").on('click', function () {
                    Notification.requestPermission(function (perm) {
                        if (perm === 'granted') {
                            $("#ask_permission").hide();
                        } else if (perm === 'denied') {
                            alert('Não está sendo possível ativar a visualização de notificações. Veja com o administrador do sistema como ativar esta opção.');
                        }
                    });
                });
            } else {
                $("#ask_permission").hide();
            }
        }
    }

    notification_support();

    //var visibilityChange = (function () {
    //    return function () {
    //        window.onfocus = window.onblur = window.onpageshow = window.onpagehide = function (e) {
    //             if (e.type) {
    //                console.log("visible");
    //            } else {
    //                console.log("hidden");
    //            }
    //        };
    //    };
    //}(this));

    //var visibilityChange = (function () {
    //    return function () {
    //        window.onfocus = window.onblur = window.onpageshow = window.onpagehide = function (e) {
    //            if (e.type === 'focus') {
    //                console.log("visible");
    //                my.hasFocus = true;
    //            } else {
    //                console.log("hidden");
    //                my.hasFocus = false;
    //            }
    //        };
    //    };
    //}(this));
    
    window.onfocus = window.onblur = window.onpageshow = window.onpagehide = function (e) {
        if (e.type === 'focus') {
            //console.log("visible");
            my.hasFocus = true;
        } else {
            //console.log("hidden");
            my.hasFocus = false;
        }
    };
    
    //setInterval(function() { visibilityChange() }, 5000);

    //    if (window.Notification.permission === 'granted') {
    //        $("#ask_permission").hide();
    //    } else {
    //        $("#ask_permission").on('click', function () {
    //            Notification.requestPermission();
    //            if (window.Notification.permission === 'granted') {
    //                $("#ask_permission").hide();
    //            }
    //        });
    //    }
    //} else {
    //    var config = {
    //        nitification: {
    //            ntitle: siteName,
    //            nbody: 'Necessitamos de sua atenção no website ' + siteURL + '.'
    //        }
    //    };
    //    $.wnf(config);
    //}

    ////// Request notification permission
    //$('#ask_permission').on('click', function () {
    //    window.Notification.requestPermission(function (grant) {
    //        // This allows to use Notification.permission with Chrome/Safari
    //        //if (window.Notification.permission !== status) {
    //        //    window.Notification.permission = status;
    //        //}
    //    });

    //    //window.Notification.requestPermission(function (grant) {
    //    //    // ['default', 'granted', 'denied'].indexOf(grant) === true
    //    //});
    //});

    if (managerRole === 'False') $('#estimatesGrid li:nth-child(4)').hide();

    $('#kdpStartDate').kendoDatePicker();
    $('#kdpEndDate').kendoDatePicker();

    $('#kddlSalesGroup').kendoDropDownList({
        autoBind: false,
        optionLabel: 'Todos Vendedores',
        dataTextField: 'DisplayName',
        dataValueField: 'UserId',
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/people/GetUsersByRoleGroup?portalId=' + portalID + '&roleGroupName=Departamentos'
                }
            }
        },
        select: function (e) {
            var dataItem = this.dataItem(e.item.index());
            my.sRId = dataItem.UserId;
        }
    });
    
    $('#ddlStatuses').kendoDropDownList({
        autoBind: false,
        optionLabel: "Todos Status",
        dataTextField: "StatusTitle",
        dataValueField: "StatusId",
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/statuses/GetStatuses?portalId=' + portalID + '&isDeleted=False'
                }
            }
        },
        dataBound: function (e) {
            if (this.dataSource.view().length > 0) {
                var dropDown = this;
                $.each(dropDown.dataSource.data(), function (i, status) {
                    if (managerRole !== 'True') {
                        if (status.StatusId === 10) {
                            var itemToRemove = dropDown.dataSource.at(i);
                            dropDown.dataSource.remove(itemToRemove);
                        }
                    }
                });
            }
        }
    });

    //$.ajax({
    //    type: 'GET',
    //    url: '/desktopmodules/riw/api/people/getPerson?personId=-1&portalId=' + portalID + '&userId=' + userID
    //}).done(function (data) {
    //    if (data) {
    //        my.createGrid();
    //        my.personId = data.PersonId;
    //    } else {
    //        $.pnotify({
    //            title: 'Erro!',
    //            text: data.Result,
    //            type: 'error',
    //            icon: 'fa fa-times-circle fa-lg',
    //            addclass: "stack-bottomright",
    //            stack: my.stack_bottomright
    //        });
    //    }
    //}).fail(function (jqXHR, textStatus) {
    //    console.log(jqXHR.responseText);
    //});

    if (my.storage) {
        if (amplify.store.sessionStorage(siteURL + '_chkDeleted')) {
            var chkDeleted = amplify.store.sessionStorage(siteURL + '_chkDeleted');
            if (chkDeleted) {
                $('#chkDeleted').prop('checked', true);
                $('#btnIsDeleted').text('Esconder Desativados');
            } else {
                $('#chkDeleted').prop('checked', false);
                $('#btnIsDeleted').text('Mostrar Desativados');
            }
        }
    }

    //my.createGrid = function () {

    my.estimatesTransport = {
        read: {
            url: '/desktopmodules/riw/api/estimates/getEstimates'
        },
        parameterMap: function (data, type) {
            return {
                portalId: portalID,
                userId: userID,
                salesRep: authorized === 1 ? -1 : (my.sRId > 0 ? my.sRId : -1),
                statusId: kendo.parseInt($('#ddlStatuses').data('kendoDropDownList').value()) > 0 ? $('#ddlStatuses').data('kendoDropDownList').value() : -1,
                sDate: $('#kdpStartDate').val().length > 0 ? moment(new Date($('#kdpStartDate').data('kendoDatePicker').value())).format() : '',
                eDate: $('#kdpEndDate').val().length > 0 ? moment(new Date($('#kdpEndDate').data('kendoDatePicker').value())).format() : '',
                filter: my.vm.filter().length ? my.vm.filter() : '""',
                filterField: $('#optionsFields').val(),
                isDeleted: $('#chkDeleted').is(':checked') ? '' : 'false',
                //getAll: (managerRole === 'True'),
                pageIndex: data.page,
                pageSize: data.pageSize,
                orderBy: data.sort[0] ? data.sort[0].field : 'EstimateId',
                orderDesc: data.sort[0] ? data.sort[0].dir : 'DESC'
                //orderBy: my.convertSortingParameters(data.sort)
            };
        }
    };

    my.estimatesData = new kendo.data.DataSource({
        transport: my.estimatesTransport,
        pageSize: 10,
        serverPaging: true,
        serverSorting: true,
        serverFiltering: true,
        sort: { field: "EstimateId", dir: "DESC" },
        schema: {
            model: {
                id: 'EstimateId',
                fields: {
                    ModifiedOnDate: { type: "date", format: "{0:g}" }
                }
            },
            data: 'data',
            total: 'total'
        }
    });

    $('#estimatesGrid').kendoGrid({
        //height: 375,
        dataSource: my.estimatesData,
        change: function () {
            var row = this.select();
            $(row).css({ 'background-color': '#F4691A' });
            var id = row.data("uid");
            my.uId = id;
            var dataItem = this.dataItem(row);
            if (dataItem) {
                //$('#btnEditSelectedEstimate').attr({ 'disabled': false });

                if (authorized > 2) {
                    $('#btnRemoveSelectedEstimate').show();
                } else {
                    $('#btnRemoveSelectedEstimate').hide();
                }

                //if (dataItem.StatusId === 5) {
                //    $('#btnRefundSelectedEstimate').show();
                //} else {
                //    $('#btnRefundSelectedEstimate').hide();
                //}
            }
        },
        toolbar: kendo.template($("#tmplToolbar").html()),
        columns: [
            {
                title: ' ',
                template: "<input type='checkbox' class='checkbox' />",
                width: 35,
                sortable: false
            },
            {
                title: 'ID',
                field: 'EstimateId',
                width: 80
            },
            {
                title: 'Cliente (Razão Social',
                field: 'ClientCompanyName',
                hidden: true
            },
            {
                title: 'Cliente (Nome no Site)',
                field: 'ClientDisplayName'
            },
            {
                title: 'Vendedor',
                field: 'SalesRepName',
                hidden: authorized <= 2
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
                attributes: { class: 'DNNAlignright' },
                headerAttributes: {
                    class: 'DNNAlignright',
                    style: 'text-align: right'
                }
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

                                $("#estimateWindow").append("<div id='window'></div>");
                                var sContent = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#estimateId/' + dataItem.EstimateId + '/personId/' + dataItem.PersonId,
                                    kendoWindow = $('#window').kendoWindow({
                                        actions: ["Maximize", "Close"],
                                        title: 'Orçamento: ' + (dataItem.EstimateTitle ? dataItem.EstimateTitle : '') + ' (ID: ' + dataItem.EstimateId + ') ' + (authorized === 1 ? (dataItem.EstimateTitle ? dataItem.EstimateTitle : '') : dataItem.ClientDisplayName + '(ID: ' + dataItem.PersonId + ')'),
                                        modal: true,
                                        width: '90%',
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
                //title: '',
                width: 45
            }
        ],
        pageable: {
            pageSizes: [20, 40, 60],
            refresh: true,
            numeric: false,
            input: true,
            messages: {
                display: "{0} - {1} de {2} orçamentos",
                empty: "Sem Registro.",
                page: "Página",
                of: "de {0}",
                itemsPerPage: "Orçamentos por página",
                first: "Ir para primeira página",
                previous: "Ir para página anterior",
                next: "Ir para próxima página",
                last: "Ir para última página",
                refresh: "Recarregar"
            }
        },
        //selectable: "row",
        //selectable: false,
        sortable: true,
        reorderable: true,
        resizable: true,
        scrollable: true,
        dataBound: function () {
            var grid = this;

            //$('#btnRefundSelectedEstimate').hide();
            $('#btnRemoveSelectedEstimate').hide();
            //$('#btnEditSelectedPerson').attr({ 'disabled': true });
            if (this.dataSource.view().length > 0) {
                $.each(grid.dataSource.data(), function (i, estimate) {
                    var rowSelector = ">tr:nth-child(" + (i + 1) + ")";
                    var row = grid.tbody.find(rowSelector);

                    if (estimate.IsDeleted) {
                        row.addClass('isDeleted');
                    }

                    if (!estimate.ViewPrice) {
                        estimate.set('TotalAmount', 'Atualizando...');
                    }

                    row.css({ 'background-color': estimate.StatusColor });

                    if ((authorized === 1) && (estimate.StatusId === 5)) row.css({ 'display': 'none' });

                    //if (my.checkedIds[estimate.EstimateId]) {
                    //    //grid.tbody.find("tr[data-uid='" + item.uid + "']")
                    //    row.addClass("k-state-selected")
                    //        .find(".checkbox")
                    //        .attr('checked', 'checked');
                    //}
                });
            } else {
                if (authorized < 2) {
                    var $dialog = $('<div></div>')
                        .html('<p class="confirmDialog">Ainda não requisitou nenhum orçamento. Dirija-se à página de produtos e inicíe seu orçamento.</label>')
                        .dialog({
                            autoOpen: false,
                            open: function () {
                                $(".ui-dialog-title").append('Aten&#231;&#227;o!');
                            },
                            modal: true,
                            resizable: true,
                            dialogClass: 'dnnFormPopup',
                            width: '50%',
                            buttons: {
                                'ok': {
                                    text: 'Ok',
                                    //priority: 'primary',
                                    "class": 'btn btn-primary btn-large',
                                    click: function (e) {
                                        $dialog.dialog('close');
                                        $dialog.dialog('destroy');
                                    }
                                }
                            }
                        });

                    $dialog.dialog('open');
                }
            }

            if (authorized === 1) {
                grid.hideColumn(2);
                //if (!my.vm.viewPrice()) {
                //    grid.hideColumn(3);
                //}
            } else if (authorized === 2) {
                grid.hideColumn(0);
                grid.hideColumn(3);
            }
        },
    });
    //};

    //bind click event to the checkbox
    $('#estimatesGrid').data('kendoGrid').table.on("click", ".checkbox", selectRow);

    my.checkedIds = {};

    //on click of the checkbox:
    function selectRow() {
        var checked = this.checked,
            row = $(this).closest("tr"),
            grid = $('#estimatesGrid').data('kendoGrid'),
            dataItem = grid.dataItem(row);

        if (checked) {
            my.checkedIds[dataItem.id] = {
                EstimateId: dataItem.EstimateId
            };

            //-select the row
            row.addClass("k-state-selected");
            $('#btnRemoveSelectedEstimate').show();
        } else {
            //-remove selection
            row.removeClass("k-state-selected");
            $('#btnRemoveSelectedEstimate').hide();
            delete my.checkedIds[dataItem.id];
        }
    }

    if (my.estimateId > 0) {
        $.ajax({
            url: '/desktopmodules/riw/api/estimates/getEstimate?estimateId=' + my.estimateId + '&portalId=' + portalID + '&userId=' + userID,
        }).done(function (dataItem) {
            if (dataItem) {

                $("#estimateWindow").append("<div id='window'></div>");
                var sContent = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#estimateId/' + dataItem.EstimateId + '/personId/' + dataItem.PersonId,
                    kendoWindow = $('#window').kendoWindow({
                        actions: ["Maximize", "Close"],
                        title: 'Orçamento: ' + (dataItem.EstimateTitle ? dataItem.EstimateTitle : '') + ' (' + dataItem.EstimateId + ') ' + (authorized === 1 ? (dataItem.EstimateTitle ? dataItem.EstimateTitle : '') : dataItem.ClientDisplayName),
                        modal: true,
                        width: '90%',
                        height: '80%',
                        content: sContent,
                        close: function (e) {
                            $("html, body").css("overflow", "");
                            my.estimatesData.read();
                        },
                        open: function () {
                            $("html, body").css("overflow", "hidden");
                            $('#window').html('<div class="k-loading-mask"><span class="k-loading-text">Loading...</span><div class="k-loading-image"/><div class="k-loading-color"/></div>');
                            //my.initEstimatesGrid();
                            history.pushState("", document.title, window.location.pathname);
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
            } else {
                $().toastmessage('showErrorToast', data.Result);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
    //} else {
    //    my.initEstimatesGrid();
    }

    $("#estimatesGrid").delegate("tbody > tr", "dblclick", function () {
        $('#btnEditSelectedEstimate').click();
    });

    //function onSalesPersonSelect(e) {
    //    var dataItem = this.dataItem(e.item.index());
    //    my.sRId = dataItem.UserId;
    //    //my.peopleData.read();
    //}

    //$('#kddlSalesGroup').data("kendoDropDownList").bind("select", onSalesPersonSelect);

    //function onStatusSelect(e) {
    //    var dataItem = this.dataItem(e.item.index());
    //    my.sId = dataItem.StatusId;
    //    //my.peopleData.read();
    //}

    //$('#ddlStatuses').data("kendoDropDownList").bind("select", onStatusSelect);

    //function onEndDateSelect(e) {
    //    if ($('#dpEndDate').val().length === 0) {
    //        $('#dpStartDate').val(null);
    //    }
    //    my.peopleData.read();
    //}

    //$('#dpEndDate').data("kendoDatePicker").bind("change", onEndDateSelect);

    $('#btnSearch').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();
        //my.sTerm = $('#searchTerm').val();
        my.estimatesData.read();
    });

    $('#btnEditSelectedEstimate').click(function (e) {
        e.preventDefault();

        var grid = $('#estimatesGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        $("#estimateWindow").append("<div id='window'></div>");
        var sContent = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#estimateId/' + dataItem.EstimateId + '/personId/' + dataItem.PersonId,
            kendoWindow = $('#window').kendoWindow({
                actions: ["Maximize", "Close"],
                title: 'Orçamento: ' + (dataItem.EstimateTitle ? dataItem.EstimateTitle : '') + ' (ID: ' + dataItem.EstimateId + ') ' + (authorized === 1 ? (dataItem.EstimateTitle ? dataItem.EstimateTitle : '') : dataItem.ClientDisplayName),
                modal: true,
                width: '90%',
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
    });

    $('#btnRemoveSelectedEstimate').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        var grid = $('#estimatesGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        //$(this).html('<i class="fa fa-spinner fa-spin"></i> Um momento...').attr({ 'disabled': true });

        var $dialog = $('<div></div>')
                        .html('<div class="confirmDialog">Tem Certeza que deseja remover este or&#231;amento?</div>')
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
                                        $this.button('loading');

                                        var estimates = [];
                                        $.each(my.checkedIds, function (i, item) {
                                            estimates.push({
                                                EstimateId: kendo.parseInt(i)
                                            });
                                        });

                                        $.ajax({
                                            type: 'DELETE',
                                            contentType: 'application/json; charset=utf-8',
                                            url: '/desktopmodules/riw/api/estimates/removeEstimate',
                                            data: JSON.stringify(estimates)
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                //$().toastmessage('showSuccessToast', 'Conta excluida com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'O or&#231;amento foi desativado.',
                                                    type: 'success',
                                                    icon: 'fa fa-check fa-lg',
                                                    addclass: "stack-bottomright",
                                                    stack: my.stack_bottomright
                                                });
                                                my.estimatesData.read();
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
                                            $dialog.dialog('close');
                                            $dialog.dialog('destroy');
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

    $('#btnRefundSelectedEstimate').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var grid = $('#estimatesGrid').data("kendoGrid");
        var dataItem = grid.dataSource.getByUid(my.uId);

        var params = {
            PortalId: portalID,
            EstimateId: dataItem.EstimateId,
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        }

        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/estimates/restoreEstimate',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                if (data.Estimate) {
                    dataItem.set('StatusTitle', data.Estimate.StatusTitle);
                    //$().toastmessage('showSuccessToast', 'Configuração atualizada com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Or&#231;amento re-inicializado.',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });

                    var row = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']");
                    row.css({ 'background-color': 'transparent' });
                }
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
        }).always(function () {
            $this.button('reset');
        });
    });

    $('#btnRemoveFilter').click(function (e) {
        var btn = this;
        e.preventDefault();
        if (e.clientX === 0) {
            return false;
        }

        $('#ddlStatuses').data('kendoDropDownList').value(null);
        $('#kddlSalesGroup').data('kendoDropDownList').value(null);
        my.vm.filter('');
        //$('#estimateIdTextBox').val(null); control no longer is being used
        $('#optionsFields').val(null);
        $('#ddlStatuses').data('kendoDropDownList').value(null);
        my.sRId = -1;
        $('#kdpStartDate').data('kendoDatePicker').value(null);
        $('#kdpEndDate').data('kendoDatePicker').value(null);

        my.estimatesData.read();
    });

    $('#btnAddEstimate').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var params = {
            PortalId: portalID,
            EstimateId: 0,
            UserId: -1,
            PersonId: parseInt(amplify.store.sessionStorage('mainConsumerId')),
            //Guid: my.estimateId > 0 ? my.estimateId : my.Right(my.generateUUID(), 8),
            SalesRep: kendo.parseInt(authorized === 2 ? userID : parseInt(amplify.store.sessionStorage('salesPerson'))),
            ViewPrice: true,
            TotalAmount: 0,
            CreatedByUser: userID,
            CreatedOnDate: moment().format()
        }

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/estimates/AddEstimate',
            data: params
        }).done(function (estimate) {
            if (estimate.EstimateId) {

                $("#estimateWindow").append("<div id='window'></div>");
                var sContent = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#estimateId/' + estimate.EstimateId + '/personId/' + amplify.store.sessionStorage('mainConsumerId'),
                    kendoWindow = $('#window').kendoWindow({
                        actions: ["Maximize", "Close"],
                        title: 'Orçamento: ID: (' + estimate.EstimateId + ') ' + amplify.store.sessionStorage('mainConsumer'),
                        modal: true,
                        width: '90%',
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
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        }).always(function () {
            $this.button('reset');
        });
    });

    //$('#btnDeleteSelectedEstimate').click(function (e) {
    //    if (e.clientX === 0) {
    //        return false;
    //    }
    //    e.preventDefault();

    //    var $this = $(this);

    //    var grid = $('#estimatesGrid').data("kendoGrid");
    //    var dataItem = grid.dataSource.getByUid(my.uId);

    //    var $dialog = $('<div></div>')
    //                    .html('<div class="confirmDialog">Tem Certeza?</div>')
    //                    .dialog({
    //                        autoOpen: false,
    //                        open: function () {
    //                            $(".ui-dialog-title").append('Aviso de Exclus&#227;o');
    //                        },
    //                        modal: true,
    //                        resizable: false,
    //                        dialogClass: 'dnnFormPopup',
    //                        buttons: {
    //                            'ok': {
    //                                text: 'Sim',
    //                                //priority: 'primary',
    //                                "class": 'btn btn-primary btn-large',
    //                                click: function () {
    //                                    $this.button('loading');

    //                                    $dialog.dialog('close');
    //                                    $dialog.dialog('destroy');

    //                                    var params = {
    //                                        PortalId: portalID,
    //                                        EstimateId: dataItem.EstimateId,
    //                                        IsDeleted: true,
    //                                        ModifiedByUser: userID,
    //                                        ModifiedOnDate: moment().format()
    //                                    };

    //                                    $.ajax({
    //                                        type: 'PUT',
    //                                        url: '/desktopmodules/riw/api/estimates/deleteEstimate',
    //                                        data: params
    //                                    }).done(function (data) {
    //                                        if (data.Result.indexOf("success") !== -1) {
    //                                            //$().toastmessage('showSuccessToast', 'O or&#231;amento foi desativado com sucesso');
    //                                            $.pnotify({
    //                                                title: 'Sucesso!',
    //                                                text: 'O or&#231;amento foi desativado.',
    //                                                type: 'success',
    //                                                icon: 'fa fa-check fa-lg',
    //                                                addclass: "stack-bottomright",
    //                                                stack: my.stack_bottomright
    //                                            });
    //                                            my.estimatesData.read();
    //                                        } else {
    //                                            $.pnotify({
    //                                                title: 'Erro!',
    //                                                text: data.Result,
    //                                                type: 'error',
    //                                                icon: 'fa fa-times-circle fa-lg',
    //                                                addclass: "stack-bottomright",
    //                                                stack: my.stack_bottomright
    //                                            });
    //                                            //$().toastmessage('showErrorToast', data.Result);
    //                                        }
    //                                    }).fail(function (jqXHR, textStatus) {
    //                                        console.log(jqXHR.responseText);
    //                                    }).always(function () {
    //                                        $this.button('reset');
    //                                    });
    //                                }
    //                            },
    //                            'cancel': {
    //                                html: 'N&#227;o',
    //                                //priority: 'secondary',
    //                                "class": 'dnnSecondaryAction',
    //                                click: function () {
    //                                    $dialog.dialog('close');
    //                                    $dialog.dialog('destroy');
    //                                }
    //                            }
    //                        }
    //                    });

    //    $dialog.dialog('open');
    //});

    //$('#btnRestoreSelectedEstimate').click(function (e) {
    //    if (e.clientX === 0) {
    //        return false;
    //    }
    //    e.preventDefault();

    //    var $this = $(this);
    //    $this.button('loading');

    //    var grid = $('#estimatesGrid').data("kendoGrid");
    //    var dataItem = grid.dataSource.getByUid(my.uId);

    //    var params = {
    //        PortalId: portalID,
    //        EstimateId: dataItem.EstimateId,
    //        IsDeleted: false,
    //        ModifiedByUser: userID,
    //        ModifiedOnDate: moment().format()
    //    };

    //    $.ajax({
    //        type: 'PUT',
    //        url: '/desktopmodules/riw/api/estimates/deleteEstimate',
    //        data: params
    //    }).done(function (data) {
    //        if (data.Result.indexOf("success") !== -1) {
    //            //$().toastmessage('showSuccessToast', 'O or&#231;amento foi ativado com sucesso');
    //            $.pnotify({
    //                title: 'Sucesso!',
    //                text: 'O or&#231;amento foi ativado.',
    //                type: 'success',
    //                icon: 'fa fa-check fa-lg',
    //                addclass: "stack-bottomright",
    //                stack: my.stack_bottomright
    //            });
    //            my.estimatesData.read();
    //        } else {
    //            $.pnotify({
    //                title: 'Erro!',
    //                text: data.Result,
    //                type: 'error',
    //                icon: 'fa fa-times-circle fa-lg',
    //                addclass: "stack-bottomright",
    //                stack: my.stack_bottomright
    //            });
    //            //$().toastmessage('showErrorToast', data.Result);
    //        }
    //    }).fail(function (jqXHR, textStatus) {
    //        console.log(jqXHR.responseText);
    //    }).always(function () {
    //        $this.button('reset');
    //    });
    //});

});
