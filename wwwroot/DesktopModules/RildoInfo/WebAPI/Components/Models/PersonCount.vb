Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_People")> _
    <PrimaryKey("PersonId", AutoIncrement:=True)> _
    <Cacheable("PersonCount", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class PersonCount
        Implements IPersonCount

        Public Property PersonId As Integer Implements IPersonCount.PersonId
    End Class
End Namespace
