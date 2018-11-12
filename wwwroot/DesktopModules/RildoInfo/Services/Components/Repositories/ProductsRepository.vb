
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

    Public Function getProducts(portalId As Integer, categoryId As Integer, lang As String, filter As String, getArchived As Boolean, featuredOnly As Boolean, orderBy As String, orderDesc As String, returnLimit As String, pageIndex As Integer, pageSize As Integer, onSale As String, searchDesc As Boolean, isDealer As Boolean, getDeleted As String, categoryList As String, excludeFeatured As Boolean) As IEnumerable(Of Models.Product) Implements IProductsRepository.getProducts
        Return CBO.FillCollection(Of Models.Product)(DataProvider.Instance().GetProducts_List(portalId, categoryId, lang, filter, getArchived, featuredOnly, orderBy, orderDesc, returnLimit, pageIndex, pageSize, onSale, searchDesc, isDealer, getDeleted, categoryList, excludeFeatured))
    End Function

    Public Function getProduct(productId As Integer, lang As String) As Models.Product Implements IProductsRepository.getProduct
        Return CBO.FillObject(Of Models.Product)(DataProvider.Instance().GetProducts(productId, lang))
    End Function

    Public Function getProductImages(productId As Integer) As IEnumerable(Of Models.ProductImage) Implements IProductsRepository.getProductImages
        Dim productImages As IEnumerable(Of Models.ProductImage)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ProductImage) = ctx.GetRepository(Of Models.ProductImage)()
            productImages = rep.Get(productId)
        End Using
        Return productImages
    End Function

    Public Function getProductLang(productId As Integer, lang As String) As Models.ProductLang Implements IProductsRepository.getProductLang
        'Return CBO.FillObject(Of Models.ProductLang)(DataProvider.Instance().GetProductsLang(productId, lang))
        Dim productLang As Models.ProductLang

        Using ctx As IDataContext = DataContext.Instance()
            productLang = ctx.ExecuteSingleOrDefault(Of Models.ProductLang)(CommandType.Text, "WHERE ProductId = @0 AND Lang = @1", productId, lang)
        End Using
        Return productLang
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

    'Public Sub copyProductToLanguages(productLang As Models.ProductLang, Optional ForceOverwrite As Boolean = True) Implements IProductsRepository.copyProductToLanguages
    '    Dim supportedLanguages As LocaleCollection = Utilities.GetValidLocales()
    '    Dim originalLang As String = productLang.Lang
    '    For Each L As String In supportedLanguages
    '        If originalLang <> L Then
    '            copyProductToLanguages(productLang, L, originalLang, ForceOverwrite)
    '        Else
    '            Dim product As New Models.Product
    '            product = getProduct(productLang.ProductId, productLang.Lang)
    '            If Null.IsNull(product) Then
    '                addProductLang(productLang)
    '            Else
    '                updateProductLang(productLang)
    '            End If
    '        End If
    '    Next
    'End Sub

    'Public Sub copyProductToLanguages(productLang As Models.ProductLang, lang As String, originalLang As String, Optional forceOverwrite As Boolean = True) Implements IProductsRepository.copyProductToLanguages
    '    Dim blnDoCopy As Boolean = True
    '    Dim objDummy As Models.Product

    '    'check if Language exists
    '    If Not forceOverwrite Then
    '        objDummy = getProduct(productLang.ProductId, lang)
    '        If objDummy Is Nothing Then
    '            blnDoCopy = True
    '        Else
    '            blnDoCopy = False
    '        End If
    '    End If

    '    If blnDoCopy Then
    '        productLang.Lang = lang
    '        addProductLang(productLang)
    '    End If
    'End Sub

    Public Sub CopyProductToLanguages(objInfo As Models.ProductLang, Lang As String, ForceOverwrite As Boolean) Implements IProductsRepository.CopyProductToLanguages
        Dim objDummy As Models.ProductLang
        Dim blnDoCopy As Boolean = True

        'check if Language exists
        If Not ForceOverwrite Then
            objDummy = getProductLang(objInfo.ProductId, Lang)
            If Null.IsNull(objDummy) Then
                blnDoCopy = True
            Else
                blnDoCopy = False
            End If
        End If

        objInfo.Lang = Lang

        If blnDoCopy Then
            addProductLang(objInfo)
        Else
            updateProductLang(objInfo)
        End If
    End Sub

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
        Return CBO.FillCollection(Of Models.ProductOption)(DataProvider.Instance().GetProductOptions(productId, lang))
    End Function

    Public Sub CopyOptionToLanguages(objInfo As Models.ProductOptionLang, Lang As String, ForceOverwrite As Boolean) Implements IProductsRepository.CopyOptionToLanguages
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
        Return CBO.FillCollection(Of Models.ProductOptionValue)(DataProvider.Instance().GetProductOptionValues(optionId, lang))
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

    Public Function getProductsRelated(portalId As Integer, productId As Integer, lang As String, relatedType As Integer, getAll As Boolean) As IEnumerable(Of Models.ProductRelated) Implements IProductsRepository.getProductsRelated
        Return CBO.FillCollection(Of Models.ProductRelated)(DataProvider.Instance().GetProductsRelated(portalId, productId, lang, relatedType, getAll))
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

    Public Sub removeProductOptionValueLang(optionValueId As Integer, lang As String) Implements IProductsRepository.removeProductOptionValueLang
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep = ctx.GetRepository(Of Models.ProductOptionValueLang)()
            rep.Delete("WHERE OptionValueId = @0 AND Lang = @1", optionValueId, lang)
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

    Public Sub removeProductLangs(productId As Integer) Implements IProductsRepository.removeProductLang
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

    Public Sub removeProductOptionLang(optionId As Integer, lang As String) Implements IProductsRepository.removeProductOptionLang
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep = ctx.GetRepository(Of Models.ProductOptionLang)()
            rep.Delete("WHERE OptionId = @0 AND Lang = @1", optionId, lang)
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

    Public Sub removeProductsRelated(productId As Integer) Implements IProductsRepository.removeProductsRelated
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep = ctx.GetRepository(Of Models.ProductRelated)()
            rep.Delete("WHERE ProductId = @0", productId)
        End Using
    End Sub

    Public Sub addProductFinance(productFinance As Models.ProductFinance) Implements IProductsRepository.addProductFinance
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ProductFinance) = ctx.GetRepository(Of Models.ProductFinance)()
            rep.Insert(productFinance)
        End Using
    End Sub

    Public Sub removeProductFinance(productId As Integer) Implements IProductsRepository.removeProductFinance
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep = ctx.GetRepository(Of Models.ProductFinance)()
            rep.Delete("WHERE ProductId = @0", productId)
        End Using
    End Sub

    Public Function getProductFinance(productId As Integer) As Models.ProductFinance Implements IProductsRepository.getProductFinance
        Dim productFinance As Models.ProductFinance

        Using ctx As IDataContext = DataContext.Instance()
            productFinance = ctx.ExecuteSingleOrDefault(Of Models.ProductFinance)(CommandType.Text, "Where ProductId = @0", productId)
        End Using
        Return productFinance
    End Function

    Public Sub addProductImage(productImage As Models.ProductImage) Implements IProductsRepository.addProductImage
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ProductImage) = ctx.GetRepository(Of Models.ProductImage)()
            rep.Insert(productImage)
        End Using
    End Sub

    Public Function getProductImage(productImageId As Integer) As Models.ProductImage Implements IProductsRepository.getProductImage
        Dim productImage As Models.ProductImage

        Using ctx As IDataContext = DataContext.Instance()
            productImage = ctx.ExecuteSingleOrDefault(Of Models.ProductImage)(CommandType.Text, "Where ProductImageId = @0", productImageId)
        End Using
        Return productImage
    End Function

    Public Sub updateProductImage(productImage As Models.ProductImage) Implements IProductsRepository.updateProductImage
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ProductImage) = ctx.GetRepository(Of Models.ProductImage)()
            rep.Update(productImage)
        End Using
    End Sub

    Public Sub removeProductImage(productImageId As Integer) Implements IProductsRepository.removeProductImage
        Dim productImage As Models.ProductImage

        Using ctx As IDataContext = DataContext.Instance()
            productImage = ctx.ExecuteSingleOrDefault(Of Models.ProductImage)(CommandType.Text, "Where ProductImageId = @0", productImageId)
        End Using
        removeProductImage(productImage)
    End Sub

    Public Sub removeProductImage(productImage As Models.ProductImage) Implements IProductsRepository.removeProductImage
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ProductImage) = ctx.GetRepository(Of Models.ProductImage)()
            rep.Delete(productImage)
        End Using
    End Sub

    Public Sub updateProductFinance(productFinance As Models.ProductFinance) Implements IProductsRepository.updateProductFinance
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ProductFinance) = ctx.GetRepository(Of Models.ProductFinance)()
            rep.Update(productFinance)
        End Using
    End Sub

    Public Function addProductVideo(productVideo As Models.ProductVideo) As Models.ProductVideo Implements IProductsRepository.addProductVideo
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ProductVideo) = ctx.GetRepository(Of Models.ProductVideo)()
            rep.Insert(productVideo)
        End Using
        Return productVideo
    End Function

    Public Function getProductVideos(productId As Integer) As List(Of Models.ProductVideo) Implements IProductsRepository.getProductVideos
        Dim productVideos As IEnumerable(Of Models.ProductVideo)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ProductVideo) = ctx.GetRepository(Of Models.ProductVideo)()
            productVideos = rep.Find("WHERE ProductId = @0", productId)
        End Using
        Return productVideos
    End Function

    Public Sub removeProductVideos(productId As Integer) Implements IProductsRepository.removeProductVideos
        Dim productVideos As IEnumerable(Of Models.ProductVideo)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ProductVideo) = ctx.GetRepository(Of Models.ProductVideo)()
            productVideos = rep.Find("WHERE ProductId = @0", productId)
        End Using

        For Each video In productVideos
            removeProductVideo(video)
        Next
    End Sub

    Public Sub removeProductVideo(videoId As Integer) Implements IProductsRepository.removeProductVideo
        Dim productVideo As Models.ProductVideo

        Using ctx As IDataContext = DataContext.Instance()
            productVideo = ctx.ExecuteSingleOrDefault(Of Models.ProductVideo)(CommandType.Text, "Where VideoId = @0", videoId)
        End Using
        removeProductVideo(productVideo)
    End Sub

    Public Sub removeProductVideo(productVideo As Models.ProductVideo) Implements IProductsRepository.removeProductVideo
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ProductVideo) = ctx.GetRepository(Of Models.ProductVideo)()
            rep.Delete(productVideo)
        End Using
    End Sub

    Public Sub removeProductOptionValueLangs(optionId As Integer, lang As String) Implements IProductsRepository.removeProductOptionValueLangs
        Dim productOptionValueLangs As IEnumerable(Of Models.ProductOptionValueLang)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ProductOptionValueLang) = ctx.GetRepository(Of Models.ProductOptionValueLang)()
            productOptionValueLangs = rep.Find("WHERE OptionId = @0 AND Lang = @1", optionId, lang)
        End Using

        For Each optionValue In productOptionValueLangs
            removeProductOptionValueLang(optionValue.OptionValueId, optionValue.Lang)
        Next
    End Sub

    Public Sub removeProductOptionValues(optionId As Integer, lang As String) Implements IProductsRepository.removeProductOptionValues
        Dim productOptionValues As IEnumerable(Of Models.ProductOptionValue)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ProductOptionValue) = ctx.GetRepository(Of Models.ProductOptionValue)()
            productOptionValues = rep.Find("WHERE OptionId = @0 AND Lang = @1", optionId, lang)
        End Using

        For Each optionValue In productOptionValues
            removeProductOptionValue(optionValue.OptionValueId, optionValue.Lang)
        Next
    End Sub

End Class
