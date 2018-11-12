Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IInvoicesRepository

        ''' <summary>
        ''' Add an invoice
        ''' </summary>
        ''' <param name="invoice"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddInvoice(invoice As Invoice) As Invoice

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
        Function GetInvoices(portalId As Integer, providerId As Integer, clientId As Integer, productId As Integer, sDate As DateTime, eDate As DateTime, pageNumber As Integer, pageSize As Integer, orderBy As String) As IEnumerable(Of Invoice)

        ''' <summary>
        ''' Gets invoice
        ''' </summary>
        ''' <param name="invoiceId">Invoice ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetInvoice(invoiceId As Integer) As Invoice

        ''' <summary>
        ''' Updates an invoice
        ''' </summary>
        ''' <param name="invoice">Invoice Model</param>
        ''' <remarks></remarks>
        Sub UpdateInvoice(invoice As Invoice)

        ''' <summary>
        ''' Removes an invoice by invoice id
        ''' </summary>
        ''' <param name="invoiceId">Invoice ID</param>
        ''' <remarks></remarks>
        Sub RemoveInvoice(invoiceId As Integer)

        ''' <summary>
        ''' Removes an invoice
        ''' </summary>
        ''' <param name="invoice">Invoice Model</param>
        ''' <remarks></remarks>
        Sub RemoveInvoice(invoice As Invoice)

        ''' <summary>
        ''' Gets invoice items by invoice id
        ''' </summary>
        ''' <param name="invoiceId">Invoice ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetInvoiceItems(invoiceId As Integer) As IEnumerable(Of InvoiceItem)

        ''' <summary>
        ''' Gets product info by product id
        ''' </summary>
        ''' <param name="invoiceItemId">Invoice Item ID</param>
        ''' <param name="invoiceId">Invoice ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetInvoiceItem(invoiceItemId As Integer, invoiceId As Integer) As InvoiceItem

        ''' <summary>
        ''' Removes an invoice by invoice id
        ''' </summary>
        ''' <param name="invoiceItemId">Invoice Item ID</param>
        ''' <param name="invoiceId">Invoice ID</param>
        ''' <remarks></remarks>
        Sub RemoveInvoiceItem1(invoiceItemId As Integer, invoiceId As Integer)

        ''' <summary>
        ''' Removes an invoice
        ''' </summary>
        ''' <param name="invoiceItem">InvoiceItem Model</param>
        ''' <remarks></remarks>
        Sub RemoveInvoiceItem(invoiceItem As InvoiceItem)

        ''' <summary>
        ''' Removes an invoice by invoice id
        ''' </summary>
        ''' <param name="invoiceId">Invoice ID</param>
        ''' <remarks></remarks>
        Sub RemoveInvoiceItems(invoiceId As Integer)

        ''' <summary>
        ''' Updates an invoice item
        ''' </summary>
        ''' <param name="invoiceItem">Invoice Item Model</param>
        ''' <remarks></remarks>
        Sub UpdateInvoiceItem(invoiceItem As InvoiceItem)

        ''' <summary>
        ''' Add an invoice item
        ''' </summary>
        ''' <param name="invoiceItem">Invoice Item Model</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddInvoiceItem(invoiceItem As InvoiceItem) As InvoiceItem

    End Interface

End Namespace
