
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    '<PrimaryKey("EstimateId", AutoIncrement:=True)> _
    <TableName("RIW_EstimateHistoryComments")> _
    <PrimaryKey("CommentId", AutoIncrement:=True)> _
    <Cacheable("EstimateHistoryComments", CacheItemPriority.Default, 20)> _
    <Scope("EstimateHistoryId")>
    Public Class EstimateHistoryComment
        Implements IEstimateHistoryComment

        Public Property CreatedByUser As Integer Implements IEstimateHistoryComment.CreatedByUser

        Public Property CreatedOnDate As Date Implements IEstimateHistoryComment.CreatedOnDate

        Public Property EstimateHistoryId As Integer Implements IEstimateHistoryComment.EstimateHistoryId

        Public Property CommentId As Integer Implements IEstimateHistoryComment.CommentId

        Public Property CommentText As String Implements IEstimateHistoryComment.CommentText

        <IgnoreColumn> _
        Public Property Avatar As String Implements IEstimateHistoryComment.Avatar

        <IgnoreColumn> _
        Public Property DisplayName As String Implements IEstimateHistoryComment.DisplayName
    End Class
End Namespace