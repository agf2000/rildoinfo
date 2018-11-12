
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
            Const assembly As String = "RIW.Modules.ContactForm.SqlDataprovider,RIW.Modules.ContactForm"
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

#Region "Docs Methods"

    Public MustOverride Function GetDocs(PortalId As Integer) As IDataReader

    Public MustOverride Function GetDoc(DocId As Integer, PortalId As Integer) As IDataReader

#End Region

End Class
