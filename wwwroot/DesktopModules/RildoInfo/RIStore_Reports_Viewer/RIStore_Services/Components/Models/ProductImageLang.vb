
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_ProductImageLang")> _
    <PrimaryKey("ImageId", AutoIncrement:=False)> _
    <Cacheable("ProductImageLang", CacheItemPriority.Default, 20)> _
    <Scope("Lang")>
    Public Class ProductImageLang
        Implements IProductImageLang

        Public Property ImageDesc As String Implements IProductImageLang.ImageDesc

        Public Property ImageId As Integer Implements IProductImageLang.ImageId

        Public Property Lang As String Implements IProductImageLang.Lang

    End Class
End Namespace