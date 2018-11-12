
Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models

Namespace Components.Repositories

    Public Class CashiersRepository
        Implements ICashiersRepository

#Region "Private Methods"

        Private Shared Function GetNull(field As Object) As Object
            Return Null.GetNull(field, DBNull.Value)
        End Function

#End Region

        'Public Function AddCashier(sale As Models.Cashier) As Models.Cashier Implements ICashiersRepository.AddCashier
        '    Using ctx As IDataContext = DataContext.Instance()
        '        Dim rep As IRepository(Of Models.Cashier) = ctx.GetRepository(Of Models.Cashier)()
        '        rep.Insert(sale)
        '    End Using
        '    Return sale
        'End Function

        Public Function GetCashiers(portalId As Integer, sDate As DateTime, eDate As DateTime, pageNumber As Integer, pageSize As Integer, orderBy As String, orderDir As String) As IEnumerable(Of Cashier) Implements ICashiersRepository.GetCashiers
            Return CBO.FillCollection(Of Cashier)(Data.DataProvider.Instance().ExecuteReader("RIW_Cashiers_GetList", portalId, GetNull(sDate), GetNull(eDate), pageNumber, pageSize, orderBy, orderDir))
        End Function

        Public Function GetCashier(cashierId As Integer) As Cashier Implements ICashiersRepository.GetCashier
            Dim cashier As Cashier
            Using ctx As IDataContext = DataContext.Instance()
                cashier = ctx.ExecuteSingleOrDefault(Of Cashier)(CommandType.Text, "where cashierId = @0", cashierId)
            End Using
            Return cashier
        End Function

        Public Sub ProcessCashier(cashierDate As Date, userId As Integer) Implements ICashiersRepository.ProcessCashier
            Data.DataProvider.Instance().ExecuteNonQuery("RIW_Cashiers_DailyProcess", cashierDate, userId)
        End Sub

        Public Sub RemoveCashier(cashierId As Integer) Implements ICashiersRepository.RemoveCashier
            Dim item As Cashier = GetCashier(cashierId)
            RemoveCashier(item)
        End Sub

        Public Sub RemoveCashier(cashier As Cashier) Implements ICashiersRepository.RemoveCashier
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Cashier) = ctx.GetRepository(Of Cashier)()
                rep.Delete(cashier)
            End Using
        End Sub
    End Class

End Namespace
