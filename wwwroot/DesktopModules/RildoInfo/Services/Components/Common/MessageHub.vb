
Imports Microsoft.AspNet.SignalR

Public Class MessageHub
    Inherits Hub

    Sub send(name As String, message As String)
        Clients.All.broadcastMessage(name, message)
    End Sub

End Class
