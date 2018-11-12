
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    '<PrimaryKey("EstimateId", AutoIncrement:=True)> _
    <TableName("RIS_Cashiers")> _
    <Cacheable("Cashiers", CacheItemPriority.Default, 20)> _
    <Scope("EstimateId")>
    Public Class Cashier
        Implements ICashier

        Public Property CreatedByUser As Integer Implements ICashier.CreatedByUser

        Public Property CreatedOnDate As Date Implements ICashier.CreatedOnDate

        Public Property EstimateId As Integer Implements ICashier.EstimateId

        Public Property PortalId As Integer Implements ICashier.PortalId

        Public Property TotalBank As Single Implements ICashier.TotalBank

        Public Property TotalCard As Single Implements ICashier.TotalCard

        Public Property TotalCash As Single Implements ICashier.TotalCash

        Public Property TotalCheck As Single Implements ICashier.TotalCheck

    End Class
End Namespace