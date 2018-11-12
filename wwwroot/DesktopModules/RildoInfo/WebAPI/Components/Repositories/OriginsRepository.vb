Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Repositories

    Public Class OriginsRepository
    Implements IOriginsRepository

        Public Function AddOrigin(origin As Origin) As Origin Implements IOriginsRepository.AddOrigin
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Origin) = ctx.GetRepository(Of Origin)()
                rep.Insert(origin)
            End Using
            Return origin
        End Function

        Public Function GetOrigins(portalId As Integer) As IEnumerable(Of Origin) Implements IOriginsRepository.GetOrigins
            Return CBO.FillCollection(Of Origin)(DataProvider.Instance().GetOrigins(portalId))
        End Function

        Public Sub UpdateOrigin(origin As Origin) Implements IOriginsRepository.UpdateOrigin
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Origin) = ctx.GetRepository(Of Origin)()
                rep.Update(origin)
            End Using
        End Sub

        Public Sub RemoveOrigin(originId As Integer, portalId As Integer) Implements IOriginsRepository.RemoveOrigin
            Dim item As Origin = GetOrigin(originId, portalId)
            If item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveOrigin(item)
            End If
        End Sub

        Public Function GetOrigin(ByVal originId As Integer, ByVal portalId As Integer) As Origin Implements IOriginsRepository.GetOrigin
            Return CBO.FillObject(Of Origin)(DataProvider.Instance().GetOrigin(originId, portalId))
        End Function

        Public Sub RemoveOrigin(origin As Origin) Implements IOriginsRepository.RemoveOrigin
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Origin) = ctx.GetRepository(Of Origin)()
                rep.Delete(origin)
            End Using
        End Sub
    End Class

End Namespace
