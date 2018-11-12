Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IClientCommRefsRepository

        ''' <summary>
        ''' Gets a list of client commerce references by client id
        ''' </summary>
        ''' <param name="personId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientCommRefs(ByVal personId As Integer) As IEnumerable(Of ClientCommRef)

        ''' <summary>
        ''' Gets a client commerce reference by id
        ''' </summary>
        ''' <param name="clientCommRefId">ClientCommRef ID</param>
        ''' <param name="personId">Clientl ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientCommRef(ByVal clientCommRefId As Integer, ByVal personId As Integer) As ClientCommRef

        ''' <summary>
        ''' Adds a client commerce reference
        ''' </summary>
        ''' <param name="clientCommRef">Client Commerce Reference Model</param>
        ''' <remarks></remarks>
        Function AddClientCommRef(clientCommRef As ClientCommRef) As ClientCommRef

        ''' <summary>
        ''' Updates a client commerce reference
        ''' </summary>
        ''' <param name="clientCommRef">Client Commerce Reference Model</param>
        ''' <remarks></remarks>
        Sub UpdateClientCommRef(clientCommRef As ClientCommRef)

        ''' <summary>
        ''' Removes client commerce reference by client id
        ''' </summary>
        ''' <param name="personId">Client ID</param>
        ''' <param name="clientCommRefId">Client Commerce Reference ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientCommRef(clientCommRefId As Integer, personId As Integer)

        ''' <summary>
        ''' Remove all client commerce reference
        ''' </summary>
        ''' <param name="personId">Client ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientCommRefs(personId As Integer)

        ''' <summary>
        ''' Removes a client commerce reference
        ''' </summary>
        ''' <param name="clientCommRef">Client Commerce Reference Model</param>
        ''' <remarks></remarks>
        Sub RemoveClientCommRef(ByVal clientCommRef As ClientCommRef)

    End Interface

End Namespace
