
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Web.Api
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.IO
Imports ImageResizer.Configuration
Imports System.Drawing

Public Class ProductsController
    Inherits DnnApiController

    Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(AccountsController))

    ''' <summary>
    ''' Gets products list
    ''' </summary>
    ''' <param name="portalId"></param>
    ''' <param name="categoryId"></param>
    ''' <param name="lang"></param>
    ''' <param name="getArchived"></param>
    ''' <param name="featuredOnly"></param>
    ''' <param name="orderDesc"></param>
    ''' <param name="returnLimit"></param>
    ''' <param name="pageIndex"></param>
    ''' <param name="pageSize"></param>
    ''' <param name="searchDesc"></param>
    ''' <param name="isDealer"></param>
    ''' <param name="excludeFeatured"></param>
    ''' <param name="filter"></param>
    ''' <param name="categoryList"></param>
    ''' <param name="orderBy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpGet> _
    Public Function getProducts(Optional portalId As Integer = 0, Optional categoryId As Integer = -1, Optional lang As String = "pt-BR", Optional getArchived As Boolean = False, Optional featuredOnly As Boolean = False, Optional orderDesc As String = "", Optional returnLimit As String = "", Optional pageIndex As Integer = 1, Optional pageSize As Integer = 10, Optional onSale As String = "", Optional searchDesc As Boolean = False, Optional isDealer As Boolean = False, Optional getDeleted As String = "", Optional excludeFeatured As Boolean = False, Optional filter As String = "", Optional categoryList As String = "", Optional orderBy As String = "") As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            Dim searchStr As String = HttpContext.Current.Request.Params("filter[filters][0][value]")
            If Not searchStr = Nothing Then
                filter = searchStr
            End If

            Dim products = productCtrl.getProducts(portalId, categoryId, lang, filter, getArchived, featuredOnly, orderBy, orderDesc, returnLimit, pageIndex, pageSize, onSale, searchDesc, isDealer, getDeleted, categoryList, excludeFeatured)

            Dim total = 0
            If returnLimit <> "" Then
                total = CInt(returnLimit)
            Else
                For Each item In products
                    total = item.TotalRows
                    Exit For
                Next
            End If

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
    'Function GetProducts(portalId As String,
    '    iType As String,
    '    isDeleted As String,
    '    pageNumber As Integer,
    '    pageSize As Integer,
    '    orderBy As String,
    '    Optional sDate As String = Nothing,
    '    Optional eDate As String = Nothing,
    '    Optional catId As Integer = -1,
    '    Optional sTerm As String = "") As HttpResponseMessage
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
            Dim productCtrl As New ProductsRepository

            Dim product = productCtrl.getProduct(productId, lang)

            If product.ProductOptionsCount > 0 Then

                product.ProductOptions = productCtrl.getProductOptions(product.ProductId, lang)

                Dim productOptionValues As New List(Of Models.ProductOptionValue)

                For Each _option In product.ProductOptions

                    Dim optionValues = productCtrl.getProductOptionValues(_option.OptionId, lang)

                    For Each _optionValue In optionValues

                        productOptionValues.Add(_optionValue)

                    Next

                Next

                If productOptionValues.Count > 0 Then

                    product.ProductOptionValues = productOptionValues

                End If

            End If

            If product.ProductImagesCount > 1 Then

                product.ProductImages = productCtrl.getProductImages(product.ProductId)

            End If

            If product.ProductsRelatedCount > 0 Then

                product.ProductsRelated = productCtrl.getProductsRelated(product.PortalId, product.ProductId, lang, Null.NullInteger, True)

            End If

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
    ''' <param name="productId">Product ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpGet> _
    Function getProductImages(productId As Integer) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            Dim productImages = productCtrl.getProductImages(productId)

            Return Request.CreateResponse(HttpStatusCode.OK, productImages)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates a product
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPost> _
    Function updateProduct(dto As Models.Product) As HttpResponseMessage
        Try
            Dim product As New Models.Product
            Dim productLang As New Models.ProductLang
            Dim productCtrl As New ProductsRepository

            If dto.ProductId > 0 Then
                product = productCtrl.getProduct(dto.ProductId, dto.Lang)
            End If

            product.ItemType = dto.ItemType
            product.Archived = dto.Archived
            product.DealerOnly = dto.DealerOnly
            product.CreatedByUser = dto.CreatedByUser
            product.CreatedOnDate = dto.CreatedOnDate
            product.Featured = dto.Featured
            product.IsHidden = dto.IsHidden
            product.ModifiedByUser = dto.ModifiedByUser
            product.ModifiedOnDate = dto.ModifiedOnDate
            product.PortalId = dto.PortalId
            product.ProductRef = dto.ProductRef
            product.ReorderPoint = dto.ReorderPoint
            product.ProductUnit = dto.ProductUnit
            product.SaleStartDate = dto.SaleStartDate
            product.SaleEndDate = dto.SaleEndDate
            product.Brand = dto.Brand
            product.BrandModel = dto.BrandModel
            product.Barcode = dto.Barcode
            product.Vendors = dto.Vendors
            'product.Width = dto.Width
            'product.Height = dto.Height
            'product.Weight = dto.Weight
            'product.Length = dto.Length
            'product.Diameter = dto.Diameter
            'product.QtyStockSet = dto.QtyStockSet

            productLang.Lang = dto.Lang
            productLang.ProductName = dto.ProductName
            productLang.Manufacturer = dto.Manufacturer
            productLang.Summary = dto.Summary
            'productLang.Description = dto.Description
            'productLang.SEOName = dto.SEOName
            'productLang.SEOPageTitle = dto.SEOPageTitle
            'productLang.TagWords = dto.TagWords

            Utilities.CreateDir(PortalController.GetCurrentPortalSettings(), Constants.PRODUCTIMAGESFOLDER)
            Utilities.CreateDir(PortalController.GetCurrentPortalSettings(), Constants.PRODUCTDOCSFOLDER)

            If dto.ProductId > 0 Then
                productCtrl.updateProduct(product)
            Else
                product.QtyStockSet = 0
                product.Width = 0
                product.Height = 0
                product.Weight = 0
                product.Length = 0
                product.Diameter = 0

                productCtrl.addProduct(product)

                Dim productFinance As New Models.ProductFinance

                productFinance.Finan_COFINS = 0
                productFinance.Finan_COFINSBase = 0
                productFinance.Finan_COFINSTributeSub = 0
                productFinance.Finan_COFINSTributeSubBase = 0
                productFinance.Finan_Cost = 0
                productFinance.Finan_DiffICMS = 0
                productFinance.Finan_Freight = 0
                productFinance.Finan_ICMS = 0
                productFinance.Finan_ICMSFreight = 0
                productFinance.Finan_IPI = 0
                productFinance.Finan_ISS = 0
                productFinance.Finan_Manager = 0
                productFinance.Finan_MarkUp = 0
                productFinance.Finan_OtherExpenses = 0
                productFinance.Finan_OtherTaxes = 0
                productFinance.Finan_Paid = 0
                productFinance.Finan_Paid_Discount = 0
                productFinance.Finan_PIS = 0
                productFinance.Finan_PISBase = 0
                productFinance.Finan_PISTributeSub = 0
                productFinance.Finan_PISTributeSubBase = 0
                productFinance.Finan_Rep = 0
                productFinance.Finan_Sale_Price = 0
                productFinance.Finan_SalesPerson = 0
                productFinance.Finan_Select = "1"
                productFinance.Finan_Special_Price = 0
                productFinance.Finan_Dealer_Price = 0
                productFinance.Finan_Tech = 0
                productFinance.Finan_Telemarketing = 0
                productFinance.Finan_TributeSubICMS = 0
                productFinance.Finan_TributeSubICMS = 0
                productFinance.ProductId = product.ProductId

                productCtrl.addProductFinance(productFinance)
            End If

            'copy to languages not created yet
            productLang.ProductId = product.ProductId
            productCtrl.CopyProductToLanguages(productLang, dto.Lang, False)

            If dto.Categories <> "" Then
                dto.Categories = dto.Categories.Replace(" ", "")
                Dim productCategory As New Models.ProductCategory
                productCtrl.removeProductCategory(product.ProductId)
                For Each categoryId In dto.Categories.Split(","c)
                    If categoryId <> "" Then
                        productCtrl.addProductCategory(product.ProductId, CInt(categoryId))
                    End If
                Next
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .ProductId = product.ProductId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates a product
    ''' </summary>
    ''' <param name="products"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPost> _
    Function updateProductStock(products As List(Of Models.Product)) As HttpResponseMessage
        Try
            Dim product As New Models.Product
            Dim productLang As New Models.ProductLang
            Dim productFinance As New Models.ProductFinance
            Dim productCtrl As New ProductsRepository

            For Each item In products
                product = productCtrl.getProduct(item.ProductId, item.Lang)

                Dim oldStock = product.QtyStockSet

                product.ProductRef = item.ProductRef
                product.Barcode = item.Barcode
                product.QtyStockSet = oldStock + item.QtyStockSet
                product.ModifiedByUser = item.ModifiedByUser
                product.ModifiedOnDate = item.ModifiedOnDate

                productCtrl.updateProduct(product)

                productLang = productCtrl.getProductLang(item.ProductId, item.Lang)
                productLang.ProductName = item.ProductName
                productCtrl.updateProductLang(productLang)

                productFinance = productCtrl.getProductFinance(item.ProductId)
                productFinance.Finan_Sale_Price = item.Finan_Sale_Price
                productCtrl.updateProductFinance(productFinance)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates a product
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPost> _
    Function updateProductSEO(dto As Models.ProductLang) As HttpResponseMessage
        Try
            Dim product As New Models.Product
            Dim productLang As New Models.ProductLang
            Dim productCtrl As New ProductsRepository

            product = productCtrl.getProduct(dto.ProductId, dto.Lang)

            product.ModifiedByUser = dto.ModifiedByUser
            product.ModifiedOnDate = dto.ModifiedOnDate

            productCtrl.updateProduct(product)

            productLang = productCtrl.getProductLang(dto.ProductId, dto.Lang)

            productLang.SEOSummary = dto.SEOSummary
            productLang.SEOName = dto.SEOName
            productLang.SEOPageTitle = dto.SEOPageTitle
            productLang.TagWords = dto.TagWords

            productCtrl.updateProductLang(productLang)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates a product description
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPost> _
    Function updateProductDescription(dto As Models.Product) As HttpResponseMessage
        Try
            Dim product As New Models.Product
            Dim productLang As New Models.ProductLang
            Dim productCtrl As New ProductsRepository

            productLang = productCtrl.getProductLang(dto.ProductId, dto.Lang)

            productLang.Description = dto.Description
            productCtrl.updateProductLang(productLang)

            product = productCtrl.getProduct(dto.ProductId, dto.Lang)

            product.ModifiedByUser = dto.ModifiedByUser
            product.ModifiedOnDate = dto.ModifiedOnDate
            productCtrl.updateProduct(product)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates product finance
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes")> _
    <HttpPost> _
    Function updateProductFinance(dto As Models.ProductFinance) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository
            Dim productFinance As New Models.ProductFinance

            If dto.ProductId > 0 Then
                productFinance = productCtrl.getProductFinance(dto.ProductId)
            End If

            productFinance.Finan_CFOP = dto.Finan_CFOP
            productFinance.Finan_COFINS = dto.Finan_COFINS
            productFinance.Finan_COFINSBase = dto.Finan_COFINSBase
            productFinance.Finan_COFINSTributeSituation = dto.Finan_COFINSTributeSituation
            productFinance.Finan_COFINSTributeSub = dto.Finan_COFINSTributeSub
            productFinance.Finan_COFINSTributeSubBase = dto.Finan_COFINSTributeSubBase
            productFinance.Finan_CST = dto.Finan_CST
            productFinance.Finan_DefaultBarCode = dto.Finan_DefaultBarCode
            productFinance.Finan_Cost = dto.Finan_Cost
            productFinance.Finan_DiffICMS = dto.Finan_DiffICMS
            productFinance.Finan_Freight = dto.Finan_Freight
            productFinance.Finan_ICMS = dto.Finan_ICMS
            productFinance.Finan_ICMSFreight = dto.Finan_ICMSFreight
            productFinance.Finan_IPI = dto.Finan_IPI
            productFinance.Finan_IPITributeSituation = dto.Finan_IPITributeSituation
            productFinance.Finan_ISS = dto.Finan_ISS
            productFinance.Finan_Manager = dto.Finan_Manager
            productFinance.Finan_MarkUp = dto.Finan_MarkUp
            productFinance.Finan_NCM = dto.Finan_NCM
            productFinance.Finan_OtherExpenses = dto.Finan_OtherExpenses
            productFinance.Finan_OtherTaxes = dto.Finan_OtherTaxes
            productFinance.Finan_Paid = dto.Finan_Paid
            productFinance.Finan_Paid_Discount = dto.Finan_Paid_Discount
            productFinance.Finan_PIS = dto.Finan_PIS
            productFinance.Finan_PISBase = dto.Finan_PISBase
            productFinance.Finan_PISTributeSituation = dto.Finan_PISTributeSituation
            productFinance.Finan_PISTributeSub = dto.Finan_PISTributeSub
            productFinance.Finan_PISTributeSubBase = dto.Finan_PISTributeSubBase
            productFinance.Finan_Rep = dto.Finan_Rep
            productFinance.Finan_Sale_Price = dto.Finan_Sale_Price
            productFinance.Finan_SalesPerson = dto.Finan_SalesPerson
            productFinance.Finan_Select = dto.Finan_Select
            productFinance.Finan_Special_Price = dto.Finan_Special_Price
            productFinance.Finan_Dealer_Price = dto.Finan_Dealer_Price
            productFinance.Finan_Tech = dto.Finan_Tech
            productFinance.Finan_Telemarketing = dto.Finan_Telemarketing
            productFinance.Finan_TributeSituationType = dto.Finan_TributeSituationType
            productFinance.Finan_TributeSubICMS = dto.Finan_TributeSubICMS

            productCtrl.updateProductFinance(productFinance)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates product special offer
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes")> _
    <HttpPut> _
    Function updateProductSpecialOffer(dto As ProductSpecialOffer) As HttpResponseMessage
        Try

            Dim productCtrl As New ProductsRepository
            Dim product As New Models.Product
            Dim productFinance As New Models.ProductFinance

            If dto.ProductId > 0 Then
                product = productCtrl.getProduct(dto.ProductId, dto.Lang)
                productFinance = productCtrl.getProductFinance(dto.ProductId)
            End If

            productFinance.Finan_Select = dto.Finan_Select
            productFinance.Finan_Special_Price = dto.Finan_Special_Price

            productCtrl.updateProductFinance(productFinance)

            product.SaleStartDate = dto.SaleStartDate
            product.SaleEndDate = dto.SaleEndDate
            product.ModifiedByUser = dto.ModifiedByUser
            product.ModifiedOnDate = dto.ModifiedOnDate

            productCtrl.updateProduct(product)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPost> _
    Async Function postProductImage() As Task(Of HttpResponseMessage)
        Try
            If Not Me.Request.Content.IsMimeMultipartContent("form-data") Then
                Throw New HttpResponseException(New HttpResponseMessage(HttpStatusCode.UnsupportedMediaType))
            End If

            Dim portalCtrl = New Portals.PortalController()
            Dim _request As HttpRequestMessage = Me.Request

            Await _request.Content.LoadIntoBufferAsync()
            Dim task = _request.Content.ReadAsMultipartAsync()
            Dim result = Await task
            Dim contents = result.Contents
            Dim httpContent As HttpContent = contents.First()
            'Dim uploadedFileMediaType As String = httpContent.Headers.ContentType.MediaType
            'Dim _file As Stream = httpContent.ReadAsStreamAsync().Result

            Dim portalId = contents(1).ReadAsStringAsync().Result
            Dim productId = contents(2).ReadAsStringAsync().Result
            Dim createdByUser = contents(3).ReadAsStringAsync().Result
            Dim createdOnDate = contents(4).ReadAsStringAsync().Result
            Dim maxWidth = contents(5).ReadAsStringAsync().Result
            Dim maxHeight = contents(6).ReadAsStringAsync().Result

            Dim guid_1 = Guid.NewGuid().ToString()

            Dim _fileName = If(Not String.IsNullOrWhiteSpace(httpContent.Headers.ContentDisposition.FileName), httpContent.Headers.ContentDisposition.FileName, guid_1).Replace("""", String.Empty)

            Using _file As Stream = httpContent.ReadAsStreamAsync().Result

                Dim settingsForImages = String.Format("width={0}&height={1}", CStr(IIf(CInt(maxWidth) > 0, maxWidth, "800")), CStr(IIf(CInt(maxHeight) > 0, maxHeight, "600")))

                If Config.Current.Pipeline.IsAcceptedImageType(_fileName) AndAlso settingsForImages IsNot Nothing Then
                    'The resizing settings can specify any of 30 commands.. See http://imageresizing.net for details.
                    'Dim resizeCropSettings As New ResizeSettings(settingsForImages)

                    'Dim resizeCropSettings As ResizeSettings = New ResizeSettings("width=" & CStr(IIf(CInt(maxWidth) > 0, maxWidth, "800")) & "&height=" & CStr(IIf(CInt(maxHeight) > 0, maxHeight, "600"))) ' & "&crop=auto")

                    Using ms As New MemoryStream()

                        _file.CopyTo(ms)

                        'Dim i = New ImageResizer.ImageJob() With {.CreateParentDirectory = True, .Source = _file, .Dest = ms, .Settings = resizeCropSettings}

                        Dim productImage As New Models.ProductImage
                        Dim productImageCtrl As New ProductsRepository

                        productImage.ProductId = productId
                        productImage.ContentLength = ms.Length
                        productImage.CreatedByUser = createdByUser
                        productImage.CreatedOnDate = createdOnDate
                        productImage.Extension = Right(_fileName, 3)
                        productImage.FileName = _fileName
                        productImage.ModifiedByUser = createdByUser
                        productImage.ModifiedOnDate = createdOnDate
                        productImage.PortalId = portalId
                        productImage.ProductImageBinary = ms.ToArray()
                        productImage.ListOrder = 1

                        productImageCtrl.addProductImage(productImage)

                        Return _request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})

                    End Using

                End If

            End Using

            Return _request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message}))
        End Try
    End Function

    ''' <summary>
    ''' Disables a product
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPut> _
    Function deleteProduct(dto As Models.Product) As HttpResponseMessage
        Try
            Dim product As New Models.Product
            Dim productCtrl As New ProductsRepository

            product = productCtrl.getProduct(dto.ProductId, dto.Lang)

            product.IsDeleted = True
            product.ModifiedByUser = dto.ModifiedByUser
            product.ModifiedOnDate = dto.ModifiedOnDate
            productCtrl.updateProduct(product)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Restores a product
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPut> _
    Function restoreProduct(dto As Models.Product) As HttpResponseMessage
        Try
            Dim product As New Models.Product
            Dim productCtrl As New ProductsRepository

            product = productCtrl.getProduct(dto.ProductId, dto.Lang)

            product.IsDeleted = False
            product.ModifiedByUser = dto.ModifiedByUser
            product.ModifiedOnDate = dto.ModifiedOnDate
            productCtrl.updateProduct(product)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes product
    ''' </summary>
    ''' <param name="productId"></param>
    ''' <param name="lang"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpDelete> _
    Function removeProduct(productId As Integer, lang As String) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            productCtrl.removeProduct(productId, lang)
            productCtrl.removeProductCategory(productId)
            productCtrl.removeProductLangs(productId)

            Dim productOptions As New List(Of Models.ProductOption)
            productCtrl.getProductOptions(productId, lang)
            For Each productOption In productOptions
                productCtrl.removeProductOption(productOption.OptionId, productOption.Lang)
                productCtrl.removeProductOptionLang(productOption.OptionId, productOption.Lang)

                Dim productOptionValues As New List(Of Models.ProductOptionValue)
                For Each productOptionValue In productOptionValues
                    productCtrl.removeProductOptionValue(productOptionValue.OptionValueId, productOptionValue.Lang)
                Next

                Dim productOptionValueLangs As New List(Of Models.ProductOptionValueLang)
                For Each productOptionValueLang In productOptionValueLangs
                    productCtrl.removeProductOptionValueLang(productOptionValueLang.OptionValueId, productOptionValueLang.Lang)
                Next
            Next

            Dim productsRelated As New List(Of Models.ProductRelated)
            For Each productRelated In productsRelated
                productCtrl.removeRelatedProduct(productRelated.RelatedId)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets list of products related
    ''' </summary>
    ''' <param name="portalId"></param>
    ''' <param name="productId"></param>
    ''' <param name="lang"></param>
    ''' <param name="relatedType"></param>
    ''' <param name="getAll"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpGet> _
    Function getProductsRelated(portalId As Integer, productId As Integer, lang As String, relatedType As Integer, getAll As Boolean) As HttpResponseMessage
        Try
            Dim productsRelatedCtrl As New ProductsRepository

            Dim productsRelated = productsRelatedCtrl.getProductsRelated(portalId, productId, lang, relatedType, getAll)

            Return Request.CreateResponse(HttpStatusCode.OK, productsRelated)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message}))
        End Try
    End Function

    ''' <summary>
    ''' Adds product related
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPost> _
    Function addProductRelated(dto As Models.ProductRelated) As HttpResponseMessage
        Try
            Dim productRelated As New Models.ProductRelated
            Dim productCtrl As New ProductsRepository

            productRelated.BiDirectional = dto.BiDirectional
            productRelated.Disabled = dto.Disabled
            productRelated.DiscountAmt = dto.DiscountAmt
            productRelated.DiscountPercent = dto.DiscountPercent
            productRelated.MaxQty = dto.MaxQty
            productRelated.NotAvailable = dto.NotAvailable
            productRelated.PortalId = dto.PortalId
            productRelated.ProductId = dto.ProductId
            productRelated.ProductQty = dto.ProductQty
            productRelated.RelatedProductId = dto.RelatedProductId
            productRelated.RelatedType = dto.RelatedType

            productCtrl.addProductRelated(productRelated)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .RelatedId = productRelated.RelatedId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes product related
    ''' </summary>
    ''' <param name="relatedProducts"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpDelete> _
    Function removeRelatedProduct(relatedProducts As List(Of Models.ProductRelated)) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            For Each product In relatedProducts
                productCtrl.removeRelatedProduct(product.RelatedId)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes all related products
    ''' </summary>
    ''' <param name="productsRelated"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpDelete> _
    Function removeProductsRelated(productsRelated As List(Of Models.ProductRelated)) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            For Each product In productsRelated
                productCtrl.removeProductsRelated(product.ProductId)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    '<DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    '<HttpDelete> _
    'Function removeProductLang(productId As Integer) As HttpResponseMessage
    '    Try
    '        Dim productCtrl As New ProductsRepository

    '        productCtrl.removeProductLangs(productId)

    '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
    '    Catch ex As Exception
    '        'DnnLog.Error(ex)
    '        Logger.[Error](ex)
    '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
    '    End Try
    'End Function

    ''' <summary>
    ''' Gets list of product attributes
    ''' </summary>
    ''' <param name="productId"></param>
    ''' <param name="lang"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpGet> _
    Function getProductOptions(productId As Integer, lang As String) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            Dim productOptionsData = productCtrl.getProductOptions(productId, lang)

            Return Request.CreateResponse(HttpStatusCode.OK, productOptionsData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds product attribute
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPost> _
    Function addProductOption(dto As Models.ProductOption) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository
            Dim productOption As New Models.ProductOption
            Dim productOptionLang As New Models.ProductOptionLang

            productOption.ListOrder = dto.ListOrder
            productOption.ProductId = dto.ProductId

            productOptionLang.Lang = dto.Lang
            productOptionLang.OptionDesc = dto.OptionDesc

            productCtrl.addProductOption(productOption)

            productOptionLang.OptionId = productOption.OptionId

            'productCtrl.removeProductOptionLang(productOption.OptionId)
            'productCtrl.addProductOptionLang(productOptionLang)

            productCtrl.CopyOptionToLanguages(productOptionLang, dto.Lang, False)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .OptionId = productOption.OptionId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates product attribute
    ''' </summary>
    ''' <param name="productOptions"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPut> _
    Function updateProductOption(productOptions As List(Of Models.ProductOption)) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository
            Dim productOption As New Models.ProductOption
            Dim productOptionLang As New Models.ProductOptionLang

            For Each attribute In productOptions
                productOption = productCtrl.getProductOption(attribute.OptionId, attribute.Lang)

                productOption.ListOrder = attribute.ListOrder
                productOption.ProductId = attribute.ProductId

                productOptionLang.Lang = attribute.Lang
                productOptionLang.OptionDesc = attribute.OptionDesc
                productOptionLang.OptionId = attribute.OptionId

                productCtrl.updateProductOption(productOption)

                'productCtrl.removeProductOptionLang(attribute.OptionId, attribute.Lang)

                'productCtrl.addProductOptionLang(productOptionLang)

                productCtrl.CopyOptionToLanguages(productOptionLang, attribute.Lang, False)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes product attribute
    ''' </summary>
    ''' <param name="productOptionId"></param>
    ''' <param name="lang"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPost> _
    Function removeProductOption(productOptionId As Integer, lang As String) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            productCtrl.removeProductOptionLang(productOptionId, lang)
            productCtrl.removeProductOption(productOptionId, lang)
            productCtrl.removeProductOptionValueLangs(productOptionId, lang)
            productCtrl.removeProductOptionValues(productOptionId, lang)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets list of product attribute's values
    ''' </summary>
    ''' <param name="optionId"></param>
    ''' <param name="lang"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpGet> _
    Function getProductOptionValues(optionId As Integer, lang As String) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            Dim productOptionValuesData = productCtrl.getProductOptionValues(optionId, lang)

            Return Request.CreateResponse(HttpStatusCode.OK, productOptionValuesData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds product attribute value
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPost> _
    Function addProductOptionValue(dto As Models.ProductOptionValue) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository
            Dim productOptionValue As New Models.ProductOptionValue
            Dim productOptionValueLang As New Models.ProductOptionValueLang

            productOptionValue.AddedCost = dto.AddedCost
            productOptionValue.ListOrder = dto.ListOrder
            productOptionValue.OptionId = dto.OptionId
            productOptionValue.QtyStockSet = dto.QtyStockSet

            productOptionValueLang.Lang = dto.Lang
            productOptionValueLang.OptionValueDesc = dto.OptionValueDesc

            productCtrl.addProductOptionValue(productOptionValue)

            productOptionValueLang.OptionValueId = productOptionValue.OptionValueId

            'productCtrl.addProductOptionValueLang(productOptionValueLang)
            productCtrl.CopyOptionValueToLanguages(productOptionValueLang, dto.Lang, False)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .OptionValueId = productOptionValue.OptionValueId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates product attribute value
    ''' </summary>
    ''' <param name="productOptionValues"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPut> _
    Function updateProductOptionValue(productOptionValues As List(Of Models.ProductOptionValue)) As HttpResponseMessage
        Try

            Dim productCtrl As New ProductsRepository
            Dim productOption As New Models.ProductOption
            Dim productOptionValue As New Models.ProductOptionValue
            Dim productOptionValueLang As New Models.ProductOptionValueLang

            Dim totalStock = 0
            Dim optionId = 0
            Dim lang = ""

            For Each optionValue In productOptionValues
                productOptionValue = productCtrl.getProductOptionValue(optionValue.OptionValueId, optionValue.Lang)

                productOptionValue.AddedCost = optionValue.AddedCost
                productOptionValue.ListOrder = optionValue.ListOrder
                productOptionValue.OptionId = optionValue.OptionId
                productOptionValue.QtyStockSet = optionValue.QtyStockSet
                totalStock += optionValue.QtyStockSet
                optionId = optionValue.OptionId
                lang = optionValue.Lang

                productOptionValueLang.Lang = optionValue.Lang
                productOptionValueLang.OptionValueDesc = optionValue.OptionValueDesc
                productOptionValueLang.OptionValueId = productOptionValue.OptionValueId

                productCtrl.updateProductOptionValue(productOptionValue)

                'productCtrl.removeProductOptionValueLang(optionValue.OptionValueId, optionValue.Lang)

                'productCtrl.addProductOptionValueLang(productOptionValueLang)

                productCtrl.CopyOptionValueToLanguages(productOptionValueLang, optionValue.Lang, False)
            Next

            productOption = productCtrl.getProductOption(optionId, lang)

            productOption.QtyStockSet = totalStock

            productCtrl.updateProductOption(productOption)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates product dimentions for shipping purposes
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPut> _
    Function UpdateProductShipping(dto As Models.Product) As HttpResponseMessage
        Try
            Dim product As New Models.Product
            Dim productCtrl As New ProductsRepository

            product = productCtrl.getProduct(dto.ProductId, dto.Lang)

            product.Weight = dto.Weight
            product.Height = dto.Height
            product.Width = dto.Width
            product.Diameter = dto.Diameter
            product.Length = dto.Length
            product.CityOrigin = dto.CityOrigin
            product.ZipOrigin = dto.ZipOrigin
            product.ModifiedByUser = dto.ModifiedByUser
            product.ModifiedOnDate = dto.ModifiedOnDate

            productCtrl.updateProduct(product)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes product attribute's option
    ''' </summary>
    ''' <param name="productOptionValueId"></param>
    ''' <param name="lang"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpDelete> _
    Function removeProductOptionValue(productOptionValueId As Integer, lang As String) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            productCtrl.removeProductOptionValueLang(productOptionValueId, lang)
            productCtrl.removeProductOptionValue(productOptionValueId, lang)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    '<DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    '<HttpDelete> _
    'Function removeProductOptionValueLang(productOptionValueId As Integer) As HttpResponseMessage
    '    Try
    '        Dim productCtrl As New ProductsRepository

    '        productCtrl.removeProductOptionValueLang(productOptionValueId)

    '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
    '    Catch ex As Exception
    '        'DnnLog.Error(ex)
    '        Logger.[Error](ex)
    '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
    '    End Try
    'End Function

    ''' <summary>
    ''' Removes product from category
    ''' </summary>
    ''' <param name="productId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
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

    ''' <summary>
    ''' Removes categories from product
    ''' </summary>
    ''' <param name="categoryId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
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

    ''' <summary>
    ''' Adds product to list of categories
    ''' </summary>
    ''' <param name="productId"></param>
    ''' <param name="categories"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
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

    ''' <summary>
    ''' Adds product to category
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
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

    ''' <summary>
    ''' Updates image view order
    ''' </summary>
    ''' <param name="ProductImages"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPost> _
    Function UpdateProductImageOrder(ProductImages As List(Of Models.ProductImage)) As HttpResponseMessage
        Try
            Dim productImage As New Models.ProductImage
            Dim productImageCtrl As New ProductsRepository

            For Each image In ProductImages

                productImage = productImageCtrl.getProductImage(image.ProductImageId)

                productImage.ListOrder = image.ListOrder

                productImageCtrl.updateProductImage(productImage)

            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes product image
    ''' </summary>
    ''' <param name="productImageId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpDelete> _
    Function removeProductImage(productImageId As Integer) As HttpResponseMessage
        Try
            Dim productImageCtrl As New ProductsRepository

            productImageCtrl.removeProductImage(productImageId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets product finance info
    ''' </summary>
    ''' <param name="productId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes")> _
    <HttpGet> _
    Function GetProductFinance(productId As Integer) As HttpResponseMessage
        Try
            Dim productFinanceCtrl As New ProductsRepository

            Dim productFinanceData = productFinanceCtrl.getProductFinance(productId)

            Return Request.CreateResponse(HttpStatusCode.OK, productFinanceData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets product videos
    ''' </summary>
    ''' <param name="productId">Product ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpGet> _
    Function getProductVideos(productId As Integer) As HttpResponseMessage
        Try
            Dim productVideosCtrl As New ProductsRepository

            Dim productVideosData = productVideosCtrl.getProductVideos(productId)

            Return Request.CreateResponse(HttpStatusCode.OK, productVideosData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds product video
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPost> _
    Function addProductVideo(dto As Models.ProductVideo) As HttpResponseMessage
        Try
            Dim productVideo As New Models.ProductVideo
            Dim productVideoCtrl As New ProductsRepository

            productVideo.Alt = dto.Alt
            productVideo.AutoStart = dto.AutoStart
            productVideo.height = dto.height
            productVideo.MediaAlignment = dto.MediaAlignment
            productVideo.MediaDesc = dto.MediaDesc
            productVideo.MediaLoop = dto.MediaLoop
            productVideo.MediaType = dto.MediaType
            productVideo.ModuleId = 0
            productVideo.NavigateUrl = dto.NavigateUrl
            productVideo.NewWindow = dto.NewWindow
            productVideo.ProductId = dto.ProductId
            productVideo.Src = dto.Src
            productVideo.TrackClicks = dto.TrackClicks
            productVideo.width = dto.width

            productVideoCtrl.addProductVideo(productVideo)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .VideoId = productVideo.VideoId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes product video
    ''' </summary>
    ''' <param name="productVideoId">Product ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpDelete> _
    Function removeProductVideo(productVideoId As Integer) As HttpResponseMessage
        Try
            Dim productVideoCtrl As New ProductsRepository

            productVideoCtrl.removeProductVideo(productVideoId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    Public Class ProductSpecialOffer
        Public Property ProductId As Integer
        Public Property Lang As String
        Public Property Finan_Select As String
        Public Property Finan_Special_Price As Single
        Public Property SaleStartDate As DateTime
        Public Property SaleEndDate As DateTime
        Public Property ModifiedByUser As Integer
        Public Property ModifiedOnDate As DateTime
    End Class

End Class
