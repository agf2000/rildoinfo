Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models

Namespace Components.Repositories

    Public Class ProductsRepository
        Implements IProductsRepository

        '#Region "Private Methods"

        '    Private Shared Function GetNull(field As Object) As Object
        '        Return Null.GetNull(field, DBNull.Value)
        '    End Function

        '#End Region

        Public Sub UpdateProduct(product As Product) Implements IProductsRepository.UpdateProduct
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Product) = ctx.GetRepository(Of Product)()
                rep.Update(product)
            End Using
        End Sub

        Public Function GetDeletedProductsCount(portalId As Integer) As Integer Implements IProductsRepository.GetDeletedProductsCount
            Dim productsCount As IEnumerable(Of Product)

            Using ctx As IDataContext = DataContext.Instance()
                'products = ctx.ExecuteSingleOrDefault(Of ProductLang)(CommandType.Text, "WHERE PortalId = @0 AND IsDeleted = @1", portalId, True)
                productsCount = ctx.ExecuteQuery(Of Product)(CommandType.Text, "WHERE PortalId = @0 AND IsDeleted = @1", portalId, True)
            End Using

            'Using ctx As IDataContext = DataContext.Instance()
            '    Dim rep As IRepository(Of Product) = ctx.GetRepository(Of Product)()
            '    products = rep.Get(portalId)
            'End Using
            Return productsCount.Count
        End Function

        Public Function GetProducts(portalId As Integer,
                                    categoryId As Integer,
                                    lang As String,
                                    searchField As String,
                                    searchString As String,
                                    getArchived As Boolean,
                                    featuredOnly As Boolean,
                                    orderBy As String,
                                    orderDesc As String,
                                    returnLimit As String,
                                    pageIndex As Integer,
                                    pageSize As Integer,
                                    onSale As String,
                                    searchDesc As Boolean,
                                    isDealer As Boolean,
                                    getDeleted As String,
                                    providerList As String,
                                    sDate As String,
                                    eDate As String,
                                    filterDate As String,
                                    categoryList As String,
                                    excludeFeatured As Boolean) As IEnumerable(Of Product) Implements IProductsRepository.GetProducts
            Return CBO.FillCollection(Of Product)(DataProvider.Instance().GetProducts_List(portalId, categoryId, lang, searchField, searchString, getArchived,
                                                                                           featuredOnly, orderBy, orderDesc, returnLimit, pageIndex, pageSize,
                                                                                           onSale, searchDesc, isDealer, getDeleted, providerList, sDate, eDate,
                                                                                           filterDate, categoryList, excludeFeatured))
        End Function

        Public Function GetProductsAll(portalId As Integer, lang As String) As IEnumerable(Of Product) Implements IProductsRepository.GetProductsAll
            Return CBO.FillCollection(Of Product)(DataProvider.Instance().GetProducts_ListAll(portalId, lang))
        End Function

        Public Function GetProduct(productId As Integer, lang As String) As Product Implements IProductsRepository.GetProduct
            Return CBO.FillObject(Of Product)(DataProvider.Instance().GetProducts(productId, lang))
        End Function

        Public Function GetProduct(productName As String, lang As String) As ProductLang Implements IProductsRepository.GetProduct
            Dim product As ProductLang

            Using ctx As IDataContext = DataContext.Instance()
                product = ctx.ExecuteSingleOrDefault(Of ProductLang)(CommandType.Text, "WHERE ProductName = @0 AND Lang = @1", productName, lang)
            End Using
            Return product
        End Function

        Public Function GetProductImages(productId As Integer) As IEnumerable(Of ProductImage) Implements IProductsRepository.GetProductImages
            Dim productImages As IEnumerable(Of ProductImage)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductImage) = ctx.GetRepository(Of ProductImage)()
                productImages = rep.Get(productId)
            End Using
            Return productImages
        End Function

        Public Function GetProductLang(productId As Integer, lang As String) As ProductLang Implements IProductsRepository.GetProductLang
            'Return CBO.FillObject(Of ProductLang)(DataProvider.Instance().GetProductsLang(productId, lang))
            Dim productLang As ProductLang

            Using ctx As IDataContext = DataContext.Instance()
                productLang = ctx.ExecuteSingleOrDefault(Of ProductLang)(CommandType.Text, "WHERE ProductId = @0 AND Lang = @1", productId, lang)
            End Using
            Return productLang
        End Function

        Public Function AddProduct(product As Product) As Product Implements IProductsRepository.AddProduct
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Product) = ctx.GetRepository(Of Product)()
                rep.Insert(product)
            End Using
            Return product
        End Function

        Public Function AddProductLang(productLang As ProductLang) As ProductLang Implements IProductsRepository.AddProductLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductLang) = ctx.GetRepository(Of ProductLang)()
                rep.Insert(productLang)
            End Using
            Return productLang
        End Function

        Public Sub UpdateProductLang(productLang As ProductLang) Implements IProductsRepository.UpdateProductLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductLang) = ctx.GetRepository(Of ProductLang)()
                rep.Update(productLang)
            End Using
        End Sub

        Sub AddProductCategories(productId As Integer, productCats As String) Implements IProductsRepository.AddProductCategories
            Dim objCtrl As New ProductsRepository

            objCtrl.RemoveProductCategories(productId)

            Dim selectedCats = productCats.Split(","c)

            For Each catId In selectedCats
                If catId <> "" Then
                    objCtrl.AddProductCategory(productId, Integer.Parse(catId))
                End If
            Next
        End Sub

        Public Sub RemoveProductCategories(productId As Integer) Implements IProductsRepository.RemoveProductCategories
            Dim productCategories As IEnumerable(Of ProductCategory) = GetProductCategories(productId)
            For Each category In productCategories
                RemoveProductCategory(category)
            Next
        End Sub

        Public Sub RemoveProductCategory(productId As Integer, categoryId As Integer) Implements IProductsRepository.RemoveProductCategory
            Dim productCategories As IEnumerable(Of ProductCategory) = GetProductCategories(productId)
            For Each category In productCategories
                RemoveProductCategory(category)
            Next
        End Sub

        Public Sub RemoveProductsCategory(categoryId As Integer) Implements IProductsRepository.RemoveProductsCategory
            Dim productsCategory As IEnumerable(Of ProductCategory) = GetProductsCategory(categoryId)
            For Each product In productsCategory
                RemoveProductCategory(product)
            Next
        End Sub

        Public Sub AddProductCategory(productId As Integer, categoryId As Integer) Implements IProductsRepository.AddProductCategory
            Dim productCategory As New ProductCategory() With {.CategoryId = categoryId, .ProductId = productId}
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductCategory) = ctx.GetRepository(Of ProductCategory)()
                rep.Insert(productCategory)
            End Using
        End Sub

        Public Sub RemoveProductCategory(productCategory As ProductCategory) Implements IProductsRepository.RemoveProductCategory
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductCategory) = ctx.GetRepository(Of ProductCategory)()
                rep.Delete(productCategory)
            End Using
        End Sub

        Public Function GetProductsCategory(categoryId As Integer) As IEnumerable(Of ProductCategory) Implements IProductsRepository.GetProductsCategory
            Dim productsCategory As IEnumerable(Of ProductCategory)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductCategory) = ctx.GetRepository(Of ProductCategory)()
                productsCategory = rep.Get(categoryId)
            End Using
            Return productsCategory
        End Function

        Public Function GetProductCategories(productId As Integer) As IEnumerable(Of ProductCategory) Implements IProductsRepository.GetProductCategories
            Dim productCategories As IEnumerable(Of ProductCategory)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductCategory) = ctx.GetRepository(Of ProductCategory)()
                productCategories = rep.Find("WHERE ProductId = @0", productId)
            End Using
            Return productCategories
        End Function

        Public Function GetProductCategory(productId As Integer, categoryId As Integer) As ProductCategory Implements IProductsRepository.GetProductCategory
            Dim productCategory As ProductCategory

            Using ctx As IDataContext = DataContext.Instance()
                productCategory = ctx.ExecuteSingleOrDefault(Of ProductCategory)(CommandType.Text, "where productId = @0 and categoryId = @1", productId, categoryId)
            End Using
            Return productCategory
        End Function

        'Public Sub copyProductToLanguages(productLang As ProductLang, Optional ForceOverwrite As Boolean = True) Implements IProductsRepository.copyProductToLanguages
        '    Dim supportedLanguages As LocaleCollection = Utilities.GetValidLocales()
        '    Dim originalLang As String = productLang.Lang
        '    For Each L As String In supportedLanguages
        '        If originalLang <> L Then
        '            copyProductToLanguages(productLang, L, originalLang, ForceOverwrite)
        '        Else
        '            Dim product As New Product
        '            product = getProduct(productLang.ProductId, productLang.Lang)
        '            If Null.IsNull(product) Then
        '                addProductLang(productLang)
        '            Else
        '                updateProductLang(productLang)
        '            End If
        '        End If
        '    Next
        'End Sub

        'Public Sub copyProductToLanguages(productLang As ProductLang, lang As String, originalLang As String, Optional forceOverwrite As Boolean = True) Implements IProductsRepository.copyProductToLanguages
        '    Dim blnDoCopy As Boolean = True
        '    Dim objDummy As Product

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

        Public Sub CopyProductToLanguages(objInfo As ProductLang, lang As String, forceOverwrite As Boolean) Implements IProductsRepository.CopyProductToLanguages
            Dim objDummy As ProductLang
            Dim blnDoCopy As Boolean = True

            'check if Language exists
            If Not forceOverwrite Then
                objDummy = GetProductLang(objInfo.ProductId, lang)
                If Null.IsNull(objDummy) Then
                    blnDoCopy = True
                Else
                    blnDoCopy = False
                End If
            End If

            objInfo.Lang = lang

            If blnDoCopy Then
                AddProductLang(objInfo)
            Else
                UpdateProductLang(objInfo)
            End If
        End Sub

        Public Function AddProductOption(productOption As ProductOption) As ProductOption Implements IProductsRepository.AddProductOption
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductOption) = ctx.GetRepository(Of ProductOption)()
                rep.Insert(productOption)
            End Using
            Return productOption
        End Function

        Public Function AddProductOptionLang(productOptionLang As ProductOptionLang) As ProductOptionLang Implements IProductsRepository.AddProductOptionLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductOptionLang) = ctx.GetRepository(Of ProductOptionLang)()
                rep.Insert(productOptionLang)
            End Using
            Return productOptionLang
        End Function

        Public Sub UpdateProductOption(productOption As ProductOption) Implements IProductsRepository.UpdateProductOption
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductOption) = ctx.GetRepository(Of ProductOption)()
                rep.Update(productOption)
            End Using
        End Sub

        Public Sub UpdateProductOptionLang(productOptionLang As ProductOptionLang) Implements IProductsRepository.UpdateProductOptionLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductOptionLang) = ctx.GetRepository(Of ProductOptionLang)()
                rep.Update(productOptionLang)
            End Using
        End Sub

        Public Function GetProductOption(optionId As Integer, lang As String) As ProductOption Implements IProductsRepository.GetProductOption
            Return CBO.FillObject(Of ProductOption)(DataProvider.Instance().GetProductOption(optionId, lang))
        End Function

        Public Function GetProductOptions(productId As Integer, lang As String) As IEnumerable(Of ProductOption) Implements IProductsRepository.GetProductOptions
            Return CBO.FillCollection(Of ProductOption)(DataProvider.Instance().GetProductOptions(productId, lang))
        End Function

        Public Sub CopyOptionToLanguages(objInfo As ProductOptionLang, lang As String, forceOverwrite As Boolean) Implements IProductsRepository.CopyOptionToLanguages
            Dim objDummy As ProductOption
            Dim blnDoCopy As Boolean = True

            'check if Language exists
            If Not forceOverwrite Then
                objDummy = GetProductOption(objInfo.OptionId, lang)
                If objDummy.Lang = "" Then
                    blnDoCopy = True
                Else
                    blnDoCopy = False
                End If
            End If

            objInfo.Lang = lang

            If blnDoCopy Then
                AddProductOptionLang(objInfo)
            Else
                UpdateProductOptionLang(objInfo)
            End If
        End Sub

        Public Function AddProductOptionValue(productOptionValue As ProductOptionValue) As ProductOptionValue Implements IProductsRepository.AddProductOptionValue
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductOptionValue) = ctx.GetRepository(Of ProductOptionValue)()
                rep.Insert(productOptionValue)
            End Using
            Return productOptionValue
        End Function

        Public Function AddProductOptionValueLang(productOptionValueLang As ProductOptionValueLang) As ProductOptionValueLang Implements IProductsRepository.AddProductOptionValueLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductOptionValueLang) = ctx.GetRepository(Of ProductOptionValueLang)()
                rep.Insert(productOptionValueLang)
            End Using
            Return productOptionValueLang
        End Function

        Public Sub CopyOptionValueToLanguages(objInfo As ProductOptionValueLang, lang As String, forceOverwrite As Boolean) Implements IProductsRepository.CopyOptionValueToLanguages
            Dim objDummy As ProductOptionValue
            Dim blnDoCopy As Boolean = True

            'check if Language exists
            If Not forceOverwrite Then
                objDummy = GetProductOptionValue(objInfo.OptionValueId, lang)
                If objDummy.Lang = "" Then
                    blnDoCopy = True
                Else
                    blnDoCopy = False
                End If
            End If

            objInfo.Lang = lang

            If blnDoCopy Then
                AddProductOptionValueLang(objInfo)
            Else
                UpdateProductOptionValueLang(objInfo)
            End If
        End Sub

        Public Function GetProductOptionValue(optionValueId As Integer, lang As String) As ProductOptionValue Implements IProductsRepository.GetProductOptionValue
            Return CBO.FillObject(Of ProductOptionValue)(DataProvider.Instance().GetProductOptionValue(optionValueId, lang))
        End Function

        Public Function GetProductOptionValues(optionId As Integer, lang As String) As IEnumerable(Of ProductOptionValue) Implements IProductsRepository.GetProductOptionValues
            Return CBO.FillCollection(Of ProductOptionValue)(DataProvider.Instance().GetProductOptionValues(optionId, lang))
        End Function

        Public Sub UpdateProductOptionValue(productOptionValue As ProductOptionValue) Implements IProductsRepository.UpdateProductOptionValue
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductOptionValue) = ctx.GetRepository(Of ProductOptionValue)()
                rep.Update(productOptionValue)
            End Using
        End Sub

        Public Sub UpdateProductOptionValueLang(productOptionValueLang As ProductOptionValueLang) Implements IProductsRepository.UpdateProductOptionValueLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductOptionValueLang) = ctx.GetRepository(Of ProductOptionValueLang)()
                rep.Update(productOptionValueLang)
            End Using
        End Sub

        Public Function AddProductRelated(productRelated As ProductRelated) As ProductRelated Implements IProductsRepository.AddProductRelated
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductRelated) = ctx.GetRepository(Of ProductRelated)()
                rep.Insert(productRelated)
            End Using
            Return productRelated
        End Function

        Public Function GetProductRelated(relatedId As Integer) As ProductRelated Implements IProductsRepository.GetProductRelated
            Return CBO.FillObject(Of ProductRelated)(DataProvider.Instance().GetProductRelated(relatedId))
        End Function

        Public Function GetProductsRelated(portalId As Integer, productId As Integer, lang As String, relatedType As Integer, getAll As Boolean) As IEnumerable(Of ProductRelated) Implements IProductsRepository.GetProductsRelated
            Return CBO.FillCollection(Of ProductRelated)(DataProvider.Instance().GetProductsRelated(portalId, productId, lang, relatedType, getAll))
        End Function

        Public Sub UpdateProductRelated(productRelated As ProductRelated) Implements IProductsRepository.UpdateProductRelated
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductRelated) = ctx.GetRepository(Of ProductRelated)()
                rep.Update(productRelated)
            End Using
        End Sub

        Public Sub RemoveProductOptionValue1(optionValueId As Integer, lang As String) Implements IProductsRepository.RemoveProductOptionValue1
            Dim optionValue As ProductOptionValue = GetProductOptionValue(optionValueId, lang)
            RemoveProductOptionValue(optionValue)
        End Sub

        Public Sub RemoveProductOptionValue(optionValue As ProductOptionValue) Implements IProductsRepository.RemoveProductOptionValue
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductOptionValue) = ctx.GetRepository(Of ProductOptionValue)()
                rep.Delete(optionValue)
            End Using
        End Sub

        Public Sub RemoveProductOptionValueLang1(optionValueId As Integer, lang As String) Implements IProductsRepository.RemoveProductOptionValueLang1
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep = ctx.GetRepository(Of ProductOptionValueLang)()
                rep.Delete("WHERE OptionValueId = @0 AND Lang = @1", optionValueId, lang)
            End Using
        End Sub

        Public Sub RemoveProductOptionValueLang(optionValueLang As ProductOptionValueLang) Implements IProductsRepository.RemoveProductOptionValueLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductOptionValueLang) = ctx.GetRepository(Of ProductOptionValueLang)()
                rep.Delete(optionValueLang)
            End Using
        End Sub

        Public Sub RemoveProduct1(productId As Integer, lang As String) Implements IProductsRepository.RemoveProduct1
            Dim product As Product = GetProduct(productId, lang)
            RemoveProduct(product)
        End Sub

        Public Sub RemoveProduct(product As Product) Implements IProductsRepository.RemoveProduct
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Product) = ctx.GetRepository(Of Product)()
                rep.Delete(product)
            End Using
        End Sub

        Public Sub RemoveProductLangs(productId As Integer) Implements IProductsRepository.RemoveProductLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep = ctx.GetRepository(Of ProductLang)()
                rep.Delete("WHERE ProductId = @0", productId)
            End Using
        End Sub

        Public Sub RemoveProductOption1(productOptionId As Integer, lang As String) Implements IProductsRepository.RemoveProductOption1
            Dim productOption As ProductOption = GetProductOption(productOptionId, lang)
            RemoveProductOption(productOption)
        End Sub

        Public Sub RemoveProductOption(productOption As ProductOption) Implements IProductsRepository.RemoveProductOption
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductOption) = ctx.GetRepository(Of ProductOption)()
                rep.Delete(productOption)
            End Using
        End Sub

        Public Sub RemoveProductOptionLang1(optionId As Integer, lang As String) Implements IProductsRepository.RemoveProductOptionLang1
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep = ctx.GetRepository(Of ProductOptionLang)()
                rep.Delete("WHERE OptionId = @0 AND Lang = @1", optionId, lang)
            End Using
        End Sub

        Public Sub RemoveProductOptionLang(productOptionLang As ProductOptionLang) Implements IProductsRepository.RemoveProductOptionLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductOptionLang) = ctx.GetRepository(Of ProductOptionLang)()
                rep.Delete(productOptionLang)
            End Using
        End Sub

        Public Sub RemoveRelatedProduct1(relatedId As Integer) Implements IProductsRepository.RemoveRelatedProduct1
            Dim relatedProduct As ProductRelated = GetProductRelated(relatedId)
            RemoveRelatedProduct(relatedProduct)
        End Sub

        Public Sub RemoveRelatedProduct(relatedProduct As ProductRelated) Implements IProductsRepository.RemoveRelatedProduct
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductRelated) = ctx.GetRepository(Of ProductRelated)()
                rep.Delete(relatedProduct)
            End Using
        End Sub

        Public Sub RemoveProductsRelated(productId As Integer) Implements IProductsRepository.RemoveProductsRelated
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep = ctx.GetRepository(Of ProductRelated)()
                rep.Delete("WHERE ProductId = @0", productId)
            End Using
        End Sub

        Public Sub AddProductFinance(productFinance As ProductFinance) Implements IProductsRepository.AddProductFinance
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductFinance) = ctx.GetRepository(Of ProductFinance)()
                rep.Insert(productFinance)
            End Using
        End Sub

        Public Sub RemoveProductFinance(productId As Integer) Implements IProductsRepository.RemoveProductFinance
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep = ctx.GetRepository(Of ProductFinance)()
                rep.Delete("WHERE ProductId = @0", productId)
            End Using
        End Sub

        Public Function GetProductFinance(productId As Integer) As ProductFinance Implements IProductsRepository.GetProductFinance
            Dim productFinance As ProductFinance

            Using ctx As IDataContext = DataContext.Instance()
                productFinance = ctx.ExecuteSingleOrDefault(Of ProductFinance)(CommandType.Text, "Where ProductId = @0", productId)
            End Using
            Return productFinance
        End Function

        Public Sub AddProductImage(productImage As ProductImage) Implements IProductsRepository.AddProductImage
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductImage) = ctx.GetRepository(Of ProductImage)()
                rep.Insert(productImage)
            End Using
        End Sub

        Public Function GetProductImage(productImageId As Integer) As ProductImage Implements IProductsRepository.GetProductImage
            Dim productImage As ProductImage

            Using ctx As IDataContext = DataContext.Instance()
                productImage = ctx.ExecuteSingleOrDefault(Of ProductImage)(CommandType.Text, "Where ProductImageId = @0", productImageId)
            End Using
            Return productImage
        End Function

        Public Sub UpdateProductImage(productImage As ProductImage) Implements IProductsRepository.UpdateProductImage
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductImage) = ctx.GetRepository(Of ProductImage)()
                rep.Update(productImage)
            End Using
        End Sub

        Public Sub RemoveProductImage1(productImageId As Integer) Implements IProductsRepository.RemoveProductImage1
            Dim productImage As ProductImage

            Using ctx As IDataContext = DataContext.Instance()
                productImage = ctx.ExecuteSingleOrDefault(Of ProductImage)(CommandType.Text, "Where ProductImageId = @0", productImageId)
            End Using
            RemoveProductImage(productImage)
        End Sub

        Public Sub RemoveProductImage(productImage As ProductImage) Implements IProductsRepository.RemoveProductImage
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductImage) = ctx.GetRepository(Of ProductImage)()
                rep.Delete(productImage)
            End Using
        End Sub

        Public Sub UpdateProductFinance(productFinance As ProductFinance) Implements IProductsRepository.UpdateProductFinance
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductFinance) = ctx.GetRepository(Of ProductFinance)()
                rep.Update(productFinance)
            End Using
        End Sub

        Public Function AddProductVideo(productVideo As ProductVideo) As ProductVideo Implements IProductsRepository.AddProductVideo
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductVideo) = ctx.GetRepository(Of ProductVideo)()
                rep.Insert(productVideo)
            End Using
            Return productVideo
        End Function

        Public Function GetProductVideos(productId As Integer) As List(Of ProductVideo) Implements IProductsRepository.GetProductVideos
            Dim productVideos As IEnumerable(Of ProductVideo)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductVideo) = ctx.GetRepository(Of ProductVideo)()
                productVideos = rep.Find("WHERE ProductId = @0", productId)
            End Using
            Return productVideos
        End Function

        Public Sub RemoveProductVideos(productId As Integer) Implements IProductsRepository.RemoveProductVideos
            Dim productVideos As IEnumerable(Of ProductVideo)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductVideo) = ctx.GetRepository(Of ProductVideo)()
                productVideos = rep.Find("WHERE ProductId = @0", productId)
            End Using

            For Each video In productVideos
                RemoveProductVideo(video)
            Next
        End Sub

        Public Sub RemoveProductVideo1(videoId As Integer) Implements IProductsRepository.RemoveProductVideo1
            Dim productVideo As ProductVideo

            Using ctx As IDataContext = DataContext.Instance()
                productVideo = ctx.ExecuteSingleOrDefault(Of ProductVideo)(CommandType.Text, "Where VideoId = @0", videoId)
            End Using
            RemoveProductVideo(productVideo)
        End Sub

        Public Sub RemoveProductVideo(productVideo As ProductVideo) Implements IProductsRepository.RemoveProductVideo
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductVideo) = ctx.GetRepository(Of ProductVideo)()
                rep.Delete(productVideo)
            End Using
        End Sub

        Public Sub RemoveProductOptionValueLangs(optionId As Integer, lang As String) Implements IProductsRepository.RemoveProductOptionValueLangs
            Dim productOptionValueLangs As IEnumerable(Of ProductOptionValueLang)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductOptionValueLang) = ctx.GetRepository(Of ProductOptionValueLang)()
                productOptionValueLangs = rep.Find("WHERE OptionValueId = @0 AND Lang = @1", optionId, lang)
            End Using

            For Each optionValue In productOptionValueLangs
                RemoveProductOptionValueLang1(optionValue.OptionValueId, optionValue.Lang)
            Next
        End Sub

        Public Sub RemoveProductOptionValues(optionId As Integer, lang As String) Implements IProductsRepository.RemoveProductOptionValues
            Dim productOptionValues As IEnumerable(Of ProductOptionValue)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductOptionValue) = ctx.GetRepository(Of ProductOptionValue)()
                productOptionValues = rep.Find("WHERE OptionId = @0 AND Lang = @1", optionId, lang)
            End Using

            For Each optionValue In productOptionValues
                RemoveProductOptionValue1(optionValue.OptionValueId, optionValue.Lang)
            Next
        End Sub

        Public Function GetProductHistory(productId As Integer) As IEnumerable(Of InvoiceItem) Implements IProductsRepository.GetProductHistory
            Dim products As IEnumerable(Of InvoiceItem)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of InvoiceItem) = ctx.GetRepository(Of InvoiceItem)()
                products = rep.Find("WHERE ProductId = @0", productId)
            End Using
            Return products
        End Function

        Public Function AddProductVendor(productVendor As ProductVendor) As ProductVendor Implements IProductsRepository.AddProductVendor
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductVendor) = ctx.GetRepository(Of ProductVendor)()
                rep.Insert(productVendor)
            End Using
            Return productVendor
        End Function

        Public Function GetProductVendors(productId As Integer) As IEnumerable(Of ProductVendor) Implements IProductsRepository.GetProductVendors
            Dim productVendors As IEnumerable(Of ProductVendor)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductVendor) = ctx.GetRepository(Of ProductVendor)()
                productVendors = rep.Find("where productId = @0", productId)
            End Using
            Return productVendors
        End Function

        Public Sub RemoveProductVendor(personId As Integer, productId As Integer) Implements IProductsRepository.RemoveProductVendor
            Dim productVendor As ProductVendor

            Using ctx As IDataContext = DataContext.Instance()
                productVendor = ctx.ExecuteSingleOrDefault(Of ProductVendor)(CommandType.Text, "where personid = {0} and productId = @1", personId, productId)
            End Using
            RemoveProductVendor(productVendor)
        End Sub

        Public Sub RemoveProductVendor(productVendor As ProductVendor) Implements IProductsRepository.RemoveProductVendor
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductVendor) = ctx.GetRepository(Of ProductVendor)()
                rep.Delete(productVendor)
            End Using
        End Sub

        Public Sub UpdateProductVendor(productVendor As ProductVendor) Implements IProductsRepository.UpdateProductVendor
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ProductVendor) = ctx.GetRepository(Of ProductVendor)()
                rep.Update(productVendor)
            End Using
        End Sub

        Public Function GetProductVendor(productId As Integer, personId As Integer) As ProductVendor Implements IProductsRepository.GetProductVendor
            Dim productVendor As ProductVendor

            Using ctx As IDataContext = DataContext.Instance()
                productVendor = ctx.ExecuteSingleOrDefault(Of ProductVendor)(CommandType.Text, "where productId = @0 and personid = @1", productId, personId)
            End Using
            Return productVendor
        End Function
    End Class

End Namespace
