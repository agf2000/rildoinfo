
Public Interface IProductsRepository

    ''' <summary>
    ''' Gets products from product category
    ''' </summary>
    ''' <param name="categoryId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getProductsCategory(categoryId As Integer) As IEnumerable(Of Models.ProductCategory)

    ''' <summary>
    ''' Adds product
    ''' </summary>
    ''' <param name="product">Product Model</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function addProduct(product As Models.Product) As Models.Product

    ''' <summary>
    ''' Updates product
    ''' </summary>
    ''' <param name="product">Product Model</param>
    ''' <remarks></remarks>
    Sub updateProduct(product As Models.Product)

    ''' <summary>
    ''' Adds product
    ''' </summary>
    ''' <param name="productLang">Product Language Model</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function addProductLang(productLang As Models.ProductLang) As Models.ProductLang

    ''' <summary>
    ''' Updates product
    ''' </summary>
    ''' <param name="ProductLang">Product Language Model</param>
    ''' <remarks></remarks>
    Sub updateProductLang(productLang As Models.ProductLang)

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
    ''' <param name="categoryList"></param>
    ''' <param name="excludeFeatured"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getProducts(portalId As Integer,
        categoryId As Integer,
        lang As String,
        filter As String,
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
        categoryList As String,
        excludeFeatured As Boolean) As IEnumerable(Of Models.Product)

    ''' <summary>
    ''' Gets product
    ''' </summary>
    ''' <param name="productId">Product ID</param>
    ''' <param name="lang">Language</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getProduct(productId As Integer, lang As String) As Models.Product

    ''' <summary>
    ''' Gets product Language
    ''' </summary>
    ''' <param name="productId">Product ID</param>
    ''' <param name="lang">Language</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getProductLang(productId As Integer, lang As String) As Models.ProductLang

    ''' <summary>
    ''' Gets product images by product id
    ''' </summary>
    ''' <param name="productId">Product ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getProductImages(productId As Integer) As IEnumerable(Of Models.ProductImage)

    ''' <summary>
    ''' Adds product to categories
    ''' </summary>
    ''' <param name="product"></param>
    ''' <param name="productCats"></param>
    ''' <remarks></remarks>
    Sub addProductCategories(product As Integer, productCats As String)

    ''' <summary>
    ''' Add product to category
    ''' </summary>
    ''' <param name="productId"></param>
    ''' <param name="categoryId"></param>
    ''' <remarks></remarks>
    Sub addProductCategory(productId As Integer, categoryId As Integer)

    ''' <summary>
    ''' Get list of product categories
    ''' </summary>
    ''' <param name="productId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getProductCategories(productId As Integer) As IEnumerable(Of Models.ProductCategory)

    ' ''' <summary>
    ' ''' Copy product languages
    ' ''' </summary>
    ' ''' <param name="productLang"></param>
    ' ''' <param name="ForceOverwrite"></param>
    ' ''' <remarks></remarks>
    'Sub copyProductToLanguages(productLang As Models.ProductLang, Optional forceOverwrite As Boolean = True)

    ' ''' <summary>
    ' ''' Copy product languages
    ' ''' </summary>
    ' ''' <param name="productLang"></param>
    ' ''' <param name="lang"></param>
    ' ''' <param name="originalLang"></param>
    ' ''' <param name="forceOverwrite"></param>
    ' ''' <remarks></remarks>
    'Sub copyProductToLanguages(productLang As Models.ProductLang, lang As String, originalLang As String, Optional forceOverwrite As Boolean = True)

    ''' <summary>
    ''' Copies option model languages
    ''' </summary>
    ''' <param name="objInfo"></param>
    ''' <param name="Lang"></param>
    ''' <param name="ForceOverwrite"></param>
    ''' <remarks></remarks>
    Sub CopyProductToLanguages(objInfo As Models.ProductLang, Lang As String, ForceOverwrite As Boolean)

    ''' <summary>
    ''' Updates product option
    ''' </summary>
    ''' <param name="productOption">Product Option</param>
    ''' <remarks></remarks>
    Sub updateProductOption(productOption As Models.ProductOption)

    ''' <summary>
    ''' Updates product option language
    ''' </summary>
    ''' <param name="productOptionLang">Product Option Language</param>
    ''' <remarks></remarks>
    Sub updateProductOptionLang(productOptionLang As Models.ProductOptionLang)

    ''' <summary>
    ''' Adds product option
    ''' </summary>
    ''' <param name="productOption"></param>
    ''' <remarks></remarks>
    Function addProductOption(productOption As Models.ProductOption) As Models.ProductOption

    ''' <summary>
    ''' Adds product option language
    ''' </summary>
    ''' <param name="productOptionLang"></param>
    ''' <remarks></remarks>
    Function addProductOptionLang(productOptionLang As Models.ProductOptionLang) As Models.ProductOptionLang

    ''' <summary>
    ''' Gets product option
    ''' </summary>
    ''' <param name="optionId">Option ID</param>
    ''' <param name="lang">Language</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getProductOption(optionId As Integer, lang As String) As Models.ProductOption

    ''' <summary>
    ''' Gets list product options
    ''' </summary>
    ''' <param name="productId"></param>
    ''' <param name="lang"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getProductOptions(productId As Integer, lang As String) As IEnumerable(Of Models.ProductOption)

    ''' <summary>
    ''' Copies option model languages
    ''' </summary>
    ''' <param name="objInfo"></param>
    ''' <param name="Lang"></param>
    ''' <param name="ForceOverwrite"></param>
    ''' <remarks></remarks>
    Sub CopyOptionToLanguages(objInfo As Models.ProductOptionLang, Lang As String, ForceOverwrite As Boolean)

    ''' <summary>
    ''' Updates product option value
    ''' </summary>
    ''' <param name="productOptionValue">Product Option</param>
    ''' <remarks></remarks>
    Sub updateProductOptionValue(productOptionValue As Models.ProductOptionValue)

    ''' <summary>
    ''' Updates product option value language
    ''' </summary>
    ''' <param name="productOptionValueLang">Product Option Value Language</param>
    ''' <remarks></remarks>
    Sub updateProductOptionValueLang(productOptionValueLang As Models.ProductOptionValueLang)

    ''' <summary>
    ''' Adds product option value
    ''' </summary>
    ''' <param name="productOptionValue"></param>
    ''' <remarks></remarks>
    Function addProductOptionValue(productOptionValue As Models.ProductOptionValue) As Models.ProductOptionValue

    ''' <summary>
    ''' Adds product option value language
    ''' </summary>
    ''' <param name="productOptionValueLang"></param>
    ''' <remarks></remarks>
    Function addProductOptionValueLang(productOptionValueLang As Models.ProductOptionValueLang) As Models.ProductOptionValueLang

    ''' <summary>
    ''' Gets product option value
    ''' </summary>
    ''' <param name="optionValueId">Option Value ID</param>
    ''' <param name="lang">Language</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getProductOptionValue(optionValueId As Integer, lang As String) As Models.ProductOptionValue

    ''' <summary>
    ''' Gets list product option values
    ''' </summary>
    ''' <param name="optionId"></param>
    ''' <param name="lang"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getProductOptionValues(optionId As Integer, lang As String) As IEnumerable(Of Models.ProductOptionValue)

    ''' <summary>
    ''' Copies option value model languages
    ''' </summary>
    ''' <param name="objInfo"></param>
    ''' <param name="Lang"></param>
    ''' <param name="ForceOverwrite"></param>
    ''' <remarks></remarks>
    Sub CopyOptionValueToLanguages(objInfo As Models.ProductOptionValueLang, Lang As String, ForceOverwrite As Boolean)

    ''' <summary>
    ''' Updates product related
    ''' </summary>
    ''' <param name="productRelated">Product Related</param>
    ''' <remarks></remarks>
    Sub updateProductRelated(productRelated As Models.ProductRelated)

    ''' <summary>
    ''' Adds product related
    ''' </summary>
    ''' <param name="related"></param>
    ''' <remarks></remarks>
    Function addProductRelated(related As Models.ProductRelated) As Models.ProductRelated

    ''' <summary>
    ''' Gets product related
    ''' </summary>
    ''' <param name="productRelatedId">Option Related ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getProductRelated(productRelatedId As Integer) As Models.ProductRelated

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
    Function getProductsRelated(portalId As Integer, productId As Integer, lang As String, relatedType As Integer, getAll As Boolean) As IEnumerable(Of Models.ProductRelated)

    ''' <summary>
    ''' Gets product finance
    ''' </summary>
    ''' <param name="productId">Product ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getProductFinance(productId As Integer) As Models.ProductFinance

    Sub addProductFinance(productFinance As Models.ProductFinance)

    Sub removeProductFinance(productId As Integer)

    Sub removeProduct(productId As Integer, lang As String)

    Sub removeProduct(product As Models.Product)

    Sub removeProductLang(productId As Integer)

    Sub removeRelatedProduct(relatedProductId As Integer)

    Sub removeRelatedProduct(relatedProduct As Models.ProductRelated)

    Sub removeProductsRelated(productId As Integer)

    Sub removeProductOption(productOptionId As Integer, lang As String)

    Sub removeProductOption(productOption As Models.ProductOption)

    Sub removeProductOptionLang(optionId As Integer, lang As String)

    Sub removeProductOptionLang(productOptionLang As Models.ProductOptionLang)

    Sub removeProductOptionValue(optionValueId As Integer, lang As String)

    Sub removeProductOptionValue(optionValue As Models.ProductOptionValue)

    Sub removeProductOptionValueLang(optionValueId As Integer, lang As String)

    Sub removeProductOptionValueLang(optionValueLang As Models.ProductOptionValueLang)

    ''' <summary>
    ''' Remove product category
    ''' </summary>
    ''' <param name="productId"></param>
    ''' <remarks></remarks>
    Sub removeProductCategory(productId As Integer)

    ''' <summary>
    ''' Remove product category
    ''' </summary>
    ''' <param name="categoryId"></param>
    ''' <remarks></remarks>
    Sub removeProductsCategory(categoryId As Integer)

    ''' <summary>
    ''' Removes product category
    ''' </summary>
    ''' <param name="productCategory"></param>
    ''' <remarks></remarks>
    Sub removeProductCategory(productCategory As Models.ProductCategory)

    ''' <summary>
    ''' Removes all attribute's values
    ''' </summary>
    ''' <param name="optionId"></param>
    ''' <param name="lang"></param>
    ''' <remarks></remarks>
    Sub removeProductOptionValues(optionId As Integer, lang As String)

    ''' <summary>
    ''' Removes all attribute's values langs
    ''' </summary>
    ''' <param name="optionId"></param>
    ''' <param name="lang"></param>
    ''' <remarks></remarks>
    Sub removeProductOptionValueLangs(optionId As Integer, lang As String)

    ''' <summary>
    ''' Adds product image
    ''' </summary>
    ''' <param name="productImage"></param>
    ''' <remarks></remarks>
    Sub addProductImage(productImage As Models.ProductImage)

    ''' <summary>
    ''' Gets a productImage
    ''' </summary>
    ''' <param name="productImageId"></param>
    ''' <remarks></remarks>
    Function getProductImage(productImageId As Integer) As Models.ProductImage

    ''' <summary>
    ''' Updates image view order
    ''' </summary>
    ''' <param name="productImage"></param>
    ''' <remarks></remarks>
    Sub updateProductImage(productImage As Models.ProductImage)

    ''' <summary>
    ''' Removes a product image
    ''' </summary>
    ''' <param name="productImageId"></param>
    ''' <remarks></remarks>
    Sub removeProductImage(productImageId As Integer)

    ''' <summary>
    ''' Removes a product image
    ''' </summary>
    ''' <param name="productImage"></param>
    ''' <remarks></remarks>
    Sub removeProductImage(productImage As Models.ProductImage)

    ''' <summary>
    ''' Updates product finance
    ''' </summary>
    ''' <param name="productFinance"></param>
    ''' <remarks></remarks>
    Sub updateProductFinance(productFinance As Models.ProductFinance)

    ''' <summary>
    ''' Gets list of product videos
    ''' </summary>
    ''' <param name="productId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getProductVideos(productId As Integer) As List(Of Models.ProductVideo)

    ''' <summary>
    ''' Removes product video
    ''' </summary>
    ''' <param name="videoId"></param>
    ''' <remarks></remarks>
    Sub removeProductVideo(videoId As Integer)

    ''' <summary>
    ''' Removes product video
    ''' </summary>
    ''' <param name="productId"></param>
    ''' <remarks></remarks>
    Sub removeProductVideos(productId As Integer)

    ''' <summary>
    ''' Removes product video
    ''' </summary>
    ''' <param name="productVideo"></param>
    ''' <remarks></remarks>
    Sub removeProductVideo(productVideo As Models.ProductVideo)

    ''' <summary>
    ''' Adds product video
    ''' </summary>
    ''' <param name="productVideo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function addProductVideo(productVideo As Models.ProductVideo) As Models.ProductVideo

End Interface
