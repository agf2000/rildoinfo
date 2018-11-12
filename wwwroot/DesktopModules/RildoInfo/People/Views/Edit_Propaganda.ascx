<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Edit_Propaganda.ascx.vb" Inherits="RIW.Modules.People.Views.EditPropaganda" %>
<div id="propagandaEditorWindow" class="row-fluid">
    <div style="margin: 5px 0 10px 10px;">
        <ul class="inline">
            <li>
                <button id="btnReturn" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-chevron-left"></i>&nbsp; Fechar</button>
            </li>
            <li>
                <button id="btnSavePropaganda" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Salvar</button>
            </li>
            <li>
                <button id="btnRemovePropaganda" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
            </li>
        </ul>
    </div>
    <div class="form-horizontal">
        <div class="control-group">
            <label class="control-label" for="radioPerson"><strong>Salvar Como:</strong></label>
            <div class="controls">
                <input id="propagandaName" type="text" placeholder="Assunto do email ao enviar esta propaganda..." />
            </div>
        </div>
    </div>
    <textarea id="propagandaTextArea" class="enterastab" rows="10" cols="30" style="width: 99%; height: 285px"></textarea>
    <div id="image-browser">
        <div class="form-horizontal">
            <div class="control-group">
                <label class="control-label" for="fileUrlAddress">Pasta:</label>
                <div class="controls">
                    <input id="folderPathTextBox" />
                    <a class="btn btn-small" title="Ir para Raíz" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." onclick="my.goUp(); return false;"><span class="k-icon k-i-folder-up"></span>Ir para Raíz</a>
                    <a class="btn btn-small hidden" title="Adicionar Nova Pasta" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." onclick="my.addPortalFolder(); return false;"><span class="k-icon k-i-folder-add"></span>Adicionar Pasta</a>
                    <a class="btn btn-small hidden" title="Excluir Pasta" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." onclick="my.removePortalFolder(); return false;"><span class="k-icon k-i-close"></span>Excluir Pasta</a>
                </div>
            </div>
            <div class="control-group">
                <div class="form-inline">
                    <label class="control-label" for="fileUrlAddress">Endereço URL:</label>
                    <div class="controls">
                        <input id="fileUrlAddress" type="text" class="input-xxlarge" />
                        <label class="checkbox">
                            <input type="checkbox" id="addLink" />
                            <span>Adicionar Link</span>
                        </label>
                    </div>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="fileUrlAddressTitle">Título:</label>
                <div class="controls">
                    <input id="fileUrlAddressTitle" type="text" />
                </div>
            </div>
        </div>
        <div class="offset1">
            <ul class="inline">
                <li>
                    <button id="btnInsert" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Inserir</button>
                </li>
                <li>
                    <button class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." onclick="my.closeImageBrowser(); return false;">Cancelar</button>
                </li>
            </ul>
        </div>
        <div class="browser-image-header">
            <div class="span6">
                <input id="files" name="files" type="file" />
                <span class="Normal"><strong>Important:</strong> Ao enviar novos arquivos, certifique-se que nomes de arquivos não contém espaços, acentos ou caracteres como "()*&^%$#@!~"</span>
            </div>
            <div class="span3 padded text-right">
                <strong>Filtrar:</strong>
                <input class="input-medium" type="text" data-bind="value: fileSearch, valueUpdate: 'afterkeydown'" /><br />
                <button id="btnDeleteFile" class="btn btn-small" title="Excluir Arquivo" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." data-bind="attr: { 'disabled': (!selectedFileId()) }"><span class="fa fa-times"></span>&nbsp; Excluir Arquivo</button>
            </div>
            <div class="clearfix"></div>
        </div>
        <div id="filesArea">
            <ul data-bind="foreach: filteredPortalFiles">
                <li data-bind="attr: { id: fileId, name: fileName, class: 'liFile' }" onclick="my.selectFile(this)">
                    <a data-bind="attr: { name: relativePath() + '?maxwidth=800&maxheight=600' }" onclick="my.openImage(this); return false;">
                        <img data-bind="attr: { alt: fileName, src: relativePath() + '?maxwidth=60&maxheight=60' }" />
                    </a>
                    <div class="truncate">
                        <span data-bind="text: fileName"></span>
                        <br />
                        <span data-bind="text: 'Formato: ' + extension()"></span>
                        <br />
                        <span data-bind="text: fileSize"></span>
                        <br />
                        <span data-bind="text: extension().toLowerCase() === 'jpg' || extension().toLowerCase() === 'png' || extension().toLowerCase() === 'gif' ? height() + ' x ' + width() : ''"></span>
                    </div>
                </li>
            </ul>
        </div>
    </div>
</div>
