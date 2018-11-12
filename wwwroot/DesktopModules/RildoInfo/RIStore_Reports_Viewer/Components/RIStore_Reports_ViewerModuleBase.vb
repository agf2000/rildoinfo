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
Imports DotNetNuke.Security.Permissions.ModulePermissionController

' <summary>
' This base class can be used to define custom properties for multiple controls. 
' An example module, DNNSimpleArticle (http://dnnsimplearticle.codeplex.com) uses this for an ArticleId
' 
' Because the class inherits from PortalModuleBase, properties like ModuleId, TabId, UserId, and others, 
' are accessible to your module's controls (that inherity from RIStore_ReportsModuleBase
' 
' </summary>

Public Class RIStore_Reports_ViewerModuleBase
    Inherits PortalModuleBase

    Private Const STR_ModulePathScript As String = "modulePathScript"

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

        Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(PortalId)

        Dim objRoleCtrl = New DotNetNuke.Security.Roles.RoleController

        Dim _scriptblock = New StringBuilder()
        _scriptblock.Append("<script>")
        _scriptblock.Append([String].Format("var _moduleID = {0};", ModuleId))
        _scriptblock.Append([String].Format("var _returnURL = ""{0}"";", DotNetNuke.Common.Globals.NavigateURL()))
        _scriptblock.Append([String].Format("var _editURL = ""{0}"";", EditUrl("kit")))
        _scriptblock.Append([String].Format("var _portalID = {0};", PortalId))
        _scriptblock.Append([String].Format("var _authorized = {0};", intAuthorized))
        _scriptblock.Append([String].Format("var _userID = {0};", UserInfo.UserID))
        _scriptblock.Append("</script>")

        ' register scripts
        If Not Page.ClientScript.IsClientScriptBlockRegistered(STR_ModulePathScript) Then
            Page.ClientScript.RegisterClientScriptBlock(GetType(Page), STR_ModulePathScript, _scriptblock.ToString())
        End If

        ' register javascript libraries
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2014.2.903/kendo.web.min.js", 20)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/kendo/2014.2.903/cultures/kendo.culture.pt-BR.min.js", 21)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-3.1.0.js", 22)
        'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout.mapping-latest.js", 23)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/knockout/knockout-kendo.min.js", 24)
        'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/shortcut.js", 25)
        'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/modernizr.min.js", 26)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/Scripts/browser-detection.js", 56)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/Scripts/jquery.colorbox-min.js", 59)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/Scripts/jquery.toastmessage.js", 60)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/Scripts/jquery.maskedinput.min.js", 61)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/jquery.scrollTo.min.js", 62)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/Scripts/tristate-checkbox.js", 63)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/Scripts/jqxcore.js", 64)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/Scripts/jqxdropdownbutton.js", 65)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/Scripts/jqxtree.js", 66)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/Scripts/jqxpanel.js", 67)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/Scripts/jqxscrollbar.js", 68)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/Scripts/jqxbuttons.js", 69)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/Scripts/App/utilities.js", 70)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristore_reports_viewer/Scripts/App/data.js", 71)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/ristore_reports_viewer/viewmodels/viewmodel.js", 75)

        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/kendo/2014.2.903/styles/kendo.common.min.css", 71)
        'ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/Content/kendo/2013.2.716/kendo.default.min.css", 82)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/browser-detection.css", 73)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/colorbox/colorbox.css", 74)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jquery.toastmessage-min.css", 75)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/jqwidgets/styles/jqx.base.css", 77)

    End Sub

End Class


