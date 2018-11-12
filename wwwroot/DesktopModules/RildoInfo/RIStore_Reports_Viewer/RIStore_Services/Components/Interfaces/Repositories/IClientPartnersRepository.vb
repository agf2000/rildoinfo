
Namespace RI.Modules.RIStore_Services
    Public Interface IClientPartnersRepository

        ''' <summary>
        ''' Gets a list of client partners by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientPartners(ByVal clientId As Integer) As IEnumerable(Of Models.ClientPartner)

        ''' <summary>
        ''' Gets a client partner by id
        ''' </summary>
        ''' <param name="clientPartnerId">Client Partner ID</param>
        ''' <param name="clientId">Clientl ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientPartner(ByVal clientPartnerId As Integer, ByVal clientId As Integer) As Models.ClientPartner

        ''' <summary>
        ''' Adds a client partner
        ''' </summary>
        ''' <param name="clientPartner">Client Partner Model</param>
        ''' <remarks></remarks>
        Function AddClientPartner(clientPartner As Models.ClientPartner) As Models.ClientPartner

        ''' <summary>
        ''' Updates a client partner
        ''' </summary>
        ''' <param name="clientPartner">Client Partner Model</param>
        ''' <remarks></remarks>
        Sub UpdateClientPartner(clientPartner As Models.ClientPartner)

        ''' <summary>
        ''' Removes client partner by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="clientPartnerId">Client Partner ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientPartner(clientPartnerId As Integer, clientId As Integer)

        ''' <summary>
        ''' Remove all client partner
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientPartners(clientId As Integer)

        ''' <summary>
        ''' Removes a client partner
        ''' </summary>
        ''' <param name="clientPartner">Client Partner Model</param>
        ''' <remarks></remarks>
        Sub RemoveClientPartner(ByVal clientPartner As Models.ClientPartner)

    End Interface
End Namespace