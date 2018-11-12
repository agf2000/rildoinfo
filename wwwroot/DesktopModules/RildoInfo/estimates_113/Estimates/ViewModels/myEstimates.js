
$(function () {

    my.uId = 0;
    my.personId = 0;
    my.hasFocus = true;
    my.estimateId = my.getQuerystring('estimateId', my.getParameterByName('estimateId'));
    my.new = my.getTopParameterByName('new') || my.getQuerystring('new');

    my.viewModel();

    //$('#ask_permission').popover({
    //    delay: {
    //        show: 500,
    //        hide: 100
    //    },
    //    placement: 'top',
    //    trigger: 'hover'
    //});

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
    
    window.onfocus = window.onblur = window.onpageshow = window.onpagehide = function (e) {
        if (e.type === 'focus') {
            //console.log("visible");
            my.hasFocus = true;
        } else {
            //console.log("hidden");
            my.hasFocus = false;
        }
    };

    if (managerRole === 'False') $('#estimatesGrid li:nth-child(4)').hide();

    my.estimatesTransport = {
        read: {
            url: '/desktopmodules/riw/api/estimates/getEstimates'
        },
        parameterMap: function (data, type) {
            return {
                portalId: portalID,
                userId: userID,
                salesRep: authorized === 1 ? -1 : (my.sRId > 0 ? my.sRId : -1),
                statusId: my.sId > 0 ? my.sId : -1,
                sDate: '', // $('#dpStartDate').data('kendoDatePicker').value(),
                eDate: '', // $('#dpEndDate').data('kendoDatePicker').value(),
                filter: my.vm.filter().length ? my.vm.filter() : '""',
                isDeleted: 'false', // $('#chkDeleted').is(':checked') ? '' : 'false',
                pageIndex: data.page,
                pageSize: data.pageSize,
                orderBy: data.sort[0] ? data.sort[0].field : 'EstimateId',
                orderDesc: data.sort[0] ? data.sort[0].dir : 'DESC'
            };
        }
    };

    my.estimatesData = new kendo.data.DataSource({
        transport: my.estimatesTransport,
        pageSize: 50,
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
        //change: function () {
        //    //var row = this.select();
        //    //$(row).css({ 'background-color': '#F4691A' });
        //    //var id = row.data("uid");
        //    //my.uId = id;
        //    //var dataItem = this.dataItem(row);
        //    //if (dataItem) {
        //    //    $('#btnEditSelectedEstimate').attr({ 'disabled': false });

        //    //    if (authorized > 2) {
        //    //        $('#btnDeleteSelectedEstimate').show();
        //    //        $('#btnRemoveSelectedEstimate').show();
        //    //    } else {
        //    //        $('#btnDeleteSelectedEstimate').hide();
        //    //        $('#btnRemoveSelectedEstimate').hide();
        //    //    }

        //    //    if (dataItem.IsDeleted) {
        //    //        $('#btnRestoreSelectedEstimate').show();
        //    //        $('#btnDeleteSelectedEstimate').hide();
        //    //    } else {
        //    //        $('#btnRestoreSelectedEstimate').hide();
        //    //    }
        //    //}
        //},
        toolbar: kendo.template($("#tmplToolbar").html()),
        columns: [
            {
                title: 'ID',
                field: 'EstimateId',
                width: 80
            },
            //{
            //    title: 'Cliente',
            //    field: 'ClientDisplayName'
            //},
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

                                $("#estimateWindow").append("<div id='window'></div>");
                                var sContent = clientEstimateURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#estimateId/' + dataItem.EstimateId + '/personId/' + dataItem.PersonId,
                                    kendoWindow = $('#window').kendoWindow({
                                        actions: ["Refresh", "Maximize", "Close"],
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

                            }
                        }
                    }
                ],
                title: 'Abrir',
                width: 55
            }
        ],
        //selectable: "row",
        //pageable: {
        //    pageSizes: [20, 40, 60],
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
        sortable: true,
        reorderable: true,
        resizable: true,
        scrollable: true,
        dataBound: function () {
            var grid = this;

            $('#btnRestoreSelectedEstimate').hide();
            $('#btnDeleteSelectedEstimate').hide();
            $('#btnRemoveSelectedEstimate').hide();
            if (this.dataSource.view().length > 0) {
                $.each(grid.dataSource.data(), function (i, estimate) {
                    var rowSelector = ">tr:nth-child(" + (i + 1) + ")";
                    var row = grid.tbody.find(rowSelector);
                    //if (estimate.IsDeleted) {
                    //    row.addClass('isDeleted');
                    //}
                    if (!estimate.ViewPrice) {
                        estimate.set('TotalAmount', 'Atualizando...');
                    }
                    //row.css({ 'background-color': estimate.StatusColor });
                    if ((authorized === 1) && (estimate.StatusId === 5)) row.css({ 'display': 'none' });
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
                                    "class": 'btn btn-primary',
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
            //if (authorized === 1) {
            //    grid.hideColumn(1);
            //    //if (!my.vm.viewPrice()) {
            //    //    grid.hideColumn(3);
            //    //}
            //} else if (authorized === 2) {
            //    grid.hideColumn(2);
            //}
        },
    });
    //};

    if (my.estimateId > 0) {
        $.ajax({
            url: '/desktopmodules/riw/api/estimates/getEstimate?estimateId=' + my.estimateId + '&portalId=' + portalID + '&userId=' + userID,
        }).done(function (data) {
            if (data.Result.indexOf('success') !== -1) {

                $("#estimateWindow").append("<div id='window'></div>");
                var sContent = clientEstimateURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#estimateId/' + data.Estimate.EstimateId + '/personId/' + data.Estimate.PersonId,
                    kendoWindow = $('#window').kendoWindow({
                        actions: ["Maximize", "Close"],
                        title: 'Orçamento: ' + (data.Estimate.EstimateTitle ? data.Estimate.EstimateTitle : '') + ' (' + data.Estimate.EstimateId + ') ' + (authorized === 1 ? (data.Estimate.EstimateTitle ? data.Estimate.EstimateTitle : '') : data.Estimate.ClientDisplayName),
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
                //$().toastmessage('showErrorToast', data.Result);
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
    //} else {
    //    my.initEstimatesGrid();
    }

    //$("#estimatesGrid").delegate("tbody > tr", "dblclick", function () {
    //    $('#btnEditSelectedEstimate').click();
    //});

});
