
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IProductModel

        Property ModelId() As Integer

        Property ProductId() As Integer

        Property ListOrder() As Integer

        Property UnitCost() As Single

        Property Barcode() As String

        Property ModelRef() As String

        Property QtyRemaining() As Decimal

        Property QtyTrans() As Decimal

        Property QtyTransDate() As DateTime

        Property Deleted() As Boolean

        Property QtyStockSet() As Decimal

        Property DealerCost() As Single

        Property PurchaseCost() As Single

        Property DealerOnly() As Boolean

        Property Allow() As Integer

        Property Lang() As String

        Property ModelName() As String

        Property Extra() As String

    End Interface
End Namespace