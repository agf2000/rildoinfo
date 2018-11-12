
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Security.Membership
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Web.Api
Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Drawing
Imports System.Threading.Tasks
Imports Microsoft.AspNet.SignalR.Hubs
Imports Microsoft.AspNet.SignalR

Public Class PeopleController
    Inherits DnnApiControllerWithHub
    'Inherits DnnApiController

    Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(AccountsController))

    ''' <summary>
    ''' Checks for an existing email from the client's table
    ''' </summary>
    ''' <param name="email"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpGet> _
    Function ValidatePerson(email As String) As HttpResponseMessage
        Try
            Dim _data = Services.ValidatePerson(email)
            Return Request.CreateResponse(HttpStatusCode.OK, _data)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets list of clients
    ''' </summary>
    ''' <param name="salesRep">Sales Person ID</param>
    ''' <param name="isDeleted">Has been set as deleted</param>
    ''' <param name="sTerm">Persons Search Term</param>
    ''' <param name="statusId">Status ID</param>
    ''' <param name="sDate">Start ModifiedOnDate</param>
    ''' <param name="eDate">End Modified Date Range</param>
    ''' <param name="pageIndex">Page Number</param>
    ''' <param name="pageSize">Page Size</param>
    ''' <param name="orderBy">Sorting Order</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getPeople(
            Optional pageIndex As Integer = 1,
            Optional pageSize As Integer = 10,
            Optional orderBy As String = "",
            Optional portalId As Integer = -1,
            Optional salesRep As Integer = -1,
            Optional isDeleted As String = "",
            Optional sTerm As String = "",
            Optional statusId As Integer = -1,
            Optional registerType As String = "",
            Optional sDate As String = Nothing,
            Optional eDate As String = Nothing) As HttpResponseMessage
        Try
            Dim searchStr As String = HttpContext.Current.Request.Params("filter[filters][0][value]")
            If sTerm Is Nothing Then
                If searchStr Is Nothing Then
                    searchStr = ""
                End If
            Else
                searchStr = sTerm
            End If

            If sDate = Nothing Then
                sDate = CStr(Null.NullDate)
            End If

            If eDate = Nothing Then
                eDate = CStr(Null.NullDate)
            End If

            'If sRep Is Nothing Then
            '    sRep = ""
            'End If

            'If sId Is Nothing Then
            '    sId = ""
            'End If

            If registerType Is Nothing Then
                registerType = ""
            End If

            Dim peopleDataCtrl As New PeopleRepository
            Dim peopleData = peopleDataCtrl.getPeople(portalId, salesRep, isDeleted, searchStr, statusId, registerType, CDate(sDate), CDate(eDate), pageIndex, pageSize, orderBy)

            Dim total = Nothing
            For Each item In peopleData
                total = item.TotalRows
                Exit For
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.data = peopleData, .total = total})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client info
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    <DnnAuthorize> _
    <HttpGet> _
    Function getPerson(personId As Integer, Optional portalId As Integer = 0) As HttpResponseMessage
        Try
            Dim personCtrl As New PeopleRepository

            Dim person = personCtrl.getPerson(personId, portalId, Null.NullInteger)

            Return Request.CreateResponse(HttpStatusCode.OK, person)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates a person account
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpPost> _
    Function updatePerson(dto As Models.Person) As HttpResponseMessage
        Try
            Dim person As New Models.Person
            Dim personHistory As New Models.PersonHistory
            Dim personHistoryCtrl As New PersonHistoriesRepository
            Dim address As New Models.PersonAddress
            Dim personCtrl As New PeopleRepository
            Dim addressCtrl As New PersonAddressesRepository

            Dim defaultStatus As New Models.Status
            Dim defaultStatusCtrl As New StatusesRepository

            If dto.PersonId > 0 Then
                person = personCtrl.getPerson(dto.PersonId, PortalController.GetCurrentPortalSettings().PortalId, Null.NullInteger)
                address = addressCtrl.getPersonAddresses(dto.PersonId)(0)
            End If

            person.DateFounded = dto.DateFounded
            person.DateRegistered = dto.DateRegistered
            person.PortalId = dto.PortalId
            person.PersonType = dto.PersonType
            person.EIN = dto.EIN
            person.CPF = dto.CPF
            person.SalesRep = dto.SalesRep
            person.CreatedByUser = dto.CreatedByUser
            person.CreatedOnDate = dto.CreatedOnDate
            person.ModifiedByUser = dto.ModifiedByUser
            person.ModifiedOnDate = dto.ModifiedOnDate
            person.DisplayName = ""

            If dto.CompanyName IsNot Nothing Then
                person.CompanyName = dto.CompanyName
                person.DisplayName = dto.CompanyName
                If dto.FirstName IsNot Nothing Then
                    person.DisplayName += String.Format("{0}{1}", Space(1), dto.FirstName)
                    If dto.LastName IsNot Nothing Then
                        person.DisplayName += String.Format("{0}{1}", Space(1), dto.LastName)
                    End If
                End If
            Else
                If dto.FirstName IsNot Nothing Then
                    person.FirstName = dto.FirstName
                    person.DisplayName += dto.FirstName
                    If dto.LastName IsNot Nothing Then
                        person.LastName = dto.LastName
                        person.DisplayName += String.Format("{0}{1}", Space(1), dto.LastName)
                    End If
                End If
            End If

            person.Telephone = dto.Telephone
            person.Cell = dto.Cell
            person.Fax = dto.Fax
            person.Zero800s = dto.Zero800s
            person.Email = dto.Email
            person.Website = dto.Website
            person.Ident = dto.Ident
            person.StateTax = dto.StateTax
            person.CityTax = dto.CityTax
            person.Comments = dto.Comments
            person.RegisterTypes = dto.RegisterTypes.Replace("[", "").Replace("]", "")

            If dto.PersonId > 0 Then
                personCtrl.updatePerson(person)

                personHistory.PersonId = dto.PersonId
                personHistory.HistoryText = "<p>Informações da conta alterada.</p>"
                personHistory.CreatedByUser = -1
                personHistory.CreatedOnDate = dto.CreatedOnDate

                personHistoryCtrl.addPersonHistory(personHistory)
            Else

                defaultStatus = defaultStatusCtrl.getStatus("Normal", dto.PortalId)

                If defaultStatus IsNot Nothing Then
                    person.StatusId = defaultStatus.StatusId
                Else
                    person.StatusId = defaultStatusCtrl.getStatuses(dto.PortalId, "False")(0).StatusId
                End If

                personCtrl.addPerson(person)

                personHistory.PersonId = person.PersonId
                personHistory.HistoryText = "<p>Conta gerada.</p>"
                personHistory.CreatedByUser = -1
                personHistory.CreatedOnDate = dto.CreatedOnDate

                personHistoryCtrl.addPersonHistory(personHistory)

                address.PersonId = person.PersonId
                address.Street = dto.Street
                address.Unit = dto.Unit
                address.Complement = dto.Complement
                address.District = dto.District
                address.City = dto.City
                address.Region = dto.Region
                address.PostalCode = dto.PostalCode
                address.Telephone = dto.Telephone
                address.Cell = dto.Cell
                address.Country = dto.Country
                address.ModifiedByUser = dto.CreatedByUser
                address.ModifiedOnDate = dto.CreatedOnDate
                address.AddressName = "Principal"
                address.ViewOrder = 1
                address.CreatedByUser = dto.CreatedByUser
                address.CreatedOnDate = dto.CreatedOnDate

                If address.Street <> "" Then
                    addressCtrl.addPersonAddress(address)
                End If

                'contact.PersonId = person.PersonId
                'contact.PersonAddressId = address.PersonAddressId
                'contact.DateBirth = CDate("01/01/1900")
                'contact.CreatedByUser = dto.CreatedByUser
                'contact.CreatedOnDate = dto.CreatedOnDate

                'contactCtrl.addPersonContact(contact)
            End If

            Dim newUId = Null.NullInteger

            If person.PersonId > 0 Then

                If dto.Industries IsNot Nothing Then
                    If dto.Industries <> "[]" Then
                        dto.Industries = dto.Industries.Replace("[", "").Replace("]", "")
                        Dim indusPerson As New Models.PersonIndustry
                        Dim indusCtrl As New PersonIndustryRepository
                        indusCtrl.removePersonIndustries(person.PersonId)
                        For Each item In dto.Industries.Split(","c)
                            indusPerson.PersonId = person.PersonId
                            'indusPerson.IndustryId = item
                            indusCtrl.addPersonIndustry(indusPerson)
                        Next
                    End If
                End If

                If dto.CreateLogin Then
                    If dto.Email IsNot Nothing Then
                        Dim oUserInfo As New Users.UserInfo() With {.PortalID = PortalController.GetCurrentPortalSettings().PortalId,
                                     .Username = dto.Email.ToLower(),
                                     .FirstName = dto.FirstName.ToLower(),
                                     .LastName = dto.LastName.ToLower(),
                                     .Email = dto.Email.ToLower(),
                                     .AffiliateID = Null.NullInteger,
                                     .LastIPAddress = DotNetNuke.Services.Authentication.AuthenticationLoginBase.GetIPAddress()}

                        Dim pass = ""
                        If dto.Password IsNot Nothing Then
                            pass = dto.Password
                        Else
                            pass = Utilities.GeneratePassword(7)
                        End If

                        oUserInfo.Membership.Approved = True
                        oUserInfo.Membership.Password = pass
                        oUserInfo.Membership.UpdatePassword = False
                        oUserInfo.DisplayName = person.DisplayName

                        Dim objUserCreateStatus = Users.UserController.CreateUser(oUserInfo)

                        If objUserCreateStatus = UserCreateStatus.Success Then

                            'set roles here
                            Dim objRoleCtrl As New RoleController
                            Dim objRoleInfo = objRoleCtrl.GetRoleByName(PortalController.GetCurrentPortalSettings().PortalId, "Clientes")
                            objRoleCtrl.AddUserRole(PortalController.GetCurrentPortalSettings().PortalId, oUserInfo.UserID, objRoleInfo.RoleID, Null.NullDate)
                            objRoleCtrl.AddUserRole(PortalController.GetCurrentPortalSettings().PortalId, oUserInfo.UserID, PortalSettings.RegisteredRoleId, Null.NullDate)

                            oUserInfo.Profile.SetProfileProperty("Country", dto.Country)
                            oUserInfo.Profile.SetProfileProperty("Photo", "0")
                            oUserInfo.Profile.SetProfileProperty("PreferredTimeZone", "E. South America Standard Time")
                            oUserInfo.Profile.SetProfileProperty("PreferredLocale", "pt-BR")

                            DotNetNuke.Entities.Profile.ProfileController.UpdateUserProfile(oUserInfo)

                            person.UserId = oUserInfo.UserID

                            personCtrl.updatePerson(person)

                            DotNetNuke.Common.Utilities.DataCache.ClearCache()

                            newUId = oUserInfo.UserID
                        Else
                            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Não possível criar o login. Email inválido ou já cadastrado.", .PersonId = person.PersonId})
                        End If
                    Else
                        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Não possível criar o login. Email inválido ou já cadastrado.", .PersonId = person.PersonId})
                    End If
                End If
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .PersonId = person.PersonId, .UserId = newUId, .DisplayName = person.DisplayName})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Deletes a person
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPut> _
    Public Function deletePerson(dto As Models.Person) As HttpResponseMessage
        Try
            Dim person As Models.Person
            Dim personCltr As New PeopleRepository

            person = personCltr.getPerson(dto.PersonId, PortalController.GetCurrentPortalSettings().PortalId, Null.NullInteger)

            person.IsDeleted = True
            person.ModifiedByUser = dto.ModifiedByUser
            person.ModifiedOnDate = dto.ModifiedOnDate

            personCltr.updatePerson(person)

            If dto.UserId > 0 Then
                Users.UserController.DeleteUser(Users.UserController.GetUserById(dto.PortalId, dto.UserId), False, False)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes a person by person id
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <param name="userId">User ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpDelete> _
    Function removePerson(personId As Integer, Optional userId As Integer = -1, Optional portalId As Integer = 0) As HttpResponseMessage
        Try
            Dim personCtrl As New PeopleRepository
            Dim personAddressCtrl As New PersonAddressesRepository
            Dim clientBankRefCtrl As New ClientBankRefsRepository
            Dim clientCommRefCtrl As New ClientCommRefsRepository
            Dim personContactCtrl As New PersonContactsRepository
            Dim personDocCtrl As New PersonDocsRepository
            Dim personHistoryCtrl As New PersonHistoriesRepository
            Dim clientIncomeSourceCtrl As New ClientIncomeSourcesRepository
            Dim personIndustryCtrl As New PersonIndustryRepository
            Dim clientPartnerCtrl As New ClientPartnersRepository
            Dim clientPartnerBankRefCtrl As New ClientPartnersBankRefsRepository
            Dim clientPersonalRefCtrl As New ClientPersonalRefsRepository
            Dim personEstimateCtrl As New EstimatesRepository

            personAddressCtrl.removePersonAddresses(personId)
            clientBankRefCtrl.removeClientBankRefs(personId)
            clientCommRefCtrl.removeClientCommRefs(personId)
            personContactCtrl.removePersonContacts(personId)
            personDocCtrl.removePersonDocs(personId)
            personHistoryCtrl.removePersonHistories(personId)
            clientIncomeSourceCtrl.removeClientIncomeSources(personId)
            personIndustryCtrl.removePersonIndustries(personId)
            clientPartnerCtrl.removeClientPartners(personId)
            clientPartnerBankRefCtrl.removeClientPartnerBankRefs(personId)
            clientPersonalRefCtrl.removeClientPersonalRefs(personId)
            personEstimateCtrl.RemoveClientEstimates(personId, portalId)
            personCtrl.removePerson(personId, portalId, userId)

            If userId > 0 Then
                Dim userInfo = Users.UserController.GetUserById(portalId, userId)
                If Not Null.IsNull(userInfo) Then
                    If Users.UserController.RemoveUser(userInfo) = False Then
                        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Erro ao excluir o cadastro de login."})
                    End If
                End If
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Restores a person
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPut> _
    Public Function restorePerson(dto As Models.Person) As HttpResponseMessage
        Try
            Dim person As Models.Person
            Dim personCltr As New PeopleRepository

            person = personCltr.getPerson(dto.PersonId, dto.PortalId, Null.NullInteger)

            person.IsDeleted = False
            person.ModifiedByUser = dto.ModifiedByUser
            person.ModifiedOnDate = dto.ModifiedOnDate

            personCltr.updatePerson(person)

            If dto.UserId > 0 Then
                Users.UserController.RestoreUser(Users.UserController.GetUserById(dto.PortalId, dto.UserId))
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Generates new random user password
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPut> _
    Function generateUserPassword(dto As userPassword) As HttpResponseMessage
        Try

            Dim portalCtrl = New Portals.PortalController()
            Dim _userInfo = Users.UserController.GetUserById(dto.PortalId, dto.UserId)
            Dim newPassword = Utilities.GeneratePassword(7)

            If Users.UserController.ChangePassword(_userInfo, _userInfo.Membership.Password, newPassword) Then

                Dim objEventLog As New Log.EventLog.EventLogController
                Dim objEventLogInfo As New Log.EventLog.LogInfo
                objEventLogInfo.AddProperty("IP", Utilities.GetIPAddress())
                objEventLogInfo.LogPortalID = dto.PortalId
                objEventLogInfo.LogPortalName = portalCtrl.GetPortal(dto.PortalId).PortalName
                objEventLogInfo.LogUserID = dto.ModifiedByUser
                objEventLogInfo.LogUserName = _userInfo.Username
                objEventLogInfo.LogTypeKey = "PASSWORD_ALTERED_SUCCESS"
                objEventLog.AddLog(objEventLogInfo)

                Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(dto.PortalId)
                Dim recipientList As New List(Of Users.UserInfo)
                Dim _portalEmail = Portals.PortalController.GetPortalSetting("RIW_StoreEmail", dto.PortalId, portalCtrl.GetPortal(dto.PortalId).Email)
                Dim _portalName = portalCtrl.GetPortal(dto.PortalId).PortalName
                Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = _portalEmail, .DisplayName = _portalName}

                Dim mailMessage As New Net.Mail.MailMessage
                Dim distList As New PostOffice

                recipientList.Add(_userInfo)

                mailMessage.From = New Net.Mail.MailAddress(storeUser.Email, _userInfo.DisplayName)
                'mailMessage.ReplyToList.Add(New Net.Mail.MailAddress(_userInfo.Email, _userInfo.DisplayName))
                mailMessage.Subject = dto.Subject
                mailMessage.Body = dto.MessageBody.Replace("[login]", _userInfo.Username).Replace("[senha]", Users.UserController.GetPassword(_userInfo, ""))
                'mailMessage.Attachments.Add(New Net.Mail.Attachment(Str, String.Format("Orcamento_{0}_{1}.pdf", CStr(dto.EstimateId), Replace(DateTime.Now.ToString(), ":", "-").Replace("/", "-"))))
                mailMessage.IsBodyHtml = True

                Dim sentMails = distList.SendMail(mailMessage, recipientList, myPortalSettings("RIW_SMTPServer"), CInt(myPortalSettings("RIW_SMTPPort")), CBool(myPortalSettings("RIW_SMTPConnection")), myPortalSettings("RIW_SMTPLogin"), myPortalSettings("RIW_SMTPPassword"))

                'Notifications.SendStoreEmail(storeUser, _userInfo, Nothing, Nothing, dto.Subject, dto.MessageBody.Replace("[login]", _userInfo.Username).Replace("[senha]", Users.UserController.GetPassword(_userInfo, "")), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, dto.PortalId, True)

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
    ''' Gets user photo by user id
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="userId">User ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getUserPhoto(portalId As Integer, userId As Integer) As HttpResponseMessage
        Try

            Dim portalCtrl As New Portals.PortalController()
            Dim oUser As Users.UserInfo = Users.UserController.GetUserById(portalId, userId)
            Dim userPhoto = oUser.Profile.GetPropertyValue("Photo")
            Dim objFileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(CInt(userPhoto))

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .fileName = objFileInfo.FileName, .filePath = objFileInfo.RelativePath})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPost> _
    Async Function saveUserPhoto() As Task(Of HttpResponseMessage)
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
            Dim httpContent As HttpContent = contents.First()
            Dim uploadedFileMediaType As String = httpContent.Headers.ContentType.MediaType

            Dim portalId = 0
            Dim userId = 0

            For i = 0 To contents.Count - 1

                Select Case contents(i).Headers.ContentDisposition.Name.Replace("""", String.Empty)
                    Case "PortalId"
                        portalId = contents(i).ReadAsStringAsync().Result
                    Case "UserId"
                        userId = contents(i).ReadAsStringAsync().Result
                    Case Else

                End Select

            Next

            Dim objUserInfo = Users.UserController.GetUserById(portalId, userId)
            Dim userPath = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(objUserInfo)

            Utilities.CreateDir(Utilities.GetPortalSettings(portalId), userPath.FolderPath)

            Dim _file As Stream = httpContent.ReadAsStreamAsync().Result

            Dim guid_1 = Guid.NewGuid().ToString()

            Dim _fileName = String.Format("{0}.{1}", guid_1, imageTypeExtensionMap(uploadedFileMediaType))
            Dim _filePath = Path.Combine(userPath.PhysicalPath, _fileName)

            If _file.CanRead Then
                Using _fileStream As New FileStream(_filePath, FileMode.Create)
                    _file.CopyTo(_fileStream)
                End Using

                DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(portalId, userPath.FolderPath)

                Dim fileManager = DotNetNuke.Services.FileSystem.FileManager.Instance()
                Dim folderManager = DotNetNuke.Services.FileSystem.FolderManager.Instance()
                Dim folder = folderManager.GetFolder(portalId, userPath.FolderPath)

                Dim objFileInfo = fileManager.GetFile(folder, _fileName)

                ImageResizer.ImageBuilder.Current.Build(objFileInfo.PhysicalPath, objFileInfo.PhysicalPath, New ImageResizer.ResizeSettings("maxwidth=120&maxheight=120&crop=auto"))

                ''The resizing settings can specify any of 30 commands.. See http://imageresizing.net for details.
                ''Destination paths can have variables like <guid> and <ext>
                'Dim j = New ImageResizer.ImageJob
                'j.CreateParentDirectory = True
                'j.Source = _fileStream
                'j.Dest = String.Format("~/{0}/{1}<filename>.<ext>", portalCtrl.GetPortal(portalId).HomeDirectory, userPath.FolderPath)

                'Dim img = Drawing.Image.FromStream(_file)

                'j.Settings = New ImageResizer.ResizeSettings("width=120&height=120&crop=auto")

                '_fileStream.Seek(0, SeekOrigin.Begin)

                'j.Build()

                objUserInfo.Profile.SetProfileProperty("Photo", CStr(objFileInfo.FileId))
                Entities.Profile.ProfileController.UpdateUserProfile(objUserInfo)

                Return request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .fileName = objFileInfo.FileName, .filePath = objFileInfo.RelativePath})
            End If

            Return request.CreateResponse(HttpStatusCode.NotAcceptable, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes user photo
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPut> _
    Function removeUserPhoto(dto As userPhoto) As HttpResponseMessage
        Try
            Dim portalCtrl = New Portals.PortalController()
            Dim _userInfo = Users.UserController.GetUserById(dto.PortalId, dto.UserId)
            Dim destinationPath = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(_userInfo)
            Dim objFolderInfo = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetFolder(dto.PortalId, destinationPath.FolderPath)
            Dim objFileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(objFolderInfo, dto.FileName)

            DotNetNuke.Services.FileSystem.FileManager.Instance().DeleteFile(objFileInfo)

            _userInfo.Profile.SetProfileProperty("Photo", "-1")
            Entities.Profile.ProfileController.UpdateUserProfile(_userInfo)

            DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(dto.PortalId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Get person inndustries
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getPersonIndustries(personId As Integer) As HttpResponseMessage
        Try
            Dim personIndustriesCtrl As New PersonIndustryRepository

            Dim personIndustriesData = personIndustriesCtrl.getPersonIndustries(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, personIndustriesData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets person addresses
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getPersonAddresses(personId As Integer) As HttpResponseMessage
        Try
            Dim personAddressesCtrl As New PersonAddressesRepository

            Dim personAddressesData = personAddressesCtrl.getPersonAddresses(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, personAddressesData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client address
    ''' </summary>
    ''' <param name="personAddressId">Person Address ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getPersonAddress(personAddressId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim personAddressCtrl As New PersonAddressesRepository

            Dim personAddressData = personAddressCtrl.getPersonAddress(personAddressId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, personAddressData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates client address
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPost> _
    Function updatePersonAddress(dto As Models.PersonAddress) As HttpResponseMessage
        Try
            Dim personAddress As New Models.PersonAddress
            Dim clientAddressCtrl As New PersonAddressesRepository

            If dto.PersonAddressId > 0 Then
                personAddress = clientAddressCtrl.getPersonAddress(dto.PersonAddressId, dto.PersonId)
            End If

            personAddress.PersonId = dto.PersonId
            personAddress.AddressName = dto.AddressName
            personAddress.Street = dto.Street
            personAddress.Unit = dto.Unit
            personAddress.Complement = dto.Complement
            personAddress.District = dto.District
            personAddress.City = dto.City
            personAddress.Region = dto.Region
            personAddress.PostalCode = dto.PostalCode
            personAddress.Telephone = dto.Telephone
            personAddress.Cell = dto.Cell
            personAddress.Fax = dto.Fax
            personAddress.Country = dto.Country
            personAddress.ViewOrder = dto.ViewOrder
            personAddress.CreatedByUser = dto.CreatedByUser
            personAddress.CreatedOnDate = dto.CreatedOnDate
            personAddress.ModifiedByUser = dto.CreatedByUser
            personAddress.ModifiedOnDate = dto.CreatedOnDate

            If dto.PersonAddressId > 0 Then
                clientAddressCtrl.updatePersonAddress(personAddress)
            Else
                clientAddressCtrl.addPersonAddress(personAddress)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .cAId = personAddress.PersonAddressId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Disabled a person address
    ''' </summary>
    ''' <param name="dto">Person Address Model</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPut> _
    Function deletePersonAddress(dto As Models.PersonAddress) As HttpResponseMessage
        Try
            Dim personAddress As Models.PersonAddress
            Dim personAddressCtrl As New PersonAddressesRepository

            personAddress = personAddressCtrl.getPersonAddress(dto.PersonAddressId, dto.PersonId)
            personAddress.IsDeleted = True

            personAddressCtrl.updatePersonAddress(personAddress)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Restores a person address
    ''' </summary>
    ''' <param name="dto">Person Address Model</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPut> _
    Function restorePersonAddress(dto As Models.PersonAddress) As HttpResponseMessage
        Try
            Dim personAddress As Models.PersonAddress
            Dim personAddressCtrl As New PersonAddressesRepository

            personAddress = personAddressCtrl.getPersonAddress(dto.PersonAddressId, dto.PersonId)
            personAddress.IsDeleted = False

            personAddressCtrl.updatePersonAddress(personAddress)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes a person address
    ''' </summary>
    ''' <param name="personAddressId">Person Address ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpDelete> _
    Function removePersonAddress(personAddressId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientAddressCtrl As New PersonAddressesRepository

            clientAddressCtrl.removePersonAddress(personAddressId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client contacts by client id
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getPersonContacts(personId As Integer) As HttpResponseMessage
        Try
            Dim personContactsCtrl As New PersonContactsRepository

            Dim personContactsData = personContactsCtrl.getPersonContacts(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, personContactsData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client contact
    ''' </summary>
    ''' <param name="personContactId">Person Contact ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getPersonContact(personContactId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientContactCtrl As New PersonContactsRepository

            Dim clientContactData = clientContactCtrl.getPersonContact(personContactId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientContactData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates client contact
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPost> _
    Function updatePersonContact(dto As Models.PersonContact) As HttpResponseMessage
        Try
            Dim clientContact As New Models.PersonContact
            Dim clientContactCtrl As New PersonContactsRepository

            If dto.PersonContactId > 0 Then
                clientContact = clientContactCtrl.getPersonContact(dto.PersonContactId, dto.PersonId)
            End If

            clientContact.PersonId = dto.PersonId
            clientContact.ContactName = dto.ContactName
            clientContact.DateBirth = dto.DateBirth
            clientContact.Dept = dto.Dept
            clientContact.ContactEmail1 = dto.ContactEmail1
            clientContact.ContactEmail2 = dto.ContactEmail2
            clientContact.ContactPhone1 = dto.ContactPhone1
            clientContact.ContactPhone2 = dto.ContactPhone2
            clientContact.PhoneExt1 = dto.PhoneExt1
            clientContact.PhoneExt2 = dto.PhoneExt2
            clientContact.Comments = dto.Comments
            clientContact.PersonAddressId = dto.PersonAddressId
            clientContact.CreatedByUser = dto.CreatedByUser
            clientContact.CreatedOnDate = dto.CreatedOnDate
            clientContact.ModifiedByUser = dto.CreatedByUser
            clientContact.ModifiedOnDate = dto.CreatedOnDate

            If dto.PersonContactId > 0 Then
                clientContactCtrl.updatePersonContact(clientContact)
            Else
                clientContactCtrl.addPersonContact(clientContact)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .cCId = clientContact.PersonContactId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes a client contact
    ''' </summary>
    ''' <param name="personContactId">Person Contact ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpDelete> _
    Function removePersonContact(personContactId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientContactCtrl As New PersonContactsRepository

            clientContactCtrl.removePersonContact(personContactId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client docs by client id
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getPersonDocs(personId As Integer) As HttpResponseMessage
        Try
            Dim clientDocsCtrl As New PersonDocsRepository

            Dim clientDocsData = clientDocsCtrl.getPersonDocs(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientDocsData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client doc
    ''' </summary>
    ''' <param name="cDId">Person Doc ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getPersonDoc(cDId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientDocCtrl As New PersonDocsRepository

            Dim clientDocData = clientDocCtrl.getPersonDoc(cDId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientDocData)
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
            Dim httpContent As HttpContent = contents.First()
            Dim uploadedFileMediaType As String = httpContent.Headers.ContentType.MediaType

            'Dim portalId = contents(1).ReadAsStringAsync().Result
            'Dim folderPath = contents(2).ReadAsStringAsync().Result

            Dim personDoc As New Models.PersonDoc()
            Dim personDocCtrl As New PersonDocsRepository()

            For i = 0 To contents.Count - 1

                Select Case contents(i).Headers.ContentDisposition.Name.Replace("""", String.Empty)
                    Case "PortalId"
                        personDoc.PortalId = contents(i).ReadAsStringAsync().Result
                    Case "PersonId"
                        personDoc.PersonId = contents(i).ReadAsStringAsync().Result
                    Case "DocName"
                        personDoc.DocName = contents(i).ReadAsStringAsync().Result
                    Case "DocDesc"
                        personDoc.DocDesc = contents(i).ReadAsStringAsync().Result
                    Case "DocUrl"
                        personDoc.DocUrl = contents(i).ReadAsStringAsync().Result
                    Case "FolderPath"
                        personDoc.FolderPath = contents(i).ReadAsStringAsync().Result
                    Case "CreatedByUser"
                        personDoc.CreatedByUser = contents(i).ReadAsStringAsync().Result
                    Case "CreatedOnDate"
                        personDoc.CreatedOnDate = contents(i).ReadAsStringAsync().Result
                    Case Else

                End Select

            Next

            Dim root = String.Format("{0}{1}", portalCtrl.GetPortal(personDoc.PortalId).HomeDirectoryMapPath, personDoc.FolderPath)

            Utilities.CreateDir(Utilities.GetPortalSettings(personDoc.PortalId), personDoc.FolderPath)

            Dim _file As Stream = httpContent.ReadAsStreamAsync().Result

            Dim guid_1 = Guid.NewGuid().ToString()

            Dim _fileName = String.Format("{0}.{1}", guid_1, mediaTypeExtensionMap(uploadedFileMediaType))
            Dim _filePath = Path.Combine(root, _fileName)

            If _file.CanRead Then
                Using _fileStream As New FileStream(_filePath, FileMode.Create)
                    _file.CopyTo(_fileStream)
                End Using
            End If

            DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(personDoc.PortalId)

            Dim objFolderInfo = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetFolder(personDoc.PortalId, personDoc.FolderPath)
            Dim objFileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(objFolderInfo, _fileName)

            personDoc.FileId = objFileInfo.FileId

            personDocCtrl.addPersonDoc(personDoc)

            Return request.CreateResponse(HttpStatusCode.Created, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message}))
        End Try
    End Function

    ''' <summary>
    ''' Removes a person Doc
    ''' </summary>
    ''' <param name="personDocId">Person Doc ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpDelete> _
    Function removePersonDoc(personDocId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientDocCtrl As New PersonDocsRepository

            ' Todo: add function to remove user's file
            clientDocCtrl.removePersonDoc(personDocId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates client finance
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPut> _
    Function updateClientFinance(dto As Models.Person) As HttpResponseMessage
        Try
            Dim culture = New CultureInfo("pt-BR")
            Dim numInfo = culture.NumberFormat

            Dim client As New Models.Person
            Dim clientCtrl As New PeopleRepository

            client = clientCtrl.getPerson(dto.PersonId, dto.PortalId, Null.NullInteger)

            client.PersonAddressId = dto.PersonAddressId
            client.MonthlyIncome = dto.MonthlyIncome
            client.ModifiedByUser = dto.ModifiedByUser
            client.ModifiedOnDate = dto.ModifiedOnDate

            clientCtrl.updatePerson(client)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client income sources
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getClientIncomeSources(personId As Integer) As HttpResponseMessage
        Try
            Dim clientIncomeSourcesCtrl As New ClientIncomeSourcesRepository

            Dim clientIncomeSources = clientIncomeSourcesCtrl.getClientIncomeSources(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientIncomeSources)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client income source
    ''' </summary>
    ''' <param name="clientIncomeSourceId">Person Income Source ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getClientIncomeSource(clientIncomeSourceId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientIncomeSourceCtrl As New ClientIncomeSourcesRepository

            Dim clientIncomeSource = clientIncomeSourceCtrl.getClientIncomeSource(clientIncomeSourceId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientIncomeSource)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates client income source
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPost> _
    Function updateClientIncomeSource(dto As Models.ClientIncomeSource) As HttpResponseMessage
        Try
            Dim culture = New CultureInfo("pt-BR")
            Dim numInfo = culture.NumberFormat

            Dim clientIncomeSource As New Models.ClientIncomeSource
            Dim clientIncomeSourceCtrl As New ClientIncomeSourcesRepository

            If dto.ClientIncomeSourceId > 0 Then
                clientIncomeSource = clientIncomeSourceCtrl.getClientIncomeSource(dto.ClientIncomeSourceId, dto.PersonId)
            End If

            clientIncomeSource.PersonId = dto.PersonId
            clientIncomeSource.ISAddress = dto.ISAddress
            clientIncomeSource.ISAddressUnit = dto.ISAddressUnit
            clientIncomeSource.ISCity = dto.ISCity
            clientIncomeSource.ISComplement = dto.ISComplement
            clientIncomeSource.ISCT = dto.ISCT
            clientIncomeSource.ISDistrict = dto.ISDistrict
            clientIncomeSource.ISEIN = dto.ISEIN
            clientIncomeSource.ISFax = dto.ISFax
            clientIncomeSource.ISIncome = dto.ISIncome
            clientIncomeSource.ISName = dto.ISName
            clientIncomeSource.ISPhone = dto.ISPhone
            clientIncomeSource.ISPostalCode = dto.ISPostalCode
            clientIncomeSource.ISProof = dto.ISProof
            clientIncomeSource.ISRegion = dto.ISRegion
            clientIncomeSource.ISST = dto.ISST

            If dto.ClientIncomeSourceId > 0 Then
                clientIncomeSourceCtrl.updateClientIncomeSource(clientIncomeSource)
            Else
                clientIncomeSourceCtrl.addClientIncomeSource(clientIncomeSource)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", clientIncomeSource.ClientIncomeSourceId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes a client income source
    ''' </summary>
    ''' <param name="clientIncomeSourceId">Person Income Source ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpDelete> _
    Function removeClientIncomeSource(clientIncomeSourceId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientIncomeCourceCtrl As New ClientIncomeSourcesRepository

            clientIncomeCourceCtrl.removeClientIncomeSource(clientIncomeSourceId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client personal references
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getClientPersonalRefs(personId As Integer) As HttpResponseMessage
        Try
            Dim clientPersonalRefsCtrl As New ClientPersonalRefsRepository

            Dim clientPersonalRefs = clientPersonalRefsCtrl.getClientPersonalRefs(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientPersonalRefs)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client personal reference
    ''' </summary>
    ''' <param name="clientPersonalRefId">Person Personal Ref ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getClientPersonalRef(clientPersonalRefId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientPersonalRefCtrl As New ClientPersonalRefsRepository

            Dim clientPersonalRef = clientPersonalRefCtrl.getClientPersonalRef(clientPersonalRefId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientPersonalRef)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates client personal reference
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPost> _
    Function updateClientPersonalRef(dto As Models.ClientPersonalRef) As HttpResponseMessage
        Try
            Dim clientPersonalRef As New Models.ClientPersonalRef
            Dim clientPersonalRefCtrl As New ClientPersonalRefsRepository

            If dto.ClientPersonalRefId > 0 Then
                clientPersonalRef = clientPersonalRefCtrl.getClientPersonalRef(dto.ClientPersonalRefId, dto.PersonId)
            End If

            clientPersonalRef.PersonId = dto.PersonId
            clientPersonalRef.PREmail = dto.PREmail
            clientPersonalRef.PRName = dto.PRName
            clientPersonalRef.PRPhone = dto.PRPhone

            If dto.ClientPersonalRefId > 0 Then
                clientPersonalRefCtrl.updateClientPersonalRef(clientPersonalRef)
            Else
                clientPersonalRefCtrl.addClientPersonalRef(clientPersonalRef)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes a client personal reference
    ''' </summary>
    ''' <param name="clientPersonalRefId">Person Personal Reference Source ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpDelete> _
    Function removeClientPersonalRef(clientPersonalRefId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientPersonalRefCtrl As New ClientPersonalRefsRepository

            clientPersonalRefCtrl.removeClientPersonalRef(clientPersonalRefId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client bank references
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getClientBankRefs(personId As Integer) As HttpResponseMessage
        Try
            Dim clientBankRefsCtrl As New ClientBankRefsRepository

            Dim clientBankRefs = clientBankRefsCtrl.getClientBankRefs(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientBankRefs)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client bank reference
    ''' </summary>
    ''' <param name="clientBankRefId">Person Bank Ref ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getClientBankRef(clientBankRefId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientBankRefCtrl As New ClientBankRefsRepository

            Dim clientBankRef = clientBankRefCtrl.getClientBankRef(clientBankRefId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientBankRef)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates client bank reference
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPost> _
    Function updateClientBankRef(dto As Models.ClientBankRef) As HttpResponseMessage
        Try
            Dim culture = New CultureInfo("pt-BR")
            Dim numInfo = culture.NumberFormat

            Dim clientBankRef As New Models.ClientBankRef
            Dim clientBankRefCtrl As New ClientBankRefsRepository

            If dto.ClientBankRefId > 0 Then
                clientBankRef = clientBankRefCtrl.getClientBankRef(dto.ClientBankRefId, dto.PersonId)
            End If

            clientBankRef.PersonId = dto.PersonId
            clientBankRef.BankRef = dto.BankRef
            clientBankRef.BankRefAccount = dto.BankRefAccount
            clientBankRef.BankRefAccountType = dto.BankRefAccountType
            clientBankRef.BankRefAgency = dto.BankRefAgency
            clientBankRef.BankRefClientSince = dto.BankRefClientSince
            clientBankRef.BankRefContact = dto.BankRefContact
            clientBankRef.BankRefCredit = dto.BankRefCredit
            clientBankRef.BankRefPhone = dto.BankRefPhone

            If dto.ClientBankRefId > 0 Then
                clientBankRefCtrl.updateClientBankRef(clientBankRef)
            Else
                clientBankRefCtrl.addClientBankRef(clientBankRef)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes a client bank reference
    ''' </summary>
    ''' <param name="clientBankReferenceId">Person Bank Reference Source ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpDelete> _
    Function removeClientBankReference(clientBankReferenceId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientBankRefCtrl As New ClientBankRefsRepository

            clientBankRefCtrl.removeClientBankRef(clientBankReferenceId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client commerce references
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getClientCommRefs(personId As Integer) As HttpResponseMessage
        Try
            Dim clientCommRefsCtrl As New ClientCommRefsRepository

            Dim clientCommRefs = clientCommRefsCtrl.getClientCommRefs(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientCommRefs)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client commerce reference
    ''' </summary>
    ''' <param name="clientCommRefId">Person Commerce Ref ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getClientCommRef(clientCommRefId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientCommRefCtrl As New ClientCommRefsRepository

            Dim clientCommRef = clientCommRefCtrl.getClientCommRef(clientCommRefId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientCommRef)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates client commerce reference
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPost> _
    Function updateClientCommRef(dto As Models.ClientCommRef) As HttpResponseMessage
        Try
            Dim culture = New CultureInfo("pt-BR")
            Dim numInfo = culture.NumberFormat

            Dim clientCommRef As New Models.ClientCommRef
            Dim clientCommRefCtrl As New ClientCommRefsRepository

            If dto.ClientCommRefId > 0 Then
                clientCommRef = clientCommRefCtrl.getClientCommRef(dto.ClientCommRefId, dto.PersonId)
            End If

            clientCommRef.PersonId = dto.PersonId
            clientCommRef.CommRefBusiness = dto.CommRefBusiness
            clientCommRef.CommRefContact = dto.CommRefContact
            clientCommRef.CommRefPhone = dto.CommRefPhone
            clientCommRef.CommRefLastActivity = dto.CommRefLastActivity
            clientCommRef.CommRefCredit = dto.CommRefCredit

            If dto.ClientCommRefId > 0 Then
                clientCommRefCtrl.updateClientCommRef(clientCommRef)
            Else
                clientCommRefCtrl.addClientCommRef(clientCommRef)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes a client commerce reference
    ''' </summary>
    ''' <param name="clientCommReferenceId">Person Commerce Reference Source ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpDelete> _
    Function removeClientCommRef(clientCommReferenceId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientCommRefCtrl As New ClientCommRefsRepository

            clientCommRefCtrl.removeClientCommRef(clientCommReferenceId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client partners
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getClientPartners(personId As Integer) As HttpResponseMessage
        Try
            Dim clientPartnersCtrl As New ClientPartnersRepository

            Dim clientPartners = clientPartnersCtrl.getClientPartners(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientPartners)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client partner
    ''' </summary>
    ''' <param name="clientPartnerId">Person Partner ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getClientPartner(clientPartnerId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientPartnerCtrl As New ClientPartnersRepository

            Dim clientPartner = clientPartnerCtrl.getClientPartner(clientPartnerId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientPartner)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates client partner
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPost> _
    Function updateClientPartner(dto As Models.ClientPartner) As HttpResponseMessage
        Try
            Dim clientPartner As New Models.ClientPartner
            Dim clientPartnerCtrl As New ClientPartnersRepository

            If dto.ClientPartnerId > 0 Then
                clientPartner = clientPartnerCtrl.getClientPartner(dto.ClientPartnerId, dto.PersonId)
            End If

            clientPartner.PersonId = dto.PersonId
            clientPartner.PartnerName = dto.PartnerName
            clientPartner.PartnerCPF = dto.PartnerCPF
            clientPartner.PartnerIdentity = dto.PartnerIdentity
            clientPartner.PartnerPhone = dto.PartnerPhone
            clientPartner.PartnerCell = dto.PartnerCell
            clientPartner.PartnerEmail = dto.PartnerEmail
            clientPartner.PartnerQuota = dto.PartnerQuota
            clientPartner.PartnerPostalCode = dto.PartnerPostalCode
            clientPartner.PartnerAddress = dto.PartnerAddress
            clientPartner.PartnerAddressUnit = dto.PartnerAddressUnit
            clientPartner.PartnerComplement = dto.PartnerComplement
            clientPartner.PartnerDistrict = dto.PartnerDistrict
            clientPartner.PartnerRegion = dto.PartnerRegion
            clientPartner.PartnerCity = dto.PartnerCity
            clientPartner.PartnerCountry = dto.PartnerCountry

            If dto.ClientPartnerId > 0 Then
                clientPartnerCtrl.updateClientPartner(clientPartner)
            Else
                clientPartnerCtrl.addClientPartner(clientPartner)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes a client partner
    ''' </summary>
    ''' <param name="clientPartnerId">Person Partner ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpDelete> _
    Function removeClientPartner(clientPartnerId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientPartnerCtrl As New ClientPartnersRepository

            clientPartnerCtrl.removeClientPartner(clientPartnerId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client partner bank references
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getClientPartnerBankRefs(personId As Integer) As HttpResponseMessage
        Try
            Dim clientPartnerBankRefsCtrl As New ClientPartnersBankRefsRepository

            Dim clientPartnerBankRefs = clientPartnerBankRefsCtrl.getClientPartnerBankRefs(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientPartnerBankRefs)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client partner bank reference
    ''' </summary>
    ''' <param name="clientPartnerBankReferenceId">Person Partner Bank Ref ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getClientPartnerBankRef(clientPartnerBankReferenceId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientPartnerBankRefCtrl As New ClientPartnersBankRefsRepository

            Dim clientPartnerBankRef = clientPartnerBankRefCtrl.getClientPartnerBankRef(clientPartnerBankReferenceId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientPartnerBankRef)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates client partner bank reference
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPost> _
    Function updateClientPartnerBankRef(dto As Models.ClientPartnerBankRef) As HttpResponseMessage
        Try
            Dim culture = New CultureInfo("pt-BR")
            Dim numInfo = culture.NumberFormat

            Dim clientPartnerBankRef As New Models.ClientPartnerBankRef
            Dim clientPartnerBankRefCtrl As New ClientPartnersBankRefsRepository

            If dto.ClientPartnerBankRefId > 0 Then
                clientPartnerBankRef = clientPartnerBankRefCtrl.getClientPartnerBankRef(dto.ClientPartnerBankRefId, dto.PersonId)
            End If

            clientPartnerBankRef.PersonId = dto.PersonId
            clientPartnerBankRef.PartnerName = dto.PartnerName
            clientPartnerBankRef.BankRef = dto.BankRef
            clientPartnerBankRef.BankRefAgency = dto.BankRefAgency
            clientPartnerBankRef.BankRefAccount = dto.BankRefAccount
            clientPartnerBankRef.BankRefClientSince = dto.BankRefClientSince
            clientPartnerBankRef.BankRefContact = dto.BankRefContact
            clientPartnerBankRef.BankRefPhone = dto.BankRefPhone
            clientPartnerBankRef.BankRefAccountType = dto.BankRefAccountType
            clientPartnerBankRef.BankRefCredit = dto.BankRefCredit

            If dto.ClientPartnerBankRefId > 0 Then
                clientPartnerBankRefCtrl.updateClientPartnerBankRef(clientPartnerBankRef)
            Else
                clientPartnerBankRefCtrl.addClientPartnerBankRef(clientPartnerBankRef)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes a client's partner bank reference
    ''' </summary>
    ''' <param name="clientPartnerBankRefId">Person Partner Bank Reference Source ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpDelete> _
    Function removeClientPartnerBankReference(clientPartnerBankRefId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientPartnerBankRefCtrl As New ClientPartnersBankRefsRepository

            clientPartnerBankRefCtrl.removeClientPartnerBankRef(clientPartnerBankRefId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates person login and or email
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPut> _
    Function updatePersonUserLogin(dto As Models.Person) As HttpResponseMessage
        Try

            If Not Services.ValidateUser(dto.Email.ToLower()) > 0 Then

                Dim person As New Models.Person
                Dim personCtrl As New PeopleRepository

                person = personCtrl.getPerson(dto.PersonId, dto.PortalId, Null.NullInteger)

                person.Email = dto.Email
                person.ModifiedByUser = dto.ModifiedByUser
                person.ModifiedOnDate = dto.ModifiedOnDate

                personCtrl.updatePerson(person)

                Users.UserController.ChangeUsername(dto.UserId, dto.Email)

                Dim oUser = Users.UserController.GetUserById(dto.PortalId, dto.UserId)
                oUser.Email = dto.Email
                Users.UserController.UpdateUser(dto.PortalId, oUser)

                DotNetNuke.Common.Utilities.DataCache.ClearUserCache(dto.PortalId, dto.Email)

                Dim recipientList As New List(Of Users.UserInfo)
                Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(dto.PortalId)
                Dim portalCtrl = New Portals.PortalController()
                Dim portal = portalCtrl.GetPortal(dto.PortalId)
                Dim _portalEmail = Portals.PortalController.GetPortalSetting("RIW_StoreEmail", dto.PortalId, portal.Email)

                Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = _portalEmail, .DisplayName = portal.PortalName}

                Dim _clientUserInfo = Users.UserController.GetUserById(dto.PortalId, dto.UserId)
                recipientList.Add(oUser)

                Dim mailMessage As New Net.Mail.MailMessage
                Dim distList As New PostOffice

                mailMessage.From = New Net.Mail.MailAddress(storeUser.Email, storeUser.DisplayName)
                mailMessage.ReplyToList.Add(New Net.Mail.MailAddress(oUser.Email, oUser.DisplayName))
                mailMessage.Subject = dto.Subject
                mailMessage.Body = dto.MessageBody.Replace("[login]", dto.Email).Replace("[senha]", Users.UserController.GetPassword(_clientUserInfo, ""))
                'mailMessage.Attachments.Add(New Net.Mail.Attachment(Str, String.Format("Orcamento_{0}_{1}.pdf", CStr(dto.EstimateId), Replace(DateTime.Now.ToString(), ":", "-").Replace("/", "-"))))
                mailMessage.IsBodyHtml = True

                Dim sentMails = distList.SendMail(mailMessage, recipientList, myPortalSettings("RIW_SMTPServer"), CInt(myPortalSettings("RIW_SMTPPort")), CBool(myPortalSettings("RIW_SMTPConnection")), myPortalSettings("RIW_SMTPLogin"), myPortalSettings("RIW_SMTPPassword"))

                'Notifications.SendStoreEmail(storeUser, _clientUserInfo, Nothing, Nothing, dto.Subject, dto.MessageBody.Replace("[login]", dto.Email).Replace("[senha]", Users.UserController.GetPassword(_clientUserInfo, "")), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, dto.PortalId, True)

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
    ''' Gets client history by client id
    ''' </summary>
    ''' <param name="personId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpGet> _
    Function getHistory(personId As Integer) As HttpResponseMessage
        Try
            Dim historyCtrl As New PersonHistoriesRepository

            Dim histories As New List(Of Models.PersonHistory)

            Dim personHistories = historyCtrl.getPersonHistories(personId)

            For Each history In personHistories
                Dim personHistoryComments = historyCtrl.getPersonHistoryComments(history.PersonHistoryId)
                history.HistoryComments = personHistoryComments
                histories.Add(history)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, histories)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates estimate message
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPost> _
    Function updateHistory(dto As Models.PersonHistory) As HttpResponseMessage
        Try
            Dim personHistory As New Models.PersonHistory
            Dim personHistoryCtrl As New PersonHistoriesRepository

            If dto.PersonHistoryId > 0 Then
                personHistory = personHistoryCtrl.getPersonHistory(dto.PersonHistoryId, dto.PersonId)
            End If

            personHistory.PersonId = dto.PersonId
            personHistory.HistoryText = dto.HistoryText
            personHistory.Locked = dto.Locked
            personHistory.CreatedByUser = dto.CreatedByUser
            personHistory.CreatedOnDate = dto.CreatedOnDate

            If dto.PersonHistoryId > 0 Then
                personHistoryCtrl.updatePersonHistory(personHistory)
            Else
                personHistoryCtrl.addPersonHistory(personHistory)
            End If

            Dim _userInfo = Users.UserController.GetUserById(Portals.PortalController.GetCurrentPortalSettings().PortalId, personHistory.CreatedByUser)
            personHistory.DisplayName = _userInfo.DisplayName
            If _userInfo.Profile.Photo <> "" Then
                personHistory.Avatar = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(_userInfo).FolderPath
                personHistory.Avatar = personHistory.Avatar & DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(_userInfo.Profile.Photo).FileName
            End If

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                peopleHub.Value.Clients.AllExcept(dto.ConnId).pushMessage(personHistory)
                'Hub.Clients.AllExcept(MessageComment.connId).pushComment(personHistoryComment, MessageComment.messageIndex)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .personHistory = personHistory})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates person history comment
    ''' </summary>
    ''' <param name="historyComment"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function updateHistoryComment(historyComment As HistoryComment) As HttpResponseMessage
        Try
            Dim personHistoryComment As New Models.PersonHistoryComment
            Dim personHistoryCtrl As New PersonHistoriesRepository

            If historyComment.dto.CommentId > 0 Then
                personHistoryComment = personHistoryCtrl.getPersonHistoryComment(historyComment.dto.CommentId, historyComment.dto.PersonHistoryId)
            End If

            personHistoryComment.PersonHistoryId = historyComment.dto.PersonHistoryId
            personHistoryComment.CommentText = historyComment.dto.CommentText
            personHistoryComment.CreatedByUser = historyComment.dto.CreatedByUser
            personHistoryComment.CreatedOnDate = historyComment.dto.CreatedOnDate

            Dim _userInfo = Users.UserController.GetUserById(Portals.PortalController.GetCurrentPortalSettings().PortalId, personHistoryComment.CreatedByUser)
            personHistoryComment.DisplayName = _userInfo.DisplayName
            If _userInfo.Profile.Photo <> "" Then
                personHistoryComment.Avatar = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(_userInfo).FolderPath
                personHistoryComment.Avatar = personHistoryComment.Avatar & DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(_userInfo.Profile.Photo).FileName
            End If

            If historyComment.dto.CommentId > 0 Then
                personHistoryCtrl.updatePersonHistoryComment(personHistoryComment)
            Else
                personHistoryCtrl.addPersonHistoryComment(personHistoryComment)
            End If

            If historyComment.connId IsNot Nothing Then
                '' SignalR
                peopleHub.Value.Clients.AllExcept(historyComment.connId).pushComment(personHistoryComment, historyComment.messageIndex)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .PersonHistoryComment = personHistoryComment})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes person history comment
    ''' </summary>
    ''' <param name="commentId"></param>
    ''' <param name="personHistoryId"></param>
    ''' <param name="connId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpDelete> _
    Function removeHistoryComment(commentId As Integer, personHistoryId As Integer, connId As String) As HttpResponseMessage
        Try
            Dim personHistoryCtrl As New PersonHistoriesRepository

            personHistoryCtrl.removePersonHistoryComment(commentId, personHistoryId)

            If connId IsNot Nothing Then
                '' SignalR
                peopleHub.Value.Clients.AllExcept(connId).removeComment(commentId, personHistoryId)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates person status  and adds person history
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPut> _
    Function updatePersonStatus(dto As Models.Person) As HttpResponseMessage
        Try
            Dim person As New Models.Person
            Dim personCtrl As New PeopleRepository

            Dim personHistory As New Models.PersonHistory
            Dim personHistoryCtrl As New PersonHistoriesRepository

            person = personCtrl.getPerson(dto.PersonId, PortalController.GetCurrentPortalSettings().PortalId, Null.NullInteger)
            person.StatusId = dto.StatusId
            person.ModifiedByUser = dto.ModifiedByUser
            person.ModifiedOnDate = dto.ModifiedOnDate
            personCtrl.updatePerson(person)

            If dto.HistoryText IsNot Nothing Then

                personHistory.PersonId = dto.PersonId
                personHistory.HistoryText = dto.HistoryText
                personHistory.CreatedByUser = dto.ModifiedByUser
                personHistory.CreatedOnDate = dto.ModifiedOnDate

                personHistoryCtrl.addPersonHistory(personHistory)
            End If

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                estimatesHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(personHistory, person.StatusId)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates person sent and adds person history
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")> _
    <HttpPut> _
    Function UpdateClientSent(dto As Models.Person) As HttpResponseMessage
        Try
            Dim person As New Models.Person
            Dim personCtrl As New PeopleRepository

            person = personCtrl.getPerson(dto.PersonId, dto.PortalId, Null.NullInteger)
            person.Sent = dto.Sent
            person.ModifiedByUser = dto.ModifiedByUser
            person.ModifiedOnDate = dto.ModifiedOnDate

            personCtrl.updatePerson(person)

            If dto.HistoryText IsNot Nothing Then
                Dim personHistory As New Models.PersonHistory
                Dim personHistoryCtrl As New PersonHistoriesRepository

                personHistory.PersonId = dto.PersonId
                personHistory.HistoryText = dto.HistoryText
                personHistory.CreatedByUser = dto.ModifiedByUser
                personHistory.CreatedOnDate = dto.ModifiedOnDate

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
    ''' Removes user photo
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="uId">User ID</param>
    ''' <param name="fileName">File Name</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores,Vendedores")> _
    <HttpPost> _
    Function removeUserPhoto(ByVal portalId As Integer, ByVal uId As Integer, ByVal fileName As String) As HttpResponseMessage
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
    ''' Gets list of users by portal id
    ''' </summary>
    ''' <param name="users"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getUsers(users As IEnumerable(Of UserInfo)) As IList(Of Models.User)
        Try
            Return users.[Select](Function(user) New Models.User(user, PortalSettings)).ToList()
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Gets list of users by portal id and role group name
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="roleGroupName">Role Group Name</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function getUsersByRoleGroup(portalId As Integer, roleGroupName As String) As HttpResponseMessage
        Try

            Dim usersList = New List(Of PersonInfo)

            Dim objRoleCtrl As New RoleController()
            Dim objRoleGroupInfo = RoleController.GetRoleGroupByName(portalId, roleGroupName)

            Dim rolesList = objRoleCtrl.GetRolesByGroup(portalId, objRoleGroupInfo.RoleGroupID)

            For Each _role As RoleInfo In rolesList

                Dim usersInRole = objRoleCtrl.GetUsersByRoleName(portalId, _role.RoleName)

                For Each _user As Users.UserInfo In usersInRole

                    If Not usersList.Exists((Function(x As PersonInfo) If((String.Equals(x.UserId, _user.UserID)), True, False))) Then

                        usersList.Add(New PersonInfo() With {.UserId = _user.UserID, .DisplayName = _user.DisplayName})

                    End If
                Next
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, usersList)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets list of users by portal id and role name
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="roleName">Role Name</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores,Vendedores")> _
    <HttpGet> _
    Function getUsersByRoleName(portalId As Integer, roleName As String) As HttpResponseMessage
        Try
            Dim roleCtlr As New DotNetNuke.Security.Roles.RoleController
            Dim usersData = roleCtlr.GetUsersByRoleName(portalId, roleName)

            Dim _users As New List(Of UserInfo)
            For Each _user As UserInfo In usersData
                _users.Add(_user)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, getUsers(_users))
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets user info
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="userId">User ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores,Vendedores")> _
    <HttpGet> _
    Function getUser(portalId As Integer, userId As Integer) As HttpResponseMessage
        Try
            Dim users = New List(Of Users.UserInfo)
            Dim user = UserController.GetUserById(portalId, userId)
            users.Add(user)

            Return Request.CreateResponse(HttpStatusCode.OK, getUsers(users))
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    Public Class HistoryComment
        Public Property dto As Models.PersonHistoryComment
        Public Property connId As String
        Public Property messageIndex As Integer
    End Class

    Public Class userPassword
        Property PortalId As Integer
        Property UserId As Integer
        Property Subject As String
        Property MessageBody As String
        Property ModifiedByUser As Integer
        Property ModifiedOnDate As DateTime
    End Class

    Private ReadOnly imageTypeExtensionMap As New Dictionary(Of String, String)() From {
        {"image/jpeg", "jpg"},
        {"image/png", "png"}
    }

    Public Class userPhoto
        Property PortalId As Integer
        Property UserId As Integer
        Property FileName As String
    End Class

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

    Private Class PersonInfo
        Public Property UserId As Integer
        Public Property DisplayName As String
    End Class

End Class
