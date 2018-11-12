
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IInvoice

        Property PortalId() As Integer

        Property AccountId() As Integer

        Property InvoiceId() As Integer

        Property InvoiceNumber() As Integer

        Property InvoiceItemId() As Integer

        Property ProviderId() As Integer

        Property ClientId() As Integer

        Property EstimateId() As Integer

        Property DisplayName() As String

        Property ClientName() As String

        Property VendorName() As String

        Property EmissionDate() As Date

        Property ProdId() As Integer

        Property ProdName() As String

        Property UnitTypeTitle() As String

        Property Qty() As Decimal

        Property UnitValue() As Single

        Property Discount() As Single

        Property TotalValue() As Single

        Property InvoiceAmount() As Single

        Property PayIn() As Single

        Property PayQty() As Integer

        Property Interval() As Integer

        Property InterestRate() As Single

        Property DueDate() As Date

        Property TotalRows() As Integer

        Property Done() As Boolean

        Property Comment() As String

        Property Locked() As Boolean

        Property CreatedByUser() As Integer

        Property CreatedDate() As Date

        Property ModifiedByUser() As Integer

        Property ModifiedOnDate() As Date

    End Interface
End Namespace