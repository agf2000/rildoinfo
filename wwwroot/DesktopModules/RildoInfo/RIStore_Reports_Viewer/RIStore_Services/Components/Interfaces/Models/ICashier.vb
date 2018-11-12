
Namespace RI.Modules.RIStore_Services.Models
    Public Interface ICashier

        Property PortalId() As Integer

        Property EstimateId() As Integer

        Property TotalCash() As Single

        Property TotalCheck() As Single

        Property TotalCard() As Single

        Property TotalBank() As Single

        Property CreatedByUser() As Integer

        Property CreatedOnDate() As DateTime

    End Interface
End Namespace