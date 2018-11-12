Namespace Components.Interfaces.Models
    Public Interface ICashier

        Property CashierId As Integer

        Property PortalId As Integer

        Property TotalCash As Single

        Property TotalCheck As Single

        Property TotalCard As Single

        Property TotalDebit As Single

        Property TotalBank As Single

        Property CreatedByUser As Integer

        Property CreatedOnDate As DateTime

        Property ModifiedByUser As Integer

        Property ModifiedOnDate As DateTime

        Property EstimateId As Integer

        Property TotalRows As Integer
    End Interface
End Namespace