
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Framework
Imports DotNetNuke.Web.Client.ClientResourceManagement

Namespace Components.Common

    Public Class ReportsModuleBase
        Inherits PortalModuleBase

#Region "Private Members"

        Private Const StrModulePathScript As String = "modulePathScript"

#End Region

        Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            jQuery.RequestDnnPluginsRegistration()
            RegisterModulePath()
        End Sub

        Public Sub RegisterModulePath()

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

            Dim mCtrl As New ModuleController()
            Dim moduleInfo = mCtrl.GetModule(ModuleId)

            Dim roleCtrl As New DotNetNuke.Security.Roles.RoleController
            Dim clientRoleId = roleCtrl.GetRoleByName(PortalId, "Clientes").RoleID
            Dim providerRoleId = roleCtrl.GetRoleByName(PortalId, "Fornecedores").RoleID

            Dim scriptblock = New StringBuilder()
            scriptblock.Append("<script type='text/javascript' id='riw_products_manager'>")
            scriptblock.Append(String.Format("var moduleID = ""{0}""; ", ModuleId))
            scriptblock.Append(String.Format("var moduleTitle = ""{0}""; ", moduleInfo.ModuleTitle))
            scriptblock.Append(String.Format("var siteName = ""{0}""; ", PortalSettings.PortalName))
            scriptblock.Append(String.Format("var siteURL = ""{0}"";", PortalSettings.PortalAlias.HTTPAlias))
            scriptblock.Append(String.Format("var tModuleID = ""{0}""; ", TabModuleId))
            scriptblock.Append(String.Format("var moduleID = ""{0}""; ", ModuleId))
            scriptblock.Append(String.Format("var moduleTitle = ""{0}""; ", moduleInfo.ModuleTitle))
            scriptblock.Append(String.Format("var managerRole = ""{0}""; ", UserInfo.IsInRole("Gerentes")))
            scriptblock.Append(String.Format("var configURL = ""{0}""; ", EditUrl("Options")))
            scriptblock.Append(String.Format("var portalID = ""{0}""; ", PortalId))
            scriptblock.Append(String.Format("var authorized = ""{0}""; ", intAuthorized))
            scriptblock.Append(String.Format("var userID = ""{0}""; ", UserInfo.UserID))
            scriptblock.Append(String.Format("var userName = ""{0}""; ", UserInfo.Username))
            scriptblock.Append(String.Format("var clientRoleId = {0}; ", clientRoleId))
            scriptblock.Append(String.Format("var providerRoleId = {0}; ", providerRoleId))
            scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(StrModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), StrModulePathScript, scriptblock.ToString())
            End If

            ' register javascript libraries
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-3.1.0.js", 20)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/kendo.web.min.js", 21)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/jszip.min.js", 23)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/cultures/kendo.culture.pt-BR.min.js", 22)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/messages/kendo.messages.pt-BR.min.js", 23)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-kendo.min.js", 24)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/modernizr.min.js", 25)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/amplify.min.js", 26)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/pnotify/jquery.pnotify.min.js", 26)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.scrollTo.min.js", 27)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/moment-with-langs.min.js", 28)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/showdown.js", 29)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/select2/select2.min.js", 30)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/select2/select2-locales/select2_locale_pt-BR.js", 31)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/app/utilities.js", 32)
            ClientResourceManager.RegisterScript(Parent.Page, String.Format("/desktopmodules/rildoinfo/{0}/viewmodels/viewModel.js", Me.ModuleConfiguration.DesktopModule.FolderName), 33)

            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.3.930/styles/kendo.common.min.css", 71)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.css", 76)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.icons.css", 77)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2.css", 78)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2-bootstrap.css", 79)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/flat-ui-fonts.css", 80)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/font-awesome.min.css", 81)

        End Sub

    End Class

End Namespace
