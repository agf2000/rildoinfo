Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_ProductImage")> _
    <PrimaryKey("ProductImageId", AutoIncrement:=True)> _
    <Cacheable("ProductImages", CacheItemPriority.Default, 20)> _
    <Scope("ProductId")>
    Public Class ProductImage
        Implements IProductImage

        Public Property ContentLength As Integer Implements IProductImage.ContentLength

        Public Property CreatedByUser As Integer Implements IProductImage.CreatedByUser

        Public Property CreatedOnDate As Date Implements IProductImage.CreatedOnDate

        Public Property Extension As String Implements IProductImage.Extension

        Public Property FileName As String Implements IProductImage.FileName

        Public Property ModifiedByUser As Integer Implements IProductImage.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IProductImage.ModifiedOnDate

        Public Property PortalId As Integer Implements IProductImage.PortalId

        Public Property ProductImageBinary As Byte() Implements IProductImage.ProductImageBinary

        Public Property ProductImageId As Integer Implements IProductImage.ProductImageId

        Public Property ProductImageUrl As String Implements IProductImage.ProductImageUrl

        Public Property ProductId As Integer Implements IProductImage.ProductId

        Public Property ListOrder As Short Implements IProductImage.ListOrder
    End Class
End Namespace