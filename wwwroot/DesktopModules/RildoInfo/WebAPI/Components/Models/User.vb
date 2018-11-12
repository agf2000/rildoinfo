
Imports DotNetNuke.Entities.Profile
Imports System.Threading
Imports DotNetNuke.Services.Tokens

Namespace Models

    Public Class User
        Private _user As UserInfo
        Private _viewer As UserInfo
        Private _settings As PortalSettings

        Public Sub New(user As UserInfo, settings As PortalSettings)
            _user = user
            _settings = settings
            _viewer = settings.UserInfo
        End Sub

        Public ReadOnly Property MemberId() As Integer
            Get
                Return _user.UserID
            End Get
        End Property

        Public ReadOnly Property LastPasswordChangeDate() As DateTime
            Get
                Return _user.LastModifiedOnDate
            End Get
        End Property

        Public ReadOnly Property City() As String
            Get
                Return GetProfileProperty("City")
            End Get
        End Property

        Public ReadOnly Property Country() As String
            Get
                Return GetProfileProperty("Country")
            End Get
        End Property

        Public ReadOnly Property DisplayName() As String
            Get
                Return _user.DisplayName
            End Get
        End Property

        Public ReadOnly Property Email() As String
            Get
                Return _user.Email ' If((_viewer.IsInRole(_settings.AdministratorRoleName)), _user.Email, [String].Empty)
            End Get
        End Property

        Public ReadOnly Property FirstName() As String
            Get
                Return GetProfileProperty("FirstName")
            End Get
        End Property

        Public ReadOnly Property FollowerStatus() As Integer
            Get
                Return If((_user.Social.Follower Is Nothing), 0, CInt(_user.Social.Follower.Status))
            End Get
        End Property

        Public ReadOnly Property FollowingStatus() As Integer
            Get
                Return If((_user.Social.Following Is Nothing), 0, CInt(_user.Social.Following.Status))
            End Get
        End Property

        Public ReadOnly Property FriendId() As Integer
            Get
                Return If((_user.Social.[Friend] Is Nothing), -1, CInt(_user.Social.[Friend].RelatedUserId))
            End Get
        End Property

        Public ReadOnly Property FriendStatus() As Integer
            Get
                Return If((_user.Social.[Friend] Is Nothing), 0, CInt(_user.Social.[Friend].Status))
            End Get
        End Property

        Public ReadOnly Property LastName() As String
            Get
                Return GetProfileProperty("LastName")
            End Get
        End Property

        Public ReadOnly Property Phone() As String
            Get
                Return GetProfileProperty("Telephone")
            End Get
        End Property

        Public ReadOnly Property PhotoURL() As String
            Get
                Return _user.Profile.PhotoURL
            End Get
        End Property

        Public ReadOnly Property ProfileProperties() As Dictionary(Of String, String)
            Get
                Dim properties = New Dictionary(Of String, String)()
                Dim propertyNotFound As Boolean = False
                Dim propertyAccess = New ProfilePropertyAccess(_user)
                For Each [property] As ProfilePropertyDefinition In _user.Profile.ProfileProperties
                    Dim value As String = propertyAccess.GetProperty([property].PropertyName, [String].Empty, Thread.CurrentThread.CurrentUICulture, _user, Scope.DefaultSettings, propertyNotFound)

                    properties([property].PropertyName) = System.Web.HttpUtility.HtmlDecode(value)
                Next
                Return properties
            End Get
        End Property

        Public ReadOnly Property Locked() As Boolean
            Get
                Return False
            End Get
        End Property

        Public ReadOnly Property Title() As String
            Get
                Return _user.Profile.Title
            End Get
        End Property

        Public ReadOnly Property UserName() As String
            Get
                Return _user.Username ' If((_viewer.IsInRole(_settings.AdministratorRoleName)), _user.Username, [String].Empty)
            End Get
        End Property

        Public ReadOnly Property Website() As String
            Get
                Return GetProfileProperty("Website")
            End Get
        End Property

        ''' <summary>
        ''' This method returns the value of the ProfileProperty if is defined, otherwise it returns an Empty string
        ''' </summary>
        ''' <param name="propertyName">property name</param>
        ''' <returns>property value</returns>
        Private Function GetProfileProperty(propertyName As String) As String
            Dim _profileProperties = ProfileProperties
            Dim value As String = ""

            Return If(_profileProperties.TryGetValue(propertyName, value), value, String.Empty)
        End Function
    End Class
End Namespace