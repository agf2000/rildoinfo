
Imports Microsoft.ApplicationBlocks.Data

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Framework.Providers
Imports DotNetNuke.Instrumentation
Imports System.Data

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

#Region "Person Docs Methods"

    Public Overrides Function GetDocs(PortalId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Catalogs_GetList", PortalId), IDataReader)
    End Function

    Public Overrides Function GetDoc(DocId As Integer, PortalId As Integer) As IDataReader
        Return CType(SqlHelper.ExecuteReader(ConnectionString, NamePrefix & "Catalogs_Get", DocId, PortalId), IDataReader)
    End Function

#End Region

#End Region

End Class
