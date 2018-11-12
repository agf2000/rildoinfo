
Public Interface IClientIncomeSourcesRepository

    ''' <summary>
    ''' Gets a list of client income sources by client id
    ''' </summary>
    ''' <param name="personId">Client ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetClientIncomeSources(ByVal personId As Integer) As IEnumerable(Of Models.ClientIncomeSource)

    ''' <summary>
    ''' Gets a client income source by id
    ''' </summary>
    ''' <param name="clientIncomeSourceId">Client Income Source ID</param>
    ''' <param name="personId">Clientl ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetClientIncomeSource(ByVal clientIncomeSourceId As Integer, ByVal personId As Integer) As Models.ClientIncomeSource

    ''' <summary>
    ''' Adds a client income source
    ''' </summary>
    ''' <param name="clientIncomeSource">Client Income Source Model</param>
    ''' <remarks></remarks>
    Function AddClientIncomeSource(clientIncomeSource As Models.ClientIncomeSource) As Models.ClientIncomeSource

    ''' <summary>
    ''' Updates a client income source
    ''' </summary>
    ''' <param name="clientIncomeSource">Client Income Source Model</param>
    ''' <remarks></remarks>
    Sub UpdateClientIncomeSource(clientIncomeSource As Models.ClientIncomeSource)

    ''' <summary>
    ''' Removes client income source by client id
    ''' </summary>
    ''' <param name="personId">Client ID</param>
    ''' <param name="clientIncomeSourceId">Client Income Source ID</param>
    ''' <remarks></remarks>
    Sub RemoveClientIncomeSource(clientIncomeSourceId As Integer, personId As Integer)

    ''' <summary>
    ''' Remove all client income source
    ''' </summary>
    ''' <param name="personId">Client ID</param>
    ''' <remarks></remarks>
    Sub RemoveClientIncomeSources(personId As Integer)

    ''' <summary>
    ''' Removes a client income source
    ''' </summary>
    ''' <param name="clientIncomeSource">Client Income Source Model</param>
    ''' <remarks></remarks>
    Sub RemoveClientIncomeSource(ByVal clientIncomeSource As Models.ClientIncomeSource)

End Interface
