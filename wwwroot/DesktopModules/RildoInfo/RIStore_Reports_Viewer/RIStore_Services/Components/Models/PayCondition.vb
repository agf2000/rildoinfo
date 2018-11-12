
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_PayConditions")> _
    <PrimaryKey("PayCondId", AutoIncrement:=True)> _
    <Cacheable("PayConditions", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class PayCondition
        Implements IPayCondition

#Region "Pat Condition Variables"

        Public Property CreatedByUser As Integer Implements IPayCondition.CreatedByUser

        Public Property CreatedOnDate As Date Implements IPayCondition.CreatedOnDate

        <IgnoreColumn> _
        Public Property EstimateId As Integer Implements IPayCondition.EstimateId

        Public Property GroupIDs As String Implements IPayCondition.GroupIDs

        <IgnoreColumn> _
        Public Property Intervalo As String Implements IPayCondition.Intervalo

        Public Property ModifiedByUser As Integer Implements IPayCondition.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IPayCondition.ModifiedOnDate

        <IgnoreColumn> _
        Public Property Parcela As Single Implements IPayCondition.Parcela

        Public Property PayCondDisc As Double Implements IPayCondition.PayCondDisc

        Public Property PayCondId As Integer Implements IPayCondition.PayCondId

        Public Property PayCondIn As Single Implements IPayCondition.PayCondIn

        Public Property PayCondInterval As Integer Implements IPayCondition.PayCondInterval

        Public Property PayCondN As Integer Implements IPayCondition.PayCondN

        Public Property PayCondPerc As Double Implements IPayCondition.PayCondPerc

        Public Property PayCondStart As Single Implements IPayCondition.PayCondStart

        Public Property PayCondTitle As String Implements IPayCondition.PayCondTitle

        Public Property PayCondType As Integer Implements IPayCondition.PayCondType

        <IgnoreColumn> _
        Public Property PayIn As Single Implements IPayCondition.PayIn

        Public Property PortalId As Integer Implements IPayCondition.PortalId

        <IgnoreColumn> _
        Public Property TotalParcelado As Single Implements IPayCondition.TotalParcelado

        <IgnoreColumn> _
        Public Property TotalPayCond As Single Implements IPayCondition.TotalPayCond

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IPayCondition.Locked

#End Region

    End Class
End Namespace