Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    '<PrimaryKey("EstimateId", AutoIncrement:=True)> _
    <TableName("RIW_EstimateHistories")> _
    <PrimaryKey("EstimateHistoryId", AutoIncrement:=True)> _
    <Cacheable("EstimateHistories", CacheItemPriority.Default, 20)> _
    <Scope("EstimateId")>
    Public Class EstimateHistory
    Implements IEstimateHistory

        Public Property CreatedByUser As Integer Implements IEstimateHistory.CreatedByUser

        Public Property CreatedOnDate As Date Implements IEstimateHistory.CreatedOnDate

        Public Property EstimateHistoryId As Integer Implements IEstimateHistory.EstimateHistoryId

        Public Property EstimateId As Integer Implements IEstimateHistory.EstimateId

        Public Property HistoryText As String Implements IEstimateHistory.HistoryText

        Public Property Locked As Boolean Implements IEstimateHistory.Locked

        <IgnoreColumn> _
        Public Property Avatar As String Implements IEstimateHistory.Avatar

        <IgnoreColumn> _
        Public Property DisplayName As String Implements IEstimateHistory.DisplayName

        <IgnoreColumn> _
        Property HistoryComments As IEnumerable(Of Components.Models.EstimateHistoryComment) Implements IEstimateHistory.HistoryComments

        <IgnoreColumn> _
        Public Property ConnId As String Implements IEstimateHistory.ConnId
    End Class

End Namespace
