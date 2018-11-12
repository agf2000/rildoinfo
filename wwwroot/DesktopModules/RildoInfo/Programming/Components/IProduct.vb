
Namespace Models
    Public Interface IProduct

        Property ProductId As Integer

        Property PortalId As Integer

        Property Featured As Boolean

        Property Archived As Boolean

        Property CreatedByUser As Integer

        Property CreatedOnDate As Date

        Property IsDeleted As Boolean

        Property ProductRef As String

        Property ModifiedByUser As Integer

        Property ModifiedOnDate As Date

        Property IsHidden As Boolean

        Property TotalRows As Integer

        Property Barcode As String

        Property QtyStockSet As Decimal

        Property DealerOnly As Boolean

        Property Brand As Integer

        Property BrandModel As Integer

        Property ItemType As String

        Property ReorderPoint As Decimal

        Property ShowPrice As Boolean

        Property ProductUnit As Integer

        Property SaleStartDate As DateTime

        Property SaleEndDate As DateTime

        Property Weight As Decimal

        Property Length As Decimal

        Property Height As Decimal

        Property Width As Decimal

        Property Diameter As Decimal

        Property ZipOrigin As String

        Property CityOrigin As String

        Property ServiceTime As String

        Property ServiceTech As String

        Property Vendors As String

        Property Categories As String

        Property Locked As Boolean

        Property UnitTypeTitle As String

        Property UnitTypeAbbv As String

        Property ProductOptionsCount As Integer

        Property ProductsRelatedCount As Integer

        Property ProductImagesCount As Integer

        Property ProductOptions As IEnumerable(Of Models.ProductOption)

        Property ProductOptionValues As IEnumerable(Of Models.ProductOptionValue)

        Property ProductImages As IEnumerable(Of Models.ProductImage)

        Property ProductsRelated As IEnumerable(Of Models.ProductRelated)

        ''Product Lang
        Property Lang As String

        Property Summary As String

        Property Description As String

        Property Manufacturer As String

        Property ProductName As String

        Property SEOName As String

        Property TagWords As String

        Property SEOPageTitle As String

        '' Finance
        Property Finan_Sale_Price As Single

        Property Finan_Special_Price As Single

        Property Finan_Cost As Decimal

        Property Finan_Rep As Decimal

        Property Finan_SalesPerson As Decimal

        Property Finan_Tech As Decimal

        Property Finan_Telemarketing As Decimal

        Property Finan_Manager As Decimal

        '' Others
        Property ProductImageId As Integer

        Property Extension As String

        Property CategoriesNames As String

        Property UnitValue As Decimal

        Property VideoSrc As String
    End Interface
End Namespace