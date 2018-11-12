
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation

Namespace RI.Modules.RIStore_Services
    Public Class IndustriesController
        Inherits DnnApiController

        Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(IndustriesController))

        ''' <summary>
        ''' Gets a list of industries by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpGet> _
        Function GetIndustries(ByVal portalId As Integer) As HttpResponseMessage
            Try
                Dim industries As IEnumerable(Of Models.Industry)

                If portalId > 0 Then
                    Using context As IDataContext = DataContext.Instance()
                        Dim repository = context.GetRepository(Of Models.Industry)()
                        industries = repository.Get(portalId)
                    End Using

                    Return Request.CreateResponse(HttpStatusCode.OK, industries)
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Adds ou updates an industry
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="industryId">Industry ID</param>
        ''' <param name="industryTitle">Industry Title</param>
        ''' <param name="uId">Created By User User ID</param>
        ''' <param name="cd">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpPost> _
        Function UpdateIndustry(ByVal portalId As Integer, ByVal IndustryTitle As String, ByVal uId As Integer, ByVal cd As Date, Optional ByVal industryId As Integer = -1) As HttpResponseMessage
            Try
                Dim industry As New Models.Industry
                Dim industryCtrl As New IndustriesRepository

                If industryId > 0 Then
                    industry = industryCtrl.GetIndustry(industryId, portalId)
                End If

                industry.IndustryTitle = IndustryTitle
                industry.CreatedByUser = uId
                industry.CreatedOnDate = cd
                industry.ModifiedByUser = uId
                industry.ModifiedOnDate = cd

                If industryId > 0 Then
                    industryCtrl.UpdateIndustry(industry)
                Else
                    industryCtrl.AddIndustry(industry)
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .IndustryId = industry.IndustryId})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Removes an industry
        ''' </summary>
        ''' <param name="industryId">Industry ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpDelete> _
        Function RemoveIndustry(ByVal industryId As Integer, portalId As Integer) As HttpResponseMessage
            Try
                Dim industryCtrl As New IndustriesRepository
                industryCtrl.RemoveIndustry(industryId, portalId)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

    End Class
End Namespace