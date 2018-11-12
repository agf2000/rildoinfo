
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_ClientHistories")> _
    <PrimaryKey("HistoryId", AutoIncrement:=True)> _
    <Cacheable("ClientHistories", CacheItemPriority.Default, 20)> _
    <Scope("ClientId")>
    Public Class ClientHistory
        Implements IClientHistory

#Region " Private variables "

        Public Property ClientId As Integer Implements IClientHistory.ClientId

        Public Property CreatedByUser As Integer Implements IClientHistory.CreatedByUser

        Public Property CreatedOnDate As Date Implements IClientHistory.CreatedOnDate

        Public Property HistoryId As Integer Implements IClientHistory.HistoryId

        Public Property HistoryText As String Implements IClientHistory.HistoryText

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IClientHistory.Locked

#End Region

    End Class
End Namespace
