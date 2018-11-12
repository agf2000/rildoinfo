' Copyright (c) 2014  Christoc.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 

Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities
Imports DotNetNuke.Services.FileSystem
Imports System.Data.SqlClient
Imports RIW.Modules.Common

''' <summary>
''' The View class displays the content
''' 
''' Typically your view control would be used to display content or functionality in your module.
''' 
''' View may be the only control you have in your project depending on the complexity of your module
''' 
''' Because the control inherits from ProgrammingModuleBase you have access to any custom properties
''' defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
''' 
''' </summary>
Partial Class View
    Inherits ProgrammingModuleBase

    Public Sub ImportCategories()
        Using strCnn As New SqlClient.SqlConnection("Data Source=(local);Initial Catalog=106;User ID=sa;Password=Jabuticaba.")
            Using strCmm As New SqlClient.SqlCommand() With {.Connection = strCnn, .CommandType = CommandType.Text, .CommandText = "select * from ris_categories"}
                strCnn.Open()
                Using CategoriesReader As SqlClient.SqlDataReader = strCmm.ExecuteReader()

                    Dim category As New Models.Category
                    Dim catLang As New Models.CategoryLang
                    Dim categoryCtrl As New CategoriesRepository

                    category.Archived = False
                    category.CreatedByUser = CategoriesReader.GetValue(13)
                    category.CreatedOnDate = CategoriesReader.GetDateTime(14)
                    category.Hidden = False
                    category.ModifiedByUser = CategoriesReader.GetValue(13)
                    category.ModifiedOnDate = CategoriesReader.GetDateTime(14)
                    category.ListAltItemTemplate = ""
                    category.ListItemTemplate = ""
                    category.ListOrder = 1
                    category.ParentCategoryId = CategoriesReader.GetValue(5)
                    category.PortalId = 0
                    category.ProductTemplate = ""

                    catLang.Message = ""
                    catLang.MetaDescription = ""
                    catLang.MetaKeywords = ""
                    catLang.CategoryName = CategoriesReader.GetString(6)
                    catLang.Lang = "pt-BR"
                    catLang.CategoryDesc = ""
                    catLang.SEOName = ""
                    catLang.SEOPageTitle = ""

                    categoryCtrl.AddCategory(category)
                    catLang.CategoryId = category.CategoryId
                    categoryCtrl.AddCategoryLang(catLang)

                    Dim catPermission As New Models.CategoryPermission() With {.PermissionId = 1, .CategoryId = category.CategoryId}

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

                End Using
            End Using
        End Using
    End Sub

    Public Shared Sub ImportExcelProducts()

        Using strCnn As New SqlConnection("Data Source=(local);Initial Catalog=112;User ID=sa;Password=Jabuticaba.")
            Using strCmm As New SqlCommand() With {.Connection = strCnn, .CommandType = CommandType.Text, .CommandText = "select * from tabela"}
                'strCmm.Parameters.AddWithValue("@IsDeleted", "False")
                strCnn.Open()
                Using ProductsReader As SqlDataReader = strCmm.ExecuteReader()
                    Do While ProductsReader.Read()

                        Dim product As New Models.Product
                        Dim productLang As New Models.ProductLang
                        Dim productCtrl As New ProductsRepository

                        If Not ProductsReader.IsDBNull(0) Then

                            product.ItemType = 1
                            product.Archived = False
                            product.DealerOnly = False
                            product.CreatedByUser = 3
                            product.CreatedOnDate = Now()
                            product.Featured = False
                            product.IsHidden = False
                            product.ModifiedByUser = 3
                            product.ModifiedOnDate = Now()
                            product.PortalId = 0
                            product.ProductRef = ""
                            product.ReorderPoint = 0
                            product.ShowPrice = False
                            product.ProductUnit = ProductsReader.GetString(1)
                            product.SaleStartDate = New Date(1900, 1, 1)
                            product.SaleEndDate = New Date(1900, 1, 1)
                            product.Brand = 0
                            product.BrandModel = 0
                            product.Barcode = ""
                            product.Vendors = ""
                            product.QtyStockSet = ProductsReader.GetValue(2)

                            productLang.Lang = "pt-BR"
                            productLang.ProductName = ProductsReader.GetString(0)
                            'productLang.Manufacturer = 0
                            productLang.Summary = ""

                            Utilities.CreateDir(Portals.PortalController.GetCurrentPortalSettings(), Constants.PRODUCTIMAGESFOLDER)
                            Utilities.CreateDir(Portals.PortalController.GetCurrentPortalSettings(), Constants.PRODUCTDOCSFOLDER)

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
                            productFinance.Finan_Sale_Price = ProductsReader.GetValue(3)
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

                            'copy to languages not created yet
                            productLang.ProductId = product.ProductId
                            productCtrl.CopyProductToLanguages(productLang, "pt-BR", False)

                        End If

                    Loop

                End Using
            End Using
        End Using
    End Sub

    'Public Sub ImportProducts()
    '    Using strCnn As New SqlClient.SqlConnection("Data Source=(local);Initial Catalog=106;User ID=sa;Password=Jabuticaba.")
    '        Using strCmm As New SqlClient.SqlCommand() With {.Connection = strCnn, .CommandType = CommandType.StoredProcedure, .CommandText = "RIS_Products_Get"}
    '            Dim param1 As New SqlParameter With {.ParameterName = "@PortalId", .Value = 0}
    '            Dim param2 As New SqlParameter With {.ParameterName = "@ItemType", .Value = ""}
    '            Dim param3 As New SqlParameter With {.ParameterName = "@SearchString", .Value = ""}
    '            Dim param4 As New SqlParameter With {.ParameterName = "@IsDeleted", .Value = "False"}
    '            Dim param5 As New SqlParameter With {.ParameterName = "@StartDate", .Value = DBNull.Value}
    '            Dim param6 As New SqlParameter With {.ParameterName = "@EndDate", .Value = DBNull.Value}
    '            Dim param7 As New SqlParameter With {.ParameterName = "@PageNumber", .Value = 1}
    '            Dim param8 As New SqlParameter With {.ParameterName = "@PageSize", .Value = 1000}
    '            Dim param9 As New SqlParameter With {.ParameterName = "@SortCol", .Value = ""}
    '            strCmm.Parameters.Add(param1)
    '            strCmm.Parameters.Add(param2)
    '            strCmm.Parameters.Add(param3)
    '            strCmm.Parameters.Add(param4)
    '            strCmm.Parameters.Add(param5)
    '            strCmm.Parameters.Add(param6)
    '            strCmm.Parameters.Add(param7)
    '            strCmm.Parameters.Add(param8)
    '            strCmm.Parameters.Add(param9)
    '            strCnn.Open()
    '            Using ProductsReader As SqlClient.SqlDataReader = strCmm.ExecuteReader()

    '                Dim productsCtrl As New ProductsRepository
    '                Dim productLang As New Models.ProductLang
    '                Dim product As New Models.Product

    '                'If ProductsReader.GetValue(3) Then

    '                'End If

    '                'product.ItemType = ProductsReader.GetValue(3)
    '                'product.Archived = False
    '                'product.DealerOnly = False
    '                'product.CreatedByUser = 2
    '                'product.CreatedOnDate = ProductsReader.GetDateTime(14)
    '                'product.Featured = False
    '                'product.IsHidden = False
    '                'product.ModifiedByUser = 2
    '                'product.ModifiedOnDate = ProductsReader.GetDateTime(15)
    '                'product.PortalId = 0
    '                'product.ProductRef = ProductsReader.GetString(7)
    '                'product.ReorderPoint = 0
    '                'product.ShowPrice = False
    '                'product.ProductUnit = ProductsReader.GetValue(21)
    '                'product.SaleStartDate = ProductsReader.GetDateTime(9)
    '                'product.SaleEndDate = ProductsReader.GetDateTime(10)
    '                'product.Brand = ProductsReader.GetValue(17)
    '                'product.BrandModel = ProductsReader.GetValue(18)
    '                'product.Barcode = ProductsReader.GetValue(8)
    '                'product.Vendors = ""
    '                'product.QtyStockSet = 0
    '                'product.Width = 0
    '                'product.Height = 0
    '                'product.Weight = 0
    '                'product.Length = 0
    '                'product.Diameter = 0

    '                'productLang.Lang = "pt-BR"
    '                'productLang.ProductName = ProductsReader.GetString(4)
    '                'productLang.Manufacturer = ""
    '                'productLang.Summary = ProductsReader.GetString(5)
    '                'productLang.Description = ProductsReader.GetString(6)

    '                'CreateDir(Portals.PortalController.GetCurrentPortalSettings(), Constants.PRODUCTIMAGESFOLDER)
    '                'CreateDir(Portals.PortalController.GetCurrentPortalSettings(), Constants.PRODUCTDOCSFOLDER)

    '                'productsCtrl.addProduct(product)

    '                'Dim productFinance As New Models.ProductFinance

    '                'productFinance.Finan_COFINS = 0
    '                'productFinance.Finan_COFINSBase = 0
    '                'productFinance.Finan_COFINSTributeSub = 0
    '                'productFinance.Finan_COFINSTributeSubBase = 0
    '                'productFinance.Finan_Cost = 0
    '                'productFinance.Finan_DiffICMS = 0
    '                'productFinance.Finan_Freight = 0
    '                'productFinance.Finan_ICMS = 0
    '                'productFinance.Finan_ICMSFreight = 0
    '                'productFinance.Finan_IPI = 0
    '                'productFinance.Finan_ISS = 0
    '                'productFinance.Finan_Manager = 0
    '                'productFinance.Finan_MarkUp = 0
    '                'productFinance.Finan_OtherExpenses = 0
    '                'productFinance.Finan_OtherTaxes = 0
    '                'productFinance.Finan_Paid = 0
    '                'productFinance.Finan_Paid_Discount = 0
    '                'productFinance.Finan_PIS = 0
    '                'productFinance.Finan_PISBase = 0
    '                'productFinance.Finan_PISTributeSub = 0
    '                'productFinance.Finan_PISTributeSubBase = 0
    '                'productFinance.Finan_Rep = 0
    '                'productFinance.Finan_Sale_Price = 0
    '                'productFinance.Finan_SalesPerson = 0
    '                'productFinance.Finan_Select = "1"
    '                'productFinance.Finan_Special_Price = 0
    '                'productFinance.Finan_Dealer_Price = 0
    '                'productFinance.Finan_Tech = 0
    '                'productFinance.Finan_Telemarketing = 0
    '                'productFinance.Finan_TributeSubICMS = 0
    '                'productFinance.Finan_TributeSubICMS = 0
    '                'productFinance.ProductId = product.ProductId

    '                'productsCtrl.addProductFinance(productFinance)

    '                'productLang.ProductId = product.ProductId
    '                'productsCtrl.CopyProductToLanguages(productLang, "pt-BR", False)

    '                'Dim categoryCtrl As New CategoriesRepository

    '                'For Each catName In ProductsReader.GetString(30).Split(","c)
    '                '    Dim productCategory As New Models.ProductCategory
    '                '    productsCtrl.removeProductCategory(product.ProductId)
    '                '    Dim category = categoryCtrl.getCategoryByName(0, catName.Trim())
    '                '    productsCtrl.addProductCategory(product.ProductId, category.CategoryId)
    '                'Next

    '            End Using
    '        End Using
    '    End Using
    'End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Try

            ImportExcelProducts()

        Catch exc As Exception
            Exceptions.ProcessModuleLoadException(Me, exc)
        End Try
    End Sub

    Public Shared Sub CreateDir(ByVal PortalSettings As DotNetNuke.Entities.Portals.PortalSettings, ByVal FolderName As String)
        Dim folderInfo As DotNetNuke.Services.FileSystem.FolderInfo
        Dim blnCreated As Boolean = False

        'try normal test (doesn;t work on medium trust, but avoids waiting for GetFolder.)
        Try
            blnCreated = System.IO.Directory.Exists(PortalSettings.HomeDirectoryMapPath & FolderName)
        Catch ex As Exception
            blnCreated = False
        End Try

        If Not blnCreated Then
            FolderManager.Instance().Synchronize(PortalSettings.PortalId, PortalSettings.HomeDirectory, True, True)
            folderInfo = CType(FolderManager.Instance().GetFolder(PortalSettings.PortalId, FolderName), DotNetNuke.Services.FileSystem.FolderInfo)
            If folderInfo Is Nothing And FolderName <> "" Then
                'add folder and permissions
                Try
                    FolderManager.Instance().AddFolder(PortalSettings.PortalId, FolderName)
                Catch ex As Exception
                End Try
                folderInfo = CType(FolderManager.Instance().GetFolder(PortalSettings.PortalId, FolderName), DotNetNuke.Services.FileSystem.FolderInfo)
                If Not folderInfo Is Nothing Then
                    Dim folderid As Integer = folderInfo.FolderID
                    Dim objPermissionController As New DotNetNuke.Security.Permissions.PermissionController
                    Dim arr As ArrayList = objPermissionController.GetPermissionByCodeAndKey("SYSTEM_FOLDER", "")
                    For Each objpermission As DotNetNuke.Security.Permissions.PermissionInfo In arr
                        If objpermission.PermissionKey = "WRITE" Then
                            ' add READ permissions to the All Users Role
                            FolderManager.Instance().SetFolderPermission(folderInfo, objpermission.PermissionID, Integer.Parse(DotNetNuke.Common.Globals.glbRoleAllUsers))
                        End If
                    Next
                End If
            End If
        End If
    End Sub

End Class