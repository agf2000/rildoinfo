
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation

Namespace RI.Modules.RIStore_Services
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
        <DnnAuthorize> _
        <HttpGet> _
        Function GetBrandModels(brandId As Integer, portalId As Integer) As HttpResponseMessage
            Try
                Dim brandModelsData As New List(Of Models.BrandModel)
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
        <DnnAuthorize> _
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
        ''' Adds or updates a model
        ''' </summary>
        ''' <param name="modelId">Model Id</param>
        ''' <param name="mTitle">Model Title</param>
        ''' <param name="brandId">Brand ID</param>
        ''' <param name="uId">Created By User User ID</param>
        ''' <param name="cd">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize> _
        <HttpPost> _
        Function UpdateBrandModel(ByVal brandId As Integer, ByVal mTitle As String, ByVal uId As Integer, ByVal cd As Date, Optional ByVal modelId As Integer = -1) As HttpResponseMessage
            Try
                Dim brandModel As New Models.BrandModel
                Dim brandModelCtrl As New BrandModelsRepository

                If modelId > 0 Then
                    brandModel = brandModelCtrl.GetBrandModel(modelId, brandId)
                End If

                brandModel.BrandId = brandId
                brandModel.ModelTitle = mTitle
                brandModel.CreatedByUser = uId
                brandModel.CreatedOnDate = cd
                brandModel.IsDeleted = False

                If modelId > 0 Then
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
End Namespace