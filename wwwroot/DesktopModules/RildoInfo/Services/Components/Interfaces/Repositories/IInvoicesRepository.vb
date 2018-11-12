
Public Interface IInvoicesRepository

    ''' <summary>
    ''' Add an invoice
    ''' </summary>
    ''' <param name="invoice"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddInvoice(invoice As Models.Invoice) As Models.Invoice

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
    Function GetInvoices(ByVal portalId As String, ByVal providerId As String, ByVal personId As String, ByVal prodId As String, ByVal sDate As DateTime, ByVal eDate As DateTime, ByVal pageNumber As Integer, ByVal pageSize As Integer, orderBy As String) As IEnumerable(Of Models.Invoice)

    ''' <summary>
    ''' Gets invoice
    ''' </summary>
    ''' <param name="invoiceId">Invoice ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetInvoice(ByVal invoiceId As Integer) As Models.Invoice

    ''' <summary>
    ''' Updates an invoice
    ''' </summary>
    ''' <param name="invoice">Invoice Model</param>
    ''' <remarks></remarks>
    Sub UpdateInvoice(invoice As Models.Invoice)

    ''' <summary>
    ''' Removes an invoice by invoice id
    ''' </summary>
    ''' <param name="invoiceId">Invoice ID</param>
    ''' <remarks></remarks>
    Sub RemoveInvoice(ByVal invoiceId As Integer)

    ''' <summary>
    ''' Removes an invoice
    ''' </summary>
    ''' <param name="invoice">Invoice Model</param>
    ''' <remarks></remarks>
    Sub RemoveInvoice(invoice As Models.Invoice)

    ''' <summary>
    ''' Gets invoice items by invoice id
    ''' </summary>
    ''' <param name="invoiceId">Invoice ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetInvoiceItems(ByVal invoiceId As Integer) As IEnumerable(Of Models.InvoiceItem)

    ''' <summary>
    ''' Gets product info by product id
    ''' </summary>
    ''' <param name="invoiceItemId">Invoice Item ID</param>
    ''' <param name="invoiceId">Invoice ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetInvoiceItem(ByVal invoiceItemId As Integer, invoiceId As Integer) As Models.InvoiceItem

    ''' <summary>
    ''' Removes an invoice by invoice id
    ''' </summary>
    ''' <param name="invoiceItemId">Invoice Item ID</param>
    ''' <param name="invoiceId">Invoice ID</param>
    ''' <remarks></remarks>
    Sub RemoveInvoiceItem(ByVal invoiceItemId As Integer, invoiceId As Integer)

    ''' <summary>
    ''' Removes an invoice
    ''' </summary>
    ''' <param name="invoiceItem">InvoiceItem Model</param>
    ''' <remarks></remarks>
    Sub RemoveInvoiceItem(invoiceItem As Models.InvoiceItem)

    ''' <summary>
    ''' Removes an invoice by invoice id
    ''' </summary>
    ''' <param name="invoiceId">Invoice ID</param>
    ''' <remarks></remarks>
    Sub RemoveInvoiceItems(ByVal invoiceId As Integer)

    ''' <summary>
    ''' Updates an invoice item
    ''' </summary>
    ''' <param name="invoiceItem">Invoice Item Model</param>
    ''' <remarks></remarks>
    Sub UpdateInvoiceItem(invoiceItem As Models.InvoiceItem)

    ''' <summary>
    ''' Add an invoice item
    ''' </summary>
    ''' <param name="invoiceItem">Invoice Item Model</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddInvoiceItem(invoiceItem As Models.InvoiceItem) As Models.InvoiceItem

End Interface
