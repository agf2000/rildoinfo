
Namespace RI.Modules.RIStore_Services.Models
    Interface IStorePortalModuleSetting

#Region "Store / Portal / Module Setting Variables"

        Property id() As Integer

        Property name() As String

        Property value() As String

        Property tabModuleSettings() As List(Of StorePortalModuleSetting)

#End Region

    End Interface
End Namespace