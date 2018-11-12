' Copyright (c) 2013 AGF
Imports DotNetNuke.Web.Client.ClientResourceManagement
'Imports DotNetNuke.Security.Permissions.ModulePermissionController
Imports RIW.Modules.Common.Utilities

Namespace Components.Common

    Public Class RiwPeopleModuleBase
        Inherits PortalModuleBase

        Private Const StrModulePathScript As String = "modulePathScript"

        Public Sub RegisterModulePath()

            Dim modulepath = DesktopModulePath
            Dim mCtrl As New ModuleController()
            Dim moduleInfo = mCtrl.GetModule(ModuleId)

            'Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(PortalId)

            Dim avatarPath = ""
            If UserInfo.Profile.Photo IsNot Nothing Then
                If UserInfo.Profile.Photo.Length > 2 Then
                    avatarPath = Services.FileSystem.FolderManager.Instance().GetUserFolder(UserInfo).FolderPath
                    avatarPath = avatarPath & Services.FileSystem.FileManager.Instance().GetFile(CInt(UserInfo.Profile.Photo)).FileName
                End If
            End If

            Dim roleCtrl As New DotNetNuke.Security.Roles.RoleController
            Dim clientRoleId = roleCtrl.GetRoleByName(PortalId, "Clientes").RoleID
            Dim providerRoleId = roleCtrl.GetRoleByName(PortalId, "Fornecedores").RoleID

            Dim scriptblock = New StringBuilder()
            scriptblock.Append("<script type='text/javascript' id='riw_people_manager'>")
            scriptblock.Append(String.Format("var modulePath = ""{0}""; ", modulepath))
            scriptblock.Append(String.Format("var moduleTitle = ""{0}""; ", moduleInfo.ModuleTitle))
            scriptblock.Append(String.Format("var avatar = ""{0}""; ", avatarPath))
            If GetPageID("RIW Products Manager", PortalId).TabID > 0 Then
                scriptblock.Append(String.Format("var editProductURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products Manager", PortalId).TabID, "Edit", "mid=" & CStr(GetModInfo("RIW Products Manager", PortalId).ModuleID))))
            End If
            If GetPageID("RIW Products", PortalId).TabID > 0 Then
                scriptblock.Append(String.Format("var productsURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products", PortalId).TabID)))
            End If
            If GetPageID("RIW Products", PortalId).TabID > 0 Then
                scriptblock.Append(String.Format("var productsPage = ""{0}""; ", NavigateURL(GetPageID("RIW Products", PortalId).TabName)))
            End If
            scriptblock.Append(String.Format("var returnURL = ""{0}""; ", Request.QueryString("returnurl")))
            scriptblock.Append(String.Format("var editURL = ""{0}""; ", EditUrl("Edit")))
            scriptblock.Append(String.Format("var editUserURL = ""{0}""; ", EditUrl("User")))
            If GetPageID("RIW My Profile", PortalId).TabID > 0 Then
                scriptblock.Append(String.Format("var profileURL = ""{0}""; ", NavigateURL(GetPageID("RIW My Profile", PortalId).TabID, "", "userId=" & UserId.ToString())))
            End If
            scriptblock.Append(String.Format("var assistURL = ""{0}""; ", EditUrl("Assist")))
            scriptblock.Append(String.Format("var finanURL = ""{0}""; ", EditUrl("Finance")))
            scriptblock.Append(String.Format("var addressesURL = ""{0}""; ", EditUrl("Addresses")))
            scriptblock.Append(String.Format("var historyURL = ""{0}""; ", EditUrl("Log")))
            scriptblock.Append(String.Format("var commURL = ""{0}""; ", EditUrl("Comm")))
            scriptblock.Append(String.Format("var editorURL = ""{0}""; ", EditUrl("Editor")))
            scriptblock.Append(String.Format("var portalID = {0}; ", PortalId))
            scriptblock.Append(String.Format("var clientRoleId = {0}; ", clientRoleId))
            scriptblock.Append(String.Format("var providerRoleId = {0}; ", providerRoleId))
            scriptblock.Append(String.Format("var clientEstimateURL = ""{0}""; ", NavigateURL(GetPageID("RIW Estimates Manager", PortalId).TabID, "Edit", "mid=" & CStr(GetModInfo("RIW Estimates Manager", PortalId).ModuleID))))
            If GetPageID("RIW Payments Manager", PortalId).TabID > 0 Then
                scriptblock.Append(String.Format("var accountsManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Payments Manager", PortalId).TabID)))
            End If
            If GetPageID("RIW Agenda", PortalId).TabID > 0 Then
                scriptblock.Append(String.Format("var agendaURL = ""{0}""; ", NavigateURL(GetPageID("RIW Agenda", PortalId).TabID)))
            End If
            If GetPageID("RIW Categories Manager", PortalId).TabID > 0 Then
                scriptblock.Append(String.Format("var categoriesManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Categories Manager", PortalId).TabID)))
            End If
            If GetPageID("RIW People Manager", PortalId).TabID > 0 Then
                scriptblock.Append(String.Format("var peopleManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW People Manager", PortalId).TabID)))
            End If
            If GetPageID("RIW Invoices Manager", PortalId).TabID > 0 Then
                scriptblock.Append(String.Format("var invoicesManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Invoices Manager", PortalId).TabID)))
            End If
            If GetPageID("RIW Products Manager", PortalId).TabID > 0 Then
                scriptblock.Append(String.Format("var productsManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products Manager", PortalId).TabID)))
            End If
            scriptblock.Append(String.Format("var reportsManagerURL = ""{0}""; ", NavigateURL(GetPageID("Reports Viewer", PortalId).TabID)))
            If GetPageID("RIW Store Admin Store", PortalId).TabID > 0 Then
                scriptblock.Append(String.Format("var storeManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Store Admin Store", PortalId).TabID)))
            End If
            If GetPageID("RIW Users Manager", PortalId).TabID > 0 Then
                scriptblock.Append(String.Format("var usersManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Users Manager", PortalId).TabID)))
            End If
            If GetPageID("RIW Quick Estimate", PortalId).TabID > 0 Then
                scriptblock.Append(String.Format("var orURL = ""{0}""; ", NavigateURL(GetPageID("RIW Quick Estimate", PortalId).TabID)))
            End If

            scriptblock.Append(String.Format("var clientsRoleId = ""{0}""; ", roleCtrl.GetRoleByName(PortalId, "Clientes").RoleID.ToString()))

            'scriptblock.Append(String.Format("var _authorized = {0}; ", intAuthorized))
            'scriptblock.Append(String.Format("var _userID = {0}; ", UserInfo.UserID))
            'scriptblock.Append(String.Format("var _managerRole = ""{0}""; ", UserInfo.IsInRole("Gerentes")))
            scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(StrModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), StrModulePathScript, scriptblock.ToString())
            End If

            ' register javascript libraries
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.signalR-1.1.4.min.js", 19)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-3.1.0.js", 20)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout.mapping-latest.js", 21)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/kendo.web.min.js", 22)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/cultures/kendo.culture.pt-BR.min.js", 23)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/messages/kendo.messages.pt-BR.min.js", 24)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-kendo.min.js", 25)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/modernizr.min.js", 26)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/browser-detection.js", 26)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/select2/select2.min.js", 27)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/select2/select2-locales/select2_locale_pt-BR.js", 28)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.toastmessage-min.js", 29)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/pnotify/jquery.pnotify.min.js", 29)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.scrollTo.min.js", 30)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/json2.min.js", 31)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.onscreen.min.js", 32)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.nailthumb.1.1.min.js", 33)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/colorbox/jquery.colorbox-min.js", 33)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/colorbox/i18n/jquery.colorbox-pt-br.js", 34)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-switch-case.min.js", 35)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxcore.js", 36)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxdropdownbutton.js", 37)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxtree.js", 38)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxmenu.js", 39)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxpanel.js", 40)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxscrollbar.js", 41)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxbuttons.js", 42)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/amplify.min.js", 43)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/alloyui/aui-min.js", 44)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/bootstrap.min.js", 45)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/bootstrap-switch.min.js", 46)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/alloyui/aui-min.js", 44)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.inputmask/jquery.inputmask.min.js", 48)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/moment-with-langs.min.js", 49)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.autogrow-textarea.js", 50)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/Markdown/Markdown.Converter.js", 51)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/Markdown/Markdown.Sanitizer.js", 52)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/Markdown/Markdown.Editor.js", 53)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/Markdown/jquery.markdown.js", 54)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/to-markdown.js", 55)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/showdown.js", 56)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/riwNotification.js", 57)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/app/utilities.js", 58)
            ClientResourceManager.RegisterScript(Parent.Page, String.Format("/desktopmodules/rildoinfo/{0}/viewmodels/viewmodel.js", Me.ModuleConfiguration.DesktopModule.FolderName), 59)
            ClientResourceManager.RegisterScript(Parent.Page, "/signalr/hubs", 60)

            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.3.930/styles/kendo.common.min.css", 71)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.3.930/styles/kendo.default.min.css", 72)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/browser-detection.css", 73)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jquery.toastmessage-min.css", 74)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.css", 75)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.icons.css", 76)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2.css", 77)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2-bootstrap.css", 78)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jquery-ui-1.9.2.custom.css", 79)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jquery.nailthumb.1.1.min.css", 80)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/colorbox/colorbox.css", 81)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jqwidgets/styles/jqx.base.css", 82)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2.css", 83)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2-bootstrap.css", 84)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap.min.css", 85)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-responsive.min.css", 86)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-theme.min.css", 87)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-switch.css", 88)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/flat-ui-fonts.css", 89)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/font-awesome.min.css", 90)

        End Sub

        Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            jQuery.RequestDnnPluginsRegistration()
            RegisterModulePath()
        End Sub

    End Class

End Namespace
