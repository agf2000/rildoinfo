
Namespace Components.Interfaces.Repositories

    Public Interface IReportsRepository

        ''' <summary>
        ''' Gets a list of reports by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetReports(portalId As Integer) As IEnumerable(Of Components.Models.Report)

        ''' <summary>
        ''' Gets a report by id
        ''' </summary>
        ''' <param name="reportId">Report ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetReport(reportId As Integer, portalId As Integer) As Components.Models.Report

        ''' <summary>
        ''' Updates a report info
        ''' </summary>
        ''' <param name="report">Report Model</param>
        ''' <remarks></remarks>
        Sub UpdateReport(report As Components.Models.Report)

        ''' <summary>
        ''' Adds a new report
        ''' </summary>
        ''' <param name="report">Report Model</param>
        ''' <remarks></remarks>
        Function AddReport(report As Components.Models.Report) As Components.Models.Report

        ''' <summary>
        ''' Removes a report by id
        ''' </summary>
        ''' <param name="reportId">Report ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <remarks></remarks>
        Sub RemoveReport(reportId As Integer, portalId As Integer)

        ''' <summary>
        ''' Removes a report
        ''' </summary>
        ''' <param name="report">Report Model</param>
        ''' <remarks></remarks>
        Sub RemoveReport(report As Components.Models.Report)

        ''' <summary>
        ''' Runs a report
        ''' </summary>
        ''' <param name="sql"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function RunReport(sql As String) As IEnumerable(Of Components.Models.ReportInfo)

        '''' <summary>
        '''' Runs a report
        '''' </summary>
        '''' <param name="sql"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Function RunReport1(sql As String) As IEnumerable(Of Components.Models.EstimateProductsReportInfo)

        '''' <summary>
        '''' Runs a report
        '''' </summary>
        '''' <param name="sql"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Function RunReport2(sql As String) As IEnumerable(Of Components.Models.ClientListReportInfo)

        '''' <summary>
        '''' Runs a report
        '''' </summary>
        '''' <param name="sql"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Function RunReport3(sql As String) As IEnumerable(Of Components.Models.ProductEstimatesReportInfo)

        '''' <summary>
        '''' Runs a report
        '''' </summary>
        '''' <param name="sql"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Function RunReport4(sql As String) As IEnumerable(Of Components.Models.ProductListReportInfo)

        '''' <summary>
        '''' Runs a report
        '''' </summary>
        '''' <param name="sql"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Function RunReport5(sql As String) As IEnumerable(Of Components.Models.PaymentManagerReportInfo)

    End Interface

End Namespace
