
Imports System.Collections.Generic
Imports DotNetNuke.Data

Namespace RI.Modules.RIStore_Services
    Public Class CashiersRepository
        Implements ICashiersRepository

        Public Function AddCashier(sale As Models.Cashier) As Models.Cashier Implements ICashiersRepository.AddCashier
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Cashier) = ctx.GetRepository(Of Models.Cashier)()
                rep.Insert(sale)
            End Using
            Return sale
        End Function

    End Class
End Namespace