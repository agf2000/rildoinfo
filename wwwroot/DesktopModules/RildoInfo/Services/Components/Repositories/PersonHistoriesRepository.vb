
Public Class PersonHistoriesRepository
    Implements IPersonHistoriesRepository

    Public Function addPersonHistory(personHistory As Models.PersonHistory) As Models.PersonHistory Implements IPersonHistoriesRepository.addPersonHistory
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonHistory) = ctx.GetRepository(Of Models.PersonHistory)()
            rep.Insert(personHistory)
        End Using
        Return personHistory
    End Function

    Public Function getPersonHistory(personHistoryId As Integer, personId As Integer) As Models.PersonHistory Implements IPersonHistoriesRepository.getPersonHistory
        Dim personHistory As Models.PersonHistory

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonHistory) = ctx.GetRepository(Of Models.PersonHistory)()
            personHistory = rep.GetById(Of Int32, Int32)(personHistoryId, personId)
        End Using
        Return personHistory
    End Function

    Public Function getPersonHistories(personId As Integer) As IEnumerable(Of Models.PersonHistory) Implements IPersonHistoriesRepository.getPersonHistories
        Return CBO.FillCollection(Of Models.PersonHistory)(DataProvider.Instance().GetPersonHistories(personId))
        'Dim personHistorys As IEnumerable(Of Models.PersonHistory)

        'Using ctx As IDataContext = DataContext.Instance()
        '    Dim rep As IRepository(Of Models.PersonHistory) = ctx.GetRepository(Of Models.PersonHistory)()
        '    personHistorys = rep.Get(personId)
        'End Using
        'Return personHistorys
    End Function

    Public Sub removePersonHistory(personHistoryId As Integer, personId As Integer) Implements IPersonHistoriesRepository.removePersonHistory
        Dim _item As Models.PersonHistory = getPersonHistory(personHistoryId, personId)
        If _item.Locked Then
            Throw New ArgumentException("Exception Occured")
        Else
            removePersonHistory(_item)
        End If
    End Sub

    Public Sub removePersonHistory(personHistory As Models.PersonHistory) Implements IPersonHistoriesRepository.removePersonHistory
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonHistory) = ctx.GetRepository(Of Models.PersonHistory)()
            rep.Delete(personHistory)
        End Using
    End Sub

    Public Sub removePersonHistories(personId As Integer) Implements IPersonHistoriesRepository.removePersonHistories
        Dim personHistorys As IEnumerable(Of Models.PersonHistory)
        Using context As IDataContext = DataContext.Instance()
            Dim rep = context.GetRepository(Of Models.PersonHistory)()
            personHistorys = rep.Find("Where PersonId = @0", personId)

            For Each history In personHistorys
                removePersonHistory(history)
            Next
        End Using
    End Sub

    Public Sub updatePersonHistory(personHistory As Models.PersonHistory) Implements IPersonHistoriesRepository.updatePersonHistory
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonHistory) = ctx.GetRepository(Of Models.PersonHistory)()
            rep.Update(personHistory)
        End Using
    End Sub

    Public Function addPersonHistoryComment(personHistoryComment As Models.PersonHistoryComment) As Models.PersonHistoryComment Implements IPersonHistoriesRepository.addPersonHistoryComment
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonHistoryComment) = ctx.GetRepository(Of Models.PersonHistoryComment)()
            rep.Insert(personHistoryComment)
        End Using
        Return personHistoryComment
    End Function

    Public Function getPersonHistoryComment(commentId As Integer, personHistoryId As Integer) As Models.PersonHistoryComment Implements IPersonHistoriesRepository.getPersonHistoryComment
        Return CType(CBO.FillObject(DataProvider.Instance().GetPersonHistoryComment(commentId, personHistoryId), GetType(Models.PersonHistoryComment)), Models.PersonHistoryComment)
    End Function

    Public Function getPersonHistoryComments(personHistoryId As Integer) As IEnumerable(Of Models.PersonHistoryComment) Implements IPersonHistoriesRepository.getPersonHistoryComments
        Return CBO.FillCollection(Of Models.PersonHistoryComment)(DataProvider.Instance().GetPersonHistoryComments(personHistoryId))
    End Function

    Public Sub removePersonHistoryComment(commentId As Integer, personHistoryId As Integer) Implements IPersonHistoriesRepository.removePersonHistoryComment
        Dim personHistoryComment As Models.PersonHistoryComment = getPersonHistoryComment(commentId, personHistoryId)
        removePersonHistoryComment(personHistoryComment)
    End Sub

    Public Sub removePersonHistoryComment(personHistoryComment As Models.PersonHistoryComment) Implements IPersonHistoriesRepository.removePersonHistoryComment
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonHistoryComment) = ctx.GetRepository(Of Models.PersonHistoryComment)()
            rep.Delete(personHistoryComment)
        End Using
    End Sub

    Public Sub updatePersonHistoryComment(personHistoryComment As Models.PersonHistoryComment) Implements IPersonHistoriesRepository.updatePersonHistoryComment
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonHistoryComment) = ctx.GetRepository(Of Models.PersonHistoryComment)()
            rep.Update(personHistoryComment)
        End Using
    End Sub

End Class
