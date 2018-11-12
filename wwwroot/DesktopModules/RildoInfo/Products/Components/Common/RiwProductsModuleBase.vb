Imports DotNetNuke.Web.Client.ClientResourceManagement
Imports RIW.Modules.Common.Utilities

Namespace Components.Common

    Public Class RiwProductsModuleBase
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

            Dim mCtrl As New ModuleController()
            Dim moduleInfo = mCtrl.GetModule(ModuleId)
            'Dim mSettings = mCtrl.GetTabModuleSettings(mCtrl.GetModuleByDefinition(PortalId, "RIW Products Manager").TabModuleID)

            Dim roleCtrl As New DotNetNuke.Security.Roles.RoleController
            Dim providerRoleId = roleCtrl.GetRoleByName(PortalId, "Fornecedores").RoleID

            Dim scriptblock = New StringBuilder()
            scriptblock.Append("<script type='text/javascript' id='riw_products_manager'>")
            scriptblock.Append(String.Format("var moduleID = ""{0}""; ", ModuleId))
            scriptblock.Append(String.Format("var moduleTitle = ""{0}""; ", moduleInfo.ModuleTitle))
            'scriptblock.Append(String.Format("var _siteEmail = ""{0}""; ", SettingsDictionay("StoreEmail")))
            'scriptblock.Append(String.Format("var authorized = {0}; ", intAuthorized))
            'scriptblock.Append(String.Format("var userID = {0}; ", UserInfo.UserID))
            'scriptblock.Append(String.Format("var userName = ""{0}""; ", UserInfo.Username))
            'scriptblock.Append(String.Format("var displayName = ""{0}""; ", UserInfo.DisplayName))
            'scriptblock.Append(String.Format("var email = ""{0}""; ", UserInfo.Email))
            '_scriptblock.Append(String.Format("var _clientID = {0}; ", _clientId))
            scriptblock.Append(String.Format("var editClientURL = ""{0}""; ", DotNetNuke.Common.Globals.NavigateURL(GetPageID("RIW People Manager", PortalId).TabID, "", "ctl=Edit&mid=" & GetModInfo("RIW People Manager", PortalId).ModuleID)))
            '_scriptblock.Append(String.Format("var _tModuleID = {0}; ", mCtrl.GetModuleByDefinition(PortalId, "Mini Estimate").TabModuleID))
            scriptblock.Append(String.Format("var productImagesURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products Manager", PortalId).TabID, "Images", "mid=" & CStr(GetModInfo("RIW Products Manager", PortalId).ModuleID))))
            scriptblock.Append(String.Format("var productFinanceURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products Manager", PortalId).TabID, "Finance", "mid=" & CStr(GetModInfo("RIW Products Manager", PortalId).ModuleID))))
            'scriptblock.Append(String.Format("var estimateSubject = ""{0}""; ", IIf(mSettings("RIW_EstimateEmailSubject") IsNot Nothing, CStr(mSettings("RIW_EstimateEmailSubject")), "")))
            'scriptblock.Append(String.Format("var estimateBody = ""{0}""; ", IIf(mSettings("RIW_EstimateEmailBody") IsNot Nothing, CStr(mSettings("RIW_EstimateEmailBody")), "")))
            'scriptblock.Append(String.Format("var estimateIntroMsg = ""{0}""; ", IIf(mSettings("RIW_EstimateIntroMsg") IsNot Nothing, CStr(mSettings("RIW_EstimateIntroMsg")), "")))
            'scriptblock.Append(String.Format("var visitorIntroMsg = ""{0}""; ", IIf(mSettings("RIW_VisitorIntroMsg") IsNot Nothing, CStr(mSettings("RIW_VisitorIntroMsg")), "")))
            scriptblock.Append(String.Format("var accountsManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Payments Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var productDescURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products Manager", PortalId).TabID, "Description", "mid=" & CStr(GetModInfo("RIW Products Manager", PortalId).ModuleID))))
            scriptblock.Append(String.Format("var productVideosURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products Manager", PortalId).TabID, "Videos", "mid=" & CStr(GetModInfo("RIW Products Manager", PortalId).ModuleID))))
            scriptblock.Append(String.Format("var categoriesURL = ""{0}""; ", NavigateURL(GetPageID("RIW Categories Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var editItemURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products Manager", PortalId).TabID, "Edit", "mid=" & CStr(GetModInfo("RIW Products Manager", PortalId).ModuleID))))
            scriptblock.Append(String.Format("var relatedProductsURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products Manager", PortalId).TabID, "Related", "mid=" & CStr(GetModInfo("RIW Products Manager", PortalId).ModuleID))))
            scriptblock.Append(String.Format("var productAttributesURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products Manager", PortalId).TabID, "Attributes", "mid=" & CStr(GetModInfo("RIW Products Manager", PortalId).ModuleID))))
            scriptblock.Append(String.Format("var productShippingURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products Manager", PortalId).TabID, "Shipping", "mid=" & CStr(GetModInfo("RIW Products Manager", PortalId).ModuleID))))
            scriptblock.Append(String.Format("var productSEOURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products Manager", PortalId).TabID, "SEO", "mid=" & CStr(GetModInfo("RIW Products Manager", PortalId).ModuleID))))
            scriptblock.Append(String.Format("var categoriesManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Categories Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var peopleManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW People Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var invoicesManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Invoices Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var productsManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var reportsManagerURL = ""{0}""; ", NavigateURL(GetPageID("Reports Viewer", PortalId).TabID)))
            scriptblock.Append(String.Format("var storeManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Store Admin Store", PortalId).TabID)))
            scriptblock.Append(String.Format("var usersManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Users Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var productsURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products", PortalId).TabID)))
            scriptblock.Append(String.Format("var estimateURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products", PortalId).TabID)))
            scriptblock.Append(String.Format("var agendaURL = ""{0}""; ", NavigateURL(GetPageID("RIW Agenda", PortalId).TabID)))
            scriptblock.Append(String.Format("var returnURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var providerRoleId = {0}; ", providerRoleId))
            scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(StrModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), StrModulePathScript, scriptblock.ToString())
            End If

            ' register javascript libraries
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/kendo.web.min.js", 20)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/cultures/kendo.culture.pt-BR.min.js", 21)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/messages/kendo.messages.pt-BR.min.js", 22)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-3.1.0.js", 23)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout.mapping-latest.js", 23)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-kendo.min.js", 24)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/modernizr.min.js", 25)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/moment-with-langs.min.js", 26)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/pnotify/jquery.pnotify.min.js", 27)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/browser-detection.js", 28)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/colorbox/jquery.colorbox-min.js", 29)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/colorbox/i18n/jquery.colorbox-pt-br.js", 30)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.scrollTo.min.js", 31)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.maskedinput.min.js", 32)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/amplify.min.js", 33)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxcore.js", 34)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxdropdownbutton.js", 35)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxtree.js", 36)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxmenu.js", 37)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxpanel.js", 38)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxscrollbar.js", 39)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxbuttons.js", 40)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/bootstrap.min.js", 42)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/bootstrap-switch.min.js", 43)
            'ClientResourceManager.RegisterScript(Parent.Page, "http://cdn.alloyui.com/2.0.0pr7/aui/aui-min.js", 44)aui/aui.js
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/alloyui/aui-min.js", 44)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.inputmask/jquery.inputmask.min.js", 45)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/knockout-bootstrap.min.js", 46)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.onscreen.min.js", 47)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/select2/select2.min.js", 48)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/select2/select2-locales/select2_locale_pt-BR.js", 49)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/tristate-checkbox.js", 50)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/showdown.js", 51)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.autogrow-textarea.js", 52)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.fileDownload.js", 52)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/app/utilities.js", 53)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/Markdown/Markdown.Converter.js", 52)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/Markdown/Markdown.Sanitizer.js", 53)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/Markdown/Markdown.Editor.js", 54)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/Markdown/jquery.markdown.js", 55)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/bootstrap-markdown.js", 53)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/markdown.js", 54)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/to-markdown.js", 55)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/showdown.js", 55)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/wysiwym/wysiwym.js", 52)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/shortcut.js", 61)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/products/scripts/App/data.js", 62)

            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.3.930/styles/kendo.common.min.css", 71)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.3.930/styles/kendo.metro.min.css", 72)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/browser-detection.css", 73)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/colorbox/colorbox.css", 74)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jquery.toastmessage-min.css", 75)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.css", 75)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.icons.css", 75)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jqwidgets/styles/jqx.base.css", 76)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2.css", 77)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2-bootstrap.css", 78)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap.min.css", 79)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-responsive.min.css", 80)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-theme.min.css", 81)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-switch.css", 82)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/font-awesome.min.css", 83)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/flat-ui-fonts.css", 84)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/wysiwym/wysiwym.css", 83)

        End Sub

#End Region

    End Class

End Namespace
