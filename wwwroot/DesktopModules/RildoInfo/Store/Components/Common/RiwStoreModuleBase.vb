Imports DotNetNuke.Web.Client.ClientResourceManagement
'Imports RIW.Modules.WebAPI
Imports RIW.Modules.Common

Namespace Components.Common

    Public Class RiwStoreModuleBase
        Inherits PortalModuleBase

        Private Const StrModulePathScript As String = "modulePathScript"

        Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            jQuery.RequestDnnPluginsRegistration()
            RegisterModulePath()
        End Sub

        Public Sub RegisterModulePath()

            'Dim intAuthorized As Integer = -1

            'If Not CBool(UserId = -1) Then
            '    '    If HasModuleAccess(SecurityAccessLevel.Edit, "EXCLUDEPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "EDITPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "INSERTPERMISSION", ModuleConfiguration) Then
            '    '        intAuthorized = 3
            '    '    ElseIf Not HasModuleAccess(SecurityAccessLevel.Edit, "EXCLUDEPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "EDITPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "INSERTPERMISSION", ModuleConfiguration) Then
            '    '        intAuthorized = 2
            '    '    ElseIf Not HasModuleAccess(SecurityAccessLevel.Edit, "EXCLUDEPERMISSION", ModuleConfiguration) AndAlso Not HasModuleAccess(SecurityAccessLevel.Edit, "EDITPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "INSERTPERMISSION", ModuleConfiguration) Then
            '    '        intAuthorized = 1
            '    '    End If
            '    If UserInfo.IsInRole("Gerentes") Then
            '        intAuthorized = 3
            '    ElseIf UserInfo.IsInRole("Vendedores") Then
            '        intAuthorized = 2
            '    ElseIf UserInfo.IsInRole("Clientes") Then
            '        intAuthorized = 1
            '    End If
            'End If

            'Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(PortalId)
            Dim mCtrl As New ModuleController()
            Dim mSettings = mCtrl.GetTabModuleSettings(mCtrl.GetModuleByDefinition(PortalId, "RIW Estimate").TabModuleID)

            Dim scriptblock = New StringBuilder()
            scriptblock.Append("<script type='text/javascript' id='riw_store'>")
            'scriptblock.Append([String].Format("var siteName = '{0}'; ", PortalSettings.PortalName))
            'scriptblock.Append([String].Format("var siteURL = '{0}'; ", PortalSettings.PortalAlias.HTTPAlias))
            'scriptblock.Append([String].Format("var managerRole = '{0}'; ", UserInfo.IsInRole("Gerentes").ToString().ToLower()))
            scriptblock.Append([String].Format("var isDealer = '{0}'; ", UserInfo.IsInRole("Revendedores").ToString().ToLower()))
            'scriptblock.Append([String].Format("var _siteEmail = '{0}'; ", settingsDictionay("StoreEmail")))
            scriptblock.Append([String].Format("var configURL = '{0}'; ", NavigateURL(Utilities.GetPageID("RIW Estimate", PortalId).TabID, "Options", "mid=" & CStr(Utilities.GetModInfo("RIW Estimate", PortalId).ModuleID))))
            '_scriptblock.Append([String].Format("var _productsData = {0}; ", productsData.Count))
            scriptblock.Append([String].Format("var categoriesURL = '{0}'; ", NavigateURL(Utilities.GetPageID("RIW Categories Manager", PortalId).TabID)))
            scriptblock.Append([String].Format("var editItemURL = '{0}'; ", NavigateURL(Utilities.GetPageID("RIW Products Manager", PortalId).TabID, "Edit", "mid=" & CStr(Utilities.GetModInfo("RIW Products Manager", PortalId).ModuleID))))
            '_scriptblock.Append([String].Format("var _detailURL = '{0}'; ", NavigateURL(Utilities.GetPageID("RIW Product Detail", PortalId).TabID)))
            scriptblock.Append([String].Format("var productsURL = '{0}'; ", NavigateURL(Utilities.GetPageID("RIW Products", PortalId).TabID)))
            scriptblock.Append([String].Format("var productsPage = '{0}'; ", NavigateURL(Utilities.GetPageID("RIW Products", PortalId).TabName)))
            scriptblock.Append([String].Format("var returnURL = '{0}'; ", NavigateURL()))
            scriptblock.Append([String].Format("var estimateURL = '{0}'; ", NavigateURL(Utilities.GetPageID("RIW My Estimates", PortalId).TabID)))
            'scriptblock.Append([String].Format("var portalID = {0}; ", PortalId))
            'scriptblock.Append([String].Format("var authorized = {0}; ", intAuthorized))
            'scriptblock.Append([String].Format("var userID = {0}; ", UserInfo.UserID))
            'scriptblock.Append([String].Format("var userName = '{0}'; ", UserInfo.Username))
            'scriptblock.Append([String].Format("var _salesPerson = {0}; ", CInt(IIf(UserInfo.IsInRole("Vendedores"), UserInfo.UserID, CInt(settingsDictionay("SalesPerson"))))))
            'scriptblock.Append([String].Format("var _viewPrice = '{0}'; ", settingsDictionay("ShowEstimatePrice").ToLower()))
            'scriptblock.Append([String].Format("var _noStockAllowed = ""{0}""; ", settingsDictionay("NoStockAllowed").ToLower()))
            'scriptblock.Append([String].Format("var _allowPurchase = '{0}'; ", settingsDictionay("AllowPurchase").ToLower()))
            'scriptblock.Append([String].Format("var displayName = '{0}'; ", UserInfo.DisplayName))
            'scriptblock.Append([String].Format("var clientEmail = '{0}'; ", UserInfo.Email))
            'scriptblock.Append([String].Format("var _pageSize = {0}; ", settingsDictionay("PageSize")))
            scriptblock.Append([String].Format("var tModuleID = {0}; ", mCtrl.GetModuleByDefinition(PortalId, "RIW Estimate").TabModuleID))
            scriptblock.Append([String].Format("var moduleID = {0}; ", mCtrl.GetModuleByDefinition(PortalId, "RIW Estimate").ModuleID))
            'scriptblock.Append([String].Format("var _watermark = '{0}'; ", settingsDictionay("ProductWatermark")))
            scriptblock.Append([String].Format("var estimateSubject = '{0}'; ", mSettings("RIW_EstimateEmailSubject")))
            scriptblock.Append([String].Format("var estimateBody = '{0}'; ", mSettings("RIW_EstimateEmailBody")))
            scriptblock.Append([String].Format("var sendNewEstimateTo = '{0}'; ", mSettings("RIW_SendNewEstimateTo")))
            scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(StrModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), StrModulePathScript, scriptblock.ToString())
            End If

            ' register javascript libraries
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-3.1.0.js", 20)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout.mapping-latest.js", 23)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/kendo.web.min.js", 22)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/cultures/kendo.culture.pt-BR.min.js", 23)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/messages/kendo.messages.pt-BR.min.js", 24)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-kendo.min.js", 25)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.flexslider-min.js", 26)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/modernizr.min.js", 27)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/browser-detection.js", 27)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/colorbox/jquery.colorbox-min.js", 28)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/colorbox/i18n/jquery.colorbox-pt-br.js", 29)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/amplify.min.js", 30)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.maskedinput.min.js", 30)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/pnotify/jquery.pnotify.min.js", 32)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/showdown.js", 33)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.scrollTo.min.js", 34)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/moment-with-langs.min.js", 35)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/amplify.min.js", 36)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxcore.js", 37)
            ''ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxdropdownbutton.js", 38)
            ''ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxtree.js", 39)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxmenu.js", 40)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxpanel.js", 41)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxscrollbar.js", 42)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxbuttons.js", 43)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/bootstrap.min.js", 44)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.carouFredSel-6.2.1-packed.js", 45)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.inputmask/jquery.inputmask.min.js", 46)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.onscreen.min.js", 47)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/select2/select2.min.js", 75)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.autogrow-textarea.js", 49)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/showdown.js", 50)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/to-markdown.js", 51)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/app/utilities.js", 52)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/slimmage.min.js", 53)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/store/scripts/App/data.js", 54)
            ClientResourceManager.RegisterScript(Parent.Page, String.Format("/desktopmodules/rildoinfo/{0}/viewmodels/viewModel.js", Me.ModuleConfiguration.DesktopModule.FolderName), 55)

            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.3.930/styles/kendo.common.min.css", 71)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.3.930/styles/kendo.default.min.css", 72)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/browser-detection.css", 73)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/colorbox/colorbox.css", 74)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jquery.toastmessage-min.css", 75)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.css", 76)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.icons.css", 77)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jqwidgets/styles/jqx.base.css", 78)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap3/css/bootstrap.min.css", 79)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap3/css/bootstrap-theme.min.css", 81)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap.min.css", 79)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-responsive.min.css", 80)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-theme.min.css", 81)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-switch.css", 82)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/font-awesome.min.css", 83)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/flat-ui-fonts.css", 84)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/flexslider.css", 85)

        End Sub

    End Class

End Namespace
