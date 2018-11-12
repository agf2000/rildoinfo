
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_Estimates")> _
    <PrimaryKey("EstimateId", AutoIncrement:=True)> _
    <Cacheable("Estimates", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class Estimate
        Implements IEstimate

        Public Property Comment As String Implements IEstimate.Comment

        Public Property CreatedByUser As Integer Implements IEstimate.CreatedByUser

        Public Property CreatedOnDate As DateTime Implements IEstimate.CreatedOnDate

        <IgnoreColumn> _
        Public Property CurrentTimestamp As Integer Implements IEstimate.CurrentTimestamp

        Public Property Discount As Decimal Implements IEstimate.Discount

        Public Property EstimateId As Integer Implements IEstimate.EstimateId

        Public Property EstimateTitle As String Implements IEstimate.EstimateTitle

        Public Property Guid As String Implements IEstimate.Guid

        Public Property Inst As String Implements IEstimate.Inst

        Public Property IsDeleted As Boolean Implements IEstimate.IsDeleted

        Public Property Locked As Boolean Implements IEstimate.Locked

        Public Property ModifiedByUser As Integer Implements IEstimate.ModifiedByUser

        Public Property ModifiedOnDate As DateTime Implements IEstimate.ModifiedOnDate

        Public Property PayCondDisc As Decimal Implements IEstimate.PayCondDisc

        Public Property PayCondIn As Single Implements IEstimate.PayCondIn

        Public Property PayCondInst As Single Implements IEstimate.PayCondInst

        Public Property PayCondInterval As Integer Implements IEstimate.PayCondInterval

        Public Property PayCondN As Integer Implements IEstimate.PayCondN

        Public Property PayCondPerc As Decimal Implements IEstimate.PayCondPerc

        Public Property PayCondType As String Implements IEstimate.PayCondType

        Public Property PayForm As Integer Implements IEstimate.PayForm

        Public Property PayOption As Integer Implements IEstimate.PayOption

        Public Property PersonId As Integer Implements IEstimate.PersonId

        Public Property PortalId As Integer Implements IEstimate.PortalId

        Public Property SalesRep As Integer Implements IEstimate.SalesRep

        Public Property StatusId As Integer Implements IEstimate.StatusId

        Public Property TotalAmount As Single Implements IEstimate.TotalAmount

        Public Property TotalPayCond As Single Implements IEstimate.TotalPayCond

        Public Property TotalPayments As Single Implements IEstimate.TotalPayments

        Public Property ViewPrice As Boolean Implements IEstimate.ViewPrice

        <IgnoreColumn> _
        Public Property TotalRows As Integer Implements IEstimate.TotalRows

        <IgnoreColumn> _
        Public Property ClientCell As String Implements IEstimate.ClientCell

        <IgnoreColumn> _
        Public Property ClientDisplayName As String Implements IEstimate.ClientDisplayName

        <IgnoreColumn> _
        Public Property ClientFax As String Implements IEstimate.ClientFax

        <IgnoreColumn> _
        Public Property ClientFirstName As String Implements IEstimate.ClientFirstName

        <IgnoreColumn> _
        Public Property ClientLastName As String Implements IEstimate.ClientLastName

        <IgnoreColumn> _
        Public Property SalesRepName As String Implements IEstimate.SalesRepName

        <IgnoreColumn> _
        Public Property SalesRepEmail As String Implements IEstimate.SalesRepEmail

        <IgnoreColumn> _
        Public Property SalesRepPhone As String Implements IEstimate.SalesRepPhone

        <IgnoreColumn> _
        Public Property ClientTelephone As String Implements IEstimate.ClientTelephone

        <IgnoreColumn> _
        Public Property StatusTitle As String Implements IEstimate.StatusTitle

        <IgnoreColumn> _
        Public Property StatusColor As String Implements IEstimate.StatusColor

        <IgnoreColumn> _
        Public Property ClientAddress As String Implements IEstimate.ClientAddress

        <IgnoreColumn> _
        Public Property ClientCity As String Implements IEstimate.ClientCity

        <IgnoreColumn> _
        Public Property ClientEIN As String Implements IEstimate.ClientEIN

        <IgnoreColumn> _
        Public Property ClientComplement As String Implements IEstimate.ClientComplement

        <IgnoreColumn> _
        Public Property ClientCityTax As String Implements IEstimate.ClientCityTax

        <IgnoreColumn> _
        Public Property ClientDistrict As String Implements IEstimate.ClientDistrict

        <IgnoreColumn> _
        Public Property ClientEmail As String Implements IEstimate.ClientEmail

        <IgnoreColumn> _
        Public Property ClientPostalCode As String Implements IEstimate.ClientPostalCode

        <IgnoreColumn> _
        Public Property ClientRegion As String Implements IEstimate.ClientRegion

        <IgnoreColumn> _
        Public Property ClientSateTax As String Implements IEstimate.ClientStateTax

        <IgnoreColumn> _
        Public Property ClientUnit As String Implements IEstimate.ClientUnit

        <IgnoreColumn> _
        Public Property ClientCompanyName As String Implements IEstimate.ClientCompanyName

        <IgnoreColumn> _
        Public Property ClientZero800s As String Implements IEstimate.ClientZero800s

        <IgnoreColumn> _
        Public Property ConnId As String Implements IEstimate.ConnId
    End Class
End Namespace
