
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_ClientPersonalRefs")> _
    <PrimaryKey("ClientPersonalRefId", AutoIncrement:=True)> _
    <Cacheable("ClientPersonalRefs", CacheItemPriority.Default, 20)> _
    <Scope("ClientId")>
    Public Class ClientPersonalRef
        Implements IClientPersonalRef

#Region " Private variables "

        Public Property ClientId As Integer Implements IClientPersonalRef.ClientId

        Public Property ClientPersonalRefId As Integer Implements IClientPersonalRef.ClientPersonalRefId

        Public Property PREmail As String Implements IClientPersonalRef.PREmail

        Public Property PRName As String Implements IClientPersonalRef.PRName

        Public Property PRPhone As String Implements IClientPersonalRef.PRPhone

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IClientPersonalRef.Locked

#End Region

    End Class
End Namespace
