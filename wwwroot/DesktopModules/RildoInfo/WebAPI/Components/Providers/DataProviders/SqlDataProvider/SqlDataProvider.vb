
Imports Microsoft.ApplicationBlocks.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Framework.Providers
Imports RIW.Modules.WebAPI.Components.Models

''' -----------------------------------------------------------------------------
''' <summary>
''' SQL Server implementation of the abstract DataProvider class
''' 
''' This concreted data provider class provides the implementation of the abstract methods 
''' from data dataprovider.cs
''' 
''' In most cases you will only modify the Public methods region below.
''' </summary>
''' -----------------------------------------------------------------------------
Public Class SqlDataProvider
    Inherits DataProvider

#Region "Private Members"

    Private Const ProviderType As String = "data"
    Private Const ModuleQualifier As String = "RIW_"

    Private ReadOnly providerConfiguration As ProviderConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType)
    Private ReadOnly connectionString As String
    Private ReadOnly softerConnectionString As String
    Private ReadOnly providerPath As String
    Private ReadOnly objectQualifier As String
    Private ReadOnly databaseOwner As String

#End Region

#Region "Constructors"

    Public Sub New()

        ' Read the configuration specific information for this provider
        Dim objProvider As Provider = DirectCast(providerConfiguration.Providers(providerConfiguration.DefaultProvider), Provider)

        ' Read the attributes for this provider

        'Get Connection string from web.config
        connectionString = Config.GetConnectionString()
        softerConnectionString = Config.GetConnectionString("SGISqlServer")

        If String.IsNullOrEmpty(connectionString) Then
            ' Use connection string specified in provider
            connectionString = objProvider.Attributes("connectionString")
        End If

        If String.IsNullOrEmpty(softerConnectionString) Then
            softerConnectionString = objProvider.Attributes("sgiConnectionString")
        End If

        providerPath = objProvider.Attributes("providerPath")

        objectQualifier = objProvider.Attributes("objectQualifier")
        If Not String.IsNullOrEmpty(objectQualifier) AndAlso objectQualifier.EndsWith("_", StringComparison.Ordinal) = False Then
            objectQualifier += "_"
        End If

        databaseOwner = objProvider.Attributes("databaseOwner")
        If Not String.IsNullOrEmpty(databaseOwner) AndAlso databaseOwner.EndsWith(".", StringComparison.Ordinal) = False Then
            databaseOwner += "."

        End If
    End Sub

#End Region

#Region "Properties"

    ' used to prefect your database objects (stored procedures, tables, views, etc)
    Private ReadOnly Property NamePrefix() As String
        Get
            Return databaseOwner & objectQualifier & ModuleQualifier
        End Get
    End Property

#End Region

#Region "Private Methods"

    Private Shared Function GetNull(field As Object) As Object
        Return Null.GetNull(field, DBNull.Value)
    End Function

#End Region

#Region "Public Methods"

