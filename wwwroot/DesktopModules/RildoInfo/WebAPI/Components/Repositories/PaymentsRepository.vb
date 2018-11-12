Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Repositories

    Public Class PaymentsRepository
    Implements IPaymentsRepository

        '#Region "Private Methods"

        'Private Shared Function GetNull(field As Object) As Object
        '    Return Null.GetNull(field, DBNull.Value)
        'End Function

        '#End Region

        Public Function AddPayment(payment As Payment) As Payment Implements IPaymentsRepository.AddPayment
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Payment) = ctx.GetRepository(Of Payment)()
                rep.Insert(payment)
            End Using
            Return payment
        End Function

        Public Function GetPayment(paymentId As Integer, portalId As Integer) As Payment Implements IPaymentsRepository.GetPayemnt
            Return CBO.FillObject(Of Payment)(DataProvider.Instance().GetPayment(paymentId, portalId))
        End Function

        Public Function GetPayments(portalId As Integer, accountId As Integer, originId As Integer, providerId As Integer, clientId As Integer, category As String, done As String, sTerm As String, sDate As DateTime, eDate As DateTime, filterDate As String, pageNumber As Integer, pageSize As Integer, orderBy As String) As IEnumerable(Of Payment) Implements IPaymentsRepository.GetPayments
            Return CBO.FillCollection(Of Payment)(DataProvider.Instance().GetPayments(portalId, accountId, originId, providerId, clientId, category, done, sTerm, sDate, eDate, filterDate, pageNumber, pageSize, orderBy))
        End Function

        Public Sub UpdatePayment(payment As Payment) Implements IPaymentsRepository.UpdatePayment
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Payment) = ctx.GetRepository(Of Payment)()
                rep.Update(payment)
            End Using
        End Sub

        Public Sub RemovePayment(paymentId As Integer, portalId As Integer) Implements IPaymentsRepository.RemovePayment
            Dim item As Payment = GetPayment(paymentId, portalId)
            RemovePayment(item)
        End Sub

        Public Sub RemovePayment(payment As Payment) Implements IPaymentsRepository.RemovePayment
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Payment) = ctx.GetRepository(Of Payment)()
                rep.Delete(payment)
            End Using
        End Sub
    End Class

End Namespace
