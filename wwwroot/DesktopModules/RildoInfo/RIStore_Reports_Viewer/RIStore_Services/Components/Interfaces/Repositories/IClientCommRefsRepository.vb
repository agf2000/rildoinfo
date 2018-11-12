
Namespace RI.Modules.RIStore_Services
    Public Interface IClientCommRefsRepository

        ''' <summary>
        ''' Gets a list of client commerce references by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientCommRefs(ByVal clientId As Integer) As IEnumerable(Of Models.ClientCommRef)

        ''' <summary>
        ''' Gets a client commerce reference by id
        ''' </summary>
        ''' <param name="clientCommRefId">ClientCommRef ID</param>
        ''' <param name="clientId">Clientl ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientCommRef(ByVal clientCommRefId As Integer, ByVal clientId As Integer) As Models.ClientCommRef

        ''' <summary>
        ''' Adds a client commerce reference
        ''' </summary>
        ''' <param name="clientCommRef">Client Commerce Reference Model</param>
        ''' <remarks></remarks>
        Function AddClientCommRef(clientCommRef As Models.ClientCommRef) As Models.ClientCommRef

        ''' <summary>
        ''' Updates a client commerce reference
        ''' </summary>
        ''' <param name="clientCommRef">Client Commerce Reference Model</param>
        ''' <remarks></remarks>
        Sub UpdateClientCommRef(clientCommRef As Models.ClientCommRef)

        ''' <summary>
        ''' Removes client commerce reference by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="clientCommRefId">Client Commerce Reference ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientCommRef(clientCommRefId As Integer, clientId As Integer)

        ''' <summary>
        ''' Remove all client commerce reference
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientCommRefs(clientId As Integer)

        ''' <summary>
        ''' Removes a client commerce reference
        ''' </summary>
        ''' <param name="clientCommRef">Client Commerce Reference Model</param>
        ''' <remarks></remarks>
        Sub RemoveClientCommRef(ByVal clientCommRef As Models.ClientCommRef)

    End Interface
End Namespace