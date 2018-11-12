
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Web.Api
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Net.Http.Formatting
Imports System.Web.Hosting
Imports Microsoft.AspNet.SignalR.Hubs
Imports Microsoft.AspNet.SignalR

Public Class EstimatesController
    'Inherits DnnApiControllerWithHub(Of EstimatesHub)
    Inherits DnnApiControllerWithHub
    'Inherits DnnApiController

    Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(AccountsController))

    ''' <summary>
    ''' Gets a list of estimates
    ''' </summary>
    ''' <param name="portalId"></param>
    ''' <param name="personId"></param>
    ''' <param name="salesRep"></param>
    ''' <param name="statusId"></param>
    ''' <param name="sDate"></param>
    ''' <param name="eDate"></param>
    ''' <param name="filter"></param>
    ''' <param name="isDeleted"></param>
    ''' <param name="pageIndex"></param>
    ''' <param name="pageSize"></param>
    ''' <param name="orderBy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetEstimates(salesRep As Integer,
            pageIndex As Integer,
            Optional portalId As Integer = 0,
            Optional personId As Integer = -1,
            Optional statusId As Integer = 0,
            Optional sDate As String = Nothing,
            Optional eDate As String = Nothing,
            Optional filter As String = "",
            Optional isDeleted As String = "",
            Optional pageSize As Integer = 10,
            Optional orderBy As String = "") As HttpResponseMessage
        Try
            Dim estimatesCtrl As New EstimatesRepository

            Dim searchStr As String = HttpContext.Current.Request.Params("filter[filters][0][value]")
            If searchStr IsNot Nothing Then
                filter = searchStr
            End If

            If sDate = Nothing Then
                sDate = CStr(Null.NullDate)
            End If

            If eDate = Nothing Then
                eDate = CStr(Null.NullDate)
            End If

            Dim estimates = estimatesCtrl.GetEstimates(portalId, personId, salesRep, statusId, sDate, eDate, filter.Replace("""", ""), isDeleted, pageIndex, pageSize, orderBy)

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
    ''' <param name="estimateId">Estimate ID</param>
    ''' <param name="portalId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetEstimate(estimateId As Integer, portalId As Integer) As HttpResponseMessage
        Try
            Dim searchStr As String = HttpContext.Current.Request.Params("filter[filters][0][value]")

            Dim estimate As New Models.Estimate
            Dim estimateCtrl As New EstimatesRepository

            If searchStr <> "" Then
                estimateId = CInt(searchStr)
            End If
            estimate = estimateCtrl.GetEstimate(estimateId, portalId)

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
    ''' <param name="estimateId">Estimate ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetEstimateItems(portalId As Integer, estimateId As Integer, lang As String) As HttpResponseMessage
        Try
            Dim estimateItemsCtrl As New EstimatesRepository

            Dim estimateItems = estimateItemsCtrl.GetEstimateItems(portalId, estimateId, lang)

            Return Request.CreateResponse(HttpStatusCode.OK, estimateItems)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds an estimate
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function addEstimate(dto As EstimateItemsRequest) As HttpResponseMessage
        Try
            Dim estimateHistory As New Models.EstimateHistory
            Dim estimate As New Models.Estimate
            'Dim estimateItems As New List(Of Models.EstimateItem)
            Dim estimateCtrl As New EstimatesRepository

            estimate.StatusId = dto.StatusId
            estimate.PortalId = dto.PortalId
            estimate.PersonId = dto.PersonId
            estimate.EstimateTitle = dto.EstimateTitle
            estimate.Guid = dto.Guid
            estimate.SalesRep = dto.SalesRep
            estimate.ViewPrice = dto.ViewPrice
            estimate.Discount = dto.Discount
            estimate.TotalAmount = dto.TotalAmount
            estimate.CreatedByUser = dto.CreatedByUser
            estimate.CreatedOnDate = dto.CreatedOnDate
            estimate.ModifiedByUser = dto.ModifiedByUser
            estimate.ModifiedOnDate = dto.ModifiedOnDate

            If Not estimate.StatusId > 0 Then
                Dim defaultStatus As New Models.Status
                Dim defaultStatusCtrl As New StatusesRepository

                defaultStatus = defaultStatusCtrl.getStatus("Normal", dto.PortalId)

                If defaultStatus IsNot Nothing Then
                    estimate.StatusId = defaultStatus.StatusId
                Else
                    estimate.StatusId = defaultStatusCtrl.getStatuses(dto.PortalId, "False")(0).StatusId
                End If
            End If

            estimateCtrl.AddEstimate(estimate)

            estimateHistory.EstimateId = estimate.EstimateId
            estimateHistory.HistoryText = "<p>Orçamento Inicializado</p>"
            estimateHistory.Locked = True
            estimateHistory.CreatedByUser = dto.ModifiedByUser
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushEstimate()
                'Hub.Clients.AllExcept(dto.ConnId).pushEstimate()

                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
            End If

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
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function addEstimateItem(dto As EstimateItemsRequest) As HttpResponseMessage
        Try
            Dim estimateHistory As New Models.EstimateHistory
            Dim estimate As New Models.Estimate
            Dim estimateItem As New Models.EstimateItem
            Dim productsCtrl As New ProductsRepository
            Dim estimateCtrl As New EstimatesRepository

            If Not dto.EstimateId > 0 Then
                estimate.StatusId = dto.StatusId
                estimate.PortalId = dto.PortalId
                estimate.PersonId = dto.PersonId
                estimate.EstimateTitle = dto.EstimateTitle
                estimate.Guid = dto.Guid
                estimate.SalesRep = dto.SalesRep
                estimate.ViewPrice = dto.ViewPrice
                estimate.Discount = 0
                estimate.TotalAmount = dto.TotalAmount
                estimate.CreatedByUser = dto.CreatedByUser
                estimate.CreatedOnDate = dto.CreatedOnDate
                estimate.ModifiedByUser = dto.ModifiedByUser
                estimate.ModifiedOnDate = dto.ModifiedOnDate

                If Not estimate.StatusId > 0 Then
                    Dim defaultStatus As New Models.Status
                    Dim defaultStatusCtrl As New StatusesRepository

                    defaultStatus = defaultStatusCtrl.getStatus("Normal", dto.PortalId)

                    If defaultStatus IsNot Nothing Then
                        estimate.StatusId = defaultStatus.StatusId
                    Else
                        estimate.StatusId = defaultStatusCtrl.getStatuses(dto.PortalId, "False")(0).StatusId
                    End If
                End If

                estimateCtrl.AddEstimate(estimate)

                estimateHistory.EstimateId = estimate.EstimateId
                estimateHistory.HistoryText = "<p>Orçamento Inicializado</p>"
                estimateHistory.Locked = True
                estimateHistory.CreatedByUser = dto.CreatedByUser
                estimateHistory.CreatedOnDate = dto.CreatedOnDate

                estimateCtrl.AddEstimateHistory(estimateHistory)
            Else
                estimate.EstimateId = dto.EstimateId
            End If

            If dto.EstimateItems.Count > 0 Then

                For Each item In dto.EstimateItems

                    estimateItem.EstimateId = estimate.EstimateId
                    estimateItem.ProductId = item.ProductId
                    estimateItem.ProductQty = item.ProductQty
                    estimateItem.ProductEstimateOriginalPrice = item.ProductEstimateOriginalPrice
                    estimateItem.ProductEstimatePrice = item.ProductEstimatePrice
                    estimateItem.CreatedByUser = item.CreatedByUser
                    estimateItem.CreatedOnDate = item.CreatedOnDate
                    estimateItem.ModifiedByUser = item.ModifiedByUser
                    estimateItem.ModifiedOnDate = item.ModifiedOnDate
                    estimateCtrl.AddEstimateItem(estimateItem)

                Next

            End If

            If dto.EstimateId > 0 Then
                estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId)
                estimate.TotalAmount = dto.TotalAmount
                estimate.ModifiedByUser = dto.ModifiedByUser
                estimate.ModifiedOnDate = dto.ModifiedOnDate
                estimateCtrl.updateEstimate(estimate)
            End If

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushEstimate()
                'Hub.Clients.AllExcept(dto.ConnId).pushEstimate()

                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .EstimateItem = estimateItem})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds a quick estimate item
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function addEstimateItems(dto As EstimateItemsRequest) As HttpResponseMessage
        Try
            Dim estimateHistory As New Models.EstimateHistory
            Dim estimate As New Models.Estimate
            Dim estimateItem As New Models.EstimateItem
            Dim productsCtrl As New ProductsRepository
            Dim estimateCtrl As New EstimatesRepository

            For Each item In dto.EstimateItems

                estimateItem.EstimateId = item.EstimateId
                estimateItem.ProductId = item.ProductId
                estimateItem.ProductQty = item.ProductQty
                estimateItem.ProductEstimateOriginalPrice = item.ProductEstimateOriginalPrice
                estimateItem.ProductEstimatePrice = item.ProductEstimatePrice
                estimateItem.CreatedByUser = item.CreatedByUser
                estimateItem.CreatedOnDate = item.CreatedOnDate
                estimateItem.ModifiedByUser = item.ModifiedByUser
                estimateItem.ModifiedOnDate = item.ModifiedOnDate
                estimateCtrl.AddEstimateItem(estimateItem)

                estimateHistory.EstimateId = item.EstimateId
                estimateHistory.HistoryText = String.Format("<p>Item ""{0}"" inserido (Qde: {1}).</p>", item.ProductName, item.ProductQty)
                estimateHistory.Locked = True
                estimateHistory.CreatedByUser = item.CreatedByUser
                estimateHistory.CreatedOnDate = item.CreatedOnDate

                estimateCtrl.AddEstimateHistory(estimateHistory)

            Next

            If dto.EstimateId > 0 Then
                estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId)
                estimate.TotalAmount = dto.TotalAmount
                estimate.ModifiedByUser = dto.ModifiedByUser
                estimate.ModifiedOnDate = dto.ModifiedOnDate
                estimateCtrl.updateEstimate(estimate)
            End If

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushEstimate()
                'Hub.Clients.AllExcept(dto.ConnId).pushEstimate()

                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes an estimate item
    ''' </summary>
    ''' <param name="jsonData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function removeEstimateItems(jsonData As EstimateItemsRequest) As HttpResponseMessage
        Try
            Dim estimateHistory As New Models.EstimateHistory
            Dim estimateRemovedItem As New Models.EstimateItemRemoved
            Dim estimateCtrl As New EstimatesRepository

            For Each item In jsonData.EstimateItemsRemoved
                estimateRemovedItem.ProductId = item.ProductId
                estimateRemovedItem.ProductQty = item.ProductQty
                estimateRemovedItem.RemoveReasonId = item.RemoveReasonId
                estimateRemovedItem.EstimateId = item.EstimateId
                estimateRemovedItem.CreatedByUser = item.CreatedByUser
                estimateRemovedItem.CreatedOnDate = item.CreatedOnDate

                estimateCtrl.AddEstimateItemRemoved(estimateRemovedItem)

                estimateCtrl.RemoveEstimateItem(item.EstimateItemId, item.EstimateId)

                estimateHistory.EstimateId = item.EstimateId
                estimateHistory.HistoryText = String.Format("<p>Item ""{0}"" removido (Qde: {1}).</p>", item.ProductName, item.ProductQty)
                estimateHistory.Locked = True
                estimateHistory.CreatedByUser = 0
                estimateHistory.CreatedOnDate = item.CreatedOnDate

                estimateCtrl.AddEstimateHistory(estimateHistory)
            Next

            Dim estimate As New Models.Estimate

            estimate = estimateCtrl.GetEstimate(jsonData.EstimateId, jsonData.PortalId)
            estimate.Discount = jsonData.Discount
            estimate.TotalAmount = jsonData.TotalAmount
            'estimate.PayCondType = ""
            'estimate.PayCondN = 0
            'estimate.PayCondPerc = 0
            'estimate.PayCondIn = 0
            'estimate.PayCondInst = 0
            'estimate.PayCondInterval = 0
            'estimate.TotalPayments = 0
            'estimate.TotalPayCond = 0
            estimate.Comment = jsonData.Comment
            estimate.ModifiedByUser = jsonData.ModifiedByUser
            estimate.ModifiedOnDate = jsonData.ModifiedOnDate

            estimateCtrl.updateEstimate(estimate)

            If jsonData.ConnId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(jsonData.ConnId).pushEstimate()
                'Hub.Clients.AllExcept(jsonData.ConnId).pushEstimate()

                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(jsonData.ConnId).pushHistory(estimateHistory)
            End If

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
    ''' <param name="jsonData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPut> _
    Function updateEstimateItems(jsonData As EstimateItemsRequest) As HttpResponseMessage ' estimateItems As List(Of Models.EstimateItem), estimateId As Integer) As HttpResponseMessage
        Try
            Dim estimate As New Models.Estimate
            Dim estimateHistory As New Models.EstimateHistory
            Dim estimateItem As New Models.EstimateItem
            Dim estimateCtrl As New EstimatesRepository

            For Each item In jsonData.EstimateItems
                estimateItem = estimateCtrl.GetEstimateItem(item.EstimateItemId, item.EstimateId)

                estimateItem.ProductQty = item.ProductQty
                estimateItem.ProductEstimatePrice = item.ProductEstimatePrice
                estimateItem.ProductDiscount = item.ProductDiscount
                estimateItem.ModifiedByUser = item.ModifiedByUser
                estimateItem.ModifiedOnDate = item.ModifiedOnDate

                estimateCtrl.updateEstimateItem(estimateItem)

                estimateHistory.EstimateId = item.EstimateId
                estimateHistory.HistoryText = String.Format("<p>Item ""{0}"" atualizado (Qde: {1}).</p>", item.ProductName, item.ProductQty)
                estimateHistory.Locked = True
                estimateHistory.CreatedByUser = 0
                estimateHistory.CreatedOnDate = item.ModifiedOnDate

                estimateCtrl.AddEstimateHistory(estimateHistory)
            Next

            estimate = estimateCtrl.GetEstimate(jsonData.EstimateId, jsonData.PortalId)
            estimate.EstimateTitle = jsonData.EstimateTitle
            estimate.Discount = jsonData.Discount
            estimate.TotalAmount = jsonData.TotalAmount
            'estimate.PayCondType = ""
            'estimate.PayCondN = 0
            'estimate.PayCondPerc = 0
            'estimate.PayCondIn = 0
            'estimate.PayCondInst = 0
            'estimate.PayCondInterval = 0
            'estimate.TotalPayments = 0
            'estimate.TotalPayCond = 0
            estimate.Comment = jsonData.Comment
            estimate.ModifiedByUser = jsonData.ModifiedByUser
            estimate.ModifiedOnDate = jsonData.ModifiedOnDate

            estimateCtrl.updateEstimate(estimate)

            If jsonData.ConnId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(jsonData.ConnId).pushEstimate()
                'Hub.Clients.AllExcept(jsonData.ConnId).pushEstimate()

                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(jsonData.ConnId).pushHistory(estimateHistory)
            End If

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
    ''' <param name="jsonData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPut> _
    Function syncEstimateItems(jsonData As EstimateItemsRequest) As HttpResponseMessage ' estimateItems As List(Of Models.EstimateItem), estimateId As Integer) As HttpResponseMessage
        Try
            Dim estimateHistory As New Models.EstimateHistory
            Dim estimateItem As New Models.EstimateItem
            Dim estimateCtrl As New EstimatesRepository

            For Each item In jsonData.EstimateItems
                estimateItem = estimateCtrl.GetEstimateItem(item.EstimateItemId, item.EstimateId)

                estimateItem.ProductQty = item.ProductQty
                estimateItem.ProductEstimateOriginalPrice = item.ProductEstimateOriginalPrice
                estimateItem.ProductEstimatePrice = item.ProductEstimatePrice
                estimateItem.ProductDiscount = item.ProductDiscount
                estimateItem.ModifiedByUser = item.ModifiedByUser
                estimateItem.ModifiedOnDate = item.ModifiedOnDate

                estimateCtrl.updateEstimateItem(estimateItem)

                estimateHistory.EstimateId = item.EstimateId
                estimateHistory.HistoryText = String.Format("<p>Preço no item ""{0}"" sincronizado.</p>", item.ProductName)
                estimateHistory.Locked = True
                estimateHistory.CreatedByUser = 0
                estimateHistory.CreatedOnDate = item.ModifiedOnDate

                estimateCtrl.AddEstimateHistory(estimateHistory)
            Next

            Dim estimate As New Models.Estimate

            estimate = estimateCtrl.GetEstimate(jsonData.EstimateId, jsonData.PortalId)
            estimate.Discount = jsonData.Discount
            estimate.TotalAmount = jsonData.TotalAmount
            'estimate.PayCondType = ""
            'estimate.PayCondN = 0
            'estimate.PayCondPerc = 0
            'estimate.PayCondIn = 0
            'estimate.PayCondInst = 0
            'estimate.PayCondInterval = 0
            'estimate.TotalPayments = 0
            'estimate.TotalPayCond = 0
            estimate.Comment = jsonData.Comment
            estimate.ModifiedByUser = jsonData.ModifiedByUser
            estimate.ModifiedOnDate = jsonData.ModifiedOnDate

            estimateCtrl.updateEstimate(estimate)

            '' SignalR
            estimatesHub.Value.Clients.AllExcept(jsonData.ConnId).pushEstimate()
            'Hub.Clients.AllExcept(jsonData.ConnId).pushEstimate()

            '' SignalR
            'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
            estimatesHub.Value.Clients.AllExcept(jsonData.ConnId).pushHistory(estimateHistory)

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
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPut> _
    Function updateEstimateClient(dto As Models.Estimate) As HttpResponseMessage
        Try
            Dim estimate As New Models.Estimate
            Dim estimateCtrl As New EstimatesRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, PortalController.GetCurrentPortalSettings().PortalId)

            estimate.PersonId = dto.PersonId
            estimate.ModifiedByUser = dto.ModifiedByUser
            estimate.ModifiedOnDate = dto.ModifiedOnDate

            estimateCtrl.updateEstimate(estimate)

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
            'estimate.PayCondType = ""
            'estimate.PayCondN = 0
            'estimate.PayCondPerc = 0
            'estimate.PayCondIn = 0
            'estimate.PayCondInst = 0
            'estimate.PayCondInterval = 0
            'estimate.TotalPayments = 0
            'estimate.TotalPayCond = 0
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
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPut> _
    Function updateEstimateTerm(dto As Models.Estimate) As HttpResponseMessage
        Try
            Dim estimateHistory As New Models.EstimateHistory
            Dim estimate As New Models.Estimate
            Dim estimateCtrl As New EstimatesRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId)

            estimate.Inst = dto.Inst.Replace(vbCrLf, "<br/>")
            estimate.ModifiedByUser = dto.ModifiedByUser
            estimate.ModifiedOnDate = dto.ModifiedOnDate

            estimateCtrl.updateEstimate(estimate)

            estimateHistory.EstimateId = estimate.EstimateId
            estimateHistory.HistoryText = "<p>Termo do orçamento atualizado.</p>"
            estimateHistory.Locked = False
            estimateHistory.CreatedByUser = dto.ModifiedByUser
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            '' SignalR
            estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushNewTerm(dto.Inst)
            'Hub.Clients.AllExcept(dto.ConnId).pushPayCondition()

            '' SignalR
            'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
            estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)

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
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPut> _
    Function updateEstimateConfig(dto As Models.Estimate) As HttpResponseMessage
        Try
            Dim estimateHistory As New Models.EstimateHistory
            Dim estimate As New Models.Estimate
            Dim estimateCtrl As New EstimatesRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId)

            estimate.SalesRep = dto.SalesRep
            estimate.Discount = dto.Discount
            estimate.StatusId = dto.StatusId
            estimate.ViewPrice = dto.ViewPrice
            estimate.Locked = dto.Locked
            estimate.ModifiedByUser = dto.ModifiedByUser
            estimate.ModifiedOnDate = dto.ModifiedOnDate

            estimateCtrl.updateEstimate(estimate)

            estimateHistory.EstimateId = estimate.EstimateId
            estimateHistory.HistoryText = "<p>Configuração do orçamento atualizada.</p>"
            estimateHistory.Locked = False
            estimateHistory.CreatedByUser = dto.ModifiedByUser
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            '' SignalR
            estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushConfig()
            'Hub.Clients.AllExcept(dto.ConnId).pushPayCondition()

            '' SignalR
            'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
            estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)

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
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPut> _
    Function updateEstimatePayCond(dto As Models.Estimate) As HttpResponseMessage
        Try
            Dim estimateHistory As New Models.EstimateHistory
            Dim estimate As New Models.Estimate
            Dim estimateCtrl As New EstimatesRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId)

            estimate.PayCondType = dto.PayCondType
            estimate.PayCondN = dto.PayCondN
            estimate.PayCondPerc = dto.PayCondPerc
            estimate.PayCondIn = dto.PayCondIn
            estimate.PayCondInst = dto.PayCondInst
            estimate.PayCondInterval = dto.PayCondInterval
            estimate.TotalPayments = dto.TotalPayments
            estimate.TotalPayCond = dto.TotalPayCond
            estimate.ModifiedByUser = dto.ModifiedByUser
            estimate.ModifiedOnDate = dto.ModifiedOnDate

            estimateCtrl.updateEstimate(estimate)

            estimateHistory.EstimateId = estimate.EstimateId
            estimateHistory.HistoryText = "<p>Condição de pagamento atuzaliada.</p>"
            estimateHistory.Locked = True
            estimateHistory.CreatedByUser = 0
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushPayCondition()
                'Hub.Clients.AllExcept(dto.ConnId).pushPayCondition()

                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
            End If

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
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPut> _
    Function cancelEstimatePayCond(dto As Models.Estimate) As HttpResponseMessage
        Try
            Dim estimateHistory As New Models.EstimateHistory
            Dim estimate As New Models.Estimate
            Dim estimateCtrl As New EstimatesRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId)

            estimate.PayCondType = dto.PayCondType
            estimate.PayCondN = dto.PayCondN
            estimate.PayCondPerc = dto.PayCondPerc
            estimate.PayCondIn = dto.PayCondIn
            estimate.PayCondInst = dto.PayCondInst
            estimate.PayCondInterval = dto.PayCondInterval
            estimate.TotalPayments = dto.TotalPayments
            estimate.TotalPayCond = dto.TotalPayCond
            estimate.ModifiedByUser = dto.ModifiedByUser
            estimate.ModifiedOnDate = dto.ModifiedOnDate

            estimateCtrl.updateEstimate(estimate)

            estimateHistory.EstimateId = estimate.EstimateId
            estimateHistory.HistoryText = "<p>Condição de pagamento cancelada.</p>"
            estimateHistory.Locked = True
            estimateHistory.CreatedByUser = 0
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()

                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
            End If

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
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPut> _
    Function updateStatus(dto As Models.Estimate) As HttpResponseMessage
        Try
            Dim estimate As New Models.Estimate
            Dim estimateCtrl As New EstimatesRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, PortalController.GetCurrentPortalSettings().PortalId)

            estimate.StatusId = dto.StatusId
            estimate.ModifiedByUser = dto.ModifiedByUser
            estimate.ModifiedOnDate = dto.ModifiedOnDate

            estimateCtrl.updateEstimate(estimate)

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
    Function AddCashier(portalId As Integer, eId As Integer, totalCash As String, totalCheck As String, totalCard As String, totalBank As String, uId As Integer, cd As Date) As HttpResponseMessage
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
    ''' Gets list estimate history
    ''' </summary>
    ''' <param name="estimateId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function getHistories(estimateId As Integer) As HttpResponseMessage
        Try
            Dim estimateCtrl As New EstimatesRepository

            Dim histories As New List(Of Models.EstimateHistory)

            Dim estimateHistories = estimateCtrl.GetEstimateHistories(estimateId)

            For Each history In estimateHistories
                Dim estimateHistoryComments = estimateCtrl.getEstimateHistoryComments(history.EstimateHistoryId)
                history.HistoryComments = estimateHistoryComments
                histories.Add(history)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, histories)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds estimate history
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function addHistory(dto As Models.EstimateHistory) As HttpResponseMessage
        Try
            Dim estimateHistory As New Models.EstimateHistory
            Dim estimateCtrl As New EstimatesRepository

            estimateHistory.EstimateId = dto.EstimateId
            estimateHistory.HistoryText = dto.HistoryText
            estimateHistory.Locked = dto.Locked
            estimateHistory.CreatedByUser = dto.CreatedByUser
            estimateHistory.CreatedOnDate = dto.CreatedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            Dim _userInfo = Users.UserController.GetUserById(Portals.PortalController.GetCurrentPortalSettings().PortalId, estimateHistory.CreatedByUser)
            estimateHistory.DisplayName = _userInfo.DisplayName
            If _userInfo.Profile.Photo <> "" Then
                estimateHistory.Avatar = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(_userInfo).FolderPath
                estimateHistory.Avatar = estimateHistory.Avatar & DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(_userInfo.Profile.Photo).FileName
            End If

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .EstimateHistory = estimateHistory})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets list estimate history
    ''' </summary>
    ''' <param name="estimateId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function getMessages(estimateId As Integer) As HttpResponseMessage
        Try
            Dim estimateCtrl As New EstimatesRepository

            Dim messages As New List(Of Models.EstimateMessage)

            Dim estimateMessages = estimateCtrl.GetEstimateMessages(estimateId)

            For Each message In estimateMessages
                Dim estimateMessageComments = estimateCtrl.getEstimateMessageComments(message.EstimateMessageId)
                message.MessageComments = estimateMessageComments
                messages.Add(message)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, messages)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates estimate message
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function updateMessage(dto As Models.EstimateMessage) As HttpResponseMessage
        Try
            Dim estimateMessage As New Models.EstimateMessage
            Dim estimateCtrl As New EstimatesRepository

            If dto.EstimateMessageId > 0 Then
                estimateMessage = estimateCtrl.GetEstimateMessage(dto.EstimateMessageId, dto.EstimateId)
            End If

            estimateMessage.EstimateId = dto.EstimateId
            estimateMessage.Allowed = dto.Allowed
            estimateMessage.MessageText = dto.MessageText
            estimateMessage.CreatedByUser = dto.CreatedByUser
            estimateMessage.CreatedOnDate = dto.CreatedOnDate
            estimateMessage.ModifiedByUser = dto.CreatedByUser
            estimateMessage.ModifiedOnDate = dto.CreatedOnDate

            If dto.EstimateMessageId > 0 Then
                estimateCtrl.updateEstimateMessage(estimateMessage)
            Else
                estimateCtrl.addEstimateMessage(estimateMessage)
            End If

            Dim _userInfo = Users.UserController.GetUserById(Portals.PortalController.GetCurrentPortalSettings().PortalId, estimateMessage.CreatedByUser)
            estimateMessage.DisplayName = _userInfo.DisplayName
            If _userInfo.Profile.Photo <> "" Then
                estimateMessage.Avatar = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(_userInfo).FolderPath
                estimateMessage.Avatar = estimateMessage.Avatar & DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(_userInfo.Profile.Photo).FileName
            End If

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushMessage(estimateMessage)
                'Hub.Clients.AllExcept(MessageComment.connId).pushComment(estimateMessageComment, MessageComment.messageIndex)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .EstimateMessage = estimateMessage})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes estimate message
    ''' </summary>
    ''' <param name="estimateMessageId"></param>
    ''' <param name="estimateId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpDelete> _
    Function removeMessage(estimateMessageId As Integer, estimateId As Integer, connId As String) As HttpResponseMessage
        Try
            Dim estimateMessageCtrl As New EstimatesRepository

            estimateMessageCtrl.removeEstimateMessage(estimateMessageId, estimateId)

            If connId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(connId).removeMessage(estimateMessageId, estimateId)
                'Hub.Clients.AllExcept(MessageComment.connId).pushComment(estimateMessageComment, MessageComment.messageIndex)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates estimate message comment
    ''' </summary>
    ''' <param name="MessageComment"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function updateMessageComment(MessageComment As MessageComment) As HttpResponseMessage
        Try
            Dim estimateMessageComment As New Models.EstimateMessageComment
            Dim estimateCtrl As New EstimatesRepository

            If MessageComment.dto.CommentId > 0 Then
                estimateMessageComment = estimateCtrl.getEstimateMessageComment(MessageComment.dto.CommentId, MessageComment.dto.EstimateMessageId)
            End If

            estimateMessageComment.EstimateMessageId = MessageComment.dto.EstimateMessageId
            estimateMessageComment.CommentText = MessageComment.dto.CommentText
            estimateMessageComment.CreatedByUser = MessageComment.dto.CreatedByUser
            estimateMessageComment.CreatedOnDate = MessageComment.dto.CreatedOnDate

            Dim _userInfo = Users.UserController.GetUserById(Portals.PortalController.GetCurrentPortalSettings().PortalId, estimateMessageComment.CreatedByUser)
            estimateMessageComment.DisplayName = _userInfo.DisplayName
            If _userInfo.Profile.Photo <> "" Then
                estimateMessageComment.Avatar = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(_userInfo).FolderPath
                estimateMessageComment.Avatar = estimateMessageComment.Avatar & DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(_userInfo.Profile.Photo).FileName
            End If

            If MessageComment.dto.CommentId > 0 Then
                estimateCtrl.updateEstimateMessageComment(estimateMessageComment)
            Else
                estimateCtrl.addEstimateMessageComment(estimateMessageComment)
            End If

            If MessageComment.connId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(MessageComment.connId).pushMessageComment(estimateMessageComment, MessageComment.messageIndex)
                'Hub.Clients.AllExcept(MessageComment.connId).pushComment(estimateMessageComment, MessageComment.messageIndex)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .EstimateMessageComment = estimateMessageComment})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes estimate message comment
    ''' </summary>
    ''' <param name="commentId"></param>
    ''' <param name="estimateMessageId"></param>
    ''' <param name="connId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpDelete> _
    Function removeMessageComment(commentId As Integer, estimateMessageId As Integer, connId As String) As HttpResponseMessage
        Try
            Dim estimateMessageCommentCtrl As New EstimatesRepository

            estimateMessageCommentCtrl.removeEstimateMessageComment(commentId, estimateMessageId)

            If connId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(connId).removeMessageComment(commentId, estimateMessageId)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates estimate message comment
    ''' </summary>
    ''' <param name="historyComment"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function updateHistoryComment(historyComment As HistoryComment) As HttpResponseMessage
        Try
            Dim estimateHistoryComment As New Models.EstimateHistoryComment
            Dim estimateCtrl As New EstimatesRepository

            If historyComment.dto.CommentId > 0 Then
                estimateHistoryComment = estimateCtrl.getEstimateHistoryComment(historyComment.dto.CommentId, historyComment.dto.EstimateHistoryId)
            End If

            estimateHistoryComment.EstimateHistoryId = historyComment.dto.EstimateHistoryId
            estimateHistoryComment.CommentText = historyComment.dto.CommentText
            estimateHistoryComment.CreatedByUser = historyComment.dto.CreatedByUser
            estimateHistoryComment.CreatedOnDate = historyComment.dto.CreatedOnDate

            Dim _userInfo = Users.UserController.GetUserById(Portals.PortalController.GetCurrentPortalSettings().PortalId, estimateHistoryComment.CreatedByUser)
            estimateHistoryComment.DisplayName = _userInfo.DisplayName
            If _userInfo.Profile.Photo <> "" Then
                estimateHistoryComment.Avatar = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(_userInfo).FolderPath
                estimateHistoryComment.Avatar = estimateHistoryComment.Avatar & DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(_userInfo.Profile.Photo).FileName
            End If

            If historyComment.dto.CommentId > 0 Then
                estimateCtrl.updateEstimateHistoryComment(estimateHistoryComment)
            Else
                estimateCtrl.addEstimateHistoryComment(estimateHistoryComment)
            End If

            If historyComment.connId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(historyComment.connId).pushHistoryComment(estimateHistoryComment, historyComment.messageIndex)
                'Hub.Clients.AllExcept(MessageComment.connId).pushComment(estimateMessageComment, MessageComment.messageIndex)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .EstimateHistoryComment = estimateHistoryComment})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes estimate message comment
    ''' </summary>
    ''' <param name="commentId"></param>
    ''' <param name="estimateHistoryId"></param>
    ''' <param name="connId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpDelete> _
    Function removeHistoryComment(commentId As Integer, estimateHistoryId As Integer, connId As String) As HttpResponseMessage
        Try
            Dim estimateHistoryCommentCtrl As New EstimatesRepository

            estimateHistoryCommentCtrl.removeEstimateHistoryComment(commentId, estimateHistoryId)

            If connId IsNot Nothing Then
                '' SignalR
                estimatesHub.Value.Clients.AllExcept(connId).removeHistoryComment(commentId, estimateHistoryId)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes product from stock
    ''' </summary>
    ''' <param name="products"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPut> _
    Function takeProductStock(products As List(Of Models.Product)) As HttpResponseMessage
        Try
            Dim product As New Models.Product
            Dim productCtrl As New ProductsRepository

            For Each item In products
                product = productCtrl.getProduct(item.ProductId, "pt-BR")

                Dim oldStock = product.QtyStockSet

                product.QtyStockSet = oldStock - item.QtyStockSet
                product.ModifiedByUser = item.ModifiedByUser
                product.ModifiedOnDate = item.ModifiedOnDate

                productCtrl.updateProduct(product)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Returns product to stock
    ''' </summary>
    ''' <param name="products"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPut> _
    Function returnProductStock(products As List(Of Models.Product)) As HttpResponseMessage
        Try
            Dim product As New Models.Product
            Dim productCtrl As New ProductsRepository

            For Each item In products
                product = productCtrl.getProduct(item.ProductId, "pt-BR")

                Dim oldStock = product.QtyStockSet

                product.QtyStockSet = oldStock + item.QtyStockSet
                product.ModifiedByUser = item.ModifiedByUser
                product.ModifiedOnDate = item.ModifiedOnDate

                productCtrl.updateProduct(product)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Downloads estimate pdf
    ''' </summary>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function downloadEstimatePdf(dto As EstimatePdf) As HttpResponseMessage
        Try
            Dim estimate As New Models.Estimate
            Dim estimateItems As New List(Of Models.EstimateItem)
            Dim estimateCtrl As New EstimatesRepository
            Dim payCondCtrl As New PayConditionsRepository
            Dim productCtrl As New ProductsRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId)

            Dim heading12 As Font = FontFactory.GetFont("ARIAL", 12)
            Dim heading10 As Font = FontFactory.GetFont("ARIAL", 10)
            Dim heading9 As Font = FontFactory.GetFont("VERDANA", 9)
            Dim heading8 As Font = FontFactory.GetFont("VERDANA", 8)
            Dim heading8b As Font = FontFactory.GetFont("VERDANA", 8, Font.BOLD)
            Dim heading7 As Font = FontFactory.GetFont("VERDANA", 7)

            'Dim str As New MemoryStream()

            Dim pdfFile = HostingEnvironment.MapPath("\Portals\0\Downloads\" & String.Format("Orcamento_{0}.pdf", CStr(dto.EstimateId)))

            Dim thePdf As New Document(PageSize.A4, 20, 20, 100, 15)

            Dim file As New System.IO.FileStream(pdfFile, System.IO.FileMode.OpenOrCreate)

            Dim writer As PdfWriter = PdfWriter.GetInstance(thePdf, file)
            'Dim writer As PdfWriter = PdfWriter.GetInstance(thePdf, str)

            Dim page As pdfPage = New pdfPage()
            writer.PageEvent = page

            thePdf.Open()

            Dim clientTable = New PdfPTable(2) With {.TotalWidth = 555.0F, .HorizontalAlignment = 0, .LockedWidth = True}
            Dim widthsHeaderTable As Single() = New Single() {1.0F, 1.0F}
            clientTable.SetWidths(widthsHeaderTable)

            Dim clientInfoHeader As New Paragraph("Informações do Cliente", heading10)

            Dim clientInfoLeft As New Paragraph(String.Format("Cliente: {0}", estimate.ClientDisplayName), heading8)
            clientInfoLeft.Add(Environment.NewLine())
            If estimate.ClientTelephone <> "" Then
                clientInfoLeft.Add(String.Format("Telefone: {0}{1}", Utilities.PhoneMask(estimate.ClientTelephone), Environment.NewLine()))
            End If
            If estimate.ClientEmail <> "" Then
                clientInfoLeft.Add(String.Format("Email: {0}{1}", estimate.ClientEmail, Environment.NewLine()))
            End If
            If estimate.ClientEIN <> "" Then
                clientInfoLeft.Add(String.Format("CNPJ: {0}{1}", estimate.ClientEIN, Environment.NewLine()))
            End If
            If estimate.ClientSateTax <> "" Then
                clientInfoLeft.Add(String.Format("Inscrição Estadual: {0}{1}", estimate.ClientSateTax, Environment.NewLine()))
            End If
            If estimate.ClientCityTax <> "" Then
                clientInfoLeft.Add(String.Format("Inscrição Municipal: {0}{1}", estimate.ClientCityTax, Environment.NewLine()))
            End If

            Dim clientInfoRight As New Paragraph()
            If estimate.ClientAddress <> "" Then
                clientInfoRight = New Paragraph(String.Format("Endereço: {0}{1}{2}{3}", estimate.ClientAddress, Space(1), estimate.ClientUnit, Environment.NewLine()), heading8)
            End If
            If estimate.ClientComplement <> "" Then
                clientInfoRight.Add(String.Format("Complement: {0}{1}", estimate.ClientComplement, Environment.NewLine()))
            End If
            If estimate.ClientDistrict <> "" Then
                clientInfoRight.Add(String.Format("Bairro: {0}{1}", estimate.ClientDistrict, Environment.NewLine()))
            End If
            If estimate.ClientCity <> "" Then
                clientInfoRight.Add(String.Format("Cidade: {0}{1}", estimate.ClientCity, Space(1)))
            End If
            If estimate.ClientRegion <> "" Then
                clientInfoRight.Add(String.Format("Estado: {0}{1}", estimate.ClientRegion, Space(1)))
            End If
            If estimate.ClientPostalCode <> "" Then
                clientInfoRight.Add(String.Format("CEP: {0}", Utilities.ZipMask(estimate.ClientPostalCode)))
            End If

            Dim clientCell1 = New PdfPCell(clientInfoHeader) With {.PaddingLeft = 7.0F, .PaddingTop = 5.0F, .PaddingBottom = 4.0F, .Colspan = 2, .BackgroundColor = New BaseColor(228, 228, 228)}
            Dim clientCell3 = New PdfPCell(clientInfoLeft) With {.Padding = 7.0F, .ExtraParagraphSpace = 2.0F}
            Dim clientCell4 = New PdfPCell(clientInfoRight) With {.Padding = 7.0F, .ExtraParagraphSpace = 2.0F}

            clientTable.AddCell(clientCell1)
            clientTable.AddCell(clientCell3)
            clientTable.AddCell(clientCell4)

            thePdf.Add(clientTable)

            Dim prodTable As New PdfPTable(dto.ColumnsCount) With {.SpacingBefore = 5.0F, .HorizontalAlignment = 0, .TotalWidth = clientTable.TotalWidth, .LockedWidth = True}
            Dim widthsProdTable As Single()
            If CSng(dto.ProductDiscountValue) > 0 Then
                widthsProdTable = New Single() {1.0F, 2.95F, 0.7F, 1.1F, 0.7F, 1.0F, 1.0F}
            Else
                widthsProdTable = New Single() {1.0F, 2.95F, 0.7F, 1.1F, 0.0F, 1.0F, 1.0F}
            End If
            prodTable.SetWidths(widthsProdTable)

            Dim prodCellHeader As PdfPCell = New PdfPCell(New Phrase("Itens do Orçamento", heading9)) With {.Colspan = dto.ColumnsCount, .PaddingLeft = 7.0F, .PaddingTop = 5.0F, .PaddingBottom = 4.0F, .BackgroundColor = New BaseColor(228, 228, 228)}
            prodTable.AddCell(prodCellHeader)

            For Each column In dto.Columns
                Dim prodCellHeaderColumns As PdfPCell = New PdfPCell(New Phrase(column, heading8)) With {.PaddingTop = 5.0F, .PaddingLeft = 5.0F}
                prodTable.AddCell(prodCellHeaderColumns)
            Next

            estimateItems = estimateCtrl.GetEstimateItems(dto.PortalId, dto.EstimateId, dto.Lang)

            For Each item In estimateItems

                Dim code = ""
                If item.Barcode <> "" Then
                    code = "CB: " & item.Barcode
                ElseIf item.ProductRef <> "" Then
                    code = "REF: " & item.ProductRef
                End If

                Dim prodCellProdRef As PdfPCell = New PdfPCell(New Phrase(code, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1}
                Dim prodCellProdName As PdfPCell = New PdfPCell(New Phrase(item.ProductName, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F}
                Dim prodCellQty As PdfPCell = New PdfPCell(New Phrase(item.ProductQty, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1}
                Dim prodCellEstProdOriginalPrice As PdfPCell = New PdfPCell(New Phrase(item.ProductEstimateOriginalPrice, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1}
                Dim prodCellDiscount As PdfPCell = New PdfPCell(New Phrase(item.ProductDiscount, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1, .HorizontalAlignment = Element.ALIGN_RIGHT}
                Dim prodCellEstProdPrice As PdfPCell = New PdfPCell(New Phrase(item.ProductEstimatePrice, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1, .HorizontalAlignment = Element.ALIGN_RIGHT}
                Dim prodCellExtendedAmount As PdfPCell = New PdfPCell(New Phrase(item.ExtendedAmount, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1, .HorizontalAlignment = Element.ALIGN_RIGHT}

                If Not (item.ItemIndex Mod 2 = 1) Then
                    prodCellProdRef.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellProdName.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellQty.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellEstProdOriginalPrice.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellDiscount.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellEstProdPrice.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellExtendedAmount.BackgroundColor = New BaseColor(195, 195, 195)
                Else
                    prodCellProdRef.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellProdName.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellQty.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellEstProdOriginalPrice.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellDiscount.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellEstProdPrice.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellExtendedAmount.BackgroundColor = New BaseColor(228, 228, 228)
                End If

                prodTable.AddCell(prodCellProdRef)
                prodTable.AddCell(prodCellProdName)
                prodTable.AddCell(prodCellQty)
                prodTable.AddCell(prodCellEstProdOriginalPrice)
                prodTable.AddCell(prodCellDiscount)
                prodTable.AddCell(prodCellEstProdPrice)
                prodTable.AddCell(prodCellExtendedAmount)

                If dto.Expand Then
                    Dim expItem = productCtrl.getProduct(item.ProductId, "pt-BR") 'Feature_Controller.Get_ProductDetail(CInt(item.GetDataKeyValue("ProdID")))
                    'For Each dRow In expItem
                    If Not expItem.Summary <> "" OrElse expItem.ProductImageId > 0 Then
                        '    Exit For
                        'End If
                        Dim prodCellProdIntro As PdfPCell = New PdfPCell(New Phrase(Utilities.RemoveHtmlTags(HttpUtility.HtmlDecode(expItem.Summary)), heading7)) With {.PaddingTop = 10.0F, .PaddingLeft = 10.0F, .Border = 0}
                        Dim prodDetailTable As PdfPTable = New PdfPTable(2)
                        Dim widthsDetailTable As Single() = New Single() {1.0F, 3.0F}
                        prodDetailTable.SetWidths(widthsDetailTable)
                        Dim prodCellImage As PdfPCell = New PdfPCell() With {.PaddingTop = 10.0F, .PaddingLeft = 10.0F, .Border = 0}

                        If expItem.ProductImageId > 0 Then
                            Dim jpgProdImage As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(New Uri(String.Format("/databaseimages/{0}.{1}?maxwidth=130&maxheight=130{2}", expItem.ProductImageId, expItem.Extension, CStr(IIf(dto.Watermark <> "", "&watermark=outglow&text=" & dto.Watermark, "")))))
                            jpgProdImage.ScaleToFit(70.0F, 40.0F)
                            prodCellImage.AddElement(jpgProdImage)
                        End If

                        Dim prodCellProdDesc As PdfPCell = New PdfPCell(New Phrase(Utilities.RemoveHtmlTags(HttpUtility.HtmlDecode(expItem.Description)), heading7)) With {.PaddingTop = 10.0F, .PaddingLeft = 10.0F, .Colspan = 2, .Border = 0}
                        prodDetailTable.AddCell(prodCellImage)
                        prodDetailTable.AddCell(prodCellProdIntro)
                        prodDetailTable.AddCell(prodCellProdDesc)
                        Dim prodCellDetail As PdfPCell = New PdfPCell(prodDetailTable) With {.Colspan = dto.Columns.Count}
                        prodTable.AddCell(prodCellDetail)
                        'Next
                    End If
                End If

            Next

            thePdf.Add(prodTable)

            Dim amountTable = New PdfPTable(2) With {.TotalWidth = prodTable.TotalWidth, .SpacingBefore = 5.0F, .LockedWidth = True}
            Dim widthsamountTable As Single() = New Single() {6.0F, 1.0F}
            amountTable.SetWidths(widthsamountTable)

            Dim amountCell = New PdfPCell() With {.Padding = 2.0F}

            'If Not UserInfo.IsInRole("Gerentes") And Not UserInfo.IsInRole("Vendedores") Then
            If dto.EstimateDiscountValue > 0 OrElse dto.ProductDiscountValue > 0 Then
                Dim OriginalAmount_Label As New Phrase("Valor Original: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(OriginalAmount_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim OriginalAmount As New Phrase(String.Format("{0}", FormatCurrency(dto.ProductOriginalAmount)), heading7)
                amountCell = New PdfPCell(OriginalAmount) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If
            'Else

            If dto.ProductDiscountValue > 0 Then
                Dim ProductDiscountValue_Label As New Phrase("Desc. Produto: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(ProductDiscountValue_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim ProductDiscountValue As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(dto.ProductDiscountValue), Environment.NewLine()), heading7)
                amountCell = New PdfPCell(ProductDiscountValue) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If

            If dto.EstimateDiscountValue > 0 Then
                Dim EstimateDiscountValue_Label As New Phrase("Desc. Orçamento: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(EstimateDiscountValue_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim _estimateDiscountValue As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(dto.EstimateDiscountValue), Environment.NewLine()), heading7)
                amountCell = New PdfPCell(_estimateDiscountValue) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If

            'End If

            If dto.TotalDiscountPerc > 0 Then
                Dim TotalDiscountTitle_Label As New Phrase("Desc. Total %: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(TotalDiscountTitle_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim _totalDiscountTitle As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatPercent(dto.TotalDiscountPerc), Environment.NewLine()), heading7)
                amountCell = New PdfPCell(_totalDiscountTitle) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If

            If dto.TotalDiscountValue > 0 Then
                Dim Discount_Label As New Phrase("Desc. Total $: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(Discount_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim _totalDiscountValue As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(dto.TotalDiscountValue), Environment.NewLine()), heading7)
                amountCell = New PdfPCell(_totalDiscountValue) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If

            Dim Payment_Label As New Phrase("Valor Final: ", heading8b) With {.Leading = 20.0F}
            amountCell = New PdfPCell(Payment_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            amountTable.AddCell(amountCell)

            Dim Payment As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(dto.EstimateTotalAmount), Environment.NewLine()), heading7)
            amountCell = New PdfPCell(Payment) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            amountTable.AddCell(amountCell)

            'If UserInfo.IsInRole("Gerentes") Then
            '    Dim MarkUpCurrency_Label As New Phrase("Markup $: ", heading8b) With {.Leading = 20.0F}
            '    amountCell = New PdfPCell(MarkUpCurrency_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            '    amountTable.AddCell(amountCell)

            '    Dim MarkUpCurrency As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(_MarkUpCurrency), Environment.NewLine()), heading7)
            '    amountCell = New PdfPCell(MarkUpCurrency) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            '    amountTable.AddCell(amountCell)

            '    Dim MarkupPerc_Label As New Phrase("Markup %: ", heading8b) With {.Leading = 20.0F}
            '    amountCell = New PdfPCell(MarkupPerc_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            '    amountTable.AddCell(amountCell)

            '    Dim MarkupPerc As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatPercent(_MarkupPerc), Environment.NewLine()), heading7)
            '    amountCell = New PdfPCell(MarkupPerc) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            '    amountTable.AddCell(amountCell)
            'End If

            thePdf.Add(amountTable)

            Dim payCond0 = payCondCtrl.GetPayConds(dto.PortalId, 0, CDec(dto.EstimateTotalAmount))
            If payCond0.Count > 0 Then
                For Each payCondType0 In payCond0
                    Dim payCondTitleType0 As New Paragraph(CStr(IIf(payCondType0.PayCondDisc > 0, String.Format("{0}{1}{0} Valor com desconto {2}", Space(1), payCondType0.PayCondTitle, FormatCurrency(dto.EstimateTotalAmount - (dto.EstimateTotalAmount / 100 * payCondType0.PayCondDisc))), payCondType0.PayCondTitle)), heading7)
                    thePdf.Add(payCondTitleType0)
                Next
            End If

            If estimate.PayCondType <> "" Then

                Dim payCondChosenTitle As Paragraph = New Paragraph("Condição de Pagamento Escolhida:", heading8b)
                thePdf.Add(payCondChosenTitle)
                Dim tablePayCondChosen = New PdfPTable(8) With {.TotalWidth = prodTable.TotalWidth, .SpacingBefore = 7.0F, .LockedWidth = True}
                Dim widthstablePayCondChosen As Single() = New Single() {1.2F, 1.4F, 1.2F, 1.4F, 1.4F, 1.0F, 1.0F, 1.2F}
                tablePayCondChosen.SetWidths(widthstablePayCondChosen)

                Dim payCondCellChosen As PdfPCell = New PdfPCell(New Phrase("Forma", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Número de parcelas", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Entrada", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Valor de Cada Parcela", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Valor do Parcelado", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Juros (a.m.)", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Interv. Dias", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Total", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(estimate.PayCondType, heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                If estimate.PayCondPerc > 0.0 Then
                    payCondCellChosen = New PdfPCell(New Phrase(String.Format("{0}x com juros", estimate.PayCondN), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                Else
                    payCondCellChosen = New PdfPCell(New Phrase(String.Format("{0}x sem juros", estimate.PayCondN), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                End If
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatCurrency(estimate.PayCondIn), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatCurrency(estimate.PayCondInst), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatCurrency(estimate.TotalPayments), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatPercent(estimate.PayCondPerc), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(CStr(IIf(CInt(estimate.PayCondInterval) > 0, estimate.PayCondInterval, "Mensal")), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatCurrency(estimate.TotalPayCond), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                thePdf.Add(tablePayCondChosen)

            End If

            Dim objPayCond = payCondCtrl.GetPayConds(dto.PortalId, Null.NullInteger, CDec(dto.EstimateTotalAmount))

            If objPayCond.Count > 0 Then

                Dim payCondTitle As Paragraph = New Paragraph("Mais Condições de Pagamento:", heading8b)
                thePdf.Add(payCondTitle)

                Dim tablePayCond = New PdfPTable(8) With {.TotalWidth = prodTable.TotalWidth, .SpacingBefore = 7.0F, .LockedWidth = True}
                Dim widthstablePayCond As Single() = New Single() {1.2F, 1.4F, 1.2F, 1.4F, 1.4F, 1.0F, 1.0F, 1.2F}
                tablePayCond.SetWidths(widthstablePayCond)

                Dim payCondCell As PdfPCell = New PdfPCell(New Phrase("Forma", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Número de parcelas", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Entrada", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Valor de Cada Parcela", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Valor do Parcelado", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Juros (a.m.)", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Interv. Dias", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Total", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)

                For Each payCond In objPayCond

                    If payCond.PayCondType > 0 Then

                        Select Case payCond.PayCondType
                            Case 5
                                payCondCell = New PdfPCell(New Phrase("Cheque Pré", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Case 1
                                payCondCell = New PdfPCell(New Phrase("Boleto", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Case 2
                                payCondCell = New PdfPCell(New Phrase("Visa", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Case 3
                                payCondCell = New PdfPCell(New Phrase("Master Card", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Case 4
                                payCondCell = New PdfPCell(New Phrase("Amex", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        End Select
                        tablePayCond.AddCell(payCondCell)

                        Dim interestRate = 0.0
                        interestRate = payCond.PayCondPerc
                        Dim paymentQty = 0
                        paymentQty = payCond.PayCondN
                        Dim payIn = 0.0
                        payIn = payCond.PayCondIn

                        interestRate = interestRate / 100

                        Dim InitialPay = (dto.EstimateTotalAmount / 100 * payIn)

                        Dim TotalLabel = dto.EstimateTotalAmount - InitialPay

                        Dim resultPayment As Double

                        If estimate.PayCondInterval > 0 Then

                            If interestRate > 0 Then

                                Dim interestADay = interestRate / (DateTime.DaysInMonth(Year(DateTime.Now()), Month(DateTime.Now())))

                                interestADay = interestADay * estimate.PayCondInterval

                                If InitialPay > 0 Then
                                    resultPayment = Pmt(interestADay, (paymentQty - 1), -TotalLabel)
                                Else
                                    resultPayment = Pmt(interestADay, paymentQty, -TotalLabel)
                                End If

                            Else

                                If InitialPay > 0 Then
                                    resultPayment = Pmt(interestRate, (paymentQty - 1), -TotalLabel)
                                Else
                                    resultPayment = Pmt(interestRate, paymentQty, -TotalLabel)
                                End If

                            End If

                        Else

                            If InitialPay > 0 Then
                                resultPayment = Pmt(interestRate, (paymentQty - 1), -TotalLabel)
                            Else
                                resultPayment = Pmt(interestRate, paymentQty, -TotalLabel)
                            End If

                        End If

                        If interestRate > 0.0 Then
                            payCondCell = New PdfPCell(New Phrase(String.Format("{0}x com juros", CStr(paymentQty)), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        Else
                            payCondCell = New PdfPCell(New Phrase(String.Format("{0}x sem juros", CStr(paymentQty)), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        End If
                        tablePayCond.AddCell(payCondCell)

                        If InitialPay > 0 Then
                            payCondCell = New PdfPCell(New Phrase(FormatCurrency(dto.EstimateTotalAmount / 100 * payIn), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        Else
                            payCondCell = New PdfPCell(New Phrase(FormatCurrency(0), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        End If
                        tablePayCond.AddCell(payCondCell)

                        payCondCell = New PdfPCell(New Phrase(FormatCurrency(resultPayment), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        tablePayCond.AddCell(payCondCell)

                        If InitialPay > 0 Then
                            payCondCell = New PdfPCell(New Phrase(FormatCurrency((resultPayment * (paymentQty - 1))), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        Else
                            payCondCell = New PdfPCell(New Phrase(FormatCurrency((resultPayment * paymentQty)), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        End If
                        tablePayCond.AddCell(payCondCell)

                        payCondCell = New PdfPCell(New Phrase(FormatPercent(interestRate), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        tablePayCond.AddCell(payCondCell)

                        payCondCell = New PdfPCell(New Phrase(CStr(IIf(payCond.PayCondInterval > 0, CStr(payCond.PayCondInterval), "Mensal")), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        tablePayCond.AddCell(payCondCell)

                        If InitialPay > 0 Then
                            payCondCell = New PdfPCell(New Phrase(FormatCurrency((resultPayment * (paymentQty - 1)) + InitialPay), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        Else
                            payCondCell = New PdfPCell(New Phrase(FormatCurrency((resultPayment * paymentQty) + InitialPay), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        End If
                        tablePayCond.AddCell(payCondCell)

                    End If

                Next

                thePdf.Add(tablePayCond)

            End If

            Dim _TermsOE = ""

            'Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(dto.PortalId)
            'If estimate.Inst <> "" Then
            _TermsOE = estimate.Inst
            'Else
            '    _TermsOE = myPortalSettings("RIW_EstimateTerm")
            'End If
            Dim obsTitle As New Paragraph("Observações Importantes:", heading9) With {.SpacingBefore = 10.0F}
            Dim obsText As New Paragraph(String.Format("{0}", Utilities.RemoveHtmlTags(HttpUtility.HtmlDecode(_TermsOE))), heading7)
            thePdf.Add(obsTitle)
            thePdf.Add(obsText)

            writer.CloseStream = False
            thePdf.Close()
            file.Close()
            'str.Position = 0

            'Dim _mediaType = MediaTypeHeaderValue.Parse("application/pdf")
            'Dim response As New HttpResponseMessage(HttpStatusCode.OK)
            'response.Content = New StreamContent(str)
            'response.Content.Headers.ContentType = _mediaType
            'response.Content.Headers.ContentDisposition = New ContentDispositionHeaderValue("fileName") With {.FileName = String.Format("Orcamento_{0}_{1}.pdf", CStr(dto.EstimateId), Replace(DateTime.Now.ToString(), ":", "-").Replace("/", "-").Replace(" ", ""))}

            Dim estimateHistory As New Models.EstimateHistory

            estimateHistory.EstimateId = dto.EstimateId
            estimateHistory.HistoryText = "<p>Novo pdf foi gerado.</p>"
            estimateHistory.Locked = True
            estimateHistory.CreatedByUser = 0
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
            End If

            'Return response
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .FileName = System.IO.Path.GetFileName(pdfFile)})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Generate Estimate PDF
    ''' </summary>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function sendEstimatePdf(dto As EstimatePdf) As HttpResponseMessage
        Try
            Dim estimate As New Models.Estimate
            Dim estimateItems As New List(Of Models.EstimateItem)
            Dim estimateCtrl As New EstimatesRepository
            Dim payCondCtrl As New PayConditionsRepository
            Dim productCtrl As New ProductsRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId)

            'Using ms As MemoryStream = New MemoryStream()

            Dim str As New MemoryStream()

            Dim heading12 As Font = FontFactory.GetFont("ARIAL", 12)
            Dim heading10 As Font = FontFactory.GetFont("ARIAL", 10)
            Dim heading9 As Font = FontFactory.GetFont("VERDANA", 9)
            Dim heading8 As Font = FontFactory.GetFont("VERDANA", 8)
            Dim heading8b As Font = FontFactory.GetFont("VERDANA", 8, Font.BOLD)
            Dim heading7 As Font = FontFactory.GetFont("VERDANA", 7)

            Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(dto.PortalId)

            'Dim pdfFile = HostingEnvironment.MapPath("\Portals\0\something.pdf")

            Dim thePdf As New Document(PageSize.A4, 20, 20, 100, 15)

            'Dim file As New System.IO.FileStream(pdfFile, System.IO.FileMode.OpenOrCreate)
            'Dim writer As PdfWriter = PdfWriter.GetInstance(thePdf, file)
            Dim writer As PdfWriter = PdfWriter.GetInstance(thePdf, str)

            Dim page As pdfPage = New pdfPage()
            writer.PageEvent = page

            thePdf.Open()

            Dim clientTable = New PdfPTable(2) With {.TotalWidth = 555.0F, .HorizontalAlignment = 0, .LockedWidth = True}
            Dim widthsHeaderTable As Single() = New Single() {1.0F, 1.0F}
            clientTable.SetWidths(widthsHeaderTable)

            Dim clientInfoHeader As New Paragraph("Informações do Cliente", heading10)

            Dim clientInfoLeft As New Paragraph(String.Format("Cliente: {0}", estimate.ClientDisplayName), heading8)
            clientInfoLeft.Add(Environment.NewLine())
            If estimate.ClientTelephone <> "" Then
                clientInfoLeft.Add(String.Format("Telefone: {0}{1}", Utilities.PhoneMask(estimate.ClientTelephone), Environment.NewLine()))
            End If
            If estimate.ClientEmail <> "" Then
                clientInfoLeft.Add(String.Format("Email: {0}{1}", estimate.ClientEmail, Environment.NewLine()))
            End If
            If estimate.ClientEIN <> "" Then
                clientInfoLeft.Add(String.Format("CNPJ: {0}{1}", estimate.ClientEIN, Environment.NewLine()))
            End If
            If estimate.ClientSateTax <> "" Then
                clientInfoLeft.Add(String.Format("Inscrição Estadual: {0}{1}", estimate.ClientSateTax, Environment.NewLine()))
            End If
            If estimate.ClientCityTax <> "" Then
                clientInfoLeft.Add(String.Format("Inscrição Municipal: {0}{1}", estimate.ClientCityTax, Environment.NewLine()))
            End If

            Dim clientInfoRight As New Paragraph()
            If estimate.ClientAddress <> "" Then
                clientInfoRight = New Paragraph(String.Format("Endereço: {0}{1}{2}{3}", estimate.ClientAddress, Space(1), estimate.ClientUnit, Environment.NewLine()), heading8)
            End If
            If estimate.ClientComplement <> "" Then
                clientInfoRight.Add(String.Format("Complement: {0}{1}", estimate.ClientComplement, Environment.NewLine()))
            End If
            If estimate.ClientDistrict <> "" Then
                clientInfoRight.Add(String.Format("Bairro: {0}{1}", estimate.ClientDistrict, Environment.NewLine()))
            End If
            If estimate.ClientCity <> "" Then
                clientInfoRight.Add(String.Format("Cidade: {0}{1}", estimate.ClientCity, Space(1)))
            End If
            If estimate.ClientRegion <> "" Then
                clientInfoRight.Add(String.Format("Estado: {0}{1}", estimate.ClientRegion, Space(1)))
            End If
            If estimate.ClientPostalCode <> "" Then
                clientInfoRight.Add(String.Format("CEP: {0}", Utilities.ZipMask(estimate.ClientPostalCode)))
            End If

            Dim clientCell1 = New PdfPCell(clientInfoHeader) With {.PaddingLeft = 7.0F, .PaddingTop = 5.0F, .PaddingBottom = 4.0F, .Colspan = 2, .BackgroundColor = New BaseColor(228, 228, 228)}
            Dim clientCell3 = New PdfPCell(clientInfoLeft) With {.Padding = 7.0F, .ExtraParagraphSpace = 2.0F}
            Dim clientCell4 = New PdfPCell(clientInfoRight) With {.Padding = 7.0F, .ExtraParagraphSpace = 2.0F}

            clientTable.AddCell(clientCell1)
            clientTable.AddCell(clientCell3)
            clientTable.AddCell(clientCell4)

            thePdf.Add(clientTable)

            Dim prodTable As New PdfPTable(dto.ColumnsCount) With {.SpacingBefore = 5.0F, .HorizontalAlignment = 0, .TotalWidth = clientTable.TotalWidth, .LockedWidth = True}
            Dim widthsProdTable As Single()
            If CSng(dto.ProductDiscountValue) > 0 Then
                widthsProdTable = New Single() {1.0F, 2.95F, 0.7F, 1.1F, 0.7F, 1.0F, 1.0F}
            Else
                widthsProdTable = New Single() {1.0F, 2.95F, 0.7F, 1.1F, 0.0F, 1.0F, 1.0F}
            End If
            prodTable.SetWidths(widthsProdTable)

            Dim prodCellHeader As PdfPCell = New PdfPCell(New Phrase("Itens do Orçamento", heading9)) With {.Colspan = dto.ColumnsCount, .PaddingLeft = 7.0F, .PaddingTop = 5.0F, .PaddingBottom = 4.0F, .BackgroundColor = New BaseColor(228, 228, 228)}
            prodTable.AddCell(prodCellHeader)

            For Each column In dto.Columns
                Dim prodCellHeaderColumns As PdfPCell = New PdfPCell(New Phrase(column, heading8)) With {.PaddingTop = 5.0F, .PaddingLeft = 5.0F}
                prodTable.AddCell(prodCellHeaderColumns)
            Next

            estimateItems = estimateCtrl.GetEstimateItems(dto.PortalId, dto.EstimateId, dto.Lang)

            For Each item In estimateItems

                Dim code = ""
                If item.Barcode <> "" Then
                    code = "CB: " & item.Barcode
                ElseIf item.ProductRef <> "" Then
                    code = "REF: " & item.ProductRef
                End If

                Dim prodCellProdRef As PdfPCell = New PdfPCell(New Phrase(code, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1}
                Dim prodCellProdName As PdfPCell = New PdfPCell(New Phrase(item.ProductName, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F}
                Dim prodCellQty As PdfPCell = New PdfPCell(New Phrase(item.ProductQty, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1}
                Dim prodCellEstProdOriginalPrice As PdfPCell = New PdfPCell(New Phrase(item.ProductEstimateOriginalPrice, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1}
                Dim prodCellDiscount As PdfPCell = New PdfPCell(New Phrase(item.ProductDiscount, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1, .HorizontalAlignment = Element.ALIGN_RIGHT}
                Dim prodCellEstProdPrice As PdfPCell = New PdfPCell(New Phrase(item.ProductEstimatePrice, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1, .HorizontalAlignment = Element.ALIGN_RIGHT}
                Dim prodCellExtendedAmount As PdfPCell = New PdfPCell(New Phrase(item.ExtendedAmount, heading7)) With {.PaddingTop = 3.0F, .PaddingLeft = 3.0F, .VerticalAlignment = 1, .HorizontalAlignment = Element.ALIGN_RIGHT}

                If Not (item.ItemIndex Mod 2 = 1) Then
                    prodCellProdRef.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellProdName.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellQty.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellEstProdOriginalPrice.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellDiscount.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellEstProdPrice.BackgroundColor = New BaseColor(195, 195, 195)
                    prodCellExtendedAmount.BackgroundColor = New BaseColor(195, 195, 195)
                Else
                    prodCellProdRef.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellProdName.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellQty.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellEstProdOriginalPrice.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellDiscount.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellEstProdPrice.BackgroundColor = New BaseColor(228, 228, 228)
                    prodCellExtendedAmount.BackgroundColor = New BaseColor(228, 228, 228)
                End If

                prodTable.AddCell(prodCellProdRef)
                prodTable.AddCell(prodCellProdName)
                prodTable.AddCell(prodCellQty)
                prodTable.AddCell(prodCellEstProdOriginalPrice)
                prodTable.AddCell(prodCellDiscount)
                prodTable.AddCell(prodCellEstProdPrice)
                prodTable.AddCell(prodCellExtendedAmount)

                If dto.Expand Then
                    Dim expItem = productCtrl.getProduct(item.ProductId, "pt-BR") 'Feature_Controller.Get_ProductDetail(CInt(item.GetDataKeyValue("ProdID")))
                    'For Each dRow In expItem
                    If Not expItem.Summary <> "" OrElse expItem.ProductImageId > 0 Then
                        '    Exit For
                        'End If
                        Dim prodCellProdIntro As PdfPCell = New PdfPCell(New Phrase(Utilities.RemoveHtmlTags(HttpUtility.HtmlDecode(expItem.Summary)), heading7)) With {.PaddingTop = 10.0F, .PaddingLeft = 10.0F, .Border = 0}
                        Dim prodDetailTable As PdfPTable = New PdfPTable(2)
                        Dim widthsDetailTable As Single() = New Single() {1.0F, 3.0F}
                        prodDetailTable.SetWidths(widthsDetailTable)
                        Dim prodCellImage As PdfPCell = New PdfPCell() With {.PaddingTop = 10.0F, .PaddingLeft = 10.0F, .Border = 0}

                        If expItem.ProductImageId > 0 Then
                            Dim jpgProdImage As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(New Uri(String.Format("/databaseimages/{0}.{1}?maxwidth=130&maxheight=130{2}", expItem.ProductImageId, expItem.Extension, CStr(IIf(dto.Watermark <> "", "&watermark=outglow&text=" & dto.Watermark, "")))))
                            jpgProdImage.ScaleToFit(70.0F, 40.0F)
                            prodCellImage.AddElement(jpgProdImage)
                        End If

                        Dim prodCellProdDesc As PdfPCell = New PdfPCell(New Phrase(Utilities.RemoveHtmlTags(HttpUtility.HtmlDecode(expItem.Description)), heading7)) With {.PaddingTop = 10.0F, .PaddingLeft = 10.0F, .Colspan = 2, .Border = 0}
                        prodDetailTable.AddCell(prodCellImage)
                        prodDetailTable.AddCell(prodCellProdIntro)
                        prodDetailTable.AddCell(prodCellProdDesc)
                        Dim prodCellDetail As PdfPCell = New PdfPCell(prodDetailTable) With {.Colspan = dto.Columns.Count}
                        prodTable.AddCell(prodCellDetail)
                        'Next
                    End If
                End If

            Next

            thePdf.Add(prodTable)

            Dim amountTable = New PdfPTable(2) With {.TotalWidth = prodTable.TotalWidth, .SpacingBefore = 5.0F, .LockedWidth = True}
            Dim widthsamountTable As Single() = New Single() {6.0F, 1.0F}
            amountTable.SetWidths(widthsamountTable)

            Dim amountCell = New PdfPCell() With {.Padding = 2.0F}

            'If Not UserInfo.IsInRole("Gerentes") And Not UserInfo.IsInRole("Vendedores") Then
            If dto.EstimateDiscountValue > 0 OrElse dto.ProductDiscountValue > 0 Then
                Dim OriginalAmount_Label As New Phrase("Valor Original: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(OriginalAmount_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim OriginalAmount As New Phrase(String.Format("{0}", FormatCurrency(dto.ProductOriginalAmount)), heading7)
                amountCell = New PdfPCell(OriginalAmount) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If
            'Else

            If dto.ProductDiscountValue > 0 Then
                Dim ProductDiscountValue_Label As New Phrase("Desc. Produto: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(ProductDiscountValue_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim ProductDiscountValue As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(dto.ProductDiscountValue), Environment.NewLine()), heading7)
                amountCell = New PdfPCell(ProductDiscountValue) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If

            If dto.EstimateDiscountValue > 0 Then
                Dim EstimateDiscountValue_Label As New Phrase("Desc. Orçamento: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(EstimateDiscountValue_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim _estimateDiscountValue As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(dto.EstimateDiscountValue), Environment.NewLine()), heading7)
                amountCell = New PdfPCell(_estimateDiscountValue) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If

            'End If

            If dto.TotalDiscountPerc > 0 Then
                Dim TotalDiscountTitle_Label As New Phrase("Desc. Total %: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(TotalDiscountTitle_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim _totalDiscountTitle As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatPercent(dto.TotalDiscountPerc), Environment.NewLine()), heading7)
                amountCell = New PdfPCell(_totalDiscountTitle) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If

            If dto.TotalDiscountValue > 0 Then
                Dim Discount_Label As New Phrase("Desc. Total $: ", heading8b) With {.Leading = 20.0F}
                amountCell = New PdfPCell(Discount_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)

                Dim _totalDiscountValue As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(dto.TotalDiscountValue), Environment.NewLine()), heading7)
                amountCell = New PdfPCell(_totalDiscountValue) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
                amountTable.AddCell(amountCell)
            End If

            Dim Payment_Label As New Phrase("Valor Final: ", heading8b) With {.Leading = 20.0F}
            amountCell = New PdfPCell(Payment_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            amountTable.AddCell(amountCell)

            Dim Payment As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(dto.EstimateTotalAmount), Environment.NewLine()), heading7)
            amountCell = New PdfPCell(Payment) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            amountTable.AddCell(amountCell)

            'If UserInfo.IsInRole("Gerentes") Then
            '    Dim MarkUpCurrency_Label As New Phrase("Markup $: ", heading8b) With {.Leading = 20.0F}
            '    amountCell = New PdfPCell(MarkUpCurrency_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            '    amountTable.AddCell(amountCell)

            '    Dim MarkUpCurrency As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatCurrency(_MarkUpCurrency), Environment.NewLine()), heading7)
            '    amountCell = New PdfPCell(MarkUpCurrency) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            '    amountTable.AddCell(amountCell)

            '    Dim MarkupPerc_Label As New Phrase("Markup %: ", heading8b) With {.Leading = 20.0F}
            '    amountCell = New PdfPCell(MarkupPerc_Label) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            '    amountTable.AddCell(amountCell)

            '    Dim MarkupPerc As New Phrase(String.Format("{0}{1}{2}", Space(3), FormatPercent(_MarkupPerc), Environment.NewLine()), heading7)
            '    amountCell = New PdfPCell(MarkupPerc) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0}
            '    amountTable.AddCell(amountCell)
            'End If

            thePdf.Add(amountTable)

            Dim payCond0 = payCondCtrl.GetPayConds(dto.PortalId, 0, CDec(dto.EstimateTotalAmount))
            If payCond0.Count > 0 Then
                For Each payCondType0 In payCond0
                    Dim payCondTitleType0 As New Paragraph(CStr(IIf(payCondType0.PayCondDisc > 0, String.Format("{0}{1}{0} Valor com desconto {2}", Space(1), payCondType0.PayCondTitle, FormatCurrency(dto.EstimateTotalAmount - (dto.EstimateTotalAmount / 100 * payCondType0.PayCondDisc))), payCondType0.PayCondTitle)), heading7)
                    thePdf.Add(payCondTitleType0)
                Next
            End If

            If estimate.PayCondType <> "" Then

                Dim payCondChosenTitle As Paragraph = New Paragraph("Condição de Pagamento Escolhida:", heading8b)
                thePdf.Add(payCondChosenTitle)
                Dim tablePayCondChosen = New PdfPTable(8) With {.TotalWidth = prodTable.TotalWidth, .SpacingBefore = 7.0F, .LockedWidth = True}
                Dim widthstablePayCondChosen As Single() = New Single() {1.2F, 1.4F, 1.2F, 1.4F, 1.4F, 1.0F, 1.0F, 1.2F}
                tablePayCondChosen.SetWidths(widthstablePayCondChosen)

                Dim payCondCellChosen As PdfPCell = New PdfPCell(New Phrase("Forma", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Número de parcelas", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Entrada", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Valor de Cada Parcela", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Valor do Parcelado", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Juros (a.m.)", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Interv. Dias", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)
                payCondCellChosen = New PdfPCell(New Phrase("Total", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(estimate.PayCondType, heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                If estimate.PayCondPerc > 0.0 Then
                    payCondCellChosen = New PdfPCell(New Phrase(String.Format("{0}x com juros", estimate.PayCondN), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                Else
                    payCondCellChosen = New PdfPCell(New Phrase(String.Format("{0}x sem juros", estimate.PayCondN), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                End If
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatCurrency(estimate.PayCondIn), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatCurrency(estimate.PayCondInst), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatCurrency(estimate.TotalPayments), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatPercent(estimate.PayCondPerc), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(CStr(IIf(CInt(estimate.PayCondInterval) > 0, estimate.PayCondInterval, "Mensal")), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                payCondCellChosen = New PdfPCell(New Phrase(FormatCurrency(estimate.TotalPayCond), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                tablePayCondChosen.AddCell(payCondCellChosen)

                thePdf.Add(tablePayCondChosen)

            End If

            Dim objPayCond = payCondCtrl.GetPayConds(dto.PortalId, Null.NullInteger, CDec(dto.EstimateTotalAmount))

            If objPayCond.Count > 0 Then

                Dim payCondTitle As Paragraph = New Paragraph("Condições de Pagamento:", heading8b)
                thePdf.Add(payCondTitle)

                Dim tablePayCond = New PdfPTable(8) With {.TotalWidth = prodTable.TotalWidth, .SpacingBefore = 7.0F, .LockedWidth = True}
                Dim widthstablePayCond As Single() = New Single() {1.2F, 1.4F, 1.2F, 1.4F, 1.4F, 1.0F, 1.0F, 1.2F}
                tablePayCond.SetWidths(widthstablePayCond)

                Dim payCondCell As PdfPCell = New PdfPCell(New Phrase("Forma", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Número de parcelas", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Entrada", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Valor de Cada Parcela", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Valor do Parcelado", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Juros (a.m.)", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Interv. Dias", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)
                payCondCell = New PdfPCell(New Phrase("Total", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER, .BackgroundColor = New BaseColor(228, 228, 228)}
                tablePayCond.AddCell(payCondCell)

                For Each payCond In objPayCond

                    If payCond.PayCondType > 0 Then

                        Select Case payCond.PayCondType
                            Case 5
                                payCondCell = New PdfPCell(New Phrase("Cheque Pré", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Case 1
                                payCondCell = New PdfPCell(New Phrase("Boleto", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Case 2
                                payCondCell = New PdfPCell(New Phrase("Visa", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Case 3
                                payCondCell = New PdfPCell(New Phrase("Master Card", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                            Case 4
                                payCondCell = New PdfPCell(New Phrase("Amex", heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        End Select
                        tablePayCond.AddCell(payCondCell)

                        Dim interestRate = 0.0
                        interestRate = payCond.PayCondPerc
                        Dim paymentQty = 0
                        paymentQty = payCond.PayCondN
                        Dim payIn = 0.0
                        payIn = payCond.PayCondIn

                        interestRate = interestRate / 100

                        Dim InitialPay = (dto.EstimateTotalAmount / 100 * payIn)

                        Dim TotalLabel = dto.EstimateTotalAmount - InitialPay

                        Dim resultPayment As Double

                        If estimate.PayCondInterval > 0 Then

                            If interestRate > 0 Then

                                Dim interestADay = interestRate / (DateTime.DaysInMonth(Year(DateTime.Now()), Month(DateTime.Now())))

                                interestADay = interestADay * estimate.PayCondInterval

                                If InitialPay > 0 Then
                                    resultPayment = Pmt(interestADay, (paymentQty - 1), -TotalLabel)
                                Else
                                    resultPayment = Pmt(interestADay, paymentQty, -TotalLabel)
                                End If

                            Else

                                If InitialPay > 0 Then
                                    resultPayment = Pmt(interestRate, (paymentQty - 1), -TotalLabel)
                                Else
                                    resultPayment = Pmt(interestRate, paymentQty, -TotalLabel)
                                End If

                            End If

                        Else

                            If InitialPay > 0 Then
                                resultPayment = Pmt(interestRate, (paymentQty - 1), -TotalLabel)
                            Else
                                resultPayment = Pmt(interestRate, paymentQty, -TotalLabel)
                            End If

                        End If

                        If interestRate > 0.0 Then
                            payCondCell = New PdfPCell(New Phrase(String.Format("{0}x com juros", CStr(paymentQty)), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        Else
                            payCondCell = New PdfPCell(New Phrase(String.Format("{0}x sem juros", CStr(paymentQty)), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        End If
                        tablePayCond.AddCell(payCondCell)

                        If InitialPay > 0 Then
                            payCondCell = New PdfPCell(New Phrase(FormatCurrency(dto.EstimateTotalAmount / 100 * payIn), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        Else
                            payCondCell = New PdfPCell(New Phrase(FormatCurrency(0), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        End If
                        tablePayCond.AddCell(payCondCell)

                        payCondCell = New PdfPCell(New Phrase(FormatCurrency(resultPayment), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        tablePayCond.AddCell(payCondCell)

                        If InitialPay > 0 Then
                            payCondCell = New PdfPCell(New Phrase(FormatCurrency((resultPayment * (paymentQty - 1))), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        Else
                            payCondCell = New PdfPCell(New Phrase(FormatCurrency((resultPayment * paymentQty)), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        End If
                        tablePayCond.AddCell(payCondCell)

                        payCondCell = New PdfPCell(New Phrase(FormatPercent(interestRate), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        tablePayCond.AddCell(payCondCell)

                        payCondCell = New PdfPCell(New Phrase(CStr(IIf(payCond.PayCondInterval > 0, CStr(payCond.PayCondInterval), "Mensal")), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        tablePayCond.AddCell(payCondCell)

                        If InitialPay > 0 Then
                            payCondCell = New PdfPCell(New Phrase(FormatCurrency((resultPayment * (paymentQty - 1)) + InitialPay), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        Else
                            payCondCell = New PdfPCell(New Phrase(FormatCurrency((resultPayment * paymentQty) + InitialPay), heading7)) With {.Padding = 3.0F, .HorizontalAlignment = Element.ALIGN_CENTER}
                        End If
                        tablePayCond.AddCell(payCondCell)

                    End If

                Next

                thePdf.Add(tablePayCond)

            End If

            Dim _TermsOE = ""

            If estimate.Inst <> "" Then
                _TermsOE = estimate.Inst
            Else
                _TermsOE = myPortalSettings("RIW_EstimateTerm")
            End If
            Dim obsTitle As New Paragraph("Observações Importantes:", heading9) With {.SpacingBefore = 10.0F}
            Dim obsText As New Paragraph(String.Format("{0}", Utilities.RemoveHtmlTags(HttpUtility.HtmlDecode(_TermsOE))), heading7)
            thePdf.Add(obsTitle)
            thePdf.Add(obsText)

            writer.CloseStream = False
            thePdf.Close()
            'file.Close()
            str.Position = 0

            Dim recipientList As New List(Of Users.UserInfo)
            Dim personCtrl As New PeopleRepository
            Dim personInfo As New Users.UserInfo()
            Dim ccUserInfo As New Users.UserInfo()
            Dim portalCtrl = New Portals.PortalController()

            Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = Portals.PortalController.GetPortalSetting("RIW_StoreEmail", dto.PortalId, portalCtrl.GetPortal(dto.PortalId).Email), .DisplayName = portalCtrl.GetPortal(dto.PortalId).PortalName}

            Dim clientInfo = personCtrl.getPerson(dto.PersonId, dto.PortalId, Null.NullInteger)

            If clientInfo.UserId > 0 Then
                personInfo = Users.UserController.GetUserById(dto.PortalId, clientInfo.UserId)
            Else
                personInfo = New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = clientInfo.Email, .DisplayName = clientInfo.DisplayName}
            End If

            recipientList.Add(personInfo)

            If dto.ToEmail.Length > 0 AndAlso Not clientInfo.Email = dto.ToEmail Then
                With ccUserInfo
                    .UserID = Null.NullInteger
                    .Email = dto.ToEmail
                    .DisplayName = clientInfo.DisplayName
                End With
                recipientList.Add(ccUserInfo)
            Else
                ccUserInfo = Nothing
            End If

            Dim _salesInfo = Users.UserController.GetUserById(dto.PortalId, dto.SalesPersonId)

            Dim mailMessage As New Net.Mail.MailMessage
            Dim distList As New PostOffice

            mailMessage.From = New Net.Mail.MailAddress(storeUser.Email, _salesInfo.DisplayName)
            mailMessage.ReplyToList.Add(New Net.Mail.MailAddress(_salesInfo.Email, _salesInfo.DisplayName))
            mailMessage.Subject = dto.Subject
            mailMessage.Body = dto.MesssageBody
            mailMessage.Attachments.Add(New Net.Mail.Attachment(str, String.Format("Orcamento_{0}_{1}.pdf", CStr(dto.EstimateId), Replace(DateTime.Now.ToString(), ":", "-").Replace("/", "-"))))
            mailMessage.IsBodyHtml = True

            Dim sentMails = distList.SendMail(mailMessage, recipientList, myPortalSettings("RIW_SMTPServer"), CInt(myPortalSettings("RIW_SMTPPort")), CBool(myPortalSettings("RIW_SMTPConnection")), myPortalSettings("RIW_SMTPLogin"), myPortalSettings("RIW_SMTPPassword"))

            'Notifications.SendStoreEmail(_salesInfo, personInfo, ccUserInfo, Nothing, dto.Subject, dto.MesssageBody, str, "application/pdf", Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, dto.PortalId, True)
            'Notifications.SendStoreEmail(storeUser, _salesInfo, "", "", subject, msg.Replace("\n", "<br />"), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)
            'Notifications.SendStoreEmail(storeUser, Users.UserController.GetUserById(portalId, clientInfo.UserId), "", "", subject, msg.Replace("\n", "<br />"), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)

            'Notifications.EstimateNotification(Constants.ContentTypeName.RIStore_Estimate, Constants.NotificationEstimateTypeName.RIStore_Estimate_Updated, Null.NullInteger, _salesInfo, "Gerentes", portalId, eId, String.Format("Novo Orçamento Inserido (ID: {0})", CStr(eId)), msg)


            'ms.Position = 0

            'Dim response As HttpResponseMessage = New HttpResponseMessage(HttpStatusCode.OK)
            'response.Content = New StreamContent(ms)
            'response.Content.Headers.ContentDisposition = New ContentDispositionHeaderValue("attachment")
            'response.Content.Headers.ContentDisposition.FileName = String.Format("Orcamento_{0}_{1}.pdf", CStr(dto.EstimateId), Replace(DateTime.Now.ToString(), ":", "-").Replace("/", "-"))
            'response.Content.Headers.ContentType = New MediaTypeHeaderValue("application/pdf")
            'response.Content.Headers.ContentLength = ms.Length

            'Response.Expires = -1
            'Response.ContentType = "application/pdf"
            'Response.AppendHeader("content-disposition", "attachment; " & String.Format("filename=Orcamento_{0}_{1}.pdf", CStr(estimateId), Replace(DateTime.Now.ToString(), ":", "-").Replace("/", "-")))
            ''Response.AppendHeader("content-disposition", "inline; filename=" & Server.MapPath(filePath & ".pdf"))
            'Response.Buffer = True
            ''Response.WriteFile(Server.MapPath(filePath & ".pdf"))
            'Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length)
            'Response.Flush()
            'Response.End()
            'Response.Close()

            'Return response
            'End Using

            Dim estimateHistory As New Models.EstimateHistory

            estimateHistory.EstimateId = dto.EstimateId
            estimateHistory.HistoryText = "<p>Orçamento enviado via email.</p>"
            estimateHistory.Locked = True
            estimateHistory.CreatedByUser = 0
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .SentEmail = sentMails})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Sends client notification about estimate being updated
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function sendEstimateNotification(dto As EstimatePdf) As HttpResponseMessage
        Try
            Dim portalCtrl = New Portals.PortalController()
            Dim _portalInfo = portalCtrl.GetPortal(dto.PortalId)
            Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(_portalInfo.PortalID)
            Dim _portalEmail = Portals.PortalController.GetPortalSetting("RIW_StoreEmail", _portalInfo.PortalID, _portalInfo.Email)

            Dim recipientList As New List(Of Users.UserInfo)

            Dim personCtrl As New PeopleRepository
            Dim personInfo As New Users.UserInfo()

            Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = _portalEmail, .DisplayName = _portalInfo.PortalName}

            Dim clientInfo = personCtrl.getPerson(dto.PersonId, dto.PortalId, Null.NullInteger)

            If clientInfo.UserId > 0 Then
                personInfo = Users.UserController.GetUserById(dto.PortalId, clientInfo.UserId)
            Else
                personInfo = New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = clientInfo.Email, .DisplayName = clientInfo.DisplayName}
            End If

            recipientList.Add(personInfo)

            Dim _salesInfo = Users.UserController.GetUserById(_portalInfo.PortalID, dto.SalesPersonId)

            Dim mailMessage As New Net.Mail.MailMessage
            Dim distList As New PostOffice

            mailMessage.From = New Net.Mail.MailAddress(storeUser.Email, _salesInfo.DisplayName)
            mailMessage.ReplyToList.Add(New Net.Mail.MailAddress(_salesInfo.Email, _salesInfo.DisplayName))
            mailMessage.Subject = dto.Subject
            mailMessage.Body = dto.MesssageBody
            mailMessage.IsBodyHtml = True

            Dim sentMails = distList.SendMail(mailMessage, recipientList, myPortalSettings("RIW_SMTPServer"), CInt(myPortalSettings("RIW_SMTPPort")), CBool(myPortalSettings("RIW_SMTPConnection")), myPortalSettings("RIW_SMTPLogin"), myPortalSettings("RIW_SMTPPassword"))

            Dim estimateHistory As New Models.EstimateHistory
            Dim estimateCtrl As New EstimatesRepository

            estimateHistory.EstimateId = dto.EstimateId
            estimateHistory.HistoryText = "<p>Notificação enviada ao cliente via email.</p>"
            estimateHistory.Locked = True
            estimateHistory.CreatedByUser = 0
            estimateHistory.CreatedOnDate = dto.ModifiedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            '' SignalR
            'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
            estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(estimateHistory)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .SentEmail = sentMails})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Deletes an estimate
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPut> _
    Function deleteEstimate(dto As Models.Estimate) As HttpResponseMessage
        Try
            Dim estimateHistory As New Models.EstimateHistory
            Dim estimate As New Models.Estimate
            Dim estimateCtrl As New EstimatesRepository

            estimate = estimateCtrl.GetEstimate(dto.EstimateId, dto.PortalId)

            estimate.IsDeleted = dto.IsDeleted
            estimate.ModifiedByUser = dto.ModifiedByUser
            estimate.ModifiedOnDate = dto.ModifiedOnDate

            estimateCtrl.deleteEstimate(estimate)

            Dim _historyText = ""
            If dto.IsDeleted Then
                _historyText = "<p>Orçamento desativado.</p>"
            Else
                _historyText = "<p>Orçamento ativado.</p>"
            End If

            estimateHistory.EstimateId = estimate.EstimateId
            estimateHistory.HistoryText = _historyText
            estimateHistory.Locked = True
            estimateHistory.CreatedByUser = 0
            estimateHistory.CreatedOnDate = dto.CreatedOnDate

            estimateCtrl.AddEstimateHistory(estimateHistory)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    <DnnAuthorize> _
    <HttpGet> _
    Function getMoreMessageComments(messageId As Integer) As HttpResponseMessage
        Try
            Dim estimateCtrl As New EstimatesRepository

            Dim commentsData = estimateCtrl.getEstimateMessageComments(messageId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.MessageComments = commentsData})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    '<DnnAuthorize> _
    '<HttpGet> _
    'Function getMoreMessageComments(messageId As Integer, comments As Integer) As HttpResponseMessage
    '    Try
    '        Dim estimateCtrl As New EstimatesRepository

    '        Dim counter = 0

    '        Do Until counter >= 300
    '            Threading.Thread.Sleep(4000)
    '            If getComments(messageId).Count > comments Then
    '                Exit Do
    '            Else
    '                counter = counter + 1
    '            End If
    '        Loop

    '        Dim commentsData = estimateCtrl.getEstimateMessageComments(messageId)
    '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.MessageComments = commentsData})
    '    Catch ex As Exception
    '        'DnnLog.Error(ex)
    '        Logger.[Error](ex)
    '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
    '    End Try
    'End Function

    'Function getComments(messageId As Integer) As IEnumerable(Of Models.EstimateMessageComment)
    '    Dim estimateCtrl As New EstimatesRepository
    '    Return estimateCtrl.getEstimateMessageComments(messageId)
    'End Function

    Public Class MessageComment
        Public Property dto As Models.EstimateMessageComment
        Public Property connId As String
        Public Property messageIndex As Integer
    End Class

    Public Class HistoryComment
        Public Property dto As Models.EstimateHistoryComment
        Public Property connId As String
        Public Property messageIndex As Integer
    End Class

    Public Class EstimateItemsRequest
        Property PortalId As Integer
        Property TotalAmount As Decimal
        Property EstimateItems As List(Of Models.EstimateItem)
        Property EstimateItemsRemoved As List(Of Models.EstimateItemRemoved)
        Property EstimateId As Integer
        Property Discount As Decimal
        Property Comment As String
        Property RemoveReasonId As String
        Property CreatedByUser As Integer
        Property CreatedOnDate As DateTime
        Property ModifiedByUser As Integer
        Property ModifiedOnDate As DateTime
        Property ConnId As String
        Property StatusId As Integer
        Property PersonId As Integer
        Property EstimateTitle As String
        Property Guid As String
        Property SalesRep As Integer
        Property ViewPrice As Boolean
        'Property ProductId As Integer
        'Property ProductName As String
        'Property ProductQty As Double
        'Property ProductEstimateOriginalPrice As Single
        'Property ProductEstimatePrice As Single
    End Class

    Public Class EstimatePdf
        Property EstimateId As Integer
        Property PortalId As Integer
        Property Lang As String
        Property ProductDiscountValue As Decimal
        Property Columns As List(Of String)
        Property ColumnsCount As Integer
        Property ProductOriginalAmount As Single
        Property EstimateDiscountValue As Single
        Property TotalDiscountPerc As Decimal
        Property TotalDiscountValue As Single
        Property EstimateTotalAmount As Single
        Property ToEmail As String
        Property Subject As String
        Property MesssageBody As String
        Property SalesPersonId As Integer
        Property PersonId As Integer
        Property ModifiedOnDate As DateTime
        Property ConnId As String
        Property Expand As Boolean
        Property Watermark As String
    End Class

End Class
