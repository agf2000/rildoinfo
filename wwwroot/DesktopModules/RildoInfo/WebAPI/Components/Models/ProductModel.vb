Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_ProductModel")> _
    <PrimaryKey("ModelId", AutoIncrement:=True)> _
    <Cacheable("ProductModel", CacheItemPriority.Default, 20)> _
    <Scope("ProductId")>
    Public Class ProductModel
    Implements IProductModel

        Public Property Allow As Integer Implements IProductModel.Allow

        Public Property Barcode As String Implements IProductModel.Barcode

        Public Property DealerCost As Single Implements IProductModel.DealerCost

        Public Property DealerOnly As Boolean Implements IProductModel.DealerOnly

        Public Property Deleted As Boolean Implements IProductModel.Deleted

        Public Property ListOrder As Integer Implements IProductModel.ListOrder

        Public Property ModelId As Integer Implements IProductModel.ModelId

        Public Property ModelRef As String Implements IProductModel.ModelRef

        Public Property ProductId As Integer Implements IProductModel.ProductId

        Public Property PurchaseCost As Single Implements IProductModel.PurchaseCost

        Public Property QtyRemaining As Decimal Implements IProductModel.QtyRemaining

        Public Property QtyStockSet As Decimal Implements IProductModel.QtyStockSet

        Public Property QtyTrans As Decimal Implements IProductModel.QtyTrans

        Public Property QtyTransDate As DateTime Implements IProductModel.QtyTransDate

        Public Property UnitCost As Single Implements IProductModel.UnitCost

        <IgnoreColumn> _
        Public Property Extra As String Implements IProductModel.Extra

        <IgnoreColumn> _
        Public Property Lang As String Implements IProductModel.Lang

        <IgnoreColumn> _
        Public Property ModelName As String Implements IProductModel.ModelName

    End Class

End Namespace
