Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models

Namespace Components.Repositories

    Public Class PersonDocsRepository
    Implements IPersonDocsRepository

        Public Function AddPersonDoc(personDoc As PersonDoc) As PersonDoc Implements IPersonDocsRepository.AddPersonDoc
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonDoc) = ctx.GetRepository(Of PersonDoc)()
                rep.Insert(personDoc)
            End Using
            Return personDoc
        End Function

        Public Function GetPersonDoc(personDocId As Integer, personId As Integer) As PersonDoc Implements IPersonDocsRepository.GetPersonDoc
            Dim personDoc As PersonDoc

            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonDoc) = ctx.GetRepository(Of PersonDoc)()
                personDoc = rep.GetById(Of Int32, Int32)(personDocId, personId)
            End Using
            Return personDoc
        End Function

        Public Function GetPersonDocs(personId As Integer) As IEnumerable(Of PersonDoc) Implements IPersonDocsRepository.GetPersonDocs
            Return CBO.FillCollection(Of PersonDoc)(DataProvider.Instance().GetPersonDocs(personId))
            'Dim personDocs As IEnumerable(Of Models.PersonDoc)

            'Using ctx As IDataContext = DataContext.Instance()
            '    Dim rep As IRepository(Of Models.PersonDoc) = ctx.GetRepository(Of Models.PersonDoc)()
            '    personDocs = rep.Get(personId)
            'End Using
            'Return personDocs
        End Function

        Public Sub RemovePersonDoc(personDocId As Integer, personId As Integer) Implements IPersonDocsRepository.RemovePersonDoc
            Dim item As PersonDoc = GetPersonDoc(personDocId, personId)
            If item.Locked Then
                Throw New ArgumentException("Exception Occured")
            Else
                RemovePersonDoc(item)
            End If
        End Sub

        Public Sub RemovePersonDoc(personDoc As PersonDoc) Implements IPersonDocsRepository.RemovePersonDoc
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonDoc) = ctx.GetRepository(Of PersonDoc)()
                rep.Delete(personDoc)
            End Using
        End Sub

        Public Sub RemovePersonDocs(personId As Integer) Implements IPersonDocsRepository.RemovePersonDocs
            Dim personDocs As IEnumerable(Of PersonDoc)
            Using context As IDataContext = DataContext.Instance()
                Dim rep = context.GetRepository(Of PersonDoc)()
                personDocs = rep.Find("Where PersonId = @0", personId)

                For Each bankRef In personDocs
                    RemovePersonDoc(bankRef)
                Next
            End Using
        End Sub

        Public Sub UpdatePersonDoc(personDoc As PersonDoc) Implements IPersonDocsRepository.UpdatePersonDoc
            Using ctx As IDataContext = DataContext.Instance()
                Dim rep As IRepository(Of PersonDoc) = ctx.GetRepository(Of PersonDoc)()
                rep.Update(personDoc)
            End Using
        End Sub

    End Class

End Namespace