#Region "Clients Methods"

    Public Overrides Function GetPeople(portalID As Integer, searchField As String, salesRep As Integer, isDeleted As String, searchString As String,
    statusId As Integer, registrationType As String, startDate As DateTime, endDate As DateTime,
    filterDate As String, pageIndex As Integer, pageSize As Integer, orderBy As String, orderDesc As String) As IDataReader
        Dim ps = New PortalSecurity()
        Dim filterSort As String = ps.InputFilter(orderBy, PortalSecurity.FilterFlag.NoSQL)
        Dim filterSortDir As String = ps.InputFilter(orderDesc, PortalSecurity.FilterFlag.NoSQL)
        Dim filterValue As String = ps.InputFilter(searchString, PortalSecurity.FilterFlag.NoSQL)
        Dim isDeletedFilter As String = ps.InputFilter(isDeleted, PortalSecurity.FilterFlag.NoSQL)
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "People_GetList", GetNull(portalID), GetNull(salesRep), isDeletedFilter,
                                             searchField, filterValue, GetNull(statusId), registrationType, GetNull(startDate), GetNull(endDate), filterDate,
                                             pageIndex, pageSize, filterSort, filterSortDir), IDataReader)
    End Function

    Public Overrides Function GetPerson(personId As Integer, portalId As Integer, userId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Person_Get", GetNull(personId), GetNull(portalId), GetNull(userId)), IDataReader)
    End Function

    Public Overrides Function GetPersonHistories(personId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Person_Histories_GetList", personId), IDataReader)
    End Function

    Public Overrides Function AddSGIPessoa(pessoa As Softer) As Integer
        Dim sqlStr = String.Format("insert into pessoas([nome],[fantasia],[natureza],[email],[site],[vendedor],[ativo],[cpf_cnpj]," _
                                   & "[rg_insc_est],[observacao],[insc_municipal],[data_cadastro],[tipocli],[data_alteracao],[codigopessoa],[tipo]) " _
                                   & "values ('{0}','{1}','{2}','{3}','{4}',{5},1,'{6}','{7}','{8}','{9}',getdate(),'C',NULL,{10},{11}) " _
                                   & "select SCOPE_IDENTITY()", pessoa.razao, pessoa.fantasia, pessoa.tipopessoafj, pessoa.email, pessoa.homepage,
                                   pessoa.codvendedor, pessoa.cgccpf, pessoa.inscricaorg, pessoa.obs, pessoa.insc_municipal, pessoa.codigo, pessoa.tipopessoa)
        Return CType(SqlHelper.ExecuteScalar(softerConnectionString, CommandType.Text, sqlStr), Integer)
    End Function

    Public Overrides Function UpdateSGIPessoa(Codigo As Integer, TipoPessoa As Integer, fantasia As String, razao As String, numero As String, complemento As String,
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
        Return CType(SqlHelper.ExecuteNonQuery(softerConnectionString, "ID10_PESSOAS", Codigo, TipoPessoa, fantasia, razao, numero, complemento,
                                               TipoPessoaFJ, cgccpf, inscricaoRG, email, homepage, ativo, revendaconsumidor, codvendedor, codconvenio,
                                               codregiao, codclasse, codprofissao, tipologradouropessoa, logradouropessoa, bairropessoa,
                                               cidadepessoa, estadopessoa, ceppessoa, tipologradourotrabalho, logradourotrabalho, bairrotrabalho,
                                               cidadetrabalho, estadotrabalho, ceptrabalho, estadocivil, filiacao, sexo,
                                               nacionalidade, naturalidade, numerotrabalho, complementotrabalho,
                                               codconjuge, telefoneprincipal, fax, limitecredito, cobrarjuros, pagarcomissao,
                                               bloquiodecredito, bloqueiodefinitivo, carencia, datanasfundacao, datacadastro, Local_trabalho,
                                               Contato, NascContato, Contato2, NascContato2, Telefone2,
                                               Referencia1, FoneRef1, Referencia2, FoneRef2, AtividadeVendedor,
                                               Salario, Renda, Admissao, Demissao, CTPS, UsarEndPrincipal, OBS), Integer)
    End Function

#End Region

#Region "Pay Forms Methods"

    Public Overrides Function GetPayForms(portalId As Integer, isDeleted As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "PayForms_GetList", portalId, isDeleted), IDataReader)
    End Function

#End Region

#Region "PayConditions Methods"

    Public Overrides Function GetPayConds(portalId As Integer, payCondType As Integer, payCondStart As Single) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "PayConds_Get", GetNull(portalId), GetNull(payCondType), GetNull(payCondStart)), IDataReader)
    End Function

#End Region

#Region "Agenda Methods"

    Public Overrides Function GetAgendaData(portalId As Integer, userID As String, startDateTime As DateTime, endDateTime As DateTime, docId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "AppointmentsData_GetList", GetNull(portalId), userID, GetNull(startDateTime), GetNull(endDateTime), GetNull(docId)), IDataReader)
    End Function

#End Region

