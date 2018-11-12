
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_ProductImage")> _
    <PrimaryKey("ImageId", AutoIncrement:=True)> _
    <Cacheable("ProductImage", CacheItemPriority.Default, 20)> _
    <Scope("ProductId")>
    Public Class ProductImage

    End Class
End Namespace