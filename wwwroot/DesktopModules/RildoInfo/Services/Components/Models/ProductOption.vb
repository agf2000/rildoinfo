﻿
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_ProductOption")> _
    <PrimaryKey("OptionId", AutoIncrement:=True)> _
    <Cacheable("ProductOption", CacheItemPriority.Default, 20)> _
    <Scope("ProductId")>
    Public Class ProductOption
        Implements IProductOption

        Public Property attributes As String Implements IProductOption.attributes

        Public Property ListOrder As Integer Implements IProductOption.ListOrder

        Public Property OptionId As Integer Implements IProductOption.OptionId

        Public Property ProductId As Integer Implements IProductOption.ProductId

        Public Property QtyStockSet As Decimal Implements IProductOption.QtyStockSet

        <IgnoreColumn> _
        Public Property Lang As String Implements IProductOption.Lang

        <IgnoreColumn> _
        Public Property OptionDesc As String Implements IProductOption.OptionDesc
    End Class
End Namespace