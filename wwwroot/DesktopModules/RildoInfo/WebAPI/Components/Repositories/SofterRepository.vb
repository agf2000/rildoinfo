Imports RIW.Modules.WebAPI.Components.Interfaces.Repositories
Imports RIW.Modules.WebAPI.Components.Models

Namespace Components.Repositories

    Public Class SofterRepository
        Implements ISofterRepository

        Public Function AddSGIPessoa(pessoa As Softer) As Integer Implements ISofterRepository.AddSGIPessoa
            DataProvider.Instance().AddSGIPessoa(pessoa)
            Return pessoa.codigo
        End Function

        Public Function UpdateSGIPessoa(pessoa As Softer) As Integer Implements ISofterRepository.UpdateSGIPessoa
            DataProvider.Instance().UpdateSGIPessoa(pessoa.codigo, pessoa.tipopessoa, pessoa.fantasia, pessoa.razao, pessoa.numero, pessoa.complemento,
                                                 pessoa.tipopessoafj, pessoa.cgccpf, pessoa.inscricaorg, pessoa.email, pessoa.homepage, pessoa.ativo,
                                                 pessoa.revendaconsumidor, pessoa.codvendedor, pessoa.codconvenio, pessoa.codregiao, pessoa.codclasse,
                                                 pessoa.codprofissao, pessoa.tipologradouropessoa, pessoa.logradouropessoa, pessoa.bairropessoa,
                                                 pessoa.cidadepessoa, pessoa.estadopessoa, pessoa.ceppessoa, pessoa.tipologradourotrabalho,
                                                 pessoa.logradourotrabalho, pessoa.bairrotrabalho, pessoa.cidadetrabalho, pessoa.estadotrabalho,
                                                 pessoa.ceptrabalho, pessoa.estadocivil, pessoa.filiacao, pessoa.sexo, pessoa.nacionalidade,
                                                 pessoa.naturalidade, pessoa.numerotrabalho, pessoa.complementotrabalho, pessoa.codconjuge,
                                                 pessoa.telefoneprincipal, pessoa.fax, pessoa.limitecredito, pessoa.cobrarjuros, pessoa.pagarcomissao,
                                                 pessoa.bloquiodecredito, pessoa.bloqueiodefinitivo, pessoa.carencia, pessoa.datanasfundacao,
                                                 pessoa.datacadastro, pessoa.local_trabalho, pessoa.contato, pessoa.nasccontato, pessoa.contato2,
                                                 pessoa.nasccontato2, pessoa.telefone2, pessoa.referencia1, pessoa.foneref1, pessoa.referencia2,
                                                 pessoa.foneref2, pessoa.atividadevendedor, pessoa.salario, pessoa.renda, pessoa.admissao, pessoa.demissao,
                                                 pessoa.ctps, pessoa.usarendprincipal, pessoa.obs)
            Return pessoa.codigo
        End Function

        Public Sub AddSGIProducts(produto As Softer) Implements ISofterRepository.AddSGIProducts
            DataProvider.Instance().AddSGIProducts(produto)
        End Sub

        Public Sub UpdateSGIProducts(produto As Softer) Implements ISofterRepository.UpdateSGIProducts
            DataProvider.Instance().UpdateSGIProducts(produto)
        End Sub

        Public Function GetSGIProducts(sDate As String, eDate As String) As IEnumerable(Of Softer) Implements ISofterRepository.GetSGIProducts
            Return CBO.FillCollection(Of Softer)(DataProvider.Instance().GetSGIProducts(sDate, eDate))
        End Function

        Public Function GetSGIProductVendor(produto As Integer, fornecedor As Integer) As Softer Implements ISofterRepository.GetSGIProductVendor
            Return CBO.FillObject(Of Softer)(DataProvider.Instance().GetSGIProductVendor(produto, fornecedor))
        End Function

        Public Function GetSGIProductCategory(grupo As Integer) As Softer Implements ISofterRepository.GetSGIProductCategory
            Return CBO.FillObject(Of Softer)(DataProvider.Instance().GetSGIProductCategory(grupo))
        End Function

        Public Function GetSGIDAVs(sDate As String, eDate As String) As IEnumerable(Of Softer) Implements ISofterRepository.GetSGIDAVs
            Return CBO.FillCollection(Of Softer)(DataProvider.Instance().GetSGIDAVs(sDate, eDate))
        End Function

        Public Function GetSGIDAVItems(numDav As Integer) As IEnumerable(Of Softer) Implements ISofterRepository.GetSGIDAVItems
            Return CBO.FillCollection(Of Softer)(DataProvider.Instance().GetSGIDAVItems(numDav))
        End Function

        Public Function GetSGIPeople(sDate As String, eDate As String) As IEnumerable(Of Softer) Implements ISofterRepository.GetSGIPeople
            Return CBO.FillCollection(Of Softer)(DataProvider.Instance().GetSGIPeople(sDate, eDate))
        End Function

        Public Function GetSGIPerson(codigo As Integer) As Softer Implements ISofterRepository.GetSGIPerson
            Return CBO.FillObject(Of Softer)(DataProvider.Instance().GetSGIPerson(codigo))
        End Function

        Public Function GetSGIPersonAddress(codigo As Integer) As Softer Implements ISofterRepository.GetSGIPersonAddress
            Return CBO.FillObject(Of Softer)(DataProvider.Instance().GetSGIPersonAddress(codigo))
        End Function

        Public Function GetSGIDav(codigo As Integer) As Softer Implements ISofterRepository.GetSGIDav
            Return CBO.FillObject(Of Softer)(DataProvider.Instance().GetSGIDav(codigo))
        End Function

        Public Function SaveSGIDAV(dav As Softer) As Integer Implements ISofterRepository.SaveSGIDAV
            Return CType(DataProvider.Instance().SaveSGIDav(dav), Integer)
        End Function

        Public Function SaveSGIDAVItem(davItem As Softer) As Integer Implements ISofterRepository.SaveSGIDAVItem
            Return CType(DataProvider.Instance().SaveSGIDAVItem(davItem), Integer)
        End Function

        Public Sub UpdateSGIDAV(numDav As Integer) Implements ISofterRepository.UpdateSGIDAV
            DataProvider.Instance().UpdateSGIDAV(numDav)
        End Sub

    End Class

End Namespace
