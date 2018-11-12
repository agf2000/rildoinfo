
Public Interface IEstimatesRepository

    ''' <summary>
    ''' Gets a list of estimates
    ''' </summary>
    ''' <param name="portalId"></param>
    ''' <param name="personId"></param>
    ''' <param name="salesRep"></param>
    ''' <param name="statusId"></param>
    ''' <param name="sDate"></param>
    ''' <param name="eDate"></param>
    ''' <param name="filter"></param>
    ''' <param name="isDeleted"></param>
    ''' <param name="pageIndex"></param>
    ''' <param name="pageSize"></param>
    ''' <param name="orderBy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getEstimates(portalId As Integer, personId As Integer, salesRep As Integer, statusId As Integer, sDate As DateTime, eDate As DateTime, filter As String, isDeleted As String, pageIndex As Integer, pageSize As Integer, orderBy As String) As IEnumerable(Of Models.Estimate)

    ''' <summary>
    ''' Gets a list of estimate items by estimate id
    ''' </summary>
    ''' <param name="portalId"></param>
    ''' <param name="estimateId">Estimate ID</param>
    ''' <param name="lang"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getEstimateItems(portalId As Integer, estimateId As Integer, lang As String) As IEnumerable(Of Models.EstimateItem)

    ''' <summary>
    ''' Gets an estimate by id
    ''' </summary>
    ''' <param name="estimateId">Estimate ID</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getEstimate(estimateId As Integer, portalId As Integer) As Models.Estimate

    ''' <summary>
    ''' Adds an estimate
    ''' </summary>
    ''' <param name="estimate">Estimate Model</param>
    ''' <remarks></remarks>
    Function addEstimate(estimate As Models.Estimate) As Models.Estimate

    ''' <summary>
    ''' Updates an estimate
    ''' </summary>
    ''' <param name="estimate">Estimate Model</param>
    ''' <remarks></remarks>
    Sub updateEstimate(estimate As Models.Estimate)

    ''' <summary>
    ''' Adds an estimate
    ''' </summary>
    ''' <param name="estimateItem">Estimate Item Model</param>
    ''' <remarks></remarks>
    Function addEstimateItem(estimateItem As Models.EstimateItem) As Models.EstimateItem

    ''' <summary>
    ''' Updates an estimate
    ''' </summary>
    ''' <param name="estimateItem">Estimate Model</param>
    ''' <remarks></remarks>
    Sub updateEstimateItem(estimateItem As Models.EstimateItem)

    ''' <summary>
    ''' Removes estimate by estimate id and client id
    ''' </summary>
    ''' <param name="personId">Client ID</param>
    ''' <param name="estimateId">Estimate ID</param>
    ''' <remarks></remarks>
    Sub removeEstimate(estimateId As Integer, personId As Integer)

    ''' <summary>
    ''' Removes all client estimates
    ''' </summary>
    ''' <param name="personId">Client ID</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <remarks></remarks>
    Sub removeClientEstimates(personId As Integer, portalId As Integer)

    ''' <summary>
    ''' Removes an estimate
    ''' </summary>
    ''' <param name="Estimate">Estimate Model</param>
    ''' <remarks></remarks>
    Sub removeEstimate(estimate As Models.Estimate)

    ''' <summary>
    ''' Adds an estimate
    ''' </summary>
    ''' <param name="estimateItem">Estimate Item Removed Model</param>
    ''' <remarks></remarks>
    Function addEstimateItemRemoved(estimateItem As Models.EstimateItemRemoved) As Models.EstimateItemRemoved

    ''' <summary>
    ''' Gets an estimate by id
    ''' </summary>
    ''' <param name="estimateItemId">Estimate Item ID</param>
    ''' <param name="estimateId">Estimate ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getEstimateItem(estimateItemId As Integer, estimateId As Integer) As Models.EstimateItem

    ''' <summary>
    ''' Removes estimate by estimate id and client id
    ''' </summary>
    ''' <param name="estimateItemId">Estimate Item ID</param>
    ''' <param name="estimateId">Estimate ID</param>
    ''' <remarks></remarks>
    Sub removeEstimateItem(estimateItemId As Integer, estimateId As Integer)

    ''' <summary>
    ''' Removes an estimate item
    ''' </summary>
    ''' <param name="estimateItem">Estimate Item Model</param>
    ''' <remarks></remarks>
    Sub removeEstimateItem(estimateItem As Models.EstimateItem)

    ''' <summary>
    ''' Adds an estimate hsitory
    ''' </summary>
    ''' <param name="estimateHistory">Estimate History Model</param>
    ''' <remarks></remarks>
    Function addEstimateHistory(estimateHistory As Models.EstimateHistory) As Models.EstimateHistory

    ''' <summary>
    ''' Deletes an estimate
    ''' </summary>
    ''' <param name="estimate"></param>
    ''' <remarks></remarks>
    Sub deleteEstimate(estimate As Models.Estimate)

    ''' <summary>
    ''' Gets estimate histories
    ''' </summary>
    ''' <param name="estimateId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getEstimateHistories(estimateId As Integer) As IEnumerable(Of Models.EstimateHistory)

    ''' <summary>
    ''' Gets list of estimate messages
    ''' </summary>
    ''' <param name="estimateId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getEstimateMessages(estimateId As Integer) As IEnumerable(Of Models.EstimateMessage)

    ''' <summary>
    ''' Gets an estimate message
    ''' </summary>
    ''' <param name="estimateMessageId"></param>
    ''' <param name="estimateId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getEstimateMessage(estimateMessageId As Integer, estimateId As Integer) As Models.EstimateMessage

    ''' <summary>
    ''' Adds an estimate message
    ''' </summary>
    ''' <param name="estimateMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function addEstimateMessage(estimateMessage As Models.EstimateMessage) As Models.EstimateMessage

    ''' <summary>
    ''' Updates an estimate message
    ''' </summary>
    ''' <param name="estimateMessage"></param>
    ''' <remarks></remarks>
    Sub updateEstimateMessage(estimateMessage As Models.EstimateMessage)

    ''' <summary>
    ''' Removes an estimate message
    ''' </summary>
    ''' <param name="estimateMessageId"></param>
    ''' <param name="estimateId"></param>
    ''' <remarks></remarks>
    Sub removeEstimateMessage(estimateMessageId As Integer, estimateId As Integer)

    ''' <summary>
    ''' Removes an estimate message
    ''' </summary>
    ''' <param name="estimateMessage"></param>
    ''' <remarks></remarks>
    Sub removeEstimateMessage(estimateMessage As Models.EstimateMessage)

    ''' <summary>
    ''' Gets list of estimate messages
    ''' </summary>
    ''' <param name="estimateMessageId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getEstimateMessageComments(estimateMessageId As Integer) As List(Of Models.EstimateMessageComment)

    ''' <summary>
    ''' Gets estimate history comments
    ''' </summary>
    ''' <param name="estimateHistoryId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getEstimateHistoryComments(estimateHistoryId As Integer) As IEnumerable(Of Models.EstimateHistoryComment)

    ''' <summary>
    ''' Adds an estimate message comment
    ''' </summary>
    ''' <param name="estimateMessageComment"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function addEstimateMessageComment(estimateMessageComment As Models.EstimateMessageComment) As Models.EstimateMessageComment

    ''' <summary>
    ''' Adds an estimate history comment
    ''' </summary>
    ''' <param name="estimateHistoryComment"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function addEstimateHistoryComment(estimateHistoryComment As Models.EstimateHistoryComment) As Models.EstimateHistoryComment

    ''' <summary>
    ''' Gets an estimate message
    ''' </summary>
    ''' <param name="commentId"></param>
    ''' <param name="estimateMessageId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getEstimateMessageComment(commentId As Integer, estimateMessageId As Integer) As Models.EstimateMessageComment

    ''' <summary>
    ''' Gets an estimate message comment
    ''' </summary>
    ''' <param name="commentId"></param>
    ''' <param name="estimateHistoryId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getEstimateHistoryComment(commentId As Integer, estimateHistoryId As Integer) As Models.EstimateHistoryComment

    ''' <summary>
    ''' Updates an estimate message comment
    ''' </summary>
    ''' <param name="estimateMessageComment"></param>
    ''' <remarks></remarks>
    Sub updateEstimateMessageComment(estimateMessageComment As Models.EstimateMessageComment)

    ''' <summary>
    ''' Updates an estimate history comment
    ''' </summary>
    ''' <param name="estimateHistoryComment"></param>
    ''' <remarks></remarks>
    Sub updateEstimateHistoryComment(estimateHistoryComment As Models.EstimateHistoryComment)

    ''' <summary>
    ''' Removes an estimate message comment
    ''' </summary>
    ''' <param name="commentId"></param>
    ''' <param name="estimateMessageId"></param>
    ''' <remarks></remarks>
    Sub removeEstimateMessageComment(commentId As Integer, estimateMessageId As Integer)

    ''' <summary>
    ''' Removes an estimate history comment
    ''' </summary>
    ''' <param name="commentId"></param>
    ''' <param name="estimateHistoryId"></param>
    ''' <remarks></remarks>
    Sub removeEstimateHistoryComment(commentId As Integer, estimateHistoryId As Integer)

    ''' <summary>
    ''' Removes an estimate message comement
    ''' </summary>
    ''' <param name="estimateMessageComment"></param>
    ''' <remarks></remarks>
    Sub removeEstimateMessageComment(estimateMessageComment As Models.EstimateMessageComment)

    ''' <summary>
    ''' Removes an estimate history comment
    ''' </summary>
    ''' <param name="estimateHistoryComment"></param>
    ''' <remarks></remarks>
    Sub removeEstimateHistoryComment(estimateHistoryComment As Models.EstimateHistoryComment)

End Interface
