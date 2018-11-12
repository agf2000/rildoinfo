<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Estimates.ascx.vb" Inherits="RIW.Modules.StoreAdmin.Views.Estimates" %>
<div id="estimates" class="container-fluid Normal">
    <div class="span6">
        <div class="form-horizontal">
            <div class="control-group">
                <label class="control-label" for="ddlSalesRep">
                    <strong>Vendedor Padrão:</strong>
                    <i class="icon icon-info-sign" title="Vendedor Padrão" data-content="Escolha o vendedor padrão. Todos os novos orçamentos serão designados a este vendedor."></i>
                </label>
                <div class="controls">
                    <input id="kddlSalesRep" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="ddlMainConsumer">
                    <strong>Consumidor Padrão:</strong>
                    <i class="icon icon-info-sign" title="Consumidor Padrão" data-content="Escolha o consumidor padrão."></i>
                </label>
                <div class="controls">
                    <input id="ddlMainConsumer" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="discountMax">
                    <strong>Desconto Máximo:</strong>
                    <i class="icon icon-info-sign" title="Desconto Máximo" data-content="O desconto máximo (em porcentagem) permitido ao vendedor."></i>
                </label>
                <div class="controls">
                    <input id="discountMax" class="enterastab input-small" data-bind="kendoNumericTextBox: { min: 5, value: estimateMaxDiscount, format: '' }" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="durationMax">
                    <strong>Duração Máxima:</strong>
                    <i class="icon icon-info-sign" title="Duração Máxima" data-content="O tempo de validade (em dias) de um orçamento."></i>
                </label>
                <div class="controls">
                    <input id="durationMax" class="enterastab input-small" data-bind="kendoNumericTextBox: { min: 3, value: estimateMaxDuration, format: '' }" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="viewPriceCheckBox">
                    <strong>Visualizar Preço:</strong>
                    <i class="icon icon-info-sign" title="Ocultar Preço" data-content="Permitir ou não, ver preço em novos orçamentos."></i>
                </label>
                <div class="controls">
                    <input id="viewPriceCheckBox" type="checkbox" class="normalCheckBox switch-small" data-on-label="SIM" data-off-label="NÃO" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="noStockAllowedCheckBox">
                    <strong>Estoque Esgotado:</strong>
                    <i class="icon icon-info-sign" title="Estoque Esgotado" data-content="Permitir orçamento nos itens esgotados."></i>
                </label>
                <div class="controls">
                    <input id="noStockAllowedCheckBox" type="checkbox" class="normalCheckBox switch-small" data-on-label="SIM" data-off-label="NÃO" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="kddlDefaultUnit">
                    <strong>Unidade Padrão:</strong>
                    <i class="icon icon-info-sign" title="Unidade Padrão" data-content="Escolha a unidade padrão para cadastro de novos produtos."></i>
                </label>
                <div class="controls">
                    <input id="kddlDefaultUnit" />
                </div>
            </div>
        </div>
    </div>
    <div class="span6">
        <div id="discountGroupsGrid"></div>
        <script id="ulToolbar" type="text/x-kendo-template">
            <ul class="inline">
                <li>
                    <h5>Grupos Especiais</h5>
                </li>
                <li class="pull-right">
                    <button class="btn btn-small btn-inverse" onclick="my.addGroup(); return false;"><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                </li>
            </ul>
        </script>
    </div>
    <div class="clearfix"></div>
    <div class="form-horizontal">
        <div class="control-group">
            <label class="control-label" for="estimateTermsTextArea">
                <strong>Termos:</strong>
                <i class="icon icon-info-sign" title="Termos" data-content="Termo do orçamento. Cada orçamento pode ter seu próprio termo."></i><br />
                <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                <button class="btn btn-mini togglePreview" value="preview" data-provider="estimateTermsTextArea" data-toggle="tooltip" title="Clique aqui para alternar em visualizar e editar o termo digitado">
                    <i class="fa fa-eye"></i>&nbsp; Visualizar
                </button>
            </label>
            <div class="controls">
                <textarea id="estimateTermsTextArea" class="markdown-editor"></textarea>
            </div>
        </div>
        <div class="form-actions">
			<button id="btnUpdateEstimate" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Atualizar</button>
        </div>
    </div>
    <div id="newGroup">
        <div class="form-horizontal">
            <div class="control-group">
                <label class="control-label" for="roleNameTextBox">
                    <strong>Nome:</strong>
                </label>
                <div class="controls">
                    <input id="roleNameTextBox" type="text" class="input-medium" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="descriptionTextArea">
                    <strong>Descrição:</strong>
                </label>
                <div class="controls">
                    <textarea id="descriptionTextArea" class="markdown-editor"></textarea>
                </div>
            </div>
            <div class="form-actions">
                <button id="btnAddNewRole" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Adicionar</button>
                <button id="btnReturn" class="btn btn-small"><i class="fa fa-chevron-left"></i>&nbsp; Retornar</button>
            </div>
        </div>
    </div>
</div>

