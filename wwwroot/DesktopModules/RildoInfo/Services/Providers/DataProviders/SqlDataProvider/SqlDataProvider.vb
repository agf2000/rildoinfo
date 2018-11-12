
Imports Microsoft.ApplicationBlocks.Data

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Framework.Providers
Imports DotNetNuke.Instrumentation


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

    Public Overrides Function GetPeople(PortalID As Integer,
        SalesRep As Integer,
        IsDeleted As String,
        SearchString As String,
        StatusId As Integer,
        RegistrionType As String,
        StartDate As DateTime,
        EndDate As DateTime,
        PageIndex As Integer,
        PageSize As Integer,
        OrderBy As String) As IDataReader
        Dim ps = New PortalSecurity()
        Dim filterSort As String = ps.InputFilter(OrderBy, PortalSecurity.FilterFlag.NoSQL)
        Dim filterValue As String = ps.InputFilter(SearchString, PortalSecurity.FilterFlag.NoSQL)
        Dim isDeletedFilter As String = ps.InputFilter(IsDeleted, PortalSecurity.FilterFlag.NoSQL)
        Return CType(SqlHelper.ExecuteReader(ConnectionString,
                                       NamePrefix & "People_GetList",
                                       GetNull(PortalID),
                                       GetNull(SalesRep),
                                       isDeletedFilter,
                                       filterValue,
                                       GetNull(StatusId),
                                       RegistrionType,
                                       GetNull(StartDate),
                                       GetNull(EndDate),
                                       PageIndex,
                                       PageSize,
                                       filterSort), IDataReader)
    End Function

    Public Overrides Function GetPerson(PersonId As Integer, PortalId As Integer, UserId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Person_Get", GetNull(PersonId), GetNull(PortalId), GetNull(UserId)), IDataReader)
    End Function

    Public Overrides Function GetPersonHistories(PersonId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Person_Histories_GetList", PersonId), IDataReader)
    End Function

#End Region

#Region "Pay Forms Methods"

    Public Overrides Function GetPayForms(PortalId As Integer, IsDeleted As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "PayForms_GetList", PortalId, IsDeleted), IDataReader)
    End Function

#End Region

#Region "PayConditions Methods"

    Public Overrides Function GetPayConds(PortalId As Integer, PayCondType As Integer, PayCondStart As Decimal) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "PayConds_Get", GetNull(PortalId), GetNull(PayCondType), GetNull(PayCondStart)), IDataReader)
    End Function

#End Region

#Region "Agenda Methods"

    Public Overrides Function GetAgendaData(ByVal PortalId As String, ByVal UserID As String, ByVal StartDateTime As DateTime, ByVal EndDateTime As DateTime) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "AppointmentsData_Get", PortalId, UserID, GetNull(StartDateTime), GetNull(EndDateTime)), IDataReader)
    End Function

#End Region

