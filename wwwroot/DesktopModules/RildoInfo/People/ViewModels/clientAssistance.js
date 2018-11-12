
$(function () {

    my.personId = my.getParameterByName('personId');
    my.dueDates = [];
    my.status = $(".status");
    my.selectedPropagandaId = my.getParameterByName('pId');
    my.selectedTab = my.getParameterByName('tabIndex');
    my.retSubSel = my.getParameterByName('retSubSel');
    my.retSel = my.getParameterByName('retSel');

    my.viewModel();

    my.hub = $.connection.peopleHub;

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

    // check for browser storage availability
    if (my.selectedTab === 0) {
        if (my.storage) {
            if (amplify.store.sessionStorage(siteURL + '_selectedTab')) my.selectedTab = amplify.store.sessionStorage(siteURL + '_selectedTab');
        }
    }

    $("#actionsMenu").jqxMenu();

    $('#divMoreOptionsTabs').kendoTabStrip({
        animation: {
            // fade-out current tab over 1000 milliseconds
            close: {
                effects: "fadeOut"
            },
            // fade-in new tab over 500 milliseconds
            open: {
                effects: "fadeIn"
            }
        },
        activate: function (e) {
            if (my.storage) {
                // convert ko view model selectedProducts to string add it storage via amplify 
                amplify.store.sessionStorage(siteURL + '_selectedTab', e.item.id);
            }
        }
    });
    $('#divMoreOptionsTabs').data('kendoTabStrip').select('#' + my.selectedTab);

    //$('#divClientAssist').dnnTabs();

    //$('.tooltip').tooltipster({
    //    icon: '<img alt="Ajuda" src="/icons/sigma/Help_32x32_Standard.png" />',
    //    iconDesktop: true,
    //    theme: '.tooltipster-blue',
    //    position: 'left'
    //});

    //$('#divClientAssist').dnnPanels();

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
                    if (data.Sent === true) {
                        $('#sentPropagandaStatus').html('&#218;ltimo envio de informa&#231;&#227;o n&#227;o confirmado. Clique aqui para confirmar recebimento do &#250;ltimo envio.');
                        $('#sentProductStatus').html('&#218;ltimo envio de informa&#231;&#227;o n&#227;o confirmado. Clique aqui para confirmar recebimento do &#250;ltimo envio.');
                        $('#sentPropagandaConfirm').attr({ 'disabled': false });
                        $('#sentProductConfirm').attr({ 'disabled': false });
                    } else {
                        $('#sentPropagandaStatus').text('Nada enviado recentemente.');
                        $('#sentProductStatus').text('Nada enviado recentemente.');
                        $('#sentPropagandaConfirm').attr({ 'disabled': true });
                        $('#sentProductConfirm').attr({ 'disabled': true });
                    }
                    my.vm.selectedSalesRepId(data.SalesRep);
                    my.vm.createdByUser(data.CreatedByUser);
                    my.vm.createdOnDate(data.CreatedOnDate);

                    //$('#moduleTitle').text('Cliente: ' + data.DisplayName + ' (ID:' + data.ClientId + ')');

                    $.getJSON('/desktopmodules/riw/api/people/getHistory?personId=' + my.personId, function (data) {
                        if (data.length > 0) {
                            my.vm.personHistories.removeAll();
                            $.each(data, function (i, item) {
                                my.vm.personHistories.unshift(new my.PersonHistory(item));
                            });
                        }
                    });
                }
            });
        }
    };

    my.getPerson();

    $('#ddlUsers').kendoDropDownList({
        //optionLabel: "Selecionar",
        dataTextField: 'DisplayName',
        dataValueField: 'UserId',
        select: updateCalendar,
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/people/GetUsersByRoleGroup?portalId=' + portalID + '&roleGroupName=Departamentos'
                }
            }
        },
        value: userID
    });

    $("#bDayTextBox").kendoDatePicker({
        format: "m",
        parseFormats: ["m"]
    });

    $("#dtpSchedule").kendoDateTimePicker();

    my.vm.loadContacts();

    function updateCalendar(e) {
        var dataItem = this.dataItem(e.item.index());
        GetDueDates(dataItem.UserId);
        $('#scheduledItems').html('');
        setTimeout(function () {
            reloadCalender();
        }, 100);
    }

    function GetDueDates(value) {
        $.ajax({
            async: false,
            url: '/desktopmodules/riw/api/agenda/GetAgenda?portalId=' + portalID + '&UserId=' + value
        }).done(function (data) {
            if (data) {
                my.dueDates.length = 0;

                for (var i = 0; i < data.length; i++) {
                    var theDate = new Date(kendo.parseDate(data[i].StartDateTime));
                    my.dueDates.push(+new Date(theDate.getFullYear(), theDate.getMonth(), theDate.getUTCDate()));
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
                //$().toastmessage('showErrorToast', data.Msg);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
    }

    function reloadCalender() {
        var cal = $("#calShedule").data('kendoCalendar');
        cal.value(null);
        cal.options.dates = my.dueDates;
        cal.navigateToFuture();
        cal.navigateToPast();
    }

    function loadCalender() {
        my.today = new Date();
        $("#calShedule").kendoCalendar({
            dates: my.dueDates,
            //min: new Date(my.today.getFullYear(), my.today.getMonth(), 1),
            navigate: function (e) {
                this.element.find("tbody").find("a").removeAttr("title");
            },
            change: selectDate,
            month: {
                // template for dates in month view
                //content: '${ dates.StartDateTime }'
                content: '# if ($.inArray(+data.date, data.dates) != -1) { #' +
                            '<div class="' +
                                'party' +
                               //'# if (data.value < 10) { #' +
                               //    "exhibition" +
                               //'# } else if ( data.value < 20 ) { #' +
                               //    "party" +
                               //'# } else { #' +
                               //    "cocktail" +
                               //'# } #' +
                            '">#= data.value #</div>' +
                         '# } else { #' +
                         '#= data.value #' +
                         '# } #'
            }
        });
    }

    $("#scheduledItems").dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        dialogClass: 'dnnFormPopup',
        title: 'Agenda',
        width: '80%',
        buttons: {
            'ok': {
                text: 'Ok',
                //priority: 'primary',
                "class": 'dnnPrimaryAction',
                click: function () {
                    $(this).dialog('close');
                }
            }
        }
    });

    function selectDate() {
        var params = {
            portalId: portalID,
            startDateTime: moment(new Date(this.value().getFullYear(), this.value().getMonth(), kendo.parseDate(kendo.toString(this.value(), 'd')).getUTCDate(), 0, 0, 0)).format(),
            endDateTime: moment(new Date(this.value().getFullYear(), this.value().getMonth(), kendo.parseDate(kendo.toString(this.value(), 'd')).getUTCDate(), 23, 59, 59)).format(),
            userId: $('#ddlUsers').val().length > 0 ? $('#ddlUsers').val() : userID
        };
        $.ajax({
            url: '/desktopmodules/riw/api/agenda/GetAgenda',
            data: params
        }).done(function (data) {
            if (data) {
                if (data.length > 0) {
                    $('#scheduledItems').html('');
                    for (var i = 0; i < data.length; i++) {
                        var startDate = kendo.toString(new Date(kendo.parseDate(data[i].StartDateTime)), 'g');
                        var endDate = kendo.toString(new Date(kendo.parseDate(data[i].StartDateTime)), 'g');
                        var userAgenda = $('#ddlUsers').val().length > 0 ? $('#ddlUsers').val() : userID;
                        var strURL = '<strong>De</strong> ' + startDate + ' <strong>a</strong> ' + endDate + ' - <strong>' + data[i].Subject + '</strong>'; // '<a href="http://' + _siteURL + '/perfil/agenda/tabid/103/UserId/' + userAgenda + '/Default.aspx" onclick="javascript:window.parent.top.location = this.href";><strong>De</strong> ' + startDate + ' <strong>a</strong> ' + endDate + ' - <strong>' + data[i].Subject + '</strong></a>';
                        $('#scheduledItems').append(strURL + ' - ' + data[i].Description);
                    }
                    $("#scheduledItems").dialog('open');
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
                //$().toastmessage('showErrorToast', data.Msg);
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
    }

    GetDueDates(userID);

    setTimeout(function () {
        loadCalender();
    }, 100);

    $('#btnSaveAgenda').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);

        if ($('#dtpSchedule').data('kendoDateTimePicker').value()) {

            var params = {
                PortalId: portalID,
                PersonId: my.personId,
                UserId: $('#ddlUsers').val() !== '' ? $('#ddlUsers').val() : userID,
                StartDateTime: moment($('#dtpSchedule').data('kendoDateTimePicker').value()).format(),
                EndDateTime: moment($('#dtpSchedule').data('kendoDateTimePicker').value()).format(),
                Reminder: 'BEGIN:VALARM  TRIGGER:-PT2880M  X-TELERIK-UID:0440c18f-ce88-4934-ac5e-5d134420d4c5  END:VALARM'
            };

            if (my.vm.selectedContacts().length) {

                $this.button('loading');

                params.Description = agendaInfo.replace('[CLIENTE]', my.vm.displayName()).replace('[DATA]', kendo.toString($('#dtpSchedule').data('kendoDateTimePicker').value(), 'F')).replace('[SOBRE]', my.converter.makeHtml($('#agendaMessageTextarea').val()));
                params.Subject = 'Agenda para ' + displayName + ' - ' + siteName;
                params.Annotations = agendaInfo.replace('[CLIENTE]', my.vm.displayName()).replace('[DATA]', kendo.toString($('#dtpSchedule').data('kendoDateTimePicker').value(), 'F')).replace('[SOBRE]', my.converter.makeHtml($('#agendaMessageTextarea').val()));
                params.HistoryText = $('#addAgendaHistory').is(':checked') ? my.converter.makeHtml($('#agendaMessageTextarea').val()) : null;
                params.Emails = my.vm.selectedContacts();
                params.CreatedByUser = userID;
                params.CreatedOnDate = moment().format();

                $.ajax({
                    type: 'POST',
                    url: '/desktopmodules/riw/api/agenda/updateAgenda',
                    data: params
                }).done(function (data) {
                    if (data.Result.indexOf("success") !== -1) {
                        $('#dtpSchedule').data('kendoDateTimePicker').value('');
                        GetDueDates($('#ddlUsers').val() !== '' ? $('#ddlUsers').val() : userID);
                        reloadCalender();
                        //my.vm.selectedContacts([]);
                        $('#scheduledItems').html('');
                        $('#scheduledItems').hide();
                        //$.getJSON('/desktopmodules/riw/api/clients/GetClientHistory?cId=' + my.vm.personId(), function (data) {
                        //    if (data.length > 0) {
                        //        // $('#history').html('');
                        //        //data.sort(my.vm.sortJsonName('HistoryId', false, function (a) { return a.toUpperCase() }));
                        //        my.vm.clientHistories.removeAll();
                        //        for (var i = 0; i < data.length; i++) {
                        //            // $('#history').append('<span>' + data[i].HistoryText + '</span><br />')
                        //            my.vm.clientHistories.unshift(new my.ClientHistory().historyId(data[i].HistoryId).historyText(data[i].HistoryText));
                        //        }
                        //    }
                        //});
                        //my.vm.personHistories.unshift(new my.PersonHistory().historyId(0).historyText(params.HistoryText));
                        if ($('#addAgendaHistory').is(':checked')) {
                            params.Avatar = amplify.store.sessionStorage('avatar');
                            params.DisplayName = displayName;
                            my.vm.personHistories.unshift(new my.PersonHistory(params));
                        }
                        $('#agendaMessageTextarea').val('');
                        //$('#btnSaveAgenda').html('<i class="icon-ok"></i> Gravar e Enviar Agenda').attr({ 'disabled': false });
                        //$().toastmessage('showSuccessToast', 'Agenda e hist&#243;rico atualizados com sucesso.');
                        $.pnotify({
                            title: 'Sucesso!',
                            text: 'Agenda e e-Mails enviados.',
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
            } else {

                var $dialog = $('<div></div>')
                        .html('<p>Nenhum contato foi selecionado para ser notificado. Deseja continuar?</p>')
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
                                    text: 'Salvar Lembrete',
                                    //priority: 'primary',
                                    "class": 'dnnPrimaryAction',
                                    click: function () {

                                        $this.button('loading');

                                        params.Description = my.converter.makeHtml($('#agendaMessageTextarea').val());
                                        params.Subject = 'Lembrete: ' + my.vm.displayName();
                                        params.Annotations = my.converter.makeHtml($('#agendaMessageTextarea').val());
                                        params.HistoryText = $('#addAgendaHistory').is(':checked') ? '<p>Lembrete gravado na agenda para: ' + kendo.toString($('#dtpSchedule').data('kendoDateTimePicker').value(), 'F') + '</p>' + my.converter.makeHtml($('#agendaMessageTextarea').val()) : null;
                                        params.Emails = '';
                                        params.CreatedByUser = userID;
                                        params.CreatedOnDate = moment().format();

                                        $.ajax({
                                            type: 'POST',
                                            url: '/desktopmodules/riw/api/agenda/updateAgenda',
                                            data: params
                                        }).done(function (data) {
                                            if (data.Result.indexOf("success") !== -1) {
                                                $('#dtpSchedule').data('kendoDateTimePicker').value('');
                                                GetDueDates($('#ddlUsers').val() !== '' ? $('#ddlUsers').val() : userID);
                                                reloadCalender();
                                                $('#scheduledItems').html('');
                                                $('#scheduledItems').hide();
                                                //$.getJSON('/desktopmodules/riw/api/clients/GetClientHistory?cId=' + my.vm.personId(), function (data) {
                                                //    if (data.length > 0) {
                                                //        // $('#history').html('');
                                                //        //data.sort(my.vm.sortJsonName('HistoryId', false, function (a) { return a.toUpperCase() }));
                                                //        my.vm.clientHistories.removeAll();
                                                //        for (var i = 0; i < data.length; i++) {
                                                //            // $('#history').append('<span>' + data[i].HistoryText + '</span><br />')
                                                //            my.vm.clientHistories.unshift(new my.ClientHistory().historyId(data[i].HistoryId).historyText(data[i].HistoryText));
                                                //        }
                                                //    }
                                                //});
                                                //my.vm.personHistories.unshift(new my.PersonHistory().historyId(0).historyText(params.HistoryText));
                                                if ($('#addAgendaHistory').is(':checked')) {
                                                    params.Avatar = amplify.store.sessionStorage('avatar');
                                                    params.DisplayName = displayName;
                                                    my.vm.personHistories.unshift(new my.PersonHistory(params));
                                                }
                                                $('#agendaMessageTextarea').val('');
                                                //$().toastmessage('showSuccessToast', 'Agenda e hist&#243;rico atualizados com sucesso.');
                                                $.pnotify({
                                                    title: 'Sucesso!',
                                                    text: 'Agenda e hist&#243;rico atualizados.',
                                                    type: 'success',
                                                    icon: 'fa fa-check fa-lg',
                                                    addclass: "stack-bottomright",
                                                    stack: my.stack_bottomright
                                                });
                                                $dialog.dialog('close');
                                                $dialog.dialog('destroy');
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
            }
        }
    });

    $('.editContacts').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        document.location.href = editURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/sel/7/subSel/3/retSel/10';
    });

    my.sentConfirm = function () {
        var params = {
            PortalId: portalID,
            PersonId: my.personId,
            Sent: false,
            HistoryText: '<p>&#218;ltimo envio de informa&#231;&#227;o marcado como recebido.</p>',
            ModifiedByUser: userID,
            ModifiedOnDate: moment().format()
        };
        $.ajax({
            type: 'PUT',
            url: '/desktopmodules/riw/api/people/UpdateClientSent',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $('#sentPropagandaStatus').html('&#218;ltimo envio marcado como recebido.');
                $('#sentPropagandaConfirm').attr({ 'disabled': true });
                $('#sentProductStatus').html('&#218;ltimo envio marcado como recebido.');
                $('#sentProductConfirm').attr({ 'disabled': true });

                params.Avatar = amplify.store.sessionStorage('avatar');
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
        });
    };

    //my.propagandas = function () {

    $('#ddlPropagandas').kendoDropDownList({
        optionLabel: 'Selecionar',
        dataTextField: 'ContentTitle',
        dataValueField: 'ContentId',
        //template: '#= Text.replace(".html", "") #',
        dataSource: {
            transport: {
                read: {
                    url: '/desktopmodules/riw/api/htmlcontents/GetHtmlContentList?portalId=' + portalID
                }
            }
        },
        dataBound: function (e) {
            if (this.dataSource.total() === 0) {
                this.text('Sem Registro');
                this.enable(false);
            }
        },
        select: function (e) {
            var dataItem = this.dataItem(e.item.index());
            if (dataItem) {
                //this.text(dataItem.Text.replace('.html', ''));
                my.selectedPropagandaId = kendo.parseInt(dataItem.ContentId);
            }
        }
    });

    //$("#ddlPropagandaContacts").kendoDropDownList({
    //    optionLabel: " Selecionar ",
    //    dataSource: my.clientContacts,
    //    dataTextField: 'ContactName',
    //    dataValueField: 'ContactEmail1',
    //    template: kendo.template($('#tmplContacts').html())
    //});

    //$('#ddlPropagandaContacts').data('kendoDropDownList').text(' Selecionar ');
    //$('#ddlPropagandaContacts').data('kendoDropDownList').bind('dataBound', function (e) {
    //    if (this.dataSource.total() === 0) {
    //        this.text('Sem Registro');
    //        this.enable(false);
    //    } else {
    //        this.text(' Selecionar ');
    //        this.enable(true);
    //    }
    //});

    //var emptyEditor = function () {
    //    parent.$('#propagandaTextArea').data('kendoEditor').value('');
    //};

    $('#btnPropagandas').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        //$('#propagandaEditorWindow').kendoWindow({
        //    title: 'Propaganda',
        //    modal: false,
        //    width: 850,
        //    height: 420,
        //    close: emptyEditor
        //}).data("kendoWindow").center().open();

        //if ($('#ddlPropagandas').val().length > 0) {
        //    my.vm.selectedPropagandaId($('#ddlPropagandas').val());
        //}

        var sContent = editorURL + '?SkinSrc=[G]Skins/riw/popUpSkin&ContainerSrc=[G]Containers/riw/popUpContainer#personId/' + my.personId + '/pId/' + my.selectedPropagandaId;
        document.location.href = sContent;

        //parent.$("#clientWindow").append("<div id='editorWindow'></div>");
        //parent.$('#editorWindow').kendoWindow({
        //    title: 'Propaganda',
        //    modal: false,
        //    width: 950,
        //    height: 580,
        //    content: sContent,
        //    open: function () {
        //        my.loadPropaganda(my.selectedPropagandaId);
        //    },
        //    close: function () {
        //        emptyEditor.open();
        //    },
        //    deactivate: function () {
        //        this.destroy();
        //    }
        //}).data('kendoWindow').open();
    });

    $('#btnSendPropaganda').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        if (($('#ddlPropagandas').data('kendoDropDownList').value().length > 0) && (my.vm.selectedContacts().length > 0)) {

            var params = {
                PortalId: portalID,
                ContentId: $('#ddlPropagandas').data('kendoDropDownList').value(),
                PersonId: my.personId,
                UserId: userID,
                Emails: my.vm.selectedContacts(),
                Subject: $('#ddlPropagandas').data('kendoDropDownList').text(),
                MessageBody: my.converter.makeHtml($('#propagandaMessageTextarea').val()),
                HistoryText: $('#addPropagandaHistory').is(':checked') ? '<p>Propaganda enviada para ' + my.vm.displayName() + ' (' + JSON.stringify(my.vm.selectedContacts()) + ')</p><strong>Propaganda: </strong>' + $('#ddlPropagandas').data('kendoDropDownList').text() : null,
                CreatedByUser: userID,
                CreatedOnDate: moment().format()
            };

            $.ajax({
                type: 'POST',
                url: '/desktopmodules/riw/api/store/SendPackage',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    $('#ddlPropagandas').data('kendoDropDownList').value('');
                    //my.vm.selectedContacts([]);
                    $('#sentPropagandaStatus').html('Propaganda enviada e recebimento n&#227;o confirmado. Clique aqui para confirmar recebimento do &#250;ltimo envio.');
                    $('#sentPropagandaConfirm').attr({ 'disabled': false });
                    //$.getJSON('/desktopmodules/riw/api/clients/GetClientHistory?cId=' + my.vm.personId(), function (data) {
                    //    if (data.length > 0) {
                    //        my.vm.clientHistories.removeAll();
                    //        for (var i = 0; i < data.length; i++) {
                    //            my.vm.clientHistories.unshift(new my.ClientHistory().historyId(data[i].HistoryId).historyText(data[i].HistoryText));
                    //        }
                    //    }
                    //});
                    //my.vm.personHistories.unshift(new my.PersonHistory().historyId(0).historyText(params.HistoryText));
                    if ($('#addPropagandaHistory').is(':checked')) {
                        params.Avatar = amplify.store.sessionStorage('avatar');
                        params.DisplayName = displayName;
                        my.vm.personHistories.unshift(new my.PersonHistory(params));
                    }
                    $('#propagandaMessageTextarea').val('');
                    //var sentParams = {
                    //    personId: my.personId,
                    //    sent: true,
                    //    hText: '',
                    //    uId: _userID,
                    //    cd: moment().format()
                    //};
                    //$.post('/desktopmodules/riw/api/clients/UpdateClientSent', sentParams, function (data) { });
                    //$().toastmessage('showSuccessToast', 'Propaganda enviada com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Propaganda transmitida.',
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
    });

    $('#sentPropagandaConfirm').mousedown(function () {
        my.sentConfirm();
    });

    $("#btnRemoveSelectedItems").click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();
        var grid = $('#selectedProductsGrid').data("kendoGrid");
        var msg = '';
        switch (true) {
            case (grid.select().length === 1):
                msg = 'Tem certeza que deseja excluir o item selecionado?';
                break;
            case (grid.select().length > 1):
                msg = 'Tem certeza que deseja excluir os itens selecionados?';
                break;
            default:
                return false;
        }
        var r = confirm(msg);
        if (r === true) {
            var dataItem = grid.dataItem(grid.select());
            my.vm.selectedProducts.remove(function (item) {
                return item.prodId() === dataItem.prodId;
            });
            grid.select().each(function () {
                grid.removeRow($(this));
            });
            $('#selectedProductsGrid').data('kendoGrid').dataSource.sync();
        }
    });

    $("#productSearch").select2({
        placeholder: "Busque por produtos...",
        allowClear: true,
        //minimumInputLength: 3,
        id: function (data) {
            return {
                ProductId: data.ProductId,
                ProductName: data.ProductName,
                Summary: data.Summary,
                ProductRef: data.ProductRef,
                Barcode: data.Barcode,
                UnitValue: data.UnitValue,
                ProductImageId: data.ProductImageId,
                Extension: data.Extension,
                CategoriesNames: data.CategoriesNames,
                ProductUnit: data.ProductUnit,
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
                return {
                    portalId: portalID,
                    filter: term,
                    pageIndex: page,
                    pageSize: 10,
                    orderBy: 'ProductName'
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
    });

    $('#productSearch').on("select2-selecting", function (e) {
        my.vm.selectedProductId(e.val.ProductId);
        my.vm.prodRef(e.val.ProductRef);
        my.vm.prodName(e.val.ProductName);
        my.vm.unitValue(e.val.UnitValue);
        my.vm.prodIntro(e.val.Summary);
        my.vm.prodImageId(e.val.ProductImageId);
        my.vm.extension(e.val.Extension);
        my.vm.prodBarCode(e.val.Barcode);
    });

    $('#btnAddProduct').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        if (my.vm.selectedProductId()) {
            my.vm.selectedProducts.unshift(new my.Product()
                .prodId(my.vm.prodId())
                .prodRef(my.vm.prodRef())
                .prodBarCode(my.vm.prodBarCode())
                .prodName(my.vm.prodName())
                .prodIntro(my.vm.prodIntro())
                .prodImageId(my.vm.prodImageId())
                .extension(my.vm.extension())
                .unitValue(my.vm.unitValue()));
            my.vm.selectedProductId(null);
            $('#productSearch').select2('val', '');
        };
    });

    my.enlargeImage = function (e) {
        var imageWindow = parent.$("#imgWindow");
        //imageWindow.closest('.k-window').css({ 'top': '10px', 'left': '10px' });
        imageWindow.kendoWindow({
            position: {
                top: 10,
                left: 10
            },
            title: e.title,
            resizable: false,
            modal: true
        }).data('kendoWindow').content('<img src=' + e.name + ' />').open();
    };

    $('#btnSendProducts').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        if ((my.vm.selectedProducts().length > 0) && (my.vm.selectedContacts().length > 0)) {
            var params = {
                PortalId: portalID,
                PersonId: my.personId,
                Subject: 'Email de ' + siteName + ', requisitado por ' + my.vm.displayName(),
                MessageBody: my.converter.makeHtml($('#productMessageTextarea').val()).replace('[PRODUTOS]', my.vm.sentProductsBody()).replace('[CLIENTE]', my.vm.personFullName()),
                Emails: JSON.stringify(my.vm.selectedContacts()),
                UserId: userID,
                CreatedByUser: userID,
                CreatedOnDate: moment().format()
            };

            params.HistoryText = $('#addProductHistory').is(':checked') ? '<p>Envio de informa&#231;&#227;o de produtos para ' +
                    my.vm.displayName() + ' ' + JSON.stringify(my.vm.selectedContacts()) + '</p>' + params.MessageBody : null;

            $.ajax({
                type: 'POST',
                url: '/desktopmodules/riw/api/store/sendPackage',
                data: params
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    $('#sentProductStatus').html('Informa&#231;&#227;o enviada e recebimento n&#227;o confirmado. Clique aqui para confirmar recebimento do &#250;ltimo envio.');
                    $('#sentProductConfirm').attr({ 'disabled': false });
                    my.vm.selectedProducts('');
                    $('#selectedProductsGrid').data('kendoGrid').dataSource.sync();
                    my.vm.selectedContacts(null);
                    $('#productMessageTextarea').val(toMarkdown(productMessage));
                    //my.vm.personHistories.unshift(new my.PersonHistory().historyId(0).historyText(params.HistoryText + params.MessageBody));
                    if ($('#addProductHistory').is(':checked')) {
                        params.Avatar = amplify.store.sessionStorage('avatar');
                        params.DisplayName = displayName;
                        params.HistoryText = params.HistoryText;
                        my.vm.personHistories.unshift(new my.PersonHistory(params));
                    }
                    //$().toastmessage('showSuccessToast', 'Mensagem enviada com sucesso.');
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Informa&#231;&#227;o sobre produto(s) transmitida.',
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
    });

    $('#sentProductConfirm').click(function () {
        my.sentConfirm();
    });
    //};

    $('.markdown-editor').css({ 'min-width': '90%', 'height': '80px', 'margin-bottom': '5px' }).attr({ 'cols': '30', 'rows': '2' });

    $('.markdown-editor').autogrow();
    $('.markdown-editor').css('overflow', 'hidden').autogrow();

    var productMessage = 'Caro(a) [CLIENTE], requisitou este email de <strong>' + siteName + '</strong><br/><br />' +
            'Veja abaixo o(s) item(ns) requisitado(s).<br/><br />' + '[PRODUTOS]<br/><br />' +
            'Descubra mais sobre estes e outros produtos no website <a href="http://' + siteURL + '" title="' + siteName + '">' + siteURL + '</a><br/><br />' +
            'Obrigado pelo interesse em nossos produtos.';

    //$('#productMessageTextarea').css({ 'height': '0 !important' });

    $('#productMessageTextarea').val(toMarkdown(productMessage));

    $('.togglePreview').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();
        var $this = $(this);

        var ele = $(this).data('provider');

        //var content = $('#historyTextArea1').val();
        //content += $('#historyTextArea2').val();
        //content += $('#historyTextArea3').val();
        //content += $('#historyTextArea4').val();
        //content += $('#historyTextArea5').val();
        //content += $('#historyTextArea6').val();
        //content += $('#historyTextArea7').val();

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

    $('.btnAddHistory').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

        var ele = $(this).data('provider');

        //var content = $('#historyTextArea1').val();
        //content += $('#historyTextArea2').val();
        //content += $('#historyTextArea3').val();

        var historyHtmlContent = my.converter.makeHtml($('#' + ele).val().trim());

        var params = {
            PortalId: portalID,
            PersonId: my.personId,
            HistoryText: historyHtmlContent.replace('[CLIENTE]', my.vm.personFullName()),
            Locked: true,
            CreatedByUser: userID,
            CreatedOnDate: moment().format(),
            ConnId: my.hub.connection.id
        };

        $.ajax({
            type: 'POST',
            url: '/desktopmodules/riw/api/people/updateHistory',
            data: params
        }).done(function (data) {
            if (data.Result.indexOf("success") !== -1) {
                $('#' + ele).val('');
                //$().toastmessage('showSuccessToast', 'Texto adicionado ao hist&#243;rico com successo!');
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Texto adicionado ao hist&#243;rico.',
                    type: 'success',
                    icon: 'fa fa-check fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
                params.Avatar = amplify.store.sessionStorage('avatar'); // ? '/portals/0/' + _avatar + '?w=45&h=45&mode=crop' : '/desktopmodules/rildoinfo/webapi/content/images/user.png?w=45&h=45';
                params.DisplayName = displayName;
                my.vm.personHistories.unshift(new my.PersonHistory(params));
                //.historyByAvatar(_avatar ? '/portals/0/' + _avatar + '?w=45&h=45&mode=crop' : '/desktopmodules/rildoinfo/webapi/content/images/user.png?w=45&h=45')
                //.historyByName(_displayName)
                //.historyId(data.EstimateHistoryId)
                //.historyText(params.HistoryText)
                //.createdOnDate(moment())
                //.locked(false));
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

    $('input.enterastab, select.enterastab, textarea.enterastab').on('keydown', function (e) {
        if (e.keyCode === 13) {
            var focusable = $('input,a,select,textarea').filter(':visible');
            if (this.name === 'streetTextBox') {
                focusable.eq(focusable.index(this) + 1).focus().select();
            } else {
                focusable.eq(focusable.index(this) + 2).focus().select();
            }
            return false;
        }
    });

    //my.initializer();
    my.vm.loadRegions();

    if (my.getParameterByName('sel')) {
        var sel = my.getParameterByName('sel');
        $('#actionsMenu ul li[id=' + sel + ']').addClass('jqx-menu-item-selected');
        if (sel <= 7) {
            $('#personMenu ul li:first-child').addClass('jqx-menu-item-selected');
            //} else {
        }
    }

    $('#actionsMenu').on('itemclick', function (event) {
        ////event.args.textContent = 'Um momento...';
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
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        var $this = $(this);
        $this.button('loading');

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

function productFormatResult(data) {
    var markup = '<table class="product-result"><tr>';
    if (data.ProductImageId > 0) {
        markup += '<td class="product-image"><img src="/databaseimages/' + data.ProductImageId + '.' + data.Extension + '?maxwidth=60&maxheight=60"/></td>';
    } else {
        markup += '<td class="product-image"><img src="/portals/0/Images/No-Image.jpg?maxwidth=60&maxheight=60"/></td>';
    }
    markup += "<td class='product-info'><div class='product-title'>" + data.ProductName + "</div>";
    if (data.Barcode) {
        markup += "<div><strong>CB: </strong>" + data.Barcode + "</div>";
    } else if (data.ProductRef) {
        markup += "<div><strong>REF: </strong>" + data.ProductRef + "</div>";
    }
    markup += "</td></tr></table>"
    return markup;
}

function productFormatSelection(data) {
    return data.ProductName;
}