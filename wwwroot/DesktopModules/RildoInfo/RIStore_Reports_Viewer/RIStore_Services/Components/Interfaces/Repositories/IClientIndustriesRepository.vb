
Namespace RI.Modules.RIStore_Services
    Public Interface IClientIndustriesRepository

        ''' <summary>
        ''' Gets a list of industries by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetClientIndustries(ByVal clientId As Integer) As IEnumerable(Of Models.Industry)

        ''' <summary>
        ''' Adds a new client industry
        ''' </summary>
        ''' <param name="t">Client Industry Model</param>
        ''' <remarks></remarks>
        Function AddClientIndustry(t As Models.ClientIndustry) As Models.ClientIndustry

        ''' <summary>
        ''' Removes client industries by client id
        ''' </summary>
        ''' <param name="clientId">Client ID</param>
        ''' <remarks></remarks>
        Sub RemoveClientIndustries(clientId As Integer)

        ''' <summary>
        ''' Removes a client industry
        ''' </summary>
        ''' <param name="t">Client Industry Model</param>
        ''' <remarks></remarks>
        Sub RemoveClientIndustry(ByVal t As Models.ClientIndustry)

    End Interface
End Namespace