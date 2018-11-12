
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IKit

        Property KitItemId() As Integer

        Property KitId() As Integer

        Property ItemId() As Integer

        Property ProdId() As Integer

        Property Qty() As Double

        Property CreatedByUser() As Integer

        Property CreatedOnDate() As Date

        Property ModifiedByUser() As Integer

        Property ModifiedOnDate() As Date

        Property PortalId() As Integer

        Property CategoryId() As Integer

        Property ParentId() As Integer

        Property Name() As String

        Property ProdName() As String

        Property DisplayName() As String

        Property ProdRef() As String

        Property ProdBarCode() As String

        Property UnitValue() As Single

    End Interface
End Namespace