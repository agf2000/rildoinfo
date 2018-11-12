Imports DotNetNuke.ComponentModel.DataAnnotations
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_HtmlContents")> _
    <PrimaryKey("ContentId", AutoIncrement:=True)> _
    <Cacheable("HtmlContents", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class HtmlContent
        Implements IHtmlContent

        Public Property HtmlContent As String Implements IHtmlContent.HtmlContent

        Public Property ContentId As Integer Implements IHtmlContent.ContentId

        Public Property ContentTitle As String Implements IHtmlContent.ContentTitle

        Public Property CreatedByUser As Integer Implements IHtmlContent.CreatedByUser

        Public Property CreatedOnDate As Date Implements IHtmlContent.CreatedOnDate

        Public Property ModifiedByUser As Integer Implements IHtmlContent.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IHtmlContent.ModifiedOnDate

        Public Property PortalId As Integer Implements IHtmlContent.PortalId
    End Class
End Namespace