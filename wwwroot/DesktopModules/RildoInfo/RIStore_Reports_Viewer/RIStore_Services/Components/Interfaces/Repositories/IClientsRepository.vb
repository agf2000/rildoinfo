
Namespace RI.Modules.RIStore_Services
    Public Interface IClientsRepository

        ''' <summary>
        ''' Gets list of clients
        ''' </summary>
        ''' <param name="PortalId">Portal ID</param>
        ''' <param name="SalesRep">Sales Rep</param>
        ''' <param name="IsDeleted">Is Deleted</param>
        ''' <param name="SearchString">Search Term</param>
        ''' <param name="StatusId">Status ID</param>
        ''' <param name="StartDate">Modified Date start Range</param>
        ''' <param name="EndDate">Modified Date end Range</param>
        ''' <param name="PageNumber">Page Number</param>
        ''' <param name="PageSize">Page Size</param>
        ''' <param name="OrderBy">Order BY</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClients(PortalId As String, SalesRep As Integer, IsDeleted As String, SearchString As String, StatusId As Integer, StartDate As DateTime, EndDate As DateTime, PageNumber As Integer, PageSize As Integer, OrderBy As String) As IEnumerable(Of Models.Client)

        ''' <summary>
        ''' Gets client info by client id
        ''' </summary>
        ''' <param name="cId">Client ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClient(cId As Integer, portalId As Integer) As Models.Client

        ''' <summary>
        ''' Adds client
        ''' </summary>
        ''' <param name="client">Client Model</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddClient(client As Models.Client) As Models.Client

        ''' <summary>
        ''' Updates a client
        ''' </summary>
        ''' <param name="client">Client Model</param>
        ''' <remarks></remarks>
        Sub UpdateClient(client As Models.Client)

        ''' <summary>
        ''' Deletes a client
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="cUserId">Client's User ID</param>
        ''' <param name="portalId">POrtal ID</param>
        ''' <remarks></remarks>
        Sub RemoveClient(ByVal clientId As Integer, ByVal portalId As Integer, ByVal cUserId As Integer)

        ''' <summary>
        ''' Deletes a client
        ''' </summary>
        ''' <param name="client">Client Model</param>
        ''' <remarks></remarks>
        Sub RemoveClient(client As Models.Client)

    End Interface
End Namespace