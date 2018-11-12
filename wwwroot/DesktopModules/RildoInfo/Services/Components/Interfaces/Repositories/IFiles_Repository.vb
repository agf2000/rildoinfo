
Public Interface IFiles_Repository

    ''' <summary>
    ''' Deletes a file
    ''' </summary>
    ''' <param name="fileName">File Name</param>
    ''' <remarks>all the items belong at the same Folder</remarks>
    ''' <returns>The not deleted items list. The subfiles / subfolders for which the user has no permissions to delete</returns>
    Function DeleteFile(fileName As String) As String

End Interface
