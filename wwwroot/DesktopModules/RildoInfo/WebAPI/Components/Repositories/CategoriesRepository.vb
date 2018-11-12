Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Repositories

    Public Class CategoriesRepository
    Implements ICategoriesRepository

        '#Region "Private Methods"

        'Private Shared Function GetNull(field As Object) As Object
        '    Return Null.GetNull(field, DBNull.Value)
        'End Function

        '#End Region

        Public Function AddCategory(category As Category) As Category Implements ICategoriesRepository.AddCategory
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Category) = ctx.GetRepository(Of Category)()
                rep.Insert(category)
            End Using
            Return category
        End Function

        Public Sub DeleteCategory(category As Category) Implements ICategoriesRepository.DeleteCategory
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Category) = ctx.GetRepository(Of Category)()
                rep.Update(category)
            End Using
        End Sub

        Public Function GetCategoriesList(portalId As Integer, lang As String, parentId As Integer, filter As String, archived As Boolean, includeArchived As Boolean) As IEnumerable(Of Category) Implements ICategoriesRepository.GetCategoriesList
            Return CBO.FillCollection(Of Category)(DataProvider.Instance().GetCategories_List(portalId, lang, parentId, filter, archived, includeArchived))
        End Function

        Public Function GetCategoriesRolePermissions(roleId As Integer) As IEnumerable(Of CategoryPermission) Implements ICategoriesRepository.GetCategoriesRolePermissions
            Dim catPermissions As IEnumerable(Of CategoryPermission)
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep = ctx.GetRepository(Of CategoryPermission)()
                catPermissions = rep.Find("Where RoleId = @0 And AllowAccess = @1", roleId, True)
            End Using
            Return catPermissions
        End Function

        Public Function GetCategory(catId As Integer, lang As String) As Category Implements ICategoriesRepository.GetCategory
            Return CBO.FillObject(Of Category)(DataProvider.Instance().GetCategory(catId, lang))
            'Dim category As Models.Category

            'Using ctx As IDataContext = DataContext.Instance()
            '    category = ctx.ExecuteSingleOrDefault(Of Models.Category)(CommandType.StoredProcedure, "RIW_Categories_Get", catId, lang)
            'End Using
            'Return category
        End Function

        Public Function GetCategoryLang(catId As Integer, lang As String) As CategoryLang Implements ICategoriesRepository.GetCategoryLang
            Dim catLang As CategoryLang

            Using ctx As IDataContext = DataContext.Instance()
                catLang = ctx.ExecuteSingleOrDefault(Of CategoryLang)(CommandType.Text, "SELECT * FROM RIW_CategoryLang WHERE CategoryId = @0 AND Lang = @1", catId, lang)
            End Using
            Return catLang
        End Function

        Public Sub RemoveCategory(category As Category) Implements ICategoriesRepository.RemoveCategory
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Category) = ctx.GetRepository(Of Category)()
                rep.Delete(category)
            End Using
        End Sub

        Public Sub RemoveCategory(catId As Integer, lang As String) Implements ICategoriesRepository.RemoveCategory
            Dim item As Category = GetCategory(catId, lang)
            If item.ProductCount > 0 Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveCategory(item)
            End If
        End Sub

        Public Sub RemoveCategorySecurities(catPermission As CategoryPermission) Implements ICategoriesRepository.RemoveCategorySecurities
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of CategoryPermission) = ctx.GetRepository(Of CategoryPermission)()
                rep.Delete(catPermission)
            End Using
        End Sub

        Public Sub ResetCategorySecurities(catId As Integer) Implements ICategoriesRepository.ResetCategorySecurities
            Dim catPermissions As IEnumerable(Of CategoryPermission) = GetCategoryPermissions(catId)
            If catPermissions.Count > 0 Then
                For Each catPermission In catPermissions
                    RemoveCategorySecurities(catPermission)
                Next
            End If
        End Sub

        Public Sub UpdateCategory(category As Category) Implements ICategoriesRepository.UpdateCategory
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Category) = ctx.GetRepository(Of Category)()
                rep.Update(category)
            End Using
        End Sub

        Public Sub AddCategorySecurity(catPermission As CategoryPermission) Implements ICategoriesRepository.AddCategorySecurity
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of CategoryPermission) = ctx.GetRepository(Of CategoryPermission)()
                rep.Insert(catPermission)
            End Using
        End Sub

        Public Function AddCategoryLang(categoryLang As CategoryLang) As CategoryLang Implements ICategoriesRepository.AddCategoryLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of CategoryLang) = ctx.GetRepository(Of CategoryLang)()
                rep.Insert(categoryLang)
            End Using
            Return categoryLang
        End Function

        Public Sub UpdateCategoryLang(categoryLang As CategoryLang) Implements ICategoriesRepository.UpdateCategoryLang
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of CategoryLang) = ctx.GetRepository(Of CategoryLang)()
                rep.Update(categoryLang)
            End Using
        End Sub

        Public Function GetCategoryPermissions(categoryId As Integer) As IEnumerable(Of CategoryPermission) Implements ICategoriesRepository.GetCategoryPermissions
            Dim catPermissions As IEnumerable(Of CategoryPermission)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep = ctx.GetRepository(Of CategoryPermission)()
                catPermissions = rep.Get(categoryId)
            End Using
            Return catPermissions
        End Function

        Public Function GetCategoryProducts(categoryId As Integer) As IEnumerable(Of ProductCategory) Implements ICategoriesRepository.GetCategoryProducts
            Dim categoryProducts As IEnumerable(Of ProductCategory)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep = ctx.GetRepository(Of ProductCategory)()
                categoryProducts = rep.Get(categoryId)
            End Using
            Return categoryProducts
        End Function

        Public Function GetProductCategories(productId As Integer) As IEnumerable(Of Category) Implements ICategoriesRepository.GetProductCategories
            Return CBO.FillCollection(Of Category)(DataProvider.Instance().GetProductCategories(productId))
        End Function

        Function GetCategoryPermission(categoryPermissionId As Integer, categoryId As Integer) As CategoryPermission Implements ICategoriesRepository.GetCategoryPermission
            Dim categoryPermission As CategoryPermission
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of CategoryPermission) = ctx.GetRepository(Of CategoryPermission)()
                categoryPermission = rep.GetById(Of Int32, Int32)(categoryPermissionId, categoryId)
            End Using
            Return categoryPermission
        End Function

        Sub UpdateCategoryPermission(catPermission As CategoryPermission) Implements ICategoriesRepository.UpdateCategoryPermission
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of CategoryPermission) = ctx.GetRepository(Of CategoryPermission)()
                rep.Update(catPermission)
            End Using
        End Sub

        Public Function GetCategoryByName(portalId As Integer, categoryName As String) As Category Implements ICategoriesRepository.GetCategoryByName
            Dim category As Category

            Using ctx As IDataContext = DataContext.Instance()
                category = ctx.ExecuteSingleOrDefault(Of Category)(CommandType.Text, "SELECT * FROM RIW_CategoryLang WHERE CategoryName = @0", categoryName)
            End Using
            Return category
        End Function
    End Class

End Namespace
