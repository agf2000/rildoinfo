Imports DotNetNuke.Web.Client.ClientResourceManagement
Imports RIW.Modules.Common.Utilities

Namespace Components

    Public Class RiwCategoriesModuleBase
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

            If Not CBool(UserId = -1) Then
                '    If HasModuleAccess(SecurityAccessLevel.Edit, "EXCLUDEPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "EDITPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "INSERTPERMISSION", ModuleConfiguration) Then
                '        intAuthorized = 3
                '    ElseIf Not HasModuleAccess(SecurityAccessLevel.Edit, "EXCLUDEPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "EDITPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "INSERTPERMISSION", ModuleConfiguration) Then
                '        intAuthorized = 2
                '    ElseIf Not HasModuleAccess(SecurityAccessLevel.Edit, "EXCLUDEPERMISSION", ModuleConfiguration) AndAlso Not HasModuleAccess(SecurityAccessLevel.Edit, "EDITPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "INSERTPERMISSION", ModuleConfiguration) Then
                '        intAuthorized = 1
                '    End If
                If UserInfo.IsInRole("Gerentes") Then
                    intAuthorized = 3
                ElseIf UserInfo.IsInRole("Vendedores") Then
                    intAuthorized = 2
                ElseIf UserInfo.IsInRole("Clientes") Then
                    intAuthorized = 1
                End If
            End If

            'Dim _modulepath = DotNetNuke.Common.Globals.DesktopModulePath
            Dim mCtrl As New DotNetNuke.Entities.Modules.ModuleController()
            Dim _moduleInfo = mCtrl.GetModule(ModuleId)
            'Dim _clientId = -1
            'Dim clientInfo = ristore_services.RIStore_Business_Controller.GetClientUser(UserInfo.UserID)
            'If Not Null.IsNull(clientInfo) Then
            '    _clientId = clientInfo.ClientId
            'End If
            'Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(PortalId)
            'Dim mCtrl As New DotNetNuke.Entities.Modules.ModuleController()
            'Dim mSettings = mCtrl.GetTabModuleSettings(mCtrl.GetModuleByDefinition(PortalId, "Mini Estimate").TabModuleID)

            Dim avatarPath = ""
            If UserInfo.Profile.Photo IsNot Nothing Then
                If UserInfo.Profile.Photo.Length > 2 Then
                    avatarPath = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(UserInfo).FolderPath
                    avatarPath = avatarPath & DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(CInt(UserInfo.Profile.Photo)).FileName
                End If
            End If

            Dim _scriptblock = New StringBuilder()
            _scriptblock.Append("<script>")
            _scriptblock.Append([String].Format("var _moduleID = {0}; ", ModuleId))
            _scriptblock.Append([String].Format("var _moduleTitle = ""{0}""; ", _moduleInfo.ModuleTitle))
            _scriptblock.Append([String].Format("var _portalID = {0}; ", PortalId))
            _scriptblock.Append([String].Format("var _authorized = {0}; ", intAuthorized))
            _scriptblock.Append([String].Format("var _userID = {0}; ", UserInfo.UserID))
            _scriptblock.Append([String].Format("var _userName = ""{0}""; ", UserInfo.Username))
            _scriptblock.Append([String].Format("var _displayName = ""{0}""; ", UserInfo.DisplayName))
            _scriptblock.Append([String].Format("var _email = ""{0}""; ", UserInfo.Email))
            _scriptblock.Append([String].Format("var _avatar = ""{0}""; ", avatarPath))
            _scriptblock.Append([String].Format("var _siteName = ""{0}""; ", PortalSettings.PortalName))
            _scriptblock.Append([String].Format("var _siteURL = ""{0}""; ", PortalSettings.PortalAlias.HTTPAlias))
            _scriptblock.Append([String].Format("var _editItemURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products Manager", PortalId).TabID, "Edit", "mid=" & CStr(GetModInfo("RIW Products Manager", PortalId).ModuleID))))
            _scriptblock.Append([String].Format("var _accountsManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Payments Manager", PortalId).TabID)))
            _scriptblock.Append([String].Format("var _agendaURL = ""{0}""; ", NavigateURL(GetPageID("RIW Agenda", PortalId).TabID)))
            _scriptblock.Append([String].Format("var _categoriesManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Categories Manager", PortalId).TabID)))
            _scriptblock.Append([String].Format("var _peopleManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW People Manager", PortalId).TabID)))
            _scriptblock.Append([String].Format("var _invoicesManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Invoices Manager", PortalId).TabID)))
            _scriptblock.Append([String].Format("var _productsManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Products Manager", PortalId).TabID)))
            _scriptblock.Append([String].Format("var _reportsManagerURL = ""{0}""; ", NavigateURL(GetPageID("Reports Viewer", PortalId).TabID)))
            _scriptblock.Append([String].Format("var _storeManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Store Admin Store", PortalId).TabID)))
            _scriptblock.Append([String].Format("var _usersManagerURL = ""{0}""; ", NavigateURL(GetPageID("RIW Users Manager", PortalId).TabID)))
            _scriptblock.Append([String].Format("var _orURL = ""{0}""; ", NavigateURL(GetPageID("RIW Quick Estimate", PortalId).TabID)))
            _scriptblock.Append("</script>")

            ' register scripts
            If Not Page.ClientScript.IsClientScriptBlockRegistered(StrModulePathScript) Then
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), StrModulePathScript, _scriptblock.ToString())
            End If

            ' register javascript libraries
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/kendo.web.min.js", 20)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/cultures/kendo.culture.pt-BR.min.js", 21)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/messages/kendo.messages.pt-BR.min.js", 21)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-3.1.0.js", 22)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout.mapping-latest.js", 23)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-kendo.min.js", 24)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/shortcut.js", 25)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/modernizr.min.js", 26)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/browser-detection.js", 27)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/colorbox/jquery.colorbox-min.js", 28)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.toastmessage-min.js", 29)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/pnotify/jquery.pnotify.min.js", 29)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.scrollTo.min.js", 30)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/moment-with-langs.min.js", 31)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/amplify.min.js", 32)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxcore.js", 33)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxdropdownbutton.js", 34)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxtree.js", 35)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxmenu.js", 36)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxpanel.js", 37)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxscrollbar.js", 38)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jqwidgets/jqxbuttons.js", 39)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/knockout-bootstrap.min.js", 40)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.onscreen.min.js", 41)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/select2/select2.min.js", 42)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/tristate-checkbox.js", 43)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/bootstrap.min.js", 44)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/bootstrap-switch.min.js", 45)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/to-markdown.js", 46)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.autogrow-textarea.js", 47)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/showdown.js", 48)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo.web.ext.js", 47)
            'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/bootstrap-acknowledgeinput.min.js", 48)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/app/utilities.js", 51)
            ClientResourceManager.RegisterScript(Parent.Page, "/desktopModules/rildoinfo/" & Me.ModuleConfiguration.DesktopModule.FolderName & "/viewmodels/viewModel.js", 52)

            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.3.930/styles/kendo.common.min.css", 71)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.3.930/styles/kendo.default.min.css", 72)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/browser-detection.css", 73)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/colorbox/colorbox.css", 74)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jquery.toastmessage-min.css", 75)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.css", 75)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.icons.css", 76)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jqwidgets/styles/jqx.base.css", 77)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/select2-min_3.4.3.css", 78)
            'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2-bootstrap.css", 79)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap.min.css", 80)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-responsive.min.css", 81)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-theme.min.css", 82)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-switch.css", 83)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/flat-ui-fonts.css", 84)
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/font-awesome.min.css", 89)

        End Sub

        #End Region

    End Class

End Namespace
