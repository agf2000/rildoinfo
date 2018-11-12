
Public Interface IAccountsRepository

    ''' <summary>
    ''' Gets a list of creditor account by portal id
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetAccounts(ByVal portalId As Integer) As IEnumerable(Of Models.Account)

    ''' <summary>
    ''' Gets a creditor account by id
    ''' </summary>
    ''' <param name="accountId">Account ID</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetAccount(ByVal accountId As Integer, ByVal portalId As Integer) As Models.Account

    ''' <summary>
    ''' Updates a creditor account info
    ''' </summary>
    ''' <param name="t">Account Model</param>
    ''' <remarks></remarks>
    Sub UpdateAccount(t As Models.Account)

    ''' <summary>
    ''' Adds a new creditor account
    ''' </summary>
    ''' <param name="t">Account Model</param>
    ''' <remarks></remarks>
    Function AddAccount(t As Models.Account) As Models.Account

    ''' <summary>
    ''' Removes a creditor account by id
    ''' </summary>
    ''' <param name="accountId">Account ID</param>
    ''' <param name="portalId">POrtal ID</param>
    ''' <remarks></remarks>
    Sub RemoveAccount(accountId As Integer, portalId As Integer)

    ''' <summary>
    ''' Removes a creditor account
    ''' </summary>
    ''' <param name="t">Account Model</param>
    ''' <remarks></remarks>
    Sub RemoveAccount(ByVal t As Models.Account)

    ''' <summary>
    ''' Gets a total balance
    ''' </summary>
    ''' <param name="portalId">POrtal ID</param>
    ''' <param name="account">Account ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetAccountsBalance(ByVal portalId As String, ByVal account As String) As Models.Account

End Interface
