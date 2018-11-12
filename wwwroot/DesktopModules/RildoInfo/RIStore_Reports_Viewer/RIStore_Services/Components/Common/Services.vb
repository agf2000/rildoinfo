
Imports System.Collections.Generic
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Services.Log.EventLog

Namespace RI.Modules.RIStore_Services

    Public Class Services
        Inherits PortalModuleBase

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
End Namespace