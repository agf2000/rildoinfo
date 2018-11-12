
Imports System.Collections.Generic
Imports DotNetNuke.Data

Namespace RI.Modules.RIStore_Services
    Public Class ClientHistoriesRepository
        Implements IClientHistoriesRepository

        Public Function AddClientHistory(clientHistory As Models.ClientHistory) As Models.ClientHistory Implements IClientHistoriesRepository.AddClientHistory
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientHistory) = ctx.GetRepository(Of Models.ClientHistory)()
                rep.Insert(clientHistory)
            End Using
            Return clientHistory
        End Function

        Public Function GetClientHistory(clientHistoryId As Integer, clientId As Integer) As Models.ClientHistory Implements IClientHistoriesRepository.GetClientHistory
            Dim clientHistory As Models.ClientHistory

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientHistory) = ctx.GetRepository(Of Models.ClientHistory)()
                clientHistory = rep.GetById(Of Int32, Int32)(clientHistoryId, clientId)
            End Using
            Return clientHistory
        End Function

        Public Function GetClientHistories(clientId As Integer) As IEnumerable(Of Models.ClientHistory) Implements IClientHistoriesRepository.GetClientHistories
            Dim clientHistorys As IEnumerable(Of Models.ClientHistory)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientHistory) = ctx.GetRepository(Of Models.ClientHistory)()
                clientHistorys = rep.Get(clientId)
            End Using
            Return clientHistorys
        End Function

        Public Sub RemoveClientHistory(clientHistoryId As Integer, clientId As Integer) Implements IClientHistoriesRepository.RemoveClientHistory
            Dim _item As Models.ClientHistory = GetClientHistory(clientHistoryId, clientId)
            If _item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveClientHistory(_item)
            End If
        End Sub

        Public Sub RemoveClientHistory(clientHistory As Models.ClientHistory) Implements IClientHistoriesRepository.RemoveClientHistory
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientHistory) = ctx.GetRepository(Of Models.ClientHistory)()
                rep.Delete(clientHistory)
            End Using
        End Sub

        Public Sub RemoveClientHistories(clientId As Integer) Implements IClientHistoriesRepository.RemoveClientHistories
            Dim clientHistorys As IEnumerable(Of Models.ClientHistory)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of Models.ClientHistory)()
                clientHistorys = rep.Find("Where ClientId = @0", clientId)

                For Each history In clientHistorys
                    RemoveClientHistory(history)
                Next
            End Using
        End Sub

        Public Sub UpdateClientHistory(clientHistory As Models.ClientHistory) Implements IClientHistoriesRepository.UpdateClientHistory
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientHistory) = ctx.GetRepository(Of Models.ClientHistory)()
                rep.Update(clientHistory)
            End Using
        End Sub

    End Class
End Namespace