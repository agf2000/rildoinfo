Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IOriginsRepository

        ''' <summary>
        ''' Gets a list of origins by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetOrigins(portalId As Integer) As IEnumerable(Of Origin)

        ''' <summary>
        ''' Gets an origin by id
        ''' </summary>
        ''' <param name="originId">Origin ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetOrigin(originId As Integer, portalId As Integer) As Origin

        ''' <summary>
        ''' Updates an origin info
        ''' </summary>
        ''' <param name="origin">Origin Model</param>
        ''' <remarks></remarks>
        Sub UpdateOrigin(origin As Origin)

        ''' <summary>
        ''' Adds an new origin
        ''' </summary>
        ''' <param name="origin">Origin Model</param>
        ''' <remarks></remarks>
        Function AddOrigin(origin As Origin) As Origin

        ''' <summary>
        ''' Removes an origin by id
        ''' </summary>
        ''' <param name="originId">Origin ID</param>
        ''' <param name="portalId">POrtal ID</param>
        ''' <remarks></remarks>
        Sub RemoveOrigin(originId As Integer, portalId As Integer)

        ''' <summary>
        ''' Removes an origin
        ''' </summary>
        ''' <param name="origin">Origin Model</param>
        ''' <remarks></remarks>
        Sub RemoveOrigin(origin As Origin)
    End Interface

End Namespace
