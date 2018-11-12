Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

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

        Public Property ProductQty As Decimal Implements IProductRelated.ProductQty

        Public Property RelatedId As Integer Implements IProductRelated.RelatedId

        Public Property RelatedProductId As Integer Implements IProductRelated.RelatedProductId

        Public Property RelatedType As Integer Implements IProductRelated.RelatedType

        <IgnoreColumn>
        Public Property RelatedProductName As String Implements IProductRelated.RelatedProductName

        <IgnoreColumn>
        Public Property RelatedProductRef As String Implements IProductRelated.RelatedProductRef

        <IgnoreColumn>
        Public Property RelatedExtension As String Implements IProductRelated.RelatedExtension

        <IgnoreColumn>
        Public Property RelatedProductImageId As Integer Implements IProductRelated.RelatedProductImageId

        <IgnoreColumn>
        Public Property RelatedProductUnit As Integer Implements IProductRelated.RelatedProductUnit

        <IgnoreColumn>
        Public Property RelatedFinan_Sale_Price As Single Implements IProductRelated.RelatedFinan_Sale_Price

        <IgnoreColumn>
        Public Property RelatedFinan_Special_Price As Single Implements IProductRelated.RelatedFinan_Special_Price

        <IgnoreColumn>
        Public Property RelatedQtyStockSet As Decimal Implements IProductRelated.RelatedQtyStockSet

        <IgnoreColumn>
        Public Property RelatedCategoryNames As String Implements IProductRelated.RelatedCategoryNames

        <IgnoreColumn>
        Public Property RelatedFinan_Cost As Single Implements IProductRelated.RelatedFinan_Cost

        <IgnoreColumn>
        Public Property RelatedFinan_Manager As Decimal Implements IProductRelated.RelatedFinan_Manager

        <IgnoreColumn>
        Public Property RelatedFinan_Rep As Decimal Implements IProductRelated.RelatedFinan_Rep

        <IgnoreColumn>
        Public Property RelatedFinan_SalesPerson As Decimal Implements IProductRelated.RelatedFinan_SalesPerson

        <IgnoreColumn>
        Public Property RelatedFinan_Tech As Decimal Implements IProductRelated.RelatedFinan_Tech

        <IgnoreColumn>
        Public Property RelatedFinan_Telemarketing As Decimal Implements IProductRelated.RelatedFinan_Telemarketing

        <IgnoreColumn>
        Public Property RelatedShowPrice As Boolean Implements IProductRelated.RelatedShowPrice

        <IgnoreColumn>
        Public Property RelatedItemType As Integer Implements IProductRelated.RelatedItemType

        <IgnoreColumn>
        Public Property RelatedFeatured As Boolean Implements IProductRelated.RelatedFeatured

        <IgnoreColumn>
        Public Property RelatedArchived As Boolean Implements IProductRelated.RelatedArchived

        <IgnoreColumn>
        Public Property RelatedCreatedByUser As Integer Implements IProductRelated.RelatedCreatedByUser

        <IgnoreColumn>
        Public Property RelatedCreatedOnDate As Date Implements IProductRelated.RelatedCreatedOnDate

        <IgnoreColumn>
        Public Property RelatedModifiedByUser As Integer Implements IProductRelated.RelatedModifiedByUser

        <IgnoreColumn>
        Public Property RelatedModifiedOnDate As Date Implements IProductRelated.RelatedModifiedOnDate

        <IgnoreColumn>
        Public Property RelatedIsDeleted As Boolean Implements IProductRelated.RelatedIsDeleted

        <IgnoreColumn>
        Public Property RelatedLang As String Implements IProductRelated.RelatedLang

        <IgnoreColumn>
        Public Property RelatedSummary As String Implements IProductRelated.RelatedSummary

        <IgnoreColumn>
        Public Property RelatedDescription As String Implements IProductRelated.RelatedDescription

        <IgnoreColumn>
        Public Property RelatedReorderPoint As Decimal Implements IProductRelated.RelatedReorderPoint

        <IgnoreColumn>
        Public Property RelatedScaleProduct As Boolean Implements IProductRelated.RelatedScaleProduct

        <IgnoreColumn>
        Public Property RelatedOldId As Integer Implements IProductRelated.RelatedOldId

        <IgnoreColumn>
        Public Property RelatedProductImagesCount As Integer Implements IProductRelated.RelatedProductImagesCount

        <IgnoreColumn>
        Public Property RelatedProductOptionsCount As Integer Implements IProductRelated.RelatedProductOptionsCount

        <IgnoreColumn>
        Public Property RelatedProductsRelatedCount As Integer Implements IProductRelated.RelatedProductsRelatedCount

        <IgnoreColumn>
        Public Property RelatedCategoriesNames As String Implements IProductRelated.RelatedCategoriesNames

        <IgnoreColumn>
        Public Property RelatedUnitValue As Single Implements IProductRelated.RelatedUnitValue

        <IgnoreColumn>
        Public Property RelatedBarcode As String Implements IProductRelated.RelatedBarcode

        <IgnoreColumn>
        Public Property RelatedQtyStockOther As Decimal Implements IProductRelated.RelatedQtyStockOther

        <IgnoreColumn>
        Public Property RelatedUnitTypeTitle As String Implements IProductRelated.RelatedUnitTypeTitle

        <IgnoreColumn>
        Public Property RelatedUnitTypeAbbv As String Implements IProductRelated.RelatedUnitTypeAbbv

        <IgnoreColumn>
        Public Property RelatedLocked As Boolean Implements IProductRelated.RelatedLocked

        <IgnoreColumn>
        Public Property RelatedSEOName As String Implements IProductRelated.RelatedSEOName

        <IgnoreColumn>
        Public Property RelatedTagWords As String Implements IProductRelated.RelatedTagWords

        <IgnoreColumn>
        Public Property RelatedIsHidden As Boolean Implements IProductRelated.RelatedIsHidden

        <IgnoreColumn>
        Public Property RelatedWeight As Decimal Implements IProductRelated.RelatedWeight

        <IgnoreColumn>
        Public Property RelatedLength As Decimal Implements IProductRelated.RelatedLength

        <IgnoreColumn>
        Public Property RelatedHeight As Decimal Implements IProductRelated.RelatedHeight

        <IgnoreColumn>
        Public Property RelatedWidth As Decimal Implements IProductRelated.RelatedWidth

        <IgnoreColumn>
        Public Property RelatedDiameter As Decimal Implements IProductRelated.RelatedDiameter

        <IgnoreColumn>
        Public Property RelatedZipOrigin As String Implements IProductRelated.RelatedZipOrigin

        <IgnoreColumn>
        Public Property RelatedCityOrigin As String Implements IProductRelated.RelatedCityOrigin

        <IgnoreColumn>
        Public Property RelatedFinan_ICMS As Decimal Implements IProductRelated.RelatedFinan_ICMS

        <IgnoreColumn>
        Public Property RelatedFinan_CST As String Implements IProductRelated.RelatedFinan_CST

        <IgnoreColumn>
        Public Property RelatedFinan_Paid As Single Implements IProductRelated.RelatedFinan_Paid

        <IgnoreColumn>
        Public Property RelatedFinan_Paid_Discount As Single Implements IProductRelated.RelatedFinan_Paid_Discount

        <IgnoreColumn>
        Public Property RelatedFinan_IPI As Decimal Implements IProductRelated.RelatedFinan_IPI

        <IgnoreColumn>
        Public Property RelatedFinan_Freight As Decimal Implements IProductRelated.RelatedFinan_Freight

        <IgnoreColumn>
        Public Property RelatedFinan_ICMSFreight As Decimal Implements IProductRelated.RelatedFinan_ICMSFreight

        <IgnoreColumn>
        Public Property RelatedFinan_OtherExpenses As Single Implements IProductRelated.RelatedFinan_OtherExpenses

        <IgnoreColumn>
        Public Property RelatedFinan_DiffICMS As Decimal Implements IProductRelated.RelatedFinan_DiffICMS

        <IgnoreColumn>
        Public Property RelatedFinan_TributeSubICMS As Decimal Implements IProductRelated.RelatedFinan_TributeSubICMS

        <IgnoreColumn>
        Public Property RelatedFinan_ISS As Decimal Implements IProductRelated.RelatedFinan_ISS

        <IgnoreColumn>
        Public Property RelatedFinan_OtherTaxes As Decimal Implements IProductRelated.RelatedFinan_OtherTaxes

        <IgnoreColumn>
        Public Property RelatedFinan_CFOP As String Implements IProductRelated.RelatedFinan_CFOP

        <IgnoreColumn>
        Public Property RelatedFinan_MarkUp As Decimal Implements IProductRelated.RelatedFinan_MarkUp

        <IgnoreColumn>
        Public Property RelatedFinan_Dealer_Price As Single Implements IProductRelated.RelatedFinan_Dealer_Price

        <IgnoreColumn>
        Public Property RelatedFinan_Select As Char Implements IProductRelated.RelatedFinan_Select

        <IgnoreColumn>
        Public Property RelatedFinan_COFINS As Decimal Implements IProductRelated.RelatedFinan_COFINS

        <IgnoreColumn>
        Public Property RelatedFinan_COFINSBase As Decimal Implements IProductRelated.RelatedFinan_COFINSBase

        <IgnoreColumn>
        Public Property RelatedFinan_COFINSTributeSituation As String Implements IProductRelated.RelatedFinan_COFINSTributeSituation

        <IgnoreColumn>
        Public Property RelatedFinan_COFINSTributeSub As String Implements IProductRelated.RelatedFinan_COFINSTributeSub

        <IgnoreColumn>
        Public Property RelatedFinan_COFINSTributeSubBase As String Implements IProductRelated.RelatedFinan_COFINSTributeSubBase

        <IgnoreColumn>
        Public Property RelatedFinan_DefaultBarCode As String Implements IProductRelated.RelatedFinan_DefaultBarCode

        <IgnoreColumn>
        Public Property RelatedFinan_IPITributeSituation As String Implements IProductRelated.RelatedFinan_IPITributeSituation

        <IgnoreColumn>
        Public Property RelatedFinan_NCM As String Implements IProductRelated.RelatedFinan_NCM

        <IgnoreColumn>
        Public Property RelatedFinan_PIS As Decimal Implements IProductRelated.RelatedFinan_PIS

        <IgnoreColumn>
        Public Property RelatedFinan_PISBase As Decimal Implements IProductRelated.RelatedFinan_PISBase

        <IgnoreColumn>
        Public Property RelatedFinan_PISTributeSituation As String Implements IProductRelated.RelatedFinan_PISTributeSituation

        <IgnoreColumn>
        Public Property RelatedFinan_PISTributeSub As Decimal Implements IProductRelated.RelatedFinan_PISTributeSub

        <IgnoreColumn>
        Public Property RelatedFinan_PISTributeSubBase As Decimal Implements IProductRelated.RelatedFinan_PISTributeSubBase

        <IgnoreColumn>
        Public Property RelatedFinan_TributeSituationType As String Implements IProductRelated.RelatedFinan_TributeSituationType
    End Class
End Namespace