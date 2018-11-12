
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation
Imports System.Globalization
Imports System.Drawing

Namespace RI.Modules.RIStore_Services
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
        ''' <param name="clientId">Client ID</param>
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
        Function GetInvoices(ByVal portalId As String, ByVal providerId As String, ByVal clientId As String, ByVal prodId As String, ByVal pageNumber As Integer, ByVal pageSize As Integer, orderBy As String, Optional sDate As String = Nothing, Optional eDate As String = Nothing) As HttpResponseMessage
            Try

                If Not sDate = Nothing Then
                    sDate = CStr(Null.NullDate)
                End If

                If Not eDate = Nothing Then
                    eDate = CStr(Null.NullDate)
                End If

                Dim invoices As New List(Of Models.Invoice)
                Dim invoicesCtrl As New InvoicesRepository

                invoices = invoicesCtrl.GetInvoices(portalId, providerId, clientId, prodId, CDate(sDate), CDate(eDate), pageNumber, pageSize, orderBy)

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
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="invoiceId">Invoice ID</param>
        ''' <param name="iNumber">InvoiceNumber</param>
        ''' <param name="iAmount">Invoice Amount</param>
        ''' <param name="payIn">Intial Pay</param>
        ''' <param name="pform">Pay Form</param>
        ''' <param name="providerId">Provider ID</param>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="estimateId">Estimate ID</param>
        ''' <param name="dueDate">Due Date</param>
        ''' <param name="eDate">Emission Date</param>
        ''' <param name="uId">Modified By User</param>
        ''' <param name="cd">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function UpdateInvoice(ByVal portalId As Integer,
        ByVal invoiceId As Integer,
        ByVal iNumber As Integer,
        ByVal iAmount As String,
        ByVal payIn As String,
        ByVal pForm As String,
        ByVal providerId As Integer,
        ByVal clientId As Integer,
        ByVal estimateId As Integer,
        ByVal dueDate As Date,
        ByVal eDate As Date,
        ByVal payQty As Integer,
        ByVal interval As Integer,
        ByVal interestRate As String,
        ByVal comment As String,
        ByVal uId As Integer,
        ByVal cd As Date) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat
                Dim intCount = 1
                Dim daysInterval = CInt(IIf(interval = 0, 30, interval))
                Dim invoiceAmount = Decimal.Parse(iAmount.Replace(".", ","), numInfo)
                dueDate = New Date(dueDate.Year(), dueDate.Month(), dueDate.Day, Utilities.GetRandom(8, 16), 0, 0)

                Dim invoice As New Models.Invoice
                Dim invoiceCtrl As New InvoicesRepository

                If invoiceId > 0 Then

                    invoice = invoiceCtrl.GetInvoice(invoiceId)

                End If

                invoice.PortalId = portalId
                invoice.InvoiceNumber = iNumber
                invoice.InvoiceAmount = invoiceAmount
                invoice.PayIn = payIn
                invoice.PayQty = payQty
                invoice.Interval = interval
                invoice.InterestRate = interestRate
                invoice.ProviderId = providerId
                invoice.ClientId = clientId
                invoice.EstimateId = estimateId
                invoice.DueDate = dueDate
                invoice.EmissionDate = eDate
                invoice.Comment = comment
                invoice.CreatedByUser = uId
                invoice.CreatedOnDate = cd
                invoice.ModifiedByUser = uId
                invoice.ModifiedOnDate = cd

                Dim payment As New Models.Payment
                Dim paymentCtrl As New PaymentsRepository

                payment.PortalId = portalId
                payment.DocId = invoiceId
                payment.Credit = 0
                payment.Debit = Decimal.Parse(payIn.Replace(".", ","), numInfo)
                payment.InterestRate = 0
                payment.ClientId = clientId
                payment.ProviderId = providerId
                payment.AccountId = 1
                payment.DueDate = dueDate
                payment.Done = False
                payment.Comment = comment.Replace("[ABC]", GetNextLetter())
                payment.ModifiedByUser = uId
                payment.ModifiedOnDate = cd

                Dim appt As New Models.Agenda
                Dim apptCtrl As New AgendasRepository

                appt.PortalId = portalId
                appt.ClientId = clientId
                appt.Subject = comment
                appt.StartDateTime = dueDate
                appt.EndDateTime = dueDate.AddHours(1)
                appt.Reminder = "BEGIN:VALARM  TRIGGER:-PT2880M  X-TELERIK-UID:0440c18f-ce88-4934-ac5e-5d134420d4c5  END:VALARM"
                appt.ModifiedByUser = uId
                appt.ModifiedOnDate = cd
                appt.CreatedByUser = uId
                appt.CreatedOnDate = cd

                If invoiceId > 0 Then

                    invoiceCtrl.UpdateInvoice(invoice)

                    If payQty > 0 Then

                        If Decimal.Parse(payIn.Replace(".", ","), numInfo) > 0 Then

                            invoiceAmount = invoiceAmount - Decimal.Parse(payIn.Replace(".", ","), numInfo)

                            payment.Debit = invoiceAmount

                            paymentCtrl.AddPayment(payment)

                            apptCtrl.AddAppointmentData(appt)

                        End If

                        invoiceAmount = invoiceAmount / payQty

                        payment.Debit = invoiceAmount

                        paymentCtrl.AddPayment(payment)

                        apptCtrl.AddAppointmentData(appt)

                        intCount = intCount + 1

                        Do While intCount <= payQty

                            dueDate = dueDate.AddDays(daysInterval)

                            payment.DueDate = dueDate

                            paymentCtrl.AddPayment(payment)

                            appt.StartDateTime = dueDate

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

                    If payQty > 0 Then

                        If Decimal.Parse(payIn.Replace(".", ","), numInfo) > 0 Then

                            invoiceAmount = invoiceAmount - Decimal.Parse(payIn.Replace(".", ","), numInfo)

                            payment.Debit = invoiceAmount

                            paymentCtrl.AddPayment(payment)

                            apptCtrl.AddAppointmentData(appt)

                        End If

                        invoiceAmount = invoiceAmount / payQty

                        payment.Debit = invoiceAmount

                        paymentCtrl.AddPayment(payment)

                        apptCtrl.AddAppointmentData(appt)

                        intCount = intCount + 1

                        Do While intCount <= payQty

                            dueDate = dueDate.AddDays(daysInterval)

                            payment.DueDate = dueDate

                            paymentCtrl.AddPayment(payment)

                            appt.StartDateTime = dueDate

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

                invoiceCtrl.removeInvoice(invoiceId)

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
                Dim invoiceItems As New List(Of Models.InvoiceItem)
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

                Dim payments As List(Of Models.Payment)
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
        ''' Updates a payment
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="pmtId">Paymment ID</param>
        ''' <param name="transId">Invoice Amount</param>
        ''' <param name="docId">Document Id</param>
        ''' <param name="transDate">Transaction Date</param>
        ''' <param name="done">Done</param>
        ''' <param name="accId">AccountId</param>
        ''' <param name="providerId">Provider ID</param>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="comment">Comment</param>
        ''' <param name="dueDate">Due Date</param>
        ''' <param name="credit">Credit</param>
        ''' <param name="debit">Debit</param>
        ''' <param name="uId">Modified By User</param>
        ''' <param name="cd">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function UpdatePayment(ByVal portalId As Integer,
        ByVal done As Boolean,
        ByVal accId As Integer,
        ByVal credit As String,
        ByVal debit As String,
        ByVal interestRate As String,
        ByVal uId As Integer,
        ByVal cd As Date,
        ByVal comment As String,
        ByVal pmtId As Integer,
        Optional ByVal dueDate As Date = Nothing,
        Optional ByVal transDate As Date = Nothing,
        Optional ByVal transId As Integer = Nothing,
        Optional ByVal docId As Integer = Nothing,
        Optional ByVal providerId As Integer = Nothing,
        Optional ByVal clientId As Integer = Nothing,
        Optional ByVal agenda As Boolean = False) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                If dueDate = Nothing Then
                    dueDate = CDate(IIf(Not transDate = Nothing, transDate, Today))
                End If

                If transDate = Nothing Then
                    transDate = Today
                End If

                dueDate = New Date(dueDate.Year(), dueDate.Month(), dueDate.Day, Utilities.GetRandom(8, 16), 0, 0)

                Dim payment As New Models.Payment
                Dim paymentCtrl As New PaymentsRepository

                If pmtId > 0 Then
                    payment = paymentCtrl.GetPayemnt(pmtId, portalId)
                End If

                payment.TransId = transId
                payment.Credit = Decimal.Parse(credit.Replace(".", ","), numInfo)
                payment.Debit = Decimal.Parse(debit.Replace(".", ","), numInfo)
                payment.InterestRate = Decimal.Parse(interestRate.Replace(".", ","), numInfo)
                payment.DocId = docId
                payment.ClientId = clientId
                payment.ProviderId = providerId
                payment.AccountId = accId
                payment.DueDate = dueDate
                payment.Done = done
                payment.Comment = comment
                payment.TransDate = transDate
                payment.CreatedByUser = uId
                payment.CreatedOnDate = cd
                payment.ModifiedByUser = uId
                payment.ModifiedOnDate = cd

                If pmtId > 0 Then
                    paymentCtrl.UpdatePayment(payment)

                    If agenda Then
                        Dim appt As New Models.Agenda
                        Dim agendaCtrl As New AgendasRepository

                        appt.PortalId = portalId
                        appt.ClientId = clientId
                        appt.UserId = Null.NullInteger
                        appt.Subject = comment
                        appt.StartDateTime = dueDate
                        appt.EndDateTime = dueDate.AddMinutes(30)
                        appt.Reminder = "BEGIN:VALARM  TRIGGER:-PT2880M  X-TELERIK-UID:0440c18f-ce88-4934-ac5e-5d134420d4c5  END:VALARM"
                        appt.CreatedByUser = uId
                        appt.CreatedOnDate = cd

                        agendaCtrl.AddAppointmentData(appt)
                    End If

                    Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
                Else

                    paymentCtrl.AddPayment(payment)

                    If agenda Then
                        Dim appt As New Models.Agenda
                        Dim agendaCtrl As New AgendasRepository

                        appt.PortalId = portalId
                        appt.ClientId = clientId
                        appt.UserId = Null.NullInteger
                        appt.Subject = comment
                        appt.StartDateTime = dueDate
                        appt.EndDateTime = dueDate.AddMinutes(30)
                        appt.Reminder = "BEGIN:VALARM  TRIGGER:-PT2880M  X-TELERIK-UID:0440c18f-ce88-4934-ac5e-5d134420d4c5  END:VALARM"
                        appt.CreatedByUser = uId
                        appt.CreatedOnDate = cd

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
End Namespace