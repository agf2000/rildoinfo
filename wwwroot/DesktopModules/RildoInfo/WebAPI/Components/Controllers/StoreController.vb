
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Security.Membership
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.FileSystem
'Imports DotNetNuke.Services.Log.EventLog
Imports DotNetNuke.Web.Api
Imports RIW.Modules.Common
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Web.Script.Serialization
Imports RIW.Modules.WebAPI.Components.Repositories
Imports System.Xml
Imports RIW.Modules.WebAPI.Components.Models
Imports System.Threading

Public Class StoreController
    Inherits DnnApiController

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(StoreController))

    Protected Property FilesController() As IFilesRepository
        Get
            Return mFilesController
        End Get
        Private Set(value As IFilesRepository)
            mFilesController = value
        End Set
    End Property

    Private mFilesController As IFilesRepository

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
            'Dim services As Services = New Services()

            ' check userid and password using DotNetNuke security framework 
            Dim loginStatus As UserLoginStatus = UserLoginStatus.LOGIN_FAILURE
            Dim objUser As UserInfo = UserController.ValidateUser(dto.portalId, dto.userName, dto.password, "", "", Authentication.AuthenticationLoginBase.GetIPAddress(), loginStatus)

            If loginStatus = UserLoginStatus.LOGIN_SUCCESS Then

                ' sign current user out
                Dim objPortalSecurity As New PortalSecurity
                objPortalSecurity.SignOut()

                UserController.UserLogin(dto.portalId, objUser, PortalSettings.PortalName, Authentication.AuthenticationLoginBase.GetIPAddress(), False)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})

            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Acesso Negado. Login e Senha Incorreto."})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates portal settings
    ''' </summary>
    ''' <param name="portal">Portal Info</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPut> _
    Function UpdatePortal(portal As MyPortalInfo) As HttpResponseMessage
        Try
            Dim portalCtrl = New PortalController
            Dim thePortal = portalCtrl.GetPortal(portal.PortalId)

            thePortal.PortalName = portal.PortalName
            thePortal.LogoFile = portal.LogoFile
            thePortal.Description = portal.Description
            thePortal.KeyWords = portal.KeyWords

            portalCtrl.UpdatePortalInfo(thePortal)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates portal settings
    ''' </summary>
    ''' <param name="portalSettings">Portal Settings</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPut> _
    Function UpdateAppSetting(appSettings As List(Of Setting)) As HttpResponseMessage
        Try
            Dim settingsCtrl As New SettingsRepository

            For Each setting In appSettings
                'Portals.PortalController.UpdatePortalSetting(setting.id, setting.name, setting.value, True)

                Dim presentSetting = settingsCtrl.GetSetting(setting.SettingName, setting.PortalId)
                presentSetting.SettingName = setting.SettingName
                presentSetting.SettingValue = setting.SettingValue
                presentSetting.CultureCode = "pt-BR"

                If presentSetting IsNot Nothing Then
                    presentSetting.ModifiedByUser = 2
                    presentSetting.ModifiedOnDate = Now()
                    settingsCtrl.UpdateSetting(presentSetting)
                Else
                    presentSetting.PortalId = setting.PortalId
                    presentSetting.CreatedByUser = 2
                    presentSetting.CreatedOnDate = Now()
                    settingsCtrl.AddSetting(presentSetting)
                End If
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    Function UpdateModuleSetting(moduleSettings As String) As HttpResponseMessage
        Try
            Dim objModules As New ModuleController

            Dim objDeserializer As New JavaScriptSerializer()
            Dim lstItems As List(Of StoreSetting) = objDeserializer.Deserialize(Of List(Of StoreSetting))(moduleSettings)

            For Each setting In lstItems
                objModules.UpdateModuleSetting(setting.id, setting.name, setting.value)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    Function UpdateTabModuleSetting(tabModuleSettings As List(Of StoreSetting)) As HttpResponseMessage
        Try
            Dim objModules As New ModuleController

            'Dim objDeserializer As New JavaScriptSerializer()
            'Dim lstItems As List(Of Models.StorePortalModuleSetting) = objDeserializer.Deserialize(Of List(Of Models.StorePortalModuleSetting))(tabModuleSettings)

            For Each setting In tabModuleSettings
                objModules.UpdateTabModuleSetting(setting.id, setting.name, setting.value.Replace(vbCrLf, "<br/>"))
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    Function GetPortalSettings(portalId As Integer) As HttpResponseMessage
        Try
            Dim thePortalSettings = PortalController.Instance.GetPortalSettings(portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, thePortalSettings)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    <AllowAnonymous> _
    <HttpGet> _
    Function Ping() As HttpResponseMessage
        Try
            Return Request.CreateResponse(HttpStatusCode.OK, "Web API Services (DnnAuthorize) 01.00.00")
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
            logger.[Error](ex)
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
    Async Function PostFile() As Task(Of HttpResponseMessage)
        Try
            If Not Request.Content.IsMimeMultipartContent("form-data") Then
                Throw New HttpResponseException(New HttpResponseMessage(HttpStatusCode.UnsupportedMediaType))
            End If

            Dim portalCtrl = New PortalController()
            Dim theRequest As HttpRequestMessage = Request

            Await theRequest.Content.LoadIntoBufferAsync()
            Dim task = theRequest.Content.ReadAsMultipartAsync()
            Dim result = Await task
            Dim contents = result.Contents
            Dim httpContent As HttpContent = contents.Last()
            'Dim uploadedFileMediaType As String = httpContent.Headers.ContentType.MediaType
            Dim theFile As Stream = httpContent.ReadAsStreamAsync().Result

            Dim portalId = contents(0).ReadAsStringAsync().Result
            Dim folderPath = contents(1).ReadAsStringAsync().Result

            Dim root = String.Format("{0}{1}", portalCtrl.GetPortal(portalId).HomeDirectoryMapPath, folderPath)
            Utilities.CreateDir(Utilities.GetPortalSettings(portalId), folderPath)

            Dim theGuid = Guid.NewGuid().ToString()

            Dim theFileName = If(Not String.IsNullOrWhiteSpace(httpContent.Headers.ContentDisposition.FileName), httpContent.Headers.ContentDisposition.FileName, theGuid).Replace("""", String.Empty)
            Dim theFilePath = Path.Combine(root, theFileName)

            If theFile.CanRead Then
                Using theFileStream As New FileStream(theFilePath, FileMode.Create)
                    theFile.CopyTo(theFileStream)
                End Using
            End If

            FolderManager.Instance().Synchronize(portalId, folderPath)

            'Dim fileManager = DotNetNuke.Services.FileSystem.FileManager.Instance()
            'Dim folderManager = DotNetNuke.Services.FileSystem.FolderManager.Instance()
            Dim folder = FolderManager.Instance().GetFolder(portalId, folderPath)

            Dim objFileInfo = FileManager.Instance().GetFile(folder, theFileName)

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

            Return theRequest.CreateResponse(HttpStatusCode.OK, fileInfo)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    Function SaveFileContent(portalId As Integer, content As String, fileName As String, folderPath As String) As HttpResponseMessage
        Try
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
            logger.[Error](ex)
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
    Function RemovePortalFile(portalId As Integer, fileName As String, fileId As Integer, folderPath As String, files As IEnumerable(Of HttpPostedFileBase)) As HttpResponseMessage
        Try
            Dim portalCtrl = New PortalController()

            If files IsNot Nothing Then
                Dim theFile = files(0)
                If String.Compare(FilesController.DeleteFile(String.Format("{0}{1}\{2}", portalCtrl.GetPortal(portalId).HomeDirectoryMapPath, folderPath, theFile.FileName)), "", False) = 0 Then
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
            logger.[Error](ex)
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
    Function SendNewEstimateNotification(portalId As Integer, cId As Integer, eId As Integer, sId As Integer, emailTo As String, subject As String, msg As String) As HttpResponseMessage
        Try
            If Not cId > 0 AndAlso Not eId > 0 AndAlso sId > 0 Then
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
            End If

            Dim ccUserInfo As New UserInfo()
            Dim portalCtrl = New PortalController()
            'Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = settingsDictionay("StoreEmail"), .DisplayName = portalCtrl.GetPortal(portalId).PortalName}
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
            Dim _salesInfo = UserController.GetUserById(portalId, sId)

            'Notifications.SendStoreEmail(storeUser, Users.UserController.GetUserById(portalId, clientInfo.UserId), ccUserInfo, Nothing, subject.Replace("[ID]", CStr(eId)), msg.Replace("[ID]", CStr(eId)), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)
            'Notifications.SendStoreEmail(storeUser, _salesInfo, ccUserInfo, ccUserInfo, subject.Replace("[ID]", CStr(eId)), msg.Replace("[ID]", CStr(eId)), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)
            ''Notifications.SendStoreEmail(storeUser, Users.UserController.GetUserById(portalId, clientInfo.UserId), "", "", subject, msg.Replace("\n", "<br />"), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)

            'Notifications.EstimateNotification(Constants.ContentTypeName.RIStore_Estimate, Constants.NotificationEstimateTypeName.RIStore_Estimate_Updated, Null.NullInteger, _salesInfo, "Gerentes", portalId, eId, String.Format("Novo Orçamento Inserido (ID: {0})", CStr(eId)), msg.Replace("[ID]", CStr(eId)))

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    Function SendNewEstimateEmail(portalId As Integer, cId As Integer, eId As Integer, sId As Integer, emailTo As String, subject As String, msg As String) As HttpResponseMessage
        Try
            If Not cId > 0 AndAlso Not eId > 0 AndAlso sId > 0 Then
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
            End If

            Dim ccUserInfo As New UserInfo()
            Dim portalCtrl = New PortalController()
            'Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = settingsDictionay("StoreEmail"), .DisplayName = portalCtrl.GetPortal(portalId).PortalName}
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
            logger.[Error](ex)
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
    Function SendNewEmail(dto As EmailSend) As HttpResponseMessage
        Try
            Dim portalCtrl = New PortalController()

            Dim settingCtrl As New SettingsRepository
            Dim settingsDictionay = settingCtrl.GetSettings(dto.PortalId)

            Dim storeUser As New UserInfo() With {.UserID = Null.NullInteger, .Email = settingsDictionay("storeEmail"), .DisplayName = portalCtrl.GetPortal(dto.PortalId).PortalName}
            Dim theUserInfo = UserController.GetUserById(dto.PortalId, dto.UserId)

            Dim recipientList As New List(Of UserInfo)
            Dim myPortalSettings = PortalController.Instance.GetPortalSettings(dto.PortalId)

            If dto.Emails.Count > 0 Then
                For Each personEmail In dto.Emails
                    Dim newPerson = UserController.GetUserByName(dto.PortalId, personEmail.Replace("""", String.Empty))
                    If newPerson IsNot Nothing Then
                        recipientList.Add(newPerson)

                        If dto.MessageBody.Contains("[SENHA]") Then
                            'If person.UserID > 0 Then
                            dto.MessageBody = dto.MessageBody.Replace("[SENHA]", UserController.GetPassword(newPerson, ""))
                            'Else
                            '    dto.MessageBody = dto.MessageBody.Replace("[SENHA]", "Não Disponível")
                            'End If
                        End If
                    Else
                        recipientList.Add(New UserInfo With {.UserID = Null.NullInteger, .Email = personEmail.Replace("""", String.Empty), .DisplayName = ""})
                    End If
                Next
            ElseIf Not dto.PersonUserId = Nothing Then
                Dim newPerson = UserController.GetUserById(dto.PortalId, dto.PersonUserId)
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

            Dim mm As New Net.Mail.MailMessage() With {.From = New Net.Mail.MailAddress(storeUser.Email, storeUser.DisplayName)}
            mm.ReplyToList.Add(New Net.Mail.MailAddress(theUserInfo.Email, theUserInfo.DisplayName))
            mm.Subject = dto.Subject
            mm.Body = dto.MessageBody
            mm.IsBodyHtml = True

            'Dim sentMails = distList.SendMail(mailMessage, recipientList, SettingsDictionay("smtpServer"), CInt(SettingsDictionay("smtpPort")), CBool(SettingsDictionay("smtpConnection")), SettingsDictionay("smtpLogin"), SettingsDictionay("smtpPassword"))

            Dim email As New Thread(Sub() PostOffice.SendMail(mm, recipientList, settingsDictionay("smtpServer"), CInt(settingsDictionay("smtpPort")),
                                                              CBool(settingsDictionay("smtpConnection")), settingsDictionay("smtpLogin"),
                                                              settingsDictionay("smtpPassword"))) With {.IsBackground = True}
            email.Start()

            If dto.HistoryText <> "" Then
                Dim personHistory As New PersonHistory
                Dim personHistoryCtrl As New PersonHistoriesRepository

                personHistory.PersonId = dto.PersonId
                personHistory.HistoryText = dto.HistoryText
                personHistory.CreatedByUser = dto.CreatedByUser
                personHistory.CreatedOnDate = dto.CreatedOnDate

                personHistoryCtrl.AddPersonHistory(personHistory)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    Function SendPackage(dto As EmailSend) As HttpResponseMessage
        Try
            Dim contentCtrl As New HtmlContentsRepository()
            Dim content As New HtmlContent()
            content = contentCtrl.GetHtmlContent(dto.PortalId, dto.ContentId)

            Dim settingCtrl As New SettingsRepository
            Dim settingsDictionay = settingCtrl.GetSettings(dto.PortalId)

            Dim contentStr = ""
            If content IsNot Nothing Then
                contentStr = content.HtmlContent
            Else
                contentStr = dto.MessageBody
            End If

            Dim portalCtrl = New PortalController()
            Dim recipientList As New List(Of UserInfo)
            Dim myPortalSettings = PortalController.Instance.GetPortalSettings(dto.PortalId)
            Dim storeUser As New UserInfo() With {.UserID = Null.NullInteger, .Email = settingsDictionay("storeEmail"), .DisplayName = portalCtrl.GetPortal(dto.PortalId).PortalName}
            Dim theUserInfo = UserController.GetUserById(dto.PortalId, dto.UserId)

            'Dim people As New List(Of Users.UserInfo)

            If dto.Emails.Count > 0 Then
                For Each personEmail In dto.Emails
                    Dim personContactCtrl As New PersonContactsRepository
                    Dim newPerson = personContactCtrl.GetPersonContact(personEmail, dto.PersonId) ' Users.UserController.GetUserByName(dto.PortalId, personEmail.Replace("""", String.Empty))
                    If newPerson IsNot Nothing Then
                        recipientList.Add(New UserInfo With {.UserID = Null.NullInteger, .DisplayName = newPerson.ContactName, .Email = newPerson.ContactEmail1})
                    Else
                        recipientList.Add(New UserInfo With {.DisplayName = "", .Email = personEmail}) ' recipientList.Add(Users.UserController.GetUserByName(dto.PortalId, personEmail))
                    End If
                Next
            End If

            Dim mm As New Net.Mail.MailMessage() With {.From = New Net.Mail.MailAddress(storeUser.Email, storeUser.DisplayName)}
            mm.ReplyToList.Add(New Net.Mail.MailAddress(theUserInfo.Email, theUserInfo.DisplayName))
            mm.Subject = dto.Subject
            mm.Body = dto.MessageBody + contentStr
            mm.IsBodyHtml = True

            Dim email As New Thread(Sub() PostOffice.SendMail(mm, recipientList, settingsDictionay("smtpServer"), CInt(settingsDictionay("smtpPort")),
                                                              CBool(settingsDictionay("smtpConnection")), settingsDictionay("smtpLogin"),
                                                              settingsDictionay("smtpPassword"))) With {.IsBackground = True}
            email.Start()

            'Using mailMessage As New Net.Mail.MailMessage()
            '    Dim distList As New PostOffice

            '    mailMessage.From = New Net.Mail.MailAddress(storeUser.Email, storeUser.DisplayName)
            '    mailMessage.ReplyToList.Add(New Net.Mail.MailAddress(theUserInfo.Email, theUserInfo.DisplayName))
            '    mailMessage.Subject = dto.Subject
            '    mailMessage.Body = contentStr
            '    'mailMessage.Attachments.Add(New Net.Mail.Attachment(Str, String.Format("Orcamento_{0}_{1}.pdf", CStr(dto.EstimateId), Replace(DateTime.Now.ToString(), ":", "-").Replace("/", "-"))))
            '    mailMessage.IsBodyHtml = True

            '    Dim sentMails = distList.SendMail(mailMessage, recipientList, SettingsDictionay("smtpServer"), CInt(SettingsDictionay("smtpPort")), CBool(SettingsDictionay("smtpConnection")), SettingsDictionay("smtpLogin"), SettingsDictionay("smtpPassword"))
            'End Using

            'For Each toUser In people
            '    Notifications.SendStoreEmail(storeUser, toUser, Nothing, Nothing, dto.Subject, contentStr, Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, dto.PortalId, True)
            'Next

            If dto.HistoryText IsNot Nothing Then
                Dim personHistory As New PersonHistory
                Dim personHistoryCtrl As New PersonHistoriesRepository

                personHistory.PersonId = dto.PersonId
                personHistory.HistoryText = dto.HistoryText
                personHistory.CreatedByUser = dto.CreatedByUser
                personHistory.CreatedOnDate = dto.CreatedOnDate

                personHistoryCtrl.AddPersonHistory(personHistory)
            End If

            Dim personCtrl As New PeopleRepository()
            Dim person = personCtrl.GetPerson(dto.PersonId, dto.PortalId, Null.NullInteger)

            person.Sent = True
            person.ModifiedByUser = dto.CreatedByUser
            person.ModifiedOnDate = dto.CreatedOnDate

            personCtrl.UpdatePerson(person)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
            'Dim session = HttpContext.Current.Session
            'session.Timeout = 20
            'Return New EmptyResult()

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
            logger.[Error](ex)
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
            logger.[Error](ex)
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
    Function GetPortalFiles(portalId As Integer, Optional folder As String = "") As HttpResponseMessage
        Try
            If folder Is Nothing Then
                folder = ""
            End If

            Dim allFiles As New List(Of PortalFile)

            Dim objFolderInfo = FolderManager.Instance.GetFolder(portalId, folder)
            Dim fileList = FolderManager.Instance.GetFiles(objFolderInfo)

            For Each theFile In fileList

                allFiles.Add(New PortalFile() With {
                             .FileId = theFile.FileId,
                             .LastModifiedOnDate = theFile.LastModifiedOnDate,
                             .FileName = theFile.FileName,
                             .FileSize = theFile.Size,
                             .RelativePath = theFile.RelativePath,
                             .PhysicalPath = theFile.PhysicalPath,
                             .Extension = theFile.Extension,
                             .ContentType = theFile.ContentType,
                             .Width = theFile.Width,
                             .Height = theFile.Height})

            Next

            Return Request.CreateResponse(HttpStatusCode.OK, allFiles)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
            logger.[Error](ex)
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

            For Each theFolder In objFolderInfo

                foldersList.Add(New PortalFolder() With {.FolderID = theFolder.FolderID, .FolderName = theFolder.FolderName, .FolderPath = theFolder.FolderPath})

            Next

            Return Request.CreateResponse(HttpStatusCode.OK, foldersList)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets list of roles by role group
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="roleGroupName">Role Group Name</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetRolesByRoleGroup(portalId As Integer, roleGroupName As String) As HttpResponseMessage
        Try
            Dim objRoleCtrl As New RoleController()

            Dim roleGroupList As New ArrayList
            Dim roleGroupNames = roleGroupName.Split(","c)
            For Each roleGroupName In roleGroupNames
                roleGroupList.Add(RoleController.GetRoleGroupByName(portalId, roleGroupName))
            Next

            Dim roles As New ArrayList
            Dim rolesList As New ArrayList
            For Each roleGroup In roleGroupList
                rolesList = objRoleCtrl.GetRolesByGroup(portalId, roleGroup.RoleGroupID)

                For Each role As RoleInfo In rolesList
                    roles.Add(New SubscribedRoles() With {.RoleId = role.RoleID, .RoleName = role.RoleName, .RoleGroupId = role.RoleGroupID, .Description = role.Description})
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
            Next

            'roles.Add(New SubscribedRoles() With {.RoleId = 9999, .RoleName = "Todos"})

            Return Request.CreateResponse(HttpStatusCode.OK, roles)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
            'Dim ps As PortalSettings = PortalController.Instance.GetCurrentPortalSettings()
            Dim rc = New RoleController()

            Dim portalRoles = rc.GetPortalRoles(portalId)

            Dim roles As New ArrayList

            For Each role In portalRoles
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
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets a list of groups by user id and portal id
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous>
    <HttpGet>
    Function GetRolesByUser(portalId As Integer, userId As Integer) As HttpResponseMessage
        Try
            'Dim ps As PortalSettings = PortalController.Instance.GetCurrentPortalSettings()
            Dim rc = New RoleController()

            Dim userRoles = rc.GetUserRoles(UserController.GetUserById(portalId, userId), True)

            Dim roles As New ArrayList

            For Each role In userRoles.ToArray()
                roles.Add(New SubscribedRoles() With {.RoleId = role.RoleID, .RoleName = role.RoleName, .EffectiveDate = role.EffectiveDate, .ExpiryDate = role.ExpiryDate})
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

            'roles.Add(New SubscribedRoles() With {.RoleId = 9999, .RoleName = "Todos"})

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
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds role
    ''' </summary>
    ''' <param name="dto">Roles</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function AddRole(dto As SubscribedRoles) As HttpResponseMessage
        Try

            Dim objRoleCtrl As New RoleController()
            Dim objRoleGroupInfo = RoleController.GetRoleGroupByName(PortalController.Instance.GetCurrentPortalSettings().PortalId, dto.RoleGroup)

            Dim rolesList = objRoleCtrl.GetRolesByGroup(PortalController.Instance.GetCurrentPortalSettings().PortalId, objRoleGroupInfo.RoleGroupID)

            'Dim roles As New ArrayList

            If rolesList.Count > 0 Then
                For Each role As RoleInfo In rolesList.ToArray()
                    If Not role.RoleName.Equals(dto.RoleName) Then
                        Dim newRole As New RoleInfo With {.PortalID = PortalController.Instance.GetCurrentPortalSettings().PortalId, .RoleGroupID = objRoleGroupInfo.RoleGroupID, .RoleName = dto.RoleName, .Description = dto.Description, .IsPublic = False, .Status = RoleStatus.Approved}
                        objRoleCtrl.AddRole(newRole)
                    End If
                Next
            Else
                Dim newRole As New RoleInfo With {.PortalID = PortalController.Instance.GetCurrentPortalSettings().PortalId, .RoleGroupID = objRoleGroupInfo.RoleGroupID, .RoleName = dto.RoleName, .Description = dto.Description, .IsPublic = False, .Status = RoleStatus.Approved}
                objRoleCtrl.AddRole(newRole)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates role
    ''' </summary>
    ''' <param name="dto">Roles</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function UpdateRole(dto As SubscribedRoles) As HttpResponseMessage
        Try

            Dim objRoleCtrl As New RoleController()
            Dim objRoleGroupInfo = RoleController.GetRoleGroupByName(PortalController.Instance.GetCurrentPortalSettings().PortalId, dto.RoleGroup)

            Dim rolesList = objRoleCtrl.GetRolesByGroup(PortalController.Instance.GetCurrentPortalSettings().PortalId, objRoleGroupInfo.RoleGroupID)

            'Dim roles As New ArrayList

            For Each role As RoleInfo In rolesList.ToArray()
                If role.RoleID.Equals(dto.RoleId) Then
                    Dim newRole As New RoleInfo With {.PortalID = role.PortalID, .RoleGroupID = role.RoleGroupID, .RoleID = role.RoleID, .RoleName = dto.RoleName, .Description = dto.Description, .IsPublic = False, .Status = RoleStatus.Approved}
                    objRoleCtrl.UpdateRole(newRole)
                End If
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes role
    ''' </summary>
    ''' <param name="roleId">Roles</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpDelete>
    Function RemoveRole(roleId As Integer, roleGroup As String) As HttpResponseMessage
        Try

            Dim objRoleCtrl As New RoleController()
            Dim objRoleGroupInfo = RoleController.GetRoleGroupByName(PortalController.Instance.GetCurrentPortalSettings().PortalId, roleGroup)

            Dim rolesList = objRoleCtrl.GetRolesByGroup(PortalController.Instance.GetCurrentPortalSettings().PortalId, objRoleGroupInfo.RoleGroupID)

            'Dim roles As New ArrayList

            For Each role As RoleInfo In rolesList.ToArray()
                If role.RoleID.Equals(roleId) Then
                    objRoleCtrl.DeleteRole(role.RoleID, role.PortalID)
                End If
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    <DnnAuthorize> _
    <HttpPost> _
    Function AddUserRole(dto As UserRole) As HttpResponseMessage
        Try
            Dim rc = New RoleController()

            rc.AddUserRole(dto.PortalId, dto.UserId, dto.RoleId, Null.NullDate)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            'Dim objRole As New RoleController()
            'Dim roles = objRole.GetUserRoles(UserController.GetUserById(portalId, uId), True)

            'Return Json(roles, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    <DnnAuthorize>
    <HttpPost>
    Function UpdateUserRole(dto As UserRole) As HttpResponseMessage
        Try
            Dim rc = New RoleController()

            rc.UpdateUserRole(dto.PortalId, dto.UserId, dto.RoleId, dto.Cancel)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            'Dim objRole As New RoleController()
            'Dim roles = objRole.GetUserRoles(UserController.GetUserById(portalId, uId), True)

            'Return Json(roles, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets host settings
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous>
    <HttpGet>
    Function GetPages() As HttpResponseMessage
        Try

            Dim tabs = New Dictionary(Of String, String)()

            For Each t In TabController.GetPortalTabs(0, Null.NullInteger, False, False, False, True)

                tabs.Add(t.TabID.ToString(), t.TabName)

            Next

            Return Request.CreateResponse(HttpStatusCode.OK, tabs)

        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    Public Class StoreSetting
        Property id As Integer
        Property name As String
        Property value As String
    End Class

    Class LoginInfo
        Property portalId As Integer
        Property userName As String
        Property password As String
    End Class

    Class PortalFile
        Property PortalId As Integer
        Property FileId As Integer
        Property FileName As String
        Property RelativePath As String
        Property PhysicalPath As String
        Property Extension As String
        Property FileSize As String
        Property LastModifiedOnDate As DateTime
        Property Width As Integer
        Property Height As Integer
        Property ContentType As String
    End Class

    Public Class PortalFolder
        Property PortalId As Integer
        Property FolderID As Integer
        Property FolderName As String
        Property FolderPath As String
    End Class

    Public Class EmailSend
        Property PortalId As Integer

        Property PersonId As Integer

        Property UserId As Integer

        Property Subject As String

        Property MessageBody As String

        Property Emails As List(Of String)

        Property HistoryText As String

        Property CreatedByUser As String

        Property ContentId As Integer

        Property CreatedOnDate As Date

        Property PersonUserId As Integer
    End Class

    ''' <summary>
    ''' Generates new random user password
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
                              <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPut> _
    Function GenerateUserPassword(dto As UserPasswordInfo) As HttpResponseMessage
        Try

            Dim portalCtrl = New PortalController()
            Dim theUserInfo = UserController.GetUserById(dto.PortalId, dto.UserId)
            Dim newPassword = Utilities.GeneratePassword(7)

            Dim settingCtrl As New SettingsRepository
            Dim settingsDictionay = settingCtrl.GetSettings(dto.PortalId)

            If UserController.ChangePassword(theUserInfo, theUserInfo.Membership.Password, newPassword) Then

                Dim objEventLog As New Log.EventLog.EventLogController
                Dim objEventLogInfo As New Log.EventLog.LogInfo
                objEventLogInfo.AddProperty("IP", Authentication.AuthenticationLoginBase.GetIPAddress())
                objEventLogInfo.LogPortalID = dto.PortalId
                objEventLogInfo.LogPortalName = portalCtrl.GetPortal(dto.PortalId).PortalName
                objEventLogInfo.LogUserID = dto.ModifiedByUser
                objEventLogInfo.LogUserName = theUserInfo.Username
                objEventLogInfo.LogTypeKey = "PASSWORD_ALTERED_SUCCESS"
                objEventLog.AddLog(objEventLogInfo)

                Dim myPortalSettings = PortalController.Instance.GetPortalSettings(dto.PortalId)
                Dim recipientList As New List(Of UserInfo)
                Dim portalEmail = settingsDictionay("storeEmail")
                Dim storeUser As New UserInfo() With {.UserID = Null.NullInteger, .Email = portalEmail, .DisplayName = portalCtrl.GetPortal(dto.PortalId).PortalName}

                recipientList.Add(theUserInfo)

                Dim mm As New Net.Mail.MailMessage() With {.From = New Net.Mail.MailAddress(storeUser.Email, theUserInfo.DisplayName), .Subject = dto.Subject, .Body = dto.MessageBody.Replace("[login]", theUserInfo.Username).Replace("[senha]", UserController.GetPassword(theUserInfo, "")), .IsBodyHtml = True}

                'Dim sentMails = distList.SendMail(mm, recipientList, SettingsDictionay("smtpServer"), CInt(SettingsDictionay("smtpPort")), CBool(SettingsDictionay("smtpConnection")), SettingsDictionay("smtpLogin"), SettingsDictionay("smtpPassword"))

                Dim email As New Thread(Sub() PostOffice.SendMail(mm, recipientList, settingsDictionay("smtpServer"), CInt(settingsDictionay("smtpPort")),
                                                              CBool(settingsDictionay("smtpConnection")), settingsDictionay("smtpLogin"),
                                                              settingsDictionay("smtpPassword"))) With {.IsBackground = True}
                email.Start()

                'Notifications.SendStoreEmail(storeUser, _userInfo, Nothing, Nothing, dto.Subject, dto.MessageBody.Replace("[login]", _userInfo.Username).Replace("[senha]", Users.UserController.GetPassword(_userInfo, "")), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, dto.PortalId, True)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})

            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets host settings
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
                                      <AllowAnonymous> _
    <HttpGet> _
    Function GetAppSettings(portalId As Integer, cultureCode As String) As HttpResponseMessage
        Try
            Dim settingsCtrl As New SettingsRepository

            Dim appSettings = settingsCtrl.GetAppSettings(portalId, cultureCode)

            Return Request.CreateResponse(HttpStatusCode.OK, appSettings)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    Public ReadOnly MediaTypeExtensionMap As New Dictionary(Of String, String)() From {
        {"image/jpeg", "jpg"},
        {"image/png", "png"},
        {"image/gif", "gif"},
        {"application/pdf", "pdf"},
        {"application/x-zip-compressed", "zip"},
        {"application/msword", "doc"},
        {"text/xml", "xml"},
        {"application/vnd.openxmlformats-officedocument.wordprocessingml.document", "docx"},
        {"application/vnd.ms-excel", "xls"},
        {"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "xlsx"}
    }

    Public Class UserRole
        Property PortalId As Integer
        Property UserId As Integer
        Property RoleId As Integer
        Property Cancel As Boolean
    End Class

    Public Class MyPortalInfo
        Property PortalId As Integer
        Property PortalName As String
        Property LogoFile As String
        Property Description As String
        Property KeyWords As String
    End Class

    Public Class SubscribedRoles
        Property RoleId As Integer
        Property RoleName As String
        Property RoleGroupId As Integer
        Property RoleGroup As String
        Property Subscribed As Boolean
        Property Description As String
        Property EffectiveDate As DateTime
        Property ExpiryDate As DateTime
    End Class

    Public Class UserPasswordInfo
        Property PortalId As Integer
        Property UserId As Integer
        Property CurrentPassword As String
        Property NewPassword As String
        Property Subject As String
        Property MessageBody As String
        Property SendPassword As Boolean
        Property ModifiedByUser As Integer
        Property ModifiedOnDate As DateTime
    End Class

End Class
