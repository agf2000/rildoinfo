
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_Payments")> _
    <PrimaryKey("PaymentId", AutoIncrement:=True)> _
    <Cacheable("Payments", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class Payment
        Implements IPayment

        Public Property AccountId As Integer Implements IPayment.AccountId

        Public Property AccountName As String Implements IPayment.AccountName

        <IgnoreColumn> _
        Public Property Balance As Single Implements IPayment.Balance

        Public Property PersonId As Integer Implements IPayment.PersonId

        Public Property ClientName As String Implements IPayment.ClientName

        Public Property Comment As String Implements IPayment.Comment

        Public Property CreatedByUser As Integer Implements IPayment.CreatedByUser

        Public Property CreatedOnDate As Date Implements IPayment.CreatedOnDate

        Public Property Credit As Single Implements IPayment.Credit

        Public Property Debit As Single Implements IPayment.Debit

        Public Property DocId As Integer Implements IPayment.DocId

        Public Property Done As Boolean Implements IPayment.Done

        Public Property DueDate As Date Implements IPayment.DueDate

        Public Property InterestRate As Decimal Implements IPayment.InterestRate

        Public Property ModifiedByUser As Integer Implements IPayment.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IPayment.ModifiedOnDate

        Public Property PaymentId As Integer Implements IPayment.PaymentId

        Public Property PortalId As Integer Implements IPayment.PortalId

        Public Property ProviderId As Integer Implements IPayment.ProviderId

        <IgnoreColumn> _
        Public Property TotalRows As Integer Implements IPayment.TotalRows

        Public Property TransDate As Date Implements IPayment.TransDate

        Public Property TransId As Integer Implements IPayment.TransId

        <IgnoreColumn> _
        Public Property VendorName As String Implements IPayment.VendorName
    End Class
End Namespace