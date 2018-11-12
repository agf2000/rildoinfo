
Namespace Models
    Public Interface IPersonContact

        Property PersonContactId As Integer

        Property PersonId As Integer

        Property ContactName As String

        Property DateBirth As Date

        Property Dept As String

        Property ContactEmail1 As String

        Property ContactEmail2 As String

        Property ContactPhone1 As String

        Property ContactPhone2 As String

        Property PhoneExt1 As String

        Property PhoneExt2 As String

        Property Comments As String

        Property PersonAddressId As Integer

        Property AddressName As String

        Property IsDeleted As Boolean

        Property CreatedByUser As Integer

        Property CreatedOnDate As DateTime

        Property ModifiedByUser As Integer

        Property ModifiedOnDate As DateTime

        Property Locked As Boolean
    End Interface
End Namespace