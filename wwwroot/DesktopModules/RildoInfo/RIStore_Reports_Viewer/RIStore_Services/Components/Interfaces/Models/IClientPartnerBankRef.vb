
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IClientPartnerBankRef

#Region "Client Partner Bank Ref"

        Property ClientPartnerBankRefId() As Integer

        Property ClientId() As Integer

        Property PartnerName() As String

        Property BankRef() As String

        Property BankRefAgency() As String

        Property BankRefAccount() As String

        Property BankRefClientSince() As Date

        Property BankRefContact() As String

        Property BankRefPhone() As String

        Property BankRefAccountType() As String

        Property BankRefCredit() As String

        Property Locked() As Boolean

#End Region

    End Interface
End Namespace