
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation

Namespace RI.Modules.RIStore_Services
    Public Class BrandsController
        Inherits DnnApiController

        Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(AccountsController))

        ''' <summary>
        ''' Gets a list of brands by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="isDeleted">Optional IsDeleted</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetBrands(portalId As Integer, Optional ByVal isDeleted As String = "") As HttpResponseMessage
            Try
                Dim brandsData As New List(Of Models.Brand)
                Dim brandsDataCtrl As New BrandsRepository

                brandsData = brandsDataCtrl.GetBrands(portalId)

                Return Request.CreateResponse(HttpStatusCode.OK, brandsData)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets list of models by brand id
        ''' </summary>
        ''' <param name="brandId">Brand ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpGet> _
        Function GetBrand(brandId As Integer) As HttpResponseMessage
            Try
                Dim brandData As New Models.Brand
                Dim brandDataCtrl As New BrandsRepository

                brandData = brandDataCtrl.GetBrand(brandId, PortalController.GetCurrentPortalSettings().PortalId)

                Return Request.CreateResponse(HttpStatusCode.OK, brandData)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Adds a new brand
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="brandId">Brand ID</param>
        ''' <param name="bTitle">Brand Title</param>
        ''' <param name="uId">Created By User User ID</param>
        ''' <param name="cd">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function UpdateBrand(ByVal portalId As Integer, ByVal bTitle As String, ByVal uId As Integer, ByVal cd As Date, Optional ByVal brandId As Integer = -1) As HttpResponseMessage
            Try
                Dim brand As New Models.Brand
                Dim brandCtrl As New BrandsRepository

                If brandId > 0 Then
                    brand = brandCtrl.GetBrand(brandId, portalId)
                End If

                brand.BrandCode = ""
                brand.BrandTitle = bTitle
                brand.CreatedByUser = uId
                brand.CreatedOnDate = cd
                brand.IsDeleted = False
                brand.PortalId = portalId

                If brandId > 0 Then
                    brandCtrl.UpdateBrand(brand)
                Else
                    brandCtrl.AddBrand(brand)
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .BrandId = brand.BrandId})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Removes brand
        ''' </summary>
        ''' <param name="brandId">Brand ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpDelete> _
        Function RemoveBrand(brandId As Integer) As HttpResponseMessage
            Try
                Dim brandCtrl As New BrandsRepository

                brandCtrl.RemoveBrand(brandId, PortalController.GetCurrentPortalSettings().PortalId)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

    End Class
End Namespace