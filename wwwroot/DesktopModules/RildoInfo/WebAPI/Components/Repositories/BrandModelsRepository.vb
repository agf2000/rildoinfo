Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Repositories

    Public Class BrandModelsRepository
    Implements IBrandModelsRepository

        Public Function AddBrandModel(brandModel As BrandModel) As BrandModel Implements IBrandModelsRepository.AddBrandModel
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of BrandModel) = ctx.GetRepository(Of BrandModel)()
                rep.Insert(brandModel)
            End Using
            Return brandModel
        End Function

        Public Function GetBrandModel(modelId As Integer, brandId As Integer) As BrandModel Implements IBrandModelsRepository.GetBrandModel
            Dim brandModel As BrandModel

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of BrandModel) = ctx.GetRepository(Of BrandModel)()
                brandModel = rep.GetById(Of Int32, Int32)(modelId, brandId)
            End Using
            Return brandModel
        End Function

        Public Function GetBrandModels(brandId As Integer) As IEnumerable(Of BrandModel) Implements IBrandModelsRepository.GetBrandModels
            Dim brandModels As IEnumerable(Of BrandModel)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of BrandModel) = ctx.GetRepository(Of BrandModel)()
                brandModels = rep.Get(brandId)
            End Using
            Return brandModels
        End Function

        Public Sub RemoveBrandModels(brandId As Integer) Implements IBrandModelsRepository.RemoveBrandModels
            Dim brandModels As IEnumerable(Of BrandModel)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of BrandModel) = ctx.GetRepository(Of BrandModel)()
                brandModels = rep.Get(brandId)
            End Using

            For Each brandModel In brandModels
                RemoveBrandModel(brandModel)
            Next
        End Sub

        Public Sub RemoveBrandModel(modelId As Integer, brandId As Integer) Implements IBrandModelsRepository.RemoveBrandModel
            Dim item As BrandModel = GetBrandModel(modelId, brandId)
            If item Is Nothing Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveBrandModel(item)
            End If
        End Sub

        Public Sub RemoveBrandModel(brandModel As BrandModel) Implements IBrandModelsRepository.RemoveBrandModel
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of BrandModel) = ctx.GetRepository(Of BrandModel)()
                rep.Delete(brandModel)
            End Using
        End Sub

        Public Sub UpdateBrandModel(brandModel As BrandModel) Implements IBrandModelsRepository.UpdateBrandModel
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of BrandModel) = ctx.GetRepository(Of BrandModel)()
                rep.Update(brandModel)
            End Using
        End Sub

    End Class

End Namespace