#Region "Estimates Methods"

    Public Overrides Function GetEstimate(estimateId As Integer, portalId As Integer, getAll As Boolean) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Estimates_Get", estimateId, portalId, getAll), IDataReader)
    End Function

    Public Overrides Function GetEstimates(portalId As Integer, personId As Integer, userId As Integer, salesRep As Integer, statusId As Integer, filterDates As String,
                                           startDate As DateTime, endDate As DateTime, filter As String, filterField As String, getAll As String,
                                           isDeleted As String, pageIndex As Integer, pageSize As Integer, orderBy As String, orderDesc As String) As IDataReader
        Dim ps = New PortalSecurity()
        Dim filterValue As String = ps.InputFilter(filter, PortalSecurity.FilterFlag.NoSQL)
        Dim filterTerm As String = ps.InputFilter(filterField, PortalSecurity.FilterFlag.NoSQL)
        Dim isDeletedFilter As String = ps.InputFilter(isDeleted, PortalSecurity.FilterFlag.NoSQL)
        Dim filterSort As String = ps.InputFilter(orderBy, PortalSecurity.FilterFlag.NoSQL)
        Dim filterDesc As String = ps.InputFilter(orderDesc, PortalSecurity.FilterFlag.NoSQL)
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Estimates_GetList", GetNull(portalId), GetNull(personId), GetNull(userId),
                                             GetNull(salesRep), GetNull(statusId), filterDates, GetNull(startDate), GetNull(endDate), filterValue, filterTerm, getAll, isDeletedFilter,
                                             pageIndex, pageSize, filterSort, filterDesc), IDataReader)
    End Function

    Public Overrides Function GetDavs(portalId As Integer, personId As Integer, getAll As String, pageIndex As Integer, pageSize As Integer,
                                      orderBy As String, orderDesc As String) As IDataReader
        Dim ps = New PortalSecurity()
        'Dim filterValue As String = ps.InputFilter(Filter, PortalSecurity.FilterFlag.NoSQL)
        'Dim filterTerm As String = ps.InputFilter(filterField, PortalSecurity.FilterFlag.NoSQL)
        Dim filterSort As String = ps.InputFilter(orderBy, PortalSecurity.FilterFlag.NoSQL)
        Dim filterDesc As String = ps.InputFilter(orderDesc, PortalSecurity.FilterFlag.NoSQL)
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Davs_GetList", GetNull(portalId), GetNull(personId), getAll,
                                             pageIndex, pageSize, filterSort, filterDesc), IDataReader)
    End Function

    Public Overrides Function GetEstimateItems(portalId As Integer, estimateId As Integer, lang As String) As IDataReader
        Dim result = CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "EstimateItems_GetList", portalId, estimateId, lang), IDataReader)
        Return result
    End Function

    Public Overrides Function GetEstimateHistories(estimateId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Estimate_Histories_GetList", estimateId), IDataReader)
    End Function

    Public Overrides Function GetEstimateMessage(estimateMessageId As Integer, estimateId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Estimate_Messages_Get", estimateMessageId, estimateId), IDataReader)
    End Function

    Public Overrides Function GetEstimateMessages(estimateId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Estimate_Messages_GetList", estimateId), IDataReader)
    End Function

    Public Overrides Function GetEstimateMessageComment(commentId As Integer, estimateMessageId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Estimate_MessageComments_Get", commentId, estimateMessageId), IDataReader)
    End Function

    Public Overrides Function GetEstimateMessageComments(estimateMessageId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Estimate_MessageComments_GetList", estimateMessageId), IDataReader)
    End Function

    Public Overrides Function GetEstimateHistoryComment(commentId As Integer, estimateHistoryId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Estimate_HistoryComments_Get", commentId, estimateHistoryId), IDataReader)
    End Function

    Public Overrides Function GetEstimateHistoryComments(estimateHistoryId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Estimate_HistoryComments_GetList", estimateHistoryId), IDataReader)
    End Function

#End Region

#Region "Categories Methods"

    Public Overrides Function GetCategories_List(portalId As Integer, lang As String, parentId As Integer, filter As String, archived As Boolean,
    includeArchived As Boolean) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Category_GetList", portalId, lang, parentId, filter, archived, includeArchived), IDataReader)
    End Function

    Public Overrides Function GetProductCategories(productId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "ProductCategories_GetAssigned", productId), IDataReader)
    End Function

    Public Overrides Function GetCategory(categoryId As Integer, lang As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Category_Get", categoryId, lang), IDataReader)
    End Function

#End Region

#Region "Products Methods"

    Public Overrides Function GetProducts_List(portalId As Integer,
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
                                               searchDescription As Boolean,
                                               isDealer As Boolean,
                                               getDeleted As String,
                                               providerList As String,
                                               sDate As String,
                                               eDate As String,
                                               filterDate As String,
                                               categoryList As String,
                                               excludeFeatured As Boolean) As IDataReader
        Dim ps = New PortalSecurity()
        Dim theSearchString As String = ps.InputFilter(searchString, PortalSecurity.FilterFlag.NoSQL)
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Products_GetList", portalId,
                                             categoryId, lang, searchField, theSearchString, getArchived, featuredOnly, orderBy, orderDesc, returnLimit, pageIndex, pageSize, onSale,
                                             searchDescription, isDealer, getDeleted, providerList, sDate, eDate, filterDate, categoryList, excludeFeatured), IDataReader)
    End Function

    Public Overrides Function GetProducts_ListAll(portalId As Integer, lang As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Products_GetAll", portalId, lang), IDataReader)
    End Function

    Public Overrides Function GetProducts(productId As Integer, lang As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Products_Get", productId, lang), IDataReader)
    End Function

    Public Overrides Function GetProductOption(optionId As Integer, lang As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "ProductOption_Get", optionId, lang), IDataReader)
    End Function

    Public Overrides Function GetProductOptionLang(optionId As Integer, lang As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "ProductOptionLang_Get", optionId, lang), IDataReader)
    End Function

    Public Overrides Function GetProductOptions(productId As Integer, lang As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "ProductOption_GetList", productId, lang), IDataReader)
    End Function

    Public Overrides Function GetProductOptionValue(optionValueId As Integer, lang As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "ProductOptionValue_Get", optionValueId, lang), IDataReader)
    End Function

    Public Overrides Function GetProductOptionValues(optionId As Integer, lang As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "ProductOptionValue_GetList", optionId, lang), IDataReader)
    End Function

    Public Overrides Function GetProductRelated(relatedId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "ProductRelated_Get", relatedId), IDataReader)
    End Function

    Public Overrides Function GetProductsRelated(portalId As Integer, productId As Integer, lang As String, relatedType As Integer, getAll As Boolean) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "ProductRelated_GetList", portalId, productId, lang, relatedType, getAll), IDataReader)
    End Function

