Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    '<PrimaryKey("EstimateId", AutoIncrement:=True)> _
    <TableName("RIW_Cashiers")> _
    <PrimaryKey("CashierId", AutoIncrement:=True)> _
    <Cacheable("Cashiers", CacheItemPriority.Default, 20)> _
    <Scope("EstimateId")>
    Public Class Cashier
        Implements ICashier

        Public Property CreatedByUser As Integer Implements ICashier.CreatedByUser

        Public Property CreatedOnDate As Date Implements ICashier.CreatedOnDate

        Public Property ModifiedByUser As Integer Implements ICashier.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements ICashier.ModifiedOnDate

        Public Property PortalId As Integer Implements ICashier.PortalId

        Public Property TotalBank As Single Implements ICashier.TotalBank

        Public Property TotalCard As Single Implements ICashier.TotalCard

        Public Property TotalDebit As Single Implements ICashier.TotalDebit

        Public Property TotalCash As Single Implements ICashier.TotalCash

        Public Property TotalCheck As Single Implements ICashier.TotalCheck

        Public Property CashierId As Integer Implements ICashier.CashierId

        <IgnoreColumn>
        Public Property EstimateId As Integer Implements ICashier.EstimateId

        <IgnoreColumn>
        Public Property TotalRows As Integer Implements ICashier.TotalRows
    End Class
End Namespace