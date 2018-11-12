
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Web.Api
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.IO
Imports RIW.Modules.Common
Imports Ionic.Zip
Imports RIW.Modules.WebAPI.Components.Models
Imports RIW.Modules.WebAPI.Components.Repositories

Public Class ProductsController
    Inherits DnnApiController

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(ProductsController))

    Public Shared Function SyncSGIProducts(ByVal sDate As String, ByVal eDate As String) As Counts
        Dim productCtrl As New ProductsRepository
        Dim produtosCtrl As New SofterRepository
        Dim unitsCtrl As New UnitTypesRepository
        Dim product As New Product
        Dim existingProduct As New Product

        Dim produtos = produtosCtrl.GetSGIProducts(sDate, eDate)
        Dim addedCount = 0
        Dim updatedCount = 0

        If produtos.Count Then
            For Each produto In produtos

                product.ItemType = If(produto.tipoproduto = "P", "1", "2")
                product.PortalId = 0
                product.ProductRef = If(produto.ref IsNot Nothing, produto.ref, produto.codigo.ToString())
                product.ReorderPoint = produto.est_min
                product.ShowPrice = False

                Dim unit = unitsCtrl.GetUnitType(produto.unidadeid, 0)
                If unit Is Nothing Then
                    Dim newUnit As New UnitType() With {.CreatedByUser = 2,
                                                        .CreatedOnDate = Today(),
                                                        .IsDeleted = False,
                                                        .ModifiedByUser = 2,
                                                        .ModifiedOnDate = Today(),
                                                        .PortalId = 0,
                                                        .UnitTypeAbbv = produto.unidade,
                                                        .UnitTypeTitle = produto.unidade}
                    unitsCtrl.AddUnitType(newUnit)
                    product.ProductUnit = newUnit.UnitTypeId
                Else
                    product.ProductUnit = If(produto.unidadeid > 0, produto.unidadeid, unitsCtrl.GetUnitType("UN", 0).UnitTypeId)
                End If

                product.Barcode = produto.cod_barras
                product.QtyStockOther = produto.estoque
                product.OldId = produto.codigo

                Dim productLang As New ProductLang() With {.Lang = "pt-BR", .ProductName = produto.nome, .Manufacturer = "", .Summary = produto.desc_compl}

                Utilities.CreateDir(Utilities.GetPortalSettings(0), Constants.PRODUCTIMAGESFOLDER)
                Utilities.CreateDir(Utilities.GetPortalSettings(0), Constants.PRODUCTDOCSFOLDER)

                product.Width = 0
                product.Height = 0
                product.Weight = 0
                product.Length = 0
                product.Diameter = 0

                Dim productFinance As New ProductFinance() With {.Finan_COFINS = produto.cofins,
                                                                 .Finan_Cost = produto.custo_liq,
                                                                 .Finan_DiffICMS = produto.sub_trib,
                                                                 .Finan_DefaultBarCode = "EAN13",
                                                                 .Finan_Freight = produto.frete,
                                                                 .Finan_ICMS = produto.icms,
                                                                 .Finan_IPI = produto.ipi,
                                                                 .Finan_ISS = produto.aliquotaiss,
                                                                 .Finan_Paid = produto.custo_final,
                                                                 .Finan_Paid_Discount = produto.desconto,
                                                                 .Finan_PIS = produto.pis,
                                                                 .Finan_CFOP = produto.cfop_saida_estado,
                                                                 .Finan_SalesPerson = produto.comissao,
                                                                 .Finan_NCM = produto.ncm,
                                                                 .SGI_Group = produto.grupofiscal,
                                                                 .Trib_ECF = produto.cod_trib_ecf,
                                                                 .Finan_Sale_Price = produto.preco}

                If produto.desconto > 0 Then
                    productFinance.Finan_Special_Price = If(produto.desconto > 0, produto.preco - (produto.desconto * produto.preco / 100), 0)
                    productFinance.Finan_Select = "2"
                Else
                    productFinance.Finan_Select = "1"
                End If

                existingProduct = productCtrl.GetProduct(produto.codigo, "pt-BR")
                If existingProduct IsNot Nothing Then
                    product.ModifiedOnDate = produto.data_alteracao
                    product.Archived = existingProduct.Archived
                    product.DealerOnly = existingProduct.DealerOnly
                    product.Featured = existingProduct.Featured
                    product.CreatedByUser = existingProduct.CreatedByUser
                    product.CreatedOnDate = existingProduct.CreatedOnDate
                    product.ModifiedByUser = existingProduct.ModifiedByUser
                    product.ModifiedOnDate = existingProduct.ModifiedOnDate
                    product.IsHidden = existingProduct.IsHidden
                    product.SaleStartDate = existingProduct.SaleStartDate
                    product.SaleEndDate = existingProduct.SaleEndDate
                    product.Brand = existingProduct.Brand
                    product.BrandModel = existingProduct.BrandModel
                    product.QtyStockSet = existingProduct.QtyStockSet
                    product.ProductId = existingProduct.ProductId
                    productCtrl.UpdateProduct(product)

                    productFinance.ProductId = product.ProductId
                    productCtrl.UpdateProductFinance(productFinance)

                    productLang.ProductId = product.ProductId

                    updatedCount = updatedCount + 1
                    'Else
                    '    product.QtyStockOther = produto.estoque
                    '    product.QtyStockSet = produto.estoque
                    '    product.Archived = False
                    '    product.DealerOnly = False
                    '    product.Featured = False
                    '    product.CreatedByUser = 2
                    '    product.CreatedOnDate = produto.data_cadastro
                    '    product.ModifiedByUser = 2
                    '    product.ModifiedOnDate = produto.data_cadastro
                    '    product.IsHidden = False
                    '    product.SaleStartDate = New Date(1900, 1, 1)
                    '    product.SaleEndDate = New Date(1900, 1, 1)
                    '    product.Brand = 0
                    '    product.BrandModel = 0
                    '    productCtrl.AddProduct(product)

                    '    productFinance.ProductId = product.ProductId
                    '    productFinance.Finan_Select = "1"
                    '    productFinance.Finan_Tech = 0
                    '    productFinance.Finan_Telemarketing = 0
                    '    productFinance.Finan_TributeSubICMS = 0
                    '    productFinance.Finan_PISBase = 0
                    '    productFinance.Finan_PISTributeSub = 0
                    '    productFinance.Finan_PISTributeSubBase = 0
                    '    productFinance.Finan_Rep = 0
                    '    productFinance.Finan_Manager = 0
                    '    productFinance.Finan_MarkUp = 0
                    '    productFinance.Finan_OtherExpenses = 0
                    '    productFinance.Finan_OtherTaxes = 0
                    '    productFinance.Finan_ICMSFreight = 0
                    '    productFinance.Finan_Dealer_Price = 0
                    '    productFinance.Finan_COFINSBase = 0
                    '    productFinance.Finan_COFINSTributeSub = 0
                    '    productFinance.Finan_COFINSTributeSubBase = 0
                    '    productCtrl.AddProductFinance(productFinance)

                    '    productLang.ProductId = product.ProductId

                    '    addedCount = addedCount + 1
                End If

                If IsNumeric(produto.grupo) Then
                    If produto.grupo > 0 Then
                        Dim newCategory As New Category
                        Dim newCatLang As New CategoryLang
                        Dim categoryCtrl As New CategoriesRepository

                        Dim sgiGrupo = produtosCtrl.GetSGIProductCategory(produto.grupo)

                        If sgiGrupo IsNot Nothing Then
                            Dim newCatId = 0
                            Dim category = categoryCtrl.GetCategory(produto.grupo, "pt-BR")
                            If category IsNot Nothing Then
                                Dim catLang = categoryCtrl.GetCategoryLang(category.CategoryId, category.Lang)
                                category.Archived = category.Archived
                                category.Hidden = category.Hidden
                                category.ModifiedByUser = category.ModifiedByUser
                                category.ModifiedOnDate = category.ModifiedOnDate
                                category.ListAltItemTemplate = category.ListAltItemTemplate
                                category.ListItemTemplate = category.ListItemTemplate
                                category.ListOrder = category.ListOrder
                                category.ParentCategoryId = category.ParentCategoryId
                                category.PortalId = category.PortalId
                                category.ProductTemplate = category.ProductTemplate

                                catLang.Message = catLang.Message
                                catLang.MetaDescription = catLang.MetaDescription
                                catLang.MetaKeywords = catLang.MetaKeywords
                                catLang.CategoryName = sgiGrupo.nome
                                catLang.Lang = "pt-BR"
                                catLang.CategoryDesc = ""
                                catLang.SEOName = sgiGrupo.nome
                                catLang.SEOPageTitle = sgiGrupo.nome

                                categoryCtrl.UpdateCategory(category)
                                categoryCtrl.UpdateCategoryLang(catLang)

                                categoryCtrl.ResetCategorySecurities(category.CategoryId)

                                newCatId = category.CategoryId
                            Else

                                newCategory.CreatedByUser = 2
                                newCategory.CreatedOnDate = New Date(1900, 1, 1)
                                newCategory.Archived = False
                                newCategory.Hidden = False
                                newCategory.ModifiedByUser = 2
                                newCategory.ModifiedOnDate = Today()
                                newCategory.ListAltItemTemplate = ""
                                newCategory.ListItemTemplate = ""
                                newCategory.ListOrder = 1
                                newCategory.ParentCategoryId = 0
                                newCategory.PortalId = product.PortalId
                                newCategory.ProductTemplate = ""

                                newCatLang.Message = ""
                                newCatLang.MetaDescription = ""
                                newCatLang.MetaKeywords = ""
                                newCatLang.CategoryName = sgiGrupo.nome
                                newCatLang.Lang = "pt-BR"
                                newCatLang.CategoryDesc = ""
                                newCatLang.SEOName = ""
                                newCatLang.SEOPageTitle = ""

                                categoryCtrl.AddCategory(newCategory)

                                newCatId = newCategory.CategoryId

                                newCatLang.CategoryId = newCategory.CategoryId
                                categoryCtrl.AddCategoryLang(newCatLang)

                                Dim catPermission As New Components.Models.CategoryPermission() With {.PermissionId = 1, .CategoryId = newCategory.CategoryId}

                                For i = 0 To 1
                                    If i = 0 Then
                                        catPermission.RoleId = 9999
                                        catPermission.AllowAccess = True
                                        categoryCtrl.AddCategorySecurity(catPermission)
                                    Else
                                        catPermission.RoleId = 0
                                        catPermission.AllowAccess = False
                                        categoryCtrl.AddCategorySecurity(catPermission)
                                    End If
                                Next

                            End If

                            productCtrl.RemoveProductCategories(product.ProductId)

                            productCtrl.AddProductCategory(product.ProductId, newCatId)
                        End If
                    End If
                End If

                Dim fornecedores = produto.fornecedores.Split(","c)
                For Each fornecedor In fornecedores
                    If fornecedor.Trim() <> "" Then

                        Dim productVendor As New ProductVendor

                        Dim sgiProductVendor = produtosCtrl.GetSGIProductVendor(produto.codigo, fornecedor)

                        productVendor.ProductId = product.ProductId
                        productVendor.PersonId = sgiProductVendor.fornecedor
                        productVendor.ProductRef = sgiProductVendor.referencia
                        productVendor.DefaultVendor = sgiProductVendor.principal

                        If Not sgiProductVendor.primeira_compra = Date.MinValue Then
                            productVendor.FirstPurchase = sgiProductVendor.primeira_compra
                        Else
                            productVendor.FirstPurchase = New Date(1900, 1, 1)
                        End If

                        If Not sgiProductVendor.ultima_compra = Date.MinValue Then
                            productVendor.LastPurchase = sgiProductVendor.ultima_compra
                        Else
                            productVendor.LastPurchase = New Date(1900, 1, 1)
                        End If

                        productVendor.RefXml = sgiProductVendor.refxml

                        Dim existingProductVendor = productCtrl.GetProductVendor(produto.codigo, fornecedor.Trim())

                        If existingProductVendor IsNot Nothing Then
                            productVendor.ProductVendorId = existingProductVendor.ProductVendorId
                            productCtrl.UpdateProductVendor(productVendor)
                        Else
                            productCtrl.AddProductVendor(productVendor)
                        End If
                    End If
                Next

                'copy to languages not created yet
                productCtrl.CopyProductToLanguages(productLang, "pt-BR", False)
            Next
        End If

        Dim counts As New Counts
        counts.Added = addedCount
        counts.Updated = updatedCount

        Return counts
    End Function

    ''' <summary>
    ''' Gets products list
    ''' </summary>
    ''' <param name="portalId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous>
    <HttpGet>
    Public Function GetDeletedProductsCount(Optional portalId As Integer = 0) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository
            Dim products = productCtrl.GetDeletedProductsCount(portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.productCount = products})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

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
    ''' <param name="searchField">Field</param>
    ''' <param name="searchString">Search Term</param>
    ''' <param name="getDeleted">Deleted?</param>
    ''' <param name="onSale">On Sale?</param>
    ''' <param name="providerList">Providers</param>
    ''' <param name="categoryList"></param>
    ''' <param name="orderBy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous>
    <HttpGet>
    Public Function GetProducts(Optional portalId As Integer = 0,
                                Optional categoryId As Integer = -1,
                                Optional lang As String = "pt-BR",
                                Optional getArchived As Boolean = False,
                                Optional featuredOnly As Boolean = False,
                                Optional orderDesc As String = "",
                                Optional returnLimit As String = "",
                                Optional pageIndex As Integer = 1,
                                Optional pageSize As Integer = 10,
                                Optional onSale As String = "",
                                Optional searchDesc As Boolean = False,
                                Optional isDealer As Boolean = False,
                                Optional getDeleted As String = "",
                                Optional excludeFeatured As Boolean = False,
                                Optional searchField As String = "ProductName",
                                Optional searchString As String = "",
                                Optional providerList As String = "",
                                Optional categoryList As String = "",
                                Optional orderBy As String = "",
                                Optional sDate As String = Nothing,
                                Optional eDate As String = Nothing,
                                Optional filterDate As String = "ALL") As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            Dim searchStr As String = HttpContext.Current.Request.Params("filter[filters][0][value]")
            If Not searchStr = Nothing Then
                searchString = searchStr
            End If

            Dim products = productCtrl.GetProducts(portalId, categoryId, lang, searchField, searchString, getArchived, featuredOnly, orderBy, orderDesc, returnLimit, pageIndex, pageSize, onSale, searchDesc, isDealer, getDeleted, providerList, sDate, eDate, filterDate, categoryList, excludeFeatured)

            Dim total = 0
            If returnLimit <> "" Then
                total = returnLimit
            Else
                For Each item In products
                    total = item.TotalRows
                    Exit For
                Next
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.data = products, .total = total})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets products list
    ''' </summary>
    ''' <param name="portalId"></param>
    ''' <param name="lang"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous>
    <HttpGet>
    Public Function GetProductsAll(portalId As Integer, lang As String) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            Dim products = productCtrl.GetProductsAll(portalId, lang)

            Return Request.CreateResponse(HttpStatusCode.OK, products)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPost>
    Function AddProductCategories(productId As Integer, categories As String) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            productCtrl.AddProductCategories(productId, categories)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds product to category
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPost>
    Function AddProductCategory(dto As ProductCategory) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            productCtrl.AddProductCategory(dto.ProductId, dto.CategoryId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds product attribute
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPost>
    Function AddProductOption(dto As ProductOption) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository
            Dim productOption As New ProductOption
            Dim productOptionLang As New ProductOptionLang

            productOption.ListOrder = dto.ListOrder
            productOption.ProductId = dto.ProductId

            productOptionLang.Lang = dto.Lang
            productOptionLang.OptionDesc = dto.OptionDesc

            productCtrl.AddProductOption(productOption)

            productOptionLang.OptionId = productOption.OptionId

            'productCtrl.removeProductOptionLang(productOption.OptionId)
            'productCtrl.addProductOptionLang(productOptionLang)

            productCtrl.CopyOptionToLanguages(productOptionLang, dto.Lang, False)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .OptionId = productOption.OptionId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds product attribute value
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPost>
    Function AddProductOptionValue(dto As ProductOptionValue) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository
            Dim productOptionValue As New ProductOptionValue
            Dim productOptionValueLang As New ProductOptionValueLang

            productOptionValue.AddedCost = dto.AddedCost
            productOptionValue.ListOrder = dto.ListOrder
            productOptionValue.OptionId = dto.OptionId
            productOptionValue.QtyStockSet = dto.QtyStockSet

            productOptionValueLang.Lang = dto.Lang
            productOptionValueLang.OptionValueDesc = dto.OptionValueDesc

            productCtrl.AddProductOptionValue(productOptionValue)

            productOptionValueLang.OptionValueId = productOptionValue.OptionValueId

            'productCtrl.addProductOptionValueLang(productOptionValueLang)
            productCtrl.CopyOptionValueToLanguages(productOptionValueLang, dto.Lang, False)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .OptionValueId = productOptionValue.OptionValueId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds product related
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPost>
    Function AddProductRelated(dto As ProductRelated) As HttpResponseMessage
        Try
            Dim productRelated As New ProductRelated
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

            productCtrl.AddProductRelated(productRelated)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .RelatedId = productRelated.RelatedId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds product video
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPost>
    Function AddProductVideo(dto As ProductVideo) As HttpResponseMessage
        Try
            Dim productVideo As New ProductVideo
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

            productVideoCtrl.AddProductVideo(productVideo)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .VideoId = productVideo.VideoId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Disables a product
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPut>
    Function DeleteProduct(dto As Product) As HttpResponseMessage
        Try
            Dim product As New Product
            Dim productCtrl As New ProductsRepository

            product = productCtrl.GetProduct(dto.ProductId, dto.Lang)

            product.IsDeleted = True
            product.ModifiedByUser = dto.ModifiedByUser
            product.ModifiedOnDate = dto.ModifiedOnDate
            productCtrl.UpdateProduct(product)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Export products
    ''' </summary>
    ''' <param name="lang">Language</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function ExportAllProducts(lang As String, portalId As Integer) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository
            Dim categoriesCtrl As New CategoriesRepository

            'Check if this is a User Folder
            Dim fileDir = New System.IO.DirectoryInfo(PortalSettings.HomeDirectoryMapPath & "Products\")
            Dim path As String = fileDir.ToString
            'If fileDir.Exists = False Then
            '    'Add File folder
            '    DotNetNuke.Services.FileSystem.FolderManager.Instance().AddFolder(portalId, "Products/")
            'End If
            Utilities.CreateDir(PortalController.Instance.GetCurrentPortalSettings(), "Products")

            Dim filePath = String.Format("{0}Produtos.TXT", path)

            Dim saveString As New StringBuilder
            'saveString.Append("05".PadRight(2))
            'saveString.Append("001".PadRight(3))
            'saveString.Append("1200".PadRight(4))
            'saveString.Append("02 TRIB 18%".PadRight(30))
            'saveString.Append("4")
            'saveString.Append("".PadRight(32))
            'saveString.AppendLine()

            For Each Category In categoriesCtrl.GetCategoriesList(0, "pt-BR", -1, "", False, True)
                saveString.Append("02".PadRight(2)) ' N Identificação do tipo do registro
                saveString.Append(Category.CategoryId.ToString().PadLeft(6, "0")) ' Código do grupo
                saveString.Append(Category.CategoryName.ToUpper().PadRight(30)) ' Nome do grupo
                saveString.AppendLine()
            Next

            For Each product In productCtrl.GetProductsAll(portalId, lang)

                saveString.Append("11".PadRight(2)) 'N Identificação do tipo do registro
                saveString.Append(product.ProductId.ToString().PadLeft(6, "0")) 'N Código do produto

                If product.ProductRef.Length > 20 Then
                    saveString.Append(product.ProductRef.Substring(0, 20).PadRight(20)) 'C Referencia do fabricante
                Else
                    saveString.Append(product.ProductRef.PadRight(20)) 'C Referencia do fabricante
                End If

                If product.Barcode.Length > 13 Then
                    saveString.Append(product.Barcode.Substring(0, 13).PadRight(14)) 'N Código de barras do produto
                Else
                    saveString.Append(product.Barcode.PadRight(14)) 'N Código de barras do produto
                End If
                If product.ProductName.Length > 50 Then
                    saveString.Append(Utilities.ReplaceAccentletters(product.ProductName.Substring(0, 50)).ToUpper().PadRight(50)) 'C Descrição completa do produto
                Else
                    saveString.Append(Utilities.ReplaceAccentletters(product.ProductName).ToUpper().PadRight(50)) 'C Descrição completa do produto
                End If

                If product.CategoriesNames <> "" Then
                    saveString.Append(product.CategoriesNames.Split(":"c)(0).PadRight(6)) 'N Código do Grupo do Produto
                Else
                    saveString.Append("000000".PadRight(6)) 'N Código do Grupo do Produto
                End If
                saveString.Append(product.Finan_ICMS.ToString().Replace(",", "").PadLeft(4, "0")) 'N Aliquota de icms do produto % (sem virgula)
                saveString.Append("00000".PadRight(5)) 'N Comissão % do produto (sem virgula)
                saveString.Append(product.UnitTypeAbbv.PadRight(10)) 'C Unidade de Venda do produto
                saveString.Append("00000".PadRight(5)) 'N Lucro a ser aplicado no produto (sem virgula)
                Select Case product.Finan_ICMS
                    Case 7
                        saveString.Append("002".PadRight(3)) 'N Código da aliquota de tributação no ecf 
                    Case 12
                        saveString.Append("003".PadRight(3)) 'N Código da aliquota de tributação no ecf 
                    Case 18
                        saveString.Append("001".PadRight(3)) 'N Código da aliquota de tributação no ecf 
                    Case 25
                        saveString.Append("004".PadRight(3)) 'N Código da aliquota de tributação no ecf 
                    Case Else
                        Select Case product.Finan_CST
                            Case "041"
                                saveString.Append("008".PadRight(3)) 'N Código da aliquota de tributação no ecf 
                            Case "040"
                                saveString.Append("007".PadRight(3)) 'N Código da aliquota de tributação no ecf 
                            Case "060"
                                saveString.Append("005".PadRight(3)) 'N Código da aliquota de tributação no ecf 
                            Case Else
                                saveString.Append("006".PadRight(3)) 'N Código da aliquota de tributação no ecf 
                        End Select
                End Select
                saveString.Append("0000".PadRight(4)) 'N Aliquota de ISS (Quando Serviço)
                saveString.Append(If(product.ItemType = 1, "P", "S")) 'C Tipo do produto "P"roduto "S"erviço
                saveString.Append(product.Finan_Cost.ToString("#.000").Replace(",", "").PadLeft(12, "0")) 'N Custo liquido da mercadoria (sem virgula)
                saveString.Append(product.Finan_Cost.ToString("#.000").Replace(",", "").PadLeft(12, "0")) 'N Custo Medio do produto (sem virgula)
                saveString.Append(product.Finan_Paid.ToString("#.000").Replace(",", "").PadLeft(12, "0")) 'N Custo mais impostos (sem virgula)
                saveString.Append(product.Finan_Sale_Price.ToString("#.000").Replace(",", "").PadLeft(12, "0")) 'N Preço de venda do produto (sem virgula)
                saveString.Append("00000".PadRight(5)) 'N Desconto % para Venda  (sem virgula)
                'saveString.Append(CStr(IIf(product.Finan_Special_Price > 0,
                '                           (product.Finan_Sale_Price - (product.Finan_Sale_Price * product.Finan_Special_Price / 100)).ToString(),
                '                           "00000")).Replace(",", "").PadLeft(5, "0")) 'N Desconto % para Venda  (sem virgula)
                saveString.Append("000000".PadRight(6)) 'N Cód. Do fornecedor do produto
                saveString.Append("000000".PadRight(6)) 'N Cód. Do fabricante do produto
                If product.QtyStockSet > 0 Then
                    saveString.Append(product.QtyStockSet.ToString("#.000").Replace(",", "").PadLeft(12, "0")) 'N Saldo de Estoque Atual do Produto (sem virg.)
                Else
                    saveString.Append(String.Format("-{0}", Math.Abs(product.QtyStockSet).ToString("#.000").Replace(",", "").PadLeft(11, "0"))) 'N Saldo de Estoque Atual do Produto (sem virg.)
                End If
                saveString.Append("000000000000".PadRight(12)) 'N Limite minimo do Saldo de Estoque (sem virg.)
                saveString.Append("000000000000".PadRight(12)) 'N Limite Maximo do Saldo de Estoque (sem virg.)
                saveString.Append("00001".PadRight(5)) 'N Unidade divisora de Estoque do Produto
                saveString.Append(product.Weight.ToString("#.000").Replace(",", "").PadLeft(10, "0")) 'N Peso Padrao do Produto (sem virg.)
                saveString.Append(product.Finan_IPI.ToString().Replace(",", "").PadLeft(5, "0")) 'N Aliquota de IPI do produto % (sem virgula)
                saveString.Append(product.Finan_NCM.PadRight(8)) 'C Codigo NCM Nacional do produto
                saveString.Append(product.CreatedOnDate.ToString("ddMMyyyy").PadRight(8)) 'D Data Inicial do Produto formato DDMMAAAA
                'saveString.Append(CStr(IIf(product.Archived = True,
                '                           "0", "1")).PadRight(1)) 'C Status de Produto DESATIVADO (SIM ou NAO)
                saveString.Append("1") 'C Status de Produto DESATIVADO (0 ou 1)
                saveString.Append(product.Finan_CST.PadLeft(3, "0")) 'C Código da Situação Tributária do Produto
                saveString.Append("000000".PadRight(6)) 'N Código do SubGrupo do Produto
                saveString.Append("".PadRight(20)) 'C Localização descritiva do produto no estoque
                saveString.Append("0000".PadRight(4)) 'N Dias de validade do produto
                saveString.Append("0000".PadRight(4)) 'N Dias de Garantia do produto 
                saveString.Append("A".PadRight(1)) 'C {A}rredondamento ou {T}runcado p/ uso em NFE
                saveString.Append("P".PadRight(1)) 'C {P}ropria ou {T}erceiros p/ uso em NFE
                saveString.Append("00000".PadRight(5)) 'N MVA % sobre Subst. Tributaria  (sem virgula)
                saveString.Append(product.Finan_DiffICMS.ToString().Replace(",", "").PadLeft(5, "0")) 'N Redução % da Subst.Tributaria  (sem virgula)
                saveString.Append(product.Finan_TributeSubICMS.ToString().Replace(",", "").PadLeft(5, "0")) 'N Subst. Tributária % do produto (sem virgula)
                saveString.Append(product.Finan_PIS.Replace(",", "").PadLeft(5, "0")) 'N Pis % do produto (sem virgula)
                saveString.Append(product.Finan_COFINS.Replace(",", "").PadLeft(5, "0")) 'N Cofins % do produto (sem virgula)
                saveString.Append(product.Finan_DiffICMS.ToString().PadLeft(5, "0")) 'N Redução % de ICMS do produto (sem virgula)
                saveString.Append(product.Finan_IPI.ToString().Replace(",", "").PadLeft(5, "0")) 'N IPI % sobre o custo unitário (sem virgula)
                saveString.Append("00000".PadRight(2)) 'N Sub. Tributária % sobre o custo unit.  (sem virg.)
                saveString.Append(product.Finan_Freight.ToString().Replace(",", "").PadLeft(5, "0")) 'N Frete % sobre o custo unitário (sem virgula)
                saveString.Append("0000000000".PadRight(10)) 'N Valor R$ Desp. Acessorais sobre o custo
                saveString.Append("0000000000".PadRight(10)) 'N Valor R$ Encargos Financeiros sobre o custo
                saveString.Append("0000000000".PadRight(10)) 'N Custo R$ por unidade da Embalagem
                saveString.Append("00000".PadRight(5)) 'N Impostos Federais % no custo unitário (sem virg.)
                saveString.Append("00000".PadRight(5)) 'N Custo Fixo % no custo unitário (sem virgula)
                saveString.Append("".PadRight(50)) 'C Descrição da aplicação do produto
                saveString.Append(Utilities.RemoveHtmlTags(product.Description).PadRight(478)) 'C Observação do produto
                saveString.AppendLine()

            Next

            If System.IO.File.Exists(filePath) Then
                System.IO.File.SetAttributes(filePath, FileAttributes.Normal)
                System.IO.File.Delete(filePath)
            End If

            Using mySw = New StreamWriter(filePath, True, System.Text.Encoding.Default)
                mySw.AutoFlush = True
                mySw.Write(saveString)
            End Using

            'DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(portalId, "Products/")

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Export products
    ''' </summary>
    ''' <param name="productId">Product ID</param>
    ''' <param name="lang">Language</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function ExportProduct(productId As Integer, lang As String, portalId As Integer) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            Dim product = productCtrl.GetProduct(productId, lang)

            'Check if this is a User Folder
            Dim fileDir = New System.IO.DirectoryInfo(PortalSettings.HomeDirectoryMapPath & "Products\")
            Dim path As String = fileDir.ToString
            'If fileDir.Exists = False Then
            '    'Add File folder
            '    DotNetNuke.Services.FileSystem.FolderManager.Instance().AddFolder(portalId, "Products/")
            'End If
            Utilities.CreateDir(PortalController.Instance.GetCurrentPortalSettings(), "Products")

            Dim filePath = String.Format("{0}E{1}.TXT", path, product.ProductId.ToString("00000000"))

            If IO.File.Exists(filePath) Then
                IO.File.Delete(filePath)
            End If

            Dim prodLayout = "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}{30}{31}{32}"

            Dim info = New UTF8Encoding(True).GetBytes(String.Format(prodLayout,
                                                           product.ProductId.ToString("00000000") & Environment.NewLine,
                                                           If(product.Barcode.Length > 0, product.Barcode, "NULL") & Environment.NewLine,
                                                           If(product.Finan_DefaultBarCode.Length > 0, product.Finan_DefaultBarCode, "NULL") & Environment.NewLine,
                                                           "NULL" & Environment.NewLine,
                                                           product.ProductName & Environment.NewLine,
                                                           product.UnitTypeAbbv & Environment.NewLine,
                                                           "NULL" & Environment.NewLine,
                                                           "NULL" & Environment.NewLine,
                                                           "0" & Environment.NewLine,
                                                           "NULL" & Environment.NewLine,
                                                           "NULL" & Environment.NewLine,
                                                           "NULL" & Environment.NewLine,
                                                           product.QtyStockSet & Environment.NewLine,
                                                           product.Finan_Cost & Environment.NewLine,
                                                           If(product.SaleEndDate >= Today, product.Finan_Special_Price, product.Finan_Sale_Price) & Environment.NewLine,
                                                           "0" & Environment.NewLine,
                                                           "NULL" & Environment.NewLine,
                                                           product.Finan_TributeSituationType & Environment.NewLine,
                                                           FormatNumber(product.Finan_ICMS, 2) & Environment.NewLine,
                                                           "0" & Environment.NewLine,
                                                           product.Finan_PISTributeSituation & Environment.NewLine,
                                                           product.Finan_PISBase & Environment.NewLine,
                                                           product.Finan_PIS & Environment.NewLine,
                                                           product.Finan_PISTributeSubBase & Environment.NewLine,
                                                           product.Finan_PISTributeSub & Environment.NewLine,
                                                           product.Finan_COFINSTributeSituation & Environment.NewLine,
                                                           product.Finan_COFINSBase & Environment.NewLine,
                                                           product.Finan_COFINS & Environment.NewLine,
                                                           product.Finan_COFINSTributeSubBase & Environment.NewLine,
                                                           product.Finan_COFINSTributeSub & Environment.NewLine,
                                                           product.Finan_NCM & Environment.NewLine,
                                                           product.Finan_CST & Environment.NewLine,
                                                           "0"))

            Using fs = IO.File.Create(filePath)
                fs.Write(info, 0, info.Length)
            End Using

            DotNetNuke.Services.FileSystem.FolderManager.Instance().Synchronize(portalId, "Products/")

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .FileName = product.ProductId.ToString("00000000")})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Export products
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize>
    <HttpPost>
    Function ExportProducts(portalId As Integer) As HttpResponseMessage
        Try
            Dim ps As New PortalSettings(portalId)
            Dim productCtrl As New ProductsRepository

            Dim products = productCtrl.GetProductsAll(portalId, "pt-BR")

            'Check if this is a User Folder
            Dim fileDir = New System.IO.DirectoryInfo(PortalSettings.HomeDirectoryMapPath & "Products\")
            Dim path As String = fileDir.ToString

            'If fileDir.Exists = False Then
            '    'Add File folder
            '    DotNetNuke.Services.FileSystem.FolderManager.Instance().AddFolder(portalId, "Products/")
            'End If
            Utilities.CreateDir(PortalController.Instance.GetCurrentPortalSettings(), "Products")

            For Each product In products

                Dim filePath = String.Format("{0}E{1}.TXT", path, product.ProductId.ToString("00000000"))

                If IO.File.Exists(filePath) Then
                    IO.File.Delete(filePath)
                End If

                Dim prodLayout = "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}{30}{31}{32}"

                Dim info = New UTF8Encoding(True).GetBytes(String.Format(prodLayout,
                                                               product.ProductId.ToString("00000000") & Environment.NewLine,
                                                               If(product.Barcode.Length > 0, product.Barcode, "NULL") & Environment.NewLine,
                                                               If(product.Finan_DefaultBarCode.Length > 0, product.Finan_DefaultBarCode, "NULL") & Environment.NewLine,
                                                               "NULL" & Environment.NewLine,
                                                               product.ProductName & Environment.NewLine,
                                                               product.UnitTypeAbbv & Environment.NewLine,
                                                               "NULL" & Environment.NewLine,
                                                               "NULL" & Environment.NewLine,
                                                               "0" & Environment.NewLine,
                                                               "NULL" & Environment.NewLine,
                                                               "NULL" & Environment.NewLine,
                                                               "NULL" & Environment.NewLine,
                                                               product.QtyStockSet & Environment.NewLine,
                                                               product.Finan_Cost & Environment.NewLine,
                                                               If(product.SaleEndDate >= Today, product.Finan_Special_Price, product.Finan_Sale_Price) & Environment.NewLine,
                                                               "0" & Environment.NewLine,
                                                               "NULL" & Environment.NewLine,
                                                               product.Finan_TributeSituationType & Environment.NewLine,
                                                               FormatNumber(product.Finan_ICMS, 2) & Environment.NewLine,
                                                               "0" & Environment.NewLine,
                                                               product.Finan_PISTributeSituation & Environment.NewLine,
                                                               product.Finan_PISBase & Environment.NewLine,
                                                               product.Finan_PIS & Environment.NewLine,
                                                               product.Finan_PISTributeSubBase & Environment.NewLine,
                                                               product.Finan_PISTributeSub & Environment.NewLine,
                                                               product.Finan_COFINSTributeSituation & Environment.NewLine,
                                                               product.Finan_COFINSBase & Environment.NewLine,
                                                               product.Finan_COFINS & Environment.NewLine,
                                                               product.Finan_COFINSTributeSubBase & Environment.NewLine,
                                                               product.Finan_COFINSTributeSub & Environment.NewLine,
                                                               product.Finan_NCM & Environment.NewLine,
                                                               product.Finan_CST & Environment.NewLine,
                                                               "0"))

                Using fs = IO.File.Create(filePath)
                    fs.Write(info, 0, info.Length)
                End Using

            Next

            Using zip As New ZipFile()
                Dim files As String() = Directory.GetFiles(ps.HomeDirectoryMapPath & "Products")
                'zip.AddSelectedFiles("*.txt", ps.HomeDirectoryMapPath & "Products", False)
                zip.AddFiles(files, False, "Produtos")
                zip.Comment = "This zip was created at " & DateTime.Now.ToString("G")
                zip.MaxOutputSegmentSize = 2 * 1024 * 1024 ' 2mb
                zip.Save(ps.HomeDirectoryMapPath & "Products\Produtos.zip")
            End Using

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets product info by product id
    ''' </summary>
    ''' <param name="productId">Product ID</param>
    ''' <param name="lang">Product Language</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous>
    <HttpGet>
    Function GetProduct(productId As Integer, lang As String) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            Dim product = productCtrl.GetProduct(productId, lang)

            If product.ProductOptionsCount > 0 Then

                product.ProductOptions = productCtrl.GetProductOptions(product.ProductId, lang)

                Dim productOptionValues As New List(Of ProductOptionValue)

                For Each theOption In product.ProductOptions

                    Dim optionValues = productCtrl.GetProductOptionValues(theOption.OptionId, lang)

                    For Each theOptionValue In optionValues

                        productOptionValues.Add(theOptionValue)

                    Next

                Next

                If productOptionValues.Count > 0 Then

                    product.ProductOptionValues = productOptionValues

                End If

            End If

            If product.ProductImagesCount > 1 Then

                product.ProductImages = productCtrl.GetProductImages(product.ProductId)

            End If

            If product.ProductsRelatedCount > 0 Then

                product.ProductsRelated = productCtrl.GetProductsRelated(product.PortalId, product.ProductId, lang, Null.NullInteger, True)

            End If

            Return Request.CreateResponse(HttpStatusCode.OK, product)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets product finance info
    ''' </summary>
    ''' <param name="productId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes")>
    <HttpGet>
    Function GetProductFinance(productId As Integer) As HttpResponseMessage
        Try
            Dim productFinanceCtrl As New ProductsRepository

            Dim productFinanceData = productFinanceCtrl.GetProductFinance(productId)

            Return Request.CreateResponse(HttpStatusCode.OK, productFinanceData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets product images by product id
    ''' </summary>
    ''' <param name="productId">Product ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous>
    <HttpGet>
    Function GetProductImages(productId As Integer) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            Dim productImages = productCtrl.GetProductImages(productId)

            Return Request.CreateResponse(HttpStatusCode.OK, productImages)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <AllowAnonymous>
    <HttpGet>
    Function GetProductOptions(productId As Integer, lang As String) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            Dim productOptionsData = productCtrl.GetProductOptions(productId, lang)

            Return Request.CreateResponse(HttpStatusCode.OK, productOptionsData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <AllowAnonymous>
    <HttpGet>
    Function GetProductOptionValues(optionId As Integer, lang As String) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            Dim productOptionValuesData = productCtrl.GetProductOptionValues(optionId, lang)

            Return Request.CreateResponse(HttpStatusCode.OK, productOptionValuesData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <AllowAnonymous>
    <HttpGet>
    Function GetProductsRelated(portalId As Integer, productId As Integer, lang As String, relatedType As Integer, getAll As Boolean) As HttpResponseMessage
        Try
            Dim productsRelatedCtrl As New ProductsRepository

            Dim productsRelated = productsRelatedCtrl.GetProductsRelated(portalId, productId, lang, relatedType, getAll)

            Return Request.CreateResponse(HttpStatusCode.OK, productsRelated)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message}))
        End Try
    End Function

    ''' <summary>
    ''' Gets product videos
    ''' </summary>
    ''' <param name="productId">Product ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous>
    <HttpGet>
    Function GetProductVideos(productId As Integer) As HttpResponseMessage
        Try
            Dim productVideosCtrl As New ProductsRepository

            Dim productVideosData = productVideosCtrl.GetProductVideos(productId)

            Return Request.CreateResponse(HttpStatusCode.OK, productVideosData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPost>
    Async Function PostProductImage() As Task(Of HttpResponseMessage)
        Try
            If Not Request.Content.IsMimeMultipartContent("form-data") Then
                Throw New HttpResponseException(New HttpResponseMessage(HttpStatusCode.UnsupportedMediaType))
            End If

            'Dim portalCtrl = New Portals.PortalController()
            Dim theRequest As HttpRequestMessage = Request

            Await theRequest.Content.LoadIntoBufferAsync()
            Dim task = theRequest.Content.ReadAsMultipartAsync()
            Dim result = Await task
            Dim contents = result.Contents
            Dim httpContent As HttpContent = contents.Last()
            'Dim uploadedFileMediaType As String = httpContent.Headers.ContentType.MediaType
            'Dim _file As Stream = httpContent.ReadAsStreamAsync().Result

            Dim portalId = contents(0).ReadAsStringAsync().Result
            Dim productId = contents(1).ReadAsStringAsync().Result
            Dim createdByUser = contents(2).ReadAsStringAsync().Result
            Dim createdOnDate = contents(3).ReadAsStringAsync().Result
            'Dim maxWidth = contents(4).ReadAsStringAsync().Result
            'Dim maxHeight = contents(5).ReadAsStringAsync().Result

            Dim theGuid = Guid.NewGuid().ToString()

            Dim theFileName = If(Not String.IsNullOrWhiteSpace(httpContent.Headers.ContentDisposition.FileName), httpContent.Headers.ContentDisposition.FileName, theGuid).Replace("""", String.Empty)

            Using theFile As Stream = httpContent.ReadAsStreamAsync().Result

                'Dim settingsForImages = String.Format("width={0}&height={1}", CStr(IIf(CInt(maxWidth) > 0, maxWidth, "800")), CStr(IIf(CInt(maxHeight) > 0, maxHeight, "600")))

                'If Config.Current.Pipeline.IsAcceptedImageType(_fileName) AndAlso settingsForImages IsNot Nothing Then
                'The resizing settings can specify any of 30 commands.. See http://imageresizing.net for details.
                'Dim resizeCropSettings As New ResizeSettings(settingsForImages)

                'Dim resizeCropSettings As ResizeSettings = New ResizeSettings("width=" & CStr(IIf(CInt(maxWidth) > 0, maxWidth, "800")) & "&height=" & CStr(IIf(CInt(maxHeight) > 0, maxHeight, "600"))) ' & "&crop=auto")

                Using ms As New MemoryStream()

                    theFile.CopyTo(ms)

                    'Dim i = New ImageResizer.ImageJob() With {.CreateParentDirectory = True, .Source = _file, .Dest = ms, .Settings = resizeCropSettings}

                    Dim productImage As New ProductImage
                    Dim productImageCtrl As New ProductsRepository

                    productImage.ProductId = productId
                    productImage.ContentLength = ms.Length
                    productImage.CreatedByUser = createdByUser
                    productImage.CreatedOnDate = createdOnDate
                    productImage.Extension = Right(theFileName, 3)
                    productImage.FileName = theFileName
                    productImage.ModifiedByUser = createdByUser
                    productImage.ModifiedOnDate = createdOnDate
                    productImage.PortalId = portalId
                    productImage.ProductImageBinary = ms.ToArray()
                    productImage.ListOrder = 1

                    productImageCtrl.AddProductImage(productImage)

                    Return theRequest.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})

                End Using

                'End If

            End Using

            Return theRequest.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message}))
        End Try
    End Function

    ''' <summary>
    ''' Removes product
    ''' </summary>
    ''' <param name="productId"></param>
    ''' <param name="lang"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpDelete>
    Function RemoveProduct(productId As Integer, lang As String) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            productCtrl.RemoveProduct1(productId, lang)
            productCtrl.RemoveProductCategories(productId)
            productCtrl.RemoveProductLangs(productId)

            Dim productOptions As New List(Of ProductOption)
            productCtrl.GetProductOptions(productId, lang)
            For Each productOption In productOptions
                productCtrl.RemoveProductOption1(productOption.OptionId, productOption.Lang)
                productCtrl.RemoveProductOptionLang1(productOption.OptionId, productOption.Lang)

                Dim productOptionValues As New List(Of ProductOptionValue)
                For Each productOptionValue In productOptionValues
                    productCtrl.RemoveProductOptionValue1(productOptionValue.OptionValueId, productOptionValue.Lang)
                Next

                Dim productOptionValueLangs As New List(Of ProductOptionValueLang)
                For Each productOptionValueLang In productOptionValueLangs
                    productCtrl.RemoveProductOptionValueLang1(productOptionValueLang.OptionValueId, productOptionValueLang.Lang)
                Next
            Next

            Dim productsRelated As New List(Of ProductRelated)
            For Each productRelated In productsRelated
                productCtrl.RemoveRelatedProduct1(productRelated.RelatedId)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpDelete>
    Function RemoveProductCategory(productId As Integer) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            productCtrl.RemoveProductCategories(productId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes product image
    ''' </summary>
    ''' <param name="productImageId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpDelete>
    Function RemoveProductImage(productImageId As Integer) As HttpResponseMessage
        Try
            Dim productImageCtrl As New ProductsRepository

            productImageCtrl.RemoveProductImage1(productImageId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPost>
    Function RemoveProductOption(productOptionId As Integer, lang As String) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            productCtrl.RemoveProductOptionLang1(productOptionId, lang)
            productCtrl.RemoveProductOption1(productOptionId, lang)
            productCtrl.RemoveProductOptionValueLangs(productOptionId, lang)
            productCtrl.RemoveProductOptionValues(productOptionId, lang)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
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
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpDelete>
    Function RemoveProductOptionValue(productOptionValueId As Integer, lang As String) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            productCtrl.RemoveProductOptionValueLang1(productOptionValueId, lang)
            productCtrl.RemoveProductOptionValue1(productOptionValueId, lang)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes categories from product
    ''' </summary>
    ''' <param name="categoryId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpDelete>
    Function RemoveProductsCategory(categoryId As Integer) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            productCtrl.RemoveProductsCategory(categoryId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes all related products
    ''' </summary>
    ''' <param name="productsRelated"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpDelete>
    Function RemoveProductsRelated(productsRelated As List(Of ProductRelated)) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            For Each product In productsRelated
                productCtrl.RemoveProductsRelated(product.ProductId)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes product video
    ''' </summary>
    ''' <param name="productVideoId">Product ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpDelete>
    Function RemoveProductVideo(productVideoId As Integer) As HttpResponseMessage
        Try
            Dim productVideoCtrl As New ProductsRepository

            productVideoCtrl.RemoveProductVideo1(productVideoId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes product related
    ''' </summary>
    ''' <param name="relatedProducts"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpDelete>
    Function RemoveRelatedProduct(relatedProducts As List(Of ProductRelated)) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository

            For Each product In relatedProducts
                productCtrl.RemoveRelatedProduct1(product.RelatedId)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Restores a product
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPut>
    Function RestoreProduct(dto As Product) As HttpResponseMessage
        Try
            Dim product As New Product
            Dim productCtrl As New ProductsRepository

            product = productCtrl.GetProduct(dto.ProductId, dto.Lang)

            product.IsDeleted = False
            product.ModifiedByUser = dto.ModifiedByUser
            product.ModifiedOnDate = dto.ModifiedOnDate
            productCtrl.UpdateProduct(product)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Syncs products from sgi
    ''' </summary>
    ''' <param name="sDate">Modified Date Start</param>
    ''' <param name="eDate">Modified Date End</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPost>
    Function SyncProducts(sDate As String, eDate As String) As HttpResponseMessage
        Try
            Dim counts = SyncSGIProducts(sDate, eDate)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .Added = counts.Added, .Updated = counts.Updated})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates a product
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPost>
    Function UpdateProduct(dto As Product) As HttpResponseMessage
        Try
            Dim product As Product
            Dim productLang As New ProductLang
            Dim productCtrl As New ProductsRepository

            product = If(dto.ProductId > 0, productCtrl.GetProduct(dto.ProductId, dto.Lang), New Product())

            product.ItemType = dto.ItemType
            product.Archived = dto.Archived
            product.DealerOnly = dto.DealerOnly
            product.CreatedByUser = dto.CreatedByUser
            product.CreatedOnDate = dto.CreatedOnDate
            product.Featured = dto.Featured
            product.IsHidden = dto.IsHidden
            product.ScaleProduct = dto.ScaleProduct
            product.ModifiedByUser = dto.ModifiedByUser
            product.ModifiedOnDate = dto.ModifiedOnDate
            product.PortalId = dto.PortalId
            product.ProductRef = dto.ProductRef
            product.ReorderPoint = dto.ReorderPoint
            product.ShowPrice = dto.ShowPrice
            product.ProductUnit = If(dto.ProductUnit > 0, dto.ProductUnit, 1)
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

            Utilities.CreateDir(PortalController.Instance.GetCurrentPortalSettings(), Constants.PRODUCTIMAGESFOLDER)
            Utilities.CreateDir(PortalController.Instance.GetCurrentPortalSettings(), Constants.PRODUCTDOCSFOLDER)

            If dto.ProductId > 0 Then
                productCtrl.UpdateProduct(product)
            Else
                product.QtyStockOther = 0
                product.QtyStockSet = 0
                product.Width = 0
                product.Height = 0
                product.Weight = 0
                product.Length = 0
                product.Diameter = 0

                productCtrl.AddProduct(product)

                Dim productFinance As New ProductFinance
                productFinance.Finan_COFINS = 0
                productFinance.Finan_COFINSBase = 0
                productFinance.Finan_COFINSTributeSub = 0
                productFinance.Finan_COFINSTributeSubBase = 0
                productFinance.Finan_Cost = 0
                productFinance.Finan_DiffICMS = 0
                productFinance.Finan_Dealer_Price = 0
                productFinance.Finan_DefaultBarCode = "EAN13"
                productFinance.Finan_Freight = 0
                productFinance.Finan_ICMS = dto.Finan_ICMS
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
                productFinance.Finan_Tech = 0
                productFinance.Finan_Telemarketing = 0
                productFinance.Finan_TributeSubICMS = 0
                productFinance.Finan_TributeSubICMS = 0
                productFinance.ProductId = product.ProductId

                productCtrl.AddProductFinance(productFinance)
            End If

            'copy to languages not created yet
            productLang.ProductId = product.ProductId
            productCtrl.CopyProductToLanguages(productLang, dto.Lang, False)

            If dto.Categories <> "" Then
                dto.Categories = dto.Categories.Replace(" ", "")
                'Dim productCategory As New ProductCategory
                productCtrl.RemoveProductCategories(product.ProductId)
                For Each categoryId In dto.Categories.Split(","c)
                    If categoryId <> "" Then
                        productCtrl.AddProductCategory(product.ProductId, categoryId)
                    End If
                Next
            End If

            If dto.Barcode Is Nothing Then
                product.Barcode = product.ProductId.ToString()
            End If

            If dto.ProductRef Is Nothing Then
                product.ProductRef = product.ProductId.ToString()
            End If

            productCtrl.UpdateProduct(product)

            If dto.SyncEnabled Then

                'Dim TribEcf = "000"
                'Select Case product.Finan_ICMS
                '    Case 7
                '        TribEcf = "007"
                '    Case 12
                '        TribEcf = "012"
                '    Case 18
                '        TribEcf = "018"
                '    Case 25
                '        TribEcf = "025"
                '    Case 30
                '        TribEcf = "030"
                '    Case Else
                '        TribEcf = "000"
                'End Select

                Dim produto As New Softer
                Dim produtoCtrl As New SofterRepository

                Dim unitCtrl As New UnitTypesRepository
                produto.unidade = unitCtrl.GetUnitType(dto.ProductUnit, dto.PortalId).UnitTypeAbbv

                produto.cst = If(product.Finan_CST IsNot Nothing, product.Finan_CST, "060")

                If Not dto.ProductId > 0 Then

                    produto.nome = dto.ProductName

                    If dto.Categories <> "" Then
                        dto.Categories = dto.Categories.Replace(" ", "")
                        For Each categoryId In dto.Categories.Split(","c)
                            If categoryId <> "" Then
                                produto.grupo = categoryId
                            End If
                            Exit For
                        Next
                    Else
                        produto.grupo = -1
                    End If

                    produto.cod_barras = product.Barcode
                    produto.tipo = CStr(If(dto.ItemType, "P", "S"))
                    produto.est_min = dto.ReorderPoint
                    produto.icms = dto.Finan_ICMS
                    produto.ativobalanca = dto.ScaleProduct

                    produtoCtrl.AddSGIProducts(produto)
                Else

                    produto.aliquotaicms = product.Finan_ICMS
                    produto.aliquotaipi = product.Finan_IPI
                    produto.aliquotaiss = 0
                    produto.aplicacao = ""
                    produto.barras = product.Barcode
                    produto.c_c_custofixo = 0
                    produto.c_c_freteper = 0
                    produto.c_c_impostfed = 0
                    produto.c_c_ipi = 0
                    produto.c_c_subtrib = 0
                    produto.cadastroinicial = product.CreatedOnDate

                    If dto.Categories <> "" Then
                        dto.Categories = dto.Categories.Replace(" ", "")
                        For Each categoryId In dto.Categories.Split(","c)
                            If categoryId <> "" Then
                                produto.categoria = categoryId
                            End If
                            Exit For
                        Next
                    Else
                        produto.categoria = -1
                    End If

                    produto.cod_grupo_fiscal = product.SGI_Group
                    produto.codigo = product.ProductId
                    produto.codigoaliquotatrib = product.Trib_Ecf 'TribEcf
                    produto.codigofabricante = -1
                    produto.codigofornecedor = -1
                    produto.codigogrupo = 999
                    produto.codigosubgrupo = -1
                    produto.cofins = product.Finan_COFINS
                    produto.comissao = 0
                    produto.csosn = ""
                    produto.custobruto = If(product.Finan_Paid > 0, product.Finan_Paid, 1)
                    produto.custoliquido = If(product.Finan_Cost > 0, product.Finan_Cost, 1)
                    produto.customedio = If(product.Finan_Cost > 0, product.Finan_Cost, 1)
                    produto.desativado = Not dto.IsDeleted
                    produto.desconto = 0
                    produto.descricao = dto.ProductName
                    produto.desp_acessorias = 0
                    produto.embalagem = 0
                    produto.encargos = 0
                    produto.estoqueatual = product.QtyStockSet
                    produto.estoquemaximo = 0
                    produto.estoqueminimo = product.ReorderPoint
                    produto.garantia = 0
                    produto.iat = ""
                    produto.ippt = ""
                    produto.localizacao = ""
                    produto.lucro = 0
                    produto.naoimportarcustos = True
                    produto.naoimportarestoque = True
                    produto.nbmsh = ""
                    produto.nfe_pmvast = 0
                    produto.nfe_predbc = 0
                    produto.nfe_predbcst = 0
                    produto.obs = ""
                    produto.pesopadrao = 0
                    produto.pis = product.Finan_PIS
                    produto.preco = product.Finan_Sale_Price
                    produto.referencia = product.ProductRef
                    produto.sub_trib = product.Finan_ICMS
                    produto.tipoproduto = "P"
                    produto.unidadedivisora = 1
                    produto.validade = 0

                    produtoCtrl.UpdateSGIProducts(produto)
                End If
            End If

            'Using myConnection = New SqlConnection(Config.GetConnectionString("SGISqlServer"))

            '    myConnection.Open()

            '    Using cmd As New SqlCommand("sp_GravaProduto", myConnection)

            '        cmd.Parameters.Add("@AutoIncremento", SqlDbType.Bit).Value = 1
            '        cmd.Parameters.Add("@codigo", SqlDbType.Int).Value = 1
            '        cmd.Parameters.Add("@nome", SqlDbType.NVarChar).Value = 1
            '        cmd.Parameters.Add("@desc_compl", SqlDbType.NVarChar).Value = 1
            '        cmd.Parameters.Add("@grupo", SqlDbType.NVarChar).Value = 1
            '        cmd.Parameters.Add("@peso", SqlDbType.Float).Value = 1
            '        cmd.Parameters.Add("@ativo", SqlDbType.Bit).Value = 1
            '        cmd.Parameters.Add("@estoque", SqlDbType.Float).Value = 1
            '        cmd.Parameters.Add("@preco", SqlDbType.Float).Value = 1
            '        cmd.Parameters.Add("@custo_medio", SqlDbType.Float).Value = 1
            '        cmd.Parameters.Add("@custo_aqu", SqlDbType.Float).Value = 1
            '        cmd.Parameters.Add("@Unidade", SqlDbType.VarChar).Value = 1
            '        cmd.Parameters.Add("@AliquotaICMS", SqlDbType.Float).Value = 1
            '        cmd.Parameters.Add("@AliquotaSubTrib", SqlDbType.Float).Value = 1
            '        cmd.CommandType = CommandType.StoredProcedure
            '        cmd.ExecuteNonQuery()

            '    End Using

            '    myConnection.Close()

            'End Using

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .ProductId = product.ProductId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates a product description
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPost>
    Function UpdateProductDescription(dto As Product) As HttpResponseMessage
        Try
            Dim product As New Product
            Dim productLang As New ProductLang
            Dim productCtrl As New ProductsRepository

            productLang = productCtrl.GetProductLang(dto.ProductId, dto.Lang)

            productLang.Description = dto.Description
            productCtrl.UpdateProductLang(productLang)

            product = productCtrl.GetProduct(dto.ProductId, dto.Lang)

            product.ModifiedByUser = dto.ModifiedByUser
            product.ModifiedOnDate = dto.ModifiedOnDate
            productCtrl.UpdateProduct(product)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates product finance
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes")>
    <HttpPost>
    Function UpdateProductFinance(dto As ProductFinance) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository
            Dim productFinance As New ProductFinance

            If dto.ProductId > 0 Then
                productFinance = productCtrl.GetProductFinance(dto.ProductId)
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
            productFinance.SGI_Group = dto.SGI_Group
            productFinance.Trib_ECF = dto.Trib_ECF

            productCtrl.UpdateProductFinance(productFinance)

            If dto.SyncEnabled Then
                Dim product = productCtrl.GetProduct(dto.ProductId, "pt-BR")

                'Dim TribEcf = "000"
                'Select Case dto.Finan_ICMS
                '    Case 7
                '        TribEcf = "007"
                '    Case 12
                '        TribEcf = "012"
                '    Case 18
                '        TribEcf = "018"
                '    Case 25
                '        TribEcf = "025"
                '    Case 30
                '        TribEcf = "030"
                '    Case Else
                '        TribEcf = "000"
                'End Select

                Dim produto As New Softer
                Dim produtoCtrl As New SofterRepository

                produto.aliquotaicms = dto.Finan_ICMS
                produto.aliquotaipi = dto.Finan_IPI
                produto.aliquotaiss = 0
                produto.aplicacao = ""
                produto.barras = product.Barcode
                produto.c_c_custofixo = 0
                produto.c_c_freteper = 0
                produto.c_c_impostfed = 0
                produto.c_c_ipi = 0
                produto.c_c_subtrib = 0
                produto.cadastroinicial = product.CreatedOnDate
                produto.categoria = -1
                produto.cod_grupo_fiscal = dto.SGI_Group
                produto.codigo = product.ProductId
                produto.codigoaliquotatrib = dto.Trib_ECF 'TribEcf
                produto.codigofabricante = -1
                produto.codigofornecedor = -1

                If product.Categories <> "" Then
                    product.Categories = product.Categories.Replace(" ", "")
                    For Each categoryId In product.Categories.Split(","c)
                        If categoryId <> "" Then
                            produto.codigogrupo = categoryId
                        End If
                        Exit For
                    Next
                Else
                    produto.codigogrupo = 999
                End If

                produto.codigosubgrupo = -1
                produto.cofins = dto.Finan_COFINS
                produto.comissao = 0
                produto.csosn = ""
                produto.cst = If(dto.Finan_CST IsNot Nothing, dto.Finan_CST, "060")
                produto.custobruto = If(dto.Finan_Paid > 0, dto.Finan_Paid, 1)
                produto.custoliquido = If(dto.Finan_Cost > 0, dto.Finan_Cost, 1)
                produto.customedio = If(dto.Finan_Cost > 0, dto.Finan_Cost, 1)
                produto.desativado = Not product.IsDeleted
                produto.desconto = 0
                produto.descricao = product.ProductName
                produto.desp_acessorias = 0
                produto.embalagem = 0
                produto.encargos = 0
                produto.estoqueatual = product.QtyStockSet
                produto.estoquemaximo = 0
                produto.estoqueminimo = product.ReorderPoint
                produto.garantia = 0
                produto.iat = ""
                produto.ippt = ""
                produto.localizacao = ""
                produto.lucro = 0
                produto.naoimportarcustos = True
                produto.naoimportarestoque = True
                produto.nbmsh = ""
                produto.nfe_pmvast = 0
                produto.nfe_predbc = 0
                produto.nfe_predbcst = 0
                produto.obs = ""
                produto.pesopadrao = 0
                produto.pis = dto.Finan_PIS

                If dto.Finan_Special_Price > 0 AndAlso (Today > product.SaleStartDate And product.SaleEndDate > Today) Then
                    produto.preco = dto.Finan_Special_Price
                Else
                    produto.preco = dto.Finan_Sale_Price
                End If

                produto.referencia = CStr(If(product.ProductRef IsNot Nothing, product.ProductRef, product.ProductId))
                produto.sub_trib = dto.Finan_ICMS
                produto.tipoproduto = "P"

                Dim unitCtrl As New UnitTypesRepository
                produto.unidade = unitCtrl.GetUnitType(product.ProductUnit, product.PortalId).UnitTypeAbbv

                produto.unidadedivisora = 1
                produto.validade = 0

                produtoCtrl.UpdateSGIProducts(produto)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates image view order
    ''' </summary>
    ''' <param name="ProductImages"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPost>
    Function UpdateProductImageOrder(productImages As List(Of ProductImage)) As HttpResponseMessage
        Try
            Dim productImage As New ProductImage
            Dim productImageCtrl As New ProductsRepository

            For Each image In productImages

                productImage = productImageCtrl.GetProductImage(image.ProductImageId)

                productImage.ListOrder = image.ListOrder

                productImageCtrl.UpdateProductImage(productImage)

            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates product attribute
    ''' </summary>
    ''' <param name="productOptions"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPut>
    Function UpdateProductOption(productOptions As List(Of ProductOption)) As HttpResponseMessage
        Try
            Dim productCtrl As New ProductsRepository
            Dim productOption As New ProductOption
            Dim productOptionLang As New ProductOptionLang

            For Each attribute In productOptions
                productOption = productCtrl.GetProductOption(attribute.OptionId, attribute.Lang)

                productOption.ListOrder = attribute.ListOrder
                productOption.ProductId = attribute.ProductId

                productOptionLang.Lang = attribute.Lang
                productOptionLang.OptionDesc = attribute.OptionDesc
                productOptionLang.OptionId = attribute.OptionId

                productCtrl.UpdateProductOption(productOption)

                'productCtrl.removeProductOptionLang(attribute.OptionId, attribute.Lang)

                'productCtrl.addProductOptionLang(productOptionLang)

                productCtrl.CopyOptionToLanguages(productOptionLang, attribute.Lang, False)
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates product attribute value
    ''' </summary>
    ''' <param name="productOptionValues"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPut>
    Function UpdateProductOptionValue(productOptionValues As List(Of ProductOptionValue)) As HttpResponseMessage
        Try

            Dim productCtrl As New ProductsRepository
            Dim productOption As New ProductOption
            Dim productOptionValue As New ProductOptionValue
            Dim productOptionValueLang As New ProductOptionValueLang

            Dim totalStock = 0
            Dim optionId = 0
            Dim lang = ""

            For Each optionValue In productOptionValues
                productOptionValue = productCtrl.GetProductOptionValue(optionValue.OptionValueId, optionValue.Lang)

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

                productCtrl.UpdateProductOptionValue(productOptionValue)

                'productCtrl.removeProductOptionValueLang(optionValue.OptionValueId, optionValue.Lang)

                'productCtrl.addProductOptionValueLang(productOptionValueLang)

                productCtrl.CopyOptionValueToLanguages(productOptionValueLang, optionValue.Lang, False)
            Next

            productOption = productCtrl.GetProductOption(optionId, lang)

            productOption.QtyStockSet = totalStock

            productCtrl.UpdateProductOption(productOption)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates a product
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPost>
    Function UpdateProductSeo(dto As ProductLang) As HttpResponseMessage
        Try
            Dim product As New Product
            Dim productLang As New ProductLang
            Dim productCtrl As New ProductsRepository

            product = productCtrl.GetProduct(dto.ProductId, dto.Lang)

            product.ModifiedByUser = dto.ModifiedByUser
            product.ModifiedOnDate = dto.ModifiedOnDate

            productCtrl.UpdateProduct(product)

            productLang = productCtrl.GetProductLang(dto.ProductId, dto.Lang)

            productLang.SEOSummary = dto.SEOSummary
            productLang.SEOName = dto.SEOName
            productLang.SEOPageTitle = dto.SEOPageTitle
            productLang.TagWords = dto.TagWords

            productCtrl.UpdateProductLang(productLang)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates product dimentions for shipping purposes
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPut>
    Function UpdateProductShipping(dto As Product) As HttpResponseMessage
        Try
            Dim product As New Product
            Dim productCtrl As New ProductsRepository

            product = productCtrl.GetProduct(dto.ProductId, dto.Lang)

            product.Weight = dto.Weight
            product.Height = dto.Height
            product.Width = dto.Width
            product.Diameter = dto.Diameter
            product.Length = dto.Length
            product.CityOrigin = dto.CityOrigin
            product.ZipOrigin = dto.ZipOrigin
            product.ModifiedByUser = dto.ModifiedByUser
            product.ModifiedOnDate = dto.ModifiedOnDate

            productCtrl.UpdateProduct(product)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Updates product special offer
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes")>
    <HttpPut>
    Function UpdateProductSpecialOffer(dto As ProductSpecialOffer) As HttpResponseMessage
        Try

            Dim productCtrl As New ProductsRepository
            Dim product As New Product
            Dim productFinance As New ProductFinance

            If dto.ProductId > 0 Then
                product = productCtrl.GetProduct(dto.ProductId, dto.Lang)
                productFinance = productCtrl.GetProductFinance(dto.ProductId)
            End If

            productFinance.Finan_Select = dto.Finan_Select
            productFinance.Finan_Special_Price = dto.Finan_Special_Price

            productCtrl.UpdateProductFinance(productFinance)

            product.SaleStartDate = dto.SaleStartDate
            product.SaleEndDate = dto.SaleEndDate
            product.ModifiedByUser = dto.ModifiedByUser
            product.ModifiedOnDate = dto.ModifiedOnDate

            productCtrl.UpdateProduct(product)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds or updates a product
    ''' </summary>
    ''' <param name="products"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")>
    <HttpPost>
    Function UpdateProductStock(products As List(Of Product)) As HttpResponseMessage
        Try
            Dim product As New Product
            Dim productLang As New ProductLang
            Dim productFinance As New ProductFinance
            Dim productCtrl As New ProductsRepository

            For Each item In products
                product = productCtrl.GetProduct(item.ProductId, item.Lang)

                Dim oldStock = product.QtyStockSet

                product.ProductRef = item.ProductRef
                product.Barcode = item.Barcode
                product.QtyStockSet = oldStock + item.QtyStockSet
                product.ModifiedByUser = item.ModifiedByUser
                product.ModifiedOnDate = item.ModifiedOnDate

                productCtrl.UpdateProduct(product)

                productLang = productCtrl.GetProductLang(item.ProductId, item.Lang)
                productLang.ProductName = item.ProductName
                productCtrl.UpdateProductLang(productLang)

                productFinance = productCtrl.GetProductFinance(item.ProductId)
                productFinance.Finan_Sale_Price = item.Finan_Sale_Price
                productCtrl.UpdateProductFinance(productFinance)

                If item.SyncEnabled Then
                    'Dim TribEcf = "000"
                    'Select Case product.Finan_ICMS
                    '    Case 7
                    '        TribEcf = "007"
                    '    Case 12
                    '        TribEcf = "012"
                    '    Case 18
                    '        TribEcf = "018"
                    '    Case 25
                    '        TribEcf = "025"
                    '    Case 30
                    '        TribEcf = "030"
                    '    Case Else
                    '        TribEcf = "000"
                    'End Select

                    Dim produto As New Softer
                    Dim produtoCtrl As New SofterRepository

                    'produto.aliquotaicms = product.Finan_ICMS
                    'produto.aliquotaipi = product.Finan_IPI
                    'produto.aliquotaiss = 0
                    'produto.aplicacao = ""
                    produto.barras = product.Barcode
                    'produto.c_c_custofixo = 0
                    'produto.c_c_freteper = 0
                    'produto.c_c_impostfed = 0
                    'produto.c_c_ipi = 0
                    'produto.c_c_subtrib = 0
                    produto.cadastroinicial = product.CreatedOnDate
                    'produto.categoria = -1
                    'produto.cod_grupo_fiscal = ""
                    produto.codigo = product.ProductId
                    'produto.codigoaliquotatrib = TribEcf
                    produto.codigofabricante = -1
                    produto.codigofornecedor = -1

                    'If product.Categories <> "" Then
                    '    product.Categories = product.Categories.Replace(" ", "")
                    '    For Each categoryId In product.Categories.Split(","c)
                    '        If categoryId <> "" Then
                    '            produto.codigogrupo = categoryId
                    '        End If
                    '        Exit For
                    '    Next
                    'Else
                    '    produto.codigogrupo = 999
                    'End If

                    'produto.codigosubgrupo = -1
                    'produto.cofins = product.Finan_COFINS
                    'produto.comissao = 0
                    'produto.csosn = ""
                    'produto.cst = If(product.Finan_CST IsNot Nothing, product.Finan_CST, "060")
                    produto.custobruto = If(product.Finan_Paid > 0, product.Finan_Paid, 1)
                    produto.custoliquido = If(product.Finan_Cost > 0, product.Finan_Cost, 1)
                    produto.customedio = If(product.Finan_Cost > 0, product.Finan_Cost, 1)
                    produto.desativado = Not item.IsDeleted
                    'produto.desconto = 0
                    produto.descricao = item.ProductName
                    'produto.desp_acessorias = 0
                    'produto.embalagem = 0
                    'produto.encargos = 0
                    produto.estoqueatual = product.QtyStockSet
                    'produto.estoquemaximo = 0
                    'produto.estoqueminimo = product.ReorderPoint
                    'produto.garantia = 0
                    'produto.iat = ""
                    'produto.ippt = ""
                    'produto.localizacao = ""
                    'produto.lucro = 0
                    produto.naoimportarcustos = True
                    produto.naoimportarestoque = True
                    'produto.nbmsh = ""
                    'produto.nfe_pmvast = 0
                    'produto.nfe_predbc = 0
                    'produto.nfe_predbcst = 0
                    produto.obs = ""
                    'produto.pesopadrao = 0
                    'produto.pis = product.Finan_PIS
                    produto.preco = productFinance.Finan_Sale_Price
                    produto.referencia = CStr(If(product.ProductRef IsNot Nothing, product.ProductRef, product.ProductId))
                    'produto.sub_trib = product.Finan_ICMS
                    produto.tipoproduto = "P"

                    Dim unitCtrl As New UnitTypesRepository
                    produto.unidade = unitCtrl.GetUnitType(product.ProductUnit, product.PortalId).UnitTypeAbbv

                    'produto.unidadedivisora = 1
                    'produto.validade = 0

                    produtoCtrl.UpdateSGIProducts(produto)
                End If
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    Public Class Counts
        Property Added As Integer
        Property Updated As Integer
    End Class

    '<DnnAuthorize> _
    '<HttpPost> _
    'Function GenerateSGIFile() As HttpResponseMessage
    '    Try
    '        Dim productCtrl As New ProductsRepository
    '        Dim product As New Product

    '        'Check if this is a products Folder
    '        Dim fileDir = New System.IO.DirectoryInfo(PortalSettings.HomeDirectoryMapPath & "Products\")
    '        Dim path As String = fileDir.ToString
    '        'If fileDir.Exists = False Then
    '        '    'Add File folder
    '        '    DotNetNuke.Services.FileSystem.FolderManager.Instance().AddFolder(product.PortalId, "Products/")
    '        'End If
    '        Utilities.CreateDir(PortalController.Instance.GetCurrentPortalSettings(), "Products")

    '        Dim filePath = String.Format("{0}{1}.TXT", path, Product.ProductId.ToString())

    '        If IO.File.Exists(filePath) Then
    '            IO.File.SetAttributes(filePath, FileAttributes.Normal)
    '            IO.File.Delete(filePath)
    '        End If

    '        product = productCtrl.GetProduct(product.ProductId, "pt-BR")

    '        Dim saveString As New StringBuilder
    '        saveString.Append("05".PadRight(2))
    '        saveString.Append("001".PadRight(3))
    '        saveString.Append("1200".PadRight(4))
    '        saveString.Append("02 TRIB 18%".PadRight(30))
    '        saveString.Append("4")
    '        saveString.Append("".PadRight(32))
    '        saveString.AppendLine()
    '        saveString.Append("11".PadRight(2)) 'N Identificação do tipo do registro
    '        saveString.Append(Product.ProductId.ToString().PadLeft(6, "0")) 'N Código do produto
    '        saveString.Append("".PadRight(20)) 'C Referencia do fabricante
    '        saveString.Append(Product.Barcode.PadRight(14)) 'N Código de barras do produto
    '        saveString.Append(Utilities.ReplaceAccentletters(Product.ProductName).ToUpper().PadRight(50)) 'C Descrição completa do produto
    '        saveString.Append("000000".PadRight(6)) 'N Código do Grupo do Produto
    '        saveString.Append(Product.Finan_ICMS.ToString().Replace(",", "").PadLeft(4, "0")) 'N Aliquota de icms do produto % (sem virgula)
    '        saveString.Append("00000".PadRight(5)) 'N Comissão % do produto (sem virgula)
    '        saveString.Append(Product.UnitTypeAbbv.PadRight(10)) 'C Unidade de Venda do produto
    '        saveString.Append("00000".PadRight(5)) 'N Lucro a ser aplicado no produto (sem virgula)
    '        saveString.Append(TribEcf) 'N Código da aliquota de tributação no ecf 
    '        saveString.Append("0000".PadRight(4)) 'N Aliquota de ISS (Quando Serviço)
    '        saveString.Append(CStr(IIf(Product.ItemType = 1, "P", "S")).PadRight(1)) 'C Tipo do produto "P"roduto "S"erviço
    '        saveString.Append(Product.Finan_Cost.ToString("#.000").Replace(",", "").PadLeft(12, "0")) 'N Custo liquido da mercadoria (sem virgula)
    '        saveString.Append(Product.Finan_Cost.ToString("#.000").Replace(",", "").PadLeft(12, "0")) 'N Custo Medio do produto (sem virgula)
    '        saveString.Append(Product.Finan_Paid.ToString("#.000").Replace(",", "").PadLeft(12, "0")) 'N Custo mais impostos (sem virgula)
    '        saveString.Append(Product.Finan_Sale_Price.ToString("#.000").Replace(",", "").PadLeft(12, "0")) 'N Preço de venda do produto (sem virgula)
    '        saveString.Append(CStr(IIf(Product.Finan_Special_Price > 0,
    '                                   (Product.Finan_Sale_Price - (Product.Finan_Sale_Price * Product.Finan_Special_Price / 100)).ToString(),
    '                                   "00000")).Replace(",", "").PadLeft(5, "0")) 'N Desconto % para Venda  (sem virgula)
    '        saveString.Append("000000".PadRight(6)) 'N Cód. Do fornecedor do produto
    '        saveString.Append("000000".PadRight(6)) 'N Cód. Do fabricante do produto
    '        saveString.Append(Product.QtyStockSet.ToString("#.000").Replace(",", "").PadLeft(12, "0")) 'N Saldo de Estoque Atual do Produto (sem virg.)
    '        saveString.Append("000000000000".PadRight(12)) 'N Limite minimo do Saldo de Estoque (sem virg.)
    '        saveString.Append("000000000000".PadRight(12)) 'N Limite Maximo do Saldo de Estoque (sem virg.)
    '        saveString.Append("00001".PadRight(5)) 'N Unidade divisora de Estoque do Produto
    '        saveString.Append(Product.Weight.ToString("#.000").Replace(",", "").PadLeft(10, "0")) 'N Peso Padrao do Produto (sem virg.)
    '        saveString.Append(Product.Finan_IPI.ToString().Replace(",", "").PadLeft(5, "0")) 'N Aliquota de IPI do produto % (sem virgula)
    '        saveString.Append(Product.Finan_NCM.PadRight(8)) 'C Codigo NCM Nacional do produto
    '        saveString.Append(Product.CreatedOnDate.ToString("ddMMyyyy").PadRight(8)) 'D Data Inicial do Produto formato DDMMAAAA
    '        saveString.Append(CStr(IIf(Product.Archived = True,
    '                                   "0", "1")).PadRight(1)) 'C Status de Produto DESATIVADO (SIM ou NAO)
    '        saveString.Append(Product.Finan_CST.PadLeft(3, "0")) 'C Código da Situação Tributária do Produto
    '        saveString.Append("000000".PadRight(6)) 'N Código do SubGrupo do Produto
    '        saveString.Append("".PadRight(20)) 'C Localização descritiva do produto no estoque
    '        saveString.Append("0000".PadRight(4)) 'N Dias de validade do produto
    '        saveString.Append("0000".PadRight(4)) 'N Dias de Garantia do produto 
    '        saveString.Append("A".PadRight(1)) 'C {A}rredondamento ou {T}runcado p/ uso em NFE
    '        saveString.Append("T".PadRight(1)) 'C {P}ropria ou {T}erceiros p/ uso em NFE
    '        saveString.Append("00000".PadRight(5)) 'N MVA % sobre Subst. Tributaria  (sem virgula)
    '        saveString.Append(Product.Finan_DiffICMS.ToString().Replace(",", "").PadLeft(5, "0")) 'N Redução % da Subst.Tributaria  (sem virgula)
    '        saveString.Append(Product.Finan_TributeSubICMS.ToString().Replace(",", "").PadLeft(5, "0")) 'N Subst. Tributária % do produto (sem virgula)
    '        saveString.Append(Product.Finan_PIS.ToString().Replace(",", "").PadLeft(5, "0")) 'N Pis % do produto (sem virgula)
    '        saveString.Append(Product.Finan_COFINS.ToString().Replace(",", "").PadLeft(5, "0")) 'N Cofins % do produto (sem virgula)
    '        saveString.Append(Product.Finan_DiffICMS.ToString().PadLeft(5, "0")) 'N Redução % de ICMS do produto (sem virgula)
    '        saveString.Append(Product.Finan_IPI.ToString().Replace(",", "").PadLeft(5, "0")) 'N IPI % sobre o custo unitário (sem virgula)
    '        saveString.Append("00000".PadRight(5)) 'N Sub. Tributária % sobre o custo unit.  (sem virg.)
    '        saveString.Append(Product.Finan_Freight.ToString().Replace(",", "").PadLeft(5, "0")) 'N Frete % sobre o custo unitário (sem virgula)
    '        saveString.Append("0000000000".PadRight(10)) 'N Valor R$ Desp. Acessorais sobre o custo
    '        saveString.Append("0000000000".PadRight(10)) 'N Valor R$ Encargos Financeiros sobre o custo
    '        saveString.Append("0000000000".PadRight(10)) 'N Custo R$ por unidade da Embalagem
    '        saveString.Append("00000".PadRight(5)) 'N Impostos Federais % no custo unitário (sem virg.)
    '        saveString.Append("00000".PadRight(5)) 'N Custo Fixo % no custo unitário (sem virgula)
    '        saveString.Append("".PadRight(50)) 'C Descrição da aplicação do produto
    '        saveString.Append(Utilities.RemoveHtmlTags(Product.Description).PadRight(512)) 'C Observação do produto

    '        Dim info = New UTF8Encoding(True).GetBytes(saveString.ToString())

    '        If System.IO.File.Exists(filePath) Then
    '            System.IO.File.SetAttributes(filePath, FileAttributes.Normal)
    '            System.IO.File.Delete(filePath)
    '        End If

    '        Using fs = IO.File.Create(filePath)
    '            fs.Write(info, 0, info.Length)
    '        End Using

    '        Dim prodLayout = "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}{30}{31}{32}"

    '        Dim info = New UTF8Encoding(True).GetBytes(String.Format(prodLayout,
    '                                                       Product.ProductId.ToString(fmt) & Environment.NewLine,
    '                                                       CStr(IIf(Product.Barcode.Length > 0, Product.Barcode, "NULL")) & Environment.NewLine,
    '                                                       CStr(IIf(Product.Finan_DefaultBarCode.Length > 0, Product.Finan_DefaultBarCode, "NULL")) & Environment.NewLine,
    '                                                       "NULL" & Environment.NewLine,
    '                                                       Product.ProductName & Environment.NewLine,
    '                                                       Product.UnitTypeAbbv & Environment.NewLine,
    '                                                       "NULL" & Environment.NewLine,
    '                                                       "NULL" & Environment.NewLine,
    '                                                       "0" & Environment.NewLine,
    '                                                       "NULL" & Environment.NewLine,
    '                                                       "NULL" & Environment.NewLine,
    '                                                       "NULL" & Environment.NewLine,
    '                                                       CStr(Product.QtyStockSet) & Environment.NewLine,
    '                                                       CStr(Product.Finan_Cost) & Environment.NewLine,
    '                                                       CStr(IIf(Product.SaleEndDate >= Today, Product.Finan_Special_Price, Product.Finan_Sale_Price)) & Environment.NewLine,
    '                                                       "0" & Environment.NewLine,
    '                                                       "NULL" & Environment.NewLine,
    '                                                       Product.Finan_TributeSituationType & Environment.NewLine,
    '                                                       FormatNumber(Product.Finan_ICMS, 2) & Environment.NewLine,
    '                                                       "0" & Environment.NewLine,
    '                                                       Product.Finan_PISTributeSituation & Environment.NewLine,
    '                                                       CStr(Product.Finan_PISBase) & Environment.NewLine,
    '                                                       CStr(Product.Finan_PIS) & Environment.NewLine,
    '                                                       CStr(Product.Finan_PISTributeSubBase) & Environment.NewLine,
    '                                                       CStr(Product.Finan_PISTributeSub) & Environment.NewLine,
    '                                                       Product.Finan_COFINSTributeSituation & Environment.NewLine,
    '                                                       CStr(Product.Finan_COFINSBase) & Environment.NewLine,
    '                                                       CStr(Product.Finan_COFINS) & Environment.NewLine,
    '                                                       CStr(Product.Finan_COFINSTributeSubBase) & Environment.NewLine,
    '                                                       CStr(Product.Finan_COFINSTributeSub) & Environment.NewLine,
    '                                                       Product.Finan_NCM & Environment.NewLine,
    '                                                       Product.Finan_CST & Environment.NewLine,
    '                                                       "0"))

    '        Using fs = IO.File.Create(filePath)
    '            fs.Write(info, 0, info.Length)
    '        End Using

    '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
    '    Catch ex As Exception
    '        'DnnLog.Error(ex)
    '        logger.[Error](ex)
    '        Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
    '    End Try
    'End Function

    Public Class ProductSpecialOffer
        Public Property Finan_Select As String
        Public Property Finan_Special_Price As Single
        Public Property Lang As String
        Public Property ModifiedByUser As Integer
        Public Property ModifiedOnDate As DateTime
        Public Property ProductId As Integer
        Public Property SaleEndDate As DateTime
        Public Property SaleStartDate As DateTime
    End Class

End Class