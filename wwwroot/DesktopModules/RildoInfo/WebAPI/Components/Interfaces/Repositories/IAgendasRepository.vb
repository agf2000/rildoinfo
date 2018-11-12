Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Interfaces.Repositories

    Public Interface IAgendasRepository

        ''' <summary>
        ''' Gets user agenda's data
        ''' </summary>
        ''' <param name="PortalId">Portal ID</param>
        ''' <param name="UserId">User ID</param>
        ''' <param name="StartDateTime">Agenda range starting date</param>
        ''' <param name="EndDateTime">Agenda range ending date</param>
        ''' <param name="docId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetAgendaData(portalId As Integer, userId As String, startDateTime As DateTime, endDateTime As DateTime, docId As Integer) As IEnumerable(Of Agenda)

        ''' <summary>
        ''' Gets user appoitment's data
        ''' </summary>
        ''' <param name="appointmentId">Appointment ID</param>
        ''' <param name="portalId">Portal ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetAppointmentData(appointmentId As Integer, portalId As Integer) As Agenda

        ''' <summary>
        ''' To be implemented
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetAppointmentsDataRes() As IEnumerable(Of Agenda)

        ''' <summary>
        ''' Adds appointment data
        ''' </summary>
        ''' <param name="appointment">Agenda Model</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddAppointmentData(appointment As Agenda) As Agenda

        ''' <summary>
        ''' Updates appointment data
        ''' </summary>
        ''' <param name="appointment">Agenda Model</param>
        ''' <remarks></remarks>
        Sub UpdateAppointmentData(appointment As Agenda)

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
        Sub RemoveAppointment(appointment As Agenda)

    End Interface

End Namespace
