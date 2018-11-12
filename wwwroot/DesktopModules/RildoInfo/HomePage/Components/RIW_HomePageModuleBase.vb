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
' are accessible to your module's controls (that inherity from RIW_HomePageModuleBase
' 
' </summary>

Public Class RIW_HomePageModuleBase
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
            ElseIf UserInfo.IsInRole("Editores") Then
                intAuthorized = 1
            ElseIf UserInfo.IsInRole("Caixas") Then
                intAuthorized = 0
            End If
        End If

        Dim _clientId = -1
        'Dim clientInfo = RIStoreDataServices.RIStore_Business_Controller.GetClientUser(UserInfo.UserID)
        'If Not Null.IsNull(clientInfo) Then
        '    _clientId = clientInfo.ClientId
        'End If
        'Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(PortalId)
        Dim mCtrl As New DotNetNuke.Entities.Modules.ModuleController()
        Dim mSettings = mCtrl.GetTabModuleSettings(mCtrl.GetModuleByDefinition(PortalId, "RIW Products Manager").TabModuleID)

        Dim _scriptblock = New StringBuilder()
        _scriptblock.Append("<script>")
        _scriptblock.Append([String].Format("var _siteName = '{0}'; ", PortalSettings.PortalName))
        _scriptblock.Append([String].Format("var _siteURL = '{0}'; ", PortalSettings.PortalAlias.HTTPAlias))
        _scriptblock.Append([String].Format("var _managerRole = '{0}'; ", UserInfo.IsInRole("Gerentes").ToString().ToLower()))
        '_scriptblock.Append([String].Format("var _siteEmail = '{0}'; ", myPortalSettings("RIW_StoreEmail")))
        '_scriptblock.Append([String].Format("var _configURL = '{0}'; ", DotNetNuke.Common.Globals.NavigateURL(RI.RIStoreDataServices.RIStore_Business_Controller.GetPageID("Mini Estimate", PortalId), "Options", "mid=" & CStr(RIStore_Business_Controller.GetModInfo("Mini Estimate", PortalId).ModuleID))))
        '_scriptblock.Append([String].Format("var _editURL = '{0}';", EditUrl()))
        _scriptblock.Append([String].Format("var _categoriesURL = '{0}'; ", NavigateURL(RIW.Modules.Services.Utilities.GetPageID("RIW Categories Manager", PortalId).TabID)))
        _scriptblock.Append([String].Format("var _editItemURL = '{0}'; ", NavigateURL(RIW.Modules.Services.Utilities.GetPageID("RIW Products Manager", PortalId).TabID, "Edit", "mid=" & CStr(RIW.Modules.Services.Utilities.GetModInfo("RIW Products Manager", PortalId).ModuleID))))
        '_scriptblock.Append([String].Format("var _detailURL = '{0}'; ", EditUrl()))
        _scriptblock.Append([String].Format("var _productsURL = '{0}'; ", NavigateURL(RIW.Modules.Services.Utilities.GetPageID("RIW Products Viewer", PortalId).TabID)))
        _scriptblock.Append([String].Format("var _returnURL = '{0}'; ", NavigateURL(RIW.Modules.Services.Utilities.GetPageID("RIW Products Manager", PortalId).TabID)))
        _scriptblock.Append([String].Format("var _estimateURL = '{0}'; ", NavigateURL(RIW.Modules.Services.Utilities.GetPageID("RIW Products Viewer", PortalId).TabID)))
        _scriptblock.Append([String].Format("var _portalID = {0}; ", PortalId))
        _scriptblock.Append([String].Format("var _authorized = {0}; ", intAuthorized))
        _scriptblock.Append([String].Format("var _userID = {0}; ", UserInfo.UserID))
        _scriptblock.Append([String].Format("var _userName = '{0}'; ", UserInfo.Username))
        '_scriptblock.Append([String].Format("var _salesPerson = {0}; ", CInt(IIf(UserInfo.IsInRole("Vendedores"), UserInfo.UserID, CInt(myPortalSettings("RIW_SalesPerson"))))))
        '_scriptblock.Append([String].Format("var _viewPrice = '{0}'; ", myPortalSettings("RIW_ShowEstimatePrice").ToLower()))
        _scriptblock.Append([String].Format("var _displayName = '{0}'; ", UserInfo.DisplayName))
        _scriptblock.Append([String].Format("var _clientEmail = '{0}'; ", UserInfo.Email))
        _scriptblock.Append([String].Format("var _clientID = {0}; ", _clientId))
        '_scriptblock.Append([String].Format("var _tModuleID = {0}; ", mCtrl.GetModuleByDefinition(PortalId, "Mini Estimate").TabModuleID))
        _scriptblock.Append([String].Format("var _productImagesUrl = '{0}'; ", NavigateURL(RIW.Modules.Services.Utilities.GetPageID("RIW Products Manager", PortalId).TabID, "Images", "mid=" & CStr(RIW.Modules.Services.Utilities.GetModInfo("RIW Products Manager", PortalId).ModuleID))))
        _scriptblock.Append([String].Format("var _moduleID = {0}; ", ModuleId))
        _scriptblock.Append([String].Format("var _estimateSubject = '{0}'; ", IIf(mSettings("RIW_EstimateEmailSubject") IsNot Nothing, CStr(mSettings("RIW_EstimateEmailSubject")), "")))
        _scriptblock.Append([String].Format("var _estimateBody = '{0}'; ", IIf(mSettings("RIW_EstimateEmailBody") IsNot Nothing, CStr(mSettings("RIW_EstimateEmailBody")), "")))
        _scriptblock.Append([String].Format("var _estimateIntroMsg = '{0}'; ", IIf(mSettings("RIW_EstimateIntroMsg") IsNot Nothing, CStr(mSettings("RIW_EstimateIntroMsg")), "")))
        _scriptblock.Append([String].Format("var _visitorIntroMsg = '{0}'; ", IIf(mSettings("RIW_VisitorIntroMsg") IsNot Nothing, CStr(mSettings("RIW_VisitorIntroMsg")), "")))
        _scriptblock.Append("</script>")

        ' register scripts
        If Not Page.ClientScript.IsClientScriptBlockRegistered(STR_ModulePathScript) Then
            Page.ClientScript.RegisterClientScriptBlock(GetType(Page), STR_ModulePathScript, _scriptblock.ToString())
        End If

        ' register javascript libraries
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/kendo/2013.2.918/kendo.all.min.js", 20)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/kendo/2013.2.918/cultures/kendo.culture.pt-BR.min.js", 21)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/knockout/knockout-2.3.0.js", 22)
        'ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/knockout.mapping-latest.js", 23)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/knockout-kendo.min.js", 24)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/shortcut.js", 25)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/modernizr-2.6.2.js", 26)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/browser-detection.js", 27)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jquery.colorbox-min.js", 28)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jquery.toastmessage-min.js", 29)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/jquery.scrollTo.min.js", 30)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jquery.maskedinput.min.js", 31)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/amplify.min.js", 32)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jqwidgets/jqxcore.js", 33)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jqwidgets/jqxdropdownbutton.js", 34)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jqwidgets/jqxtree.js", 35)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jqwidgets/jqxmenu.js", 36)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jqwidgets/jqxpanel.js", 37)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jqwidgets/jqxscrollbar.js", 38)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jqwidgets/jqxbuttons.js", 39)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/bootstrap/bootstrap.min.js", 40)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/bootstrap/bootstrap-switch.min.js", 41)
        ClientResourceManager.RegisterScript(Parent.Page, "http://cdn.alloyui.com/2.0.0pr7/aui/aui-min.js", 42)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jquery.inputmask/jquery.inputmask.min.js", 43)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/bootstrap/knockout-bootstrap.min.js", 44)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/jquery.onscreen.min.js", 45)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/select2/select2.min.js", 46)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/Scripts/tristate-checkbox.js", 47)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/jquery.autogrow-textarea.js", 48)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/Markdown.Converter.js", 49)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/Markdown.Sanitizer.js", 50)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/Markdown.Editor.js", 51)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/jquery.markdown.js", 52)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/moment-with-langs.min.js", 53)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/services/scripts/app/utilities.js", 54)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/Products/viewModels/viewModel.js", 55)
        ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/Products/Scripts/App/data.js", 56)

        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/kendo/2013.2.918/kendo.common.min.css", 81)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/kendo/2013.2.918/kendo.default.min.css", 82)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/browser-detection.css", 83)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/colorbox.css", 84)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/jquery.toastmessage-min.css", 85)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/jqwidgets/jqx.base.css", 87)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/content/select2/select2.css", 88)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/bootstrap/css/bootstrap.min.css", 89)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/bootstrap/css/bootstrap-responsive.min.css", 90)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/bootstrap/css/bootstrap-theme.min.css", 91)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/bootstrap/css/bootstrap-switch.css", 92)
        ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/services/Content/bootstrap/css/flat-ui-fonts.css", 93)

    End Sub

#End Region

End Class
