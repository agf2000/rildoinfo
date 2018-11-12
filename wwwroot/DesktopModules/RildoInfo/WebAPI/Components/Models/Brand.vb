Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_Brands")> _
    <PrimaryKey("BrandId", AutoIncrement:=True)> _
    <Cacheable("Brands", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class Brand
        Implements IBrand

        Public Property BrandCode As String Implements IBrand.BrandCode

        Public Property BrandId As Integer Implements IBrand.BrandId

        Public Property BrandTitle As String Implements IBrand.BrandTitle

        Public Property CreatedByUser As Integer Implements IBrand.CreatedByUser

        Public Property CreatedOnDate As Date Implements IBrand.CreatedOnDate

        Public Property IsDeleted As Boolean Implements IBrand.IsDeleted

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IBrand.Locked

        Public Property ModifiedByUser As Integer? Implements IBrand.ModifiedByUser

        Public Property ModifiedOnDate As Date? Implements IBrand.ModifiedOnDate

        Public Property PortalId As Integer Implements IBrand.PortalId
    End Class
End Namespace
