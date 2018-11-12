
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_Categories")> _
    <PrimaryKey("CategoryId", AutoIncrement:=True)> _
    <Cacheable("Categories", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class Category
        Implements ICategory

        Public Property Archived As Boolean Implements ICategory.Archived

        <IgnoreColumn> _
        Public Property CategoryDesc As String Implements ICategory.CategoryDesc

        Public Property CategoryId As Integer Implements ICategory.CategoryId

        <IgnoreColumn> _
        Public Property CategoryName As String Implements ICategory.CategoryName

        Public Property CreatedByUser As Integer Implements ICategory.CreatedByUser

        Public Property CreatedOnDate As Date Implements ICategory.CreatedOnDate

        Public Property Hidden As Boolean Implements ICategory.Hidden

        Public Property ImageFile As Integer Implements ICategory.ImageFile

        Public Property ImageURL As String Implements ICategory.ImageURL

        <IgnoreColumn> _
        Public Property Lang As String Implements ICategory.Lang

        Public Property ListAltItemTemplate As String Implements ICategory.ListAltItemTemplate

        Public Property ListItemTemplate As String Implements ICategory.ListItemTemplate

        Public Property ListOrder As Integer Implements ICategory.ListOrder

        <IgnoreColumn> _
        Public Property Message As String Implements ICategory.Message

        <IgnoreColumn> _
        Public Property MetaDescription As String Implements ICategory.MetaDescription

        <IgnoreColumn> _
        Public Property MetaKeywords As String Implements ICategory.MetaKeywords

        Public Property ParentCategoryId As Integer Implements ICategory.ParentCategoryId

        <IgnoreColumn> _
        Public Property ParentName As String Implements ICategory.ParentName

        Public Property PortalId As Integer Implements ICategory.PortalId

        <IgnoreColumn> _
        Public Property ProductCount As Integer Implements ICategory.ProductCount

        Public Property ProductTemplate As String Implements ICategory.ProductTemplate

        <IgnoreColumn> _
        Public Property SEOName As String Implements ICategory.SEOName

        <IgnoreColumn> _
        Public Property SEOPageTitle As String Implements ICategory.SEOPageTitle

        Public Property ModifiedByUser As Integer Implements ICategory.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements ICategory.ModifiedOnDate

        <IgnoreColumn> _
        Public Property SubCategories As Boolean Implements ICategory.SubCategories

        <IgnoreColumn> _
        Public Property ProductId As Integer Implements ICategory.ProductId
    End Class
End Namespace
