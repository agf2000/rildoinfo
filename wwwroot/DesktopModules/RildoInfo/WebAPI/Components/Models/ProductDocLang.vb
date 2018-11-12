Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_ProductDocLang")> _
    <PrimaryKey("DocId", AutoIncrement:=False)> _
    <Cacheable("ProductDocLang", CacheItemPriority.Default, 20)> _
    <Scope("Lang")>
    Public Class ProductDocLang
        Implements IProductDocLang

        Public Property DocDesc As String Implements IProductDocLang.DocDesc

        Public Property DocId As Integer Implements IProductDocLang.DocId

        Public Property Lang As String Implements IProductDocLang.Lang
    End Class
End Namespace