
Public Interface IAgendasRepository

    ''' <summary>
    ''' Gets user agenda's data
    ''' </summary>
    ''' <param name="PortalId">Portal ID</param>
    ''' <param name="UserId">User ID</param>
    ''' <param name="StartDateTime">Agenda range starting date</param>
    ''' <param name="EndDateTime">Agenda range ending date</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetAgendaData(ByVal PortalId As String, ByVal UserId As String, ByVal StartDateTime As DateTime, ByVal EndDateTime As DateTime) As IEnumerable(Of Models.Agenda)

    ''' <summary>
    ''' Gets user appoitment's data
    ''' </summary>
    ''' <param name="AppointmentId">Appointment ID</param>
    ''' <param name="UserId">User ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetAppointmentData(AppointmentId As Integer, UserId As Integer) As Models.Agenda

    ''' <summary>
    ''' To be implemented
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetAppointmentsDataRes() As IEnumerable(Of Models.Agenda)

    ''' <summary>
    ''' Adds appointment data
    ''' </summary>
    ''' <param name="appointment">Agenda Model</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddAppointmentData(appointment As Models.Agenda) As Models.Agenda

    ''' <summary>
    ''' Updates appointment data
    ''' </summary>
    ''' <param name="appointment">Agenda Model</param>
    ''' <remarks></remarks>
    Sub UpdateAppointmentData(appointment As Models.Agenda)

    ''' <summary>
    ''' Removes appointment by appointment id
    ''' </summary>
    ''' <param name="appointmentId">AppointmentId ID</param>
    ''' <param name="userId">User ID</param>
    ''' <remarks></remarks>
    Sub RemoveAppointment(appointmentId As Integer, userId As Integer)

    ''' <summary>
    ''' Remove all appointments by user id
    ''' </summary>
    ''' <param name="userId">User ID</param>
    ''' <remarks></remarks>
    Sub RemoveAppointments(userId As Integer)

    ''' <summary>
    ''' Removes appointment
    ''' </summary>
    ''' <param name="appointment">Agenda Model</param>
    ''' <remarks></remarks>
    Sub RemoveAppointment(Appointment As Models.Agenda)

End Interface
