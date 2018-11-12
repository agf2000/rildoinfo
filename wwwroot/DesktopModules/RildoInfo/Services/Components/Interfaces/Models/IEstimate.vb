
Namespace Models
    Public Interface IEstimate

        Property PortalId As Integer

        Property EstimateId As Integer

        Property EstimateTitle As String

        Property PersonId As Integer

        Property Comment As String

        Property Inst As String

        Property ViewPrice As Boolean

        Property Discount As Decimal

        Property TotalAmount As Single

        Property SalesRep As Integer

        Property PayOption As Integer

        Property PayForm As Integer

        Property Guid As String

        Property StatusId As Integer

        Property Locked As Boolean

        Property IsDeleted As Boolean

        Property PayCondType As String

        Property PayCondN As Integer

        Property PayCondPerc As Decimal

        Property PayCondDisc As Decimal

        Property PayCondIn As Single

        Property PayCondInst As Single

        Property PayCondInterval As Integer

        Property TotalPayments As Single

        Property TotalPayCond As Single

        Property CreatedByUser As Integer

        Property CreatedOnDate As DateTime

        Property ModifiedByUser As Integer

        Property ModifiedOnDate As DateTime

        Property CurrentTimestamp As Integer

        Property TotalRows As Integer

        Property ClientDisplayName As String

        Property ClientFirstName As String

        Property ClientLastName As String

        Property ClientTelephone As String

        Property ClientCell As String

        Property ClientFax As String

        Property SalesRepName As String

        Property SalesRepEmail As String

        Property SalesRepPhone As String

        Property StatusColor As String

        Property StatusTitle As String

        Property ClientEmail As String

        Property ClientEIN As String

        Property ClientStateTax As String

        Property ClientCityTax As String

        Property ClientAddress As String

        Property ClientUnit As String

        Property ClientComplement As String

        Property ClientDistrict As String

        Property ClientCity As String

        Property ClientRegion As String

        Property ClientPostalCode As String

        Property ClientCompanyName As String

        Property ClientZero800s As String

        Property ConnId As String
    End Interface
End Namespace