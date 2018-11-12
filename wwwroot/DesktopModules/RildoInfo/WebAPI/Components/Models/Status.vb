Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_Statuses")> _
    <PrimaryKey("StatusId", AutoIncrement:=True)> _
    <Cacheable("Statuses", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class Status
        Implements IStatus

        Public Property CreatedByUser As Integer Implements IStatus.CreatedByUser

        Public Property CreatedOnDate As Date Implements IStatus.CreatedOnDate

        Public Property IsDeleted As Boolean Implements IStatus.IsDeleted

        Public Property IsReadOnly As Boolean Implements IStatus.IsReadOnly

        Public Property ModifiedByUser As Integer Implements IStatus.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IStatus.ModifiedOnDate

        Public Property PortalId As Integer Implements IStatus.PortalId

        Public Property StatusColor As String Implements IStatus.StatusColor

        Public Property StatusId As Integer Implements IStatus.StatusId

        Public Property StatusTitle As String Implements IStatus.StatusTitle
    End Class
End Namespace