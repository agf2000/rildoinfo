
Imports DotNetNuke.ComponentModel.DataAnnotations

Namespace Models

    'setup the primary key for table
    'configure caching using PetaPoco
    'scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    <TableName("RIW_Agenda")> _
    <PrimaryKey("AppointmentId", AutoIncrement:=True)> _
    <Cacheable("Agenda", CacheItemPriority.Default, 20)> _
    <Scope("UserId")>
    Public Class Agenda
        Implements IAgenda

        Public Property Annotations As String Implements IAgenda.Annotations

        Public Property AppointmentId As Integer Implements IAgenda.AppointmentId

        Public Property PersonId As Integer Implements IAgenda.PersonId

        Public Property CreatedByUser As Integer Implements IAgenda.CreatedByUser

        Public Property CreatedOnDate As Date Implements IAgenda.CreatedOnDate

        Public Property Description As String Implements IAgenda.Description

        Public Property DocId As Integer Implements IAgenda.DocId

        Public Property EndDateTime As Date Implements IAgenda.EndDateTime

        Public Property ModifiedByUser As Integer Implements IAgenda.ModifiedByUser

        Public Property ModifiedOnDate As Date Implements IAgenda.ModifiedOnDate

        Public Property PortalId As Integer Implements IAgenda.PortalId

        Public Property RecurrenceParentId As Integer Implements IAgenda.RecurrenceParentId

        Public Property RecurrenceRule As String Implements IAgenda.RecurrenceRule

        Public Property Reminder As String Implements IAgenda.Reminder

        <IgnoreColumn> _
        Public Property Scheduled As Boolean Implements IAgenda.Scheduled

        Public Property SentEmails As String Implements IAgenda.SentEmails

        Public Property StartDateTime As Date Implements IAgenda.StartDateTime

        Public Property Subject As String Implements IAgenda.Subject

        Public Property UserId As Integer Implements IAgenda.UserId

        <IgnoreColumn> _
        Public Property Currency As String Implements IAgenda.Currency

        <IgnoreColumn> _
        Public Property HistoryText As String Implements IAgenda.HistoryText

        <IgnoreColumn> _
        Public Property Emails As String Implements IAgenda.Emails
    End Class
End Namespace
