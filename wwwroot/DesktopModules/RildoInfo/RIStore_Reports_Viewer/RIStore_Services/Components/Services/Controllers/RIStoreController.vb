
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Services.Log.EventLog
Imports DotNetNuke.Security.Membership
Imports DotNetNuke.Security.Roles
Imports System.Web.Script.Serialization
Imports DotNetNuke.Instrumentation
Imports System.IO

Namespace RI.Modules.RIStore_Services

    Public Class RIStoreController
        Inherits DnnApiController

        Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(RIStoreController))

        Protected Property FilesController() As IFiles_Repository
            Get
                Return m_FilesController
            End Get
            Private Set(value As IFiles_Repository)
                m_FilesController = value
            End Set
        End Property
        Private m_FilesController As IFiles_Repository

        Private Shared Sub AddSearchTerm(ByRef propertyNames As String, ByRef propertyValues As String, name As String, value As String)
            If Not [String].IsNullOrEmpty(value) Then
                propertyNames += name & ","
                propertyValues += value & ","
            End If
        End Sub

        '<ValidateAntiForgeryToken()> _
        '<HttpGet> _
        '<DnnAuthorize> _

        ''' <summary>
        ''' Logs user in
        ''' Never Cache
        ''' </summary>
        ''' <param name="portalId"></param>
        ''' <param name="userName"></param>
        ''' <param name="password"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpGet> _
        <AllowAnonymous> _
        Function Login(ByVal portalId As Integer, ByVal userName As String, ByVal password As String) As HttpResponseMessage
            Try
                Dim services As Services = New Services()

                ' check userid and password using DotNetNuke security framework 
                Dim loginStatus As UserLoginStatus = UserLoginStatus.LOGIN_FAILURE
                Dim objUser As Users.UserInfo = Users.UserController.ValidateUser(0, userName, password, "", "", Authentication.AuthenticationLoginBase.GetIPAddress(), loginStatus)

                If loginStatus = UserLoginStatus.LOGIN_SUCCESS Then

                    ' sign current user out
                    Dim objPortalSecurity As New PortalSecurity
                    objPortalSecurity.SignOut()

                    Users.UserController.UserLogin(portalId, objUser, PortalSettings.PortalName, Authentication.AuthenticationLoginBase.GetIPAddress(), False)

                    Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})

                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Acesso Negado. Login e Senha Incorreto."})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets host settings
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <RequireHost> _
        <HttpGet> _
        Function HostSettings() As HttpResponseMessage
            Try
                Dim hostSetting = DotNetNuke.Entities.Controllers.HostController.Instance.GetSettingsDictionary()

                Return Request.CreateResponse(HttpStatusCode.OK, hostSetting)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets a list of public roles
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetPublicRoles() As HttpResponseMessage
            Try
                Dim ps As PortalSettings = PortalController.GetCurrentPortalSettings()
                Dim rc = New RoleController()

                Dim lstRoles As ArrayList = rc.GetPortalRoles(ps.PortalId)
                lstRoles.Add(New Models.SubscribedRoles() With {.RoleId = 9999, .RoleName = "Todos", .Subscribed = True})

                'Dim results = (From objRole In lstRoles Where objRole.IsPublic Select New Models.SubscribedRoles() With {.RoleId = objRole.RoleID, .RoleName = objRole.RoleName, .Subscribed = UserController.GetCurrentUserInfo().IsInRole(objRole.RoleName)}).ToList().OrderBy(Function(rs) rs.RoleName)
                Dim results = (From objRole In lstRoles Where Not objRole.IsPublic Select New Models.SubscribedRoles() With {.RoleId = objRole.RoleID, .RoleName = objRole.RoleName, .Subscribed = UserController.GetCurrentUserInfo().IsInRole(objRole.RoleName)}).ToList().OrderBy(Function(rs) rs.RoleName)

                Return Request.CreateResponse(HttpStatusCode.OK, results)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Updates portal settings
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="settingName">Portal Setting Name</param>
        ''' <param name="settingValue">Portal Setting Value</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPut> _
        Function UpdatePortalSetting(ByVal portalId As Integer, ByVal settingName As String, settingValue As String) As HttpResponseMessage
            Try
                Portals.PortalController.UpdatePortalSetting(portalId, settingName, settingValue, True)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Updates portal settings
        ''' </summary>
        ''' <param name="moduleSettings">Module Settings</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPut> _
        Function UpdateModuleSetting(ByVal moduleSettings As String) As HttpResponseMessage
            Try
                Dim objModules As New Entities.Modules.ModuleController

                Dim objDeserializer As New JavaScriptSerializer()
                Dim lstItems As List(Of Models.StorePortalModuleSetting) = objDeserializer.Deserialize(Of List(Of Models.StorePortalModuleSetting))(moduleSettings)

                For Each setting In lstItems
                    objModules.UpdateModuleSetting(setting.id, setting.name, setting.value)
                Next

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Updates portal settings
        ''' </summary>
        ''' <param name="tabModuleSettings"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPut> _
        Function UpdateTabModuleSetting(ByVal tabModuleSettings As String) As HttpResponseMessage
            Try
                Dim objModules As New Entities.Modules.ModuleController

                Dim objDeserializer As New JavaScriptSerializer()
                Dim lstItems As List(Of Models.StorePortalModuleSetting) = objDeserializer.Deserialize(Of List(Of Models.StorePortalModuleSetting))(tabModuleSettings)

                For Each setting In lstItems
                    objModules.UpdateTabModuleSetting(setting.id, setting.name, setting.value)
                Next

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets all current portal settings
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <AllowAnonymous> _
        <HttpGet> _
        Function GetPortalSettings(portalId As Integer) As HttpResponseMessage
            Try
                Dim objCtrl As New Portals.PortalController
                Dim _portalSettings = PortalController.GetPortalSettingsDictionary(portalId)

                Return Request.CreateResponse(HttpStatusCode.OK, _portalSettings)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets list of users by portal id
        ''' </summary>
        ''' <param name="users"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetUsers(users As IEnumerable(Of UserInfo)) As IList(Of Models.User)
            Try
                Return users.[Select](Function(user) New Models.User(user, PortalSettings)).ToList()
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Gets list of users by portal id and role name
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="roleName">Role Name</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetUsersByRoleName(portalId As Integer, userId As Integer, roleName As String, pageIndex As Integer, pageSize As Integer, sortField As String, sortOrder As String) As HttpResponseMessage
            Try
                Dim roleCtlr As New DotNetNuke.Security.Roles.RoleController
                Dim usersData = roleCtlr.GetUsersByRoleName(portalId, roleName)

                Dim _users As New List(Of UserInfo)
                For Each _user In usersData
                    _users.Add(_user)
                Next

                Return Request.CreateResponse(HttpStatusCode.OK, GetUsers(_users))
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets user info by portal id and user id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="uId">User ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetUser(portalId As Integer, uId As Integer) As HttpResponseMessage
            Try
                Dim users = New List(Of Users.UserInfo)
                Dim user = UserController.GetUserById(portalId, uId)
                users.Add(user)

                Return Request.CreateResponse(HttpStatusCode.OK, GetUsers(users))
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        <DnnAuthorize> _
        <HttpGet> _
        Function Ping() As HttpResponseMessage
            Try
                Return Request.CreateResponse(HttpStatusCode.OK, "Web API Services (DnnAuthorize) 01.00.00")
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        <HttpGet> _
        Function PingHost() As HttpResponseMessage
            Try
                Return Request.CreateResponse(HttpStatusCode.OK, "Web API Services (Host) 01.00.00")
            Catch ex As Exception
                'DnnLog.Error(ex)
                Dim services = New Services()
                Dim action As Models.ServicesAction = New Models.ServicesAction() With {.LogTypeKey = "LOGIN_SUCCESS", .AppName = "Hello", .Username = "Paulo"}
                services.Log(action)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        '<DnnAuthorize> _
        '<HttpPost> _
        'Function LoginUser(action As ServicesAction) As HttpResponseMessage
        '    Try

        '        Dim services = New Services()
        '        Dim servicesUser = services.GetUserByName(action.Username)

        '        If servicesUser.IsSuperUser Then
        '            Return Host(action)
        '        Else
        '            action.LogTypeKey = "LOGIN_SUCCESS"
        '            services.Log(action)
        '            Return Request.CreateResponse(HttpStatusCode.OK, "sucess")
        '        End If

        '    Catch ex As Exception
        '        'Logger.Error(ex)
        '        'DnnLog.Error(ex)
        '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        '    End Try
        'End Function

        '<HttpPost> _
        'Function Host(action As ServicesAction) As HttpResponseMessage
        '    Try

        '        action.LogTypeKey = "LOGIN_SUPERUSER"
        '        Dim services = New Services()
        '        services.Log(action)
        '        Return Request.CreateResponse(HttpStatusCode.OK, "sucess")

        '    Catch ex As Exception
        '        'Logger.Error(ex)
        '        'DnnLog.Error(ex)
        '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        '    End Try
        'End Function

        ''' <summary>
        ''' Adds folders by portal id
        ''' </summary>
        ''' <param name="portalId"></param>
        ''' <param name="folder">Folder Name</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpGet> _
        <DnnAuthorize> _
        Function AddPortalFolder(ByVal portalId As Integer, ByVal folder As String) As HttpResponseMessage
            Try
                FolderManager.Instance.AddFolder(portalId, folder)

                FolderManager.Instance.Synchronize(portalId, "", True, False)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Saves File
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="maxWidth">Image's width</param>
        ''' <param name="maxHeight">Image's height</param>
        ''' <param name="folderPath">Save path</param>
        ''' <param name="files">Uploaded Files</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpPost> _
        Function SaveFile(ByVal portalId As Integer,
            ByVal maxWidth As Integer,
            ByVal maxHeight As Integer,
            ByVal folderPath As String,
            ByVal files As IEnumerable(Of HttpPostedFileBase)) As HttpResponseMessage
            Try
                Dim portalCtrl = New Portals.PortalController()
                Dim destinationPath = FolderManager.Instance.GetFolder(portalId, folderPath)

                Dim _file = files(0)

                Dim fileName = Path.GetFileName(_file.FileName)

                If ImageResizer.Configuration.Config.Current.Pipeline.IsAcceptedImageType(fileName) Then

                    'The resizing settings can specify any of 30 commands.. See http://imageresizing.net for details.
                    'Destination paths can have variables like <guid> and <ext>
                    Dim i = New ImageResizer.ImageJob() With {.CreateParentDirectory = True, .Source = _file, .Dest = String.Format(
                            "~/{0}/{1}/<filename>.<ext>", portalCtrl.GetPortal(portalId).HomeDirectory, folderPath)}

                    Dim img = Drawing.Image.FromStream(_file.InputStream)
                    If img.Height > img.Width Then
                        i.Settings = New ImageResizer.ResizeSettings(
                            String.Format("width={0}&height={1}&crop=auto", CStr(IIf(maxWidth > 0, CStr(maxWidth), "600")), CStr(IIf(maxHeight > 0, CStr(maxHeight), "800"))))
                    Else
                        i.Settings = New ImageResizer.ResizeSettings(
                            String.Format("width={0}&height={1}&crop=auto", CStr(IIf(maxWidth > 0, CStr(maxWidth), "800")), CStr(IIf(maxHeight > 0, CStr(maxHeight), "600"))))
                    End If

                    _file.InputStream.Seek(0, SeekOrigin.Begin)

                    i.Build()
                Else
                    _file.SaveAs(destinationPath.PhysicalPath & fileName)
                End If

                FolderManager.Instance.Synchronize(portalId)

                Dim objFileInfo = FileManager.Instance.GetFile(destinationPath, _file.FileName)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .fileInfo = objFileInfo})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Saves File
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="content">File Content</param>
        ''' <param name="fileName">File Name</param>
        ''' <param name="folderPath">Folder Name</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpPost> _
        Function SaveFileContent(ByVal portalId As Integer, ByVal content As String, ByVal fileName As String, ByVal folderPath As String) As HttpResponseMessage
            Try
                Dim portalCtrl = New Portals.PortalController()
                Dim destinationPath = FolderManager.Instance.GetFolder(portalId, folderPath)

                Dim saveString = content

                If destinationPath Is Nothing Then
                    FolderManager.Instance.AddFolder(portalId, folderPath)
                    FolderManager.Instance.Synchronize(portalId)
                    destinationPath = FolderManager.Instance.GetFolder(portalId, folderPath)
                End If

                Dim filePath = String.Format("{0}{1}", destinationPath.PhysicalPath, fileName.Trim())

                If IO.File.Exists(filePath) Then
                    IO.File.Delete(filePath)
                End If

                Using sw As New StreamWriter(filePath, True)
                    sw.WriteLine(saveString)
                    sw.Close()

                    FolderManager.Instance.Synchronize(portalId, destinationPath.FolderPath, False, True)

                    Dim objFileInfo = FileManager.Instance.GetFile(destinationPath, fileName.Trim())

                    Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .fileId = objFileInfo.FileId})
                End Using

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Removes File
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="fileName">File Name</param>
        ''' <param name="fileId">File ID</param>
        ''' <param name="folderPath">Folder Path</param>
        ''' <param name="files">File to be deleted</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpPost> _
        Function RemovePortalFile(ByVal portalId As Integer, ByVal fileName As String, ByVal fileId As Integer, ByVal folderPath As String, ByVal files As IEnumerable(Of HttpPostedFileBase)) As HttpResponseMessage
            Try
                Dim portalCtrl = New Portals.PortalController()

                If files IsNot Nothing Then
                    Dim _file = files(0)
                    If String.Compare(FilesController.DeleteFile(String.Format("{0}{1}\{2}", portalCtrl.GetPortal(portalId).HomeDirectoryMapPath, folderPath, _file.FileName)), "", False) = 0 Then
                        FolderManager.Instance.Synchronize(portalId)

                        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
                    End If
                Else
                    If fileId > 0 Then
                        Dim objFileInfo = FileManager.Instance.GetFile(fileId)
                        'FileManager.Instance.DeleteFile(objFileInfo)
                        Dim deleted = False
                        'FileManager.Instance.DeleteFile(_file)
                        If IO.File.Exists(objFileInfo.PhysicalPath) = True Then
                            Try
                                'IO.File.Delete(objFileInfo.PhysicalPath)
                                FileManager.Instance.DeleteFile(objFileInfo)
                                deleted = True
                            Catch ex As Exception
                                'Dim portalPath = portalCtrl.GetPortal(portalId).HomeDirectoryMapPath & "Temp\"
                                Dim objFolderInfo = FolderManager.Instance.GetFolder(portalId, "Temp/")
                                'IO.File.Move(objFileInfo.PhysicalPath, portalPath & objFileInfo.FileName)
                                FileManager.Instance.MoveFile(objFileInfo, objFolderInfo)
                                deleted = True
                            End Try
                        End If
                        If deleted Then
                            FolderManager.Instance.Synchronize(portalId)

                            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
                        End If
                    Else
                        If String.Compare(FilesController.DeleteFile(fileName), "", False) = 0 Then
                            FolderManager.Instance.Synchronize(portalId)

                            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
                        End If
                    End If
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Sends an email notification about a new estimate
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="cid">Client ID</param>
        ''' <param name="eId">Estimate ID</param>
        ''' <param name="sId">Sales Person ID</param>
        ''' <param name="subject">Email Subject</param>
        ''' <param name="msg">Mensagem</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function SendNewEstimateNotification(ByVal portalId As Integer, ByVal cId As Integer, ByVal eId As Integer, ByVal sId As Integer, ByVal emailTo As String, ByVal subject As String, ByVal msg As String) As HttpResponseMessage
            Try
                If Not cId > 0 AndAlso Not eId > 0 AndAlso sId > 0 Then
                    Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
                End If

                Dim ccUserInfo As New Users.UserInfo()
                Dim portalCtrl = New Portals.PortalController()
                Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = Portals.PortalController.GetPortalSetting("RIS_StoreEmail", portalId, portalCtrl.GetPortal(portalId).Email), .DisplayName = portalCtrl.GetPortal(portalId).PortalName}
                'Dim clientInfo = RIStore_Business_Controller.GetClient(cId)
                'If emailTo.Length > 0 AndAlso Not clientInfo.Email = emailTo Then
                '    With ccUserInfo
                '        .UserID = Null.NullInteger
                '        .Email = emailTo
                '        .DisplayName = clientInfo.DisplayName
                '    End With
                'Else
                '    ccUserInfo = Nothing
                'End If

                'Dim estimateInfo = RIStore_Business_Controller.GetEstimateById(eId)
                Dim _salesInfo = Users.UserController.GetUserById(portalId, sId)

                'Notifications.SendStoreEmail(storeUser, Users.UserController.GetUserById(portalId, clientInfo.UserId), ccUserInfo, Nothing, subject.Replace("[ID]", CStr(eId)), msg.Replace("[ID]", CStr(eId)), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)
                'Notifications.SendStoreEmail(storeUser, _salesInfo, ccUserInfo, ccUserInfo, subject.Replace("[ID]", CStr(eId)), msg.Replace("[ID]", CStr(eId)), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)
                ''Notifications.SendStoreEmail(storeUser, Users.UserController.GetUserById(portalId, clientInfo.UserId), "", "", subject, msg.Replace("\n", "<br />"), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)

                'Notifications.EstimateNotification(Constants.ContentTypeName.RIStore_Estimate, Constants.NotificationEstimateTypeName.RIStore_Estimate_Updated, Null.NullInteger, _salesInfo, "Gerentes", portalId, eId, String.Format("Novo Orçamento Inserido (ID: {0})", CStr(eId)), msg.Replace("[ID]", CStr(eId)))

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Sends an email notification about a new estimate
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="cid">Client ID</param>
        ''' <param name="eId">Estimate ID</param>
        ''' <param name="sId">Sales Person ID</param>
        ''' <param name="subject">Email Subject</param>
        ''' <param name="msg">Mensagem</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function SendNewEstimateEmail(portalId As Integer, cId As Integer, eId As Integer, sId As Integer, emailTo As String, subject As String, msg As String) As HttpResponseMessage
            Try
                If Not cId > 0 AndAlso Not eId > 0 AndAlso sId > 0 Then
                    Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
                End If

                Dim ccUserInfo As New Users.UserInfo()
                Dim portalCtrl = New Portals.PortalController()
                Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = Portals.PortalController.GetPortalSetting("RIS_StoreEmail", portalId, portalCtrl.GetPortal(portalId).Email), .DisplayName = portalCtrl.GetPortal(portalId).PortalName}
                'Dim clientInfo = RIStore_Business_Controller.GetClient(cId)
                'If emailTo.Length > 0 AndAlso Not clientInfo.Email = emailTo Then
                '    With ccUserInfo
                '        .UserID = Null.NullInteger
                '        .Email = emailTo
                '        .DisplayName = clientInfo.DisplayName
                '    End With
                'Else
                '    ccUserInfo = Nothing
                'End If

                'Notifications.SendStoreEmail(storeUser, Users.UserController.GetUserById(portalId, clientInfo.UserId), ccUserInfo, Nothing, subject, msg.Replace("\n", "<br />"), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Sends new email
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="cUid">Client User ID</param>
        ''' <param name="uId">User ID</param>
        ''' <param name="hText">History</param>
        ''' <param name="subject">Email Subject</param>
        ''' <param name="body">Mensagem</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function SendNewEmail(ByVal portalId As Integer, ByVal cUId As Integer, ByVal uId As Integer, ByVal hText As String, ByVal subject As String, ByVal body As String) As HttpResponseMessage
            Try
                Dim portalCtrl = New Portals.PortalController()
                Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = Portals.PortalController.GetPortalSetting("RIS_StoreEmail", portalId, portalCtrl.GetPortal(portalId).Email), .DisplayName = portalCtrl.GetPortal(portalId).PortalName}
                Dim _clientInfo = Users.UserController.GetUserById(portalId, cUId)
                Dim _userInfo = Users.UserController.GetUserById(portalId, uId)

                If body.Contains("[SENHA]") Then
                    body = body.Replace("[SENHA]", MembershipProvider.Instance.GetPassword(_clientInfo, ""))
                End If

                'Notifications.SendStoreEmail(storeUser, _clientInfo, Nothing, Nothing, subject, body, Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)

                'If hText <> "" Then
                '    DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("RIS_Client_History_Add", RIStore_Business_Controller.GetClientUser(cUId).ClientId, hText, uId, Now())
                'End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Sends new email
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <param name="uId">User ID</param>
        ''' <param name="subject">Email Subject</param>
        ''' <param name="body">Mensagem</param>
        ''' <param name="emails">Endereços de email</param>
        ''' <param name="hText">History Text</param>
        ''' <param name="cd">Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function SendNews(ByVal portalId As Integer, ByVal cId As Integer, ByVal uId As Integer, ByVal subject As String, ByVal body As String, ByVal emails As String, ByVal hText As String, ByVal cd As Date) As HttpResponseMessage
            Try
                Dim portalCtrl = New Portals.PortalController()
                Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = Portals.PortalController.GetPortalSetting("RIS_StoreEmail", portalId, portalCtrl.GetPortal(portalId).Email), .DisplayName = portalCtrl.GetPortal(portalId).PortalName}
                Dim _clientInfo As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = emails, .DisplayName = ""}
                Dim _userInfo = Users.UserController.GetUserById(portalId, uId)

                'Notifications.SendStoreEmail(storeUser, _clientInfo, Nothing, Nothing, subject, body, Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)

                DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("RIS_Client_History_Add", cId, hText, uId, cd)
                DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("RIS_Client_Sent_Update", cId, True, uId, cd)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' This is used from JavaScript to re-establish the user's session
        ''' Needs to implement "Never Cache"
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <AllowAnonymous> _
        <HttpGet> _
        Function ExtendTime() As HttpResponseMessage
            Try
                ' Re-establish the session timeout
                Dim session = HttpContext.Current.Session
                session.Timeout = 20
                'Return New EmptyResult()

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Logs current user out
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <AllowAnonymous> _
        <HttpGet> _
        Function ExpireTime() As HttpResponseMessage
            Try
                Dim portalSecurity = New PortalSecurity
                portalSecurity.SignOut()

                ' Redirect to the role-specified "session expired" view
                ' This needs to be a separate Action because we need to issue a separate
                ' request once the session has been abandoned in order to have the correct
                ' context (that the user is logged out).
                'Return RedirectToAction(returnUrl)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Updates user password
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="uId">User ID</param>
        ''' <param name="currPassword">Current Password</param>
        ''' <param name="newPassword">New Password</param>
        ''' <param name="subject">Subject</param>
        ''' <param name="body">Message</param>
        ''' <param name="mUId">Modified By User ID</param>
        ''' <param name="md">Modified Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPut> _
        Function UpdateUserPassword(ByVal portalId As Integer, ByVal uId As Integer, ByVal currPassword As String, ByVal newPassword As String, ByVal subject As String, ByVal body As String, ByVal mUId As Integer, ByVal md As Date) As HttpResponseMessage
            Try

                Dim portalCtrl = New Portals.PortalController()
                Dim _userInfo = Users.UserController.GetUserById(portalId, uId)

                If currPassword.Length > 0 Then

                    If Users.UserController.ChangePassword(_userInfo, currPassword, newPassword) Then

                        Dim objEventLog As New EventLogController
                        Dim objEventLogInfo As New LogInfo
                        objEventLogInfo.AddProperty("IP", Utilities.GetIPAddress())
                        objEventLogInfo.LogPortalID = PortalSettings.PortalId
                        objEventLogInfo.LogPortalName = PortalSettings.PortalName
                        objEventLogInfo.LogUserID = uId
                        objEventLogInfo.LogUserName = _userInfo.Username
                        objEventLogInfo.LogTypeKey = "PASSWORD_ALTERED_SUCCESS"
                        objEventLog.AddLog(objEventLogInfo)

                        Dim _portalEmail = Portals.PortalController.GetPortalSetting("RIS_StoreEmail", portalId, portalCtrl.GetPortal(portalId).Email)
                        Dim _portalName = portalCtrl.GetPortal(portalId).PortalName
                        Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = _portalEmail, .DisplayName = _portalName}

                        Notifications.SendStoreEmail(storeUser, _userInfo, Nothing, Nothing, subject, body.Replace("[LOGIN]", _userInfo.Username).Replace("[*********]", MembershipProvider.Instance().GetPassword(_userInfo, "")), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)

                        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})

                    Else
                        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Sua senha original está incorreta!"})
                    End If

                Else

                    If Users.UserController.ChangePassword(_userInfo, _userInfo.Membership.Password, newPassword) Then

                        Dim objEventLog As New EventLogController
                        Dim objEventLogInfo As New LogInfo
                        objEventLogInfo.AddProperty("IP", Utilities.GetIPAddress())
                        objEventLogInfo.LogPortalID = portalId
                        objEventLogInfo.LogPortalName = portalCtrl.GetPortal(portalId).PortalName
                        objEventLogInfo.LogUserID = uId
                        objEventLogInfo.LogUserName = _userInfo.Username
                        objEventLogInfo.LogTypeKey = "PASSWORD_ALTERED_SUCCESS"
                        objEventLog.AddLog(objEventLogInfo)

                        Dim _portalEmail = Portals.PortalController.GetPortalSetting("RIS_StoreEmail", portalId, portalCtrl.GetPortal(portalId).Email)
                        Dim _portalName = portalCtrl.GetPortal(portalId).PortalName
                        Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = _portalEmail, .DisplayName = _portalName}

                        Notifications.SendStoreEmail(storeUser, _userInfo, Nothing, Nothing, subject, body.Replace("[LOGIN]", _userInfo.Username).Replace("[*********]", MembershipProvider.Instance().GetPassword(_userInfo, "")), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)

                        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
                    Else

                    End If
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Generates new random user password by user id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="uId">User ID</param>
        ''' <param name="subject">Subject</param>
        ''' <param name="body">Message</param>
        ''' <param name="mUId">Modified By User ID</param>
        ''' <param name="md">Modified Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPut> _
        Function GenerateUserPassword(ByVal portalId As Integer, ByVal uId As Integer, ByVal subject As String, ByVal body As String, ByVal mUId As Integer, ByVal md As Date) As HttpResponseMessage
            Try

                Dim portalCtrl = New Portals.PortalController()
                Dim _userInfo = Users.UserController.GetUserById(portalId, uId)
                Dim newPassword = Utilities.GeneratePassword(7)

                If Users.UserController.ChangePassword(_userInfo, _userInfo.Membership.Password, newPassword) Then

                    Dim objEventLog As New EventLogController
                    Dim objEventLogInfo As New LogInfo
                    objEventLogInfo.AddProperty("IP", Utilities.GetIPAddress())
                    objEventLogInfo.LogPortalID = portalId
                    objEventLogInfo.LogPortalName = portalCtrl.GetPortal(portalId).PortalName
                    objEventLogInfo.LogUserID = uId
                    objEventLogInfo.LogUserName = _userInfo.Username
                    objEventLogInfo.LogTypeKey = "PASSWORD_ALTERED_SUCCESS"
                    objEventLog.AddLog(objEventLogInfo)

                    Dim _portalEmail = Portals.PortalController.GetPortalSetting("RIS_StoreEmail", portalId, portalCtrl.GetPortal(portalId).Email)
                    Dim _portalName = portalCtrl.GetPortal(portalId).PortalName
                    Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = _portalEmail, .DisplayName = _portalName}

                    Notifications.SendStoreEmail(storeUser, _userInfo, Nothing, Nothing, subject, body.Replace("[LOGIN]", _userInfo.Username).Replace("[*********]", MembershipProvider.Instance().GetPassword(_userInfo, "")), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)

                    Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Saves user photo
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="uId">User ID</param>
        ''' <param name="maxWidth">Image's width</param>
        ''' <param name="maxHeight">Image's height</param>
        ''' <param name="photos">Uploaded Files</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function SaveUserPhoto(ByVal portalId As Integer, ByVal uId As Integer, ByVal maxWidth As Integer, ByVal maxHeight As Integer, ByVal photos As IEnumerable(Of HttpPostedFileBase)) As HttpResponseMessage
            Try
                Dim portalCtrl = New Portals.PortalController()
                Dim _userInfo = Users.UserController.GetUserById(portalId, uId)
                Dim destinationPath = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(_userInfo)

                Dim fileName = ""

                For Each _file In photos

                    fileName = Path.GetFileName(_file.FileName)

                    If ImageResizer.Configuration.Config.Current.Pipeline.IsAcceptedImageType(fileName) Then

                        'The resizing settings can specify any of 30 commands.. See http://imageresizing.net for details.
                        'Destination paths can have variables like <guid> and <ext>
                        Dim i = New ImageResizer.ImageJob
                        i.CreateParentDirectory = True
                        i.Source = _file
                        i.Dest = String.Format("~/{0}/{1}<filename>.<ext>", portalCtrl.GetPortal(portalId).HomeDirectory, destinationPath.FolderPath)

                        Dim img = Drawing.Image.FromStream(_file.InputStream)

                        i.Settings = New ImageResizer.ResizeSettings(String.Format("width={0}&height={1}", CStr(IIf(maxWidth > 0, CStr(maxWidth), "120")), CStr(IIf(maxHeight > 0, CStr(maxHeight), "120"))))

                        _file.InputStream.Seek(0, SeekOrigin.Begin)

                        i.Build()
                    Else
                        _file.SaveAs(portalCtrl.GetPortal(portalId).HomeDirectoryMapPath & destinationPath.PhysicalPath & fileName)
                    End If

                    DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(portalId)

                    Dim objFolderInfo = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetFolder(portalId, destinationPath.FolderPath)
                    Dim objFileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(objFolderInfo, _file.FileName)

                    _userInfo.Profile.SetProfileProperty("Photo", CStr(objFileInfo.FileId))
                    Entities.Profile.ProfileController.UpdateUserProfile(_userInfo)

                Next

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .photoPath = String.Format("/{0}/{1}{2}", portalCtrl.GetPortal(portalId).HomeDirectory, destinationPath.FolderPath, fileName)})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Removes user photo
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="uId">User ID</param>
        ''' <param name="fileName">File Name</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function RemoveUserPhoto(ByVal portalId As Integer, ByVal uId As Integer, ByVal fileName As String) As HttpResponseMessage
            Try
                Dim portalCtrl = New Portals.PortalController()
                Dim _userInfo = Users.UserController.GetUserById(portalId, uId)
                Dim destinationPath = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(_userInfo)
                Dim objFolderInfo = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetFolder(portalId, destinationPath.FolderPath)
                Dim objFileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(objFolderInfo, fileName)

                DotNetNuke.Services.FileSystem.FileManager.Instance().DeleteFile(objFileInfo)

                _userInfo.Profile.SetProfileProperty("Photo", "-1")
                Entities.Profile.ProfileController.UpdateUserProfile(_userInfo)

                DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(portalId)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .photoPath = String.Format("/{0}/{1}{2}", portalCtrl.GetPortal(portalId).HomeDirectory, destinationPath.FolderPath, fileName)})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets a list of roles by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetRoles(portalId As Integer) As HttpResponseMessage
            Try
                Dim objRole As New RoleController
                Dim roles = objRole.GetPortalRoles(portalId)

                If roles.Count <> 0 Then

                    For Each group As RoleInfo In roles.ToArray()
                        If group.RoleName = "Administradores" Then
                            roles.Remove(group)
                        End If
                        If group.RoleName = "Unverified Users" Then
                            roles.Remove(group)
                        End If
                        If group.RoleName = "Usuários Cadastrados" Then
                            roles.Remove(group)
                        End If
                    Next

                    roles.Add(New RoleInfo() With {.Description = "Todos Usuários.", .PortalID = 0, .RoleID = 9999, .RoleGroupID = -1, .RoleName = "Todos"})

                    Return Request.CreateResponse(HttpStatusCode.OK, roles)
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

    End Class
End Namespace