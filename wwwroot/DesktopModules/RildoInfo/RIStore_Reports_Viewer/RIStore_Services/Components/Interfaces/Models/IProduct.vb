
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IProduct

        Property ProductId() As Integer

        Property PortalId() As Integer

        Property TaxCategoryId() As Integer

        Property Featured() As Boolean

        Property Archived() As Boolean

        Property CreatedByUser() As Integer

        Property CreatedOnDate() As Date

        Property IsDeleted() As Boolean

        Property ProductRef() As String

        Property ModifiedByUser() As Integer

        Property ModifiedOnDate() As Date

        Property IsHidden() As Boolean

        Property TotalRows() As Integer

        ''Product Lang
        Property Lang() As String

        Property Summary() As String

        Property Description() As String

        Property Manufacturer() As String

        Property ProductName() As String

        Property SEOName() As String

        Property TagWords() As String

        Property SEOPageTitle() As String

    End Interface
End Namespace