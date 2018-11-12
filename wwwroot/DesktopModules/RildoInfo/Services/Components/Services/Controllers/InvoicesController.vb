
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Web.Api
Imports System.Globalization
Imports System.Net
Imports System.Net.Http

Public Class InvoicesController
    Inherits DnnApiController

#Region "Private Methods"

    Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(AccountsController))

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
    ''' <param name="personId">Client ID</param>
    ''' <param name="providerId">Provider ID</param>
    ''' <param name="prodId">Product ID</param>
    ''' <param name="sDate">Emission Date Start</param>
    ''' <param name="eDate">Emission Date End</param>
    ''' <param name="pageNumber">Page Number</param>
    ''' <param name="pageSize">How many pages</param>
    ''' <param name="orderBy">Field to order by</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetInvoices(ByVal portalId As String, ByVal providerId As String, ByVal personId As String, ByVal prodId As String, ByVal pageNumber As Integer, ByVal pageSize As Integer, orderBy As String, Optional sDate As String = Nothing, Optional eDate As String = Nothing) As HttpResponseMessage
        Try

            If Not sDate = Nothing Then
                sDate = CStr(Null.NullDate)
            End If

            If Not eDate = Nothing Then
                eDate = CStr(Null.NullDate)
            End If

            Dim invoices As IEnumerable(Of Models.Invoice)
            Dim invoicesCtrl As New InvoicesRepository

            invoices = invoicesCtrl.GetInvoices(portalId, providerId, personId, prodId, CDate(sDate), CDate(eDate), pageNumber, pageSize, orderBy)

            Dim total = Nothing
            For Each item In invoices
                total = item.TotalRows
                Exit For
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.data = invoices, .total = total})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
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
    Function GetInvoice(ByVal invoiceId As Integer) As HttpResponseMessage
        Try
            Dim invoice As New Models.Invoice
            Dim invoiceCtrl As New InvoicesRepository

            invoice = invoiceCtrl.GetInvoice(invoiceId)

            Return Request.CreateResponse(HttpStatusCode.OK, invoice)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
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
    Function UpdateInvoice(dto As Models.Invoice) As HttpResponseMessage
        Try
            Dim culture = New CultureInfo("pt-BR")
            Dim numInfo = culture.NumberFormat
            Dim intCount = 1
            Dim daysInterval = CInt(IIf(dto.Interval = 0, 30, dto.Interval))
            Dim invoiceAmount = dto.InvoiceAmount ' Decimal.Parse(dto.InvoiceAmount.Replace(".", ","), numInfo)
            dto.DueDate = New Date(dto.DueDate.Year(), dto.DueDate.Month(), dto.DueDate.Day, Utilities.GetRandom(8, 16), 0, 0)

            Dim invoice As New Models.Invoice
            Dim invoiceCtrl As New InvoicesRepository

            If dto.InvoiceId > 0 Then
                invoice = invoiceCtrl.GetInvoice(dto.InvoiceId)
            End If

            invoice.PortalId = dto.PortalId
            invoice.InvoiceNumber = dto.InvoiceNumber
            invoice.InvoiceAmount = invoiceAmount
            invoice.PayIn = dto.PayIn
            invoice.PayQty = dto.PayQty
            invoice.Interval = daysInterval
            invoice.InterestRate = dto.InterestRate
            invoice.ProviderId = dto.ProviderId
            invoice.PersonId = dto.PersonId
            invoice.EstimateId = dto.EstimateId
            invoice.DueDate = dto.DueDate
            invoice.EmissionDate = dto.EmissionDate
            invoice.Comment = dto.Comment
            invoice.CreatedByUser = dto.CreatedByUser
            invoice.CreatedOnDate = dto.CreatedOnDate
            invoice.ModifiedByUser = dto.CreatedByUser
            invoice.ModifiedOnDate = dto.CreatedOnDate

            Dim payment As New Models.Payment
            Dim paymentCtrl As New PaymentsRepository

            payment.PortalId = dto.PortalId
            payment.DocId = dto.InvoiceId
            payment.Credit = 0
            payment.Debit = dto.PayIn
            payment.InterestRate = 0
            payment.PersonId = dto.PersonId
            payment.ProviderId = dto.ProviderId
            payment.AccountId = 1
            payment.DueDate = dto.DueDate
            payment.Done = False
            payment.Comment = dto.Comment.Replace("[ABC]", GetNextLetter())
            payment.ModifiedByUser = dto.CreatedByUser
            payment.ModifiedOnDate = dto.CreatedOnDate

            Dim appt As New Models.Agenda
            Dim apptCtrl As New AgendasRepository

            appt.PortalId = dto.PortalId
            appt.PersonId = dto.PersonId
            appt.Subject = dto.Comment
            appt.StartDateTime = dto.DueDate
            appt.EndDateTime = dto.DueDate.AddHours(1)
            appt.Reminder = "BEGIN:VALARM  TRIGGER:-PT2880M  X-TELERIK-UID:0440c18f-ce88-4934-ac5e-5d134420d4c5  END:VALARM"
            appt.ModifiedByUser = dto.CreatedByUser
            appt.ModifiedOnDate = dto.CreatedOnDate
            appt.CreatedByUser = dto.CreatedByUser
            appt.CreatedOnDate = dto.CreatedOnDate

            If dto.InvoiceId > 0 Then

                invoiceCtrl.UpdateInvoice(invoice)

                If dto.PayQty > 0 Then

                    If dto.PayIn > 0 Then

                        invoiceAmount = invoiceAmount - dto.PayIn

                        payment.Debit = invoiceAmount

                        paymentCtrl.AddPayment(payment)

                        apptCtrl.AddAppointmentData(appt)

                    End If

                    invoiceAmount = invoiceAmount / dto.PayQty

                    payment.Debit = invoiceAmount

                    paymentCtrl.AddPayment(payment)

                    apptCtrl.AddAppointmentData(appt)

                    intCount = intCount + 1

                    Do While intCount <= dto.PayQty

                        dto.DueDate = dto.DueDate.AddDays(daysInterval)

                        payment.DueDate = dto.DueDate

                        paymentCtrl.AddPayment(payment)

                        appt.StartDateTime = dto.DueDate

                        apptCtrl.AddAppointmentData(appt)

                        intCount = intCount + 1

                    Loop

                Else

                    paymentCtrl.AddPayment(payment)

                    apptCtrl.AddAppointmentData(appt)

                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})

            Else

                invoiceCtrl.AddInvoice(invoice)

                payment.DocId = invoice.InvoiceId

                appt.DocId = invoice.InvoiceId

                If dto.PayQty > 0 Then

                    If dto.PayIn > 0 Then

                        invoiceAmount = invoiceAmount - dto.PayIn

                        payment.Debit = invoiceAmount

                        paymentCtrl.AddPayment(payment)

                        apptCtrl.AddAppointmentData(appt)

                    End If

                    invoiceAmount = invoiceAmount / dto.PayQty

                    payment.Debit = invoiceAmount

                    paymentCtrl.AddPayment(payment)

                    apptCtrl.AddAppointmentData(appt)

                    intCount = intCount + 1

                    Do While intCount <= dto.PayQty

                        dto.DueDate = dto.DueDate.AddDays(daysInterval)

                        payment.DueDate = dto.DueDate

                        paymentCtrl.AddPayment(payment)

                        appt.StartDateTime = dto.DueDate

                        apptCtrl.AddAppointmentData(appt)

                        intCount = intCount + 1

                    Loop

                Else

                    paymentCtrl.AddPayment(payment)

                    apptCtrl.AddAppointmentData(appt)

                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .InvoiceId = invoice.InvoiceId})

            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes an invoice by invoice id
    ''' </summary>
    ''' <param name="invoiceId">Invoice ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpDelete> _
    Function RemoveInvoice(ByVal invoiceId As Integer) As HttpResponseMessage
        Try
            Dim invoiceCtrl As New InvoicesRepository

            invoiceCtrl.RemoveInvoice(invoiceId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds an invoice item by invoice item id
    ''' </summary>
    ''' <param name="iItemId">Invoice Detail ID</param>
    ''' <param name="prodId">Product ID</param>
    ''' <param name="qty">Quantity</param>
    ''' <param name="discount">Discount</param>
    ''' <param name="uValue">Unit Value</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function UpdateInvoiceItem(ByVal invoiceId As Integer,
        ByVal prodId As Integer,
        ByVal qty As String,
        ByVal discount As String,
        ByVal uValue As String,
        ByVal uId As Integer,
        ByVal md As Date,
        Optional ByVal iItemId As Integer = -1) As HttpResponseMessage
        Try
            Dim culture = New CultureInfo("pt-BR")
            Dim numInfo = culture.NumberFormat

            Dim invoiceItem As New Models.InvoiceItem
            Dim invoiceCtrl As New InvoicesRepository

            If Not iItemId = Nothing Then
                invoiceItem = invoiceCtrl.GetInvoiceItem(iItemId, invoiceId)
            End If

            invoiceItem.Discount = Decimal.Parse(discount.Replace(".", ","), numInfo)
            invoiceItem.InvoiceId = invoiceId
            invoiceItem.ProdId = prodId
            invoiceItem.Qty = Decimal.Parse(qty.Replace(".", ","), numInfo)
            invoiceItem.UnitValue = Decimal.Parse(uValue.Replace(".", ","), numInfo)

            If Not iItemId = Nothing Then
                invoiceCtrl.UpdateInvoiceItem(invoiceItem)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Else
                invoiceCtrl.AddInvoiceItem(invoiceItem)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .InvoiceDetailId = invoiceItem.InvoiceItemId})
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets a list if invoice items by invoice id
    ''' </summary>
    ''' <param name="invoiceId">Invoice ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetInvoiceItems(ByVal invoiceId As Integer) As HttpResponseMessage
        Try
            Dim invoiceItems As IEnumerable(Of Models.InvoiceItem)
            Dim invoiceItemsCtrl As New InvoicesRepository

            invoiceItems = invoiceItemsCtrl.GetInvoiceItems(invoiceId)

            Return Request.CreateResponse(HttpStatusCode.OK, invoiceItems)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ' ''' <summary>
    ' ''' Removes an invoice item by invoice item id
    ' ''' </summary>
    ' ''' <param name="iItemId">Invoice Item ID</param>
    ' ''' <param name="invoiceId">Invoice ID</param>
    ' ''' <param name="prodId">Product ID</param>
    ' ''' <param name="qty">Quantity</param>
    ' ''' <param name="uId">ModifiedByUser</param>
    ' ''' <param name="md">Modified On Date</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    '<DnnAuthorize> _
    '<HttpPost> _
    'Function RemoveInvoiceItem(ByVal invoiceId As Integer, ByVal iItemId As Integer, ByVal prodId As Integer, ByVal qty As String, uId As Integer, md As DateTime) As HttpResponseMessage
    '    Try
    '        Dim culture = New CultureInfo("pt-BR")
    '        Dim numInfo = culture.NumberFormat

    '        Dim invoiceItemsCtrl As New InvoicesRepository

    '        invoiceItemsCtrl.RemoveInvoiceItem(iItemId, invoiceId)

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

    ''' <summary>
    ''' Gets a list of payments
    ''' </summary>
    ''' <param name="portalId">POrtal ID</param>
    ''' <param name="account">Account ID</param>
    ''' <param name="provider">Provider ID</param>
    ''' <param name="client">Client ID</param>
    ''' <param name="cat">Credit or Debit</param>
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
    Function GetPayments(ByVal portalId As String, ByVal account As String, ByVal provider As String, ByVal client As String, ByVal cat As String, ByVal done As Boolean, ByVal sTerm As String, ByVal pageNumber As Integer, ByVal pageSize As Integer, orderBy As String, Optional ByVal sDate As String = Nothing, Optional ByVal eDate As String = Nothing) As HttpResponseMessage
        Try

            If sDate = Nothing Then
                sDate = CStr(Null.NullDate)
            End If

            If eDate = Nothing Then
                eDate = CStr(Null.NullDate)
            End If

            Dim payments As IEnumerable(Of Models.Payment)
            Dim paymentsCtrl As New PaymentsRepository

            payments = paymentsCtrl.GetPayments(portalId, account, provider, client, cat, done, sTerm, CDate(sDate), CDate(eDate), pageNumber, pageSize, orderBy)

            Dim total = Nothing
            For Each item In payments
                total = item.TotalRows
                Exit For
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.data = payments, .total = total})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets a payment into
    ''' </summary>
    ''' <param name="pmtId">Payment ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetPayment(ByVal pmtId As Integer) As HttpResponseMessage
        Try
            Dim payment As New Models.Payment
            Dim paymentCtrl As New PaymentsRepository

            payment = paymentCtrl.GetPayemnt(pmtId, PortalController.GetCurrentPortalSettings().PortalId)

            Return Request.CreateResponse(HttpStatusCode.OK, payment)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Add or updates payment
    ''' </summary>
    ''' <param name="agenda"></param>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function UpdatePayment(agenda As Boolean, dto As Models.Payment) As HttpResponseMessage
        Try
            Dim culture = New CultureInfo("pt-BR")
            Dim numInfo = culture.NumberFormat

            If dto.DueDate = Nothing Then
                dto.DueDate = CDate(IIf(Not dto.TransDate = Nothing, dto.TransDate, Today))
            End If

            If dto.TransDate = Nothing Then
                dto.TransDate = Today
            End If

            dto.DueDate = New Date(dto.DueDate.Year(), dto.DueDate.Month(), dto.DueDate.Day, Utilities.GetRandom(8, 16), 0, 0)

            Dim payment As New Models.Payment
            Dim paymentCtrl As New PaymentsRepository

            If dto.PaymentId > 0 Then
                payment = paymentCtrl.GetPayemnt(dto.PaymentId, dto.PortalId)
            End If

            payment.TransId = dto.TransId
            payment.Credit = dto.Credit
            payment.Debit = dto.Debit
            payment.InterestRate = dto.InterestRate
            payment.DocId = dto.DocId
            payment.PersonId = dto.PersonId
            payment.ProviderId = dto.ProviderId
            payment.AccountId = dto.AccountId
            payment.DueDate = dto.DueDate
            payment.Done = dto.Done
            payment.Comment = dto.Comment
            payment.TransDate = dto.TransDate
            payment.CreatedByUser = dto.CreatedByUser
            payment.CreatedOnDate = dto.CreatedOnDate
            payment.ModifiedByUser = dto.ModifiedByUser
            payment.ModifiedOnDate = dto.ModifiedOnDate

            If dto.PaymentId > 0 Then
                paymentCtrl.UpdatePayment(payment)

                If agenda Then
                    Dim appt As New Models.Agenda
                    Dim agendaCtrl As New AgendasRepository

                    appt.PortalId = dto.PortalId
                    appt.PersonId = dto.PersonId
                    appt.UserId = Null.NullInteger
                    appt.Subject = dto.Comment
                    appt.StartDateTime = dto.DueDate
                    appt.EndDateTime = dto.DueDate.AddMinutes(30)
                    appt.Reminder = "BEGIN:VALARM  TRIGGER:-PT2880M  X-TELERIK-UID:0440c18f-ce88-4934-ac5e-5d134420d4c5  END:VALARM"
                    appt.CreatedByUser = dto.CreatedByUser
                    appt.CreatedOnDate = dto.CreatedOnDate

                    agendaCtrl.AddAppointmentData(appt)
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Else

                paymentCtrl.AddPayment(payment)

                If agenda Then
                    Dim appt As New Models.Agenda
                    Dim agendaCtrl As New AgendasRepository

                    appt.PortalId = dto.PortalId
                    appt.PersonId = dto.PersonId
                    appt.UserId = Null.NullInteger
                    appt.Subject = dto.Comment
                    appt.StartDateTime = dto.DueDate
                    appt.EndDateTime = dto.DueDate.AddMinutes(30)
                    appt.Reminder = "BEGIN:VALARM  TRIGGER:-PT2880M  X-TELERIK-UID:0440c18f-ce88-4934-ac5e-5d134420d4c5  END:VALARM"
                    appt.CreatedByUser = dto.CreatedByUser
                    appt.CreatedOnDate = dto.CreatedOnDate

                    agendaCtrl.AddAppointmentData(appt)
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .PaymentId = payment.PaymentId})
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets a total balance
    ''' </summary>
    ''' <param name="portalId">POrtal ID</param>
    ''' <param name="account">Account ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetAccountsBalance(ByVal portalId As String, ByVal account As String) As HttpResponseMessage
        Try
            Dim accountBalance As New Models.Account
            Dim accountCtrl As New AccountsRepository

            accountBalance = accountCtrl.GetAccountsBalance(portalId, account)

            Return Request.CreateResponse(HttpStatusCode.OK, accountBalance)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

End Class
