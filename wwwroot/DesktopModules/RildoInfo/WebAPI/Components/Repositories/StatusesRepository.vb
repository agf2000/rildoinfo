Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
 
Namespace Components.Repositories

    Public Class StatusesRepository
    Implements IStatusesRepository

        Public Function AddStatus(status As Components.Models.Status) As Components.Models.Status Implements IStatusesRepository.AddStatus
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.Status) = ctx.GetRepository(Of Components.Models.Status)()
                rep.Insert(status)
            End Using
            Return status
        End Function

        Public Function GetStatus1(statusId As Integer, portalId As Integer) As Components.Models.Status Implements IStatusesRepository.GetStatus1
            Dim status As Components.Models.Status

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.Status) = ctx.GetRepository(Of Components.Models.Status)()
                status = rep.GetById(Of Int32, Int32)(statusId, portalId)
            End Using
            Return status
        End Function

        Public Function GetStatus(statusName As String, portalId As Integer) As Components.Models.Status Implements IStatusesRepository.GetStatus
            Dim status As Components.Models.Status

            Using ctx As IDataContext = DataContext.Instance()
                status = ctx.ExecuteSingleOrDefault(Of Components.Models.Status)(CommandType.Text, "select top 1 * from RIW_statuses where statustitle = @0 and portalid = @1", statusName, portalId)
            End Using
            Return status
        End Function

        Public Function GetStatuses(portalId As Integer, isDeleted As String) As IEnumerable(Of Components.Models.Status) Implements IStatusesRepository.GetStatuses
            Dim statuses As IEnumerable(Of Components.Models.Status)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.Status) = ctx.GetRepository(Of Components.Models.Status)()
                statuses = rep.Find("Where ((PortalId = @0) AND ((@1 = '') OR (IsDeleted = @1)))", portalId, isDeleted)
            End Using
            Return statuses
        End Function

        Public Sub RemoveStatus1(statusId As Integer, portalId As Integer) Implements IStatusesRepository.RemoveStatus1
            Dim status As Components.Models.Status = GetStatus1(statusId, portalId)
            RemoveStatus(status)
        End Sub

        Public Sub RemoveStatus(status As Components.Models.Status) Implements IStatusesRepository.RemoveStatus
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.Status) = ctx.GetRepository(Of Components.Models.Status)()
                rep.Delete(status)
            End Using
        End Sub

        Public Sub UpdateStatus(status As Components.Models.Status) Implements IStatusesRepository.UpdateStatus
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.Status) = ctx.GetRepository(Of Components.Models.Status)()
                rep.Update(status)
            End Using
        End Sub

    End Class

End Namespace
