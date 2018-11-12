
Namespace RI.Modules.RIStore_Services.Models
    Public Interface IAgenda

#Region "Agenda Variables"

        Property PortalId() As Integer

        Property AppointmentId() As Integer

        Property ClientId() As Integer

        Property UserId() As Integer

        Property Subject() As String

        Property Description() As String

        Property StartDateTime() As Date

        Property EndDateTime() As Date

        Property Reminder() As String

        Property RecurrenceRule() As String

        Property RecurrenceParentId() As Integer

        Property Annotations() As String

        Property SentEmails() As String

        Property DocId() As Integer

        Property Scheduled() As Boolean

        Property CreatedByUser() As Integer

        Property CreatedOnDate() As Date

        Property ModifiedByUser() As Integer

        Property ModifiedOnDate() As Date

        Property Currency() As String

#End Region

    End Interface
End Namespace