#End Region

#Region "SGI Methods"

    Public Overrides Sub AddSGIProducts(produto As Components.Models.Softer)
        Dim sqlInsertText = String.Format("INSERT INTO PRODUTO ([nome], [grupo], [cod_barras], [tipo], [unidade], [unidiv], [est_min], [ativo], [icms], [cst], " _
                                          & "[AtivoBalanca]) VALUES ('{0}', {1}, {2}, '{3}', (select [Codigo] from unidade where Sigla = '{4}'), {5}, {6}, {7}, " _
                                          & "{8}, '{9}', '{10}')", produto.nome, CStr(If(CInt(produto.grupo) > 0, produto.grupo, "NULL")), produto.cod_barras,
                                          produto.tipo, produto.unidade, 1, produto.est_min, 1, produto.aliquotaicms, produto.cst, produto.ativobalanca)
        SqlHelper.ExecuteNonQuery(softerConnectionString, CommandType.Text, sqlInsertText)
    End Sub

    Public Overrides Sub UpdateSGIProducts(produto As Components.Models.Softer)
        SqlHelper.ExecuteNonQuery(softerConnectionString, "ID11_PRODUTOS", produto.codigo, produto.referencia, produto.barras, produto.descricao, produto.codigogrupo,
                                  GetNull(produto.codigosubgrupo), produto.aliquotaicms, produto.comissao, produto.unidade, produto.lucro, produto.codigoaliquotatrib,
                                  produto.aliquotaiss, produto.tipoproduto, produto.custoliquido, produto.customedio, produto.custobruto, produto.preco, produto.desconto,
                                  GetNull(produto.codigofornecedor), GetNull(produto.codigofabricante), produto.estoqueatual, produto.estoqueminimo, produto.estoquemaximo,
                                  produto.unidadedivisora, produto.pesopadrao, produto.aliquotaipi, produto.nbmsh, produto.cadastroinicial, produto.desativado, produto.cst,
                                  produto.localizacao, produto.validade, produto.garantia, produto.iat, produto.ippt, produto.nfe_pmvast, produto.nfe_predbcst,
                                  produto.sub_trib, produto.pis, produto.cofins, produto.nfe_predbc, produto.c_c_ipi, produto.c_c_subtrib, produto.c_c_freteper,
                                  produto.desp_acessorias, produto.encargos, produto.embalagem, produto.c_c_impostfed, produto.c_c_custofixo, produto.csosn,
                                  produto.cod_grupo_fiscal, GetNull(produto.categoria), produto.aplicacao, produto.obs, produto.naoimportarestoque, produto.naoimportarcustos, False)
    End Sub

    Public Overrides Function GetSGIProducts(sDate As String, eDate As String) As IDataReader
        Dim sqlCommand = String.Format("select p.codigo, p.nome, p.desc_compl, isnull(p.grupo, 0) as grupo, p.cod_barras, p.tipo as tipoproduto, p.unidade as unidadeid, (select sigla from unidade where codigo = p.unidade) as unidade, p.estoque, p.est_min, p.custo_final, p.custo_liq, p.preco, p.desconto, p.comissao, p.peso, p.ncm, isnull(p.obs, '') as obs, p.icms, p.ipi, p.sub_trib, p.frete, p.pis, p.cofins, isnull(p.cod_trib_ecf, '060') as cod_trib_ecf, isnull(p.cst, '') as cst, p.composicao, p.seriais, p.vencimentos, isnull(p.ingredientes, '') as ingredientes, p.descrevenda, p.data_cadastro, isnull(p.data_alteracao, cast('1900-01-01' as date)) as data_alteracao, isnull(p.aliquotaiss, 3.5) as aliquotaiss, isnull(p.grupofiscal, '') as grupofiscal, isnull(p.iat, 'a') as iat, isnull(p.ippt, 't') as ippt, p.produtomontado, isnull(p.cfop_saida_estado, '') as cfop_saida_estado, isnull((select top 1 pf.referencia from produtofornecedor as pf where pf.produto = p.codigo and pf.principal = 1), '') as ref, isnull((select top 1 f.fornecedor from produtofornecedor as f where f.produto = p.codigo and f.principal = 1), 0) as fornecedor, isnull((select (cast(v.fornecedor as nvarchar(max)) + ',') as [text()] from produtofornecedor v where v.produto = p.codigo for xml path('')), '') as fornecedores from produto as p where (p.data_cadastro between cast('{0}' as date) and cast('{1}' as date)) or (p.data_alteracao between cast('{0}' as date) and cast('{1}' as date)) order by p.codigo desc;", sDate, eDate)
        Return CType(SqlHelper.ExecuteReader(softerConnectionString, CommandType.Text, sqlCommand), IDataReader)
    End Function

    Public Overrides Function GetSGIProductVendor(produto As Integer, fornecedor As Integer) As IDataReader
        Dim sqlCommand = String.Format("select v.refxml, v.codigo, v.fornecedor, v.primeira_compra, v.principal, v.produto, v.referencia, v.ultima_compra from produtofornecedor v where v.produto = {0} and v.fornecedor = {1}", produto, fornecedor)
        Return CType(SqlHelper.ExecuteReader(softerConnectionString, CommandType.Text, sqlCommand), IDataReader)
    End Function

    Public Overrides Function GetSGIProductCategory(grupo As Integer) As IDataReader
        Dim sqlCommand = String.Format("select g.ncm, g.ativo, g.codigo, g.nome, g.cadastro from grupo g where g.codigo = {0}", grupo)
        Return CType(SqlHelper.ExecuteReader(softerConnectionString, CommandType.Text, sqlCommand), IDataReader)
    End Function

    Public Overrides Function GetSGIDAVs(sDate As String, eDate As String) As IDataReader
        Dim sqlCommand = String.Format("select isnull(d.numdoc, 0) as numdoc, d.numdav, sequenciadav = ISNULL(d.sequenciadav, 0), coo = ISNULL(d.coo, 0), coo_vinculado = ISNULL(d.coo_vinculado, 0), ccf = ISNULL(d.ccf, 0), d.datavenda, cupomcancelado = ISNULL(d.coo_cancelado, 0), codicli = ISNULL(d.codcliente, '0'), codiven = ISNULL(d.codvendedor, '0'), outrosdescontos = CAST(ISNULL(d.descontovalor, 0) as money), outrosacrescimos = CAST(ISNULL(d.acrescimoreal, 0) as money), valordin = CAST(ISNULL(d.valordinheiro, 0) as money), vrchevis = CAST(ISNULL(d.cheque, 0) as money), vrcartao = CAST(ISNULL(d.valorcartao, 0) as money), vrcred = CAST(ISNULL(d.valorcrediario, 0) as money), condpag = ISNULL(d.codcondpagto, 0), nome = ISNULL(cd.nome, ''), cd.comentrada, ISNULL(cd.acrescimo, 0), cd.intervalo, cd.nro_parcelas, d.[status] from dav_cab d left join condicaopagto cd on cd.codigo = d.codcondpagto where d.data_cadastro between CAST('{0}' as date) and CAST('{1}' as date) and d.sequenciadav > 0", sDate, eDate)
        Return CType(SqlHelper.ExecuteReader(softerConnectionString, CommandType.Text, sqlCommand), IDataReader)
    End Function

    Public Overrides Function GetSGIDAVItems(numDav As Integer) As IDataReader
        Dim sqlCommand = String.Format("select numdav, codproduto, valorunitario, quantidade, descontoperc, tipo, data_cadastro, data_alteracao, unidade, complemento, prevproxcompra, cancelado, campolivre1, campolivre2, campolivre3, ordem, codvendedor, dataaltpreco, hashstr, codigo, valortotal from dav_itens where numdav = {0}", numDav)
        Return CType(SqlHelper.ExecuteReader(softerConnectionString, CommandType.Text, sqlCommand), IDataReader)
    End Function

    Public Overrides Function GetSGIDav(codigo As Integer) As IDataReader
        Dim sqlCommand = String.Format("select 1 from dav_cab where numdav = {0}", codigo)
        Return CType(SqlHelper.ExecuteReader(softerConnectionString, CommandType.Text, sqlCommand), IDataReader)
    End Function

    Public Overrides Function SaveSGIDav(dav As Components.Models.Softer) As Integer
        Dim sqlInst = String.Format("insert into dav_cab (codCliente, codVendedor, valorTotal, dataVenda, [status], codCondPagto, valorDinheiro, " +
                                        "valorCartao, valorCrediario, descontoValor, descontoPerc, observacao) values ({0}, {1}, " +
                                        "convert(money, {2}), cast('{3}' as datetime), 1, {4}, convert(money, {5}), convert(money, {6}), convert(money, {7}), " +
                                        "convert(numeric(18,2), {8}), convert(numeric(18,2), {9}), '{10}') select scope_identity()",
                                        dav.codicli, dav.codiven, dav.total, dav.datavenda.ToString("dd/MM/yyyy"), dav.codpay, dav.valordin, dav.vrcartao, dav.vrcred, dav.desconto, dav.descperc, dav.observacao)
        'Return CType(SqlHelper.ExecuteScalar(softerConnectionString, "SP_GERAR_DAV_CAB", dav.codicli, dav.codiven, dav.total, dav.datavenda, GetNull(dav.status), GetNull(dav.codpay),
        '                           dav.valordin, dav.vrcartao, dav.vrcartao, dav.vrcred, False, dav.desconto, dav.descperc, dav.observacao, GetNull(Null.NullInteger), GetNull(Null.NullInteger),
        '                           GetNull(Null.NullString), GetNull(Null.NullString), GetNull(Null.NullString), GetNull(Null.NullString), 0), Integer)

        Return CType(SqlHelper.ExecuteScalar(softerConnectionString, CommandType.Text, sqlInst), Integer)
    End Function

    Public Overrides Function SaveSGIDAVItem(davItem As Components.Models.Softer) As Integer
        Return CType(SqlHelper.ExecuteScalar(softerConnectionString, "SP_GERAR_DAV_ITENS", davItem.numdav, davItem.codproduto, davItem.quantidade, davItem.valorunitario,
                                  davItem.descontoperc, GetNull(Null.NullString), GetNull(Null.NullString), GetNull(Null.NullString)), Integer)
    End Function

    Public Overrides Sub UpdateSGIDAV(numDav As Integer)
        Dim sqlCommand = String.Format("update dav_cab set status = 1 where numdav = {0}", numDav)
        SqlHelper.ExecuteNonQuery(softerConnectionString, CommandType.Text, sqlCommand)
    End Sub

    Public Overrides Function GetEstimateDav(numDav As Integer, portalId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Davs_Get", numDav, portalId), IDataReader)
    End Function

#End Region

#Region "People Methods"

    Public Overrides Function GetPersonDocs(personId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "PeopleDocs_GetList", personId), IDataReader)
    End Function

    Public Overrides Function GetPersonHistoryComment(commentId As Integer, personHistoryId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Person_HistoryComments_Get", commentId, personHistoryId), IDataReader)
    End Function

    Public Overrides Function GetPersonHistoryComments(personHistoryId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Person_HistoryComments_GetList", personHistoryId), IDataReader)
    End Function

    Public Overrides Function GetUsers(portalId As Integer, roleName As String, isDeleted As String, searchTerm As String, startDate As DateTime,
    endDate As DateTime, pageIndex As Integer, pageSize As Integer, sortCol As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Users_GetList", portalId, roleName, isDeleted, searchTerm,
                                             GetNull(startDate), GetNull(endDate), pageIndex, pageSize, sortCol), IDataReader)
    End Function

    Public Overrides Function GetSGIPeople(sDate As String, eDate As String) As IDataReader
        Dim sqlCommand = String.Format("select p.codigo, p.tipo, p.nome, p.fantasia, p.natureza, ISNULL(ve.cep, 00000000) as cep, ISNULL(p.numero, '') as numero, ISNULL(p.complemento, '') as complemento, ISNULL(p.email, '') as email, ISNULL(p.site, '') as website, ISNULL(p.vendedor, 0) as vendedor, p.ativo, ISNULL(p.cpf_cnpj, '') as cpf_cnpj, ISNULL(p.rg_insc_est, '') as rg_insc_est, ISNULL(p.nascimento, CAST('1900-01-01' as date)) as nascimento, ISNULL(p.observacao, '') as observacao, ISNULL(p.insc_municipal, '') as insc_municipal, ISNULL(p.data_cadastro, CAST('1900-01-01' as date)) as data_cadastro, p.tipocli, ISNULL(p.data_alteracao, CAST('1900-01-01' as date)) as data_altercao, ISNULL(p.codigopessoa, 0) as codigopessoa, ISNULL(ve.logradouro, '') as logradouro, ISNULL(ve.tipo_logradouro, '') as tipo_logradouro, ISNULL(ve.bairro, '') as bairro, ISNULL(ve.cidade, '') as cidade, ISNULL(ve.estado, '') as estado, ISNULL(ve.nompais, '') as nompais, ISNULL(f.bloqueado, 0) as bloqueado, ISNULL(f.limite_credito, 0) as limite_credito, SUBSTRING(ISNULL(t.telefone, ''), 0, 10) as telefone from dbo.pessoas p left outer join view_endereco_completo ve on p.cep = ve.codigo left outer join financeira f on p.codigo = f.pessoa left outer join telefone t on p.codigo = t.pessoa and t.padrao = 1 and t.padrao <> null where ((p.tipo in (1, 2)) and ((p.data_cadastro between CAST('{0}' as date) and CAST('{1}' as date)) or (p.data_alteracao between CAST('{0}' as date) and CAST('{1}' as date)))) order by p.codigo", sDate, eDate)
        Return CType(SqlHelper.ExecuteReader(softerConnectionString, CommandType.Text, sqlCommand), IDataReader)
    End Function

    Public Overrides Function GetSGIPerson(codigo As Integer) As IDataReader
        Dim sqlCommand = String.Format("select p.codigo, p.tipo as tipopessoa, p.nome, p.nome as razao, p.fantasia, p.natureza, p.natureza as tipopessoafj, isnull(ve.cep, 00000000) as cep, isnull(ve.cep, 00000000) as ceppessoa, isnull(p.numero, '') as numero, isnull(p.complemento, '') as complemento, isnull(p.email, '') as email, isnull(p.site, '') as website, isnull(p.site, '') as homepage, isnull(p.vendedor, 0) as vendedor, isnull(p.vendedor, 0) as codvendedor, isnull(p.convenio, 0) as codconvenio, isnull(p.regiao, 0) as codregiao, isnull(p.classe, 0) as codclasse, 0 as codprofissao, p.ativo, isnull(p.cpf_cnpj, '') as cpf_cnpj, isnull(p.cpf_cnpj, '') as cgccpf, isnull(p.rg_insc_est, '') as rg_insc_est, isnull(p.rg_insc_est, '') as inscricaorg, isnull(p.nascimento, cast('1900-01-01' as date)) as nascimento, isnull(p.observacao, '') as observacao, isnull(p.insc_municipal, '') as insc_municipal, isnull(p.data_cadastro, cast('1900-01-01' as date)) as data_cadastro, isnull(p.data_cadastro, cast('1900-01-01' as date)) as datacadastro, isnull(p.data_alteracao, cast('1900-01-01' as date)) as data_altercao, isnull(p.codigopessoa, 0) as codigopessoa, isnull(ve.logradouro, '') as logradouro, isnull(ve.logradouro, '') as logradouropessoa, isnull(ve.tipo_logradouro, 'rua') as tipo_logradouro, isnull(ve.tipo_logradouro, 'rua') as tipologradouropessoa, isnull(ve.bairro, '') as bairro, isnull(ve.bairro, '') as bairropessoa, isnull(ve.cidade, '') as cidade, isnull(ve.cidade, '') as cidadepessoa, isnull(ve.estado, '') as estado, isnull(ve.estado, '') as estadopessoa, isnull(ve.nompais, '') as nompais, isnull(f.bloqueado, 0) as bloqueado, isnull(f.bloqueado, 0) as bloqueiodefinitivo, isnull(f.limite_credito, 0) as limite_credito, isnull(f.limite_credito, 0) as limitecredito, substring(isnull(t.telefone, ''), 0, 10) as telefone, substring(isnull(t.telefone, ''), 0, 10) as telefoneprincipal, p.tipocli, isnull(p.tipocli, 'c') as revendaconsumidor, isnull(p.observacao, '') as obs from dbo.pessoas p left outer join view_endereco_completo ve on p.cep = ve.codigo left outer join financeira f on p.codigo = f.pessoa left outer join telefone t on p.codigo = t.pessoa and t.padrao = 1 and t.padrao <> null where p.codigo = {0}", codigo)
        Return CType(SqlHelper.ExecuteReader(softerConnectionString, CommandType.Text, sqlCommand), IDataReader)
    End Function

    Public Overrides Function GetSGIPersonAddress(codigo As Integer) As IDataReader
        Dim sqlCommand = String.Format("select p.numero, p.complemento, v.bairro, v.cep, v.cidade, v.estado, v.logradouro, v.tipo_logradouro, v.codibge, v.nompais, v.nome_estado from pessoas p left outer join view_endereco_completo v on p.cep = v.codigo where p.codigo = {0}", codigo)
        Return CType(SqlHelper.ExecuteReader(softerConnectionString, CommandType.Text, sqlCommand), IDataReader)
    End Function

#End Region

#Region "Invoice Methods"

    Public Overrides Function GetInvoices(portalId As Integer, providerId As Integer, clientId As Integer, productId As Integer, startingDate As DateTime,
    endingDate As DateTime, pageNumber As Integer, pageSize As Integer, orderBy As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Invoices_GetList", GetNull(portalId), GetNull(providerId), GetNull(clientId),
                                             GetNull(productId), GetNull(startingDate), GetNull(endingDate), pageNumber, pageSize, orderBy), IDataReader)
    End Function

    Public Overrides Function GetInvoice(invoiceId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Invoices_Get", invoiceId), IDataReader)
    End Function

    Public Overrides Function GetInvoiceItems(invoiceId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "InvoiceItems_GetList", invoiceId), IDataReader)
    End Function

