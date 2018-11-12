
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IBrandModel

#Region "Brand's Model Variables"

        Property ModelId() As Integer

        Property BrandId() As Integer

        Property ModelTitle() As String

        Property IsDeleted() As Boolean

        Property CreatedByUser() As Integer

        Property CreatedOnDate() As Date

        Property ModifiedByUser() As System.Nullable(Of Integer)

        Property ModifiedOnDate() As System.Nullable(Of Date)

        Property Locked() As Boolean

#End Region

    End Interface
End Namespace