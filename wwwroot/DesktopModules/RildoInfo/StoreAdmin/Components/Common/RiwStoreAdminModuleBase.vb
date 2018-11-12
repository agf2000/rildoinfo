
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Web.Client.ClientResourceManagement
'Imports DotNetNuke.Security.Permissions.ModulePermissionController
Imports RIW.Modules.Common.Utilities

Namespace Components.Common

    Public Class RiwStoreAdminModuleBase
    Inherits PortalModuleBase

        Private Const StrModulePathScript As String = "modulePathScript"

        Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            jQuery.RequestDnnPluginsRegistration()
            RegisterModulePath()
        End Sub

        Public Sub RegisterModulePath()

            'Dim intAuthorized As Integer = -1

            'If Not CBool(UserId = -1) Then
            '    'If HasModuleAccess(SecurityAccessLevel.Edit, "EXCLUDEPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "EDITPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "INSERTPERMISSION", ModuleConfiguration) Then
            '    '    intAuthorized = 3
            '    'ElseIf Not HasModuleAccess(SecurityAccessLevel.Edit, "EXCLUDEPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "EDITPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "INSERTPERMISSION", ModuleConfiguration) Then
            '    '    intAuthorized = 2
            '    'ElseIf Not HasModuleAccess(SecurityAccessLevel.Edit, "EXCLUDEPERMISSION", ModuleConfiguration) AndAlso Not HasModuleAccess(SecurityAccessLevel.Edit, "EDITPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "INSERTPERMISSION", ModuleConfiguration) Then
            '    '    intAuthorized = 1
            '    'End If
            '    If UserInfo.IsInRole("Gerentes") Then
            '        intAuthorized = 3
            '    ElseIf UserInfo.IsInRole("Vendedores") Then
            '        intAuthorized = 2
            '    ElseIf UserInfo.IsInRole("Editores") Then
            '        intAuthorized = 1
            '    ElseIf UserInfo.IsInRole("Caixas") Then
            '        intAuthorized = 0
            '    End If
            'End If

            Dim modulepath = DotNetNuke.Common.Globals.DesktopModulePath
            Dim mCtrl As New ModuleController()
            Dim moduleInfo = mCtrl.GetModule(ModuleId)

            Dim roleCtrl As New DotNetNuke.Security.Roles.RoleController
            Dim clientRoleId = roleCtrl.GetRoleByName(PortalId, "Clientes").RoleID
            Dim theUsers As New Users.UserController

            Dim scriptblock = New StringBuilder()
            scriptblock.Append("<script type='text/javascript' id='riw_store_admin'>")
            scriptblock.Append(String.Format("var portalID = {0}; ", PortalId))
            scriptblock.Append(String.Format("var modulePath = ""{0}""; ", modulepath))
            scriptblock.Append(String.Format("var moduleTitle = ""{0}""; ", moduleInfo.ModuleTitle))
            scriptblock.Append(String.Format("var clientRoleId = ""{0}""; ", clientRoleId))
            scriptblock.Append(String.Format("var returnURL = ""{0}""; ", Request.QueryString("returnurl")))
            'scriptblock.Append(String.Format("var authorized = {0}; ", intAuthorized))
            scriptblock.Append(String.Format("var siteURL = ""{0}""; ", PortalSettings.PortalAlias.HTTPAlias))
            scriptblock.Append(String.Format("var allowedExtensions = ""{0}""; ", DotNetNuke.Entities.Host.Host.AllowedExtensionWhitelist))
            scriptblock.Append(String.Format("var userID = ""{0}""; ", UserInfo.UserID))
            scriptblock.Append(String.Format("var addressURL = ""{0}""; ", NavigateURL(GetPageID("RIW Store Admin Contacts", PortalId).TabID)))
            scriptblock.Append(String.Format("var registrationURL = ""{0}""; ", NavigateURL(GetPageID("RIW Store Admin Registration", PortalId).TabID)))
            scriptblock.Append(String.Format("var payCondsURL = ""{0}""; ", NavigateURL(GetPageID("RIW Store Admin Pay Forms", PortalId).TabID)))
            scriptblock.Append(String.Format("var estimateURL = ""{0}""; ", NavigateURL(GetPageID("RIW Store Admin Estimates", PortalId).TabID)))
            scriptblock.Append(String.Format("var syncURL = ""{0}""; ", NavigateURL(GetPageID("RIW Store Admin Sync", PortalId).TabID)))
            scriptblock.Append(String.Format("var smtpURL = ""{0}""; ", NavigateURL(GetPageID("RIW Store Admin SMTP", PortalId).TabID)))
            scriptblock.Append(String.Format("var davReturnsURL = ""{0}""; ", NavigateURL(GetPageID("RIW Store Admin DAV Returns", PortalId).TabID)))
            scriptblock.Append(String.Format("var statusesManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Store Admin Statuses", PortalId).TabID)))
            scriptblock.Append(String.Format("var websiteManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Store Admin Main Portal", PortalId).TabID)))
            scriptblock.Append(String.Format("var templatesManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Store Admin Templates", PortalId).TabID)))
            '_scriptblock.Append(String.Format("var displayName = ""{0}""; ", UserInfo.DisplayName))
            '_scriptblock.Append(String.Format("var email = ""{0}""; ", UserInfo.Email))
            '_scriptblock.Append(String.Format("var avatar = ""{0}""; ", avatarPath))
            scriptblock.Append(String.Format("var managerRole = ""{0}""; ", UserInfo.IsInRole("Gerentes")))
            scriptblock.Append(String.Format("var fileManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Store Admin Main Portal", PortalId).TabID, "FileManager", "mid=" & CStr(GetModInfo("RIW Store Admin Main Portal", PortalId).ModuleID))))
            scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(StrModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), StrModulePathScript, scriptblock.ToString())
            End If

            ' register javascript libraries
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.signalR-1.1.4.min.js", 19)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-3.1.0.js", 20)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout.mapping-latest.js", 21)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/kendo.web.min.js", 22)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/cultures/kendo.culture.pt-BR.min.js", 23)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/messages/kendo.messages.pt-BR.min.js", 24)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-kendo.min.js", 25)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/modernizr.min.js", 26)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/browser-detection.js", 26)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/select2/select2.min.js", 27)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/select2/select2-locales/select2_locale_pt-BR.js", 28)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/engage.itoggle-min.js", 29)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/pnotify/jquery.pnotify.min.js", 27)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.scrollTo.min.js", 30)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/json2.js", 31)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.onscreen.min.js", 32)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.easing.1.3.js", 33)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/colorbox/jquery.colorbox-min.js", 33)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/colorbox/i18n/jquery.colorbox-pt-br.js", 34)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-switch-case.min.js", 35)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxcore.js", 36)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxdropdownbutton.js", 37)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxtree.js", 38)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxmenu.js", 39)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxpanel.js", 40)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxscrollbar.js", 41)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxbuttons.js", 42)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/amplify.min.js", 43)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/alloyui/aui-min.js", 44)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/bootstrap.min.js", 45)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/bootstrap-switch.min.js", 28)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/alloyui/aui-min.js", 44)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.inputmask/jquery.inputmask.min.js", 29)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/moment.min.js", 30)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/moment-with-langs.min.js", 31)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.autogrow-textarea.js", 32)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/Markdown/Markdown.Converter.js", 51)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/Markdown/Markdown.Sanitizer.js", 52)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/Markdown/Markdown.Editor.js", 53)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/Markdown/jquery.markdown.js", 54)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/to-markdown.js", 33)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/showdown.js", 34)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/app/utilities.js", 35)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/Scripts/jscolor/jscolor.js", 36)
            ClientResourceManager.RegisterScript(Parent.Page, String.Format("/desktopmodules/rildoinfo/{0}/viewmodels/viewModel.js", Me.ModuleConfiguration.DesktopModule.FolderName), 37)
            'ClientResourceManager.RegisterScript(Parent.Page, "/signalr/hubs", 60)

            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.3.930/styles/kendo.common.min.css", 71)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.3.930/styles/kendo.metro.min.css", 72)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/browser-detection.css", 73)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/css/engage.itoggle.css", 74)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.css", 75)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.icons.css", 76)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2.css", 77)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2-bootstrap.css", 78)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jquery-ui-1.9.2.custom.css", 79)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jquery.nailthumb.1.1.min.css", 80)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/colorbox/colorbox.css", 81)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jqwidgets/styles/jqx.base.css", 82)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2.css", 78)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2-bootstrap.css", 79)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2.css", 83)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2-bootstrap.css", 84)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap.min.css", 85)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-responsive.min.css", 86)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-theme.min.css", 87)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-switch.css", 88)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/flat-ui-fonts.css", 89)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/font-awesome.min.css", 90)

        End Sub

    End Class

End Namespace
