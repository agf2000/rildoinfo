
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_Products")> _
    <PrimaryKey("ProductId", AutoIncrement:=True)> _
    <Cacheable("Products", CacheItemPriority.Default, 20000)> _
    <Scope("PortalId")>
    Public Class Product
        Implements IProduct

        Public Property Archived As Boolean Implements IProduct.Archived

        Public Property Barcode As String Implements IProduct.Barcode

        Public Property Brand As Integer Implements IProduct.Brand

        Public Property BrandModel As Integer Implements IProduct.BrandModel

        Public Property CreatedByUser As Integer Implements IProduct.CreatedByUser

        Public Property CreatedOnDate As Date Implements IProduct.CreatedOnDate

        Public Property DealerOnly As Boolean Implements IProduct.DealerOnly

        Public Property ItemType As String Implements IProduct.ItemType

        Public Property ProductUnit As Integer Implements IProduct.ProductUnit

        Public Property ReorderPoint As Decimal Implements IProduct.ReorderPoint

        Public Property SaleStartDate As DateTime Implements IProduct.SaleStartDate

        Public Property SaleEndDate As DateTime Implements IProduct.SaleEndDate

        Public Property CityOrigin As String Implements IProduct.CityOrigin

        Public Property Diameter As Decimal Implements IProduct.Diameter

        Public Property Height As Decimal Implements IProduct.Height

        Public Property Length As Decimal Implements IProduct.Length

        Public Property ServiceTech As String Implements IProduct.ServiceTech

        Public Property ServiceTime As String Implements IProduct.ServiceTime

        Public Property Weight As Decimal Implements IProduct.Weight

        Public Property Width As Decimal Implements IProduct.Width

        Public Property ZipOrigin As String Implements IProduct.ZipOrigin

        Public Property Vendors As String Implements IProduct.Vendors

        <IgnoreColumn> _
        Public Property UnitTypeTitle As String Implements IProduct.UnitTypeTitle

        <IgnoreColumn> _
        Public Property Description As String Implements IProduct.Description

        Public Property Featured As Boolean Implements IProduct.Featured

        Public Property IsDeleted As Boolean Implements IProduct.IsDeleted

        Public Property IsHidden As Boolean Implements IProduct.IsHidden

        <IgnoreColumn> _
        Public Property Lang As String Implements IProduct.Lang

        <IgnoreColumn> _
        Public Property Manufacturer As String Implements IProduct.Manufacturer

        Public Property ModifiedByUser As Integer Implements IProduct.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IProduct.ModifiedOnDate

        Public Property PortalId As Integer Implements IProduct.PortalId

        Public Property ProductId As Integer Implements IProduct.ProductId

        <IgnoreColumn> _
        Public Property ProductName As String Implements IProduct.ProductName

        Public Property ProductRef As String Implements IProduct.ProductRef

        Public Property QtyStockSet As Decimal Implements IProduct.QtyStockSet

        <IgnoreColumn> _
        Public Property ProductOptionsCount As Integer Implements IProduct.ProductOptionsCount

        <IgnoreColumn> _
        Public Property ProductsRelatedCount As Integer Implements IProduct.ProductsRelatedCount

        <IgnoreColumn> _
        Public Property ProductImagesCount As Integer Implements IProduct.ProductImagesCount

        <IgnoreColumn> _
        Public Property ProductImages As IEnumerable(Of ProductImage) Implements IProduct.ProductImages

        <IgnoreColumn> _
        Public Property ProductOptions As IEnumerable(Of Models.ProductOption) Implements IProduct.ProductOptions

        <IgnoreColumn> _
        Public Property ProductOptionValues As IEnumerable(Of Models.ProductOptionValue) Implements IProduct.ProductOptionValues

        <IgnoreColumn> _
        Public Property ProductsRelated As IEnumerable(Of ProductRelated) Implements IProduct.ProductsRelated

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IProduct.Locked

        <IgnoreColumn> _
        Public Property SEOName As String Implements IProduct.SEOName

        <IgnoreColumn> _
        Public Property SEOPageTitle As String Implements IProduct.SEOPageTitle

        <IgnoreColumn> _
        Public Property Summary As String Implements IProduct.Summary

        <IgnoreColumn> _
        Public Property TagWords As String Implements IProduct.TagWords

        <IgnoreColumn> _
        Public Property TotalRows As Integer Implements IProduct.TotalRows

        <IgnoreColumn> _
        Public Property Categories As String Implements IProduct.Categories

        <IgnoreColumn> _
        Public Property Extension As String Implements IProduct.Extension

        <IgnoreColumn> _
        Public Property ProductImageId As Integer Implements IProduct.ProductImageId

        <IgnoreColumn> _
        Public Property CategoriesNames As String Implements IProduct.CategoriesNames

        '' Finance
        <IgnoreColumn> _
        Public Property UnitValue As Decimal Implements IProduct.UnitValue

        <IgnoreColumn> _
        Public Property Finan_Sale_Price As Single Implements IProduct.Finan_Sale_Price

        <IgnoreColumn> _
        Public Property Finan_Special_Price As Single Implements IProduct.Finan_Special_Price

        <IgnoreColumn> _
        Public Property Finan_Cost As Decimal Implements IProduct.Finan_Cost

        <IgnoreColumn> _
        Public Property Finan_Manager As Decimal Implements IProduct.Finan_Manager

        <IgnoreColumn> _
        Public Property Finan_Rep As Decimal Implements IProduct.Finan_Rep

        <IgnoreColumn> _
        Public Property Finan_SalesPerson As Decimal Implements IProduct.Finan_SalesPerson

        <IgnoreColumn> _
        Public Property Finan_Tech As Decimal Implements IProduct.Finan_Tech

        <IgnoreColumn> _
        Public Property Finan_Telemarketing As Decimal Implements IProduct.Finan_Telemarketing
    End Class
End Namespace