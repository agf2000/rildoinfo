
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation

Namespace RI.Modules.RIStore_Services
    Public Class AgendaController
        Inherits DnnApiController

        Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(AccountsController))

        ''' <summary>
        ''' Gets user calendar events
        ''' </summary>
        ''' <param name="portalId"></param>
        ''' <param name="uId"></param>
        ''' <param name="startDate">Start Calendar Date</param>
        ''' <param name="endDate">End Calendar Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetAgenda(ByVal portalId As String, ByVal uId As String, Optional ByVal startDate As Date = Nothing, Optional ByVal endDate As Date = Nothing) As HttpResponseMessage
            Try

                If startDate = Nothing Then
                    startDate = Null.NullDate
                End If

                If endDate = Nothing Then
                    endDate = Null.NullDate
                End If

                Dim userGendaData As New List(Of Models.Agenda)
                Dim userGendaDataCtrl As New AgendasRepository

                userGendaData = userGendaDataCtrl.GetAgendaData(portalId, uId, startDate, endDate)

                Return Request.CreateResponse(HttpStatusCode.OK, userGendaData)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Adds appointment data
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <param name="uId">User ID</param>
        ''' <param name="startDate">Start Date Time</param>
        ''' <param name="endDate">End Date Time</param>
        ''' <param name="reminder">Reminder</param>
        ''' <param name="desc">Description</param>
        ''' <param name="subject">Subject</param>
        ''' <param name="annot">Annotations</param>
        ''' <param name="hText">History Text</param>
        ''' <param name="cUId">Created By User ID</param>
        ''' <param name="cd">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function UpdateAgenda(ByVal portalId As Integer,
        ByVal startDate As Date,
        ByVal reminder As String,
        ByVal desc As String,
        ByVal subject As String,
        ByVal cUId As Integer,
        ByVal cd As Date,
        Optional ByVal annot As String = "",
        Optional ByVal endDate As Date = Nothing,
        Optional ByVal cId As Integer = -1,
        Optional ByVal uId As Integer = -1,
        Optional ByVal hText As String = "",
        Optional ByVal emails As String = "",
        Optional ByVal recRule As String = "",
        Optional ByVal recId As Integer = Nothing,
        Optional ByVal docId As Integer = Nothing,
        Optional appId As Integer = -1) As HttpResponseMessage
            Try

                Dim appointment As New Models.Agenda
                Dim agendaCtrl As New AgendasRepository

                If appId > 0 Then
                    appointment = agendaCtrl.GetAppointmentData(appId, uId)
                End If

                If annot <> "" Then
                    appointment.Annotations = annot.Trim()
                End If
                If cId > 0 Then
                    appointment.ClientId = cId
                End If
                appointment.CreatedByUser = cUId
                appointment.CreatedOnDate = cd
                appointment.Description = desc.Trim()
                If docId > 0 Then
                    appointment.DocId = docId
                End If
                If Not endDate = Nothing Then
                    appointment.EndDateTime = endDate
                Else
                    appointment.EndDateTime = startDate.AddHours(1)
                End If
                appointment.ModifiedByUser = cUId
                appointment.ModifiedOnDate = cd
                appointment.PortalId = portalId
                appointment.RecurrenceParentId = recId
                appointment.RecurrenceRule = recRule
                appointment.Reminder = reminder ' BEGIN:VALARM  TRIGGER:-PT2880M  X-TELERIK-UID:0440c18f-ce88-4934-ac5e-5d134420d4c5  END:VALARM
                appointment.StartDateTime = startDate
                appointment.Subject = subject
                appointment.UserId = uId

                If appId > 0 Then
                    agendaCtrl.UpdateAppointmentData(appointment)
                Else
                    agendaCtrl.AddAppointmentData(appointment)
                End If

                'DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("RIS_AppointmentData_Add", portalId, cId, uId, subject, desc, startDate, startDate.AddHours(1), "BEGIN:VALARM  TRIGGER:-PT2880M  X-TELERIK-UID:0440c18f-ce88-4934-ac5e-5d134420d4c5  END:VALARM", "", Null.NullInteger, annot, "", cUId, cd)
                'DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("RIS_Client_History_Add", cId, hText.Replace("BR".ToUpper(), "<br /><br />"), uId, cd)

                If hText <> "" Then
                    Dim clientHistory As New Models.ClientHistory
                    Dim clientHistoryCtrl As New ClientHistoriesRepository

                    clientHistory.HistoryText = hText.Replace("BR".ToUpper(), "<br /><br />")
                    clientHistory.CreatedByUser = uId
                    clientHistory.CreatedOnDate = cd

                    clientHistoryCtrl.AddClientHistory(clientHistory)
                End If

                If emails.Length > 0 Then

                    Dim portalCtrl = New Portals.PortalController()
                    Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = Portals.PortalController.GetPortalSetting("RIS_StoreEmail", portalId, portalCtrl.GetPortal(portalId).Email), .DisplayName = portalCtrl.GetPortal(portalId).PortalName}
                    Dim _clientInfo As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = emails, .DisplayName = ""}
                    Dim _userInfo = Users.UserController.GetUserById(portalId, uId)

                    Notifications.SendStoreEmail(_userInfo, _clientInfo, Nothing, Nothing, subject, desc.Replace("BR", Environment.NewLine), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Text, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)

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
End Namespace