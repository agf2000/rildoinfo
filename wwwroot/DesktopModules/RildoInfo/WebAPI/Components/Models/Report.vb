
Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_Reports")> _
    <PrimaryKey("ReportId", AutoIncrement:=True)> _
    <Cacheable("Reports", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class Report
        Implements IReport

        Public Property CreatedByUser As Integer Implements IReport.CreatedByUser

        Public Property CreatedOnDate As Date Implements IReport.CreatedOnDate

        Public Property ModifiedByUser As Integer Implements IReport.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IReport.ModifiedOnDate

        Public Property PortalId As Integer Implements IReport.PortalId

        Public Property ReportDescription As String Implements IReport.ReportDescription

        Public Property ReportId As Integer Implements IReport.ReportId

        Public Property ReportName As String Implements IReport.ReportName

        Public Property ReportSql As String Implements IReport.ReportSql

        Public Property ReportSqlDetail As String Implements IReport.ReportSqlDetail
    End Class
End Namespace
