
Namespace Components.Interfaces.Models

    Public Interface IAccount

        Property PortalId As Integer

        Property AccountId As Integer

        Property AccountName As String

        Property CreatedByUser As Integer

        Property CreatedOnDate As DateTime

        Property ModifiedByUser As Integer

        Property ModifiedOnDate As DateTime

        Property Locked As Boolean

        Property Balance As Single

        Property TotalBalance As Single

        Property TotalProductSales As Single

        Property TotalServiceSales As Single

        Property TotalProductsEstimates As Single

        Property TotalServicesEstimates As Single

        Property Debit As Single

        Property Debit4Seen As Single

        Property DebitActual As Single

        Property Credit As Single

        Property Credit4Seen As Single

        Property CreditActual As Single

        Property Sales4Seen As Single

        Property SalesActual As Single

        Property CreditBalance As Single

        Property DebitBalance As Single
    End Interface
End Namespace