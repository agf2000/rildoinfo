
$(function () {

    my.viewModel();

    var CustomAgenda = kendo.ui.AgendaView.extend({
        endDate: function () {
            var date = kendo.ui.AgendaView.fn.endDate.call(this);
            return kendo.date.addDays(date, 31);
        }
    });

    my.agendaTransport = {
        read: {
            url: '/desktopmodules/riw/api/agenda/getAgenda'
        },
        update: {
            type: 'POST',
            //contentType: "application/json; charset=utf-8",
            url: '/desktopmodules/riw/api/agenda/updateAgenda'
        },
        create: {
            type: 'POST',
            //contentType: "application/json; charset=utf-8",
            url: '/desktopmodules/riw/api/agenda/updateAgenda'
        },
        destroy: {
            type: 'DELETE',
            url: '/desktopmodules/riw/api/agenda/removeAppointment'
        },
        parameterMap: function (options, operation) {
            if (operation !== "read" && options) {
                options.StartDateTime = moment(options.StartDateTime).format();
                options.EndDateTime = moment(options.EndDateTime).format();
                options.UserId = userID;
                options.CreatedByUser = userID;
                options.CreatedOnDate = moment(new Date()).format();
                options.ModifiedByUser = userID;
                options.ModifiedOnDate = moment(new Date()).format();
                //var params = [];
                //$.each(options.models, function (i, a) {
                //    params.push({
                //        PortalId: portalID,
                //        AppointmentId: a.AppointmentId,
                //        Subject: a.Subject,
                //        StartDateTime: moment(a.StartDateTime).format(),
                //        EndDateTime: moment(a.EndDateTime).format(),
                //        Description: a.Description,
                //        UserId: userID,
                //        PersonId: a.PersonId,
                //        RecurrenceRule: a.RecurrenceRule,
                //        CreatedByUser: userID,
                //        CreatedOnDate: moment().format(),
                //        ModifiedByUser: userID,
                //        ModifiedOnDate: moment().format()
                //    });
                //});
                //return kendo.stringify(params);
                return options;
            } else {
                return {
                    portalId: 0,
                    userId: '-1,' + kendo.toString(userID)
                    //startDateTime: moment('1/1/1900').format(),
                    //endDateTime: moment('1/1/1900').format()
                };
            }
        }
    };

    my.agendaData = new kendo.data.SchedulerDataSource({
        batch: false,
        transport: my.agendaTransport,
        //transport: {
        //    read: {
        //        url: '/desktopmodules/riw/api/agenda/getAgenda?portalId=' + portalID + '&userId=' + '-1,' + kendo.toString(userID)
        //    }
        //    //update: function (e) {
        //    //    return false;
        //    //},
        //    //create: function (e) {
        //    //    return false;
        //    //},
        //    //destroy: function (e) {
        //    //    return false;
        //    //}
        //},
        schema: {
            model: {
                id: "taskId",
                fields: {
                    taskId: { from: "AppointmentId", type: "number" },
                    title: { from: "Subject", defaultValue: "No title", validation: { required: true } },
                    start: { type: "date", from: "StartDateTime" },
                    end: { type: "date", from: "EndDateTime" },
                    description: { field: "Description" },
                    personId: { field: "PersonId" },
                    recurrenceId: { from: "RecurrenceParentId" },
                    recurrenceRule: { from: "RecurrenceRule" },
                    //recurrenceException: { from: "RecurrenceException" },
                    ownerId: { field: "UserId", defaultValue: 1 }
                    //isAllDay: { type: "boolean", field: "IsAllDay" }
                }
            }
        }
    });

    //function calendar_Save(e) {
        
    //    var $this = this;

    //    var params = {
    //        PortalId: portalID,
    //        AppointmentId: e.event.taskId,
    //        Subject: e.event.title,
    //        StartDateTime: moment(e.event.start).format(),
    //        EndDateTime: moment(e.event.end).format(),
    //        Description: e.event.description,
    //        UserId: userID,
    //        PersonId: e.event.personId,
    //        RecurrenceId: e.event.recurrenceId,
    //        RecurrenceRule: e.event.recurrenceRule,
    //        CreatedByUser: userID,
    //        CreatedOnDate: moment().format(),
    //        ModifiedByUser: userID,
    //        ModifiedOnDate: moment().format()
    //    };

    //    $.ajax({
    //        type: 'POST',
    //        url: '/desktopmodules/riw/api/agenda/updateAgenda',
    //        data: params
    //    }).done(function (data) {
    //        if (data.Result.indexOf("success") !== -1) {
    //            my.agendaData.read();
    //            $this.refresh();
    //            $.pnotify({
    //                title: 'Sucesso!',
    //                text: 'Calend&#225;rio salvo.',
    //                type: 'success',
    //                icon: 'fa fa-check fa-lg',
    //                addclass: "stack-bottomright",
    //                stack: my.stack_bottomright
    //            });
    //        } else {
    //            $.pnotify({
    //                title: 'Erro!',
    //                text: data.Result,
    //                type: 'error',
    //                icon: 'fa fa-times-circle fa-lg',
    //                addclass: "stack-bottomright",
    //                stack: my.stack_bottomright
    //            });
    //        }
    //    }).fail(function (jqXHR, textStatus) {
    //        console.log(jqXHR.responseText);
    //    });
    //}

    $('#scheduler').kendoTooltip({
        filter: ".k-event",
        position: "top",
        content: function (e) {
            var target = e.target; // the element for which the tooltip is shown
            return target.text(); // set the element text as content of the tooltip
        },
        show: function (e) {
            var target = this.target(); // the element for which the tooltip is shown
        }
    });

    $("#scheduler").kendoScheduler({
        edit: function (e) {
            e.container.find("[for=recurrenceRule]").parent().hide();
            e.container.find("[data-container-for=recurrenceRule]").hide();
            e.container.find("[for=isAllDay]").parent().hide();
            e.container.find("[data-container-for=isAllDay]").hide();
        },
        date: new Date(),
        workDayStart: new Date("2014/1/1 07:00 AM"),
        workDayEnd: new Date("2014/1/1 6:00 PM"),
        //startTime: new Date("2013/6/13 07:00 AM"),
        //endTime: new Date("2013/6/13 06:00 PM"),
        eventTemplate: '#= title #',
        views: [
            {
                type: "day", showWorkHours: true
            },
            {
                type: "week", showWorkHours: true
            },
            {
                type: 'month',
                selected: true
            },
            //"agenda",
            { type: CustomAgenda, title: "Agenda do Mês" }
        ],
        dataSource: my.agendaData,
        allDaySlot: false,
        //selectable: true,
        editable: {
            confirmation: "Tem certeza que deseja excluir este evento?"
            //template: $('#editor').html()
        },
        //save: calendar_Save,
        messages: {
            today: "Hoje",
            save: "Salvar",
            cancel: "Cancelar",
            destroy: "Remover",
            event: "Evento",
            date: "Data",
            time: "Horário",
            allDay: "O Dia Inteiro",
            deleteWindowTitle: "Remover Evento",
            showFullDay: "Mostrar 24h",
            showWorkDay: "Mostrar Horário Comercial",
            views: {
                day: "Dia",
                week: "Semana",
                month: "Mês",
                agenda: "Lista de Eventos"
            },
            recurrenceMessages: {
                deleteWindowTitle: "Remover evento com ocorrêcias",
                deleteWindowOccurrence: "Remover evento atual",
                deleteWindowSeries: "Remover Todas as ocorrências deste evento",
                editWindowTitle: "Editar ocorrências do evento",
                editWindowOccurrence: "Editar evento atual",
                editWindowSeries: "Editar todas as ocorrências deste evento",
                editRecurring: "Você quer editar apenas esse evento ou todas as ocorrências?",
                deleteRecurring: "Excluir apenas esta ocorrência do evento ou toda a série?"
            },
            editor: {
                title: "Título:",
                start: "Início:",
                end: "Términio:",
                allDayEvent: "O Dia Inteiro:",
                description: "Descrição:",
                repeat: "Repetir:",
                timezone: "Zona Horária:",
                startTimezone: "Início Zona Horária:",
                endTimezone: "Final Zona Horária:",
                separateTimezones: "Diferenciada Zona Horária:",
                timezoneEditorTitle: "Zonas Horárias:",
                timezoneEditorButton: "Zona Horária:",
                editorTitle: "Evento"
            },
            recurrenceEditor: {
                frequencies: {
                    never: "Nunca",
                    daily: "Diariamente",
                    weekly: "Semanalmente",
                    monthly: "Mensalmente",
                    yearly: "Anualmente"
                },
                daily: {
                    interval: " dia(s).",
                    repeatEvery: "Repetir a Cada: ",
                    days: " Dia(s)"
                },
                weekly: {
                    interval: " semana(s)",
                    repeatEvery: "Repetir a Cada:",
                    repeatOn: "Repetir em: "
                },
                weekdays: {
                    day: "Dia",
                    weekday: "Dia de Semana",
                    weekend: "Final de Semana"
                },
                monthly: {
                    interval: ' mes(es).',
                    repeatEvery: "Repetir a Cada: ",
                    repeatOn: "Repetir em: ",
                    day: "Day"
                },
                yearly: {
                    repeatEvery: "Repetir a Cada: ",
                    repeatOn: "Repetir em: ",
                    interval: " ano(s)",
                    of: "de"
                },
                end: {
                    after: "após ",
                    label: "Termina: ",
                    never: "Nunca",
                    endCountOccurrence: " nº repetições",
                    on: "em "
                },
                offsetPositions: {
                    first: "primeiro",
                    second: "segundo",
                    third: "terceiro",
                    fourth: "quarto",
                    last: "último"
                }
            }
        }
    });

});
