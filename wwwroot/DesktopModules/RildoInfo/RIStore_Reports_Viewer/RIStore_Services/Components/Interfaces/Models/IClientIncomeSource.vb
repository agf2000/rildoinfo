
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IClientIncomeSource

#Region "Client Income Source Variables"

        Property ClientISId() As Integer

        Property ClientId() As Integer

        Property ISName() As String

        Property ISEIN() As String

        Property ISST() As String

        Property ISCT() As String

        Property ISPhone() As String

        Property ISFax() As String

        Property ISIncome() As Single

        Property ISPostalCode() As String

        Property ISAddress() As String

        Property ISAddressUnit() As String

        Property ISComplement() As String

        Property ISDistrict() As String

        Property ISRegion() As String

        Property ISCity() As String

        Property ISProof() As Boolean

        Property Locked() As Boolean

#End Region

    End Interface
End Namespace