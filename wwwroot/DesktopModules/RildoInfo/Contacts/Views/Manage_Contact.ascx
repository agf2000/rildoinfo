<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Manage_Contact.ascx.vb" Inherits="Manage_Contact" %>
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
                        <input id="sendToTextBox" type="text" class="enterastab input-xxlarge" data-bind="value: _sendTo" required="required" oninvalid="this.setCustomValidity('opção obrigatória')" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label required" for="postOfficeMessageTextBox">
                        <strong>Solicitações Via Correio:</strong><br />
                        <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                        <button class="btn btn-mini togglePreview" value="preview" data-provider="postOfficeMessageTextBox" data-toggle="tooltip" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                            <i class="fa fa-eye"></i>&nbsp; Visualizar
                        </button>
                    </label>
                    <div class="controls">
                        <textarea id="postOfficeMessageTextBox" class="markdown-editor" required="required" oninvalid="this.setCustomValidity('opção obrigatória')"></textarea>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label required" for="emailMessageTextBox">
                        <strong>Solicitações Via Email:</strong><br />
                        <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                        <button class="btn btn-mini togglePreview" value="preview" data-provider="emailMessageTextBox" data-toggle="tooltip" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                            <i class="fa fa-eye"></i>&nbsp; Visualizar
                        </button>
                    </label>
                    <div class="controls">
                        <textarea id="emailMessageTextBox" class="markdown-editor" required="required" oninvalid="this.setCustomValidity('opção obrigatória')"></textarea>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label required" for="autoAnswerTextBox">
                        <strong>Auto Resposta:</strong><br />
                        <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                        <button class="btn btn-mini togglePreview" value="preview" data-provider="autoAnswerTextBox" data-toggle="tooltip" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                            <i class="fa fa-eye"></i>&nbsp; Visualizar
                        </button>
                    </label>
                    <div class="controls">
                        <textarea id="autoAnswerTextBox" class="markdown-editor" required="required" oninvalid="this.setCustomValidity('opção obrigatória')"></textarea>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="reqMethodCheckbox"><strong>Requisitar Método:</strong></label>
                    <div class="controls">
                        <div class="make-switch" data-on-label="SIM" data-off-label="NÃO" data-bind="bootstrapSwitchOn: reqMethod">
                            <input id="reqMethodCheckbox" type="checkbox" checked="checked" />
                        </div>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="reqAddressCheckbox"><strong>Requisitar Endereço:</strong></label>
                    <div class="controls">
                        <div class="make-switch" data-on-label="SIM" data-off-label="NÃO" data-bind="bootstrapSwitchOn: reqAddress">
                            <input id="reqAddressCheckbox" type="checkbox" checked="checked" />
                        </div>
                    </div>
                </div>
                <ul class="inline">
                    <li>
                        <button id="btnSaveDefinition" class="btn btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Salvar Denifições</button>
                    </li>
                    <li>
                        <button id="btnCancelDefinition" class="btn" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Retornar</button>
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
                        <div class="make-switch" data-on-label="SIM" data-off-label="NÃO" data-bind="bootstrapSwitchOn: smtpConnection">
                            <input id="smtpConnection" type="checkbox" class="normalCheckBox" />
                        </div>
                    </div>
                </div>
                <ul class="inline">
                    <li>
                        <button id="btnSaveSMTP" class="btn btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Salvar SMTP</button>
                    </li>
                    <li>
                        <button id="btnCancelSMTP" class="btn" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Cancelar</button>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</div>
<div id="markdownPreview"></div>