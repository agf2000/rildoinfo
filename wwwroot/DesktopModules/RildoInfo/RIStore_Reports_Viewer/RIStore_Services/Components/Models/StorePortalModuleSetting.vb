
Namespace RI.Modules.RIStore_Services.Models

    Public Class StorePortalModuleSetting
        Implements IStorePortalModuleSetting

#Region "Store / Portal / Module Settings Variables"

        Public Property id As Integer Implements IStorePortalModuleSetting.id

        Public Property name As String Implements IStorePortalModuleSetting.name

        Public Property tabModuleSettings As List(Of StorePortalModuleSetting) Implements IStorePortalModuleSetting.tabModuleSettings

        Public Property value As String Implements IStorePortalModuleSetting.value

#End Region

    End Class

End Namespace