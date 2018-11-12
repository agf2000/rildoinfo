Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IClientBankRefsRepository

        ''' <summary>
        ''' Gets a list of client's bank references
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientBankRefs(ByVal personId As Integer) As IEnumerable(Of ClientBankRef)

        ''' <summary>
        ''' Gets a client bank reference
        ''' </summary>
        ''' <param name="clientBankRefId">Client Bank Ref ID</param>
        ''' <param name="personId">Personl ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientBankRef(ByVal clientBankRefId As Integer, ByVal personId As Integer) As ClientBankRef

        ''' <summary>
        ''' Adds a client bank reference
        ''' </summary>
        ''' <param name="clientBankRef">Client Address Model</param>
        ''' <remarks></remarks>
        Function AddClientBankRef(clientBankRef As ClientBankRef) As ClientBankRef

        ''' <summary>
        ''' Updates a client bank reference
        ''' </summary>
        ''' <param name="clientBankRef">Person Address Model</param>
        ''' <remarks></remarks>
        Sub UpdateClientBankRef(clientBankRef As ClientBankRef)

        ''' <summary>
        ''' Removes client bank reference
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <param name="clientBankRefId">Person Address ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientBankRef(clientBankRefId As Integer, personId As Integer)

        ''' <summary>
        ''' Remove all client bank reference
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientBankRefs(personId As Integer)

        ''' <summary>
        ''' Removes a client bank reference
        ''' </summary>
        ''' <param name="clientBankRef">Person Address Model</param>
        ''' <remarks></remarks>
        Sub RemoveClientBankRef(ByVal clientBankRef As ClientBankRef)

    End Interface

End Namespace
