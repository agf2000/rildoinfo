Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IBrandsRepository

        ''' <summary>
        ''' Adds a new brand
        ''' </summary>
        ''' <param name="brand">Brand Model</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddBrand(brand As Brand) As Brand

        ''' <summary>
        ''' Gets a list of brands by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetBrands(portalId As Integer) As IEnumerable(Of Brand)

        ''' <summary>
        ''' Gets list of models by brand id
        ''' </summary>
        ''' <param name="brandId">Brand ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetBrand(brandId As Integer, portalId As Integer) As Brand

        ''' <summary>
        ''' Gets list of models by brand title
        ''' </summary>
        ''' <param name="brandTitle">Brand Title</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetBrand(brandTitle As String, portalId As Integer) As Brand

        ''' <summary>
        ''' Adds a new brand
        ''' </summary>
        ''' <param name="brand">Brand Model</param>
        ''' <remarks></remarks>
        Sub UpdateBrand(brand As Brand)

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
        Sub RemoveBrand(brand As Brand)

    End Interface

End Namespace
