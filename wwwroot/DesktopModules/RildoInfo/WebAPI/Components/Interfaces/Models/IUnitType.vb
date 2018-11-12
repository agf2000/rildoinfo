Namespace Components.Interfaces.Models
    Public Interface IUnitType

        Property PortalId As Integer

        Property UnitTypeId As Integer

        Property UnitTypeTitle As String

        Property IsDeleted As Boolean

        Property CreatedByUser As Integer

        Property CreatedOnDate As Date

        Property ModifiedByUser As Integer

        Property ModifiedOnDate As Date

        Property UnitTypeAbbv As String
    End Interface
End Namespace