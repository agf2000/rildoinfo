Imports RIW.Modules.WebAPI.Components.Models

Namespace Components.Interfaces.Repositories

    Public Interface ISofterRepository

        ''' <summary>
        ''' Adds or updates sgi client
        ''' </summary>
        ''' <param name="pessoa">Pessoa Model</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AddSGIPessoa(pessoa As Softer) As Integer

        ''' <summary>
        ''' Adds or updates sgi client
        ''' </summary>
        ''' <param name="pessoa">Pessoa Model</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function UpdateSGIPessoa(pessoa As Softer) As Integer

        ''' <summary>
        ''' Adds or updates produto
        ''' </summary>
        ''' <param name="produto">Softer Model</param>
        ''' <remarks></remarks>
        Sub AddSGIProducts(produto As Softer)

        ''' <summary>
        ''' Adds or updates produto
        ''' </summary>
        ''' <param name="produto">Softer Model</param>
        ''' <remarks></remarks>
        Sub UpdateSGIProducts(produto As Softer)

        ''' <summary>
        ''' Syncs SGI products
        ''' </summary>
        ''' <param name="sDate">Syncronization date starting</param>
        ''' <param name="eDate">Syncronization date ending</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetSGIProducts(sDate As String, eDate As String) As IEnumerable(Of Softer)

        ''' <summary>
        ''' Syncs SGI products
        ''' </summary>
        ''' <param name="produto"></param>
        ''' <param name="fornecedor"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetSGIProductVendor(produto As Integer, fornecedor As Integer) As Softer

        ''' <summary>
        ''' Syncs SGI products
        ''' </summary>
        ''' <param name="grupo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetSGIProductCategory(grupo As Integer) As Softer

        ''' <summary>
        ''' Gets SGI's DAVs
        ''' </summary>
        ''' <param name="sDate">Syncronization date starting</param>
        ''' <param name="eDate">Syncronization date ending</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetSGIDAVs(sDate As String, eDate As String) As IEnumerable(Of Softer)

        ''' <summary>
        ''' Gets SGI's DAVs
        ''' </summary>
        ''' <param name="codigo">Numdoc</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetSGIDav(codigo As Integer) As Softer

        ''' <summary>
        ''' Saves SGI DAV
        ''' </summary>
        ''' <param name="dav"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function SaveSGIDAV(dav As Softer) As Integer

        ''' <summary>
        ''' Updates SGI DAV
        ''' </summary>
        ''' <param name="numDav"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Sub UpdateSGIDAV(numDav As Integer)

        ''' <summary>
        ''' Saves SGI DAV
        ''' </summary>
        ''' <param name="davItem"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function SaveSGIDAVItem(davItem As Softer) As Integer

        ''' <summary>
        ''' Gets SGI's DAV Items
        ''' </summary>
        ''' <param name="numDav"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetSGIDAVItems(numDav As Integer) As IEnumerable(Of Softer)

        ''' <summary>
        ''' Gets SGI people
        ''' </summary>
        ''' <param name="sDate">Syncronization date starting</param>
        ''' <param name="eDate">Syncronization date ending</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetSGIPeople(sDate As String, eDate As String) As IEnumerable(Of Softer)

        ''' <summary>
        ''' Gets SGI person
        ''' </summary>
        ''' <param name="codigo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetSGIPerson(codigo As Integer) As Softer

        ''' <summary>
        ''' Gets SGI person address
        ''' </summary>
        ''' <param name="codigo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetSGIPersonAddress(codigo As Integer) As Softer

    End Interface

End Namespace
