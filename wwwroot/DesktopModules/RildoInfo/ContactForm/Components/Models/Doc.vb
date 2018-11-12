
Imports System.Web.Caching
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_Catalogs")> _
    <PrimaryKey("DocId", AutoIncrement:=True)> _
    <Cacheable("Catalogs", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")> _
    Public Class Doc
        Implements IDoc

        Public Property CreatedByUser As Integer Implements IDoc.CreatedByUser

        Public Property CreatedOnDate As Date Implements IDoc.CreatedOnDate

        Public Property DocDesc As String Implements IDoc.DocDesc

        Public Property DocName As String Implements IDoc.DocName

        Public Property Downloads As Integer Implements IDoc.Downloads

        Public Property DocId As Integer Implements IDoc.DocId

        Public Property FileId As Integer Implements IDoc.FileId

        Public Property ModuleId As Integer Implements IDoc.ModuleId

        Public Property PortalId As Integer Implements IDoc.PortalId

        Public Property Requests As Integer Implements IDoc.Requests

        <IgnoreColumn> _
        Public Property ContentType As String Implements IDoc.ContentType

        <IgnoreColumn> _
        Public Property Extension As String Implements IDoc.Extension

        <IgnoreColumn> _
        Public Property FileName As String Implements IDoc.FileName
    End Class
End Namespace