Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_People")> _
    <PrimaryKey("PersonId", AutoIncrement:=True)> _
    <Cacheable("People", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class Person
        Implements IPerson

        <IgnoreColumn> _
        Public Property Activities As String Implements IPerson.Activities

        Public Property Cell As String Implements IPerson.Cell

        Public Property PersonId As Integer Implements IPerson.PersonId

        Public Property EIN As String Implements IPerson.EIN

        Public Property Comments As String Implements IPerson.Comments

        Public Property CompanyName As String Implements IPerson.CompanyName

        Public Property CPF As String Implements IPerson.CPF

        Public Property CreatedByUser As Integer Implements IPerson.CreatedByUser

        Public Property CreatedOnDate As DateTime Implements IPerson.CreatedOnDate

        Public Property CreditLimit As Single Implements IPerson.CreditLimit

        Public Property DisplayName As String Implements IPerson.DisplayName

        Public Property ECFRequired As Boolean Implements IPerson.ECFRequired

        Public Property Email As String Implements IPerson.Email

        Public Property Fax As String Implements IPerson.Fax

        Public Property FirstName As String Implements IPerson.FirstName

        Public Property GroupIds As String Implements IPerson.GroupIds

        Public Property HasECF As Boolean Implements IPerson.HasECF

        Public Property Ident As String Implements IPerson.Ident

        Public Property StateTax As String Implements IPerson.StateTax

        Public Property CityTax As String Implements IPerson.CityTax

        Public Property IsDeleted As Boolean Implements IPerson.IsDeleted

        Public Property LastName As String Implements IPerson.LastName

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IPerson.Locked

        Public Property MonthlyIncome As Single Implements IPerson.MonthlyIncome

        Public Property ReasonBlocked As String Implements IPerson.ReasonBlocked

        Public Property PayMethods As String Implements IPerson.PayMethods

        Public Property PayPlans As String Implements IPerson.PayPlans

        Public Property PersonType As Boolean Implements IPerson.PersonType

        Public Property PortalId As Integer Implements IPerson.PortalId

        Public Property PreDiscount As String Implements IPerson.PreDiscount

        Public Property Blocked As Boolean Implements IPerson.Blocked

        Public Property RegisterTypes As String Implements IPerson.RegisterTypes

        Public Property SalesRep As Integer Implements IPerson.SalesRep

        Public Property Scheduled As Boolean Implements IPerson.Scheduled

        Public Property Sent As Boolean Implements IPerson.Sent

        Public Property StatusId As Integer Implements IPerson.StatusId

        Public Property Telephone As String Implements IPerson.Telephone

        Public Property Website As String Implements IPerson.Website

        Public Property Zero800s As String Implements IPerson.Zero800s

        <IgnoreColumn> _
        Public Property TotalRows As Integer Implements IPerson.TotalRows

        Public Property PersonAddressId As Integer Implements IPerson.PersonAddressId

        Public Property DateFounded As DateTime Implements IPerson.DateFounded

        Public Property DateRegistered As DateTime Implements IPerson.DateRegistered

        Public Property ModifiedByUser As Integer Implements IPerson.ModifiedByUser

        Public Property ModifiedOnDate As DateTime Implements IPerson.ModifiedOnDate

        Public Property UserId As Integer Implements IPerson.UserId

        <IgnoreColumn> _
        Public Property UserName As String Implements IPerson.UserName

        <IgnoreColumn> _
        Public Property PersonAddresses As List(Of Models.PersonAddress) Implements IPerson.PersonAddresses

        <IgnoreColumn> _
        Public Property HistoryText As String Implements IPerson.HistoryText

        <IgnoreColumn> _
        Public Property Country As String Implements IPerson.Country

        <IgnoreColumn> _
        Public Property Biography As String Implements IPerson.Biography

        <IgnoreColumn> _
        Public Property City As String Implements IPerson.City

        <IgnoreColumn> _
        Public Property Complement As String Implements IPerson.Complement

        <IgnoreColumn> _
        Public Property District As String Implements IPerson.District

        <IgnoreColumn> _
        Public Property PostalCode As String Implements IPerson.PostalCode

        <IgnoreColumn> _
        Public Property Region As String Implements IPerson.Region

        <IgnoreColumn> _
        Public Property Street As String Implements IPerson.Street

        <IgnoreColumn> _
        Public Property Unit As String Implements IPerson.Unit

        <IgnoreColumn> _
        Public Property CreateLogin As Boolean Implements IPerson.CreateLogin

        <IgnoreColumn> _
        Public Property Industries As String Implements IPerson.Industries

        <IgnoreColumn> _
        Public Property StatusTitle As String Implements IPerson.StatusTitle

        <IgnoreColumn> _
        Public Property MessageBody As String Implements IPerson.MessageBody

        <IgnoreColumn> _
        Public Property Subject As String Implements IPerson.Subject

        <IgnoreColumn> _
        Public Property HasLogin As Boolean Implements IPerson.HasLogin

        <IgnoreColumn> _
        Public Property SalesRepName As String Implements IPerson.SalesRepName

        <IgnoreColumn> _
        Public Property ConnId As String Implements IPerson.ConnId

        <IgnoreColumn> _
        Public Property SalesRepEmail As String Implements IPerson.SalesRepEmail

        <IgnoreColumn> _
        Public Property SalesRepPhone As String Implements IPerson.SalesRepPhone

        <IgnoreColumn> _
        Public Property Password As String Implements IPerson.Password

        Public Property OldId As Integer Implements IPerson.OldId

        <IgnoreColumn> _
        Public Property SyncEnabled As Boolean Implements IPerson.SyncEnabled
    End Class
End Namespace
