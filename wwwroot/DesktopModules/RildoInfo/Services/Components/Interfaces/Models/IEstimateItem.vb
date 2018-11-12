
Namespace Models
    Public Interface IEstimateItem

        Property EstimateItemId As Integer

        Property EstimateId As Integer

        Property ProductId As Integer

        Property ProductQty As Double

        Property ProductEstimateOriginalPrice As Single

        Property ProductEstimatePrice As Single

        Property ProductDiscount As Decimal

        Property POSels As String

        Property POSelsText As String

        Property RemoveReasonId As String

        Property CreatedByUser As Integer

        Property CreatedOnDate As DateTime

        Property ModifiedByUser As Integer

        Property ModifiedOnDate As DateTime

        Property CurrentTimestamp As String

        Property TotalAmount As Single

        Property PortalId As Integer

        Property Comment As String

        Property ProductName As String

        Property Summary As String

        Property ProductRef As String

        Property Barcode As String

        Property ProductUnit As Integer

        Property UnitTypeTitle As String

        Property UnitValue As Single

        Property ExtendedAmount As Decimal

        Property ProductImageId As Integer

        Property Extension As String

        Property CategoriesNames As String

        Property Finan_Cost As Single

        Property Finan_Rep As Decimal

        Property Finan_SalesPerson As Decimal

        Property Finan_Tech As Decimal

        Property Finan_Telemarketing As Decimal

        Property Finan_Manager As Decimal

        Property QtyStockSet As Decimal

        Property IsDeleted As Boolean

        Property RowID As Integer

        Property ConnId As String
    End Interface
End Namespace