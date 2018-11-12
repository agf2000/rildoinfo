
Imports System.Net
Imports System.Net.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation
Imports RIW.Modules.WebAPI.Components.Repositories
Imports System.Threading

Public Class AgendaController
    Inherits DnnApiController

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(AgendaController))

    ''' <summary>
    ''' Gets user calendar events
    ''' </summary>
    ''' <param name="portalId"></param>
    ''' <param name="userId"></param>
    ''' <param name="startDateTime">Start Calendar Date</param>
    ''' <param name="endDateTime">End Calendar Date</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetAgenda(portalId As Integer, userId As String, Optional startDateTime As String = Nothing, Optional endDateTime As String = Nothing) As HttpResponseMessage
        Try

            If startDateTime = Nothing Then
                startDateTime = Null.NullDate
            End If

            If endDateTime = Nothing Then
                endDateTime = Null.NullDate
            End If

            Dim userAgendaDataCtrl As New AgendasRepository
            Dim userAgendaData = userAgendaDataCtrl.GetAgendaData(portalId, userId, CDate(startDateTime), CDate(endDateTime), Null.NullInteger)

            Return Request.CreateResponse(HttpStatusCode.OK, userAgendaData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates or adds appointment data
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function UpdateAgenda(dto As Components.Models.Agenda) As HttpResponseMessage
        Try
            Dim appointment As New Components.Models.Agenda
            Dim agendaCtrl As New AgendasRepository

            Dim settingCtrl As New SettingsRepository
            Dim settingsDictionay = settingCtrl.GetSettings(dto.PortalId)

            'For Each dto In appts

            If dto.AppointmentId > 0 Then
                appointment = agendaCtrl.GetAppointmentData(dto.AppointmentId, dto.PortalId)
            End If

            appointment.Annotations = dto.Annotations
            appointment.PersonId = dto.PersonId
            appointment.Description = dto.Description
            appointment.DocId = dto.DocId

            If Not dto.EndDateTime = Nothing Then
                appointment.EndDateTime = dto.EndDateTime
            Else
                appointment.EndDateTime = dto.StartDateTime.AddHours(1)
            End If

            appointment.ModifiedByUser = dto.CreatedByUser
            appointment.ModifiedOnDate = dto.CreatedOnDate
            appointment.PortalId = dto.PortalId
            appointment.RecurrenceParentId = dto.RecurrenceParentId

            If dto.RecurrenceRule IsNot Nothing Then
                appointment.RecurrenceRule = dto.RecurrenceRule
            End If

            appointment.Reminder = dto.Reminder ' BEGIN:VALARM  TRIGGER:-PT2880M  X-TELERIK-UID:0440c18f-ce88-4934-ac5e-5d134420d4c5  END:VALARM
            appointment.StartDateTime = dto.StartDateTime
            appointment.Subject = dto.Subject
            appointment.UserId = dto.UserId

            If dto.AppointmentId > 0 Then
                agendaCtrl.UpdateAppointmentData(appointment)
            Else
                appointment.CreatedByUser = dto.CreatedByUser
                appointment.CreatedOnDate = dto.CreatedOnDate
                agendaCtrl.AddAppointmentData(appointment)
            End If

            If dto.HistoryText IsNot Nothing Then
                Dim clientHistory As New Components.Models.PersonHistory
                Dim clientHistoryCtrl As New PersonHistoriesRepository

                clientHistory.PersonId = dto.PersonId
                clientHistory.HistoryText = dto.HistoryText.Replace("BR".ToUpper(), "<br /><br />")
                clientHistory.CreatedByUser = dto.CreatedByUser
                clientHistory.CreatedOnDate = dto.CreatedOnDate

                clientHistoryCtrl.AddPersonHistory(clientHistory)
            End If

            If dto.Emails.Count > 0 Then

                Dim recipientList As New List(Of Users.UserInfo)
                Dim portalCtrl = New Portals.PortalController()
                Dim portalInfo = portalCtrl.GetPortal(dto.PortalId)
                'Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(portalInfo.PortalID)
                'Dim portalEmail = Portals.PortalController.GetPortalSetting("RIW_StoreEmail", portalInfo.PortalID, portalInfo.Email)

                Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = settingsDictionay("storeEmail"), .DisplayName = portalInfo.PortalName}

                For Each email In dto.Emails

                    Dim clientInfo As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = email, .DisplayName = ""}
                    recipientList.Add(clientInfo)

                Next

                Dim userInfo = Users.UserController.GetUserById(dto.PortalId, dto.CreatedByUser)
                recipientList.Add(userInfo)

                Dim mm As New Net.Mail.MailMessage() With {.Subject = dto.Subject, .Body = dto.Description.Replace("br", Environment.NewLine), .IsBodyHtml = True, .From = New Net.Mail.MailAddress(storeUser.Email, userInfo.DisplayName)}
                mm.ReplyToList.Add(New Net.Mail.MailAddress(userInfo.Email, userInfo.DisplayName))

                'Dim sentMails = distList.SendMail(mailMessage, recipientList, SettingsDictionay("smtpServer"), CInt(SettingsDictionay("smtpPort")), CBool(SettingsDictionay("smtpConnection")), SettingsDictionay("smtpLogin"), SettingsDictionay("smtpPassword"))

                Dim emailThreading As New Thread(Sub() PostOffice.SendMail(mm, recipientList, SettingsDictionay("smtpServer"), CInt(SettingsDictionay("smtpPort")),
                                                              CBool(SettingsDictionay("smtpConnection")), SettingsDictionay("smtpLogin"),
                                                              SettingsDictionay("smtpPassword"))) With {.IsBackground = True}
                emailThreading.Start()

                'Notifications.SendStoreEmail(_userInfo, _clientInfo, Nothing, Nothing, dto.Subject, dto.Description.Replace("br", Environment.NewLine), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Text, DotNetNuke.Services.Mail.MailPriority.High, storeUser, dto.PortalId, True)

            End If
            'Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            'Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .AppointmentId = appointment.AppointmentId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes appointment by appointment id
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpDelete> _
    Function RemoveAppointment(dto As Components.Models.Agenda) As HttpResponseMessage
        Try
            Dim agendaCtrl As New AgendasRepository
            agendaCtrl.RemoveAppointment(dto.AppointmentId, dto.PortalId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

End Class
