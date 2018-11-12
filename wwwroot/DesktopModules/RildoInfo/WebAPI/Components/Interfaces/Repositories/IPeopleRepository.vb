Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IPeopleRepository

        ''' <summary>
        ''' Gets list of clients
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="searchField"></param>
        ''' <param name="salesRep">Sales Rep</param>
        ''' <param name="isDeleted">Is Deleted</param>
        ''' <param name="searchString">Search Term</param>
        ''' <param name="statusId">Status ID</param>
        ''' <param name="registrationType"></param>
        ''' <param name="startDate">Modified Date start Range</param>
        ''' <param name="endDate">Modified Date end Range</param>
        ''' <param name="filterDate"></param>
        ''' <param name="pageIndex">Page Number</param>
        ''' <param name="pageSize">Page Size</param>
        ''' <param name="orderBy">Order BY</param>
        ''' <param name="orderDesc">Order Direction</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPeople(portalId As Integer, searchField As String, salesRep As Integer, isDeleted As String, searchString As String, statusId As Integer, registrationType As String, startDate As DateTime, endDate As DateTime, filterDate As String, pageIndex As Integer, pageSize As Integer, orderBy As String, orderDesc As String) As IEnumerable(Of Person)

        ''' <summary>
        ''' Gets client info by client id
        ''' </summary>
        ''' <param name="personId">Client ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="userId">UserId</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPerson(personId As Integer, portalId As Integer, userId As Integer) As Person

        ''' <summary>
        ''' Adds client
        ''' </summary>
        ''' <param name="client">Client Model</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddPerson(client As Person) As Person

        ''' <summary>
        ''' Updates client
        ''' </summary>
        ''' <param name="client">Client Model</param>
        ''' <remarks></remarks>
        Sub UpdatePerson(client As Person)

        ''' <summary>
        ''' Deletes a client
        ''' </summary>
        ''' <param name="personId">Client ID</param>
        ''' <param name="cUserId">Client's User ID</param>
        ''' <param name="portalId">POrtal ID</param>
        ''' <remarks></remarks>
        Sub RemovePerson(ByVal personId As Integer, ByVal portalId As Integer, ByVal cUserId As Integer)

        ''' <summary>
        ''' Deletes a client
        ''' </summary>
        ''' <param name="client">Client Model</param>
        ''' <remarks></remarks>
        Sub RemovePerson(client As Person)

        ''' <summary>
        ''' Gets active people count
        ''' </summary>
        ''' <param name="portalId"></param>
        ''' <param name="userId">User ID</param>
        ''' <param name="types">Types</param>
        ''' <param name="isDeleted">Are deleted or not</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPeopleCount(portalId As Integer, userId As Integer, types As String, Optional isDeleted As Boolean = False) As Integer

        Function GetUsers(portalId As Integer, roleName As String, isDeleted As String, searchStr As String, startDate As DateTime, endDate As DateTime, pageIndex As Integer, pageSize As Integer, sortCol As String) As IEnumerable(Of Person)

        Function GetUsersCount(portalId As Integer) As Integer

    End Interface

End Namespace
