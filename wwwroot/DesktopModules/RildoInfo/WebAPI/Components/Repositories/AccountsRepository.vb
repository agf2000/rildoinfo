Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Repositories

    Public Class AccountsRepository
    Implements IAccountsRepository

        Public Function AddAccount(account As Account) As Account Implements IAccountsRepository.AddAccount
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Account) = ctx.GetRepository(Of Account)()
                rep.Insert(account)
            End Using
            Return account
        End Function

        Public Function GetAccounts(portalId As Integer) As IEnumerable(Of Account) Implements IAccountsRepository.GetAccounts
            Return CBO.FillCollection(Of Account)(DataProvider.Instance().GetAccounts(portalId))
        End Function

        Public Sub UpdateAccount(account As Account) Implements IAccountsRepository.UpdateAccount
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Account) = ctx.GetRepository(Of Account)()
                rep.Update(account)
            End Using
        End Sub

        Public Sub RemoveAccount(accountId As Integer, portalId As Integer) Implements IAccountsRepository.RemoveAccount
            Dim item As Account = GetAccount(accountId, portalId)
            If item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveAccount(item)
            End If
        End Sub

        Public Function GetAccount(ByVal accountId As Integer, ByVal portalId As Integer) As Account Implements IAccountsRepository.GetAccount
            Return CBO.FillObject(Of Account)(DataProvider.Instance().GetAccount(accountId, portalId))
        End Function

        Public Sub RemoveAccount(account As Account) Implements IAccountsRepository.RemoveAccount
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Account) = ctx.GetRepository(Of Account)()
                rep.Delete(account)
            End Using
        End Sub

        Public Function GetAccountsBalance(portalId As Integer, accountId As Integer, sDate As DateTime, eDate As DateTime, filterDate As String) As Account Implements IAccountsRepository.GetAccountsBalance
            Return CBO.FillObject(Of Account)(DataProvider.Instance().GetAccountBalance(portalId, accountId, sDate, eDate, filterDate))
        End Function
    End Class

End Namespace
