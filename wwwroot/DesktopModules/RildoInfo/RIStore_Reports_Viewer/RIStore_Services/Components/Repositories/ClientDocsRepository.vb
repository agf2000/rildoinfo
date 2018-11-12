
Imports System.Collections.Generic
Imports DotNetNuke.Data

Namespace RI.Modules.RIStore_Services
    Public Class ClientDocsRepository
        Implements IClientDocsRepository

        Public Function AddClientDoc(clientDoc As Models.ClientDoc) As Models.ClientDoc Implements IClientDocsRepository.AddClientDoc
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientDoc) = ctx.GetRepository(Of Models.ClientDoc)()
                rep.Insert(clientDoc)
            End Using
            Return clientDoc
        End Function

        Public Function GetClientDoc(clientDocId As Integer, clientId As Integer) As Models.ClientDoc Implements IClientDocsRepository.GetClientDoc
            Dim clientDoc As Models.ClientDoc

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientDoc) = ctx.GetRepository(Of Models.ClientDoc)()
                clientDoc = rep.GetById(Of Int32, Int32)(clientDocId, clientId)
            End Using
            Return clientDoc
        End Function

        Public Function GetClientDocs(clientId As Integer) As IEnumerable(Of Models.ClientDoc) Implements IClientDocsRepository.GetClientDocs
            Dim clientDocs As IEnumerable(Of Models.ClientDoc)

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientDoc) = ctx.GetRepository(Of Models.ClientDoc)()
                clientDocs = rep.Get(clientId)
            End Using
            Return clientDocs
        End Function

        Public Sub RemoveClientDoc(clientDocId As Integer, clientId As Integer) Implements IClientDocsRepository.RemoveClientDoc
            Dim _item As Models.ClientDoc = GetClientDoc(clientDocId, clientId)
            If _item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemoveClientDoc(_item)
            End If
        End Sub

        Public Sub RemoveClientDoc(clientDoc As Models.ClientDoc) Implements IClientDocsRepository.RemoveClientDoc
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientDoc) = ctx.GetRepository(Of Models.ClientDoc)()
                rep.Delete(clientDoc)
            End Using
        End Sub

        Public Sub RemoveClientDocs(clientId As Integer) Implements IClientDocsRepository.RemoveClientDocs
            Dim clientDocs As IEnumerable(Of Models.ClientDoc)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of Models.ClientDoc)()
                clientDocs = rep.Find("Where ClientId = @0", clientId)

                For Each bankRef In clientDocs
                    RemoveClientDoc(bankRef)
                Next
            End Using
        End Sub

        Public Sub UpdateClientDoc(clientDoc As Models.ClientDoc) Implements IClientDocsRepository.UpdateClientDoc
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of Models.ClientDoc) = ctx.GetRepository(Of Models.ClientDoc)()
                rep.Update(clientDoc)
            End Using
        End Sub

    End Class
End Namespace