
Namespace Components.Interfaces.Models
    Public Interface IInvoice

        Property PortalId As Integer

        Property InvoiceId As Integer

        Property InvoiceNumber As Integer

        Property InvoiceAmount As Single

        Property PayIn As Single

        Property Freight As Single

        Property PayQty As Integer

        Property Interval As Integer

        Property InterestRate As Double

        Property ClientId As Integer

        Property DueDate As Date

        Property EmissionDate As Date

        Property Comment As String

        Property CreatedOnDate As Date

        Property CreatedByUser As Integer

        Property ModifiedOnDate As Date

        Property ModifiedByUser As Integer

        Property AccountId As Integer

        Property ClientName As String

        Property Discount As Single

        Property DisplayName As String

        Property Done As Boolean

        Property EstimateId As Integer

        Property Locked As Boolean

        Property ProdName As String

        Property ProviderId As Integer

        Property Qty As Decimal

        Property TotalRows As Integer

        Property TotalValue As Single

        Property UnitTypeTitle As String

        Property UnitValue As Single

        Property VendorName As String

        Property InvoiceItems As IEnumerable(Of Components.Models.InvoiceItem)

        Property Payments As IEnumerable(Of Components.Models.Payment)

        Property CreditDebit As Boolean

        Property Purchase As Boolean
    End Interface
End Namespace