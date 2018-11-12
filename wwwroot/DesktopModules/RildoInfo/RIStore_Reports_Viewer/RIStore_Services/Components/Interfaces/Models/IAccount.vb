
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IAccount

#Region "Creditor Account Variables"

        Property PortalId() As Integer

        Property AccountId() As Integer

        Property AccountName() As String

        Property CreatedByUser() As Integer

        Property CreatedOnDate() As Date

        Property ModifiedByUser() As System.Nullable(Of Integer)

        Property ModifiedOnDate() As System.Nullable(Of Date)

        Property Locked() As Boolean

#End Region

    End Interface
End Namespace