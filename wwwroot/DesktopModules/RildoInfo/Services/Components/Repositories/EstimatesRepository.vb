
Public Class EstimatesRepository
    Implements IEstimatesRepository

#Region "Private Methods"

    Private Shared Function GetNull(field As Object) As Object
        Return Null.GetNull(field, DBNull.Value)
    End Function

#End Region

    Public Function AddEstimate(estimate As Models.Estimate) As Models.Estimate Implements IEstimatesRepository.addEstimate
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Estimate) = ctx.GetRepository(Of Models.Estimate)()
            rep.Insert(estimate)
        End Using
        Return estimate
    End Function

    Public Function AddEstimateItem(estimateItem As Models.EstimateItem) As Models.EstimateItem Implements IEstimatesRepository.addEstimateItem
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.EstimateItem) = ctx.GetRepository(Of Models.EstimateItem)()
            rep.Insert(estimateItem)
        End Using
        Return estimateItem
    End Function

    Public Function GetEstimate(estimateId As Integer, portalId As Integer) As Models.Estimate Implements IEstimatesRepository.getEstimate
        Return CType(CBO.FillObject(DataProvider.Instance().GetEstimate(estimateId, portalId), GetType(Models.Estimate)), Models.Estimate)
    End Function

    Public Function GetEstimates(portalId As Integer, personId As Integer, salesRep As Integer, statusId As Integer, sDate As DateTime, eDate As DateTime, filter As String, isDeleted As String, pageIndex As Integer, pageSize As Integer, orderBy As String) As IEnumerable(Of Models.Estimate) Implements IEstimatesRepository.getEstimates
        Return CBO.FillCollection(Of Models.Estimate)(DataProvider.Instance().GetEstimates(portalId, personId, salesRep, statusId, sDate, eDate, filter, isDeleted, pageIndex, pageSize, orderBy))
    End Function

    Public Function GetEstimateItems(portalId As Integer, estimateId As Integer, lang As String) As IEnumerable(Of Models.EstimateItem) Implements IEstimatesRepository.getEstimateItems
        Return CBO.FillCollection(Of Models.EstimateItem)(DataProvider.Instance().GetEstimateItems(portalId, estimateId, lang))
    End Function

    Public Sub RemoveEstimate(estimateId As Integer, portalId As Integer) Implements IEstimatesRepository.removeEstimate
        Dim _item As Models.Estimate = GetEstimate(estimateId, portalId)
        RemoveEstimate(_item)
    End Sub

    Public Sub RemoveEstimate(estimate As Models.Estimate) Implements IEstimatesRepository.removeEstimate
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Estimate) = ctx.GetRepository(Of Models.Estimate)()
            rep.Delete(estimate)
        End Using
    End Sub

    Public Sub RemoveClientEstimates(personId As Integer, portalId As Integer) Implements IEstimatesRepository.removeClientEstimates
        Dim estimates As IEnumerable(Of Models.Estimate)
        Using context As IDataContext = DataContext.Instance()
            Dim rep = context.GetRepository(Of Models.Estimate)()
            estimates = rep.Find("Where PersonId = @0 And PortalId = @1 ", personId, portalId)

            For Each estimate In estimates
                RemoveEstimate(estimate)
            Next
        End Using
    End Sub

    Public Sub updateEstimate(estimate As Models.Estimate) Implements IEstimatesRepository.updateEstimate
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Estimate) = ctx.GetRepository(Of Models.Estimate)()
            rep.Update(estimate)
        End Using
    End Sub

    Public Sub updateEstimateItem(estimateItem As Models.EstimateItem) Implements IEstimatesRepository.updateEstimateItem
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.EstimateItem) = ctx.GetRepository(Of Models.EstimateItem)()
            rep.Update(estimateItem)
        End Using
    End Sub

    Public Function GetEstimateItem(estimateItemId As Integer, estimateId As Integer) As Models.EstimateItem Implements IEstimatesRepository.getEstimateItem
        Dim estimateItem As Models.EstimateItem

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.EstimateItem) = ctx.GetRepository(Of Models.EstimateItem)()
            estimateItem = rep.GetById(Of Int32, Int32)(estimateItemId, estimateId)
        End Using
        Return estimateItem
    End Function

    Public Sub RemoveEstimateItem(estimateItemId As Integer, estimateId As Integer) Implements IEstimatesRepository.removeEstimateItem
        Dim _item As Models.EstimateItem = GetEstimateItem(estimateItemId, estimateId)
        RemoveEstimateItem(_item)
    End Sub

    Public Sub RemoveEstimateItem(estimateItem As Models.EstimateItem) Implements IEstimatesRepository.removeEstimateItem
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.EstimateItem) = ctx.GetRepository(Of Models.EstimateItem)()
            rep.Delete(estimateItem)
        End Using
    End Sub

    Public Function AddEstimateItemRemoved(estimateItem As Models.EstimateItemRemoved) As Models.EstimateItemRemoved Implements IEstimatesRepository.addEstimateItemRemoved
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.EstimateItemRemoved) = ctx.GetRepository(Of Models.EstimateItemRemoved)()
            rep.Insert(estimateItem)
        End Using
        Return estimateItem
    End Function

    Public Function AddEstimateHistory(estimateHistory As Models.EstimateHistory) As Models.EstimateHistory Implements IEstimatesRepository.addEstimateHistory
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.EstimateHistory) = ctx.GetRepository(Of Models.EstimateHistory)()
            rep.Insert(estimateHistory)
        End Using
        Return estimateHistory
    End Function

    Public Sub deleteEstimate(estimate As Models.Estimate) Implements IEstimatesRepository.deleteEstimate
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Estimate) = ctx.GetRepository(Of Models.Estimate)()
            rep.Update(estimate)
        End Using
    End Sub

    Public Function GetEstimateHistories(estimateId As Integer) As IEnumerable(Of Models.EstimateHistory) Implements IEstimatesRepository.getEstimateHistories
        Return CBO.FillCollection(Of Models.EstimateHistory)(DataProvider.Instance().GetEstimateHistories(estimateId))
        'Dim estimateHistories As IEnumerable(Of Models.EstimateHistory)

        'Using ctx As IDataContext = DataContext.Instance()
        '    Dim rep As IRepository(Of Models.EstimateHistory) = ctx.GetRepository(Of Models.EstimateHistory)()
        '    estimateHistories = rep.Get(estimateId)
        'End Using
        'Return estimateHistories
    End Function

    Public Function addEstimateMessage(estimateMessage As Models.EstimateMessage) As Models.EstimateMessage Implements IEstimatesRepository.addEstimateMessage
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.EstimateMessage) = ctx.GetRepository(Of Models.EstimateMessage)()
            rep.Insert(estimateMessage)
        End Using
        Return estimateMessage
    End Function

    Public Function GetEstimateMessage(estimateMessageId As Integer, estimateId As Integer) As Models.EstimateMessage Implements IEstimatesRepository.getEstimateMessage
        Return CType(CBO.FillObject(DataProvider.Instance().GetEstimateMessage(estimateMessageId, estimateId), GetType(Models.EstimateMessage)), Models.EstimateMessage)
    End Function

    Public Function GetEstimateMessages(estimateId As Integer) As IEnumerable(Of Models.EstimateMessage) Implements IEstimatesRepository.getEstimateMessages
        Return CBO.FillCollection(Of Models.EstimateMessage)(DataProvider.Instance().GetEstimateMessages(estimateId))
    End Function

    Public Sub removeEstimateMessage(estimateMessageId As Integer, estimateId As Integer) Implements IEstimatesRepository.removeEstimateMessage
        Dim estimateMessage As Models.EstimateMessage = GetEstimateMessage(estimateMessageId, estimateId)
        removeEstimateMessage(estimateMessage)
    End Sub

    Public Sub removeEstimateMessage(estimateMessage As Models.EstimateMessage) Implements IEstimatesRepository.removeEstimateMessage
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.EstimateMessage) = ctx.GetRepository(Of Models.EstimateMessage)()
            rep.Delete(estimateMessage)
        End Using
    End Sub

    Public Sub updateEstimateMessage(estimateMessage As Models.EstimateMessage) Implements IEstimatesRepository.updateEstimateMessage
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.EstimateMessage) = ctx.GetRepository(Of Models.EstimateMessage)()
            rep.Update(estimateMessage)
        End Using
    End Sub

    Public Function addEstimateMessageComment(estimateMessageComment As Models.EstimateMessageComment) As Models.EstimateMessageComment Implements IEstimatesRepository.addEstimateMessageComment
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.EstimateMessageComment) = ctx.GetRepository(Of Models.EstimateMessageComment)()
            rep.Insert(estimateMessageComment)
        End Using
        Return estimateMessageComment
    End Function

    Public Function getEstimateMessageComment(commentId As Integer, estimateMessageId As Integer) As Models.EstimateMessageComment Implements IEstimatesRepository.getEstimateMessageComment
        Return CType(CBO.FillObject(DataProvider.Instance().GetEstimateMessageComment(commentId, estimateMessageId), GetType(Models.EstimateMessageComment)), Models.EstimateMessageComment)
    End Function

    Public Function getEstimateMessageComments(estimateMessageId As Integer) As List(Of Models.EstimateMessageComment) Implements IEstimatesRepository.getEstimateMessageComments
        Return CBO.FillCollection(Of Models.EstimateMessageComment)(DataProvider.Instance().GetEstimateMessageComments(estimateMessageId))
    End Function

    Public Sub removeEstimateMessageComment(estimateMessageComment As Models.EstimateMessageComment) Implements IEstimatesRepository.removeEstimateMessageComment
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.EstimateMessageComment) = ctx.GetRepository(Of Models.EstimateMessageComment)()
            rep.Delete(estimateMessageComment)
        End Using
    End Sub

    Public Sub removeEstimateMessageComment(commentId As Integer, estimateMessageId As Integer) Implements IEstimatesRepository.removeEstimateMessageComment
        Dim estimateMessageComment As Models.EstimateMessageComment = getEstimateMessageComment(commentId, estimateMessageId)
        removeEstimateMessageComment(estimateMessageComment)
    End Sub

    Public Sub updateEstimateMessageComment(estimateMessageComment As Models.EstimateMessageComment) Implements IEstimatesRepository.updateEstimateMessageComment
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.EstimateMessageComment) = ctx.GetRepository(Of Models.EstimateMessageComment)()
            rep.Update(estimateMessageComment)
        End Using
    End Sub

    Public Function addEstimateHistoryComment(estimateHistoryComment As Models.EstimateHistoryComment) As Models.EstimateHistoryComment Implements IEstimatesRepository.addEstimateHistoryComment
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.EstimateHistoryComment) = ctx.GetRepository(Of Models.EstimateHistoryComment)()
            rep.Insert(estimateHistoryComment)
        End Using
        Return estimateHistoryComment
    End Function

    Public Function getEstimateHistoryComment(commentId As Integer, estimateHistoryId As Integer) As Models.EstimateHistoryComment Implements IEstimatesRepository.getEstimateHistoryComment
        Return CType(CBO.FillObject(DataProvider.Instance().GetEstimateHistoryComment(commentId, estimateHistoryId), GetType(Models.EstimateHistoryComment)), Models.EstimateHistoryComment)
    End Function

    Public Function getEstimateHistoryComments(estimateHistoryId As Integer) As IEnumerable(Of Models.EstimateHistoryComment) Implements IEstimatesRepository.getEstimateHistoryComments
        Return CBO.FillCollection(Of Models.EstimateHistoryComment)(DataProvider.Instance().GetEstimateHistoryComments(estimateHistoryId))
    End Function

    Public Sub removeEstimateHistoryComment(commentId As Integer, estimateHistoryId As Integer) Implements IEstimatesRepository.removeEstimateHistoryComment
        Dim estimateHistoryComment As Models.EstimateHistoryComment = getEstimateHistoryComment(commentId, estimateHistoryId)
        removeEstimateHistoryComment(estimateHistoryComment)
    End Sub

    Public Sub removeEstimateHistoryComment(estimateHistoryComment As Models.EstimateHistoryComment) Implements IEstimatesRepository.removeEstimateHistoryComment
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.EstimateHistoryComment) = ctx.GetRepository(Of Models.EstimateHistoryComment)()
            rep.Delete(estimateHistoryComment)
        End Using
    End Sub

    Public Sub updateEstimateHistoryComment(estimateHistoryComment As Models.EstimateHistoryComment) Implements IEstimatesRepository.updateEstimateHistoryComment
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.EstimateHistoryComment) = ctx.GetRepository(Of Models.EstimateHistoryComment)()
            rep.Update(estimateHistoryComment)
        End Using
    End Sub

End Class
