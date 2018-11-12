
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Security.Membership
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Web.Api
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports RIW.Modules.Common
Imports DotNetNuke.Services.Log.EventLog
Imports RIW.Modules.WebAPI.Components.Repositories
Imports System.Threading

Public Class PeopleController
    Inherits DnnApiControllerWithHub
    'Inherits DnnApiController

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(PeopleController))

    ''' <summary>
    ''' Checks for an existing user from the user's table
    ''' </summary>
    ''' <param name="vTerm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpGet> _
    Function ValidateUserName(vTerm As String) As HttpResponseMessage
        Try
            'Dim peopleDataCtrl As New PeopleRepository
            'Dim peopleData = peopleDataCtrl.GetPeople(PortalController.Instance.GetCurrentPortalSettings().PortalId, Null.NullInteger, Null.NullInteger, "", vTerm, Null.NullInteger, "", Null.NullDate, Null.NullDate, "2", 1, 1, "")

            Dim counter = 0
            counter += UserController.GetUsersByUserName(Null.NullInteger, vTerm.ToLower(), Null.NullInteger(), Null.NullInteger(), Null.NullInteger()).Count
            'counter += Users.UserController.GetUsersByEmail(Null.NullInteger, vTerm.ToLower(), Null.NullInteger(), Null.NullInteger(), Null.NullInteger()).Count
            'counter += peopleData.Count

            Return Request.CreateResponse(HttpStatusCode.OK, counter)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Checks for an existing user from the user's table
    ''' </summary>
    ''' <param name="vTerm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous>
    <HttpGet>
    Function ValidateUserEmail(vTerm As String) As HttpResponseMessage
        Try
            Dim peopleDataCtrl As New PeopleRepository
            Dim peopleData = peopleDataCtrl.GetPeople(PortalController.Instance.GetCurrentPortalSettings().PortalId, Null.NullString, Null.NullInteger, "", vTerm, Null.NullInteger, "", Null.NullDate, Null.NullDate, "2", 1, 1, "", "")

            Dim counter = 0
            'counter += Users.UserController.GetUsersByUserName(Null.NullInteger, vTerm.ToLower(), Null.NullInteger(), Null.NullInteger(), Null.NullInteger()).Count
            counter += UserController.GetUsersByEmail(Null.NullInteger, vTerm.ToLower(), Null.NullInteger(), Null.NullInteger(), Null.NullInteger()).Count
            counter += peopleData.Count

            Return Request.CreateResponse(HttpStatusCode.OK, counter)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Checks for an existing user from the user's table
    ''' </summary>
    ''' <param name="vTerm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous>
    <HttpGet>
    Function ValidateUser(vTerm As String) As HttpResponseMessage
        Try
            Dim peopleDataCtrl As New PeopleRepository
            Dim peopleData = peopleDataCtrl.GetPeople(PortalController.Instance.GetCurrentPortalSettings().PortalId, Null.NullString, Null.NullInteger, "", vTerm, Null.NullInteger, "", Null.NullDate, Null.NullDate, "2", 1, 1, "", "")

            Dim counter = 0
            counter += UserController.GetUsersByUserName(Null.NullInteger, vTerm.ToLower(), Null.NullInteger(), Null.NullInteger(), Null.NullInteger()).Count
            counter += UserController.GetUsersByEmail(Null.NullInteger, vTerm.ToLower(), Null.NullInteger(), Null.NullInteger(), Null.NullInteger()).Count
            counter += peopleData.Count

            Return Request.CreateResponse(HttpStatusCode.OK, counter)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Checks for an existing email from the client's table
    ''' </summary>
    ''' <param name="email"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous>
    <HttpGet>
    Function ValidatePerson(email As String) As HttpResponseMessage
        Dim clients As IEnumerable(Of Components.Models.Person)
        Using context As IDataContext = DataContext.Instance()
            Dim rep = context.GetRepository(Of Components.Models.Person)()
            clients = rep.Find("Where Email = @0", email)
        End Using
        Return Request.CreateResponse(HttpStatusCode.OK, clients.Count)
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
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")>
    <HttpGet>
    Function GetPeople(Optional searchField As String = "ALL",
                       Optional pageIndex As Integer = 1,
                       Optional pageSize As Integer = 10,
                       Optional orderBy As String = "",
                       Optional orderDesc As String = "",
                       Optional portalId As Integer = -1,
                       Optional salesRep As Integer = -1,
                       Optional isDeleted As String = "",
                       Optional sTerm As String = "",
                       Optional statusId As Integer = -1,
                       Optional registerType As String = "",
                       Optional sDate As String = Nothing,
                       Optional eDate As String = Nothing,
                       Optional filterDate As String = "ALL") As HttpResponseMessage
        Try
            Dim searchStr As String = HttpContext.Current.Request.Params("filter[filters][0][value]")
            If sTerm Is Nothing Then
                If searchStr Is Nothing Then
                    searchStr = ""
                End If
            Else
                If sTerm.Length > 0 Then
                    searchStr = sTerm
                End If
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
            Dim peopleData = peopleDataCtrl.GetPeople(portalId, searchField, salesRep, isDeleted, searchStr, statusId, registerType, CDate(sDate), CDate(eDate), filterDate, pageIndex, pageSize, orderBy, orderDesc)

            Dim total = Nothing
            For Each item In peopleData
                total = item.TotalRows
                Exit For
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.data = peopleData, .total = total})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpGet>
    Function GetPerson(Optional personId As Integer = -1, Optional portalId As Integer = 0, Optional userId As Integer = -1) As HttpResponseMessage
        Try
            Dim personCtrl As New PeopleRepository

            Dim person = personCtrl.GetPerson(personId, portalId, userId)

            Return Request.CreateResponse(HttpStatusCode.OK, person)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates a person account
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous>
    <HttpPost>
    Function UpdatePerson(dto As Components.Models.Person) As HttpResponseMessage
        Try
            Dim person As New Components.Models.Person
            Dim personHistory As New Components.Models.PersonHistory
            Dim personHistoryCtrl As New PersonHistoriesRepository
            Dim address As New Components.Models.PersonAddress
            Dim personCtrl As New PeopleRepository
            Dim addressCtrl As New PersonAddressesRepository

            'Dim defaultStatus As New Models.Status
            'Dim defaultStatusCtrl As New StatusesRepository

            If dto.PersonId > 0 Then
                person = personCtrl.GetPerson(dto.PersonId, PortalController.Instance.GetCurrentPortalSettings().PortalId, Null.NullInteger)
                address = addressCtrl.GetPersonAddresses(dto.PersonId)(0)
            End If

            person.DateFounded = dto.DateFounded
            person.DateRegistered = dto.DateRegistered
            person.PortalId = dto.PortalId
            person.PersonType = dto.PersonType
            person.EIN = dto.EIN
            person.CPF = dto.CPF
            person.SalesRep = dto.SalesRep
            person.Blocked = dto.Blocked
            person.ReasonBlocked = dto.ReasonBlocked
            person.CreatedByUser = dto.CreatedByUser
            person.CreatedOnDate = dto.CreatedOnDate
            person.ModifiedByUser = dto.ModifiedByUser
            person.ModifiedOnDate = dto.ModifiedOnDate
            person.CompanyName = ""
            person.FirstName = ""
            person.LastName = ""
            person.DisplayName = ""

            If dto.CompanyName IsNot Nothing Then
                person.CompanyName = dto.CompanyName
                If dto.DisplayName <> "" Then
                    person.DisplayName = dto.DisplayName
                Else
                    person.DisplayName = dto.CompanyName
                End If
                If dto.FirstName IsNot Nothing Then
                    person.FirstName = dto.FirstName
                    If dto.LastName IsNot Nothing Then
                        person.LastName = dto.LastName
                    End If
                End If
            Else
                person.FirstName = dto.FirstName
                If dto.LastName IsNot Nothing Then
                    person.LastName = dto.LastName
                End If
                If dto.DisplayName <> "" Then
                    person.DisplayName = dto.DisplayName
                Else
                    person.DisplayName += String.Format("{0} {1}", dto.FirstName, dto.LastName)
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
            person.RegisterTypes = dto.RegisterTypes

            If dto.PersonId > 0 Then
                person.OldId = person.OldId
                personCtrl.UpdatePerson(person)

                personHistory.PersonId = dto.PersonId
                personHistory.HistoryText = "<p>Informações da conta alterada.</p>"
                personHistory.CreatedByUser = -1
                personHistory.CreatedOnDate = dto.CreatedOnDate

                personHistoryCtrl.AddPersonHistory(personHistory)
            Else

                'defaultStatus = defaultStatusCtrl.getStatus("Normal", dto.PortalId)

                'If defaultStatus IsNot Nothing Then
                '    person.StatusId = defaultStatus.StatusId
                'Else
                person.StatusId = 1 ' defaultStatusCtrl.getStatuses(dto.PortalId, "False")(0).StatusId
                'End If

                person.CreditLimit = 0
                person.MonthlyIncome = 0
                person.OldId = person.PersonId

                personCtrl.AddPerson(person)

                personHistory.PersonId = person.PersonId
                personHistory.HistoryText = "<p>Conta gerada.</p>"
                personHistory.CreatedByUser = -1
                personHistory.CreatedOnDate = dto.CreatedOnDate

                personHistoryCtrl.AddPersonHistory(personHistory)

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
                    addressCtrl.AddPersonAddress(address)
                End If

                'contact.PersonId = person.PersonId
                'contact.PersonAddressId = address.PersonAddressId
                'contact.DateBirth = CDate("01/01/1900")
                'contact.CreatedByUser = dto.CreatedByUser
                'contact.CreatedOnDate = dto.CreatedOnDate

                'contactCtrl.addPersonContact(contact)
            End If

            Dim newUId = Null.NullInteger

            Dim oUserInfo As New UserInfo() With {.PortalID = PortalController.Instance.GetCurrentPortalSettings().PortalId,
                .FirstName = dto.FirstName,
                .LastName = dto.LastName,
                .AffiliateID = Null.NullInteger,
                .LastIPAddress = Authentication.AuthenticationLoginBase.GetIPAddress()}

            If person.PersonId > 0 Then

                If dto.Industries IsNot Nothing Then
                    If dto.Industries <> "[]" Then
                        dto.Industries = dto.Industries.Replace("[", "").Replace("]", "")
                        Dim indusPerson As New Components.Models.PersonIndustry
                        Dim indusCtrl As New PersonIndustryRepository
                        indusCtrl.RemovePersonIndustries(person.PersonId)
                        For Each item In dto.Industries.Split(","c)
                            indusPerson.PersonId = person.PersonId
                            'indusPerson.IndustryId = item
                            indusCtrl.AddPersonIndustry(indusPerson)
                        Next
                    End If
                End If

                If dto.CreateLogin Then
                    If dto.Email IsNot Nothing Then

                        Dim pass = ""
                        pass = If(dto.Password IsNot Nothing, dto.Password, Utilities.GeneratePassword(7))

                        oUserInfo.Username = dto.Email.ToLower().Trim()
                        oUserInfo.Email = dto.Email.ToLower().Trim()
                        oUserInfo.Membership.Approved = True
                        oUserInfo.Membership.Password = pass
                        oUserInfo.Membership.UpdatePassword = False

                        Dim objUserCreateStatus = UserController.CreateUser(oUserInfo)

                        If objUserCreateStatus = UserCreateStatus.Success Then

                            'set roles here
                            Dim objRoleCtrl As New RoleController
                            Dim objRoleInfo = objRoleCtrl.GetRoleByName(PortalController.Instance.GetCurrentPortalSettings().PortalId, "Clientes")
                            objRoleCtrl.AddUserRole(PortalController.Instance.GetCurrentPortalSettings().PortalId, oUserInfo.UserID, objRoleInfo.RoleID, Null.NullDate, Null.NullDate)
                            objRoleCtrl.AddUserRole(PortalController.Instance.GetCurrentPortalSettings().PortalId, oUserInfo.UserID, PortalSettings.RegisteredRoleId, Null.NullDate, Null.NullDate)

                            oUserInfo.Profile.SetProfileProperty("Photo", "0")
                            oUserInfo.Profile.SetProfileProperty("PreferredTimeZone", "E. South America Standard Time")
                            oUserInfo.Profile.SetProfileProperty("PreferredLocale", "pt-BR")

                            Profile.ProfileController.UpdateUserProfile(oUserInfo)

                            person.UserId = oUserInfo.UserID

                            personCtrl.UpdatePerson(person)

                            DataCache.ClearCache()

                            newUId = oUserInfo.UserID
                        Else
                            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Não possível criar o login. Email inválido ou já cadastrado.", .PersonId = person.PersonId})
                        End If
                    Else
                        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Não possível criar o login. Email inválido ou já cadastrado.", .PersonId = person.PersonId})
                    End If
                End If
            End If

            If person.UserId > 0 Then
                oUserInfo = UserController.GetUserById(dto.PortalId, person.UserId)

                If Not Null.IsNull(oUserInfo) Then
                    oUserInfo.DisplayName = person.DisplayName

                    newUId = oUserInfo.UserID

                    UserController.UpdateUser(dto.PortalId, oUserInfo)

                    Profile.ProfileController.UpdateUserProfile(oUserInfo)
                End If
            End If

            If dto.SyncEnabled Then
                Dim pessoa As New Components.Models.Softer
                Dim pessoaCtrl As New SofterRepository

                pessoa.admissao = New Date(1900, 1, 1)
                pessoa.atividadevendedor = 0
                pessoa.codvendedor = person.SalesRep

                If dto.UserId Then
                    Dim userCtrl As New UserController
                    Dim user = userCtrl.GetUser(dto.PortalId, dto.UserId)
                    If user.IsInRole("Vendedores") Then
                        pessoa.atividadevendedor = 1
                    End If
                End If

                pessoa.ativo = Not person.IsDeleted
                pessoa.bloqueiodefinitivo = person.IsDeleted
                pessoa.bloquiodecredito = person.IsDeleted
                pessoa.carencia = 0

                pessoa.cobrarjuros = False
                pessoa.codclasse = 0
                pessoa.codconjuge = 0
                pessoa.codconvenio = 0

                pessoa.codigo = person.OldId

                pessoa.codprofissao = 0

                If dto.Industries IsNot Nothing Then
                    If dto.Industries <> "[]" Then
                        dto.Industries = dto.Industries.Replace("[", "").Replace("]", "")
                        pessoa.codprofissao = dto.Industries.Split(","c)(0)
                    End If
                End If

                pessoa.ctps = ""
                pessoa.datacadastro = person.CreatedOnDate
                pessoa.email = CStr(If(person.Email <> "", person.Email, "")).Trim()
                pessoa.estadocivil = ""

                pessoa.filiacao = ""
                pessoa.foneref1 = ""
                pessoa.foneref2 = ""
                pessoa.homepage = CStr(If(person.Website <> "", person.Website, "")).Trim()
                pessoa.inscricaorg = ""

                pessoa.codregiao = 0
                pessoa.datanasfundacao = New Date(1900, 1, 1) ' person.DateRegistered
                pessoa.demissao = New Date(1900, 1, 1)

                pessoa.limitecredito = 0

                pessoa.ceppessoa = "00000000"
                pessoa.tipologradouropessoa = "RUA"
                pessoa.logradouropessoa = ""
                pessoa.estadopessoa = ""
                pessoa.numero = ""
                pessoa.complemento = ""
                pessoa.cidadepessoa = ""
                pessoa.bairropessoa = ""

                If address.PersonAddressId > 0 Then
                    If address.PostalCode IsNot Nothing Then
                        pessoa.ceppessoa = address.PostalCode.Replace(".", "").Replace("-", "").PadRight(8, "0")
                    End If

                    If address.Street.Trim() <> "" Then
                        pessoa.estadopessoa = CStr(If(address.Region <> "", address.Region, "")).Trim()
                        pessoa.numero = CStr(If(address.Unit <> "", address.Unit, "")).Trim()
                        pessoa.complemento = CStr(If(address.Complement <> "", address.Complement, "")).Trim()
                        pessoa.cidadepessoa = CStr(If(address.City <> "", address.City, "")).Trim()
                        pessoa.bairropessoa = CStr(If(address.District <> "", address.District, "")).Trim()
                        Dim streetAddress = address.Street.Split(" ")
                        pessoa.tipologradouropessoa = streetAddress(0)
                        pessoa.logradouropessoa = address.Street.Replace(pessoa.tipologradouropessoa, "").Trim()
                    End If
                End If

                pessoa.local_trabalho = ""
                pessoa.cidadetrabalho = ""
                pessoa.complementotrabalho = ""
                pessoa.estadotrabalho = ""
                pessoa.tipologradourotrabalho = "RUA"
                pessoa.numerotrabalho = ""
                pessoa.ceptrabalho = "00000000"
                pessoa.bairrotrabalho = ""
                pessoa.logradourotrabalho = ""

                pessoa.nasccontato = New Date(1900, 1, 1)
                pessoa.nasccontato2 = New Date(1900, 1, 1)
                pessoa.naturalidade = ""

                pessoa.obs = CStr(If(person.Comments <> "", person.Comments, "")).Trim()
                pessoa.pagarcomissao = False
                pessoa.nacionalidade = "BRASILEIRA"

                pessoa.fantasia = String.Format("{0} {1}", person.FirstName, person.LastName).Trim()
                pessoa.razao = String.Format("{0} {1}", person.FirstName, person.LastName).Trim()

                If dto.CompanyName IsNot Nothing Then
                    pessoa.razao = dto.CompanyName
                    If dto.DisplayName <> "" Then
                        pessoa.fantasia = dto.DisplayName
                    Else
                        pessoa.fantasia = dto.CompanyName
                    End If
                Else
                    pessoa.razao = String.Format("{0}", dto.FirstName)
                    If dto.DisplayName <> "" Then
                        pessoa.fantasia = dto.DisplayName
                    End If
                    If dto.LastName IsNot Nothing Then
                        pessoa.razao = String.Format("{0} {1}", dto.FirstName, dto.LastName)
                    Else
                        pessoa.razao = String.Format("{0}", dto.FirstName)
                    End If
                End If

                pessoa.referencia1 = ""
                pessoa.referencia2 = ""
                pessoa.renda = CSng(If(person.MonthlyIncome <> 0, person.MonthlyIncome, 0))
                pessoa.revendaconsumidor = "C"
                pessoa.salario = CSng(If(person.MonthlyIncome <> 0, person.MonthlyIncome, 0))
                pessoa.sexo = "M"

                If person.Telephone <> "" Then
                    pessoa.telefoneprincipal = person.Telephone
                    pessoa.contato = dto.FirstName
                Else
                    pessoa.telefoneprincipal = ""
                    pessoa.contato = ""
                End If

                pessoa.telefone2 = CStr(If(person.Cell <> "", person.Cell, ""))
                pessoa.contato2 = ""

                pessoa.fax = CStr(If(person.Fax <> "", person.Fax, ""))

                Dim roleCtrl As New RoleController
                If person.RegisterTypes.Split(","c).Length > 1 Then
                    Dim types = person.RegisterTypes.Split(","c)
                    For Each Type In types
                        Dim roleName = roleCtrl.GetRoleById(dto.PortalId, CInt(Type.Replace(",", ""))).RoleName
                        Select Case roleName
                            Case "Fornecedores"
                                pessoa.tipopessoa = 2
                            Case "Transportadoras"
                                pessoa.tipopessoa = 5
                            Case "Revendedores"
                                pessoa.tipopessoa = 1
                                pessoa.revendaconsumidor = "R"
                            Case Else
                                pessoa.tipopessoa = 1
                                Exit Select
                        End Select
                    Next
                Else
                    Dim roleName = roleCtrl.GetRoleById(dto.PortalId, CInt(person.RegisterTypes.Split(","c)(0).Replace(",", ""))).RoleName
                    Select Case roleName
                        Case "Fornecedores"
                            pessoa.tipopessoa = 2
                        Case "Transportadoras"
                            pessoa.tipopessoa = 5
                        Case "Revendedores"
                            pessoa.tipopessoa = 1
                            pessoa.revendaconsumidor = "R"
                        Case Else
                            pessoa.tipopessoa = 1
                    End Select
                End If

                pessoa.usarendprincipal = True

                If person.PersonType Then
                    pessoa.insc_municipal = person.CityTax
                End If

                If person.PersonType Then
                    pessoa.cgccpf = CStr(If(person.CPF <> "", person.CPF, ""))
                Else
                    pessoa.cgccpf = CStr(If(person.EIN <> "", person.EIN, ""))
                End If

                If person.PersonType Then
                    pessoa.inscricaorg = CStr(If(person.Ident <> "", person.Ident, "")).Trim()
                Else
                    pessoa.inscricaorg = CStr(If(person.StateTax <> "", person.StateTax, "")).Trim()
                End If

                pessoa.tipopessoafj = CStr(If(person.PersonType, "F", "J"))

                Dim theResult = -1

                If Not dto.PersonId > 0 Then
                    theResult = pessoaCtrl.AddSGIPessoa(pessoa)
                Else
                    theResult = pessoaCtrl.UpdateSGIPessoa(pessoa)
                End If
            End If

            ''Check if this is a products Folder
            'Dim fileDir = New System.IO.DirectoryInfo(PortalSettings.HomeDirectoryMapPath & "People\")
            'Dim path As String = fileDir.ToString
            ''If fileDir.Exists = False Then
            ''    'Add File folder
            ''    DotNetNuke.Services.FileSystem.FolderManager.Instance().AddFolder(person.PortalId, "People/")
            ''End If
            'Utilities.CreateDir(PortalController.Instance.GetCurrentPortalSettings(), "People")

            'Dim filePath = String.Format("{0}{1}.TXT", path, person.PersonId.ToString())

            'Dim saveString As New StringBuilder
            'saveString.Append("10") ' N Tipo de registro 1
            'saveString.Append("01") ' N Cliente ou fornecedor 3
            'saveString.Append(person.DisplayName.PadRight(60)) ' C Nome fantasia 5
            'saveString.Append(CStr(IIf(person.CompanyName <> "", person.CompanyName, String.Format("{0} {1}", person.FirstName, person.LastName))).PadRight(60)) ' C Razão social 65
            'saveString.Append("".PadRight(5)) ' Numero do endereço 125
            'saveString.Append("".PadRight(10)) ' Complemento do endereço 130
            'saveString.Append(CStr(IIf(person.PersonType, "F", "J"))) ' Perssoa física ou juríduca 140
            'If person.CPF IsNot Nothing OrElse person.EIN IsNot Nothing Then
            '    saveString.Append(CStr(IIf(person.PersonType, person.CPF, person.EIN)).Replace("-", "").Replace(".", "").Replace("/", "").PadRight(18)) 'N CPF ou CNPJ 141
            'Else
            '    saveString.Append("".PadRight(18)) 'N CPF ou CNPJ 141
            'End If
            'If person.Ident IsNot Nothing OrElse person.CityTax IsNot Nothing Then
            '    If person.PersonType Then
            '        saveString.Append(CStr(IIf(person.CityTax IsNot Nothing, person.CityTax, "")).PadRight(15)) 'C Ins Est 159
            '    Else
            '        saveString.Append(CStr(IIf(person.Ident IsNot Nothing, person.Ident, "")).PadRight(15)) 'C Identidade 159
            '    End If
            'Else
            '    saveString.Append("".PadRight(15)) 'C Ins Est ou Identidade 159
            'End If
            'saveString.Append(CStr(IIf(person.Website IsNot Nothing, person.Website, "")).PadRight(80)) 'C Website 174
            'saveString.Append("1") 'B Ativado ou Não 254
            'saveString.Append("C") 'C Revendedor ou Não 255
            'saveString.Append(person.SalesRep.ToString().PadLeft(6, "0")) 'N Código do vendedor 256
            'saveString.Append("000000") 'N Código do convênio 262
            'saveString.Append("000000") 'C Código da região 268
            'saveString.Append("000000") 'C Código da classe 274
            'saveString.Append("000000") 'C Código da profissão 280
            'saveString.Append("".PadRight(10)) 'C Tipo de logradouro 286
            'saveString.Append("".PadRight(60)) 'C Logradouro 296
            'saveString.Append("".PadRight(30)) 'C Bairro 356
            'saveString.Append("".PadRight(60)) 'C Cidade 386
            'saveString.Append("".PadRight(2)) 'C Estado 446
            'saveString.Append("".PadRight(8)) 'C CEP 448
            'saveString.Append("".PadRight(10)) 'C Tipo de logradouro do trabalho 456
            'saveString.Append("".PadRight(60)) 'C Logradouro do trabalho 466
            'saveString.Append("".PadRight(30)) 'C Bairro do  da trabalho 526
            'saveString.Append("".PadRight(60)) 'C Cidade do trabalho 556
            'saveString.Append("".PadRight(2)) 'C Estado do trabalho 616
            'saveString.Append("".PadRight(8)) 'C CEP do trabalho 618
            'saveString.Append("".PadRight(1)) 'C Estado civil 626
            'saveString.Append("".PadRight(100)) 'C Nome do pai e mãe 627
            'saveString.Append("M".PadRight(1)) 'C Masculino ou feminino 727
            'saveString.Append("BRASILEIRA".PadRight(20)) 'C Nacionalidade 728
            'saveString.Append("".PadRight(20)) 'C Naturalidade 748
            'saveString.Append("".PadRight(5)) 'C Número do trabalho 768
            'saveString.Append("".PadRight(10)) 'C Complemento do trabalho 773
            'saveString.Append("000000".PadLeft(6, "0")) 'C Código da conjuge 783
            'saveString.Append(CStr(IIf(person.Telephone IsNot Nothing, person.Telephone, "")).PadRight(11)) 'C Telefone 789
            'saveString.Append(CStr(IIf(person.Fax IsNot Nothing, person.Fax, "")).PadRight(11)) 'C Fax 800
            'saveString.Append("000000000000") 'N Valor do limite de crédito 811
            'saveString.Append("0") 'B Cobrar juros 823
            'saveString.Append("0") 'B Pagar comissão 824
            'saveString.Append("0") 'B Bloqueio de crédito 825
            'saveString.Append("0") 'B Bloqueio definitivo 826
            'saveString.Append("00") 'N Dias de carência 827
            'saveString.Append("".PadRight(8)) 'D Data de nascimento or fundação 829
            'saveString.Append(person.CreatedOnDate.ToString("ddMMyyyy").PadRight(8)) 'D Data de cadastro DDMMAAAA 837
            'saveString.Append("".PadRight(30)) 'C Nome do contato 845
            'saveString.Append(person.PersonId.ToString().PadLeft(6, "0")) 'C Código de origem875
            'saveString.AppendLine()
            'saveString.Append("12") ' N Tipo de registro 1
            'saveString.Append("01") ' N Cliente ou fornecedor 03
            'saveString.Append(person.PersonId.ToString().PadLeft(6, "0")) 'C Código de origem 5
            'saveString.Append("".PadRight(50)) 'C Local de trabalho 11
            'saveString.Append(0.ToString("#.00").Replace(",", "").PadLeft(12, "0")) 'N Salário do cliente 61
            'saveString.Append(0.ToString("#.00").Replace(",", "").PadLeft(12, "0")) 'Renda familiar do cliente 73
            'saveString.Append("".PadRight(32)) 'C Caiteira de trabalho por tempo e serviço 85
            'saveString.Append("".PadRight(8)) 'D Data de admissão 117
            'saveString.Append("".PadRight(8)) 'D Data de demissão 125
            'saveString.Append("".PadRight(8)) 'D Data de nascimento do contato 133
            'saveString.Append("".PadRight(30)) 'C Nome do segundo contato da pessoa 141
            'saveString.Append("".PadRight(11)) 'C Telefone do segundo contato da pessoa 171
            'saveString.Append("".PadRight(8)) 'D Data de nascimento do segundo contato 182 
            'saveString.Append("".PadRight(40)) 'C Referência comercial 190
            'saveString.Append("".PadRight(20)) 'C Telefone referência comercial 230
            'saveString.Append("".PadRight(40)) 'C Referência comercial 250
            'saveString.Append("".PadRight(20)) 'C Telefone referência comercial 290
            'saveString.Append("1".PadRight(1)) 'B Usar endereço principal p/ cobrança 310
            'saveString.Append("0".PadRight(1)) 'B Atua como revendedor 311
            'saveString.Append(CStr(IIf(person.Comments IsNot Nothing, person.Comments, "")).PadRight(460)) 'C Observação 312
            'saveString.Append(CStr(IIf(person.Email IsNot Nothing, person.Email, "")).PadRight(128)) 'C Email 772

            ''Dim info = New UTF8Encoding(True).GetBytes(saveString.ToString())

            'If File.Exists(filePath) Then
            '    File.SetAttributes(filePath, FileAttributes.Normal)
            '    File.Delete(filePath)
            'End If

            ''IO.File.WriteAllText(filePath, saveString.ToString(), System.Text.Encoding.GetEncoding(1256))

            'Using mySw = New StreamWriter(filePath, True, System.Text.Encoding.Default)
            '    mySw.AutoFlush = True
            '    mySw.Write(saveString)
            '    'fs.Write(info, 0, info.Length)
            'End Using

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .PersonId = person.PersonId, .UserId = person.UserId, .DisplayName = person.DisplayName})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Syncs people from sgi
    ''' </summary>
    ''' <param name="sDate">Modified Date Start</param>
    ''' <param name="eDate">Modified Date End</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function SyncPeople(sDate As String, eDate As String) As HttpResponseMessage
        Try
            Dim counts = SyncSGIPeople(sDate, eDate)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .Added = counts.Added, .Updated = counts.Updated})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Export all clients
    ''' </summary>
    ''' <param name="portalId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ExportAllClients(portalId As Integer) As HttpResponseMessage
        Try

            Dim peopleAddresses As New PersonAddressesRepository
            Dim peopleDataCtrl As New PeopleRepository
            Dim peopleData = peopleDataCtrl.GetPeople(portalId, -1, "", "", "", -1, "", Null.NullDate, Null.NullDate, 2, 1, 10000, "", "")

            'Check if this is a products Folder
            Dim fileDir = New System.IO.DirectoryInfo(PortalSettings.HomeDirectoryMapPath & "People\")
            Dim path As String = fileDir.ToString
            Dim filePath = String.Format("{0}CLIENTES.TXT", path)

            If File.Exists(filePath) Then
                File.SetAttributes(filePath, FileAttributes.Normal)
                File.Delete(filePath)
            End If

            Dim saveString As New StringBuilder
            For Each person In peopleData
                saveString.AppendLine()
                saveString.Append("10") ' N Tipo de registro
                saveString.Append("01") ' N Cliente ou fornecedor
                saveString.Append(person.DisplayName.PadRight(60)) ' C Nome fantasia
                saveString.Append(person.CompanyName.PadRight(60)) ' C Razão social
                saveString.Append("".PadRight(5)) ' Numero do endereço
                saveString.Append("".PadRight(10)) ' Complemento do endereço
                saveString.Append(CStr(If(person.PersonType = 1, "F", "J"))) ' Perssoa física ou juríduca
                If person.CPF IsNot Nothing OrElse person.EIN IsNot Nothing Then
                    saveString.Append(CStr(If(person.PersonType = 1, person.CPF, person.EIN)).PadLeft(18, "0")) 'N CPF ou CNPJ
                Else
                    saveString.Append("000000000000000000") 'N CPF ou CNPJ
                End If
                If person.Ident IsNot Nothing OrElse person.CityTax IsNot Nothing Then
                    saveString.Append(CStr(If(person.PersonType = 1, person.Ident, person.CityTax)).PadRight(15)) 'C Ins Est ou Identidade
                Else
                    saveString.Append("".PadRight(15)) 'C Ins Est ou Identidade
                End If
                saveString.Append(CStr(If(person.Website IsNot Nothing, person.Website, "")).PadRight(80)) 'C Website
                saveString.Append("1") 'B Ativado ou Não
                saveString.Append("0") 'B Revendedor ou Não
                saveString.Append(person.SalesRep.ToString().PadLeft(6, "0")) 'N Código do vendedor
                saveString.Append("000000") 'N Código do convênio
                saveString.Append("000000") 'C Código da região
                saveString.Append("000000") 'C Código da classe
                saveString.Append("000000") 'C Código da profissão
                saveString.Append("".PadRight(10)) 'C Tipo de logradouro
                saveString.Append("".PadRight(60)) 'C Logradouro
                saveString.Append("".PadRight(30)) 'C Bairro
                saveString.Append("".PadRight(60)) 'C Cidade
                saveString.Append("".PadRight(2)) 'C Estado
                saveString.Append("".PadRight(8)) 'C CEP
                saveString.Append("".PadRight(10)) 'C Tipo de logradouro do trabalho
                saveString.Append("".PadRight(60)) 'C Logradouro do trabalho
                saveString.Append("".PadRight(30)) 'C Bairro do  da trabalho
                saveString.Append("".PadRight(60)) 'C Cidade do trabalho
                saveString.Append("".PadRight(2)) 'C Estado do trabalho
                saveString.Append("".PadRight(8)) 'C CEP do trabalho
                saveString.Append("".PadRight(1)) 'C Estado civil
                saveString.Append("".PadRight(100)) 'C Nome do pai e mãe
                saveString.Append("".PadRight(1)) 'C Masculino ou feminino
                saveString.Append("".PadRight(20)) 'C Nacionalidade
                saveString.Append("".PadRight(20)) 'C Naturalidade
                saveString.Append("".PadRight(5)) 'C Número do trabalho
                saveString.Append("".PadRight(10)) 'C Complemento do trabalho
                saveString.Append("000000".PadLeft(6, "0")) 'C Código da conjuge
                saveString.Append(CStr(If(person.Telephone IsNot Nothing, person.Telephone, "")).PadRight(11)) 'C Telefone
                saveString.Append(CStr(If(person.Fax IsNot Nothing, person.Fax, "")).PadRight(11)) 'C Fax
                saveString.Append("000000000000") 'N Valor do limite de crédito
                saveString.Append("0") 'B Cobrar juros
                saveString.Append("0") 'B Pagar comissão
                saveString.Append("0") 'B Bloqueio de crédito
                saveString.Append("0") 'B Bloqueio definitivo
                saveString.Append("0") 'N Ativado ou Não
                saveString.Append("00") 'N Dias de carência
                saveString.Append("".PadRight(8)) 'D Data de nascimento or fundação
                saveString.Append(person.CreatedOnDate.ToString("ddMMyyyy").PadRight(8)) 'D Data de cadastro DDMMAAAA
                saveString.Append("".PadRight(30)) 'C Nome do contato
                saveString.Append(person.PersonId.ToString().PadLeft(6, "0")) 'C Código de origem
                saveString.AppendLine()
                saveString.Append("12")
                saveString.Append("01") ' N Cliente ou fornecedor
                saveString.Append(person.PersonId.ToString().PadLeft(6, "0")) 'C Código de origem
                saveString.Append("".PadRight(50)) 'C Local de trabalho
                saveString.Append(0.ToString("#.00").Replace(",", "").PadLeft(12, "0")) 'N Salário do cliente
                saveString.Append(0.ToString("#.00").Replace(",", "").PadLeft(12, "0")) 'Renda familiar do cliente
                saveString.Append("".PadRight(32)) 'C Caiteira de trabalho por tempo e serviço
                saveString.Append("".PadRight(8)) 'D Data de admissão
                saveString.Append("".PadRight(8)) 'D Data de demissão
                saveString.Append("".PadRight(8)) 'D Data de nascimento do contato
                saveString.Append("".PadRight(30)) 'C Nome do segundo contato da pessoa
                saveString.Append("".PadRight(11)) 'C Telefone do segundo contato da pessoa
                saveString.Append("".PadRight(8)) 'D Data de nascimento do segundo contato
                saveString.Append("".PadRight(40)) 'C Referência comercial 1
                saveString.Append("".PadRight(20)) 'C Telefone referência comercial 1
                saveString.Append("".PadRight(40)) 'C Referência comercial 2
                saveString.Append("".PadRight(20)) 'C Telefone referência comercial 2
                saveString.Append("0".PadRight(1)) 'B Usar endereço principal p/ cobrança
                saveString.Append("0".PadRight(1)) 'B Atua como revendedor
                saveString.Append(person.Comments.PadRight(460)) 'C Observação
                saveString.Append(CStr(If(person.Email IsNot Nothing, person.Email, "")).PadRight(128)) 'C Email

                'If peopleAddresses.GetPersonAddresses(person.PersonId).Count > 0 Then

                'End If
            Next

            Dim info = New UTF8Encoding(True).GetBytes(saveString.ToString())

            If File.Exists(filePath) Then
                File.SetAttributes(filePath, FileAttributes.Normal)
                File.Delete(filePath)
            End If

            Using fs = File.Create(filePath)
                fs.Write(info, 0, info.Length)
            End Using

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Deletes a person
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function DeletePerson(dto As Components.Models.Person) As HttpResponseMessage
        Try
            Dim person As Components.Models.Person
            Dim personCltr As New PeopleRepository

            person = personCltr.GetPerson(dto.PersonId, PortalController.Instance.GetCurrentPortalSettings().PortalId, Null.NullInteger)

            person.IsDeleted = True
            person.ModifiedByUser = dto.ModifiedByUser
            person.ModifiedOnDate = dto.ModifiedOnDate

            personCltr.UpdatePerson(person)

            If dto.UserId > 0 Then
                UserController.DeleteUser(UserController.GetUserById(dto.PortalId, dto.UserId), False, False)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpDelete>
    Function RemovePerson(personId As Integer, Optional userId As Integer = -1, Optional portalId As Integer = 0) As HttpResponseMessage
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

            personAddressCtrl.RemovePersonAddresses(personId)
            clientBankRefCtrl.RemoveClientBankRefs(personId)
            clientCommRefCtrl.RemoveClientCommRefs(personId)
            personContactCtrl.RemovePersonContacts(personId)
            personDocCtrl.RemovePersonDocs(personId)
            personHistoryCtrl.RemovePersonHistories(personId)
            clientIncomeSourceCtrl.RemoveClientIncomeSources(personId)
            personIndustryCtrl.RemovePersonIndustries(personId)
            clientPartnerCtrl.RemoveClientPartners(personId)
            clientPartnerBankRefCtrl.RemoveClientPartnerBankRefs(personId)
            clientPersonalRefCtrl.RemoveClientPersonalRefs(personId)
            personEstimateCtrl.RemoveClientEstimates(personId, portalId)
            personCtrl.RemovePerson(personId, portalId, userId)

            If userId > 0 Then
                Dim userInfo = UserController.GetUserById(portalId, userId)
                If Not Null.IsNull(userInfo) Then
                    If UserController.RemoveUser(userInfo) = False Then
                        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Erro ao excluir o cadastro de login."})
                    End If
                End If
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Restores a person
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")>
    <HttpPut>
    Function RestorePerson(dto As Components.Models.Person) As HttpResponseMessage
        Try
            Dim person As Components.Models.Person
            Dim personCltr As New PeopleRepository

            person = personCltr.GetPerson(dto.PersonId, dto.PortalId, Null.NullInteger)

            person.IsDeleted = False
            person.ModifiedByUser = dto.ModifiedByUser
            person.ModifiedOnDate = dto.ModifiedOnDate

            personCltr.UpdatePerson(person)

            If dto.UserId > 0 Then
                UserController.RestoreUser(UserController.GetUserById(dto.PortalId, dto.UserId))
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Deletes an user
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")>
    <HttpPut>
    Function DeleteUser(dto As Components.Models.Person) As HttpResponseMessage
        Try
            If dto.UserId > 0 Then
                UserController.DeleteUser(UserController.GetUserById(dto.PortalId, dto.UserId), False, False)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes an user
    ''' </summary>
    ''' <param name="portalId">Person ID</param>
    ''' <param name="userId">User ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpDelete>
    Function RemoveUser(portalId As Integer, userId As Integer) As HttpResponseMessage
        Try
            If userId > 0 Then
                Dim userInfo = UserController.GetUserById(portalId, userId)
                If Not Null.IsNull(userInfo) Then
                    If UserController.RemoveUser(userInfo) = False Then
                        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Erro ao excluir o cadastro de login."})
                    End If
                End If
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Restores an user
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")>
    <HttpPut>
    Function RestoreUser(dto As Components.Models.Person) As HttpResponseMessage
        Try
            If dto.UserId > 0 Then
                UserController.RestoreUser(UserController.GetUserById(dto.PortalId, dto.UserId))
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpGet>
    Function GetUserPhoto(portalId As Integer, userId As Integer) As HttpResponseMessage
        Try

            'Dim portalCtrl As New Portals.PortalController()
            Dim oUser As UserInfo = UserController.GetUserById(portalId, userId)
            Dim userPhoto = oUser.Profile.GetPropertyValue("Photo")
            Dim objFileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(CInt(userPhoto))

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .fileName = objFileInfo.FileName, .filePath = objFileInfo.RelativePath})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    <DnnAuthorize>
    <HttpPost>
    Async Function SaveUserPhoto() As Task(Of HttpResponseMessage)
        Try
            If Not Me.Request.Content.IsMimeMultipartContent("form-data") Then
                Throw New HttpResponseException(New HttpResponseMessage(HttpStatusCode.UnsupportedMediaType))
            End If

            'Dim portalCtrl = New Portals.PortalController()
            Dim request As HttpRequestMessage = Me.Request

            Await request.Content.LoadIntoBufferAsync()
            Dim task = request.Content.ReadAsMultipartAsync()
            Dim result = Await task
            Dim contents = result.Contents
            Dim httpContent As HttpContent = contents.Last()
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

            Dim objUserInfo = UserController.GetUserById(portalId, userId)
            Dim userPath = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(objUserInfo)

            Utilities.CreateDir(Utilities.GetPortalSettings(portalId), userPath.FolderPath)

            Dim theFile As Stream = httpContent.ReadAsStreamAsync().Result

            Dim theGuid = Guid.NewGuid().ToString()

            Dim theFileName = String.Format("{0}.{1}", theGuid, imageTypeExtensionMap(uploadedFileMediaType))
            Dim theFilePath = Path.Combine(userPath.PhysicalPath, theFileName)

            If theFile.CanRead Then
                Using theFileStream As New FileStream(theFilePath, FileMode.Create)
                    theFile.CopyTo(theFileStream)
                End Using

                DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(portalId, userPath.FolderPath)

                Dim fileManager = DotNetNuke.Services.FileSystem.FileManager.Instance()
                Dim folderManager = DotNetNuke.Services.FileSystem.FolderManager.Instance()
                Dim folder = folderManager.GetFolder(portalId, userPath.FolderPath)

                Dim objFileInfo = fileManager.GetFile(folder, theFileName)

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
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes user photo
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function RemoveUserPhoto(dto As UserPhoto) As HttpResponseMessage
        Try
            'Dim portalCtrl = New Portals.PortalController()
            Dim theUserInfo = UserController.GetUserById(dto.PortalId, dto.UserId)
            Dim destinationPath = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(theUserInfo)
            Dim objFolderInfo = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetFolder(dto.PortalId, destinationPath.FolderPath)
            Dim objFileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(objFolderInfo, dto.FileName)

            DotNetNuke.Services.FileSystem.FileManager.Instance().DeleteFile(objFileInfo)

            theUserInfo.Profile.SetProfileProperty("Photo", "-1")
            Entities.Profile.ProfileController.UpdateUserProfile(theUserInfo)

            DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(dto.PortalId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Get person inndustries
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetPersonIndustries(personId As Integer) As HttpResponseMessage
        Try
            Dim personIndustriesCtrl As New PersonIndustryRepository

            Dim personIndustriesData = personIndustriesCtrl.GetPersonIndustries(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, personIndustriesData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets person addresses
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetPersonAddresses(personId As Integer) As HttpResponseMessage
        Try
            Dim personAddressesCtrl As New PersonAddressesRepository

            Dim personAddressesData = personAddressesCtrl.GetPersonAddresses(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, personAddressesData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpGet>
    Function GetPersonAddress(personAddressId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim personAddressCtrl As New PersonAddressesRepository

            Dim personAddressData = personAddressCtrl.GetPersonAddress(personAddressId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, personAddressData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates client address
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function UpdatePersonAddress(dto As Components.Models.PersonAddress) As HttpResponseMessage
        Try
            Dim clientAddressCtrl As New PersonAddressesRepository
            Dim personAddress As New Components.Models.PersonAddress

            If dto.PersonAddressId > 0 Then
                personAddress = clientAddressCtrl.GetPersonAddress(dto.PersonAddressId, dto.PersonId)
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
                clientAddressCtrl.UpdatePersonAddress(personAddress)
            Else
                clientAddressCtrl.AddPersonAddress(personAddress)
            End If

            If dto.SyncEnabled Then
                Dim personCtrl As New PeopleRepository
                Dim person = personCtrl.GetPerson(dto.PersonId, 0, Null.NullInteger)

                Dim pessoaCtrl As New SofterRepository
                Dim pessoa = pessoaCtrl.GetSGIPerson(person.PersonId)

                pessoa.codigopessoa = person.PersonId
                pessoa.codigo = person.OldId
                pessoa.admissao = New Date(1900, 1, 1)
                pessoa.datanasfundacao = New Date(1900, 1, 1)
                pessoa.demissao = New Date(1900, 1, 1)
                pessoa.nasccontato = New Date(1900, 1, 1)
                pessoa.nasccontato2 = New Date(1900, 1, 1)
                pessoa.local_trabalho = ""
                pessoa.cidadetrabalho = ""
                pessoa.complementotrabalho = ""
                pessoa.estadotrabalho = ""
                pessoa.tipologradourotrabalho = "RUA"
                pessoa.numerotrabalho = ""
                pessoa.ceptrabalho = "00000000"
                pessoa.bairrotrabalho = ""
                pessoa.logradourotrabalho = ""
                pessoa.carencia = 0
                pessoa.cobrarjuros = False
                pessoa.codclasse = 0
                pessoa.codconjuge = 0
                pessoa.codconvenio = 0
                pessoa.codprofissao = 0
                pessoa.ctps = ""
                pessoa.estadocivil = ""
                pessoa.filiacao = ""
                pessoa.foneref1 = ""
                pessoa.foneref2 = ""
                pessoa.inscricaorg = ""
                pessoa.codregiao = 0
                pessoa.ceppessoa = "00000000"
                pessoa.tipologradouropessoa = "RUA"
                pessoa.logradouropessoa = ""
                pessoa.estadopessoa = ""
                pessoa.numero = ""
                pessoa.complemento = ""
                pessoa.cidadepessoa = ""
                pessoa.bairropessoa = ""
                pessoa.naturalidade = ""
                pessoa.pagarcomissao = False
                pessoa.nacionalidade = "BRASILEIRA"
                pessoa.referencia1 = ""
                pessoa.referencia2 = ""
                pessoa.revendaconsumidor = "C"
                pessoa.sexo = "M"
                pessoa.contato2 = ""

                If personAddress.PostalCode IsNot Nothing Then
                    pessoa.ceppessoa = personAddress.PostalCode.Replace(".", "").Replace("-", "").PadRight(8, "0")
                End If

                If personAddress.Street.Trim() <> "" Then
                    pessoa.estadopessoa = CStr(If(personAddress.Region <> "", personAddress.Region, ""))
                    pessoa.numero = CStr(If(personAddress.Unit <> "", personAddress.Unit, "")).Trim()
                    pessoa.complemento = CStr(If(personAddress.Complement <> "", personAddress.Complement, "")).Trim()
                    pessoa.cidadepessoa = CStr(If(personAddress.City <> "", personAddress.City, "")).Trim()
                    pessoa.bairropessoa = CStr(If(personAddress.District <> "", personAddress.District, "")).Trim()
                    Dim streetAddress = personAddress.Street.Split(" ")
                    pessoa.tipologradouropessoa = streetAddress(0)
                    pessoa.logradouropessoa = personAddress.Street.Replace(pessoa.tipologradouropessoa, "").Trim()
                End If

                pessoa.usarendprincipal = True

                Dim theResult = pessoaCtrl.UpdateSGIPessoa(pessoa)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .cAId = personAddress.PersonAddressId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Disabled a person address
    ''' </summary>
    ''' <param name="dto">Person Address Model</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function DeletePersonAddress(dto As Components.Models.PersonAddress) As HttpResponseMessage
        Try
            Dim personAddress As Components.Models.PersonAddress
            Dim personAddressCtrl As New PersonAddressesRepository

            personAddress = personAddressCtrl.GetPersonAddress(dto.PersonAddressId, dto.PersonId)
            personAddress.IsDeleted = True

            personAddressCtrl.UpdatePersonAddress(personAddress)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Restores a person address
    ''' </summary>
    ''' <param name="dto">Person Address Model</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function RestorePersonAddress(dto As Components.Models.PersonAddress) As HttpResponseMessage
        Try
            Dim personAddress As Components.Models.PersonAddress
            Dim personAddressCtrl As New PersonAddressesRepository

            personAddress = personAddressCtrl.GetPersonAddress(dto.PersonAddressId, dto.PersonId)
            personAddress.IsDeleted = False

            personAddressCtrl.UpdatePersonAddress(personAddress)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpDelete>
    Function RemovePersonAddress(personAddressId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientAddressCtrl As New PersonAddressesRepository

            clientAddressCtrl.RemovePersonAddress(personAddressId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client contacts by client id
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")>
    <HttpGet>
    Function GetPersonContacts(personId As Integer) As HttpResponseMessage
        Try
            Dim personContactsCtrl As New PersonContactsRepository

            Dim personContactsData = personContactsCtrl.GetPersonContacts(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, personContactsData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")>
    <HttpGet>
    Function GetPersonContact(personContactId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientContactCtrl As New PersonContactsRepository

            Dim clientContactData = clientContactCtrl.GetPersonContact(personContactId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientContactData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates client contact
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")>
    <HttpPost>
    Function UpdatePersonContact(dto As Components.Models.PersonContact) As HttpResponseMessage
        Try
            Dim clientContact As New Components.Models.PersonContact
            Dim clientContactCtrl As New PersonContactsRepository

            If dto.PersonContactId > 0 Then
                clientContact = clientContactCtrl.GetPersonContact(dto.PersonContactId, dto.PersonId)
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
                clientContactCtrl.UpdatePersonContact(clientContact)
            Else
                clientContactCtrl.AddPersonContact(clientContact)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .cCId = clientContact.PersonContactId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")>
    <HttpDelete>
    Function RemovePersonContact(personContactId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientContactCtrl As New PersonContactsRepository

            clientContactCtrl.RemovePersonContact(personContactId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client docs by client id
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetPersonDocs(personId As Integer) As HttpResponseMessage
        Try
            Dim clientDocsCtrl As New PersonDocsRepository

            Dim clientDocsData = clientDocsCtrl.GetPersonDocs(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientDocsData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpGet>
    Function GetPersonDoc(cDId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientDocCtrl As New PersonDocsRepository

            Dim clientDocData = clientDocCtrl.GetPersonDoc(cDId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientDocData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates a person account
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous>
    <HttpPost>
    Function UpdateUser(dto As RiwUserInfo) As HttpResponseMessage
        Try
            Dim objRoleCtrl As New RoleController
            Dim oUserInfo As New UserInfo

            If dto.UserId > 0 Then
                oUserInfo = UserController.GetUserById(PortalController.Instance.GetCurrentPortalSettings().PortalId, dto.UserId)
            End If

            oUserInfo.PortalID = PortalController.Instance.GetCurrentPortalSettings().PortalId
            oUserInfo.LastIPAddress = Authentication.AuthenticationLoginBase.GetIPAddress()

            oUserInfo.FirstName = dto.FirstName.Trim()
            oUserInfo.DisplayName = dto.FirstName.Trim()
            If dto.LastName IsNot Nothing Then
                oUserInfo.LastName = dto.LastName.Trim()
                oUserInfo.DisplayName = String.Format("{0} {1}", oUserInfo.DisplayName, dto.LastName.Trim())
            End If

            oUserInfo.Profile.SetProfileProperty("Telephone", dto.Telephone)
            oUserInfo.Profile.SetProfileProperty("Cell", dto.Cell)
            oUserInfo.Profile.SetProfileProperty("Fax", dto.Fax)
            If dto.Comments IsNot Nothing Then
                oUserInfo.Profile.SetProfileProperty("Comments", CStr(If(dto.Comments IsNot Nothing, dto.Comments.Trim(), Nothing)))
            End If
            If dto.Biography IsNot Nothing Then
                oUserInfo.Profile.SetProfileProperty("Biography", CStr(If(dto.Biography IsNot Nothing, dto.Biography.Trim(), Nothing)))
            End If
            oUserInfo.Profile.SetProfileProperty("PostalCode", dto.PostalCode)
            oUserInfo.Profile.SetProfileProperty("Street", dto.Street)
            oUserInfo.Profile.SetProfileProperty("Unit", dto.Unit)
            If dto.Complement IsNot Nothing Then
                oUserInfo.Profile.SetProfileProperty("IM", CStr(If(dto.Complement IsNot Nothing, dto.Complement.Trim(), Nothing)))
            End If
            If dto.District IsNot Nothing Then
                oUserInfo.Profile.SetProfileProperty("LinkedIn", CStr(If(dto.District IsNot Nothing, dto.District.Trim(), Nothing)))
            End If
            If dto.City IsNot Nothing Then
                oUserInfo.Profile.SetProfileProperty("City", CStr(If(dto.City IsNot Nothing, dto.City.Trim(), Nothing)))
            End If
            oUserInfo.Profile.SetProfileProperty("Region", dto.Region)
            oUserInfo.Profile.SetProfileProperty("Country", dto.Country)

            If dto.UserId > 0 Then

                UserController.UpdateUser(PortalController.Instance.GetCurrentPortalSettings().PortalId, oUserInfo)
                Profile.ProfileController.UpdateUserProfile(oUserInfo)

            Else

                If dto.Email IsNot Nothing Then

                    oUserInfo.Email = dto.Email.Trim().ToLower()
                    oUserInfo.Username = dto.Email.ToLower()
                    oUserInfo.AffiliateID = Null.NullInteger

                    Dim pass = ""
                    pass = If(dto.Password IsNot Nothing, dto.Password, Utilities.GeneratePassword(7))

                    oUserInfo.Membership.Approved = True
                    oUserInfo.Membership.Password = pass
                    oUserInfo.Membership.UpdatePassword = False

                    Dim objUserCreateStatus = UserController.CreateUser(oUserInfo)

                    If objUserCreateStatus = UserCreateStatus.Success Then

                        objRoleCtrl.AddUserRole(PortalController.Instance.GetCurrentPortalSettings().PortalId, oUserInfo.UserID, PortalSettings.RegisteredRoleId, Null.NullDate, Null.NullDate)

                        If dto.Groups IsNot Nothing Then
                            Dim groups = dto.Groups.Replace("[", "").Replace("]", "")
                            If groups <> "" Then
                                For Each groupId In groups.Split(","c)
                                    Dim objRoleInfo As RoleInfo = objRoleCtrl.GetRoleById(PortalController.Instance.GetCurrentPortalSettings().PortalId, groupId)
                                    If Not oUserInfo.IsInRole(objRoleInfo.RoleName) Then
                                        objRoleCtrl.AddUserRole(PortalController.Instance.GetCurrentPortalSettings().PortalId, oUserInfo.UserID, objRoleInfo.RoleID, Null.NullDate, Null.NullDate)
                                    End If
                                Next
                            End If
                        End If

                        oUserInfo.Profile.SetProfileProperty("Photo", "0")
                        oUserInfo.Profile.SetProfileProperty("PreferredTimeZone", "E. South America Standard Time")
                        oUserInfo.Profile.SetProfileProperty("PreferredLocale", "pt-BR")

                        Data.DataProvider.Instance().ExecuteNonQuery("CoreMessaging_SetUserPreference", PortalController.Instance.GetCurrentPortalSettings().PortalId, oUserInfo.UserID, -1, -1)

                        Profile.ProfileController.UpdateUserProfile(oUserInfo)

                        DataCache.ClearCache()

                    End If
                End If
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .UserId = oUserInfo.UserID})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates user password
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores,Vendedores")>
    <HttpPut>
    Function UpdateUserPassword(dto As UserPasswordInfo) As HttpResponseMessage
        Try
            Dim portalCtrl = New PortalController()
            Dim portalInfo = portalCtrl.GetPortal(dto.PortalId)
            Dim myPortalSettings = PortalController.Instance.GetPortalSettings(portalInfo.PortalID)

            Dim settingCtrl As New SettingsRepository
            Dim settingsDictionay = settingCtrl.GetSettings(dto.PortalId)

            Dim objEventLog As New EventLogController
            Dim objEventLogInfo As New LogInfo

            Dim theUserInfo = UserController.GetUserById(portalInfo.PortalID, dto.UserId)

            Dim recipientList As New List(Of UserInfo)

            'Dim portalEmail = Portals.PortalController.GetPortalSetting("RIW_StoreEmail", portalInfo.PortalID, portalInfo.Email)
            Dim storeUser As New UserInfo() With {.UserID = Null.NullInteger, .Email = settingsDictionay("storeEmail"), .DisplayName = portalInfo.PortalName}

            'mailMessage.ReplyToList.Add(New Net.Mail.MailAddress(_userInfo.Email, _userInfo.DisplayName))
            Dim mm As New Net.Mail.MailMessage() With {.From = New Net.Mail.MailAddress(storeUser.Email, storeUser.DisplayName), .Subject = dto.Subject, .IsBodyHtml = True}

            If dto.CurrentPassword IsNot Nothing Then

                If UserController.ChangePassword(theUserInfo, dto.CurrentPassword, dto.NewPassword) Then

                    theUserInfo = UserController.GetUserById(portalInfo.PortalID, dto.UserId)
                    recipientList.Add(theUserInfo)

                    mm.Body = dto.MessageBody.Replace("[LOGIN]", theUserInfo.Username).Replace("[SENHA]", UserController.GetPassword(theUserInfo, ""))

                    objEventLogInfo.AddProperty("IP", Authentication.AuthenticationLoginBase.GetIPAddress())
                    objEventLogInfo.LogPortalID = portalInfo.PortalID
                    objEventLogInfo.LogPortalName = portalInfo.PortalName
                    objEventLogInfo.LogUserID = theUserInfo.UserID
                    objEventLogInfo.LogUserName = theUserInfo.Username
                    objEventLogInfo.LogTypeKey = "PASSWORD_ALTERED_SUCCESS"

                    objEventLog.AddLog(objEventLogInfo)

                    'Dim sentMails = distList.SendMail(mailMessage, recipientList, SettingsDictionay("smtpServer"), CInt(SettingsDictionay("smtpPort")), CBool(SettingsDictionay("smtpConnection")), SettingsDictionay("smtpLogin"), SettingsDictionay("smtpPassword"))

                    Dim email As New Thread(Sub() PostOffice.SendMail(mm, recipientList, settingsDictionay("smtpServer"), CInt(settingsDictionay("smtpPort")),
                                                              CBool(settingsDictionay("smtpConnection")), settingsDictionay("smtpLogin"),
                                                              settingsDictionay("smtpPassword"))) With {.IsBackground = True}
                    email.Start()

                    'Notifications.SendStoreEmail(storeUser, _userInfo, Nothing, Nothing, subject, body.Replace("[LOGIN]", _userInfo.Username).Replace("[*********]", Users.UserController.GetPassword(_userInfo, "")), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)

                    Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})

                Else
                    Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Sua senha original está incorreta!"})
                End If

            Else

                If UserController.ChangePassword(theUserInfo, theUserInfo.Membership.Password, dto.NewPassword) Then

                    theUserInfo = UserController.GetUserById(portalInfo.PortalID, dto.UserId)
                    recipientList.Add(theUserInfo)

                    mm.Body = dto.MessageBody.Replace("[LOGIN]", theUserInfo.Username).Replace("[SENHA]", UserController.GetPassword(theUserInfo, ""))

                    objEventLogInfo.AddProperty("IP", Authentication.AuthenticationLoginBase.GetIPAddress())
                    objEventLogInfo.LogPortalID = portalInfo.PortalID
                    objEventLogInfo.LogPortalName = portalInfo.PortalName
                    objEventLogInfo.LogUserID = theUserInfo.UserID
                    objEventLogInfo.LogUserName = theUserInfo.Username
                    objEventLogInfo.LogTypeKey = "PASSWORD_ALTERED_SUCCESS"

                    objEventLog.AddLog(objEventLogInfo)

                    'Dim sentMails = distList.SendMail(mailMessage, recipientList, SettingsDictionay("smtpServer"), CInt(SettingsDictionay("smtpPort")), CBool(SettingsDictionay("smtpConnection")), SettingsDictionay("smtpLogin"), SettingsDictionay("smtpPassword"))

                    Dim email As New Thread(Sub() PostOffice.SendMail(mm, recipientList, settingsDictionay("smtpServer"), CInt(settingsDictionay("smtpPort")),
                                                              CBool(settingsDictionay("smtpConnection")), settingsDictionay("smtpLogin"),
                                                              settingsDictionay("smtpPassword"))) With {.IsBackground = True}
                    email.Start()

                    'Notifications.SendStoreEmail(storeUser, _userInfo, Nothing, Nothing, subject, body.Replace("[LOGIN]", _userInfo.Username).Replace("[*********]", Users.UserController.GetPassword(_userInfo, "")), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)

                    Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
                    'Else

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
    ''' Saves person documents
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous>
    <HttpPost>
    Async Function PostFile() As Task(Of HttpResponseMessage)
        Try
            If Not Me.Request.Content.IsMimeMultipartContent("form-data") Then
                Throw New HttpResponseException(New HttpResponseMessage(HttpStatusCode.UnsupportedMediaType))
            End If

            Dim portalCtrl = New PortalController()
            Dim request As HttpRequestMessage = Me.Request

            Await request.Content.LoadIntoBufferAsync()
            Dim task = request.Content.ReadAsMultipartAsync()
            Dim result = Await task
            Dim contents = result.Contents
            Dim httpContent As HttpContent = contents.Last()
            Dim uploadedFileMediaType As String = httpContent.Headers.ContentType.MediaType

            'Dim _file As Stream = httpContent.ReadAsStreamAsync().Result

            'Dim portalId = contents(1).ReadAsStringAsync().Result
            'Dim folderPath = contents(2).ReadAsStringAsync().Result

            'Dim root = String.Format("{0}{1}", portalCtrl.GetPortal(portalId).HomeDirectoryMapPath, folderPath)
            'Utilities.CreateDir(Utilities.GetPortalSettings(portalId), folderPath)

            'Dim guid_1 = Guid.NewGuid().ToString()

            'Dim _fileName = If(Not String.IsNullOrWhiteSpace(httpContent.Headers.ContentDisposition.FileName), httpContent.Headers.ContentDisposition.FileName, guid_1).Replace("""", String.Empty)
            'Dim _filePath = Path.Combine(root, _fileName)

            'If _file.CanRead Then
            '    Using _fileStream As New FileStream(_filePath, FileMode.Create)
            '        _file.CopyTo(_fileStream)
            '    End Using
            'End If

            'Dim portalId = contents(1).ReadAsStringAsync().Result
            'Dim folderPath = contents(2).ReadAsStringAsync().Result

            Dim personDoc As New Components.Models.PersonDoc()
            Dim personDocCtrl As New PersonDocsRepository()

            personDoc.PortalId = contents(0).ReadAsStringAsync().Result
            personDoc.PersonId = contents(1).ReadAsStringAsync().Result
            personDoc.DocName = contents(2).ReadAsStringAsync().Result
            personDoc.DocDesc = contents(3).ReadAsStringAsync().Result
            personDoc.DocUrl = contents(4).ReadAsStringAsync().Result
            personDoc.FolderPath = contents(5).ReadAsStringAsync().Result
            personDoc.CreatedByUser = contents(6).ReadAsStringAsync().Result
            personDoc.CreatedOnDate = contents(7).ReadAsStringAsync().Result

            Dim root = String.Format("{0}{1}", portalCtrl.GetPortal(personDoc.PortalId).HomeDirectoryMapPath, personDoc.FolderPath)

            Utilities.CreateDir(Utilities.GetPortalSettings(personDoc.PortalId), personDoc.FolderPath)

            Dim theFile As Stream = httpContent.ReadAsStreamAsync().Result

            Dim theGuid = Guid.NewGuid().ToString()

            'Dim _fileName = If(Not String.IsNullOrWhiteSpace(httpContent.Headers.ContentDisposition.FileName), httpContent.Headers.ContentDisposition.FileName, guid_1).Replace("""", String.Empty)
            'Dim _filePath = Path.Combine(root, _fileName)

            Dim theFileName = String.Format("{0}.{1}", theGuid, MediaTypeExtensionMap(uploadedFileMediaType))
            Dim theFilePath = Path.Combine(root, theFileName)

            If theFile.CanRead Then
                Using theFileStream As New FileStream(theFilePath, FileMode.Create)
                    theFile.CopyTo(theFileStream)
                End Using
            End If

            DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(personDoc.PortalId)

            Dim objFolderInfo = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetFolder(personDoc.PortalId, personDoc.FolderPath)
            Dim objFileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(objFolderInfo, theFileName)

            personDoc.FileId = objFileInfo.FileId

            personDocCtrl.AddPersonDoc(personDoc)

            Return request.CreateResponse(HttpStatusCode.Created, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpDelete>
    Function RemovePersonDoc(personDocId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientDocCtrl As New PersonDocsRepository

            Dim doc = clientDocCtrl.GetPersonDoc(personDocId, personId)

            clientDocCtrl.RemovePersonDoc(doc)

            DotNetNuke.Services.FileSystem.FileManager.Instance().DeleteFile(DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(doc.FileId))

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates client finance
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function UpdateClientFinance(dto As Components.Models.Person) As HttpResponseMessage
        Try
            'Dim culture = New CultureInfo("pt-BR")
            'Dim numInfo = culture.NumberFormat

            Dim client As New Components.Models.Person
            Dim clientCtrl As New PeopleRepository

            client = clientCtrl.GetPerson(dto.PersonId, dto.PortalId, Null.NullInteger)

            client.PersonAddressId = dto.PersonAddressId
            client.MonthlyIncome = dto.MonthlyIncome
            client.ModifiedByUser = dto.ModifiedByUser
            client.ModifiedOnDate = dto.ModifiedOnDate

            clientCtrl.UpdatePerson(client)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client income sources
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetClientIncomeSources(personId As Integer) As HttpResponseMessage
        Try
            Dim clientIncomeSourcesCtrl As New ClientIncomeSourcesRepository

            Dim clientIncomeSources = clientIncomeSourcesCtrl.GetClientIncomeSources(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientIncomeSources)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpGet>
    Function GetClientIncomeSource(clientIncomeSourceId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientIncomeSourceCtrl As New ClientIncomeSourcesRepository

            Dim clientIncomeSource = clientIncomeSourceCtrl.GetClientIncomeSource(clientIncomeSourceId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientIncomeSource)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates client income source
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function UpdateClientIncomeSource(dto As Components.Models.ClientIncomeSource) As HttpResponseMessage
        Try
            'Dim culture = New CultureInfo("pt-BR")
            'Dim numInfo = culture.NumberFormat

            Dim clientIncomeSource As New Components.Models.ClientIncomeSource
            Dim clientIncomeSourceCtrl As New ClientIncomeSourcesRepository

            If dto.ClientIncomeSourceId > 0 Then
                clientIncomeSource = clientIncomeSourceCtrl.GetClientIncomeSource(dto.ClientIncomeSourceId, dto.PersonId)
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
                clientIncomeSourceCtrl.UpdateClientIncomeSource(clientIncomeSource)
            Else
                clientIncomeSourceCtrl.AddClientIncomeSource(clientIncomeSource)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", clientIncomeSource.ClientIncomeSourceId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpDelete>
    Function RemoveClientIncomeSource(clientIncomeSourceId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientIncomeCourceCtrl As New ClientIncomeSourcesRepository

            clientIncomeCourceCtrl.RemoveClientIncomeSource1(clientIncomeSourceId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client personal references
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetClientPersonalRefs(personId As Integer) As HttpResponseMessage
        Try
            Dim clientPersonalRefsCtrl As New ClientPersonalRefsRepository

            Dim clientPersonalRefs = clientPersonalRefsCtrl.GetClientPersonalRefs(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientPersonalRefs)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpGet>
    Function GetClientPersonalRef(clientPersonalRefId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientPersonalRefCtrl As New ClientPersonalRefsRepository

            Dim clientPersonalRef = clientPersonalRefCtrl.GetClientPersonalRef(clientPersonalRefId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientPersonalRef)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates client personal reference
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function UpdateClientPersonalRef(dto As Components.Models.ClientPersonalRef) As HttpResponseMessage
        Try
            Dim clientPersonalRef As New Components.Models.ClientPersonalRef
            Dim clientPersonalRefCtrl As New ClientPersonalRefsRepository

            If dto.ClientPersonalRefId > 0 Then
                clientPersonalRef = clientPersonalRefCtrl.GetClientPersonalRef(dto.ClientPersonalRefId, dto.PersonId)
            End If

            clientPersonalRef.PersonId = dto.PersonId
            clientPersonalRef.PREmail = dto.PREmail
            clientPersonalRef.PRName = dto.PRName
            clientPersonalRef.PRPhone = dto.PRPhone

            If dto.ClientPersonalRefId > 0 Then
                clientPersonalRefCtrl.UpdateClientPersonalRef(clientPersonalRef)
            Else
                clientPersonalRefCtrl.AddClientPersonalRef(clientPersonalRef)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpDelete>
    Function RemoveClientPersonalRef(clientPersonalRefId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientPersonalRefCtrl As New ClientPersonalRefsRepository

            clientPersonalRefCtrl.RemoveClientPersonalRef(clientPersonalRefId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client bank references
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetClientBankRefs(personId As Integer) As HttpResponseMessage
        Try
            Dim clientBankRefsCtrl As New ClientBankRefsRepository

            Dim clientBankRefs = clientBankRefsCtrl.GetClientBankRefs(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientBankRefs)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpGet>
    Function GetClientBankRef(clientBankRefId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientBankRefCtrl As New ClientBankRefsRepository

            Dim clientBankRef = clientBankRefCtrl.GetClientBankRef(clientBankRefId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientBankRef)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates client bank reference
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function UpdateClientBankRef(dto As Components.Models.ClientBankRef) As HttpResponseMessage
        Try
            'Dim culture = New CultureInfo("pt-BR")
            'Dim numInfo = culture.NumberFormat

            Dim clientBankRef As New Components.Models.ClientBankRef
            Dim clientBankRefCtrl As New ClientBankRefsRepository

            If dto.ClientBankRefId > 0 Then
                clientBankRef = clientBankRefCtrl.GetClientBankRef(dto.ClientBankRefId, dto.PersonId)
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
                clientBankRefCtrl.UpdateClientBankRef(clientBankRef)
            Else
                clientBankRefCtrl.AddClientBankRef(clientBankRef)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpDelete>
    Function RemoveClientBankReference(clientBankReferenceId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientBankRefCtrl As New ClientBankRefsRepository

            clientBankRefCtrl.RemoveClientBankRef(clientBankReferenceId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client commerce references
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetClientCommRefs(personId As Integer) As HttpResponseMessage
        Try
            Dim clientCommRefsCtrl As New ClientCommRefsRepository

            Dim clientCommRefs = clientCommRefsCtrl.GetClientCommRefs(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientCommRefs)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpGet>
    Function GetClientCommRef(clientCommRefId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientCommRefCtrl As New ClientCommRefsRepository

            Dim clientCommRef = clientCommRefCtrl.GetClientCommRef(clientCommRefId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientCommRef)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates client commerce reference
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function UpdateClientCommRef(dto As Components.Models.ClientCommRef) As HttpResponseMessage
        Try
            'Dim culture = New CultureInfo("pt-BR")
            'Dim numInfo = culture.NumberFormat

            Dim clientCommRef As New Components.Models.ClientCommRef
            Dim clientCommRefCtrl As New ClientCommRefsRepository

            If dto.ClientCommRefId > 0 Then
                clientCommRef = clientCommRefCtrl.GetClientCommRef(dto.ClientCommRefId, dto.PersonId)
            End If

            clientCommRef.PersonId = dto.PersonId
            clientCommRef.CommRefBusiness = dto.CommRefBusiness
            clientCommRef.CommRefContact = dto.CommRefContact
            clientCommRef.CommRefPhone = dto.CommRefPhone
            clientCommRef.CommRefLastActivity = dto.CommRefLastActivity
            clientCommRef.CommRefCredit = dto.CommRefCredit

            If dto.ClientCommRefId > 0 Then
                clientCommRefCtrl.UpdateClientCommRef(clientCommRef)
            Else
                clientCommRefCtrl.AddClientCommRef(clientCommRef)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpDelete>
    Function RemoveClientCommRef(clientCommReferenceId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientCommRefCtrl As New ClientCommRefsRepository

            clientCommRefCtrl.RemoveClientCommRef(clientCommReferenceId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client partners
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetClientPartners(personId As Integer) As HttpResponseMessage
        Try
            Dim clientPartnersCtrl As New ClientPartnersRepository

            Dim clientPartners = clientPartnersCtrl.GetClientPartners(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientPartners)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpGet>
    Function GetClientPartner(clientPartnerId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientPartnerCtrl As New ClientPartnersRepository

            Dim clientPartner = clientPartnerCtrl.GetClientPartner(clientPartnerId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientPartner)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates client partner
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function UpdateClientPartner(dto As Components.Models.ClientPartner) As HttpResponseMessage
        Try
            Dim clientPartner As New Components.Models.ClientPartner
            Dim clientPartnerCtrl As New ClientPartnersRepository

            If dto.ClientPartnerId > 0 Then
                clientPartner = clientPartnerCtrl.GetClientPartner(dto.ClientPartnerId, dto.PersonId)
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
                clientPartnerCtrl.UpdateClientPartner(clientPartner)
            Else
                clientPartnerCtrl.AddClientPartner(clientPartner)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpDelete>
    Function RemoveClientPartner(clientPartnerId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientPartnerCtrl As New ClientPartnersRepository

            clientPartnerCtrl.RemoveClientPartner(clientPartnerId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client partner bank references
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetClientPartnerBankRefs(personId As Integer) As HttpResponseMessage
        Try
            Dim clientPartnerBankRefsCtrl As New ClientPartnersBankRefsRepository

            Dim clientPartnerBankRefs = clientPartnerBankRefsCtrl.GetClientPartnerBankRefs(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientPartnerBankRefs)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpGet>
    Function GetClientPartnerBankRef(clientPartnerBankReferenceId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientPartnerBankRefCtrl As New ClientPartnersBankRefsRepository

            Dim clientPartnerBankRef = clientPartnerBankRefCtrl.GetClientPartnerBankRef(clientPartnerBankReferenceId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, clientPartnerBankRef)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates client partner bank reference
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function UpdateClientPartnerBankRef(dto As Components.Models.ClientPartnerBankRef) As HttpResponseMessage
        Try
            'Dim culture = New CultureInfo("pt-BR")
            'Dim numInfo = culture.NumberFormat

            Dim clientPartnerBankRef As New Components.Models.ClientPartnerBankRef
            Dim clientPartnerBankRefCtrl As New ClientPartnersBankRefsRepository

            If dto.ClientPartnerBankRefId > 0 Then
                clientPartnerBankRef = clientPartnerBankRefCtrl.GetClientPartnerBankRef(dto.ClientPartnerBankRefId, dto.PersonId)
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
                clientPartnerBankRefCtrl.UpdateClientPartnerBankRef(clientPartnerBankRef)
            Else
                clientPartnerBankRefCtrl.AddClientPartnerBankRef(clientPartnerBankRef)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpDelete>
    Function RemoveClientPartnerBankReference(clientPartnerBankRefId As Integer, personId As Integer) As HttpResponseMessage
        Try
            Dim clientPartnerBankRefCtrl As New ClientPartnersBankRefsRepository

            clientPartnerBankRefCtrl.RemoveClientPartnerBankRef(clientPartnerBankRefId, personId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates person login and or email
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function UpdatePersonUserLogin(dto As Components.Models.Person) As HttpResponseMessage
        Try
            Dim settingCtrl As New SettingsRepository
            Dim settingsDictionay = settingCtrl.GetSettings(dto.PortalId)

            Dim person As New Components.Models.Person
            Dim personCtrl As New PeopleRepository

            person = personCtrl.GetPerson(dto.PersonId, dto.PortalId, Null.NullInteger)

            person.Email = dto.Email
            person.ModifiedByUser = dto.ModifiedByUser
            person.ModifiedOnDate = dto.ModifiedOnDate

            personCtrl.UpdatePerson(person)

            UserController.ChangeUsername(dto.UserId, dto.Email)

            Dim oUser = UserController.GetUserById(dto.PortalId, dto.UserId)
            oUser.Email = dto.Email
            UserController.UpdateUser(dto.PortalId, oUser)

            DataCache.ClearUserCache(dto.PortalId, dto.Email)

            Dim recipientList As New List(Of UserInfo)
            Dim myPortalSettings = PortalController.Instance.GetPortalSettings(dto.PortalId)
            Dim portalCtrl = New PortalController()
            Dim portal = portalCtrl.GetPortal(dto.PortalId)
            'Dim portalEmail = Portals.PortalController.GetPortalSetting("RIW_StoreEmail", dto.PortalId, portal.Email)

            Dim storeUser As New UserInfo() With {.UserID = Null.NullInteger, .Email = settingsDictionay("storeEmail"), .DisplayName = portal.PortalName}

            Dim clientUserInfo = UserController.GetUserById(dto.PortalId, dto.UserId)
            recipientList.Add(oUser)

            Dim mm As New Net.Mail.MailMessage() With {.Subject = dto.Subject, .IsBodyHtml = True, .From = New Net.Mail.MailAddress(storeUser.Email, storeUser.DisplayName)}
            mm.ReplyToList.Add(New Net.Mail.MailAddress(oUser.Email, oUser.DisplayName))
            mm.Body = dto.MessageBody.Replace("[LOGIN]", dto.Email).Replace("[SENHA]", UserController.GetPassword(clientUserInfo, ""))

            'Dim sentMails = distList.SendMail(mailMessage, recipientList, SettingsDictionay("smtpServer"), CInt(SettingsDictionay("smtpPort")), CBool(SettingsDictionay("smtpConnection")), SettingsDictionay("smtpLogin"), SettingsDictionay("smtpPassword"))

            Dim email As New System.Threading.Thread(Sub() PostOffice.SendMail(mm, recipientList, settingsDictionay("smtpServer"), CInt(settingsDictionay("smtpPort")),
                                                              CBool(settingsDictionay("smtpConnection")), settingsDictionay("smtpLogin"),
                                                              settingsDictionay("smtpPassword"))) With {.IsBackground = True}
            email.Start()

            'Notifications.SendStoreEmail(storeUser, _clientUserInfo, Nothing, Nothing, dto.Subject, dto.MessageBody.Replace("[login]", dto.Email).Replace("[senha]", Users.UserController.GetPassword(_clientUserInfo, "")), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, dto.PortalId, True)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            'End If

            'Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates person login and or email
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPut>
    Function UpdateUserLogin(dto As Components.Models.Person) As HttpResponseMessage
        Try

            UserController.ChangeUsername(dto.UserId, dto.UserName)

            'Dim oUser = Users.UserController.GetUserById(dto.PortalId, dto.UserId)

            'If dto.Email.IndexOf("@") > 1 Then
            '    oUser.Email = dto.Email
            '    DotNetNuke.Common.Utilities.DataCache.ClearUserCache(dto.PortalId, dto.Email)

            '    'Dim recipientList As New List(Of Users.UserInfo)
            '    ''Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(dto.PortalId)
            '    'Dim portalCtrl = New Portals.PortalController()
            '    'Dim portal = portalCtrl.GetPortal(dto.PortalId)
            '    'Dim portalEmail = Portals.PortalController.GetPortalSetting("RIW_StoreEmail", dto.PortalId, portal.Email)

            '    'Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = portalEmail, .DisplayName = portal.PortalName}

            '    'Dim clienteUserInfo = Users.UserController.GetUserById(dto.PortalId, dto.UserId)
            '    'recipientList.Add(oUser)

            '    'Dim mailMessage As New Net.Mail.MailMessage
            '    'Dim distList As New PostOffice

            '    'Using mailMessage As New Net.Mail.MailMessage
            '    '    mailMessage.From = New Net.Mail.MailAddress(storeUser.Email, storeUser.DisplayName)
            '    '    mailMessage.ReplyToList.Add(New Net.Mail.MailAddress(oUser.Email, oUser.DisplayName))
            '    '    mailMessage.Subject = dto.Subject
            '    '    mailMessage.Body = dto.MessageBody.Replace("[LOGIN]", dto.Email).Replace("[SENHA]", Users.UserController.GetPassword(clienteUserInfo, ""))
            '    '    'mailMessage.Attachments.Add(New Net.Mail.Attachment(Str, String.Format("Orcamento_{0}_{1}.pdf", CStr(dto.EstimateId), Replace(DateTime.Now.ToString(), ":", "-").Replace("/", "-"))))
            '    '    mailMessage.IsBodyHtml = True
            '    'End Using

            '    'Dim sentMails = distList.SendMail(mailMessage, recipientList, SettingsDictionay("smtpServer"), CInt(SettingsDictionay("smtpPort")), CBool(SettingsDictionay("smtpConnection")), SettingsDictionay("smtpLogin"), SettingsDictionay("smtpPassword"))

            '    'Notifications.SendStoreEmail(storeUser, _clientUserInfo, Nothing, Nothing, dto.Subject, dto.MessageBody.Replace("[login]", dto.Email).Replace("[senha]", Users.UserController.GetPassword(_clientUserInfo, "")), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, dto.PortalId, True)

            '    Users.UserController.UpdateUser(dto.PortalId, oUser)
            'End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            'End If

            'Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client history by client id
    ''' </summary>
    ''' <param name="personId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")>
    <HttpGet>
    Function GetHistory(personId As Integer) As HttpResponseMessage
        Try
            Dim historyCtrl As New PersonHistoriesRepository

            Dim histories As New List(Of Components.Models.PersonHistory)

            Dim personHistories = historyCtrl.GetPersonHistories(personId)

            For Each history In personHistories
                Dim personHistoryComments = historyCtrl.GetPersonHistoryComments(history.PersonHistoryId)
                history.HistoryComments = personHistoryComments
                histories.Add(history)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, histories)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates estimate message
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")>
    <HttpPost>
    Function UpdateHistory(dto As Components.Models.PersonHistory) As HttpResponseMessage
        Try
            Dim personHistory As New Components.Models.PersonHistory
            Dim personHistoryCtrl As New PersonHistoriesRepository

            If dto.PersonHistoryId > 0 Then
                personHistory = personHistoryCtrl.GetPersonHistory(dto.PersonHistoryId, dto.PersonId)
            End If

            personHistory.PersonId = dto.PersonId
            personHistory.HistoryText = dto.HistoryText
            personHistory.Locked = dto.Locked
            personHistory.CreatedByUser = dto.CreatedByUser
            personHistory.CreatedOnDate = dto.CreatedOnDate

            If dto.PersonHistoryId > 0 Then
                personHistoryCtrl.UpdatePersonHistory(personHistory)
            Else
                personHistoryCtrl.AddPersonHistory(personHistory)
            End If

            Dim theUserInfo = UserController.GetUserById(PortalController.Instance.GetCurrentPortalSettings().PortalId, personHistory.CreatedByUser)
            personHistory.DisplayName = theUserInfo.DisplayName
            If theUserInfo.Profile.Photo IsNot Nothing Then
                If theUserInfo.Profile.Photo.Length > 2 Then
                    personHistory.Avatar = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(theUserInfo).FolderPath
                    personHistory.Avatar = personHistory.Avatar & DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(theUserInfo.Profile.Photo).FileName
                End If
            End If

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                peopleHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(personHistory)
                'Hub.Clients.AllExcept(MessageComment.connId).pushComment(personHistoryComment, MessageComment.messageIndex)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .personHistory = personHistory})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates person history comment
    ''' </summary>
    ''' <param name="historyComment"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function UpdateHistoryComment(historyComment As HistoryComment) As HttpResponseMessage
        Try
            Dim personHistoryComment As New Components.Models.PersonHistoryComment
            Dim personHistoryCtrl As New PersonHistoriesRepository

            If historyComment.dto.CommentId > 0 Then
                personHistoryComment = personHistoryCtrl.GetPersonHistoryComment(historyComment.dto.CommentId, historyComment.dto.PersonHistoryId)
            End If

            personHistoryComment.PersonHistoryId = historyComment.dto.PersonHistoryId
            personHistoryComment.CommentText = historyComment.dto.CommentText
            personHistoryComment.CreatedByUser = historyComment.dto.CreatedByUser
            personHistoryComment.CreatedOnDate = historyComment.dto.CreatedOnDate

            Dim theUserInfo = UserController.GetUserById(PortalController.Instance.GetCurrentPortalSettings().PortalId, personHistoryComment.CreatedByUser)
            personHistoryComment.DisplayName = theUserInfo.DisplayName
            If theUserInfo.Profile.Photo IsNot Nothing Then
                If theUserInfo.Profile.Photo.Length > 2 Then
                    personHistoryComment.Avatar = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(theUserInfo).FolderPath
                    personHistoryComment.Avatar = personHistoryComment.Avatar & DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(theUserInfo.Profile.Photo).FileName
                End If
            End If

            If historyComment.dto.CommentId > 0 Then
                personHistoryCtrl.UpdatePersonHistoryComment(personHistoryComment)
            Else
                personHistoryCtrl.AddPersonHistoryComment(personHistoryComment)
            End If

            If historyComment.connId IsNot Nothing Then
                '' SignalR
                peopleHub.Value.Clients.AllExcept(historyComment.connId).pushHistoryComment(personHistoryComment, historyComment.messageIndex)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .PersonHistoryComment = personHistoryComment})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpDelete>
    Function RemoveHistoryComment(commentId As Integer, personHistoryId As Integer, connId As String) As HttpResponseMessage
        Try
            Dim personHistoryCtrl As New PersonHistoriesRepository

            personHistoryCtrl.RemovePersonHistoryComment(commentId, personHistoryId)

            If connId IsNot Nothing Then
                '' SignalR
                peopleHub.Value.Clients.AllExcept(connId).removeComment(commentId, personHistoryId)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates person status  and adds person history
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")>
    <HttpPut>
    Function UpdatePersonStatus(dto As Components.Models.Person) As HttpResponseMessage
        Try
            Dim person As New Components.Models.Person
            Dim personCtrl As New PeopleRepository

            Dim personHistory As New Components.Models.PersonHistory
            Dim personHistoryCtrl As New PersonHistoriesRepository

            person = personCtrl.GetPerson(dto.PersonId, PortalController.Instance.GetCurrentPortalSettings().PortalId, Null.NullInteger)
            person.StatusId = dto.StatusId
            person.ModifiedByUser = dto.ModifiedByUser
            person.ModifiedOnDate = dto.ModifiedOnDate
            personCtrl.UpdatePerson(person)

            If dto.HistoryText IsNot Nothing Then

                personHistory.PersonId = dto.PersonId
                personHistory.HistoryText = dto.HistoryText
                personHistory.CreatedByUser = dto.ModifiedByUser
                personHistory.CreatedOnDate = dto.ModifiedOnDate

                personHistoryCtrl.AddPersonHistory(personHistory)
            End If

            Dim theUserInfo = UserController.GetUserById(PortalController.Instance.GetCurrentPortalSettings().PortalId, personHistory.CreatedByUser)
            personHistory.DisplayName = theUserInfo.DisplayName
            If theUserInfo.Profile.Photo IsNot Nothing Then
                If theUserInfo.Profile.Photo.Length > 2 Then
                    personHistory.Avatar = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(theUserInfo).FolderPath
                    personHistory.Avatar = personHistory.Avatar & DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(theUserInfo.Profile.Photo).FileName
                End If
            End If

            If dto.ConnId IsNot Nothing Then
                '' SignalR
                'Hub.Clients.AllExcept(dto.ConnId).pushCancelPayCondition()
                peopleHub.Value.Clients.AllExcept(dto.ConnId).pushHistory(personHistory)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates person sent and adds person history
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Vendedores,Editores")>
    <HttpPut>
    Function UpdateClientSent(dto As Components.Models.Person) As HttpResponseMessage
        Try
            Dim person As New Components.Models.Person
            Dim personCtrl As New PeopleRepository

            person = personCtrl.GetPerson(dto.PersonId, dto.PortalId, Null.NullInteger)
            person.Sent = dto.Sent
            person.ModifiedByUser = dto.ModifiedByUser
            person.ModifiedOnDate = dto.ModifiedOnDate

            personCtrl.UpdatePerson(person)

            If dto.HistoryText IsNot Nothing Then
                Dim personHistory As New Components.Models.PersonHistory
                Dim personHistoryCtrl As New PersonHistoriesRepository

                personHistory.PersonId = dto.PersonId
                personHistory.HistoryText = dto.HistoryText
                personHistory.CreatedByUser = dto.ModifiedByUser
                personHistory.CreatedOnDate = dto.ModifiedOnDate

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
    ''' Removes user photo
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="uId">User ID</param>
    ''' <param name="fileName">File Name</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores,Vendedores")>
    <HttpPost>
    Function RemoveUserPhoto1(ByVal portalId As Integer, ByVal uId As Integer, ByVal fileName As String) As HttpResponseMessage
        Try
            Dim portalCtrl = New PortalController()
            Dim theUserInfo = UserController.GetUserById(portalId, uId)
            Dim destinationPath = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetUserFolder(theUserInfo)
            Dim objFolderInfo = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetFolder(portalId, destinationPath.FolderPath)
            Dim objFileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(objFolderInfo, fileName)

            DotNetNuke.Services.FileSystem.FileManager.Instance().DeleteFile(objFileInfo)

            theUserInfo.Profile.SetProfileProperty("Photo", "-1")
            Entities.Profile.ProfileController.UpdateUserProfile(theUserInfo)

            DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .photoPath = String.Format("/{0}/{1}{2}", portalCtrl.GetPortal(portalId).HomeDirectory, destinationPath.FolderPath, fileName)})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
            Return users.[Select](Function(user) New Models.User(user, PortalSettings())).ToList()
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Nothing
        End Try
    End Function

    <DnnAuthorize>
    <HttpGet>
    Function GetEmployees(portalId As Integer, Optional roleGroupName As String = "", Optional sTerm As String = "", Optional isDeleted As String = "", Optional roleName As String = "") As HttpResponseMessage
        Try
            'Function getEmployees(portalId As Integer, pageIndex As Integer, pageSize As Integer, Optional roleGroupName As String = "", Optional startDate As DateTime = Nothing, Optional endDate As DateTime = Nothing, Optional sTerm As String = "", Optional sortCol As String = "", Optional isDeleted As String = "False", Optional roleName As String = "") As HttpResponseMessage
            'Dim peopleCtrl As New PeopleRepository
            Dim empList = New List(Of PersonInfo)

            Dim objRoleCtrl As New RoleController()
            Dim objRoleGroupInfo = RoleController.GetRoleGroupByName(portalId, roleGroupName)

            Dim rolesList As New ArrayList

            If Not Null.IsNull(objRoleGroupInfo) Then
                rolesList = objRoleCtrl.GetRolesByGroup(portalId, objRoleGroupInfo.RoleGroupID)
            End If

            Dim searchStr As String = HttpContext.Current.Request.Params("filter[filters][0][value]")
            If sTerm Is Nothing Then
                If searchStr Is Nothing Then
                    searchStr = ""
                End If
            Else
                searchStr = sTerm
            End If

            'If startDate = Nothing Then
            '    startDate = CStr(Null.NullDate)
            'End If

            'If endDate = Nothing Then
            '    endDate = CStr(Null.NullDate)
            'End If

            'Dim usersList = peopleCtrl.getUsers(portalId, roleName, isDeleted, searchStr, endDate, startDate, pageIndex, pageSize, sortCol)

            Dim usersList As New ArrayList

            usersList = UserController.GetUsers(True, False, portalId)

            If roleName <> "" Then

                For Each emp In usersList

                    If Not empList.Exists((Function(x As PersonInfo) If((String.Equals(x.UserId, emp.UserID)), True, False))) Then

                        If emp.IsInRole(roleName) Then

                            empList.Add(New PersonInfo() With {
                                  .UserId = emp.UserID,
                                  .FirstName = emp.FirstName,
                                  .LastName = emp.LastName,
                                  .DisplayName = emp.DisplayName,
                                  .ModifiedOnDate = emp.LastModifiedOnDate})

                        End If
                    End If
                Next

            Else

                For Each emp In usersList

                    For Each role As RoleInfo In rolesList

                        If emp.IsInRole(role.RoleName) Then

                            If Not empList.Exists((Function(x As PersonInfo) If((String.Equals(x.UserId, emp.UserID)), True, False))) Then

                                empList.Add(New PersonInfo() With {
                                      .UserId = emp.UserID,
                                      .FirstName = emp.FirstName,
                                      .LastName = emp.LastName,
                                      .DisplayName = emp.DisplayName,
                                      .IsDeleted = emp.IsDeleted,
                                      .ModifiedOnDate = emp.LastModifiedOnDate})
                            End If
                        End If
                    Next
                Next
            End If

            'Dim total = Nothing
            'For Each item In usersList
            '    total = item.TotalRows
            '    Exit For
            'Next

            'Dim usersCountList = New List(Of PersonInfo)
            'Dim usersInRole As New ArrayList
            'For Each _role As RoleInfo In rolesList
            '    If roleName <> "" Then
            '        usersInRole = objRoleCtrl.GetUsersByRoleName(portalId, roleName)
            '    Else
            '        usersInRole = objRoleCtrl.GetUsersByRoleName(portalId, _role.RoleName)
            '    End If

            '    For Each _user As Users.UserInfo In usersInRole
            '        'If Not usersCountList.Exists((Function(x As PersonInfo) If((String.Equals(x.UserId, _user.UserID)), True, False))) Then
            '        usersCountList.Add(New PersonInfo() With {
            '              .UserId = _user.UserID})
            '        'End If
            '    Next
            'Next

            'If roleName <> "" Then

            '    For Each emp As Users.UserInfo In usersList
            '        If Not emp.IsInRole(roleName) Then
            '            usersList.Remove(emp)
            '        End If
            '    Next

            'End If

            'total = usersList.Count

            Return Request.CreateResponse(HttpStatusCode.OK, empList)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets list of users by portal id and role group name
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="roleGroupName">Role Group Name</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetUsersByRoleGroup(portalId As Integer, roleGroupName As String, Optional roleName As String = "", Optional isDeleted As String = "") As HttpResponseMessage
        Try

            Dim usersList = New List(Of PersonInfo)

            Dim objRoleCtrl As New RoleController()
            Dim objRoleGroupInfo = RoleController.GetRoleGroupByName(portalId, roleGroupName)

            Dim rolesList = objRoleCtrl.GetRolesByGroup(portalId, objRoleGroupInfo.RoleGroupID)

            For Each role As RoleInfo In rolesList

                Dim usersInRole As New ArrayList
                usersInRole = If(roleName <> "", objRoleCtrl.GetUsersByRoleName(portalId, roleName), objRoleCtrl.GetUsersByRoleName(portalId, role.RoleName))

                For Each user As UserInfo In usersInRole

                    If Not usersList.Exists((Function(x As PersonInfo) If((String.Equals(x.UserId, user.UserID)), True, False))) Then

                        usersList.Add(New PersonInfo() With {
                              .UserId = user.UserID,
                              .UserName = user.Username,
                              .FirstName = user.FirstName,
                              .LastName = user.LastName,
                              .DisplayName = user.DisplayName,
                              .ModifiedOnDate = user.LastModifiedOnDate})
                    End If
                Next
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, usersList)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpGet>
    Function GetUsersByRoleName(portalId As Integer, roleName As String) As HttpResponseMessage
        Try
            Dim roleCtlr As New RoleController
            Dim usersData = roleCtlr.GetUsersByRole(portalId, roleName)

            Dim users As New List(Of UserInfo)
            For Each user As UserInfo In usersData
                users.Add(user)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, GetUsers(users))
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize>
    <HttpGet>
    Function GetUser(portalId As Integer, userId As Integer) As HttpResponseMessage
        Try
            Dim users = New List(Of UserInfo)
            Dim user = UserController.GetUserById(portalId, userId)
            users.Add(user)

            Return Request.CreateResponse(HttpStatusCode.OK, GetUsers(users))
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    Public Shared Function SyncSGIPeople(ByVal sDate As String, ByVal eDate As String) As Counts
        Dim personHistory As New Components.Models.PersonHistory
        Dim personHistoryCtrl As New PersonHistoriesRepository
        Dim peopleCtrl As New PeopleRepository
        Dim pessoasCtrl As New SofterRepository
        Dim person As New Components.Models.Person
        Dim existingPerson As New Components.Models.Person

        Dim pessoas = pessoasCtrl.GetSGIPeople(sDate, eDate)
        Dim addedCount = 0
        Dim updatedCount = 0

        If pessoas.Count Then
            For Each pessoa In pessoas

                person.DateFounded = New Date(1900, 1, 1)
                person.DateRegistered = New Date(1900, 1, 1)
                person.PortalId = 0
                person.PersonType = CBool(If(pessoa.natureza = "F", True, False))
                person.EIN = CStr(If(pessoa.natureza = "F", "", pessoa.cpf_cnpj))
                person.CPF = CStr(If(pessoa.natureza = "F", pessoa.cpf_cnpj, ""))
                person.SalesRep = pessoa.vendedor
                person.Blocked = pessoa.bloqueado
                person.CompanyName = ""
                person.FirstName = pessoa.nome.Trim()
                person.LastName = ""
                person.DisplayName = pessoa.fantasia.Trim()

                If pessoa.nome.IndexOf(" ") > 0 Then
                    person.FirstName = pessoa.nome.Substring(0, pessoa.nome.IndexOf(" ")).Trim()
                    person.LastName = pessoa.nome.Remove(0, pessoa.nome.IndexOf(" ")).Trim()
                End If

                If pessoa.natureza = "J" Then
                    person.CompanyName = pessoa.nome.Trim()
                End If

                person.Telephone = pessoa.telefone
                person.Cell = ""
                person.Fax = ""
                person.Zero800s = ""
                person.Email = pessoa.email
                person.Website = pessoa.website
                person.Ident = CStr(If(pessoa.natureza = "F", pessoa.rg_insc_est, ""))
                person.StateTax = CStr(If(pessoa.natureza = "J", "", pessoa.rg_insc_est))
                person.CityTax = pessoa.insc_municipal
                person.Comments = pessoa.observacao
                person.OldId = pessoa.codigopessoa

                Dim roleCtrl As New RoleController
                Select Case pessoa.tipo
                    Case "1"
                        person.RegisterTypes = roleCtrl.GetRoleByName(0, "Clientes").RoleID.ToString()
                    Case "2"
                        person.RegisterTypes = roleCtrl.GetRoleByName(0, "Fornecedores").RoleID.ToString()
                    Case Else

                End Select

                person.ModifiedByUser = 2
                person.ModifiedOnDate = pessoa.data_alteracao
                person.CreatedByUser = 2
                person.CreatedOnDate = pessoa.data_cadastro
                person.ModifiedByUser = 2
                person.ModifiedOnDate = Now()

                existingPerson = peopleCtrl.GetPerson(pessoa.codigo, 0, -1)

                If existingPerson IsNot Nothing Then
                    person.PersonId = existingPerson.PersonId
                    person.StatusId = existingPerson.StatusId
                    person.ReasonBlocked = existingPerson.ReasonBlocked
                    person.ModifiedByUser = existingPerson.ModifiedByUser
                    person.ModifiedOnDate = Now()

                    peopleCtrl.UpdatePerson(person)

                    personHistory.PersonId = existingPerson.PersonId
                    personHistory.HistoryText = "<p>Informações da conta alterada.</p>"
                    personHistory.CreatedByUser = -1
                    personHistory.CreatedOnDate = Now()

                    personHistoryCtrl.AddPersonHistory(personHistory)

                    person.PersonId = existingPerson.PersonId

                    updatedCount = updatedCount + 1
                Else

                    person.StatusId = 1
                    person.CreditLimit = 0
                    person.MonthlyIncome = 0
                    person.OldId = person.PersonId

                    peopleCtrl.AddPerson(person)

                    personHistory.PersonId = person.PersonId
                    personHistory.HistoryText = "<p>Conta gerada.</p>"
                    personHistory.CreatedByUser = -1
                    personHistory.CreatedOnDate = pessoa.data_cadastro

                    personHistoryCtrl.AddPersonHistory(personHistory)

                    addedCount = addedCount + 1
                End If

                If pessoa.logradouro.Trim() <> "" Then

                    Dim addressCtrl As New PersonAddressesRepository

                    Dim existingAddress = addressCtrl.GetPersonMainAddress(person.PersonId)

                    If existingAddress IsNot Nothing Then

                        existingAddress.PersonId = person.PersonId
                        existingAddress.Street = pessoa.tipo_logradouro + " " + pessoa.logradouro
                        existingAddress.Unit = pessoa.numero
                        existingAddress.Complement = pessoa.complemento
                        existingAddress.District = pessoa.bairro
                        existingAddress.City = pessoa.cidade
                        existingAddress.Region = pessoa.estado
                        existingAddress.PostalCode = pessoa.cep.Replace(".", "").Replace("-", "")
                        existingAddress.Telephone = pessoa.telefone
                        existingAddress.Cell = ""
                        existingAddress.Country = pessoa.nompais
                        existingAddress.ModifiedByUser = 2
                        existingAddress.ModifiedOnDate = Now()
                        existingAddress.AddressName = "Principal"
                        existingAddress.ViewOrder = 1

                        addressCtrl.UpdatePersonAddress(existingAddress)
                    Else
                        Dim newAddress As New Components.Models.PersonAddress

                        newAddress.PersonId = person.PersonId
                        newAddress.Street = pessoa.tipo_logradouro + " " + pessoa.logradouro
                        newAddress.Unit = pessoa.numero
                        newAddress.Complement = pessoa.complemento
                        newAddress.District = pessoa.bairro
                        newAddress.City = pessoa.cidade
                        newAddress.Region = pessoa.estado
                        newAddress.PostalCode = pessoa.cep.Replace(".", "").Replace("-", "")
                        newAddress.Telephone = pessoa.telefone
                        newAddress.Cell = ""
                        newAddress.Country = pessoa.nompais
                        newAddress.ModifiedByUser = 2
                        newAddress.ModifiedOnDate = Now()
                        newAddress.AddressName = "Principal"
                        newAddress.ViewOrder = 1
                        newAddress.CreatedByUser = 2
                        newAddress.CreatedOnDate = Now()

                        addressCtrl.AddPersonAddress(newAddress)
                    End If
                End If
            Next
        End If

        Dim counts As New Counts
        counts.Added = addedCount
        counts.Updated = updatedCount

        Return counts
    End Function

    ''' <summary>
    ''' Gets client info
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpGet>
    Function GetSGIPersonAddress(personId As Integer) As HttpResponseMessage
        Try
            Dim personAddressCtrl As New SofterRepository

            Dim personAddress = personAddressCtrl.GetSGIPersonAddress(personId)

            Return Request.CreateResponse(HttpStatusCode.OK, personAddress)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets client info
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function SyncSGIPersonAddress(codigo As Integer) As HttpResponseMessage
        Try
            Dim pessoaAddressCtrl As New SofterRepository
            Dim pessoaAddress = pessoaAddressCtrl.GetSGIPersonAddress(codigo)

            Dim personAddressCtrl As New PersonAddressesRepository
            Dim personAddress As New Components.Models.PersonAddress

            personAddress.PersonId = codigo
            personAddress.AddressName = "Principal"
            personAddress.Street = pessoaAddress.tipo_logradouro & " " & pessoaAddress.logradouro
            personAddress.Unit = pessoaAddress.numero
            personAddress.Complement = pessoaAddress.complemento
            personAddress.District = pessoaAddress.bairro
            personAddress.City = pessoaAddress.cidade
            personAddress.Region = pessoaAddress.estado
            personAddress.PostalCode = pessoaAddress.cep
            personAddress.Country = pessoaAddress.nompais
            personAddress.ViewOrder = 1
            personAddress.CreatedByUser = 2
            personAddress.CreatedOnDate = Today()
            personAddress.ModifiedByUser = 2
            personAddress.ModifiedOnDate = Today()

            Dim existingAddress = personAddressCtrl.GetPersonMainAddress(codigo)

            If existingAddress IsNot Nothing Then
                personAddress.PersonAddressId = existingAddress.PersonAddressId
                personAddressCtrl.UpdatePersonAddress(personAddress)
            Else
                personAddressCtrl.AddPersonAddress(personAddress)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

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

    Public Class RiwUserInfo
        Property PortalId As Integer
        Property UserId As Integer
        Property Password As String
        Property Email As String
        Property FirstName As String
        Property LastName As String
        Property Country As String
        Property Telephone As String
        Property Cell As String
        Property Fax As String
        Property PostalCode As String
        Property Biography As String
        Property Street As String
        Property Unit As String
        Property Complement As String
        Property District As String
        Property City As String
        Property Region As String
        Property Comments As String
        Property Groups As String
    End Class

    Public Class HistoryComment
        Property dto As Components.Models.PersonHistoryComment
        Property connId As String
        Property messageIndex As Integer
    End Class

    Private ReadOnly imageTypeExtensionMap As New Dictionary(Of String, String)() From {
        {"image/jpeg", "jpg"},
        {"image/png", "png"}
    }

    Public Class UserPhoto
        Property PortalId As Integer
        Property UserId As Integer
        Property FileName As String
    End Class

    Public ReadOnly MediaTypeExtensionMap As New Dictionary(Of String, String)() From {
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

    Public Class PersonInfo
        Property UserId As Integer
        Property UserName As String
        Property DisplayName As String
        Property FirstName As String
        Property LastName As String
        Property IsDeleted As Boolean
        Property ModifiedOnDate As DateTime
    End Class

    Public Class SubscribedRoles
        Public Property RoleId As Integer
        Public Property RoleName As String
        Public Property RoleGroupId As Integer
        Public Property RoleGroup As String
        Public Property Subscribed As Boolean
        Public Property Description As String
    End Class

    Public Class Counts
        Property Added As Integer
        Property Updated As Integer
    End Class

End Class
