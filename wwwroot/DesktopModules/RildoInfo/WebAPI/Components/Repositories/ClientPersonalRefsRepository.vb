Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models

Namespace Components.Repositories

    Public Class ClientPersonalRefsRepository
    Implements IClientPersonalRefsRepository

        Public Function AddClientPersonalRef(clientPersonalRef As ClientPersonalRef) As ClientPersonalRef Implements IClientPersonalRefsRepository.AddClientPersonalRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientPersonalRef) = ctx.GetRepository(Of ClientPersonalRef)()
                rep.Insert(clientPersonalRef)
            End Using
            Return clientPersonalRef
        End Function

        Public Function GetClientPersonalRef(clientPersonalRefId As Integer, personId As Integer) As ClientPersonalRef Implements IClientPersonalRefsRepository.GetClientPersonalRef
            Dim clientPersonalRef As ClientPersonalRef

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientPersonalRef) = ctx.GetRepository(Of ClientPersonalRef)()
                clientPersonalRef = rep.GetById(Of Int32, Int32)(clientPersonalRefId, personId)
            End Using
            Return clientPersonalRef
        End Function

        Public Function GetClientPersonalRefs(personId As Integer) As IEnumerable(Of ClientPersonalRef) Implements IClientPersonalRefsRepository.GetClientPersonalRefs
            Dim clientPersonalRefs As IEnumerable(Of ClientPersonalRef)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientPersonalRef) = ctx.GetRepository(Of ClientPersonalRef)()
                clientPersonalRefs = rep.Get(personId)
            End Using
            Return clientPersonalRefs
        End Function

        Public Sub RemoveClientPersonalRef(clientPersonalRefId As Integer, personId As Integer) Implements IClientPersonalRefsRepository.RemoveClientPersonalRef
            Dim item As ClientPersonalRef = GetClientPersonalRef(clientPersonalRefId, personId)
            If item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveClientPersonalRef(item)
            End If
        End Sub

        Public Sub RemoveClientPersonalRef(clientPersonalRef As ClientPersonalRef) Implements IClientPersonalRefsRepository.RemoveClientPersonalRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientPersonalRef) = ctx.GetRepository(Of ClientPersonalRef)()
                rep.Delete(clientPersonalRef)
            End Using
        End Sub

        Public Sub RemoveClientPersonalRefs(personId As Integer) Implements IClientPersonalRefsRepository.RemoveClientPersonalRefs
            Dim clientPersonalRefs As IEnumerable(Of ClientPersonalRef)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of ClientPersonalRef)()
                clientPersonalRefs = rep.Find("Where PersonId = @0", personId)

                For Each personalRef In clientPersonalRefs
                    RemoveClientPersonalRef(personalRef)
                Next
            End Using
        End Sub

        Public Sub UpdateClientPersonalRef(clientPersonalRef As ClientPersonalRef) Implements IClientPersonalRefsRepository.UpdateClientPersonalRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of ClientPersonalRef) = ctx.GetRepository(Of ClientPersonalRef)()
                rep.Update(clientPersonalRef)
            End Using
        End Sub

    End Class

End Namespace
