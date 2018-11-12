
Namespace RI.Modules.RIStore_Services
    Public Interface IClientAddressesRepository

        ''' <summary>
        ''' Gets a list of client addresses by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientAddresses(ByVal clientId As Integer) As IEnumerable(Of Models.ClientAddress)

        ''' <summary>
        ''' Gets a client address by id
        ''' </summary>
        ''' <param name="ClientAddressId">ClientAddress ID</param>
        ''' <param name="clientId">Clientl ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientAddress(ByVal clientAddressId As Integer, ByVal clientId As Integer) As Models.ClientAddress

        ''' <summary>
        ''' Adds a client address
        ''' </summary>
        ''' <param name="clientAddress">Client Address Model</param>
        ''' <remarks></remarks>
        Function AddClientAddress(clientAddress As Models.ClientAddress) As Models.ClientAddress

        ''' <summary>
        ''' Updates a client address
        ''' </summary>
        ''' <param name="clientAddress">Client Address Model</param>
        ''' <remarks></remarks>
        Sub UpdateClientAddress(clientAddress As Models.ClientAddress)

        ''' <summary>
        ''' Removes client address by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="clientAddressId">Client Address ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientAddress(clientAddressId As Integer, clientId As Integer)

        ''' <summary>
        ''' Remove all client addresses
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientAddresses(clientId As Integer)

        ''' <summary>
        ''' Removes a client address
        ''' </summary>
        ''' <param name="clientAddress">Client Address Model</param>
        ''' <remarks></remarks>
        Sub RemoveClientAddress(ByVal clientAddress As Models.ClientAddress)

    End Interface
End Namespace