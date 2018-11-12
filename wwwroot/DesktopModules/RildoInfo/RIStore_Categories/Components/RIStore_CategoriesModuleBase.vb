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
' are accessible to your module's controls (that inherity from RIStore_CategoriesModuleBase
' 
' </summary>

Public Class RIStore_CategoriesModuleBase
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

        'Dim _modulepath = DotNetNuke.Common.Globals.DesktopModulePath
        'Dim _clientId = -1
        'Dim clientInfo = ristore_services.RIStore_Business_Controller.GetClientUser(UserInfo.UserID)
        'If Not Null.IsNull(clientInfo) Then
        '    _clientId = clientInfo.ClientId
        'End If
        'Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(PortalId)
        'Dim mCtrl As New DotNetNuke.Entities.Modules.ModuleController()
        'Dim mSettings = mCtrl.GetTabModuleSettings(mCtrl.GetModuleByDefinition(PortalId, "Mini Estimate").TabModuleID)

        Dim _scriptblock = New StringBuilder()
        _scriptblock.Append("<script>")
        _scriptblock.Append([String].Format("var _moduleID = {0}; ", ModuleId))
        _scriptblock.Append([String].Format("var _portalID = {0}; ", PortalId))
        _scriptblock.Append([String].Format("var _authorized = {0}; ", intAuthorized))
        _scriptblock.Append([String].Format("var _userID = {0}; ", UserInfo.UserID))
        _scriptblock.Append([String].Format("var _userName = '{0}'; ", UserInfo.Username))
        _scriptblock.Append([String].Format("var _siteName = '{0}'; ", PortalSettings.PortalName))
        _scriptblock.Append([String].Format("var _siteURL = '{0}'; ", PortalSettings.PortalAlias.HTTPAlias))
        _scriptblock.Append("</script>")

        ' register scripts
        If Not Page.ClientScript.IsClientScriptBlockRegistered(STR_ModulePathScript) Then
            Page.ClientScript.RegisterClientScriptBlock(GetType(Page), STR_ModulePathScript, _scriptblock.ToString())
        End If

        ' register javascript libraries
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/kendo/2013.2.716/kendo.all.min.js", 50)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/kendo/2013.2.716/cultures/kendo.culture.pt-BR.min.js", 51)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/knockout-2.3.0.js", 52)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/knockout.mapping-latest.js", 53)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/knockout-kendo.min.js", 54)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/shortcut.js", 55)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/modernizr-2.6.2.js", 56)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/browser-detection.js", 57)
        'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jquery.colorbox-min.js", 58)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jquery.toastmessage.js", 59)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/jquery.scrollTo.min.js", 60)
        'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jquery.maskedinput.min.js", 62)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/amplify.min.js", 63)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jqwidgets/jqxcore.js", 64)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jqwidgets/jqxdropdownbutton.js", 65)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jqwidgets/jqxtree.js", 66)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jqwidgets/jqxmenu.js", 67)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jqwidgets/jqxpanel.js", 68)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jqwidgets/jqxscrollbar.js", 69)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jqwidgets/jqxbuttons.js", 70)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/bootstrap/knockout-bootstrap.min.js", 71)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jquery.onscreen.min.js", 72)
        'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/select2.min.js", 73)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/tristate-checkbox.js", 74)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/app/utilities.js", 75)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/bootstrap/bootstrap.min.js", 76)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/bootstrap/bootstrap-switch.min.js", 77)
        'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/kendo.web.ext.js", 78)
        'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/bootstrap/bootstrap-acknowledgeinput.min.js", 78)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristore_categories/Scripts/App/data.js", 79)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristore_categories/viewModels/viewModel.js", 80)

        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/kendo/2013.2.716/kendo.common.min.css", 81)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/kendo/2013.2.716/kendo.default.min.css", 82)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/browser-detection.css", 83)
        'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/colorbox.css", 84)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/jquery.toastmessage.css", 85)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/jqwidgets/jqx.base.css", 87)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/select2.css", 88)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/bootstrap/css/bootstrap.min.css", 89)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/bootstrap/css/bootstrap-responsive.min.css", 90)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/bootstrap/css/bootstrap-theme.min.css", 91)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/bootstrap/css/bootstrap-switch.css", 92)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/bootstrap/css/flat-ui-fonts.css", 93)

    End Sub

#End Region

End Class
