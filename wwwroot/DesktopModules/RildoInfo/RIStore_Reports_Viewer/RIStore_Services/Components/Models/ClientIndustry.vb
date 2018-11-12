
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace RI.Modules.RIStore_Services.Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIS_ClientIndustries")> _
    <PrimaryKey("ClientIndustryId", AutoIncrement:=True)> _
    <Cacheable("ClientIndustries", CacheItemPriority.Default, 20)> _
    <Scope("ClientId")>
    Public Class ClientIndustry
        Implements IClientIndustry

        Public Property ClientId As Integer Implements IClientIndustry.ClientId

        Public Property ClientIndustryId As Integer Implements IClientIndustry.ClientIndustryId

        Public Property IndustryId As Integer Implements IClientIndustry.IndustryId

    End Class
End Namespace