#Region "Estimates Methods"

    Public Overrides Function GetEstimate(EstimateId As Integer, PortalId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Estimates_Get", EstimateId, PortalId), IDataReader)
    End Function

    Public Overrides Function GetEstimates(PortalId As Integer,
        PersonId As Integer,
        SalesRep As Integer,
        StatusId As Integer,
        StartDate As DateTime,
        EndDate As DateTime,
        Filter As String,
        IsDeleted As String,
        PageIndex As Integer,
        PageSize As Integer,
        OrderBy As String) As IDataReader
        Dim ps = New PortalSecurity()
        Dim filterValue As String = ps.InputFilter(Filter, PortalSecurity.FilterFlag.NoSQL)
        Dim isDeletedFilter As String = ps.InputFilter(IsDeleted, PortalSecurity.FilterFlag.NoSQL)
        Dim filterSort As String = ps.InputFilter(OrderBy, PortalSecurity.FilterFlag.NoSQL)
        Return CType(SqlHelper.ExecuteReader(ConnectionString,
                         NamePrefix & "Estimates_GetList",
                         GetNull(PortalId),
                         GetNull(PersonId),
                         GetNull(SalesRep),
                         GetNull(StatusId),
                         GetNull(StartDate),
                         GetNull(EndDate),
                         filterValue,
                         isDeletedFilter,
                         PageIndex,
                         PageSize,
                         filterSort), IDataReader)
    End Function

    Public Overrides Function GetEstimateItems(PortalId As Integer, EstimateId As Integer, Lang As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "EstimateItems_GetList", PortalId, EstimateId, Lang), IDataReader)
    End Function

    Public Overrides Function GetEstimateHistories(EstimateId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Estimate_Histories_GetList", EstimateId), IDataReader)
    End Function

    Public Overrides Function GetEstimateMessage(EstimateMessageId As Integer, EstimateId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Estimate_Messages_Get", EstimateMessageId, EstimateId), IDataReader)
    End Function

    Public Overrides Function GetEstimateMessages(EstimateId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Estimate_Messages_GetList", EstimateId), IDataReader)
    End Function

    Public Overrides Function GetEstimateMessageComment(CommentId As Integer, EstimateMessageId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Estimate_MessageComments_Get", CommentId, EstimateMessageId), IDataReader)
    End Function

    Public Overrides Function GetEstimateMessageComments(EstimateMessageId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Estimate_MessageComments_GetList", EstimateMessageId), IDataReader)
    End Function

    Public Overrides Function GetEstimateHistoryComment(CommentId As Integer, EstimateHistoryId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Estimate_HistoryComments_Get", CommentId, EstimateHistoryId), IDataReader)
    End Function

    Public Overrides Function GetEstimateHistoryComments(EstimateHistoryId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Estimate_HistoryComments_GetList", EstimateHistoryId), IDataReader)
    End Function

#End Region

#Region "Categories Methods"

    Public Overrides Function GetCategories_List(PortalId As Integer, Lang As String, ParentId As Integer, Filter As String, Archived As Boolean, IncludeArchived As Boolean) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Category_GetList", PortalId, Lang, ParentId, Filter, Archived, IncludeArchived), IDataReader)
    End Function

    Public Overrides Function GetProductCategories(ProductId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductCategories_GetAssigned", ProductId), IDataReader)
    End Function

    Public Overrides Function GetCategory(CategoryId As Integer, Lang As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Category_Get", CategoryId, Lang), IDataReader)
    End Function

#End Region

#Region "Products Methods"

    Public Overrides Function GetProducts_List(PortalId As Integer,
        CategoryId As Integer,
        Lang As String,
        SearchText As String,
        GetArchived As Boolean,
        FeaturedOnly As Boolean,
        OrderBy As String,
        OrderDesc As String,
        ReturnLimit As String,
        PageIndex As Integer,
        PageSize As Integer,
        OnSale As String,
        SearchDescription As Boolean,
        IsDealer As Boolean,
        GetDeleted As String,
        CategoryList As String,
        ExcludeFeatured As Boolean) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString,
                         NamePrefix & "Product_GetList",
                         PortalId,
                         CategoryId,
                         Lang,
                         SearchText,
                         GetArchived,
                         FeaturedOnly,
                         OrderBy,
                         OrderDesc,
                         ReturnLimit,
                         PageIndex,
                         PageSize,
                         OnSale,
                         SearchDescription,
                         IsDealer,
                         GetDeleted,
                         CategoryList,
                         ExcludeFeatured), IDataReader)
    End Function

    Public Overrides Function GetProducts(ProductId As Integer, Lang As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Product_Get", ProductId, Lang), IDataReader)
    End Function

    'Public Overrides Function GetProductsLang(ProductId As Integer, Lang As String) As IDataReader
    '    Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductLang_Get", ProductId, Lang), IDataReader)
    'End Function

    Public Overrides Function GetProductOption(OptionId As Integer, Lang As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductOption_Get", OptionId, Lang), IDataReader)
    End Function

    Public Overrides Function GetProductOptionLang(OptionId As Integer, Lang As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductOptionLang_Get", OptionId, Lang), IDataReader)
    End Function

    Public Overrides Function GetProductOptions(ProductId As Integer, Lang As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductOption_GetList", ProductId, Lang), IDataReader)
    End Function

    Public Overrides Function GetProductOptionValue(OptionValueId As Integer, Lang As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductOptionValue_Get", OptionValueId, Lang), IDataReader)
    End Function

    Public Overrides Function GetProductOptionValues(OptionId As Integer, Lang As String) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductOptionValue_GetList", OptionId, Lang), IDataReader)
    End Function

    Public Overrides Function GetProductRelated(RelatedId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductRelated_Get", RelatedId), IDataReader)
    End Function

    Public Overrides Function GetProductsRelated(PortalId As Integer, ProductId As Integer, Lang As String, RelatedType As Integer, GetAll As Boolean) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "ProductRelated_GetList", PortalId, ProductId, Lang, RelatedType, GetAll), IDataReader)
    End Function

#End Region

#Region "Person Methods"

    Public Overrides Function GetPersonDocs(PersonId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "PeopleDocs_GetList", PersonId), IDataReader)
    End Function

    Public Overrides Function GetPersonHistoryComment(CommentId As Integer, PersonHistoryId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Person_HistoryComments_Get", CommentId, PersonHistoryId), IDataReader)
    End Function

    Public Overrides Function GetPersonHistoryComments(PersonHistoryId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Person_HistoryComments_GetList", PersonHistoryId), IDataReader)
    End Function

#End Region

#End Region

End Class
