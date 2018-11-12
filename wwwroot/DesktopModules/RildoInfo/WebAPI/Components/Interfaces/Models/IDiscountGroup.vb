Namespace Components.Interfaces.Models
    Public Interface IDiscountGroup

        Property PortalId As Integer

        Property DiscountGroupId As Integer

        Property DiscountTitle As String

        Property DiscountDesc As String

        Property DiscountPercent As Double

        Property DiscountValue As Double

        Property DiscountBase As Double

        Property IsDeleted As Boolean

        Property CreatedOnDate As Date

        Property CreatedByUser As Integer

        Property ModifiedOnDate As Date

        Property ModifiedByUser As Integer

    End Interface
End Namespace