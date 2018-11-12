
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation
Imports System.Globalization
Imports System.Web.Script.Serialization

Namespace RI.Modules.RIStore_Services
    Public Class ProductsController
        Inherits DnnApiController

        Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(AccountsController))

        <AllowAnonymous> _
        <HttpGet> _
        Public Function getProducts(Optional ByVal portalId As Integer = 0, Optional ByVal categoryId As Integer = 0, Optional ByVal lang As String = "pt-BR", Optional ByVal getArchived As Boolean = False, Optional ByVal featuredOnly As Boolean = False, Optional ByVal orderDesc As Boolean = False, Optional ByVal returnLimit As String = "", Optional ByVal pageIndex As Integer = 1, Optional ByVal pageSize As Integer = 10, Optional ByVal searchDesc As Boolean = False, Optional ByVal isDealer As Boolean = False, Optional ByVal excludeFeatured As Boolean = False, Optional ByVal filter As String = "", Optional ByVal categoryList As String = "", Optional ByVal orderBy As String = "") As HttpResponseMessage
            Try
                Dim products As IEnumerable(Of Models.Product)
                Dim productCtrl As New ProductsRepository

                Dim searchStr As String = HttpContext.Current.Request.Params("filter[filters][0][value]")
                If Not searchStr = Nothing Then
                    filter = searchStr
                End If

                products = productCtrl.getProducts(portalId, categoryId, lang, filter, getArchived, featuredOnly, orderBy, orderDesc, returnLimit, pageIndex, pageSize, searchDesc, isDealer, categoryList, excludeFeatured)

                Dim total = 0
                For Each item In products
                    total = item.TotalRows
                    Exit For
                Next

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.data = products, .total = total})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ' ''' <summary>
        ' ''' Get products and services by portal id and search term
        ' ''' </summary>
        ' ''' <param name="portalId">Portal ID</param>
        ' ''' <param name="catId">Category ID</param>
        ' ''' <param name="sTerm">Search Term</param>
        ' ''' <param name="isDeleted">Has been deleted or not</param>
        ' ''' <param name="sDate">Modified Date Start</param>
        ' ''' <param name="eDate">Modified Date End</param>
        ' ''' <param name="pageNumber">Page Number</param>
        ' ''' <param name="pageSize">How many pages</param>
        ' ''' <param name="orderBy">Field to order by</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        '<DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        '<HttpGet> _
        'Function GetProducts(ByVal portalId As String,
        '    ByVal iType As String,
        '    ByVal isDeleted As String,
        '    ByVal pageNumber As Integer,
        '    ByVal pageSize As Integer,
        '    ByVal orderBy As String,
        '    Optional ByVal sDate As String = Nothing,
        '    Optional ByVal eDate As String = Nothing,
        '    Optional ByVal catId As Integer = -1,
        '    Optional ByVal sTerm As String = "") As HttpResponseMessage
        '    Try

        '        Dim searchStr As String = HttpContext.Current.Request.Params("filter[filters][0][value]")
        '        If Not sTerm.Length > 0 Then
        '            If searchStr Is Nothing Then
        '                searchStr = ""
        '            End If
        '        Else
        '            searchStr = sTerm
        '        End If

        '        If sDate = Nothing Then
        '            sDate = CStr(Null.NullDate)
        '        End If

        '        If eDate = Nothing Then
        '            eDate = CStr(Null.NullDate)
        '        End If

        '        Dim productsData As List(Of Models.Product)
        '        Dim productCtrl As New ProductsRepository

        '        If catId = -1 Then
        '            catId = Null.NullInteger
        '        End If

        '        productsData = productCtrl.GetProducts(portalId, iType, catId, searchStr, isDeleted, sDate, eDate, pageNumber, pageSize, orderBy)

        '        Dim total = Nothing
        '        For Each item In productsData
        '            total = item.TotalRows
        '            Exit For
        '        Next

        '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.data = productsData, .total = total})
        '    Catch ex As Exception
        '        'DnnLog.Error(ex)
        '        Logger.[Error](ex)
        '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        '    End Try
        'End Function

        ''' <summary>
        ''' Gets product info by product id
        ''' </summary>
        ''' <param name="productId">Product ID</param>
        ''' <param name="lang">Product Language</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <AllowAnonymous> _
        <HttpGet> _
        Function getProduct(productId As Integer, lang As String) As HttpResponseMessage
            Try
                Dim product As Models.Product
                Dim productCtrl As New ProductsRepository

                product = productCtrl.getProduct(productId, lang)

                Return Request.CreateResponse(HttpStatusCode.OK, product)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Gets product images by product id
        ''' </summary>
        ''' <param name="pId">Product ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        <HttpGet> _
        Function getProductImages(pId As Integer) As HttpResponseMessage
            Try
                Dim productImages As IEnumerable(Of Models.ProductImage)
                Dim productCtrl As New ProductsRepository

                productImages = productCtrl.getImages(pId)

                Return Request.CreateResponse(HttpStatusCode.OK, productImages)
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Add or updates a product
        ''' </summary>
        ''' <param name="productId"></param>
        ''' <param name="portalId"></param>
        ''' <param name="TaxCategoryID"></param>
        ''' <param name="featured"></param>
        ''' <param name="createdByUser"></param>
        ''' <param name="createdOnDate"></param>
        ''' <param name="isDeleted"></param>
        ''' <param name="productRef"></param>
        ''' <param name="lang"></param>
        ''' <param name="summary"></param>
        ''' <param name="productDesc"></param>
        ''' <param name="manufacturer"></param>
        ''' <param name="productName"></param>
        ''' <param name="seoName"></param>
        ''' <param name="tagWords"></param>
        ''' <param name="isHidden"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        <HttpPost> _
        Function updateProduct(ByVal portalId As Integer,
            ByVal featured As Boolean,
            ByVal createdByUser As String,
            ByVal createdOnDate As Date,
            ByVal productName As String,
            Optional ByVal archived As Boolean = False,
            Optional ByVal summary As String = Nothing,
            Optional ByVal productDesc As String = Nothing,
            Optional ByVal manufacturer As String = Nothing,
            Optional ByVal productId As Integer = -1,
            Optional ByVal allow As Boolean = True,
            Optional ByVal barCode As String = Nothing,
            Optional ByVal TaxCategoryID As Integer = Nothing,
            Optional ByVal isDeleted As Boolean = False,
            Optional ByVal productRef As String = Nothing,
            Optional ByVal lang As String = "pt-BR",
            Optional ByVal seoName As String = Nothing,
            Optional ByVal seoPageTitle As String = Nothing,
            Optional ByVal tagWords As String = Nothing,
            Optional ByVal isHidden As Boolean = False,
            Optional ByVal dealerCost As String = Nothing,
            Optional ByVal dealerOnly As Boolean = False,
            Optional ByVal productModels As String = "",
            Optional ByVal categories As String = "") As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim productCtrl As New ProductsRepository
                Dim product As New Models.Product
                Dim productLang As New Models.ProductLang
                Dim productModel As New Models.ProductModel

                If productId > 0 Then
                    product = productCtrl.getProduct(productId, lang)
                    'productLang = productCtrl.getProductLang(productId, lang)
                End If

                product.Archived = archived
                product.CreatedByUser = createdByUser
                product.CreatedOnDate = createdOnDate
                product.Featured = featured
                product.IsDeleted = isDeleted
                product.IsHidden = isHidden
                product.ModifiedByUser = createdByUser
                product.ModifiedOnDate = createdOnDate
                product.PortalId = portalId
                product.ProductRef = productRef

                productLang.PortalId = product.ProductId
                productLang.Description = productDesc
                productLang.Lang = lang
                productLang.Manufacturer = manufacturer
                productLang.ProductName = productName
                productLang.SEOName = seoName
                productLang.SEOPageTitle = seoPageTitle
                productLang.Summary = summary
                productLang.TagWords = tagWords

                Utilities.CreateDir(PortalController.GetCurrentPortalSettings(), Constants.PRODUCTIMAGESFOLDER)
                Utilities.CreateDir(PortalController.GetCurrentPortalSettings(), Constants.PRODUCTDOCSFOLDER)

                If productId > 0 Then
                    productCtrl.updateProduct(product)
                Else
                    productCtrl.addProduct(product)
                End If

                'copy to languages not created yet
                productLang.ProductId = product.ProductId
                productCtrl.copyProductToLanguages(productLang, False)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .ProductId = product.ProductId})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Adds or updates products models
        ''' </summary>
        ''' <param name="ProductModels"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        <HttpPost> _
        Function updateProductModels(ProductModels As String) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim productCtrl As New ProductsRepository
                Dim productModel As New Models.ProductModel
                Dim productModelLang As New Models.ProductModelLang

                Dim objDeserializer As New JavaScriptSerializer()

                Dim prodModels As List(Of Models.ProductModel) = objDeserializer.Deserialize(Of List(Of Models.ProductModel))(ProductModels)
                If ProductModels <> "" Then
                    For Each prodModel In prodModels
                        If prodModel.ModelId > 0 Then
                            productCtrl.updateProductModel(prodModel)
                        Else
                            productCtrl.addProductModel(prodModel)
                        End If
                    Next
                End If

                Dim prodModelsLang As List(Of Models.ProductModelLang) = objDeserializer.Deserialize(Of List(Of Models.ProductModelLang))(ProductModels)
                If ProductModels <> "" Then
                    For Each prodModelLang In prodModelsLang
                        If prodModelLang.ModelId > 0 Then
                            productCtrl.updateProductModelLang(prodModelLang)
                        Else
                            productCtrl.addProductModelLang(prodModelLang)
                        End If
                    Next
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Add or updates product model
        ''' </summary>
        ''' <param name="productId"></param>
        ''' <param name="modelName">Model Name</param>
        ''' <param name="listOrder"></param>
        ''' <param name="modelRef"></param>
        ''' <param name="lang"></param>
        ''' <param name="modelDesc"></param>
        ''' <param name="barcode"></param>
        ''' <param name="qtyRemaining"></param>
        ''' <param name="qtyTrans"></param>
        ''' <param name="qtyTransDate"></param>
        ''' <param name="deleted"></param>
        ''' <param name="qtyStockSet"></param>
        ''' <param name="dealerCost"></param>
        ''' <param name="purchaseCost"></param>
        ''' <param name="extra"></param>
        ''' <param name="dealerOnly"></param>
        ''' <param name="modelId"></param>
        ''' <param name="allow"></param>
        ''' <param name="unitCost">Unit Cost</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        <HttpPost> _
        Function updateProductModel(ByVal productId As Integer,
            ByVal modelName As String,
            ByVal lang As String,
            ByVal qtyTransDate As Date,
            Optional ByVal barcode As String = Nothing,
            Optional ByVal qtyRemaining As String = "-1",
            Optional ByVal qtyTrans As String = "0",
            Optional ByVal deleted As Boolean = False,
            Optional ByVal qtyStockSet As String = "0",
            Optional ByVal dealerCost As String = "0",
            Optional ByVal purchaseCost As String = "0",
            Optional ByVal extra As String = Nothing,
            Optional ByVal dealerOnly As Boolean = False,
            Optional ByVal modelId As Integer = -1,
            Optional ByVal allow As Integer = -1,
            Optional ByVal unitCost As String = "0",
            Optional ByVal listOrder As Integer = 1,
            Optional ByVal modelRef As String = Nothing,
            Optional ByVal modelDesc As String = Nothing) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim productCtrl As New ProductsRepository
                Dim productModel As New Models.ProductModel
                Dim productModelLang As New Models.ProductModelLang

                If modelId > 0 Then
                    productModel = productCtrl.getProductModel(modelId, lang)
                End If

                productModel.Allow = allow
                productModel.Barcode = barcode
                productModel.DealerCost = Decimal.Parse(dealerCost.Replace(".", ","), numInfo)
                productModel.DealerOnly = dealerOnly
                productModel.Deleted = deleted
                productModel.ListOrder = listOrder
                productModel.ModelRef = modelRef
                productModel.ProductId = productId
                productModel.PurchaseCost = Decimal.Parse(purchaseCost.Replace(".", ","), numInfo)
                productModel.QtyRemaining = Decimal.Parse(qtyRemaining.Replace(".", ","), numInfo)
                productModel.QtyStockSet = Decimal.Parse(qtyStockSet.Replace(".", ","), numInfo)
                productModel.UnitCost = Decimal.Parse(unitCost.Replace(".", ","), numInfo)
                productModel.QtyTransDate = qtyTransDate

                productModelLang.Extra = extra
                productModelLang.Lang = lang
                productModelLang.ModelName = modelName

                If modelId > 0 Then
                    productCtrl.updateProductModel(productModel)
                Else
                    productCtrl.addProductModel(productModel)
                End If

                productModelLang.ModelId = productModel.ModelId
                productCtrl.CopyModelToLanguages(productModelLang, False)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Adds or updates product option
        ''' </summary>
        ''' <param name="productId"></param>
        ''' <param name="listOrder"></param>
        ''' <param name="attributes"></param>
        ''' <param name="optionId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        <HttpPost> _
        Function updateProductOption(ByVal productId As Integer,
            ByVal lang As String,
            Optional ByVal listOrder As Integer = 1,
            Optional ByVal attributes As String = Nothing,
            Optional ByVal optionId As Integer = -1,
            Optional ByVal optionDesc As String = Nothing) As HttpResponseMessage
            Try

                Dim productCtrl As New ProductsRepository
                Dim productOption As New Models.ProductOption
                Dim productOptionLang As New Models.ProductOptionLang

                If optionId > 0 Then
                    productOption = productCtrl.getProductOption(optionId, lang)
                End If

                productOption.attributes = attributes
                productOption.ListOrder = listOrder
                productOption.ProductId = productId

                productOptionLang.OptionDesc = optionDesc
                productOptionLang.Lang = lang

                If optionId > 0 Then
                    productCtrl.updateProductOption(productOption)
                Else
                    productCtrl.addProductOption(productOption)
                End If

                productOptionLang.OptionId = productOption.OptionId
                productCtrl.CopyOptionToLanguages(productOptionLang, lang, False)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Adds or updates product option value
        ''' </summary>
        ''' <param name="optionId"></param>
        ''' <param name="lang"></param>
        ''' <param name="listOrder"></param>
        ''' <param name="addedCost"></param>
        ''' <param name="optionValueId"></param>
        ''' <param name="optionValueDesc"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        <HttpPost> _
        Function updateProductOptionValue(ByVal optionId As Integer,
            ByVal lang As String,
            Optional ByVal listOrder As Integer = 1,
            Optional ByVal addedCost As String = "0",
            Optional ByVal optionValueId As Integer = -1,
            Optional ByVal optionValueDesc As String = Nothing) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim productCtrl As New ProductsRepository
                Dim productOptionValue As New Models.ProductOptionValue
                Dim productOptionValueLang As New Models.ProductOptionValueLang

                If optionValueId > 0 Then
                    productOptionValue = productCtrl.getProductOptionValue(optionValueId, lang)
                End If

                productOptionValue.AddedCost = Decimal.Parse(addedCost.Replace(".", ","), numInfo)
                productOptionValue.ListOrder = listOrder
                productOptionValue.OptionId = optionId

                productOptionValueLang.Lang = lang
                productOptionValueLang.OptionValueDesc = optionValueDesc

                If optionValueId > 0 Then
                    productCtrl.updateProductOptionValue(productOptionValue)
                Else
                    productCtrl.addProductOptionValue(productOptionValue)
                End If

                productOptionValueLang.OptionValueId = productOptionValue.OptionValueId
                productCtrl.CopyOptionValueToLanguages(productOptionValueLang, lang, False)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Adds or updates product related
        ''' </summary>
        ''' <param name="productId"></param>
        ''' <param name="relatedProductId"></param>
        ''' <param name="relatedId"></param>
        ''' <param name="discountAmt"></param>
        ''' <param name="discountPercent"></param>
        ''' <param name="portalId"></param>
        ''' <param name="ProductQty"></param>
        ''' <param name="MaxQty"></param>
        ''' <param name="RelatedType"></param>
        ''' <param name="Disabled"></param>
        ''' <param name="NotAvailable"></param>
        ''' <param name="BiDirectional"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        <HttpPost> _
        Function updateProductRelated(ByVal productId As Integer,
            ByVal relatedProductId As Integer,
            Optional ByVal relatedId As Integer = -1,
            Optional ByVal discountAmt As String = "0",
            Optional ByVal discountPercent As String = "0",
            Optional ByVal portalId As Integer = 0,
            Optional ByVal productQty As Integer = 0,
            Optional ByVal maxQty As Integer = 0,
            Optional ByVal relatedType As Integer = 0,
            Optional ByVal disabled As Integer = 0,
            Optional ByVal notAvailable As Integer = 0,
            Optional ByVal biDirectional As Integer = 0) As HttpResponseMessage
            Try
                Dim culture = New CultureInfo("pt-BR")
                Dim numInfo = culture.NumberFormat

                Dim productCtrl As New ProductsRepository
                Dim productRelated As New Models.ProductRelated

                If relatedId > 0 Then
                    productRelated = productCtrl.getProductRelated(relatedId)
                End If

                productRelated.BiDirectional = biDirectional
                productRelated.Disabled = disabled
                productRelated.DiscountAmt = Decimal.Parse(discountAmt.Replace(".", ","), numInfo)
                productRelated.DiscountPercent = Decimal.Parse(discountPercent.Replace(".", ","), numInfo)
                productRelated.MaxQty = maxQty
                productRelated.NotAvailable = notAvailable
                productRelated.PortalId = portalId
                productRelated.ProductId = productId
                productRelated.ProductQty = productQty
                productRelated.RelatedProductId = relatedProductId
                productRelated.RelatedType = relatedType

                If relatedId > 0 Then
                    productCtrl.updateProductRelated(productRelated)
                Else
                    productCtrl.addProductRelated(productRelated)
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        <HttpDelete> _
        Function removeRelatedProduct(relatedId As Integer) As HttpResponseMessage
            Try
                Dim productCtrl As New ProductsRepository

                productCtrl.removeRelatedProduct(relatedId)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        <HttpDelete> _
        Function removeProduct(productId As Integer, lang As String) As HttpResponseMessage
            Try
                Dim productCtrl As New ProductsRepository

                productCtrl.removeProduct(productId, lang)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        <HttpDelete> _
        Function removeProductLang(productId As Integer) As HttpResponseMessage
            Try
                Dim productCtrl As New ProductsRepository

                productCtrl.removeProductLang(productId)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        <HttpDelete> _
        Function removeProductOption(productOptionId As Integer, lang As String) As HttpResponseMessage
            Try
                Dim productCtrl As New ProductsRepository

                productCtrl.removeProductOption(productOptionId, lang)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        <HttpDelete> _
        Function removeProductOptionLang(productOptionId As Integer) As HttpResponseMessage
            Try
                Dim productCtrl As New ProductsRepository

                productCtrl.removeProductOptionLang(productOptionId)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        <HttpDelete> _
        Function removeProductOptionValue(productOptionValueId As Integer, lang As String) As HttpResponseMessage
            Try
                Dim productCtrl As New ProductsRepository

                productCtrl.removeProductOptionValue(productOptionValueId, lang)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        <HttpDelete> _
        Function removeProductOptionValueLang(productOptionValueId As Integer) As HttpResponseMessage
            Try
                Dim productCtrl As New ProductsRepository

                productCtrl.removeProductOptionValueLang(productOptionValueId)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        <HttpDelete> _
        Function removeProductCategory(productId As Integer) As HttpResponseMessage
            Try
                Dim productCtrl As New ProductsRepository

                productCtrl.removeProductCategory(productId)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        <HttpDelete> _
        Function removeProductsCategory(categoryId As Integer) As HttpResponseMessage
            Try
                Dim productCtrl As New ProductsRepository

                productCtrl.removeProductsCategory(categoryId)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        <HttpPost> _
        Function addProductCategories(productId As Integer, categories As String) As HttpResponseMessage
            Try
                Dim productCtrl As New ProductsRepository

                productCtrl.addProductCategories(productId, categories)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
        <HttpPost> _
        Function addProductCategory(dto As Models.ProductCategory) As HttpResponseMessage
            Try
                Dim productCtrl As New ProductsRepository

                productCtrl.addProductCategory(dto.ProductId, dto.CategoryId)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

    End Class
End Namespace