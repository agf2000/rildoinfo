
Imports DotNetNuke.Services.Social.Notifications
Imports DotNetNuke.Security.Roles

Public Class Notifications

#Region "Subs and Functions"

    Friend Shared Sub SendNotification(_contentTypeName As Constants.ContentTypeName, _notificationTypeName As Constants.NotificationTypeName, _senderId As Integer, _recipient As Users.UserInfo, _role As String, _portalId As Integer, _estimateId As Integer, _subject As String, _body As String)

        Dim notificationType As NotificationType = NotificationsController.Instance.GetNotificationType(_notificationTypeName.ToString())

        Dim notificationKey = String.Empty
        If _recipient IsNot Nothing Then
            notificationKey = String.Format("{0}:{1}:{2}:{3}", _contentTypeName.ToString(), _notificationTypeName.ToString(), CStr(_estimateId), CStr(_recipient.UserID))
        Else
            notificationKey = String.Format("{0}:{1}:{2}", _contentTypeName.ToString(), _notificationTypeName.ToString(), CStr(_estimateId))
        End If

        Dim objNotify As Notification = NotificationsController.Instance.GetNotificationByContext(notificationType.NotificationTypeId, notificationKey).SingleOrDefault

        If objNotify Is Nothing Then

            Dim objNotification As New Notification

            objNotification.NotificationTypeID = notificationType.NotificationTypeId
            objNotification.Subject = _subject
            objNotification.Body = _body
            objNotification.IncludeDismissAction = True
            objNotification.Context = notificationKey
            'objNotification.ExpirationDate = DateTime.Now()

            If _senderId > 0 Then
                objNotification.SenderUserID = _senderId
            End If

            Dim colRoles As New List(Of RoleInfo)
            If _role <> "" Then
                Dim objRoleCtrl As New RoleController
                Dim objRoleInfo = objRoleCtrl.GetRoleByName(_portalId, _role)
                If Not Null.IsNull(objRoleInfo) Then
                    colRoles.Add(objRoleInfo)
                End If

            End If

            'Dim _recipientUser = Users.UserController.GetUserById(_portalId, _recipient)
            Dim colRecipients As New List(Of Users.UserInfo)
            If Not Null.IsNull(_recipient) Then
                colRecipients.Add(_recipient)
            End If

            NotificationsController.Instance.SendNotification(objNotification, 0, colRoles, colRecipients)

        End If

    End Sub

    Friend Shared Sub RemoveEstimateEntryNotification(_contentTypeName As Constants.ContentTypeName, _notificationTypeName As Constants.NotificationTypeName, ByVal _estimateId As Integer, _userId As Integer)

        Dim notificationType = NotificationsController.Instance.GetNotificationType(_notificationTypeName.ToString())

        Dim notificationKey = String.Empty
        If _userId > 0 Then
            notificationKey = String.Format("{0}:{1}:{2}:{3}", _contentTypeName.ToString(), _notificationTypeName.ToString(), CStr(_estimateId), CStr(_userId))
        Else
            notificationKey = String.Format("{0}:{1}:{2}", _contentTypeName.ToString(), _notificationTypeName.ToString(), CStr(_estimateId))
        End If

        Dim objNotify = NotificationsController.Instance.GetNotificationByContext(notificationType.NotificationTypeId, notificationKey).SingleOrDefault

        If objNotify IsNot Nothing Then
            NotificationsController.Instance.DeleteAllNotificationRecipients(objNotify.NotificationID)
        End If

    End Sub

    ' ''' <summary>
    ' ''' Sends an email
    ' ''' </summary>
    ' ''' <param name="fromUser">From</param>
    ' ''' <param name="toUser">To</param>
    ' ''' <param name="cctoUser">Carbon Copy</param>
    ' ''' <param name="bccToUser">Blind Carbon Copy</param>
    ' ''' <param name="_subject">Subject</param>
    ' ''' <param name="_body">Body</param>
    ' ''' <param name="_fileAttach">File Attachment</param>
    ' ''' <param name="_fileType">File Type</param>
    ' ''' <param name="_fileIdAttach">File Attachment ID</param>
    ' ''' <param name="_bodyType">Body Type (html or plain text)</param>
    ' ''' <param name="_priority">Email Priority</param>
    ' ''' <param name="sendUser">Sender</param>
    ' ''' <param name="_portalId">Portal ID</param>
    ' ''' <param name="useStoreSMTP">USe store SMTP settings</param>
    ' ''' <remarks></remarks>
    'Public Shared Sub SendStoreEmail(fromUser As Users.UserInfo, toUser As Users.UserInfo, cctoUser As Users.UserInfo, bccToUser As Users.UserInfo, _subject As String, _body As String, _fileAttach As IO.Stream, _fileType As String, _fileIdAttach As Integer, _bodyType As DotNetNuke.Services.Mail.MailFormat, _priority As DotNetNuke.Services.Mail.MailPriority, sendUser As Users.UserInfo, _portalId As Integer, useStoreSMTP As Boolean)

    '    Using objEmail As New SendEmail() With {.Body = _body, .Subject = _subject, .BodyFormat = _bodyType, .Priority = _priority}

    '        Dim _smtpServer = Null.NullString
    '        Dim _smtpPort = Null.NullInteger
    '        Dim _authType = Null.NullString
    '        Dim _smtpLogin = Null.NullString
    '        Dim _smtpPassword = Null.NullString
    '        Dim _smtpSSL = Null.NullBoolean

    '        If useStoreSMTP Then
    '            Dim myPortalSettings = Portals.PortalController.GetPortalSettingsDictionary(_portalId)
    '            _smtpServer = SettingsDictionay("smtpServer")
    '            _smtpPort = CInt(SettingsDictionay("smtpPort"))
    '            _authType = "1"
    '            _smtpLogin = SettingsDictionay("smtpLogin")
    '            _smtpPassword = SettingsDictionay("smtpPassword")
    '            _smtpSSL = CBool(SettingsDictionay("smtpConnection"))
    '        End If

    '        objEmail.SetSMTPServer(_smtpServer, _smtpPort, _authType, _smtpLogin, _smtpPassword, _smtpSSL)

    '        objEmail.SendingUser = sendUser
    '        objEmail.ReplyTo = fromUser
    '        objEmail.AddAddressedUser(toUser)

    '        If cctoUser IsNot Nothing Then
    '            objEmail.AddAddressedUser(cctoUser)
    '        End If

    '        If _fileType IsNot Nothing Then
    '            Dim mimeType = New ContentType(_fileType)
    '            objEmail.AddAttachment(_fileAttach, mimeType)
    '        End If

    '        If _fileIdAttach > 0 Then
    '            Dim objFileInfo = FileManager.Instance.GetFile(_fileIdAttach)
    '            objEmail.AddAttachment(FileManager.Instance.GetFileContent(objFileInfo), New ContentType() With {.MediaType = objFileInfo.ContentType, .Name = objFileInfo.FileName})
    '        End If

    '        'objEmail.Send()
    '        Dim objThread As New Thread(AddressOf objEmail.Send)
    '        objThread.Start()
    '    End Using

    'End Sub

    '        Public Shared Sub SendStoreEmail(fromUser As Users.UserInfo, toUser As Users.UserInfo, cc As String, bcc As String, _subject As String, _body As String, _fileAttach As IO.Stream, _fileType As String, _fileIdAttach As Integer, _bodyType As DotNetNuke.Services.Mail.MailFormat, _priority As DotNetNuke.Services.Mail.MailPriority, sendUser As Users.UserInfo, _smtpServer As String, _smtpPort As Integer, _authType As String, _smtpLogin As String, _smtpPassword As String, _smtpSSL As Boolean)

    '            Using objEmail As New SendEmail() With {.Body = _body, .Subject = _subject, .BodyFormat = _bodyType, .Priority = _priority}

    '                objEmail.SetSMTPServer(_smtpServer, _smtpPort, _authType, _smtpLogin, _smtpPassword, _smtpSSL)

    '                objEmail.SendingUser = sendUser
    '                objEmail.ReplyTo = fromUser
    '                objEmail.AddAddressedUser(toUser)

    '                If _fileType IsNot Nothing Then
    '                    Dim mimeType = New ContentType(_fileType)
    '                    objEmail.AddAttachment(_fileAttach, mimeType)
    '                End If

    '                If _fileIdAttach > 0 Then
    '                    Dim objFileInfo = FileManager.Instance.GetFile(_fileIdAttach)
    '                    objEmail.AddAttachment(FileManager.Instance.GetFileContent(objFileInfo), New ContentType() With {.MediaType = objFileInfo.ContentType, .Name = objFileInfo.FileName _
    '})
    '                End If

    '                'objEmail.Send()
    '                Dim objThread As New Thread(AddressOf objEmail.Send)
    '                objThread.Start()
    '            End Using

    '        End Sub

