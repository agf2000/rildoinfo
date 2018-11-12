' Copyright (c) 2013 Christoc.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARIWING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 

Imports System.Web.UI
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Security.Permissions


' <summary>
' This base class can be used to define custom properties for multiple controls. 
' An example module, DNNSimpleArticle (http://dnnsimplearticle.codeplex.com) uses this for an ArticleId
' 
' Because the class inherits from PortalModuleBase, properties like ModuleId, TabId, UserId, and others, 
' are accessible to your module's controls (that inherity from ContactFormModuleBase
' 
' </summary>

Public Class ContactFormModuleBase
    Inherits PortalModuleBase

    Private Const STR_ModulePathScript As String = "modulePathScript"

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        jQuery.RequestDnnPluginsRegistration()
        RegisterModulePath()
    End Sub

    Public Sub RegisterModulePath()

        Dim intAuthorized As Integer = -1

        'Dim myPermissionCollection = ModuleConfiguration.ModulePermissions

        If Not CBool(UserId = -1) Then
            'If Not ModulePermissionController.HasModulePermission(myPermissionCollection, "View") Then
            '    Response.Redirect(NavigateURL(PortalSettings.HomeTabId))
            'End If
            '    If HasModuleAccess(SecurityAccessLevel.Edit, "EXCLUDEPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "EDITPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "INSERTPERMISSION", ModuleConfiguration) Then
            '        intAuthorized = 3
            '    ElseIf Not HasModuleAccess(SecurityAccessLevel.Edit, "EXCLUDEPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "EDITPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "INSERTPERMISSION", ModuleConfiguration) Then
            '        intAuthorized = 2
            '    ElseIf Not HasModuleAccess(SecurityAccessLevel.Edit, "EXCLUDEPERMISSION", ModuleConfiguration) AndAlso Not HasModuleAccess(SecurityAccessLevel.Edit, "EDITPERMISSION", ModuleConfiguration) AndAlso HasModuleAccess(SecurityAccessLevel.Edit, "INSERTPERMISSION", ModuleConfiguration) Then
            '        intAuthorized = 1
            '    End If
            If UserInfo.IsInRole("Gerentes") Then
                intAuthorized = 3
            ElseIf UserInfo.IsInRole("Editores") Then
                intAuthorized = 2
            ElseIf UserInfo.IsInRole("Clientes") Then
                intAuthorized = 1
            End If
        End If

        Dim _modulepath = DotNetNuke.Common.Globals.DesktopModulePath
        Dim mCtrl As New ModuleController()
        Dim mSettings = mCtrl.GetTabModuleSettings(mCtrl.GetModuleByDefinition(PortalId, "RIW Contact Form").TabModuleID)

        Dim settingCtrl As New WebAPI.Components.Repositories.SettingsRepository
        Dim settingsDictionay = settingCtrl.GetSettings(PortalId)

        Dim _scriptblock = New StringBuilder()
        _scriptblock.Append("<script>")
        _scriptblock.Append([String].Format("var _siteName = '{0}'; ", PortalSettings.PortalName))
        _scriptblock.Append([String].Format("var _siteURL = '{0}'; ", PortalSettings.PortalAlias.HTTPAlias))
        _scriptblock.Append([String].Format("var _logoURL = '{0}'; ", PortalSettings.HomeDirectory & PortalSettings.LogoFile))
        _scriptblock.Append([String].Format("var _storeStreet = '{0}'; ", settingsDictionay("storeAddress")))
        _scriptblock.Append([String].Format("var _storeUnit = '{0}'; ", settingsDictionay("storeUnit")))
        _scriptblock.Append([String].Format("var _storeComplement = '{0}'; ", settingsDictionay("storeComplement")))
        _scriptblock.Append([String].Format("var _storeDistrict = '{0}'; ", settingsDictionay("storeDistrict")))
        _scriptblock.Append([String].Format("var _storeCity = '{0}'; ", settingsDictionay("storeCity")))
        _scriptblock.Append([String].Format("var _storePostalCode = '{0}'; ", settingsDictionay("storePostalCode")))
        _scriptblock.Append([String].Format("var _storeRegion = '{0}'; ", settingsDictionay("storeRegion")))
        _scriptblock.Append([String].Format("var _storeCountry = '{0}'; ", settingsDictionay("storeCountry")))
        _scriptblock.Append([String].Format("var _storeEmail = '{0}'; ", settingsDictionay("storeEmail")))
        _scriptblock.Append([String].Format("var _storePhone1 = '{0}'; ", settingsDictionay("storePhone1").ToLower()))
        _scriptblock.Append([String].Format("var _storePhone2 = '{0}'; ", settingsDictionay("storePhone2").ToLower()))
        _scriptblock.Append([String].Format("var _modulePath = '{0}'; ", _modulepath))
        _scriptblock.Append([String].Format("var _returnURL = '{0}'; ", NavigateURL()))
        _scriptblock.Append([String].Format("var _optionsURL = '{0}'; ", EditUrl()))
        _scriptblock.Append([String].Format("var _portalID = {0}; ", PortalId))
        _scriptblock.Append([String].Format("var _authorized = {0}; ", intAuthorized))
        _scriptblock.Append([String].Format("var _userID = {0}; ", UserInfo.UserID))
        _scriptblock.Append([String].Format("var _company = '{0}'; ", UserInfo.Profile.GetPropertyValue("Company")))
        _scriptblock.Append([String].Format("var _displayName = '{0}'; ", UserInfo.DisplayName))
        _scriptblock.Append([String].Format("var _telephone = '{0}'; ", UserInfo.Profile.Telephone))
        _scriptblock.Append([String].Format("var _email = '{0}'; ", UserInfo.Email))
        _scriptblock.Append([String].Format("var _website = '{0}'; ", UserInfo.Profile.Website))
        _scriptblock.Append([String].Format("var _postalCode = '{0}'; ", UserInfo.Profile.PostalCode))
        _scriptblock.Append([String].Format("var _street = '{0}'; ", UserInfo.Profile.Street))
        _scriptblock.Append([String].Format("var _unit = '{0}'; ", UserInfo.Profile.Unit))
        _scriptblock.Append([String].Format("var _complement = '{0}'; ", UserInfo.Profile.GetPropertyValue("Suffix")))
        _scriptblock.Append([String].Format("var _district = '{0}'; ", UserInfo.Profile.IM))
        _scriptblock.Append([String].Format("var _city = '{0}'; ", UserInfo.Profile.City))
        _scriptblock.Append([String].Format("var _region = '{0}'; ", UserInfo.Profile.Region))
        _scriptblock.Append([String].Format("var _moduleID = {0}; ", mCtrl.GetModuleByDefinition(PortalId, "RIW Contact Form").ModuleID))
        _scriptblock.Append([String].Format("var _tModuleID = {0}; ", mCtrl.GetModuleByDefinition(PortalId, "RIW Contact Form").TabModuleID))
        _scriptblock.Append([String].Format("var _sendTo = '{0}'; ", CStr(mSettings("RIW_ContactForm_SendTo"))))
        _scriptblock.Append([String].Format("var _postOfficeMessage = '{0}'; ", CStr(mSettings("RIW_ContactForm_poMessage"))))
        _scriptblock.Append([String].Format("var _emailMessage = '{0}'; ", CStr(mSettings("RIW_ContactForm_emailMessage"))))
        _scriptblock.Append([String].Format("var _autoAnswer = '{0}'; ", CStr(mSettings("RIW_ContactForm_autoAnswer"))))
        _scriptblock.Append([String].Format("var _reqTelephone = '{0}'; ", IIf(mSettings("RIW_ContactForm_reqTelephone") IsNot Nothing, CStr(mSettings("RIW_ContactForm_reqTelephone")), "false")))
        _scriptblock.Append([String].Format("var _reqSend = '{0}'; ", IIf(mSettings("RIW_ContactForm_reqSend") IsNot Nothing, CStr(mSettings("RIW_ContactForm_reqSend")), "false")))
        _scriptblock.Append([String].Format("var _reqAddress = '{0}'; ", IIf(mSettings("RIW_ContactForm_reqAddress") IsNot Nothing, CStr(mSettings("RIW_ContactForm_reqAddress")), "false")))
        _scriptblock.Append([String].Format("var _smtpServer = '{0}'; ", CStr(mSettings("RIW_ContactForm_smtpServer"))))
        _scriptblock.Append([String].Format("var _smtpPort = '{0}'; ", CStr(mSettings("RIW_ContactForm_smtpPort"))))
        _scriptblock.Append([String].Format("var _smtpLogin = '{0}'; ", CStr(mSettings("RIW_ContactForm_smtpLogin"))))
        _scriptblock.Append([String].Format("var _smtpPassword = '{0}'; ", CStr(mSettings("RIW_ContactForm_smtpPassword"))))
        _scriptblock.Append([String].Format("var _smtpConnection = '{0}'; ", IIf(mSettings("RIW_ContactForm_smtpConnection") IsNot Nothing, CStr(mSettings("RIW_ContactForm_smtpConnection")), "false")))
        _scriptblock.Append("</script>")

        ' register scripts
        If Not Page.ClientScript.IsClientScriptBlockRegistered(STR_ModulePathScript) Then
            Page.ClientScript.RegisterClientScriptBlock(GetType(Page), STR_ModulePathScript, _scriptblock.ToString())
        End If

        ' register javascript libraries
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-3.1.0.js", 20)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout.mapping-latest.js", 21)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/kendo.web.min.js", 22)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/cultures/kendo.culture.pt-BR.min.js", 23)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2015.3.930/messages/kendo.messages.pt-BR.min.js", 24)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-kendo.min.js", 25)
        'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/modernizr.min.js", 26)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/browser-detection.js", 27)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/select2/select2.min.js", 28)
        'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.maskedinput.min.js", 28)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/pnotify/jquery.pnotify.min.js", 29)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.scrollTo.min.js", 30)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/amplify.min.js", 31)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/alloyui/aui-min.js", 32)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/bootstrap.min.js", 33)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/knockout-bootstrap.min.js", 34)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/bootstrap/bootstrap-switch.min.js", 35)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.inputmask/jquery.inputmask.min.js", 36)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/moment-with-langs.min.js", 37)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.autogrow-textarea.js", 38)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/showdown.js", 39)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/markdown/to-markdown.js", 40)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/app/utilities.js", 41)
        ClientResourceManager.RegisterScript(Parent.Page, String.Format("/desktopmodules/rildoinfo/{0}/viewmodels/viewModel.js", Me.ModuleConfiguration.DesktopModule.FolderName), 42)

        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.3.930/styles/kendo.common.min.css", 71)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2015.3.930/styles/kendo.default.min.css", 72)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/browser-detection.css", 73)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.css", 75)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/pnotify/jquery.pnotify.default.icons.css", 76)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2.css", 77)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/select2/css/select2-bootstrap.css", 78)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap.min.css", 79)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-responsive.min.css", 80)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-theme.min.css", 81)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/bootstrap-switch.css", 82)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/flat-ui-fonts.css", 83)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/font-awesome.min.css", 84)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/bootstrap/css/fam-icons.css", 84)

    End Sub

End Class
