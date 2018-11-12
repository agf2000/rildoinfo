
Namespace Models
    Public Interface IEstimateHistory

        Property EstimateHistoryId As Integer

        Property EstimateId As Integer

        Property HistoryText As String

        Property CreatedByUser As Integer

        Property CreatedOnDate As Date

        Property Locked As Boolean

        Property Avatar As String

        Property DisplayName As String

        Property HistoryComments As IEnumerable(Of EstimateHistoryComment)

        Property ConnId As String
    End Interface
End Namespace