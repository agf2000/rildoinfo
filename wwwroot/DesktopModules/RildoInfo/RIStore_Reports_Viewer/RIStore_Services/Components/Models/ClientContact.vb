
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_ClientContacts")> _
    <PrimaryKey("ClientContactId", AutoIncrement:=True)> _
    <Cacheable("ClientContacts", CacheItemPriority.Default, 20)> _
    <Scope("ClientId")>
    Public Class ClientContact
        Implements IClientContact

#Region " Private variables "

        Public Property ClientAddressId As Integer Implements IClientContact.ClientAddressId

        Public Property ClientContactId As Integer Implements IClientContact.ClientContactId

        Public Property ClientId As Integer Implements IClientContact.ClientId

        Public Property Comments As String Implements IClientContact.Comments

        Public Property ContactEmail1 As String Implements IClientContact.ContactEmail1

        Public Property ContactEmail2 As String Implements IClientContact.ContactEmail2

        Public Property ContactName As String Implements IClientContact.ContactName

        Public Property ContactPhone1 As String Implements IClientContact.ContactPhone1

        Public Property ContactPhone2 As String Implements IClientContact.ContactPhone2

        Public Property CreatedByUser As Integer Implements IClientContact.CreatedByUser

        Public Property CreatedOnDate As Date Implements IClientContact.CreatedOnDate

        Public Property DateBirth As Date Implements IClientContact.DateBirth

        Public Property Dept As String Implements IClientContact.Dept

        Public Property IsDeleted As Boolean Implements IClientContact.IsDeleted

        Public Property ModifiedByUser As Integer? Implements IClientContact.ModifiedByUser

        Public Property ModifiedOnDate As Date? Implements IClientContact.ModifiedOnDate

        Public Property PhoneExt1 As String Implements IClientContact.PhoneExt1

        Public Property PhoneExt2 As String Implements IClientContact.PhoneExt2

        <IgnoreColumn> _
        Public Property AddressName As String Implements IClientContact.AddressName

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IClientContact.Locked

#End Region

    End Class
End Namespace
