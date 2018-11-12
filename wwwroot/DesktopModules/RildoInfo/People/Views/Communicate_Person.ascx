<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Communicate_Person.ascx.vb" Inherits="RIW.Modules.People.Views.CommunicatePerson" %>
<div id="editMsg" class="row-fluid">
    <div class="span12">
        <div id="actionsMenu">
            <ul>
                <li id="12">
	                <i class="fa fa-chevron-left"></i>&nbsp; Retornar
                </li>
                <li id="7">
	                <i class="icon icon-edit"></i>&nbsp;Dados Básicos
                </li>
                <li id="8">
	                <i class="fa fa-phone-square"></i>&nbsp; Atender
                </li>
                <li id="9">
	                <i class="fa fa-usd"></i>&nbsp; Dados Financeiros
                </li>
                <li id="11">
	                <i class="fa fa-envelope"></i>&nbsp; Comunicar
                </li>
                <li id="10">
	                <i class="icon-tasks"></i>&nbsp;Mais Ações
                </li>
            </ul>
        </div>
        <div class="accordion">
            <div class="accordion-group">
                <div class="accordion-heading">
                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseCommunicate">
                        <h4>Enviar Mensagem ao Cliente</h4>
                    </a>
                </div>
                <div id="collapseCommunicate" class="accordion-body collapse in">
                    <div class="accordion-inner">
                        <div class="status NormalRed"></div>
                        <div class="form-horizontal">
                            <div class="control-group">
                                <label class="control-label" for="emailSubjectTextBox"><strong>Assunto:</strong></label>
                                <div class="controls">
                                    <input id="emailSubjectTextBox" type="text" class="enterastab" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label" for="emailBody">
                                    <strong>Mensagem:</strong><br />
                                    <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a>
                                </label>
                                <div id="liEditor" class="controls">
                                    <textarea id="emailBody" class="enterastab" rows="10" cols="30" style="width: 99%; height: 260px;"></textarea>
                                    <label class="checkbox">
                                        <input id="addToHistory" type="checkbox" />
                                        <span>Adicionar ao histórico</span>
                                    </label>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label" for="ddlMsgContacts"><strong>Para Quem?</strong></label>
                                <div class="controls" style="white-space: nowrap;">
                                    <input class="ddlContacts" data-bind="kendoMultiSelect: { data: personContactsData, dataValueField: 'contactEmail', dataTextField: 'contactName', value: selectedContacts, itemTemplate: kendo.template($('#tmplContacts').html()), tagTemplate: kendo.template($('#tmplContacts').html()) }" />
                                    <%--<input id="ddlMsgContacts" data-bind="kendoDropDownList: { index: 0, data: personContactsData, dataTextField: 'contactName', dataValueField: 'contactEmail', template: kendo.template($('#tmplContacts').html()) }" />--%>
                                    <button class="editContacts btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Editar Contatos</button>
                                    <script type="text/x-kendo-tmpl" id="tmplContacts">
                                        <span title="${ contactEmail } ${ data.contactPhone }"> ${ contactName }</span>
                                    </script>
                                </div>
                            </div>
                            <div class="form-actions">
                                <button id="btnSendMsg" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-envelope"></i>&nbsp; Enviar Mensagem</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="accordion-group">
                <div class="accordion-heading">
                    <div class="pull-left">
                        <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseHistory3">
                            <h4>Histórico (<span data-bind="text: filteredHistories().length"></span>)</h4>
                        </a>
                    </div>
                    <div class="pull-right">
                        <br />
                        <strong>Filtrar: </strong>
                        <input type="text" data-bind="value: filterHistoryTerm, valueUpdate: 'afterkeydown'" placeholder="Filtrar histórico..." /> &nbsp;
                    </div>
                    <div class="clearfix"></div>
                </div>
                <div id="collapseHistory3" class="accordion-body collapse">
                    <div class="accordion-inner">
                        <div class="form-horizontal">
                            <div class="control-group">
                                <label class="control-label">
                                    <strong>Adicionar ao Histórico:</strong><br />
                                    <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                                    <button class="btn btn-mini togglePreview" value="preview" data-toggle="tooltip" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                                        <i class="fa fa-eye"></i>&nbsp; Visualizar
                                    </button>
                                </label>
                                <div class="controls">
                                    <textarea id="historyTextarea" class="markdown-editor"></textarea><br />
                                    <button class="btn btn-small btn-inverse btnAddHistory" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                                </div>
                            </div>
                        </div>
                        <ul class="msgHolder" data-bind="foreach: filteredHistories">
                            <li class="postHolder">
                                <img data-bind="attr: { src: historyByAvatar }" />
                                <p>
                                    <a data-bind="text: historyByName"></a>: <span class="timeago" data-bind="    html: createdOnDate"></span><span data-bind="    html: historyText"></span>
                                </p>
                                <div class="postFooter">
                                    <a class="linkComment" href="#" data-bind="click: toggleComment">Comentários (<span data-bind="    text: historyComments().length"></span>)</a>
                                    <div class="commentSection">
                                        <ul data-bind="foreach: historyComments">
                                            <li class="commentHolder">
                                                <img data-bind="attr: { src: commentedByAvatar }"><a data-bind="    text: commentedByName"></a>: <span class="timeago" data-bind="    html: createdOnDate"></span><span data-bind="    html: commentText"></span>
                                            </li>
                                        </ul>
                                        <div class="publishComment">
                                            <input class="commentTextArea" data-bind="value: newCommentHistory, jqAutoresize: {}" placeholder="digite seu comentário..." />
                                            <input type="button" value="Comentar" class="btnComment btn btn-mini btn-primary" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." data-bind="click: addHistoryComment.bind($data, $index())" />
                                        </div>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="image-browser">
        <div class="dnnLeft">
            <strong>Pasta:</strong>
            <input id="folderPathTextBox" />
            <a class="k-button" title="Ir para Raíz" onclick="my.goUp(); return false;"><span class="k-icon k-i-folder-up"></span>Ir para Raíz</a>
            <a class="k-button" style="display: none" title="Adicionar Nova Pasta" onclick="my.addPortalFolder(); return false;"><span class="k-icon k-i-folder-add"></span>Adicionar Pasta</a>
            <a class="k-button" style="display: none" title="Excluir Pasta" onclick="my.removePortalFolder(); return false;"><span class="k-icon k-i-close"></span>Excluir Pasta</a>
        </div>
        <div class="dnnRight">
            <strong>Filtrar:</strong>
            <input class="k-textbox" data-bind="value: fileSearch, valueUpdate: 'afterkeydown'" />
        </div>
        <div class="clearfix"></div>
        <div class="browser-image-header">
            <div class="dnnLeft">
                <input id="files" name="files" type="file" class="reset" />
                <span><strong>Important:</strong> Nomes de arquivos não devem contêr espaços, acentos ou caracteres como "()*&^%$#@!~"</span>
            </div>
            <div class="dnnRight DNNAlignright">
                <a class="k-button" title="Excluir Arquivo" data-bind="attr: { 'disabled': selectedFileId() > 0, class: selectedFileId() > 0 ? 'k-button' : 'k-button k-state-disabled' }" onclick="my.removePortalFile(this); return false;"><span class="k-icon k-i-close"></span>Excluir Arquivo</a>
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
        <div class="dnnLeft">
            <div style="padding-bottom: 5px;">
                <label style="width: 120px; display: inline-block; text-align: right; font-weight: bold;">Endereço URL:</label>
                <input id="fileUrlAddress" class="k-textbox" />
                <input type="checkbox" id="addLink" />
                <span>Adicionar Link</span>
            </div>
            <div>
                <label style="width: 120px; display: inline-block; text-align: right; font-weight: bold;">Título:</label>
                <input id="fileUrlAddressTitle" class="k-textbox" />
            </div>
        </div>
        <div class="dnnRight">
            <button id="btnInsert" class="btn btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Inserir</button>
            <button class="btn" onclick="my.closeImageBrowser(); return false;">Cancelar</button>
        </div>
        <div class="clearfix"></div>
    </div>
</div>
<div id="HTML5Audio">
    <input id="audiofile" type="text" value="" style="display: none;" />
</div>
<audio id="myaudio">
    <script>
        function LegacyPlaySound(soundobj) {
            var thissound = document.getElementById(soundobj);
            thissound.Play();
        }
    </script>
    <span id="OldSound"></span>
    <input type="button" value="Play Sound" onclick="LegacyPlaySound('LegacySound')">
</audio>