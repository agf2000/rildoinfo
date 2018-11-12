
$(function () {

    my.personId = my.getParameterByName('personId');

    my.status = $(".status");

    my.viewModel();

    my.hub = $.connection.peopleHub;

    $("#actionsMenu").jqxMenu();

    //$('#divClientHistory').dnnPanels();

    //$('.tooltip').tooltipster({
    //    icon: '<img alt="Ajuda" src="/icons/sigma/Help_32x32_Standard.png" />',
    //    iconDesktop: true,
    //    theme: '.tooltipster-blue',
    //    position: 'left'
    //});

    my.hub.client.pushHistoryComment = function (item, index) {
        my.vm.filteredHistories()[index].historyComments.unshift(new my.HistoryComment(item));
        my.sendNotification('/desktopmodules/rildoinfo/webapi/content/images/ri-logo.png', siteName, 'O website ' + siteURL + ' foi atualizado.', 6000, !my.hasFocus);
    };

    my.hub.client.pushHistory = function (item) {
        my.vm.personHistories.unshift(new my.PersonHistory(item));
        my.sendNotification('/desktopmodules/rildoinfo/webapi/content/images/ri-logo.png', siteName, 'O website ' + siteURL + ' foi atualizado.', 6000, !my.hasFocus);
    };

    $.connection.hub.start().done(function () {
        my.hub.server.clientsJoin(portalID.toString() + '_' + my.personId.toString());
    });

    my.getPerson = function () {
        if (my.personId !== 0) {
            $.ajax({
                url: '/desktopmodules/riw/api/people/GetPerson?personId=' + my.personId,
                async: false
            }).done(function (data) {
                if (data) {
                    $('#actionsMenu').show();

                    my.vm.personType(data.PersonType);
                    my.vm.personId(data.PersonId);
                    my.vm.personUserId(data.UserId);
                    my.vm.displayName(data.DisplayName);
                    my.vm.firstName(data.FirstName);
                    my.vm.lastName(data.LastName);
                    my.vm.cpf(data.Cpf);
                    my.vm.ident(data.Ident);
                    my.vm.telephone(data.Telephone);
                    my.vm.cell(data.Cell);
                    my.vm.fax(data.Fax);
                    my.vm.originalEmail(data.Email);
                    if (data.Email.length > 2) {
                        my.vm.email(data.Email);
                        $('#actionsMenu li:nth-child(5)').show();
                    }
                    my.vm.companyName(data.CompanyName);
                    my.vm.zero800(data.Zero800s);
                    my.vm.dateFound(data.DateFound);
                    my.vm.dateRegistered(data.DateRegistered);
                    my.vm.statusId(data.StatusId);
                    my.vm.ein(data.Cnpj);
                    my.vm.stateTax(data.StateTax);
                    my.vm.cityTax(data.CityTax);
                    my.vm.comments(data.Comments);
                    my.vm.bio(data.Biography);
                    my.vm.locked(data.Locked);
                    if (data.MonthlyIncome > 0) my.vm.income(data.MonthlyIncome);
                    my.vm.selectedFinanAddress(data.PersonAddressId);
                    my.vm.website(data.Website);
                    my.vm.selectedSalesRepId(data.SalesRep);
                    my.vm.createdByUser(data.CreatedByUser);
                    my.vm.createdOnDate(data.CreatedOnDate);

                    $.getJSON('/desktopmodules/riw/api/people/getHistory?personId=' + my.personId, function (data) {
                        if (data.length > 0) {
                            my.vm.personHistories.removeAll();
                            $.each(data, function (i, item) {
                                my.vm.personHistories.unshift(new my.PersonHistory(item));
                            });
                            // $('#history').html('');
                            //data.sort(my.vm.sortJsonName('HistoryId', false, function (a) { return a.toUpperCase() }));
                            //my.vm.personHistories.removeAll();
                            //for (var i = 0; i < data.length; i++) {
                            //    // $('#history').append('<span>' + data[i].HistoryText + '</span><br />')
                            //    my.vm.personHistories.unshift(new my.PersonHistory().historyId(data[i].HistoryId).historyText(data[i].HistoryText));
                            //}
                        }
                    });
                }
            });
        }
    };
    my.getPerson();

    $('#ddlStatuses').kendoDropDownList({
        dataTextField: 'StatusTitle',
        dataValueField: 'StatusId',
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/statuses/GetStatuses?portalId=' + portalID + '&isDeleted=false'
                }
            }
        },
        dataBound: function (e) {
            var $this = this;
            setTimeout(function () {
                $this.value(my.vm.statusId());
            }, 100);
        }
    });

    if (my.getParameterByName('sel')) {
        var sel = my.getParameterByName('sel');
        $('#actionsMenu ul li[id=' + sel + ']').addClass('jqx-menu-item-selected');
        if (sel <= 7) {
            $('#personMenu ul li:first-child').addClass('jqx-menu-item-selected');
            //} else {
        }
    }

    $('#btnAddHistory').click(function (e) {
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var historyHtmlContent = my.converter.makeHtml($('#historyTextarea').val().trim());

        var params = {
            PersonId: my.personId,
            StatusId: $('#ddlStatuses').data('kendoDropDownList').value(),
            HistoryText: historyHtmlContent,
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format(),
            ConnId: my.hub.connection.id
        };
        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/people/UpdatePersonStatus',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $('.markdown-editor').val('');
                //$.getJSON('/desktopmodules/riw/api/clients/GetClientHistory?cId=' + my.vm.personId(), function (data) {
                //    if (data.length > 0) {
                //        // $('#history').html('');
                //        my.vm.clientHistories.removeAll();
                //        for (var i = 0; i < data.length; i++) {
                //            // $('#history').append('<span>' + data[i].HistoryText + '</span><br />')
                //            my.vm.clientHistories.unshift(new my.ClientHistory().historyId(data[i].HistoryId).historyText(data[i].HistoryText));
                //        }
                //    }
                //});
                //my.vm.personHistories.unshift(new my.PersonHistory().historyId(0).historyText(params.HistoryText));
                params.Avatar = amplify.store.sessionStorage('avatar'); // ? '/portals/0/' + _avatar + '?w=45&h=45&mode=crop' : '/desktopmodules/rildoinfo/webapi/content/images/user.png?w=45&h=45';
                params.DisplayName = displayName;
                my.vm.personHistories.unshift(new my.PersonHistory(params));
                //$().toastmessage('showSuccessToast', 'Hist&#243;rico atualizado com sucesso.');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Hist&#243;rico atualizado.',
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

    $('.markdown-editor').css({ 'min-width': '90%', 'height': '80px', 'margin-bottom': '5px' }).attr({ 'cols': '30', 'rows': '2' });

    $('.markdown-editor').autogrow();
    $('.markdown-editor').css('overflow', 'hidden').autogrow();

    $('.togglePreview').click(function (e) {
        e.preventDefault();
        var $this = $(this);

        var ele = $(this).data('provider');

        var $dialog = $('<div></div>')
            .html(my.converter.makeHtml($('#' + ele).val().trim()))
            .dialog({
                autoOpen: false,
                open: function () {
                    $(".ui-dialog-title").append('Texto');
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

    $('#actionsMenu').on('itemclick', function (event) {
        //event.args.textContent = 'Um momento...';
        switch (event.args.id) {
            case '7':
                document.location.href = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/7';
                break;
            case '8':
                document.location.href = historyURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/8';
                break;
            case '9':
                document.location.href = finanURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/9';
                break;
            case '10':
                document.location.href = assistURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/10';
                break;
            case '11':
                document.location.href = commURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/11';
                break;
            default:
                var urlAddress = '';
                if (my.retSel > 0) {
                    switch (my.retSel) {
                        case 7:
                            urlAddress = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/7';
                            break;
                        case 8:
                            urlAddress = historyURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/8';
                            break;
                        case 9:
                            urlAddress = finanURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/9';
                            break;
                        case 10:
                            urlAddress = assistURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/10';
                            break;
                        case 11:
                            urlAddress = commURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/11';
                            break;
                    }
                    document.location.href = urlAddress;
                } else {
                    //parent.$(".k-window-content").each(function (i, win) {
                    parent.$('#window').data("kendoWindow").close();
                    //});
                    //$(".k-window .k-window-content").each(function (index, element) {
                    //    setTimeout(function () {
                    //        $(element).data('kendoWindow').close();
                    //    }, 1000);
                    //});
                    //if (parent.$('#clientFinanWindow').data('kendoWindow')) {
                    //    parent.$('#clientFinanWindow').data('kendoWindow').close();
                    //}
                    //if (parent.$('#clientEditWindow').data('kendoWindow')) {
                    //    parent.$('#clientEditWindow').data('kendoWindow').close();
                    //}
                    //if (parent.$('#clientAssistWindow').data('kendoWindow')) {
                    //    parent.$('#clientAssistWindow').data('kendoWindow').close();
                    //}
                    //if (parent.$('#clientCommWindow').data('kendoWindow')) {
                    //    parent.$('#clientCommWindow').data('kendoWindow').close();
                    //}
                    //if (parent.$('#clientHistoryWindow').data('kendoWindow')) {
                    //    parent.$('#clientHistoryWindow').data('kendoWindow').close();
                    //}
                }
        }
    });

    $('.btnReturn').click(function (e) {
        e.preventDefault();

        $(this).html('<i class="fa fa-spinner fa-spin"></i>Um momento...').attr({ 'disabled': true });
        var urlAddress = '';
        if (my.retSel > 0) {
            switch (my.retSel) {
                case 7:
                    urlAddress = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/7';
                    break;
                case 8:
                    urlAddress = historyURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/8';
                    break;
                case 9:
                    urlAddress = finanURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/9';
                    break;
                case 10:
                    urlAddress = assistURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/10';
                    break;
                case 11:
                    urlAddress = commURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/11';
                    break;
            }
            document.location.href = urlAddress;
        } else {
            //parent.$(".k-window-content").each(function (i, win) {
            parent.$('#window').data("kendoWindow").close();
            //});
            //$(".k-window .k-window-content").each(function (index, element) {
            //    setTimeout(function () {
            //        $(element).data('kendoWindow').close();
            //    }, 1000);
            //});
            //if (parent.$('#clientFinanWindow').data('kendoWindow')) {
            //    parent.$('#clientFinanWindow').data('kendoWindow').close();
            //}
            //if (parent.$('#clientEditWindow').data('kendoWindow')) {
            //    parent.$('#clientEditWindow').data('kendoWindow').close();
            //}
            //if (parent.$('#clientAssistWindow').data('kendoWindow')) {
            //    parent.$('#clientAssistWindow').data('kendoWindow').close();
            //}
            //if (parent.$('#clientCommWindow').data('kendoWindow')) {
            //    parent.$('#clientCommWindow').data('kendoWindow').close();
            //}
            //if (parent.$('#clientHistoryWindow').data('kendoWindow')) {
            //    parent.$('#clientHistoryWindow').data('kendoWindow').close();
            //}
        }
    });

});