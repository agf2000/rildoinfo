
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Security.Membership
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Services.Log.EventLog
Imports DotNetNuke.Web.Api

Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Web.Script.Serialization

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
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpPost> _
    Function Login(dto As LoginInfo) As HttpResponseMessage
        Try
            Dim services As Services = New Services()

            ' check userid and password using DotNetNuke security framework 
            Dim loginStatus As UserLoginStatus = UserLoginStatus.LOGIN_FAILURE
            Dim objUser As Users.UserInfo = Users.UserController.ValidateUser(dto.portalId, dto.userName, dto.password, "", "", Authentication.AuthenticationLoginBase.GetIPAddress(), loginStatus)

            If loginStatus = UserLoginStatus.LOGIN_SUCCESS Then

                ' sign current user out
                Dim objPortalSecurity As New PortalSecurity
                objPortalSecurity.SignOut()

                Users.UserController.UserLogin(dto.portalId, objUser, PortalSettings.PortalName, Authentication.AuthenticationLoginBase.GetIPAddress(), False)

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
    ''' Gets a list of roles by portal
    ''' </summary>
    ''' <param name="portalId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetPublicRoles(portalId As Integer) As HttpResponseMessage
        Try
            Dim ps As PortalSettings = PortalController.GetCurrentPortalSettings()
            Dim rc = New RoleController()

            Dim portalRoles = rc.GetPortalRoles(portalId)

            Dim roles As New ArrayList

            For Each role As RoleInfo In portalRoles.ToArray()
                roles.Add(New SubscribedRoles() With {.RoleId = role.RoleID, .RoleName = role.RoleName})
                'If group.RoleName = "Administradores" Then
                '    roles.Remove(group)
                'End If
                'If group.RoleName = "Unverified Users" Then
                '    roles.Remove(group)
                'End If
                'If group.RoleName = "Usuários Cadastrados" Then
                '    roles.Remove(group)
                'End If
            Next

            roles.Add(New SubscribedRoles() With {.RoleId = 9999, .RoleName = "Todos"})

            'Dim lstRoles As ArrayList = rc.GetPortalRoles(ps.PortalId)
            ''lstRoles.Add(New SubscribedRoles() With {.RoleId = 9999, .RoleName = "Todos", .Subscribed = True})

            ''Dim results = (From objRole In lstRoles Where objRole.IsPublic Select New Models.SubscribedRoles() With {.RoleId = objRole.RoleID, .RoleName = objRole.RoleName, .Subscribed = UserController.GetCurrentUserInfo().IsInRole(objRole.RoleName)}).ToList().OrderBy(Function(rs) rs.RoleName)
            'Dim results As IList = (From objRole In lstRoles Where CType(objRole, RoleInfo).IsPublic Select New SubscribedRoles() With {.RoleId = CType(objRole, RoleInfo).RoleID, .RoleName = CType(objRole, RoleInfo).RoleName, .Subscribed = UserController.GetCurrentUserInfo().IsInRole(CType(objRole, RoleInfo).RoleName)}).ToList()

            'results.Add(New SubscribedRoles() With {.RoleId = 9999, .RoleName = "Todos", .Subscribed = True})

            Return Request.CreateResponse(HttpStatusCode.OK, roles)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets a list of groups by user id and portal id
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpGet> _
    Function GetRolesByUser(portalId As Integer, uId As Integer) As HttpResponseMessage
        Try
            Dim ps As PortalSettings = PortalController.GetCurrentPortalSettings()
            Dim rc = New RoleController()

            Dim userRoles = rc.GetUserRoles(UserController.GetUserById(portalId, uId), True)

            Dim roles As New ArrayList

            For Each role As RoleInfo In userRoles.ToArray()
                roles.Add(New SubscribedRoles() With {.RoleId = role.RoleID, .RoleName = role.RoleName})
                'If group.RoleName = "Administradores" Then
                '    roles.Remove(group)
                'End If
                'If group.RoleName = "Unverified Users" Then
                '    roles.Remove(group)
                'End If
                'If group.RoleName = "Usuários Cadastrados" Then
                '    roles.Remove(group)
                'End If
            Next

            roles.Add(New SubscribedRoles() With {.RoleId = 9999, .RoleName = "Todos"})

            'Dim lstRoles As ArrayList = rc.GetPortalRoles(ps.PortalId)
            ''lstRoles.Add(New SubscribedRoles() With {.RoleId = 9999, .RoleName = "Todos", .Subscribed = True})

            ''Dim results = (From objRole In lstRoles Where objRole.IsPublic Select New Models.SubscribedRoles() With {.RoleId = objRole.RoleID, .RoleName = objRole.RoleName, .Subscribed = UserController.GetCurrentUserInfo().IsInRole(objRole.RoleName)}).ToList().OrderBy(Function(rs) rs.RoleName)
            'Dim results As IList = (From objRole In lstRoles Where CType(objRole, RoleInfo).IsPublic Select New SubscribedRoles() With {.RoleId = CType(objRole, RoleInfo).RoleID, .RoleName = CType(objRole, RoleInfo).RoleName, .Subscribed = UserController.GetCurrentUserInfo().IsInRole(CType(objRole, RoleInfo).RoleName)}).ToList()

            'results.Add(New SubscribedRoles() With {.RoleId = 9999, .RoleName = "Todos", .Subscribed = True})

            Return Request.CreateResponse(HttpStatusCode.OK, roles)
            'Dim objRole As New RoleController()
            'Dim roles = objRole.GetUserRoles(UserController.GetUserById(portalId, uId), True)

            'Return Json(roles, JsonRequestBehavior.AllowGet)
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
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPut> _
    Function updatePortalSetting(portalId As Integer, settingName As String, settingValue As String) As HttpResponseMessage
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
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPut> _
    Function updateModuleSetting(moduleSettings As String) As HttpResponseMessage
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
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPut> _
    Function updateTabModuleSetting(tabModuleSettings As List(Of Models.StorePortalModuleSetting)) As HttpResponseMessage
        Try
            Dim objModules As New Entities.Modules.ModuleController

            'Dim objDeserializer As New JavaScriptSerializer()
            'Dim lstItems As List(Of Models.StorePortalModuleSetting) = objDeserializer.Deserialize(Of List(Of Models.StorePortalModuleSetting))(tabModuleSettings)

            For Each setting In tabModuleSettings
                objModules.UpdateTabModuleSetting(setting.id, setting.name, setting.value.Replace(vbCrLf, "<br/>"))
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
    <DnnAuthorize(StaticRoles:="Gerentes,Editores,Vendedores")> _
    <HttpGet> _
    Function getPortalSettings(portalId As Integer) As HttpResponseMessage
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

    <AllowAnonymous> _
    <HttpGet> _
    Function ping() As HttpResponseMessage
        Try
            Return Request.CreateResponse(HttpStatusCode.OK, "Web API Services (DnnAuthorize) 01.00.00")
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    <AllowAnonymous> _
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
    <DnnAuthorize(StaticRoles:="Gerentes,Editores,Vendedores")> _
    <HttpGet> _
    Function AddPortalFolder(portalId As Integer, folder As String) As HttpResponseMessage
        Try
            Utilities.CreateDir(Utilities.GetPortalSettings(portalId), folder)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    'Private ReadOnly mediaTypeExtensionMap As New Dictionary(Of String, String)() From {
    '    {"image/jpeg", "jpg"},
    '    {"image/png", "png"},
    '    {"image/gif", "gif"},
    '    {"application/pdf", "pdf"},
    '    {"application/x-zip-compressed", "zip"},
    '    {"application/msword", "doc"},
    '    {"application/vnd.openxmlformats-officedocument.wordprocessingml.document", "docx"},
    '    {"application/vnd.ms-excel", "xls"},
    '    {"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "xlsx"}
    '}

    <AllowAnonymous> _
    <HttpPost> _
    Async Function postFile() As Task(Of HttpResponseMessage)
        Try
            If Not Me.Request.Content.IsMimeMultipartContent("form-data") Then
                Throw New HttpResponseException(New HttpResponseMessage(HttpStatusCode.UnsupportedMediaType))
            End If

            Dim portalCtrl = New Portals.PortalController()
            Dim _request As HttpRequestMessage = Me.Request

            Await _request.Content.LoadIntoBufferAsync()
            Dim task = _request.Content.ReadAsMultipartAsync()
            Dim result = Await task
            Dim contents = result.Contents
            Dim httpContent As HttpContent = contents.First()
            'Dim uploadedFileMediaType As String = httpContent.Headers.ContentType.MediaType
            Dim _file As Stream = httpContent.ReadAsStreamAsync().Result

            Dim portalId = contents(1).ReadAsStringAsync().Result
            Dim folderPath = contents(2).ReadAsStringAsync().Result

            Dim root = String.Format("{0}{1}", portalCtrl.GetPortal(portalId).HomeDirectoryMapPath, folderPath)
            Utilities.CreateDir(Utilities.GetPortalSettings(portalId), folderPath)

            Dim guid_1 = Guid.NewGuid().ToString()

            Dim _fileName = If(Not String.IsNullOrWhiteSpace(httpContent.Headers.ContentDisposition.FileName), httpContent.Headers.ContentDisposition.FileName, guid_1).Replace("""", String.Empty)
            Dim _filePath = Path.Combine(root, _fileName)

            If _file.CanRead Then
                Using _fileStream As New FileStream(_filePath, FileMode.Create)
                    _file.CopyTo(_fileStream)
                End Using
            End If

            DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(portalId, folderPath)

            'Dim fileManager = DotNetNuke.Services.FileSystem.FileManager.Instance()
            'Dim folderManager = DotNetNuke.Services.FileSystem.FolderManager.Instance()
            Dim folder = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetFolder(portalId, folderPath)

            Dim objFileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(folder, _fileName)

            Dim fileInfo As New PortalFile() With {
                .ContentType = objFileInfo.ContentType,
                .Extension = objFileInfo.Extension,
                .FileId = objFileInfo.FileId,
                .FileName = objFileInfo.FileName,
                .FileSize = objFileInfo.Size,
                .Height = objFileInfo.Height,
                .LastModifiedOnDate = objFileInfo.LastModifiedOnDate,
                .PhysicalPath = objFileInfo.PhysicalPath,
                .PortalId = objFileInfo.PortalId,
                .RelativePath = objFileInfo.RelativePath,
                .Width = objFileInfo.Width
            }

            Return _request.CreateResponse(HttpStatusCode.OK, fileInfo)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message}))
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
    <DnnAuthorize(StaticRoles:="Gerentes,Editores,Vendedores")> _
    <HttpPost> _
    Function saveFileContent(portalId As Integer, content As String, fileName As String, folderPath As String) As HttpResponseMessage
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
    <DnnAuthorize(StaticRoles:="Gerentes,Editores,Vendedores")> _
    <HttpPost> _
    Function removePortalFile(portalId As Integer, fileName As String, fileId As Integer, folderPath As String, files As IEnumerable(Of HttpPostedFileBase)) As HttpResponseMessage
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
    <DnnAuthorize(StaticRoles:="Gerentes,Editores,Vendedores")> _
    <HttpPost> _
    Function sendNewEstimateNotification(portalId As Integer, cId As Integer, eId As Integer, sId As Integer, emailTo As String, subject As String, msg As String) As HttpResponseMessage
        Try
            If Not cId > 0 AndAlso Not eId > 0 AndAlso sId > 0 Then
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
            End If

            Dim ccUserInfo As New Users.UserInfo()
            Dim portalCtrl = New Portals.PortalController()
            Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = Portals.PortalController.GetPortalSetting("RIW_StoreEmail", portalId, portalCtrl.GetPortal(portalId).Email), .DisplayName = portalCtrl.GetPortal(portalId).PortalName}
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
    <DnnAuthorize(StaticRoles:="Gerentes,Editores,Vendedores")> _
    <HttpPost> _
    Function sendNewEstimateEmail(portalId As Integer, cId As Integer, eId As Integer, sId As Integer, emailTo As String, subject As String, msg As String) As HttpResponseMessage
        Try
            If Not cId > 0 AndAlso Not eId > 0 AndAlso sId > 0 Then
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
            End If

            Dim ccUserInfo As New Users.UserInfo()
            Dim portalCtrl = New Portals.PortalController()
            Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = Portals.PortalController.GetPortalSetting("RIW_StoreEmail", portalId, portalCtrl.GetPortal(portalId).Email), .DisplayName = portalCtrl.GetPortal(portalId).PortalName}
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
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores,Vendedores")> _
    <HttpPost> _
    Function sendNewEmail(dto As EmailSend) As HttpResponseMessage
        Try
            Dim portalCtrl = New Portals.PortalController()
            Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = Portals.PortalController.GetPortalSetting("RIW_StoreEmail", dto.PortalId, portalCtrl.GetPortal(dto.PortalId).Email), .DisplayName = portalCtrl.GetPortal(dto.PortalId).PortalName}
            Dim _userInfo = Users.UserController.GetUserById(dto.PortalId, dto.UserId)

            Dim recipientList As New List(Of Users.UserInfo)
            Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(dto.PortalId)

            If dto.Emails IsNot Nothing Then
                If dto.Emails <> "[]" Then
                    dto.Emails = dto.Emails.Replace("[", "").Replace("]", "")
                    For Each personEmail In dto.Emails.Split(","c)
                        Dim newPerson = Users.UserController.GetUserByName(dto.PortalId, personEmail.Replace("""", String.Empty))
                        If newPerson IsNot Nothing Then
                            recipientList.Add(newPerson)

                            If dto.MessageBody.Contains("[SENHA]") Then
                                'If person.UserID > 0 Then
                                dto.MessageBody = dto.MessageBody.Replace("[SENHA]", Users.UserController.GetPassword(newPerson, ""))
                                'Else
                                '    dto.MessageBody = dto.MessageBody.Replace("[SENHA]", "Não Disponível")
                                'End If
                            End If
                        Else
                            recipientList.Add(New Users.UserInfo With {.UserID = Null.NullInteger, .Email = personEmail.Replace("""", String.Empty), .DisplayName = ""})
                        End If
                    Next
                End If
            ElseIf Not dto.PersonUserId = Nothing Then
                Dim newPerson = Users.UserController.GetUserById(dto.PortalId, dto.PersonUserId)
                recipientList.Add(newPerson)

                If dto.MessageBody.Contains("[SENHA]") Then
                    'If person.UserID > 0 Then
                    'dto.MessageBody = dto.MessageBody.Replace("[SENHA]", Users.UserController.GetPassword(newPerson, ""))
                    'Else
                    dto.MessageBody = dto.MessageBody.Replace("[SENHA]", "Não Disponível")
                    'End If
                End If
            End If

            'For Each person In people
            '    If dto.MessageBody.Contains("[SENHA]") Then
            '        If person.UserID > 0 Then
            '            dto.MessageBody = dto.MessageBody.Replace("[SENHA]", Users.UserController.GetPassword(person, ""))
            '        Else
            '            dto.MessageBody = dto.MessageBody.Replace("[SENHA]", "Não Disponível")
            '        End If
            '    End If
            '    'Notifications.SendStoreEmail(storeUser, person, Nothing, Nothing, dto.Subject, dto.MessageBody, Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, dto.PortalId, True)
            'Next

            Dim mailMessage As New Net.Mail.MailMessage
            Dim distList As New PostOffice

            mailMessage.From = New Net.Mail.MailAddress(storeUser.Email, storeUser.DisplayName)
            mailMessage.ReplyToList.Add(New Net.Mail.MailAddress(_userInfo.Email, _userInfo.DisplayName))
            mailMessage.Subject = dto.Subject
            mailMessage.Body = dto.MessageBody
            'mailMessage.Attachments.Add(New Net.Mail.Attachment(Str, String.Format("Orcamento_{0}_{1}.pdf", CStr(dto.EstimateId), Replace(DateTime.Now.ToString(), ":", "-").Replace("/", "-"))))
            mailMessage.IsBodyHtml = True

            Dim sentMails = distList.SendMail(mailMessage, recipientList, myPortalSettings("RIW_SMTPServer"), CInt(myPortalSettings("RIW_SMTPPort")), CBool(myPortalSettings("RIW_SMTPConnection")), myPortalSettings("RIW_SMTPLogin"), myPortalSettings("RIW_SMTPPassword"))

            If dto.HistoryText <> "" Then
                Dim personHistory As New Models.PersonHistory
                Dim personHistoryCtrl As New PersonHistoriesRepository

                personHistory.PersonId = dto.PersonId
                personHistory.HistoryText = dto.HistoryText
                personHistory.CreatedByUser = dto.CreatedByUser
                personHistory.CreatedOnDate = dto.CreatedOnDate

                personHistoryCtrl.addPersonHistory(personHistory)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Sends html content
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores,Vendedores")> _
    <HttpPost> _
    Function sendPackage(dto As EmailSend) As HttpResponseMessage
        Try
            Dim contentCtrl As New HtmlContentsRepository()
            Dim content As New Models.HtmlContent()
            content = contentCtrl.getHtmlContent(dto.PortalId, dto.ContentId)

            Dim contentStr = ""
            If content IsNot Nothing Then
                contentStr = content.HtmlContent
            Else
                contentStr = dto.MessageBody
            End If

            Dim portalCtrl = New Portals.PortalController()
            Dim recipientList As New List(Of Users.UserInfo)
            Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(dto.PortalId)
            Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = Portals.PortalController.GetPortalSetting("RIW_StoreEmail", dto.PortalId, portalCtrl.GetPortal(dto.PortalId).Email), .DisplayName = portalCtrl.GetPortal(dto.PortalId).PortalName}
            Dim _userInfo = Users.UserController.GetUserById(dto.PortalId, dto.UserId)

            'Dim people As New List(Of Users.UserInfo)

            If dto.Emails IsNot Nothing Then
                If dto.Emails <> "[]" Then
                    dto.Emails = dto.Emails.Replace("[", "").Replace("]", "")
                    For Each personEmail In dto.Emails.Split(","c)
                        Dim personContactCtrl As New PersonContactsRepository
                        Dim newPerson = personContactCtrl.getPersonContact(personEmail.Replace("""", String.Empty), dto.PortalId) ' Users.UserController.GetUserByName(dto.PortalId, personEmail.Replace("""", String.Empty))
                        If newPerson IsNot Nothing Then
                            recipientList.Add(New Users.UserInfo With {.UserID = Null.NullInteger, .DisplayName = newPerson.ContactName, .Email = newPerson.ContactEmail1})
                        Else
                            recipientList.Add(Users.UserController.GetUserByName(dto.PortalId, personEmail.Replace("""", String.Empty)))
                        End If
                    Next
                End If
            End If

            Dim mailMessage As New Net.Mail.MailMessage
            Dim distList As New PostOffice

            mailMessage.From = New Net.Mail.MailAddress(storeUser.Email, storeUser.DisplayName)
            mailMessage.ReplyToList.Add(New Net.Mail.MailAddress(_userInfo.Email, _userInfo.DisplayName))
            mailMessage.Subject = dto.Subject
            mailMessage.Body = contentStr
            'mailMessage.Attachments.Add(New Net.Mail.Attachment(Str, String.Format("Orcamento_{0}_{1}.pdf", CStr(dto.EstimateId), Replace(DateTime.Now.ToString(), ":", "-").Replace("/", "-"))))
            mailMessage.IsBodyHtml = True

            Dim sentMails = distList.SendMail(mailMessage, recipientList, myPortalSettings("RIW_SMTPServer"), CInt(myPortalSettings("RIW_SMTPPort")), CBool(myPortalSettings("RIW_SMTPConnection")), myPortalSettings("RIW_SMTPLogin"), myPortalSettings("RIW_SMTPPassword"))

            'For Each toUser In people
            '    Notifications.SendStoreEmail(storeUser, toUser, Nothing, Nothing, dto.Subject, contentStr, Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, dto.PortalId, True)
            'Next

            If dto.HistoryText IsNot Nothing Then
                Dim personHistory As New Models.PersonHistory
                Dim personHistoryCtrl As New PersonHistoriesRepository

                personHistory.PersonId = dto.PersonId
                personHistory.HistoryText = dto.HistoryText
                personHistory.CreatedByUser = dto.CreatedByUser
                personHistory.CreatedOnDate = dto.CreatedOnDate

                personHistoryCtrl.addPersonHistory(personHistory)
            End If

            Dim personCtrl As New PeopleRepository()
            Dim person = personCtrl.getPerson(dto.PersonId, dto.PortalId, Null.NullInteger)

            person.Sent = True
            person.ModifiedByUser = dto.CreatedByUser
            person.ModifiedOnDate = dto.CreatedOnDate

            personCtrl.updatePerson(person)

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
    Function extendTime() As HttpResponseMessage
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
    Function expireTime() As HttpResponseMessage
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
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores,Vendedores")> _
    <HttpPut> _
    Function updateUserPassword(dto As UserPasswordInfo) As HttpResponseMessage
        Try
            Dim portalCtrl = New Portals.PortalController()
            Dim _portalInfo = portalCtrl.GetPortal(dto.PortalId)
            Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(_portalInfo.PortalID)

            Dim _userInfo = Users.UserController.GetUserById(_portalInfo.PortalID, dto.UserId)

            Dim objEventLog As New EventLogController
            Dim objEventLogInfo As New LogInfo

            objEventLogInfo.AddProperty("IP", Utilities.GetIPAddress())
            objEventLogInfo.LogPortalID = _portalInfo.PortalID
            objEventLogInfo.LogPortalName = _portalInfo.PortalName
            objEventLogInfo.LogUserID = _userInfo.UserID
            objEventLogInfo.LogUserName = _userInfo.Username
            objEventLogInfo.LogTypeKey = "PASSWORD_ALTERED_SUCCESS"

            Dim recipientList As New List(Of Users.UserInfo)
            recipientList.Add(_userInfo)

            Dim _portalEmail = Portals.PortalController.GetPortalSetting("RIW_StoreEmail", _portalInfo.PortalID, _portalInfo.Email)
            Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = _portalEmail, .DisplayName = _portalInfo.PortalName}

            Dim distList As New PostOffice

            'mailMessage.ReplyToList.Add(New Net.Mail.MailAddress(_userInfo.Email, _userInfo.DisplayName))
            Dim mailMessage As New Net.Mail.MailMessage() With {
                .From = New Net.Mail.MailAddress(storeUser.Email, storeUser.DisplayName),
                .Subject = dto.Subject,
                .Body = dto.MessageBody.Replace("[LOGIN]", _userInfo.Username).Replace("[*********]", Users.UserController.GetPassword(_userInfo, "")),
                .IsBodyHtml = True}

            If dto.CurrentPassword.Length > 0 Then

                If Users.UserController.ChangePassword(_userInfo, dto.CurrentPassword, dto.NewPassword) Then

                    objEventLog.AddLog(objEventLogInfo)

                    Dim sentMails = distList.SendMail(mailMessage, recipientList, myPortalSettings("RIW_SMTPServer"), CInt(myPortalSettings("RIW_SMTPPort")), CBool(myPortalSettings("RIW_SMTPConnection")), myPortalSettings("RIW_SMTPLogin"), myPortalSettings("RIW_SMTPPassword"))

                    'Notifications.SendStoreEmail(storeUser, _userInfo, Nothing, Nothing, subject, body.Replace("[LOGIN]", _userInfo.Username).Replace("[*********]", Users.UserController.GetPassword(_userInfo, "")), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)

                    Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})

                Else
                    Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Sua senha original está incorreta!"})
                End If

            Else

                If Users.UserController.ChangePassword(_userInfo, _userInfo.Membership.Password, dto.NewPassword) Then

                    objEventLog.AddLog(objEventLogInfo)

                    Dim sentMails = distList.SendMail(mailMessage, recipientList, myPortalSettings("RIW_SMTPServer"), CInt(myPortalSettings("RIW_SMTPPort")), CBool(myPortalSettings("RIW_SMTPConnection")), myPortalSettings("RIW_SMTPLogin"), myPortalSettings("RIW_SMTPPassword"))

                    'Notifications.SendStoreEmail(storeUser, _userInfo, Nothing, Nothing, subject, body.Replace("[LOGIN]", _userInfo.Username).Replace("[*********]", Users.UserController.GetPassword(_userInfo, "")), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)

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
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores,Vendedores")> _
    <HttpPut> _
    Function generateUserPassword(dto As UserPasswordInfo) As HttpResponseMessage
        Try
            Dim portalCtrl = New Portals.PortalController()
            Dim _portalInfo = portalCtrl.GetPortal(dto.PortalId)
            Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(_portalInfo.PortalID)

            Dim _userInfo = Users.UserController.GetUserById(_portalInfo.PortalID, dto.UserId)

            Dim recipientList As New List(Of Users.UserInfo)
            recipientList.Add(_userInfo)

            Dim newPassword = Utilities.GeneratePassword(7)

            If Users.UserController.ChangePassword(_userInfo, _userInfo.Membership.Password, newPassword) Then

                Dim objEventLog As New EventLogController
                Dim objEventLogInfo As New LogInfo
                objEventLogInfo.AddProperty("IP", Utilities.GetIPAddress())
                objEventLogInfo.LogPortalID = _portalInfo.PortalID
                objEventLogInfo.LogPortalName = _portalInfo.PortalName
                objEventLogInfo.LogUserID = _userInfo.UserID
                objEventLogInfo.LogUserName = _userInfo.Username
                objEventLogInfo.LogTypeKey = "PASSWORD_ALTERED_SUCCESS"
                objEventLog.AddLog(objEventLogInfo)

                Dim _portalEmail = Portals.PortalController.GetPortalSetting("RIW_StoreEmail", _portalInfo.PortalID, _portalInfo.Email)
                Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = _portalEmail, .DisplayName = _portalInfo.PortalName}

                Dim mailMessage As New Net.Mail.MailMessage
                Dim distList As New PostOffice

                mailMessage.From = New Net.Mail.MailAddress(storeUser.Email, _userInfo.DisplayName)
                'mailMessage.ReplyToList.Add(New Net.Mail.MailAddress(_userInfo.Email, _userInfo.DisplayName))
                mailMessage.Subject = dto.Subject
                mailMessage.Body = dto.MessageBody.Replace("[LOGIN]", _userInfo.Username).Replace("[*********]", Users.UserController.GetPassword(_userInfo, ""))
                mailMessage.IsBodyHtml = True

                Dim sentMails = distList.SendMail(mailMessage, recipientList, myPortalSettings("RIW_SMTPServer"), CInt(myPortalSettings("RIW_SMTPPort")), CBool(myPortalSettings("RIW_SMTPConnection")), myPortalSettings("RIW_SMTPLogin"), myPortalSettings("RIW_SMTPPassword"))

                'Notifications.SendStoreEmail(storeUser, _userInfo, Nothing, Nothing, subject, body.Replace("[LOGIN]", _userInfo.Username).Replace("[*********]", Users.UserController.GetPassword(_userInfo, "")), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)

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
    ''' Gets a list of roles by portal id
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function getRoles(portalId As Integer) As HttpResponseMessage
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

    ''' <summary>
    ''' Gets files by portal id, filtered by folder name
    ''' </summary>
    ''' <param name="portalId"></param>
    ''' <param name="folder"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores,Vendedores")> _
    <HttpGet> _
    Function getPortalFiles(portalId As Integer, Optional folder As String = "") As HttpResponseMessage
        Try
            If folder Is Nothing Then
                folder = ""
            End If

            Dim allFiles As New List(Of PortalFile)

            Dim objFolderInfo = FolderManager.Instance.GetFolder(portalId, folder)
            Dim fileList = FolderManager.Instance.GetFiles(objFolderInfo)

            For Each _file In fileList

                allFiles.Add(New PortalFile() With {
                             .FileId = _file.FileId,
                             .LastModifiedOnDate = _file.LastModifiedOnDate,
                             .FileName = _file.FileName,
                             .FileSize = _file.Size,
                             .RelativePath = _file.RelativePath,
                             .PhysicalPath = _file.PhysicalPath,
                             .Extension = _file.Extension,
                             .ContentType = _file.ContentType,
                             .Width = _file.Width,
                             .Height = _file.Height})

            Next

            Return Request.CreateResponse(HttpStatusCode.OK, allFiles)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Syncs folders by portal id
    ''' </summary>
    ''' <param name="portalId"></param>
    ''' <param name="folder">Folder Name</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function SyncPortalFolders(portalId As Integer, Optional folder As String = "") As HttpResponseMessage
        Try
            If folder Is Nothing Then
                folder = ""
            End If

            FolderManager.Instance.Synchronize(portalId, folder, True, False)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets files by portal id, permission and user id
    ''' </summary>
    ''' <param name="portalId"></param>
    ''' <param name="permissions">Permission READ, ADD, DELETE</param>
    ''' <param name="uId">User ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores,Vendedores")> _
    <HttpGet> _
    Function GetPortalFolders(ByVal portalId As Integer, ByVal permissions As String, uId As Integer) As HttpResponseMessage
        Try

            Dim foldersList = New ArrayList()
            Dim objFolderInfo = FolderManager.Instance.GetFolders(portalId, permissions, uId)

            For Each _folder In objFolderInfo

                foldersList.Add(New PortalFolder() With {.FolderID = _folder.FolderID, .FolderName = _folder.FolderName, .FolderPath = _folder.FolderPath})

            Next

            Return Request.CreateResponse(HttpStatusCode.OK, foldersList)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    Public Class UserPasswordInfo
        Public Property PortalId As Integer
        Public Property UserId As Integer
        Public Property CurrentPassword As String
        Public Property NewPassword As String
        Public Property Subject As String
        Public Property MessageBody As String
    End Class

    Class LoginInfo
        Property portalId As Integer
        Property userName As String
        Property password As String
    End Class

    Class PortalFile
        Public Property PortalId As Integer
        Public Property FileId As Integer
        Public Property FileName As String
        Public Property RelativePath As String
        Public Property PhysicalPath As String
        Public Property Extension As String
        Public Property FileSize As String
        Public Property LastModifiedOnDate As DateTime
        Public Property Width As Integer
        Public Property Height As Integer
        Public Property ContentType As String
    End Class

    Public Class PortalFolder
        Public Property PortalId As Integer
        Public Property FolderID As Integer
        Public Property FolderName As String
        Public Property FolderPath As String
    End Class

    Public Class EmailSend
        Property PortalId As Integer
        Property PersonId As Integer
        Property UserId As Integer
        Property Subject As String
        Property MessageBody As String
        Property Emails As String
        Property HistoryText As String
        Property CreatedByUser As String
        Property ContentId As Integer
        Property CreatedOnDate As Date
        Property PersonUserId As Integer
    End Class

    Public Class SubscribedRoles
        Public Property RoleId As Integer
        Public Property RoleName As String
        Public Property Subscribed As Boolean
    End Class

End Class
