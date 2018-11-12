
Namespace Models
    Public Interface IProductRelated

        Property RelatedId As Integer

        Property PortalId As Integer

        Property ProductId As Integer

        Property RelatedProductId As Integer

        Property DiscountAmt As Integer

        Property DiscountPercent As Integer

        Property ProductQty As Integer

        Property MaxQty As Integer

        Property RelatedType As Integer

        Property Disabled As Boolean

        Property NotAvailable As Boolean

        Property BiDirectional As Boolean

        Property RelatedProductName As String

        Property RelatedProductRef As String

        Property RelatedProductBarcode As String

        Property RelatedProductImageId As Integer

        Property RelatedExtension As String

        Property RelatedProductPrice As Single

        Property RelatedProductSpecialPrice As Single

        Property RelatedUnitValue As Decimal

        Property RelatedProductUnit As Integer

        Property RelatedFinan_Sale_Price As Single

        Property RelatedFinan_Special_Price As Single

        Property RelatedQtyStockSet As Decimal
    End Interface
End Namespace