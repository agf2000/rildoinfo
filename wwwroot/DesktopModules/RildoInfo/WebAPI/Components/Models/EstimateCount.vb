Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_Estimates")> _
    <PrimaryKey("EstimateId", AutoIncrement:=True)> _
    <Cacheable("EstimateCounts", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class EstimateCount
        Implements IEstimateCount

        Public Property EstimateId As Integer Implements IEstimateCount.EstimateId
    End Class
End Namespace
