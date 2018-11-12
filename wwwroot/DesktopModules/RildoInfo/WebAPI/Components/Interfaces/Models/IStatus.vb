Namespace Components.Interfaces.Models
    Public Interface IStatus

        Property PortalId As Integer

        Property StatusId As Integer

        Property StatusTitle As String

        Property StatusColor As String

        Property IsReadOnly As Boolean

        Property IsDeleted As Boolean

        Property CreatedByUser As Integer

        Property CreatedOnDate As Date

        Property ModifiedByUser As Integer

        Property ModifiedOnDate As Date
    End Interface
End Namespace