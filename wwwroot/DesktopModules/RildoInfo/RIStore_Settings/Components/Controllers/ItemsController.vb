
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports System.Web.Routing
Imports DotNetNuke.Web.Api

Namespace RI.Modules.RIStore_Settings

    Public Class ItemsController
        Inherits DnnApiController

        '<ValidateAntiForgeryToken()> _
        <HttpGet> _
        <RequireHost> _
        Public Function GetItems(moduleId As Integer) As HttpResponseMessage
            Try
                Dim objCtrl = New ItemController
                Dim customer = objCtrl.GetItems(moduleId)

                Return Request.CreateResponse(HttpStatusCode.OK, customer)
            Catch ex As Exception
                Exceptions.LogException(ex)
                Return Request.CreateResponse(HttpStatusCode.OK, "error")
            End Try
        End Function

    End Class
End Namespace