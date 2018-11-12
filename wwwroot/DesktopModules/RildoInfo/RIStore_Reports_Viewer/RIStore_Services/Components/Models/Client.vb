
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_Clients")> _
    <PrimaryKey("ClientId", AutoIncrement:=True)> _
    <Cacheable("Clients", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class Client
        Implements IClient

#Region "Client Variables"

        <IgnoreColumn> _
        Public Property Activities As String Implements IClient.Activities

        Public Property Cell As String Implements IClient.Cell

        Public Property ClientId As Integer Implements IClient.ClientId

        Public Property Cnpj As String Implements IClient.Cnpj

        Public Property Comments As String Implements IClient.Comments

        Public Property CompanyName As String Implements IClient.CompanyName

        Public Property Cpf As String Implements IClient.Cpf

        Public Property CreatedByUser As Integer Implements IClient.CreatedByUser

        Public Property CreatedOnDate As DateTime Implements IClient.CreatedOnDate

        Public Property CreditLimit As Single Implements IClient.CreditLimit

        Public Property DisplayName As String Implements IClient.DisplayName

        Public Property ECFRequired As Boolean Implements IClient.ECFRequired

        Public Property Email As String Implements IClient.Email

        Public Property Fax As String Implements IClient.Fax

        Public Property FirstName As String Implements IClient.FirstName

        Public Property GroupIds As String Implements IClient.GroupIds

        Public Property HasECF As Boolean Implements IClient.HasECF

        Public Property Ident As String Implements IClient.Ident

        Public Property InsEst As String Implements IClient.InsEst

        Public Property InsMun As String Implements IClient.InsMun

        Public Property IsDeleted As Boolean Implements IClient.IsDeleted

        Public Property LastName As String Implements IClient.LastName

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IClient.Locked

        Public Property MonthlyIncome As Single Implements IClient.MonthlyIncome

        Public Property Networked As Boolean Implements IClient.Networked

        Public Property PayMethods As String Implements IClient.PayMethods

        Public Property PayPlans As String Implements IClient.PayPlans

        Public Property PersonType As Boolean Implements IClient.PersonType

        Public Property PortalId As Integer Implements IClient.PortalId

        Public Property PreDiscount As String Implements IClient.PreDiscount

        Public Property Protested As Boolean Implements IClient.Protested

        Public Property RegisterTypes As String Implements IClient.RegisterTypes

        Public Property SalesRep As Integer Implements IClient.SalesRep

        Public Property Scheduled As Boolean Implements IClient.Scheduled

        Public Property Sent As Boolean Implements IClient.Sent

        Public Property StatusId As String Implements IClient.StatusId

        Public Property Telephone As String Implements IClient.Telephone

        Public Property Website As String Implements IClient.Website

        Public Property Zero800s As String Implements IClient.Zero800s

        <IgnoreColumn> _
        Public Property TotalRows As Integer Implements IClient.TotalRows

        Public Property ClientAddressId As Integer Implements IClient.ClientAddressId

        Public Property DateFoun As DateTime? Implements IClient.DateFoun

        Public Property DateRegistered As DateTime? Implements IClient.DateRegistered

        Public Property ModifiedByUser As Integer Implements IClient.ModifiedByUser

        Public Property ModifiedOnDate As DateTime Implements IClient.ModifiedOnDate

        Public Property UserId As Integer Implements IClient.UserId

        <IgnoreColumn> _
        Public Property clientAddresses As List(Of Models.ClientAddress) Implements IClient.clientAddresses

#End Region
    End Class
End Namespace
