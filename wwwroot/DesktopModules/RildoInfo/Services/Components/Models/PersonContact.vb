
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_PeopleContacts")> _
    <PrimaryKey("PersonContactId", AutoIncrement:=True)> _
    <Cacheable("PeopleContacts", CacheItemPriority.Default, 20)> _
    <Scope("PersonId")>
    Public Class PersonContact
        Implements IPersonContact

        Public Property PersonAddressId As Integer Implements IPersonContact.PersonAddressId

        Public Property PersonContactId As Integer Implements IPersonContact.PersonContactId

        Public Property PersonId As Integer Implements IPersonContact.PersonId

        Public Property Comments As String Implements IPersonContact.Comments

        Public Property ContactEmail1 As String Implements IPersonContact.ContactEmail1

        Public Property ContactEmail2 As String Implements IPersonContact.ContactEmail2

        Public Property ContactName As String Implements IPersonContact.ContactName

        Public Property ContactPhone1 As String Implements IPersonContact.ContactPhone1

        Public Property ContactPhone2 As String Implements IPersonContact.ContactPhone2

        Public Property CreatedByUser As Integer Implements IPersonContact.CreatedByUser

        Public Property CreatedOnDate As Date Implements IPersonContact.CreatedOnDate

        Public Property DateBirth As Date Implements IPersonContact.DateBirth

        Public Property Dept As String Implements IPersonContact.Dept

        Public Property IsDeleted As Boolean Implements IPersonContact.IsDeleted

        Public Property ModifiedByUser As Integer Implements IPersonContact.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IPersonContact.ModifiedOnDate

        Public Property PhoneExt1 As String Implements IPersonContact.PhoneExt1

        Public Property PhoneExt2 As String Implements IPersonContact.PhoneExt2

        <IgnoreColumn> _
        Public Property AddressName As String Implements IPersonContact.AddressName

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IPersonContact.Locked
    End Class
End Namespace
