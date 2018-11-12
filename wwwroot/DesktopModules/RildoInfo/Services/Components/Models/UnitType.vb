
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_UnitTypes")> _
    <PrimaryKey("UnitTypeId", AutoIncrement:=True)> _
    <Cacheable("UnitTypes", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class UnitType
        Implements IUnitType

        Public Property CreatedByUser As Integer Implements IUnitType.CreatedByUser

        Public Property CreatedOnDate As Date Implements IUnitType.CreatedOnDate

        Public Property IsDeleted As Boolean Implements IUnitType.IsDeleted

        Public Property ModifiedByUser As Integer Implements IUnitType.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IUnitType.ModifiedOnDate

        Public Property PortalId As Integer Implements IUnitType.PortalId

        Public Property UnitTypeId As Integer Implements IUnitType.UnitTypeId

        Public Property UnitTypeTitle As String Implements IUnitType.UnitTypeTitle
    End Class
End Namespace