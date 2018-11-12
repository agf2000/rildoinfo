
Imports System.Net
Imports System.Net.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation
Imports RIW.Modules.WebAPI.Components.Repositories

Public Class DiscountGroupsController
    Inherits DnnApiController

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(DiscountGroupsController))

    ''' <summary>
    ''' Gets a list of discount groups by portal id
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function GetDiscountGroups(ByVal portalId As Integer) As HttpResponseMessage
        Try
            Dim discountGroupsCtrl As New DiscountGroupsRepository

            Dim discountGroups = discountGroupsCtrl.GetDiscountGroups(portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, discountGroups)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    <DnnAuthorize> _
    <HttpPost> _
    Function UpdateDiscountGroup(dto As Components.Models.DiscountGroup) As HttpResponseMessage
        Try
            Dim discountGroup As New Components.Models.DiscountGroup
            Dim discountGroupCtrl As New DiscountGroupsRepository

            If dto.DiscountGroupId > 0 Then
                discountGroup = discountGroupCtrl.GetDiscountGroup(dto.DiscountGroupId, dto.PortalId)
            End If

            discountGroup.DiscountTitle = dto.DiscountTitle

            If dto.DiscountGroupId > 0 Then
                discountGroupCtrl.UpdateDiscountGroup(discountGroup)
            Else
                discountGroupCtrl.AddDiscountGroup(discountGroup)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .DiscountGroup = discountGroup})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes an account
    ''' </summary>
    ''' <param name="discountGroupId">Discount Group ID</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpDelete> _
    Function RemoveDiscountGroup(discountGroupId As Integer, portalId As Integer) As HttpResponseMessage
        Try
            Dim discountGroupCtrl As New DiscountGroupsRepository

            discountGroupCtrl.RemoveDiscountGroup1(discountGroupId, portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

End Class
