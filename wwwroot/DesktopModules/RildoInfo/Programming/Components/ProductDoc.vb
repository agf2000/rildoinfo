
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_ProductDoc")> _
    <PrimaryKey("DocId", AutoIncrement:=True)> _
    <Cacheable("ProductDoc", CacheItemPriority.Default, 20)> _
    <Scope("ProductId")>
    Public Class ProductDoc
        Implements IProductDoc

        Public Property DocId As Integer Implements IProductDoc.DocId

        Public Property DocPath As String Implements IProductDoc.DocPath

        Public Property FileExt As String Implements IProductDoc.FileExt

        Public Property FileName As String Implements IProductDoc.FileName

        Public Property Hidden As Boolean Implements IProductDoc.Hidden

        Public Property ListOrder As Integer Implements IProductDoc.ListOrder

        Public Property ProductId As Integer Implements IProductDoc.ProductId
    End Class
End Namespace