
Public Interface IClientBankRefsRepository

    ''' <summary>
    ''' Gets a list of client's bank references
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getClientBankRefs(ByVal personId As Integer) As IEnumerable(Of Models.ClientBankRef)

    ''' <summary>
    ''' Gets a client bank reference
    ''' </summary>
    ''' <param name="clientBankRefId">Client Bank Ref ID</param>
    ''' <param name="personId">Personl ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getClientBankRef(ByVal clientBankRefId As Integer, ByVal personId As Integer) As Models.ClientBankRef

    ''' <summary>
    ''' Adds a client bank reference
    ''' </summary>
    ''' <param name="clientBankRef">Client Address Model</param>
    ''' <remarks></remarks>
    Function addClientBankRef(clientBankRef As Models.ClientBankRef) As Models.ClientBankRef

    ''' <summary>
    ''' Updates a client bank reference
    ''' </summary>
    ''' <param name="clientBankRef">Person Address Model</param>
    ''' <remarks></remarks>
    Sub updateClientBankRef(clientBankRef As Models.ClientBankRef)

    ''' <summary>
    ''' Removes client bank reference
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <param name="clientBankRefId">Person Address ID</param>
    ''' <remarks></remarks>
    Sub removeClientBankRef(clientBankRefId As Integer, personId As Integer)

    ''' <summary>
    ''' Remove all client bank reference
    ''' </summary>
    ''' <param name="personId">Person ID</param>
    ''' <remarks></remarks>
    Sub removeClientBankRefs(personId As Integer)

    ''' <summary>
    ''' Removes a client bank reference
    ''' </summary>
    ''' <param name="clientBankRef">Person Address Model</param>
    ''' <remarks></remarks>
    Sub removeClientBankRef(ByVal clientBankRef As Models.ClientBankRef)

End Interface
