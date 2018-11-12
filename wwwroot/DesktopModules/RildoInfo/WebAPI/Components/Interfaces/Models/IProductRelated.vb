Namespace Components.Interfaces.Models
    Public Interface IProductRelated

        Property RelatedId As Integer

        Property PortalId As Integer

        Property ProductId As Integer

        Property RelatedProductId As Integer

        Property DiscountAmt As Integer

        Property DiscountPercent As Integer

        Property ProductQty As Decimal

        Property MaxQty As Integer

        Property RelatedType As Integer

        Property Disabled As Boolean

        Property NotAvailable As Boolean

        Property BiDirectional As Boolean

        Property RelatedCategoryNames As String

        Property RelatedItemType As Integer

        Property RelatedFeatured As Boolean

        Property RelatedArchived As Boolean

        Property RelatedCreatedByUser As Integer

        Property RelatedCreatedOnDate As DateTime

        Property RelatedModifiedByUser As Integer

        Property RelatedModifiedOnDate As DateTime

        Property RelatedIsDeleted As Boolean

        Property RelatedProductRef As String

        Property RelatedLang As String

        Property RelatedSummary As String

        Property RelatedDescription As String

        Property RelatedReorderPoint As Decimal

        Property RelatedShowPrice As Boolean

        Property RelatedScaleProduct As Boolean

        Property RelatedOldId As Integer

        Property RelatedProductImagesCount As Integer

        Property RelatedProductOptionsCount As Integer

        Property RelatedProductsRelatedCount As Integer

        Property RelatedCategoriesNames As String

        Property RelatedUnitValue As Single

        Property RelatedBarcode As String

        Property RelatedQtyStockSet As Decimal

        Property RelatedQtyStockOther As Decimal

        Property RelatedProductImageId As Integer

        Property RelatedExtension As String

        Property RelatedProductName As String

        Property RelatedUnitTypeTitle As String

        Property RelatedUnitTypeAbbv As String

        Property RelatedLocked As Boolean

        Property RelatedSEOName As String

        Property RelatedTagWords As String

        Property RelatedIsHidden As Boolean

        Property RelatedProductUnit As Integer

        Property RelatedWeight As Decimal

        Property RelatedLength As Decimal

        Property RelatedHeight As Decimal

        Property RelatedWidth As Decimal

        Property RelatedDiameter As Decimal

        Property RelatedZipOrigin As String

        Property RelatedCityOrigin As String

        Property RelatedFinan_ICMS As Decimal

        Property RelatedFinan_CST As String

        Property RelatedFinan_Manager As Decimal

        Property RelatedFinan_Cost As Single

        Property RelatedFinan_SalesPerson As Decimal

        Property RelatedFinan_Rep As Decimal

        Property RelatedFinan_Tech As Decimal

        Property RelatedFinan_Telemarketing As Decimal

        Property RelatedFinan_Special_Price As Single

        Property RelatedFinan_Sale_Price As Single

        Property RelatedFinan_Paid As Single

        Property RelatedFinan_Paid_Discount As Single

        Property RelatedFinan_IPI As Decimal

        Property RelatedFinan_Freight As Decimal

        Property RelatedFinan_ICMSFreight As Decimal

        Property RelatedFinan_OtherExpenses As Single

        Property RelatedFinan_DiffICMS As Decimal

        Property RelatedFinan_TributeSubICMS As Decimal

        Property RelatedFinan_ISS As Decimal

        Property RelatedFinan_OtherTaxes As Decimal

        Property RelatedFinan_CFOP As String

        Property RelatedFinan_MarkUp As Decimal

        Property RelatedFinan_Dealer_Price As Single

        Property RelatedFinan_Select As Char

        Property RelatedFinan_COFINS As Decimal

        Property RelatedFinan_COFINSBase As Decimal

        Property RelatedFinan_COFINSTributeSituation As String

        Property RelatedFinan_COFINSTributeSub As String

        Property RelatedFinan_COFINSTributeSubBase As String

        Property RelatedFinan_DefaultBarCode As String

        Property RelatedFinan_IPITributeSituation As String

        Property RelatedFinan_NCM As String

        Property RelatedFinan_PIS As Decimal

        Property RelatedFinan_PISBase As Decimal

        Property RelatedFinan_PISTributeSituation As String

        Property RelatedFinan_PISTributeSub As Decimal

        Property RelatedFinan_PISTributeSubBase As Decimal

        Property RelatedFinan_TributeSituationType As String
    End Interface
End Namespace