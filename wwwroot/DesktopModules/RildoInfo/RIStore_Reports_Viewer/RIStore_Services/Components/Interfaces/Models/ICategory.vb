
Namespace RI.Modules.RIStore_Services.Models
    Public Interface ICategory

#Region "Category Variables"

        Property CategoryId() As Integer

        Property Lang() As String

        Property CategoryName() As String

        Property CategoryDesc() As String

        Property Message() As String

        Property PortalId() As Integer

        Property Archived() As Boolean

        Property Hidden() As Boolean

        Property CreatedByUser() As Integer

        Property CreatedOnDate As DateTime

        Property ParentCategoryId() As Integer

        Property ParentName() As String

        Property ListOrder() As Integer

        Property ProductTemplate() As String

        Property ListItemTemplate() As String

        Property ListAltItemTemplate() As String

        'Property ImageBinary() As System.Nullable(Of Byte())

        Property ImageFile() As Integer

        Property ImageURL() As String

        Property SEOPageTitle() As String

        Property SEOName() As String

        Property MetaDescription() As String

        Property MetaKeywords() As String

        Property ProductCount() As Integer

        Property ModifiedByUser() As Integer

        Property ModifiedOnDate() As DateTime

        Property SubCategories() As Boolean

#End Region

    End Interface
End Namespace