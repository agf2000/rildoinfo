
Namespace RI.Modules.RIStore_Services
    Public Interface IClientBankRefsRepository

        ''' <summary>
        ''' Gets a list of client bank references by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientBankRefs(ByVal clientId As Integer) As IEnumerable(Of Models.ClientBankRef)

        ''' <summary>
        ''' Gets a client bank reference by id
        ''' </summary>
        ''' <param name="clientBankRefId">ClientBankRef ID</param>
        ''' <param name="clientId">Clientl ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientBankRef(ByVal clientBankRefId As Integer, ByVal clientId As Integer) As Models.ClientBankRef

        ''' <summary>
        ''' Adds a client bank reference
        ''' </summary>
        ''' <param name="clientBankRef">Client Address Model</param>
        ''' <remarks></remarks>
        Function AddClientBankRef(clientBankRef As Models.ClientBankRef) As Models.ClientBankRef

        ''' <summary>
        ''' Updates a client bank reference
        ''' </summary>
        ''' <param name="clientBankRef">Client Address Model</param>
        ''' <remarks></remarks>
        Sub UpdateClientBankRef(clientBankRef As Models.ClientBankRef)

        ''' <summary>
        ''' Removes client bank reference by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="clientBankRefId">Client Address ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientBankRef(clientBankRefId As Integer, clientId As Integer)

        ''' <summary>
        ''' Remove all client bank reference
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientBankRefs(clientId As Integer)

        ''' <summary>
        ''' Removes a client bank reference
        ''' </summary>
        ''' <param name="clientBankRef">Client Address Model</param>
        ''' <remarks></remarks>
        Sub RemoveClientBankRef(ByVal clientBankRef As Models.ClientBankRef)

    End Interface
End Namespace