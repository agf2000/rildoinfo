
Imports DotNetNuke.Web.Api
Imports System.Web.Http
Imports System.Web.Routing

Public Class ContactFormServicesRouteMapper
    Implements IServiceRouteMapper

    Public Sub RegisterRoutes(mapRouteManager As IMapRoute) Implements IServiceRouteMapper.RegisterRoutes
        mapRouteManager.MapHttpRoute("riwcf", "default", "{Controller}/{action}", New String() {"RIW.Modules.ContactForm"})
    End Sub

End Class
