
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_Products")> _
    <PrimaryKey("ProductId", AutoIncrement:=True)> _
    <Cacheable("Products", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class Product
        Implements IProduct

        Public Property Archived As Boolean Implements IProduct.Archived

        Public Property CreatedByUser As Integer Implements IProduct.CreatedByUser

        Public Property CreatedOnDate As Date Implements IProduct.CreatedOnDate

        Public Property Featured As Boolean Implements IProduct.Featured

        Public Property IsDeleted As Boolean Implements IProduct.IsDeleted

        Public Property IsHidden As Boolean Implements IProduct.IsHidden

        Public Property ModifiedByUser As Integer Implements IProduct.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IProduct.ModifiedOnDate

        Public Property PortalId As Integer Implements IProduct.PortalId

        Public Property ProductId As Integer Implements IProduct.ProductId

        Public Property ProductRef As String Implements IProduct.ProductRef

        Public Property TaxCategoryId As Integer Implements IProduct.TaxCategoryId

        <IgnoreColumn> _
        Public Property TotalRows As Integer Implements IProduct.TotalRows

        ''Product Language
        <IgnoreColumn> _
        Public Property Description As String Implements IProduct.Description

        <IgnoreColumn> _
        Public Property Lang As String Implements IProduct.Lang

        <IgnoreColumn> _
        Public Property Manufacturer As String Implements IProduct.Manufacturer

        <IgnoreColumn> _
        Public Property ProductName As String Implements IProduct.ProductName

        <IgnoreColumn> _
        Public Property SEOName As String Implements IProduct.SEOName

        <IgnoreColumn> _
        Public Property SEOPageTitle As String Implements IProduct.SEOPageTitle

        <IgnoreColumn> _
        Public Property Summary As String Implements IProduct.Summary

        <IgnoreColumn> _
        Public Property TagWords As String Implements IProduct.TagWords

    End Class
End Namespace