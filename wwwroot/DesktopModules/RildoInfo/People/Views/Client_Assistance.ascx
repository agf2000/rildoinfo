<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Client_Assistance.ascx.vb" Inherits="RIW.Modules.People.Views.ClientAssistance" %>
<div id="divClientAssist" class="row-fluid">
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
    <div class="status NormalRed padded"></div>
    <div id="divMoreOptionsTabs">
        <ul>
            <li id="liAgenda" class="k-state-active">Agenda</li>
            <li id="liPropagandas">Propaganda</li>
            <li id="liProducts">Produtos</li>
        </ul>
        <div>
            <div id="divAgenda">
                <br />
                <div class="form-horizontal">
                    <div class="span8">
                        <div class="control-group">
                            <label class="control-label" for="ddlUsers"><strong>Agendar P/ Quem:</strong></label>
                            <div class="controls">
                                <input id="ddlUsers" />
                                <span id="imgLoading"></span>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label required" for="dtpSchedule"><strong>Data da Agenda:</strong></label>
                            <div class="controls">
                                <input id="dtpSchedule" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label"><strong>Com quem:</strong></label>
                            <div class="controls">
                                <input data-bind="kendoMultiSelect: { data: personContactsData, dataValueField: 'contactEmail', dataTextField: 'contactName', value: selectedContacts, itemTemplate: kendo.template($('#tmplContacts').html()), tagTemplate: kendo.template($('#tmplContacts').html()), placeholder: (personContactsData().length > 0 ? 'Selecionar Contato(s)' : 'Sem Registro') }" />
                                <button class="editContacts btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Editar Contatos</button>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label">
                                <strong>Mensagem:</strong><br />
                                <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                                <button class="btn btn-mini togglePreview" value="preview" data-provider="agendaMessageTextarea" data-toggle="tooltip" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                                    <i class="fa fa-eye"></i>&nbsp; Visualizar
                                </button>
                            </label>
                            <div class="controls">
                                <textarea id="agendaMessageTextarea" class="markdown-editor"></textarea>
                                <label class="checkbox">
                                    <input id="addAgendaHistory" type="checkbox" />
                                    <span>Adicionar ao histórico</span>
                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="pull-left">
                        <div><strong>Consultar Agenda:</strong></div>
                        <div id="calShedule"></div>
                        <div id="scheduledItems"></div>
                    </div>
                    <div class="clearfix"></div>
                    <div class="form-actions">
                        <button id="btnSaveAgenda" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Gravar e Enviar Agenda</button>
                    </div>
                </div>
                <div class="accordion">
                    <div class="accordion-group">
                        <div class="accordion-heading">
                            <div class="pull-left">
                                <a class="accordion-toggle" data-toggle="collapse" href="#collapseHistory1">
                                    <h4>Histórico (<span data-bind="text: filteredHistories().length"></span>)</h4>
                                </a>
                            </div>
                            <div class="pull-right">
                                <br />
                                <strong>Filtrar: </strong>
                                <input type="text" data-bind="value: filterHistoryTerm, valueUpdate: 'afterkeydown'" placeholder="Filtrar histórico..." />
                                &nbsp;
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div id="collapseHistory1" class="accordion-body collapse">
                            <div class="accordion-inner">
                                <div class="form-horizontal">
                                    <div class="control-group">
                                        <label class="control-label">
                                            <strong>Adicionar ao Histórico:</strong><br />
                                            <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                                            <button class="btn btn-mini togglePreview" value="preview" data-toggle="tooltip" data-provider="historyTextArea1" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                                                <i class="fa fa-eye"></i>&nbsp; Visualizar
                                            </button>
                                        </label>
                                        <div class="controls">
                                            <textarea id="historyTextArea1" class="markdown-editor"></textarea><br />
                                            <button class="btn btn-small btn-inverse btnAddHistory" data-provider="historyTextArea1" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
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
        </div>
        <div>
            <div id="divPropagandas">
                <div class="form-horizontal">
                    <div class="control-group">
                        <label class="control-label" for="sentPropagandaConfirm"><strong>Status:</strong></label>
                        <div class="controls">
                            <label class="checkbox">
                                <input id="sentPropagandaConfirm" type="checkbox" />
                                <span id="sentPropagandaStatus">Nenhuma propaganda enviada recentemente.</span>
                            </label>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label required" for="ddlPropagandas"><strong>Propagandas:</strong></label>
                        <div class="controls">
                            <input id="ddlPropagandas" />
                            <button id="btnPropagandas" title="Clique aqui para visualizar propagandas" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Propagandas</button>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label"><strong>Para Quem?</strong></label>
                        <div class="controls" style="white-space: nowrap;">
                            <input data-bind="kendoMultiSelect: { data: personContactsData, dataValueField: 'contactEmail', dataTextField: 'contactName', value: selectedContacts, itemTemplate: kendo.template($('#tmplContacts').html()), tagTemplate: kendo.template($('#tmplContacts').html()), placeholder: (personContactsData().length > 0 ? 'Selecionar Contato(s)' : 'Sem Registro') }" />
                            <button class="editContacts btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Editar Contatos</button>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">
                            <strong>Mensagem:</strong><br />
                            <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                            <button class="btn btn-mini togglePreview" value="preview" data-provider="propagandaMessageTextarea" data-toggle="tooltip" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                                <i class="fa fa-eye"></i>&nbsp; Visualizar
                            </button>
                        </label>
                        <div class="controls">
                            <textarea id="propagandaMessageTextarea" class="markdown-editor"></textarea>
                            <label class="checkbox">
                                <input id="addPropagandaHistory" type="checkbox" checked="checked" />
                                <span>Adicionar ao histórico</span>
                            </label>
                        </div>
                    </div>
                    <div class="form-actions">
                        <button id="btnSendPropaganda" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-envelope"></i>&nbsp; Enviar Propaganda</button>
                    </div>
                </div>
                <div class="accordion">
                    <div class="accordion-group">
                        <div class="accordion-heading">
                            <div class="pull-left">
                                <a class="accordion-toggle" data-toggle="collapse" href="#collapseHistory2">
                                    <h4>Histórico (<span data-bind="text: filteredHistories().length"></span>)</h4>
                                </a>
                            </div>
                            <div class="pull-right">
                                <br />
                                <strong>Filtrar: </strong>
                                <input type="text" data-bind="value: filterHistoryTerm, valueUpdate: 'afterkeydown'" placeholder="Filtrar histórico..." />
                                &nbsp;
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div id="collapseHistory2" class="accordion-body collapse">
                            <div class="accordion-inner">
                                <div class="form-horizontal">
                                    <div class="control-group">
                                        <label class="control-label">
                                            <strong>Adicionar ao Histórico:</strong><br />
                                            <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                                            <button class="btn btn-mini togglePreview" value="preview" data-toggle="tooltip" data-provider="historyTextArea2" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                                                <i class="fa fa-eye"></i>&nbsp; Visualizar
                                            </button>
                                        </label>
                                        <div class="controls">
                                            <textarea id="historyTextArea2" class="markdown-editor"></textarea><br />
                                            <button class="btn btn-small btn-inverse btnAddHistory" data-provider="historyTextArea2" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
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
        </div>
        <div>
            <div id="divSearchProducts" class="form-inline padded offset1">
                <strong>Produtos:</strong>
                <input id="productSearch" class="input-xlarge" placeholder="Insira nome, referência ou código de barras" />&nbsp;
                    <button id="btnAddProduct" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus"></i>&nbsp; Adicionar</button>
            </div>
            <div id="selectedProductsGrid" data-bind="kendoGrid: { data: selectedProducts, selectable: 'row', sortable: true, toolbar: kendo.template($('#availProductsHeaderTemplate').html()), columns: [{ field: 'image', title: ' ', template: kendo.template($('#tmplProductImage').html()), width: 100 }, { field: 'prodName', title: 'Produto', template: kendo.template($('#tmplProductIntro').html()) }, { field: 'unitValue', title: 'Preço', width: 100, template: kendo.template($('#tmplProductPrice').html()) }] }">
            </div>
            <script type="text/x-kendo-tmpl" id="tmplProductPrice">
                    ${ kendo.toString(unitValue, "n") }
            </script>
            <script id="availProductsHeaderTemplate" type="text/x-kendo-tmpl">
                    <div class="dnnLeft" style="padding-top: 5px;">Itens Selecionados</div>
                    <div class="dnnRight">
                        <button id="btnRemoveSelectedItems" class="btn btn-mini">Remover Item</button>
                    </div>
                    <div class="clearfix"></div>
            </script>
            <script type="text/x-kendo-tmpl" id="tmplProductIntro">
                    <span>${ prodName }</span><br />
                    <span>#= my.converter.makeHtml(prodIntro) #</span>
            </script>
            <script type="text/x-kendo-tmpl" id="tmplProductImage">
                    # if (prodImageId > 0) { #
                        <a title="${ prodName }" name="/databaseimages/${ prodImageId }.${ extension }" style="cursor:url(/desktopmodules/rildoinfo/webapi/content/images/zoomin.cur), pointer;" onclick="my.enlargeImage(this); return false;"><img src="/databaseimages/${ prodImageId }.${ extension }?width=60&height=60" style="cursor: url(/desktopmodules/rildoinfo/webapi/content/images/zoomin.cur), pointer;" /></a>
                    # } else { #
                        <img src="/desktopmodules/rildoinfo/webapi/content/images/No-Image.jpg?maxwidth=60&maxheight=60" />
                    # } #
            </script>
            <div class="form-horizontal">
                <div class="control-group">
                    <label class="control-label" for="sentProductConfirm"><strong>Status:</strong></label>
                    <div class="controls">
                        <label class="checkbox">
                            <input id="sentProductConfirm" type="checkbox" />
                            <span id="sentProductStatus">Nenhuma informação enviada recentemente.</span>
                        </label>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label required"><strong>Enviar p/ Quem:</strong></label>
                    <div class="controls">
                        <input data-bind="kendoMultiSelect: { data: personContactsData, dataValueField: 'contactEmail', dataTextField: 'contactName', value: selectedContacts, itemTemplate: kendo.template($('#tmplContacts').html()), tagTemplate: kendo.template($('#tmplContacts').html()), placeholder: (personContactsData().length > 0 ? 'Selecionar Contato(s)' : 'Sem Registro') }" />
                        <button class="editContacts btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Editar Contatos</button>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        <strong>Mensagem:</strong><br />
                        <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                        <button class="btn btn-mini togglePreview" value="preview" data-toggle="tooltip" data-provider="productMessageTextarea" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                            <i class="fa fa-eye"></i>&nbsp; Visualizar
                        </button>
                    </label>
                    <div class="controls">
                        <textarea id="productMessageTextarea" class="markdown-editor"></textarea>
                        <label class="checkbox">
                            <input id="addProductHistory" type="checkbox" checked="checked" />
                            <span>Adicionar ao histórico</span>
                        </label>
                    </div>
                </div>
                <div class="form-actions">
                    <button id="btnSendProducts" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-envelope"></i>&nbsp; Enviar Produtos</button>
                </div>
            </div>
            <div class="accordion">
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
                            <input type="text" data-bind="value: filterHistoryTerm, valueUpdate: 'afterkeydown'" placeholder="Filtrar histórico..." />
                            &nbsp;
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
                                        <button class="btn btn-mini togglePreview" value="preview" data-toggle="tooltip" data-provider="historyTextArea3" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                                            <i class="fa fa-eye"></i>&nbsp; Visualizar
                                        </button>
                                    </label>
                                    <div class="controls">
                                        <textarea id="historyTextArea3" class="markdown-editor"></textarea><br />
                                        <button class="btn btn-small btn-inverse btnAddHistory" data-provider="historyTextArea3" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
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
    </div>
</div>
<script type="text/x-kendo-tmpl" id="tmplContacts">
    <span title="${ contactEmail } ${ data.contactPhone }"> ${ contactName }</span>
</script>
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

