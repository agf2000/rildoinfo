Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IDiscountGroupsRepository

        ''' <summary>
        ''' Gets a list of discount groups by portal id
        ''' </summary>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetDiscountGroups(portalId As Integer) As IEnumerable(Of DiscountGroup)

        ''' <summary>
        ''' Gets a discount group by id
        ''' </summary>
        ''' <param name="discountGroupId">Account ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetDiscountGroup(discountGroupId As Integer, portalId As Integer) As DiscountGroup

        ''' <summary>
        ''' Updates a discount group info
        ''' </summary>
        ''' <param name="discountGroup">Discount Group Model</param>
        ''' <remarks></remarks>
        Sub UpdateDiscountGroup(discountGroup As DiscountGroup)

        ''' <summary>
        ''' Adds a new discount group
        ''' </summary>
        ''' <param name="discountGroup">Discount Group Model</param>
        ''' <remarks></remarks>
        Function AddDiscountGroup(discountGroup As DiscountGroup) As DiscountGroup

        ''' <summary>
        ''' Removes a discount group by id
        ''' </summary>
        ''' <param name="discountGroupId">Discount Group ID</param>
        ''' <param name="portalId">POrtal ID</param>
        ''' <remarks></remarks>
        Sub RemoveDiscountGroup1(discountGroupId As Integer, portalId As Integer)

        ''' <summary>
        ''' Removes a discount group
        ''' </summary>
        ''' <param name="discountGroup">Discount Group Model</param>
        ''' <remarks></remarks>
        Sub RemoveDiscountGroup(discountGroup As DiscountGroup)

    End Interface

End Namespace
