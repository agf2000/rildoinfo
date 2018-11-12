Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_ProductFinance")> _
    <PrimaryKey("ProductId", AutoIncrement:=False)> _
    Public Class ProductFinance
        Implements IProductFinance

        Public Property Finan_CFOP As String Implements IProductFinance.Finan_CFOP

        Public Property Finan_COFINS As Double Implements IProductFinance.Finan_COFINS

        Public Property Finan_COFINSBase As Double Implements IProductFinance.Finan_COFINSBase

        Public Property Finan_COFINSTributeSituation As String Implements IProductFinance.Finan_COFINSTributeSituation

        Public Property Finan_COFINSTributeSub As Double Implements IProductFinance.Finan_COFINSTributeSub

        Public Property Finan_COFINSTributeSubBase As Double Implements IProductFinance.Finan_COFINSTributeSubBase

        Public Property Finan_Cost As Single Implements IProductFinance.Finan_Cost

        Public Property Finan_CST As String Implements IProductFinance.Finan_CST

        Public Property Finan_DefaultBarCode As String Implements IProductFinance.Finan_DefaultBarCode

        Public Property Finan_DiffICMS As Double Implements IProductFinance.Finan_DiffICMS

        Public Property Finan_Freight As Single Implements IProductFinance.Finan_Freight

        Public Property Finan_ICMS As Double Implements IProductFinance.Finan_ICMS

        Public Property Finan_ICMSFreight As Double Implements IProductFinance.Finan_ICMSFreight

        Public Property Finan_IPI As Double Implements IProductFinance.Finan_IPI

        Public Property Finan_IPITributeSituation As String Implements IProductFinance.Finan_IPITributeSituation

        Public Property Finan_ISS As Double Implements IProductFinance.Finan_ISS

        Public Property Finan_MarkUp As Double Implements IProductFinance.Finan_MarkUp

        Public Property Finan_NCM As String Implements IProductFinance.Finan_NCM

        Public Property Finan_OtherExpenses As Single Implements IProductFinance.Finan_OtherExpenses

        Public Property Finan_OtherTaxes As Double Implements IProductFinance.Finan_OtherTaxes

        Public Property Finan_Paid As Single Implements IProductFinance.Finan_Paid

        Public Property Finan_Paid_Discount As Single Implements IProductFinance.Finan_Paid_Discount

        Public Property Finan_PIS As Double Implements IProductFinance.Finan_PIS

        Public Property Finan_PISBase As Double Implements IProductFinance.Finan_PISBase

        Public Property Finan_PISTributeSituation As String Implements IProductFinance.Finan_PISTributeSituation

        Public Property Finan_PISTributeSub As Double Implements IProductFinance.Finan_PISTributeSub

        Public Property Finan_PISTributeSubBase As Double Implements IProductFinance.Finan_PISTributeSubBase

        Public Property Finan_Rep As Double Implements IProductFinance.Finan_Rep

        Public Property Finan_Sale_Price As Single Implements IProductFinance.Finan_Sale_Price

        Public Property Finan_Manager As Double Implements IProductFinance.Finan_Manager

        Public Property Finan_SalesPerson As Double Implements IProductFinance.Finan_SalesPerson

        Public Property Finan_Select As Char Implements IProductFinance.Finan_Select

        Public Property Finan_Special_Price As Single Implements IProductFinance.Finan_Special_Price

        Public Property Finan_Dealer_Price As Single Implements IProductFinance.Finan_Dealer_Price

        Public Property Finan_Tech As Double Implements IProductFinance.Finan_Tech

        Public Property Finan_Telemarketing As Double Implements IProductFinance.Finan_Telemarketing

        Public Property Finan_TributeSituationType As String Implements IProductFinance.Finan_TributeSituationType

        Public Property Finan_TributeSubICMS As Double Implements IProductFinance.Finan_TributeSubICMS

        Public Property SGI_Group As Integer Implements IProductFinance.SGI_Group

        Public Property Trib_ECF As Integer Implements IProductFinance.Trib_ECF

        Public Property ProductId As Integer Implements IProductFinance.ProductId

        <IgnoreColumn> _
        Public Property SyncEnabled As Boolean Implements IProductFinance.SyncEnabled
    End Class
End Namespace