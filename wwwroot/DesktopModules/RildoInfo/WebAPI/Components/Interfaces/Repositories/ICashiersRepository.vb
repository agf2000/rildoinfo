Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface ICashiersRepository

        '''' <summary>
        '''' Adds a quick sales
        '''' </summary>
        '''' <param name="sale">Cashier Model</param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Function AddCashier(sale As Cashier) As Cashier

        ''' <summary>
        ''' Gets cashiers by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="sDate">Emission Date Start</param>
        ''' <param name="eDate">Emission Date End</param>
        ''' <param name="pageNumber">Page Number</param>
        ''' <param name="pageSize">How many pages</param>
        ''' <param name="orderBy">Field to order by</param>
        ''' <param name="orderDir">Field to order by</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetCashiers(portalId As Integer, sDate As DateTime, eDate As DateTime, pageNumber As Integer, pageSize As Integer, orderBy As String, orderDir As String) As IEnumerable(Of Cashier)

        ''' <summary>
        ''' Gets cashier
        ''' </summary>
        ''' <param name="cashierId">Cashier ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetCashier(cashierId As Integer) As Cashier

        ''' <summary>
        ''' Process Cashier by date
        ''' </summary>
        ''' <param name="cashier">Cashier Date</param>
        ''' <param name="userId">User ID</param>
        ''' <remarks></remarks>
        Sub ProcessCashier(cashierDate As Date, userId As Integer)

        ''' <summary>
        ''' Removes cashier by cashier id
        ''' </summary>
        ''' <param name="cashierId">Cashier ID</param>
        ''' <remarks></remarks>
        Sub RemoveCashier(cashierId As Integer)

        ''' <summary>
        ''' Removes cashier
        ''' </summary>
        ''' <param name="cashier">Cashier Model</param>
        ''' <remarks></remarks>
        Sub RemoveCashier(cashier As Cashier)

    End Interface

End Namespace
