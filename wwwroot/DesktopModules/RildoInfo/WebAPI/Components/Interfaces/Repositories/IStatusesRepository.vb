Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IStatusesRepository

        ''' <summary>
        ''' Adds a new status
        ''' </summary>
        ''' <param name="status">Status Model</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddStatus(status As Status) As Status

        ''' <summary>
        ''' Gets a list of statuses by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="isDeleted"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetStatuses(portalId As Integer, isDeleted As String) As IEnumerable(Of Status)

        ''' <summary>
        ''' Gets status
        ''' </summary>
        ''' <param name="statusId">Status ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetStatus1(statusId As Integer, portalId As Integer) As Status

        ''' <summary>
        ''' Gets status
        ''' </summary>
        ''' <param name="statusName">Status Name</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetStatus(statusName As String, portalId As Integer) As Status

        ''' <summary>
        ''' Updates a status
        ''' </summary>
        ''' <param name="status">Status Model</param>
        ''' <remarks></remarks>
        Sub UpdateStatus(status As Status)

        ''' <summary>
        ''' Removes status
        ''' </summary>
        ''' <param name="statusId">Status ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <remarks></remarks>
        Sub RemoveStatus1(statusId As Integer, portalId As Integer)

        ''' <summary>
        ''' Removes status
        ''' </summary>
        ''' <param name="status">Status Model</param>
        ''' <remarks></remarks>
        Sub RemoveStatus(status As Status)

    End Interface

End Namespace
