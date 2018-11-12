
Namespace Components.Interfaces.Models

    Public Interface ISetting

        Property PortalId As Integer

        Property AppSettingId As Integer

        Property SettingName As String

        Property SettingValue As String

        Property CreatedByUser As Integer

        Property CreatedOnDate As DateTime

        Property ModifiedByUser As Integer

        Property ModifiedOnDate As DateTime

        Property CultureCode As String
    End Interface
End Namespace