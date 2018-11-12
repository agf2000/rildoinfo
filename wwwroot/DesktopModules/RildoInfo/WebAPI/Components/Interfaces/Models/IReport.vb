
Namespace Components.Interfaces.Models

    Public Interface IReport

        Property PortalId As Integer

        Property ReportId As Integer

        Property ReportName As String

        Property ReportDescription As String

        Property ReportSql As String

        Property ReportSqlDetail As String

        Property CreatedByUser As Integer

        Property CreatedOnDate As DateTime

        Property ModifiedByUser As Integer

        Property ModifiedOnDate As DateTime
    End Interface
End Namespace