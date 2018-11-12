
Imports DotNetNuke.Entities.Host
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Services.Scheduling
Imports DotNetNuke.Instrumentation
Imports RIW.Modules.Common
Imports RIW.Modules.WebAPI.Components

Public Class ScheduleSyncProducts
    Inherits SchedulerClient

    Public Sub New(historyItem As ScheduleHistoryItem)
        MyBase.New()
        ScheduleHistoryItem = historyItem
    End Sub

    Public Overrides Sub DoWork()
        Progressing()

        Try
            Dim counts = ProductsController.SyncSGIProducts(Today(), Today().AddDays(1))

            ScheduleHistoryItem.AddLogNote("I: " & counts.added.ToString() & " A: " & counts.updated.ToString())

            ScheduleHistoryItem.Succeeded = True

        Catch ex As Exception
            ScheduleHistoryItem.Succeeded = False
            DotNetNuke.Services.Exceptions.Exceptions.LogException(ex)
        End Try
    End Sub

End Class
