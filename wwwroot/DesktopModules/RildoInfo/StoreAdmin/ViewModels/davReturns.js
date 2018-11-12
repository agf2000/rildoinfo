
$(function () {

    my.viewModel();

    $('#moduleTitleSkinObject').html(moduleTitle);

    my.davsTransport = {
        read: {
            url: '/desktopmodules/riw/api/estimates/GetDavs'
        },
        parameterMap: function (data, type) {
            return {
                portalId: portalID,
                userId: userID,
                salesRep: -1, // authorized === 1 ? -1 : (my.sRId > 0 ? my.sRId : -1),
                statusId: 10, // my.sId > 0 ? my.sId : -1,
                sDate: '', // $('#kdpStartDate').val().length > 0 ? moment(new Date($('#kdpStartDate').data('kendoDatePicker').value())).format() : '',
                eDate: '', // $('#kdpEndDate').val().length > 0 ? moment(new Date($('#kdpEndDate').data('kendoDatePicker').value())).format() : '',
                filter: '""', // my.vm.filter().length ? my.vm.filter() : '""',
                filterField: '""', // $('#optionsFields').val(),
                getAll: 'False', // (managerRole === 'True'),
                pageIndex: data.page,
                pageSize: data.pageSize,
                orderBy: data.sort[0] ? data.sort[0].field : 'EstimateId',
                orderDesc: data.sort[0] ? data.sort[0].dir : 'DESC'
                //orderBy: my.convertSortingParameters(data.sort)
            };
        }
    };

    my.davsData = new kendo.data.DataSource({
        transport: my.davsTransport,
        pageSize: 10,
        serverPaging: true,
        serverSorting: true,
        serverFiltering: true,
        sort: { field: "EstimateId", dir: "DESC" },
        schema: {
            model: {
                id: 'EstimateId',
                fields: {
                    SaleDate: { type: "date", format: "{0:g}" }
                }
            },
            data: 'data',
            total: 'total'
        }
    });

    $('#davsGrid').kendoGrid({
        dataSource: my.davsData,
        toolbar: kendo.template($("#tmplToolbar").html()),
        columns: [
            {
                title: 'ORC',
                field: 'EstimateId',
                width: 60
            },
            {
                title: 'Num DAV',
                field: 'SequenciaDav',
                width: 80
            },
            {
                title: 'Cupom',
                template: '#= Coupon > 0 ? my.zeroPad(Coupon, 6) : "" #',
                width: 80
            },
            {
                title: 'Cliente',
                template: '#= ClientDisplayName #'
            },
            {
                title: 'Valor',
                template: '#= kendo.toString(TotalAmount, "c") #',
                width: 100
            },
            {
                title: 'Vendedor',
                field: 'SalesRepName'
            },
            {
                title: 'Status',
                field: 'StatusTitle',
                width: 120,
                hidden: true
            },
            {
                title: 'Finalizador',
                field: 'PayTypes',
                width: 80,
                attributes: { class: 'text-center' }
            },
            {
                title: 'Data',
                field: 'SaleDate',
                format: '{0:g}',
                //template: '#= kendo.toString(ModifiedOnDate, "g") #',
                width: 120
            },
            {
                title: 'Cancelado',
                field: 'Canceled',
                width: 80,
                template: '#= Canceled ? "Sim" : "Não" #',
                attributes: { class: 'text-center' }
            },
            {
                command: [
                    {
                        name: "download",
                        text: " ",
                        imageClass: "icon icon-download",
                        click: function (e) {
                            e.preventDefault();

                            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                            if (dataItem) {

                                $.ajax({
                                    type: 'POST',
                                    url: '/desktopmodules/riw/api/estimates/saveDav?estimateId=' + dataItem.EstimateId
                                }).done(function (data) {
                                    if (data.Result.indexOf("success") !== -1) {
                                        $.pnotify({
                                            title: 'Sucesso!',
                                            text: 'DAV salvo.',
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
                                    //}).always(function () {
                                    //    $this.button('reset');
                                });

                                //var kendoWindow = $("<div />").kendoWindow({
                                //    title: "Baixar DAV",
                                //    resizable: false,
                                //    modal: true,
                                //    width: 550
                                //});

                                //kendoWindow.data("kendoWindow")
                                //    .content('<h3 class="DNNAligncenter">DAV ' + dataItem.EstimateId.toString() +
                                //             '</h3><h5 class="DNNAligncenter"><a href="/portals/0/davs/DV' +
                                //             my.padLeft(dataItem.EstimateId.toString(), 6) + '.txt">' +
                                //             dataItem.EstimateId.toString() +
                                //             '.txt</a></h5><h6 class="DNNAligncenter">Clique com o bot&#227;o da direita do mouse no link e escolha a op&#231;&#227;o "salvar link".</h6>')
                                //    .center().open();
                            }
                        }
                    },
                    {
                        name: "open",
                        text: " ",
                        imageClass: "icon icon-print",
                        click: function (e) {
                            e.preventDefault();

                            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                            if (dataItem) {

                                var newWindow = window.open('', '_blank');
                                var locationStr = 'http://' + siteURL + '/portals/0/downloads/recibo_' + dataItem.EstimateId + '.html'; // + '_' + moment().format('DD-MM-YYYY HH').replace(':', '-').replace(' ', '_') + '.html';

                                $.ajax({
                                    type: 'POST',
                                    url: '/desktopmodules/riw/api/estimates/DownloadEstimateHtmlReceipt',
                                    data: {
                                        PortalId: portalID,
                                        EstimateId: dataItem.EstimateId
                                    },
                                    cache: false
                                }).done(function (data) {
                                    if (data.Result.indexOf("success") !== -1) {
                                        setTimeout(function () {
                                            newWindow.location = locationStr;
                                        }, 1000);
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
                                    $.pnotify({
                                        title: 'Erro!',
                                        text: data.Result,
                                        type: 'error',
                                        icon: 'fa fa-times-circle fa-lg',
                                        addclass: "stack-bottomright",
                                        stack: my.stack_bottomright
                                    });
                                });

                                //var newWindow = window.open('', '_blank');

                                //$.ajax({
                                //    type: 'POST',
                                //    url: '/desktopmodules/riw/api/estimates/downloadEstimateTxtReceipt',
                                //    data: {
                                //        PortalId: portalID,
                                //        EstimateId: dataItem.EstimateId,
                                //        PersonId: dataItem.PersonId,
                                //        TxtHeader: '[DATA][BR][BR][EMPRESA] / [SITE][BR][RUA] [NUM] Bairro: [BAIRRO] / [CIDADE] / [ESTADO][BR]',
                                //        TxtSubHeader: 'Telefone: [FONE][BR]Email: [EMAIL][BR][BR]ORCAMENTO n [ORC][BR][BR]',
                                //        TxtClientInfo: 'Cliente: [NOME][BR][RUA] [NUM][BR]Bairro: [BAIRRO][BR][CIDADE] / [ESTADO][BR]CEP: [CEP] / Telefone: [TELEFONE][BR][BR]',
                                //        TxtColumnHeader: 'ITEM   DESCRICAO                   CODIGO[BR]VL.UN      QTD   UN   DESC     TOTAL[BR]------------------------------------------------[BR]',
                                //        TxtItemName: '[ITEMNAME:28]',
                                //        TxtItemRef: '[ITEMREF:01]',
                                //        TxtItemPrice: '[ITEMPRICE:11]',
                                //        TxtItemUni: '[ITEMUNI:05]',
                                //        TxtItemQty: '[ITEMQTY:06]',
                                //        TxtItemDisc: '[ITEMDISC:09]',
                                //        TxtDiscount: '[15:DISCOUNT:12]',
                                //        TxtSubTotal: '[15:SUBTOTAL:12]',
                                //        TxtTotal: '[15:TOTAL:12]',
                                //        TxtBankIn: dataItem.CreditAmount,
                                //        TxtCheckIn: dataItem.ChequeAmount,
                                //        TxtCardIn: dataItem.CardAmount,
                                //        TxtCashIn: dataItem.CashAmount,
                                //        TxtCheck: '[Cheque:18]',
                                //        TxtConditionColumnHeader: 'Forma             Qde P.    Valor Parcela.[BR]Ent.         Jur. AM   Total         Inter. Dias[BR]------------------------------------------------',
                                //        TxtPayQty: '[PAYQTY:10]',
                                //        TxtPayments: '[PAYMENTS:14]',
                                //        TxtInitialPay: '[INITIALPAY:13]',
                                //        TxtInterest: '[INTEREST:10]',
                                //        TxtTotalPays: '[TOTALPAYS:14]',
                                //    },
                                //    cache: false
                                //}).done(function (data) {
                                //    if (data.Result.indexOf("success") !== -1) {
                                //        setTimeout(function () {
                                //            newWindow.location = 'http://' + siteURL + '/portals/0/downloads/recibo_' + dataItem.EstimateId + '_' + moment().format('DD-MM-YYYY HH').replace(':', '-').replace(' ', '_') + '.txt';
                                //        }, 1000);
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

                            }
                        }
                    }
                ],
                width: 90
            }
        ],
        sortable: true,
        reorderable: true,
        resizable: true,
        scrollable: true,
        pageable: {
            pageSizes: [20, 40, 60],
            refresh: true,
            numeric: false,
            input: true,
            messages: {
                display: "{0} - {1} de {2} DAVs",
                empty: "Sem Registro.",
                page: "Página",
                of: "de {0}",
                itemsPerPage: "DAVs por página",
                first: "Ir para primeira página",
                previous: "Ir para página anterior",
                next: "Ir para próxima página",
                last: "Ir para última página",
                refresh: "Recarregar"
            }
        },
        dataBound: function (e) {
            var grid = this;

            if (this.dataSource.view().length > 0) {
                $.each(grid.dataSource.data(), function (i, estimate) {
                    var dataItem = grid.dataSource.view()[i];

                    var payType = '';

                    if (dataItem.CashAmount) {
                        if (payType !== '') {
                            payType += ', DIN'
                        } else {
                            payType += 'DIN'
                        }
                    }

                    if (dataItem.CheckAmount) {
                        if (payType !== '') {
                            payType += ', CHK'
                        } else {
                            payType += 'CHK'
                        }
                    }

                    if (dataItem.CardAmount) {
                        if (payType !== '') {
                            payType += ', CAR'
                        } else {
                            payType += 'CAR'
                        }
                    }

                    if (dataItem.DebitAmount) {
                        if (payType !== '') {
                            payType += ', DEB'
                        } else {
                            payType += 'DEB'
                        }
                    }

                    if (dataItem.BankAmount) {
                        if (payType !== '') {
                            payType += ', BOL'
                        } else {
                            payType += 'BOL'
                        }
                    }

                    dataItem.set('PayTypes', payType)
                })
            }
        },
        editable: false
    });

    // search for product and fill grid from new datasource
    $('#btnSync').click(function (e) {
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
            url: '/desktopmodules/riw/api/estimates/SyncEstimates?sDate=' + moment(new Date(2015, 5, 1)).format('YYYY-MM-DD') + '&eDate=' + moment(new Date()).add(1, 'days').format('YYYY-MM-DD')
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
                $('#SyncMsg').html('<strong>' + data.Updated.toString() + '</strong> Atualizados.')
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

    $('#files').kendoUpload({
        async: {
            saveUrl: '/desktopmodules/riw/api/store/PostDav',
            removeUrl: "remove",
            autoUpload: true
        },
        multiple: false,
        //showFileList: false,
        localization: {
            cancel: 'Cancelar',
            dropFilesHere: 'Arraste o arquivo aqui para envia-lo',
            remove: 'Remover',
            select: 'Selecionar Arquivo',
            statusUploading: 'Enviando Arquivo',
            uploadSelectedFiles: 'Enviar',
            headerStatusUploaded: 'Completo',
            //headerStatusUploading: "customHeaderStatusUploading",
            retry: "Tente Novamente",
            statusFailed: "Falha no Envio",
            statusUploaded: "statusUploaded"
        },
        select: function (e) {
            $.each(e.files, function (index, value) {
                if (value.extension.toUpperCase() !== '.XML') {
                    e.preventDefault();
                    $.pnotify({
                        title: 'Erro!',
                        text: '&#201; permitido enviar somente arquivos com formato xml.',
                        type: 'error',
                        icon: 'fa fa-times-circle fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                }
            });
            setTimeout(function () {
                $('.k-upload').css({ 'max-width': '80%' });
            });
        },
        upload: function (e) {
            e.data = {
                PortalId: portalID,
                FolderPath: 'Davs',
                ModifiedByUser: userID,
                ModifiedOnDate: moment().format()
            };
        },
        success: function (e) {
            $('#davsGrid').data('kendoGrid').read();
            $.pnotify({
                title: 'Sucesso!',
                text: 'Arquivo enviado.',
                type: 'success',
                icon: 'fa fa-check fa-lg',
                addclass: "stack-bottomright",
                stack: my.stack_bottomright
            });
            $('#davsGrid').focus();
        },
        remove: function (e) {
            //$('.k-upload-button').show();
        },
        error: function (e) {
            $.pnotify({
                title: 'Erro!',
                text: 'N&#227;o foi poss&#237;vel o envio do arquivo.',
                type: 'error',
                icon: 'fa fa-times-circle fa-lg',
                addclass: "stack-bottomright",
                stack: my.stack_bottomright
            });
        }
    });

});
