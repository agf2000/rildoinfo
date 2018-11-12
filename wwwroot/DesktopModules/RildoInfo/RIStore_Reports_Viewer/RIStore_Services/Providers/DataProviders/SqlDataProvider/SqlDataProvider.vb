
Imports Microsoft.ApplicationBlocks.Data

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Framework.Providers
Imports DotNetNuke.Instrumentation


Namespace RI.Modules.RIStore_Services

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
        Private Const ModuleQualifier As String = "RIS_"

        Private ReadOnly _providerConfiguration As ProviderConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType)
        Private ReadOnly _connectionString As String
        Private ReadOnly _providerPath As String
        Private ReadOnly _objectQualifier As String
        Private ReadOnly _databaseOwner As String

#End Region

#Region "Constructors"

        Public Sub New()

            ' Read the configuration specific information for this provider
            Dim objProvider As Provider = DirectCast(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Provider)

            ' Read the attributes for this provider

            'Get Connection string from web.config
            _connectionString = Config.GetConnectionString()

            If String.IsNullOrEmpty(_connectionString) Then
                ' Use connection string specified in provider
                _connectionString = objProvider.Attributes("connectionString")
            End If

            _providerPath = objProvider.Attributes("providerPath")

            _objectQualifier = objProvider.Attributes("objectQualifier")
            If Not String.IsNullOrEmpty(_objectQualifier) AndAlso _objectQualifier.EndsWith("_", StringComparison.Ordinal) = False Then
                _objectQualifier += "_"
            End If

            _databaseOwner = objProvider.Attributes("databaseOwner")
            If Not String.IsNullOrEmpty(_databaseOwner) AndAlso _databaseOwner.EndsWith(".", StringComparison.Ordinal) = False Then
                _databaseOwner += "."

            End If
        End Sub

#End Region

#Region "Properties"

        Public ReadOnly Property ConnectionString() As String
            Get
                Return _connectionString
            End Get
        End Property

        Public ReadOnly Property ProviderPath() As String
            Get
                Return _providerPath
            End Get
        End Property

        Public ReadOnly Property ObjectQualifier() As String
            Get
                Return _objectQualifier
            End Get
        End Property

        Public ReadOnly Property DatabaseOwner() As String
            Get
                Return _databaseOwner
            End Get
        End Property

        ' used to prefect your database objects (stored procedures, tables, views, etc)
        Private ReadOnly Property NamePrefix() As String
            Get
                Return DatabaseOwner & ObjectQualifier & ModuleQualifier
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

        Public Overrides Function GetClients(PortalID As Integer,
            SalesRep As Integer,
            IsDeleted As String,
            SearchString As String,
            StatusId As Integer,
            StartDate As Date,
            EndDate As Date,
            PageNumber As Integer,
            PageSize As Integer,
            OrderBy As String) As IDataReader
            Dim ps = New PortalSecurity()
            Dim filterSort As String = ps.InputFilter(OrderBy, PortalSecurity.FilterFlag.NoSQL)
            Dim filterValue As String = ps.InputFilter(SearchString, PortalSecurity.FilterFlag.NoSQL)
            Dim isDeletedFilter As String = ps.InputFilter(IsDeleted, PortalSecurity.FilterFlag.NoSQL)
            Return SqlHelper.ExecuteReader(ConnectionString,
                                           NamePrefix & "Clients_GetList",
                                           GetNull(PortalID),
                                           GetNull(SalesRep),
                                           isDeletedFilter,
                                           filterValue,
                                           GetNull(StatusId),
                                           GetNull(StartDate),
                                           GetNull(EndDate),
                                           PageNumber,
                                           PageSize,
                                           filterSort)
        End Function

        Public Overrides Function GetClient(clientId As Integer, portalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Clients_Get", clientId, portalId)
        End Function

#End Region

#Region "PayConditions Methods"

        Public Overrides Function GetPayConds(PortalId As Integer, PayCondType As Integer, PayCondStart As Decimal) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "PayConds_Get",
                                                 GetNull(PortalId), GetNull(PayCondType), GetNull(PayCondStart)), IDataReader)
        End Function

