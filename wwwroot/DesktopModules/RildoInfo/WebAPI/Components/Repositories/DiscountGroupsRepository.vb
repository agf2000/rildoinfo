Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Repositories

    Public Class DiscountGroupsRepository
    Implements IDiscountGroupsRepository

        Public Function AddDiscountGroup(discountGroup As DiscountGroup) As DiscountGroup Implements IDiscountGroupsRepository.AddDiscountGroup
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of DiscountGroup) = ctx.GetRepository(Of DiscountGroup)()
                rep.Insert(discountGroup)
            End Using
            Return discountGroup
        End Function

        Public Function GetDiscountGroup(discountGroupId As Integer, portalId As Integer) As DiscountGroup Implements IDiscountGroupsRepository.GetDiscountGroup
            Dim discountGroup As DiscountGroup

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of DiscountGroup) = ctx.GetRepository(Of DiscountGroup)()
                discountGroup = rep.GetById(Of Int32, Int32)(discountGroupId, portalId)
            End Using
            Return discountGroup
        End Function

        Public Function GetDiscountGroups(portalId As Integer) As IEnumerable(Of DiscountGroup) Implements IDiscountGroupsRepository.GetDiscountGroups
            Dim discountGroups As IEnumerable(Of DiscountGroup)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of DiscountGroup) = ctx.GetRepository(Of DiscountGroup)()
                discountGroups = rep.Get(portalId)
            End Using
            Return discountGroups
        End Function

        Public Sub RemoveDiscountGroup1(discountGroupId As Integer, portalId As Integer) Implements IDiscountGroupsRepository.RemoveDiscountGroup1
            Dim discountGroup As DiscountGroup = GetDiscountGroup(discountGroupId, portalId)
            RemoveDiscountGroup(discountGroup)
        End Sub

        Public Sub RemoveDiscountGroup(discountGroup As DiscountGroup) Implements IDiscountGroupsRepository.RemoveDiscountGroup
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of DiscountGroup) = ctx.GetRepository(Of DiscountGroup)()
                rep.Delete(discountGroup)
            End Using
        End Sub

        Public Sub UpdateDiscountGroup(discountGroup As DiscountGroup) Implements IDiscountGroupsRepository.UpdateDiscountGroup
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of DiscountGroup) = ctx.GetRepository(Of DiscountGroup)()
                rep.Update(discountGroup)
            End Using
        End Sub
    End Class

End Namespace
