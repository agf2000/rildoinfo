<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Manage_Estimates.ascx.vb" Inherits="RIW.Modules.Estimates.Views.ManageEstimates" %>
<div id="divManageEstimates" class="row-fluid">
    <div class="accordion">
        <div class="accordion-group">
            <div class="accordion-heading">
                <a class="accordion-toggle" data-toggle="collapse" href="#collapseFilter">
                    <h4>Filtro</h4>
                </a>
            </div>
            <div id="collapseFilter" class="accordion-body collapse">
                <div class="accordion-inner">
                    <input id="ask_permission" class="btn" type="button" value="Permitir Notificações" title="Permissões" data-content="Clique aqui para permitir o recebimento de notificações.">
                    <ul class="inline">
                        <li>
                            <strong>Data Inicial:</strong>
                            <i class="icon icon-exclamation-sign" title="Data de Inserção" data-content="Filtre por datas de inserção. Insire a data inicial e final clique em [Aplicar Filtro]."></i>
                            <input id="kdpStartDate" placeholder="Data Inicial" title="Data da última alteração" />
                            <div class="padded"></div>
                            <strong>Data Final:</strong>
                            <i class="icon icon-exclamation-sign" title="Data de Inserção" data-content="Filtre por datas de inserção. Insire a data inicial final e clique em [Aplicar Filtro]."></i>
                            <input id="kdpEndDate" placeholder="Data Final" title="Data da última alteração" />
                        </li>
                        <li>
                            <strong>Status:</strong>
                            <i class="icon icon-exclamation-sign" title="Filtrar por status" data-content="Escolha o status e clique em [Aplicar Filtro]."></i>
                            <input id="ddlStatuses" title="Filtrar lista por status" />
                            <div class="padded"></div>
                            <div data-bind="visible: authorized > 2">
                                <strong>Vendedores:</strong>
                                <i class="icon icon-exclamation-sign" title="Filtrar por vendedor" data-content="Escolha o vendedor e clique em [Aplicar Filtro]."></i>
                                <input id="kddlSalesGroup" title="Filtrar lista por vendedor" />
                            </div>
                        </li>
                        <li>
                            <strong>Campos:</strong>
                            <i class="icon icon-exclamation-sign" title="Campos Disponíveis" data-content="Escolha o campo disponível para sua busca."></i>
                            <select id="optionsFields" class="input-medium">
                                <option value="EstimateId" selected="selected">ID do Orçamento</option>
                                <option value="EstimateTitle">Título do Orçamento</option>
                                <option value="FirstName">Cliente (Nome)</option>
                                <option value="LastName">Cliente (Sobrenome)</option>
                                <option value="DisplayName">Cliente (Nome no Site)</option>
                                <option value="CompanyNameName">Cliente (Razão Social)</option>
                                <option value="CreatedOnDate">Criado Em</option>
                                <option value="ModifiedOnDate">Alterado Em</option>
                            </select>
                            <div class="form-inline">
                                <strong>Palavra Chave:</strong>
                                <i class="icon icon-exclamation-sign" title="Palavra Chave" data-content="Filtre por palavras chaves. As opções são: Título do orçamento, nome próprio, nome da empresas, email, telefone, cpf ou cnpj. Insire a palavra e clique em [Aplicar Filtro]."></i>
                                <input autocomplete="off" type="text" class="input-small" placeholder="Palavra Chave" data-bind="value: filter" />
                                <button id="btnSearch" class="btn btn-small" title="Clique aqui para fazer sua busca"><span class="icon icon-search"></span>&nbsp; Aplicar Filtro</button>
                                <button id="btnRemoveFilter" class="btn btn-small" title="Clique aqui para remover filtros"><span class="k-icon k-i-cancel"></span></button>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div class="clearfix"></div>
    <div id="estimatesGrid"></div>
    <script id="tmplToolbar" type="text/x-kendo-template">
        <ul id="ulToolbar">
            <li>
                <button id="btnAddEstimate" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-plus"></i>&nbsp; Adicionar</button>
            </li>
            <li>
                <button id="btnRemoveSelectedEstimate" class="btn btn-small btn-danger" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
            </li>
            <li>
                <button id="btnRefundSelectedEstimate" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon icon-refresh"></i> Restituir</button>
            </li>
        </ul>
    </script>
</div>
<div id="estimateWindow"></div>
<div id="HTML5Audio">
    <input id="audiofile" type="text" value="" style="display: none;"/></div>
<audio id="myaudio">
    <script>
        function LegacyPlaySound(soundobj) {
            var thissound = document.getElementById(soundobj);
            thissound.Play();
        }
    </script>
    <span id="OldSound"></span>        
    <input type="button" value="Play Sound" onClick="LegacyPlaySound('LegacySound')">
</audio>