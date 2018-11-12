
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IClientBankRef

#Region "Client Bank Reference Variables"

        Property ClientBankRefId() As Integer

        Property ClientId() As Integer

        Property BankRef() As String

        Property BankRefAgency() As String

        Property BankRefAccount() As String

        Property BankRefClientSince() As Date

        Property BankRefContact() As String

        Property BankRefPhone() As String

        Property BankRefAccountType() As String

        Property BankRefCredit() As Single

        Property Locked() As Boolean

#End Region

    End Interface
End Namespace