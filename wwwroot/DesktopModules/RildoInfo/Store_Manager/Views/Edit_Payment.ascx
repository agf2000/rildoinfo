<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Edit_Payment.ascx.vb" Inherits="RIW.Modules.Store_Manager.Views.EditPayment" %>
<div id="editPayment" class="row-fluid">
    <div class="span12">
        <div class="form-horizontal">
            <div class="span7">
                <div class="control-group">
                    <label class="control-label" for="payRadio"><strong>Tipo:</strong></label>
                    <div class="controls">
                        <label class="radio inline">
                            <input type="radio" name="optionsRadios" id="creDebRadio1" value="1" checked>
                            Cr&#233;dito
                        </label>
                        <label class="radio inline">
                            <input type="radio" name="optionsRadios" id="creDebRadio2" value="2">
                            D&#233;bito
                        </label>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="ddlAccounts"><strong>Conta:</strong></label>
                    <div class="controls">
                        <input id="ddlAccounts" />
                        <button id="btnAddAccount" class="btn btn-small" title="Clique aqui para adicionar nova conta"><span class="icon icon-plus"></span></button>
                        <button id="btnUpdateAccount" class="btn btn-small" title="Clique aqui para atualizar a conta selecionada"><span class="fa fa-check"></span></button>
                        <button id="btnRemoveAccount" class="btn btn-small" name="remove" title="Clique aqui para remover conta"><span class="k-icon k-i-close"></span></button>
                    </div>
                </div>
                <div id="divVendorTextBox" class="control-group">
                    <label class="control-label" for="vendorSearchBox"><strong>Fornecedor:</strong></label>
                    <div class="controls">
                        <input id="vendorSearchBox" />
                        <button id="btnAddVendor" class="btn btn-small" title="Clique aqui para adicionar novo fornecedor"><span class="icon icon-plus"></span></button>
                    </div>
                </div>
                <div id="divVendorLabel" class="control-group">
                    <label class="control-label" for="labelVendorName"><strong>Fornecedor:</strong></label>
                    <div class="controls">
                        <span id="labelVendorName"></span>
                        <a href="/" onclick="my.editVendor(); return false;" title="Alterar Fornecedor" data-content="Alterar fornecedor referente ao crédito ou débito."><span class="fa fa-edit"></span></a>
                    </div>
                </div>
                <div id="divClientTextBox" class="control-group">
                    <label class="control-label" for="clientSearchBox"><strong>Cliente:</strong></label>
                    <div class="controls">
                        <input id="clientSearchBox" />
                        <button id="btnAddClient" class="btn btn-small" title="Clique aqui para adicionar novo cliente"><span class="icon icon-plus"></span></button>
                        <br />
                    </div>
                </div>
                <div id="divClientLabel" class="control-group">
                    <label class="control-label" for="labelClientName"><strong>Cliente:</strong></label>
                    <div class="controls">
                        <span id="labelClientName"></span>
                        <a href="/" onclick="my.editClient(); return false;" title="Alterar Cliente" data-content="Alterar cliente referente ao crédito ou débito."><span class="fa fa-edit"></span></a>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="ddlOrigins"><strong>Origem:</strong></label>
                    <div class="controls">
                        <input id="ddlOrigins" />
                        <button id="btnAddOrigin" class="btn btn-small" title="Clique aqui para adicionar nova origem"><span class="icon icon-plus"></span></button>
                        <button id="btnUpdateOrigin" class="btn btn-small" title="Clique aqui para atualizar a origem selecionada"><span class="fa fa-check"></span></button>
                        <button id="btnRemoveOrigin" class="btn btn-small" name="remove" title="Clique aqui para remover origem"><span class="k-icon k-i-close"></span></button>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="commentTextarea"><strong>Comentário:</strong></label>
                    <div class="controls">
                        <textarea id="commentTextarea" cols="1" rows="5"></textarea>
                    </div>
                </div>
            </div>
            <div class="span5">
                <div id="divDone" class="control-group">
                    <label class="control-label" for="doneCheckbox"><strong>Quitado?</strong></label>
                    <div class="controls">
                        <input id="doneCheckbox" type="checkbox" class="normalCheckBox switch-small" data-on-label="Sim" data-off-label="Não" />
                    </div>
                </div>
                <div id="divDueDate" class="control-group">
                    <label class="control-label" for="kdpDueDate"><strong>Vencimento:</strong></label>
                    <div class="controls">
                        <input id="kdpDueDate" class="enterastab" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="kdpTransDate"><strong>Data Trans.:</strong></label>
                    <div class="controls">
                        <input id="kdpTransDate" class="enterastab" />
                    </div>
                </div>
                <div class="control-group" style="display: none;">
                    <label class="control-label" for="modifiedDateLabel"><strong>Data Mov.:</strong></label>
                    <div class="controls">
                        <span id="modifiedDateLabel"></span>
                    </div>
                </div>
                <div id="divDoc" class="control-group">
                    <label class="control-label" for="ntbDocId"><strong>Nº Doc.:</strong></label>
                    <div class="controls">
                        <input id="ntbDocId" class="enterastab input-small" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="ntbTransId"><strong>Nº Trans.:</strong></label>
                    <div class="controls">
                        <input id="ntbTransId" class="enterastab input-small" />
                    </div>
                </div>
                <div id="divOriginalAmount" class="control-group">
                    <label class="control-label" for="ntbOriginalAmount"><strong>Valor:</strong></label>
                    <div class="controls">
                        <input id="ntbOriginalAmount" class="enterastab input-small" data-bind="kendoNumericTextBox: { value: originalAmount, min: 0, format: 'c', spinners: false }" />
                    </div>
                </div>
                <div id="divInterestRate" class="control-group">
                    <label class="control-label" for="ntbInterestRate"><strong>Multa %:</strong></label>
                    <div class="controls">
                        <input id="ntbInterestRate" class="enterastab input-small" data-bind="kendoNumericTextBox: { value: interestRate, min: 0, decimals: '2', spinners: false }" />
                    </div>
                </div>
                <div id="divFee" class="control-group">
                    <label class="control-label" for="ntbFee"><strong>Encargos:</strong></label>
                    <div class="controls">
                        <input id="ntbFee" class="enterastab input-small" data-bind="kendoNumericTextBox: { value: fee, min: 0, format: 'c', spinners: false }" />
                    </div>
                </div>
                <div id="divTotal" class="control-group">
                    <label class="control-label" for="ntbPayAmount"><strong>Total:</strong></label>
                    <div class="controls">
                        <input id="ntbPayAmount" class="enterastab input-small" data-bind="kendoNumericTextBox: { value: payAmount, min: 0, format: 'c', spinners: false, enabled: payAmountBox }" />
                    </div>
                </div>
                <div id="divAgendaCheckBox" class="control-group">
                    <label class="control-label" for="agendaCheckbox"><strong>Gravar Agenda:</strong></label>
                    <div class="controls">
                        <input id="agendaCheckbox" type="checkbox" class="normalCheckBox switch-small" data-on-label="Sim" data-off-label="Não" />
                    </div>
                </div>
            </div>
            <div class="clearfix">
            </div>
        </div>
        <div class="form-actions">
            <button id="btnUpdatePayment" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><span class="fa fa-check"></span>&nbsp; Salvar</button>
            <button id="btnDelete" class="btn btn-small btn-danger"><span class="k-icon k-i-close"></span>Excluir</button>
        </div>
    </div>
</div>
<div id="clientWindow"></div>
