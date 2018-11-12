
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_CategoryPermission")> _
    <PrimaryKey("CategoryPermissionId", AutoIncrement:=True)> _
    <Cacheable("CategoryPermission", CacheItemPriority.Default, 20)> _
    <Scope("CategoryId")>
    Public Class CategoryPermission
        Implements ICategoryPermission

#Region "Category Permission variables "

        Public Property AllowAccess As Boolean Implements ICategoryPermission.AllowAccess

        Public Property CategoryId As Integer Implements ICategoryPermission.CategoryId

        Public Property CategoryPermissionId As Integer Implements ICategoryPermission.CategoryPermissionId

        Public Property PermissionId As Integer Implements ICategoryPermission.PermissionId

        Public Property RoleId As Integer Implements ICategoryPermission.RoleId

#End Region

    End Class
End Namespace
