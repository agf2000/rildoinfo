<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Options.ascx.vb" Inherits="RIW.Modules.RIW_Contacts.Options" %>
<div class="container-flui Normal">
    <div id="tabstrip">
        <ul>
            <li class="k-state-active">Definições
            </li>
            <li>Arquivos
            </li>
            <li>SMTP
            </li>
        </ul>
        <div>
            <br />
            <div class="form-horizontal">
                <div class="control-group">
                    <label class="control-label" for="sendToTextBox"><strong>Enviar Para:</strong></label>
                    <div class="controls">
                        <input id="sendToTextBox" type="text" class="enterastab" data-bind="value: _sendTo" required="required" oninvalid="this.setCustomValidity('opção obrigatória')" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="postOfficeMessageTextBox"><strong>Solicitações Via Correio:</strong></label>
                    <div class="controls">
                        <input id="postOffieMessageTextBox" type="text" class="enterastab" data-bind="value: _postOfficeMessage" required="required" oninvalid="this.setCustomValidity('opção obrigatória')" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="emailMessageTextBox"><strong>Solicitações Via Email:</strong></label>
                    <div class="controls">
                        <input id="emailMessageTextBox" type="text" class="enterastab" data-bind="value: _emailMessage" oninvalid="this.setCustomValidity('opção obrigatória')" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="autoAnswerTextBox"><strong>Auto Resposta:</strong></label>
                    <div class="controls">
                        <input id="autoAnswerTextBox" type="text" class="enterastab" data-bind="value: _autoAnswer" oninvalid="this.setCustomValidity('opção obrigatória')" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="reqMethodCheckbox"><strong>Requisitar Método:</strong></label>
                    <div class="controls">
                        <input id="reqMethodCheckbox" type="checkbox" checked="checked" data-bind="value: _reqMethod" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="reqAddressCheckbox"><strong>Requisitar Endereço:</strong></label>
                    <div class="controls">
                        <input id="reqAddressCheckbox" type="checkbox" checked="checked" data-bind="value: _reqAddress" />
                    </div>
                </div>
                <ul class="inline">
                    <li>
                        <button id="btnSaveDefinition" class="btn btn-inverse">Salvar Denifições</button>
                    </li>
                    <li>
                        <button id="btnCancelDefinition" class="btn">Cancelar Definições</button>
                    </li>
                </ul>
            </div>
        </div>
        <div>
            <br />
            <div class="form-horizontal">
                <div class="control-group">
                    <label class="control-label"><strong>Enviar Arquivos:</strong></label>
                    <div class="controls">
                    </div>
                </div>
            </div>
        </div>
        <div>
            <br />
            <div class="form-horizontal">
                <div class="control-group">
                    <label class="control-label" for="smtpServer"><strong>Servidor SMTP:</strong></label>
                    <div class="controls">
                        <input id="smtpServer" type="text" class="input-xlarge" data-bind="value: _smtpServer" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="smtpPort"><strong>Porta do SMTP:</strong></label>
                    <div class="controls">
                        <input id="smtpPort" type="text" class="input-xlarge" data-bind="value: _smtpPort" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="smtpLogin"><strong>Login do SMTP:</strong></label>
                    <div class="controls">
                        <input id="smtpLogin" type="text" class="input-xlarge" data-bind="value: _smtpLogin" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="smtpPassword"><strong>Senha do SMTP:</strong></label>
                    <div class="controls">
                        <input id="smtpPassword" type="text" class="input-medium" data-bind="value: _smtpPassword" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="smtpConnection"><strong>Connecção do SSL:</strong></label>
                    <div class="controls">
                        <div class="make-switch" data-on-label="SIM" data-off-label="NÃO" data-bind="bootstrapSwitchOn: _smtpConnection">
                            <input id="smtpConnection" type="checkbox" class="normalCheckBox" />
                        </div>
                    </div>
                </div>
                <ul class="inline">
                    <li>
                        <button id="btnSaveSMTP" class="btn btn-inverse">Salvar SMTP</button>
                    </li>
                    <li>
                        <button id="btnCancelSMTP" class="btn">Cancelar SMTP</button>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</div>
