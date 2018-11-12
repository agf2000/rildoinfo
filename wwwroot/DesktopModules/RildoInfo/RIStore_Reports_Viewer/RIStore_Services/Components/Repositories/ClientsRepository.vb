
Imports System.Collections.Generic
Imports DotNetNuke.Data

Namespace RI.Modules.RIStore_Services
    Public Class ClientsRepository
        Implements IClientsRepository

#Region "Private Methods"

        Private Shared Function GetNull(field As Object) As Object
            Return Null.GetNull(field, DBNull.Value)
        End Function

#End Region

#Region "Public Methods"

        Public Function GetClients(PortalId As String, SalesRep As Integer, IsDeleted As String, SearchString As String, StatusId As Integer, StartDate As DateTime, EndDate As DateTime, PageNumber As Integer, PageSize As Integer, OrderBy As String) As IEnumerable(Of Models.Client) Implements IClientsRepository.GetClients
            Return CBO.FillCollection(Of Models.Client)(DataProvider.Instance().GetClients(PortalId, SalesRep, IsDeleted, SearchString, StatusId, StartDate, EndDate, PageNumber, PageSize, OrderBy))
            'Dim clients As IEnumerable(Of Models.Client)

            'Using ctx As IDataContext = DataContext.Instance()
            '    clients = ctx.ExecuteQuery(Of Models.Client)(CommandType.StoredProcedure, "RIS_Clients_Get", PortalId, GetNull(SalesRep), IsDeleted, SearchString, GetNull(StatusId), GetNull(StartDate), GetNull(EndDate), PageNumber, PageSize, OrderBy)
            'End Using
            'Return clients
        End Function

        Public Function GetClient(clientId As Integer, portalId As Integer) As Models.Client Implements IClientsRepository.GetClient
            Return CBO.FillObject(Of Models.Client)(DataProvider.Instance().GetClient(clientId, portalId))
            'Dim client As Models.Client

            'Using ctx As IDataContext = DataContext.Instance()
            '    client = ctx.ExecuteSingleOrDefault(Of Models.Client)(CommandType.StoredProcedure, "RIS_Client_Get", clientId)
            'End Using
            'Return client
        End Function

        Public Function AddClient(client As Models.Client) As Models.Client Implements IClientsRepository.AddClient
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Client) = ctx.GetRepository(Of Models.Client)()
                rep.Insert(client)
            End Using
            Return client
        End Function

        Public Sub UpdateClient(client As Models.Client) Implements IClientsRepository.UpdateClient
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Client) = ctx.GetRepository(Of Models.Client)()
                rep.Update(client)
            End Using
        End Sub

        Public Sub RemoveClient(client As Models.Client) Implements IClientsRepository.RemoveClient
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Client) = ctx.GetRepository(Of Models.Client)()
                rep.Delete(client)
            End Using
        End Sub

        Public Sub RemoveClient(clientId As Integer, portalId As Integer, cUserId As Integer) Implements IClientsRepository.RemoveClient
            Dim _item As Models.Client = GetClient(clientId, portalId)
            If _item Is Nothing Then
                Throw New ArgumentException("Cliente não encontrado!")
            Else
                RemoveClient(_item)

                If cUserId > 0 Then
                    Users.UserController.RemoveUser(Users.UserController.GetUserById(portalId, cUserId))
                End If
            End If
        End Sub

#End Region

    End Class
End Namespace