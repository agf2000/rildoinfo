
Namespace Models
    Public Interface IPersonHistory

        Property PersonHistoryId As Integer

        Property PersonId As Integer

        Property HistoryText As String

        Property CreatedByUser As Integer

        Property CreatedOnDate As Date

        Property Locked As Boolean

        Property Avatar As String

        Property DisplayName As String

        Property HistoryComments As IEnumerable(Of PersonHistoryComment)

        Property ConnId As String
    End Interface
End Namespace