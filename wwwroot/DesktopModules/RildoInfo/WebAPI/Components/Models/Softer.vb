
Imports RIW.Modules.WebAPI.Components.Interfaces.Models

Namespace Components.Models

    Public Class Softer
        Implements ISofter

        Public Property acrescimo As Single Implements ISofter.acrescimo

        Public Property admissao As Date Implements ISofter.admissao

        Public Property aliquotaicms As Double Implements ISofter.aliquotaicms

        Public Property aliquotaipi As Double Implements ISofter.aliquotaipi

        Public Property aliquotaiss As Decimal Implements ISofter.aliquotaiss

        Public Property aplicacao As String Implements ISofter.aplicacao

        Public Property atividadevendedor As Integer Implements ISofter.atividadevendedor

        Public Property ativo As Boolean Implements ISofter.ativo

        Public Property ax As String Implements ISofter.ax

        Public Property bairro As String Implements ISofter.bairro

        Public Property bairropessoa As String Implements ISofter.bairropessoa

        Public Property bairrotrabalho As String Implements ISofter.bairrotrabalho

        Public Property barras As String Implements ISofter.barras

        Public Property bloqueado As Boolean Implements ISofter.bloqueado

        Public Property bloqueiodefinitivo As Boolean Implements ISofter.bloqueiodefinitivo

        Public Property bloquiodecredito As Boolean Implements ISofter.bloquiodecredito

        Public Property c_c_custofixo As Double Implements ISofter.c_c_custofixo

        Public Property c_c_freteper As Double Implements ISofter.c_c_freteper

        Public Property c_c_impostfed As Double Implements ISofter.c_c_impostfed

        Public Property c_c_ipi As Double Implements ISofter.c_c_ipi

        Public Property c_c_subtrib As Double Implements ISofter.c_c_subtrib

        Public Property cadastro As Date Implements ISofter.cadastro

        Public Property cadastroinicial As Date Implements ISofter.cadastroinicial

        Public Property carencia As Integer Implements ISofter.carencia

        Public Property cartao As Boolean Implements ISofter.cartao

        Public Property categoria As Integer Implements ISofter.categoria

        Public Property ccf As Integer Implements ISofter.ccf

        Public Property cep As String Implements ISofter.cep

        Public Property ceppessoa As String Implements ISofter.ceppessoa

        Public Property ceptrabalho As String Implements ISofter.ceptrabalho

        Public Property cfop_saida_estado As String Implements ISofter.cfop_saida_estado

        Public Property cgccpf As String Implements ISofter.cgccpf

        Public Property cidade As String Implements ISofter.cidade

        Public Property cidadepessoa As String Implements ISofter.cidadepessoa

        Public Property cidadetrabalho As String Implements ISofter.cidadetrabalho

        Public Property classe As Integer Implements ISofter.classe

        Public Property classificacao As Integer Implements ISofter.classificacao

        Public Property cobrarjuros As Boolean Implements ISofter.cobrarjuros

        Public Property cod_barras As String Implements ISofter.cod_barras

        Public Property cod_grupo_fiscal As String Implements ISofter.cod_grupo_fiscal

        Public Property cod_trib_ecf As Integer Implements ISofter.cod_trib_ecf

        Public Property codclasse As Integer Implements ISofter.codclasse

        Public Property codicli As Integer Implements ISofter.codicli

        Public Property codconjuge As Integer Implements ISofter.codconjuge

        Public Property codconvenio As String Implements ISofter.codconvenio

        Public Property codigo As Integer Implements ISofter.codigo

        Public Property codigoaliquotatrib As Integer Implements ISofter.codigoaliquotatrib

        Public Property codigofabricante As Integer Implements ISofter.codigofabricante

        Public Property codigofornecedor As Integer Implements ISofter.codigofornecedor

        Public Property codigogrupo As Integer Implements ISofter.codigogrupo

        Public Property codigopessoa As Integer Implements ISofter.codigopessoa

        Public Property codigosubgrupo As Integer Implements ISofter.codigosubgrupo

        Public Property codpay As Integer Implements ISofter.codpay

        Public Property codproduto As Integer Implements ISofter.codproduto

        Public Property codprofissao As Integer Implements ISofter.codprofissao

        Public Property codregiao As Integer Implements ISofter.codregiao

        Public Property codiven As Integer Implements ISofter.codiven

        Public Property codvendedor As Integer Implements ISofter.codvendedor

        Public Property cofins As Decimal Implements ISofter.cofins

        Public Property comentrada As Boolean Implements ISofter.comentrada

        Public Property comissao As Decimal Implements ISofter.comissao

        Public Property complemento As String Implements ISofter.complemento

        Public Property complementotrabalho As String Implements ISofter.complementotrabalho

        Public Property composicao As Boolean Implements ISofter.composicao

        Public Property condpag As Integer Implements ISofter.condpag

        Public Property condpagnome As String Implements ISofter.condpagnome

        Public Property contato As String Implements ISofter.contato

        Public Property contato2 As String Implements ISofter.contato2

        Public Property convenio As Integer Implements ISofter.convenio

        Public Property coo As Integer Implements ISofter.coo

        Public Property coo_vinculado As Integer Implements ISofter.coo_vinculado

        Public Property cpf_cnpj As String Implements ISofter.cpf_cnpj

        'Public Property cro As Integer Implements ISofter.cro

        'Public Property crz As Integer Implements ISofter.crz

        Public Property csosn As String Implements ISofter.csosn

        Public Property cst As String Implements ISofter.cst

        Public Property ctps As String Implements ISofter.ctps

        Public Property cupomcancelado As Boolean Implements ISofter.cupomcancelado

        Public Property custo_final As Single Implements ISofter.custo_final

        Public Property custo_liq As Single Implements ISofter.custo_liq

        Public Property custobruto As Single Implements ISofter.custobruto

        Public Property custoliquido As Single Implements ISofter.custoliquido

        Public Property customedio As Single Implements ISofter.customedio

        Public Property data_alteracao As Date Implements ISofter.data_alteracao

        Public Property data_cadastro As Date Implements ISofter.data_cadastro

        Public Property datavenda As Date Implements ISofter.datavenda

        Public Property datacadastro As Date Implements ISofter.datacadastro

        Public Property datanasfundacao As Date Implements ISofter.datanasfundacao

        Public Property demissao As Date Implements ISofter.demissao

        Public Property desativado As Boolean Implements ISofter.desativado

        Public Property desc_compl As String Implements ISofter.desc_compl

        Public Property desconto As Decimal Implements ISofter.desconto

        Public Property descontoperc As Decimal Implements ISofter.descontoperc

        Public Property descperc As Decimal Implements ISofter.descperc

        Public Property descrevenda As Single Implements ISofter.descrevenda

        Public Property descricao As String Implements ISofter.descricao

        Public Property desp_acessorias As Double Implements ISofter.desp_acessorias

        Public Property email As String Implements ISofter.email

        Public Property embalagem As Double Implements ISofter.embalagem

        Public Property encargos As Double Implements ISofter.encargos

        Public Property est_min As Decimal Implements ISofter.est_min

        Public Property estado As String Implements ISofter.estado

        Public Property estadocivil As String Implements ISofter.estadocivil

        Public Property estadopessoa As String Implements ISofter.estadopessoa

        Public Property estadotrabalho As String Implements ISofter.estadotrabalho

        Public Property estoque As Decimal Implements ISofter.estoque

        Public Property estoqueatual As Double Implements ISofter.estoqueatual

        Public Property estoquemaximo As Double Implements ISofter.estoquemaximo

        Public Property estoqueminimo As Double Implements ISofter.estoqueminimo

        Public Property fantasia As String Implements ISofter.fantasia

        Public Property fax As String Implements ISofter.fax

        Public Property filiacao As String Implements ISofter.filiacao

        Public Property flexivel As Boolean Implements ISofter.flexivel

        Public Property foneref1 As String Implements ISofter.foneref1

        Public Property foneref2 As String Implements ISofter.foneref2

        Public Property fornecedor As Integer Implements ISofter.fornecedor

        Public Property fornecedores As String Implements ISofter.fornecedores

        Public Property frete As Decimal Implements ISofter.frete

        Public Property garantia As Integer Implements ISofter.garantia

        'Public Property gnf As Integer Implements ISofter.gnf

        Public Property grupo As String Implements ISofter.grupo

        Public Property grupofiscal As Integer Implements ISofter.grupofiscal

        Public Property homepage As String Implements ISofter.homepage

        Public Property iat As String Implements ISofter.iat

        Public Property icms As Decimal Implements ISofter.icms

        Public Property incres As Integer Implements ISofter.incres

        Public Property ingredientes As String Implements ISofter.ingredientes

        Public Property insc_municipal As String Implements ISofter.insc_municipal

        Public Property inscricaorg As String Implements ISofter.inscricaorg

        Public Property intervalo As Integer Implements ISofter.intervalo

        Public Property intervalo_dias As String Implements ISofter.intervalo_dias

        Public Property ipi As Decimal Implements ISofter.ipi

        Public Property ippt As String Implements ISofter.ippt

        Public Property limitecredito As Single Implements ISofter.limitecredito

        Public Property local_trabalho As String Implements ISofter.local_trabalho

        Public Property localizacao As String Implements ISofter.localizacao

        Public Property logradouro As String Implements ISofter.logradouro

        Public Property logradouropessoa As String Implements ISofter.logradouropessoa

        Public Property logradourotrabalho As String Implements ISofter.logradourotrabalho

        Public Property lucro As Double Implements ISofter.lucro

        Public Property nacionalidade As String Implements ISofter.nacionalidade

        Public Property naoimportarcustos As Boolean Implements ISofter.naoimportarcustos

        Public Property naoimportarestoque As Boolean Implements ISofter.naoimportarestoque

        Public Property nasccontato As Date Implements ISofter.nasccontato

        Public Property nasccontato2 As Date Implements ISofter.nasccontato2

        Public Property nascimento As Date Implements ISofter.nascimento

        Public Property naturalidade As String Implements ISofter.naturalidade

        Public Property natureza As String Implements ISofter.natureza

        Public Property nbmsh As String Implements ISofter.nbmsh

        Public Property ncm As String Implements ISofter.ncm

        Public Property nfe_pmvast As Double Implements ISofter.nfe_pmvast

        Public Property nfe_predbc As Double Implements ISofter.nfe_predbc

        Public Property nfe_predbcst As Double Implements ISofter.nfe_predbcst

        Public Property nome As String Implements ISofter.nome

        Public Property nompais As String Implements ISofter.nompais

        Public Property nro_parcelas As Integer Implements ISofter.nro_parcelas

        Public Property numdav As Integer Implements ISofter.numdav

        Public Property numdoc As Integer Implements ISofter.numdoc

        Public Property numero As String Implements ISofter.numero

        Public Property numerotrabalho As String Implements ISofter.numerotrabalho

        'Public Property numfab As String Implements ISofter.numfab

        'Public Property nummaquina As String Implements ISofter.nummaquina

        Public Property obs As String Implements ISofter.obs

        Public Property observacao As String Implements ISofter.observacao

        Public Property opercard As Integer Implements ISofter.opercard

        Public Property outrosacrescimos As Single Implements ISofter.outrosacrescimos

        Public Property outrosdescontos As Single Implements ISofter.outrosdescontos

        Public Property padrao As Boolean Implements ISofter.padrao

        Public Property pagarcomissao As Boolean Implements ISofter.pagarcomissao

        Public Property peso As Decimal Implements ISofter.peso

        Public Property pesopadrao As Double Implements ISofter.pesopadrao

        Public Property pis As Decimal Implements ISofter.pis

        Public Property politica_comissao As Integer Implements ISofter.politica_comissao

        Public Property preco As Single Implements ISofter.preco

        Public Property primeira_compra As Date Implements ISofter.primeira_compra

        Public Property principal As Boolean Implements ISofter.principal

        Public Property produto As Integer Implements ISofter.produto

        Public Property produtomontado As Boolean Implements ISofter.produtomontado

        Public Property quantidade As Double Implements ISofter.quantidade

        Public Property razao As String Implements ISofter.razao

        Public Property ref As String Implements ISofter.ref

        Public Property referencia As String Implements ISofter.referencia

        Public Property referencia1 As String Implements ISofter.referencia1

        Public Property referencia2 As String Implements ISofter.referencia2

        Public Property refxml As String Implements ISofter.refxml

        Public Property renda As Single Implements ISofter.renda

        Public Property responsavel As String Implements ISofter.responsavel

        Public Property revendaconsumidor As String Implements ISofter.revendaconsumidor

        Public Property rg_insc_est As String Implements ISofter.rg_insc_est

        Public Property salario As Single Implements ISofter.salario

        Public Property sequenciadav As Integer Implements ISofter.sequenciadav

        Public Property seriais As Boolean Implements ISofter.seriais

        Public Property sexo As String Implements ISofter.sexo

        Public Property spc As Boolean Implements ISofter.spc

        Public Property status As Integer Implements ISofter.status

        Public Property sub_trib As Decimal Implements ISofter.sub_trib

        Public Property telefone As String Implements ISofter.telefone

        Public Property telefone2 As String Implements ISofter.telefone2

        Public Property telefoneprincipal As String Implements ISofter.telefoneprincipal

        Public Property tipo As String Implements ISofter.tipo

        Public Property tipo_logradouro As String Implements ISofter.tipo_logradouro

        Public Property tipologradouropessoa As String Implements ISofter.tipologradouropessoa

        Public Property tipologradourotrabalho As String Implements ISofter.tipologradourotrabalho

        Public Property tipopessoa As Integer Implements ISofter.tipopessoa

        Public Property tipopessoafj As String Implements ISofter.tipopessoafj

        Public Property tipoproduto As String Implements ISofter.tipoproduto

        Public Property total As Single Implements ISofter.total

        Public Property ultima_compra As Date Implements ISofter.ultima_compra

        Public Property unidade As String Implements ISofter.unidade

        Public Property unidadedivisora As Integer Implements ISofter.unidadedivisora

        Public Property unidadeid As Integer Implements ISofter.unidadeid

        Public Property usarendprincipal As Boolean Implements ISofter.usarendprincipal

        Public Property validade As Integer Implements ISofter.validade

        Public Property valordin As Single Implements ISofter.valordin

        Public Property valorunitario As Single Implements ISofter.valorunitario

        Public Property vencimentos As Boolean Implements ISofter.vencimentos

        Public Property vendedor As Integer Implements ISofter.vendedor

        Public Property vrcartao As Single Implements ISofter.vrcartao

        Public Property vrchepre As Single Implements ISofter.vrchepre

        Public Property vrchevis As Single Implements ISofter.vrchevis

        Public Property vrconv As Single Implements ISofter.vrconv

        Public Property vrcred As Single Implements ISofter.vrcred

        Public Property vrvale As Single Implements ISofter.vrvale

        Public Property website As String Implements ISofter.website

        Public Property ativobalanca As Boolean Implements ISofter.ativobalanca
        '    Get
        '        Throw New NotImplementedException()
        '    End Get
        '    Set(value As Boolean)
        '        Throw New NotImplementedException()
        '    End Set
        'End Property
    End Class
End Namespace