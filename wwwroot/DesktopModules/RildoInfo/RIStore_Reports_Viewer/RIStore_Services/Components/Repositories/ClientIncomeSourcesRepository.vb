
Imports System.Collections.Generic
Imports DotNetNuke.Data

Namespace RI.Modules.RIStore_Services
    Public Class ClientIncomeSourcesRepository
        Implements IClientIncomeSourcesRepository

        Public Function AddClientIncomeSource(clientIncomeSource As Models.ClientIncomeSource) As Models.ClientIncomeSource Implements IClientIncomeSourcesRepository.AddClientIncomeSource
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientIncomeSource) = ctx.GetRepository(Of Models.ClientIncomeSource)()
                rep.Insert(clientIncomeSource)
            End Using
            Return clientIncomeSource
        End Function

        Public Function GetClientIncomeSource(clientIncomeSourceId As Integer, clientId As Integer) As Models.ClientIncomeSource Implements IClientIncomeSourcesRepository.GetClientIncomeSource
            Dim clientIncomeSource As Models.ClientIncomeSource

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientIncomeSource) = ctx.GetRepository(Of Models.ClientIncomeSource)()
                clientIncomeSource = rep.GetById(Of Int32, Int32)(clientIncomeSourceId, clientId)
            End Using
            Return clientIncomeSource
        End Function

        Public Function GetClientIncomeSources(clientId As Integer) As IEnumerable(Of Models.ClientIncomeSource) Implements IClientIncomeSourcesRepository.GetClientIncomeSources
            Dim clientIncomeSources As IEnumerable(Of Models.ClientIncomeSource)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientIncomeSource) = ctx.GetRepository(Of Models.ClientIncomeSource)()
                clientIncomeSources = rep.Get(clientId)
            End Using
            Return clientIncomeSources
        End Function

        Public Sub RemoveClientIncomeSource(clientIncomeSourceId As Integer, clientId As Integer) Implements IClientIncomeSourcesRepository.RemoveClientIncomeSource
            Dim _item As Models.ClientIncomeSource = GetClientIncomeSource(clientIncomeSourceId, clientId)
            If _item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveClientIncomeSource(_item)
            End If
        End Sub

        Public Sub RemoveClientIncomeSource(clientIncomeSource As Models.ClientIncomeSource) Implements IClientIncomeSourcesRepository.RemoveClientIncomeSource
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientIncomeSource) = ctx.GetRepository(Of Models.ClientIncomeSource)()
                rep.Delete(clientIncomeSource)
            End Using
        End Sub

        Public Sub RemoveClientIncomeSources(clientId As Integer) Implements IClientIncomeSourcesRepository.RemoveClientIncomeSources
            Dim clientIncomeSources As IEnumerable(Of Models.ClientIncomeSource)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of Models.ClientIncomeSource)()
                clientIncomeSources = rep.Find("Where ClientId = @0", clientId)

                For Each incomeSource In clientIncomeSources
                    RemoveClientIncomeSource(incomeSource)
                Next
            End Using
        End Sub

        Public Sub UpdateClientIncomeSource(clientIncomeSource As Models.ClientIncomeSource) Implements IClientIncomeSourcesRepository.UpdateClientIncomeSource
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientIncomeSource) = ctx.GetRepository(Of Models.ClientIncomeSource)()
                rep.Update(clientIncomeSource)
            End Using
        End Sub

    End Class
End Namespace