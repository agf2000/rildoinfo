
Imports System.Collections.Generic
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Services.Log.EventLog

Public Class Services
    Inherits PortalModuleBase

    Public Shared Function ValidateNewUser(userName As String) As Integer
        Return Users.UserController.GetUsersByEmail(Null.NullInteger, userName.ToLower(), Null.NullInteger(), Null.NullInteger(), Null.NullInteger()).Count
    End Function

    Public Shared Function ValidateUser(userName As String) As Integer
        Return Users.UserController.GetUsersByUserName(Null.NullInteger, userName.ToLower(), Null.NullInteger(), Null.NullInteger(), Null.NullInteger()).Count
    End Function

    Public Shared Function ValidatePerson(vTerm As String) As Integer
        Dim clients As IEnumerable(Of Components.Models.Person)
        Using context As IDataContext = DataContext.Instance()
            Dim rep = context.GetRepository(Of Components.Models.Person)()
            clients = rep.Find("Where Email = @0", vTerm)
            'If Not clients.Count > 0 Then
            '    clients = repository.Find("Where Telephone = @0", vTerm)
            '    If Not clients.Count > 0 Then
            '        clients = repository.Find("Where Cell = @0", vTerm)
            '    End If
            'End If
        End Using
        Return clients.Count
    End Function

    Public Sub Log(action As Models.ServicesAction)
        Dim eventLog As New EventLogController()

        Dim logInfo As DotNetNuke.Services.Log.EventLog.LogInfo = Nothing
        logInfo = New LogInfo()
        logInfo.LogUserName = action.Username
        logInfo.LogPortalID = PortalId
        logInfo.LogTypeKey = action.LogTypeKey
        logInfo.LogServerName = action.LogServerName
        logInfo.AddProperty("Requested By", action.AppName)
        'logInfo.AddProperty("PropertyName2", propertyValue2);

        eventLog.AddLog(logInfo)
    End Sub

    Public Function GetUserByName(username As String) As Models.ServicesUser
        Dim user As New Models.ServicesUser()
        Dim userInfo As UserInfo = UserController.GetUserByName(username)
        user.AffiliateID = userInfo.AffiliateID
        user.DisplayName = userInfo.DisplayName
        user.Email = userInfo.Email
        user.FirstName = userInfo.FirstName
        user.IsSuperUser = userInfo.IsSuperUser
        user.LastIPAddress = userInfo.LastIPAddress
        user.LastName = userInfo.LastName
        user.PortalID = userInfo.PortalID
        user.Roles = userInfo.Roles
        user.UserID = userInfo.UserID
        user.Username = userInfo.Username
        Return user
    End Function

    Public Function GetUserID(username As String) As Integer
        Dim userID As Integer = 0
        Dim userInfo As UserInfo = UserController.GetUserByName(username)
        userID = userInfo.UserID
        Return userID
    End Function

End Class
