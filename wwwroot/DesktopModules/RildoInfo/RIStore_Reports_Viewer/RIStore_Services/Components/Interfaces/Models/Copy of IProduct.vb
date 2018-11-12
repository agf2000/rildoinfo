
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IProduct

        Property Name() As String

        Property ProdId() As Integer

        Property ItemType() As Integer

        Property ProdRef() As String

        Property ProdName() As String

        Property DisplayName() As String

        Property ProdIntro() As String

        Property ProdDesc() As String

        Property ProdBrand() As Integer

        Property ProdModel() As Integer

        Property ProdBarCode() As String

        Property ProdUnit() As Integer

        Property ProdImagesId() As Integer

        Property Extension() As String

        Property PubStartDate() As DateTime

        Property PubEndDate() As DateTime

        Property SaleStartDate() As DateTime

        Property SaleEndDate() As DateTime

        Property FreeShipping() As Boolean

        Property Stock() As Decimal

        Property ReorderPoint() As Integer

        Property UnitTypeId() As Integer

        Property UnitTypeTitle() As String

        Property HasOptions() As Boolean

        Property HasRequiredOptions() As Boolean

        Property HasImages() As Integer

        Property HasVideos() As Boolean

        Property Finan_Sale_Price() As Single

        Property Finan_Special_Price() As Single

        Property IsDeleted() As Boolean

        Property Locked() As Boolean

        Property UnitValue() As Single

        Property TotalRows() As Integer

        Property CreatedByUser() As Integer

        Property CreatedDate() As Date

        Property ModifiedByUser() As Integer

        Property ModifiedOnDate() As Date

    End Interface
End Namespace