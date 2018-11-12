
Namespace Components.Interfaces.Repositories

    Public Interface IPaymentsRepository

        Function AddPayment(payment As Components.Models.Payment) As Components.Models.Payment

        ''' <summary>
        ''' Gets a list of payments
        ''' </summary>
        ''' <param name="portalId">POrtal ID</param>
        ''' <param name="accountId">Account ID</param>
        ''' <param name="originId">Origin ID</param>
        ''' <param name="providerId">Provider ID</param>
        ''' <param name="clientId">Client ID</param>
        ''' <param name="category">Credit or Debit</param>
        ''' <param name="sDate">Start Transaction Date</param>
        ''' <param name="eDate">End Transaction Date</param>
        ''' <param name="sTerm">Search Term</param>
        ''' <param name="done">Done</param>
        ''' <param name="pageNumber">Page Number</param>
        ''' <param name="pageSize">Page Size</param> 
        ''' <param name="orderBy">Order By</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPayments(portalId As Integer, accountId As Integer, originId As Integer, providerId As Integer, clientId As Integer, category As String, done As String, sTerm As String, sDate As DateTime, eDate As DateTime, filterDate As String, pageNumber As Integer, pageSize As Integer, orderBy As String) As IEnumerable(Of Components.Models.Payment)

        Function GetPayemnt(paymentId As Integer, portalId As Integer) As Components.Models.Payment

        Sub UpdatePayment(payment As Components.Models.Payment)

        Sub RemovePayment(paymentId As Integer, portalId As Integer)

        Sub RemovePayment(payment As Components.Models.Payment)

    End Interface

End Namespace
