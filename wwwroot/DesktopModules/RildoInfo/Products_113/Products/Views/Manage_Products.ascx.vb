Imports RIW.Modules.Products.Components.Common
 
Namespace Views

    Public Class ManageProducts
    Inherits RiwProductsModuleBase

        Private Const RiwModulePathScript As String = "mod_ModulePathScript"

        Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            ClientResourceManagement.ClientResourceManager.RegisterScript(Parent.Page, String.Format("DesktopModules/RildoInfo/{0}/viewmodels/manageProducts.js", Me.ModuleConfiguration.DesktopModule.FolderName), 99)
            RegisterModuleInfo()
        End Sub

        Public Sub RegisterModuleInfo()

            'Dim modCtrl As New Entities.Modules.ModuleController()

            Dim scriptblock = New StringBuilder()
            scriptblock.Append("<script>")
            scriptblock.Append(String.Format("var ascxVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(String.Format("{0}DesktopModules\RildoInfo\{1}\views\Manage_Products.ascx", Request.PhysicalApplicationPath, Me.ModuleConfiguration.DesktopModule.FolderName)).ToString("g")))
            scriptblock.Append(String.Format("var jsVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(String.Format("{0}DesktopModules\RildoInfo\{1}\viewmodels\manageProducts.js", Request.PhysicalApplicationPath, Me.ModuleConfiguration.DesktopModule.FolderName)).ToString("g")))
            scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(RiwModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), RiwModulePathScript, scriptblock.ToString())
            End If
        End Sub
    
    End Class

End Namespace