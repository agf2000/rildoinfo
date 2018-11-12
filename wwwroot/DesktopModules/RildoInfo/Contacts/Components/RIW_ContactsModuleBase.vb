' Copyright (c) 2013 AGF
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 

Imports DotNetNuke.Web.Client.ClientResourceManagement

' <summary>
' This base class can be used to define custom properties for multiple controls. 
' An example module, DNNSimpleArticle (http://dnnsimplearticle.codeplex.com) uses this for an ArticleId
' 
' Because the class inherits from PortalModuleBase, properties like ModuleId, TabId, UserId, and others, 
' are accessible to your module's controls (that inherity from RIW_ContactsModuleBase
' 
' </summary>

Public Class RIW_ContactsModuleBase
    Inherits PortalModuleBase

	#Region "Private Members"

    Private Const STR_ModulePathScript As String = "modulePathScript"

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

        Dim _modulepath = DotNetNuke.Common.Globals.DesktopModulePath
        'Dim _clientId = -1
        'Dim clientInfo = ristore_services.RIStore_Business_Controller.GetClientUser(UserInfo.UserID)
        'If Not Null.IsNull(clientInfo) Then
        '    _clientId = clientInfo.ClientId
        'End If
        Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(PortalId)
        Dim mCtrl As New DotNetNuke.Entities.Modules.ModuleController()
        Dim mSettings = mCtrl.GetTabModuleSettings(mCtrl.GetModuleByDefinition(PortalId, "RIW Contacts").TabModuleID)

        Dim _scriptblock = New StringBuilder()
        _scriptblock.Append("<script>")
        _scriptblock.Append([String].Format("var _siteName = '{0}'; ", PortalSettings.PortalName))
        _scriptblock.Append([String].Format("var _siteURL = '{0}'; ", PortalSettings.PortalAlias.HTTPAlias))
        _scriptblock.Append([String].Format("var _logoURL = '{0}'; ", PortalSettings.HomeDirectory & PortalSettings.LogoFile))
        _scriptblock.Append([String].Format("var _storeStreet = '{0}'; ", myPortalSettings("RIW_StoreAddress")))
        _scriptblock.Append([String].Format("var _storeUnit = '{0}'; ", myPortalSettings("RIW_StoreUnit")))
        _scriptblock.Append([String].Format("var _storeComplement = '{0}'; ", myPortalSettings("RIW_StoreComplement")))
        _scriptblock.Append([String].Format("var _storeDistrict = '{0}'; ", myPortalSettings("RIW_StoreDistrict")))
        _scriptblock.Append([String].Format("var _storeCity = '{0}'; ", myPortalSettings("RIW_StoreCity")))
        _scriptblock.Append([String].Format("var _storePostalCode = '{0}'; ", myPortalSettings("RIW_StorePostalCode")))
        _scriptblock.Append([String].Format("var _storeRegion = '{0}'; ", myPortalSettings("RIW_StoreRegion")))
        _scriptblock.Append([String].Format("var _storeCountry = '{0}'; ", myPortalSettings("RIW_StoreCountry")))
        _scriptblock.Append([String].Format("var _storeEmail = '{0}'; ", myPortalSettings("RIW_StoreEmail")))
        _scriptblock.Append([String].Format("var _storePhone = '{0}'; ", myPortalSettings("RIW_StorePhones").ToLower()))
        _scriptblock.Append([String].Format("var _modulePath = '{0}'; ", _modulepath))
        _scriptblock.Append([String].Format("var _returnURL = '{0}'; ", NavigateURL()))
        _scriptblock.Append([String].Format("var _optionsURL = '{0}'; ", EditUrl("Manage")))
        _scriptblock.Append([String].Format("var _portalID = {0};", PortalId))
        _scriptblock.Append([String].Format("var _authorized = {0};", intAuthorized))
        _scriptblock.Append([String].Format("var _userID = {0};", UserInfo.UserID))
        _scriptblock.Append([String].Format("var _displayName = '{0}'; ", UserInfo.DisplayName))
        _scriptblock.Append([String].Format("var _sendTo = '{0}';", IIf(mSettings("RIW_Contacts_SendTo") IsNot Nothing, mSettings("RIW_Contacts_SendTo"), "")))
        _scriptblock.Append([String].Format("var _postOfficeMessage = '{0}';", IIf(mSettings("RIW_Contacts_poMessage") IsNot Nothing, mSettings("RIW_Contacts_poMessage"), "")))
        _scriptblock.Append([String].Format("var _emailMessage = '{0}';", IIf(mSettings("RIW_Contacts_emailMessage") IsNot Nothing, mSettings("RIW_Contacts_emailMessage"), "")))
        _scriptblock.Append([String].Format("var _autoAnswer = '{0}';", IIf(mSettings("RIW_Contacts_autoAnser") IsNot Nothing, mSettings("RIW_Contacts_autoAnser"), "")))
        _scriptblock.Append([String].Format("var _reqMethod = '{0}';", IIf(mSettings("RIW_Contacts_reqMethod") IsNot Nothing, mSettings("RIW_Contacts_reqMethod"), "")))
        _scriptblock.Append([String].Format("var _reqAddress = '{0}';", IIf(mSettings("RIW_Contacts_reqAddress") IsNot Nothing, mSettings("RIW_Contacts_reqAddress"), "")))
        _scriptblock.Append([String].Format("var _smtpServer = '{0}';", IIf(mSettings("RIW_Contacts_smtpServer") IsNot Nothing, mSettings("RIW_Contacts_smtpServer"), "")))
        _scriptblock.Append([String].Format("var _smtpPort = '{0}';", IIf(mSettings("RIW_Contacts_smtpPort") IsNot Nothing, mSettings("RIW_Contacts_smtpPort"), "")))
        _scriptblock.Append([String].Format("var _smtpLogin = '{0}';", IIf(mSettings("RIW_Contacts_smtpLogin") IsNot Nothing, mSettings("RIW_Contacts_smtpLogin"), "")))
        _scriptblock.Append([String].Format("var _smtpPassword = '{0}';", IIf(mSettings("RIW_Contacts_smtpPassword") IsNot Nothing, mSettings("RIW_Contacts_smtpPassword"), "")))
        _scriptblock.Append([String].Format("var _smtpConnection = '{0}';", IIf(mSettings("RIW_Contacts_smtpConnection") IsNot Nothing, mSettings("RIW_Contacts_smtpConnection"), "")))
        _scriptblock.Append("</script>")

        ' register scripts
        If Not Page.ClientScript.IsClientScriptBlockRegistered(STR_ModulePathScript) Then
            Page.ClientScript.RegisterClientScriptBlock(GetType(Page), STR_ModulePathScript, _scriptblock.ToString())
        End If

        ' register javascript libraries
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/knockout/knockout-2.3.0.js", 20)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/knockout/knockout.mapping-latest.js", 21)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/kendo/2013.2.918/kendo.all.min.js", 22)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/kendo/2013.2.918/cultures/kendo.culture.pt-BR.min.js", 23)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/knockout/knockout-kendo.min.js", 24)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/modernizr-2.6.2.js", 25)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/browser-detection.js", 26)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/select2/select2.min.js", 27)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jquery.maskedinput.min.js", 28)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/pnotify/jquery.pnotify.min.js", 29)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/jquery.scrollTo.min.js", 30)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/amplify.min.js", 31)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/aui/aui/aui-min.js", 32)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/bootstrap/bootstrap.min.js", 33)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/bootstrap/knockout-bootstrap.min.js", 34)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/bootstrap/bootstrap-switch.min.js", 35)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jquery.inputmask/jquery.inputmask.min.js", 36)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/moment-with-langs.min.js", 37)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/app/utilities.js", 38)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/jquery.autogrow-textarea.js", 39)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/markdown/showdown.js", 40)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/markdown/to-markdown.js", 41)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/contacts/Scripts/App/data.js", 42)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/contacts/viewmodels/viewmodel.js", 43)

        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/kendo/2013.2.918/kendo.common.min.css", 71)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/kendo/2013.2.918/kendo.default.min.css", 72)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/browser-detection.css", 73)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/content/pnotify/jquery.pnotify.default.css", 75)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/content/pnotify/jquery.pnotify.default.icons.css", 76)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/content/select2/select2-min.css", 77)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/content/select2/select2-bootstrap.css", 78)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/bootstrap/css/bootstrap.min.css", 79)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/bootstrap/css/bootstrap-responsive.min.css", 80)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/bootstrap/css/bootstrap-theme.min.css", 81)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/bootstrap/css/bootstrap-switch.css", 82)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/bootstrap/css/flat-ui-fonts.css", 83)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/content/bootstrap/css/font-awesome.min.css", 84)

    End Sub

#End Region

End Class
