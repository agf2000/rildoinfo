
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_ProductCategory")> _
    <PrimaryKey("ProductCategoryId", AutoIncrement:=True)> _
    <Cacheable("ProductCategory", CacheItemPriority.Default, 20)> _
    <Scope("CategoryId")>
    Public Class ProductCategory
        Implements IProductCategory

        Public Property ProductCategoryId As Integer Implements IProductCategory.ProductCategoryId

        Public Property CategoryId As Integer Implements IProductCategory.CategoryId

        Public Property ProductId As Integer Implements IProductCategory.ProductId
    End Class
End Namespace