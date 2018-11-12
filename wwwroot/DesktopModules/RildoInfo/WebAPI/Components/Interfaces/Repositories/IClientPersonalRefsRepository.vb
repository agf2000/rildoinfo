Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IClientPersonalRefsRepository

        ''' <summary>
        ''' Gets a list of client's personal refs
        ''' </summary>
        ''' <param name="personId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientPersonalRefs(ByVal personId As Integer) As IEnumerable(Of ClientPersonalRef)

        ''' <summary>
        ''' Gets a client personal ref by id
        ''' </summary>
        ''' <param name="clientPersonalRefId">Client Personal Ref ID</param>
        ''' <param name="personId">Person ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientPersonalRef(ByVal clientPersonalRefId As Integer, ByVal personId As Integer) As ClientPersonalRef

        ''' <summary>
        ''' Adds a client personal ref
        ''' </summary>
        ''' <param name="clientPersonalRef">Client Personal Ref Model</param>
        ''' <remarks></remarks>
        Function AddClientPersonalRef(clientPersonalRef As ClientPersonalRef) As ClientPersonalRef

        ''' <summary>
        ''' Updates a client personal ref
        ''' </summary>
        ''' <param name="clientPersonalRef">Client Personal Ref Model</param>
        ''' <remarks></remarks>
        Sub UpdateClientPersonalRef(clientPersonalRef As ClientPersonalRef)

        ''' <summary>
        ''' Removes client personal ref
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <param name="clientPersonalRefId">Client Personal Ref ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientPersonalRef(clientPersonalRefId As Integer, personId As Integer)

        ''' <summary>
        ''' Remove all client personal ref
        ''' </summary>
        ''' <param name="personId">Person ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientPersonalRefs(personId As Integer)

        ''' <summary>
        ''' Removes a client personal ref
        ''' </summary>
        ''' <param name="clientPersonalRef">Client Personal Ref Model</param>
        ''' <remarks></remarks>
        Sub RemoveClientPersonalRef(ByVal clientPersonalRef As ClientPersonalRef)

    End Interface

End Namespace
