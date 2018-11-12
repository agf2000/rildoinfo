Imports DotNetNuke.Instrumentation
Imports System.Net
Imports System.Net.Mail
Imports System.ComponentModel
Imports System.IO

Public Class PostOffice

    Private Shared ReadOnly logger As ILog = LoggerSource.Instance.GetLogger(GetType(PostOffice))

    Public Shared Sub SendMail(mm As MailMessage, recipients As List(Of UserInfo), smtpServer As String, smtpPort As Integer, smtpConnection As Boolean, smtpLogin As String, smtpPassword As String)
        For Each recipient In recipients
            mm.To.Clear()
            mm.To.Add(New MailAddress(recipient.Email.Replace("[", "").Replace("}", ""), recipient.DisplayName))
            Using smtp As New SmtpClient()
                smtp.Host = smtpServer
                smtp.EnableSsl = smtpConnection
                Dim NetworkCred As New NetworkCredential(smtpLogin, smtpPassword)
                smtp.UseDefaultCredentials = True
                smtp.Credentials = NetworkCred
                smtp.Port = smtpPort
                Try
                    smtp.Send(mm)
                Catch failedRec As SmtpFailedRecipientsException
                    'DnnLog.Error(failedRec)
                    logger.[Error](failedRec)
                Catch smtpExc As SmtpException
                    'DnnLog.Error(smtpExc)
                    logger.[Error](smtpExc)
                Catch ex As Exception
                    'DnnLog.Error(ex)
                    logger.[Error](ex)
                End Try
            End Using
        Next
    End Sub

    'Implements IDisposable

    'Public sentEmails As Integer
    'Public smtpClient As New Net.Mail.SmtpClient()

    'Public Function SendMail(mailMessage As Net.Mail.MailMessage, recipients As List(Of Users.UserInfo), smtpServer As String, smtpPort As Integer, smtpConnection As Boolean, smtpLogin As String, smtpPassword As String) As Integer

    '    AddHandler smtpClient.SendCompleted, AddressOf SmtpClient_OnCompleted

    '    smtpClient.Host = smtpServer
    '    smtpClient.Port = smtpPort
    '    smtpClient.EnableSsl = smtpConnection
    '    smtpClient.Credentials = New Net.NetworkCredential(smtpLogin, smtpPassword)

    '    sentEmails = 0

    '    For Each recipient In recipients
    '        mailMessage.To.Clear()
    '        mailMessage.To.Add(New Net.Mail.MailAddress(recipient.Email.Replace("[", "").Replace("}", ""), recipient.DisplayName))
    '        Dim userState As Object = mailMessage
    '        Try
    '            smtpClient.SendAsync(mailMessage, userState)
    '        Catch failedRec As Net.Mail.SmtpFailedRecipientsException
    '            DnnLog.Error(failedRec)
    '        Catch smtpExc As Net.Mail.SmtpException
    '            DnnLog.Error(smtpExc)
    '        Catch ex As Exception
    '            DnnLog.Error(ex)
    '        End Try
    '    Next

    '    Return sentEmails
    'End Function

    'Private Sub SendMessage(mailMessage As Net.Mail.MailMessage)

    '    Dim userState As Object = mailMessage

    '    Try
    '        smtpClient.SendAsync(mailMessage, userState)
    '    Catch failedRec As Net.Mail.SmtpFailedRecipientsException
    '        DnnLog.Error(failedRec)
    '    Catch smtpExc As Net.Mail.SmtpException
    '        DnnLog.Error(smtpExc)
    '    Catch ex As Exception
    '        DnnLog.Error(ex)
    '    End Try

    'End Sub

    'Public Sub SmtpClient_OnCompleted(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs)

    '    Dim mailMessage As Net.Mail.MailMessage = CType(e.UserState, Net.Mail.MailMessage)

    '    If (e.Error Is Nothing) Then
    '        sentEmails = sentEmails + 1
    '        DnnLog.Info("Foi enviado um total de {0} email(s)", sentEmails)
    '    Else
    '        DnnLog.Error("Error")
    '    End If

    'End Sub

    'Public Sub Dispose() Implements IDisposable.Dispose
    '    Dispose(True)
    '    GC.SuppressFinalize(Me)
    'End Sub

    'Protected Overridable Sub Dispose(ByVal disposing As Boolean)
    '    If disposing Then
    '        If smtpClient IsNot Nothing Then
    '            smtpClient.Dispose()
    '            smtpClient = Nothing
    '        End If
    '    End If
    'End Sub

    'Sub Finalize()
    '    Dispose(False)
    'End Sub

End Class
