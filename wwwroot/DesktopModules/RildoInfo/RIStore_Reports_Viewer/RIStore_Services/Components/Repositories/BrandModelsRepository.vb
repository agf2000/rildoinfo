
Imports System.Collections.Generic
Imports DotNetNuke.Data

Namespace RI.Modules.RIStore_Services
    Public Class BrandModelsRepository
        Implements IBrandModelsRepository

        Public Function AddBrandModel(brandModel As Models.BrandModel) As Models.BrandModel Implements IBrandModelsRepository.AddBrandModel
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.BrandModel) = ctx.GetRepository(Of Models.BrandModel)()
                rep.Insert(brandModel)
            End Using
            Return brandModel
        End Function

        Public Function GetBrandModel(modelId As Integer, brandId As Integer) As Models.BrandModel Implements IBrandModelsRepository.GetBrandModel
            Dim brandModel As Models.BrandModel

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.BrandModel) = ctx.GetRepository(Of Models.BrandModel)()
                brandModel = rep.GetById(Of Int32, Int32)(modelId, brandId)
            End Using
            Return brandModel
        End Function

        Public Function GetBrandModels(brandId As Integer) As IEnumerable(Of Models.BrandModel) Implements IBrandModelsRepository.GetBrandModels
            Dim brandModels As IEnumerable(Of Models.BrandModel)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.BrandModel) = ctx.GetRepository(Of Models.BrandModel)()
                brandModels = rep.Get(brandId)
            End Using
            Return brandModels
        End Function

        Public Sub RemoveBrandModel(modelId As Integer, brandId As Integer) Implements IBrandModelsRepository.RemoveBrandModel
            Dim _item As Models.BrandModel = GetBrandModel(modelId, brandId)
            If _item Is Nothing Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveBrandModel(_item)
            End If
        End Sub

        Public Sub RemoveBrandModel(brandModel As Models.BrandModel) Implements IBrandModelsRepository.RemoveBrandModel
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.BrandModel) = ctx.GetRepository(Of Models.BrandModel)()
                rep.Delete(brandModel)
            End Using
        End Sub

        Public Sub UpdateBrandModel(brandModel As Models.BrandModel) Implements IBrandModelsRepository.UpdateBrandModel
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.BrandModel) = ctx.GetRepository(Of Models.BrandModel)()
                rep.Update(brandModel)
            End Using
        End Sub

    End Class
End Namespace