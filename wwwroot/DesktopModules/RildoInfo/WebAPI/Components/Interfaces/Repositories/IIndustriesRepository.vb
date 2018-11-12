Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IIndustriesRepository

        ''' <summary>
        ''' Gets a list of industries by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <param name="isDeleted"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetIndustries(portalId As Integer, isDeleted As String) As IEnumerable(Of Industry)

        ''' <summary>
        ''' Gets a creditor account by id
        ''' </summary>
        ''' <param name="accountId">Account ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetIndustry(accountId As Integer, portalId As Integer) As Industry

        ''' <summary>
        ''' Updates an industry info
        ''' </summary>
        ''' <param name="industry">Industry Model</param>
        ''' <remarks></remarks>
        Sub UpdateIndustry(industry As Industry)

        ''' <summary>
        ''' Adds a new industry
        ''' </summary>
        ''' <param name="industry">Industry Model</param>
        ''' <remarks></remarks>
        Function AddIndustry(industry As Industry) As Industry

        ''' <summary>
        ''' Removes an industry by id
        ''' </summary>
        ''' <param name="industryId">Industry ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <remarks></remarks>
        Sub RemoveIndustry(industryId As Integer, portalId As Integer)

        ''' <summary>
        ''' Removes an industry
        ''' </summary>
        ''' <param name="industry">Industry Model</param>
        ''' <remarks></remarks>
        Sub RemoveIndustry(industry As Industry)

    End Interface

End Namespace
