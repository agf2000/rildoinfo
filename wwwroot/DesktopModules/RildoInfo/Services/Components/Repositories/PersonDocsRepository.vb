
Public Class PersonDocsRepository
    Implements IPersonDocsRepository

    Public Function addPersonDoc(personDoc As Models.PersonDoc) As Models.PersonDoc Implements IPersonDocsRepository.addPersonDoc
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonDoc) = ctx.GetRepository(Of Models.PersonDoc)()
            rep.Insert(personDoc)
        End Using
        Return personDoc
    End Function

    Public Function getPersonDoc(personDocId As Integer, personId As Integer) As Models.PersonDoc Implements IPersonDocsRepository.getPersonDoc
        Dim personDoc As Models.PersonDoc

        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonDoc) = ctx.GetRepository(Of Models.PersonDoc)()
            personDoc = rep.GetById(Of Int32, Int32)(personDocId, personId)
        End Using
        Return personDoc
    End Function

    Public Function getPersonDocs(personId As Integer) As IEnumerable(Of Models.PersonDoc) Implements IPersonDocsRepository.getPersonDocs
        Return CBO.FillCollection(Of Models.PersonDoc)(DataProvider.Instance().GetPersonDocs(personId))
        'Dim personDocs As IEnumerable(Of Models.PersonDoc)

        'Using ctx As IDataContext = DataContext.Instance()
        '    Dim rep As IRepository(Of Models.PersonDoc) = ctx.GetRepository(Of Models.PersonDoc)()
        '    personDocs = rep.Get(personId)
        'End Using
        'Return personDocs
    End Function

    Public Sub removePersonDoc(personDocId As Integer, personId As Integer) Implements IPersonDocsRepository.removePersonDoc
        Dim _item As Models.PersonDoc = getPersonDoc(personDocId, personId)
        If _item.Locked Then
            Throw New ArgumentException("Exception Occured")
        Else
            removePersonDoc(_item)
        End If
    End Sub

    Public Sub removePersonDoc(personDoc As Models.PersonDoc) Implements IPersonDocsRepository.removePersonDoc
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonDoc) = ctx.GetRepository(Of Models.PersonDoc)()
            rep.Delete(personDoc)
        End Using
    End Sub

    Public Sub removePersonDocs(personId As Integer) Implements IPersonDocsRepository.removePersonDocs
        Dim personDocs As IEnumerable(Of Models.PersonDoc)
        Using context As IDataContext = DataContext.Instance()
            Dim rep = context.GetRepository(Of Models.PersonDoc)()
            personDocs = rep.Find("Where PersonId = @0", personId)

            For Each bankRef In personDocs
                removePersonDoc(bankRef)
            Next
        End Using
    End Sub

    Public Sub updatePersonDoc(personDoc As Models.PersonDoc) Implements IPersonDocsRepository.updatePersonDoc
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.PersonDoc) = ctx.GetRepository(Of Models.PersonDoc)()
            rep.Update(personDoc)
        End Using
    End Sub

End Class
