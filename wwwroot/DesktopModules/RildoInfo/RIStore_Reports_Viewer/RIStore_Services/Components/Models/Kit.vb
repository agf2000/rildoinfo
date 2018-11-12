
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_KitItems")> _
    <PrimaryKey("KitItemId", AutoIncrement:=True)> _
    <Cacheable("KitItems", CacheItemPriority.Default, 20)> _
    <Scope("KitId")>
    Public Class Kit
        Implements IKit

        <IgnoreColumn> _
        Public Property PortalId() As Integer Implements IKit.PortalId

        <IgnoreColumn> _
        Public Property CategoryId() As Integer Implements IKit.CategoryId

        <IgnoreColumn> _
        Public Property ParentId() As Integer Implements IKit.ParentId

        <IgnoreColumn> _
        Public Property Name() As String Implements IKit.Name

        <IgnoreColumn> _
        Public Property ProdName() As String Implements IKit.ProdName

        <IgnoreColumn> _
        Public Property DisplayName() As String Implements IKit.DisplayName

        <IgnoreColumn> _
        Public Property ProdRef() As String Implements IKit.ProdRef

        <IgnoreColumn> _
        Public Property ProdBarCode() As String Implements IKit.ProdBarCode

        <IgnoreColumn> _
        Public Property UnitValue() As Single Implements IKit.UnitValue

        Public Property CreatedByUser As Integer Implements IKit.CreatedByUser

        Public Property CreatedOnDate As Date Implements IKit.CreatedOnDate

        Public Property ItemId As Integer Implements IKit.ItemId

        Public Property KitId As Integer Implements IKit.KitId

        Public Property KitItemId As Integer Implements IKit.KitItemId

        Public Property ModifiedByUser As Integer Implements IKit.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IKit.ModifiedOnDate

        Public Property ProdId As Integer Implements IKit.ProdId

        Public Property Qty As Double Implements IKit.Qty

    End Class
End Namespace