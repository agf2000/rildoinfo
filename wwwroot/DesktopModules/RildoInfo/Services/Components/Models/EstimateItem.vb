
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_EstimateItems")> _
    <PrimaryKey("EstimateItemId", AutoIncrement:=True)> _
    <Cacheable("EstimateItems", CacheItemPriority.Default, 20)> _
    <Scope("EstimateId")>
    Public Class EstimateItem
        Implements IEstimateItem

        Public Property CreatedByUser As Integer Implements IEstimateItem.CreatedByUser

        Public Property CreatedOnDate As DateTime Implements IEstimateItem.CreatedOnDate

        <IgnoreColumn> _
        Public Property CurrentTimestamp As String Implements IEstimateItem.CurrentTimestamp

        Public Property ProductDiscount As Decimal Implements IEstimateItem.ProductDiscount

        Public Property EstimateId As Integer Implements IEstimateItem.EstimateId

        Public Property EstimateItemId As Integer Implements IEstimateItem.EstimateItemId

        Public Property ModifiedByUser As Integer Implements IEstimateItem.ModifiedByUser

        Public Property ModifiedOnDate As DateTime Implements IEstimateItem.ModifiedOnDate

        Public Property POSels As String Implements IEstimateItem.POSels

        Public Property POSelsText As String Implements IEstimateItem.POSelsText

        Public Property ProductEstimateOriginalPrice As Single Implements IEstimateItem.ProductEstimateOriginalPrice

        Public Property ProductEstimatePrice As Single Implements IEstimateItem.ProductEstimatePrice

        Public Property ProductId As Integer Implements IEstimateItem.ProductId

        Public Property ProductQty As Double Implements IEstimateItem.ProductQty

        <IgnoreColumn> _
        Public Property RemoveReasonId As String Implements IEstimateItem.RemoveReasonId

        <IgnoreColumn> _
        Public Property PortalId As Integer Implements IEstimateItem.PortalId

        <IgnoreColumn> _
        Public Property TotalAmount As Single Implements IEstimateItem.TotalAmount

        <IgnoreColumn> _
        Public Property Comment As String Implements IEstimateItem.Comment

        <IgnoreColumn> _
        Public Property Barcode As String Implements IEstimateItem.Barcode

        <IgnoreColumn> _
        Public Property ProductName As String Implements IEstimateItem.ProductName

        <IgnoreColumn> _
        Public Property ProductRef As String Implements IEstimateItem.ProductRef

        <IgnoreColumn> _
        Public Property Summary As String Implements IEstimateItem.Summary

        <IgnoreColumn> _
        Public Property UnitTypeTitle As String Implements IEstimateItem.UnitTypeTitle

        <IgnoreColumn> _
        Public Property ProductUnit As Integer Implements IEstimateItem.ProductUnit

        <IgnoreColumn> _
        Public Property UnitValue As Single Implements IEstimateItem.UnitValue

        <IgnoreColumn> _
        Public Property ExtendedAmount As Decimal Implements IEstimateItem.ExtendedAmount

        <IgnoreColumn> _
        Public Property Extension As String Implements IEstimateItem.Extension

        <IgnoreColumn> _
        Public Property ProductImageId As Integer Implements IEstimateItem.ProductImageId

        <IgnoreColumn> _
        Public Property CategoriesNames As String Implements IEstimateItem.CategoriesNames

        <IgnoreColumn> _
        Public Property Finan_Cost As Single Implements IEstimateItem.Finan_Cost

        <IgnoreColumn> _
        Public Property Finan_Rep As Decimal Implements IEstimateItem.Finan_Rep

        <IgnoreColumn> _
        Public Property Finan_SalesPerson As Decimal Implements IEstimateItem.Finan_SalesPerson

        <IgnoreColumn> _
        Public Property Finan_Tech As Decimal Implements IEstimateItem.Finan_Tech

        <IgnoreColumn> _
        Public Property Finan_Telemarketing As Decimal Implements IEstimateItem.Finan_Telemarketing

        <IgnoreColumn> _
        Public Property Finan_Manager As Decimal Implements IEstimateItem.Finan_Manager

        <IgnoreColumn> _
        Public Property QtyStockSet As Decimal Implements IEstimateItem.QtyStockSet

        <IgnoreColumn> _
        Public Property IsDeleted As Boolean Implements IEstimateItem.IsDeleted

        <IgnoreColumn> _
        Public Property ItemIndex As Integer Implements IEstimateItem.RowID

        <IgnoreColumn> _
        Public Property ConnId As String Implements IEstimateItem.ConnId
    End Class
End Namespace