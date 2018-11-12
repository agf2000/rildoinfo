
Public Class AgendasRepository
    Implements IAgendasRepository

    Public Function AddAppointmentData(appointment As Models.Agenda) As Models.Agenda Implements IAgendasRepository.AddAppointmentData
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Agenda) = ctx.GetRepository(Of Models.Agenda)()
            rep.Insert(appointment)
        End Using
        Return appointment
    End Function

    Public Function GetAgendaData(PortalId As String, UserId As String, StartDateTime As Date, EndDateTime As Date) As IEnumerable(Of Models.Agenda) Implements IAgendasRepository.GetAgendaData
        Return CBO.FillCollection(Of Models.Agenda)(DataProvider.Instance().GetAgendaData(PortalId, UserId, StartDateTime, EndDateTime))
    End Function

    Public Function GetAppointmentData(AppointmentId As Integer, UserId As Integer) As Models.Agenda Implements IAgendasRepository.GetAppointmentData
        Dim appointment As Models.Agenda

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Agenda) = ctx.GetRepository(Of Models.Agenda)()
            appointment = rep.GetById(Of Int32, Int32)(AppointmentId, UserId)
        End Using
        Return appointment
    End Function

    Public Function GetAppointmentsDataRes() As IEnumerable(Of Models.Agenda) Implements IAgendasRepository.GetAppointmentsDataRes
        Throw New NotImplementedException()
    End Function

    Public Sub UpdateAppointmentData(appointment As Models.Agenda) Implements IAgendasRepository.UpdateAppointmentData
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Agenda) = ctx.GetRepository(Of Models.Agenda)()
            rep.Update(appointment)
        End Using
    End Sub

    Public Sub RemoveAppointment(appointmentId As Integer, userId As Integer) Implements IAgendasRepository.RemoveAppointment
        Dim _item As Models.Agenda = GetAppointmentData(appointmentId, userId)
        If _item Is Nothing Then
            Throw New ArgumentException("Exception Occured")
        Else
            RemoveAppointment(_item)
        End If
    End Sub

    Public Sub RemoveAppointment(Appointment As Models.Agenda) Implements IAgendasRepository.RemoveAppointment
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Agenda) = ctx.GetRepository(Of Models.Agenda)()
            rep.Delete(Appointment)
        End Using
    End Sub

    Public Sub RemoveAppointments(userId As Integer) Implements IAgendasRepository.RemoveAppointments
        Dim appointments As IEnumerable(Of Models.Agenda)
        Using context As IDataContext = DataContext.Instance()
            Dim rep = context.GetRepository(Of Models.Agenda)()
            appointments = rep.Find("Where UserId = @0", userId)

            For Each appointment In appointments
                RemoveAppointment(appointment)
            Next
        End Using
    End Sub
End Class
