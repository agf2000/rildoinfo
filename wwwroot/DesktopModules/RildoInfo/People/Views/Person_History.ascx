<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Person_History.ascx.vb" Inherits="RIW.Modules.People.Views.PersonHistory" %>
<div id="divClientHistory" class="row-fluid">
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
        <div id="risHistory">
            <div class="status NormalRed"></div>
            <div class="form-horizontal">
                <div class="control-group">
                    <label class="control-label" for="radioPerson"><strong>Status:</strong></label>
                    <div class="controls">
                        <input id="ddlStatuses" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        <strong>Adicionar ao Histórico:</strong><br />
                        <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                        <button class="btn btn-mini togglePreview" value="preview" data-provider="historyTextarea" data-toggle="tooltip" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                            <i class="fa fa-eye"></i>&nbsp; Visualizar
                        </button>
                    </label>
                    <div class="controls">
                        <textarea id="historyTextarea" class="markdown-editor"></textarea><br />
                        <button id="btnAddHistory" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="accordion">
            <div class="accordion-group">
                <div class="accordion-heading">
                    <div class="pull-left">
                        <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseHistory">
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
                <div id="collapseHistory" class="accordion-body collapse in">
                    <div class="accordion-inner">
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

