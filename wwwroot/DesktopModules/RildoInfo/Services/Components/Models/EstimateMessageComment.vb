
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    '<PrimaryKey("EstimateId", AutoIncrement:=True)> _
    <TableName("RIW_EstimateMessageComments")> _
    <PrimaryKey("CommentId", AutoIncrement:=True)> _
    <Cacheable("EstimateMessageComments", CacheItemPriority.Default, 20)> _
    <Scope("EstimateMessageId")>
    Public Class EstimateMessageComment
        Implements IEstimateMessageComment

        Public Property CreatedByUser As Integer Implements IEstimateMessageComment.CreatedByUser

        Public Property CreatedOnDate As Date Implements IEstimateMessageComment.CreatedOnDate

        Public Property EstimateMessageId As Integer Implements IEstimateMessageComment.EstimateMessageId

        Public Property CommentId As Integer Implements IEstimateMessageComment.CommentId

        Public Property CommentText As String Implements IEstimateMessageComment.CommentText

        <IgnoreColumn> _
        Public Property Avatar As String Implements IEstimateMessageComment.Avatar

        <IgnoreColumn> _
        Public Property DisplayName As String Implements IEstimateMessageComment.DisplayName
    End Class
End Namespace