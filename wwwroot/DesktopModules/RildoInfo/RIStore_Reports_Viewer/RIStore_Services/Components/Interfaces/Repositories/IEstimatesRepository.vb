
Namespace RI.Modules.RIStore_Services
    Public Interface IEstimatesRepository

        ''' <summary>
        ''' Gets a list of estimates by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="salesRep">Sales Rep. ID</param>
        ''' <param name="SearchTerm">Search Term</param>
        ''' <param name="pageNumber">Page Number</param>
        ''' <param name="pageSize">Page Size</param>
        ''' <param name="orderBy">Order By</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimates(ByVal portalId As String, ByVal clientId As String, ByVal salesRep As String, ByVal SearchTerm As String, ByVal pageNumber As Integer, ByVal pageSize As Integer, ByVal orderBy As String) As IEnumerable(Of Models.Estimate)

        ''' <summary>
        ''' Gets a list of estimate items by estimate id
        ''' </summary>
        ''' <param name="eId">Estimate ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimateItems(eId As Integer) As IEnumerable(Of Models.EstimateItem)

        ''' <summary>
        ''' Gets an estimate by id
        ''' </summary>
        ''' <param name="estimateId">Estimate ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimate(ByVal estimateId As Integer, ByVal portalId As Integer) As Models.Estimate

        ''' <summary>
        ''' Adds an estimate
        ''' </summary>
        ''' <param name="estimate">Estimate Model</param>
        ''' <remarks></remarks>
        Function AddEstimate(estimate As Models.Estimate) As Models.Estimate

        ''' <summary>
        ''' Adds an estimate
        ''' </summary>
        ''' <param name="estimateItem">Estimate Item Model</param>
        ''' <remarks></remarks>
        Function AddEstimateItem(estimateItem As Models.EstimateItem) As Models.EstimateItem

        ''' <summary>
        ''' Updates an estimate
        ''' </summary>
        ''' <param name="estimate">Estimate Model</param>
        ''' <remarks></remarks>
        Sub UpdateEstimate(estimate As Models.Estimate)

        ''' <summary>
        ''' Removes estimate by estimate id and client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="estimateId">Estimate ID</param>
        ''' <remarks></remarks>
        Sub RemoveEstimate(estimateId As Integer, clientId As Integer)

        ''' <summary>
        ''' Removes all client estimates
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <remarks></remarks>
        Sub RemoveEstimates(clientId As Integer, portalId As Integer)

        ''' <summary>
        ''' Removes an estimate
        ''' </summary>
        ''' <param name="Estimate">Estimate Model</param>
        ''' <remarks></remarks>
        Sub RemoveEstimate(ByVal estimate As Models.Estimate)

        ''' <summary>
        ''' Adds an estimate
        ''' </summary>
        ''' <param name="estimateItem">Estimate Item Removed Model</param>
        ''' <remarks></remarks>
        Function AddEstimateItemRemoved(estimateItem As Models.EstimateRemovedItem) As Models.EstimateRemovedItem

        ''' <summary>
        ''' Gets an estimate by id
        ''' </summary>
        ''' <param name="estimateItemId">Estimate Item ID</param>
        ''' <param name="estimateId">Estimate ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEstimateItem(ByVal estimateItemId As Integer, ByVal estimateId As Integer) As Models.EstimateItem

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
        Sub RemoveEstimateItem(ByVal estimateItem As Models.EstimateItem)

        ''' <summary>
        ''' Adds an estimate hsitory
        ''' </summary>
        ''' <param name="estimateHistory">Estimate History Model</param>
        ''' <remarks></remarks>
        Function AddEstimateHistory(estimateHistory As Models.EstimateHistory) As Models.EstimateHistory

    End Interface
End Namespace