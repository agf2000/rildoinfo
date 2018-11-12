
Imports DotNetNuke.Web.Api
Imports Microsoft.AspNet.SignalR
Imports Microsoft.AspNet.SignalR.Hubs

Public MustInherit Class DnnApiControllerWithHub
    Inherits DnnApiController

    Protected ReadOnly estimatesHub As New Lazy(Of IHubContext)(Function() GlobalHost.ConnectionManager.GetHubContext(Of EstimatesHub)())
    'Protected ReadOnly AdminHub As New Lazy(Of IHubContext)(Function() GlobalHost.ConnectionManager.GetHubContext(Of AdminHub)())

    Protected ReadOnly peopleHub As New Lazy(Of IHubContext)(Function() GlobalHost.ConnectionManager.GetHubContext(Of PeopleHub)())

End Class
'Public MustInherit Class DnnApiControllerWithHub(Of THub As IHub)
'    Inherits DnnApiController
'    Private m_hub As New Lazy(Of IHubContext)(Function() GlobalHost.ConnectionManager.GetHubContext(Of THub)())

'    Protected ReadOnly Property Hub() As IHubContext
'        Get
'            Return m_hub.Value
'        End Get
'    End Property
'End Class
