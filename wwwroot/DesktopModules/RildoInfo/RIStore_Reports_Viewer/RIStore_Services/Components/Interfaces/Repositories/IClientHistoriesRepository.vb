
Namespace RI.Modules.RIStore_Services
    Public Interface IClientHistoriesRepository

        ''' <summary>
        ''' Gets a list of client Histories by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientHistories(ByVal clientId As Integer) As IEnumerable(Of Models.ClientHistory)

        ''' <summary>
        ''' Gets a client doc by id
        ''' </summary>
        ''' <param name="clientHistoryId">ClientHistory ID</param>
        ''' <param name="clientId">Clientl ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientHistory(ByVal clientHistoryId As Integer, ByVal clientId As Integer) As Models.ClientHistory

        ''' <summary>
        ''' Adds a client doc
        ''' </summary>
        ''' <param name="clientHistory">Client History Model</param>
        ''' <remarks></remarks>
        Function AddClientHistory(clientHistory As Models.ClientHistory) As Models.ClientHistory

        ''' <summary>
        ''' Updates a client doc
        ''' </summary>
        ''' <param name="clientHistory">Client History Model</param>
        ''' <remarks></remarks>
        Sub UpdateClientHistory(clientHistory As Models.ClientHistory)

        ''' <summary>
        ''' Removes client doc by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="clientHistoryId">Client History ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientHistory(clientHistoryId As Integer, clientId As Integer)

        ''' <summary>
        ''' Remove all client doc
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientHistories(clientId As Integer)

        ''' <summary>
        ''' Removes a client doc
        ''' </summary>
        ''' <param name="clientHistory">Client History Model</param>
        ''' <remarks></remarks>
        Sub RemoveClientHistory(ByVal clientHistory As Models.ClientHistory)

    End Interface
End Namespace