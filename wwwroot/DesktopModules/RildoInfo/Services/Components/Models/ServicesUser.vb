
Namespace Models
    Public Class ServicesUser
        Implements IServicesUser

        Public Property AffiliateID As Integer Implements IServicesUser.AffiliateID

        Public Property DisplayName As String Implements IServicesUser.DisplayName

        Public Property Email As String Implements IServicesUser.Email

        Public Property FirstName As String Implements IServicesUser.FirstName

        Public Property IsSuperUser As Boolean Implements IServicesUser.IsSuperUser

        Public Property LastIPAddress As String Implements IServicesUser.LastIPAddress

        Public Property LastName As String Implements IServicesUser.LastName

        Public Property PortalID As Integer Implements IServicesUser.PortalID

        Public Property Roles As String() Implements IServicesUser.Roles

        Public Property UserID As Integer Implements IServicesUser.UserID

        Public Property Username As String Implements IServicesUser.Username
    End Class
End Namespace