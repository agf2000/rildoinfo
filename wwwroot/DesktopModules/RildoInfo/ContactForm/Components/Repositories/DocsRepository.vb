
Imports DotNetNuke.Common.Utilities
Imports RIW.Modules.ContactForm

Public Class DocsRepository
    Implements IDocsRepository

    Public Function addDoc(doc As Models.Doc) As Models.Doc Implements IDocsRepository.addDoc
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Doc) = ctx.GetRepository(Of Models.Doc)()
            rep.Insert(doc)
        End Using
        Return doc
    End Function

    Public Function getDoc(docId As Integer, portalId As Integer) As Models.Doc Implements IDocsRepository.getDoc
        Return CBO.FillObject(Of Models.Doc)(DataProvider.Instance().GetDoc(docId, portalId))
    End Function

    Public Function getDocs(portalId As Integer) As IEnumerable(Of Models.Doc) Implements IDocsRepository.getDocs
        Return CBO.FillCollection(Of Models.Doc)(DataProvider.Instance().GetDocs(portalId))
    End Function

    Public Sub removeDoc(docId As Integer, portalId As Integer) Implements IDocsRepository.removeDoc
        Dim doc As Models.Doc = getDoc(docId, portalId)
        removeDoc(doc)
    End Sub

    Public Sub removeDoc(doc As Models.Doc) Implements IDocsRepository.removeDoc
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Doc) = ctx.GetRepository(Of Models.Doc)()
            rep.Delete(doc)
        End Using
    End Sub

    Public Sub updateDoc(doc As Models.Doc) Implements IDocsRepository.updateDoc
        Using ctx As IDataContext = DataContext.Instance()
            Dim rep As IRepository(Of Models.Doc) = ctx.GetRepository(Of Models.Doc)()
            rep.Update(doc)
        End Using
    End Sub
End Class
