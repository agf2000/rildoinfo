
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_ProductRelated")> _
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

        <IgnoreColumn> _
        Public Property RelatedProductBarcode As String Implements IProductRelated.RelatedProductBarcode

        <IgnoreColumn> _
        Public Property RelatedProductName As String Implements IProductRelated.RelatedProductName

        <IgnoreColumn> _
        Public Property RelatedProductRef As String Implements IProductRelated.RelatedProductRef

        <IgnoreColumn> _
        Public Property RelatedUnitValue As Decimal Implements IProductRelated.RelatedUnitValue

        <IgnoreColumn> _
        Public Property RelatedExtension As String Implements IProductRelated.RelatedExtension

        <IgnoreColumn> _
        Public Property RelatedProductImageId As Integer Implements IProductRelated.RelatedProductImageId

        <IgnoreColumn> _
        Public Property RelatedProductPrice As Single Implements IProductRelated.RelatedProductPrice

        <IgnoreColumn> _
        Public Property RelatedProductSpecialPrice As Single Implements IProductRelated.RelatedProductSpecialPrice

        <IgnoreColumn> _
        Public Property RelatedProductUnit As Integer Implements IProductRelated.RelatedProductUnit

        <IgnoreColumn> _
        Public Property RelatedFinan_Sale_Price As Single Implements IProductRelated.RelatedFinan_Sale_Price

        <IgnoreColumn> _
        Public Property RelatedFinan_Special_Price As Single Implements IProductRelated.RelatedFinan_Special_Price

        <IgnoreColumn> _
        Public Property RelatedQtyStockSet As Decimal Implements IProductRelated.RelatedQtyStockSet

        <IgnoreColumn> _
        Public Property RelatedCategoryNames As String Implements IProductRelated.RelatedCategoryNames

        <IgnoreColumn> _
        Public Property RelatedFinan_Cost As Single Implements IProductRelated.RelatedFinan_Cost

        <IgnoreColumn> _
        Public Property RelatedFinan_Manager As Decimal Implements IProductRelated.RelatedFinan_Manager

        <IgnoreColumn> _
        Public Property RelatedFinan_Rep As Decimal Implements IProductRelated.RelatedFinan_Rep

        <IgnoreColumn> _
        Public Property RelatedFinan_SalesPerson As Decimal Implements IProductRelated.RelatedFinan_SalesPerson

        <IgnoreColumn> _
        Public Property RelatedFinan_Tech As Decimal Implements IProductRelated.RelatedFinan_Tech

        <IgnoreColumn> _
        Public Property RelatedFinan_Telemarketing As Decimal Implements IProductRelated.RelatedFinan_Telemarketing

        <IgnoreColumn> _
        Public Property RelatedShowPrice As Boolean Implements IProductRelated.RelatedShowPrice
    End Class
End Namespace