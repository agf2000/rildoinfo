
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_BrandModels")> _
    <PrimaryKey("ModelId", AutoIncrement:=True)> _
    <Cacheable("BrandModels", CacheItemPriority.Default, 20)> _
    <Scope("BrandId")>
    Public Class BrandModel
        Implements IBrandModel

        Public Property BrandId As Integer Implements IBrandModel.BrandId

        Public Property CreatedByUser As Integer Implements IBrandModel.CreatedByUser

        Public Property CreatedOnDate As Date Implements IBrandModel.CreatedOnDate

        Public Property IsDeleted As Boolean Implements IBrandModel.IsDeleted

        <IgnoreColumn> _
        Public Property Locked As Boolean Implements IBrandModel.Locked

        Public Property ModelId As Integer Implements IBrandModel.ModelId

        Public Property ModelTitle As String Implements IBrandModel.ModelTitle

        Public Property ModifiedByUser As Integer? Implements IBrandModel.ModifiedByUser

        Public Property ModifiedOnDate As Date? Implements IBrandModel.ModifiedOnDate
    End Class
End Namespace
