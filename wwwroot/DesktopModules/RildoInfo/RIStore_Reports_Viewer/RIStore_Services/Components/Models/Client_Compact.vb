
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_Clients")> _
    <PrimaryKey("ClientId", AutoIncrement:=True)> _
    <Cacheable("Clients", CacheItemPriority.Default, 20)> _
    <Scope("PortaId")>
    Public Class Client_Compact

#Region " Client "

        Public Property PortalId() As Integer

        Public Property ClientId() As Integer

        Public Property UserId() As Integer

        Public Property PersonType() As Boolean

        Public Property CompanyName() As String

        Public Property DisplayName() As String

        Public Property DateFoun() As Date

        Public Property DateRegistered() As Date

        Public Property FirstName() As String

        Public Property LastName() As String

        Public Property Telephone() As String

        Public Property Cell() As String

        Public Property Fax() As String

        Public Property Zero800s() As String

        Public Property Email() As String

        Public Property CNPJ() As String

        Public Property CPF() As String

        Public Property Ident() As String

        Public Property InsEst() As String

        Public Property InsMun() As String

        Public Property Website() As String

        Public Property CreditLimit() As Single

        Public Property PreDiscount() As String

        Public Property Comments() As String

        Public Property IsDeleted() As Boolean

        Public Property HistoryText() As String

        Public Property Sent() As Boolean

        Public Property StatusId() As Integer

        Public Property IndustryId() As Integer

        Public Property StatusTitle() As String

        Public Property MonthlyIncome() As Single

        Public Property SalesRep() As Integer

        Public Property SalesRepName() As String

        Public Property SalesRepEmail() As String

        Public Property SalesRepPhone() As String

        Public Property GroupIDs As String

        Public Property Street() As String

        Public Property Unit() As String

        Public Property Complement() As String

        Public Property District() As String

        Public Property City() As String

        Public Property Region() As String

        Public Property PostalCode() As String

        Public Property Country() As String

        Public Property Locked() As Boolean

        Public Property CreatedByUser() As Integer

        Public Property CreatedDate() As Date

        Public Property ModifiedByUser() As Integer

        Public Property ModifiedDate() As Date

        Public Property TotalRows() As Integer

        Public Property Activities() As String

#End Region

    End Class
End Namespace
