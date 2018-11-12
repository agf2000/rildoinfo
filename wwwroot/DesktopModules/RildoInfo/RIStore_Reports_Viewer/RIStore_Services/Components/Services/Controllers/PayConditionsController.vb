
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation

Namespace RI.Modules.RIStore_Services
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
        Function GetPayConds(portalId As Integer, pcType As String, pcStart As Decimal) As HttpResponseMessage
            Try
                Dim payCondsData As New List(Of Models.PayCondition)
                Dim payCondsDatasCtrl As New PayConditionsRepository

                If pcType = "all" Then
                    pcType = CStr(Null.NullInteger)
                End If

                payCondsData = payCondsDatasCtrl.GetPayConds(portalId, CInt(pcType), pcStart)

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
        ''' <param name="payCondId">PayCondition ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="pcType">Pay Condition Type</param>
        ''' <param name="pcTitle">Payment Condition Title</param>
        ''' <param name="pcStart">Payment Condition apartir de</param>
        ''' <param name="pcN">Payment Condition number of payments</param>
        ''' <param name="pcPerc">Payment Condition interest</param>
        ''' <param name="pcIn">Payment Condition first installment</param>
        ''' <param name="pcDisc">Unknown</param>
        ''' <param name="pcInterval">Payment Condition days interval (0 para 30 mensal)</param>
        ''' <param name="pcGroupIds">Payment Condition special group</param>
        ''' <param name="uId">Creator ID</param>
        ''' <param name="cd">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function UpdatePayCond(portalId As Integer, pcType As Integer, pcStart As String, pcN As Integer, pcPerc As String, pcIn As String, pcDisc As String, pcInterval As Integer, uId As Integer, cd As Date, Optional ByVal payCondId As Integer = -1, Optional ByVal pcGroupIds As String = Nothing, Optional ByVal pcTitle As String = Nothing) As HttpResponseMessage
            Try
                Dim payCondData As New Models.PayCondition
                Dim payCondDatasCtrl As New PayConditionsRepository

                If payCondId > 0 Then
                    payCondData = payCondDatasCtrl.GetPayCond(payCondId, portalId)
                End If

                payCondData.CreatedByUser = uId
                payCondData.CreatedOnDate = cd
                If pcGroupIds IsNot Nothing Then
                    payCondData.GroupIDs = pcGroupIds
                End If
                payCondData.ModifiedByUser = uId
                payCondData.ModifiedOnDate = cd
                payCondData.PayCondDisc = pcDisc
                payCondData.PayCondIn = pcIn
                payCondData.PayCondInterval = pcInterval
                payCondData.PayCondN = pcN
                payCondData.PayCondPerc = pcPerc
                If pcTitle IsNot Nothing Then
                    payCondData.PayCondTitle = pcTitle
                End If
                payCondData.PayCondType = pcType
                payCondData.PayCondStart = pcStart
                payCondData.PortalId = portalId

                If payCondId > 0 Then
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
End Namespace