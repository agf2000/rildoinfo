
Imports System.Net
Imports System.Net.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation

Public Class BrandModelsController
    Inherits DnnApiController

    Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(AccountsController))

    ''' <summary>
    ''' Gets a list of models by brand id
    ''' </summary>
    ''' <param name="brandId">Brand ID</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpGet> _
    Function GetBrandModels(brandId As Integer, portalId As Integer) As HttpResponseMessage
        Try
            Dim brandModelsData As IEnumerable(Of Models.BrandModel)
            Dim brandModelsDataCtrl As New BrandModelsRepository

            brandModelsData = brandModelsDataCtrl.GetBrandModels(brandId)

            Return Request.CreateResponse(HttpStatusCode.OK, brandModelsData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets model by Model id
    ''' </summary>
    ''' <param name="modelId">Model ID</param>
    ''' <param name="brandId">Brand ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpGet> _
    Function GetBrandModel(modelId As Integer, brandId As Integer) As HttpResponseMessage
        Try
            Dim brandModelData As New Models.BrandModel
            Dim brandModelDataCtrl As New BrandModelsRepository

            brandModelData = brandModelDataCtrl.GetBrandModel(modelId, brandId)

            Return Request.CreateResponse(HttpStatusCode.OK, brandModelData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates a brand model
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function UpdateBrandModel(dto As Models.BrandModel) As HttpResponseMessage
        Try
            Dim brandModel As New Models.BrandModel
            Dim brandModelCtrl As New BrandModelsRepository

            If dto.ModelId > 0 Then
                brandModel = brandModelCtrl.GetBrandModel(dto.ModelId, dto.BrandId)
            End If

            brandModel.BrandId = dto.BrandId
            brandModel.ModelTitle = dto.ModelTitle
            brandModel.CreatedByUser = dto.CreatedByUser
            brandModel.CreatedOnDate = dto.CreatedOnDate
            brandModel.ModifiedByUser = dto.CreatedByUser
            brandModel.ModifiedOnDate = dto.CreatedOnDate
            brandModel.IsDeleted = False

            If dto.ModelId > 0 Then
                brandModelCtrl.UpdateBrandModel(brandModel)
            Else
                brandModelCtrl.AddBrandModel(brandModel)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .ModelId = brandModel.ModelId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes model
    ''' </summary>
    ''' <param name="modelId">Model ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpDelete> _
    Function RemoveBrandModel(modelId As Integer, brandId As Integer) As HttpResponseMessage
        Try
            Dim brandModelCtrl As New BrandModelsRepository

            brandModelCtrl.RemoveBrandModel(modelId, brandId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

End Class
