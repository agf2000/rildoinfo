
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_ClientCommRefs")> _
    <PrimaryKey("ClientCommRefId", AutoIncrement:=True)> _
    <Cacheable("ClientCommRefs", CacheItemPriority.Default, 20)> _
    <Scope("ClientId")>
    Public Class ClientCommRef
        Implements IClientCommRef

#Region " Private variables "

        Public Property ClientCommRefId As Integer Implements IClientCommRef.ClientCommRefId

        Public Property ClientId As Integer Implements IClientCommRef.ClientId

        Public Property CommRefBusiness As String Implements IClientCommRef.CommRefBusiness

        Public Property CommRefContact As String Implements IClientCommRef.CommRefContact

        Public Property CommRefCredit As Single Implements IClientCommRef.CommRefCredit

        Public Property CommRefLastActivity As Date Implements IClientCommRef.CommRefLastActivity

        Public Property CommRefPhone As String Implements IClientCommRef.CommRefPhone

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IClientCommRef.Locked

#End Region

    End Class
End Namespace
