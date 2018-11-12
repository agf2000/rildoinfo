
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    '<PrimaryKey("KitItemId", AutoIncrement:=True)> _
    <TableName("RIW_EstimateItemsRemoved")> _
    <PrimaryKey("EstimateItemRemovedId", AutoIncrement:=True)> _
    <Cacheable("EstimateItemsRemoved", CacheItemPriority.Default, 20)> _
    <Scope("EstimateId")>
    Public Class EstimateItemRemoved
        Implements IEstimateItemRemoved

        Public Property CreatedByUser As Integer Implements IEstimateItemRemoved.CreatedByUser

        Public Property CreatedOnDate As Date Implements IEstimateItemRemoved.CreatedOnDate

        Public Property EstimateId As Integer Implements IEstimateItemRemoved.EstimateId

        Public Property ProductId As Integer Implements IEstimateItemRemoved.ProductId

        Public Property ProductQty As Double Implements IEstimateItemRemoved.ProductQty

        Public Property RemoveReasonId As Char Implements IEstimateItemRemoved.RemoveReasonId

        <IgnoreColumn> _
        Public Property EstimateItemId As Integer Implements IEstimateItemRemoved.EstimateItemId

        <IgnoreColumn> _
        Public Property ProductName As String Implements IEstimateItemRemoved.ProductName
    End Class
End Namespace