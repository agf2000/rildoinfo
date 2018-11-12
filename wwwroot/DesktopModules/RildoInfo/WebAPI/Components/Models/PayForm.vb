Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_PayForms")> _
    <PrimaryKey("PayFormId", AutoIncrement:=True)> _
    <Cacheable("PayForms", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class PayForm
        Implements IPayForm

        Public Property CreatedByUser As Integer Implements IPayForm.CreatedByUser

        Public Property CreatedOnDate As Date Implements IPayForm.CreatedOnDate

        Public Property ModifiedByUser As Integer Implements IPayForm.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IPayForm.ModifiedOnDate

        Public Property PayFormId As Integer Implements IPayForm.PayFormId

        Public Property PayFormTitle As String Implements IPayForm.PayFormTitle

        Public Property PortalId As Integer Implements IPayForm.PortalId
    End Class
End Namespace