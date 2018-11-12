'Imports DotNetNuke.Services.Exceptions
'Imports DotNetNuke.Entities.Users
Imports RIW.Modules.Reports.Components.Common

Namespace Views

    Public Class Edit
        Inherits ReportsModuleBase

        Private Const RiwModulePathScript As String = "mod_ModulePathScript"

#Region "Events"

        Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            ClientResourceManagement.ClientResourceManager.RegisterScript(Parent.Page, String.Format("DesktopModules/RildoInfo/{0}/viewmodels/edit.js", Me.ModuleConfiguration.DesktopModule.FolderName), 99)
        End Sub

        Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            RegisterModuleInfo()
        End Sub

        Public Sub RegisterModuleInfo()

            'Dim modCtrl As New Entities.Modules.ModuleController()

            Dim scriptblock = New StringBuilder()
            scriptblock.Append("<script>")
            scriptblock.Append(String.Format("var ascxVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(String.Format("{0}DesktopModules\RildoInfo\{1}\views\Edit.ascx", Request.PhysicalApplicationPath, Me.ModuleConfiguration.DesktopModule.FolderName)).ToString("g")))
            scriptblock.Append(String.Format("var jsVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(String.Format("{0}DesktopModules\RildoInfo\{1}\viewmodels\edit.js", Request.PhysicalApplicationPath, Me.ModuleConfiguration.DesktopModule.FolderName)).ToString("g")))
            scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(RiwModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), RiwModulePathScript, scriptblock.ToString())
            End If
        End Sub

#End Region

        'Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        '    Try

        '        If Not Page.IsPostBack Then
        '            ddlAssignedUser.DataSource = UserController.GetUsers(PortalId)
        '            ddlAssignedUser.DataTextField = "Username"
        '            ddlAssignedUser.DataValueField = "UserId"
        '            ddlAssignedUser.DataBind()

        '            If ItemId > 0 Then
        '                Dim tc As New ItemController
        '                Dim t As Item = tc.GetItem(ItemId, ModuleId)
        '                If t IsNot Nothing Then
        '                    txtName.Text = t.ItemName
        '                    txtDescription.Text = t.ItemDescription
        '                    ddlAssignedUser.Items.FindByValue(t.AssignedUserId.ToString()).Selected = True
        '                End If
        '            End If
        '        End If
        '    Catch exc As Exception
        '        Exceptions.ProcessModuleLoadException(Me, exc)
        '    End Try

        'End Sub

    End Class

End Namespace