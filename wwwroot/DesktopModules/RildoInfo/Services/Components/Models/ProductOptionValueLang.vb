
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_ProductOptionValueLang")> _
    <PrimaryKey("OptionValueId", AutoIncrement:=False)> _
    <Cacheable("ProductOptionValueLang", CacheItemPriority.Default, 20)> _
    <Scope("Lang")>
    Public Class ProductOptionValueLang
        Implements IProductOptionValueLang

        Public Property Lang As String Implements IProductOptionValueLang.Lang

        Public Property OptionValueDesc As String Implements IProductOptionValueLang.OptionValueDesc

        Public Property OptionValueId As Integer Implements IProductOptionValueLang.OptionValueId
    End Class
End Namespace