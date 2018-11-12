
Imports System.Net
Imports System.Net.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Services.Social.Messaging.Internal
Imports DotNetNuke.Services.Social.Notifications
Imports RIW.Modules.WebAPI.Components.Repositories

Public Class MessagesController
    Inherits DnnApiController

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(MessagesController))

    ''' <summary>
    ''' Gets messages and notifications unread totals
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetTotals() As HttpResponseMessage
        Try
            Dim estimateCtrl As New EstimatesRepository
            Dim peopleCtrl As New PeopleRepository
            Dim portalId = PortalController.GetEffectivePortalId(UserController.Instance.GetCurrentUserInfo().PortalID)
            Dim totalsViewModel = New TotalsViewModel() With {
                .TotalUnreadMessages = InternalMessagingController.Instance.CountUnreadMessages(UserInfo.UserID, portalId),
                .TotalUnreadNotifications = NotificationsController.Instance.CountNotifications(UserInfo.UserID, portalId)}

            Dim roleCtrl As New DotNetNuke.Security.Roles.RoleController

            If Not UserController.Instance.GetCurrentUserInfo().IsInRole("Gerentes") Then
                totalsViewModel.TotalOpenedEstimates = estimateCtrl.GetEstimatesOpenedCount(portalId, UserController.Instance.GetCurrentUserInfo().UserID)
                totalsViewModel.TotalSales = estimateCtrl.GetEstimatesSoldCount(portalId, UserController.Instance.GetCurrentUserInfo().UserID)
                totalsViewModel.TotalClients = peopleCtrl.GetPeopleCount(portalId, UserController.Instance.GetCurrentUserInfo().UserID, roleCtrl.GetRoleByName(portalId, "Clientes").RoleID.ToString())
            Else
                totalsViewModel.TotalOpenedEstimates = estimateCtrl.GetEstimatesOpenedCount(portalId, -1)
                totalsViewModel.TotalSales = estimateCtrl.GetEstimatesSoldCount(portalId, -1)
                totalsViewModel.TotalClients = peopleCtrl.GetPeopleCount(portalId, -1, roleCtrl.GetRoleByName(portalId, "Clientes").RoleID.ToString())
            End If

            totalsViewModel.TotalDeletedClients = peopleCtrl.GetPeopleCount(portalId, -1, roleCtrl.GetRoleByName(portalId, "Clientes").RoleID.ToString(), True)

            Return Request.CreateResponse(HttpStatusCode.OK, totalsViewModel)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    Public Class TotalsViewModel
        Property TotalUnreadMessages As Integer
        Property TotalUnreadNotifications As Integer
        Property TotalOpenedEstimates As Integer
        Property TotalSales As Integer
        Property TotalClients As Integer
        Property TotalDeletedClients As Integer
    End Class

End Class
