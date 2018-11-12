
Imports System.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Framework.Providers
Imports DotNetNuke.Instrumentation

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

    Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(DataProvider))

#Region "Shared/Static Methods"

    Private Shared provider As DataProvider

    ' return the provider
    Public Shared Function Instance() As DataProvider
        If provider Is Nothing Then
            Const assembly As String = "RIW.Modules.Services.SqlDataprovider,RIW.Modules.Services"
            Dim objectType As Type = Type.[GetType](assembly, True, True)

            provider = DirectCast(Activator.CreateInstance(objectType), DataProvider)
            DataCache.SetCache(objectType.FullName, provider)
        End If

        Return provider
    End Function

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification:="Not returning class state information")> _
    Public Shared Function GetConnection() As IDbConnection
        Const providerType As String = "data"
        Dim _providerConfiguration As ProviderConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType)

        Dim objProvider As Provider = DirectCast(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Provider)
        Dim _connectionString As String
        If Not [String].IsNullOrEmpty(objProvider.Attributes("connectionStringName")) AndAlso Not [String].IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings(objProvider.Attributes("connectionStringName"))) Then
            _connectionString = System.Configuration.ConfigurationManager.AppSettings(objProvider.Attributes("connectionStringName"))
        Else
            _connectionString = objProvider.Attributes("connectionString")
        End If

        Dim newConnection As IDbConnection = New System.Data.SqlClient.SqlConnection() With {.ConnectionString = _connectionString.ToString()}
        newConnection.Open()
        Return newConnection
    End Function

#End Region

#Region "Clients Methods"

    Public MustOverride Function GetPeople(PortalID As Integer, SalesRep As Integer, IsDeleted As String, SearchString As String, StatusId As Integer, RegistrionType As String, StartDate As DateTime, EndDate As DateTime, PageIndex As Integer, PageSize As Integer, OrderBy As String) As IDataReader
    Public MustOverride Function GetPerson(PersonId As Integer, PortalId As Integer, Userid As Integer) As IDataReader
    Public MustOverride Function GetPersonHistories(PersonId As Integer) As IDataReader

#End Region

#Region "Pay Forms Methods"

    Public MustOverride Function GetPayForms(PortalId As Integer, IsDeleted As String) As IDataReader

#End Region

#Region "PayConditions Methods"

    Public MustOverride Function GetPayConds(PortalID As Integer, PayCondType As Integer, PayCondStart As Decimal) As IDataReader

#End Region

#Region "Agenda Methods"

    Public MustOverride Function GetAgendaData(PortalId As String, UserID As String, StartDateTime As DateTime, EndDateTime As DateTime) As IDataReader

#End Region

#Region "Estimates Methods"

    Public MustOverride Function GetEstimate(EstimateId As Integer, PortalId As Integer) As IDataReader
    Public MustOverride Function GetEstimates(PortalId As Integer, PersonId As Integer, SalesRep As Integer, StatusId As Integer, StartDate As DateTime, EndDate As DateTime, Filter As String, IsDeleted As String, PageIndex As Integer, PageSize As Integer, OrderBy As String) As IDataReader
    Public MustOverride Function GetEstimateItems(PortalId As Integer, EstimateId As Integer, Lang As String) As IDataReader
    Public MustOverride Function GetEstimateHistories(EstimateId As Integer) As IDataReader
    Public MustOverride Function GetEstimateMessage(EstimateMessageId As Integer, EstimateId As Integer) As IDataReader
    Public MustOverride Function GetEstimateMessages(EstimateId As Integer) As IDataReader
    Public MustOverride Function GetEstimateMessageComment(CommentId As Integer, EstimateMessageId As Integer) As IDataReader
    Public MustOverride Function GetEstimateMessageComments(EstimateMessageId As Integer) As IDataReader
    Public MustOverride Function GetEstimateHistoryComments(EstimateHistoryId As Integer) As IDataReader
    Public MustOverride Function GetEstimateHistoryComment(CommentId As Integer, EstimateHistoryId As Integer) As IDataReader

#End Region

#Region "Categories Methods"

    Public MustOverride Function GetCategories_List(PortalId As Integer, Lang As String, ParentId As Integer, Filter As String, Archived As Boolean, IncludeArchived As Boolean) As IDataReader
    Public MustOverride Function GetProductCategories(ProductId As Integer) As IDataReader
    Public MustOverride Function GetCategory(CategoryId As Integer, Lang As String) As IDataReader

#End Region

#Region "Products Methods"

    Public MustOverride Function GetProducts_List(PortalId As Integer, CategoryId As Integer, Lang As String, Filter As String, GetArchived As Boolean, FeaturedOnly As Boolean, OrderBy As String, OrderDesc As String, ReturnLimit As String, PageIndex As Integer, PageSize As Integer, OnSale As String, SearchDesc As Boolean, IsDealer As Boolean, GetDeleted As String, CategoryList As String, ExcludeFeatured As Boolean) As IDataReader
    Public MustOverride Function GetProducts(ProductId As Integer, Lang As String) As IDataReader
    'Public MustOverride Function GetProductsLang(ProductId As Integer, Lang As String) As IDataReader
    Public MustOverride Function GetProductOption(OptionId As Integer, Lang As String) As IDataReader
    Public MustOverride Function GetProductOptionLang(OptionId As Integer, Lang As String) As IDataReader
    Public MustOverride Function GetProductOptions(ProductId As Integer, Lang As String) As IDataReader
    Public MustOverride Function GetProductOptionValues(OptionId As Integer, Lang As String) As IDataReader
    Public MustOverride Function GetProductOptionValue(OptionValueId As Integer, Lang As String) As IDataReader
    Public MustOverride Function GetProductRelated(RelatedId As Integer) As IDataReader
    Public MustOverride Function GetProductsRelated(PortalId As Integer, ProductId As Integer, Lang As String, RelatedType As Integer, GetAll As Boolean) As IDataReader

#End Region

#Region "Person Methods"

    Public MustOverride Function GetPersonDocs(PersonId As Integer) As IDataReader
    Public MustOverride Function GetPersonHistoryComment(CommentId As Integer, PersonHistoryId As Integer) As IDataReader
    Public MustOverride Function GetPersonHistoryComments(PersonHistoryId As Integer) As IDataReader

#End Region

End Class
