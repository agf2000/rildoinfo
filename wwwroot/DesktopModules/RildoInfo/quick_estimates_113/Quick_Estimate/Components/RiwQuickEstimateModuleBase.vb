
Imports DotNetNuke.Web.Client.ClientResourceManagement
Imports RIW.Modules.Common

Namespace Components

    Public Class RiwQuickEstimateModuleBase
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

            'Dim intAuthorized As Integer = -1

            'If UserInfo.IsInRole("Gerentes") Then
            '    intAuthorized = 3
            'ElseIf UserInfo.IsInRole("Vendedores") Then
            '    intAuthorized = 1
            'ElseIf UserInfo.IsInRole("Editores") Then
            '    intAuthorized = 0
            'ElseIf UserInfo.IsInRole("Caixas") Then
            '    intAuthorized = 2
            'End If

            Dim modulepath = DotNetNuke.Common.Globals.DesktopModulePath

            Dim mCtrl As New ModuleController()
            Dim mSettings = mCtrl.GetTabModuleSettings(mCtrl.GetModuleByDefinition(PortalId, "RIW Quick Estimate").TabModuleID)
            'Dim subject = mSettings("RIW_EstimateEmailSubject")
            'Dim body = mSettings("RIW_EstimateEmailBody")

            Dim orLogoUrl = ""
            Dim orLogoFile = ""
            If Not Null.IsNull(mSettings("RIW_OR_Logo")) Then
                If mSettings("RIW_OR_Logo") <> "" Then
                    orLogoUrl = PortalSettings.HomeDirectory & mSettings("RIW_OR_Logo")
                    orLogoFile = mSettings("RIW_OR_Logo")
                End If
            End If

            Dim roleCtrl As New DotNetNuke.Security.Roles.RoleController
            Dim clientRoleId = roleCtrl.GetRoleByName(PortalId, "Clientes").RoleID
            Dim providerRoleId = roleCtrl.GetRoleByName(PortalId, "Fornecedores").RoleID

            Dim scriptblock = New StringBuilder()
            scriptblock.Append("<script type='text/javascript' id='riw_or'>")
            scriptblock.Append(String.Format("var siteName = '{0}'; ", PortalSettings.PortalName))
            'scriptblock.Append(String.Format("var customerLogo = '{0}'; ", customerLogo))
            'scriptblock.Append(String.Format("var footerText = '{0}'; ", PortalSettings.FooterText.Replace("[year]", Now.Year.ToString())))
            'scriptblock.Append(String.Format("var pdfFooter = '{0}'; ", strFooter.ToString()))
            'scriptblock.Append(String.Format("var siteURL = '{0}'; ", PortalSettings.PortalAlias.HTTPAlias))
            'scriptblock.Append(String.Format("var logoURL = '{0}'; ", PortalSettings.HomeDirectory & PortalSettings.LogoFile))
            'scriptblock.Append(String.Format("var storeStreet = '{0}'; ", SettingsDictionay("StoreAddress")))
            'scriptblock.Append(String.Format("var storeUnit = '{0}'; ", SettingsDictionay("StoreUnit")))
            'scriptblock.Append(String.Format("var storeComplement = '{0}'; ", SettingsDictionay("StoreComplement")))
            'scriptblock.Append(String.Format("var storeDistrict = '{0}'; ", SettingsDictionay("StoreDistrict")))
            'scriptblock.Append(String.Format("var storeCity = '{0}'; ", SettingsDictionay("StoreCity")))
            'scriptblock.Append(String.Format("var storePostalCode = '{0}'; ", SettingsDictionay("StorePostalCode")))
            'scriptblock.Append(String.Format("var storeRegion = '{0}'; ", SettingsDictionay("StoreRegion")))
            'scriptblock.Append(String.Format("var storeCountry = '{0}'; ", SettingsDictionay("StoreCountry")))
            'scriptblock.Append(String.Format("var storeEmail = '{0}'; ", SettingsDictionay("StoreEmail")))
            'scriptblock.Append(String.Format("var storePhone = '{0}'; ", SettingsDictionay("StorePhones").ToLower()))
            'scriptblock.Append(String.Format("var watermark = '{0}'; ", SettingsDictionay("ProductWatermark")))
            scriptblock.Append(String.Format("var modulePath = '{0}'; ", modulepath))
            scriptblock.Append(String.Format("var orLogoURL = ""{0}""; ", orLogoUrl))
            scriptblock.Append(String.Format("var orLogoFile = ""{0}""; ", orLogoFile))
            scriptblock.Append(String.Format("var slogan = ""{0}""; ", mSettings("RIW_OR_Slogan")))
            scriptblock.Append(String.Format("var tModuleID = {0}; ", mCtrl.GetModuleByDefinition(PortalId, "RIW Quick Estimate").TabModuleID))
            scriptblock.Append(String.Format("var returnURL = '{0}'; ", DotNetNuke.Common.Globals.NavigateURL()))
            '_scriptblock.Append(String.Format("var newClientURL = '{0}'; ", DotNetNuke.Common.Globals.NavigateURL(Utilities.GetPageID("RIW People Manager", PortalId), "", "ctl=Edit&mid=" & Utilities.GetModInfo("RIW People Manager", PortalId).ModuleID)))
            scriptblock.Append(String.Format("var editClientURL = '{0}'; ", DotNetNuke.Common.Globals.NavigateURL(Utilities.GetPageID("RIW People Manager", PortalId).TabID, "", "ctl=Edit&mid=" & Utilities.GetModInfo("RIW People Manager", PortalId).ModuleID)))
            scriptblock.Append(String.Format("var editClientModuleID = '{0}'; ", Utilities.GetModInfo("RIW People Manager", PortalId).ModuleID))
            scriptblock.Append(String.Format("var estimateURL = '{0}'; ", DotNetNuke.Common.Globals.NavigateURL(Utilities.GetPageID("RIW Estimates Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var configURL = '{0}'; ", EditUrl("Options")))
            scriptblock.Append(String.Format("var portalID = {0}; ", PortalId))
            scriptblock.Append(String.Format("var clientRoleId = {0}; ", clientRoleId))
            scriptblock.Append(String.Format("var providerRoleId = {0}; ", providerRoleId))
            'scriptblock.Append(String.Format("var authorized = {0}; ", intAuthorized))
            'scriptblock.Append(String.Format("var userID = {0}; ", UserInfo.UserID))
            'scriptblock.Append(String.Format("var userName = '{0}'; ", UserInfo.Username))
            scriptblock.Append(String.Format("var qeImage = '{0}'; ", IIf(mSettings("RIW_QEImage") IsNot Nothing, mSettings("RIW_QEImage"), "spacer.gif")))
            scriptblock.Append(String.Format("var estimateSubject = '{0}'; ", mSettings("RIW_EstimateEmailSubject")))
            scriptblock.Append(String.Format("var estimateBody = '{0}'; ", mSettings("RIW_EstimateEmailBody")))

            'If subject IsNot Nothing Then
            '    scriptblock.Append(String.Format("var estimateSubject = '{0}'; ", Utilities.DecodeAccentletters(CStr(subject))))
            'Else
            '    scriptblock.Append(String.Format("var estimateSubject = '{0}'; ", ""))
            'End If

            'If body IsNot Nothing Then
            '    scriptblock.Append(String.Format("var estimateBody = '{0}'; ", Utilities.DecodeAccentletters(CStr(body))))
            'Else
            '    scriptblock.Append(String.Format("var estimateBody = '{0}'; ", ""))
            'End If

            scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(StrModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), StrModulePathScript, scriptblock.ToString())
            End If

            ' register javascript libraries
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.hotkeys.min.js", 20)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/shortcut.js", 21)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-3.1.0.js", 21)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout.mapping-latest.js", 23)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/kendo.web.min.js", 24)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/cultures/kendo.culture.pt-BR.min.js", 25)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/messages/kendo.messages.pt-BR.min.js", 26)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-kendo.min.js", 27)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/modernizr.min.js", 28)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/moment-with-langs.min.js", 29)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.autogrow-textarea.js", 30)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/pnotify/jquery.pnotify.min.js", 31)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/showdown.js", 32)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/browser-detection.js", 31)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/colorbox/jquery.colorbox-min.js", 33)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/colorbox/i18n/jquery.colorbox-pt-br.js", 34)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/to-markdown.js", 35)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/select2/select2.min.js", 36)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/select2/select2-locales/select2_locale_pt-BR.js", 37)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.inputmask/jquery.inputmask.min.js", 38)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/bootstrap.min.js", 38)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/idle-timer.min.js", 39)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/app/utilities.js", 40)
            ClientResourceManager.RegisterScript(Parent.Page, String.Format("/desktopmodules/rildoinfo/{0}/viewmodels/viewModel.js", Me.ModuleConfiguration.DesktopModule.FolderName), 41)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/quick_estimate/scripts/App/shortcuts.js", 42)

            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.3.930/styles/kendo.common.min.css", 71)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.3.930/styles/kendo.default.min.css", 72)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/browser-detection.css", 73)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/colorbox/colorbox.css", 74)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jquery.toastmessage.css", 75)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.css", 75)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.icons.css", 75)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap.min.css", 76)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-responsive.min.css", 77)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-theme.min.css", 78)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-formhelpers.min.css", 79)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2.css", 80)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2-bootstrap.css", 81)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/hoverEffect.css", 82)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/font-awesome.min.css", 83)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/flat-ui-fonts.css", 84)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/markdown/markitup/skins/default/style.css", 84)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/quick_estimate/content/markdown/markitup/sets/markdown/style.css", 85)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/preview.css", 85)

        End Sub

        #End Region

    End Class

End Namespace
