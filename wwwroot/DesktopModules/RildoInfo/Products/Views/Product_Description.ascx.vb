
Imports RIW.Modules.Products.Components.Common
 
Namespace Views

    Partial Class ProductDescription
    Inherits RiwProductsModuleBase

        Private Const RiwModulePathScript As String = "mod_ModulePathScript"

        Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            ClientResourceManagement.ClientResourceManager.RegisterScript(Parent.Page, String.Format("DesktopModules/RildoInfo/{0}/viewmodels/productDescription.js", ModuleConfiguration.DesktopModule.FolderName), 99)
            RegisterModuleInfo()
        End Sub

        Public Sub RegisterModuleInfo()

            Dim intAuthorized As Integer = -1

            If UserInfo.IsInRole("Gerentes") Then
                intAuthorized = 3
            ElseIf UserInfo.IsInRole("Vendedores") Then
                intAuthorized = 1
            ElseIf UserInfo.IsInRole("Editores") Then
                intAuthorized = 0
            ElseIf UserInfo.IsInRole("Caixas") Then
                intAuthorized = 2
            End If

            'Dim modCtrl As New Entities.Modules.ModuleController()

            Dim scriptblock = New StringBuilder()
            scriptblock.Append("<script>")
            scriptblock.Append(String.Format("var portalID = ""{0}""; ", PortalId))
            scriptblock.Append(String.Format("var authorized = ""{0}""; ", intAuthorized))
            scriptblock.Append(String.Format("var userID = ""{0}""; ", UserInfo.UserID))
            scriptblock.Append(String.Format("var userName = ""{0}""; ", UserInfo.Username))
            scriptblock.Append(String.Format("var ascxVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(String.Format("{0}DesktopModules\RildoInfo\{1}\views\product_description.ascx", Request.PhysicalApplicationPath, ModuleConfiguration.DesktopModule.FolderName)).ToString("g")))
            scriptblock.Append(String.Format("var jsVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(String.Format("{0}DesktopModules\RildoInfo\{1}\viewmodels\productDescription.js", Request.PhysicalApplicationPath, ModuleConfiguration.DesktopModule.FolderName)).ToString("g")))
            scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(RiwModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), RiwModulePathScript, scriptblock.ToString())
            End If
        End Sub

    End Class

End Namespace