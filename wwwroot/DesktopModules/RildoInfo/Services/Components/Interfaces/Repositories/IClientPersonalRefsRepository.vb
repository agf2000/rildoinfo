
Public Interface IClientPersonalRefsRepository

    ''' <summary>
    ''' Gets a list of client's personal refs
    ''' </summary>
    ''' <param name="personId">Client ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getClientPersonalRefs(ByVal personId As Integer) As IEnumerable(Of Models.ClientPersonalRef)

    ''' <summary>
    ''' Gets a client personal ref by id
    ''' </summary>
    ''' <param name="ClientPersonalRefId">Client Personal Ref ID</param>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getClientPersonalRef(ByVal ClientPersonalRefId As Integer, ByVal personId As Integer) As Models.ClientPersonalRef

    ''' <summary>
    ''' Adds a client personal ref
    ''' </summary>
    ''' <param name="clientPersonalRef">Client Personal Ref Model</param>
    ''' <remarks></remarks>
    Function addClientPersonalRef(clientPersonalRef As Models.ClientPersonalRef) As Models.ClientPersonalRef

    ''' <summary>
    ''' Updates a client personal ref
    ''' </summary>
    ''' <param name="clientPersonalRef">Client Personal Ref Model</param>
    ''' <remarks></remarks>
    Sub updateClientPersonalRef(clientPersonalRef As Models.ClientPersonalRef)

    ''' <summary>
    ''' Removes client personal ref
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <param name="ClientPersonalRefId">Client Personal Ref ID</param>
    ''' <remarks></remarks>
    Sub removeClientPersonalRef(ClientPersonalRefId As Integer, personId As Integer)

    ''' <summary>
    ''' Remove all client personal ref
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <remarks></remarks>
    Sub removeClientPersonalRefs(personId As Integer)

    ''' <summary>
    ''' Removes a client personal ref
    ''' </summary>
    ''' <param name="clientPersonalRef">Client Personal Ref Model</param>
    ''' <remarks></remarks>
    Sub removeClientPersonalRef(ByVal clientPersonalRef As Models.ClientPersonalRef)

End Interface
