Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    '<PrimaryKey("EstimateId", AutoIncrement:=True)> _
    <TableName("RIW_PeopleHistoryComments")> _
    <PrimaryKey("CommentId", AutoIncrement:=True)> _
    <Cacheable("PersonHistoryComments", CacheItemPriority.Default, 20)> _
    <Scope("PersonHistoryId")>
    Public Class PersonHistoryComment
        Implements IPersonHistoryComment

        Public Property CreatedByUser As Integer Implements IPersonHistoryComment.CreatedByUser

        Public Property CreatedOnDate As Date Implements IPersonHistoryComment.CreatedOnDate

        Public Property PersonHistoryId As Integer Implements IPersonHistoryComment.PersonHistoryId

        Public Property CommentId As Integer Implements IPersonHistoryComment.CommentId

        Public Property CommentText As String Implements IPersonHistoryComment.CommentText

        <IgnoreColumn> _
        Public Property Avatar As String Implements IPersonHistoryComment.Avatar

        <IgnoreColumn> _
        Public Property DisplayName As String Implements IPersonHistoryComment.DisplayName
    End Class
End Namespace