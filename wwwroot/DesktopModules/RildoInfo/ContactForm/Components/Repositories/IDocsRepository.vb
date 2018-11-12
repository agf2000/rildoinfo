
Imports RIW.Modules.ContactForm

Public Interface IDocsRepository

    Function getDoc(docId As Integer, portalId As Integer) As Models.Doc

    Function getDocs(portalId As Integer) As IEnumerable(Of Models.Doc)

    Function addDoc(doc As Models.Doc) As Models.Doc

    Sub updateDoc(doc As Models.Doc)

    Sub removeDoc(docId As Integer, portalId As Integer)

    Sub removeDoc(doc As Models.Doc)
End Interface
