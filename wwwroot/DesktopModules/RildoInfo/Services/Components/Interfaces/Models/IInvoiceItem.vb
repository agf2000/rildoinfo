
Namespace Models
    Public Interface IInvoiceItem

        Property InvoiceDetailId As Integer

        Property InvoiceId As Integer

        Property Qty As Decimal

        Property UnitValue As Single

        Property Discount As Single

        Property ProdId As Integer
    End Interface
End Namespace