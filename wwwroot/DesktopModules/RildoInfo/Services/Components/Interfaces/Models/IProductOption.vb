
Namespace Models
    Public Interface IProductOption

        Property OptionId As Integer

        Property ProductId As Integer

        Property ListOrder As Integer

        Property attributes As String

        Property Lang As String

        Property OptionDesc As String

        Property QtyStockSet As Decimal
    End Interface
End Namespace