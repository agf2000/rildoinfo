Imports RIW.Modules.WebAPI.Components.Models

Namespace Components.Interfaces.Repositories

    Public Interface IPersonIndustriesRepository

        ''' <summary>
        ''' Gets a list of industries by person id
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPersonIndustries(ByVal personId As Integer) As IEnumerable(Of Industry)

        ''' <summary>
        ''' Adds a new person industry
        ''' </summary>
        ''' <param name="personIndustry">Person Industry Model</param>
        ''' <remarks></remarks>
        Function AddPersonIndustry(personIndustry As PersonIndustry) As PersonIndustry

        ''' <summary>
        ''' Removes person industries by person id
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <remarks></remarks>
        Sub RemovePersonIndustries(personId As Integer)

        ''' <summary>
        ''' Removes a person industry
        ''' </summary>
        ''' <param name="personIndustry">Person Industry Model</param>
        ''' <remarks></remarks>
        Sub RemovePersonIndustry(ByVal personIndustry As PersonIndustry)

    End Interface

End Namespace
