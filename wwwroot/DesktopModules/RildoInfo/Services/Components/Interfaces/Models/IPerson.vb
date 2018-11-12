
Namespace Models
    Public Interface IPerson

        Property PortalId As Integer

        Property PersonId As Integer

        Property UserId As Integer

        Property PersonType As Boolean

        Property StatusId As Integer

        Property CompanyName As String

        Property DisplayName As String

        Property DateFounded As DateTime

        Property DateRegistered As DateTime

        Property FirstName As String

        Property LastName As String

        Property Telephone As String

        Property Cell As String

        Property Fax As String

        Property Zero800s As String

        Property Email As String

        Property EIN As String

        Property CPF As String

        Property Ident As String

        Property StateTax As String

        Property CityTax As String

        Property Website As String

        Property RegisterTypes As String

        Property HasECF As Boolean

        Property ECFRequired As Boolean

        Property Networked As Boolean

        Property PayMethods As String

        Property PayPlans As String

        Property CreditLimit As Single

        Property PreDiscount As String

        Property Protested As Boolean

        Property Comments As String

        Property Scheduled As Boolean

        Property Sent As Boolean

        Property IsDeleted As Boolean

        Property PersonAddressId As Integer

        Property MonthlyIncome As Single

        Property SalesRep As Integer

        Property GroupIds As String

        Property CreatedByUser As Integer

        Property CreatedOnDate As DateTime

        Property ModifiedByUser As Integer

        Property ModifiedOnDate As DateTime

        Property Locked As Boolean

        Property Activities As String

        Property TotalRows As Integer

        Property PersonAddresses As List(Of Models.PersonAddress)

        Property Industries As String

        Property CreateLogin As Boolean

        Property HasLogin As Boolean

        Property SalesRepName As String

        Property SalesRepEmail As String

        Property SalesRepPhone As String

        Property Password As String

        'Property ClientId As Integer

        'Property UserId As Integer

        'Property IndustryId As Integer

        'Property PersonType As Boolean

        'Property GroupIDs As String

        'Property PropositionIds As String

        'Property CompanyName As String

        'Property DisplayName As String

        'Property DateFoun As DateTime

        'Property DateRegistered As DateTime

        'Property FirstName As String

        'Property LastName As String

        'Property Telephone As String

        'Property Cell As String

        'Property Fax As String

        'Property Zero800s As String

        'Property Email As String

        'Property Cnpj As String

        'Property Cpf As String

        'Property Ident As String

        'Property StateTax As String

        'Property CityTax As String

        'Property Website As String

        'Property RegisterTypes As String

        'Property HasECF As Boolean

        'Property ECFRequired As Boolean

        'Property Networked As Boolean

        'Property PayMethods As String

        'Property PayPlans As String

        'Property CreditLimit As Single

        'Property PreDiscount As String

        'Property Protested As Boolean

        'Property Comments As String

        'Property Scheduled As Boolean

        'Property Sent As Boolean

        'Property IsDeleted As Boolean

        'Property PersonAddressId As Integer

        'Property MonthlyIncome As Single

        'Property CreatedByUser As Integer

        'Property CreatedOnDate As DateTime

        'Property ModifiedByUser As Integer

        'Property ModifiedOnDate As DateTime

        'Property ClientsCount As Integer

        'Property PropositionId As Integer

        'Property PropositionTitle As String

        'Property HistoryId As Integer

        Property HistoryText As String

        'Property StatusId As Integer

        Property Street As String

        Property Unit As String

        Property Complement As String

        Property District As String

        Property City As String

        Property Region As String

        Property PostalCode As String

        Property Country As String

        'Property SalesRep As Integer

        'Property SalesRepName As String

        'Property SalesRepEmail As String

        'Property SalesRepPhone As String

        Property Biography As String

        Property StatusTitle As String

        Property Subject As String

        Property MessageBody As String

        Property ConnId As String
    End Interface
End Namespace