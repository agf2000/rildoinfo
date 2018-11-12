<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Cashiers_Manager.ascx.vb" Inherits="RIW.Modules.Store_Manager.Views.CashiersManager" %>
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
                                <span class="NormalBold">Filtrar por Data:</span>
                                <i class="icon icon-exclamation-sign" title="Datas de Emissão" data-content="Filtrar por Datas. Insire as datas e clique em [Aplicar Filtro]."></i>
                                <div class="padded"></div>
                                <input id="kdpStartDate" placeholder="Data Inicial" title="Data inicial" />&nbsp;            
                                <input id="kdpEndDate" placeholder="Data Final" title="Data final" />
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
        <div style="margin: 5px 0 0 0; padding: 5px 3px; background-color: #E2F0F6; border-top: 1px solid #94C0D2; border-left: 1px solid #94C0D2; border-right: 1px solid #94C0D2;">
            <ul class="inline">
                <li>
                    <input id="kdpCloseDate" placeholder="Data do Caixa" title="Data do Caixa" /> <button id="btnClose" class="btn btn-small btn-inverse"><span class="icon-plus icon-white"></span>&nbsp; Fechar Caixa</button>
                </li>
            </ul>   
        </div>
        <div id="cashiersGrid"></div>
    </div>
</div>