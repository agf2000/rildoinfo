Imports DotNetNuke.Data
Imports DotNetNuke.Common.Utilities

Public Class CategoriesRepository
    Implements ICategoriesRepository

#Region "Private Methods"

    Private Shared Function GetNull(field As Object) As Object
        Return Null.GetNull(field, DBNull.Value)
    End Function

#End Region

    Public Function AddCategory(category As Models.Category) As Models.Category Implements ICategoriesRepository.addCategory
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Category) = ctx.GetRepository(Of Models.Category)()
            rep.Insert(category)
        End Using
        Return category
    End Function

    Public Sub DeleteCategory(category As Models.Category) Implements ICategoriesRepository.deleteCategory
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Category) = ctx.GetRepository(Of Models.Category)()
            rep.Update(category)
        End Using
    End Sub

    Public Function GetCategories_List(portalId As Integer, lang As String, parentId As Integer, filter As String, archived As Boolean, includeArchived As Boolean) As IEnumerable(Of Models.Category) Implements ICategoriesRepository.getCategories_List
        Return CBO.FillCollection(Of Models.Category)(DataProvider.Instance().GetCategories_List(portalId, lang, parentId, filter, archived, includeArchived))
    End Function

    Public Function GetCategoriesRolePermissions(roleId As Integer) As IEnumerable(Of Models.CategoryPermission) Implements ICategoriesRepository.getCategoriesRolePermissions
        Dim catPermissions As IEnumerable(Of Models.CategoryPermission)
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep = ctx.GetRepository(Of Models.CategoryPermission)()
            catPermissions = rep.Find("Where RoleId = @0 And AllowAccess = @1", roleId, True)
        End Using
        Return catPermissions
    End Function

    Public Function GetCategory(catId As Integer, lang As String) As Models.Category Implements ICategoriesRepository.getCategory
        Return CBO.FillObject(Of Models.Category)(DataProvider.Instance().GetCategory(catId, lang))
        'Dim category As Models.Category

        'Using ctx As IDataContext = DataContext.Instance()
        '    category = ctx.ExecuteSingleOrDefault(Of Models.Category)(CommandType.StoredProcedure, "RIW_Categories_Get", catId, lang)
        'End Using
        'Return category
    End Function

    Public Function GetCategoryLang(catId As Integer, lang As String) As Models.CategoryLang Implements ICategoriesRepository.getCategoryLang
        Dim catLang As Models.CategoryLang

        Using ctx As IDataContext = DataContext.Instance()
            catLang = ctx.ExecuteSingleOrDefault(Of Models.CategoryLang)(CommandType.Text, "SELECT * FROM RIW_CategoryLang WHERE CategoryId = @0 AND Lang = @1", catId, lang)
        End Using
        Return catLang
    End Function

    Public Sub RemoveCategory(category As Models.Category) Implements ICategoriesRepository.removeCategory
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Category) = ctx.GetRepository(Of Models.Category)()
            rep.Delete(category)
        End Using
    End Sub

    Public Sub RemoveCategory(catId As Integer, lang As String) Implements ICategoriesRepository.removeCategory
        Dim _item As Models.Category = GetCategory(catId, lang)
        If _item.ProductCount > 0 Then
            Throw New ArgumentException("Exception Occured")
        Else
            RemoveCategory(_item)
        End If
    End Sub

    Public Sub RemoveCategorySecurities(catPermission As Models.CategoryPermission) Implements ICategoriesRepository.removeCategorySecurities
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.CategoryPermission) = ctx.GetRepository(Of Models.CategoryPermission)()
            rep.Delete(catPermission)
        End Using
    End Sub

    Public Sub ResetCategorySecurities(catId As Integer) Implements ICategoriesRepository.resetCategorySecurities
        Dim catPermissions As IEnumerable(Of Models.CategoryPermission) = GetCategoryPermissions(catId)
        If catPermissions.Count > 0 Then
            For Each catPermission In catPermissions
                RemoveCategorySecurities(catPermission)
            Next
        End If
    End Sub

    Public Sub UpdateCategory(category As Models.Category) Implements ICategoriesRepository.updateCategory
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Category) = ctx.GetRepository(Of Models.Category)()
            rep.Update(category)
        End Using
    End Sub

    Public Sub AddCategorySecurity(catPermission As Models.CategoryPermission) Implements ICategoriesRepository.addCategorySecurity
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.CategoryPermission) = ctx.GetRepository(Of Models.CategoryPermission)()
            rep.Insert(catPermission)
        End Using
    End Sub

    Public Function AddCategoryLang(categoryLang As Models.CategoryLang) As Models.CategoryLang Implements ICategoriesRepository.addCategoryLang
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.CategoryLang) = ctx.GetRepository(Of Models.CategoryLang)()
            rep.Insert(categoryLang)
        End Using
        Return categoryLang
    End Function

    Public Sub UpdateCategoryLang(categoryLang As Models.CategoryLang) Implements ICategoriesRepository.updateCategoryLang
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.CategoryLang) = ctx.GetRepository(Of Models.CategoryLang)()
            rep.Update(categoryLang)
        End Using
    End Sub

    Public Function GetCategoryPermissions(categoryId As Integer) As IEnumerable(Of Models.CategoryPermission) Implements ICategoriesRepository.getCategoryPermissions
        Dim catPermissions As IEnumerable(Of Models.CategoryPermission)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep = ctx.GetRepository(Of Models.CategoryPermission)()
            catPermissions = rep.Get(categoryId)
        End Using
        Return catPermissions
    End Function

    Public Function GetCategoryProducts(categoryId As Integer) As IEnumerable(Of Models.ProductCategory) Implements ICategoriesRepository.getCategoryProducts
        Dim categoryProducts As IEnumerable(Of Models.ProductCategory)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep = ctx.GetRepository(Of Models.ProductCategory)()
            categoryProducts = rep.Get(categoryId)
        End Using
        Return categoryProducts
    End Function

    Public Function GetProductCategories(productId As Integer) As IEnumerable(Of Models.Category) Implements ICategoriesRepository.getProductCategories
        Return CBO.FillCollection(Of Models.Category)(DataProvider.Instance().GetProductCategories(productId))
    End Function

    Function GetCategoryPermission(categoryPermissionId As Integer, categoryId As Integer) As Models.CategoryPermission Implements ICategoriesRepository.getCategoryPermission
        Dim categoryPermission As Models.CategoryPermission
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.CategoryPermission) = ctx.GetRepository(Of Models.CategoryPermission)()
            categoryPermission = rep.GetById(Of Int32, Int32)(categoryPermissionId, categoryId)
        End Using
        Return categoryPermission
    End Function

    Sub UpdateCategoryPermission(catPermission As Models.CategoryPermission) Implements ICategoriesRepository.updateCategoryPermission
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.CategoryPermission) = ctx.GetRepository(Of Models.CategoryPermission)()
            rep.Update(catPermission)
        End Using
    End Sub

    Public Function getCategoryByName(portalId As Integer, categoryName As String) As Models.Category Implements ICategoriesRepository.getCategoryByName
        Dim category As Models.Category

        Using ctx As IDataContext = DataContext.Instance()
            category = ctx.ExecuteSingleOrDefault(Of Models.Category)(CommandType.Text, "SELECT * FROM RIW_CategoryLang WHERE CategoryName = @0", categoryName)
        End Using
        Return category
    End Function
End Class
