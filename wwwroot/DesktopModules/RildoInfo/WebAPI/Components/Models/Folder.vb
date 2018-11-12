Imports RIW.Modules.WebAPI.Components.Interfaces.Models
 
Namespace Components.Models
    Public Class Folder
        Implements IFolder

        Public Property FolderID As Integer Implements IFolder.FolderID

        Public Property FolderName As String Implements IFolder.FolderName

        Public Property FolderPath As String Implements IFolder.FolderPath

        Public Property PortalId As Integer Implements IFolder.PortalId
    End Class
End Namespace
