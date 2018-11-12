
Namespace Models
    Public Interface IProductOptionValue

        Property OptionValueId As Integer

        Property OptionId As Integer

        Property AddedCost As Single

        Property ListOrder As Integer

        Property attributes As String

        Property Lang As String

        Property OptionValueDesc As String

        Property QtyStockSet As Decimal
    End Interface
End Namespace