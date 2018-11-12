﻿Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IBrandModelsRepository

        ''' <summary>
        ''' Adds a new brandModel
        ''' </summary>
        ''' <param name="brandModel">BrandModel Model</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddBrandModel(brandModel As BrandModel) As BrandModel

        ''' <summary>
        ''' Gets a brandModel
        ''' </summary>
        ''' <param name="modelId">Model ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetBrandModel(modelId As Integer, brandId As Integer) As BrandModel

        ''' <summary>
        ''' Gets list of brandModels bt brand id
        ''' </summary>
        ''' <param name="brandId">Brand ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetBrandModels(brandId As Integer) As IEnumerable(Of BrandModel)

        ''' <summary>
        ''' Adds a new brandModel
        ''' </summary>
        ''' <param name="brandModel">BrandModel Model</param>
        ''' <remarks></remarks>
        Sub UpdateBrandModel(brandModel As BrandModel)

        ''' <summary>
        ''' Removes brandModel
        ''' </summary>
        ''' <param name="brandModelId">BrandModel ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <remarks></remarks>
        Sub RemoveBrandModel(brandModelId As Integer, portalId As Integer)

        ''' <summary>
        ''' Removes brandModel
        ''' </summary>
        ''' <param name="brandId">Brand ID</param>
        ''' <remarks></remarks>
        Sub RemoveBrandModels(brandId As Integer)

        ''' <summary>
        ''' Removes brandModel
        ''' </summary>
        ''' <param name="brandModel">BrandModel Model</param>
        ''' <remarks></remarks>
        Sub RemoveBrandModel(brandModel As BrandModel)

    End Interface

End Namespace