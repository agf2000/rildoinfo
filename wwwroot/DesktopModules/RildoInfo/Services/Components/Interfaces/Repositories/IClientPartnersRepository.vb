
Public Interface IClientPartnersRepository

    ''' <summary>
    ''' Gets a list of client partners by client id
    ''' </summary>
    ''' <param name="personId">Client ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getClientPartners(ByVal personId As Integer) As IEnumerable(Of Models.ClientPartner)

    ''' <summary>
    ''' Gets a client partner by id
    ''' </summary>
    ''' <param name="clientPartnerId">Client Partner ID</param>
    ''' <param name="personId">Clientl ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getClientPartner(ByVal clientPartnerId As Integer, ByVal personId As Integer) As Models.ClientPartner

    ''' <summary>
    ''' Adds a client partner
    ''' </summary>
    ''' <param name="clientPartner">Client Partner Model</param>
    ''' <remarks></remarks>
    Function addClientPartner(clientPartner As Models.ClientPartner) As Models.ClientPartner

    ''' <summary>
    ''' Updates a client partner
    ''' </summary>
    ''' <param name="clientPartner">Client Partner Model</param>
    ''' <remarks></remarks>
    Sub updateClientPartner(clientPartner As Models.ClientPartner)

    ''' <summary>
    ''' Removes client partner by client id
    ''' </summary>
    ''' <param name="personId">Client ID</param>
    ''' <param name="clientPartnerId">Client Partner ID</param>
    ''' <remarks></remarks>
    Sub removeClientPartner(clientPartnerId As Integer, personId As Integer)

    ''' <summary>
    ''' Remove all client partner
    ''' </summary>
    ''' <param name="personId">Client ID</param>
    ''' <remarks></remarks>
    Sub removeClientPartners(personId As Integer)

    ''' <summary>
    ''' Removes a client partner
    ''' </summary>
    ''' <param name="clientPartner">Client Partner Model</param>
    ''' <remarks></remarks>
    Sub removeClientPartner(ByVal clientPartner As Models.ClientPartner)

End Interface
