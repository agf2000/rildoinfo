Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IClientPartnerBankRefsRepository

        ''' <summary>
        ''' Gets a list of client partner's bank refs by client id
        ''' </summary>
        ''' <param name="personId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientPartnerBankRefs(ByVal personId As Integer) As IEnumerable(Of ClientPartnerBankRef)

        ''' <summary>
        ''' Gets a client partner's bank ref by id
        ''' </summary>
        ''' <param name="clientPartnerBankRefId">Client Partner Bank Ref ID</param>
        ''' <param name="personId">Clientl ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientPartnerBankRef(ByVal clientPartnerBankRefId As Integer, ByVal personId As Integer) As ClientPartnerBankRef

        ''' <summary>
        ''' Adds a client partner's bank ref
        ''' </summary>
        ''' <param name="clientPartnerBankRef">Client Partner Bank Ref Model</param>
        ''' <remarks></remarks>
        Function AddClientPartnerBankRef(clientPartnerBankRef As ClientPartnerBankRef) As ClientPartnerBankRef

        ''' <summary>
        ''' Updates a client partner's bank ref
        ''' </summary>
        ''' <param name="clientPartnerBankRef">Client Partner Bank Ref Model</param>
        ''' <remarks></remarks>
        Sub UpdateClientPartnerBankRef(clientPartnerBankRef As ClientPartnerBankRef)

        ''' <summary>
        ''' Removes client partner's bank ref by client id
        ''' </summary>
        ''' <param name="personId">Client ID</param>
        ''' <param name="clientPartnerBankRefId">Client Partner Bank Ref ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientPartnerBankRef(clientPartnerBankRefId As Integer, personId As Integer)

        ''' <summary>
        ''' Remove all client partner's bank ref
        ''' </summary>
        ''' <param name="personId">Client ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientPartnerBankRefs(personId As Integer)

        ''' <summary>
        ''' Removes a client partner's bank ref
        ''' </summary>
        ''' <param name="clientPartnerBankRef">Client Partner Bank Ref Model</param>
        ''' <remarks></remarks>
        Sub RemoveClientPartnerBankRef(ByVal clientPartnerBankRef As ClientPartnerBankRef)

    End Interface

End Namespace
