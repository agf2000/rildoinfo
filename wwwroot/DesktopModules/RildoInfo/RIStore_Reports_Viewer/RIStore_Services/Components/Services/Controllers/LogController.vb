Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports DotNetNuke.Services.Log.EventLog
Imports DotNetNuke.Web.Api

Namespace RI.Modules.RIStore_Services

    Public Class LogController
        Inherits DnnApiController

        ''' <summary>
        ''' WebAPI deserializes json post into DnnServicesObjects.ServicesAction
        ''' </summary>
        ''' <param name="action">json object</param>
        ''' <returns></returns>
        <AllowAnonymous> _
        <HttpPost> _
        Public Function LogAnonymous(action As Models.ServicesAction) As HttpResponseMessage
            Try

                Dim services As New Services()
                services.Log(action)
                Return Request.CreateResponse(HttpStatusCode.OK, "sucess")

            Catch ex As Exception
                'DnnLog.Error(ex)
                Return Request.CreateResponse(HttpStatusCode.OK, New With {.Result = "Um erro imprevisto ocorreu. Caso este erro persista, contate o administrador do sistema.", .Msg = ex.Message})
            End Try
        End Function

    End Class
End Namespace
