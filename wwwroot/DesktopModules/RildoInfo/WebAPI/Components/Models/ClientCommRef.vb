Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_ClientCommRefs")> _
    <PrimaryKey("ClientCommRefId", AutoIncrement:=True)> _
    <Cacheable("ClientCommRefs", CacheItemPriority.Default, 20)> _
    <Scope("PersonId")>
    Public Class ClientCommRef
        Implements IClientCommRef

        Public Property ClientCommRefId As Integer Implements IClientCommRef.ClientCommRefId

        Public Property PersonId As Integer Implements IClientCommRef.PersonId

        Public Property CommRefBusiness As String Implements IClientCommRef.CommRefBusiness

        Public Property CommRefContact As String Implements IClientCommRef.CommRefContact

        Public Property CommRefCredit As Single Implements IClientCommRef.CommRefCredit

        Public Property CommRefLastActivity As Date Implements IClientCommRef.CommRefLastActivity

        Public Property CommRefPhone As String Implements IClientCommRef.CommRefPhone

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IClientCommRef.Locked
    End Class
End Namespace
