Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_DiscountGroups")> _
    <PrimaryKey("DiscountGroupId", AutoIncrement:=True)> _
    <Cacheable("DiscountGroups", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class DiscountGroup
        Implements IDiscountGroup

        Public Property CreatedByUser As Integer Implements IDiscountGroup.CreatedByUser

        Public Property CreatedOnDate As Date Implements IDiscountGroup.CreatedOnDate

        Public Property DiscountBase As Double Implements IDiscountGroup.DiscountBase

        Public Property DiscountDesc As String Implements IDiscountGroup.DiscountDesc

        Public Property DiscountGroupId As Integer Implements IDiscountGroup.DiscountGroupId

        Public Property DiscountPercent As Double Implements IDiscountGroup.DiscountPercent

        Public Property DiscountTitle As String Implements IDiscountGroup.DiscountTitle

        Public Property DiscountValue As Double Implements IDiscountGroup.DiscountValue

        Public Property IsDeleted As Boolean Implements IDiscountGroup.IsDeleted

        Public Property ModifiedByUser As Integer Implements IDiscountGroup.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IDiscountGroup.ModifiedOnDate

        Public Property PortalId As Integer Implements IDiscountGroup.PortalId
    End Class
End Namespace