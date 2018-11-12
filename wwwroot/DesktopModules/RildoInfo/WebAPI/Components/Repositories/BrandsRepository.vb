Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Repositories

    Public Class BrandsRepository
    Implements IBrandsRepository

        Public Function AddBrand(brand As Brand) As Brand Implements IBrandsRepository.AddBrand
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Brand) = ctx.GetRepository(Of Brand)()
                rep.Insert(brand)
            End Using
            Return brand
        End Function

        Public Function GetBrand(brandId As Integer, portalId As Integer) As Brand Implements IBrandsRepository.GetBrand
            Dim brand As Brand

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Brand) = ctx.GetRepository(Of Brand)()
                brand = rep.GetById(Of Int32, Int32)(brandId, portalId)
            End Using
            Return brand
        End Function

        Public Function GetBrand(brandTitle As String, portalId As Integer) As Brand Implements IBrandsRepository.GetBrand
            Dim brand As Brand

            Using ctx As IDataContext = DataContext.Instance()
                brand = ctx.ExecuteSingleOrDefault(Of Brand)(CommandType.Text, "where BrandTitle = @0 and PortalId = @1", brandTitle, portalId)
            End Using
            Return brand
        End Function

        Public Function GetBrands(portalId As Integer) As IEnumerable(Of Brand) Implements IBrandsRepository.GetBrands
            Dim brands As IEnumerable(Of Brand)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Brand) = ctx.GetRepository(Of Brand)()
                brands = rep.Get(portalId)
            End Using
            Return brands
        End Function

        Public Sub RemoveBrand(brandId As Integer, portalId As Integer) Implements IBrandsRepository.RemoveBrand
            Dim item As Brand = GetBrand(brandId, portalId)
            If item Is Nothing Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveBrand(item)
            End If
        End Sub

        Public Sub RemoveBrand(brand As Brand) Implements IBrandsRepository.RemoveBrand
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Brand) = ctx.GetRepository(Of Brand)()
                rep.Delete(brand)
            End Using
        End Sub

        Public Sub UpdateBrand(brand As Brand) Implements IBrandsRepository.UpdateBrand
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Brand) = ctx.GetRepository(Of Brand)()
                rep.Update(brand)
            End Using
        End Sub
    End Class

End Namespace
