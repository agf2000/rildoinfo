Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models
 
Namespace Components.Repositories

    Public Class ClientCommRefsRepository
    Implements IClientCommRefsRepository

        Public Function AddClientCommRef(clientCommRef As ClientCommRef) As ClientCommRef Implements IClientCommRefsRepository.AddClientCommRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientCommRef) = ctx.GetRepository(Of ClientCommRef)()
                rep.Insert(clientCommRef)
            End Using
            Return clientCommRef
        End Function

        Public Function GetClientCommRef(clientCommRefId As Integer, personId As Integer) As ClientCommRef Implements IClientCommRefsRepository.GetClientCommRef
            Dim clientCommRef As ClientCommRef

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientCommRef) = ctx.GetRepository(Of ClientCommRef)()
                clientCommRef = rep.GetById(Of Int32, Int32)(clientCommRefId, personId)
            End Using
            Return clientCommRef
        End Function

        Public Function GetClientCommRefs(personId As Integer) As IEnumerable(Of ClientCommRef) Implements IClientCommRefsRepository.GetClientCommRefs
            Dim clientCommRefs As IEnumerable(Of ClientCommRef)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientCommRef) = ctx.GetRepository(Of ClientCommRef)()
                clientCommRefs = rep.Get(personId)
            End Using
            Return clientCommRefs
        End Function

        Public Sub RemoveClientCommRef(clientCommRefId As Integer, personId As Integer) Implements IClientCommRefsRepository.RemoveClientCommRef
            Dim item As ClientCommRef = GetClientCommRef(clientCommRefId, personId)
            If item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveClientCommRef(item)
            End If
        End Sub

        Public Sub RemoveClientCommRef(clientCommRef As ClientCommRef) Implements IClientCommRefsRepository.RemoveClientCommRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientCommRef) = ctx.GetRepository(Of ClientCommRef)()
                rep.Delete(clientCommRef)
            End Using
        End Sub

        Public Sub RemoveClientCommRefs(personId As Integer) Implements IClientCommRefsRepository.RemoveClientCommRefs
            Dim clientCommRefs As IEnumerable(Of ClientCommRef)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of ClientCommRef)()
                clientCommRefs = rep.Find("Where PersonId = @0", personId)

                For Each commRef In clientCommRefs
                    removeClientCommRef(commRef)
                Next
            End Using
        End Sub

        Public Sub UpdateClientCommRef(clientCommRef As ClientCommRef) Implements IClientCommRefsRepository.UpdateClientCommRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientCommRef) = ctx.GetRepository(Of ClientCommRef)()
                rep.Update(clientCommRef)
            End Using
        End Sub

    End Class

End Namespace
