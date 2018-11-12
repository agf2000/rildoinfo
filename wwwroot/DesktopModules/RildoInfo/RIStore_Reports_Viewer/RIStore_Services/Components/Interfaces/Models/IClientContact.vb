
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IClientContact

        Property ClientContactId() As Integer

        Property ClientId() As Integer

        Property ContactName() As String

        Property DateBirth() As Date

        Property Dept() As String

        Property ContactEmail1() As String

        Property ContactEmail2() As String

        Property ContactPhone1() As String

        Property ContactPhone2() As String

        Property PhoneExt1() As String

        Property PhoneExt2() As String

        Property Comments() As String

        Property ClientAddressId() As Integer

        Property AddressName() As String

        Property IsDeleted() As Boolean

        Property CreatedByUser() As Integer

        Property CreatedOnDate() As Date

        Property ModifiedByUser() As System.Nullable(Of Integer)

        Property ModifiedOnDate() As System.Nullable(Of Date)

        Property Locked() As Boolean

    End Interface
End Namespace