
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Instrumentation

Namespace RI.Modules.RIStore_Services
    Public Class AccountsController
        Inherits DnnApiController

        Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(AccountsController))

        ''' <summary>
        ''' Gets a list of accounts by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpGet> _
        Function GetAccounts(ByVal portalId As Integer) As HttpResponseMessage
            Try
                Dim accounts As IEnumerable(Of Models.Account)

                If portalId > 0 Then
                    Using context As IDataContext = DataContext.Instance()
                        Dim repository = context.GetRepository(Of Models.Account)()
                        accounts = repository.Get(portalId)
                    End Using

                    Return Request.CreateResponse(HttpStatusCode.OK, accounts)
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Adds ou updates an account
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="accId">Account ID</param>
        ''' <param name="AccName">Brand Title</param>
        ''' <param name="uId">Created By User User ID</param>
        ''' <param name="cd">Created Date</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpPost> _
        Function UpdateAccount(ByVal portalId As Integer, ByVal accName As String, ByVal uId As Integer, ByVal cd As Date, Optional ByVal accId As Integer = -1) As HttpResponseMessage
            Try
                Dim t As New Models.Account
                Dim tc As New AccountsRepository

                If accId > 0 Then
                    
                    t = tc.GetAccount(accId, portalId)
                    t.AccountName = accName
                    t.ModifiedByUser = uId
                    t.ModifiedOnDate = cd
                    tc.UpdateAccount(t)

                    Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
                Else
                    t.AccountName = accName
                    t.CreatedByUser = uId
                    t.CreatedOnDate = cd

                    tc.AddAccount(t)

                    Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success", .AccountId = t.AccountId})
                End If

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema."})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

        ''' <summary>
        ''' Removes an account
        ''' </summary>
        ''' <param name="accId">Account ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpDelete> _
        Function RemoveAccount(ByVal accId As Integer, portalId As Integer) As HttpResponseMessage
            Try
                Dim accountCtrl As New AccountsRepository
                accountCtrl.RemoveAccount(accId, portalId)

                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "success"})
            Catch ex As Exception
                'DnnLog.Error(ex)
                Logger.[Error](ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

    End Class
End Namespace