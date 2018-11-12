
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_ProductImages")> _
    <PrimaryKey("ProdImagesId", AutoIncrement:=True)> _
    <Cacheable("ProductImages", CacheItemPriority.Default, 20)> _
    <Scope("ProdId")>
    Public Class ProductFinance

    End Class
End Namespace