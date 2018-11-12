
Imports DotNetNuke.Web.Client.ClientResourceManagement
Imports RIW.Modules.Common.Utilities

Namespace Components.Common

    Public Class CarouselModuleBase
        Inherits PortalModuleBase

#Region "Private Members"

        Private Const StrModulePathScript As String = "modulePathScript"

#End Region

#Region "Events"

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

            'Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(PortalId)

            Dim modulepath = DotNetNuke.Common.Globals.DesktopModulePath
            Dim mCtrl As New ModuleController()
            Dim mSettings = mCtrl.GetTabModuleSettings(mCtrl.GetModuleByDefinition(PortalId, "RIW Estimates Manager").TabModuleID)
            Dim moduleInfo = mCtrl.GetModule(ModuleId)

            Dim roleCtrl As New DotNetNuke.Security.Roles.RoleController
            Dim clientRoleId = roleCtrl.GetRoleByName(PortalId, "Clientes").RoleID
            Dim providerRoleId = roleCtrl.GetRoleByName(PortalId, "Fornecedores").RoleID

            'Dim avatarPath = ""
            'If UserInfo.Profile.Photo IsNot Nothing Then
            '    If UserInfo.Profile.Photo.Length > 2 Then
            '        avatarPath = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(UserInfo).FolderPath
            '        avatarPath = avatarPath & DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(UserInfo.Profile.Photo).FileName
            '    End If
            'End If

            Dim scriptblock = New StringBuilder()
            scriptblock.Append("<script type='text/javascript' id='riw_estimate_manager'>")
            scriptblock.Append(String.Format("var modulePath = ""{0}""; ", modulepath))
            scriptblock.Append(String.Format("var moduleTitle = ""{0}""; ", moduleInfo.ModuleTitle))
            'scriptblock.Append(String.Format("var siteName = ""{0}""; ", PortalSettings.PortalName))
            'scriptblock.Append(String.Format("var footerText = ""{0}""; ", PortalSettings.FooterText.Replace("[year]", Now.Year.ToString())))
            'scriptblock.Append(String.Format("var siteURL = ""{0}""; ", PortalSettings.PortalAlias.HTTPAlias))
            'scriptblock.Append(String.Format("var logoURL = ""{0}""; ", PortalSettings.HomeDirectory & PortalSettings.LogoFile))
            scriptblock.Append(String.Format("var productDetailURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products", PortalId).TabID)))
            scriptblock.Append(String.Format("var returnURL = ""{0}""; ", NavigateURL()))
            'scriptblock.Append(String.Format("var editURL = ""{0}""; ", NavigateURL(GetPageID("RIW Estimates Manager", PortalId).TabID, "Edit", "mid=" & CStr(GetModInfo("RIW Estimates Manager", PortalId).ModuleID))))
            'scriptblock.Append(String.Format("var clientEstimateURL = ""{0}""; ", NavigateURL(GetPageID("RIW My Estimates", PortalId).TabID, "Edit", "mid=" & CStr(GetModInfo("RIW My Estimates", PortalId).ModuleID))))
            'scriptblock.Append(String.Format("var portalID = {0}; ", PortalId))
            scriptblock.Append(String.Format("var orURL = ""{0}""; ", NavigateURL(GetPageID("RIW Quick Estimate", PortalId).TabID)))
            'scriptblock.Append(String.Format("var authorized = {0}; ", intAuthorized))
            'scriptblock.Append(String.Format("var userID = {0};", UserInfo.UserID))
            'scriptblock.Append(String.Format("var displayName = ""{0}""; ", UserInfo.DisplayName))
            scriptblock.Append(String.Format("var clientRoleId = {0}; ", clientRoleId))
            scriptblock.Append(String.Format("var providerRoleId = {0}; ", providerRoleId))
            scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(StrModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), StrModulePathScript, scriptblock.ToString())
            End If

            ' register javascript libraries
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.signalR-1.1.4.min.js", 19)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-3.1.0.js", 20)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout.mapping-latest.js", 23)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.1.429/kendo.web.min.js", 22)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.1.429/cultures/kendo.culture.pt-BR.min.js", 23)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.1.429/messages/kendo.messages.pt-BR.min.js", 24)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-kendo.min.js", 25)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/modernizr.min.js", 26)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.toastmessage-min.js", 26)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/pnotify/jquery.pnotify.min.js", 27)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/moment-with-langs.min.js", 28) ' /desktopmodules/rildoinfo/webapi/scripts/moment.min.js
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/colorbox/jquery.colorbox-min.js", 29)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/colorbox/i18n/jquery.colorbox-pt-br.js", 30)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.scrollTo.min.js", 31)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/notify.js", 31)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/amplify.min.js", 32)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxcore.js", 33)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxdropdownbutton.js", 34)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxtree.js", 35)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxmenu.js", 36)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxpanel.js", 37)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxscrollbar.js", 38)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxbuttons.js", 39)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.onscreen.min.js", 40)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/select2/select2.min.js", 41)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/select2/select2-locales/select2_locale_pt-BR.js", 42)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/browser-detection.js", 44)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/knockout-bootstrap.min.js", 45)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/bootstrap.min.js", 46)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/bootstrap-switch.min.js", 47)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/to-markdown.js", 48)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.autogrow-textarea.js", 49)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/showdown.js", 50)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.jcarousel.min.js", 51)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/app/utilities.js", 52)
            'ClientResourceManager.RegisterScript(Parent.Page, String.Format("/desktopmodules/rildoinfo/{0}/viewmodels/viewModel.js", Me.ModuleConfiguration.DesktopModule.FolderName), 53)
            'ClientResourceManager.RegisterScript(Parent.Page, "/signalr/hubs", 55)

            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.1.429/styles/kendo.common.min.css", 71)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.1.429/styles/kendo.default.min.css", 72)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/browser-detection.css", 73)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/colorbox/colorbox.css", 74)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jquery.toastmessage-min.css", 75)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.css", 75)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.icons.css", 76)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jqwidgets/styles/jqx.base.css", 77)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2.css", 78)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2-bootstrap.css", 79)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap.min.css", 80)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-responsive.min.css", 81)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-theme.min.css", 82)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-switch.css", 83)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-markdown.min.css", 83)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/font-awesome.min.css", 85)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/flat-ui-fonts.css", 86)

        End Sub

#End Region

    End Class

End Namespace
