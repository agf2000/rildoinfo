
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IProductRelated

        Property RelatedId() As Integer

        Property PortalId() As Integer

        Property ProductId() As Integer

        Property RelatedProductId() As Integer

        Property DiscountAmt() As Integer

        Property DiscountPercent() As Integer

        Property ProductQty() As Integer

        Property MaxQty() As Integer

        Property RelatedType() As Integer

        Property Disabled() As Boolean

        Property NotAvailable() As Boolean

        Property BiDirectional() As Boolean

    End Interface
End Namespace