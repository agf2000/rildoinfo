
Public Interface IStatusesRepository

    ''' <summary>
    ''' Adds a new status
    ''' </summary>
    ''' <param name="status">Status Model</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function addStatus(status As Models.Status) As Models.Status

    ''' <summary>
    ''' Gets a list of statuses by portal id
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="isDeleted"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getStatuses(portalId As Integer, isDeleted As String) As IEnumerable(Of Models.Status)

    ''' <summary>
    ''' Gets status
    ''' </summary>
    ''' <param name="statusId">Status ID</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getStatus(statusId As Integer, portalId As Integer) As Models.Status

    ''' <summary>
    ''' Gets status
    ''' </summary>
    ''' <param name="statusName">Status Name</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getStatus(statusName As String, portalId As Integer) As Models.Status

    ''' <summary>
    ''' Updates a status
    ''' </summary>
    ''' <param name="status">Status Model</param>
    ''' <remarks></remarks>
    Sub updateStatus(status As Models.Status)

    ''' <summary>
    ''' Removes status
    ''' </summary>
    ''' <param name="statusId">Status ID</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <remarks></remarks>
    Sub removeStatus(statusId As Integer, portalId As Integer)

    ''' <summary>
    ''' Removes status
    ''' </summary>
    ''' <param name="status">Status Model</param>
    ''' <remarks></remarks>
    Sub removeStatus(status As Models.Status)

End Interface
