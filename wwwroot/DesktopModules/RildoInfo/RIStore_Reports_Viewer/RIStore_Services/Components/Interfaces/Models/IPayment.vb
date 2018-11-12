
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IPayment

        Property PortalId() As Integer

        Property PaymentId() As Integer

        Property TransId() As Integer

        Property DocId() As Integer

        Property Credit() As Single

        Property Debit() As Single

        Property InterestRate() As Decimal

        Property ClientId() As Integer

        Property ProviderId() As Integer

        Property ClientName() As String

        Property VendorName() As String

        Property AccountId() As Integer

        Property AccountName() As String

        Property DueDate() As Date

        Property Done() As Boolean

        Property Comment() As String

        Property TransDate() As Date

        Property TotalRows() As Integer

        Property Balance() As Single

        Property CreatedByUser() As Integer

        Property CreatedDate() As Date

        Property ModifiedByUser() As Integer

        Property ModifiedOnDate() As Date

    End Interface
End Namespace