
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Security.Membership
Imports DotNetNuke.Security.Roles
Imports System.IO
Imports System.Globalization

Namespace RI.Modules.RIStore_Services
    Public Class ClientsController
        Inherits DnnApiController

        Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(AccountsController))

        ''' <summary>
        ''' Checks for an existing email from the client's table
        ''' </summary>
        ''' <param name="email"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpGet> _
        Function ValidateClient(email As String) As HttpResponseMessage
            Try
                Dim _data = Utilities.ValidateClient(email)
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
        ''' <param name="sRep">Sales Person ID</param>
        ''' <param name="isDeleted">Has been set as deleted</param>
        ''' <param name="sTerm">Clients Search Term</param>
        ''' <param name="sId">Status ID</param>
        ''' <param name="sDate">Start ModifiedOnDate</param>
        ''' <param name="eDate">End Modified Date Range</param>
        ''' <param name="page">Page Number</param>
        ''' <param name="pageSize">Page Size</param>
        ''' <param name="orderBy">Sorting Order</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpGet> _
        Function GetClientsAll(
            ByVal page As Integer,
            ByVal pageSize As Integer,
            ByVal orderBy As String,
            Optional ByVal portalId As Integer = Nothing,
            Optional ByVal sRep As Integer = Nothing,
            Optional ByVal isDeleted As String = "",
            Optional ByVal sTerm As String = Nothing,
            Optional ByVal sId As Integer = Nothing,
            Optional ByVal sDate As String = Nothing,
            Optional ByVal eDate As String = Nothing) As HttpResponseMessage
            Try
                Dim searchStr As String = HttpContext.Current.Request.Params("filter[filters][0][value]")
                If sTerm Is Nothing Then
                    If searchStr Is Nothing Then
                        searchStr = ""
                    End If
                Else
                    searchStr = sTerm
                End If

                If sRep = Nothing Then
                    sRep = Null.NullInteger
                End If

                If sId = Nothing Then
                    sId = CStr(Null.NullInteger)
                End If

                If sDate = Nothing Then
                    sDate = CStr(Null.NullDate)
                End If

                If eDate = Nothing Then
                    eDate = CStr(Null.NullDate)
                End If

                Dim clientsDataCtrl As New ClientsRepository
                Dim clientsData = clientsDataCtrl.GetClients(PortalController.GetCurrentPortalSettings().PortalId, sRep, isDeleted, searchStr, sId, sDate, eDate, page, pageSize, orderBy)

                Dim total = Nothing
                For Each item In clientsData
                    total = item.TotalRows
                    Exit For
                Next

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.data = clientsData, .total = total})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets client info
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClient(clientId As Integer, portalId As Integer) As HttpResponseMessage
            Try
                Dim client As Models.Client
                Dim clientCtrl As New ClientsRepository

                client = clientCtrl.GetClient(clientId, portalId)

                Return Request.CreateResponse(HttpStatusCode.OK, client)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Adds client
        ''' </summary>
        ''' <param name="pType">Person Type</param>
        ''' <param name="cName">Company Name</param>
        ''' <param name="fName">First Name</param>
        ''' <param name="lName">Last Name</param>
        ''' <param name="phone">Telephone</param>
        ''' <param name="cell">Cell</param>
        ''' <param name="fax">Fax</param>
        ''' <param name="zero800s">Zero 800</param>
        ''' <param name="email">Email</param>
        ''' <param name="website">Website</param>
        ''' <param name="dateFound"></param>
        ''' <param name="dateRegistered">Date Registered</param>
        ''' <param name="ein">ein</param>
        ''' <param name="cpf">CPF</param>
        ''' <param name="ident">Identity</param>
        ''' <param name="st">State Tax</param>
        ''' <param name="im">Local Tax</param>
        ''' <param name="comments">Comments</param>
        ''' <param name="salesRep">Sales Rep</param>
        ''' <param name="industries">Industry IDs</param>
        ''' <param name="createdById">Created By User ID</param>
        ''' <param name="createdOnDate">Created Date</param>
        ''' <param name="city">City</param>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="country">Country</param>
        ''' <param name="complement">Complement</param>
        ''' <param name="createLogin">Create Login</param>
        ''' <param name="district">District</param>
        ''' <param name="postalCode">Postal Code</param>
        ''' <param name="region">Region</param>
        ''' <param name="rTypes">Register Types</param>
        ''' <param name="street">Street</param>
        ''' <param name="unit">Unit</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function UpdateClient(
        ByVal portalId As Integer,
        ByVal pType As Boolean,
        ByVal fName As String,
        ByVal lName As String,
        ByVal rTypes As String,
        ByVal createLogin As Boolean,
        ByVal createdById As Integer,
        ByVal createdOnDate As Date,
        Optional ByVal clientId As Integer = -1,
        Optional ByVal cName As String = Nothing,
        Optional ByVal phone As String = Nothing,
        Optional ByVal cell As String = Nothing,
        Optional ByVal fax As String = Nothing,
        Optional ByVal zero800s As String = Nothing,
        Optional ByVal email As String = Nothing,
        Optional ByVal website As String = Nothing,
        Optional ByVal dateFound As String = Nothing,
        Optional ByVal dateRegistered As String = Nothing,
        Optional ByVal ein As String = Nothing,
        Optional ByVal cpf As String = Nothing,
        Optional ByVal ident As String = Nothing,
        Optional ByVal st As String = Nothing,
        Optional ByVal im As String = Nothing,
        Optional ByVal comments As String = Nothing,
        Optional ByVal salesRep As Integer = Nothing,
        Optional ByVal industries As String = Nothing,
        Optional ByVal postalCode As String = Nothing,
        Optional ByVal street As String = Nothing,
        Optional ByVal unit As String = Nothing,
        Optional ByVal complement As String = Nothing,
        Optional ByVal district As String = Nothing,
        Optional ByVal city As String = Nothing,
        Optional ByVal region As String = Nothing,
        Optional ByVal country As String = Nothing) As HttpResponseMessage

            Dim client As New Models.Client
            Dim clientCtrl As New ClientsRepository

            Try

                If clientId > 0 Then
                    client = clientCtrl.GetClient(clientId, PortalController.GetCurrentPortalSettings().PortalId)
                Else
                    If email <> "" Then
                        If Utilities.ValidateClient(email) > 0 Then
                            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "failed", .Msg = "Não foi possível adicionar o cliente."})
                        End If
                    End If
                End If

                If dateFound = Nothing Then
                    dateFound = ""
                    client.DateFoun = Utilities.ToNullableDateTime(dateFound)
                End If

                If dateRegistered = Nothing Then
                    dateRegistered = ""
                    client.DateRegistered = Utilities.ToNullableDateTime(dateRegistered)
                End If

                client.PortalId = portalId
                client.PersonType = pType
                client.Cnpj = ein
                client.Cpf = cpf
                client.SalesRep = salesRep
                client.CreatedByUser = createdById
                client.CreatedOnDate = createdOnDate
                client.ModifiedByUser = createdById
                client.ModifiedOnDate = createdOnDate
                client.DisplayName = ""

                If cName IsNot Nothing Then
                    client.CompanyName = cName.Trim()
                    client.DisplayName = cName.Trim()
                    If fName IsNot Nothing Then
                        client.DisplayName += String.Format("{0}{1}", Space(1), fName.Trim())
                        If lName IsNot Nothing Then
                            client.DisplayName += String.Format("{0}{1}", Space(1), lName.Trim())
                        End If
                    End If
                Else
                    If fName IsNot Nothing Then
                        client.FirstName = fName.Trim()
                        client.DisplayName += fName.Trim()
                        If lName IsNot Nothing Then
                            client.LastName = lName.Trim()
                            client.DisplayName += String.Format("{0}{1}", Space(1), lName.Trim())
                        End If
                    End If
                End If

                If phone IsNot Nothing Then
                    client.Telephone = phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
                End If
                If phone IsNot Nothing Then
                    client.Telephone = phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
                End If
                If cell IsNot Nothing Then
                    client.Cell = cell.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
                End If
                If fax IsNot Nothing Then
                    client.Fax = fax.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
                End If
                If zero800s IsNot Nothing Then
                    client.Zero800s = zero800s.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
                End If
                If email IsNot Nothing Then
                    client.Email = email.Trim()
                End If
                If website IsNot Nothing Then
                    client.Website = website.Trim()
                End If
                If rTypes IsNot Nothing Then
                    client.RegisterTypes = rTypes
                End If
                If ident IsNot Nothing Then
                    client.Ident = ident.Trim()
                End If
                If st IsNot Nothing Then
                    client.InsEst = st.Trim()
                End If
                If im IsNot Nothing Then
                    client.InsMun = im.Trim()
                End If
                If comments IsNot Nothing Then
                    client.Comments = comments.Trim()
                End If

                If clientId > 0 Then
                    clientCtrl.UpdateClient(client)
                Else
                    clientCtrl.AddClient(client)
                End If

                If client.ClientId > 0 Then

                    If industries IsNot Nothing Then
                        industries = industries.Replace("[", "").Replace("]", "")
                        Dim indusClient As New Models.ClientIndustry
                        Dim indusCtrl As New ClientIndustryRepository
                        indusCtrl.RemoveClientIndustries(client.ClientId)
                        For Each item In industries.Split(","c)
                            indusClient.ClientId = client.ClientId
                            indusClient.IndustryId = item
                            indusCtrl.AddClientIndustry(indusClient)
                        Next
                    End If

                    Dim newUId = Null.NullInteger
                    If createLogin Then
                        Dim oUserInfo As New Users.UserInfo() With {.PortalID = PortalController.GetCurrentPortalSettings().PortalId,
                                     .Username = email.Trim().ToLower(),
                                     .FirstName = fName.Trim().ToLower(),
                                     .LastName = lName.Trim().ToLower(),
                                     .Email = email.Trim().ToLower(),
                                     .AffiliateID = Null.NullInteger,
                                     .LastIPAddress = DotNetNuke.Services.Authentication.AuthenticationLoginBase.GetIPAddress()}

                        oUserInfo.Membership.Approved = True
                        oUserInfo.Membership.Password = Utilities.GeneratePassword(7)
                        oUserInfo.Membership.UpdatePassword = True
                        oUserInfo.DisplayName = client.DisplayName

                        Dim objUserCreateStatus = Users.UserController.CreateUser(oUserInfo)

                        If objUserCreateStatus = UserCreateStatus.Success Then

                            'set roles here
                            Dim objRoleCtrl As New RoleController
                            Dim objRoleInfo = objRoleCtrl.GetRoleByName(PortalController.GetCurrentPortalSettings().PortalId, "Clientes")
                            objRoleCtrl.AddUserRole(PortalController.GetCurrentPortalSettings().PortalId, oUserInfo.UserID, objRoleInfo.RoleID, Null.NullDate)
                            objRoleCtrl.AddUserRole(PortalController.GetCurrentPortalSettings().PortalId, oUserInfo.UserID, PortalSettings.RegisteredRoleId, Null.NullDate)

                            oUserInfo.Profile.SetProfileProperty("Country", country)
                            oUserInfo.Profile.SetProfileProperty("Photo", "0")
                            oUserInfo.Profile.SetProfileProperty("PreferredTimeZone", "E. South America Standard Time")
                            oUserInfo.Profile.SetProfileProperty("PreferredLocale", "pt-BR")

                            DotNetNuke.Entities.Profile.ProfileController.UpdateUserProfile(oUserInfo)

                            client.UserId = oUserInfo.UserID

                            clientCtrl.UpdateClient(client)

                            DotNetNuke.Common.Utilities.DataCache.ClearCache()

                            newUId = oUserInfo.UserID
                        Else
                            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Não possível criar o login.", .Msg = "Email já cadastrado."})
                        End If
                    End If
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .ClientId = client.ClientId, .UserId = client.UserId})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Deletes a client by client id
        ''' </summary>
        ''' <param name="cId">Client ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="cUId">Client User ID</param>
        ''' <param name="uId">Created By User</param>
        ''' <param name="cd">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Public Function DeleteClient(ByVal cId As Integer, ByVal portalId As Integer, ByVal uId As Integer, ByVal cd As Date, Optional ByVal cUId As Integer = -1) As HttpResponseMessage
            Dim client As New Models.Client
            Dim clientCltr As New ClientsRepository

            Try
                If cId > 0 Then
                    Using context As IDataContext = DataContext.Instance()
                        Dim repository = context.GetRepository(Of Models.Client)()
                        client = repository.GetById(cId, PortalController.GetCurrentPortalSettings().PortalId)

                        client.IsDeleted = True
                        client.ModifiedByUser = uId
                        client.ModifiedOnDate = cd

                        clientCltr.UpdateClient(client)

                        If cUId > 0 Then
                            Users.UserController.DeleteUser(Users.UserController.GetUserById(portalId, cUId), False, False)
                        End If
                    End Using

                    Return Request.CreateResponse(HttpStatusCode.OK, "success")
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Removes a client by client id
        ''' </summary>
        ''' <param name="cId">Client ID</param>
        ''' <param name="cUId">Client User ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpDelete> _
        Function RemoveClient(ByVal cId As Integer, Optional ByVal cUId As Integer = -1) As HttpResponseMessage
            Try
                Dim clientCtrl As New ClientsRepository
                Dim clientAddressCtrl As New ClientAddressesRepository
                Dim clientBankRefCtrl As New ClientBankRefsRepository
                Dim clientCommRefCtrl As New ClientCommRefsRepository
                Dim clientContactCtrl As New ClientContactsRepository
                Dim clientDocCtrl As New ClientDocsRepository
                Dim clientHistoryCtrl As New ClientHistoriesRepository
                Dim clientIncomeSourceCtrl As New ClientIncomeSourcesRepository
                Dim clientIndustryCtrl As New ClientIndustryRepository
                Dim clientPartnerCtrl As New ClientPartnersRepository
                Dim clientPartnerBankRefCtrl As New ClientPartnersBankRefsRepository
                Dim clientPersonalRefCtrl As New ClientPersonalRefsRepository
                Dim clientEstimateCtrl As New EstimatesRepository

                clientAddressCtrl.RemoveClientAddresses(cId)
                clientBankRefCtrl.RemoveClientBankRefs(cId)
                clientCommRefCtrl.RemoveClientCommRefs(cId)
                clientContactCtrl.RemoveClientContacts(cId)
                clientDocCtrl.RemoveClientDocs(cId)
                clientHistoryCtrl.RemoveClientHistories(cId)
                clientIncomeSourceCtrl.RemoveClientIncomeSources(cId)
                clientIndustryCtrl.RemoveClientIndustries(cId)
                clientPartnerCtrl.RemoveClientPartners(cId)
                clientPartnerBankRefCtrl.RemoveClientPartnerBankRefs(cId)
                clientPersonalRefCtrl.RemoveClientPersonalRefs(cId)
                clientEstimateCtrl.RemoveEstimates(cId, PortalController.GetCurrentPortalSettings().PortalId)
                clientCtrl.RemoveClient(cId, PortalController.GetCurrentPortalSettings().PortalId, cUId)

                If cUId > 0 Then
                    Users.UserController.RemoveUser(Users.UserController.GetUserById(PortalController.GetCurrentPortalSettings().PortalId, cUId))
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Get client inndustris by client id
        ''' </summary>
        ''' <param name="cId">Client ID</param>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientIndustries(ByVal cId As Integer) As HttpResponseMessage
            Try
                Dim clientIndustriesCtrl As New ClientIndustryRepository
                Dim clientIndustriesData = clientIndustriesCtrl.GetClientIndustries(cId)

                Return Request.CreateResponse(HttpStatusCode.OK, clientIndustriesData)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets client addresses by client id
        ''' </summary>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientAddresses(ByVal cId As Integer) As HttpResponseMessage
            Try
                Dim clientAddressesCtrl As New ClientAddressesRepository
                Dim clientAddressesData = clientAddressesCtrl.GetClientAddresses(cId)

                Return Request.CreateResponse(HttpStatusCode.OK, clientAddressesData)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets client address
        ''' </summary>
        ''' <param name="cAId">Client Address ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientAddress(ByVal cAId As Integer, cId As Integer) As HttpResponseMessage
            Try
                Dim clientAddressCtrl As New ClientAddressesRepository
                Dim clientAddressData = clientAddressCtrl.GetClientAddress(cAId, cId)

                Return Request.CreateResponse(HttpStatusCode.OK, clientAddressData)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Adds or updates client address
        ''' </summary>
        ''' <param name="cId">Client ID</param>
        ''' <param name="aName">Address Name</param>
        ''' <param name="street">Street</param>
        ''' <param name="unit">Unit</param>
        ''' <param name="complem">Complement</param>
        ''' <param name="dist">District</param>
        ''' <param name="city">City</param>
        ''' <param name="region">Region</param>
        ''' <param name="zip">Postal Code</param>
        ''' <param name="country">Country</param>
        ''' <param name="phone">Telephone</param>
        ''' <param name="cell">Cellular</param>
        ''' <param name="fax">Fax</param>
        ''' <param name="vieworder">View Order</param>
        ''' <param name="uId">Created By User</param>
        ''' <param name="cd">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function UpdateClientAddress(ByVal cId As Integer,
        ByVal aName As String,
        ByVal street As String,
        ByVal unit As String,
        ByVal complem As String,
        ByVal dist As String,
        ByVal city As String,
        ByVal region As String,
        ByVal zip As String,
        ByVal country As String,
        ByVal phone As String,
        ByVal cell As String,
        ByVal fax As String,
        ByVal viewOrder As Integer,
        ByVal uId As Integer,
        ByVal cd As Date,
        Optional ByVal cAId As Integer = -1) As HttpResponseMessage
            Try
                Dim clientAddress As New Models.ClientAddress
                Dim clientAddressCtrl As New ClientAddressesRepository

                If cAId > 0 Then
                    clientAddress = clientAddressCtrl.GetClientAddress(cAId, cId)
                End If

                clientAddress.ClientId = cId
                clientAddress.AddressName = aName.Trim()
                clientAddress.Street = street.Trim()
                If unit IsNot Nothing Then
                    clientAddress.Unit = unit.Trim()
                End If
                If complem IsNot Nothing Then
                    clientAddress.Complement = complem.Trim()
                End If
                If dist IsNot Nothing Then
                    clientAddress.District = dist.Trim()
                End If
                If city IsNot Nothing Then
                    clientAddress.City = city.Trim()
                End If
                clientAddress.Region = region
                If zip IsNot Nothing Then
                    clientAddress.PostalCode = zip
                End If
                If phone IsNot Nothing Then
                    clientAddress.Telephone = phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
                End If
                If cell IsNot Nothing Then
                    clientAddress.Cell = cell.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
                End If
                If fax IsNot Nothing Then
                    clientAddress.Fax = fax.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
                End If
                clientAddress.Country = country
                clientAddress.ViewOrder = viewOrder
                clientAddress.CreatedByUser = uId
                clientAddress.CreatedOnDate = cd
                clientAddress.ModifiedByUser = uId
                clientAddress.ModifiedOnDate = cd

                If cAId > 0 Then
                    clientAddressCtrl.UpdateClientAddress(clientAddress)
                Else
                    clientAddressCtrl.AddClientAddress(clientAddress)
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .cAId = clientAddress.ClientAddressId})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Removes a client address
        ''' </summary>
        ''' <param name="cAId">Client Address ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpDelete> _
        Function RemoveClientAddress(ByVal cAId As Integer, cId As Integer) As HttpResponseMessage
            Try
                Dim clientAddressCtrl As New ClientAddressesRepository

                clientAddressCtrl.RemoveClientAddress(cAId, cId)

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
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientContacts(ByVal cId As Integer) As HttpResponseMessage
            Try
                Dim clientContactsCtrl As New ClientContactsRepository
                Dim clientContactsData = clientContactsCtrl.GetClientContacts(cId)

                Return Request.CreateResponse(HttpStatusCode.OK, clientContactsData)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets client contact
        ''' </summary>
        ''' <param name="cCId">Client Contact ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientContact(ByVal cCId As Integer, cId As Integer) As HttpResponseMessage
            Try
                Dim clientContactCtrl As New ClientContactsRepository
                Dim clientContactData = clientContactCtrl.GetClientContact(cCId, cId)

                Return Request.CreateResponse(HttpStatusCode.OK, clientContactData)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Add or updates client contact
        ''' </summary>
        ''' <param name="cCId">Client ID</param>
        ''' <param name="cName">Contact Name</param>
        ''' <param name="bDay">Birthday Day and Month</param>
        ''' <param name="dept">Department</param>
        ''' <param name="email1">Email 1</param>
        ''' <param name="email2">Email 2</param>
        ''' <param name="phone1">Telephone 1</param>
        ''' <param name="phone2">Telephone 2</param>
        ''' <param name="phoneExt1">Telephone Extension 1</param>
        ''' <param name="phoneExt2">Telephone Extension 2</param>
        ''' <param name="comments">Comments</param>
        ''' <param name="cAId">Contact Address ID</param>
        ''' <param name="uId">Created By User</param>
        ''' <param name="cd">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function UpdateClientContact(ByVal cId As Integer,
        ByVal cName As String,
        ByVal bDay As String,
        ByVal dept As String,
        ByVal email1 As String,
        ByVal email2 As String,
        ByVal phone1 As String,
        ByVal phone2 As String,
        ByVal phoneExt1 As String,
        ByVal phoneExt2 As String,
        ByVal comments As String,
        ByVal uId As Integer,
        ByVal cd As Date,
        Optional ByVal cAId As Integer = -1,
        Optional ByVal cCId As Integer = -1) As HttpResponseMessage
            Try
                Dim clientContact As New Models.ClientContact
                Dim clientContactCtrl As New ClientContactsRepository

                If cCId > 0 Then
                    clientContact = clientContactCtrl.GetClientContact(cCId, cId)
                End If

                clientContact.ContactName = cName.Trim()
                If bDay IsNot Nothing Then
                    clientContact.DateBirth = bDay.Trim()
                End If
                If dept IsNot Nothing Then
                    clientContact.Dept = dept.Trim()
                End If
                If email1 IsNot Nothing Then
                    clientContact.ContactEmail1 = email1.Trim()
                End If
                If email2 IsNot Nothing Then
                    clientContact.ContactEmail2 = email2.Trim()
                End If
                If phone1 IsNot Nothing Then
                    clientContact.ContactPhone1 = phone1.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
                End If
                If phone2 IsNot Nothing Then
                    clientContact.ContactPhone2 = phone2.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
                End If
                If phoneExt1 IsNot Nothing Then
                    clientContact.PhoneExt1 = phoneExt1
                End If
                If phoneExt2 IsNot Nothing Then
                    clientContact.PhoneExt2 = phoneExt2
                End If
                If comments IsNot Nothing Then
                    clientContact.Comments = comments
                End If
                clientContact.ClientAddressId = cAId
                clientContact.CreatedByUser = uId
                clientContact.CreatedOnDate = cd
                clientContact.ModifiedByUser = uId
                clientContact.ModifiedOnDate = cd

                If cCId > 0 Then
                    clientContactCtrl.UpdateClientContact(clientContact)
                Else
                    clientContactCtrl.AddClientContact(clientContact)
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .cCId = clientContact.ClientContactId})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Removes a client contact
        ''' </summary>
        ''' <param name="cCId">Client Contact ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpDelete> _
        Function RemoveClientContact(ByVal cCId As Integer, cId As Integer) As HttpResponseMessage
            Try
                Dim clientContactCtrl As New ClientContactsRepository

                clientContactCtrl.RemoveClientContact(cCId, cId)

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
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientDocs(ByVal cId As Integer) As HttpResponseMessage
            Try
                Dim clientDocsCtrl As New ClientDocsRepository
                Dim clientDocsData = clientDocsCtrl.GetClientDocs(cId)

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
        ''' <param name="cDId">Client Doc ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientDoc(ByVal cDId As Integer, cId As Integer) As HttpResponseMessage
            Try
                Dim clientDocCtrl As New ClientDocsRepository
                Dim clientDocData = clientDocCtrl.GetClientDoc(cDId, cId)

                Return Request.CreateResponse(HttpStatusCode.OK, clientDocData)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function
        ''' <summary>
        ''' Saves a file by client id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <param name="docName">Document Name</param>
        ''' <param name="docDesc">Document Desc</param>
        ''' <param name="docUrl">External Document URL</param>
        ''' <param name="maxWidth">Image's width</param>
        ''' <param name="maxHeight">Image's height</param>
        ''' <param name="folderPath">Save path</param>
        ''' <param name="uId">Created By User ID</param>
        ''' <param name="cd">Created Date</param>
        ''' <param name="files">Uploaded Files</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function SaveClientDoc(ByVal portalId As Integer, ByVal cId As Integer, ByVal docName As String, ByVal docDesc As String, ByVal docUrl As String, ByVal maxWidth As Integer, ByVal maxHeight As Integer, ByVal folderPath As String, ByVal uId As Integer, ByVal cd As Date, ByVal files As IEnumerable(Of HttpPostedFileBase)) As HttpResponseMessage
            Try
                Dim portalCtrl = New Portals.PortalController()

                For Each _file In files

                    Dim fileName = Path.GetFileName(_file.FileName)
                    Dim destinationPath = portalCtrl.GetPortal(portalId).HomeDirectoryMapPath & folderPath

                    Dim FileDir As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(destinationPath)
                    If FileDir.Exists = False Then
                        FileDir.Create()
                    End If

                    If ImageResizer.Configuration.Config.Current.Pipeline.IsAcceptedImageType(fileName) Then

                        'The resizing settings can specify any of 30 commands.. See http://imageresizing.net for details.
                        'Destination paths can have variables like <guid> and <ext>
                        Dim i = New ImageResizer.ImageJob
                        i.CreateParentDirectory = True
                        i.Source = _file
                        i.Dest = String.Format("~/{0}/{1}/<filename>.<ext>", portalCtrl.GetPortal(portalId).HomeDirectory, folderPath)

                        Dim img = Drawing.Image.FromStream(_file.InputStream)
                        If img.Height > img.Width Then
                            i.Settings = New ImageResizer.ResizeSettings(String.Format("width={0}&height={1}&crop=auto", CStr(IIf(maxWidth > 0, CStr(maxWidth), "600")), CStr(IIf(maxHeight > 0, CStr(maxHeight), "800"))))
                        Else
                            i.Settings = New ImageResizer.ResizeSettings(String.Format("width={0}&height={1}&crop=auto", CStr(IIf(maxWidth > 0, CStr(maxWidth), "800")), CStr(IIf(maxHeight > 0, CStr(maxHeight), "600"))))
                        End If

                        _file.InputStream.Seek(0, SeekOrigin.Begin)

                        i.Build()
                    Else
                        _file.SaveAs(String.Format("{0}\{1}", destinationPath, fileName))
                    End If

                    DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(portalId)

                    Dim objFolderInfo = DotNetNuke.Services.FileSystem.FolderManager.Instance().GetFolder(portalId, folderPath)
                    Dim objFileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance().GetFile(objFolderInfo, _file.FileName)

                    Dim clientDoc As New Models.ClientDoc
                    Dim clientDocCtrl As New ClientDocsRepository

                    clientDoc.ClientId = cId
                    clientDoc.DocName = docName
                    clientDoc.DocDesc = docDesc
                    clientDoc.DocUrl = docUrl
                    clientDoc.FileId = objFileInfo.FileId
                    clientDoc.CreatedByUser = uId
                    clientDoc.CreatedOnDate = cd

                    clientDocCtrl.AddClientDoc(clientDoc)

                Next

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Removes a client Doc
        ''' </summary>
        ''' <param name="cDId">Client Doc ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpDelete> _
        Function RemoveClientDoc(ByVal cDId As Integer, cId As Integer) As HttpResponseMessage
            Try
                Dim clientDocCtrl As New ClientDocsRepository

                ' Todo: add function to remove user's file
                clientDocCtrl.RemoveClientDoc(cDId, cId)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Updates client finance by client id
        ''' </summary>
        ''' <param name="cId">Client ID</param>
        ''' <param name="cAId">Client Address ID</param>
        ''' <param name="mIncome">Monthly Income</param>
        ''' <param name="uId">Created By User ID</param>
        ''' <param name="md">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPut> _
        Function UpdateClientFinance(ByVal cId As Integer, ByVal cAId As Integer, ByVal mIncome As String, ByVal uId As Integer, ByVal md As Date) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim client As New Models.Client
                Dim clientCtrl As New ClientsRepository

                client = clientCtrl.GetClient(cId, PortalController.GetCurrentPortalSettings().PortalId)

                client.ClientAddressId = cAId
                client.MonthlyIncome = Decimal.Parse(mIncome.Replace(".", ","), numInfo)
                client.ModifiedByUser = uId
                client.ModifiedOnDate = md

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets client income sources by client id
        ''' </summary>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientIncomeSources(ByVal cId As Integer) As HttpResponseMessage
            Try
                Dim clientIncomeSources As New List(Of Models.ClientIncomeSource)
                Dim clientIncomeSourcesCtrl As New ClientIncomeSourcesRepository

                clientIncomeSources = clientIncomeSourcesCtrl.GetClientIncomeSources(cId)

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
        ''' <param name="cISId">Client Income Source ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientIncomeSource(ByVal cISId As Integer, cId As Integer) As HttpResponseMessage
            Try
                Dim clientIncomeSource As New Models.ClientIncomeSource
                Dim clientIncomeSourceCtrl As New ClientIncomeSourcesRepository

                clientIncomeSource = clientIncomeSourceCtrl.GetClientIncomeSource(cISId, cId)

                Return Request.CreateResponse(HttpStatusCode.OK, clientIncomeSource)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Updates client income source by income source id
        ''' </summary>
        ''' <param name="cISId">Income Source ID</param>
        ''' <param name="iSName">Income Source Title</param>
        ''' <param name="iSEIN">Employer Identification Number</param>
        ''' <param name="iSST">State Tax ID</param>
        ''' <param name="iSCT">City Tax ID</param>
        ''' <param name="iSPhone">Telephone</param>
        ''' <param name="iSFax">Fax</param>
        ''' <param name="iSIncome">Monthly Income</param>
        ''' <param name="isZip">Postal Code</param>
        ''' <param name="isStreet">Street Address</param>
        ''' <param name="iSUnit">Unit</param>
        ''' <param name="iSComplem">Complement</param>
        ''' <param name="iSDist">District</param>
        ''' <param name="iSCity">City</param>
        ''' <param name="iSRegion">Region</param>
        ''' <param name="iSCountry">Country</param>
        ''' <param name="isProof">Income Proovable</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function UpdateClientIncomeSource(ByVal cId As Integer, ByVal iSName As String, ByVal iSEIN As String, ByVal iSST As String, ByVal iSCT As String, ByVal iSPhone As String, ByVal iSFax As String, ByVal iSIncome As String, ByVal isZip As String, ByVal isStreet As String, ByVal iSUnit As String, ByVal iSComplem As String, ByVal iSDist As String, ByVal iSCity As String, ByVal iSRegion As String, iSCountry As String, ByVal isProof As Boolean, Optional ByVal cISId As Integer = -1) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim clientIncomeSource As New Models.ClientIncomeSource
                Dim clientIncomeCourceCtrl As New ClientIncomeSourcesRepository

                If cISId > 0 Then
                    clientIncomeSource = clientIncomeCourceCtrl.GetClientIncomeSource(cISId, cId)
                End If

                clientIncomeSource.ISAddress = isStreet.Trim()
                clientIncomeSource.ISAddressUnit = iSUnit.Trim()
                clientIncomeSource.ISCity = iSCity.Trim()
                clientIncomeSource.ISComplement = iSComplem.Trim()
                clientIncomeSource.ISCT = iSCT.Trim()
                clientIncomeSource.ISDistrict = iSDist.Trim()
                clientIncomeSource.ISEIN = iSEIN
                clientIncomeSource.ISFax = iSFax
                clientIncomeSource.ISIncome = Decimal.Parse(iSIncome.Replace(".", ","), numInfo)
                clientIncomeSource.ISName = iSName.Trim()
                clientIncomeSource.ISPhone = iSPhone
                clientIncomeSource.ISPostalCode = isZip
                clientIncomeSource.ISProof = isProof
                clientIncomeSource.ISRegion = iSRegion
                clientIncomeSource.ISST = iSST

                If cISId > 0 Then
                    clientIncomeCourceCtrl.UpdateClientIncomeSource(clientIncomeSource)
                Else
                    clientIncomeCourceCtrl.AddClientIncomeSource(clientIncomeSource)
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, clientIncomeSource)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Removes a client income source
        ''' </summary>
        ''' <param name="cISId">Client Income Source ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpDelete> _
        Function RemoveClientIncomeSource(ByVal cISId As Integer, cId As Integer) As HttpResponseMessage
            Try
                Dim clientIncomeCourceCtrl As New ClientIncomeSourcesRepository

                clientIncomeCourceCtrl.RemoveClientIncomeSource(cISId, cId)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets client personal references by client id
        ''' </summary>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientPersonalRefs(ByVal cId As Integer) As HttpResponseMessage
            Try
                Dim clientPersonalRefs As New List(Of Models.ClientPersonalRef)
                Dim clientPersonalRefsCtrl As New ClientPersonalRefsRepository

                clientPersonalRefs = clientPersonalRefsCtrl.GetClientPersonalRefs(cId)

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
        ''' <param name="cPRId">Client Personal Ref ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientPersonalRef(ByVal cPRId As Integer, cId As Integer) As HttpResponseMessage
            Try
                Dim clientPersonalRef As New Models.ClientPersonalRef
                Dim clientPersonalRefCtrl As New ClientPersonalRefsRepository

                clientPersonalRef = clientPersonalRefCtrl.GetClientPersonalRef(cPRId, cId)

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
        ''' <param name="cId">Client ID</param>
        ''' <param name="cPRId">Personal Reference ID</param>
        ''' <param name="pRName">Name</param>
        ''' <param name="pRPhone">Phone Number</param>
        ''' <param name="pREmail">Email</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function UpdateClientPersonalRef(ByVal cId As Integer, ByVal pRName As String, ByVal pRPhone As String, ByVal pREmail As String, Optional ByVal cPRId As Integer = -1) As HttpResponseMessage
            Try
                Dim clientPersonalRef As New Models.ClientPersonalRef
                Dim clientPersonalRefCtrl As New ClientPersonalRefsRepository

                If cPRId > 0 Then
                    clientPersonalRef = clientPersonalRefCtrl.GetClientPersonalRef(cPRId, cId)
                End If

                clientPersonalRef.PREmail = pREmail.Trim()
                clientPersonalRef.PRName = pRName.Trim()
                clientPersonalRef.PRPhone = pRPhone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")

                If cPRId > 0 Then
                    clientPersonalRefCtrl.UpdateClientPersonalRef(clientPersonalRef)
                Else
                    clientPersonalRefCtrl.AddClientPersonalRef(clientPersonalRef)
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
        ''' <param name="cPRId">Client Personal Reference Source ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpDelete> _
        Function RemovePersonalReference(ByVal cPRId As Integer, cId As Integer) As HttpResponseMessage
            Try
                Dim clientPersonalRefCtrl As New ClientPersonalRefsRepository

                clientPersonalRefCtrl.RemoveClientPersonalRef(cPRId, cId)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets client bank references by client id
        ''' </summary>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientBankRefs(ByVal cId As Integer) As HttpResponseMessage
            Try
                Dim clientBankRefs As New List(Of Models.ClientBankRef)
                Dim clientBankRefsCtrl As New ClientBankRefsRepository

                clientBankRefs = clientBankRefsCtrl.GetClientBankRefs(cId)

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
        ''' <param name="cBRId">Client Bank Ref ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientBankRef(ByVal cBRId As Integer, cId As Integer) As HttpResponseMessage
            Try
                Dim clientBankRef As New Models.ClientBankRef
                Dim clientBankRefCtrl As New ClientBankRefsRepository

                clientBankRef = clientBankRefCtrl.GetClientBankRef(cBRId, cId)

                Return Request.CreateResponse(HttpStatusCode.OK, clientBankRef)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Updates client bank reference by bank reference id
        ''' </summary>
        ''' <param name="cBRId">Bank Reference ID</param>
        ''' <param name="bRef">Bank Reference</param>
        ''' <param name="bRefAgency">Bank Agency</param>
        ''' <param name="bRefAcc">Bank Account Number</param>
        ''' <param name="bRefClientSince">Client Since</param>
        ''' <param name="bRefContact">Bank Contact</param>
        ''' <param name="bRefPhone">Bank Contact Phone</param>
        ''' <param name="bRefAccType">Bank Account Type</param>
        ''' <param name="bRefCredit">Account Credit Limit</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function UpdateClientBankRef(ByVal cId As Integer, ByVal bRef As String, ByVal bRefAgency As String, ByVal bRefAcc As String, ByVal bRefClientSince As String, ByVal bRefContact As String, bRefPhone As String, ByVal bRefAccType As String, ByVal bRefCredit As String, Optional ByVal cBRId As Integer = -1) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim clientBankRef As New Models.ClientBankRef
                Dim clientBankRefCtrl As New ClientBankRefsRepository

                If cBRId > 0 Then
                    clientBankRef = clientBankRefCtrl.GetClientBankRef(cBRId, cId)
                End If

                clientBankRef.BankRef = bRef.Trim()
                clientBankRef.BankRefAccount = bRefAcc.Trim()
                clientBankRef.BankRefAccountType = bRefAccType.Trim()
                clientBankRef.BankRefAgency = bRefAgency.Trim()
                clientBankRef.BankRefClientSince = bRefClientSince
                clientBankRef.BankRefContact = bRefContact.Trim()
                clientBankRef.BankRefCredit = Decimal.Parse(bRefCredit.Replace(".", ","), numInfo)
                clientBankRef.BankRefPhone = bRefPhone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")

                If cBRId > 0 Then
                    clientBankRefCtrl.UpdateClientBankRef(clientBankRef)
                Else
                    clientBankRefCtrl.AddClientBankRef(clientBankRef)
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
        ''' <param name="cBRId">Client Bank Reference Source ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpDelete> _
        Function RemoveBankReference(ByVal cBRId As Integer, cId As Integer) As HttpResponseMessage
            Try
                Dim clientBankRefCtrl As New ClientBankRefsRepository

                clientBankRefCtrl.RemoveClientBankRef(cBRId, cId)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets client commerce references by client id
        ''' </summary>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientCommRefs(ByVal cId As Integer) As HttpResponseMessage
            Try
                Dim clientCommRefs As New List(Of Models.ClientCommRef)
                Dim clientCommRefsCtrl As New ClientCommRefsRepository

                clientCommRefs = clientCommRefsCtrl.GetClientCommRefs(cId)

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
        ''' <param name="cCRId">Client Commerce Ref ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientCommRef(ByVal cCRId As Integer, cId As Integer) As HttpResponseMessage
            Try
                Dim clientCommRef As New Models.ClientCommRef
                Dim clientCommRefCtrl As New ClientCommRefsRepository

                clientCommRef = clientCommRefCtrl.GetClientCommRef(cCRId, cId)

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
        ''' <param name="cCRId">Client Commerce Reference ID</param>
        ''' <param name="cRef">Commerce Reference</param>
        ''' <param name="cRefContact">Commerce Contact</param>
        ''' <param name="cRefPhone">Commerce Contact Phone</param>
        ''' <param name="cRefLastActivity">Commerce Last Activity</param>
        ''' <param name="cRefCredit">Commerce Credit Limit</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function UpdateClientCommRef(ByVal cId As Integer, ByVal cRef As String, ByVal cRefContact As String, ByVal cRefPhone As String, ByVal cRefLastActivity As Date, ByVal cRefCredit As String, Optional ByVal cCRId As Integer = -1) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim clientCommRef As New Models.ClientCommRef
                Dim clientCommRefCtrl As New ClientCommRefsRepository

                If cCRId > 0 Then
                    clientCommRef = clientCommRefCtrl.GetClientCommRef(cCRId, cId)
                End If

                clientCommRef.CommRefBusiness = cRef.Trim()
                clientCommRef.CommRefContact = cRefContact.Trim()
                clientCommRef.CommRefPhone = cRefPhone
                clientCommRef.CommRefLastActivity = cRefLastActivity
                clientCommRef.CommRefCredit = Decimal.Parse(cRefCredit.Replace(".", ","), numInfo)

                If cCRId > 0 Then
                    clientCommRefCtrl.UpdateClientCommRef(clientCommRef)
                Else
                    clientCommRefCtrl.AddClientCommRef(clientCommRef)
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
        ''' <param name="cCRId">Client Commerce Reference Source ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpDelete> _
        Function RemoveCommRef(ByVal cCRId As Integer, cId As Integer) As HttpResponseMessage
            Try
                Dim clientCommRefCtrl As New ClientCommRefsRepository

                clientCommRefCtrl.RemoveClientCommRef(cCRId, cId)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets client partners by client id
        ''' </summary>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientPartners(ByVal cId As Integer) As HttpResponseMessage
            Try
                Dim clientPartners As New List(Of Models.ClientPartner)
                Dim clientPartnersCtrl As New ClientPartnersRepository

                clientPartners = clientPartnersCtrl.GetClientPartners(cId)

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
        ''' <param name="cPId">Client Partner ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientPartner(ByVal cPId As Integer, cId As Integer) As HttpResponseMessage
            Try
                Dim clientPartner As New Models.ClientPartner
                Dim clientPartnerCtrl As New ClientPartnersRepository

                clientPartner = clientPartnerCtrl.GetClientPartner(cPId, cId)

                Return Request.CreateResponse(HttpStatusCode.OK, clientPartner)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Adds or updates client partner by partner id
        ''' </summary>
        ''' <param name="cId">Client ID</param>
        ''' <param name="cPId">Client Partner ID</param>
        ''' <param name="pName">Name</param>
        ''' <param name="pCPF">CPF</param>
        ''' <param name="pIdent">Identity</param>
        ''' <param name="pPhone">Telephone</param>
        ''' <param name="pCell">Cellular</param>
        ''' <param name="pEmail">Email</param>
        ''' <param name="pQuota">Quota</param>
        ''' <param name="pZip">Postal Code</param>
        ''' <param name="pStreet">Address</param>
        ''' <param name="pUnit">Unit</param>
        ''' <param name="pComplemen">Complement</param>
        ''' <param name="pDist">District</param>
        ''' <param name="pCity">City</param>
        ''' <param name="pRegion">Region</param>
        ''' <param name="pCountry">Country</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function UpdateClientPartner(ByVal cId As Integer,
        ByVal pName As String,
        ByVal pCPF As String,
        ByVal pIdent As String,
        ByVal pPhone As String,
        ByVal pCell As String,
        ByVal pEmail As String,
        ByVal pQuota As String,
        ByVal pZip As String,
        ByVal pStreet As String,
        ByVal pUnit As String,
        ByVal pComplemen As String,
        ByVal pDist As String,
        ByVal pCity As String,
        ByVal pRegion As String,
        ByVal pCountry As String,
        Optional ByVal cPId As Integer = -1) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim clientPartner As New Models.ClientPartner
                Dim clientPartnerCtrl As New ClientPartnersRepository

                If cPId > 0 Then
                    clientPartner = clientPartnerCtrl.GetClientPartner(cPId, cId)
                End If

                clientPartner.PartnerName = pName.Trim()
                clientPartner.PartnerCPF = pCPF
                clientPartner.PartnerIdentity = pIdent.Trim()
                clientPartner.PartnerPhone = pPhone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
                clientPartner.PartnerCell = pCell.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
                clientPartner.PartnerEmail = pEmail.Trim()
                clientPartner.PartnerQuota = Decimal.Parse(pQuota.Replace(".", ","), numInfo)
                clientPartner.PartnerPostalCode = pZip
                clientPartner.PartnerAddress = pStreet.Trim()
                clientPartner.PartnerAddressUnit = pUnit.Trim()
                clientPartner.PartnerComplement = pComplemen.Trim()
                clientPartner.PartnerDistrict = pDist.Trim()
                clientPartner.PartnerRegion = pRegion.Trim()
                clientPartner.PartnerCity = pCity.Trim()

                If cPId > 0 Then
                    clientPartnerCtrl.UpdateClientPartner(clientPartner)
                Else
                    clientPartnerCtrl.AddClientPartner(clientPartner)
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
        ''' <param name="cPId">Client Partner ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpDelete> _
        Function RemovePartner(ByVal cPId As Integer, cId As Integer) As HttpResponseMessage
            Try
                Dim clientPartnerCtrl As New ClientPartnersRepository

                clientPartnerCtrl.RemoveClientPartner(cPId, cId)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets client partner bank references by client id
        ''' </summary>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientPartnerBankRefs(ByVal cId As Integer) As HttpResponseMessage
            Try
                Dim clientPartnerBankRefs As New List(Of Models.ClientPartnerBankRef)
                Dim clientPartnerBankRefsCtrl As New ClientPartnersBankRefsRepository

                clientPartnerBankRefs = clientPartnerBankRefsCtrl.GetClientPartnerBankRefs(cId)

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
        ''' <param name="cPBRId">Client Partner Bank Ref ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientPartnerBankRef(ByVal cPBRId As Integer, cId As Integer) As HttpResponseMessage
            Try
                Dim clientPartnerBankRef As New Models.ClientPartnerBankRef
                Dim clientPartnerBankRefCtrl As New ClientPartnersBankRefsRepository

                clientPartnerBankRef = clientPartnerBankRefCtrl.GetClientPartnerBankRef(cPBRId, cId)

                Return Request.CreateResponse(HttpStatusCode.OK, clientPartnerBankRef)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Updates client partner bank reference by partner bank reference id
        ''' </summary>
        ''' <param name="cId">Client ID</param>
        ''' <param name="cPBRId">Partner Bank Reference ID</param>
        ''' <param name="pName">Parnter Name</param>
        ''' <param name="bRef">Bank Reference</param>
        ''' <param name="bRefAgency">Bank Agency</param>
        ''' <param name="bRefAcc">Bank Account Number</param>
        ''' <param name="bRefClientSince">Client Since</param>
        ''' <param name="bRefContact">Bank Contact</param>
        ''' <param name="bRefPhone">Bank Contact Phone</param>
        ''' <param name="bRefAccType">Bank Account Type</param>
        ''' <param name="bRefCredit">Account Credit Limit</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function UpdateClientPartnerBankRef(ByVal cId As Integer, ByVal pName As String, ByVal bRef As String, ByVal bRefAgency As String, ByVal bRefAcc As String, ByVal bRefClientSince As String, ByVal bRefContact As String, bRefPhone As String, ByVal bRefAccType As String, ByVal bRefCredit As String, Optional ByVal cPBRId As Integer = -1) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim clientPartnerBankRef As New Models.ClientPartnerBankRef
                Dim clientPartnerBankRefCtrl As New ClientPartnersBankRefsRepository

                If cPBRId > 0 Then
                    clientPartnerBankRef = clientPartnerBankRefCtrl.GetClientPartnerBankRef(cPBRId, cId)
                End If

                clientPartnerBankRef.PartnerName = pName.Trim()
                clientPartnerBankRef.BankRef = bRef.Trim()
                clientPartnerBankRef.BankRefAgency = bRefAgency.Trim()
                clientPartnerBankRef.BankRefAccount = bRefAcc.Trim()
                clientPartnerBankRef.BankRefClientSince = bRefClientSince
                clientPartnerBankRef.BankRefContact = bRefContact.Trim()
                clientPartnerBankRef.BankRefPhone = bRefPhone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
                clientPartnerBankRef.BankRefAccountType = bRefAccType.Trim()
                clientPartnerBankRef.BankRefCredit = Decimal.Parse(bRefCredit.Replace(".", ","), numInfo)

                If cPBRId > 0 Then
                    clientPartnerBankRefCtrl.UpdateClientPartnerBankRef(clientPartnerBankRef)
                Else
                    clientPartnerBankRefCtrl.AddClientPartnerBankRef(clientPartnerBankRef)
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
        ''' <param name="cPBRId">Client Partner Bank Reference Source ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpDelete> _
        Function RemovePartnerBankReference(ByVal cPBRId As Integer, cId As Integer) As HttpResponseMessage
            Try
                Dim clientPartnerBankRefCtrl As New ClientPartnersBankRefsRepository

                clientPartnerBankRefCtrl.RemoveClientPartnerBankRef(cPBRId, cId)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Updates client login and or email by client user id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="cId">Client ID</param>
        ''' <param name="cUId">User ID</param>
        ''' <param name="newUsername">New Username</param>
        ''' <param name="subject">Subject</param>
        ''' <param name="body">Message</param>
        ''' <param name="mUId">Modified By User ID</param>
        ''' <param name="md">Modified Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPut> _
        Function UpdateClientUserLogin(ByVal portalId As Integer, ByVal cId As Integer, ByVal cUId As Integer, ByVal newUsername As String, ByVal subject As String, ByVal body As String, ByVal mUId As Integer, ByVal md As Date) As HttpResponseMessage
            Try

                If Not Utilities.ValidateUser(newUsername.ToLower()) > 0 Then

                    Dim clientUser As New Models.Client
                    Dim clientUserCtrl As New ClientsRepository

                    clientUser = clientUserCtrl.GetClient(cId, PortalController.GetCurrentPortalSettings().PortalId)

                    clientUser.Email = newUsername
                    clientUser.ModifiedByUser = mUId
                    clientUser.ModifiedOnDate = md

                    DotNetNuke.Security.Membership.MembershipProvider.Instance().ChangeUsername(cUId, newUsername)

                    DotNetNuke.Common.Utilities.DataCache.ClearCache()

                    Dim portalCtrl = New Portals.PortalController()
                    Dim _portalEmail = Portals.PortalController.GetPortalSetting("RIS_StoreEmail", portalId, portalCtrl.GetPortal(portalId).Email)
                    Dim _portalName = portalCtrl.GetPortal(portalId).PortalName
                    Dim storeUser As New Users.UserInfo() With {.UserID = Null.NullInteger, .Email = _portalEmail, .DisplayName = _portalName}
                    Dim _clientUserInfo = Users.UserController.GetUserById(portalId, cUId)

                    Notifications.SendStoreEmail(storeUser, _clientUserInfo, Nothing, Nothing, subject, body.Replace("[LOGIN]", newUsername).Replace("[*********]", MembershipProvider.Instance().GetPassword(_clientUserInfo, "")), Nothing, Nothing, Null.NullInteger, DotNetNuke.Services.Mail.MailFormat.Html, DotNetNuke.Services.Mail.MailPriority.High, storeUser, portalId, True)

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
        ''' <param name="cId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetClientHistory(ByVal cId As Integer) As HttpResponseMessage
            Try

                Dim clientHistories As New List(Of Models.ClientHistory)
                Dim clientHistoryCtrl As New ClientHistoriesRepository

                clientHistories = clientHistoryCtrl.GetClientHistories(cId)

                Return Request.CreateResponse(HttpStatusCode.OK, clientHistories)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Adds client history
        ''' </summary>
        ''' <param name="cId">Client ID</param>
        ''' <param name="hText">History Text</param>
        ''' <param name="uId">User ID</param>
        ''' <param name="cd">Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function AddClientHistory(ByVal cId As Integer, ByVal hText As String, ByVal uId As Integer, ByVal cd As Date) As HttpResponseMessage
            Try
                Dim clientHistory As New Models.ClientHistory
                Dim clientHistoryCtrl As New ClientHistoriesRepository

                clientHistory.HistoryText = hText
                clientHistory.CreatedByUser = uId
                clientHistory.CreatedOnDate = cd

                clientHistoryCtrl.AddClientHistory(clientHistory)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Adds client history
        ''' </summary>
        ''' <param name="cId">Client ID</param>
        ''' <param name="sId">Status ID</param>
        ''' <param name="hText">History</param>
        ''' <param name="uId">User ID</param>
        ''' <param name="cd">Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPut> _
        Function UpdateClientStatus(ByVal cId As Integer, ByVal sId As Integer, ByVal uId As Integer, ByVal cd As Date, Optional ByVal hText As String = "") As HttpResponseMessage
            Try
                Dim client As New Models.Client
                Dim clientCtrl As New ClientsRepository

                client = clientCtrl.GetClient(cId, PortalController.GetCurrentPortalSettings().PortalId)
                client.StatusId = sId
                client.ModifiedByUser = uId
                client.ModifiedOnDate = cd

                If hText <> "" Then
                    Dim clientHistory As New Models.ClientHistory
                    Dim clientHistoryCtrl As New ClientHistoriesRepository

                    clientHistory.HistoryText = hText
                    clientHistory.CreatedByUser = uId
                    clientHistory.CreatedOnDate = cd

                    clientHistoryCtrl.AddClientHistory(clientHistory)
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Adds client history
        ''' </summary>
        ''' <param name="cId">Client ID</param>
        ''' <param name="sent">Status ID</param>
        ''' <param name="hText">History Text</param>
        ''' <param name="uId">User ID</param>
        ''' <param name="cd">Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPut> _
        Function UpdateClientSent(ByVal cId As Integer, ByVal sent As Boolean, ByVal uId As Integer, ByVal cd As Date, Optional ByVal hText As String = "") As HttpResponseMessage
            Try
                Dim client As New Models.Client
                Dim clientCtrl As New ClientsRepository

                client = clientCtrl.GetClient(cId, PortalController.GetCurrentPortalSettings().PortalId)
                client.Sent = sent
                client.ModifiedByUser = uId
                client.ModifiedOnDate = cd

                If hText <> "" Then
                    Dim clientHistory As New Models.ClientHistory
                    Dim clientHistoryCtrl As New ClientHistoriesRepository

                    clientHistory.HistoryText = hText
                    clientHistory.CreatedByUser = uId
                    clientHistory.CreatedOnDate = cd

                    clientHistoryCtrl.AddClientHistory(clientHistory)
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

    End Class
End Namespace