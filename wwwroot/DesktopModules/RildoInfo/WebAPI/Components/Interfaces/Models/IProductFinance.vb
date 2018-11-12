Namespace Components.Interfaces.Models
    Public Interface IProductFinance

        Property ProductId As Integer

        Property Finan_Paid As Single

        Property Finan_Paid_Discount As Single

        Property Finan_IPI As Double

        Property Finan_Freight As Single

        Property Finan_ICMSFreight As Double

        Property Finan_OtherExpenses As Single

        Property Finan_DiffICMS As Double

        Property Finan_TributeSubICMS As Double

        Property Finan_ISS As Double

        Property Finan_OtherTaxes As Double

        Property Finan_CFOP As String

        Property Finan_ICMS As Double

        Property Finan_CST As String

        Property Finan_MarkUp As Double

        Property Finan_Sale_Price As Single

        Property Finan_Special_Price As Single

        Property Finan_Dealer_Price As Single

        Property Finan_Manager As Double

        Property Finan_SalesPerson As Double

        Property Finan_Rep As Double

        Property Finan_Telemarketing As Double

        Property Finan_Tech As Double

        Property Finan_Select As Char

        Property Finan_Cost As Single

        Property Finan_COFINS As Double

        Property Finan_COFINSBase As Double

        Property Finan_COFINSTributeSituation As String

        Property Finan_COFINSTributeSub As Double

        Property Finan_COFINSTributeSubBase As Double

        Property Finan_DefaultBarCode As String

        Property Finan_IPITributeSituation As String

        Property Finan_NCM As String

        Property Finan_PIS As Double

        Property Finan_PISBase As Double

        Property Finan_PISTributeSituation As String

        Property Finan_PISTributeSub As Double

        Property Finan_PISTributeSubBase As Double

        Property Finan_TributeSituationType As String

        Property SGI_Group As Integer

        Property Trib_ECF As Integer

        Property SyncEnabled As Boolean
    End Interface
End Namespace