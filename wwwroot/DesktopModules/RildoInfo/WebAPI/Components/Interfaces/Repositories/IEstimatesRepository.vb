Namespace Components.Interfaces.Repositories

    Public Interface IEstimatesRepository

        ''' <summary>
        ''' Gets a list of estimates
        ''' </summary>
        ''' <param name="portalId"></param>
        ''' <param name="personId"></param>
        ''' <param name="userId">User ID</param>
        ''' <param name="salesRep"></param>
        ''' <param name="statusId"></param>
        ''' <param name="sDate"></param>
        ''' <param name="eDate"></param>
        ''' <param name="filter"></param>
        ''' <param name="filterField">Filter Field</param>
        ''' <param name="getAll">Get Them All</param>
        ''' <param name="isDeleted"></param>
        ''' <param name="pageIndex"></param>
        ''' <param name="pageSize"></param>
        ''' <param name="orderBy"></param>
        ''' <param name="orderDesc">Order Descent</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimates(portalId As Integer, personId As Integer, userId As Integer, salesRep As Integer, statusId As Integer, filterDates As String, sDate As DateTime,
                              eDate As DateTime, filter As String, filterField As String, getAll As String, isDeleted As String, pageIndex As Integer,
                              pageSize As Integer, orderBy As String, orderDesc As String) As IEnumerable(Of Components.Models.Estimate)

        ''' <summary>
        ''' Gets a list of estimates
        ''' </summary>
        ''' <param name="portalId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetDavs(portalId As Integer, personId As Integer, getAll As String, pageIndex As Integer, pageSize As Integer,
                                         orderBy As String, orderDesc As String) As IEnumerable(Of Components.Models.Estimate)

        ''' <summary>
        ''' Gets a list of estimate items by estimate id
        ''' </summary>
        ''' <param name="portalId"></param>
        ''' <param name="estimateId">Estimate ID</param>
        ''' <param name="lang"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimateItems(portalId As Integer, estimateId As Integer, lang As String) As IEnumerable(Of Components.Models.EstimateItem)

        ''' <summary>
        ''' Gets a list of estimate items by estimate id
        ''' </summary>
        ''' <param name="estimateId">Estimate ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimateItems(estimateId As Integer) As IEnumerable(Of Components.Models.EstimateItem)

        ''' <summary>
        ''' Gets an estimate by id
        ''' </summary>
        ''' <param name="estimateId">Estimate ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimate(estimateId As Integer, portalId As Integer, getAll As Boolean) As Components.Models.Estimate

        ''' <summary>
        ''' Gets an estimate by id
        ''' </summary>
        ''' <param name="numDav">Estimate ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimateDav(numDav As Integer, portalId As Integer) As Components.Models.Estimate

        ''' <summary>
        ''' Adds an estimate
        ''' </summary>
        ''' <param name="estimate">Estimate Model</param>
        ''' <remarks></remarks>
        Function AddEstimate(estimate As Components.Models.Estimate) As Components.Models.Estimate

        ''' <summary>
        ''' Updates an estimate
        ''' </summary>
        ''' <param name="estimate">Estimate Model</param>
        ''' <remarks></remarks>
        Sub UpdateEstimate(estimate As Components.Models.Estimate)

        ''' <summary>
        ''' Adds an estimate
        ''' </summary>
        ''' <param name="estimateItem">Estimate Item Model</param>
        ''' <remarks></remarks>
        Function AddEstimateItem(estimateItem As Components.Models.EstimateItem) As Components.Models.EstimateItem

        ''' <summary>
        ''' Updates an estimate
        ''' </summary>
        ''' <param name="estimateItem">Estimate Model</param>
        ''' <remarks></remarks>
        Sub UpdateEstimateItem(estimateItem As Components.Models.EstimateItem)

        ''' <summary>
        ''' Removes estimate by estimate id and client id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="estimateId">Estimate ID</param>
        ''' <remarks></remarks>
        Sub RemoveEstimate(estimateId As Integer, portalId As Integer)

        ''' <summary>
        ''' Removes all client estimates
        ''' </summary>
        ''' <param name="personId">Client ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientEstimates(personId As Integer, portalId As Integer)

        ''' <summary>
        ''' Removes an estimate
        ''' </summary>
        ''' <param name="Estimate">Estimate Model</param>
        ''' <remarks></remarks>
        Sub RemoveEstimate(estimate As Components.Models.Estimate)

        ''' <summary>
        ''' Adds an estimate
        ''' </summary>
        ''' <param name="estimateItem">Estimate Item Removed Model</param>
        ''' <remarks></remarks>
        Function AddEstimateItemRemoved(estimateItem As Components.Models.EstimateItemRemoved) As Components.Models.EstimateItemRemoved

        ''' <summary>
        ''' Gets an estimate item by id
        ''' </summary>
        ''' <param name="estimateItemId">Estimate Item ID</param>
        ''' <param name="estimateId">Estimate ID</param>
        ''' <param name="lang">Language</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimateItem(estimateItemId As Integer, estimateId As Integer) As Components.Models.EstimateItem

        ''' <summary>
        ''' Gets an estimate item by dav
        ''' </summary>
        ''' <param name="numDavItem"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimateItemDav(numDavItem As Integer) As Components.Models.EstimateItem

        ''' <summary>
        ''' Gets an estimate item by product id
        ''' </summary>
        ''' <param name="estimateId">Estimate Item ID</param>
        ''' <param name="productId">Estimate ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimateItemByProduct(estimateId As Integer, productId As Integer) As Components.Models.EstimateItem

        ''' <summary>
        ''' Removes estimate by estimate id and client id
        ''' </summary>
        ''' <param name="estimateItemId">Estimate Item ID</param>
        ''' <param name="estimateId">Estimate ID</param>
        ''' <remarks></remarks>
        Sub RemoveEstimateItem(estimateItemId As Integer, estimateId As Integer)

        ''' <summary>
        ''' Removes an estimate item
        ''' </summary>
        ''' <param name="estimateItem">Estimate Item Model</param>
        ''' <remarks></remarks>
        Sub RemoveEstimateItem(estimateItem As Components.Models.EstimateItem)

        ''' <summary>
        ''' Adds an estimate hsitory
        ''' </summary>
        ''' <param name="estimateHistory">Estimate History Model</param>
        ''' <remarks></remarks>
        Function AddEstimateHistory(estimateHistory As Components.Models.EstimateHistory) As Components.Models.EstimateHistory

        ''' <summary>
        ''' Deletes an estimate
        ''' </summary>
        ''' <param name="estimate"></param>
        ''' <remarks></remarks>
        Sub DeleteEstimate(estimate As Components.Models.Estimate)

        ''' <summary>
        ''' Gets estimate histories
        ''' </summary>
        ''' <param name="estimateId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimateHistories(estimateId As Integer) As IEnumerable(Of Components.Models.EstimateHistory)

        ''' <summary>
        ''' Gets list of estimate messages
        ''' </summary>
        ''' <param name="estimateId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimateMessages(estimateId As Integer) As IEnumerable(Of Components.Models.EstimateMessage)

        ''' <summary>
        ''' Gets an estimate message
        ''' </summary>
        ''' <param name="estimateMessageId"></param>
        ''' <param name="estimateId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimateMessage(estimateMessageId As Integer, estimateId As Integer) As Components.Models.EstimateMessage

        ''' <summary>
        ''' Adds an estimate message
        ''' </summary>
        ''' <param name="estimateMessage"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddEstimateMessage(estimateMessage As Components.Models.EstimateMessage) As Components.Models.EstimateMessage

        ''' <summary>
        ''' Updates an estimate message
        ''' </summary>
        ''' <param name="estimateMessage"></param>
        ''' <remarks></remarks>
        Sub UpdateEstimateMessage(estimateMessage As Components.Models.EstimateMessage)

        ''' <summary>
        ''' Removes an estimate message
        ''' </summary>
        ''' <param name="estimateMessageId"></param>
        ''' <param name="estimateId"></param>
        ''' <remarks></remarks>
        Sub RemoveEstimateMessage(estimateMessageId As Integer, estimateId As Integer)

        ''' <summary>
        ''' Removes an estimate message
        ''' </summary>
        ''' <param name="estimateMessage"></param>
        ''' <remarks></remarks>
        Sub RemoveEstimateMessage(estimateMessage As Components.Models.EstimateMessage)

        ''' <summary>
        ''' Gets list of estimate messages
        ''' </summary>
        ''' <param name="estimateMessageId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimateMessageComments(estimateMessageId As Integer) As List(Of Components.Models.EstimateMessageComment)

        ''' <summary>
        ''' Gets estimate history comments
        ''' </summary>
        ''' <param name="estimateHistoryId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimateHistoryComments(estimateHistoryId As Integer) As IEnumerable(Of Components.Models.EstimateHistoryComment)

        ''' <summary>
        ''' Adds an estimate message comment
        ''' </summary>
        ''' <param name="estimateMessageComment"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddEstimateMessageComment(estimateMessageComment As Components.Models.EstimateMessageComment) As Components.Models.EstimateMessageComment

        ''' <summary>
        ''' Adds an estimate history comment
        ''' </summary>
        ''' <param name="estimateHistoryComment"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddEstimateHistoryComment(estimateHistoryComment As Components.Models.EstimateHistoryComment) As Components.Models.EstimateHistoryComment

        ''' <summary>
        ''' Gets an estimate message
        ''' </summary>
        ''' <param name="commentId"></param>
        ''' <param name="estimateMessageId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimateMessageComment(commentId As Integer, estimateMessageId As Integer) As Components.Models.EstimateMessageComment

        ''' <summary>
        ''' Gets an estimate message comment
        ''' </summary>
        ''' <param name="commentId"></param>
        ''' <param name="estimateHistoryId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimateHistoryComment(commentId As Integer, estimateHistoryId As Integer) As Components.Models.EstimateHistoryComment

        ''' <summary>
        ''' Updates an estimate message comment
        ''' </summary>
        ''' <param name="estimateMessageComment"></param>
        ''' <remarks></remarks>
        Sub UpdateEstimateMessageComment(estimateMessageComment As Components.Models.EstimateMessageComment)

        ''' <summary>
        ''' Updates an estimate history comment
        ''' </summary>
        ''' <param name="estimateHistoryComment"></param>
        ''' <remarks></remarks>
        Sub UpdateEstimateHistoryComment(estimateHistoryComment As Components.Models.EstimateHistoryComment)

        ''' <summary>
        ''' Removes an estimate message comment
        ''' </summary>
        ''' <param name="commentId"></param>
        ''' <param name="estimateMessageId"></param>
        ''' <remarks></remarks>
        Sub RemoveEstimateMessageComment(commentId As Integer, estimateMessageId As Integer)

        ''' <summary>
        ''' Removes an estimate history comment
        ''' </summary>
        ''' <param name="commentId"></param>
        ''' <param name="estimateHistoryId"></param>
        ''' <remarks></remarks>
        Sub RemoveEstimateHistoryComment(commentId As Integer, estimateHistoryId As Integer)

        ''' <summary>
        ''' Removes an estimate message comement
        ''' </summary>
        ''' <param name="estimateMessageComment"></param>
        ''' <remarks></remarks>
        Sub RemoveEstimateMessageComment(estimateMessageComment As Components.Models.EstimateMessageComment)

        ''' <summary>
        ''' Removes an estimate history comment
        ''' </summary>
        ''' <param name="estimateHistoryComment"></param>
        ''' <remarks></remarks>
        Sub RemoveEstimateHistoryComment(estimateHistoryComment As Components.Models.EstimateHistoryComment)

        ''' <summary>
        ''' Gets opened estimates count
        ''' </summary>
        ''' <param name="portalId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimatesOpenedCount(portalId As Integer, userId As Integer) As Integer

        ''' <summary>
        ''' Gets opened estimates count
        ''' </summary>
        ''' <param name="portalId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimatesSoldCount(portalId As Integer, userId As Integer) As Integer

    End Interface

End Namespace
