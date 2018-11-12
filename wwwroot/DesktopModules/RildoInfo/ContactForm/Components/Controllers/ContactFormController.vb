Imports RIW.Modules.WebAPI.Components.Models
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Instrumentation
Imports System.Web.Http
Imports System.Threading.Tasks
Imports RIW.Modules.Common

Public Class ContactFormController
    Inherits DnnApiController

    Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(ContactFormController))

    ''' <summary>
    ''' Updates tab module settings
    ''' </summary>
    ''' <param name="tabSettings"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPut> _
    Function updateTabModuleSettings(tabSettings As List(Of StorePortalModuleSetting)) As HttpResponseMessage
        Try
            Dim objModules As New Entities.Modules.ModuleController

            For Each setting In tabSettings

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
    ''' Gets doc
    ''' </summary>
    ''' <param name="docId">Doc ID</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function getDoc(docId As Integer, portalId As Integer) As HttpResponseMessage
        Try
            Dim docCtrl As New DocsRepository

            Dim docData = docCtrl.getDoc(docId, portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, docData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets list of docs
    ''' </summary>
    ''' <param name="portalId">Person Doc ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpGet> _
    Function getDocs(portalId As Integer) As HttpResponseMessage
        Try
            Dim docCtrl As New DocsRepository

            Dim docsData = docCtrl.getDocs(portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, docsData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Saves person documents
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpPost> _
    Async Function postFile() As Task(Of HttpResponseMessage)
        Try
            If Not Me.Request.Content.IsMimeMultipartContent("form-data") Then
                Throw New HttpResponseException(New HttpResponseMessage(HttpStatusCode.UnsupportedMediaType))
            End If

            Dim portalCtrl = New Portals.PortalController()
            Dim request As HttpRequestMessage = Me.Request

            Await request.Content.LoadIntoBufferAsync()
            Dim task = request.Content.ReadAsMultipartAsync()
            Dim result = Await task
            Dim contents = result.Contents
            Dim httpContent As HttpContent = contents.Last()
            Dim uploadedFileMediaType As String = httpContent.Headers.ContentType.MediaType

            Dim doc As New Models.Doc()
            Dim docCtrl As New DocsRepository()

            doc.PortalId = contents(0).ReadAsStringAsync().Result
            doc.DocName = contents(1).ReadAsStringAsync().Result
            doc.DocDesc = contents(2).ReadAsStringAsync().Result
            doc.CreatedByUser = contents(3).ReadAsStringAsync().Result
            doc.CreatedOnDate = contents(4).ReadAsStringAsync().Result

            Dim root = String.Format("{0}{1}", portalCtrl.GetPortal(doc.PortalId).HomeDirectoryMapPath, "Catalogs")

            Utilities.CreateDir(Utilities.GetPortalSettings(doc.PortalId), "Catalogs")

            Dim _file As Stream = httpContent.ReadAsStreamAsync().Result

            Dim guid_1 = Guid.NewGuid().ToString()

            Dim _fileName = String.Format("{0}.{1}", guid_1, mediaTypeExtensionMap(uploadedFileMediaType))
            Dim _filePath = Path.Combine(root, _fileName)

            If _file.CanRead Then
                Using _fileStream As New FileStream(_filePath, FileMode.Create)
                    _file.CopyTo(_fileStream)
                End Using
            End If

            DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(doc.PortalId)

            Dim objFolderInfo = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetFolder(doc.PortalId, "Catalogs")
            Dim objFileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(objFolderInfo, _fileName)

            doc.FileId = objFileInfo.FileId

            docCtrl.addDoc(doc)

            Return request.CreateResponse(HttpStatusCode.Created, New With {.Result = "success", .DocId = doc.DocId, .Extension = objFileInfo.Extension})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message}))
        End Try
    End Function

    ''' <summary>
    ''' Removes a person Doc
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="docId">Doc ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpDelete> _
    Function removeDoc(portalId As Integer, docId As Integer) As HttpResponseMessage
        Try
            Dim docCtrl As New DocsRepository

            ' Todo: add function to remove user's file
            docCtrl.removeDoc(docId, portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    <DnnAuthorize> _
    <HttpPost> _
    Function sendCatalogs(dto As ContactMessage) As HttpResponseMessage
        Try
            Dim portalCtrl = New Portals.PortalController()
            Dim catalogCtrl As New DocsRepository
            Dim mCtrl As New DotNetNuke.Entities.Modules.ModuleController()
            Dim mSettings = mCtrl.GetTabModuleSettings(mCtrl.GetModuleByDefinition(dto.PortalId, "RIW Contact Form").TabModuleID)

            Dim sender As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = CStr(mSettings("RIW_Contacts_SendTo")), .DisplayName = portalCtrl.GetPortal(dto.PortalId).PortalName}

            Dim distList1 As New PostOffice
            Dim distList2 As New PostOffice
            Dim distList3 As New PostOffice
            Dim distList4 As New PostOffice

            '' Email to administrators
            Dim recipientListToAdmin As New List(Of Users.UserInfo)
            recipientListToAdmin.Add(New Users.UserInfo With {.UserID = Null.NullInteger, .Email = sender.Email, .DisplayName = sender.DisplayName})

            '' Email to user
            Dim recipientListToUser As New List(Of Users.UserInfo)
            recipientListToUser.Add(New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = dto.Email, .DisplayName = dto.DisplayName})

            Dim emailMessageToAdmin1 As New Net.Mail.MailMessage With {.From = New Net.Mail.MailAddress(sender.Email, sender.DisplayName), .IsBodyHtml = True}
            Dim emailMessageToAdmin2 As New Net.Mail.MailMessage With {.From = New Net.Mail.MailAddress(sender.Email, sender.DisplayName), .IsBodyHtml = True}
            Dim emailMessageToUser1 As New Net.Mail.MailMessage With {.From = New Net.Mail.MailAddress(sender.Email, sender.DisplayName), .IsBodyHtml = True}
            Dim emailMessageToUser2 As New Net.Mail.MailMessage With {.From = New Net.Mail.MailAddress(sender.Email, sender.DisplayName), .IsBodyHtml = True}

            Dim msgToAdmin = ""
            Dim composeMsgUser = ""

            '' Compose messages
            If dto.EmailMethodType OrElse dto.POMethodType Then

                If dto.EmailMethodType Then
                    composeMsgUser = ""
                    composeMsgUser = CStr(mSettings("RIW_Contacts_emailMessage")).Replace("[CLIENTE]", dto.DisplayName)
                    composeMsgUser = composeMsgUser.Replace("[WEBSITELINK]", "<a href=http://" & Utilities.GetPortalSettings(dto.PortalId).PortalAlias.HTTPAlias & ">" & portalCtrl.GetPortal(dto.PortalId).PortalName & "</a>")

                    emailMessageToUser1.ReplyToList.Add(New Net.Mail.MailAddress(sender.Email, sender.DisplayName))
                    emailMessageToUser1.Subject = "Materiais requisitados no website " & Utilities.GetPortalSettings(dto.PortalId).PortalAlias.HTTPAlias
                    emailMessageToUser1.Body = composeMsgUser

                    Dim catalogs = catalogCtrl.getDocs(dto.PortalId)

                    Dim catalogNames = ""
                    For Each catalog In catalogs
                        If dto.Catalogs.Contains(catalog.DocId) Then
                            catalogNames += catalog.DocName & "<br />"
                            Dim catalogFile = DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(catalog.FileId)
                            emailMessageToUser1.Attachments.Add(New Net.Mail.Attachment(catalogFile.PhysicalPath))
                        End If
                    Next

                    Dim sentMailsToUser1 = distList1.SendMail(emailMessageToUser1, recipientListToUser, CStr(mSettings("RIW_Contacts_smtpServer")), CInt(mSettings("RIW_Contacts_smtpPort")), CBool(mSettings("RIW_Contacts_smtpConnection")), CStr(mSettings("RIW_Contacts_smtpLogin")), CStr(mSettings("RIW_Contacts_smtpPassword")))

                    msgToAdmin = ""
                    msgToAdmin = String.Format("O seguinte material foi solicitado e enviado para...<br /><br />Nome: {0}<br />Empresa: {1}<br />" &
                                               "Email: {2}<br />Telefone: {3}<br />Website: {4}",
                                               dto.DisplayName.ToUpper(), dto.Company.ToUpper(), dto.Email.ToLower(), dto.Telephone, dto.Website.ToLower())

                    msgToAdmin = msgToAdmin & CStr(IIf(dto.Address.Length > 0, String.Format("<br /><br />{0} Nº {1}<br />{2}<br />{3}<br />{4}<br />{5}<br />{6} {7}",
                                               dto.Address, dto.Unit, dto.Complement, dto.District, dto.City, dto.State, dto.PostalCode, dto.Country), ""))

                    msgToAdmin = msgToAdmin & String.Format("<br /><br />Mensagem:<br /><br />{0}<br /><br />Materiais:<br /><br />{1}",
                                               dto.Message, catalogNames)

                    emailMessageToAdmin1.ReplyToList.Add(New Net.Mail.MailAddress(dto.Email, dto.DisplayName))
                    emailMessageToAdmin1.Subject = CStr(IIf(dto.Catalogs.Count > 0, "Materiais requisitados no website " & Utilities.GetPortalSettings(dto.PortalId).PortalAlias.HTTPAlias, "Novo contato no website " & Utilities.GetPortalSettings(dto.PortalId).PortalAlias.HTTPAlias))
                    emailMessageToAdmin1.Body = msgToAdmin

                    Dim sentMailsToAdmin1 = distList2.SendMail(emailMessageToAdmin1, recipientListToAdmin, CStr(mSettings("RIW_Contacts_smtpServer")), CInt(mSettings("RIW_Contacts_smtpPort")), CBool(mSettings("RIW_Contacts_smtpConnection")), CStr(mSettings("RIW_Contacts_smtpLogin")), CStr(mSettings("RIW_Contacts_smtpPassword")))

                End If

                If dto.POMethodType Then

                    Dim catalogs = catalogCtrl.getDocs(dto.PortalId)

                    Dim catalogNames = ""
                    For Each catalog In catalogs
                        If dto.Catalogs.Contains(catalog.DocId) Then
                            catalogNames += catalog.DocName & "<br />"
                        End If
                    Next

                    composeMsgUser = ""
                    composeMsgUser = CStr(mSettings("RIW_Contacts_poMessage")).Replace("[CLIENTE]", dto.DisplayName)
                    composeMsgUser = composeMsgUser.Replace("[ENDERECO]", String.Format("Os Materiais serão enviados para o seguinte endereço:<br /><br />{0} Nº {1}<br />{2}<br />{3}<br />{4} {5}<br />{6}<br />{7}<br /><br />Materiais:<br /><br />{8}", dto.Address, dto.Unit, dto.Complement, dto.District, dto.City, dto.State, dto.PostalCode, dto.Country, catalogNames))
                    composeMsgUser = composeMsgUser.Replace("[WEBSITELINK]", "<a href=http://" & Utilities.GetPortalSettings(dto.PortalId).PortalAlias.HTTPAlias & ">" & portalCtrl.GetPortal(dto.PortalId).PortalName & "</a>")

                    emailMessageToUser2.ReplyToList.Add(New Net.Mail.MailAddress(sender.Email, sender.DisplayName))
                    emailMessageToUser2.Subject = "Materiais requisitados no website " & Utilities.GetPortalSettings(dto.PortalId).PortalAlias.HTTPAlias
                    emailMessageToUser2.Body = composeMsgUser

                    Dim sentMailsToUser1 = distList3.SendMail(emailMessageToUser2, recipientListToUser, CStr(mSettings("RIW_Contacts_smtpServer")), CInt(mSettings("RIW_Contacts_smtpPort")), CBool(mSettings("RIW_Contacts_smtpConnection")), CStr(mSettings("RIW_Contacts_smtpLogin")), CStr(mSettings("RIW_Contacts_smtpPassword")))

                    msgToAdmin = ""
                    msgToAdmin = String.Format("Os seguintes materiais foram solicitados por...<br /><br />Nome: {0}<br />Empresa: {1}<br />Email: {2}<br />" &
                                               "Telefone: {3}<br />Website: {4}<br /><br />{5}<br />O(s) seguinte(s) catálago(s) deve(m) ser enviado(s)" &
                                               "via correio para o endereço abaixo:<br /><br />{6} Nº {7}<br />{8}<br />{9}<br />{10}<br />{11}<br />{12}<br />" &
                                               "<br />Mensagem:<br /><br />{13}<br /><br />Materiais:<br /><br />{14}",
                                               dto.DisplayName.ToUpper(), dto.Company.ToUpper(), dto.Email.ToLower(), dto.Telephone, dto.Website.ToLower(),
                                               dto.Address, dto.Unit, dto.Complement, dto.District, dto.City, dto.State, dto.PostalCode, dto.Country, dto.Message, catalogNames)

                    emailMessageToAdmin2.ReplyToList.Add(New Net.Mail.MailAddress(dto.Email, dto.DisplayName))
                    emailMessageToAdmin2.Subject = CStr(IIf(dto.Catalogs.Count > 0, "Materiais requisitados no website " & Utilities.GetPortalSettings(dto.PortalId).PortalAlias.HTTPAlias, "Novo contato no website " & Utilities.GetPortalSettings(dto.PortalId).PortalAlias.HTTPAlias))
                    emailMessageToAdmin2.Body = msgToAdmin

                    Dim sentMailsToAdmin2 = distList4.SendMail(emailMessageToAdmin2, recipientListToAdmin, CStr(mSettings("RIW_Contacts_smtpServer")), CInt(mSettings("RIW_Contacts_smtpPort")), CBool(mSettings("RIW_Contacts_smtpConnection")), CStr(mSettings("RIW_Contacts_smtpLogin")), CStr(mSettings("RIW_Contacts_smtpPassword")))

                End If
            Else
                composeMsgUser = ""
                composeMsgUser = CStr(mSettings("RIW_Contacts_autoAnswer")).Replace("[CLIENTE]", dto.DisplayName)
                composeMsgUser = composeMsgUser.Replace("[WEBSITELINK]", String.Format("<a href=http://{0}>{1}</a>", Utilities.GetPortalSettings(dto.PortalId).PortalAlias.HTTPAlias, portalCtrl.GetPortal(dto.PortalId).PortalName))

                emailMessageToUser1.ReplyToList.Add(New Net.Mail.MailAddress(sender.Email, sender.DisplayName))
                emailMessageToUser1.Subject = "Seu contato no website " & Utilities.GetPortalSettings(dto.PortalId).PortalAlias.HTTPAlias
                emailMessageToUser1.Body = composeMsgUser

                Dim sentMailsToUser1 = distList1.SendMail(emailMessageToUser1, recipientListToUser, CStr(mSettings("RIW_Contacts_smtpServer")), CInt(mSettings("RIW_Contacts_smtpPort")), CBool(mSettings("RIW_Contacts_smtpConnection")), CStr(mSettings("RIW_Contacts_smtpLogin")), CStr(mSettings("RIW_Contacts_smtpPassword")))

                emailMessageToAdmin1.ReplyToList.Add(New Net.Mail.MailAddress(dto.Email, dto.DisplayName))
                emailMessageToAdmin1.Subject = "Novo contato no website " & Utilities.GetPortalSettings(dto.PortalId).PortalAlias.HTTPAlias

                msgToAdmin = String.Format("Nome: {0}<br />Empresa: {1}<br />Email: {2}<br />Telefone: {3}<br />Website: {4}",
                                               dto.DisplayName.ToUpper(), dto.Company.ToUpper(), dto.Email.ToLower(), dto.Telephone, dto.Website.ToLower())

                msgToAdmin = msgToAdmin & CStr(IIf(dto.Address.Length > 0, String.Format("<br /><br />{0} Nº {1}<br />{2}<br />{3}<br />{4}<br />{5}<br />{6} {7}",
                                                                                         dto.Address, dto.Unit, dto.Complement, dto.District, dto.City,
                                                                                         dto.State, dto.PostalCode, dto.Country), ""))

                msgToAdmin = msgToAdmin & String.Format("<br /><br />Mensagem:<br /><br />{0}", dto.Message)

                emailMessageToAdmin1.Body = msgToAdmin

                Dim sentMailsToAdmin1 = distList2.SendMail(emailMessageToAdmin1, recipientListToAdmin, CStr(mSettings("RIW_Contacts_smtpServer")), CInt(mSettings("RIW_Contacts_smtpPort")), CBool(mSettings("RIW_Contacts_smtpConnection")), CStr(mSettings("RIW_Contacts_smtpLogin")), CStr(mSettings("RIW_Contacts_smtpPassword")))

            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    Private ReadOnly mediaTypeExtensionMap As New Dictionary(Of String, String)() From {
        {"image/jpeg", "jpg"},
        {"image/png", "png"},
        {"image/gif", "gif"},
        {"application/pdf", "pdf"},
        {"application/x-zip-compressed", "zip"},
        {"application/msword", "doc"},
        {"application/vnd.openxmlformats-officedocument.wordprocessingml.document", "docx"},
        {"application/vnd.ms-excel", "xls"},
        {"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "xlsx"}
    }

    ''' <summary>
    ''' Store / Portal / Module Settings Variables
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StorePortalModuleSetting
        Public Property id As Integer
        Public Property name As String
        Public Property value As String
    End Class

    Public Class ContactMessage
        Property Catalogs As IEnumerable(Of Integer)
        Property PortalId As Integer
        Property EmailMethodType As Boolean
        Property POMethodType As Boolean
        Property Company As String
        Property DisplayName As String
        Property Telephone As String
        Property Email As String
        Property Website As String
        Property Message As String
        Property PostalCode As String
        Property Address As String
        Property Unit As String
        Property Complement As String
        Property District As String
        Property City As String
        Property State As String
        Property Country As String
        Property TabModuleId As Integer
    End Class
End Class
