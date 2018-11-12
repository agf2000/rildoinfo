
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_Industries")> _
    <PrimaryKey("IndustryId", AutoIncrement:=True)> _
    <Cacheable("Industries", CacheItemPriority.Default, 20)> _
    <Scope("PortalId")>
    Public Class Industry
        Implements IIndustry

        Public Property CreatedByUser As Integer Implements IIndustry.CreatedByUser

        Public Property CreatedOnDate As Date Implements IIndustry.CreatedOnDate

        Public Property IndustryId As Integer Implements IIndustry.IndustryId

        Public Property IndustryTitle As String Implements IIndustry.IndustryTitle

        Public Property IsDeleted As Boolean Implements IIndustry.IsDeleted

        Public Property ModifiedByUser As Integer Implements IIndustry.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IIndustry.ModifiedOnDate

        Public Property PortalId As Integer Implements IIndustry.PortalId
    End Class
End Namespace