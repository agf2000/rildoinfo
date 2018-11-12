
Namespace Models

    Public Interface IDoc

        Property PortalId As Integer

        Property ModuleId As Integer

        Property DocId As Integer

        Property FileId As Integer

        Property DocName As String

        Property DocDesc As String

        Property Downloads As Integer

        Property Requests As Integer

        Property CreatedByUser As Integer

        Property CreatedOnDate As Date

        Property Extension As String

        Property FileName As String

        Property ContentType As String
    End Interface
End Namespace