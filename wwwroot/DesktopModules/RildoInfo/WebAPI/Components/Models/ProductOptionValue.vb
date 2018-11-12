Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_ProductOptionValue")> _
    <PrimaryKey("OptionValueId", AutoIncrement:=True)> _
    <Cacheable("ProductOptionValue", CacheItemPriority.Default, 20)> _
    <Scope("OptionId")>
    Public Class ProductOptionValue
        Implements IProductOptionValue

        Public Property AddedCost As Single Implements IProductOptionValue.AddedCost

        Public Property Attributes As String Implements IProductOptionValue.Attributes

        Public Property ListOrder As Integer Implements IProductOptionValue.ListOrder

        Public Property OptionId As Integer Implements IProductOptionValue.OptionId

        Public Property OptionValueId As Integer Implements IProductOptionValue.OptionValueId

        Public Property QtyStockSet As Decimal Implements IProductOptionValue.QtyStockSet

        <IgnoreColumn> _
        Public Property Lang As String Implements IProductOptionValue.Lang

        <IgnoreColumn> _
        Public Property OptionValueDesc As String Implements IProductOptionValue.OptionValueDesc
    End Class
End Namespace