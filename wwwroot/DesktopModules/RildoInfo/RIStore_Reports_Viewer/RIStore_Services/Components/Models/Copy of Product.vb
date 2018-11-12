
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_Products")> _
    <PrimaryKey("ProdId", AutoIncrement:=True)> _
    <Cacheable("Products", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class Product
        Implements IProduct

#Region " Product variables"

        Public Property CreatedByUser As Integer Implements IProduct.CreatedByUser

        Public Property CreatedOnDate As Date Implements IProduct.CreatedDate

        <IgnoreColumn> _
        Public Property DisplayName As String Implements IProduct.DisplayName

        <IgnoreColumn> _
        Public Property Extension As String Implements IProduct.Extension

        <IgnoreColumn> _
        Public Property Finan_Sale_Price As Single Implements IProduct.Finan_Sale_Price

        <IgnoreColumn> _
        Public Property Finan_Special_Price As Single Implements IProduct.Finan_Special_Price

        Public Property FreeShipping As Boolean Implements IProduct.FreeShipping

        <IgnoreColumn> _
        Public Property HasImages As Integer Implements IProduct.HasImages

        <IgnoreColumn> _
        Public Property HasOptions As Boolean Implements IProduct.HasOptions

        <IgnoreColumn> _
        Public Property HasRequiredOptions As Boolean Implements IProduct.HasRequiredOptions

        <IgnoreColumn> _
        Public Property HasVideos As Boolean Implements IProduct.HasVideos

        Public Property IsDeleted As Boolean Implements IProduct.IsDeleted

        Public Property ItemType As Integer Implements IProduct.ItemType

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IProduct.Locked

        <IgnoreColumn> _
        Public Property ModifiedByUser As Integer Implements IProduct.ModifiedByUser

        <IgnoreColumn> _
        Public Property ModifiedOnDate As Date Implements IProduct.ModifiedOnDate

        <IgnoreColumn> _
        Public Property Name As String Implements IProduct.Name

        Public Property ProdBarCode As String Implements IProduct.ProdBarCode

        Public Property ProdBrand As Integer Implements IProduct.ProdBrand

        Public Property ProdDesc As String Implements IProduct.ProdDesc

        Public Property ProdId As Integer Implements IProduct.ProdId

        Public Property ProdImagesId As Integer Implements IProduct.ProdImagesId

        Public Property ProdIntro As String Implements IProduct.ProdIntro

        Public Property ProdModel As Integer Implements IProduct.ProdModel

        Public Property ProdName As String Implements IProduct.ProdName

        Public Property ProdRef As String Implements IProduct.ProdRef

        Public Property ProdUnit As Integer Implements IProduct.ProdUnit

        Public Property PubEndDate As Date Implements IProduct.PubEndDate

        Public Property PubStartDate As Date Implements IProduct.PubStartDate

        Public Property ReorderPoint As Integer Implements IProduct.ReorderPoint

        Public Property SaleEndDate As Date Implements IProduct.SaleEndDate

        Public Property SaleStartDate As Date Implements IProduct.SaleStartDate

        Public Property Stock As Decimal Implements IProduct.Stock

        <IgnoreColumn> _
        Public Property TotalRows As Integer Implements IProduct.TotalRows

        <IgnoreColumn> _
        Public Property UnitTypeId As Integer Implements IProduct.UnitTypeId

        <IgnoreColumn> _
        Public Property UnitTypeTitle As String Implements IProduct.UnitTypeTitle

        <IgnoreColumn> _
        Public Property UnitValue As Single Implements IProduct.UnitValue

#End Region

#Region " Attributes variables"



#End Region

    End Class
End Namespace