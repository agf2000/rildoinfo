
Imports System.Net
Imports System.Net.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation
Imports RIW.Modules.WebAPI.Components.Repositories

Public Class PayConditionsController
    Inherits DnnApiController

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(PayConditionsController))

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
    Function GetPayConds(portalId As Integer, pcType As Integer, pcStart As Single) As HttpResponseMessage
        Try
            Dim payCondsDatasCtrl As New PayConditionsRepository

            If pcStart < 0 Then
                pcStart = Null.NullSingle
            End If

            Dim payCondsData = payCondsDatasCtrl.GetPayConds(portalId, pcType, pcStart)

            Return Request.CreateResponse(HttpStatusCode.OK, payCondsData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
            Dim payCondData As New Components.Models.PayCondition
            Dim payCondDatasCtrl As New PayConditionsRepository

            payCondData = payCondDatasCtrl.GetPayCond(pcId, PortalController.Instance.GetCurrentPortalSettings().PortalId)

            Return Request.CreateResponse(HttpStatusCode.OK, payCondData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    Function UpdatePayCond(dto As Components.Models.PayCondition) As HttpResponseMessage
        Try
            Dim payCondData As New Components.Models.PayCondition
            Dim payCondDatasCtrl As New PayConditionsRepository

            If dto.PayCondId > 0 Then
                payCondData = payCondDatasCtrl.GetPayCond(dto.PayCondId, dto.PortalId)
            End If

            payCondData.CreatedByUser = dto.CreatedByUser
            payCondData.CreatedOnDate = dto.CreatedOnDate
            If dto.DiscountGroupId > 0 Then
                payCondData.DiscountGroupId = dto.DiscountGroupId
            End If
            payCondData.ModifiedByUser = dto.CreatedByUser
            payCondData.ModifiedOnDate = dto.CreatedOnDate
            payCondData.PayCondDisc = dto.PayCondDisc
            payCondData.PayCondIn = dto.PayCondIn
            payCondData.PayInDays = dto.PayInDays
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
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes payment condition by id
    ''' </summary>
    ''' <param name="payCondId">Payment Condition ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpDelete> _
    Function RemovePayCond(payCondId As Integer) As HttpResponseMessage
        Try
            Dim payCondDatasCtrl As New PayConditionsRepository

            payCondDatasCtrl.RemovePayCond(payCondId, PortalController.Instance.GetCurrentPortalSettings().PortalId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function


End Class
