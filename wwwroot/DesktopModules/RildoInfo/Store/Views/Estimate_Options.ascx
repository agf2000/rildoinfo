<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Estimate_Options.ascx.vb" Inherits="RIW.Modules.Store.Views.EstimateOptions" %>
<div class="container-fluid Normal">
    <div class="form-horizontal">
        <div class="control-group hidden">
            <label class="control-label required" for="sendToTextBox">
                <strong>Enviar Para:</strong>
                <i class="icon icon-exclamation-sign" title="Email do Administrador" data-content="Email de quem receberá as mensagens sobre novos orçamentos."></i>
            </label>
            <div class="controls">
                <input id="sendNewEstimateToTextBox" type="text" class="enterastab input-xlarge" />
            </div>
        </div>
        <div class="control-group">
            <label class="control-label required" for="subjectTextBox">
                <strong>Assunto:</strong>
                <i class="icon icon-exclamation-sign" title="Assunto" data-content="Texto que será enviado no campo de assunto do email do cliente após requisitar novo orçamento. Alguns tokens podem ser usados, como [ID] e [WEBSITELINK]. Estes tokens serão substituidos por seus respectivos dados."></i>
            </label>
            <div class="controls">
                <input id="subjectTextBox" type="text" class="enterastab input-xxlarge" />
            </div>
        </div>
        <div class="control-group">
            <label class="control-label required" for="postOfficeMessageTextBox">
                <strong>Mensagem:</strong>
                <i class="icon icon-exclamation-sign" title="Mensagem" data-content="Texto enviado para o email do cliente após requisitar novo orçamento. Alguns tokens podem ser usados, como [CLIENTE], [ID] e [ORCAMENTOLINK]. Estes tokens serão substituidos por seus respectivos dados."></i><br />
                <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                <button class="btn btn-mini togglePreview" value="preview" data-provider="emailBody" data-toggle="tooltip" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                    <i class="fa fa-eye"></i>&nbsp; Visualizar
                </button>
            </label>
            <div class="controls">
                <textarea id="emailBody" class="markdown-editor"></textarea>
            </div>
        </div>
    </div>
    <ul class="inline buttons">
        <li>
            <button id="btnUpdate" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><span class="fa fa-check"></span>&nbsp; Atualizar</button>
        </li>
        <li>
            <button id="btnReturn" class="btn btn-small"><span class="fa fa-chevron-left"></span>&nbsp; Retornar</button>
        </li>
    </ul>
</div>

