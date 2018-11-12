<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Invoices_Manager.ascx.vb" Inherits="RIW.Modules.Store_Manager.Views.InvoicesManager" %>
<div id="divInvoices" class="row-fluid">
    <div class="span12">
        <div class="accordion">
            <div class="accordion-group">
                <div class="accordion-heading">
                    <a class="accordion-toggle" data-toggle="collapse" href="#collapseFilter">
                        <h4>Filtro</h4>
                    </a>
                </div>
                <div id="collapseFilter" class="accordion-body collapse">
                    <div class="accordion-inner">
                        <ul id="divInvoicesButtons" class="inline">
                            <li>
                                <span class="NormalBold">Datas de Emissão:</span>
                                <i class="icon icon-exclamation-sign" title="Datas de Emissão" data-content="Filtrar por Datas de Emissão de Nota Fiscal. Insire as datas e clique em [Aplicar Filtro]."></i>
                                <div class="padded"></div>
                                <input id="kdpStartDate" placeholder="Data Inicial" title="Data da emissão" />&nbsp;            
                                <input id="kdpEndDate" placeholder="Data Final" title="Data da emissão" />
                            </li>
                            <li>
                                <strong>Fornecedores:</strong>
                                <i class="icon icon-exclamation-sign" title="Filtre por Fornecedores" data-content="Digite 5 carecteres ou mais para iniciar sua busca. Encontre o fornecedor e clique em [Aplicar Filtro]."></i>
                                <input id="vendorSearchBox" type="text" class="input-xlarge" />
                                <div class="padded"></div>
                                <strong>Cliente:</strong>
                                <i class="icon icon-exclamation-sign" title="Filtre por Clientes" data-content="Digite 5 carecteres ou mais para iniciar sua busca. Encontre o cliente e clique em [Aplicar Filtro]."></i>
                                <input id="clientSearchBox" type="text" class="input-xlarge" />
                            </li>
                            <li>
                                <div class="padded"></div>
                                <button id="btnSearch" class="btn btn-small" title="Clique aqui para fazer sua busca"><span class="icon icon-search"></span>&nbsp; Aplicar Filtro</button>
                                <button id="btnRemoveFilter" class="btn btn-small" title="Clique aqui para remover filtros"><span class="k-icon k-i-cancel"></span></button>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div id="invoicesGrid"></div>
        <div id="invoiceWindow"></div>
        <script id="tmplToolbar" type="text/x-kendo-template">        
            <ul id="ulToolbar" class="inline">
                <li>
                    <button id="btnAddNewInvoice" class="btn btn-small btn-inverse"><span class="icon-plus icon-white"></span>&nbsp; Adicionar</button>
                </li>
                <li>
                    <button id="btnEditSelected" class="btn btn-small" disabled="disabled"><span class="icon icon-edit"></span> Editar</button>
                </li>
                <li>
                    <button id="btnRemoveSelected" class="btn btn-small"><span class="fa fa-remove"></span> Excluir</button>
                </li>
            </ul>        
        </script>
    </div>
</div>