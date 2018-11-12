Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_ProductVendor")> _
    <PrimaryKey("ProductVendorId", AutoIncrement:=True)> _
    <Cacheable("ProductVendor", CacheItemPriority.Default, 20)> _
    <Scope("ProductId")>
    Public Class ProductVendor
        Implements IProductVendor

        Public Property DefaultVendor As Boolean Implements IProductVendor.DefaultVendor

        Public Property FirstPurchase As DateTime? Implements IProductVendor.FirstPurchase

        Public Property LastPurchase As DateTime? Implements IProductVendor.LastPurchase

        Public Property PersonId As Integer Implements IProductVendor.PersonId

        Public Property ProductId As Integer Implements IProductVendor.ProductId

        Public Property ProductRef As String Implements IProductVendor.ProductRef

        Public Property ProductVendorId As Integer Implements IProductVendor.ProductVendorId

        Public Property RefXml As String Implements IProductVendor.RefXml
    End Class
End Namespace