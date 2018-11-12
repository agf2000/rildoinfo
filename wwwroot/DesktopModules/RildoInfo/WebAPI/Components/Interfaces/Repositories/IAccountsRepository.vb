
Namespace Components.Interfaces.Repositories

    Public Interface IAccountsRepository

        ''' <summary>
        ''' Gets a list of accounts by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetAccounts(portalId As Integer) As IEnumerable(Of Components.Models.Account)

        ''' <summary>
        ''' Gets an account by id
        ''' </summary>
        ''' <param name="accountId">Account ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetAccount(accountId As Integer, portalId As Integer) As Components.Models.Account

        ''' <summary>
        ''' Updates a creditor account info
        ''' </summary>
        ''' <param name="account">Account Model</param>
        ''' <remarks></remarks>
        Sub UpdateAccount(account As Components.Models.Account)

        ''' <summary>
        ''' Adds a new account
        ''' </summary>
        ''' <param name="account">Account Model</param>
        ''' <remarks></remarks>
        Function AddAccount(account As Components.Models.Account) As Components.Models.Account

        ''' <summary>
        ''' Removes an account by id
        ''' </summary>
        ''' <param name="accountId">Account ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <remarks></remarks>
        Sub RemoveAccount(accountId As Integer, portalId As Integer)

        ''' <summary>
        ''' Removes an account
        ''' </summary>
        ''' <param name="account">Account Model</param>
        ''' <remarks></remarks>
        Sub RemoveAccount(account As Components.Models.Account)

        ''' <summary>
        ''' Gets account's total balance
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="accountId">Account ID</param>
        ''' <param name="sDate">Staring Date</param>
        ''' <param name="eDate">Ending Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetAccountsBalance(portalId As Integer, accountId As Integer, sDate As DateTime, eDate As DateTime, filterDate As String) As Components.Models.Account

    End Interface

End Namespace
