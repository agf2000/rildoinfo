Namespace Components.Interfaces.Models
    Public Interface IEstimateItemRemoved

        Property EstimateId As Integer

        Property RemoveReasonId As Char

        Property ProductId As Integer

        Property ProductQty As Double

        Property CreatedByUser As Integer

        Property CreatedOnDate As Date

        Property EstimateItemId As Integer

        Property ProductName As String
    End Interface
End Namespace