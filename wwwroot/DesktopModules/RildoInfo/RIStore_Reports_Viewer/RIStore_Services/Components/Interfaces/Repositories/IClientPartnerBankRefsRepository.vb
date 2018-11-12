
Namespace RI.Modules.RIStore_Services
    Public Interface IClientPartnerBankRefsRepository

        ''' <summary>
        ''' Gets a list of client partner's bank refs by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientPartnerBankRefs(ByVal clientId As Integer) As IEnumerable(Of Models.ClientPartnerBankRef)

        ''' <summary>
        ''' Gets a client partner's bank ref by id
        ''' </summary>
        ''' <param name="clientPartnerBankRefId">Client Partner Bank Ref ID</param>
        ''' <param name="clientId">Clientl ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientPartnerBankRef(ByVal clientPartnerBankRefId As Integer, ByVal clientId As Integer) As Models.ClientPartnerBankRef

        ''' <summary>
        ''' Adds a client partner's bank ref
        ''' </summary>
        ''' <param name="clientPartnerBankRef">Client Partner Bank Ref Model</param>
        ''' <remarks></remarks>
        Function AddClientPartnerBankRef(clientPartnerBankRef As Models.ClientPartnerBankRef) As Models.ClientPartnerBankRef

        ''' <summary>
        ''' Updates a client partner's bank ref
        ''' </summary>
        ''' <param name="clientPartnerBankRef">Client Partner Bank Ref Model</param>
        ''' <remarks></remarks>
        Sub UpdateClientPartnerBankRef(clientPartnerBankRef As Models.ClientPartnerBankRef)

        ''' <summary>
        ''' Removes client partner's bank ref by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="clientPartnerBankRefId">Client Partner Bank Ref ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientPartnerBankRef(clientPartnerBankRefId As Integer, clientId As Integer)

        ''' <summary>
        ''' Remove all client partner's bank ref
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientPartnerBankRefs(clientId As Integer)

        ''' <summary>
        ''' Removes a client partner's bank ref
        ''' </summary>
        ''' <param name="clientPartnerBankRef">Client Partner Bank Ref Model</param>
        ''' <remarks></remarks>
        Sub RemoveClientPartnerBankRef(ByVal clientPartnerBankRef As Models.ClientPartnerBankRef)

    End Interface
End Namespace