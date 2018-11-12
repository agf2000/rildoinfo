
Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories

Namespace Components.Repositories

    Public Class EstimatesRepository
        Implements IEstimatesRepository

        '#Region "Private Methods"

        '    Private Shared Function GetNull(field As Object) As Object
        '        Return Null.GetNull(field, DBNull.Value)
        '    End Function

        '#End Region

        Public Function AddEstimate(estimate As Models.Estimate) As Models.Estimate Implements IEstimatesRepository.AddEstimate
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Estimate) = ctx.GetRepository(Of Models.Estimate)()
                rep.Insert(estimate)
            End Using
            Return estimate
        End Function

        Public Function AddEstimateItem(estimateItem As Models.EstimateItem) As Models.EstimateItem Implements IEstimatesRepository.AddEstimateItem
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateItem) = ctx.GetRepository(Of Models.EstimateItem)()
                rep.Insert(estimateItem)
            End Using
            Return estimateItem
        End Function

        Public Function GetEstimate(estimateId As Integer, portalId As Integer, getAll As Boolean) As Models.Estimate Implements IEstimatesRepository.GetEstimate
            Return CType(CBO.FillObject(DataProvider.Instance().GetEstimate(estimateId, portalId, getAll), GetType(Models.Estimate)), Models.Estimate)
        End Function

        Public Function GetEstimateDav(numDav As Integer, portalId As Integer) As Models.Estimate Implements IEstimatesRepository.GetEstimateDav
            Return CType(CBO.FillObject(DataProvider.Instance().GetEstimateDav(numDav, portalId), GetType(Models.Estimate)), Models.Estimate)
        End Function

        Public Function GetEstimates(portalId As Integer, personId As Integer, userId As Integer, salesRep As Integer, statusId As Integer, filterDates As String, sDate As DateTime, eDate As DateTime,
                                     filter As String, filterField As String, getAll As String, isDeleted As String, pageIndex As Integer, pageSize As Integer, orderBy As String, orderDesc As String) As IEnumerable(Of Models.Estimate) Implements IEstimatesRepository.GetEstimates
            Return CBO.FillCollection(Of Models.Estimate)(DataProvider.Instance().GetEstimates(portalId, personId, userId, salesRep, statusId, filterDates, sDate, eDate, filter,
                                                                                                          filterField, getAll, isDeleted, pageIndex, pageSize, orderBy, orderDesc))
        End Function

        Public Function GetDavs(portalId As Integer, personId As Integer, getAll As String, pageIndex As Integer, pageSize As Integer, orderBy As String,
                                orderDesc As String) As IEnumerable(Of Models.Estimate) Implements IEstimatesRepository.GetDavs
            Return CBO.FillCollection(Of Models.Estimate)(DataProvider.Instance().GetDavs(portalId, personId, getAll, pageIndex, pageSize, orderBy, orderDesc))
        End Function

        Public Function GetEstimateItems(portalId As Integer, estimateId As Integer, lang As String) As IEnumerable(Of Models.EstimateItem) Implements IEstimatesRepository.GetEstimateItems
            Dim result = CBO.FillCollection(Of Models.EstimateItem)(DataProvider.Instance().GetEstimateItems(portalId, estimateId, lang))
            Return result
        End Function

        Public Function GetEstimateItems(estimateId As Integer) As IEnumerable(Of Models.EstimateItem) Implements IEstimatesRepository.GetEstimateItems
            Dim estimateItems As IEnumerable(Of Models.EstimateItem)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of Models.EstimateItem)()
                estimateItems = rep.Find("Where EstimateId = @0 ", estimateId)
            End Using
            Return estimateItems
        End Function

        Public Sub RemoveEstimate(estimateId As Integer, portalId As Integer) Implements IEstimatesRepository.RemoveEstimate
            Dim item As Models.Estimate = GetEstimate(estimateId, portalId, True)
            RemoveEstimate(item)
        End Sub

        Public Sub RemoveEstimate(estimate As Models.Estimate) Implements IEstimatesRepository.RemoveEstimate
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Estimate) = ctx.GetRepository(Of Models.Estimate)()
                rep.Delete(estimate)
            End Using
        End Sub

        Public Sub RemoveClientEstimates(personId As Integer, portalId As Integer) Implements IEstimatesRepository.RemoveClientEstimates
            Dim estimates As IEnumerable(Of Models.Estimate)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of Models.Estimate)()
                estimates = rep.Find("Where PersonId = @0 And PortalId = @1 ", personId, portalId)

                For Each estimate In estimates
                    RemoveEstimate(estimate)
                Next
            End Using
        End Sub

        Public Sub UpdateEstimate(estimate As Models.Estimate) Implements IEstimatesRepository.UpdateEstimate
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Estimate) = ctx.GetRepository(Of Models.Estimate)()
                rep.Update(estimate)
            End Using
        End Sub

        Public Sub UpdateEstimateItem(estimateItem As Models.EstimateItem) Implements IEstimatesRepository.UpdateEstimateItem
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateItem) = ctx.GetRepository(Of Models.EstimateItem)()
                rep.Update(estimateItem)
            End Using
        End Sub

        Public Function GetEstimateItem(estimateItemId As Integer, estimateId As Integer) As Models.EstimateItem Implements IEstimatesRepository.GetEstimateItem
            Dim estimateItem As Models.EstimateItem

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateItem) = ctx.GetRepository(Of Models.EstimateItem)()
                estimateItem = rep.GetById(Of Int32, Int32)(estimateItemId, estimateId)
            End Using
            Return estimateItem
            'Return CType(CBO.FillObject(DataProvider.Instance().GetEstimateItem(estimateItemId, estimateId), GetType(Components.Models.EstimateItem)), Components.Models.EstimateItem)
        End Function

        Public Function GetEstimateItemDav(numDavItem As Integer) As Models.EstimateItem Implements IEstimatesRepository.GetEstimateItemDav
            Dim estimateItem As Models.EstimateItem
            Using context As IDataContext = DataContext.Instance()
                'Dim rep = context.GetRepository(Of Models.EstimateItem)()
                estimateItem = context.ExecuteSingleOrDefault(Of Models.EstimateItem)(CommandType.Text, "where numDavItem = @0 ", numDavItem)
            End Using
            Return estimateItem
        End Function

        Public Function GetEstimateItemByProduct(estimateId As Integer, productId As Integer) As Models.EstimateItem Implements IEstimatesRepository.GetEstimateItemByProduct
            Dim estimateItem As Models.EstimateItem

            Using ctx As IDataContext = DataContext.Instance()
                estimateItem = ctx.ExecuteSingleOrDefault(Of Models.EstimateItem)(CommandType.Text, "where estimateid = {0} and productId = @1", estimateId, productId)
            End Using
            Return estimateItem
        End Function

        Public Sub RemoveEstimateItem(estimateItemId As Integer, estimateId As Integer) Implements IEstimatesRepository.RemoveEstimateItem
            Dim item As Models.EstimateItem = GetEstimateItem(estimateItemId, estimateId)
            RemoveEstimateItem(item)
        End Sub

        Public Sub RemoveEstimateItem(estimateItem As Models.EstimateItem) Implements IEstimatesRepository.RemoveEstimateItem
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateItem) = ctx.GetRepository(Of Models.EstimateItem)()
                rep.Delete(estimateItem)
            End Using
        End Sub

        Public Function AddEstimateItemRemoved(estimateItem As Models.EstimateItemRemoved) As Models.EstimateItemRemoved Implements IEstimatesRepository.AddEstimateItemRemoved
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateItemRemoved) = ctx.GetRepository(Of Models.EstimateItemRemoved)()
                rep.Insert(estimateItem)
            End Using
            Return estimateItem
        End Function

        Public Function AddEstimateHistory(estimateHistory As Models.EstimateHistory) As Models.EstimateHistory Implements IEstimatesRepository.AddEstimateHistory
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateHistory) = ctx.GetRepository(Of Models.EstimateHistory)()
                rep.Insert(estimateHistory)
            End Using
            Return estimateHistory
        End Function

        Public Sub DeleteEstimate(estimate As Models.Estimate) Implements IEstimatesRepository.DeleteEstimate
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Estimate) = ctx.GetRepository(Of Models.Estimate)()
                rep.Update(estimate)
            End Using
        End Sub

        Public Function GetEstimateHistories(estimateId As Integer) As IEnumerable(Of Models.EstimateHistory) Implements IEstimatesRepository.GetEstimateHistories
            Return CBO.FillCollection(Of Models.EstimateHistory)(DataProvider.Instance().GetEstimateHistories(estimateId))
            'Dim estimateHistories As IEnumerable(Of Models.EstimateHistory)

            'Using ctx As IDataContext = DataContext.Instance()
            '    Dim rep As IRepository(Of Models.EstimateHistory) = ctx.GetRepository(Of Models.EstimateHistory)()
            '    estimateHistories = rep.Get(estimateId)
            'End Using
            'Return estimateHistories
        End Function

        Public Function AddEstimateMessage(estimateMessage As Models.EstimateMessage) As Models.EstimateMessage Implements IEstimatesRepository.AddEstimateMessage
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateMessage) = ctx.GetRepository(Of Models.EstimateMessage)()
                rep.Insert(estimateMessage)
            End Using
            Return estimateMessage
        End Function

        Public Function GetEstimateMessage(estimateMessageId As Integer, estimateId As Integer) As Models.EstimateMessage Implements IEstimatesRepository.GetEstimateMessage
            Return CType(CBO.FillObject(DataProvider.Instance().GetEstimateMessage(estimateMessageId, estimateId), GetType(Models.EstimateMessage)), Models.EstimateMessage)
        End Function

        Public Function GetEstimateMessages(estimateId As Integer) As IEnumerable(Of Models.EstimateMessage) Implements IEstimatesRepository.GetEstimateMessages
            Return CBO.FillCollection(Of Models.EstimateMessage)(DataProvider.Instance().GetEstimateMessages(estimateId))
        End Function

        Public Sub RemoveEstimateMessage(estimateMessageId As Integer, estimateId As Integer) Implements IEstimatesRepository.RemoveEstimateMessage
            Dim estimateMessage As Models.EstimateMessage = GetEstimateMessage(estimateMessageId, estimateId)
            RemoveEstimateMessage(estimateMessage)
        End Sub

        Public Sub RemoveEstimateMessage(estimateMessage As Models.EstimateMessage) Implements IEstimatesRepository.RemoveEstimateMessage
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateMessage) = ctx.GetRepository(Of Models.EstimateMessage)()
                rep.Delete(estimateMessage)
            End Using
        End Sub

        Public Sub UpdateEstimateMessage(estimateMessage As Models.EstimateMessage) Implements IEstimatesRepository.UpdateEstimateMessage
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateMessage) = ctx.GetRepository(Of Models.EstimateMessage)()
                rep.Update(estimateMessage)
            End Using
        End Sub

        Public Function AddEstimateMessageComment(estimateMessageComment As Models.EstimateMessageComment) As Models.EstimateMessageComment Implements IEstimatesRepository.AddEstimateMessageComment
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateMessageComment) = ctx.GetRepository(Of Models.EstimateMessageComment)()
                rep.Insert(estimateMessageComment)
            End Using
            Return estimateMessageComment
        End Function

        Public Function GetEstimateMessageComment(commentId As Integer, estimateMessageId As Integer) As Models.EstimateMessageComment Implements IEstimatesRepository.GetEstimateMessageComment
            Return CType(CBO.FillObject(DataProvider.Instance().GetEstimateMessageComment(commentId, estimateMessageId), GetType(Models.EstimateMessageComment)), Models.EstimateMessageComment)
        End Function

        Public Function GetEstimateMessageComments(estimateMessageId As Integer) As List(Of Models.EstimateMessageComment) Implements IEstimatesRepository.GetEstimateMessageComments
            Return CBO.FillCollection(Of Models.EstimateMessageComment)(DataProvider.Instance().GetEstimateMessageComments(estimateMessageId))
        End Function

        Public Sub RemoveEstimateMessageComment(estimateMessageComment As Models.EstimateMessageComment) Implements IEstimatesRepository.RemoveEstimateMessageComment
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateMessageComment) = ctx.GetRepository(Of Models.EstimateMessageComment)()
                rep.Delete(estimateMessageComment)
            End Using
        End Sub

        Public Sub RemoveEstimateMessageComment(commentId As Integer, estimateMessageId As Integer) Implements IEstimatesRepository.RemoveEstimateMessageComment
            Dim estimateMessageComment As Models.EstimateMessageComment = GetEstimateMessageComment(commentId, estimateMessageId)
            RemoveEstimateMessageComment(estimateMessageComment)
        End Sub

        Public Sub UpdateEstimateMessageComment(estimateMessageComment As Models.EstimateMessageComment) Implements IEstimatesRepository.UpdateEstimateMessageComment
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateMessageComment) = ctx.GetRepository(Of Models.EstimateMessageComment)()
                rep.Update(estimateMessageComment)
            End Using
        End Sub

        Public Function AddEstimateHistoryComment(estimateHistoryComment As Models.EstimateHistoryComment) As Models.EstimateHistoryComment Implements IEstimatesRepository.AddEstimateHistoryComment
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateHistoryComment) = ctx.GetRepository(Of Models.EstimateHistoryComment)()
                rep.Insert(estimateHistoryComment)
            End Using
            Return estimateHistoryComment
        End Function

        Public Function GetEstimateHistoryComment(commentId As Integer, estimateHistoryId As Integer) As Models.EstimateHistoryComment Implements IEstimatesRepository.GetEstimateHistoryComment
            Return CType(CBO.FillObject(DataProvider.Instance().GetEstimateHistoryComment(commentId, estimateHistoryId), GetType(Models.EstimateHistoryComment)), Models.EstimateHistoryComment)
        End Function

        Public Function GetEstimateHistoryComments(estimateHistoryId As Integer) As IEnumerable(Of Models.EstimateHistoryComment) Implements IEstimatesRepository.GetEstimateHistoryComments
            Return CBO.FillCollection(Of Models.EstimateHistoryComment)(DataProvider.Instance().GetEstimateHistoryComments(estimateHistoryId))
        End Function

        Public Sub RemoveEstimateHistoryComment(commentId As Integer, estimateHistoryId As Integer) Implements IEstimatesRepository.RemoveEstimateHistoryComment
            Dim estimateHistoryComment As Models.EstimateHistoryComment = GetEstimateHistoryComment(commentId, estimateHistoryId)
            RemoveEstimateHistoryComment(estimateHistoryComment)
        End Sub

        Public Sub RemoveEstimateHistoryComment(estimateHistoryComment As Models.EstimateHistoryComment) Implements IEstimatesRepository.RemoveEstimateHistoryComment
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateHistoryComment) = ctx.GetRepository(Of Models.EstimateHistoryComment)()
                rep.Delete(estimateHistoryComment)
            End Using
        End Sub

        Public Sub UpdateEstimateHistoryComment(estimateHistoryComment As Models.EstimateHistoryComment) Implements IEstimatesRepository.UpdateEstimateHistoryComment
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.EstimateHistoryComment) = ctx.GetRepository(Of Models.EstimateHistoryComment)()
                rep.Update(estimateHistoryComment)
            End Using
        End Sub

        Public Function GetEstimatesOpenedCount(portalId As Integer, userId As Integer) As Integer Implements IEstimatesRepository.GetEstimatesOpenedCount
            Dim estimatesCount As IEnumerable(Of Models.EstimateCount)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of Models.EstimateCount)()
                estimatesCount = rep.Find("where not statusid in (10,9) and portalid = @0 and ((not @1 > 0) or (salesrep = @1))", portalId, userId)
            End Using
            Return estimatesCount.Count
        End Function

        Public Function GetEstimatesSoldCount(portalId As Integer, userId As Integer) As Integer Implements IEstimatesRepository.GetEstimatesSoldCount
            Dim estimatesCount As IEnumerable(Of Models.EstimateCount)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of Models.EstimateCount)()
                estimatesCount = rep.Find("where statusid = (select statusid from riw_statuses where statustitle = 'Venda') and portalid = @0 and ((not @1 > 0) or (salesrep = @1))", portalId, userId)
            End Using
            Return estimatesCount.Count
        End Function
    End Class

End Namespace
