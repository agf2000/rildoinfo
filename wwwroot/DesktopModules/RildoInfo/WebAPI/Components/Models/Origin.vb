Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_Origins")> _
    <PrimaryKey("OriginId", AutoIncrement:=True)> _
    <Cacheable("Origins", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class Origin
        Implements IOrigin

        Public Property OriginId As Integer Implements IOrigin.OriginId

        Public Property OriginName As String Implements IOrigin.OriginName

        Public Property CreatedByUser As Integer Implements IOrigin.CreatedByUser

        Public Property CreatedOnDate As Date Implements IOrigin.CreatedOnDate

        Public Property PortalId As Integer Implements IOrigin.PortalId

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IOrigin.Locked

        Public Property ModifiedByUser As Integer Implements IOrigin.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IOrigin.ModifiedOnDate
    End Class
End Namespace
