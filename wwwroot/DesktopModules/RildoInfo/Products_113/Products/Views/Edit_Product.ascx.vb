
Imports RIW.Modules.Products.Components.Common

Namespace Views

    Partial Class EditProduct
    Inherits RiwProductsModuleBase

        Private Const RiwModulePathScript As String = "mod_ModulePathScript"

        Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            ClientResourceManagement.ClientResourceManager.RegisterScript(Parent.Page, "DesktopModules/RildoInfo/" & Me.ModuleConfiguration.DesktopModule.FolderName & "/viewmodels/editProduct.js", 99)
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
            scriptblock.Append(String.Format("var ascxVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(Request.PhysicalApplicationPath & "DesktopModules\RildoInfo\" & Me.ModuleConfiguration.DesktopModule.FolderName & "\views\edit_product.ascx").ToString("g")))
            scriptblock.Append(String.Format("var jsVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(Request.PhysicalApplicationPath & "DesktopModules\RildoInfo\" & Me.ModuleConfiguration.DesktopModule.FolderName & "\viewmodels\editProduct.js").ToString("g")))
            scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(RiwModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), RiwModulePathScript, scriptblock.ToString())
            End If
        End Sub

    End Class

End Namespace