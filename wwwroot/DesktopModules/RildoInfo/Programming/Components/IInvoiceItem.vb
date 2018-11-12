
Namespace Models
    Public Interface IInvoiceItem

        Property InvoiceDetailId As Integer

        Property InvoiceId As Integer

        Property Qty As Decimal

        Property UnitValue1 As Single

        Property UnitValue2 As Single

        Property Discount As Decimal

        Property ProductId As Integer

        Property ProductName As String

        Property UnitTypeAbbv As String

        Property Barcode As String

        Property ProductRef As String

        Property CreatedByUser As Integer

        Property CreatedOnDate As DateTime

        Property ModifiedOnDate As DateTime

        Property ModifiedByUser As Integer

        Property UpdateProduct As Boolean
    End Interface
End Namespace