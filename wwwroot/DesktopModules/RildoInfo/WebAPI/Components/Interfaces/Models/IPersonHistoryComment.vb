﻿Namespace Components.Interfaces.Models
    Public Interface IPersonHistoryComment

        Property PersonHistoryId As Integer

        Property CommentId As Integer

        Property CommentText As String

        Property CreatedByUser As Integer

        Property CreatedOnDate As Date

        Property Avatar As String

        Property DisplayName As String

    End Interface
End Namespace