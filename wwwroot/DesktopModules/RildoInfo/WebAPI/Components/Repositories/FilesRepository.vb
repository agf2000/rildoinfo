﻿
Public Class FilesRepository
    Implements IFilesRepository

    Function DeleteFile(fileName As String) As String Implements IFilesRepository.DeleteFile
        Dim ret As String = ""

        If IO.File.Exists(fileName) = True Then
            Dim timedOut As Boolean = False
            Dim deleted As Boolean = False
            Dim starttime As DateTime = Now
            Do Until deleted OrElse timedOut
                Try
                    IO.File.Delete(fileName)
                    deleted = True
                Catch ex As Exception
                    deleted = False
                    timedOut = DateAdd(DateInterval.Second, 2, starttime) < Now
                    If timedOut Then
                        ret = "Could not delete destination File. Error: " + ex.Message
                        Exit Do
                    End If
                End Try
            Loop
        End If

        Return ret
    End Function

End Class
