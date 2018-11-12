
Public Interface IPersonIndustriesRepository

    ''' <summary>
    ''' Gets a list of industries by person id
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getPersonIndustries(ByVal personId As Integer) As IEnumerable(Of Models.Industry)

    ''' <summary>
    ''' Adds a new person industry
    ''' </summary>
    ''' <param name="personIndustry">Person Industry Model</param>
    ''' <remarks></remarks>
    Function addPersonIndustry(personIndustry As Models.PersonIndustry) As Models.PersonIndustry

    ''' <summary>
    ''' Removes person industries by person id
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <remarks></remarks>
    Sub removePersonIndustries(personId As Integer)

    ''' <summary>
    ''' Removes a person industry
    ''' </summary>
    ''' <param name="personIndustry">Person Industry Model</param>
    ''' <remarks></remarks>
    Sub removePersonIndustry(ByVal personIndustry As Models.PersonIndustry)

End Interface
