Namespace Components.Interfaces.Models
    Public Interface IPayment

        Property PortalId As Integer

        Property PaymentId As Integer

        Property TransId As Integer

        Property DocId As Integer

        Property Credit As Single

        Property Debit As Single

        Property InterestRate As Double

        Property ClientId As Integer

        Property ProviderId As Integer

        Property AccountId As Integer

        Property DueDate As Date

        Property Done As Boolean?

        Property Comment As String

        Property TransDate As Date

        Property CreatedByUser As Integer

        Property CreatedOnDate As Date

        Property ModifiedByUser As Integer?

        Property ModifiedOnDate As Date?

        Property ProviderName As String

        Property ClientName As String

        Property TotalRows As Integer

        Property Balance As Single

        Property Agenda As Boolean

        Property Total As Single

        Property Fee As Single

        Property OriginalDueDate As DateTime

        Property OriginId As Integer

        Property IsDeleted As Boolean?

        Property AmountPaid As Single
    End Interface
End Namespace