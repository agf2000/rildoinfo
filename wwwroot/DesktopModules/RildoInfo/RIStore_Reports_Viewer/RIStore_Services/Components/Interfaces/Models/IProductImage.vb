
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IProductImage

        Property ImageId() As Integer

        Property ProductId() As Integer

        Property FileId() As Integer

        Property ImagePath() As String

        Property ProdImageBinary() As Boolean

        Property ContentLength() As Integer

        Property FileName() As String

        Property Extension() As String

        Property ListOrder() As Integer

        Property Hidden() As Boolean

        Property ImageURL() As String

    End Interface
End Namespace