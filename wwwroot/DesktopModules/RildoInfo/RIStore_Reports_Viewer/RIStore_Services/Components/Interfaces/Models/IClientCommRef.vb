
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IClientCommRef

        Property ClientCommRefId() As Integer

        Property ClientId() As Integer

        Property CommRefBusiness() As String

        Property CommRefContact() As String

        Property CommRefPhone() As String

        Property CommRefLastActivity() As Date

        Property CommRefCredit() As Single

        Property Locked() As Boolean

    End Interface
End Namespace