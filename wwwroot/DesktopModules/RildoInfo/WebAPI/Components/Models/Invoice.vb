
Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_Invoices")> _
    <PrimaryKey("InvoiceId", AutoIncrement:=True)> _
    <Cacheable("Invoices", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class Invoice
        Implements IInvoice

        <IgnoreColumn> _
        Public Property AccountId As Integer Implements IInvoice.AccountId

        <IgnoreColumn> _
        Public Property ClientName As String Implements IInvoice.ClientName

        <IgnoreColumn> _
        Public Property Discount As Single Implements IInvoice.Discount

        <IgnoreColumn> _
        Public Property DisplayName As String Implements IInvoice.DisplayName

        <IgnoreColumn> _
        Public Property Done As Boolean Implements IInvoice.Done

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IInvoice.Locked

        <IgnoreColumn> _
        Public Property ProdName As String Implements IInvoice.ProdName

        <IgnoreColumn> _
        Public Property Qty As Decimal Implements IInvoice.Qty

        <IgnoreColumn> _
        Public Property TotalRows As Integer Implements IInvoice.TotalRows

        <IgnoreColumn> _
        Public Property TotalValue As Single Implements IInvoice.TotalValue

        <IgnoreColumn> _
        Public Property UnitTypeTitle As String Implements IInvoice.UnitTypeTitle

        <IgnoreColumn> _
        Public Property UnitValue As Single Implements IInvoice.UnitValue

        <IgnoreColumn> _
        Public Property VendorName As String Implements IInvoice.VendorName

        Public Property ClientId As Integer Implements IInvoice.ClientId

        Public Property Comment As String Implements IInvoice.Comment

        Public Property CreatedByUser As Integer Implements IInvoice.CreatedByUser

        Public Property CreatedOnDate As Date Implements IInvoice.CreatedOnDate

        Public Property DueDate As Date Implements IInvoice.DueDate

        Public Property EmissionDate As Date Implements IInvoice.EmissionDate

        Public Property EstimateId As Integer Implements IInvoice.EstimateId

        Public Property Freight As Single Implements IInvoice.Freight

        Public Property InterestRate As Double Implements IInvoice.InterestRate

        Public Property Interval As Integer Implements IInvoice.Interval

        Public Property InvoiceAmount As Single Implements IInvoice.InvoiceAmount

        Public Property InvoiceId As Integer Implements IInvoice.InvoiceId

        Public Property InvoiceNumber As Integer Implements IInvoice.InvoiceNumber

        Public Property ModifiedByUser As Integer Implements IInvoice.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IInvoice.ModifiedOnDate

        Public Property PayIn As Single Implements IInvoice.PayIn

        Public Property PayQty As Integer Implements IInvoice.PayQty

        Public Property PortalId As Integer Implements IInvoice.PortalId

        Public Property ProviderId As Integer Implements IInvoice.ProviderId

        <IgnoreColumn> _
        Public Property InvoiceItems As IEnumerable(Of InvoiceItem) Implements IInvoice.InvoiceItems

        <IgnoreColumn>
        Public Property Payments As IEnumerable(Of Payment) Implements IInvoice.Payments

        Public Property CreditDebit As Boolean Implements IInvoice.CreditDebit

        Public Property Purchase As Boolean Implements IInvoice.Purchase
    End Class
End Namespace