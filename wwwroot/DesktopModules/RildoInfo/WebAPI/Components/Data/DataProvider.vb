
Imports System.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Framework.Providers
Imports RIW.Modules.WebAPI.Components.Models

''' -----------------------------------------------------------------------------
''' <summary>
''' An abstract class for the data access layer
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' </history>
''' -----------------------------------------------------------------------------
Public MustInherit Class DataProvider

#Region "Shared/Static Methods"

    Private Shared provider As DataProvider

    ' return the provider
    Public Shared Function Instance() As DataProvider
        If provider Is Nothing Then
            Const [Assembly] As String = "RIW.Modules.WebAPI.SqlDataprovider,RIW.Modules.WebAPI"
            Dim objectType As Type = Type.[GetType]([Assembly], True, True)

            provider = DirectCast(Activator.CreateInstance(objectType), DataProvider)
            DataCache.SetCache(objectType.FullName, provider)
        End If

        Return provider
    End Function

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification:="Not returning class state information")>
    Public Shared Function GetConnection() As IDbConnection
        Const ProviderType As String = "data"
        Dim providerConfiguration As ProviderConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType)

        Dim objProvider As Provider = DirectCast(providerConfiguration.Providers(providerConfiguration.DefaultProvider), Provider)
        Dim connectionString As String
        If Not [String].IsNullOrEmpty(objProvider.Attributes("connectionStringName")) AndAlso Not [String].IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings(objProvider.Attributes("connectionStringName"))) Then
            connectionString = System.Configuration.ConfigurationManager.AppSettings(objProvider.Attributes("connectionStringName"))
        Else
            connectionString = objProvider.Attributes("connectionString")
        End If

        Dim newConnection As IDbConnection = New System.Data.SqlClient.SqlConnection() With {.ConnectionString = connectionString.ToString()}
        newConnection.Open()
        Return newConnection
    End Function

#End Region

#Region "Pay Forms Methods"

    Public MustOverride Function GetPayForms(portalId As Integer, isDeleted As String) As IDataReader

#End Region

#Region "PayConditions Methods"

    Public MustOverride Function GetPayConds(portalID As Integer, payCondType As Integer, payCondStart As Single) As IDataReader

#End Region

#Region "Agenda Methods"

    Public MustOverride Function GetAgendaData(portalId As Integer, userID As String, startDateTime As DateTime, endDateTime As DateTime, docId As Integer) As IDataReader

#End Region

#Region "Estimates Methods"

    Public MustOverride Function GetEstimate(estimateId As Integer, portalId As Integer, getAll As Boolean) As IDataReader
    Public MustOverride Function GetEstimates(portalId As Integer, personId As Integer, userId As Integer, salesRep As Integer, statusId As Integer, filterDates As String,
                                              startDate As DateTime, endDate As DateTime, filter As String, filterField As String, getAll As String,
                                              isDeleted As String, pageIndex As Integer, pageSize As Integer, orderBy As String, orderDesc As String) As IDataReader
    Public MustOverride Function GetDavs(portalId As Integer, personId As Integer, getAll As String, pageIndex As Integer, pageSize As Integer,
                                         orderBy As String, orderDesc As String) As IDataReader
    Public MustOverride Function GetEstimateItems(portalId As Integer, estimateId As Integer, lang As String) As IDataReader
    Public MustOverride Function GetEstimateHistories(estimateId As Integer) As IDataReader
    Public MustOverride Function GetEstimateMessage(estimateMessageId As Integer, estimateId As Integer) As IDataReader
    Public MustOverride Function GetEstimateMessages(estimateId As Integer) As IDataReader
    Public MustOverride Function GetEstimateMessageComment(commentId As Integer, estimateMessageId As Integer) As IDataReader
    Public MustOverride Function GetEstimateMessageComments(estimateMessageId As Integer) As IDataReader
    Public MustOverride Function GetEstimateHistoryComments(estimateHistoryId As Integer) As IDataReader
    Public MustOverride Function GetEstimateHistoryComment(commentId As Integer, estimateHistoryId As Integer) As IDataReader
    Public MustOverride Function GetEstimateDav(numDav As Integer, portalId As Integer) As IDataReader

