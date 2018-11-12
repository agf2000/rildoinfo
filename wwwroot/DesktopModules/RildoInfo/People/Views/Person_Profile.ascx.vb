﻿' Copyright (c) 2013  AGF
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 

''' <summary>
''' The View class displays the content
''' 
''' Typically your view control would be used to display content or functionality in your module.
''' 
''' View may be the only control you have in your project depending on the complexity of your module
''' 
''' Because the control inherits from RIW_PeopleModuleBase you have access to any custom properties
''' defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
''' 
''' </summary>
Partial Class Person_Profile
    Inherits RIW_PeopleModuleBase

    Private Const RIW_ModulePathScript As String = "mod_ModulePathScript"

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        ClientResourceManagement.ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/People/viewmodels/personProfile.js", 99)
        RegisterModuleInfo()
    End Sub

    Public Sub RegisterModuleInfo()

        Dim modCtrl As New Entities.Modules.ModuleController()

        Dim _scriptblock = New StringBuilder()
        _scriptblock.Append("<script>")
        _scriptblock.Append([String].Format("var _ascxVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(Request.PhysicalApplicationPath & "DesktopModules\RildoInfo\" & Me.ModuleConfiguration.DesktopModule.FolderName & "\views\person_profile.ascx").ToString("g")))
        _scriptblock.Append([String].Format("var _jsVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(Request.PhysicalApplicationPath & "DesktopModules\RildoInfo\" & Me.ModuleConfiguration.DesktopModule.FolderName & "\viewmodels\personProfile.js").ToString("g")))
        _scriptblock.Append("</script>")

        ' register scripts
        If Not Page.ClientScript.IsClientScriptBlockRegistered(RIW_ModulePathScript) Then
            Page.ClientScript.RegisterClientScriptBlock(GetType(Page), RIW_ModulePathScript, _scriptblock.ToString())
        End If
    End Sub

End Class