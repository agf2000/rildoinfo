
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Web.Api
Imports System.Net
Imports System.Net.Http
Imports RIW.Modules.Common
Imports RIW.Modules.WebAPI.Components.Models
Imports RIW.Modules.WebAPI.Components.Repositories

Public Class InvoicesController
    Inherits DnnApiController

#Region "Private Methods"

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(InvoicesController))

    Private lastLetter As String = "A"
    Private count As Integer = 0

#End Region

    Public Function GetNextLetter() As Char
        Dim letter As String = lastLetter
        If letter = "A" Then
            lastLetter = Chr(Asc(lastLetter) + 1)
            Return CChar(letter)
        End If

        lastLetter = Chr(Asc(lastLetter) + count)
        count = ++1

        Return CChar(lastLetter)
    End Function

    ''' <summary>
    ''' Get invoices by portal id
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="clientId">Client ID</param>
    ''' <param name="providerId">Provider ID</param>
    ''' <param name="productId">Product ID</param>
    ''' <param name="sDate">Emission Date Start</param>
    ''' <param name="eDate">Emission Date End</param>
    ''' <param name="pageNumber">Page Number</param>
    ''' <param name="pageSize">How many pages</param>
    ''' <param name="orderBy">Field to order by</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetInvoices(Optional portalId As Integer = 0, Optional providerId As Integer = -1, Optional clientId As Integer = -1, Optional productId As Integer = -1, Optional pageNumber As Integer = 1, Optional pageSize As Integer = 10, Optional orderBy As String = "", Optional sDate As String = Nothing, Optional eDate As String = Nothing) As HttpResponseMessage
        Try

            If sDate = Nothing Then
                sDate = Null.NullDate
            End If

            If eDate = Nothing Then
                eDate = Null.NullDate
            End If

            Dim invoicesCtrl As New InvoicesRepository

            Dim invoices = invoicesCtrl.GetInvoices(portalId, providerId, clientId, productId, CDate(sDate), CDate(eDate), pageNumber, pageSize, orderBy)

            Dim total = Nothing
            For Each item In invoices
                total = item.TotalRows
                Exit For
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.data = invoices, .total = total})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets product info by product id
    ''' </summary>
    ''' <param name="invoiceId">Invoice ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetInvoice(invoiceId As Integer) As HttpResponseMessage
        Try
            Dim invoiceCtrl As New InvoicesRepository

            Dim invoice = invoiceCtrl.GetInvoice(invoiceId)

            invoice.InvoiceItems = invoiceCtrl.GetInvoiceItems(invoiceId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .Invoice = invoice})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates an invoice
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function UpdateInvoice(dto As Invoice) As HttpResponseMessage
        Try
            Dim invoice As New Invoice
            Dim invoiceCtrl As New InvoicesRepository

            If dto.InvoiceId > 0 Then
                invoice = invoiceCtrl.GetInvoice(dto.InvoiceId)
            End If

            invoice.PortalId = dto.PortalId
            invoice.InvoiceNumber = dto.InvoiceNumber
            'invoice.InvoiceAmount = invoiceAmount
            'invoice.PayIn = dto.PayIn
            'invoice.PayQty = dto.PayQty
            'invoice.Interval = daysInterval
            'invoice.InterestRate = dto.InterestRate
            invoice.ProviderId = dto.ProviderId
            invoice.ClientId = dto.ClientId
            'invoice.EstimateId = dto.EstimateId
            invoice.Purchase = dto.Purchase
            invoice.DueDate = dto.DueDate
            invoice.EmissionDate = dto.EmissionDate
            invoice.Comment = dto.Comment
            invoice.CreatedByUser = dto.CreatedByUser
            invoice.CreatedOnDate = dto.CreatedOnDate
            invoice.ModifiedByUser = dto.CreatedByUser
            invoice.ModifiedOnDate = dto.CreatedOnDate

            If dto.InvoiceId > 0 Then
                invoiceCtrl.UpdateInvoice(invoice)
            Else
                invoiceCtrl.AddInvoice(invoice)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .Invoice = invoice})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates an invoice
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function UpdateInvoicePayment(dto As Invoice) As HttpResponseMessage
        Try
            Dim intCount = 1
            Dim daysInterval = CInt(If(dto.Interval = 0, 30, dto.Interval))
            Dim invoiceAmount = dto.InvoiceAmount
            dto.DueDate = New Date(dto.DueDate.Year(), dto.DueDate.Month(), dto.DueDate.Day, Utilities.GetRandom(8, 16), 0, 0)

            Dim invoice As New Invoice
            Dim invoiceCtrl As New InvoicesRepository

            invoice = invoiceCtrl.GetInvoice(dto.InvoiceId)

            invoice.PortalId = dto.PortalId
            invoice.InvoiceAmount = invoiceAmount
            invoice.PayIn = dto.PayIn
            invoice.PayQty = dto.PayQty
            invoice.Interval = daysInterval
            invoice.InterestRate = dto.InterestRate
            invoice.Freight = dto.Freight
            invoice.DueDate = dto.DueDate
            invoice.CreditDebit = dto.CreditDebit
            invoice.CreatedByUser = dto.CreatedByUser
            invoice.CreatedOnDate = dto.CreatedOnDate
            invoice.ModifiedByUser = dto.CreatedByUser
            invoice.ModifiedOnDate = dto.CreatedOnDate

            Dim payment As New Payment
            Dim paymentCtrl As New PaymentsRepository

            payment.PortalId = dto.PortalId
            payment.DocId = dto.InvoiceId
            payment.Credit = CSng(If(dto.CreditDebit, invoiceAmount, 0))
            payment.Debit = CSng(If(dto.CreditDebit, 0, invoiceAmount)) ' dto.PayIn
            payment.InterestRate = 0
            payment.ClientId = dto.ClientId
            payment.ProviderId = dto.ProviderId
            payment.AccountId = 1
            payment.DueDate = dto.DueDate
            payment.Done = False
            payment.OriginId = -1
            payment.TransDate = New Date(1900, 1, 1)
            payment.Comment = dto.Comment
            payment.CreatedByUser = dto.CreatedByUser
            payment.CreatedOnDate = dto.CreatedOnDate
            payment.ModifiedByUser = dto.ModifiedByUser
            payment.ModifiedOnDate = dto.ModifiedOnDate

            Dim appt As New Agenda
            Dim apptCtrl As New AgendasRepository

            appt.PortalId = dto.PortalId
            appt.PersonId = dto.ClientId
            appt.UserId = -1
            appt.Subject = dto.Comment
            appt.StartDateTime = dto.DueDate
            appt.EndDateTime = dto.DueDate.AddHours(1)
            appt.Reminder = "BEGIN:VALARM  TRIGGER:-PT2880M  X-TELERIK-UID:0440c18f-ce88-4934-ac5e-5d134420d4c5  END:VALARM"
            appt.ModifiedByUser = dto.CreatedByUser
            appt.ModifiedOnDate = dto.CreatedOnDate
            appt.CreatedByUser = dto.CreatedByUser
            appt.CreatedOnDate = dto.CreatedOnDate

            invoiceCtrl.UpdateInvoice(invoice)

            If dto.PayQty > 0 Then

                If dto.PayIn > 0 Then

                    invoiceAmount = invoiceAmount - dto.PayIn

                    If dto.CreditDebit Then
                        payment.Credit = dto.PayIn ' invoiceAmount
                    Else
                        payment.Debit = dto.PayIn ' invoiceAmount
                    End If

                    payment.Comment = GetNextLetter() & ": " & dto.Comment ' dto.Comment.Replace("[ABC]", "")

                    paymentCtrl.AddPayment(payment)

                    appt.Subject = payment.Comment

                    apptCtrl.AddAppointmentData(appt)

                End If

                invoiceAmount = invoiceAmount / dto.PayQty

                If dto.CreditDebit Then
                    payment.Credit = invoiceAmount
                Else
                    payment.Debit = invoiceAmount
                End If

                payment.Comment = GetNextLetter() & ": " & dto.Comment

                paymentCtrl.AddPayment(payment)

                appt.Subject = payment.Comment

                apptCtrl.AddAppointmentData(appt)

                intCount = intCount + 1

                Do While intCount <= dto.PayQty

                    dto.DueDate = dto.DueDate.AddDays(daysInterval)

                    payment.DueDate = dto.DueDate

                    payment.Comment = GetNextLetter() & ": " & dto.Comment

                    paymentCtrl.AddPayment(payment)

                    appt.StartDateTime = dto.DueDate

                    appt.EndDateTime = dto.DueDate.AddHours(1)

                    appt.Subject = payment.Comment

                    apptCtrl.AddAppointmentData(appt)

                    intCount = intCount + 1

                Loop

            Else

                paymentCtrl.AddPayment(payment)

                apptCtrl.AddAppointmentData(appt)

            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ' ''' <summary>
    ' ''' Updates an invoice
    ' ''' </summary>
    ' ''' <param name="dto"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    '<DnnAuthorize> _
    '<HttpPost> _
    'Function updateInvoice(dto As Invoice) As HttpResponseMessage
    '    Try
    '        Dim intCount = 1
    '        Dim daysInterval = CInt(IIf(dto.Interval = 0, 30, dto.Interval))
    '        Dim invoiceAmount = dto.InvoiceAmount
    '        dto.DueDate = New Date(dto.DueDate.Year(), dto.DueDate.Month(), dto.DueDate.Day, Utilities.GetRandom(8, 16), 0, 0)

    '        Dim invoice As New Invoice
    '        Dim invoiceCtrl As New InvoicesRepository

    '        If dto.InvoiceId > 0 Then
    '            invoice = invoiceCtrl.GetInvoice(dto.InvoiceId)
    '        End If

    '        invoice.PortalId = dto.PortalId
    '        invoice.InvoiceNumber = dto.InvoiceNumber
    '        invoice.InvoiceAmount = invoiceAmount
    '        invoice.PayIn = dto.PayIn
    '        invoice.PayQty = dto.PayQty
    '        invoice.Interval = daysInterval
    '        invoice.InterestRate = dto.InterestRate
    '        invoice.ProviderId = dto.ProviderId
    '        invoice.ClientId = dto.ClientId
    '        invoice.EstimateId = dto.EstimateId
    '        invoice.DueDate = dto.DueDate
    '        invoice.EmissionDate = dto.EmissionDate
    '        invoice.Comment = dto.Comment
    '        invoice.CreatedByUser = dto.CreatedByUser
    '        invoice.CreatedOnDate = dto.CreatedOnDate
    '        invoice.ModifiedByUser = dto.CreatedByUser
    '        invoice.ModifiedOnDate = dto.CreatedOnDate

    '        Dim payment As New Payment
    '        Dim paymentCtrl As New PaymentsRepository

    '        payment.PortalId = dto.PortalId
    '        payment.DocId = dto.InvoiceId
    '        payment.Credit = 0
    '        payment.Debit = invoiceAmount ' dto.PayIn
    '        payment.InterestRate = 0
    '        payment.ClientId = dto.ClientId
    '        payment.ProviderId = dto.ProviderId
    '        payment.AccountId = 1
    '        payment.DueDate = dto.DueDate
    '        payment.Done = False
    '        payment.TransDate = New Date(1900, 1, 1)
    '        payment.Comment = dto.Comment
    '        payment.CreatedByUser = dto.CreatedByUser
    '        payment.CreatedOnDate = dto.CreatedOnDate
    '        payment.ModifiedByUser = dto.ModifiedByUser
    '        payment.ModifiedOnDate = dto.ModifiedOnDate

    '        Dim appt As New Agenda
    '        Dim apptCtrl As New AgendasRepository

    '        appt.PortalId = dto.PortalId
    '        appt.PersonId = dto.ClientId
    '        appt.UserId = -1
    '        appt.Subject = dto.Comment
    '        appt.StartDateTime = dto.DueDate
    '        appt.EndDateTime = dto.DueDate.AddHours(1)
    '        appt.Reminder = "BEGIN:VALARM  TRIGGER:-PT2880M  X-TELERIK-UID:0440c18f-ce88-4934-ac5e-5d134420d4c5  END:VALARM"
    '        appt.ModifiedByUser = dto.CreatedByUser
    '        appt.ModifiedOnDate = dto.CreatedOnDate
    '        appt.CreatedByUser = dto.CreatedByUser
    '        appt.CreatedOnDate = dto.CreatedOnDate

    '        If dto.InvoiceId > 0 Then

    '            invoiceCtrl.UpdateInvoice(invoice)

    '            If dto.PayQty > 0 Then

    '                If dto.PayIn > 0 Then

    '                    invoiceAmount = invoiceAmount - dto.PayIn

    '                    payment.Debit = dto.PayIn ' invoiceAmount

    '                    payment.Comment = dto.Comment.Replace("[ABC]", "")

    '                    paymentCtrl.AddPayment(payment)

    '                    appt.Subject = payment.Comment

    '                    apptCtrl.AddAppointmentData(appt)

    '                End If

    '                invoiceAmount = invoiceAmount / dto.PayQty

    '                payment.Debit = invoiceAmount

    '                payment.Comment = dto.Comment.Replace("[ABC]", getNextLetter())

    '                paymentCtrl.AddPayment(payment)

    '                appt.Subject = payment.Comment

    '                apptCtrl.AddAppointmentData(appt)

    '                intCount = intCount + 1

    '                Do While intCount <= dto.PayQty

    '                    dto.DueDate = dto.DueDate.AddDays(daysInterval)

    '                    payment.DueDate = dto.DueDate

    '                    payment.Comment = dto.Comment.Replace("[ABC]", getNextLetter())

    '                    paymentCtrl.AddPayment(payment)

    '                    appt.StartDateTime = dto.DueDate

    '                    appt.EndDateTime = dto.DueDate.AddHours(1)

    '                    appt.Subject = payment.Comment

    '                    apptCtrl.AddAppointmentData(appt)

    '                    intCount = intCount + 1

    '                Loop

    '            Else

    '                paymentCtrl.AddPayment(payment)

    '                apptCtrl.AddAppointmentData(appt)

    '            End If

    '            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})

    '        Else

    '            invoiceCtrl.AddInvoice(invoice)

    '            payment.DocId = invoice.InvoiceId

    '            appt.DocId = invoice.InvoiceId

    '            If dto.PayQty > 0 Then

    '                If dto.PayIn > 0 Then

    '                    invoiceAmount = invoiceAmount - dto.PayIn

    '                    payment.Debit = dto.PayIn ' invoiceAmount

    '                    payment.Comment = dto.Comment.Replace("[ABC]", "")

    '                    paymentCtrl.AddPayment(payment)

    '                    appt.Subject = payment.Comment

    '                    apptCtrl.AddAppointmentData(appt)

    '                End If

    '                invoiceAmount = invoiceAmount / dto.PayQty

    '                payment.Debit = invoiceAmount

    '                payment.Comment = dto.Comment.Replace("[ABC]", getNextLetter())

    '                paymentCtrl.AddPayment(payment)

    '                appt.Subject = payment.Comment

    '                apptCtrl.AddAppointmentData(appt)

    '                intCount = intCount + 1

    '                Do While intCount <= dto.PayQty

    '                    dto.DueDate = dto.DueDate.AddDays(daysInterval)

    '                    payment.DueDate = dto.DueDate

    '                    payment.Comment = dto.Comment.Replace("[ABC]", getNextLetter())

    '                    paymentCtrl.AddPayment(payment)

    '                    appt.StartDateTime = dto.DueDate

    '                    appt.EndDateTime = dto.DueDate.AddHours(1)

    '                    appt.Subject = payment.Comment

    '                    apptCtrl.AddAppointmentData(appt)

    '                    intCount = intCount + 1

    '                Loop

    '            Else

    '                paymentCtrl.AddPayment(payment)

    '                apptCtrl.AddAppointmentData(appt)

    '            End If

    '            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .InvoiceId = invoice.InvoiceId})

    '        End If

    '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
    '    Catch ex As Exception
    '        'DnnLog.Error(ex)
    '        Logger.[Error](ex)
    '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
    '    End Try
    'End Function

    ''' <summary>
    ''' Removes an invoice by invoice id
    ''' </summary>
    ''' <param name="invoiceId">Invoice ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpDelete> _
    Function RemoveInvoice(invoiceId As Integer) As HttpResponseMessage
        Try
            Dim invoiceCtrl As New InvoicesRepository

            invoiceCtrl.RemoveInvoice(invoiceId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ' ''' <summary>
    ' ''' Gets a list if invoice items by invoice id
    ' ''' </summary>
    ' ''' <param name="invoiceId">Invoice ID</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    '<DnnAuthorize> _
    '<HttpGet> _
    'Function getInvoiceItems(invoiceId As Integer) As HttpResponseMessage
    '    Try
    '        Dim invoiceItems As IEnumerable(Of InvoiceItem)
    '        Dim invoiceItemsCtrl As New InvoicesRepository

    '        invoiceItems = invoiceItemsCtrl.GetInvoiceItems(invoiceId)

    '        Return Request.CreateResponse(HttpStatusCode.OK, invoiceItems)
    '    Catch ex As Exception
    '        'DnnLog.Error(ex)
    '        Logger.[Error](ex)
    '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
    '    End Try
    'End Function

    ''' <summary>
    ''' Adds invoice items
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function AddInvoiceItem(dto As InvoiceItem) As HttpResponseMessage
        Try
            Dim invoiceItem As New InvoiceItem
            Dim invoiceCtrl As New InvoicesRepository

            invoiceItem.Discount = dto.Discount
            invoiceItem.InvoiceId = dto.InvoiceId
            invoiceItem.ProductId = dto.ProductId
            invoiceItem.Qty = dto.Qty
            invoiceItem.UnitValue1 = dto.UnitValue1
            invoiceItem.UnitValue2 = dto.UnitValue2
            invoiceItem.UpdateProduct = dto.UpdateProduct
            invoiceItem.CreatedByUser = dto.CreatedByUser
            invoiceItem.CreatedOnDate = dto.CreatedOnDate
            invoiceItem.ModifiedByUser = dto.ModifiedByUser
            invoiceItem.ModifiedOnDate = dto.ModifiedOnDate

            invoiceCtrl.AddInvoiceItem(invoiceItem)

            If Not dto.Purchase Then

                Dim product As New Product
                Dim productCtrl As New ProductsRepository

                product = productCtrl.GetProduct(dto.ProductId, "pt-BR")

                Dim oldStock = product.QtyStockSet

                product.QtyStockSet = oldStock + dto.Qty
                product.ModifiedByUser = dto.ModifiedByUser
                product.ModifiedOnDate = dto.ModifiedOnDate

                productCtrl.UpdateProduct(product)

                If dto.UpdateProduct Then
                    Dim productFinance As New ProductFinance

                    productFinance = productCtrl.GetProductFinance(dto.ProductId)

                    productFinance.ProductId = dto.ProductId
                    productFinance.Finan_Paid = dto.UnitValue2
                    productFinance.Finan_Paid_Discount = dto.Discount

                    productCtrl.UpdateProductFinance(productFinance)
                End If
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .InvoiceItem = invoiceItem})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates invoice items
    ''' </summary>
    ''' <param name="invoiceItems"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function UpdateInvoiceItem(invoiceItems As List(Of InvoiceItem)) As HttpResponseMessage
        Try
            Dim invoiceItem As New InvoiceItem
            Dim invoiceCtrl As New InvoicesRepository

            For Each item In invoiceItems

                invoiceItem = invoiceCtrl.GetInvoiceItem(item.InvoiceItemId, item.InvoiceId)

                invoiceItem.Discount = item.Discount
                invoiceItem.ProductId = item.ProductId
                invoiceItem.Qty = item.Qty
                invoiceItem.UnitValue1 = item.UnitValue1
                invoiceItem.UnitValue2 = item.UnitValue2
                invoiceItem.ModifiedByUser = item.ModifiedByUser
                invoiceItem.ModifiedOnDate = item.ModifiedOnDate

                invoiceCtrl.UpdateInvoiceItem(invoiceItem)

                Dim product As New Product
                Dim productCtrl As New ProductsRepository

                product = productCtrl.GetProduct(item.ProductId, "pt-BR")

                Dim oldStock = product.QtyStockSet

                product.QtyStockSet = oldStock - item.Qty
                product.ModifiedByUser = item.ModifiedByUser
                product.ModifiedOnDate = item.ModifiedOnDate

                productCtrl.UpdateProduct(product)

                Dim productFinance As New ProductFinance

                productFinance = productCtrl.GetProductFinance(item.ProductId)

                productFinance.ProductId = item.ProductId
                productFinance.Finan_Cost = item.UnitValue2

                productCtrl.UpdateProductFinance(productFinance)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes an invoice item by invoice item id
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function RemoveInvoiceItem(dto As InvoiceItem) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository
            Dim invoiceItemsCtrl As New InvoicesRepository

            Dim productHistory = productCtrl.GetProductHistory(dto.ProductId)

            Dim lastAmount = 0.0
            Dim discountAmount = 0.0

            If productHistory.Count > 0 Then
                If productHistory.Count > 2 Then
                    lastAmount = productHistory((productHistory.Count - 2)).UnitValue2
                    discountAmount = productHistory((productHistory.Count - 2)).Discount
                ElseIf productHistory.Count > 1 Then
                    lastAmount = productHistory((productHistory.Count - 1)).UnitValue2
                    discountAmount = productHistory((productHistory.Count - 1)).Discount
                Else
                    lastAmount = productHistory(0).UnitValue2
                    discountAmount = productHistory(0).Discount
                End If
            End If

            Dim productFinance As New ProductFinance

            productFinance = productCtrl.GetProductFinance(dto.ProductId)

            productFinance.Finan_Paid = lastAmount
            productFinance.Finan_Paid_Discount = discountAmount

            productCtrl.UpdateProductFinance(productFinance)

            Dim product = productCtrl.GetProduct(dto.ProductId, "pt-BR")

            Dim oldStock = product.QtyStockSet

            product.QtyStockSet = oldStock - dto.Qty
            product.ModifiedByUser = dto.ModifiedByUser
            product.ModifiedOnDate = dto.ModifiedOnDate

            productCtrl.UpdateProduct(product)

            invoiceItemsCtrl.RemoveInvoiceItem1(dto.InvoiceItemId, dto.InvoiceId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets a list of payments
    ''' </summary>
    ''' <param name="portalId">POrtal ID</param>
    ''' <param name="accountId">Account ID</param>
    ''' <param name="originId">Origin ID</param>
    ''' <param name="providerId">Provider ID</param>
    ''' <param name="clientId">Client ID</param>
    ''' <param name="category">Credit or Debit</param>
    ''' <param name="sDate">Start Transaction Date</param>
    ''' <param name="eDate">End Transaction Date</param>
    ''' <param name="sTerm">Search Term</param>
    ''' <param name="done">Done</param>
    ''' <param name="pageNumber">Page Number</param>
    ''' <param name="pageSize">Page Size</param> 
    ''' <param name="orderBy">Order By</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetPayments(portalId As Integer, accountId As Integer, originId As Integer, providerId As Integer, clientId As Integer, pageNumber As Integer, pageSize As Integer, orderBy As String, Optional done As String = "", Optional category As String = "", Optional sTerm As String = "", Optional sDate As String = Nothing, Optional eDate As String = Nothing, Optional filterDate As String = "0") As HttpResponseMessage
        Try

            If sDate = Nothing Then
                sDate = CStr(Null.NullDate)
            End If

            If eDate = Nothing Then
                eDate = CStr(Null.NullDate)
            End If

            Select Case filterDate
                Case "ModifiedOnDate"
                    filterDate = "2"
                Case "CreatedOnDate"
                    filterDate = "3"
                Case Else
                    filterDate = "1"
            End Select

            Dim paymentsCtrl As New PaymentsRepository

            Dim payments = paymentsCtrl.GetPayments(portalId, accountId, originId, providerId, clientId, category.Replace("""", ""), done.Replace("""", ""), sTerm.Replace("""", ""), CDate(sDate), CDate(eDate), filterDate, pageNumber, pageSize, orderBy)

            Dim total = Nothing
            For Each item In payments
                total = item.TotalRows
                Exit For
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.data = payments, .total = total})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets a payment into
    ''' </summary>
    ''' <param name="pmtId">Payment ID</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetPayment(pmtId As Integer, portalId As Integer) As HttpResponseMessage
        Try
            Dim paymentCtrl As New PaymentsRepository

            Dim payment = paymentCtrl.GetPayment(pmtId, portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, payment)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Add or updates payment
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function UpdatePayment(dto As Payment) As HttpResponseMessage
        Try
            'If dto.DueDate = Nothing Then
            '    dto.DueDate = CDate(IIf(Not dto.TransDate = Nothing, dto.TransDate, Today))
            'End If

            If dto.TransDate = Nothing Then
                dto.TransDate = New Date(1900, 1, 1)
            End If

            dto.DueDate = New Date(dto.DueDate.Year(), dto.DueDate.Month(), dto.DueDate.Day, Utilities.GetRandom(8, 16), 0, 0)

            Dim payment As New Payment
            Dim paymentCtrl As New PaymentsRepository

            If dto.PaymentId > 0 Then
                payment = paymentCtrl.GetPayment(dto.PaymentId, dto.PortalId)
            End If

            payment.AccountId = dto.AccountId
            payment.TransId = dto.TransId
            payment.Credit = dto.Credit
            payment.Debit = dto.Debit
            payment.InterestRate = dto.InterestRate
            payment.Fee = dto.Fee
            payment.DocId = dto.DocId
            payment.ClientId = dto.ClientId
            payment.ProviderId = dto.ProviderId
            payment.AccountId = dto.AccountId
            payment.OriginId = dto.OriginId
            payment.DueDate = dto.DueDate
            payment.Done = dto.Done
            payment.Comment = dto.Comment
            payment.TransDate = dto.TransDate
            payment.CreatedByUser = dto.CreatedByUser
            payment.CreatedOnDate = dto.CreatedOnDate
            payment.ModifiedByUser = dto.ModifiedByUser
            payment.ModifiedOnDate = dto.ModifiedOnDate

            If dto.Fee > 0 Or dto.InterestRate > 0 Then
                payment.AmountPaid = dto.Debit + dto.Debit + (dto.Debit * dto.InterestRate / 100)
            Else
                payment.AmountPaid = dto.Debit
            End If

            If dto.PaymentId > 0 Then
                paymentCtrl.UpdatePayment(payment)
            Else
                paymentCtrl.AddPayment(payment)
            End If

            Dim agendaCtrl As New AgendasRepository

            Dim appts = agendaCtrl.GetAgendaData(dto.PortalId, Null.NullInteger, dto.OriginalDueDate, Null.NullDate, dto.DocId)

            If appts.Count > 0 Then
                For Each item In appts
                    Dim appt = agendaCtrl.GetAppointmentData(item.AppointmentId, dto.PortalId)

                    appt.StartDateTime = dto.DueDate

                    agendaCtrl.UpdateAppointmentData(appt)
                Next
            End If

            If dto.Agenda Then
                Dim appt As New Agenda

                appt.PortalId = dto.PortalId
                appt.PersonId = dto.ClientId
                appt.UserId = -1
                appt.Subject = dto.Comment
                appt.StartDateTime = dto.DueDate
                appt.EndDateTime = dto.DueDate.AddMinutes(30)
                appt.Reminder = "BEGIN:VALARM  TRIGGER:-PT2880M  X-TELERIK-UID:0440c18f-ce88-4934-ac5e-5d134420d4c5  END:VALARM"
                appt.ModifiedByUser = dto.CreatedByUser
                appt.ModifiedOnDate = dto.ModifiedOnDate
                appt.CreatedByUser = dto.CreatedByUser
                appt.CreatedOnDate = dto.CreatedOnDate

                agendaCtrl.AddAppointmentData(appt)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .PaymentId = payment.PaymentId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Cancels payment
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPut> _
    Function DeletePayment(dto As Payment) As HttpResponseMessage
        Try
            Dim paymentCtrl As New PaymentsRepository

            Dim payment = paymentCtrl.GetPayment(dto.PaymentId, dto.PortalId)

            payment.IsDeleted = True
            payment.ModifiedByUser = dto.ModifiedByUser
            payment.ModifiedOnDate = dto.ModifiedOnDate

            paymentCtrl.UpdatePayment(payment)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Cancels payment
    ''' </summary>
    ''' <param name="Payments"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPut> _
    Function RemovePayments(dto As Invoice) As HttpResponseMessage
        Try
            Dim paymentCtrl As New PaymentsRepository

            For Each payment In dto.Payments

                paymentCtrl.RemovePayment(payment.PaymentId, dto.PortalId)

            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Restores payment
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPut> _
    Function RestorePayment(dto As Payment) As HttpResponseMessage
        Try
            Dim paymentCtrl As New PaymentsRepository

            Dim payment = paymentCtrl.GetPayment(dto.PaymentId, dto.PortalId)

            payment.IsDeleted = False
            payment.ModifiedByUser = dto.ModifiedByUser
            payment.ModifiedOnDate = dto.ModifiedOnDate

            paymentCtrl.UpdatePayment(payment)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

End Class
