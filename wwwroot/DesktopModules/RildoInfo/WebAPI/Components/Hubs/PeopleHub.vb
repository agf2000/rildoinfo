
Imports Microsoft.AspNet.SignalR
Imports System.Threading.Tasks
Imports System.Collections.Concurrent

'Public NotInheritable Class UserHandler
'    Private Sub New()
'    End Sub
'    Public Shared ConnectedIds As New HashSet(Of String)()
'End Class

Public Class PeopleHub
    Inherits Hub

    Public Shared UserList As New ConcurrentDictionary(Of String, Models.UserListConnected)()

    Public Overrides Function OnConnected() As System.Threading.Tasks.Task

        Clients.Caller.connectedUser()

        Return MyBase.OnConnected()

    End Function

    'Public Overrides Function OnReconnected() As System.Threading.Tasks.Task
    '    'Dim clientId As String = GetConnectionId()
    '    'If _users.IndexOf(clientId) = -1 Then
    '    '    _users.Add(clientId)
    '    'End If

    '    'ShowUsersOnLine()

    '    Return MyBase.OnReconnected()
    'End Function

    Public Overrides Function OnDisconnected() As System.Threading.Tasks.Task
        Dim value As New Models.UserListConnected
        If value IsNot Nothing Then
            UserList.TryRemove(Context.ConnectionId, value)
            If value IsNot Nothing Then
                Return Clients.Group(value.GroupName).showConnected(UserList)
            End If
        End If
        Return Nothing
        'Return MyBase.OnDisconnected()
    End Function

    Public Sub ClientsJoin(groupName As String)
        Groups.Add(Context.ConnectionId, groupName)
    End Sub

    Public Sub GetUsersOnLine(groupName As String)
        Clients.Group(groupName).showConnected(UserList)
    End Sub

    Public Sub Join(data As Models.UserListConnected)
        Dim userInfo = Users.UserController.GetUserById(data.PortalId, data.UserId)
        data.DisplayName = userInfo.DisplayName

        If Not UserList.TryUpdate(Context.ConnectionId, data, data) Then
            UserList.TryAdd(Context.ConnectionId, data)
        End If

        Clients.Group(data.GroupName).showConnected(UserList)
    End Sub

    ''a list of connectionrecords to keep track of users connected
    'Private Shared ReadOnly ClientUsers As New List(Of UserListConnected)()

    'Public Overrides Function OnConnected() As Task
    '    UserHandler.ConnectedIds.Add(Context.ConnectionId)
    '    Clients.Caller.connectedUser()
    '    Return MyBase.OnConnected()
    'End Function

    'Public Overrides Function OnReconnected() As Task
    '    Clients.Caller.ReconnectUser()
    '    Return MyBase.OnReconnected()
    'End Function

    'Public Overrides Function OnDisconnected() As Task
    '    UserHandler.ConnectedIds.Remove(Context.ConnectionId)
    '    Clients.Caller.disconectUser()
    '    Return MyBase.OnDisconnected()
    'End Function

    'Public Function GetUserInfo(portalId As Integer, userid As Integer) As Task
    '    Dim _user = New models.user With {.u UserController.GetUserById(portalId, userid)

    '    Clients.Caller.addUserInfo(getUsers(users))
    '    Return MyBase.OnConnected()
    'End Function

End Class
