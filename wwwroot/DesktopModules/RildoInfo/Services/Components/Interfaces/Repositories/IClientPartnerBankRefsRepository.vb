
Public Interface IClientPartnerBankRefsRepository

    ''' <summary>
    ''' Gets a list of client partner's bank refs by client id
    ''' </summary>
    ''' <param name="personId">Client ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getClientPartnerBankRefs(ByVal personId As Integer) As IEnumerable(Of Models.ClientPartnerBankRef)

    ''' <summary>
    ''' Gets a client partner's bank ref by id
    ''' </summary>
    ''' <param name="clientPartnerBankRefId">Client Partner Bank Ref ID</param>
    ''' <param name="personId">Clientl ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getClientPartnerBankRef(ByVal clientPartnerBankRefId As Integer, ByVal personId As Integer) As Models.ClientPartnerBankRef

    ''' <summary>
    ''' Adds a client partner's bank ref
    ''' </summary>
    ''' <param name="clientPartnerBankRef">Client Partner Bank Ref Model</param>
    ''' <remarks></remarks>
    Function addClientPartnerBankRef(clientPartnerBankRef As Models.ClientPartnerBankRef) As Models.ClientPartnerBankRef

    ''' <summary>
    ''' Updates a client partner's bank ref
    ''' </summary>
    ''' <param name="clientPartnerBankRef">Client Partner Bank Ref Model</param>
    ''' <remarks></remarks>
    Sub updateClientPartnerBankRef(clientPartnerBankRef As Models.ClientPartnerBankRef)

    ''' <summary>
    ''' Removes client partner's bank ref by client id
    ''' </summary>
    ''' <param name="personId">Client ID</param>
    ''' <param name="clientPartnerBankRefId">Client Partner Bank Ref ID</param>
    ''' <remarks></remarks>
    Sub removeClientPartnerBankRef(clientPartnerBankRefId As Integer, personId As Integer)

    ''' <summary>
    ''' Remove all client partner's bank ref
    ''' </summary>
    ''' <param name="personId">Client ID</param>
    ''' <remarks></remarks>
    Sub removeClientPartnerBankRefs(personId As Integer)

    ''' <summary>
    ''' Removes a client partner's bank ref
    ''' </summary>
    ''' <param name="clientPartnerBankRef">Client Partner Bank Ref Model</param>
    ''' <remarks></remarks>
    Sub RemoveClientPartnerBankRef(ByVal clientPartnerBankRef As Models.ClientPartnerBankRef)

End Interface
