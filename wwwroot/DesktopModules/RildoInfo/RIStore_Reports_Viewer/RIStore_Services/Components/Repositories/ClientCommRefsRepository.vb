
Imports System.Collections.Generic
Imports DotNetNuke.Data

Namespace RI.Modules.RIStore_Services
    Public Class ClientCommRefsRepository
        Implements IClientCommRefsRepository

        Public Function AddClientCommRef(clientCommRef As Models.ClientCommRef) As Models.ClientCommRef Implements IClientCommRefsRepository.AddClientCommRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientCommRef) = ctx.GetRepository(Of Models.ClientCommRef)()
                rep.Insert(clientCommRef)
            End Using
            Return clientCommRef
        End Function

        Public Function GetClientCommRef(clientCommRefId As Integer, clientId As Integer) As Models.ClientCommRef Implements IClientCommRefsRepository.GetClientCommRef
            Dim clientCommRef As Models.ClientCommRef

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientCommRef) = ctx.GetRepository(Of Models.ClientCommRef)()
                clientCommRef = rep.GetById(Of Int32, Int32)(clientCommRefId, clientId)
            End Using
            Return clientCommRef
        End Function

        Public Function GetClientCommRefs(clientId As Integer) As IEnumerable(Of Models.ClientCommRef) Implements IClientCommRefsRepository.GetClientCommRefs
            Dim clientCommRefs As IEnumerable(Of Models.ClientCommRef)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientCommRef) = ctx.GetRepository(Of Models.ClientCommRef)()
                clientCommRefs = rep.Get(clientId)
            End Using
            Return clientCommRefs
        End Function

        Public Sub RemoveClientCommRef(clientCommRefId As Integer, clientId As Integer) Implements IClientCommRefsRepository.RemoveClientCommRef
            Dim _item As Models.ClientCommRef = GetClientCommRef(clientCommRefId, clientId)
            If _item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveClientCommRef(_item)
            End If
        End Sub

        Public Sub RemoveClientCommRef(clientCommRef As Models.ClientCommRef) Implements IClientCommRefsRepository.RemoveClientCommRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientCommRef) = ctx.GetRepository(Of Models.ClientCommRef)()
                rep.Delete(clientCommRef)
            End Using
        End Sub

        Public Sub RemoveClientCommRefs(clientId As Integer) Implements IClientCommRefsRepository.RemoveClientCommRefs
            Dim clientCommRefs As IEnumerable(Of Models.ClientCommRef)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of Models.ClientCommRef)()
                clientCommRefs = rep.Find("Where ClientId = @0", clientId)

                For Each commRef In clientCommRefs
                    RemoveClientCommRef(commRef)
                Next
            End Using
        End Sub

        Public Sub UpdateClientCommRef(clientCommRef As Models.ClientCommRef) Implements IClientCommRefsRepository.UpdateClientCommRef
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientCommRef) = ctx.GetRepository(Of Models.ClientCommRef)()
                rep.Update(clientCommRef)
            End Using
        End Sub

    End Class
End Namespace