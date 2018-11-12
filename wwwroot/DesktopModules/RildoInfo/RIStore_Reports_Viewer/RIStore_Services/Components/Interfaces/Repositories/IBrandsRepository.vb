
Namespace RI.Modules.RIStore_Services
    Public Interface IBrandsRepository

        ''' <summary>
        ''' Adds a new brand
        ''' </summary>
        ''' <param name="brand">Brand Model</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddBrand(brand As Models.Brand) As Models.Brand

        ''' <summary>
        ''' Gets a list of brands by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetBrands(portalId As Integer) As IEnumerable(Of Models.Brand)

        ''' <summary>
        ''' Gets list of models by brand id
        ''' </summary>
        ''' <param name="brandId">Brand ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetBrand(brandId As Integer, portalId As Integer) As Models.Brand

        ''' <summary>
        ''' Adds a new brand
        ''' </summary>
        ''' <param name="brand">Brand Model</param>
        ''' <remarks></remarks>
        Sub UpdateBrand(brand As Models.Brand)

        ''' <summary>
        ''' Removes brand
        ''' </summary>
        ''' <param name="brandId">Brand ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <remarks></remarks>
        Sub RemoveBrand(brandId As Integer, portalId As Integer)

        ''' <summary>
        ''' Removes brand
        ''' </summary>
        ''' <param name="brand">Brand Model</param>
        ''' <remarks></remarks>
        Sub RemoveBrand(brand As Models.Brand)

    End Interface
End Namespace