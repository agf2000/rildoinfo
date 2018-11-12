Namespace Components.Interfaces.Models
    Public Interface IClientBankRef

        Property ClientBankRefId As Integer

        Property PersonId As Integer

        Property BankRef As String

        Property BankRefAgency As String

        Property BankRefAccount As String

        Property BankRefClientSince As Date

        Property BankRefContact As String

        Property BankRefPhone As String

        Property BankRefAccountType As String

        Property BankRefCredit As Single

        Property Locked As Boolean
    End Interface
End Namespace