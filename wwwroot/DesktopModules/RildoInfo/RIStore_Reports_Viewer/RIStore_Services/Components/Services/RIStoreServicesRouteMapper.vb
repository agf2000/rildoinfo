
Imports DotNetNuke.Web.Api
Imports System.Web.Http

Namespace RI.Modules.RIStore_Services
    Public Class RIStoreServicesRouteMapper
        Implements IServiceRouteMapper

        Public Sub RegisterRoutes(mapRouteManager As IMapRoute) Implements IServiceRouteMapper.RegisterRoutes
            mapRouteManager.MapHttpRoute("riw", "identity", "{Controller}/{action}/{id}", New String() {"RI.Modules.RIStore_Services"})
            mapRouteManager.MapHttpRoute("riw", "default", "{Controller}/{action}", New String() {"RI.Modules.RIStore_Services"})
            mapRouteManager.MapHttpRoute("riw", "list", "{controller}/{action}", New String() {"RI.Modules.RIStore_Services"})
            mapRouteManager.MapHttpRoute("riw", "sublist", "{controller}/{action}/{code}", New String() {"RI.Modules.RIStore_Services"})
        End Sub

    End Class
End Namespace