
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IClientHistory

#Region "Client History Variables"

        Property HistoryId() As Integer

        Property ClientId() As Integer

        Property HistoryText() As String

        Property CreatedByUser() As Integer

        Property CreatedOnDate() As Date

        Property Locked() As Boolean

#End Region

    End Interface
End Namespace