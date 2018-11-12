
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Web.Api
Imports System.Net
Imports System.Net.Http
Imports RIW.Modules.Common
Imports RIW.Modules.WebAPI.Components.Models
Imports RIW.Modules.WebAPI.Components.Repositories

Public Class CashiersController
    Inherits DnnApiController

#Region "Private Methods"

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(InvoicesController))

#End Region

    ''' <summary>
    ''' Get cashiers by portal id
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="sDate">Emission Date Start</param>
    ''' <param name="eDate">Emission Date End</param>
    ''' <param name="pageNumber">Page Number</param>
    ''' <param name="pageSize">How many pages</param>
    ''' <param name="orderBy">Field to order by</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetCashiers(Optional portalId As Integer = 0, Optional sDate As String = Nothing, Optional eDate As String = Nothing, Optional pageNumber As Integer = 1, Optional pageSize As Integer = 10, Optional orderBy As String = "", Optional orderDir As String = "") As HttpResponseMessage
        Try

            If sDate = Nothing Then
                sDate = Null.NullDate
            End If

            If eDate = Nothing Then
                eDate = Null.NullDate
            End If

            Dim cashiersCtrl As New CashiersRepository

            Dim cashiers = cashiersCtrl.GetCashiers(portalId, CDate(sDate), CDate(eDate), pageNumber, pageSize, orderBy, orderDir)

            Dim total = Nothing
            For Each item In cashiers
                total = item.TotalRows
                Exit For
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.data = cashiers, .total = total})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets cashier info by cashier id
    ''' </summary>
    ''' <param name="cashierId">Invoice ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetCashier(cashierId As Integer) As HttpResponseMessage
        Try
            Dim cashierCtrl As New InvoicesRepository

            Dim cashier = cashierCtrl.GetInvoice(cashierId)

            cashier.InvoiceItems = cashierCtrl.GetInvoiceItems(cashierId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .Cashier = cashier})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Ads or updates a cashier
    ''' </summary>
    ''' <param name="cashierDate">Cashier CreatedOnDate</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function ProcessCashier(cashierDate As Date, userId As Integer) As HttpResponseMessage
        Try
            Dim cashierCtrl As New CashiersRepository

            cashierCtrl.ProcessCashier(cashierDate, userId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes a cashier by cashier id
    ''' </summary>
    ''' <param name="cashierId">Cashier ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpDelete>
    Function RemoveCashier(cashierId As Integer) As HttpResponseMessage
        Try
            Dim cashierCtrl As New CashiersRepository

            cashierCtrl.RemoveCashier(cashierId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

End Class
