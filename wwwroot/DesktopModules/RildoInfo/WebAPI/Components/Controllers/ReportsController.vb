
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Web.Api
Imports System.Net
Imports System.Net.Http
Imports RIW.Modules.WebAPI.Components.Repositories
'Imports System.Web.Script.Serialization
Imports DotNetNuke.Security.Roles

Public Class ReportsController
    Inherits DnnApiController

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(ReportsController))

    ''' <summary>
    ''' Gets reports list
    ''' </summary>
    ''' <param name="portalId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpGet> _
    Public Function GetReports(Optional portalId As Integer = 0) As HttpResponseMessage
        Try
            Dim reportsCtrl As New ReportsRepository

            Dim reports = reportsCtrl.GetReports(portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, reports)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets report by id
    ''' </summary>
    ''' <param name="reportId"></param>
    ''' <param name="portalId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpGet> _
    Public Function GetReport(reportId As Integer, portalId As Integer) As HttpResponseMessage
        Try
            Dim reportsCtrl As New ReportsRepository

            Dim report = reportsCtrl.GetReport(reportId, portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, report)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Run report with optional parameters
    ''' </summary>
    ''' <param name="portalId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes")>
    <HttpGet>
    Public Function RunReport(reportId As Integer,
                              date1 As String,
                              date2 As String,
                              Optional dateField As String = "",
                              Optional detailId As Integer = 0,
                              Optional portalId As Integer = 0,
                              Optional personId As String = "",
                              Optional productId As String = "",
                              Optional statusIds As String = "",
                              Optional salesRepId As String = "",
                              Optional registerType As String = "",
                              Optional credit As String = "",
                              Optional done As String = "") As HttpResponseMessage
        Try
            Dim reportsCtrl As New ReportsRepository
            Dim objReport As New Components.Models.Report
            Dim objReportInfoList As New List(Of Components.Models.ReportInfo)
            Dim roleCtrl As New RoleController

            If reportId > 0 Then

                objReport = reportsCtrl.GetReport(reportId, portalId)

                Dim sqlCom = objReport.ReportSql.ToUpper()
                sqlCom = sqlCom.Replace("[PORTALID]", portalId.ToString())
                sqlCom = sqlCom.Replace("[DATEFIELD]", If(dateField IsNot Nothing, String.Format("{0}", dateField), ""))
                sqlCom = sqlCom.Replace("[DATE1]", String.Format("'{0}'", CDate(date1).ToString("yyyy-MM-dd")))
                sqlCom = sqlCom.Replace("[DATE2]", String.Format("'{0}'", CDate(date2).ToString("yyyy-MM-dd")))
                sqlCom = sqlCom.Replace("[CLIENT]", If(CInt(personId) > 0, String.Format(" AND ('{0}' = C.PERSONID)", personId), ""))
                sqlCom = sqlCom.Replace("[SALESPERSON]", If(CInt(salesRepId) > 0, String.Format(" AND ('{0}' = A.USERID)", salesRepId), ""))
                sqlCom = sqlCom.Replace("[PRODUCT]", If(CInt(productId) > 0, String.Format(" AND ('{0}' = P.PRODUCTID)", productId), ""))
                sqlCom = sqlCom.Replace("[STATUS]", statusIds) ' If(CInt(statusIds) > 0, String.Format(" AND ('{0}' = S.STATUSTITLE)", statusIds), ""))
                sqlCom = sqlCom.Replace("[REGISTERTYPE:1]", If(CInt(personId) > 0, String.Format(" AND (CLIENT.REGISTERTYPES LIKE '{0}%')", roleCtrl.GetRoleByName(portalId, "Clientes").RoleID.ToString()), ""))
                sqlCom = sqlCom.Replace("[REGISTERTYPE:2]", If(CInt(personId) > 0, String.Format(" AND (PROVIDER.REGISTERTYPES LIKE '{0}%')", roleCtrl.GetRoleByName(portalId, "Fornecedores").RoleID.ToString()), ""))
                sqlCom = sqlCom.Replace("[CREDIT]", If(credit IsNot Nothing, If(credit = "true", " AND (CREDIT > 0 AND DEBIT = 0)", " AND (DEBIT > 0 AND CREDIT = 0)"), ""))
                sqlCom = sqlCom.Replace("[DONE]", If(done IsNot Nothing, If(done = "true", " AND (DONE = 1)", " AND (DONE = 0)"), ""))

                objReportInfoList = reportsCtrl.RunReport(sqlCom)

                If objReport.ReportSqlDetail <> "" Then

                    For Each item In objReportInfoList

                        Dim sqlComDetail = objReport.ReportSqlDetail
                        sqlComDetail = sqlComDetail.Replace("[ID]".ToUpper(), item.Id.ToString())
                        sqlComDetail = sqlComDetail.Replace("[PORTALID]".ToUpper(), portalId.ToString())
                        sqlComDetail = sqlComDetail.Replace("[DATE1]".ToUpper(), String.Format("'{0}'", CDate(date1).ToString("yyyy-MM-dd")))
                        sqlComDetail = sqlComDetail.Replace("[DATE2]".ToUpper(), String.Format("'{0}'", CDate(date2).ToString("yyyy-MM-dd")))
                        sqlComDetail = sqlComDetail.Replace("[CLIENT]".ToUpper(), If(CInt(personId) > 0, String.Format(" and ('{0}' = C.PERSONID)", personId), ""))
                        sqlComDetail = sqlComDetail.Replace("[SALESPERSON]".ToUpper(), If(CInt(salesRepId) > 0, String.Format(" AND ('{0}' = A.USERID)", salesRepId), ""))
                        sqlComDetail = sqlComDetail.Replace("[PRODUCT]".ToUpper(), If(CInt(productId) > 0, String.Format(" AND ('{0}' = P.PRODUCTID)", productId), ""))
                        sqlComDetail = sqlComDetail.Replace("[STATUS]".ToUpper(), statusIds) ' If(CInt(statusIds) > 0, String.Format(" AND ('{0}' = S.STATUSTITLE)", statusIds), ""))
                        sqlComDetail = sqlComDetail.Replace("[REGISTERTYPE:1]".ToUpper(), If(CInt(personId) > 0, String.Format(" AND (REGISTERTYPES LIKE '{0}%')", roleCtrl.GetRoleByName(portalId, "Clientes").RoleID.ToString()), ""))
                        sqlComDetail = sqlComDetail.Replace("[REGISTERTYPE:2]".ToUpper(), If(CInt(personId) > 0, String.Format(" AND (PROVIDER.REGISTERTYPES LIKE '{0}%')", roleCtrl.GetRoleByName(portalId, "Fornecedores").RoleID.ToString()), ""))

                        Dim items = reportsCtrl.RunReport(sqlComDetail)

                        If items.Count > 0 Then
                            item.Items = items
                        End If

                    Next
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.data = objReportInfoList, .total = objReportInfoList.Count})
            End If

            Return Request.CreateResponse(HttpStatusCode.OK)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

End Class
