
'Imports DotNetNuke
Imports DotNetNuke.Web.Client.ClientResourceManagement
Imports RIW.Modules.Common
Imports RIW.Modules.WebAPI.Components.Common
Imports RIW.Modules.WebAPI.Components.Models
Imports RIW.Modules.WebAPI.Components.Repositories

''' <summary>
''' The View class displays the content
''' 
''' Typically your view control would be used to display content or functionality in your module.
''' 
''' View may be the only control you have in your project depending on the complexity of your module
''' 
''' Because the control inherits from WebAPIModuleBase you have access to any custom properties
''' defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
''' 
''' </summary>
Partial Class RiwWebApi
    Inherits RiwWebApiModuleBase

    Private Const RiwModulePathScript As String = "mod_ModulePathScript"

    'Public Shared Sub ImportExcelProducts()

    '    Using strCnn As New SqlConnection("Data Source=(local);Initial Catalog=114;User ID=sa;Password=Jabuticaba.")
    '        Using strCmm As New SqlCommand() With {.Connection = strCnn, .CommandType = CommandType.Text, .CommandText = "select * from Plan1"}
    '            'strCmm.Parameters.AddWithValue("@IsDeleted", "False")
    '            strCnn.Open()
    '            Using productsReader As SqlDataReader = strCmm.ExecuteReader()
    '                Do While productsReader.Read()

    '                    Dim product As New Product
    '                    'Dim unitType As New Models.UnitType
    '                    Dim productLang As New ProductLang
    '                    Dim productCtrl As New ProductsRepository
    '                    Dim unitTypeCtrl As New UnitTypesRepository

    '                    If Not productsReader.IsDBNull(0) Then

    '                        product.ItemType = 1
    '                        product.Archived = False
    '                        product.DealerOnly = False
    '                        product.CreatedByUser = 3
    '                        product.CreatedOnDate = Now()
    '                        product.Featured = False
    '                        product.IsHidden = False
    '                        product.ModifiedByUser = 3
    '                        product.ModifiedOnDate = Now()
    '                        product.PortalId = 0
    '                        'product.ProductRef = ""
    '                        'product.ReorderPoint = 0
    '                        product.ShowPrice = False
    '                        product.ProductUnit = unitTypeCtrl.GetUnitType(productsReader.GetValue(3), 0).UnitTypeId
    '                        product.SaleStartDate = New Date(1900, 1, 1)
    '                        product.SaleEndDate = New Date(1900, 1, 1)
    '                        'product.Brand = 0
    '                        'product.BrandModel = 0
    '                        If Not productsReader.IsDBNull(1) Then
    '                            product.Barcode = productsReader.GetValue(1)
    '                        End If
    '                        'product.Vendors = ""
    '                        product.QtyStockSet = 0 'productsReader.GetValue(2)

    '                        productLang.Lang = "pt-BR"
    '                        productLang.ProductName = productsReader.GetString(2)
    '                        'productLang.Manufacturer = 0
    '                        productLang.Summary = ""

    '                        CreateDir(Portals.PortalController.Instance.GetCurrentPortalSettings(), Constants.PRODUCTIMAGESFOLDER)
    '                        CreateDir(Portals.PortalController.Instance.GetCurrentPortalSettings(), Constants.PRODUCTDOCSFOLDER)

    '                        'product.QtyStockSet = 0
    '                        'product.Width = 0
    '                        'product.Height = 0
    '                        'product.Weight = 0
    '                        'product.Length = 0
    '                        'product.Diameter = 0

    '                        productCtrl.AddProduct(product)

    '                        Dim productFinance As New ProductFinance

    '                        productFinance.Finan_COFINS = 0
    '                        productFinance.Finan_COFINSBase = 0
    '                        productFinance.Finan_COFINSTributeSub = 0
    '                        productFinance.Finan_COFINSTributeSubBase = 0
    '                        productFinance.Finan_Cost = 0
    '                        productFinance.Finan_DiffICMS = 0
    '                        productFinance.Finan_Freight = 0
    '                        productFinance.Finan_ICMS = 0
    '                        productFinance.Finan_ICMSFreight = 0
    '                        productFinance.Finan_IPI = 0
    '                        productFinance.Finan_ISS = 0
    '                        productFinance.Finan_Manager = 0
    '                        productFinance.Finan_MarkUp = 0
    '                        productFinance.Finan_OtherExpenses = 0
    '                        productFinance.Finan_OtherTaxes = 0
    '                        productFinance.Finan_Paid = 0
    '                        productFinance.Finan_Paid_Discount = 0
    '                        productFinance.Finan_PIS = 0
    '                        productFinance.Finan_PISBase = 0
    '                        productFinance.Finan_PISTributeSub = 0
    '                        productFinance.Finan_PISTributeSubBase = 0
    '                        productFinance.Finan_Rep = 0
    '                        productFinance.Finan_Sale_Price = productsReader.GetValue(4)
    '                        productFinance.Finan_SalesPerson = 0
    '                        productFinance.Finan_Select = "1"
    '                        productFinance.Finan_Special_Price = 0
    '                        productFinance.Finan_Dealer_Price = 0
    '                        productFinance.Finan_Tech = 0
    '                        productFinance.Finan_Telemarketing = 0
    '                        productFinance.Finan_TributeSubICMS = 0
    '                        productFinance.Finan_TributeSubICMS = 0
    '                        productFinance.ProductId = product.ProductId

    '                        productCtrl.AddProductFinance(productFinance)

    '                        'copy to languages not created yet
    '                        productLang.ProductId = product.ProductId
    '                        productCtrl.CopyProductToLanguages(productLang, "pt-BR", False)

    '                    End If

    '                Loop

    '            End Using
    '        End Using
    '    End Using
    'End Sub

    Public Sub ImportCategories()
        Using strCnn As New SqlClient.SqlConnection("Data Source=(local);Initial Catalog=CORETINTAS_DNN;User ID=sa;Password=Jabuticaba.")
            Using strCmm As New SqlClient.SqlCommand() With {.Connection = strCnn, .CommandType = CommandType.Text, .CommandText = "select Name, ParentId, CreatedDate, ModifiedDate, Active from ris_categories"}
                strCnn.Open()
                Using CategoriesReader As SqlClient.SqlDataReader = strCmm.ExecuteReader()
                    Do While CategoriesReader.Read()

                        Dim category As New Components.Models.Category
                        Dim catLang As New CategoryLang
                        Dim categoryCtrl As New CategoriesRepository

                        category.Archived = False
                        category.CreatedByUser = 2
                        category.CreatedOnDate = CategoriesReader.GetDateTime(2)
                        category.Hidden = False
                        category.ModifiedByUser = 2
                        category.ModifiedOnDate = CategoriesReader.GetDateTime(3)
                        category.ListAltItemTemplate = ""
                        category.ListItemTemplate = ""
                        category.ListOrder = 1
                        category.ParentCategoryId = CategoriesReader.GetValue(1)
                        category.PortalId = 0
                        category.ProductTemplate = ""

                        catLang.Message = ""
                        catLang.MetaDescription = ""
                        catLang.MetaKeywords = ""
                        catLang.CategoryName = CategoriesReader.GetString(0)
                        catLang.Lang = "pt-BR"
                        catLang.CategoryDesc = ""
                        catLang.SEOName = ""
                        catLang.SEOPageTitle = ""

                        categoryCtrl.AddCategory(category)
                        catLang.CategoryId = category.CategoryId
                        categoryCtrl.AddCategoryLang(catLang)

                        Dim catPermission As New CategoryPermission() With {.PermissionId = 1, .CategoryId = category.CategoryId}

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
                    Loop
                End Using
            End Using
        End Using
    End Sub

    'Public Sub ImportBrands()
    '    Using strCnn As New SqlClient.SqlConnection("Data Source=(local);Initial Catalog=CORETINTAS_DNN;User ID=sa;Password=Jabuticaba.")
    '        Using strCmm As New SqlClient.SqlCommand() With {.Connection = strCnn, .CommandType = CommandType.Text, .CommandText = "select BrandTitle from ris_brands"}
    '            strCnn.Open()
    '            Using BrandsReader As SqlClient.SqlDataReader = strCmm.ExecuteReader()
    '                Do While BrandsReader.Read()

    '                    Dim brand As New Models.Brand
    '                    Dim brandCtrl As New BrandsRepository

    '                    brand.BrandCode = ""
    '                    brand.BrandTitle = BrandsReader.GetString(0)
    '                    brand.CreatedByUser = 2
    '                    brand.CreatedOnDate = Now()
    '                    brand.ModifiedByUser = 2
    '                    brand.ModifiedOnDate = Now()
    '                    brand.IsDeleted = False
    '                    brand.PortalId = 0

    '                    brandCtrl.AddBrand(brand)
    '                Loop
    '            End Using
    '        End Using
    '    End Using
    'End Sub

    Public Sub ImportProducts()
        Using strCnn As New SqlClient.SqlConnection("Data Source=(local);Initial Catalog=CORETINTAS_DNN;User ID=sa;Password=Jabuticaba.")
            Using strCmm As New SqlClient.SqlCommand() With {.Connection = strCnn, .CommandType = CommandType.StoredProcedure, .CommandText = "RIS_Products_GetList"}
                strCnn.Open()
                Using ProductsReader As SqlClient.SqlDataReader = strCmm.ExecuteReader()
                    Do While ProductsReader.Read()
                        Try

                            Dim unitCtrl As New UnitTypesRepository
                            Dim brandCtrl As New BrandsRepository
                            Dim productsCtrl As New ProductsRepository
                            Dim productLang As New Components.Models.ProductLang
                            Dim product As New Components.Models.Product

                            If productsCtrl.GetProduct(ProductsReader.GetString(0).Trim(), "pt-BR") Is Nothing Then

                                product.ItemType = "1"
                                product.Archived = False
                                product.DealerOnly = False
                                product.IsDeleted = False
                                product.CreatedByUser = 2
                                product.CreatedOnDate = ProductsReader.GetDateTime(4)
                                product.Featured = False
                                product.IsHidden = False
                                product.ModifiedByUser = 2
                                product.ModifiedOnDate = ProductsReader.GetDateTime(3)
                                product.PortalId = 0
                                product.ProductRef = ProductsReader.GetString(1)
                                product.ReorderPoint = 0
                                product.ShowPrice = False
                                Dim unitType = unitCtrl.GetUnitType(ProductsReader.GetString(6), 0)
                                If unitType Is Nothing Then
                                    Dim unitsCtrl As New UnitTypesRepository
                                    Dim newUnit As New UnitType() With {.CreatedByUser = 2,
                                                        .CreatedOnDate = Today(),
                                                        .IsDeleted = False,
                                                        .ModifiedByUser = 2,
                                                        .ModifiedOnDate = Today(),
                                                        .PortalId = 0,
                                                        .UnitTypeAbbv = ProductsReader.GetString(6),
                                                        .UnitTypeTitle = ProductsReader.GetString(6)}
                                    unitsCtrl.AddUnitType(newUnit)
                                    product.ProductUnit = newUnit.UnitTypeId
                                Else
                                    product.ProductUnit = unitType.UnitTypeId
                                End If

                                product.SaleStartDate = New Date(1900, 1, 1)
                                product.SaleEndDate = New Date(1900, 1, 1)
                                product.Brand = 0
                                product.BrandModel = 0
                                product.Barcode = ProductsReader.GetString(2)
                                product.Vendors = ""
                                product.QtyStockSet = ProductsReader.GetValue(5)
                                product.Width = 0
                                product.Height = 0
                                product.Weight = 0
                                product.Length = 0
                                product.Diameter = 0

                                productLang.Lang = "pt-BR"
                                productLang.ProductName = ProductsReader.GetString(0)
                                productLang.Manufacturer = ""
                                productLang.Summary = ""

                                Utilities.CreateDir(Portals.PortalController.Instance.GetCurrentPortalSettings(), Constants.PRODUCTIMAGESFOLDER)
                                Utilities.CreateDir(Portals.PortalController.Instance.GetCurrentPortalSettings(), Constants.PRODUCTDOCSFOLDER)

                                Dim newProduct = productsCtrl.AddProduct(product)

                                If Not ProductsReader.GetString(1) <> "" Then
                                    product.ProductRef = newProduct.ProductId
                                End If

                                If Not ProductsReader.GetString(2) <> "" Then
                                    product.Barcode = newProduct.ProductId
                                End If

                                product.OldId = newProduct.ProductId

                                productsCtrl.UpdateProduct(product)

                                Dim productFinance As New Components.Models.ProductFinance

                                productFinance.Finan_CFOP = "5403"
                                productFinance.Finan_COFINS = 3
                                productFinance.Finan_COFINSBase = 0
                                productFinance.Finan_COFINSTributeSub = 0
                                productFinance.Finan_COFINSTributeSubBase = 0
                                productFinance.Finan_Cost = 0
                                productFinance.Finan_CST = "060"
                                productFinance.Finan_DiffICMS = 0
                                productFinance.Finan_Freight = 0
                                If ProductsReader.GetValue(11) > 0 Then
                                    productFinance.Finan_ICMS = ProductsReader.GetValue(11)
                                Else
                                    productFinance.Finan_ICMS = 18
                                End If

                                productFinance.Finan_ICMSFreight = 0
                                productFinance.Finan_IPI = 0
                                productFinance.Finan_ISS = 0
                                productFinance.Finan_Manager = 0
                                productFinance.Finan_MarkUp = 0
                                productFinance.Finan_OtherExpenses = 0
                                productFinance.Finan_OtherTaxes = 0
                                If Not ProductsReader.IsDBNull(11) Then
                                    productFinance.Finan_Paid = ProductsReader.GetValue(11)
                                Else
                                    productFinance.Finan_Paid = 0
                                End If
                                productFinance.Finan_Paid_Discount = 0
                                productFinance.Finan_PIS = 0.65
                                productFinance.Finan_PISBase = 0
                                productFinance.Finan_PISTributeSub = 0
                                productFinance.Finan_PISTributeSubBase = 0
                                productFinance.Finan_Rep = 0
                                productFinance.Finan_Sale_Price = ProductsReader.GetValue(12)
                                productFinance.Finan_SalesPerson = 0
                                productFinance.Finan_Select = "1"
                                productFinance.Finan_Special_Price = 0
                                productFinance.Finan_Dealer_Price = 0
                                productFinance.Finan_Tech = 0
                                productFinance.Finan_Telemarketing = 0
                                productFinance.Finan_TributeSubICMS = 0
                                productFinance.Finan_TributeSubICMS = 0
                                productFinance.ProductId = newProduct.ProductId

                                productsCtrl.AddProductFinance(productFinance)

                                productLang.ProductId = newProduct.ProductId
                                productsCtrl.CopyProductToLanguages(productLang, "pt-BR", False)

                                If ProductsReader.GetString(10) <> "" Then
                                    Dim newCategory As New Category
                                    Dim newCatLang As New CategoryLang
                                    Dim categoryCtrl As New CategoriesRepository

                                    Dim newCatId = 0
                                    Dim category = categoryCtrl.GetCategoryByName(0, ProductsReader.GetString(10))
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

                                        If catLang IsNot Nothing Then
                                            catLang.Message = ""
                                            catLang.MetaDescription = ""
                                            catLang.MetaKeywords = ""
                                            catLang.CategoryName = ProductsReader.GetString(10)
                                            catLang.Lang = "pt-BR"
                                            catLang.CategoryDesc = ""
                                            catLang.SEOName = ProductsReader.GetString(10)
                                            catLang.SEOPageTitle = ProductsReader.GetString(10)

                                            categoryCtrl.UpdateCategory(category)
                                            categoryCtrl.UpdateCategoryLang(catLang)
                                        End If

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
                                        newCatLang.CategoryName = ProductsReader.GetString(10)
                                        newCatLang.Lang = "pt-BR"
                                        newCatLang.CategoryDesc = ""
                                        newCatLang.SEOName = ProductsReader.GetString(10)
                                        newCatLang.SEOPageTitle = ProductsReader.GetString(10)

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

                                    productsCtrl.RemoveProductCategories(product.ProductId)

                                    productsCtrl.AddProductCategory(product.ProductId, newCatId)
                                End If

                                If Not ProductsReader.IsDBNull(15) Then
                                    Dim productImageCtrl As New ProductsRepository
                                    Dim productImage As New Components.Models.ProductImage

                                    productImage.ProductId = product.ProductId
                                    productImage.ContentLength = ProductsReader.GetValue(13)
                                    productImage.CreatedByUser = 2
                                    productImage.CreatedOnDate = product.CreatedOnDate
                                    productImage.Extension = ProductsReader.GetString(9)
                                    productImage.FileName = ProductsReader.GetString(14)
                                    productImage.ModifiedByUser = 2
                                    productImage.ModifiedOnDate = product.ModifiedOnDate
                                    productImage.PortalId = PortalId
                                    productImage.ProductImageBinary = ProductsReader.GetSqlBinary(15)
                                    productImage.ListOrder = 1

                                    productImageCtrl.AddProductImage(productImage)
                                End If
                            End If

                        Catch ex As Exception
                            Dim s = ex
                        End Try
                    Loop
                End Using
            End Using
        End Using
    End Sub

    'Public Sub ImportProductsData()
    '    Using dr = DotNetNuke.Data.DataProvider.Instance().ExecuteReader("RIW_ProductsData_Get")
    '        Dim productsCtrl As New ProductsRepository
    '        While dr.Read
    '            Try
    '                Dim productLang As New Models.ProductLang
    '                Dim product As New Models.Product

    '                product.ItemType = 1
    '                product.Archived = False
    '                product.DealerOnly = False
    '                product.IsDeleted = False
    '                product.CreatedByUser = 2
    '                product.CreatedOnDate = Today()
    '                product.Featured = False
    '                product.IsHidden = False
    '                product.ModifiedByUser = 2
    '                product.ModifiedOnDate = Today()
    '                product.PortalId = 0
    '                product.ProductRef = dr.GetString(0)
    '                product.ReorderPoint = 0
    '                product.ShowPrice = False

    '                Dim ut As New Models.UnitType
    '                Dim unitTypeCtrl As New UnitTypesRepository

    '                ut = unitTypeCtrl.GetUnitType(dr.GetString(2), 0)

    '                If ut Is Nothing Then
    '                    Dim unitType As New Models.UnitType

    '                    unitType.CreatedByUser = 2
    '                    unitType.CreatedOnDate = Today()
    '                    unitType.ModifiedByUser = 2
    '                    unitType.ModifiedOnDate = Today()
    '                    unitType.IsDeleted = False
    '                    unitType.PortalId = 0
    '                    unitType.UnitTypeAbbv = dr.GetString(2)
    '                    unitType.UnitTypeTitle = dr.GetString(2)

    '                    unitTypeCtrl.AddUnitType(unitType)

    '                    ut.UnitTypeId = unitType.UnitTypeId
    '                End If

    '                product.ProductUnit = ut.UnitTypeId
    '                product.SaleStartDate = New Date(1900, 1, 1)
    '                product.SaleEndDate = New Date(1900, 1, 1)
    '                'product.Brand = ProductsReader.GetValue(17)
    '                'product.BrandModel = ProductsReader.GetValue(18)
    '                'product.Barcode = 
    '                product.Vendors = ""
    '                product.QtyStockSet = dr.GetString(4)
    '                product.Width = 0
    '                product.Height = 0
    '                product.Weight = 0
    '                product.Length = 0
    '                product.Diameter = 0

    '                productLang.Lang = "pt-BR"
    '                productLang.ProductName = dr.GetString(1)
    '                productLang.Manufacturer = ""
    '                'productLang.Summary = ProductsReader.GetString(5)
    '                'productLang.Description = ProductsReader.GetString(6)

    '                CreateDir(Portals.PortalController.Instance.GetCurrentPortalSettings(), Constants.PRODUCTIMAGESFOLDER)
    '                CreateDir(Portals.PortalController.Instance.GetCurrentPortalSettings(), Constants.PRODUCTDOCSFOLDER)

    '                productsCtrl.addProduct(product)

    '                Dim productFinance As New Models.ProductFinance

    '                productFinance.Finan_CST = dr.GetString(5)
    '                productFinance.Finan_COFINS = 0
    '                productFinance.Finan_COFINSBase = 0
    '                productFinance.Finan_COFINSTributeSub = 0
    '                productFinance.Finan_COFINSTributeSubBase = 0
    '                productFinance.Finan_Cost = 0
    '                productFinance.Finan_DiffICMS = 0
    '                productFinance.Finan_Freight = 0
    '                productFinance.Finan_ICMS = 0
    '                productFinance.Finan_ICMSFreight = 0
    '                productFinance.Finan_IPI = 0
    '                productFinance.Finan_ISS = 0
    '                productFinance.Finan_Manager = 0
    '                productFinance.Finan_MarkUp = 0
    '                productFinance.Finan_OtherExpenses = 0
    '                productFinance.Finan_OtherTaxes = 0
    '                productFinance.Finan_Paid = 0
    '                productFinance.Finan_Paid_Discount = 0
    '                productFinance.Finan_PIS = 0
    '                productFinance.Finan_PISBase = 0
    '                productFinance.Finan_PISTributeSub = 0
    '                productFinance.Finan_PISTributeSubBase = 0
    '                productFinance.Finan_Rep = 0
    '                productFinance.Finan_Sale_Price = dr.GetString(3)
    '                productFinance.Finan_SalesPerson = 0
    '                productFinance.Finan_Select = "1"
    '                productFinance.Finan_Special_Price = 0
    '                productFinance.Finan_Dealer_Price = 0
    '                productFinance.Finan_Tech = 0
    '                productFinance.Finan_Telemarketing = 0
    '                productFinance.Finan_TributeSubICMS = 0
    '                productFinance.Finan_TributeSubICMS = 0
    '                productFinance.ProductId = product.ProductId

    '                productsCtrl.addProductFinance(productFinance)

    '                productLang.ProductId = product.ProductId
    '                productsCtrl.CopyProductToLanguages(productLang, "pt-BR", False)
    '            Catch ex As Exception
    '                Dim s = ex
    '            End Try
    '        End While
    '    End Using
    'End Sub

    'Public Sub ImportClients()
    '    Using strCnn As New SqlClient.SqlConnection("Data Source=(local);Initial Catalog=CORETINTAS_DNN;User ID=sa;Password=Jabuticaba.")
    '        Using strCmm As New SqlClient.SqlCommand() With {.Connection = strCnn, .CommandType = CommandType.StoredProcedure, .CommandText = "RIS_Clients_GetList"}
    '            strCnn.Open()
    '            Using ClientsReader As SqlClient.SqlDataReader = strCmm.ExecuteReader()
    '                Do While ClientsReader.Read()
    '                    Try

    '                        Dim statusCtl As New StatusesRepository
    '                        Dim person As New Models.Person
    '                        Dim personHistory As New Models.PersonHistory
    '                        Dim personHistoryCtrl As New PersonHistoriesRepository
    '                        Dim address As New Models.PersonAddress
    '                        Dim personCtrl As New PeopleRepository
    '                        Dim addressCtrl As New PersonAddressesRepository

    '                        person.DateFounded = CDate("1900-01-01 00:00:00")
    '                        person.DateRegistered = CDate("1900-01-01 00:00:00")
    '                        person.PortalId = 0
    '                        person.OldId = ClientsReader.GetValue(0)
    '                        person.DisplayName = ""
    '                        If Not ClientsReader.IsDBNull(1) Then
    '                            person.CompanyName = ClientsReader.GetString(1)
    '                            person.DisplayName = ClientsReader.GetString(1)
    '                            If Not ClientsReader.IsDBNull(2) Then
    '                                person.FirstName = ClientsReader.GetString(2)
    '                                person.DisplayName += String.Format("{0}-{0}{1}", Space(1), ClientsReader.GetString(2))
    '                                If Not ClientsReader.IsDBNull(3) Then
    '                                    person.LastName = ClientsReader.GetString(3)
    '                                    person.DisplayName += String.Format("{0}{1}", Space(1), ClientsReader.GetString(3))
    '                                End If
    '                            End If
    '                        Else
    '                            If Not ClientsReader.IsDBNull(2) Then
    '                                person.FirstName = ClientsReader.GetString(2)
    '                                person.DisplayName += ClientsReader.GetString(2)
    '                                If Not ClientsReader.IsDBNull(3) Then
    '                                    person.LastName = ClientsReader.GetString(3)
    '                                    person.DisplayName += String.Format("{0}{1}", Space(1), ClientsReader.GetString(3))
    '                                End If
    '                            End If
    '                        End If

    '                        If Not ClientsReader.IsDBNull(4) Then
    '                            person.Telephone = ClientsReader.GetString(4).Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
    '                        End If
    '                        If Not ClientsReader.IsDBNull(5) Then
    '                            person.Cell = ClientsReader.GetString(5).Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
    '                        End If
    '                        If Not ClientsReader.IsDBNull(6) Then
    '                            person.Fax = ClientsReader.GetString(6).Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
    '                        End If
    '                        If Not ClientsReader.IsDBNull(7) Then
    '                            person.Zero800s = ClientsReader.GetString(7).Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
    '                        End If
    '                        If Not ClientsReader.IsDBNull(8) Then
    '                            person.Email = ClientsReader.GetString(8).Trim().ToLower()
    '                        End If
    '                        If Not ClientsReader.IsDBNull(9) Then
    '                            person.EIN = ClientsReader.GetString(9)
    '                        End If
    '                        If Not ClientsReader.IsDBNull(10) Then
    '                            person.CPF = ClientsReader.GetString(10)
    '                        End If
    '                        person.SalesRep = 3
    '                        person.IsDeleted = ClientsReader.GetBoolean(11)
    '                        person.CreatedByUser = 2
    '                        person.CreatedOnDate = ClientsReader.GetDateTime(12)
    '                        person.ModifiedByUser = 2
    '                        person.ModifiedOnDate = ClientsReader.GetDateTime(13)
    '                        person.PersonType = ClientsReader.GetValue(14)
    '                        If Not ClientsReader.IsDBNull(15) Then
    '                            person.Website = ClientsReader.GetString(15).Trim().ToLower()
    '                        End If
    '                        If Not ClientsReader.IsDBNull(16) Then
    '                            person.Ident = ClientsReader.GetString(16).Trim().ToLower()
    '                        End If
    '                        If Not ClientsReader.IsDBNull(17) Then
    '                            person.StateTax = ClientsReader.GetString(17).Trim().ToLower()
    '                        End If
    '                        If Not ClientsReader.IsDBNull(18) Then
    '                            person.CityTax = ClientsReader.GetString(18).Trim().ToLower()
    '                        End If
    '                        If Not ClientsReader.IsDBNull(19) Then
    '                            person.Comments = ClientsReader.GetString(19)
    '                        End If
    '                        If Not ClientsReader.IsDBNull(20) Then
    '                            person.RegisterTypes = ClientsReader.GetString(20)
    '                        End If

    '                        person.StatusId = statusCtl.getStatus(ClientsReader.GetString(21), 0).StatusId
    '                        'person.Activities = ClientsReader.GetString(16)

    '                        personCtrl.addPerson(person)

    '                        personHistory.PersonId = person.PersonId
    '                        personHistory.HistoryText = "<p>Conta gerada.</p>"
    '                        personHistory.CreatedByUser = -1
    '                        personHistory.CreatedOnDate = ClientsReader.GetDateTime(12)

    '                        personHistoryCtrl.addPersonHistory(personHistory)

    '                    Catch ex As Exception
    '                        Dim s = ex
    '                    End Try
    '                Loop
    '            End Using
    '        End Using
    '    End Using
    'End Sub

    'Public Sub ImportClientsAddresses()
    '    Using strCnn As New SqlClient.SqlConnection("Data Source=(local);Initial Catalog=CORETINTAS_DNN;User ID=sa;Password=Jabuticaba.")
    '        Using strCmm As New SqlClient.SqlCommand() With {.Connection = strCnn, .CommandType = CommandType.StoredProcedure, .CommandText = "RIS_Clients_Addresses_Get"}
    '            strCnn.Open()
    '            Using ClientsAddressesReader As SqlClient.SqlDataReader = strCmm.ExecuteReader()

    '                Dim peopleCtrl As New PeopleRepository
    '                Dim people = peopleCtrl.getPeople(0, Null.NullInteger, Null.NullInteger, "", "", Null.NullInteger, "", Null.NullDate, Null.NullDate, "2", 1, 2000, "")

    '                Do While ClientsAddressesReader.Read()

    '                    Dim address As New Models.PersonAddress
    '                    Dim addressCtrl As New PersonAddressesRepository

    '                    For Each person In people
    '                        If person.OldId.Equals(ClientsAddressesReader.GetValue(0)) Then
    '                            address.PersonId = person.PersonId
    '                            If Not ClientsAddressesReader.IsDBNull(1) Then
    '                                address.AddressName = ClientsAddressesReader.GetString(1)
    '                            End If
    '                            If Not ClientsAddressesReader.IsDBNull(3) Then
    '                                address.Street = ClientsAddressesReader.GetString(3)
    '                            End If
    '                            If Not ClientsAddressesReader.IsDBNull(4) Then
    '                                address.Unit = ClientsAddressesReader.GetString(4)
    '                            End If
    '                            If Not ClientsAddressesReader.IsDBNull(5) Then
    '                                address.Complement = ClientsAddressesReader.GetString(5)
    '                            End If
    '                            If Not ClientsAddressesReader.IsDBNull(6) Then
    '                                address.District = ClientsAddressesReader.GetString(6)
    '                            End If
    '                            If Not ClientsAddressesReader.IsDBNull(7) Then
    '                                address.City = ClientsAddressesReader.GetString(7)
    '                            End If
    '                            If Not ClientsAddressesReader.IsDBNull(8) Then
    '                                address.Region = ClientsAddressesReader.GetString(8)
    '                            End If
    '                            If Not ClientsAddressesReader.IsDBNull(2) Then
    '                                address.PostalCode = ClientsAddressesReader.GetString(2)
    '                            End If
    '                            If Not ClientsAddressesReader.IsDBNull(10) Then
    '                                address.Telephone = ClientsAddressesReader.GetString(10)
    '                            End If
    '                            If Not ClientsAddressesReader.IsDBNull(11) Then
    '                                address.Cell = ClientsAddressesReader.GetString(11)
    '                            End If
    '                            If Not ClientsAddressesReader.IsDBNull(12) Then
    '                                address.Fax = ClientsAddressesReader.GetString(12)
    '                            End If
    '                            address.Country = "Brasil"
    '                            address.ModifiedByUser = 2
    '                            If Not ClientsAddressesReader.IsDBNull(18) Then
    '                                address.ModifiedOnDate = ClientsAddressesReader.GetDateTime(18)
    '                            End If
    '                            If Not ClientsAddressesReader.IsDBNull(13) Then
    '                                address.ViewOrder = ClientsAddressesReader.GetValue(13)
    '                            End If
    '                            address.CreatedByUser = 2
    '                            If Not ClientsAddressesReader.IsDBNull(16) Then
    '                                address.CreatedOnDate = ClientsAddressesReader.GetDateTime(16)
    '                            End If

    '                            addressCtrl.addPersonAddress(address)
    '                        End If
    '                    Next
    '                Loop
    '            End Using
    '        End Using
    '    End Using
    'End Sub

    'Public Sub ImportClientsContacts()
    '    Using strCnn As New SqlClient.SqlConnection("Data Source=(local);Initial Catalog=CORETINTAS_DNN;User ID=sa;Password=Jabuticaba.")
    '        Using strCmm As New SqlClient.SqlCommand() With {.Connection = strCnn, .CommandType = CommandType.StoredProcedure, .CommandText = "RIS_Clients_Contacts_Get"}
    '            strCnn.Open()
    '            Using ClientsContactsReader As SqlClient.SqlDataReader = strCmm.ExecuteReader()

    '                Dim peopleCtrl As New PeopleRepository
    '                Dim people = peopleCtrl.getPeople(0, Null.NullInteger, Null.NullInteger, "", "", Null.NullInteger, "", Null.NullDate, Null.NullDate, "2", 1, 2000, "")

    '                Do While ClientsContactsReader.Read()

    '                    Dim clientContact As New Models.PersonContact
    '                    Dim clientContactCtrl As New PersonContactsRepository

    '                    For Each person In people
    '                        If person.OldId.Equals(ClientsContactsReader.GetValue(0)) Then
    '                            clientContact.PersonId = person.PersonId
    '                            clientContact.ContactName = ClientsContactsReader.GetString(1)
    '                            clientContact.DateBirth = CDate("1900-01-01 00:00:00")
    '                            clientContact.Dept = ClientsContactsReader.GetString(9)
    '                            clientContact.ContactEmail1 = ClientsContactsReader.GetString(2)
    '                            clientContact.ContactEmail2 = ClientsContactsReader.GetString(3)
    '                            clientContact.ContactPhone1 = ClientsContactsReader.GetString(4)
    '                            clientContact.ContactPhone2 = ClientsContactsReader.GetString(5)
    '                            clientContact.PhoneExt1 = ClientsContactsReader.GetString(6)
    '                            clientContact.PhoneExt2 = ClientsContactsReader.GetString(7)
    '                            clientContact.Comments = ClientsContactsReader.GetString(10)
    '                            clientContact.PersonAddressId = 0
    '                            clientContact.CreatedByUser = 2
    '                            clientContact.CreatedOnDate = ClientsContactsReader.GetDateTime(12)
    '                            clientContact.ModifiedByUser = 2
    '                            clientContact.ModifiedOnDate = ClientsContactsReader.GetDateTime(13)

    '                            clientContactCtrl.addPersonContact(clientContact)
    '                        End If
    '                    Next
    '                Loop
    '            End Using
    '        End Using
    '    End Using
    'End Sub

    'Public Sub ImportEstimates()
    '    Using strCnn As New SqlClient.SqlConnection("Data Source=(local);Initial Catalog=106;User ID=sa;Password=Jabuticaba.")
    '        Using strCmm As New SqlClient.SqlCommand() With {.Connection = strCnn, .CommandType = CommandType.StoredProcedure, .CommandText = "RIS_Estimates_GetList"}
    '            strCnn.Open()
    '            Using EstimatesReader As SqlClient.SqlDataReader = strCmm.ExecuteReader()

    '                'Dim oldPeople = DotNetNuke.Data.DataProvider.Instance().ExecuteReader("")
    '                'Dim oldProducts = DotNetNuke.Data.DataProvider.Instance().ExecuteReader("")
    '                'Dim oldStatuses = DotNetNuke.Data.DataProvider.Instance().ExecuteReader("RIS_Statuses_Get")
    '                'Dim oldEstimates = DotNetNuke.Data.DataProvider.Instance().ExecuteReader("RIS_Estimates_Get")
    '                'Dim oldEstimatesHistory = DotNetNuke.Data.DataProvider.Instance().ExecuteReader("")
    '                'Dim oldEstimatesComments = DotNetNuke.Data.DataProvider.Instance().ExecuteReader("")

    '                Dim newStatusesCtrl As New StatusesRepository
    '                'Dim newStatuses = newStatusesCtrl.getStatuses(0, False)

    '                Dim newProductsCtrl As New ProductsRepository
    '                'Dim newProducts = newProductsCtrl.getProducts(0, Null.NullInteger, "pt-BR", "", False, False, "", "", "", 1, 10000, "", False, False, "", "", "", False)

    '                Dim newPeopleCtrl As New PeopleRepository
    '                Dim newPeople = newPeopleCtrl.getPeople(0, Null.NullInteger, Null.NullInteger, "", "", Null.NullInteger, "", Null.NullDate, Null.NullDate, "", 1, 10000, "")

    '                Dim estimateCtrl As New EstimatesRepository

    '                Do While EstimatesReader.Read
    '                    Try
    '                        Dim estimate As New Models.Estimate

    '                        estimate.OldId = EstimatesReader.GetValue(1)
    '                        estimate.StatusId = EstimatesReader.GetValue(3)
    '                        estimate.PortalId = 0

    '                        For Each person In newPeople
    '                            If person.OldId.Equals(EstimatesReader.GetValue(2)) Then
    '                                estimate.PersonId = person.PersonId
    '                            End If
    '                        Next

    '                        estimate.EstimateTitle = ""
    '                        estimate.Guid = EstimatesReader.GetString(7)
    '                        estimate.SalesRep = 8
    '                        estimate.ViewPrice = True
    '                        estimate.Discount = 0
    '                        estimate.TotalAmount = EstimatesReader.GetValue(8)
    '                        estimate.CreatedByUser = 8
    '                        estimate.CreatedOnDate = EstimatesReader.GetDateTime(4)
    '                        estimate.ModifiedByUser = 8
    '                        estimate.ModifiedOnDate = EstimatesReader.GetDateTime(5)
    '                        If Not EstimatesReader.IsDBNull(9) Then
    '                            estimate.PayCondDisc = EstimatesReader.GetValue(9)
    '                        End If
    '                        If Not EstimatesReader.IsDBNull(10) Then
    '                            estimate.PayCondIn = EstimatesReader.GetValue(10)
    '                        End If
    '                        If Not EstimatesReader.IsDBNull(11) Then
    '                            estimate.PayCondInst = EstimatesReader.GetValue(11)
    '                        End If
    '                        If Not EstimatesReader.IsDBNull(12) Then
    '                            estimate.PayCondN = EstimatesReader.GetValue(12)
    '                        End If
    '                        If Not EstimatesReader.IsDBNull(13) Then
    '                            estimate.PayCondPerc = EstimatesReader.GetValue(13)
    '                        End If
    '                        If Not EstimatesReader.IsDBNull(14) Then
    '                            estimate.PayCondType = EstimatesReader.GetString(14)
    '                        End If
    '                        If Not EstimatesReader.IsDBNull(15) Then
    '                            estimate.PayCondInterval = EstimatesReader.GetValue(15)
    '                        End If
    '                        If Not EstimatesReader.IsDBNull(16) Then
    '                            estimate.TotalPayments = EstimatesReader.GetValue(16)
    '                        End If
    '                        If Not EstimatesReader.IsDBNull(17) Then
    '                            estimate.TotalPayCond = EstimatesReader.GetValue(17)
    '                        End If

    '                        estimateCtrl.AddEstimate(estimate)

    '                    Catch ex As Exception
    '                        Dim s = ex
    '                    End Try
    '                Loop
    '            End Using
    '        End Using
    '    End Using
    'End Sub

    'Public Sub ImportEstimatesItems()
    '    Using strCnn As New SqlClient.SqlConnection("Data Source=(local);Initial Catalog=106;User ID=sa;Password=Jabuticaba.")
    '        Using strCmm As New SqlClient.SqlCommand() With {.Connection = strCnn, .CommandType = CommandType.StoredProcedure, .CommandText = "RIS_EstimatesItems_GetList"}
    '            strCnn.Open()
    '            Using EstimatesItemsReader As SqlClient.SqlDataReader = strCmm.ExecuteReader()

    '                Dim estimatesCtrl As New EstimatesRepository
    '                Dim estimates = estimatesCtrl.GetEstimates(0, Null.NullInteger, Null.NullInteger, Null.NullInteger, Null.NullInteger, Null.NullDate, Null.NullDate, "", "EstimateTitle", True, "", 1, 10000, "", "")

    '                Dim productsCtrl As New ProductsRepository
    '                Dim products = productsCtrl.getProducts(0, Null.NullInteger, "pt-BR", "", False, False, "", "", "", 1, 10000, "", False, False, "", "", "", False)

    '                Dim estimateCtrl As New EstimatesRepository

    '                Do While EstimatesItemsReader.Read
    '                    Try
    '                        Dim estimateItem As New Models.EstimateItem

    '                        For Each est In estimates
    '                            If est.OldId.Equals(EstimatesItemsReader.GetValue(0)) Then
    '                                estimateItem.EstimateId = est.EstimateId
    '                            End If
    '                        Next

    '                        For Each prod In products
    '                            If prod.OldId.Equals(EstimatesItemsReader.GetValue(1)) Then
    '                                estimateItem.ProductId = prod.ProductId
    '                            End If
    '                        Next

    '                        estimateItem.ProductQty = EstimatesItemsReader.GetValue(7)
    '                        estimateItem.ProductEstimateOriginalPrice = EstimatesItemsReader.GetValue(4)
    '                        estimateItem.ProductEstimatePrice = EstimatesItemsReader.GetValue(5)
    '                        estimateItem.CreatedByUser = 2
    '                        estimateItem.CreatedOnDate = EstimatesItemsReader.GetDateTime(2)
    '                        estimateItem.ModifiedByUser = 2
    '                        estimateItem.ModifiedOnDate = EstimatesItemsReader.GetDateTime(3)

    '                        estimateCtrl.AddEstimateItem(estimateItem)

    '                    Catch ex As Exception
    '                        Dim s = ex
    '                    End Try
    '                Loop
    '            End Using
    '        End Using
    '    End Using
    'End Sub

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        ClientResourceManager.RegisterScript(Parent.Page, "DesktopModules/RildoInfo/WebAPI/riw_webapi.js", 99)
        'RegisterModuleInfo()
    End Sub

    'Public Sub RegisterModuleInfo()

    '    'Dim modCtrl As New Entities.Modules.ModuleController()

    '    Dim scriptblock = New StringBuilder()
    '    scriptblock.Append("<script>")
    '    scriptblock.Append([String].Format("var _ascxVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(Request.PhysicalApplicationPath & "DesktopModules\RildoInfo\" & Me.ModuleConfiguration.DesktopModule.FolderName & "\views\riw_webapi.ascx").ToString("g")))
    '    scriptblock.Append([String].Format("var _jsVersion = ""{0}""; ", System.IO.File.GetLastWriteTime(Request.PhysicalApplicationPath & "DesktopModules\RildoInfo\" & Me.ModuleConfiguration.DesktopModule.FolderName & "\viewmodels\riw_webapi.js").ToString("g")))
    '    scriptblock.Append("</script>")

    '    ' register scripts
    '    If Not Page.ClientScript.IsClientScriptBlockRegistered(RiwModulePathScript) Then
    '        Page.ClientScript.RegisterClientScriptBlock(GetType(Page), RiwModulePathScript, scriptblock.ToString())
    '    End If
    'End Sub

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not UserInfo.IsSuperUser Then
            btnSubmit.Visible = False
        End If
        If Not Page.IsPostBack Then
            WritePortalIdCookie()
        End If
    End Sub

    Protected Sub WritePortalIdCookie()
        Dim cookie As HttpCookie = HttpContext.Current.Request.Cookies("myPs")

        If cookie Is Nothing Then
            cookie = New HttpCookie("myPs")
        End If

        Dim cookieValue = PortalId.ToString()

        cookie("PortalId") = cookieValue

        HttpContext.Current.Response.Cookies.Add(cookie)
    End Sub

    Private Sub BtnSubmitClick(sender As Object, e As EventArgs) Handles btnSubmit.Click
        'ImportExcelProducts()
        'ImportCategories()
        'ImportBrands()
        ImportProducts()
        'ImportClients()
        'ImportClientsAddresses()
        'ImportClientsContacts()
    End Sub

    '    Public Class Estimate_Info

    '#Region "Estimate"

    '        Public Property ClientID() As Integer
    '        Public Property CommentText() As String
    '        Public Property CreatedByUser() As Integer
    '        Public Property CreatedDate() As DateTime
    '        Public Property CurrentTimestamp As String
    '        Public Property Description() As String
    '        Public Property Discount() As Double
    '        Public Property DisplayName() As String
    '        Public Property EstDiscount() As Integer
    '        Public Property EstimateDetailID() As Integer
    '        Public Property EstimateID() As Integer
    '        Public Property EstProdOriginalPrice() As Single
    '        Public Property EstProdPrice() As Single
    '        Public Property ExtendedAmount() As Single
    '        Public Property Finan_Cost() As Single
    '        Public Property Finan_Rep() As Single
    '        Public Property Finan_SalesPerson() As Single
    '        Public Property Finan_Tech() As Single
    '        Public Property Finan_Telemarketing() As Single
    '        Public Property GUID() As String
    '        Public Property HistoryText() As String
    '        Public Property Inst() As String
    '        Public Property IsDeleted() As Boolean
    '        Public Property Locked() As Boolean
    '        Public Property ModifiedByUser() As Integer
    '        Public Property ModifiedDate() As DateTime
    '        Public Property MsgText() As String
    '        'Public Property OrderQuant() As String
    '        Public Property PayCondDisc() As Double
    '        Public Property PayCondIn() As Single
    '        Public Property PayCondInst() As Single
    '        Public Property PayCondInterval() As Integer
    '        Public Property PayCondN() As Integer
    '        Public Property PayCondPerc() As Double
    '        Public Property PayCondType() As String
    '        Public Property PayForm() As Integer
    '        Public Property PayOption() As Integer
    '        Public Property PortalId() As Integer
    '        Public Property POSels() As String
    '        Public Property POSelsText() As String
    '        Public Property Price() As Single
    '        Public Property ProdBarCode() As String
    '        Public Property ProdDesc() As String
    '        Public Property ProdID() As Integer
    '        Public Property ProdImagesId() As Integer
    '        Public Property Extension() As String
    '        Public Property ProdImageBinary() As Byte()
    '        Public Property ProdImageUrl As String
    '        Public Property ProdIntro() As String
    '        Public Property ProdName() As String
    '        Public Property ProdRef() As String
    '        Public Property ProdUnit() As Integer
    '        Public Property Qty() As Decimal
    '        Public Property RemoveReasonID() As String
    '        Public Property SaleEndDate() As Date
    '        Public Property SalesRep() As Integer
    '        Public Property SalesRepName() As String
    '        Public Property SaleStartDate() As Date
    '        Public Property ShowPrice1() As Boolean
    '        Public Property Specs() As String
    '        Public Property StatusColor() As String
    '        Public Property StatusID() As Integer
    '        Public Property StatusTitle() As String
    '        Public Property Stock() As Decimal
    '        Public Property TotalAmount() As Single
    '        Public Property TotalPayments() As Single
    '        Public Property TotalPayCond() As Single
    '        Public Property UnitAbv() As String
    '        Public Property UnitTypeID() As Integer
    '        Public Property UnitTypeTitle() As String
    '        Public Property ViewPrice() As Boolean


    '#End Region

    '    End Class

    '    Public Class Client_Info

    '#Region "Client"

    '        Public Property PortalId As Integer
    '        Public Property ClientId As Integer
    '        Public Property UserId As Integer
    '        Public Property PersonType As Boolean
    '        Public Property StatusId As String
    '        Public Property CompanyName As String
    '        Public Property DisplayName As String
    '        Public Property DateFoun As Date
    '        Public Property DateRegistered As Date
    '        Public Property FirstName As String
    '        Public Property LastName As String
    '        Public Property Telephone As String
    '        Public Property Cell As String
    '        Public Property Fax As String
    '        Public Property Zero800s As String
    '        Public Property Email As String
    '        Public Property Cnpj As String
    '        Public Property Cpf As String
    '        Public Property Ident As String
    '        Public Property InsEst As String
    '        Public Property InsMun As String
    '        Public Property Website As String
    '        Public Property RegisterTypes As String
    '        Public Property HasECF As Boolean
    '        Public Property ECFRequired As Boolean
    '        Public Property Networked As Boolean
    '        Public Property PayMethods As String
    '        Public Property PayPlans As String
    '        Public Property CreditLimit As Single
    '        Public Property PreDiscount As String
    '        Public Property Protested As Boolean
    '        Public Property Comments As String
    '        Public Property Scheduled As Boolean
    '        Public Property Sent As Boolean
    '        Public Property IsDeleted As Boolean
    '        Public Property ClientAddressId As Integer
    '        Public Property MonthlyIncome As Single
    '        Public Property SalesRep As Integer
    '        Public Property GroupIds As String
    '        Public Property CreatedByUser As Integer
    '        Public Property CreatedDate As Date
    '        Public Property ModifiedByUser As Integer
    '        Public Property ModifiedDate As Date
    '#End Region

    '    End Class

    '    Public Class Product_Info

    '        Public Property cod As String
    '        Public Property name As String
    '        Public Property unit As String
    '        Public Property price As String
    '        Public Property stock As String
    '        Public Property cst As String
    '    End Class

    '    Public Class Status_Info

    '        Public Property PortalId As Integer
    '        Public Property StatusId As Integer
    '        Public Property StatusTitle As String
    '        Public Property StatusColor As String
    '        Public Property IsReadOnly As Boolean
    '        Public Property IsDeleted As Boolean
    '        Public Property CreatedByUser As Integer
    '        Public Property CreatedDate As Date
    '        Public Property ModifiedByUser As Integer
    '        Public Property ModifiedDate As Date
    '    End Class

End Class