#End Region

#Region "Accounts Methods"

    Public Overrides Function GetAccounts(portalId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Accounts_GetList", portalId), IDataReader)
    End Function

    Public Overrides Function GetAccount(accountId As Integer, portalId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Accounts_Get", accountId, portalId), IDataReader)
    End Function

    Public Overrides Function GetAccountBalance(portalId As Integer, accountId As Integer, startingDate As DateTime, endingDate As DateTime,
    filterDate As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "AccountsBalance_Get", GetNull(portalId), GetNull(accountId), startingDate,
                                             endingDate, filterDate), IDataReader)
    End Function

#End Region

#Region "Origins Methods"

    Public Overrides Function GetOrigins(portalId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Origins_GetList", portalId), IDataReader)
    End Function

    Public Overrides Function GetOrigin(originId As Integer, portalId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Origins_Get", originId, portalId), IDataReader)
    End Function

#End Region

#Region "Payments Methods"

    Public Overrides Function GetPayments(portalId As Integer, accountId As Integer, originId As Integer, providerId As Integer,
    clientId As Integer, category As String, done As String, searchTerm As String, startingDate As DateTime,
    endingDate As DateTime, filterDate As String, pageNumber As Integer, pageSize As Integer, orderBy As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Payments_GetList", GetNull(portalId), GetNull(accountId), GetNull(originId),
                                             GetNull(providerId), GetNull(clientId), category, done, searchTerm, GetNull(startingDate), GetNull(endingDate),
                                             filterDate, pageNumber, pageSize, orderBy), IDataReader)
    End Function

    Public Overrides Function GetPayment(paymentId As Integer, portalId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, NamePrefix & "Payments_Get", paymentId, portalId), IDataReader)
    End Function

#End Region

#Region "Reports Methods"

    Public Overrides Function RunReport(sql As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql), IDataReader)
    End Function

#End Region

#End Region

End Class
