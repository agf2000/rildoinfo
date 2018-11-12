Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_Payments")> _
    <PrimaryKey("PaymentId", AutoIncrement:=True)> _
    <Cacheable("Payments", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class Payment
        Implements IPayment

        <IgnoreColumn> _
        Public Property ProviderName As String Implements IPayment.ProviderName

        <IgnoreColumn> _
        Public Property ClientName As String Implements IPayment.ClientName

        <IgnoreColumn> _
        Public Property TotalRows As Integer Implements IPayment.TotalRows

        <IgnoreColumn> _
        Public Property Balance As Single Implements IPayment.Balance

        Public Property AccountId As Integer Implements IPayment.AccountId

        Public Property ClientId As Integer Implements IPayment.ClientId

        Public Property Comment As String Implements IPayment.Comment

        Public Property CreatedByUser As Integer Implements IPayment.CreatedByUser

        Public Property CreatedOnDate As Date Implements IPayment.CreatedOnDate

        Public Property Credit As Single Implements IPayment.Credit

        Public Property Debit As Single Implements IPayment.Debit

        Public Property DocId As Integer Implements IPayment.DocId

        Public Property Done As Boolean? Implements IPayment.Done

        Public Property DueDate As Date Implements IPayment.DueDate

        Public Property InterestRate As Double Implements IPayment.InterestRate

        Public Property ModifiedByUser As Integer? Implements IPayment.ModifiedByUser

        Public Property ModifiedOnDate As Date? Implements IPayment.ModifiedOnDate

        Public Property PaymentId As Integer Implements IPayment.PaymentId

        Public Property PortalId As Integer Implements IPayment.PortalId

        Public Property ProviderId As Integer Implements IPayment.ProviderId

        Public Property TransDate As Date Implements IPayment.TransDate

        Public Property TransId As Integer Implements IPayment.TransId

        <IgnoreColumn> _
        Public Property Agenda As Boolean Implements IPayment.Agenda

        <IgnoreColumn> _
        Public Property Total As Single Implements IPayment.Total

        Public Property Fee As Single Implements IPayment.Fee

        <IgnoreColumn> _
        Public Property OriginalDueDate As Date Implements IPayment.OriginalDueDate

        Public Property OriginId As Integer Implements IPayment.OriginId

        Public Property IsDeleted As Boolean? Implements IPayment.IsDeleted

        Public Property AmountPaid As Single Implements IPayment.AmountPaid
    End Class
End Namespace