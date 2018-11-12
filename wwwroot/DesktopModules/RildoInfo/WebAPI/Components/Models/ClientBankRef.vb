Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_ClientBankRefs")> _
    <PrimaryKey("ClientBankRefId", AutoIncrement:=True)> _
    <Cacheable("ClientBankRefs", CacheItemPriority.Default, 20)> _
    <Scope("PersonId")>
    Public Class ClientBankRef
        Implements IClientBankRef

        Public Property BankRef As String Implements IClientBankRef.BankRef

        Public Property BankRefAccount As String Implements IClientBankRef.BankRefAccount

        Public Property BankRefAccountType As String Implements IClientBankRef.BankRefAccountType

        Public Property BankRefAgency As String Implements IClientBankRef.BankRefAgency

        Public Property BankRefClientSince As Date Implements IClientBankRef.BankRefClientSince

        Public Property BankRefContact As String Implements IClientBankRef.BankRefContact

        Public Property BankRefCredit As Single Implements IClientBankRef.BankRefCredit

        Public Property BankRefPhone As String Implements IClientBankRef.BankRefPhone

        Public Property ClientBankRefId As Integer Implements IClientBankRef.ClientBankRefId

        Public Property PersonId As Integer Implements IClientBankRef.PersonId

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IClientBankRef.Locked
    End Class
End Namespace
