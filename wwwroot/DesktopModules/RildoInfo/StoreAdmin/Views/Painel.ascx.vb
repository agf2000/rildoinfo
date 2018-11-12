
Imports RIW.Modules.Common.Utilities
Imports RIW.Modules.StoreAdmin.Components.Common

Namespace Views

    Partial Class Painel
        Inherits RiwStoreAdminModuleBase

        Private Const RiwModulePathScript As String = "mod_ModulePathScript"

        Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            ClientResourceManager.RegisterScript(Parent.Page, String.Format("DesktopModules/RildoInfo/{0}/viewmodels/painel.js", ModuleConfiguration.DesktopModule.FolderName), 99)
        End Sub

        Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                linkAgenda.NavigateUrl = NavigateURL(GetPageID("RIW Agenda", PortalId).TabID)
                linkCategories.NavigateUrl = NavigateURL(GetPageID("RIW Categories Manager", PortalId).TabID)
                linkEntities.NavigateUrl = NavigateURL(GetPageID("RIW People Manager", PortalId).TabID)
                linkFinances.NavigateUrl = NavigateURL(GetPageID("RIW Finances", PortalId).TabID)
                linkProducts.NavigateUrl = NavigateURL(GetPageID("RIW Products Manager", PortalId).TabID)
                linkReports.NavigateUrl = NavigateURL(GetPageID("RIW Reports", PortalId).TabID)
                linkOR.NavigateUrl = NavigateURL(GetPageID("RIW Quick Estimate", PortalId).TabID)
                linkStore.NavigateUrl = NavigateURL(GetPageID("RIW Store Admin Store", PortalId).TabID)
                linkUsers.NavigateUrl = NavigateURL(GetPageID("RIW Users Manager", PortalId).TabID)
                linkDav.NavigateUrl = NavigateURL(GetPageID("RIW DAV Returns", PortalId).TabID)
            End If

            RegisterModuleInfo()
        End Sub

        Public Sub RegisterModuleInfo()

            'Dim modCtrl As New Entities.Modules.ModuleController()

            Dim scriptblock = New StringBuilder()
            scriptblock.Append("<script>")
            scriptblock.Append(String.Format("var ascxVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(String.Format("{0}DesktopModules\RildoInfo\{1}\views\painel.ascx", Request.PhysicalApplicationPath, ModuleConfiguration.DesktopModule.FolderName)).ToString("g")))
            scriptblock.Append(String.Format("var jsVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(String.Format("{0}DesktopModules\RildoInfo\{1}\viewmodels\painel.js", Request.PhysicalApplicationPath, ModuleConfiguration.DesktopModule.FolderName)).ToString("g")))
            scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(RiwModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), RiwModulePathScript, scriptblock.ToString())
            End If
        End Sub
    End Class

End Namespace