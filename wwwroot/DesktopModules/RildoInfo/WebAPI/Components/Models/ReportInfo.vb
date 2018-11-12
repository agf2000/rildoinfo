
Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    Public Class ReportInfo
        Implements IReportInfo

        Public Property BankAmount As Single Implements IReportInfo.BankAmount

        Public Property DebitAmount As Single Implements IReportInfo.DebitAmount

        Public Property CardAmount As Single Implements IReportInfo.CardAmount

        Public Property CashAmount As Single Implements IReportInfo.CashAmount

        Public Property CheckAmount As Single Implements IReportInfo.CheckAmount

        Public Property Comission As Single Implements IReportInfo.Comission

        Public Property Cost As Single Implements IReportInfo.Cost

        Public Property CovenantAmount As Single Implements IReportInfo.CovenantAmount

        Public Property CreditAmount As Single Implements IReportInfo.CreditAmount

        Public Property DisplayName As String Implements IReportInfo.DisplayName

        Public Property EstimateId As Integer Implements IReportInfo.EstimateId

        Public Property ModifiedOnDate As Date Implements IReportInfo.ModifiedOnDate

        Public Property PayCond As String Implements IReportInfo.PayCond

        Public Property Profit As Single Implements IReportInfo.Profit

        Public Property SalesRepName As String Implements IReportInfo.SalesRepName

        Public Property StatusTitle As String Implements IReportInfo.StatusTitle

        Public Property TicketAmount As Single Implements IReportInfo.TicketAmount

        Public Property TotalAmount As Single Implements IReportInfo.TotalAmount

        Public Property Id As Integer Implements IReportInfo.Id

        Public Property Items As IEnumerable(Of ReportInfo) Implements IReportInfo.Items

        Public Property AmountValue As Single Implements IReportInfo.AmountValue

        Public Property ProductName As String Implements IReportInfo.ProductName

        Public Property Qty As Decimal Implements IReportInfo.Qty

        Public Property CreatedOnDate As Date Implements IReportInfo.CreatedOnDate

        Public Property Barcode As String Implements IReportInfo.Barcode

        Public Property ProductRef As String Implements IReportInfo.ProductRef

        Public Property QtyStockOther As Decimal Implements IReportInfo.QtyStockOther

        Public Property QtyStockSet As Decimal Implements IReportInfo.QtyStockSet

        Public Property EstimateInfo As String Implements IReportInfo.EstimateInfo

        Public Property Telephone As String Implements IReportInfo.Telephone

        Public Property ContactName As String Implements IReportInfo.ContactName

        Public Property Email As String Implements IReportInfo.Email

        Public Property DocId As Integer Implements IReportInfo.DocId

        Public Property Credit As Single Implements IReportInfo.Credit

        Public Property Debit As Single Implements IReportInfo.Debit

        Public Property Done As Boolean Implements IReportInfo.Done

        Public Property DueDate As Date Implements IReportInfo.DueDate

        Public Property Fee As Single Implements IReportInfo.Fee

        Public Property InterestRate As Decimal Implements IReportInfo.InterestRate

        Public Property Provider As String Implements IReportInfo.Provider

        Public Property Client As String Implements IReportInfo.Client

        Public Property TransDate As String Implements IReportInfo.TransDate

        Public Property Comment As String Implements IReportInfo.Comment

        Public Property Origin As String Implements IReportInfo.Origin

        Public Property Price As Single Implements IReportInfo.Price

        Public Property ProductId As Integer Implements IReportInfo.ProductId

        Public Property UnitTypeAbbv As String Implements IReportInfo.UnitTypeAbbv

        Public Property UnitValue As Single Implements IReportInfo.UnitValue

        Public Property Finan_Cfop As String Implements IReportInfo.Finan_Cfop

        Public Property Finan_Cst As String Implements IReportInfo.Finan_Cst

        Public Property Finan_Cost As Single Implements IReportInfo.Finan_Cost

        Public Property Finan_Icms As String Implements IReportInfo.Finan_Icms

        Public Property Finan_Ipi As String Implements IReportInfo.Finan_Ipi

        Public Property Finan_Ncm As String Implements IReportInfo.Finan_Ncm

        Public Property Finan_Cest As String Implements IReportInfo.Finan_Cest

        Public Property Finan_Pis As String Implements IReportInfo.Finan_Pis
    End Class

    Public Class ClientListReportInfo
        Implements IClientListReportInfo

        Public Property Codigo As Integer Implements IClientListReportInfo.Codigo

        Public Property Cliente As String Implements IClientListReportInfo.Cliente

        Public Property CreatedOnDate As Date Implements IClientListReportInfo.CreatedOnDate

        Public Property Status As String Implements IClientListReportInfo.Status

        Public Property Vendedor As String Implements IClientListReportInfo.Vendedor

        Public Property Atendido As String Implements IClientListReportInfo.Atendido
    End Class

    Public Class ProductListReportInfo
        Implements IProductListReportInfo

        Public Property Estoque As Decimal Implements IProductListReportInfo.Estoque

        Public Property Produto As String Implements IProductListReportInfo.Produto

        Public Property Ref As String Implements IProductListReportInfo.Ref

        Public Property Codigo As String Implements IProductListReportInfo.Codigo

        Public Property Preco As Single Implements IProductListReportInfo.Preco
    End Class

    Public Class EstimateProductsReportInfo
        Implements IEstimateProductsReportInfo

        Public Property Cliente As String Implements IEstimateProductsReportInfo.Cliente

        Public Property Comis As Single Implements IEstimateProductsReportInfo.Comis

        Public Property Cond_Pag As String Implements IEstimateProductsReportInfo.Cond_Pag

        Public Property Data As String Implements IEstimateProductsReportInfo.Data

        Public Property Forma_Pag As String Implements IEstimateProductsReportInfo.Forma_Pag

        Public Property Lucro As Single Implements IEstimateProductsReportInfo.Lucro

        Public Property Status As String Implements IEstimateProductsReportInfo.Status

        Public Property Total As Single Implements IEstimateProductsReportInfo.Total

        Public Property Vendedor As String Implements IEstimateProductsReportInfo.Vendedor

        Public Property Codigo As String Implements IEstimateProductsReportInfo.Codigo
    End Class

    Public Class ProductEstimatesReportInfo
        Implements IProductEstimatesReportInfo

        Public Property Produto As String Implements IProductEstimatesReportInfo.Produto

        Public Property Qde As Decimal Implements IProductEstimatesReportInfo.Qde

        Public Property Valor As Single Implements IProductEstimatesReportInfo.Valor

        Public Property Codigo As String Implements IProductEstimatesReportInfo.Codigo
    End Class

    Public Class PaymentManagerReportInfo
        Implements IPaymentManagerReportInfo

        Public Property Client As String Implements IPaymentManagerReportInfo.Client

        Public Property Comment As String Implements IPaymentManagerReportInfo.Comment

        Public Property Credit As Single Implements IPaymentManagerReportInfo.Credit

        Public Property Debit As Single Implements IPaymentManagerReportInfo.Debit

        Public Property DocId As Integer Implements IPaymentManagerReportInfo.DocId

        Public Property Done As Boolean Implements IPaymentManagerReportInfo.Done

        Public Property DueDate As Date Implements IPaymentManagerReportInfo.DueDate

        Public Property Fee As Single Implements IPaymentManagerReportInfo.Fee

        Public Property InterestRate As Decimal Implements IPaymentManagerReportInfo.InterestRate

        Public Property Provider As String Implements IPaymentManagerReportInfo.Provider

        Public Property TransDate As String Implements IPaymentManagerReportInfo.TransDate

    End Class

End Namespace
