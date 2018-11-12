
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_ProductLang")> _
    <PrimaryKey("ProductId", AutoIncrement:=False)> _
    <Cacheable("ProductLang", CacheItemPriority.Default, 20)> _
    <Scope("Lang")>
    Public Class ProductLang
        Implements IProductLang

        <IgnoreColumn> _
        Public Property PortalId As Integer Implements IProductLang.PortalId

        Public Property Description As String Implements IProductLang.Description

        Public Property Lang As String Implements IProductLang.Lang

        Public Property Manufacturer As String Implements IProductLang.Manufacturer

        Public Property ProductId As Integer Implements IProductLang.ProductId

        Public Property ProductName As String Implements IProductLang.ProductName

        Public Property SEOName As String Implements IProductLang.SEOName

        Public Property SEOPageTitle As String Implements IProductLang.SEOPageTitle

        Public Property Summary As String Implements IProductLang.Summary

        Public Property SEOSummary As String Implements IProductLang.SEOSummary

        Public Property TagWords As String Implements IProductLang.TagWords

        <IgnoreColumn> _
        Public Property ModifiedByUser As Integer Implements IProductLang.ModifiedByUser

        <IgnoreColumn> _
        Public Property ModifiedOnDate As Date Implements IProductLang.ModifiedOnDate
    End Class
End Namespace