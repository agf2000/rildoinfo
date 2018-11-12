Imports RIW.Modules.WebAPI.Components.Models

Namespace Components.Interfaces.Repositories

    Public Interface IProductsRepository

        ''' <summary>
        ''' Gets products from product category
        ''' </summary>
        ''' <param name="categoryId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProductsCategory(categoryId As Integer) As IEnumerable(Of ProductCategory)

        ''' <summary>
        ''' Adds product
        ''' </summary>
        ''' <param name="product">Product Model</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddProduct(product As Product) As Product

        ''' <summary>
        ''' Updates product
        ''' </summary>
        ''' <param name="product">Product Model</param>
        ''' <remarks></remarks>
        Sub UpdateProduct(product As Product)

        ''' <summary>
        ''' Adds product
        ''' </summary>
        ''' <param name="productLang">Product Language Model</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddProductLang(productLang As ProductLang) As ProductLang

        ''' <summary>
        ''' Updates product
        ''' </summary>
        ''' <param name="ProductLang">Product Language Model</param>
        ''' <remarks></remarks>
        Sub UpdateProductLang(productLang As ProductLang)

        ''' <summary>
        ''' Gets list of products
        ''' </summary>
        ''' <param name="portalId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetDeletedProductsCount(portalId As Integer) As Integer

        ''' <summary>
        ''' Gets list of products
        ''' </summary>
        ''' <param name="portalId"></param>
        ''' <param name="categoryId"></param>
        ''' <param name="lang"></param>
        ''' <param name="filter"></param>
        ''' <param name="getArchived"></param>
        ''' <param name="featuredOnly"></param>
        ''' <param name="orderBy"></param>
        ''' <param name="orderDesc"></param>
        ''' <param name="returnLimit"></param>
        ''' <param name="pageIndex"></param>
        ''' <param name="pageSize"></param>
        ''' <param name="searchDesc"></param>
        ''' <param name="isDealer"></param>
        ''' <param name="getDeleted"></param>
        ''' <param name="onSale"></param>
        ''' <param name="providerList"></param>
        ''' <param name="categoryList"></param>
        ''' <param name="excludeFeatured"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProducts(portalId As Integer,
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
                             excludeFeatured As Boolean) As IEnumerable(Of Product)

        ''' <summary>
        ''' Gets list of products all
        ''' </summary>
        ''' <param name="portalId"></param>
        ''' <param name="lang"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProductsAll(portalId As Integer, lang As String) As IEnumerable(Of Product)

        ''' <summary>
        ''' Gets product
        ''' </summary>
        ''' <param name="productId">Product ID</param>
        ''' <param name="lang">Language</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProduct(productId As Integer, lang As String) As Product

        ''' <summary>
        ''' Gets product
        ''' </summary>
        ''' <param name="productName">Product Name</param>
        ''' <param name="lang">Language</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProduct(productName As String, lang As String) As ProductLang

        ''' <summary>
        ''' Gets product Language
        ''' </summary>
        ''' <param name="productId">Product ID</param>
        ''' <param name="lang">Language</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProductLang(productId As Integer, lang As String) As ProductLang

        ''' <summary>
        ''' Gets product images by product id
        ''' </summary>
        ''' <param name="productId">Product ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProductImages(productId As Integer) As IEnumerable(Of ProductImage)

        ''' <summary>
        ''' Adds product to categories
        ''' </summary>
        ''' <param name="product"></param>
        ''' <param name="productCats"></param>
        ''' <remarks></remarks>
        Sub AddProductCategories(product As Integer, productCats As String)

        ''' <summary>
        ''' Add product to category
        ''' </summary>
        ''' <param name="productId"></param>
        ''' <param name="categoryId"></param>
        ''' <remarks></remarks>
        Sub AddProductCategory(productId As Integer, categoryId As Integer)

        ''' <summary>
        ''' Get list of product categories
        ''' </summary>
        ''' <param name="productId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProductCategories(productId As Integer) As IEnumerable(Of ProductCategory)

        ''' <summary>
        ''' Gets product category
        ''' </summary>
        ''' <param name="productId"></param>
        ''' <param name="categoryId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProductCategory(productId As Integer, categoryId As Integer) As ProductCategory

        ' ''' <summary>
        ' ''' Copy product languages
        ' ''' </summary>
        ' ''' <param name="productLang"></param>
        ' ''' <param name="ForceOverwrite"></param>
        ' ''' <remarks></remarks>
        'Sub copyProductToLanguages(productLang As ProductLang, Optional forceOverwrite As Boolean = True)

        ' ''' <summary>
        ' ''' Copy product languages
        ' ''' </summary>
        ' ''' <param name="productLang"></param>
        ' ''' <param name="lang"></param>
        ' ''' <param name="originalLang"></param>
        ' ''' <param name="forceOverwrite"></param>
        ' ''' <remarks></remarks>
        'Sub copyProductToLanguages(productLang As ProductLang, lang As String, originalLang As String, Optional forceOverwrite As Boolean = True)

        ''' <summary>
        ''' Copies option model languages
        ''' </summary>
        ''' <param name="objInfo"></param>
        ''' <param name="lang"></param>
        ''' <param name="forceOverwrite"></param>
        ''' <remarks></remarks>
        Sub CopyProductToLanguages(objInfo As ProductLang, lang As String, forceOverwrite As Boolean)

        ''' <summary>
        ''' Updates product option
        ''' </summary>
        ''' <param name="productOption">Product Option</param>
        ''' <remarks></remarks>
        Sub UpdateProductOption(productOption As ProductOption)

        ''' <summary>
        ''' Updates product option language
        ''' </summary>
        ''' <param name="productOptionLang">Product Option Language</param>
        ''' <remarks></remarks>
        Sub UpdateProductOptionLang(productOptionLang As ProductOptionLang)

        ''' <summary>
        ''' Adds product option
        ''' </summary>
        ''' <param name="productOption"></param>
        ''' <remarks></remarks>
        Function AddProductOption(productOption As ProductOption) As ProductOption

        ''' <summary>
        ''' Adds product option language
        ''' </summary>
        ''' <param name="productOptionLang"></param>
        ''' <remarks></remarks>
        Function AddProductOptionLang(productOptionLang As ProductOptionLang) As ProductOptionLang

        ''' <summary>
        ''' Gets product option
        ''' </summary>
        ''' <param name="optionId">Option ID</param>
        ''' <param name="lang">Language</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProductOption(optionId As Integer, lang As String) As ProductOption

        ''' <summary>
        ''' Gets list product options
        ''' </summary>
        ''' <param name="productId"></param>
        ''' <param name="lang"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProductOptions(productId As Integer, lang As String) As IEnumerable(Of ProductOption)

        ''' <summary>
        ''' Copies option model languages
        ''' </summary>
        ''' <param name="objInfo"></param>
        ''' <param name="lang"></param>
        ''' <param name="forceOverwrite"></param>
        ''' <remarks></remarks>
        Sub CopyOptionToLanguages(objInfo As ProductOptionLang, lang As String, forceOverwrite As Boolean)

        ''' <summary>
        ''' Updates product option value
        ''' </summary>
        ''' <param name="productOptionValue">Product Option</param>
        ''' <remarks></remarks>
        Sub UpdateProductOptionValue(productOptionValue As ProductOptionValue)

        ''' <summary>
        ''' Updates product option value language
        ''' </summary>
        ''' <param name="productOptionValueLang">Product Option Value Language</param>
        ''' <remarks></remarks>
        Sub UpdateProductOptionValueLang(productOptionValueLang As ProductOptionValueLang)

        ''' <summary>
        ''' Adds product option value
        ''' </summary>
        ''' <param name="productOptionValue"></param>
        ''' <remarks></remarks>
        Function AddProductOptionValue(productOptionValue As ProductOptionValue) As ProductOptionValue

        ''' <summary>
        ''' Adds product option value language
        ''' </summary>
        ''' <param name="productOptionValueLang"></param>
        ''' <remarks></remarks>
        Function AddProductOptionValueLang(productOptionValueLang As ProductOptionValueLang) As ProductOptionValueLang

        ''' <summary>
        ''' Gets product option value
        ''' </summary>
        ''' <param name="optionValueId">Option Value ID</param>
        ''' <param name="lang">Language</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProductOptionValue(optionValueId As Integer, lang As String) As ProductOptionValue

        ''' <summary>
        ''' Gets list product option values
        ''' </summary>
        ''' <param name="optionId"></param>
        ''' <param name="lang"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProductOptionValues(optionId As Integer, lang As String) As IEnumerable(Of ProductOptionValue)

        ''' <summary>
        ''' Copies option value model languages
        ''' </summary>
        ''' <param name="objInfo"></param>
        ''' <param name="lang"></param>
        ''' <param name="forceOverwrite"></param>
        ''' <remarks></remarks>
        Sub CopyOptionValueToLanguages(objInfo As ProductOptionValueLang, lang As String, forceOverwrite As Boolean)

        ''' <summary>
        ''' Updates product related
        ''' </summary>
        ''' <param name="productRelated">Product Related</param>
        ''' <remarks></remarks>
        Sub UpdateProductRelated(productRelated As ProductRelated)

        ''' <summary>
        ''' Adds product related
        ''' </summary>
        ''' <param name="related"></param>
        ''' <remarks></remarks>
        Function AddProductRelated(related As ProductRelated) As ProductRelated

        ''' <summary>
        ''' Gets product related
        ''' </summary>
        ''' <param name="productRelatedId">Option Related ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProductRelated(productRelatedId As Integer) As ProductRelated

        ''' <summary>
        ''' Gets products related
        ''' </summary>
        ''' <param name="portalId"></param>
        ''' <param name="productId"></param>
        ''' <param name="lang"></param>
        ''' <param name="relatedType"></param>
        ''' <param name="getAll"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProductsRelated(portalId As Integer, productId As Integer, lang As String, relatedType As Integer, getAll As Boolean) As IEnumerable(Of ProductRelated)

        ''' <summary>
        ''' Gets product finance
        ''' </summary>
        ''' <param name="productId">Product ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProductFinance(productId As Integer) As ProductFinance

        Sub AddProductFinance(productFinance As ProductFinance)

        Sub RemoveProductFinance(productId As Integer)

        Sub RemoveProduct1(productId As Integer, lang As String)

        Sub RemoveProduct(product As Product)

        Sub RemoveProductLang(productId As Integer)

        Sub RemoveRelatedProduct1(relatedProductId As Integer)

        Sub RemoveRelatedProduct(relatedProduct As ProductRelated)

        Sub RemoveProductsRelated(productId As Integer)

        Sub RemoveProductOption1(productOptionId As Integer, lang As String)

        Sub RemoveProductOption(productOption As ProductOption)

        Sub RemoveProductOptionLang1(optionId As Integer, lang As String)

        Sub RemoveProductOptionLang(productOptionLang As ProductOptionLang)

        Sub RemoveProductOptionValue1(optionValueId As Integer, lang As String)

        Sub RemoveProductOptionValue(optionValue As ProductOptionValue)

        Sub RemoveProductOptionValueLang1(optionValueId As Integer, lang As String)

        Sub RemoveProductOptionValueLang(optionValueLang As ProductOptionValueLang)

        ''' <summary>
        ''' Remove product category
        ''' </summary>
        ''' <param name="productId"></param>
        ''' <param name="categoryId"></param>
        ''' <remarks></remarks>
        Sub RemoveProductCategory(productId As Integer, categoryId As Integer)

        ''' <summary>
        ''' Remove product category
        ''' </summary>
        ''' <param name="productId"></param>
        ''' <remarks></remarks>
        Sub RemoveProductCategories(productId As Integer)

        ''' <summary>
        ''' Remove product category
        ''' </summary>
        ''' <param name="categoryId"></param>
        ''' <remarks></remarks>
        Sub RemoveProductsCategory(categoryId As Integer)

        ''' <summary>
        ''' Removes product category
        ''' </summary>
        ''' <param name="productCategory"></param>
        ''' <remarks></remarks>
        Sub RemoveProductCategory(productCategory As ProductCategory)

        ''' <summary>
        ''' Removes all attribute's values
        ''' </summary>
        ''' <param name="optionId"></param>
        ''' <param name="lang"></param>
        ''' <remarks></remarks>
        Sub RemoveProductOptionValues(optionId As Integer, lang As String)

        ''' <summary>
        ''' Removes all attribute's values langs
        ''' </summary>
        ''' <param name="optionId"></param>
        ''' <param name="lang"></param>
        ''' <remarks></remarks>
        Sub RemoveProductOptionValueLangs(optionId As Integer, lang As String)

        ''' <summary>
        ''' Adds product image
        ''' </summary>
        ''' <param name="productImage"></param>
        ''' <remarks></remarks>
        Sub AddProductImage(productImage As ProductImage)

        ''' <summary>
        ''' Gets a productImage
        ''' </summary>
        ''' <param name="productImageId"></param>
        ''' <remarks></remarks>
        Function GetProductImage(productImageId As Integer) As ProductImage

        ''' <summary>
        ''' Updates image view order
        ''' </summary>
        ''' <param name="productImage"></param>
        ''' <remarks></remarks>
        Sub UpdateProductImage(productImage As ProductImage)

        ''' <summary>
        ''' Removes a product image
        ''' </summary>
        ''' <param name="productImageId"></param>
        ''' <remarks></remarks>
        Sub RemoveProductImage1(productImageId As Integer)

        ''' <summary>
        ''' Removes a product image
        ''' </summary>
        ''' <param name="productImage"></param>
        ''' <remarks></remarks>
        Sub RemoveProductImage(productImage As ProductImage)

        ''' <summary>
        ''' Updates product finance
        ''' </summary>
        ''' <param name="productFinance"></param>
        ''' <remarks></remarks>
        Sub UpdateProductFinance(productFinance As ProductFinance)

        ''' <summary>
        ''' Gets list of product videos
        ''' </summary>
        ''' <param name="productId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProductVideos(productId As Integer) As List(Of ProductVideo)

        ''' <summary>
        ''' Removes product video
        ''' </summary>
        ''' <param name="videoId"></param>
        ''' <remarks></remarks>
        Sub RemoveProductVideo1(videoId As Integer)

        ''' <summary>
        ''' Removes product video
        ''' </summary>
        ''' <param name="productId"></param>
        ''' <remarks></remarks>
        Sub RemoveProductVideos(productId As Integer)

        ''' <summary>
        ''' Removes product video
        ''' </summary>
        ''' <param name="productVideo"></param>
        ''' <remarks></remarks>
        Sub RemoveProductVideo(productVideo As ProductVideo)

        ''' <summary>
        ''' Adds product video
        ''' </summary>
        ''' <param name="productVideo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddProductVideo(productVideo As ProductVideo) As ProductVideo

        ''' <summary>
        ''' Gets product history
        ''' </summary>
        ''' <param name="productId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProductHistory(productId As Integer) As IEnumerable(Of InvoiceItem)

        ''' <summary>
        ''' Gets vendors from product id
        ''' </summary>
        ''' <param name="productId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProductVendors(productId As Integer) As IEnumerable(Of ProductVendor)

        ''' <summary>
        ''' Gets product vendor
        ''' </summary>
        ''' <param name="productId"></param>
        ''' <param name="personId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProductVendor(productId As Integer, personId As Integer) As ProductVendor

        ''' <summary>
        ''' Adds product vendor
        ''' </summary>
        ''' <param name="productVendor"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddProductVendor(productVendor As ProductVendor) As ProductVendor

        ''' <summary>
        ''' Updates product vendor
        ''' </summary>
        ''' <param name="productVendor"></param>
        ''' <remarks></remarks>
        Sub UpdateProductVendor(productVendor As ProductVendor)

        ''' <summary>
        ''' Removes product vendor by id
        ''' </summary>
        ''' <param name="personId"></param>
        ''' <param name="productId"></param>
        ''' <remarks></remarks>
        Sub RemoveProductVendor(personId As Integer, productId As Integer)

        ''' <summary>
        ''' Removes product vendor
        ''' </summary>
        ''' <param name="productVendor"></param>
        ''' <remarks></remarks>
        Sub RemoveProductVendor(productVendor As ProductVendor)

    End Interface

End Namespace
