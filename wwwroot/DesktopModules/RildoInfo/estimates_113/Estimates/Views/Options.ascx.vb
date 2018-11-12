Imports RIW.Modules.Estimates.Components.Common
 
Namespace Views

    Public Class Options
    Inherits RiwEstimatesModuleBase

        Private Const RiwModulePathScript As String = "mod_ModulePathScript"

        Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            ClientResourceManagement.ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/Estimates/viewmodels/options.js", 99)
            RegisterModuleInfo()
        End Sub

        Public Sub RegisterModuleInfo()

            'Dim modCtrl As New Entities.Modules.ModuleController()

            Dim scriptblock = New StringBuilder()
            scriptblock.Append("<script>")
            scriptblock.Append([String].Format("var ascxVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(Request.PhysicalApplicationPath & "DesktopModules\RildoInfo\" & Me.ModuleConfiguration.DesktopModule.FolderName & "\views\options.ascx").ToString("g")))
            scriptblock.Append([String].Format("var jsVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(Request.PhysicalApplicationPath & "DesktopModules\RildoInfo\" & Me.ModuleConfiguration.DesktopModule.FolderName & "\viewmodels\options.js").ToString("g")))
            scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(RiwModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), RiwModulePathScript, scriptblock.ToString())
            End If
        End Sub
    
    End Class

End Namespace