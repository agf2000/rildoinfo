
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_Accounts")> _
    <PrimaryKey("AccountId", AutoIncrement:=True)> _
    <Cacheable("Accounts", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class Account
        Implements IAccount

#Region " Account variables "

        Public Property AccountId As Integer Implements IAccount.AccountId

        Public Property AccountName As String Implements IAccount.AccountName

        Public Property CreatedByUser As Integer Implements IAccount.CreatedByUser

        Public Property CreatedOnDate As Date Implements IAccount.CreatedOnDate

        Public Property ModifiedByUser As Integer? Implements IAccount.ModifiedByUser

        Public Property ModifiedOnDate As Date? Implements IAccount.ModifiedOnDate

        Public Property PortalId As Integer Implements IAccount.PortalId

        Public Property Locked As Boolean Implements IAccount.Locked

#End Region

    End Class
End Namespace
