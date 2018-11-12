
Public Class PaymentsRepository
    Implements IPaymentsRepository

#Region "Private Methods"

    Private Shared Function GetNull(field As Object) As Object
        Return Null.GetNull(field, DBNull.Value)
    End Function

#End Region

    Public Function AddPayment(payment As Models.Payment) As Models.Payment Implements IPaymentsRepository.AddPayment
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Payment) = ctx.GetRepository(Of Models.Payment)()
            rep.Insert(payment)
        End Using
        Return payment
    End Function

    Public Function GetPayemnt(paymentId As Integer, portalId As Integer) As Models.Payment Implements IPaymentsRepository.GetPayemnt
        Dim product As Models.Payment

        Using ctx As IDataContext = DataContext.Instance()
            product = ctx.ExecuteSingleOrDefault(Of Models.Payment)(CommandType.StoredProcedure, "RIW_Payment_Get", paymentId)
        End Using
        Return product
    End Function

    Public Function GetPayments(ByVal portalId As String, ByVal account As String, ByVal provider As String, ByVal client As String, ByVal cat As String, ByVal done As Boolean, ByVal sTerm As String, ByVal sDate As DateTime, ByVal eDate As DateTime, ByVal pageNumber As Integer, ByVal pageSize As Integer, orderBy As String) As IEnumerable(Of Models.Payment) Implements IPaymentsRepository.GetPayments
        Dim payments As IEnumerable(Of Models.Payment)

        Using ctx As IDataContext = DataContext.Instance()
            payments = ctx.ExecuteQuery(Of Models.Payment)(CommandType.StoredProcedure, "RIW_Payments_Get", portalId, account, provider, client, cat, done, sTerm, GetNull(sDate), GetNull(eDate), pageNumber, pageSize, orderBy)
        End Using
        Return payments
    End Function

    Public Sub UpdatePayment(payment As Models.Payment) Implements IPaymentsRepository.UpdatePayment
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Payment) = ctx.GetRepository(Of Models.Payment)()
            rep.Update(payment)
        End Using
    End Sub

End Class
