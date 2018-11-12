
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
    Function getCategories_List(portalId As Integer, lang As String, parentId As Integer, filter As String, archived As Boolean, includeArchived As Boolean) As IEnumerable(Of Models.Category)

    ''' <summary>
    ''' Gets category
    ''' </summary>
    ''' <param name="catId">Category ID</param>
    ''' <param name="lang">Category Language</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getCategory(catId As Integer, lang As String) As Models.Category

    ''' <summary>
    ''' Gets category by name
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="categoryName">Category Name</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getCategoryByName(portalId As Integer, categoryName As String) As Models.Category

    ''' <summary>
    ''' Gets category Language
    ''' </summary>
    ''' <param name="catId">Category ID</param>
    ''' <param name="lang">Category Language</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getCategoryLang(catId As Integer, lang As String) As Models.CategoryLang

    ''' <summary>
    ''' Gets a list of category permissions by category id
    ''' </summary>
    ''' <param name="categoryId">Category ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getCategoryPermissions(categoryId As Integer) As IEnumerable(Of Models.CategoryPermission)

    ''' <summary>
    ''' Gets list of products
    ''' </summary>
    ''' <param name="categoryId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getCategoryProducts(categoryId As Integer) As IEnumerable(Of Models.ProductCategory)

    ''' <summary>
    ''' Gets list of categories
    ''' </summary>
    ''' <param name="productId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getProductCategories(productId As Integer) As IEnumerable(Of Models.Category)

    ''' <summary>
    ''' Gets category permission
    ''' </summary>
    ''' <param name="categoryPermissionId"></param>
    ''' <param name="categoryId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getCategoryPermission(categoryPermissionId As Integer, categoryId As Integer) As Models.CategoryPermission

    ''' <summary>
    ''' Gets a list of permissions by category id
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getCategoriesRolePermissions(roleId As Integer) As IEnumerable(Of Models.CategoryPermission)

    ''' <summary>
    ''' Adds or updates a category
    ''' </summary>
    ''' <param name="category">Category Model</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function addCategory(category As Models.Category) As Models.Category

    ''' <summary>
    ''' Adds or updates a category
    ''' </summary>
    ''' <param name="categoryLang">Category Language Model</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function addCategoryLang(categoryLang As Models.CategoryLang) As Models.CategoryLang

    ''' <summary>
    ''' Adds or updates a category
    ''' </summary>
    ''' <param name="category">Category Model</param>
    ''' <remarks></remarks>
    Sub updateCategory(category As Models.Category)

    ''' <summary>
    ''' Adds or updates a category
    ''' </summary>
    ''' <param name="categoryLang">Category Language Model</param>
    ''' <remarks></remarks>
    Sub updateCategoryLang(categoryLang As Models.CategoryLang)

    ''' <summary>
    ''' Updates a category security
    ''' </summary>
    ''' <param name="catPermission">Category Permission Model</param>
    ''' <remarks></remarks>
    Sub addCategorySecurity(catPermission As Models.CategoryPermission)

    ''' <summary>
    ''' Updates category permission
    ''' </summary>
    ''' <param name="catPermission"></param>
    ''' <remarks></remarks>
    Sub updateCategoryPermission(catPermission As Models.CategoryPermission)

    ''' <summary>
    ''' Resets category securities
    ''' </summary>
    ''' <param name="catPermission">Category Permission Model</param>
    ''' <remarks></remarks>
    Sub removeCategorySecurities(catPermission As Models.CategoryPermission)

    ''' <summary>
    ''' Resets category securities
    ''' </summary>
    ''' <param name="catId">Category ID</param>
    ''' <remarks></remarks>
    Sub resetCategorySecurities(catId As Integer)

    ''' <summary>
    ''' Removes category
    ''' </summary>
    ''' <param name="category">Category Model</param>
    ''' <remarks></remarks>
    Sub removeCategory(category As Models.Category)

    ''' <summary>
    ''' Removes category
    ''' </summary>
    ''' <param name="catId">Category ID</param>
    ''' <param name="lang">Language</param>
    ''' <remarks></remarks>
    Sub removeCategory(catId As Integer, lang As String)

    ''' <summary>
    ''' Deletes category
    ''' </summary>
    ''' <param name="category">Category Model</param>
    ''' <remarks></remarks>
    Sub deleteCategory(category As Models.Category)

End Interface
