
Public Class StatusesRepository
    Implements IStatusesRepository

    Public Function addStatus(status As Models.Status) As Models.Status Implements IStatusesRepository.addStatus
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Status) = ctx.GetRepository(Of Models.Status)()
            rep.Insert(status)
        End Using
        Return status
    End Function

    Public Function getStatus(statusId As Integer, portalId As Integer) As Models.Status Implements IStatusesRepository.getStatus
        Dim status As Models.Status

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Status) = ctx.GetRepository(Of Models.Status)()
            status = rep.GetById(Of Int32, Int32)(statusId, portalId)
        End Using
        Return status
    End Function

    Public Function getStatus(statusName As String, portalId As Integer) As Models.Status Implements IStatusesRepository.getStatus
        Dim status As Models.Status

        Using ctx As IDataContext = DataContext.Instance()
            status = ctx.ExecuteSingleOrDefault(Of Models.Status)(CommandType.Text, "select top 1 * from RIW_statuses where statustitle = @0 and portalid = @1", statusName, portalId)
        End Using
        Return status
    End Function

    Public Function getStatuses(portalId As Integer, isDeleted As String) As IEnumerable(Of Models.Status) Implements IStatusesRepository.getStatuses
        Dim statuses As IEnumerable(Of Models.Status)

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Status) = ctx.GetRepository(Of Models.Status)()
            statuses = rep.Find("Where PortalId = @0 AND IsDeleted = @1", portalId, isDeleted)
        End Using
        Return statuses
    End Function

    Public Sub removeStatus(statusId As Integer, portalId As Integer) Implements IStatusesRepository.removeStatus
        Dim status As Models.Status = getStatus(statusId, portalId)
        removeStatus(status)
    End Sub

    Public Sub removeStatus(status As Models.Status) Implements IStatusesRepository.removeStatus
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Status) = ctx.GetRepository(Of Models.Status)()
            rep.Delete(status)
        End Using
    End Sub

    Public Sub updateStatus(status As Models.Status) Implements IStatusesRepository.updateStatus
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Status) = ctx.GetRepository(Of Models.Status)()
            rep.Update(status)
        End Using
    End Sub

End Class
