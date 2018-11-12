
Namespace Models
    Public Interface IEstimateMessage

        Property EstimateMessageId As Integer

        Property EstimateId As Integer

        Property MessageText As String

        Property CreatedByUser As Integer

        Property CreatedOnDate As Date

        Property Allowed As Boolean

        Property ModifiedOnDate As DateTime

        Property ModifiedByUser As Integer

        Property Avatar As String

        Property DisplayName As String

        Property MessageComments As List(Of Models.EstimateMessageComment)

        Property ConnId As String
    End Interface
End Namespace