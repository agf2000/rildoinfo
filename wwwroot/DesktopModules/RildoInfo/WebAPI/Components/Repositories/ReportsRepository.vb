Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
Imports System.Data.SqlClient

Namespace Components.Repositories

    Public Class ReportsRepository
        Implements IReportsRepository

        ''' <summary>
        ''' Gets the connection string from the web.config
        ''' </summary>
        ''' <returns></returns>
        Private Shared Function ConnectionString() As String
            Return ConfigurationManager.ConnectionStrings("SiteSqlServer").ConnectionString
            'return DotNetNuke.Data.DataProvider.Instance().GetConnectionStringBuilder().ConnectionString;
        End Function

        Public Function AddReport(report As Report) As Report Implements IReportsRepository.AddReport
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Report) = ctx.GetRepository(Of Report)()
                rep.Insert(report)
            End Using
            Return report
        End Function

        Public Function GetReport(reportId As Integer, portalId As Integer) As Report Implements IReportsRepository.GetReport
            Dim report As Report

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Report) = ctx.GetRepository(Of Report)()
                report = rep.GetById(Of Int32, Int32)(reportId, portalId)
            End Using
            Return report
        End Function

        Public Function GetReports(portalId As Integer) As IEnumerable(Of Report) Implements IReportsRepository.GetReports
            Dim reports As IEnumerable(Of Report)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Report) = ctx.GetRepository(Of Report)()
                reports = rep.Get(portalId)
            End Using
            Return reports
        End Function

        Public Sub RemoveReport(reportId As Integer, portalId As Integer) Implements IReportsRepository.RemoveReport
            Dim report As Report = GetReport(reportId, portalId)
            RemoveReport(report)
        End Sub

        Public Sub RemoveReport(report As Report) Implements IReportsRepository.RemoveReport
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Report) = ctx.GetRepository(Of Report)()
                rep.Delete(report)
            End Using
        End Sub

        Public Sub UpdateReport(report As Report) Implements IReportsRepository.UpdateReport
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Report) = ctx.GetRepository(Of Report)()
                rep.Update(report)
            End Using
        End Sub

        Public Function RunReport(sql As String) As IEnumerable(Of ReportInfo) Implements IReportsRepository.RunReport
            Return CBO.FillCollection(Of ReportInfo)(DataProvider.Instance().RunReport(sql))
        End Function

        'Public Function RunReport1(sql As String) As IEnumerable(Of EstimateProductsReportInfo) Implements IReportsRepository.RunReport1
        '    Return CBO.FillCollection(Of EstimateProductsReportInfo)(DataProvider.Instance().RunReport(sql))
        'End Function

        'Public Function RunReport2(sql As String) As IEnumerable(Of ClientListReportInfo) Implements IReportsRepository.RunReport2
        '    Return CBO.FillCollection(Of ClientListReportInfo)(DataProvider.Instance().RunReport(sql))
        'End Function

        'Public Function RunReport3(sql As String) As IEnumerable(Of ProductEstimatesReportInfo) Implements IReportsRepository.RunReport3
        '    Return CBO.FillCollection(Of ProductEstimatesReportInfo)(DataProvider.Instance().RunReport(sql))
        'End Function

        'Public Function RunReport4(sql As String) As IEnumerable(Of ProductListReportInfo) Implements IReportsRepository.RunReport4
        '    Return CBO.FillCollection(Of ProductListReportInfo)(DataProvider.Instance().RunReport(sql))
        'End Function

        'Public Function RunReport5(sql As String) As IEnumerable(Of PaymentManagerReportInfo) Implements IReportsRepository.RunReport5
        '    Return CBO.FillCollection(Of ProductListReportInfo)(DataProvider.Instance().RunReport(sql))
        'End Function
    End Class

End Namespace
