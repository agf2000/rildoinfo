
Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_Accounts")> _
    <PrimaryKey("AccountId", AutoIncrement:=True)> _
    <Cacheable("Accounts", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class Account
        Implements IAccount

        Public Property AccountId As Integer Implements IAccount.AccountId

        Public Property AccountName As String Implements IAccount.AccountName

        Public Property CreatedByUser As Integer Implements IAccount.CreatedByUser

        Public Property CreatedOnDate As Date Implements IAccount.CreatedOnDate

        Public Property PortalId As Integer Implements IAccount.PortalId

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IAccount.Locked

        <IgnoreColumn> _
        Public Property Balance As Single Implements IAccount.Balance

        <IgnoreColumn> _
        Public Property Credit As Single Implements IAccount.Credit

        <IgnoreColumn> _
        Public Property Debit As Single Implements IAccount.Debit

        Public Property ModifiedByUser As Integer Implements IAccount.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IAccount.ModifiedOnDate

        <IgnoreColumn> _
        Public Property Credit4Seen As Single Implements IAccount.Credit4Seen

        <IgnoreColumn> _
        Public Property CreditActual As Single Implements IAccount.CreditActual

        <IgnoreColumn> _
        Public Property Debit4Seen As Single Implements IAccount.Debit4Seen

        <IgnoreColumn> _
        Public Property DebitActual As Single Implements IAccount.DebitActual

        <IgnoreColumn> _
        Public Property TotalBalance As Single Implements IAccount.TotalBalance

        <IgnoreColumn> _
        Public Property TotalProductSales As Single Implements IAccount.TotalProductSales

        <IgnoreColumn> _
        Public Property TotalServiceSales As Single Implements IAccount.TotalServiceSales

        <IgnoreColumn> _
        Public Property TotalProductsEstimates As Single Implements IAccount.TotalProductsEstimates

        <IgnoreColumn> _
        Public Property TotalServicesEstimates As Single Implements IAccount.TotalServicesEstimates

        <IgnoreColumn> _
        Public Property Sales4Seen As Single Implements IAccount.Sales4Seen

        <IgnoreColumn> _
        Public Property SalesActual As Single Implements IAccount.SalesActual

        <IgnoreColumn> _
        Public Property CreditBalance As Single Implements IAccount.CreditBalance

        <IgnoreColumn> _
        Public Property DebitBalance As Single Implements IAccount.DebitBalance
    End Class
End Namespace
