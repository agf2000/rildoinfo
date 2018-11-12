
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_ProductCategory")> _
    <PrimaryKey("ProductCategoryId")> _
    <Cacheable("ProductCategory", CacheItemPriority.Default, 20)> _
    <Scope("CategoryId")>
    Public Class ProductCategory
        Implements IProductCategory

        Public Property ProductCategoryId As Integer Implements IProductCategory.ProductCategoryId

        Public Property CategoryId As Integer Implements IProductCategory.CategoryId

        Public Property ProductId As Integer Implements IProductCategory.ProductId

    End Class
End Namespace