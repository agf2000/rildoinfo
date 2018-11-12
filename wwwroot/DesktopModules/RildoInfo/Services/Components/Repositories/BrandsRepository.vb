
Public Class BrandsRepository
    Implements IBrandsRepository

    Public Function AddBrand(brand As Models.Brand) As Models.Brand Implements IBrandsRepository.AddBrand
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Brand) = ctx.GetRepository(Of Models.Brand)()
            rep.Insert(brand)
        End Using
        Return brand
    End Function

    Public Function GetBrand(brandId As Integer, portalId As Integer) As Models.Brand Implements IBrandsRepository.GetBrand
        Dim brand As Models.Brand

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Brand) = ctx.GetRepository(Of Models.Brand)()
            brand = rep.GetById(Of Int32, Int32)(brandId, portalId)
        End Using
        Return brand
    End Function

    Public Function GetBrands(portalId As Integer) As IEnumerable(Of Models.Brand) Implements IBrandsRepository.GetBrands
        Dim brands As IEnumerable(Of Models.Brand)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Brand) = ctx.GetRepository(Of Models.Brand)()
            brands = rep.Get(portalId)
        End Using
        Return brands
    End Function

    Public Sub RemoveBrand(brandId As Integer, portalId As Integer) Implements IBrandsRepository.RemoveBrand
        Dim _item As Models.Brand = GetBrand(brandId, portalId)
        If _item Is Nothing Then
            Throw New ArgumentException("Exception Occured")
        Else
            RemoveBrand(_item)
        End If
    End Sub

    Public Sub RemoveBrand(brand As Models.Brand) Implements IBrandsRepository.RemoveBrand
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Brand) = ctx.GetRepository(Of Models.Brand)()
            rep.Delete(brand)
        End Using
    End Sub

    Public Sub UpdateBrand(brand As Models.Brand) Implements IBrandsRepository.UpdateBrand
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Brand) = ctx.GetRepository(Of Models.Brand)()
            rep.Update(brand)
        End Using
    End Sub
End Class
