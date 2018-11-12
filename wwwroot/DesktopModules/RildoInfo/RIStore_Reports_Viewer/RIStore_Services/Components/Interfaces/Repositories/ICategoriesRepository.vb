
Namespace RI.Modules.RIStore_Services
    Public Interface ICategoriesRepository

        ''' <summary>
        ''' Gets a list of categories by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="lang">Language</param>
        ''' <param name="parentId">Parent ID</param>
        ''' <param name="filter">Filter</param>
        ''' <param name="archived">Maked As Deleted</param>
        ''' <param name="includeArchived">Include Deleted</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetCategories_List(portalId As Integer, lang As String, parentId As Integer, filter As String, archived As Boolean, includeArchived As Boolean) As IEnumerable(Of Models.Category)

        ''' <summary>
        ''' Gets category
        ''' </summary>
        ''' <param name="catId">Category ID</param>
        ''' <param name="lang">Category Language</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetCategory(catId As Integer, lang As String) As Models.Category

        ''' <summary>
        ''' Gets category Language
        ''' </summary>
        ''' <param name="catId">Category ID</param>
        ''' <param name="lang">Category Language</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetCategoryLang(catId As Integer, lang As String) As Models.CategoryLang

        ''' <summary>
        ''' Gets a list of category permissions by category id
        ''' </summary>
        ''' <param name="catId">Category ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetCategoryPermissions(catId As Integer) As IEnumerable(Of Models.CategoryPermission)

        ''' <summary>
        ''' Gets category permission
        ''' </summary>
        ''' <param name="categoryPermissionId"></param>
        ''' <param name="categoryId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetCategoryPermission(categoryPermissionId As Integer, categoryId As Integer) As Models.CategoryPermission

        ''' <summary>
        ''' Gets a list of permissions by category id
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetCategoriesRolePermissions(roleId As Integer) As IEnumerable(Of Models.CategoryPermission)

        ''' <summary>
        ''' Adds or updates a category
        ''' </summary>
        ''' <param name="category">Category Model</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddCategory(category As Models.Category) As Models.Category

        ''' <summary>
        ''' Adds or updates a category
        ''' </summary>
        ''' <param name="categoryLang">Category Language Model</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddCategoryLang(categoryLang As Models.CategoryLang) As Models.CategoryLang

        ''' <summary>
        ''' Adds or updates a category
        ''' </summary>
        ''' <param name="category">Category Model</param>
        ''' <remarks></remarks>
        Sub UpdateCategory(category As Models.Category)

        ''' <summary>
        ''' Adds or updates a category
        ''' </summary>
        ''' <param name="categoryLang">Category Language Model</param>
        ''' <remarks></remarks>
        Sub UpdateCategoryLang(categoryLang As Models.CategoryLang)

        ''' <summary>
        ''' Updates a category security
        ''' </summary>
        ''' <param name="catPermission">Category Permission Model</param>
        ''' <remarks></remarks>
        Sub AddCategorySecurity(catPermission As Models.CategoryPermission)

        ''' <summary>
        ''' Updates category permission
        ''' </summary>
        ''' <param name="catPermission"></param>
        ''' <remarks></remarks>
        Sub UpdateCategoryPermission(catPermission As Models.CategoryPermission)

        ''' <summary>
        ''' Resets category securities
        ''' </summary>
        ''' <param name="catPermission">Category Permission Model</param>
        ''' <remarks></remarks>
        Sub RemoveCategorySecurities(catPermission As Models.CategoryPermission)

        ''' <summary>
        ''' Resets category securities
        ''' </summary>
        ''' <param name="catId">Category ID</param>
        ''' <remarks></remarks>
        Sub ResetCategorySecurities(catId As Integer)

        ''' <summary>
        ''' Removes category
        ''' </summary>
        ''' <param name="category">Category Model</param>
        ''' <remarks></remarks>
        Sub RemoveCategory(category As Models.Category)

        ''' <summary>
        ''' Removes category
        ''' </summary>
        ''' <param name="catId">Category ID</param>
        ''' <param name="lang">Language</param>
        ''' <remarks></remarks>
        Sub RemoveCategory(catId As Integer, lang As String)

        ''' <summary>
        ''' Deletes category
        ''' </summary>
        ''' <param name="category">Category Model</param>
        ''' <remarks></remarks>
        Sub DeleteCategory(category As Models.Category)

        ''' <summary>
        ''' Gets list of products
        ''' </summary>
        ''' <param name="categoryId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetCategoryProducts(categoryId As Integer) As IEnumerator(Of Models.ProductCategory)

    End Interface
End Namespace