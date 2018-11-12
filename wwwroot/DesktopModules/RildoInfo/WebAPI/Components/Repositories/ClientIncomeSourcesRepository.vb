Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Repositories

    Public Class ClientIncomeSourcesRepository
    Implements IClientIncomeSourcesRepository

        Public Function AddClientIncomeSource(clientIncomeSource As ClientIncomeSource) As ClientIncomeSource Implements IClientIncomeSourcesRepository.AddClientIncomeSource
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientIncomeSource) = ctx.GetRepository(Of ClientIncomeSource)()
                rep.Insert(clientIncomeSource)
            End Using
            Return clientIncomeSource
        End Function

        Public Function GetClientIncomeSource(clientIncomeSourceId As Integer, personId As Integer) As ClientIncomeSource Implements IClientIncomeSourcesRepository.GetClientIncomeSource
            Dim clientIncomeSource As ClientIncomeSource

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientIncomeSource) = ctx.GetRepository(Of ClientIncomeSource)()
                clientIncomeSource = rep.GetById(Of Int32, Int32)(clientIncomeSourceId, personId)
            End Using
            Return clientIncomeSource
        End Function

        Public Function GetClientIncomeSources(personId As Integer) As IEnumerable(Of ClientIncomeSource) Implements IClientIncomeSourcesRepository.GetClientIncomeSources
            Dim clientIncomeSources As IEnumerable(Of ClientIncomeSource)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientIncomeSource) = ctx.GetRepository(Of ClientIncomeSource)()
                clientIncomeSources = rep.Get(personId)
            End Using
            Return clientIncomeSources
        End Function

        Public Sub RemoveClientIncomeSource1(clientIncomeSourceId As Integer, personId As Integer) Implements IClientIncomeSourcesRepository.RemoveClientIncomeSource1
            Dim item As ClientIncomeSource = GetClientIncomeSource(clientIncomeSourceId, personId)
            If item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveClientIncomeSource(item)
            End If
        End Sub

        Public Sub RemoveClientIncomeSource(clientIncomeSource As ClientIncomeSource) Implements IClientIncomeSourcesRepository.RemoveClientIncomeSource
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientIncomeSource) = ctx.GetRepository(Of ClientIncomeSource)()
                rep.Delete(clientIncomeSource)
            End Using
        End Sub

        Public Sub RemoveClientIncomeSources(personId As Integer) Implements IClientIncomeSourcesRepository.RemoveClientIncomeSources
            Dim clientIncomeSources As IEnumerable(Of ClientIncomeSource)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of ClientIncomeSource)()
                clientIncomeSources = rep.Find("Where PersonId = @0", personId)

                For Each incomeSource In clientIncomeSources
                    RemoveClientIncomeSource(incomeSource)
                Next
            End Using
        End Sub

        Public Sub UpdateClientIncomeSource(clientIncomeSource As ClientIncomeSource) Implements IClientIncomeSourcesRepository.UpdateClientIncomeSource
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientIncomeSource) = ctx.GetRepository(Of ClientIncomeSource)()
                rep.Update(clientIncomeSource)
            End Using
        End Sub

    End Class

End Namespace
