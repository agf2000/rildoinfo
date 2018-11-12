Namespace Components.Interfaces.Models
    Public Interface IProductVendor

        Property ProductVendorId As Integer
        Property ProductId As Integer
        Property ProductRef As String
        Property PersonId As Integer
        Property DefaultVendor As Boolean
        Property RefXml As String
        Property FirstPurchase As Nullable(Of DateTime)
        Property LastPurchase As Nullable(Of DateTime)

    End Interface
End Namespace