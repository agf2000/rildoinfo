
Imports RIW.Modules.Common.Utilities
Imports RIW.Modules.StoreAdmin.Components.Common

Namespace Views

    Partial Class Store
    Inherits RiwStoreAdminModuleBase

        Private Const RiwModulePathScript As String = "mod_ModulePathScript"

        Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            ClientResourceManager.RegisterScript(Parent.Page, String.Format("DesktopModules/RildoInfo/{0}/viewmodels/store.js", ModuleConfiguration.DesktopModule.FolderName), 99)
        End Sub

        Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                linkAddress.NavigateUrl = NavigateURL(GetPageID("RIW Store Admin Contacts", PortalId).TabID)
                linkEstimate.NavigateUrl = NavigateURL(GetPageID("RIW Store Admin Estimates", PortalId).TabID)
                linkPayConds.NavigateUrl = NavigateURL(GetPageID("RIW Store Admin Pay Forms", PortalId).TabID)
                linkRegistration.NavigateUrl = NavigateURL(GetPageID("RIW Store Admin Registration", PortalId).TabID)
                linkSMTP.NavigateUrl = NavigateURL(GetPageID("RIW Store Admin SMTP", PortalId).TabID)
                linkStatuses.NavigateUrl = NavigateURL(GetPageID("RIW Store Admin Statuses", PortalId).TabID)
                linkTemplates.NavigateUrl = NavigateURL(GetPageID("RIW Store Admin Templates", PortalId).TabID)
                linkWebsite.NavigateUrl = NavigateURL(GetPageID("RIW Store Admin Main Portal", PortalId).TabID)
            End If
            RegisterModuleInfo()
        End Sub

        Public Sub RegisterModuleInfo()

            'Dim modCtrl As New Entities.Modules.ModuleController()

            Dim scriptblock = New StringBuilder()
            scriptblock.Append("<script>")
            scriptblock.Append(String.Format("var ascxVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(String.Format("{0}DesktopModules\RildoInfo\{1}\views\store.ascx", Request.PhysicalApplicationPath, ModuleConfiguration.DesktopModule.FolderName)).ToString("g")))
            scriptblock.Append(String.Format("var jsVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(String.Format("{0}DesktopModules\RildoInfo\{1}\viewmodels\store.js", Request.PhysicalApplicationPath, ModuleConfiguration.DesktopModule.FolderName)).ToString("g")))
            scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(RiwModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), RiwModulePathScript, scriptblock.ToString())
            End If
        End Sub

    End Class

End Namespace