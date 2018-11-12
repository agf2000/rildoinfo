<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Edit.ascx.vb" Inherits="RIW.Modules.ContactForm.Edit" %>
<div id="definitions" class="container-flui Normal">
    <div class="tabbable">
        <ul id="tabs" class="nav nav-pills">
            <li class="active"><a href="#pane1" data-toggle="tab">Definições</a></li>
            <li><a href="#pane2" data-toggle="tab">Arquivos</a></li>
            <li><a href="#pane3" data-toggle="tab">SMTP</a></li>
        </ul>
        <div class="tab-content">
            <div id="pane1" class="tab-pane active">
                <div class="form-horizontal">
                    <div class="control-group">
                        <label class="control-label" for="sendToTextBox">
                            <strong>Enviar Para:</strong>
                            <i class="fa fa-exclamation-circle" title="Email do Administrador" data-content="Email de quem receberá as mensagens de contato."></i>
                        </label>
                        <div class="controls">
                            <input id="sendToTextBox" type="text" class="enterastab input-xxlarge" data-bind="value: sendTo" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label" for="postOfficeMessageTextBox">
                            <strong>Solicitações Via Correio:</strong>
                            <i class="fa fa-exclamation-circle" title="Solicitações via Correio" data-content="Texto enviado para o email do cliente que deseja receber catálagos via correio. Alguns tokens podem ser usados, como [CLIENTE], [ENDERECO] e [WEBSITELINK]. Estes tokens serão substituidos por seus respectivos dados."></i><br />
                            <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                            <button class="btn btn-mini togglePreview" value="preview" data-provider="postOfficeMessageTextBox" data-toggle="tooltip" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                                <i class="fa fa-eye"></i>&nbsp; Visualizar
                            </button>
                        </label>
                        <div class="controls">
                            <textarea id="postOfficeMessageTextBox" class="markdown-editor"="required"></textarea>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label" for="emailMessageTextBox">
                            <strong>Solicitações Via Email:</strong>
                            <i class="fa fa-exclamation-circle" title="Solicitações via Correio" data-content="Texto enviado para o email do cliente que deseja receber catálagos via email. Alguns tokens podem ser usados, como [CLIENTE] e [WEBSITELINK]. Estes tokens serão substituidos por seus respectivos dados."></i><br />
                            <br />
                            <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                            <button class="btn btn-mini togglePreview" value="preview" data-provider="emailMessageTextBox" data-toggle="tooltip" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                                <i class="fa fa-eye"></i>&nbsp; Visualizar
                            </button>
                        </label>
                        <div class="controls">
                            <textarea id="emailMessageTextBox" class="markdown-editor"></textarea>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label" for="autoAnswerTextBox">
                            <strong>Auto Resposta:</strong>
                            <i class="fa fa-exclamation-circle" title="Auto resposta do contato" data-content="Texto enviado para o email do cliente assim que o formlário de contato for preenchido e enviado. Alguns tokens podem ser usados, como [CLIENTE] e [WEBSITELINK]. Estes tokens serão substituidos por seus respectivos dados."></i><br />
                            <br />
                            <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                            <button class="btn btn-mini togglePreview" value="preview" data-provider="autoAnswerTextBox" data-toggle="tooltip" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                                <i class="fa fa-eye"></i>&nbsp; Visualizar
                            </button>
                        </label>
                        <div class="controls">
                            <textarea id="autoAnswerTextBox" class="markdown-editor"></textarea>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label" for="reqMethodCheckbox">
                            <strong>Requisitar Método:</strong>
                            <i class="fa fa-exclamation-circle" title="Requisitar método de envio" data-content="Se sim, o cliente que deseja receber catálagos, poderá escolher por receber via Email, Correio ou ambos."></i><br />
                        </label>
                        <div class="controls">
                            <div class="make-switch" data-on-label="SIM" data-off-label="NÃO" data-bind="bootstrapSwitchOn: reqSend">
                                <input id="reqMethodCheckbox" type="checkbox" />
                            </div>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label" for="reqAddressCheckbox">
                            <strong>Requisitar Endereço:</strong>
                            <i class="fa fa-exclamation-circle" title="Requisitar preenchimento de endereço" data-content="Se sim, o cliente será apresentado com um formlário de endereço. Caso ofereça o fornecimento de catálagos via correio, certifique-se de marcar como sim a opção acima."></i><br />
                        </label>
                        <div class="controls">
                            <div class="make-switch" data-on-label="SIM" data-off-label="NÃO" data-bind="bootstrapSwitchOn: reqAddress">
                                <input id="reqAddressCheckbox" type="checkbox" />
                            </div>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label" for="reqTelephoneCheckbox">
                            <strong>Telefone Obrigatório:</strong>
                            <i class="fa fa-exclamation-circle" title="Preenchimento obrigatório de telefone" data-content="Se sim, o preenchimento de telefone será obrigatório."></i><br />
                        </label>
                        <div class="controls">
                            <div class="make-switch" data-on-label="SIM" data-off-label="NÃO" data-bind="bootstrapSwitchOn: reqTelephone">
                                <input id="reqTelephoneCheckbox" type="checkbox" />
                            </div>
                        </div>
                    </div>
                    <ul class="inline buttons">
                        <li>
                            <button id="btnSaveDefinition" class="btn btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Salvar Denifições</button>
                        </li>
                        <li>
                            <button class="btn return" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Retornar</button>
                        </li>
                    </ul>
                </div>
            </div>
            <div id="pane2" class="tab-pane">
                <div class="form-horizontal">
                    <div class="control-group">
                        <label class="control-label" for="contactNameTextBox"><strong>Nome:</strong></label>
                        <div class="controls">
                            <input id="docNameTextBox" type="text" class="enterastab" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label" for="contactNameTextBox"><strong>Descrição</strong></label>
                        <div class="controls">
                            <textarea id="docDescTextArea" class="enterastab"></textarea>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label" for="contactNameTextBox"><strong>Arquivo:</strong></label>
                        <div class="controls">
                            <input name="files" id="files" type="file" />
                        </div>
                    </div>
                </div>
                <div id="docsGrid"></div>
            </div>
            <div id="pane3" class="tab-pane">
                <div class="form-horizontal">
                    <div class="control-group">
                        <label class="control-label" for="smtpServer"><strong>Servidor SMTP:</strong></label>
                        <div class="controls">
                            <input id="smtpServer" type="text" class="input-xlarge" data-bind="value: smtpServer" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label" for="smtpPort"><strong>Porta do SMTP:</strong></label>
                        <div class="controls">
                            <input id="smtpPort" type="text" class="input-xlarge" data-bind="value: smtpPort" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label" for="smtpLogin"><strong>Login do SMTP:</strong></label>
                        <div class="controls">
                            <input id="smtpLogin" type="text" class="input-xlarge" data-bind="value: smtpLogin" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label" for="smtpPassword"><strong>Senha do SMTP:</strong></label>
                        <div class="controls">
                            <input id="smtpPassword" type="text" class="input-medium" data-bind="value: smtpPassword" />
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
                    <ul class="inline buttons">
                        <li>
                            <button id="btnSaveSMTP" class="btn btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Salvar SMTP</button>
                        </li>
                        <li>
                            <button class="btn return" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Cancelar</button>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</div>
