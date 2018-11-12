Namespace Components.Interfaces.Models
    Public Interface IClientPartnerBankRef

        Property ClientPartnerBankRefId As Integer

        Property PersonId As Integer

        Property PartnerName As String

        Property BankRef As String

        Property BankRefAgency As String

        Property BankRefAccount As String

        Property BankRefClientSince As Date

        Property BankRefContact As String

        Property BankRefPhone As String

        Property BankRefAccountType As String

        Property BankRefCredit As String

        Property Locked As Boolean
    End Interface
End Namespace