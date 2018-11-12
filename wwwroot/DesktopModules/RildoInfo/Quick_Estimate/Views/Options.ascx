<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Options.ascx.vb" Inherits="RIW.Modules.Quick_Estimate.Views.Options" %>
<div class="container-fluid Normal">
    <div class="row-fluid">
        <div id="optionsTabs">
            <ul>
                <li class="k-state-active">
                    OR
                </li>
                <li>
                    Email
                </li>
            </ul>
            <div id="divSlogan" style="min-height: 300px;">
                <div class="form-horizontal">
                    <div class="control-group">
                        <label class="control-label" for="mainLineTextBox">
                            <strong>Slogan:</strong>
                            <i class="icon icon-exclamation-sign" title="Slogan" data-content="Texto apresentado abaixo de TOTAIS."></i>
                        </label>
                        <div class="controls">
                            <input id="mainLineTextBox" type="text" class="enterastab input-xlarge" />
                        </div>
                    </div>
                    <div class="span6">
                        <div class="control-group">
                            <label class="control-label required" for="rootImages"><strong>Imagens Existente:</strong></label>
                            <div class="controls">
                                <input id="rootImages" class="enterastab" />
                                <button class="btn btn-small fileManager"><i class="fa fa-folder-open"></i></button>
                            </div>
                        </div>
                    </div>
                    <div class="span5">
                        <div class="text-center">
                            <img id="logoImage" alt="Logo para Impressão" src="/desktopmodules/rildoinfo/webapi/content/images/spacer.gif" />
                        </div>
                    </div>
                    <div class="clearfix">
                    </div>
                    <div class="form-actions">
                        <button id="btnUpdateOr" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Atualizar</button>
                        <button class="btn btn-small btnReturn"><span class="fa fa-chevron-left"></span>&nbsp; Retornar</button>
                    </div>
                </div>
            </div>
            <div id="divEmail">
                <p class="alert alert-info">
                    Configurações para o módulo "Orçamento Rápido".<br />
                    Personalize o email com o link do orçamento. 
                    Nota*: Nos campos assunto e de mensagem do email existem palavras chaves chamadas "Tokens". 
                    Estes token são usados pelo sistema e substituidos por itens dinâmicos.<br />
                    [ID] = O número do orçamento.<br />
                    [WEBSITELINK] = O endereço do website.<br />
                    [CLIENTE] = O nome do cliente.<br />
                    [ORCAMENTOLINK] = O endereço para o orçamento.<br />
                </p>
                <div class="form-horizontal">
                    <div class="control-group">
                        <label class="control-label required" for="subjectTextBox">
                            <strong>Assunto:</strong>
                            <i class="icon icon-exclamation-sign" title="Assunto" data-content="Texto que será enviado no campo de assunto do email para o cliente. Alguns tokens podem ser usados, como [ID] e [WEBSITELINK]. Estes tokens serão substituidos por seus respectivos dados."></i>
                        </label>
                        <div class="controls">
                            <input id="subjectTextBox" type="text" class="enterastab input-xxlarge" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label required" for="postOfficeMessageTextBox">
                            <strong>Mensagem:</strong>
                            <i class="icon icon-exclamation-sign" title="Mensagem" data-content="Texto enviado para o email do cliente. Alguns tokens podem ser usados, como [CLIENTE], [ID], [ORCAMENTOLINK] e [WEBSITELINK]. Estes tokens serão substituidos por seus respectivos dados."></i>
                            <br />
                            <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                            <button class="btn btn-mini togglePreview" value="preview" data-provider="emailBody" data-toggle="tooltip" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                                <i class="fa fa-eye"></i>&nbsp; Visualizar
                            </button>
                        </label>
                        <div class="controls">
                            <textarea id="emailBody" class="markdown-editor"></textarea>
                        </div>
                    </div>
                    <div class="form-actions">
                        <button id="btnUpdate" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><span class="fa fa-check"></span>&nbsp; Atualizar</button>
                        <button class="btn btn-small btnReturn"><span class="fa fa-chevron-left"></span>&nbsp; Retornar</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
