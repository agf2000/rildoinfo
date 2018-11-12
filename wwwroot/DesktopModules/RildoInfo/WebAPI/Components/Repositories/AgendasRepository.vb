
Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
 
Namespace Components.Repositories

    Public Class AgendasRepository
    Implements IAgendasRepository

        Public Function AddAppointmentData(appointment As Components.Models.Agenda) As Components.Models.Agenda Implements IAgendasRepository.AddAppointmentData
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.Agenda) = ctx.GetRepository(Of Components.Models.Agenda)()
                rep.Insert(appointment)
            End Using
            Return appointment
        End Function

        Public Function GetAgendaData(portalId As Integer, userId As String, startDateTime As DateTime, endDateTime As DateTime, docId As Integer) As IEnumerable(Of Components.Models.Agenda) Implements IAgendasRepository.GetAgendaData
            Return CBO.FillCollection(Of Components.Models.Agenda)(DataProvider.Instance().GetAgendaData(portalId, userId, startDateTime, endDateTime, docId))
        End Function

        Public Function GetAppointmentData(appointmentId As Integer, portalId As Integer) As Components.Models.Agenda Implements IAgendasRepository.GetAppointmentData
            Dim appointment As Components.Models.Agenda

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.Agenda) = ctx.GetRepository(Of Components.Models.Agenda)()
                appointment = rep.GetById(Of Int32, Int32)(appointmentId, portalId)
            End Using
            Return appointment
        End Function

        Public Function GetAppointmentsDataRes() As IEnumerable(Of Components.Models.Agenda) Implements IAgendasRepository.GetAppointmentsDataRes
            Throw New NotImplementedException()
        End Function

        Public Sub UpdateAppointmentData(appointment As Components.Models.Agenda) Implements IAgendasRepository.UpdateAppointmentData
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.Agenda) = ctx.GetRepository(Of Components.Models.Agenda)()
                rep.Update(appointment)
            End Using
        End Sub

        Public Sub RemoveAppointment(appointmentId As Integer, userId As Integer) Implements IAgendasRepository.RemoveAppointment
            Dim item As Components.Models.Agenda = GetAppointmentData(appointmentId, userId)
            If item Is Nothing Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveAppointment(item)
            End If
        End Sub

        Public Sub RemoveAppointment(appointment As Components.Models.Agenda) Implements IAgendasRepository.RemoveAppointment
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Components.Models.Agenda) = ctx.GetRepository(Of Components.Models.Agenda)()
                rep.Delete(appointment)
            End Using
        End Sub

        Public Sub RemoveAppointments(userId As Integer) Implements IAgendasRepository.RemoveAppointments
            Dim appointments As IEnumerable(Of Components.Models.Agenda)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of Components.Models.Agenda)()
                appointments = rep.Find("Where UserId = @0", userId)

                For Each appointment In appointments
                    RemoveAppointment(appointment)
                Next
            End Using
        End Sub
    End Class

End Namespace
