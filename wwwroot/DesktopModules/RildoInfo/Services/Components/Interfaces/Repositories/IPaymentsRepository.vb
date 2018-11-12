
Public Interface IPaymentsRepository

    Function AddPayment(payment As Models.Payment) As Models.Payment

    ''' <summary>
    ''' Gets a list of payments
    ''' </summary>
    ''' <param name="portalId">POrtal ID</param>
    ''' <param name="account">Account ID</param>
    ''' <param name="provider">Provider ID</param>
    ''' <param name="client">Client ID</param>
    ''' <param name="cat">Credit or Debit</param>
    ''' <param name="sDate">Start Transaction Date</param>
    ''' <param name="eDate">End Transaction Date</param>
    ''' <param name="sTerm">Search Term</param>
    ''' <param name="done">Done</param>
    ''' <param name="pageNumber">Page Number</param>
    ''' <param name="pageSize">Page Size</param> 
    ''' <param name="orderBy">Order By</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetPayments(ByVal portalId As String, ByVal account As String, ByVal provider As String, ByVal client As String, ByVal cat As String, ByVal done As Boolean, ByVal sTerm As String, ByVal sDate As DateTime, ByVal eDate As DateTime, ByVal pageNumber As Integer, ByVal pageSize As Integer, orderBy As String) As IEnumerable(Of Models.Payment)

    Function GetPayemnt(paymentId As Integer, portalId As Integer) As Models.Payment

    Sub UpdatePayment(payment As Models.Payment)

End Interface
