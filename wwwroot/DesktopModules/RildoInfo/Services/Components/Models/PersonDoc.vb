
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_PeopleDocs")> _
    <PrimaryKey("PersonDocId", AutoIncrement:=True)> _
    <Cacheable("PeopleDocs", CacheItemPriority.Default, 20)> _
    <Scope("PersonId")>
    Public Class PersonDoc
        Implements IPersonDoc

        Public Property PersonDocId As Integer Implements IPersonDoc.PersonDocId

        Public Property PersonId As Integer Implements IPersonDoc.PersonId

        <IgnoreColumn> _
        Public Property ContentType As String Implements IPersonDoc.ContentType

        Public Property CreatedByUser As Integer Implements IPersonDoc.CreatedByUser

        <IgnoreColumn> _
        Public Property CreatedByUserID As Integer Implements IPersonDoc.CreatedByUserID

        Public Property CreatedOnDate As Date Implements IPersonDoc.CreatedOnDate

        <IgnoreColumn> _
        Public Property DisplayName As String Implements IPersonDoc.DisplayName

        Public Property DocDesc As String Implements IPersonDoc.DocDesc

        Public Property DocName As String Implements IPersonDoc.DocName

        Public Property DocUrl As String Implements IPersonDoc.DocUrl

        <IgnoreColumn> _
        Public Property Extension As String Implements IPersonDoc.Extension

        Public Property FileId As Integer Implements IPersonDoc.FileId

        <IgnoreColumn> _
        Public Property FileName As String Implements IPersonDoc.FileName

        <IgnoreColumn> _
        Public Property Folder As String Implements IPersonDoc.Folder

        <IgnoreColumn> _
        Public Property LastModifiedByUserID As Integer Implements IPersonDoc.LastModifiedByUserID

        <IgnoreColumn> _
        Public Property LastModifiedOnDate As Date Implements IPersonDoc.LastModifiedOnDate

        <IgnoreColumn> _
        Public Property Size As Integer Implements IPersonDoc.Size

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IPersonDoc.Locked

        <IgnoreColumn> _
        Public Property FolderPath As String Implements IPersonDoc.FolderPath

        <IgnoreColumn> _
        Public Property MaxHeight As Integer Implements IPersonDoc.MaxHeight

        <IgnoreColumn> _
        Public Property MaxWidth As Integer Implements IPersonDoc.MaxWidth

        <IgnoreColumn> _
        Public Property PortalId As Integer Implements IPersonDoc.PortalId
    End Class
End Namespace
