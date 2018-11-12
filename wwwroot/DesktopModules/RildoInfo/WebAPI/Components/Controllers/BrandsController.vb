
Imports System.Net
Imports System.Net.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation
Imports RIW.Modules.WebAPI.Components.Repositories

Public Class BrandsController
    Inherits DnnApiController

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(BrandsController))

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
            Dim brandsData As IEnumerable(Of Components.Models.Brand)
            Dim brandsDataCtrl As New BrandsRepository

            brandsData = brandsDataCtrl.GetBrands(portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, brandsData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
            Dim brandData As New Components.Models.Brand
            Dim brandDataCtrl As New BrandsRepository

            brandData = brandDataCtrl.GetBrand(brandId, PortalController.Instance.GetCurrentPortalSettings().PortalId)

            Return Request.CreateResponse(HttpStatusCode.OK, brandData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates a brand
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function UpdateBrand(dto As Components.Models.Brand) As HttpResponseMessage
        Try
            Dim brand As New Components.Models.Brand
            Dim brandCtrl As New BrandsRepository

            If dto.BrandId > 0 Then
                brand = brandCtrl.GetBrand(dto.BrandId, dto.PortalId)
            End If

            brand.BrandCode = ""
            brand.BrandTitle = dto.BrandTitle
            brand.CreatedByUser = dto.CreatedByUser
            brand.CreatedOnDate = dto.CreatedOnDate
            brand.ModifiedByUser = dto.CreatedByUser
            brand.ModifiedOnDate = dto.ModifiedOnDate
            brand.IsDeleted = False
            brand.PortalId = dto.PortalId

            If dto.BrandId > 0 Then
                brandCtrl.UpdateBrand(brand)
            Else
                brandCtrl.AddBrand(brand)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .BrandId = brand.BrandId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
            brandCtrl.RemoveBrand(brandId, PortalController.Instance.GetCurrentPortalSettings().PortalId)

            Dim brandModelCtrl As New BrandModelsRepository
            brandModelCtrl.RemoveBrandModels(brandId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

End Class
