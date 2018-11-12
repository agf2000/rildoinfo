
Imports System.Net
Imports System.Net.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation
Imports RIW.Modules.WebAPI.Components.Repositories

Public Class OriginsController
    Inherits DnnApiController

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(OriginsController))

    ''' <summary>
    ''' Gets a list of origins by portal id
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetOrigins(portalId As Integer) As HttpResponseMessage
        Try
            Dim originsCtrl As New OriginsRepository

            Dim origins = originsCtrl.GetOrigins(portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, origins)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds ou updates an origin
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function UpdateOrigin(dto As Components.Models.Origin) As HttpResponseMessage
        Try
            Dim origin As New Components.Models.Origin
            Dim originCtrl As New OriginsRepository

            If dto.OriginId > 0 Then
                origin = originCtrl.GetOrigin(dto.OriginId, dto.PortalId)
            End If

            origin.OriginName = dto.OriginName
            origin.ModifiedByUser = dto.ModifiedByUser
            origin.ModifiedOnDate = dto.ModifiedOnDate

            If dto.OriginId > 0 Then
                originCtrl.UpdateOrigin(origin)
            Else
                origin.CreatedByUser = dto.CreatedByUser
                origin.CreatedOnDate = dto.CreatedOnDate
                originCtrl.AddOrigin(origin)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .Origin = origin})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes an origin
    ''' </summary>
    ''' <param name="originId">Origin ID</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpDelete> _
    Function RemoveOrigin(originId As Integer, portalId As Integer) As HttpResponseMessage
        Try
            Dim originCtrl As New OriginsRepository

            originCtrl.RemoveOrigin(originId, portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

End Class
