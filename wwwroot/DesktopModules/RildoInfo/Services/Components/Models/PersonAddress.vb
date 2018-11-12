
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_PeopleAddresses")> _
    <PrimaryKey("PersonAddressId", AutoIncrement:=True)> _
    <Cacheable("PeopleAddresses", CacheItemPriority.Default, 20)> _
    <Scope("PersonId")>
    Public Class PersonAddress
        Implements IPersonAddress

        Public Property AddressName As String Implements IPersonAddress.AddressName

        Public Property Cell As String Implements IPersonAddress.Cell

        Public Property City As String Implements IPersonAddress.City

        Public Property PersonAddressId As Integer Implements IPersonAddress.PersonAddressId

        Public Property PersonId As Integer Implements IPersonAddress.PersonId

        Public Property Complement As String Implements IPersonAddress.Complement

        Public Property Country As String Implements IPersonAddress.Country

        Public Property CreatedByUser As Integer Implements IPersonAddress.CreatedByUser

        Public Property CreatedOnDate As Date Implements IPersonAddress.CreatedOnDate

        Public Property District As String Implements IPersonAddress.District

        Public Property Fax As String Implements IPersonAddress.Fax

        Public Property IsDeleted As Boolean Implements IPersonAddress.IsDeleted

        Public Property ModifiedByUser As Integer Implements IPersonAddress.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IPersonAddress.ModifiedOnDate

        Public Property PostalCode As String Implements IPersonAddress.PostalCode

        Public Property Region As String Implements IPersonAddress.Region

        Public Property Street As String Implements IPersonAddress.Street

        Public Property Telephone As String Implements IPersonAddress.Telephone

        Public Property Unit As String Implements IPersonAddress.Unit

        Public Property ViewOrder As Integer Implements IPersonAddress.ViewOrder

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IPersonAddress.Locked

        Public Property Comment As String Implements IPersonAddress.Comment
    End Class
End Namespace
