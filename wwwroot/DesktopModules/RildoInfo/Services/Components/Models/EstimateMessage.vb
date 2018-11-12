
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    '<PrimaryKey("EstimateId", AutoIncrement:=True)> _
    <TableName("RIW_EstimateMessages")> _
    <PrimaryKey("EstimateMessageId", AutoIncrement:=True)> _
    <Cacheable("EstimateMessages", CacheItemPriority.Default, 20)> _
    <Scope("EstimateId")>
    Public Class EstimateMessage
        Implements IEstimateMessage

        Public Property CreatedByUser As Integer Implements IEstimateMessage.CreatedByUser

        Public Property CreatedOnDate As Date Implements IEstimateMessage.CreatedOnDate

        Public Property EstimateMessageId As Integer Implements IEstimateMessage.EstimateMessageId

        Public Property EstimateId As Integer Implements IEstimateMessage.EstimateId

        Public Property MessageText As String Implements IEstimateMessage.MessageText

        Public Property Allowed As Boolean Implements IEstimateMessage.Allowed

        Public Property ModifiedByUser As Integer Implements IEstimateMessage.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IEstimateMessage.ModifiedOnDate

        <IgnoreColumn> _
        Public Property Avatar As String Implements IEstimateMessage.Avatar

        <IgnoreColumn> _
        Public Property DisplayName As String Implements IEstimateMessage.DisplayName

        <IgnoreColumn> _
        Public Property MessageComments As List(Of Models.EstimateMessageComment) Implements IEstimateMessage.MessageComments

        <IgnoreColumn> _
        Public Property ConnId As String Implements IEstimateMessage.ConnId
    End Class
End Namespace