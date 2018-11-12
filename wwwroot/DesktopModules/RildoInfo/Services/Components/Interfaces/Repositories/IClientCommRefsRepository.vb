
Public Interface IClientCommRefsRepository

    ''' <summary>
    ''' Gets a list of client commerce references by client id
    ''' </summary>
    ''' <param name="personId">Client ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getClientCommRefs(ByVal personId As Integer) As IEnumerable(Of Models.ClientCommRef)

    ''' <summary>
    ''' Gets a client commerce reference by id
    ''' </summary>
    ''' <param name="clientCommRefId">ClientCommRef ID</param>
    ''' <param name="personId">Clientl ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getClientCommRef(ByVal clientCommRefId As Integer, ByVal personId As Integer) As Models.ClientCommRef

    ''' <summary>
    ''' Adds a client commerce reference
    ''' </summary>
    ''' <param name="clientCommRef">Client Commerce Reference Model</param>
    ''' <remarks></remarks>
    Function addClientCommRef(clientCommRef As Models.ClientCommRef) As Models.ClientCommRef

    ''' <summary>
    ''' Updates a client commerce reference
    ''' </summary>
    ''' <param name="clientCommRef">Client Commerce Reference Model</param>
    ''' <remarks></remarks>
    Sub updateClientCommRef(clientCommRef As Models.ClientCommRef)

    ''' <summary>
    ''' Removes client commerce reference by client id
    ''' </summary>
    ''' <param name="personId">Client ID</param>
    ''' <param name="clientCommRefId">Client Commerce Reference ID</param>
    ''' <remarks></remarks>
    Sub RemoveClientCommRef(clientCommRefId As Integer, personId As Integer)

    ''' <summary>
    ''' Remove all client commerce reference
    ''' </summary>
    ''' <param name="personId">Client ID</param>
    ''' <remarks></remarks>
    Sub removeClientCommRefs(personId As Integer)

    ''' <summary>
    ''' Removes a client commerce reference
    ''' </summary>
    ''' <param name="clientCommRef">Client Commerce Reference Model</param>
    ''' <remarks></remarks>
    Sub removeClientCommRef(ByVal clientCommRef As Models.ClientCommRef)

End Interface
