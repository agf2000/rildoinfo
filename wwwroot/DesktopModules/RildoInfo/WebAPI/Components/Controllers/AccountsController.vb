
Imports System.Net
Imports System.Net.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation
Imports RIW.Modules.WebAPI.Components.Repositories

Public Class AccountsController
    Inherits DnnApiController

    Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(AccountsController))

    ''' <summary>
    ''' Gets a list of accounts by portal id
    ''' </summary>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function getAccounts(portalId As Integer) As HttpResponseMessage
        Try
            Dim accountsCtrl As New AccountsRepository

            Dim accounts = accountsCtrl.GetAccounts(portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, accounts)

        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Adds ou updates an account
    ''' </summary>
    ''' <param name="dto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpPost> _
    Function updateAccount(dto As Components.Models.Account) As HttpResponseMessage
        Try
            Dim account As New Components.Models.Account
            Dim accountCtrl As New AccountsRepository

            If dto.AccountId > 0 Then
                account = accountCtrl.GetAccount(dto.AccountId, dto.PortalId)
            End If

            account.AccountName = dto.AccountName
            account.ModifiedByUser = dto.ModifiedByUser
            account.ModifiedOnDate = dto.ModifiedOnDate

            If dto.AccountId > 0 Then
                accountCtrl.UpdateAccount(account)
            Else
                account.CreatedByUser = dto.CreatedByUser
                account.CreatedOnDate = dto.CreatedOnDate
                accountCtrl.AddAccount(account)
            End If

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .Account = account})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Removes an account
    ''' </summary>
    ''' <param name="accountId">Account ID</param>
    ''' <param name="portalId">Portal ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpDelete> _
    Function removeAccount(accountId As Integer, portalId As Integer) As HttpResponseMessage
        Try
            Dim accountCtrl As New AccountsRepository

            accountCtrl.RemoveAccount(accountId, portalId)

            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Gets a total balance
    ''' </summary>
    ''' <param name="portalId">POrtal ID</param>
    ''' <param name="accountId">Account ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DnnAuthorize> _
    <HttpGet> _
    Function getAccountsBalance(sDate As DateTime, eDate As DateTime, Optional portalId As Integer = -1, Optional accountId As Integer = -1, Optional filterDate As String = "") As HttpResponseMessage
        Try
            Dim accountCtrl As New AccountsRepository

            Select Case filterDate
                Case "ModifiedOnDate"
                    filterDate = "2"
                Case "CreatedOnDate"
                    filterDate = "3"
                Case Else
                    filterDate = "1"
            End Select

            Dim accountBalance = accountCtrl.GetAccountsBalance(portalId, accountId, sDate, eDate, filterDate)

            Return Request.CreateResponse(HttpStatusCode.OK, accountBalance)
        Catch ex As Exception
            'DnnLog.Error(ex)
            Logger.[Error](ex)
            Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
        End Try
    End Function

End Class
