
Public Interface IPeopleRepository

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
    ''' <param name="PageIndex">Page Number</param>
    ''' <param name="PageSize">Page Size</param>
    ''' <param name="OrderBy">Order BY</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getPeople(portalId As Integer, salesRep As Integer, isDeleted As String, searchString As String, statusId As Integer, registrionType As String, startDate As DateTime, endDate As DateTime, pageIndex As Integer, pageSize As Integer, orderBy As String) As IEnumerable(Of Models.Person)

    ''' <summary>
    ''' Gets client info by client id
    ''' </summary>
    ''' <param name="personId">Client ID</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <param name="userId">UserId</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getPerson(personId As Integer, portalId As Integer, userId As Integer) As Models.Person

    ''' <summary>
    ''' Adds client
    ''' </summary>
    ''' <param name="client">Client Model</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function addPerson(client As Models.Person) As Models.Person

    ''' <summary>
    ''' Updates a client
    ''' </summary>
    ''' <param name="client">Client Model</param>
    ''' <remarks></remarks>
    Sub updatePerson(client As Models.Person)

    ''' <summary>
    ''' Deletes a client
    ''' </summary>
    ''' <param name="personId">Client ID</param>
    ''' <param name="cUserId">Client's User ID</param>
    ''' <param name="portalId">POrtal ID</param>
    ''' <remarks></remarks>
    Sub removePerson(ByVal personId As Integer, ByVal portalId As Integer, ByVal cUserId As Integer)

    ''' <summary>
    ''' Deletes a client
    ''' </summary>
    ''' <param name="client">Client Model</param>
    ''' <remarks></remarks>
    Sub removePerson(client As Models.Person)

End Interface
