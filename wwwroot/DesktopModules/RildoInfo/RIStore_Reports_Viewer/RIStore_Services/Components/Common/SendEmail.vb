
Imports System.Globalization
Imports System.Linq
Imports System.Net.Mail
Imports System.Net.Mime
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Mail
Imports DotNetNuke.Security.Roles.Internal
Imports DotNetNuke.Entities.Profile
Imports System.Web

Namespace RI.Modules.RIStore_Services

    Public Class SendEmail
        Implements IDisposable

#Region "Private Members"

        Private ReadOnly _addressedRoles As New List(Of String)()
        Private ReadOnly _addressedUsers As New List(Of Users.UserInfo)()
        Private ReadOnly _attachments As New List(Of Attachment)()

        Private _replyToUser As Users.UserInfo
        Private _smtpEnableSSL As Boolean
        Private _portalSettings As Portals.PortalSettings
        Private _sendingUser As Users.UserInfo
        Private _strSenderLanguage As String

        Private _smtpAuthenticationMethod As String = ""
        Private _smtpPassword As String = ""
        Private _smtpServer As String = ""
        Private _smtpPort As Integer
        Private _smtpUsername As String = ""

        Private _mailBody As String
        Private _isDisposed As Boolean

        Private _Msg As String

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' Priority of emails to be sent
        ''' </summary>
        Public Property Priority() As DotNetNuke.Services.Mail.MailPriority

        ''' <summary>
        ''' Subject of the emails to be sent
        ''' </summary>
        ''' <remarks>may contain tokens</remarks>
        Public Property Subject() As String

        ''' <summary>
        ''' body text of the email to be sent
        ''' </summary>
        ''' <remarks>may contain HTML tags and tokens. Side effect: sets BodyFormat autmatically</remarks>
        Public Property Body() As String
            Get
                Return _mailBody
            End Get
            Set(value As String)
                _mailBody = value
                BodyFormat = If(HtmlUtils.IsHtml(_mailBody), MailFormat.Html, MailFormat.Text)
            End Set
        End Property

        ''' <summary>format of body text for the email to be sent.</summary>
        ''' <remarks>by default activated, if tokens are found in Body and subject.</remarks>
        Public Property BodyFormat() As MailFormat

        Public ReadOnly Property Msg As String
            Get
                Return _Msg
            End Get
        End Property

        ''' <summary>portal alias http path to be used for links to images, ...</summary>
        Public Property PortalAlias() As String

        ''' <summary>UserInfo of the user sending the mail</summary>
        ''' <remarks>if not set explicitely, currentuser will be used</remarks>
        Public Property SendingUser() As Users.UserInfo
            Get
                Return _sendingUser
            End Get
            Set(value As Users.UserInfo)
                _sendingUser = value
                If _sendingUser.Profile.PreferredLocale IsNot Nothing Then
                    _strSenderLanguage = _sendingUser.Profile.PreferredLocale
                Else
                    Dim portalSettings = Portals.PortalController.GetCurrentPortalSettings()
                    _strSenderLanguage = portalSettings.DefaultLanguage
                End If
            End Set
        End Property

        ''' <summary>email of the user to be shown in the mail as replyTo address</summary>
        ''' <remarks>if not set explicitely, sendingUser will be used</remarks>
        Public Property ReplyTo() As Users.UserInfo
            Get
                Return If(_replyToUser, SendingUser)
            End Get
            Set(value As Users.UserInfo)
                _replyToUser = value
            End Set
        End Property

        ''' <summary>shall duplicate email addresses be ignored? (default value: false)</summary>
        ''' <remarks>Duplicate Users (e.g. from multiple role selections) will always be ignored.</remarks>
        Public Property RemoveDuplicates() As Boolean

        Public Property LanguageFilter() As String()

#End Region

#Region "Constructs"

        Public Sub New()
            BodyFormat = MailFormat.Text
            Subject = ""
            Priority = DotNetNuke.Services.Mail.MailPriority.Normal
            Initialize()
        End Sub

        Public Sub New(addressedRoles As List(Of String), addressedUsers As List(Of Users.UserInfo), _subject As String, _body As String)

            BodyFormat = MailFormat.Text
            Priority = DotNetNuke.Services.Mail.MailPriority.Normal
            _addressedRoles = addressedRoles
            _addressedUsers = addressedUsers
            Subject = _subject
            Body = _body

            Initialize()
        End Sub

#End Region

