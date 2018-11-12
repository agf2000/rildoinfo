
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_InvoiceItems")> _
    <PrimaryKey("InvoiceItemId", AutoIncrement:=True)> _
    <Cacheable("InvoiceItems", CacheItemPriority.Default, 20)> _
    <Scope("InvoiceId")>
    Public Class InvoiceItem
        Implements IInvoiceItem

        Public Property Discount As Single Implements IInvoiceItem.Discount

        Public Property InvoiceItemId As Integer Implements IInvoiceItem.InvoiceDetailId

        Public Property InvoiceId As Integer Implements IInvoiceItem.InvoiceId

        Public Property Qty As Decimal Implements IInvoiceItem.Qty

        Public Property UnitValue As Single Implements IInvoiceItem.UnitValue

        Public Property ProdId As Integer Implements IInvoiceItem.ProdId

    End Class
End Namespace