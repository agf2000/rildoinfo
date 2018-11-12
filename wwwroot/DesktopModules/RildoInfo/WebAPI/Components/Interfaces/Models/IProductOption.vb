Namespace Components.Interfaces.Models
    Public Interface IProductOption

        Property OptionId As Integer

        Property ProductId As Integer

        Property ListOrder As Integer

        Property Attributes As String

        Property Lang As String

        Property OptionDesc As String

        Property QtyStockSet As Decimal
    End Interface
End Namespace