#Region "Private Methods"

        Private Sub Initialize()
            _portalSettings = Portals.PortalController.GetCurrentPortalSettings()
            PortalAlias = _portalSettings.PortalAlias.HTTPAlias
            SendingUser = DirectCast(HttpContext.Current.Items("UserInfo"), Users.UserInfo)

            _smtpEnableSSL = Host.Host.EnableSMTPSSL
        End Sub

        Private Function LoadAttachments() As List(Of Attachment)
            Dim attachments = New List(Of Attachment)()
            For Each attachment In _attachments
                Dim buffer = New Byte(4095) {}
                Dim memoryStream = New IO.MemoryStream()
                While True
                    Dim read = attachment.ContentStream.Read(buffer, 0, 4096)
                    If read <= 0 Then
                        Exit While
                    End If
                    memoryStream.Write(buffer, 0, read)
                End While

                Dim newAttachment = New Attachment(memoryStream, attachment.ContentType)
                newAttachment.ContentStream.Position = 0
                attachments.Add(newAttachment)
                'reset original position
                attachment.ContentStream.Position = 0
            Next

            Return attachments
        End Function

        ''' <summary>add a user to the userlist, if it is not already in there</summary>
        ''' <param name="user">user to add</param>
        ''' <param name="keyList">list of key (either email addresses or userid's)</param>
        ''' <param name="userList">List of users</param>
        ''' <remarks>for use by Recipients method only</remarks>
        Private Sub ConditionallyAddUser(user As Users.UserInfo, ByRef keyList As List(Of String), ByRef userList As List(Of Users.UserInfo))
            If ((user.UserID <= 0 OrElse user.Membership.Approved) AndAlso user.Email <> String.Empty) AndAlso MatchLanguageFilter(user.Profile.PreferredLocale) Then
                Dim key As String
                If RemoveDuplicates OrElse user.UserID = Null.NullInteger Then
                    key = user.Email
                Else
                    key = user.UserID.ToString(CultureInfo.InvariantCulture)
                End If
                If key <> String.Empty AndAlso Not keyList.Contains(key) Then
                    userList.Add(user)
                    keyList.Add(key)
                End If
            End If
        End Sub

        ''' <summary>check, if the user's language matches the current language filter</summary>
        ''' <param name="userLanguage">Language of the user</param>
        ''' <returns>userlanguage matches current languageFilter</returns>
        ''' <remarks>if filter not set, true is returned</remarks>
        Private Function MatchLanguageFilter(userLanguage As String) As Boolean
            If LanguageFilter Is Nothing OrElse LanguageFilter.Length = 0 Then
                Return True
            End If

            If String.IsNullOrEmpty(userLanguage) Then
                userLanguage = _portalSettings.DefaultLanguage
            End If

            Return LanguageFilter.Any(Function(s) userLanguage.ToLowerInvariant().StartsWith(s.ToLowerInvariant()))
        End Function

#End Region

#Region "Public Methods"

        ''' <summary>Specify SMTP server to be used</summary>
        ''' <param name="smtpServer">name of the SMTP server</param>
        ''' <param name="smtpAuthentication">authentication string (0: anonymous, 1: basic, 2: NTLM)</param>
        ''' <param name="smtpUsername">username to log in SMTP server</param>
        ''' <param name="smtpPassword">password to log in SMTP server</param>
        ''' <param name="smtpEnableSSL">SSL used to connect tp SMTP server</param>
        ''' <returns>always true</returns>
        ''' <remarks>if not called, values will be taken from host settings</remarks>
        Public Function SetSMTPServer(smtpServer As String, smtpPort As Integer, smtpAuthentication As String, smtpUsername As String, smtpPassword As String, smtpEnableSSL As Boolean) As Boolean
            EnsureNotDisposed()

            _smtpServer = smtpServer
            _smtpPort = smtpPort
            _smtpAuthenticationMethod = smtpAuthentication
            _smtpUsername = smtpUsername
            _smtpPassword = smtpPassword
            _smtpEnableSSL = smtpEnableSSL
            Return True
        End Function


        ''' <summary>Add a single attachment file to the email</summary>
        ''' <param name="localPath">path to file to attach</param>
        ''' <remarks>only local stored files can be added with a path</remarks>
        Public Sub AddAttachment(localPath As String)
            EnsureNotDisposed()
            _attachments.Add(New Attachment(localPath))
        End Sub

        Public Sub AddAttachment(contentStream As IO.Stream, contentType As ContentType)
            EnsureNotDisposed()
            _attachments.Add(New Attachment(contentStream, contentType))
        End Sub

        ''' <summary>Add a single recipient</summary>
        ''' <param name="recipient">userinfo of user to add</param>
        ''' <remarks>emaiol will be used for addressing, other properties might be used for TokenReplace</remarks>
        Public Sub AddAddressedUser(recipient As Users.UserInfo)
            EnsureNotDisposed()
            _addressedUsers.Add(recipient)
        End Sub

        ''' <summary>Add all members of a role to recipient list</summary>
        ''' <param name="roleName">name of a role, whose members shall be added to recipients</param>
        ''' <remarks>emaiol will be used for addressing, other properties might be used for TokenReplace</remarks>
        Public Sub AddAddressedRole(roleName As String)
            EnsureNotDisposed()
            _addressedRoles.Add(roleName)
        End Sub

        ''' <summary>All bulk mail recipients, derived from role names and individual adressees </summary>
        ''' <returns>List of userInfo objects, who receive the bulk mail </returns>
        ''' <remarks>user.Email used for sending, other properties might be used for TokenReplace</remarks>
        Public Function Recipients() As List(Of Users.UserInfo)
            EnsureNotDisposed()

            Dim userList = New List(Of Users.UserInfo)()
            Dim keyList = New List(Of String)()
            Dim roleController = New DotNetNuke.Security.Roles.RoleController()

            For Each roleName As String In _addressedRoles
                Dim role As String = roleName
                Dim roleInfo = TestableRoleController.Instance.GetRole(_portalSettings.PortalId, Function(r) r.RoleName = role)

                For Each objUser As Users.UserInfo In roleController.GetUsersByRoleName(_portalSettings.PortalId, roleName)
                    Dim user As Users.UserInfo = objUser
                    ProfileController.GetUserProfile(user)
                    Dim userRole = roleController.GetUserRole(_portalSettings.PortalId, objUser.UserID, roleInfo.RoleID)
                    'only add if user role has not expired and effectivedate has been passed
                    If (userRole.EffectiveDate <= DateTime.Now OrElse Null.IsNull(userRole.EffectiveDate)) AndAlso (userRole.ExpiryDate >= DateTime.Now OrElse Null.IsNull(userRole.ExpiryDate)) Then
                        ConditionallyAddUser(objUser, keyList, userList)
                    End If
                Next
            Next

            For Each objUser As Users.UserInfo In _addressedUsers
                ConditionallyAddUser(objUser, keyList, userList)
            Next

            Return userList
        End Function

        Public Sub SendMails()
            Try

                Dim mail As New MailMessage() With {.From = New MailAddress(SendingUser.Email)}
                ' mail.Headers.Add("In-Reply-To", 1)

                For Each _recipient In _addressedUsers
                    mail.[To].Add(New MailAddress(_recipient.Email, _recipient.DisplayName))
                Next

                For Each attchment In LoadAttachments()
                    mail.Attachments.Add(attchment)
                Next

                mail.ReplyToList.Add(New MailAddress(ReplyTo.Email))
                mail.Subject = Subject
                mail.Body = Body
                mail.IsBodyHtml = (BodyFormat = MailFormat.Html)

                'added support for multipart html messages
                'add text part as alternate view
                Dim PlainView = AlternateView.CreateAlternateViewFromString(DotNetNuke.Services.Mail.Mail.ConvertToText(Body), Nothing, "text/plain")
                mail.AlternateViews.Add(PlainView)
                If mail.IsBodyHtml Then
                    Dim HTMLView = AlternateView.CreateAlternateViewFromString(Body, Nothing, "text/html")
                    mail.AlternateViews.Add(HTMLView)
                End If

                Using SmtpServer As New SmtpClient(_smtpServer, _smtpPort) With {.EnableSsl = _smtpEnableSSL, .UseDefaultCredentials = False, .Credentials = New System.Net.NetworkCredential(_smtpUsername, _smtpPassword)}
                    SmtpServer.Send(mail)
                End Using

                _Msg = " Check Your Mail "
            Catch ex As Exception
                _Msg = ex.Message
            End Try
        End Sub

        ''' <summary>Wrapper for Function SendMails</summary>
        Public Sub Send()
            EnsureNotDisposed()
            SendMails()
        End Sub

#End Region

#Region "IDisposable Support"

        Private Sub EnsureNotDisposed()
            If _isDisposed Then
                Throw New ObjectDisposedException("SharedDictionary")
            End If
        End Sub

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not _isDisposed Then
                If disposing Then
                    'get rid of managed resources
                    For Each attachment As Attachment In _attachments
                        attachment.Dispose()
                        _isDisposed = True
                    Next
                    ' get rid of unmanaged resources
                End If
            End If
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Protected Overrides Sub Finalize()
            Try
                Dispose(False)
            Finally
                MyBase.Finalize()
            End Try
        End Sub

#End Region

    End Class

End Namespace