
Imports System.Net
Imports System.Net.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation
Imports RIW.Modules.WebAPI.Components.Repositories

Public Class HtmlContentsController
    Inherits DnnApiController

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(HtmlContentsController))

    ''' <summary>
    ''' Gets list of html content
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetHtmlContentList(portalId As Integer) As HttpResponseMessage
        Try
            Dim contentListCtrl As New HtmlContentsRepository()
            Dim contentList = contentListCtrl.GetHtmlContentList(portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, contentList)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets html content
    ''' </summary>
    ''' <param name="portalId"></param>
    ''' <param name="contentId">Content ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetHtmlContent(portalId As Integer, contentId As Integer) As HttpResponseMessage
        Try
            Dim contentDataCtrl As New HtmlContentsRepository()
            Dim contentData = contentDataCtrl.GetHtmlContent(portalId, contentId)

            Return Request.CreateResponse(HttpStatusCode.OK, contentData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates html content
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function UpdateHtmlContent(dto As Components.Models.HtmlContent) As HttpResponseMessage
        Try
            Dim contentCtrl As New HtmlContentsRepository()
            Dim content As New Components.Models.HtmlContent()

            If dto.ContentId > 0 Then
                content = contentCtrl.GetHtmlContent(dto.PortalId, dto.ContentId)
            End If

            content.HtmlContent = dto.HtmlContent
            content.ContentTitle = dto.ContentTitle
            content.CreatedByUser = dto.CreatedByUser
            content.CreatedOnDate = dto.CreatedOnDate
            content.ModifiedByUser = dto.CreatedByUser
            content.ModifiedOnDate = dto.CreatedOnDate
            content.PortalId = dto.PortalId

            If dto.ContentId > 0 Then
                contentCtrl.UpdateHtmlContent(content)
            Else
                contentCtrl.AddHtmlContent(content)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .ContentId = content.ContentId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    <DnnAuthorize> _
    <HttpDelete> _
    Function RemoveHtmlContent(contentId As Integer, portalId As Integer) As HttpResponseMessage
        Try
            Dim contentCtrl As New HtmlContentsRepository()

            contentCtrl.RemoveHtmlContent(contentId, portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

End Class
