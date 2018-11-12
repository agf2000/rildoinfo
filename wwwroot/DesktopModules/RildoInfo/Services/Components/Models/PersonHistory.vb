
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_PeopleHistories")> _
    <PrimaryKey("PersonHistoryId", AutoIncrement:=True)> _
    <Cacheable("PeopleHistories", CacheItemPriority.Default, 20)> _
    <Scope("PersonId")>
    Public Class PersonHistory
        Implements IPersonHistory

        Public Property PersonId As Integer Implements IPersonHistory.PersonId

        Public Property CreatedByUser As Integer Implements IPersonHistory.CreatedByUser

        Public Property CreatedOnDate As Date Implements IPersonHistory.CreatedOnDate

        Public Property PersonHistoryId As Integer Implements IPersonHistory.PersonHistoryId

        Public Property HistoryText As String Implements IPersonHistory.HistoryText

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IPersonHistory.Locked

        <IgnoreColumn> _
        Public Property Avatar As String Implements IPersonHistory.Avatar

        <IgnoreColumn> _
        Public Property DisplayName As String Implements IPersonHistory.DisplayName

        <IgnoreColumn> _
        Property HistoryComments As IEnumerable(Of PersonHistoryComment) Implements IPersonHistory.HistoryComments

        <IgnoreColumn> _
        Public Property ConnId As String Implements IPersonHistory.ConnId
    End Class
End Namespace
