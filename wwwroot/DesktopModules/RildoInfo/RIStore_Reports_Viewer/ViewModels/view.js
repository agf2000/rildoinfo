$(function () {

    my.viewModel();

    $('#kddlReports').kendoDropDownList({
        autoBind: false,
        optionLabel: " Selecionar ",
        dataTextField: "ReportName",
        datValueField: "ReportId",
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/rildoinfo/api/reports/Reports?portalId=' + _portalID
                }
            }
        }
    });

    $('#kddlSalesPerson').kendoDropDownList({
        autoBind: false,
        optionLabel: "Todos Vendedores",
        dataTextField: 'DisplayName',
        dataValueField: 'UserId',
        //select: my.clients.read(),
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/rildoinfo/api/ristore/GetUsersByRoleGroup?portalId=' + _portalID + '&roleGroupName=Departamentos'
                }
            }
        }
    });

    $('#clientSearchBox').kendoAutoComplete({
        minLength: 4,
        highlightFirst: true,
        dataTextField: 'DisplayName',
        placeholder: 'Insira Telefone, Nome, Empresa, CFP ou CNPJ.',
        template: '<strong>Cliente: </strong><span>${ data.DisplayName }</span><br /><strong>Email: </strong><span>${ data.Email }</span><br /><strong>Telefone: </strong><span>${ data.Telephone }</span>',
        dataSource: {
            transport: {
                read: '/desktopmodules/rildoinfo/api/clients/GetClients?portalid=' + _portalID + '&isDeleted=False'
            },
            serverFiltering: true
        },
        filter: "contains",
        dataBound: function () {
            switch (true) {
                case (this.dataSource.total() === 0):
                    if (!$('.toast-item-wrapper').length) $().toastmessage('showWarningToast', 'Sua busca não trouxe resultado algum.');
                    break;
                case (this.dataSource.total() > 1):
                    if (!$('.toast-item-wrapper').length) $().toastmessage('showNoticeToast', this.dataSource.total() + ' clientes encontrados.');
                    break;
                case (this.dataSource.total() > 20):
                    if (!$('.toast-item-wrapper').length) $().toastmessage('showNoticeToast', 'Dezenas de clientes encontrados... refina sua busca');
                    break;
            }
        },
        select: function (e) {
            e.preventDefault();
            var dataItem = this.dataItem(e.item.index());
            if (dataItem) {
                my.vm.clientId(dataItem.ClientId);
                my.vm.displayName(dataItem.DisplayName);
                my.vm.telephone(my.formatPhone(dataItem.Telephone));
                my.vm.email(dataItem.Email);
                my.vm.street(dataItem.Street);
                my.vm.unit(dataItem.Unit);
                my.vm.complement(dataItem.Complement);
                my.vm.district(dataItem.District);
                my.vm.city(dataItem.City);
                my.vm.region(dataItem.Region);
                this.value('');

                if (dataItem.UserId > 0) {
                    $('[data-bind*="clientId"]').css({ color: 'green' });
                    my.vm.clientUserId(dataItem.UserId);
                    my.vm.hasLogin(true);
                } else {
                    $('[data-bind*="clientId"]').css({ color: 'blue' });
                }

                $("#clientsSearchBox").data('kendoAutoComplete').close();
                $('#clientInfo').fadeIn();
                //if (!$('#clientArea').is(':hidden')) $('#clientArea').delay(3000).kendoAnimate({ effects: "slide:up fade:out", hide: true });
                setTimeout(function () {
                    $(my.itemsSearchBox).focus();
                }, 100);

                if (my.vm.estimateId() > 0) {
                    $.post('/desktopmodules/rildoinfo/api/estimate/UpdateClient', { eId: my.vm.estimateId(), cid: my.vm.clientId(), uId: _userID, md: kendo.toString(new Date(), 'g') }, function (data) {
                        if (data.Result.indexOf("success") !== -1) {
                            document.location.hash = 'eid/' + my.vm.estimateId() + '/cid/' + my.vm.clientId();
                        }
                    });
                }
            }
        }
    });

    $('#kddlStatuses').kendoDropDownList({
        autoBind: false,
        optionLabel: "Todos Status",
        dataTextField: "StatusTitle",
        datValueField: "StatusId",
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/rildoinfo/api/statuses/GetStatuses?portalId=' + _portalID + '&isDeleted=False'
                }
            }
        }
    });

    $('#kdpStartDate').kendoDatePicker();

    $('#kdpEndDate').kendoDatePicker();

    my.reportsTransport = {
        read: {
            url: '/desktopmodules/rildoinfo/api/reports/RunReport'
        },
        parameterMap: function (data, type) {
            return {
                portalId: _portalID,
                reportId: $('#kddlReports').val().length ? $('#kddlReports').data('kendoDropDownList').value() : 1,
                date1: '11/09/2013',
                date2: '11/09/2013'
            };
        }
    };

    my.reportsData = new kendo.data.DataSource({
        transport: my.reportsTransport
    });
    
    $('#reportGrid').kendoGrid({
        dataSource: my.reportsData
    });

    $('#btnFilter').click(function (e) {
        e.preventDefault();

        my.reportsData.read();
    });

});