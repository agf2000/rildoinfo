
Namespace RI.Modules.RIStore_Services
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
        ''' <param name="getDeleted"></param>
        ''' <param name="featuredOnly"></param>
        ''' <param name="orderBy"></param>
        ''' <param name="orderDesc"></param>
        ''' <param name="returnLimit"></param>
        ''' <param name="pageIndex"></param>
        ''' <param name="pageSize"></param>
        ''' <param name="searchDesc"></param>
        ''' <param name="isDealer"></param>
        ''' <param name="categoryList"></param>
        ''' <param name="excludeFeatured"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function getProducts(ByVal portalId As Integer,
            ByVal categoryId As Integer,
            ByVal lang As String,
            ByVal filter As String,
            ByVal getDeleted As Boolean,
            ByVal featuredOnly As Boolean,
            ByVal orderBy As String,
            ByVal orderDesc As Boolean,
            ByVal returnLimit As String,
            ByVal pageIndex As Integer,
            ByVal pageSize As Integer,
            ByVal searchDesc As Boolean,
            ByVal isDealer As Boolean,
            ByVal categoryList As String,
            ByVal excludeFeatured As Boolean) As IEnumerable(Of Models.Product)

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
        ''' Gets product model
        ''' </summary>
        ''' <param name="productId">Product ID</param>
        ''' <param name="lang">Language</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function getProductModel(productId As Integer, lang As String) As Models.ProductModel

        ''' <summary>
        ''' Gets list product models
        ''' </summary>
        ''' <param name="portalId"></param>
        ''' <param name="productId"></param>
        ''' <param name="lang"></param>
        ''' <param name="isDealer"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function getProductModels(ByVal portalId As Integer, ByVal productId As Integer, ByVal lang As String, ByVal isDealer As Boolean) As IEnumerable(Of Models.ProductModel)

        ' ''' <summary>
        ' ''' Gets list of products
        ' ''' </summary>
        ' ''' <param name="PortalID">Portal ID</param>
        ' ''' <param name="ItemType">Item Type ID</param>
        ' ''' <param name="CategoryId">Category ID</param>
        ' ''' <param name="SearchString">Search Term</param>
        ' ''' <param name="IsDeleted">Is Deleted</param>
        ' ''' <param name="StartDate">Modified Date Start</param>
        ' ''' <param name="EndDate">Modified Date End Range</param>
        ' ''' <param name="PageNumber">Page Number</param>
        ' ''' <param name="PageSize">Page Size</param>
        ' ''' <param name="OrderBy">Order BY</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Function GetProducts(ByVal PortalID As String, ByVal ItemType As String, ByVal CategoryId As Integer, ByVal SearchString As String, ByVal IsDeleted As String, ByVal StartDate As Date, ByVal EndDate As Date, ByVal PageNumber As Integer, ByVal PageSize As Integer, ByVal OrderBy As String) As IEnumerable(Of Models.Product)

        ''' <summary>
        ''' Gets product images by product id
        ''' </summary>
        ''' <param name="productId">Product ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function getImages(productId As Integer)

        ''' <summary>
        ''' Updates product model
        ''' </summary>
        ''' <param name="productModel">Product Model</param>
        ''' <remarks></remarks>
        Sub updateProductModel(productModel As Models.ProductModel)

        ''' <summary>
        ''' Updates product model language
        ''' </summary>
        ''' <param name="productModelLang">Product Model Language</param>
        ''' <remarks></remarks>
        Sub updateProductModelLang(productModelLang As Models.ProductModelLang)

        ''' <summary>
        ''' Adds product model
        ''' </summary>
        ''' <param name="productModel"></param>
        ''' <remarks></remarks>
        Function addProductModel(productModel As Models.ProductModel) As Models.ProductModel

        ''' <summary>
        ''' Adds product model language
        ''' </summary>
        ''' <param name="productModelLang"></param>
        ''' <remarks></remarks>
        Function addProductModelLang(productModelLang As Models.ProductModelLang) As Models.ProductModelLang

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

        ''' <summary>
        ''' Copy product languages
        ''' </summary>
        ''' <param name="productLang"></param>
        ''' <param name="ForceOverwrite"></param>
        ''' <remarks></remarks>
        Sub copyProductToLanguages(productLang As Models.ProductLang, Optional ByVal forceOverwrite As Boolean = True)

        ''' <summary>
        ''' Copy product languages
        ''' </summary>
        ''' <param name="productLang"></param>
        ''' <param name="lang"></param>
        ''' <param name="originalLang"></param>
        ''' <param name="forceOverwrite"></param>
        ''' <remarks></remarks>
        Sub copyProductToLanguages(productLang As Models.ProductLang, lang As String, originalLang As String, Optional ByVal forceOverwrite As Boolean = True)

        ''' <summary>
        ''' Copies product model languages
        ''' </summary>
        ''' <param name="productModelLang"></param>
        ''' <param name="forceOverwrite"></param>
        ''' <remarks></remarks>
        Sub copyModelToLanguages(ByVal productModelLang As Models.ProductModelLang, ByVal ForceOverwrite As Boolean)

        ''' <summary>
        ''' Copies product model languages
        ''' </summary>
        ''' <param name="productModelLang"></param>
        ''' <param name="lang"></param>
        ''' <param name="forceOverwrite"></param>
        ''' <remarks></remarks>
        Sub copyModelToLanguages(productModelLang As Models.ProductModelLang, ByVal Lang As String, ByVal ForceOverwrite As Boolean)

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
        Sub CopyOptionToLanguages(ByVal objInfo As Models.ProductOptionLang, ByVal Lang As String, ByVal ForceOverwrite As Boolean)

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
        Sub CopyOptionValueToLanguages(ByVal objInfo As Models.ProductOptionValueLang, ByVal Lang As String, ByVal ForceOverwrite As Boolean)

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
        ''' <param name="productId">Product ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function getProductsRelated(productId As Integer) As IEnumerable(Of Models.ProductRelated)

        Sub removeProduct(productId As Integer, lang As String)

        Sub removeProduct(product As Models.Product)

        Sub removeProductLang(productId As Integer)

        Sub removeRelatedProduct(relatedProductId As Integer)

        Sub removeRelatedProduct(relatedProduct As Models.ProductRelated)

        Sub removeProductOption(productOptionId As Integer, lang As String)

        Sub removeProductOption(productOption As Models.ProductOption)

        Sub removeProductOptionLang(productOptionId As Integer)

        Sub removeProductOptionLang(productOptionLang As Models.ProductOptionLang)

        Sub removeProductOptionValue(optionValueId As Integer, lang As String)

        Sub removeProductOptionValue(optionValue As Models.ProductOptionValue)

        Sub removeProductOptionValueLang(optionValueId As Integer)

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

    End Interface
End Namespace