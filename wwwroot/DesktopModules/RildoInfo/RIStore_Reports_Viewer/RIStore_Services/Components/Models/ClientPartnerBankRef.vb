
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_ClientPartnerBankRefs")> _
    <PrimaryKey("ClientPartnerBankRefId", AutoIncrement:=True)> _
    <Cacheable("ClientPartnerBankRefs", CacheItemPriority.Default, 20)> _
    <Scope("ClientId")>
    Public Class ClientPartnerBankRef
        Implements IClientPartnerBankRef

#Region " Private variables "

        Public Property BankRef As String Implements IClientPartnerBankRef.BankRef

        Public Property BankRefAccount As String Implements IClientPartnerBankRef.BankRefAccount

        Public Property BankRefAccountType As String Implements IClientPartnerBankRef.BankRefAccountType

        Public Property BankRefAgency As String Implements IClientPartnerBankRef.BankRefAgency

        Public Property BankRefClientSince As Date Implements IClientPartnerBankRef.BankRefClientSince

        Public Property BankRefContact As String Implements IClientPartnerBankRef.BankRefContact

        Public Property BankRefCredit As String Implements IClientPartnerBankRef.BankRefCredit

        Public Property BankRefPhone As String Implements IClientPartnerBankRef.BankRefPhone

        Public Property ClientId As Integer Implements IClientPartnerBankRef.ClientId

        Public Property ClientPartnerBankRefId As Integer Implements IClientPartnerBankRef.ClientPartnerBankRefId

        Public Property PartnerName As String Implements IClientPartnerBankRef.PartnerName

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IClientPartnerBankRef.Locked

#End Region

    End Class
End Namespace
