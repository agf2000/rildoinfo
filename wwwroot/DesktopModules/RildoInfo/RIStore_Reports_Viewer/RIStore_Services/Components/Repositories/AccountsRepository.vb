
Imports System.Collections.Generic
Imports DotNetNuke.Data

Namespace RI.Modules.RIStore_Services
    Public Class AccountsRepository
        Implements IAccountsRepository

        Public Function AddAccount(account As Models.Account) As Models.Account Implements IAccountsRepository.AddAccount
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Account) = ctx.GetRepository(Of Models.Account)()
                rep.Insert(account)
            End Using
            Return account
        End Function

        Public Function GetAccounts(portalId As Integer) As IEnumerable(Of Models.Account) Implements IAccountsRepository.GetAccounts
            Dim account As IEnumerable(Of Models.Account)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Account) = ctx.GetRepository(Of Models.Account)()
                account = rep.Get(portalId)
            End Using
            Return account
        End Function

        Public Sub UpdateAccount(account As Models.Account) Implements IAccountsRepository.UpdateAccount
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Account) = ctx.GetRepository(Of Models.Account)()
                rep.Update(account)
            End Using
        End Sub

        Public Sub RemoveAccount(accountId As Integer, portalId As Integer) Implements IAccountsRepository.RemoveAccount
            Dim _item As Models.Account = GetAccount(accountId, portalId)
            If _item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveAccount(_item)
            End If
        End Sub

        Public Function GetAccount(ByVal itemid As Integer, ByVal portalId As Integer) As Models.Account Implements IAccountsRepository.GetAccount
            Dim t As Models.Account

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Account) = ctx.GetRepository(Of Models.Account)()
                t = rep.GetById(Of Int32, Int32)(itemid, portalId)
            End Using
            Return t
        End Function

        Public Sub RemoveAccount(account As Models.Account) Implements IAccountsRepository.RemoveAccount
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.Account) = ctx.GetRepository(Of Models.Account)()
                rep.Delete(account)
            End Using
        End Sub

        Public Function GetAccountsBalance(portalId As String, account As String) As Models.Account Implements IAccountsRepository.GetAccountsBalance
            Dim accountBalance As Models.Account

            Using ctx As IDataContext = DataContext.Instance()
                accountBalance = ctx.ExecuteQuery(Of Models.Account)(CommandType.StoredProcedure, "RIS_AccountsBalance_Get", portalId, account)
            End Using
            Return accountBalance
        End Function
    End Class
End Namespace