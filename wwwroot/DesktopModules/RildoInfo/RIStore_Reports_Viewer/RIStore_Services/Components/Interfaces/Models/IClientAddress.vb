
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IClientAddress

#Region "Client Address Variables"

        Property ClientAddressId() As Integer

        Property ClientId() As Integer

        Property AddressName() As String

        Property Street() As String

        Property Unit() As String

        Property Complement() As String

        Property District() As String

        Property City() As String

        Property Region() As String

        Property PostalCode() As String

        Property Country() As String

        Property Telephone() As String

        Property Cell() As String

        Property Fax() As String

        Property ViewOrder() As Integer

        Property IsDeleted() As Boolean

        Property CreatedByUser() As Integer

        Property CreatedOnDate() As Date

        Property ModifiedByUser() As System.Nullable(Of Integer)

        Property ModifiedOnDate() As System.Nullable(Of Date)

        Property Locked() As Boolean

#End Region

    End Interface
End Namespace