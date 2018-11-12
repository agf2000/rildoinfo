Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_ProductOptionLang")> _
    <PrimaryKey("OptionId", AutoIncrement:=False)> _
    <Cacheable("ProductOptionLang", CacheItemPriority.Default, 20)> _
    <Scope("Lang")>
    Public Class ProductOptionLang
        Implements IProductOptionLang

        Public Property Lang As String Implements IProductOptionLang.Lang

        Public Property OptionDesc As String Implements IProductOptionLang.OptionDesc

        Public Property OptionId As Integer Implements IProductOptionLang.OptionId
    End Class
End Namespace