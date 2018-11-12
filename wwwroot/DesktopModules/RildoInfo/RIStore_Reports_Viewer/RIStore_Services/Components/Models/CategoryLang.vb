
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_CategoryLang")> _
    <PrimaryKey("CategoryId", AutoIncrement:=False)> _
    <Cacheable("CategoryLang", CacheItemPriority.Default, 20)> _
    <Scope("Lang")>
    Public Class CategoryLang
        Implements ICategoryLang

        Public Property CategoryDesc As String Implements ICategoryLang.CategoryDesc

        Public Property CategoryId As Integer Implements ICategoryLang.CategoryId

        Public Property CategoryName As String Implements ICategoryLang.CategoryName

        Public Property Lang As String Implements ICategoryLang.Lang

        Public Property Message As String Implements ICategoryLang.Message

        Public Property MetaDescription As String Implements ICategoryLang.MetaDescription

        Public Property MetaKeywords As String Implements ICategoryLang.MetaKeywords

        Public Property SEOName As String Implements ICategoryLang.SEOName

        Public Property SEOPageTitle As String Implements ICategoryLang.SEOPageTitle

    End Class
End Namespace