#End Region

#Region "Categories Methods"

    Public MustOverride Function GetCategories_List(portalId As Integer, lang As String, parentId As Integer, filter As String, archived As Boolean,
    includeArchived As Boolean) As IDataReader
    Public MustOverride Function GetProductCategories(productId As Integer) As IDataReader
    Public MustOverride Function GetCategory(categoryId As Integer, lang As String) As IDataReader

#End Region

#Region "Products Methods"

    Public MustOverride Function GetProducts_List(portalId As Integer,
                                                  categoryId As Integer,
                                                  lang As String,
                                                  searchField As String,
                                                  searchString As String,
                                                  getArchived As Boolean,
                                                  featuredOnly As Boolean,
                                                  orderBy As String,
                                                  orderDesc As String,
                                                  returnLimit As String,
                                                  pageIndex As Integer,
                                                  pageSize As Integer,
                                                  onSale As String,
                                                  searchDesc As Boolean,
                                                  isDealer As Boolean,
                                                  getDeleted As String,
                                                  providerList As String,
                                                  sDate As String,
                                                  eDate As String,
                                                  filterDate As String,
                                                  categoryList As String,
                                                  excludeFeatured As Boolean) As IDataReader
    Public MustOverride Function GetProducts_ListAll(portalId As Integer, lang As String) As IDataReader
    Public MustOverride Function GetProducts(productId As Integer, lang As String) As IDataReader
    Public MustOverride Function GetProductOption(optionId As Integer, lang As String) As IDataReader
    Public MustOverride Function GetProductOptionLang(optionId As Integer, lang As String) As IDataReader
    Public MustOverride Function GetProductOptions(productId As Integer, lang As String) As IDataReader
    Public MustOverride Function GetProductOptionValues(optionId As Integer, lang As String) As IDataReader
    Public MustOverride Function GetProductOptionValue(optionValueId As Integer, lang As String) As IDataReader
    Public MustOverride Function GetProductRelated(relatedId As Integer) As IDataReader
    Public MustOverride Function GetProductsRelated(portalId As Integer, productId As Integer, lang As String, relatedType As Integer, getAll As Boolean) As IDataReader
    Public MustOverride Sub AddSGIProducts(produto As Components.Models.Softer)
    Public MustOverride Sub UpdateSGIProducts(produto As Components.Models.Softer)
    Public MustOverride Function GetSGIProducts(sDate As String, eDate As String) As IDataReader
    Public MustOverride Function GetSGIProductVendor(produto As Integer, fornecedor As Integer) As IDataReader
    Public MustOverride Function GetSGIProductCategory(grupo As Integer) As IDataReader
    Public MustOverride Function GetSGIDAVs(sDate As String, eDate As String) As IDataReader
    Public MustOverride Function GetSGIDAVItems(numDav As Integer) As IDataReader
    Public MustOverride Function GetSGIDav(codigo As Integer) As IDataReader
    Public MustOverride Function SaveSGIDav(dav As Components.Models.Softer) As Integer
    Public MustOverride Function SaveSGIDAVItem(davItem As Components.Models.Softer) As Integer
    Public MustOverride Sub UpdateSGIDAV(numDav As Integer)

#End Region

