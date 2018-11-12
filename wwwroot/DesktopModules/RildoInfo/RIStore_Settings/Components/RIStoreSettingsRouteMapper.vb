
Imports DotNetNuke.Web.Api

Namespace RI.Modules.RIStore_Settings
    Public Class RIStoreSettingsRouteMapper
        Implements IServiceRouteMapper

        Public Sub RegisterRoutes(mapRouteManager As IMapRoute) Implements IServiceRouteMapper.RegisterRoutes
            mapRouteManager.MapHttpRoute("rildoinfo", "default", "{Controller}/{action}", New String() {"RI.Modules.RIStore_Settings"})
        End Sub

    End Class
End Namespace