#End Region

#Region "Agenda Methods"

        Public Overrides Function GetAgendaData(ByVal PortalId As String, ByVal UserID As String, ByVal StartDateTime As DateTime, ByVal EndDateTime As DateTime) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "AppointmentsData_Get", PortalId, UserID, GetNull(StartDateTime), GetNull(EndDateTime)), IDataReader)
        End Function

#End Region

#Region "Estimates Methods"

        Public Overrides Function GetEstimates(ByVal PortalID As String, ByVal ClientId As String, ByVal SalesRep As String, ByVal SearchTerm As String, ByVal PageNumber As Integer, ByVal PageSize As Integer, ByVal OrderBy As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Estimates_Get", PortalID, ClientId, SalesRep, SearchTerm, PageNumber, PageSize, OrderBy), IDataReader)
        End Function

        Public Overrides Function GetEstimateItems(EstimateId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "EstimateItems_Get", EstimateId), IDataReader)
        End Function

#End Region

#Region "Categories Methods"

        Public Overrides Function GetCategories_List(PortalId As Integer, Lang As String, ParentId As Integer, Filter As String, Archived As Boolean, IncludeArchived As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Categories_GetList", PortalId, Lang, ParentId, Filter, Archived, IncludeArchived), IDataReader)
        End Function

        Public Overrides Function GetCategory(CategoryId As Integer, Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Categories_Get", CategoryId, Lang), IDataReader)
        End Function

#End Region

#Region "Products Methods"

        Public Overrides Function GetProducts_List(PortalId As Integer,
            CategoryId As Integer,
            Lang As String,
            SearchText As String,
            GetDeleted As Boolean,
            FeaturedOnly As Boolean,
            OrderBy As String,
            OrderDesc As Boolean,
            ReturnLimit As String,
            PageIndex As Integer,
            PageSize As Integer,
            SearchDescription As Boolean,
            IsDealer As Boolean,
            CategoryList As String,
            ExcludeFeatured As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString,
                             NamePrefix & "Products_GetList",
                             PortalId,
                             CategoryId,
                             Lang,
                             SearchText,
                             GetDeleted,
                             FeaturedOnly,
                             OrderBy,
                             OrderDesc,
                             ReturnLimit,
                             PageIndex,
                             PageSize,
                             SearchDescription,
                             IsDealer,
                             CategoryList,
                             ExcludeFeatured), IDataReader)
        End Function

        Public Overrides Function GetProducts(ProductId As Integer, Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Products_Get", ProductId, Lang), IDataReader)
        End Function

        Public Overrides Function GetProductsLang(ProductId As Integer, Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductLang_Get", ProductId, Lang), IDataReader)
        End Function

        Public Overrides Function GetProductModel(ModelId As Integer, Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductModel_Get", ModelId, Lang), IDataReader)
        End Function

        Public Overrides Function GetProductModels(PortalID As Integer, ProductID As Integer, Lang As String, IsDealer As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductModel_GetList", PortalID, ProductID, Lang, IsDealer), IDataReader)
        End Function

        Public Overrides Function GetProductOption(OptionId As Integer, Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductOption_Get", OptionId, Lang), IDataReader)
        End Function

        Public Overrides Function GetProductOptionLang(OptionId As Integer, Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductOptionLang_Get", optionId, Lang), IDataReader)
        End Function

        Public Overrides Function GetProductOptions(ProductId As Integer, Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductOption_GetList", productId, lang), IDataReader)
        End Function

        Public Overrides Function GetProductOptionValue(OptionValueId As Integer, Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductOptionValue_Get", optionValueId, lang), IDataReader)
        End Function

        Public Overrides Function GetProductOptionValues(OptionId As Integer, Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductOptionValue_GetList", optionId, lang), IDataReader)
        End Function

        Public Overrides Function GetProductRelated(RelatedId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductRelated_Get", RelatedId), IDataReader)
        End Function

        Public Overrides Function GetProductsRelated(ProductId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductRelated_GetList", ProductId), IDataReader)
        End Function

#End Region

#End Region

    End Class
End Namespace