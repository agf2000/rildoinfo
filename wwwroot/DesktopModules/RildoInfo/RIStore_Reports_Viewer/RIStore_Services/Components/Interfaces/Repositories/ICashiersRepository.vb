
Namespace RI.Modules.RIStore_Services
    Public Interface ICashiersRepository

        ''' <summary>
        ''' Adds a quick sales
        ''' </summary>
        ''' <param name="sale">Cashier Model</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddCashier(sale As Models.Cashier) As Models.Cashier

    End Interface
End Namespace