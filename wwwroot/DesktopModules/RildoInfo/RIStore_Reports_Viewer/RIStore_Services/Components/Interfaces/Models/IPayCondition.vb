﻿
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IPayCondition

#Region " Private variables "

        Property PortalId() As Integer

        Property EstimateId() As Integer

        Property TotalPayCond() As Single

        Property PayCondId() As Integer

        Property PayCondTitle() As String

        Property PayCondType() As Integer

        Property PayCondStart() As Single

        Property PayCondN() As Integer

        Property PayCondPerc() As Double

        Property PayCondIn() As Single

        Property PayCondDisc() As Double

        Property PayCondInterval() As Integer

        Property GroupIDs() As String

        Property PayIn() As Single

        Property Parcela() As Single

        Property TotalParcelado() As Single

        Property Intervalo() As String

        Property CreatedByUser() As Integer

        Property CreatedOnDate() As Date

        Property ModifiedByUser() As Integer

        Property ModifiedOnDate() As Date

        Property Locked() As Boolean

#End Region

    End Interface
End Namespace