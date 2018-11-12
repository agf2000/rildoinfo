Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_ClientPartnerBankRefs")> _
    <PrimaryKey("ClientPartnerBankRefId", AutoIncrement:=True)> _
    <Cacheable("ClientPartnerBankRefs", CacheItemPriority.Default, 20)> _
    <Scope("PersonId")>
    Public Class ClientPartnerBankRef
        Implements IClientPartnerBankRef

        Public Property BankRef As String Implements IClientPartnerBankRef.BankRef

        Public Property BankRefAccount As String Implements IClientPartnerBankRef.BankRefAccount

        Public Property BankRefAccountType As String Implements IClientPartnerBankRef.BankRefAccountType

        Public Property BankRefAgency As String Implements IClientPartnerBankRef.BankRefAgency

        Public Property BankRefClientSince As Date Implements IClientPartnerBankRef.BankRefClientSince

        Public Property BankRefContact As String Implements IClientPartnerBankRef.BankRefContact

        Public Property BankRefCredit As String Implements IClientPartnerBankRef.BankRefCredit

        Public Property BankRefPhone As String Implements IClientPartnerBankRef.BankRefPhone

        Public Property PersonId As Integer Implements IClientPartnerBankRef.PersonId

        Public Property ClientPartnerBankRefId As Integer Implements IClientPartnerBankRef.ClientPartnerBankRefId

        Public Property PartnerName As String Implements IClientPartnerBankRef.PartnerName

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IClientPartnerBankRef.Locked
    End Class
End Namespace
