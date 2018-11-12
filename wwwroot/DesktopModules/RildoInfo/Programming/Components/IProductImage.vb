
Namespace Models
    Public Interface IProductImage

        Property PortalId As Integer

        Property ProductImageId As Integer

        Property ProductId As Integer

        Property ProductImageUrl As String

        Property ProductImageBinary As Byte()

        Property ContentLength As Integer

        Property FileName As String

        Property Extension As String

        Property ListOrder As Int16

        Property CreatedByUser As Integer

        Property CreatedOnDate As Date

        Property ModifiedByUser As Integer

        Property ModifiedOnDate As Date
    End Interface
End Namespace