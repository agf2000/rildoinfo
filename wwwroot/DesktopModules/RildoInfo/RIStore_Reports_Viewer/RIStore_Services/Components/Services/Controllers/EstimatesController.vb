
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation
Imports System.Globalization

Namespace RI.Modules.RIStore_Services
    Public Class EstimatesController
        Inherits DnnApiController

        Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(AccountsController))

        ''' <summary>
        ''' Gets a list of estimates by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="salesRep">Sales Rep. ID</param>
        ''' <param name="pageNumber">Page Number</param>
        ''' <param name="pageSize">Page Size</param>
        ''' <param name="orderBy">Order By</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetEstimates(ByVal portalId As String, ByVal clientId As String, ByVal salesRep As String, ByVal pageNumber As Integer, ByVal pageSize As Integer, orderBy As String) As HttpResponseMessage
            Try

                Dim searchStr As String = HttpContext.Current.Request.Params("filter[filters][0][value]")
                If searchStr Is Nothing Then
                    searchStr = ""
                End If

                Dim estimates As New List(Of Models.Estimate)
                Dim estimatesCtrl As New EstimatesRepository

                estimates = estimatesCtrl.GetEstimates(portalId, clientId, salesRep, searchStr, pageNumber, pageSize, orderBy)
                
                Dim total = Nothing
                For Each item In estimates
                    total = item.TotalRows
                    Exit For
                Next

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.data = estimates, .total = total})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets an estimate by estimate id
        ''' </summary>
        ''' <param name="eId">Estimate ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetEstimate(eId As Integer) As HttpResponseMessage
            Try
                Dim searchStr As String = HttpContext.Current.Request.Params("filter[filters][0][value]")

                Dim estimate As New Models.Estimate
                Dim estimateCtrl As New EstimatesRepository

                If searchStr <> "" Then
                    eId = CInt(searchStr)
                End If
                estimate = estimateCtrl.GetEstimate(eId, PortalController.GetCurrentPortalSettings().PortalId)

                Return Request.CreateResponse(HttpStatusCode.OK, estimate)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets list of estimate items by estimate id
        ''' </summary>
        ''' <param name="eId">Estimate ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetEstimateItems(eId As Integer) As HttpResponseMessage
            Try
                Dim estimateItems As New List(Of Models.EstimateItem)
                Dim estimateItemsCtrl As New EstimatesRepository

                estimateItems = estimateItemsCtrl.GetEstimateItems(eId)

                Return Request.CreateResponse(HttpStatusCode.OK, estimateItems)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Adds a quick estimate
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="clientId">Client Title</param>
        ''' <param name="estimateTitle">Estimate Title</param>
        ''' <param name="guid">GUID</param>
        ''' <param name="salesPerson">Sales Persom ID</param>
        ''' <param name="viewPrice">View Price Permission</param>
        ''' <param name="discount">Estimate Discount</param>
        ''' <param name="totalAmount">Total Estimate Amount</param>
        ''' <param name="createdByUser">Created By User User ID</param>
        ''' <param name="createdOnDate">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function AddEstimate(ByVal portalId As Integer, ByVal clientId As Integer, ByVal estimateTitle As String, ByVal guid As String, ByVal salesPerson As Integer, ByVal viewPrice As Boolean, ByVal discount As String, ByVal totalAmount As String, ByVal createdByUser As Integer, ByVal createdOnDate As Date) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim estimate As New Models.Estimate
                Dim estimateCtrl As New EstimatesRepository

                estimate.PortalId = portalId
                estimate.ClientId = clientId
                estimate.EstimateTitle = estimateTitle
                estimate.Guid = guid
                estimate.SalesRep = salesPerson
                estimate.ViewPrice = viewPrice
                estimate.Discount = Decimal.Parse(discount.Replace(".", ","), numInfo)
                estimate.TotalAmount = Decimal.Parse(totalAmount.Replace(".", ","), numInfo)
                estimate.CreatedByUser = createdByUser
                estimate.CreatedOnDate = createdOnDate

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .EstimateId = estimate.EstimateId})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Adds a quick estimate item
        ''' </summary>
        ''' <param name="eId">Estimate ID</param>
        ''' <param name="pId">Product ID</param>
        ''' <param name="qTy">Quantity</param>
        ''' <param name="uv">Price</param>
        ''' <param name="amt">Total Amount</param>
        ''' <param name="uId">Created By User User ID</param>
        ''' <param name="cd">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function AddEstimateItem(ByVal eId As Integer, ByVal pId As Integer, ByVal qTy As String, ByVal uv As String, ByVal amt As String, ByVal uId As Integer, ByVal cd As Date) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim estimateItem As New Models.EstimateItem
                Dim estimateCtrl As New EstimatesRepository

                estimateItem.EstimateId = eId
                estimateItem.ProdId = pId
                estimateItem.Qty = Decimal.Parse(qTy.Replace(".", ","), numInfo)
                estimateItem.EstProdOriginalPrice = Decimal.Parse(uv.Replace(".", ","), numInfo)
                estimateItem.EstProdPrice = Decimal.Parse(uv.Replace(".", ","), numInfo)
                estimateItem.CreatedByUser = uId
                estimateItem.CreatedOnDate = cd
                estimateCtrl.AddEstimateItem(estimateItem)

                Dim estimate As New Models.Estimate

                estimate = estimateCtrl.GetEstimate(eId, PortalController.GetCurrentPortalSettings().PortalId)
                estimate.TotalAmount = Decimal.Parse(amt.Replace(".", ","), numInfo)
                estimate.ModifiedByUser = uId
                estimate.ModifiedOnDate = cd

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .EstimateId = estimateItem.EstimateItemId})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Removes a quick estimate item
        ''' </summary>
        ''' <param name="eId">Estimate ID</param>
        ''' <param name="eItemId">Estimate Item ID</param>
        ''' <param name="pId">Product ID</param>
        ''' <param name="qTy">Quantity</param>
        ''' <param name="rId">Reason ID</param>
        ''' <param name="uId">Created By User ID</param>
        ''' <param name="cd">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function RemoveQItem(ByVal eId As Integer, ByVal eItemId As Integer, ByVal pId As Integer, ByVal qTy As String, ByVal rId As Integer, ByVal uId As Integer, ByVal cd As Date) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim estimateItem As New Models.EstimateRemovedItem
                Dim estimateCtrl As New EstimatesRepository

                estimateCtrl.RemoveEstimateItem(eItemId, eId)

                estimateItem.EstimateId = eId
                estimateItem.ProdId = pId
                estimateItem.Qty = Decimal.Parse(qTy.Replace(".", ","), numInfo)
                estimateItem.RemoveReasonId = rId
                estimateItem.CreatedByUser = uId
                estimateItem.CreatedOnDate = cd

                estimateCtrl.AddEstimateItemRemoved(estimateItem)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Updates a quick estimate item
        ''' </summary>
        ''' <param name="eId">Estimate ID</param>
        ''' <param name="eItemId">Estimate Item ID</param>
        ''' <param name="qTy">Quantity</param>
        ''' <param name="price">Product New Price</param>
        ''' <param name="pDisc">Product Discount %</param>
        ''' <param name="eDisc">Estimate Discount</param>
        ''' <param name="eAmount">Estimate Amount</param>
        ''' <param name="comments">Instructions</param>
        ''' <param name="uId">Modified By User ID</param>
        ''' <param name="md">Modified Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPut> _
        Function UpdateEstimateItem(ByVal eId As Integer, ByVal eItemId As Integer, ByVal qTy As String, ByVal price As String, ByVal pDisc As String, ByVal eDisc As String, ByVal eAmount As String, ByVal comments As String, ByVal uId As Integer, ByVal md As Date) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim estimateItem As New Models.EstimateItem
                Dim estimateCtrl As New EstimatesRepository

                estimateItem = estimateCtrl.GetEstimateItem(eItemId, eId)

                estimateItem.Qty = Decimal.Parse(qTy.Replace(".", ","), numInfo)
                estimateItem.EstProdPrice = Decimal.Parse(price.Replace(".", ","), numInfo)
                estimateItem.Discount = Decimal.Parse(pDisc.Replace(".", ","), numInfo)
                estimateItem.ModifiedByUser = uId
                estimateItem.ModifiedOnDate = md

                Dim estimate As New Models.Estimate

                estimate = estimateCtrl.GetEstimate(eId, PortalController.GetCurrentPortalSettings().PortalId)
                estimate.Discount = Decimal.Parse(eDisc.Replace(".", ","), numInfo)
                estimate.TotalAmount = Decimal.Parse(eAmount.Replace(".", ","), numInfo)
                estimate.PayCondType = ""
                estimate.PayCondN = 0
                estimate.PayCondPerc = 0
                estimate.PayCondIn = 0
                estimate.PayCondInst = 0
                estimate.PayCondInterval = 0
                estimate.TotalPayments = 0
                estimate.TotalPayCond = 0
                estimate.Comment = comments
                estimate.ModifiedByUser = uId
                estimate.ModifiedOnDate = md

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Updates estimate client id
        ''' </summary>
        ''' <param name="eId">Estimate ID</param>
        ''' <param name="cid">Client ID</param>
        ''' <param name="uId">Modified By User User ID</param>
        ''' <param name="md">Modified Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPut> _
        Function UpdateEstimateClient(eId As Integer, cid As Integer, uId As Integer, md As Date) As HttpResponseMessage
            Try
                Dim estimate As New Models.Estimate
                Dim estimateCtrl As New EstimatesRepository

                estimate = estimateCtrl.GetEstimate(eId, PortalController.GetCurrentPortalSettings().PortalId)

                estimate.ClientId = cid
                estimate.ModifiedByUser = uId
                estimate.ModifiedOnDate = md

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Updates estimate total amount
        ''' </summary>
        ''' <param name="eId">Estimate ID</param>
        ''' <param name="amt">Amount</param>
        ''' <param name="uId">Modified By User User ID</param>
        ''' <param name="md">Modified Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPut> _
        Function UpdateEstimateAmount(eId As Integer, amt As String, uId As Integer, md As Date) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim estimate As New Models.Estimate
                Dim estimateCtrl As New EstimatesRepository

                estimate = estimateCtrl.GetEstimate(eId, PortalController.GetCurrentPortalSettings().PortalId)

                estimate.TotalAmount = Decimal.Parse(amt.Replace(".", ","), numInfo)
                estimate.PayCondType = ""
                estimate.PayCondN = 0
                estimate.PayCondPerc = 0
                estimate.PayCondIn = 0
                estimate.PayCondInst = 0
                estimate.PayCondInterval = 0
                estimate.TotalPayments = 0
                estimate.TotalPayCond = 0
                estimate.ModifiedByUser = uId
                estimate.ModifiedOnDate = md

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function


        ''' <summary>
        ''' Updates estimate pay form and condition
        ''' </summary>
        ''' <param name="eId">Estimate ID</param>
        ''' <param name="pcType">Payment Type</param>
        ''' <param name="pcN">Number of Payments</param>
        ''' <param name="pcPerc">Interest</param>
        ''' <param name="pcIn">Down Paymnent</param>
        ''' <param name="pcInst">Payment</param>
        ''' <param name="pcInterval">Interval (days)</param>
        ''' <param name="totalPayments">Total Payments</param>
        ''' <param name="totalPC">Total Pay Condition</param>
        ''' <param name="uId">Creator ID</param>
        ''' <param name="md">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPut> _
        Function UpdateEstimatePayCond(ByVal eId As Integer, ByVal pcType As String, ByVal pcN As Integer, ByVal pcPerc As String, ByVal pcIn As String, ByVal pcInst As String, ByVal pcInterval As Integer, ByVal totalPayments As String, ByVal totalPC As String, ByVal uId As Integer, ByVal md As Date) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim estimate As New Models.Estimate
                Dim estimateCtrl As New EstimatesRepository

                estimate = estimateCtrl.GetEstimate(eId, PortalController.GetCurrentPortalSettings().PortalId)

                estimate.PayCondType = pcType
                estimate.PayCondN = pcN
                estimate.PayCondPerc = Decimal.Parse(pcPerc.Replace(".", ","), numInfo)
                estimate.PayCondIn = Decimal.Parse(pcIn.Replace(".", ","), numInfo)
                estimate.PayCondInst = Decimal.Parse(pcInst.Replace(".", ","), numInfo)
                estimate.PayCondInterval = pcInterval
                estimate.TotalPayments = Decimal.Parse(totalPayments.Replace(".", ","), numInfo)
                estimate.TotalPayCond = Decimal.Parse(totalPC.Replace(".", ","), numInfo)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Updates estimate status
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="eId">Estimate ID</param>
        ''' <param name="status">Status</param>
        ''' <param name="uId">Modified By User User ID</param>
        ''' <param name="md">Modified Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPut> _
        Function UpdateStatus(ByVal portalId As Integer, ByVal eId As Integer, ByVal status As String, ByVal uId As Integer, ByVal md As Date) As HttpResponseMessage
            Try
                Dim estimate As New Models.Estimate
                Dim estimateCtrl As New EstimatesRepository

                estimate = estimateCtrl.GetEstimate(eId, PortalController.GetCurrentPortalSettings().PortalId)

                estimate.StatusId = status

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Adds a quick sales
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="eId">Estimate ID</param>
        ''' <param name="totalCash">Total Cash Received</param>
        ''' <param name="totalCheck">Total Received in Check</param>
        ''' <param name="totalCard">Total Received in Credit Card</param>
        ''' <param name="totalBank">Total Bank Pay Created</param>
        ''' <param name="uId">Created By User User ID</param>
        ''' <param name="cd">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function AddCashier(ByVal portalId As Integer, ByVal eId As Integer, ByVal totalCash As String, ByVal totalCheck As String, totalCard As String, totalBank As String, ByVal uId As Integer, ByVal cd As Date) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim cashier As New Models.Cashier
                Dim cashierCtrl As New CashiersRepository

                cashier.CreatedByUser = uId
                cashier.CreatedOnDate = cd
                cashier.EstimateId = eId
                cashier.PortalId = portalId
                cashier.TotalBank = Decimal.Parse(totalBank.Replace(".", ","), numInfo)
                cashier.TotalCard = Decimal.Parse(totalCard.Replace(".", ","), numInfo)
                cashier.TotalCash = Decimal.Parse(totalCash.Replace(".", ","), numInfo)
                cashier.TotalCheck = Decimal.Parse(totalCheck.Replace(".", ","), numInfo)

                cashierCtrl.AddCashier(cashier)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Adds estimate history
        ''' </summary>
        ''' <param name="eId">Estimate ID</param>
        ''' <param name="hText">History Text</param>
        ''' <param name="uId">Created By User User ID</param>
        ''' <param name="cd">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function AddHistory(ByVal eId As Integer, ByVal hText As String, ByVal uId As Integer, ByVal cd As Date) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim estimateHistory As New Models.EstimateHistory
                Dim estimateCtrl As New EstimatesRepository

                estimateHistory.EstimateId = eId
                estimateHistory.HistoryText = hText
                estimateHistory.CreatedByUser = uId
                estimateHistory.CreatedOnDate = cd

                estimateCtrl.AddEstimateHistory(estimateHistory)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ' ''' <summary>
        ' ''' Removes product from stock
        ' ''' </summary>
        ' ''' <param name="prodId">Product ID</param>
        ' ''' <param name="qty">Quantity</param>
        ' ''' <param name="uId">Modified By User User ID</param>
        ' ''' <param name="md">Modified Date</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        '<DnnAuthorize> _
        '<HttpPut> _
        'Function TakeProductStock(ByVal prodId As Integer, ByVal qty As String, ByVal uId As Integer, ByVal md As Date) As HttpResponseMessage
        '    Try
        '        Dim culture = New CultureInfo("pt-BR")
        '        Dim numInfo = culture.NumberFormat

        '        Dim product As New Models.Product
        '        Dim productCtrl As New ProductsRepository

        '        product = productCtrl.GetProduct(prodId, PortalController.GetCurrentPortalSettings().PortalId)

        '        product.Stock = product.Stock - Decimal.Parse(qty.Replace(".", ","), numInfo)
        '        product.ModifiedByUser = uId
        '        product.ModifiedOnDate = md

        '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        '    Catch ex As Exception
        '        'DnnLog.Error(ex)
        '        Logger.[Error](ex)
        '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        '    End Try
        'End Function

        ' ''' <summary>
        ' ''' Returns product to stock
        ' ''' </summary>
        ' ''' <param name="prodId">Product ID</param>
        ' ''' <param name="qty">Quantity</param>
        ' ''' <param name="uId">Modified By User User ID</param>
        ' ''' <param name="md">Modified Date</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        '<DnnAuthorize> _
        '<HttpPut> _
        'Function ReturnProductStock(ByVal prodId As Integer, ByVal qty As String, ByVal uId As Integer, ByVal md As Date) As HttpResponseMessage
        '    Try
        '        Dim culture = New CultureInfo("pt-BR")
        '        Dim numInfo = culture.NumberFormat

        '        Dim product As New Models.Product
        '        Dim productCtrl As New ProductsRepository

        '        product = productCtrl.GetProduct(prodId, PortalController.GetCurrentPortalSettings().PortalId)

        '        product.Stock = product.Stock + Decimal.Parse(qty.Replace(".", ","), numInfo)
        '        product.ModifiedByUser = uId
        '        product.ModifiedOnDate = md

        '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        '    Catch ex As Exception
        '        'DnnLog.Error(ex)
        '        Logger.[Error](ex)
        '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        '    End Try
        'End Function

    End Class
End Namespace