
Imports RIW.Modules.Common.Utilities
Imports RIW.Modules.Store_Manager.Components.Common
Imports DotNetNuke.Web.Client.ClientResourceManagement

Namespace Views

    Partial Class Finances
        Inherits StoreManagerModuleBase

        Private Const RiwModulePathScript As String = "mod_ModulePathScript"

        Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            ClientResourceManager.RegisterScript(Parent.Page, String.Format("DesktopModules/RildoInfo/{0}/viewmodels/finances.js", ModuleConfiguration.DesktopModule.FolderName), 99)
        End Sub

        Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                linkInvoices.NavigateUrl = NavigateURL(GetPageID("RIW Invoices Manager", PortalId).TabID)
                linkFluxo.NavigateUrl = NavigateURL(GetPageID("RIW Payments Manager", PortalId).TabID)
                linkCashiers.NavigateUrl = NavigateURL(GetPageID("RIW Cashiers Manager", PortalId).TabID)
            End If
            RegisterModuleInfo()
        End Sub

        Public Sub RegisterModuleInfo()

            'Dim modCtrl As New Entities.Modules.ModuleController()

            Dim scriptblock = New StringBuilder()
            scriptblock.Append("<script>")
            scriptblock.Append(String.Format("var ascxVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(String.Format("{0}DesktopModules\RildoInfo\{1}\views\Finances.ascx", Request.PhysicalApplicationPath, ModuleConfiguration.DesktopModule.FolderName)).ToString("g")))
            scriptblock.Append(String.Format("var jsVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(String.Format("{0}DesktopModules\RildoInfo\{1}\viewmodels\finances.js", Request.PhysicalApplicationPath, ModuleConfiguration.DesktopModule.FolderName)).ToString("g")))
            scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(RiwModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), RiwModulePathScript, scriptblock.ToString())
            End If
        End Sub

    End Class

End Namespace