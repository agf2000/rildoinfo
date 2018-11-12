
Public Interface IIndustriesRepository

    ''' <summary>
    ''' Gets a list of industries by portal id
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetIndustries(ByVal portalId As Integer) As IEnumerable(Of Models.Industry)

    ''' <summary>
    ''' Gets a creditor account by id
    ''' </summary>
    ''' <param name="accountId">Account ID</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetIndustry(ByVal accountId As Integer, ByVal portalId As Integer) As Models.Industry

    ''' <summary>
    ''' Updates an industry info
    ''' </summary>
    ''' <param name="industry">Industry Model</param>
    ''' <remarks></remarks>
    Sub UpdateIndustry(industry As Models.Industry)

    ''' <summary>
    ''' Adds a new industry
    ''' </summary>
    ''' <param name="industry">Industry Model</param>
    ''' <remarks></remarks>
    Function AddIndustry(industry As Models.Industry) As Models.Industry

    ''' <summary>
    ''' Removes an industry by id
    ''' </summary>
    ''' <param name="industryId">Industry ID</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <remarks></remarks>
    Sub RemoveIndustry(industryId As Integer, portalId As Integer)

    ''' <summary>
    ''' Removes an industry
    ''' </summary>
    ''' <param name="industry">Industry Model</param>
    ''' <remarks></remarks>
    Sub RemoveIndustry(ByVal industry As Models.Industry)

End Interface
