
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IEstimate

#Region "Estimate Variables"

        Property PortalId() As Integer

        Property EstimateId() As Integer

        Property EstimateTitle() As String

        Property ClientId() As Integer

        Property Comment() As String

        Property Inst() As String

        Property ViewPrice() As Boolean

        Property Discount() As Double

        Property TotalAmount() As Single

        Property SalesRep() As Integer

        Property PayOption() As Integer

        Property PayForm() As Integer

        Property Guid() As String

        Property StatusId() As Integer

        Property Locked() As Boolean

        Property IsDeleted() As Boolean

        Property PayCondType() As String

        Property PayCondN() As Integer

        Property PayCondPerc() As Double

        Property PayCondDisc() As Double

        Property PayCondIn() As Single

        Property PayCondInst() As Single

        Property PayCondInterval() As Integer

        Property TotalPayments() As Single

        Property TotalPayCond() As Single

        Property CreatedByUser() As Integer

        Property ModifiedByUser() As Integer

        Property CreatedOnDate() As Date

        Property ModifiedOnDate() As Date

        Property CurrentTimestamp() As Boolean

        Property CommentText() As String

        Property Description() As String

        Property DisplayName() As String

        Property EstDiscount() As Decimal

        Property EstimateItemId() As Integer

        Property DisplayTitle() As String

        Property EstProdOriginalPrice() As String

        Property EstProdPrice() As String

        Property ExtendedAmount() As Single

        Property Finan_Cost() As String

        Property Finan_Rep() As String

        Property Finan_SalesPerson() As String

        Property Finan_Tech() As String

        Property Finan_Telemarketing() As String

        Property HistoryText() As String

        Property MsgText() As String

        Property POSels() As String

        Property POSelsText() As String

        Property Price() As String

        Property ProdBarCode() As String

        Property ProdDesc() As String

        Property ProdId() As Integer

        Property ProdImageBinary() As Byte()

        Property ProdImageUrl As String

        Property ProdImagesId() As Integer

        Property ProdIntro() As String

        Property ProdName() As String

        Property ProdRef() As String

        Property ProdUnit() As Integer

        Property Qty() As String

        Property RemoveReasonId() As String

        Property SaleEndDate() As Date

        Property SalesRepName() As String

        Property SaleStartDate() As Date

        Property ShowPrice() As Boolean

        Property Specs() As String

        Property StatusColor() As String

        Property StatusTitle() As String

        Property Stock() As Integer

        Property UnitAbv() As String

        Property UnitTypeId() As Integer

        Property UnitTypeTitle() As String

        Property UnitValue() As Single

        Property Total() As Single

        Property TotalRows() As Integer

#End Region

    End Interface
End Namespace