Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_InvoiceItems")> _
    <PrimaryKey("InvoiceItemId", AutoIncrement:=True)> _
    <Cacheable("InvoiceItems", CacheItemPriority.Default, 20)> _
    <Scope("InvoiceId")>
    Public Class InvoiceItem
        Implements IInvoiceItem

        Public Property Discount As Decimal Implements IInvoiceItem.Discount

        Public Property InvoiceItemId As Integer Implements IInvoiceItem.InvoiceDetailId

        Public Property InvoiceId As Integer Implements IInvoiceItem.InvoiceId

        Public Property Qty As Decimal Implements IInvoiceItem.Qty

        Public Property UnitValue1 As Single Implements IInvoiceItem.UnitValue1

        Public Property UnitValue2 As Single Implements IInvoiceItem.UnitValue2

        Public Property ProductId As Integer Implements IInvoiceItem.ProductId

        Public Property CreatedByUser As Integer Implements IInvoiceItem.CreatedByUser

        Public Property CreatedOnDate As DateTime Implements IInvoiceItem.CreatedOnDate

        Public Property ModifiedByUser As Integer Implements IInvoiceItem.ModifiedByUser

        Public Property ModifiedOnDate As DateTime Implements IInvoiceItem.ModifiedOnDate

        <IgnoreColumn> _
        Public Property ProductName As String Implements IInvoiceItem.ProductName

        <IgnoreColumn> _
        Public Property UnitTypeAbbv As String Implements IInvoiceItem.UnitTypeAbbv

        <IgnoreColumn> _
        Public Property Barcode As String Implements IInvoiceItem.Barcode

        <IgnoreColumn> _
        Public Property ProductRef As String Implements IInvoiceItem.ProductRef

        Public Property UpdateProduct As Boolean Implements IInvoiceItem.UpdateProduct

        <IgnoreColumn>
        Public Property Purchase As Boolean Implements IInvoiceItem.Purchase
    End Class
End Namespace