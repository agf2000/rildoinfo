Namespace RI.Modules.RIStore_Services.Models
    Public Interface IIndustry

#Region "Industries"

        Property PortalId() As Integer

        Property IndustryId() As Integer

        Property IndustryTitle() As String

        Property ClientIndustryId() As Integer

        Property IsDeleted() As Boolean

        Property Locked() As Boolean

        Property CreatedByUser() As Integer

        Property CreatedOnDate() As Date

        Property ModifiedByUser() As Integer

        Property ModifiedOnDate() As Date

#End Region

    End Interface
End Namespace