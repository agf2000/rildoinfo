Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IClientPartnersRepository

        ''' <summary>
        ''' Gets a list of client partners by client id
        ''' </summary>
        ''' <param name="personId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientPartners(ByVal personId As Integer) As IEnumerable(Of ClientPartner)

        ''' <summary>
        ''' Gets a client partner by id
        ''' </summary>
        ''' <param name="clientPartnerId">Client Partner ID</param>
        ''' <param name="personId">Clientl ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientPartner(ByVal clientPartnerId As Integer, ByVal personId As Integer) As ClientPartner

        ''' <summary>
        ''' Adds a client partner
        ''' </summary>
        ''' <param name="clientPartner">Client Partner Model</param>
        ''' <remarks></remarks>
        Function AddClientPartner(clientPartner As ClientPartner) As ClientPartner

        ''' <summary>
        ''' Updates a client partner
        ''' </summary>
        ''' <param name="clientPartner">Client Partner Model</param>
        ''' <remarks></remarks>
        Sub UpdateClientPartner(clientPartner As ClientPartner)

        ''' <summary>
        ''' Removes client partner by client id
        ''' </summary>
        ''' <param name="personId">Client ID</param>
        ''' <param name="clientPartnerId">Client Partner ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientPartner(clientPartnerId As Integer, personId As Integer)

        ''' <summary>
        ''' Remove all client partner
        ''' </summary>
        ''' <param name="personId">Client ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientPartners(personId As Integer)

        ''' <summary>
        ''' Removes a client partner
        ''' </summary>
        ''' <param name="clientPartner">Client Partner Model</param>
        ''' <remarks></remarks>
        Sub RemoveClientPartner(ByVal clientPartner As ClientPartner)

    End Interface

End Namespace
