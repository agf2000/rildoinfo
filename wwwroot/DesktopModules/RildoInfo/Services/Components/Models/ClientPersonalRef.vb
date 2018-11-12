
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_ClientPersonalRefs")> _
    <PrimaryKey("ClientPersonalRefId", AutoIncrement:=True)> _
    <Cacheable("ClientPersonalRefs", CacheItemPriority.Default, 20)> _
    <Scope("PersonId")>
    Public Class ClientPersonalRef
        Implements IClientPersonalRef

        Public Property PersonId As Integer Implements IClientPersonalRef.PersonId

        Public Property ClientPersonalRefId As Integer Implements IClientPersonalRef.ClientPersonalRefId

        Public Property PREmail As String Implements IClientPersonalRef.PREmail

        Public Property PRName As String Implements IClientPersonalRef.PRName

        Public Property PRPhone As String Implements IClientPersonalRef.PRPhone

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IClientPersonalRef.Locked
    End Class
End Namespace
