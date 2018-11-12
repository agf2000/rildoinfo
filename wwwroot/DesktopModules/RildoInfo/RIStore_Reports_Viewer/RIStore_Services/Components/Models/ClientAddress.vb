
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_ClientAddresses")> _
    <PrimaryKey("ClientAddressId", AutoIncrement:=True)> _
    <Cacheable("ClientAddresses", CacheItemPriority.Default, 20)> _
    <Scope("ClientId")>
    Public Class ClientAddress
        Implements IClientAddress

#Region " Private variables "

        Public Property AddressName As String Implements IClientAddress.AddressName

        Public Property Cell As String Implements IClientAddress.Cell

        Public Property City As String Implements IClientAddress.City

        Public Property ClientAddressId As Integer Implements IClientAddress.ClientAddressId

        Public Property ClientId As Integer Implements IClientAddress.ClientId

        Public Property Complement As String Implements IClientAddress.Complement

        Public Property Country As String Implements IClientAddress.Country

        Public Property CreatedByUser As Integer Implements IClientAddress.CreatedByUser

        Public Property CreatedOnDate As Date Implements IClientAddress.CreatedOnDate

        Public Property District As String Implements IClientAddress.District

        Public Property Fax As String Implements IClientAddress.Fax

        Public Property IsDeleted As Boolean Implements IClientAddress.IsDeleted

        Public Property ModifiedByUser As Integer? Implements IClientAddress.ModifiedByUser

        Public Property ModifiedOnDate As Date? Implements IClientAddress.ModifiedOnDate

        Public Property PostalCode As String Implements IClientAddress.PostalCode

        Public Property Region As String Implements IClientAddress.Region

        Public Property Street As String Implements IClientAddress.Street

        Public Property Telephone As String Implements IClientAddress.Telephone

        Public Property Unit As String Implements IClientAddress.Unit

        Public Property ViewOrder As Integer Implements IClientAddress.ViewOrder

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IClientAddress.Locked

#End Region

    End Class
End Namespace
