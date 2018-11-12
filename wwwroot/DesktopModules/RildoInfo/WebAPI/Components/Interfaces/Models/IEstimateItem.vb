Namespace Components.Interfaces.Models
    Public Interface IEstimateItem

        Property Barcode As String
        Property CategoriesNames As String
        Property Comment As String
        Property ConnId As String
        Property CreatedByUser As Integer
        Property CreatedOnDate As DateTime
        Property CurrentTimestamp As String
        Property EstimateId As Integer
        Property EstimateItemId As Integer
        Property ExtendedAmount As Decimal
        Property Extension As String
        Property Finan_Cost As Single
        Property Finan_Manager As Decimal
        Property Finan_Rep As Decimal
        Property Finan_SalesPerson As Decimal
        Property Finan_Tech As Decimal
        Property Finan_Telemarketing As Decimal
        Property IsDeleted As Boolean
        Property ModifiedByUser As Integer
        Property ModifiedOnDate As Nullable(Of DateTime)
        Property NumDav As Integer
        Property NumDavItem As Integer
        Property NumDoc As Integer
        Property PortalId As Integer
        Property POSels As String
        Property POSelsText As String
        Property ProductDiscount As Decimal
        Property ProductEstimateOriginalPrice As Single
        Property ProductEstimatePrice As Single
        Property ProductId As Integer
        Property ProductImageId As Integer
        Property ProductName As String
        Property ProductQty As Double
        Property ProductRef As String
        Property ProductUnit As Integer
        Property QtyStockSet As Decimal
        Property RemoveReasonId As String
        Property RowID As Integer
        Property Summary As String
        Property TotalAmount As Single
        Property UnitTypeAbbv As String
        Property UnitTypeTitle As String
        Property UnitValue As Single
    End Interface
End Namespace