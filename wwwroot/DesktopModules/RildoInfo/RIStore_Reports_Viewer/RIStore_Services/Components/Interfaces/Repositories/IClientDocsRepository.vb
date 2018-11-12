
Namespace RI.Modules.RIStore_Services
    Public Interface IClientDocsRepository

        ''' <summary>
        ''' Gets a list of client docs by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientDocs(ByVal clientId As Integer) As IEnumerable(Of Models.ClientDoc)

        ''' <summary>
        ''' Gets a client doc by id
        ''' </summary>
        ''' <param name="clientDocId">Client Doc ID</param>
        ''' <param name="clientId">Clientl ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientDoc(ByVal clientDocId As Integer, ByVal clientId As Integer) As Models.ClientDoc

        ''' <summary>
        ''' Adds a client doc
        ''' </summary>
        ''' <param name="clientDoc">Client Doc Model</param>
        ''' <remarks></remarks>
        Function AddClientDoc(clientDoc As Models.ClientDoc) As Models.ClientDoc

        ''' <summary>
        ''' Updates a client doc
        ''' </summary>
        ''' <param name="clientDoc">Client Doc Model</param>
        ''' <remarks></remarks>
        Sub UpdateClientDoc(clientDoc As Models.ClientDoc)

        ''' <summary>
        ''' Removes client doc by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="clientDocId">Client Doc ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientDoc(clientDocId As Integer, clientId As Integer)

        ''' <summary>
        ''' Remove all client doc
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientDocs(clientId As Integer)

        ''' <summary>
        ''' Removes a client doc
        ''' </summary>
        ''' <param name="clientDoc">Client Doc Model</param>
        ''' <remarks></remarks>
        Sub RemoveClientDoc(ByVal clientDoc As Models.ClientDoc)

    End Interface
End Namespace