#End Region

#Region "Install"

    ''' <summary>
    ''' This will create a notification type associated w/ the module and also handle the actions that must be associated with it.
    ''' </summary>
    ''' <remarks>This should only ever run once, during 5.0.0 install (via IUpgradeable)</remarks>
    Friend Shared Sub AddNotificationTypes()

        Dim values = [Enum].GetNames(GetType(Constants.NotificationTypeName))

        For Each notificationType In values
            Dim objNotificationType As NotificationType = New NotificationType() With {
                .Name = notificationType,
                .Description = "RIW"}
            'objNotificationType.TimeToLive = New TimeSpan(0, 5, 0)

            If NotificationsController.Instance.GetNotificationType(objNotificationType.Name) Is Nothing Then
                'Dim objAction As New NotificationTypeAction
                'objAction.NameResourceKey = "OpenNewEstimate"
                'objAction.DescriptionResourceKey = "OpenNewEstimate_Desc"
                'objAction.APICall = "/DesktopModules/store/API/RIStoreService.ashx/OpenNewEstimate"
                'objAction.Order = 1
                'actions.Add(objAction)

                'objAction = New NotificationTypeAction
                'objAction.NameResourceKey = "SeeDeliveries"
                'objAction.DescriptionResourceKey = "SeeDeliveries_Desc"
                'objAction.APICall = "Entregas/EntregasProgramadas"
                'objAction.Order = 3
                'actions.Add(objAction)

                NotificationsController.Instance.CreateNotificationType(objNotificationType)
                'NotificationsController.Instance.SetNotificationTypeActions(actions, objNotificationType.NotificationTypeId)
            End If
        Next

    End Sub

#End Region

End Class
