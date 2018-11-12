
Namespace Models
    Public Interface IPersonAddress

        Property PersonAddressId As Integer

        Property PersonId As Integer

        Property AddressName As String

        Property Street As String

        Property Unit As String

        Property Complement As String

        Property District As String

        Property City As String

        Property Region As String

        Property PostalCode As String

        Property Country As String

        Property Telephone As String

        Property Cell As String

        Property Fax As String

        Property ViewOrder As Integer

        Property IsDeleted As Boolean

        Property Locked As Boolean

        Property Comment As String

        Property CreatedByUser As Integer

        Property CreatedOnDate As Date

        Property ModifiedByUser As Integer

        Property ModifiedOnDate As Date
    End Interface
End Namespace