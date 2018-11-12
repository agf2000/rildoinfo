
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_ClientDocs")> _
    <PrimaryKey("ClientDocId", AutoIncrement:=True)> _
    <Cacheable("ClientDocs", CacheItemPriority.Default, 20)> _
    <Scope("ClientId")>
    Public Class ClientDoc
        Implements IClientDoc

#Region " Private variables "

        Public Property ClientDocId As Integer Implements IClientDoc.ClientDocId

        Public Property ClientId As Integer Implements IClientDoc.ClientId

        <IgnoreColumn> _
        Public Property ContentType As String Implements IClientDoc.ContentType

        Public Property CreatedByUser As Integer Implements IClientDoc.CreatedByUser

        <IgnoreColumn> _
        Public Property CreatedByUserID As Integer Implements IClientDoc.CreatedByUserID

        Public Property CreatedOnDate As Date Implements IClientDoc.CreatedOnDate

        <IgnoreColumn> _
        Public Property DisplayName As String Implements IClientDoc.DisplayName

        Public Property DocDesc As String Implements IClientDoc.DocDesc

        Public Property DocName As String Implements IClientDoc.DocName

        Public Property DocUrl As String Implements IClientDoc.DocUrl

        <IgnoreColumn> _
        Public Property Extension As String Implements IClientDoc.Extension

        Public Property FileId As Integer Implements IClientDoc.FileId

        <IgnoreColumn> _
        Public Property FileName As String Implements IClientDoc.FileName

        <IgnoreColumn> _
        Public Property Folder As String Implements IClientDoc.Folder

        <IgnoreColumn> _
        Public Property LastModifiedByUserID As Integer Implements IClientDoc.LastModifiedByUserID

        <IgnoreColumn> _
        Public Property LastModifiedOnDate As Date Implements IClientDoc.LastModifiedOnDate

        <IgnoreColumn> _
        Public Property Size As Integer Implements IClientDoc.Size

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IClientDoc.Locked

#End Region

    End Class
End Namespace
