
Imports System.Net
Imports System.Net.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation

Public Class PayConditionsController
    Inherits DnnApiController

    Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(AccountsController))

    ''' <summary>
    ''' Gets list of payment conditions by portal id
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="pcType">Payment Condition Type</param>
    ''' <param name="pcStart">Payment Condition Start Value</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function getPayConds(portalId As Integer, pcType As String, pcStart As Decimal) As HttpResponseMessage
        Try
            Dim payCondsData As IEnumerable(Of Models.PayCondition)
            Dim payCondsDatasCtrl As New PayConditionsRepository

            If pcType = "all" Then
                pcType = CStr(Null.NullInteger)
            End If

            payCondsData = payCondsDatasCtrl.GetPayConds(portalId, pcType, pcStart)

            Return Request.CreateResponse(HttpStatusCode.OK, payCondsData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets payment condition by id
    ''' </summary>
    ''' <param name="pcId">Pay Condition ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetPayCond(pcId As Integer) As HttpResponseMessage
        Try
            Dim payCondData As New Models.PayCondition
            Dim payCondDatasCtrl As New PayConditionsRepository

            payCondData = payCondDatasCtrl.GetPayCond(pcId, PortalController.GetCurrentPortalSettings().PortalId)

            Return Request.CreateResponse(HttpStatusCode.OK, payCondData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds payment condition
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function UpdatePayCond(dto As Models.PayCondition) As HttpResponseMessage
        Try
            Dim payCondData As New Models.PayCondition
            Dim payCondDatasCtrl As New PayConditionsRepository

            If dto.PayCondId > 0 Then
                payCondData = payCondDatasCtrl.GetPayCond(dto.PayCondId, dto.PortalId)
            End If

            payCondData.CreatedByUser = dto.CreatedByUser
            payCondData.CreatedOnDate = dto.CreatedOnDate
            If dto.GroupIds IsNot Nothing Then
                payCondData.GroupIds = dto.GroupIds
            End If
            payCondData.ModifiedByUser = dto.CreatedByUser
            payCondData.ModifiedOnDate = dto.CreatedOnDate
            payCondData.PayCondDisc = dto.PayCondDisc
            payCondData.PayCondIn = dto.PayIn
            payCondData.PayCondInterval = dto.PayCondInterval
            payCondData.PayCondN = dto.PayCondN
            payCondData.PayCondPerc = dto.PayCondPerc
            If dto.PayCondTitle IsNot Nothing Then
                payCondData.PayCondTitle = dto.PayCondTitle
            End If
            payCondData.PayCondType = dto.PayCondType
            payCondData.PayCondStart = dto.PayCondStart
            payCondData.PortalId = dto.PortalId

            If dto.PayCondId > 0 Then
                payCondDatasCtrl.UpdatePayCond(payCondData)
            Else
                payCondDatasCtrl.AddPayCond(payCondData)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .PayCondId = payCondData.PayCondId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes payment condition by id
    ''' </summary>
    ''' <param name="pcId">Payment Condition ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpDelete> _
    Function RemovePayCond(pcId As Integer) As HttpResponseMessage
        Try
            Dim payCondDatasCtrl As New PayConditionsRepository

            payCondDatasCtrl.RemovePayCond(pcId, PortalController.GetCurrentPortalSettings().PortalId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function


End Class
