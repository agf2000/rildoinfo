
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Web.Api
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports RIW.Modules.WebAPI.Components.Repositories

Public Class IndustriesController
    Inherits DnnApiController

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(IndustriesController))

    ''' <summary>
    ''' Gets a list of industries by portal id
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetIndustries(portalId As Integer, Optional isDeleted As String = "") As HttpResponseMessage
        Try
            Dim industriesCtrl As New IndustriesRepository

            Dim industries = industriesCtrl.GetIndustries(portalId, isDeleted)

            Return Request.CreateResponse(HttpStatusCode.OK, industries)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds ou updates an industry
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function UpdateIndustry(dto As Components.Models.Industry) As HttpResponseMessage
        Try
            Dim industry As New Components.Models.Industry
            Dim industryCtrl As New IndustriesRepository

            If dto.IndustryId > 0 Then
                industry = industryCtrl.GetIndustry(dto.IndustryId, dto.PortalId)
            End If

            industry.PortalId = dto.PortalId
            industry.IndustryTitle = dto.IndustryTitle
            industry.IsDeleted = dto.IsDeleted
            industry.CreatedByUser = dto.CreatedByUser
            industry.CreatedOnDate = dto.CreatedOnDate
            industry.ModifiedByUser = dto.ModifiedByUser
            industry.ModifiedOnDate = dto.ModifiedOnDate

            If dto.IndustryId > 0 Then
                industryCtrl.UpdateIndustry(industry)
            Else
                industryCtrl.AddIndustry(industry)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .Industry = industry})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize> _
    <HttpDelete> _
    Function RemoveIndustry(ByVal industryId As Integer, portalId As Integer) As HttpResponseMessage
        Try
            Dim industryCtrl As New IndustriesRepository

            industryCtrl.RemoveIndustry(industryId, portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

End Class
