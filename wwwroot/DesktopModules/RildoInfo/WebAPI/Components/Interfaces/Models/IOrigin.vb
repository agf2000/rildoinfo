Namespace Components.Interfaces.Models
    Public Interface IOrigin

        Property PortalId As Integer

        Property OriginId As Integer

        Property OriginName As String

        Property CreatedByUser As Integer

        Property CreatedOnDate As DateTime

        Property ModifiedByUser As Integer

        Property ModifiedOnDate As DateTime

        Property Locked As Boolean
    End Interface
End Namespace