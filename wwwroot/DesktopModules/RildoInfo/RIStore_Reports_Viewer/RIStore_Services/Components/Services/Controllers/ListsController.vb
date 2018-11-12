
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.Http
Imports DotNetNuke.Common.Lists
Imports DotNetNuke.Web.Api

Namespace RI.Modules.RIStore_Services
    Public Class ListsController
        Inherits DnnApiController

        ''' <summary>
        ''' Anonymous REST API that returns list of coutries in DNN
        ''' </summary>
        ''' <remarks></remarks>
        <HttpGet> _
        <ActionName("countries")> _
        <AllowAnonymous> _
        Public Function GetCountryList() As List(Of NameValueItem)
            Dim lc As New DotNetNuke.Common.Lists.ListController()
            Dim leCountries As List(Of ListEntryInfo) = lc.GetListEntryInfoItems("Country", "", MyBase.PortalSettings.PortalId).ToList()
            Return leCountries.[Select](Function(x) New NameValueItem() With {.Text = x.Text, .Value = x.Value}).ToList()
        End Function

        ''' <summary>
        ''' GET: /Regions/{code}
        ''' </summary>
        ''' <param name="code"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpGet> _
        <ActionName("regions")> _
        <AllowAnonymous> _
        Public Function GetRegionList(code As String) As List(Of NameValueItem)
            Dim lc As New DotNetNuke.Common.Lists.ListController()
            Dim leRegions As List(Of ListEntryInfo) = lc.GetListEntryInfoItems("Region", "Country." & code, MyBase.PortalSettings.PortalId).ToList()
            Return leRegions.[Select](Function(x) New NameValueItem() With {.Text = x.Text, .Value = x.Value}).ToList()
        End Function

        Public Class NameValueItem
            Public Property Value() As String
            Public Property Text() As String
        End Class
    End Class
End Namespace