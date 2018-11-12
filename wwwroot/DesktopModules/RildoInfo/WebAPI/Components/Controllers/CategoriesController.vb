
Imports System.Net
Imports System.Net.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation
Imports RIW.Modules.WebAPI.Components.Models
Imports RIW.Modules.WebAPI.Components.Repositories

Public Class CategoriesController
    Inherits DnnApiController

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(CategoriesController))

    ''' <summary>
    ''' Gets a list of brands by portal id
    ''' </summary>
    ''' <param name="portalId"></param>
    ''' <param name="lang"></param>
    ''' <param name="parentId"></param>
    ''' <param name="filter"></param>
    ''' <param name="archived"></param>
    ''' <param name="includeArchived"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <ActionName("Categories")> _
    <AllowAnonymous> _
    <HttpGet> _
    Function GetCategories(portalId As Integer, lang As String, Optional parentId As Integer = -1, Optional filter As String = "", Optional archived As Boolean = False, Optional includeArchived As Boolean = False) As HttpResponseMessage
        Try
            Dim catsData As IEnumerable(Of Category)
            Dim catsDataCtrl As New CategoriesRepository

            catsData = catsDataCtrl.GetCategoriesList(portalId, lang, parentId, filter, archived, includeArchived)

            Return Request.CreateResponse(HttpStatusCode.OK, catsData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets category
    ''' </summary>
    ''' <param name="catId">Category ID</param>
    ''' <param name="lang">Category Language</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <ActionName("Category")> _
    <AllowAnonymous> _
    <HttpGet> _
    Function GetCategory(catId As Integer, lang As String) As HttpResponseMessage
        Try
            Dim catData As Category
            Dim catDataCtrl As New CategoriesRepository

            catData = catDataCtrl.GetCategory(catId, lang)

            Return Request.CreateResponse(HttpStatusCode.OK, catData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets category
    ''' </summary>
    ''' <param name="catId">Category ID</param>
    ''' <param name="lang">Category Language</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <ActionName("CategoryLang")> _
    <AllowAnonymous> _
    <HttpGet> _
    Function GetCategoryLang(catId As Integer, lang As String) As HttpResponseMessage
        Try
            Dim catData As CategoryLang
            Dim catDataCtrl As New CategoriesRepository

            catData = catDataCtrl.GetCategoryLang(catId, lang)

            Return Request.CreateResponse(HttpStatusCode.OK, catData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets a list of permissions by category id
    ''' </summary>
    ''' <param name="catId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpGet> _
    Function GetCategoryPermissions(catId As Integer) As HttpResponseMessage
        Try
            Dim catPermissionsData As IEnumerable(Of Components.Models.CategoryPermission)
            Dim catPermissionsDataCtrl As New CategoriesRepository

            catPermissionsData = catPermissionsDataCtrl.GetCategoryPermissions(catId)

            Return Request.CreateResponse(HttpStatusCode.OK, catPermissionsData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets a list of permissions by category id
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpGet> _
    Function GetCategoriesRolePermissions(roleId As Integer) As HttpResponseMessage
        Try
            Dim catPermissionsData As IEnumerable(Of Components.Models.CategoryPermission)
            Dim catPermissionsCtrl As New CategoriesRepository

            catPermissionsData = catPermissionsCtrl.GetCategoriesRolePermissions(roleId)

            Return Request.CreateResponse(HttpStatusCode.OK, catPermissionsData)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Resets category security
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPost> _
    Function UpdateCategorySecurity(dto As Components.Models.CategoryPermission) As HttpResponseMessage
        Try
            Dim catPermission As New Components.Models.CategoryPermission
            Dim catPermissionCtrl As New CategoriesRepository

            If dto.CategoryPermissionId > 0 Then
                catPermission = catPermissionCtrl.GetCategoryPermission(dto.CategoryPermissionId, dto.CategoryId)
            End If

            catPermission.CategoryId = dto.CategoryId
            catPermission.PermissionId = dto.PermissionId
            catPermission.RoleId = dto.RoleId
            catPermission.AllowAccess = dto.AllowAccess

            If dto.CategoryPermissionId > 0 Then
                catPermissionCtrl.UpdateCategoryPermission(catPermission)
            Else
                catPermissionCtrl.AddCategorySecurity(catPermission)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .CatetoryPermissionId = catPermission.CategoryPermissionId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets or updates category model
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpPost> _
    Function UpdateCategory(dto As Category) As HttpResponseMessage
        Try
            Dim category As New Category
            Dim catLang As New CategoryLang
            Dim categoryCtrl As New CategoriesRepository

            If dto.CategoryId > 0 Then
                category = categoryCtrl.GetCategory(dto.CategoryId, dto.Lang)
                catLang = categoryCtrl.GetCategoryLang(dto.CategoryId, dto.Lang)
            End If

            category.Archived = dto.Archived
            category.CreatedByUser = dto.CreatedByUser
            category.CreatedOnDate = dto.CreatedOnDate
            'category.ImageBinary = CType(DBNull.Value, System.Data.SqlTypes.SqlBinary)
            'category.ImageUrl = Nothing
            category.Hidden = dto.Hidden
            category.ModifiedByUser = dto.CreatedByUser
            category.ModifiedOnDate = dto.CreatedOnDate
            category.ListAltItemTemplate = dto.ListAltItemTemplate
            category.ListItemTemplate = dto.ListItemTemplate
            category.ListOrder = dto.ListOrder
            category.ParentCategoryId = dto.ParentCategoryId
            category.PortalId = dto.PortalId
            category.ProductTemplate = dto.ProductTemplate

            catLang.Message = dto.Message
            catLang.MetaDescription = dto.MetaDescription
            catLang.MetaKeywords = dto.MetaKeywords
            catLang.CategoryName = dto.CategoryName
            catLang.Lang = dto.Lang
            catLang.CategoryDesc = dto.CategoryDesc
            catLang.SEOName = dto.SEOName
            catLang.SEOPageTitle = dto.SEOPageTitle

            If dto.CategoryId > 0 Then
                categoryCtrl.UpdateCategory(category)
                categoryCtrl.UpdateCategoryLang(catLang)

                categoryCtrl.ResetCategorySecurities(category.CategoryId)
            Else
                categoryCtrl.AddCategory(category)
                catLang.CategoryId = category.CategoryId
                categoryCtrl.AddCategoryLang(catLang)

                Dim catPermission As New Components.Models.CategoryPermission() With {.PermissionId = 1, .CategoryId = category.CategoryId}

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

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .CategoryId = category.CategoryId})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes category
    ''' </summary>
    ''' <param name="catId">Kit ID</param>
    ''' <param name="lang">Language</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize(StaticRoles:="Gerentes,Editores")> _
    <HttpDelete> _
    Function RemoveCategory(catId As Integer, Optional lang As String = "pt-BR") As HttpResponseMessage
        Try
            Dim categoryCtrl As New CategoriesRepository

            categoryCtrl.RemoveCategory(catId, lang)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets list of products
    ''' </summary>
    ''' <param name="categoryId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpGet> _
    Function GetProductsCategory(Optional categoryId As Integer = 0) As HttpResponseMessage
        Try
            Dim products As IEnumerable(Of Product)
            Dim productsCtrl As New ProductsRepository

            products = productsCtrl.GetProducts(PortalController.Instance.GetCurrentPortalSettings().PortalId, categoryId, "pt-BR", "ProductName", "", True, False, "", False, "", 1, 10, "", False, False, "", "", "", "", "ALL", "", False)

            Dim total = Nothing
            For Each item In products
                total = item.TotalRows
                Exit For
            Next

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.data = products, .total = total})
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets list of products
    ''' </summary>
    ''' <param name="productId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AllowAnonymous> _
    <HttpGet> _
    Function GetCategoriesProduct(productId As Integer) As HttpResponseMessage
        Try
            Dim categoriesCtrl As New CategoriesRepository

            Dim categories = categoriesCtrl.GetProductCategories(productId)

            Return Request.CreateResponse(HttpStatusCode.OK, categories)
        Catch ex As Exception
            'DnnLog.Error(ex)
            logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

End Class
