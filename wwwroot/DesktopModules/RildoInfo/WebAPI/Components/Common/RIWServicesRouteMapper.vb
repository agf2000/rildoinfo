
Imports DotNetNuke.Web.Api
Imports System.Web.Http
Imports System.Web.Routing

Public Class RIWServicesRouteMapper
    Implements IServiceRouteMapper

    Public Sub RegisterRoutes(mapRouteManager As IMapRoute) Implements IServiceRouteMapper.RegisterRoutes
        mapRouteManager.MapHttpRoute("riw", "default", "{Controller}/{action}", New String() {"RIW.Modules.WebAPI"})
        mapRouteManager.MapHttpRoute("riw", "identity", "{Controller}/{action}/{id}", New String() {"RIW.Modules.WebAPI"})
        mapRouteManager.MapHttpRoute("riw", "lists", "{controller}/{action}", New String() {"RIW.Modules.WebAPI"})
        mapRouteManager.MapHttpRoute("riw", "sublist", "{controller}/{action}/{code}", New String() {"RIW.Modules.WebAPI"})
        RouteTable.Routes.MapHubs()
    End Sub

End Class
