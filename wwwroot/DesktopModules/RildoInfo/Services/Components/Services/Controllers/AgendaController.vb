
Imports System.Net
Imports System.Net.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation

Public Class AgendaController
    Inherits DnnApiController

    Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(AccountsController))

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
    Function getAgenda(ByVal portalId As String, ByVal userId As String, Optional ByVal startDateTime As DateTime = Nothing, Optional ByVal endDateTime As DateTime = Nothing) As HttpResponseMessage
        Try

            If startDateTime = Nothing Then
                startDateTime = Null.NullDate
            End If

            If endDateTime = Nothing Then
                endDateTime = Null.NullDate
            End If

            Dim userAgendaDataCtrl As New AgendasRepository
            Dim userAgendaData = userAgendaDataCtrl.GetAgendaData(portalId, userId, startDateTime, endDateTime)

            Return Request.CreateResponse(HttpStatusCode.OK, userAgendaData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
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
    Function updateAgenda(dto As Models.Agenda) As HttpResponseMessage
        Try
            Dim appointment As New Models.Agenda
            Dim agendaCtrl As New AgendasRepository

            If dto.AppointmentId > 0 Then
                appointment = agendaCtrl.GetAppointmentData(dto.AppointmentId, dto.UserId)
            End If

            appointment.Annotations = dto.Annotations
            appointment.PersonId = dto.PersonId
            appointment.CreatedByUser = dto.CreatedByUser
            appointment.CreatedOnDate = dto.CreatedOnDate
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
            appointment.RecurrenceRule = dto.RecurrenceRule
            appointment.Reminder = dto.Reminder ' BEGIN:VALARM  TRIGGER:-PT2880M  X-TELERIK-UID:0440c18f-ce88-4934-ac5e-5d134420d4c5  END:VALARM
            appointment.StartDateTime = dto.StartDateTime
            appointment.Subject = dto.Subject
            appointment.UserId = dto.UserId

            If dto.AppointmentId > 0 Then
                agendaCtrl.UpdateAppointmentData(appointment)
            Else
                agendaCtrl.AddAppointmentData(appointment)
            End If

            If dto.HistoryText IsNot Nothing Then
                Dim clientHistory As New Models.PersonHistory
                Dim clientHistoryCtrl As New PersonHistoriesRepository

                clientHistory.PersonId = dto.PersonId
                clientHistory.HistoryText = dto.HistoryText.Replace("BR".ToUpper(), "<br /><br />")
                clientHistory.CreatedByUser = dto.CreatedByUser
                clientHistory.CreatedOnDate = dto.CreatedOnDate

                clientHistoryCtrl.addPersonHistory(clientHistory)
            End If

            If dto.Emails IsNot Nothing Then

                Dim recipientList As New List(Of Users.UserInfo)
                Dim portalCtrl = New Portals.PortalController()
                Dim _portalInfo = portalCtrl.GetPortal(dto.PortalId)
                Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(_portalInfo.PortalID)
                Dim _portalEmail = Portals.PortalController.GetPortalSetting("RIW_StoreEmail", _portalInfo.PortalID, _portalInfo.Email)

                Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = _portalEmail, .DisplayName = _portalInfo.PortalName}

                Dim _clientInfo As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = dto.Emails, .DisplayName = ""}
                recipientList.Add(_clientInfo)

                Dim _userInfo = Users.UserController.GetUserById(dto.PortalId, dto.CreatedByUser)
                recipientList.Add(_userInfo)

                Dim mailMessage As New Net.Mail.MailMessage
                Dim distList As New PostOffice

                mailMessage.From = New Net.Mail.MailAddress(storeUser.Email, _userInfo.DisplayName)
                mailMessage.ReplyToList.Add(New Net.Mail.MailAddress(_userInfo.Email, _userInfo.DisplayName))
                mailMessage.Subject = dto.Subject
                mailMessage.Body = dto.Description.Replace("br", Environment.NewLine)
                'mailMessage.Attachments.Add(New Net.Mail.Attachment(Str, String.Format("Orcamento_{0}_{1}.pdf", CStr(dto.EstimateId), Replace(DateTime.Now.ToString(), ":", "-").Replace("/", "-"))))
                mailMessage.IsBodyHtml = True

                Dim sentMails = distList.SendMail(mailMessage, recipientList, myPortalSettings("RIW_SMTPServer"), CInt(myPortalSettings("RIW_SMTPPort")), CBool(myPortalSettings("RIW_SMTPConnection")), myPortalSettings("RIW_SMTPLogin"), myPortalSettings("RIW_SMTPPassword"))

                'Notifications.SendStoreEmail(_userInfo, _clientInfo, Nothing, Nothing, dto.Subject, dto.Description.Replace("br", Environment.NewLine), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Text, DotNetNuke.Services.Mail.MailPriority.High, storeUser, dto.PortalId, True)

            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .AppointmentId = appointment.AppointmentId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes appointment by appointment id
    ''' </summary>
    ''' <param name="appId">Appointment ID</param>
    ''' <param name="userId">User ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpDelete> _
    Function RemoveAppointment(ByVal appId As Integer, userId As Integer) As HttpResponseMessage
        Try
            Dim agendaCtrl As New AgendasRepository
            agendaCtrl.RemoveAppointment(appId, userId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

End Class