#Region "People Methods"

    Public MustOverride Function GetPeople(portalID As Integer, searchField As String, salesRep As Integer, isDeleted As String, searchString As String,
                                           statusId As Integer, registrationType As String, startDate As DateTime, endDate As DateTime,
    filterDate As String, pageIndex As Integer, pageSize As Integer, orderBy As String, orderDesc As String) As IDataReader
    Public MustOverride Function GetPerson(personId As Integer, portalId As Integer, userId As Integer) As IDataReader
    Public MustOverride Function GetPersonHistories(personId As Integer) As IDataReader
    Public MustOverride Function GetPersonDocs(personId As Integer) As IDataReader
    Public MustOverride Function GetPersonHistoryComment(commentId As Integer, personHistoryId As Integer) As IDataReader
    Public MustOverride Function GetPersonHistoryComments(personHistoryId As Integer) As IDataReader
    Public MustOverride Function GetUsers(portalId As Integer, roleName As String, isDeleted As String, searchTerm As String, startDate As DateTime,
                                          endDate As DateTime, pageIndex As Integer, pageSize As Integer, sortCol As String) As IDataReader
    Public MustOverride Function AddSGIPessoa(pessoa As Softer) As Integer
    Public MustOverride Function UpdateSGIPessoa(Codigo As Integer, TipoPessoa As Integer, fantasia As String, razao As String, numero As String, complemento As String,
                                              TipoPessoaFJ As String, cgccpf As String, inscricaoRG As String, email As String, homepage As String, ativo As Boolean,
                                              revendaconsumidor As String, codvendedor As Integer, codconvenio As Integer, codregiao As Integer, codclasse As Integer,
                                              codprofissao As Integer, tipologradouropessoa As String, logradouropessoa As String, bairropessoa As String,
                                              cidadepessoa As String, estadopessoa As String, ceppessoa As String, tipologradourotrabalho As String,
                                              logradourotrabalho As String, bairrotrabalho As String, cidadetrabalho As String, estadotrabalho As String,
                                              ceptrabalho As String, estadocivil As String, filiacao As String, sexo As String, nacionalidade As String,
                                              naturalidade As String, numerotrabalho As String, complementotrabalho As String, codconjuge As Integer,
                                              telefoneprincipal As String, fax As String, limitecredito As Single, cobrarjuros As Boolean, pagarcomissao As Boolean,
                                              bloquiodecredito As Boolean, bloqueiodefinitivo As Boolean, carencia As Integer, datanasfundacao As DateTime,
                                              datacadastro As DateTime, Local_trabalho As String, Contato As String, NascContato As DateTime, Contato2 As String,
                                              NascContato2 As DateTime, Telefone2 As String, Referencia1 As String, FoneRef1 As String, Referencia2 As String,
                                              FoneRef2 As String, AtividadeVendedor As Integer, Salario As Single, Renda As Single, Admissao As DateTime,
                                              Demissao As DateTime, CTPS As String, UsarEndPrincipal As Boolean, OBS As String) As Integer
    Public MustOverride Function GetSGIPeople(sDate As String, eDate As String) As IDataReader
    Public MustOverride Function GetSGIPerson(codigo As Integer) As IDataReader
    Public MustOverride Function GetSGIPersonAddress(codigo As Integer) As IDataReader

#End Region

#Region "Invoice Methods"

    Public MustOverride Function GetInvoices(portalId As Integer, providerId As Integer, clientId As Integer, productId As Integer,
    startingDate As DateTime, endingDate As DateTime, pageNumber As Integer, pageSize As Integer, orderBy As String) As IDataReader
    Public MustOverride Function GetInvoice(invoiceId As Integer) As IDataReader
    Public MustOverride Function GetInvoiceItems(invoiceId As Integer) As IDataReader

#End Region

#Region "Accounts Methods"

    Public MustOverride Function GetAccounts(portalId As Integer) As IDataReader
    Public MustOverride Function GetAccount(accountId As Integer, portalId As Integer) As IDataReader
    Public MustOverride Function GetAccountBalance(portalId As Integer, accountId As Integer, startingDate As DateTime,
    endingDate As DateTime, filterDate As String) As IDataReader

#End Region

#Region "Origins Methods"

    Public MustOverride Function GetOrigins(portalId As Integer) As IDataReader
    Public MustOverride Function GetOrigin(originId As Integer, portalId As Integer) As IDataReader

#End Region

#Region "Payments Methods"

    Public MustOverride Function GetPayments(portalId As Integer, accountId As Integer, originId As Integer, providerId As Integer,
    clientId As Integer, category As String, done As String, searchTerm As String, startingDate As DateTime,
    endingDate As DateTime, filterDate As String, pageNumber As Integer, pageSize As Integer, orderBy As String) As IDataReader
    Public MustOverride Function GetPayment(paymentId As Integer, portalId As Integer) As IDataReader

#End Region

#Region "Reports Methods"

    Public MustOverride Function RunReport(sql As String) As IDataReader

#End Region

End Class
