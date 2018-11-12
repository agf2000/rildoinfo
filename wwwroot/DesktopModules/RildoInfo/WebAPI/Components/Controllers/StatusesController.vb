
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Web.Api
Imports System.Net
Imports System.Net.Http
Imports RIW.Modules.WebAPI.Components.Repositories

Public Class StatusesController
    Inherits DnnApiController

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(StatusesController))

    ''' <summary>
    ''' Gets a list of statuses by portal id
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="isDeleted">Optional IsDeleted</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetStatuses(Optional portalId As Integer = 0, Optional isDeleted As String = "") As HttpResponseMessage
        Try
            Dim statusesData As IEnumerable(Of Components.Models.Status)
            Dim statusesDataCtrl As New StatusesRepository

            statusesData = statusesDataCtrl.GetStatuses(portalId, isDeleted)

            Return Request.CreateResponse(HttpStatusCode.OK, statusesData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets status
    ''' </summary>
    ''' <param name="statusId">Status ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetStatus(statusId As Integer) As HttpResponseMessage
        Try
            Dim statusData As New Components.Models.Status
            Dim statusDataCtrl As New StatusesRepository

            statusData = statusDataCtrl.GetStatus1(statusId, PortalController.Instance.GetCurrentPortalSettings().PortalId)

            Return Request.CreateResponse(HttpStatusCode.OK, statusData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds a new status
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function UpdateStatus(dto As Components.Models.Status) As HttpResponseMessage
        Try
            Dim status As New Components.Models.Status
            Dim statusCtrl As New StatusesRepository

            If dto.StatusId > 0 Then
                status = statusCtrl.GetStatus1(dto.StatusId, dto.PortalId)
            End If

            status.CreatedByUser = dto.CreatedByUser
            status.CreatedOnDate = dto.CreatedOnDate
            status.IsDeleted = dto.IsDeleted
            status.IsReadOnly = dto.IsReadOnly
            status.ModifiedByUser = dto.ModifiedByUser
            status.ModifiedOnDate = dto.ModifiedOnDate
            status.PortalId = dto.PortalId
            status.StatusColor = dto.StatusColor
            status.StatusTitle = dto.StatusTitle

            If dto.StatusId > 0 Then
                statusCtrl.UpdateStatus(status)
            Else
                statusCtrl.AddStatus(status)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .StatusId = status.StatusId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes status
    ''' </summary>
    ''' <param name="statusId">Status ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpDelete> _
    Function RemoveStatus(statusId As Integer) As HttpResponseMessage
        Try
            Dim statusCtrl As New StatusesRepository

            statusCtrl.RemoveStatus1(statusId, PortalController.Instance.GetCurrentPortalSettings().PortalId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

End Class
