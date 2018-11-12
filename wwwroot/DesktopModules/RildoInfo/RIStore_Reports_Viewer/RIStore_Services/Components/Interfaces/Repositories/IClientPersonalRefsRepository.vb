
Namespace RI.Modules.RIStore_Services
    Public Interface IClientPersonalRefsRepository

        ''' <summary>
        ''' Gets a list of client personal refs by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientPersonalRefs(ByVal clientId As Integer) As IEnumerable(Of Models.ClientPersonalRef)

        ''' <summary>
        ''' Gets a client personal ref by id
        ''' </summary>
        ''' <param name="clientPersonalRefId">Client Personal Ref ID</param>
        ''' <param name="clientId">Clientl ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientPersonalRef(ByVal clientPersonalRefId As Integer, ByVal clientId As Integer) As Models.ClientPersonalRef

        ''' <summary>
        ''' Adds a client personal ref
        ''' </summary>
        ''' <param name="clientPersonalRef">Client Personal Ref Model</param>
        ''' <remarks></remarks>
        Function AddClientPersonalRef(clientPersonalRef As Models.ClientPersonalRef) As Models.ClientPersonalRef

        ''' <summary>
        ''' Updates a client personal ref
        ''' </summary>
        ''' <param name="clientPersonalRef">Client Personal Ref Model</param>
        ''' <remarks></remarks>
        Sub UpdateClientPersonalRef(clientPersonalRef As Models.ClientPersonalRef)

        ''' <summary>
        ''' Removes client personal ref by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="clientPersonalRefId">Client Personal Ref ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientPersonalRef(clientPersonalRefId As Integer, clientId As Integer)

        ''' <summary>
        ''' Remove all client personal ref
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientPersonalRefs(clientId As Integer)

        ''' <summary>
        ''' Removes a client personal ref
        ''' </summary>
        ''' <param name="clientPersonalRef">Client Personal Ref Model</param>
        ''' <remarks></remarks>
        Sub RemoveClientPersonalRef(ByVal clientPersonalRef As Models.ClientPersonalRef)

    End Interface
End Namespace