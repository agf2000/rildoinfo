<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Estimate_History.ascx.vb" Inherits="RIW.Modules.Estimates.Estimate_History" %>
<div id="divManageEstimates" class="container-fluid Normal">
    <div class="container">
        <input type="text" id="message" />
        <input type="button" id="sendmessage" value="Send" />
        <input type="hidden" id="displayname" />
        <ul id="discussion">
        </ul>
    </div>
    <div id="estimateTabs">
        <ul>
            <li id="tab_1">Orçamento
            </li>
            <li id="tab_2" class="k-state-active">Histórico
            </li>
            <li id="tab_3">Mensagens
            </li>
            <li id="tab_4">Termos
            </li>
            <li id="tab_5">Configurações
            </li>
        </ul>
        <div class="form-horizontal">
            <div class="control-group">
                <label class="control-label">
                    <strong>Mensagem:</strong><br />
                    <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                    <button id="togglePreview" class="btn btn-mini" value="preview" data-toggle="tooltip" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                        <i class="fa fa-eye"></i>&nbsp; Visualizar
                    </button>
                </label>
                <div class="controls">
                    <textarea id="messageBody" name="content" data-provide="markdown"></textarea>
                </div>
            </div>
        </div>
        <strong>Filtrar: </strong>
        <input type="text" data-bind="value: filterTerm, valueUpdate: 'afterkeydown'" />
        <div class="historyGrid" data-bind="kendoGrid: { data: filteredHistory, pageable: false, columns: [{ field: 'historyText', title: ' ', template: '#= historyText #' }], height: 240 }"></div>
    </div>
</div>

