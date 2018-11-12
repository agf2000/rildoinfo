
Imports System.Collections.Generic
Imports DotNetNuke.Data

Namespace RI.Modules.RIStore_Services
    Public Class ClientPersonalRefsRepository
        Implements IClientPersonalRefsRepository

        Public Function AddClientPersonalRef(clientPersonalRef As Models.ClientPersonalRef) As Models.ClientPersonalRef Implements IClientPersonalRefsRepository.AddClientPersonalRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientPersonalRef) = ctx.GetRepository(Of Models.ClientPersonalRef)()
                rep.Insert(clientPersonalRef)
            End Using
            Return clientPersonalRef
        End Function

        Public Function GetClientPersonalRef(clientPersonalRefId As Integer, clientId As Integer) As Models.ClientPersonalRef Implements IClientPersonalRefsRepository.GetClientPersonalRef
            Dim clientPersonalRef As Models.ClientPersonalRef

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientPersonalRef) = ctx.GetRepository(Of Models.ClientPersonalRef)()
                clientPersonalRef = rep.GetById(Of Int32, Int32)(clientPersonalRefId, clientId)
            End Using
            Return clientPersonalRef
        End Function

        Public Function GetClientPersonalRefs(clientId As Integer) As IEnumerable(Of Models.ClientPersonalRef) Implements IClientPersonalRefsRepository.GetClientPersonalRefs
            Dim clientPersonalRefs As IEnumerable(Of Models.ClientPersonalRef)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientPersonalRef) = ctx.GetRepository(Of Models.ClientPersonalRef)()
                clientPersonalRefs = rep.Get(clientId)
            End Using
            Return clientPersonalRefs
        End Function

        Public Sub RemoveClientPersonalRef(clientPersonalRefId As Integer, clientId As Integer) Implements IClientPersonalRefsRepository.RemoveClientPersonalRef
            Dim _item As Models.ClientPersonalRef = GetClientPersonalRef(clientPersonalRefId, clientId)
            If _item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveClientPersonalRef(_item)
            End If
        End Sub

        Public Sub RemoveClientPersonalRef(clientPersonalRef As Models.ClientPersonalRef) Implements IClientPersonalRefsRepository.RemoveClientPersonalRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientPersonalRef) = ctx.GetRepository(Of Models.ClientPersonalRef)()
                rep.Delete(clientPersonalRef)
            End Using
        End Sub

        Public Sub RemoveClientPersonalRefs(clientId As Integer) Implements IClientPersonalRefsRepository.RemoveClientPersonalRefs
            Dim clientPersonalRefs As IEnumerable(Of Models.ClientPersonalRef)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of Models.ClientPersonalRef)()
                clientPersonalRefs = rep.Find("Where ClientId = @0", clientId)

                For Each personalRef In clientPersonalRefs
                    RemoveClientPersonalRef(personalRef)
                Next
            End Using
        End Sub

        Public Sub UpdateClientPersonalRef(clientPersonalRef As Models.ClientPersonalRef) Implements IClientPersonalRefsRepository.UpdateClientPersonalRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientPersonalRef) = ctx.GetRepository(Of Models.ClientPersonalRef)()
                rep.Update(clientPersonalRef)
            End Using
        End Sub

    End Class
End Namespace