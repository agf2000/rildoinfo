
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_ProductModelLang")> _
    <PrimaryKey("ModelId", AutoIncrement:=False)> _
    <Cacheable("ProductModelLang", CacheItemPriority.Default, 20)> _
    <Scope("Lang")>
    Public Class ProductModelLang
        Implements IProductModelLang

        Public Property Extra As String Implements IProductModelLang.Extra

        Public Property Lang As String Implements IProductModelLang.Lang

        Public Property ModelId As Integer Implements IProductModelLang.ModelId

        Public Property ModelName As String Implements IProductModelLang.ModelName

    End Class
End Namespace