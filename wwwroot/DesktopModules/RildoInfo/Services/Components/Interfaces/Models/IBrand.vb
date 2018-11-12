
Namespace Models
    Public Interface IBrand

        Property PortalId() As Integer

        Property BrandId() As Integer

        Property BrandCode() As String

        Property BrandTitle() As String

        Property IsDeleted() As Boolean

        Property CreatedByUser() As Integer

        Property CreatedOnDate() As Date

        Property ModifiedByUser() As System.Nullable(Of Integer)

        Property ModifiedOnDate() As System.Nullable(Of Date)

        Property Locked() As Boolean
    End Interface
End Namespace