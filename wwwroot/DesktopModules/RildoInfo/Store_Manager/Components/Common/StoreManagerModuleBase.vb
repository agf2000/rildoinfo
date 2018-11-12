
Imports DotNetNuke.Web.Client.ClientResourceManagement
'Imports DotNetNuke.Security.Permissions.ModulePermissionController
'Imports DotNetNuke.Common.Utilities
Imports RIW.Modules.Common.Utilities

Namespace Components.Common

    Public Class StoreManagerModuleBase
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

            'Dim modulepath = DesktopModulePath
            'Dim clientId = -1
            'Dim clientInfo = RIStoreDataServices.GetClientUser(UserInfo.UserID)
            'If Not Null.IsNull(clientInfo) Then
            '    clientId = clientInfo.ClientId
            'End If

            'Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(PortalId)
            Dim mCtrl As New DotNetNuke.Entities.Modules.ModuleController()
            Dim mSettings = mCtrl.GetTabModuleSettings(mCtrl.GetModuleByDefinition(PortalId, "RIW Estimates Manager").TabModuleID)
            Dim moduleInfo = mCtrl.GetModule(ModuleId)

            Dim roleCtrl As New DotNetNuke.Security.Roles.RoleController
            Dim clientRoleId = roleCtrl.GetRoleByName(PortalId, "Clientes").RoleID
            Dim providerRoleId = roleCtrl.GetRoleByName(PortalId, "Fornecedores").RoleID

            Dim scriptblock = New StringBuilder()
            scriptblock.Append("<script type='text/javascript' id='riw_store_manager'>")
            'scriptblock.Append(String.Format("var siteName = ""{0}""; ", PortalSettings.PortalName))
            'scriptblock.Append(String.Format("var siteURL = ""{0}"";", PortalSettings.PortalAlias.HTTPAlias))
            scriptblock.Append(String.Format("var tModuleID = ""{0}""; ", TabModuleId))
            scriptblock.Append(String.Format("var moduleID = ""{0}""; ", ModuleId))
            scriptblock.Append(String.Format("var moduleTitle = ""{0}""; ", moduleInfo.ModuleTitle))
            scriptblock.Append(String.Format("var managerRole = ""{0}""; ", UserInfo.IsInRole("Gerentes")))
            scriptblock.Append(String.Format("var configURL = ""{0}""; ", EditUrl("Options")))
            scriptblock.Append(String.Format("var editInvoiceURL = ""{0}""; ", NavigateURL(GetPageID("RIW Edit Invoice", PortalId).TabID)))
            scriptblock.Append(String.Format("var editPaymentURL = ""{0}""; ", NavigateURL(GetPageID("RIW Edit Payment", PortalId).TabID)))
            scriptblock.Append(String.Format("var clientURL = ""{0}""; ", NavigateURL(GetPageID("RIW People Manager", PortalId).TabID, "Edit", "mid=" & CStr(GetModInfo("RIW People Manager", PortalId).ModuleID))))
            scriptblock.Append(String.Format("var calendarURL = ""{0}""; ", NavigateURL(GetPageID("RIW Payments Manager", PortalId).TabID, "Calendar", "mid=" & CStr(GetModInfo("RIW Payments Manager", PortalId).ModuleID))))
            scriptblock.Append(String.Format("var editItemURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products Manager", PortalId).TabID, "Edit", "mid=" & CStr(GetModInfo("RIW Products Manager", PortalId).ModuleID))))
            'scriptblock.Append(String.Format("var detailURL = ""{0}""; ", NavigateURL(GetPageID("Store Product Detail", PortalId).TabID)))
            scriptblock.Append(String.Format("var productsURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products", PortalId).TabID)))
            'scriptblock.Append(String.Format("var returnURL = ""{0}""; ", NavigateURL()))
            scriptblock.Append(String.Format("var paymentsURL = ""{0}""; ", NavigateURL(GetPageID("RIW Payments Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var estimateURL = ""{0}""; ", NavigateURL(GetPageID("RIW Estimates Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var clientRoleId = {0}; ", clientRoleId))
            scriptblock.Append(String.Format("var providerRoleId = {0}; ", providerRoleId))
            'scriptblock.Append(String.Format("var authorized = {0}; ", intAuthorized))
            'scriptblock.Append(String.Format("var clientEmail = ""{0}""; ", UserInfo.Email))
            'scriptblock.Append(String.Format("var clientID = {0}; ", clientId))
            scriptblock.Append(String.Format("var accountsManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Payments Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var agendaURL = ""{0}""; ", NavigateURL(GetPageID("RIW Agenda", PortalId).TabID)))
            scriptblock.Append(String.Format("var categoriesManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Categories Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var peopleManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW People Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var invoicesManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Invoices Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var cashiersManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Cashiers Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var productsManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var reportsManagerURL = ""{0}""; ", NavigateURL(GetPageID("Reports Viewer", PortalId).TabID)))
            scriptblock.Append(String.Format("var storeManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Store Admin Store", PortalId).TabID)))
            scriptblock.Append(String.Format("var usersManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Users Manager", PortalId).TabID)))
            scriptblock.Append(String.Format("var orURL = ""{0}""; ", NavigateURL(GetPageID("RIW Quick Estimate", PortalId).TabID)))
            scriptblock.Append(String.Format("var estimateSubject = ""{0}""; ", IIf(mSettings("RIW_EstimateEmailSubject") IsNot Nothing, DecodeAccentletters(CStr(mSettings("RIW_EstimateEmailSubject"))), "")))
            scriptblock.Append(String.Format("var estimateBody = ""{0}""; ", IIf(mSettings("RIW_EstimateEmailBody") IsNot Nothing, DecodeAccentletters(CStr(mSettings("RIW_EstimateEmailBody"))), "")))
            scriptblock.Append(String.Format("var estimateIntroMsg = ""{0}""; ", IIf(mSettings("RIW_EstimateIntroMsg") IsNot Nothing, DecodeAccentletters(CStr(mSettings("RIW_EstimateIntroMsg"))), "")))
            scriptblock.Append(String.Format("var visitorIntroMsg = ""{0}""; ", IIf(mSettings("RIW_VisitorIntroMsg") IsNot Nothing, DecodeAccentletters(CStr(mSettings("RIW_VisitorIntroMsg"))), "")))
            scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(StrModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), StrModulePathScript, scriptblock.ToString())
            End If

            ' register javascript libraries
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/kendo/2013.2.716/kendo.all.min.js", 50)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/kendo/2013.2.716/cultures/kendo.culture.pt-BR.min.js", 51)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/knockout-2.2.1.js", 52)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/knockout.mapping-latest.js", 53)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/knockout-kendo.min.js", 54)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/shortcut.js", 55)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/modernizr-2.6.2.js", 56)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/browser-detection.js", 57)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/jquery.colorbox-min.js", 58)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/jquery.toastmessage.js", 59)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/scripts/jquery.scrollTo.min.js", 60)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/json2.js", 61)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/jquery.maskedinput.min.js", 62)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/amplify.min.js", 63)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/jqxcore.js", 64)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/jqxdropdownbutton.js", 65)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/jqxtree.js", 66)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/jqxmenu.js", 67)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/jqxpanel.js", 68)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/jqxscrollbar.js", 69)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/jqxbuttons.js", 70)
            ''ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/jquery.inputmask/jquery.inputmask.date.extensions.min.js", 71)
            ''ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/jquery.inputmask/jquery.inputmask.extensions.min.js", 72)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/jquery.inputmask/jquery.inputmask.min.js", 73)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/jquery.onscreen.min.js", 74)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/select2/select2.min.js", 75)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/Scripts/jquery.tooltipster.min.js", 76)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristoredataservices/scripts/app/utilities.js", 77)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-3.1.0.js", 20)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout.mapping-latest.js", 23)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/kendo.web.min.js", 22)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/cultures/kendo.culture.pt-BR.min.js", 23)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/messages/kendo.messages.pt-BR.min.js", 24)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-kendo.min.js", 25)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.flexslider-min.js", 25)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/modernizr.min.js", 26)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/browser-detection.js", 27)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/colorbox/jquery.colorbox-min.js", 28)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/colorbox/i18n/jquery.colorbox-pt-br.js", 29)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/amplify.min.js", 30)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.maskedinput.min.js", 30)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/pnotify/jquery.pnotify.min.js", 32)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/showdown.js", 33)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.scrollTo.min.js", 34)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/moment-with-langs.min.js", 35)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/amplify.min.js", 36)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxcore.js", 37)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxdropdownbutton.js", 38)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxtree.js", 39)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxmenu.js", 40)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxpanel.js", 41)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxscrollbar.js", 42)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxbuttons.js", 43)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/bootstrap.min.js", 44)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/bootstrap-switch.min.js", 44)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.carouFredSel-6.2.1-packed.js", 45)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.inputmask/jquery.inputmask.min.js", 46)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.onscreen.min.js", 47)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/select2/select2.min.js", 48)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.autogrow-textarea.js", 49)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/showdown.js", 50)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/to-markdown.js", 51)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/app/utilities.js", 52)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/store_manager/scripts/App/data.js", 78)
            ClientResourceManager.RegisterScript(Parent.Page, String.Format("/desktopmodules/rildoinfo/{0}/viewmodels/viewModel.js", Me.ModuleConfiguration.DesktopModule.FolderName), 53)

            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.3.930/styles/kendo.common.min.css", 71)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.3.930/styles/kendo.bootstrap.min.css", 72)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/browser-detection.css", 73)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/colorbox/colorbox.css", 74)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jquery.toastmessage-min.css", 75)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.css", 76)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.icons.css", 77)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jqwidgets/styles/jqx.base.css", 78)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap3/css/bootstrap.min.css", 79)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap3/css/bootstrap-theme.min.css", 81)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2.css", 78)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2-bootstrap.css", 79)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap.min.css", 80)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-responsive.min.css", 81)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-theme.min.css", 82)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-switch.min.css", 83)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/font-awesome.min.css", 84)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/flat-ui-fonts.css", 85)

        End Sub

    End Class

End Namespace
