Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Repositories

    Public Class PersonHistoriesRepository
    Implements IPersonHistoriesRepository

        Public Function AddPersonHistory(personHistory As PersonHistory) As PersonHistory Implements IPersonHistoriesRepository.AddPersonHistory
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonHistory) = ctx.GetRepository(Of PersonHistory)()
                rep.Insert(personHistory)
            End Using
            Return personHistory
        End Function

        Public Function GetPersonHistory(personHistoryId As Integer, personId As Integer) As PersonHistory Implements IPersonHistoriesRepository.GetPersonHistory
            Dim personHistory As PersonHistory

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonHistory) = ctx.GetRepository(Of PersonHistory)()
                personHistory = rep.GetById(Of Int32, Int32)(personHistoryId, personId)
            End Using
            Return personHistory
        End Function

        Public Function GetPersonHistories(personId As Integer) As IEnumerable(Of PersonHistory) Implements IPersonHistoriesRepository.GetPersonHistories
            Return CBO.FillCollection(Of PersonHistory)(DataProvider.Instance().GetPersonHistories(personId))
            'Dim personHistorys As IEnumerable(Of Models.PersonHistory)

            'Using ctx As IDataContext = DataContext.Instance()
            '    Dim rep As IRepository(Of Models.PersonHistory) = ctx.GetRepository(Of Models.PersonHistory)()
            '    personHistorys = rep.Get(personId)
            'End Using
            'Return personHistorys
        End Function

        Public Sub RemovePersonHistory(personHistoryId As Integer, personId As Integer) Implements IPersonHistoriesRepository.RemovePersonHistory
            Dim item As PersonHistory = GetPersonHistory(personHistoryId, personId)
            If item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemovePersonHistory(item)
            End If
        End Sub

        Public Sub RemovePersonHistory(personHistory As PersonHistory) Implements IPersonHistoriesRepository.RemovePersonHistory
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonHistory) = ctx.GetRepository(Of PersonHistory)()
                rep.Delete(personHistory)
            End Using
        End Sub

        Public Sub RemovePersonHistories(personId As Integer) Implements IPersonHistoriesRepository.RemovePersonHistories
            Dim personHistorys As IEnumerable(Of PersonHistory)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of PersonHistory)()
                personHistorys = rep.Find("Where PersonId = @0", personId)

                For Each history In personHistorys
                    RemovePersonHistory(history)
                Next
            End Using
        End Sub

        Public Sub UpdatePersonHistory(personHistory As PersonHistory) Implements IPersonHistoriesRepository.UpdatePersonHistory
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonHistory) = ctx.GetRepository(Of PersonHistory)()
                rep.Update(personHistory)
            End Using
        End Sub

        Public Function AddPersonHistoryComment(personHistoryComment As PersonHistoryComment) As PersonHistoryComment Implements IPersonHistoriesRepository.AddPersonHistoryComment
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonHistoryComment) = ctx.GetRepository(Of PersonHistoryComment)()
                rep.Insert(personHistoryComment)
            End Using
            Return personHistoryComment
        End Function

        Public Function GetPersonHistoryComment(commentId As Integer, personHistoryId As Integer) As PersonHistoryComment Implements IPersonHistoriesRepository.GetPersonHistoryComment
            Return CType(CBO.FillObject(DataProvider.Instance().GetPersonHistoryComment(commentId, personHistoryId), GetType(PersonHistoryComment)), PersonHistoryComment)
        End Function

        Public Function GetPersonHistoryComments(personHistoryId As Integer) As IEnumerable(Of PersonHistoryComment) Implements IPersonHistoriesRepository.GetPersonHistoryComments
            Return CBO.FillCollection(Of PersonHistoryComment)(DataProvider.Instance().GetPersonHistoryComments(personHistoryId))
        End Function

        Public Sub RemovePersonHistoryComment(commentId As Integer, personHistoryId As Integer) Implements IPersonHistoriesRepository.RemovePersonHistoryComment
            Dim personHistoryComment As PersonHistoryComment = GetPersonHistoryComment(commentId, personHistoryId)
            RemovePersonHistoryComment(personHistoryComment)
        End Sub

        Public Sub RemovePersonHistoryComment(personHistoryComment As PersonHistoryComment) Implements IPersonHistoriesRepository.RemovePersonHistoryComment
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonHistoryComment) = ctx.GetRepository(Of PersonHistoryComment)()
                rep.Delete(personHistoryComment)
            End Using
        End Sub

        Public Sub UpdatePersonHistoryComment(personHistoryComment As PersonHistoryComment) Implements IPersonHistoriesRepository.UpdatePersonHistoryComment
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonHistoryComment) = ctx.GetRepository(Of PersonHistoryComment)()
                rep.Update(personHistoryComment)
            End Using
        End Sub

    End Class

End Namespace
