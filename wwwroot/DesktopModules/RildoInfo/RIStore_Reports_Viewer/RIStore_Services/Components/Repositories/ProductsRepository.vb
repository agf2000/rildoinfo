
Imports System.Collections.Generic
Imports DotNetNuke.Data
Imports System.Web.Script.Serialization
Imports DotNetNuke.Services.Localization

Namespace RI.Modules.RIStore_Services
    Public Class ProductsRepository
        Implements IProductsRepository

#Region "Private Methods"

        Private Shared Function GetNull(field As Object) As Object
            Return Null.GetNull(field, DBNull.Value)
        End Function

#End Region

        Public Sub updateProduct(product As Models.Product) Implements IProductsRepository.updateProduct
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Product) = ctx.GetRepository(Of Models.Product)()
                rep.Update(product)
            End Using
        End Sub

        Public Function getProducts(ByVal portalId As Integer, ByVal categoryId As Integer, ByVal lang As String, ByVal filter As String, ByVal getDeleted As Boolean, ByVal featuredOnly As Boolean, ByVal orderBy As String, ByVal orderDesc As Boolean, ByVal returnLimit As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal searchDesc As Boolean, ByVal isDealer As Boolean, ByVal categoryList As String, ByVal excludeFeatured As Boolean) As IEnumerable(Of Models.Product) Implements IProductsRepository.getProducts
            Return CBO.FillCollection(Of Models.Product)(DataProvider.Instance().GetProducts_List(portalId, categoryId, lang, filter, getDeleted, featuredOnly, orderBy, orderDesc, returnLimit, pageIndex, pageSize, searchDesc, isDealer, categoryList, excludeFeatured))
        End Function

        Public Function getProduct(productId As Integer, lang As String) As Models.Product Implements IProductsRepository.getProduct
            Return CBO.FillObject(Of Models.Product)(DataProvider.Instance().GetProducts(productId, lang))
        End Function

        Public Function getImages(pId As Integer) As Object Implements IProductsRepository.getImages
            Dim productImages As IEnumerable(Of Models.ProductImage)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductImage) = ctx.GetRepository(Of Models.ProductImage)()
                productImages = rep.Get(pId)
            End Using
            Return productImages
        End Function

        Public Function getProductLang(productId As Integer, lang As String) As Models.ProductLang Implements IProductsRepository.getProductLang
            Return CBO.FillObject(Of Models.ProductLang)(DataProvider.Instance().GetProductsLang(productId, lang))
        End Function

        Public Function getProductModels(ByVal portalId As Integer, ByVal productId As Integer, ByVal lang As String, ByVal isDealer As Boolean) As IEnumerable(Of Models.ProductModel) Implements IProductsRepository.getProductModels
            Return CBO.FillCollection(DataProvider.Instance().GetProductModels(portalId, productId, lang, isDealer), GetType(Models.ProductModel))
            'Return CBO.FillObject(Of Models.ProductModel)(DataProvider.Instance().GetProductsModel(ProductId, Lang))
        End Function

        Public Sub updateProductModel(productModel As Models.ProductModel) Implements IProductsRepository.updateProductModel
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductModel) = ctx.GetRepository(Of Models.ProductModel)()
                rep.Update(productModel)
            End Using
        End Sub

        Public Sub updateProductModelLang(productModelLang As Models.ProductModelLang) Implements IProductsRepository.updateProductModelLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductModelLang) = ctx.GetRepository(Of Models.ProductModelLang)()
                rep.Update(productModelLang)
            End Using
        End Sub

        Public Function addProductModel(productModel As Models.ProductModel) As Models.ProductModel Implements IProductsRepository.addProductModel
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductModel) = ctx.GetRepository(Of Models.ProductModel)()
                rep.Insert(productModel)
            End Using
            Return productModel
        End Function

        Public Function addProductModelLang(productModelLang As Models.ProductModelLang) As Models.ProductModelLang Implements IProductsRepository.addProductModelLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductModelLang) = ctx.GetRepository(Of Models.ProductModelLang)()
                rep.Insert(productModelLang)
            End Using
            Return productModelLang
        End Function

        Public Function addProduct(product As Models.Product) As Models.Product Implements IProductsRepository.addProduct
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Product) = ctx.GetRepository(Of Models.Product)()
                rep.Insert(product)
            End Using
            Return product
        End Function

        Public Function addProductLang(productLang As Models.ProductLang) As Models.ProductLang Implements IProductsRepository.addProductLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductLang) = ctx.GetRepository(Of Models.ProductLang)()
                rep.Insert(productLang)
            End Using
            Return productLang
        End Function

        Public Sub updateProductLang(productLang As Models.ProductLang) Implements IProductsRepository.updateProductLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductLang) = ctx.GetRepository(Of Models.ProductLang)()
                rep.Update(productLang)
            End Using
        End Sub

        Sub addProductCategories(productId As Integer, productCats As String) Implements IProductsRepository.addProductCategories
            Dim objCtrl As New ProductsRepository

            objCtrl.removeProductCategory(productId)

            Dim selectedCats = productCats.Split(","c)

            For Each catId In selectedCats
                If catId <> "" Then
                    objCtrl.addProductCategory(productId, Integer.Parse(catId))
                End If
            Next
        End Sub

        Public Sub removeProductCategory(productId As Integer) Implements IProductsRepository.removeProductCategory
            Dim productCategories As IEnumerable(Of Models.ProductCategory) = getProductCategories(productId)
            For Each category In productCategories
                removeProductCategory(category)
            Next
        End Sub

        Public Sub removeProductsCategory(categoryId As Integer) Implements IProductsRepository.removeProductsCategory
            Dim productsCategory As IEnumerable(Of Models.ProductCategory) = getProductsCategory(categoryId)
            For Each product In productsCategory
                removeProductCategory(product)
            Next
        End Sub

        Public Sub addProductCategory(productId As Integer, categoryId As Integer) Implements IProductsRepository.addProductCategory
            Dim productCategory As New Models.ProductCategory() With {.CategoryId = categoryId, .ProductId = productId}
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductCategory) = ctx.GetRepository(Of Models.ProductCategory)()
                rep.Insert(productCategory)
            End Using
        End Sub

        Public Sub removeProductCategory(productCategory As Models.ProductCategory) Implements IProductsRepository.removeProductCategory
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductCategory) = ctx.GetRepository(Of Models.ProductCategory)()
                rep.Delete(productCategory)
            End Using
        End Sub

        Public Function getProductsCategory(categoryId As Integer) As IEnumerable(Of Models.ProductCategory) Implements IProductsRepository.getProductsCategory
            Dim productsCategory As IEnumerable(Of Models.ProductCategory)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductCategory) = ctx.GetRepository(Of Models.ProductCategory)()
                productsCategory = rep.Get(categoryId)
            End Using
            Return productsCategory
        End Function

        Public Function getProductCategories(productId As Integer) As IEnumerable(Of Models.ProductCategory) Implements IProductsRepository.getProductCategories
            Dim productCategories As IEnumerable(Of Models.ProductCategory)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductCategory) = ctx.GetRepository(Of Models.ProductCategory)()
                productCategories = rep.Find("WHERE ProductId = @0", productId)
            End Using
            Return productCategories
        End Function

        Public Sub copyProductToLanguages(productLang As Models.ProductLang, Optional ForceOverwrite As Boolean = True) Implements IProductsRepository.copyProductToLanguages
            Dim supportedLanguages As LocaleCollection = Utilities.GetValidLocales()
            Dim originalLang As String = productLang.Lang
            For Each L As String In supportedLanguages
                If originalLang <> L Then
                    copyProductToLanguages(productLang, L, originalLang, ForceOverwrite)
                Else
                    Dim product As New Models.Product
                    product = getProduct(productLang.ProductId, productLang.Lang)
                    If Null.IsNull(product) Then
                        addProductLang(productLang)
                    Else
                        updateProductLang(productLang)
                    End If
                End If
            Next
        End Sub

        Public Sub copyProductToLanguages(productLang As Models.ProductLang, lang As String, originalLang As String, Optional forceOverwrite As Boolean = True) Implements IProductsRepository.copyProductToLanguages
            Dim blnDoCopy As Boolean = True
            Dim objDummy As Models.Product

            'check if Language exists
            If Not forceOverwrite Then
                objDummy = getProduct(productLang.ProductId, lang)
                If objDummy Is Nothing Then
                    blnDoCopy = True
                Else
                    blnDoCopy = False
                End If
            End If

            If blnDoCopy Then
                productLang.Lang = lang
                addProductLang(productLang)
            End If

            'Dim arylist As ArrayList

            ''copy models -----------------------------------
            'Dim objModelInfo As Models.ProductModel
            ''get new models
            'arylist = getProductModels(productLang.PortalId, productLang.ProductId, originalLang, True)
            'For Each objModelInfo In arylist
            '    CopyModelToLanguages(objModelInfo, lang, forceOverwrite)
            'Next
            '------------------------------

            ''copy options -----------------------------------
            'Dim objOptionInfo As NB_Store_OptionInfo
            ''get new options
            'Dim arylist2 As ArrayList
            'Dim objOptionVInfo As NB_Store_OptionValueInfo
            'arylist = GetOptionList(objInfo.ProductID, originalLang)
            'For Each objOptionInfo In arylist
            '    CopyOptionToLanguages(objOptionInfo, lang, forceOverwrite)

            '    'copy option values 
            '    arylist2 = GetOptionValueList(objOptionInfo.OptionID, originalLang)
            '    For Each objOptionVInfo In arylist2
            '        CopyOptionValueToLanguages(objOptionVInfo, lang, forceOverwrite)
            '    Next

            'Next
            ''------------------------------

            ''copy images ---------------------
            'Dim objPIInfo As NB_Store_ProductImageInfo
            'arylist = GetProductImageList(objInfo.ProductID, originalLang)
            'For Each objPIInfo In arylist
            '    CopyProductImageToLanguages(objPIInfo, lang, forceOverwrite)
            'Next
            ''------------------------------

            ''copy docs ---------------------
            'Dim objDInfo As NB_Store_ProductDocInfo
            'arylist = GetProductDocList(objInfo.ProductID, originalLang)
            'For Each objDInfo In arylist
            '    CopyProductDocToLanguages(objDInfo, lang, forceOverwrite)
            'Next
            ''------------------------------

        End Sub

        Public Sub CopyModelToLanguages(ByVal objInfo As Models.ProductModelLang, ByVal ForceOverwrite As Boolean) Implements IProductsRepository.copyModelToLanguages
            Dim supportedLanguages As LocaleCollection = Utilities.GetValidLocales()
            For Each L As String In supportedLanguages
                CopyModelToLanguages(objInfo, L, ForceOverwrite)
            Next
        End Sub

        Public Sub CopyModelToLanguages(ByVal objInfo As Models.ProductModelLang, ByVal Lang As String, ByVal ForceOverwrite As Boolean) Implements IProductsRepository.copyModelToLanguages
            Dim objDummy As Models.ProductModel
            Dim blnDoCopy As Boolean = True

            'check if Language exists
            If Not ForceOverwrite Then
                objDummy = getProductModel(objInfo.ModelId, Lang)
                If objDummy.Lang = "" Then
                    blnDoCopy = True
                Else
                    blnDoCopy = False
                End If
            End If

            objInfo.Lang = Lang

            If blnDoCopy Then
                addProductModelLang(objInfo)
            Else
                updateProductModelLang(objInfo)
            End If
        End Sub

        Public Function getProductModel(modelId As Integer, lang As String) As Models.ProductModel Implements IProductsRepository.getProductModel
            Return CBO.FillObject(Of Models.ProductModel)(DataProvider.Instance().GetProductModel(modelId, lang))
        End Function

        Public Function addProductOption(productOption As Models.ProductOption) As Models.ProductOption Implements IProductsRepository.addProductOption
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductOption) = ctx.GetRepository(Of Models.ProductOption)()
                rep.Insert(productOption)
            End Using
            Return productOption
        End Function

        Public Function addProductOptionLang(productOptionLang As Models.ProductOptionLang) As Models.ProductOptionLang Implements IProductsRepository.addProductOptionLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductOptionLang) = ctx.GetRepository(Of Models.ProductOptionLang)()
                rep.Insert(productOptionLang)
            End Using
            Return productOptionLang
        End Function

        Public Sub updateProductOption(productOption As Models.ProductOption) Implements IProductsRepository.updateProductOption
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductOption) = ctx.GetRepository(Of Models.ProductOption)()
                rep.Update(productOption)
            End Using
        End Sub

        Public Sub updateProductOptionLang(productOptionLang As Models.ProductOptionLang) Implements IProductsRepository.updateProductOptionLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductOptionLang) = ctx.GetRepository(Of Models.ProductOptionLang)()
                rep.Update(productOptionLang)
            End Using
        End Sub

        Public Function getProductOption(optionId As Integer, lang As String) As Models.ProductOption Implements IProductsRepository.getProductOption
            Return CBO.FillObject(Of Models.ProductOption)(DataProvider.Instance().GetProductOption(optionId, lang))
        End Function

        Public Function getProductOptions(productId As Integer, lang As String) As IEnumerable(Of Models.ProductOption) Implements IProductsRepository.getProductOptions
            Return CBO.FillCollection(DataProvider.Instance().GetProductOptions(productId, lang), GetType(Models.ProductOption))
        End Function

        Public Sub CopyOptionToLanguages(ByVal objInfo As Models.ProductOptionLang, ByVal Lang As String, ByVal ForceOverwrite As Boolean) Implements IProductsRepository.CopyOptionToLanguages
            Dim objDummy As Models.ProductOption
            Dim blnDoCopy As Boolean = True

            'check if Language exists
            If Not ForceOverwrite Then
                objDummy = getProductOption(objInfo.OptionId, Lang)
                If objDummy.Lang = "" Then
                    blnDoCopy = True
                Else
                    blnDoCopy = False
                End If
            End If

            objInfo.Lang = Lang

            If blnDoCopy Then
                addProductOptionLang(objInfo)
            Else
                updateProductOptionLang(objInfo)
            End If
        End Sub

        Public Function addProductOptionValue(productOptionValue As Models.ProductOptionValue) As Models.ProductOptionValue Implements IProductsRepository.addProductOptionValue
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductOptionValue) = ctx.GetRepository(Of Models.ProductOptionValue)()
                rep.Insert(productOptionValue)
            End Using
            Return productOptionValue
        End Function

        Public Function addProductOptionValueLang(productOptionValueLang As Models.ProductOptionValueLang) As Models.ProductOptionValueLang Implements IProductsRepository.addProductOptionValueLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductOptionValueLang) = ctx.GetRepository(Of Models.ProductOptionValueLang)()
                rep.Insert(productOptionValueLang)
            End Using
            Return productOptionValueLang
        End Function

        Public Sub CopyOptionValueToLanguages(objInfo As Models.ProductOptionValueLang, Lang As String, ForceOverwrite As Boolean) Implements IProductsRepository.CopyOptionValueToLanguages
            Dim objDummy As Models.ProductOptionValue
            Dim blnDoCopy As Boolean = True

            'check if Language exists
            If Not ForceOverwrite Then
                objDummy = getProductOptionValue(objInfo.OptionValueId, Lang)
                If objDummy.Lang = "" Then
                    blnDoCopy = True
                Else
                    blnDoCopy = False
                End If
            End If

            objInfo.Lang = Lang

            If blnDoCopy Then
                addProductOptionValueLang(objInfo)
            Else
                updateProductOptionValueLang(objInfo)
            End If
        End Sub

        Public Function getProductOptionValue(optionValueId As Integer, lang As String) As Models.ProductOptionValue Implements IProductsRepository.getProductOptionValue
            Return CBO.FillObject(Of Models.ProductOptionValue)(DataProvider.Instance().GetProductOptionValue(optionValueId, lang))
        End Function

        Public Function getProductOptionValues(optionId As Integer, lang As String) As IEnumerable(Of Models.ProductOptionValue) Implements IProductsRepository.getProductOptionValues
            Return CBO.FillCollection(DataProvider.Instance().GetProductOptionValues(optionId, lang), GetType(Models.ProductOptionValue))
        End Function

        Public Sub updateProductOptionValue(productOptionValue As Models.ProductOptionValue) Implements IProductsRepository.updateProductOptionValue
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductOptionValue) = ctx.GetRepository(Of Models.ProductOptionValue)()
                rep.Update(productOptionValue)
            End Using
        End Sub

        Public Sub updateProductOptionValueLang(productOptionValueLang As Models.ProductOptionValueLang) Implements IProductsRepository.updateProductOptionValueLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductOptionValueLang) = ctx.GetRepository(Of Models.ProductOptionValueLang)()
                rep.Update(productOptionValueLang)
            End Using
        End Sub

        Public Function addProductRelated(productRelated As Models.ProductRelated) As Models.ProductRelated Implements IProductsRepository.addProductRelated
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductRelated) = ctx.GetRepository(Of Models.ProductRelated)()
                rep.Insert(productRelated)
            End Using
            Return productRelated
        End Function

        Public Function getProductRelated(relatedId As Integer) As Models.ProductRelated Implements IProductsRepository.getProductRelated
            Return CBO.FillObject(Of Models.ProductRelated)(DataProvider.Instance().GetProductRelated(relatedId))
        End Function

        Public Function getProductsRelated(productId As Integer) As IEnumerable(Of Models.ProductRelated) Implements IProductsRepository.getProductsRelated
            Return CBO.FillCollection(DataProvider.Instance().GetProductsRelated(productId), GetType(Models.ProductRelated))
        End Function

        Public Sub updateProductRelated(productRelated As Models.ProductRelated) Implements IProductsRepository.updateProductRelated
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductRelated) = ctx.GetRepository(Of Models.ProductRelated)()
                rep.Update(productRelated)
            End Using
        End Sub

        Public Sub removeProductOptionValue(optionValueId As Integer, lang As String) Implements IProductsRepository.removeProductOptionValue
            Dim optionValue As Models.ProductOptionValue = getProductOptionValue(optionValueId, lang)
            removeProductOptionValue(optionValue)
        End Sub

        Public Sub removeProductOptionValue(optionValue As Models.ProductOptionValue) Implements IProductsRepository.removeProductOptionValue
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductOptionValue) = ctx.GetRepository(Of Models.ProductOptionValue)()
                rep.Delete(optionValue)
            End Using
        End Sub

        Public Sub removeProductOptionValueLang(optionValueId As Integer) Implements IProductsRepository.removeProductOptionValueLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep = ctx.GetRepository(Of Models.ProductOptionLang)()
                rep.Delete("WHERE OptionValueId = @0", optionValueId)
            End Using
        End Sub

        Public Sub removeProductOptionValueLang(optionValueLang As Models.ProductOptionValueLang) Implements IProductsRepository.removeProductOptionValueLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductOptionValueLang) = ctx.GetRepository(Of Models.ProductOptionValueLang)()
                rep.Delete(optionValueLang)
            End Using
        End Sub

        Public Sub removeProduct(productId As Integer, lang As String) Implements IProductsRepository.removeProduct
            Dim product As Models.Product = getProduct(productId, lang)
            removeProduct(product)
        End Sub

        Public Sub removeProduct(product As Models.Product) Implements IProductsRepository.removeProduct
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Product) = ctx.GetRepository(Of Models.Product)()
                rep.Delete(product)
            End Using
        End Sub

        Public Sub removeProductLang(productId As Integer) Implements IProductsRepository.removeProductLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep = ctx.GetRepository(Of Models.ProductLang)()
                rep.Delete("WHERE ProductId = @0", productId)
            End Using
        End Sub

        Public Sub removeProductOption(productOptionId As Integer, lang As String) Implements IProductsRepository.removeProductOption
            Dim productOption As Models.ProductOption = getProductOption(productOptionId, lang)
            removeProductOption(productOption)
        End Sub

        Public Sub removeProductOption(productOption As Models.ProductOption) Implements IProductsRepository.removeProductOption
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductOption) = ctx.GetRepository(Of Models.ProductOption)()
                rep.Delete(productOption)
            End Using
        End Sub

        Public Sub removeProductOptionLang(productOptionId As Integer) Implements IProductsRepository.removeProductOptionLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep = ctx.GetRepository(Of Models.ProductOptionLang)()
                rep.Delete("WHERE ProductOptionId = @0", productOptionId)
            End Using
        End Sub

        Public Sub removeProductOptionLang(productOptionLang As Models.ProductOptionLang) Implements IProductsRepository.removeProductOptionLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductOptionLang) = ctx.GetRepository(Of Models.ProductOptionLang)()
                rep.Delete(productOptionLang)
            End Using
        End Sub

        Public Sub removeRelatedProduct(relatedId As Integer) Implements IProductsRepository.removeRelatedProduct
            Dim relatedProduct As Models.ProductRelated = getProductRelated(relatedId)
            removeRelatedProduct(relatedProduct)
        End Sub

        Public Sub removeRelatedProduct(relatedProduct As Models.ProductRelated) Implements IProductsRepository.removeRelatedProduct
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ProductRelated) = ctx.GetRepository(Of Models.ProductRelated)()
                rep.Delete(relatedProduct)
            End Using
        End Sub

    End Class

End Namespace