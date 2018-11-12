
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_ProductRelated")> _
    <PrimaryKey("RelatedId", AutoIncrement:=True)> _
    <Cacheable("ProductRelated", CacheItemPriority.Default, 20)> _
    <Scope("ProductId")>
    Public Class ProductRelated
        Implements IProductRelated

        Public Property BiDirectional As Boolean Implements IProductRelated.BiDirectional

        Public Property Disabled As Boolean Implements IProductRelated.Disabled

        Public Property DiscountAmt As Integer Implements IProductRelated.DiscountAmt

        Public Property DiscountPercent As Integer Implements IProductRelated.DiscountPercent

        Public Property MaxQty As Integer Implements IProductRelated.MaxQty

        Public Property NotAvailable As Boolean Implements IProductRelated.NotAvailable

        Public Property PortalId As Integer Implements IProductRelated.PortalId

        Public Property ProductId As Integer Implements IProductRelated.ProductId

        Public Property ProductQty As Integer Implements IProductRelated.ProductQty

        Public Property RelatedId As Integer Implements IProductRelated.RelatedId

        Public Property RelatedProductId As Integer Implements IProductRelated.RelatedProductId

        Public Property RelatedType As Integer Implements IProductRelated.RelatedType

    End Class
End Namespace