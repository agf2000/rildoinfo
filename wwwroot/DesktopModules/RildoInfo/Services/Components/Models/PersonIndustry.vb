
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_PeopleIndustries")> _
    <PrimaryKey("PersonIndustryId", AutoIncrement:=True)> _
    <Cacheable("PeopleIndustries", CacheItemPriority.Default, 20)> _
    <Scope("PersonId")>
    Public Class PersonIndustry
        Implements IPersonIndustry

        Public Property PersonId As Integer Implements IPersonIndustry.PersonId

        Public Property PersonIndustryId As Integer Implements IPersonIndustry.PersonIndustryId

        Public Property IndustryId As Integer Implements IPersonIndustry.IndustryId
    End Class
End Namespace