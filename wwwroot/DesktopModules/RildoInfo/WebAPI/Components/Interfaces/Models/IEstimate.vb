
Namespace Components.Interfaces.Models
    Public Interface IEstimate

        Property Canceled As Boolean
        Property CardAmount As Single
        Property DebitAmount As Single
        Property CashAmount As Single
        Property Ccf As Integer
        Property ChequeAmount As Single
        Property ChequePreAmount As Single
        Property ClientAddress As String
        Property ClientCell As String
        Property ClientCity As String
        Property ClientCityTax As String
        Property ClientCompanyName As String
        Property ClientComplement As String
        Property ClientDisplayName As String
        Property ClientDistrict As String
        Property ClientEin As String
        Property ClientEmail As String
        Property ClientFax As String
        Property ClientFirstName As String
        Property ClientLastName As String
        Property ClientPostalCode As String
        Property ClientRegion As String
        Property ClientStateTax As String
        Property ClientTelephone As String
        Property ClientUnit As String
        Property ClientZero800s As String
        Property Comment As String
        Property ConnId As String
        Property Coupon As Integer
        Property CouponAttached As Integer
        Property CovenantAmount As Single
        Property CreatedByUser As Integer
        Property CreatedOnDate As DateTime
        Property CreditAmount As Single
        Property CurrentTimestamp As Integer
        Property Discount As Decimal
        Property EstimateId As Integer
        Property EstimateItems As IEnumerable(Of Components.Models.EstimateItem)
        Property EstimateTitle As String
        Property Extras As Single
        Property Guid As String
        Property Inst As String
        Property IsDeleted As Boolean
        Property Locked As Boolean
        Property ModifiedByUser As Integer
        Property ModifiedOnDate As Nullable(Of DateTime)
        Property NumDav As Integer
        Property NumDoc As Integer
        Property OldId As Integer
        Property OtherDiscount As Single
        Property PayCondDisc As Decimal
        Property PayCondId As Integer
        Property PayCondIn As Single
        Property PayCondInst As Single
        Property PayCondInterval As Integer
        Property PayCondN As Integer
        Property PayCondPerc As Decimal
        Property PayCondType As String
        Property PayForm As Integer
        Property PayInDays As Integer
        Property PayOption As Integer
        Property PersonId As Integer
        Property PortalId As Integer
        Property Products As IEnumerable(Of Components.Models.Product)
        Property SaleDate As Nullable(Of DateTime)
        Property SalesRep As Integer
        Property SalesRepEmail As String
        Property SalesRepName As String
        Property SalesRepPhone As String
        Property SequenciaDav As Integer
        Property StatusColor As String
        Property StatusId As Integer
        Property StatusTitle As String
        Property TicketAmount As Single
        Property TotalAmount As Single
        Property TotalBank As Single
        Property TotalCard As Single
        Property TotalCash As Single
        Property TotalCheck As Single
        Property TotalEstimates As Single
        Property TotalPayCond As Single
        Property TotalPayments As Single
        Property TotalRows As Integer
        Property TotalSales As Single
        Property ViewPrice As Boolean
    End Interface
End Namespace