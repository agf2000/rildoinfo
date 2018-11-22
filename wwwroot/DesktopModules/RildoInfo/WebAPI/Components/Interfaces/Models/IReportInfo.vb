
Namespace Components.Interfaces.Models

    Public Interface IReportInfo

        Property Id As Integer

        Property EstimateId As Integer

        Property DisplayName As String

        Property ModifiedOnDate As DateTime

        Property SalesRepName As String

        Property StatusTitle As String

        Property TotalAmount As Single

        Property CashAmount As Single

        Property CheckAmount As Single

        Property CardAmount As Single

        Property CreditAmount As Single

        Property CovenantAmount As Single

        Property TicketAmount As Single

        Property BankAmount As Single

        Property DebitAmount As Single

        Property PayCond As String

        Property Comission As Single

        Property Cost As Single

        Property Profit As Single

        Property Items As IEnumerable(Of Components.Models.ReportInfo)

        Property ProductName As String

        Property Qty As Decimal

        Property AmountValue As Single

        Property CreatedOnDate As DateTime

        Property ProductRef As String

        Property Barcode As String

        Property QtyStockSet As Decimal

        Property QtyStockOther As Decimal

        Property EstimateInfo As String

        Property Telephone As String

        Property ContactName As String

        Property Email As String

        Property DocId As Integer

        Property Credit As Single

        Property Debit As Single

        Property Done As Boolean

        Property DueDate As Date

        Property Fee As Single

        Property InterestRate As Decimal

        Property Provider As String

        Property Client As String

        Property TransDate As String

        Property Comment As String

        Property Origin As String

        Property Price As Single

        Property ProductId As Integer

        Property UnitTypeAbbv As String

        Property UnitValue As Single

        Property Finan_Cfop As String

        Property Finan_Cst As String

        Property Finan_Cost As Single

        Property Finan_Icms As String

        Property Finan_Ipi As String

        Property Finan_Ncm As String

        Property Finan_Cest As String

        Property Finan_Pis As String

        Property Discount As Single
    End Interface

    Public Interface IClientListReportInfo

        Property Codigo As Integer

        Property Cliente As String

        Property Vendedor As String

        Property Status As String

        Property CreatedOnDate As DateTime

        Property Atendido As String
    End Interface

    Public Interface IProductListReportInfo

        Property Codigo As String

        Property Ref As String

        Property Produto As String

        Property Estoque As Decimal

        Property Preco As Single
    End Interface

    Public Interface IEstimateProductsReportInfo

        Property Codigo As String

        Property Cliente As String

        Property Data As String

        Property Vendedor As String

        Property Status As String

        Property Total As Single

        Property Forma_Pag As String

        Property Cond_Pag As String

        Property Comis As Single

        Property Lucro As Single
    End Interface

    Public Interface IProductEstimatesReportInfo

        Property Codigo As String

        Property Produto As String

        Property Qde As Decimal

        Property Valor As Single
    End Interface

    Public Interface IPaymentManagerReportInfo

        Property DocId As Integer

        Property Credit As Single

        Property Debit As Single

        Property Done As Boolean

        Property DueDate As Date

        Property Fee As Single

        Property InterestRate As Decimal

        Property Provider As String

        Property Client As String

        Property TransDate As String

        Property Comment As String

    End Interface

End Namespace