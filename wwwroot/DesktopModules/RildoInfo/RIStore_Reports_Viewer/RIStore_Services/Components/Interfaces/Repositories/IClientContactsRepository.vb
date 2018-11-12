
Namespace RI.Modules.RIStore_Services
    Public Interface IClientContactsRepository

        ''' <summary>
        ''' Gets a list of client contacts by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientContacts(ByVal clientId As Integer) As IEnumerable(Of Models.ClientContact)

        ''' <summary>
        ''' Gets a client contact by id
        ''' </summary>
        ''' <param name="clientContactId">ClientContact ID</param>
        ''' <param name="clientId">Clientl ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientContact(ByVal clientContactId As Integer, ByVal clientId As Integer) As Models.ClientContact

        ''' <summary>
        ''' Adds a client contact
        ''' </summary>
        ''' <param name="clientContact">Client Contact Model</param>
        ''' <remarks></remarks>
        Function AddClientContact(clientContact As Models.ClientContact) As Models.ClientContact

        ''' <summary>
        ''' Updates a client contact
        ''' </summary>
        ''' <param name="clientContact">Client Contact Model</param>
        ''' <remarks></remarks>
        Sub UpdateClientContact(clientContact As Models.ClientContact)

        ''' <summary>
        ''' Removes client contact by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="clientContactId">Client Contact ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientContact(clientContactId As Integer, clientId As Integer)

        ''' <summary>
        ''' Remove all client contact
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientContacts(clientId As Integer)

        ''' <summary>
        ''' Removes a client contact
        ''' </summary>
        ''' <param name="clientContact">Client Contact Model</param>
        ''' <remarks></remarks>
        Sub RemoveClientContact(ByVal clientContact As Models.ClientContact)

    End Interface
End Namespace