
Public Class ClientIncomeSourcesRepository
    Implements IClientIncomeSourcesRepository

    Public Function addClientIncomeSource(clientIncomeSource As Models.ClientIncomeSource) As Models.ClientIncomeSource Implements IClientIncomeSourcesRepository.AddClientIncomeSource
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientIncomeSource) = ctx.GetRepository(Of Models.ClientIncomeSource)()
            rep.Insert(clientIncomeSource)
        End Using
        Return clientIncomeSource
    End Function

    Public Function getClientIncomeSource(clientIncomeSourceId As Integer, personId As Integer) As Models.ClientIncomeSource Implements IClientIncomeSourcesRepository.GetClientIncomeSource
        Dim clientIncomeSource As Models.ClientIncomeSource

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientIncomeSource) = ctx.GetRepository(Of Models.ClientIncomeSource)()
            clientIncomeSource = rep.GetById(Of Int32, Int32)(clientIncomeSourceId, personId)
        End Using
        Return clientIncomeSource
    End Function

    Public Function getClientIncomeSources(personId As Integer) As IEnumerable(Of Models.ClientIncomeSource) Implements IClientIncomeSourcesRepository.GetClientIncomeSources
        Dim clientIncomeSources As IEnumerable(Of Models.ClientIncomeSource)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientIncomeSource) = ctx.GetRepository(Of Models.ClientIncomeSource)()
            clientIncomeSources = rep.Get(personId)
        End Using
        Return clientIncomeSources
    End Function

    Public Sub removeClientIncomeSource(clientIncomeSourceId As Integer, personId As Integer) Implements IClientIncomeSourcesRepository.RemoveClientIncomeSource
        Dim _item As Models.ClientIncomeSource = getClientIncomeSource(clientIncomeSourceId, personId)
        If _item.Locked Then
            Throw New ArgumentException("Exception Occured")
        Else
            removeClientIncomeSource(_item)
        End If
    End Sub

    Public Sub removeClientIncomeSource(clientIncomeSource As Models.ClientIncomeSource) Implements IClientIncomeSourcesRepository.RemoveClientIncomeSource
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientIncomeSource) = ctx.GetRepository(Of Models.ClientIncomeSource)()
            rep.Delete(clientIncomeSource)
        End Using
    End Sub

    Public Sub removeClientIncomeSources(personId As Integer) Implements IClientIncomeSourcesRepository.RemoveClientIncomeSources
        Dim clientIncomeSources As IEnumerable(Of Models.ClientIncomeSource)
        Using context As IDataContext = DataContext.Instance()
            Dim rep = context.GetRepository(Of Models.ClientIncomeSource)()
            clientIncomeSources = rep.Find("Where PersonId = @0", personId)

            For Each incomeSource In clientIncomeSources
                removeClientIncomeSource(incomeSource)
            Next
        End Using
    End Sub

    Public Sub updateClientIncomeSource(clientIncomeSource As Models.ClientIncomeSource) Implements IClientIncomeSourcesRepository.UpdateClientIncomeSource
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.ClientIncomeSource) = ctx.GetRepository(Of Models.ClientIncomeSource)()
            rep.Update(clientIncomeSource)
        End Using
    End Sub

End Class
