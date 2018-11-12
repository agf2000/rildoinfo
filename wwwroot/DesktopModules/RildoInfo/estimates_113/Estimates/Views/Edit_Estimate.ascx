<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Edit_Estimate.ascx.vb" Inherits="RIW.Modules.Estimates.Views.EditEstimate" %>
<div id="divEditEstimate" class="row-fluid">
    <div class="span12">
        <div id="estimateTabs">
            <ul>
                <li id="tab_1" class="k-state-active">Orçamento
                </li>
                <li id="tab_2" data-bind="visible: authorized > 1">Mensagens
                </li>
                <li id="tab_3">Histórico
                </li>
                <li id="tab_4" data-bind="visible: authorized > 1">Termos
                </li>
                <li id="tab_5" data-bind="visible: authorized > 1">Configurações
                </li>
                <li id="tab_6" data-bind="visible: authorized > 1">Email
                </li>
            </ul>
            <div>
                <div id="divOpenExtraInfo" class="right"><a id="clickExtraInfo">Mais Informações</a> &nbsp;</div>
                <div id="divExtraInfo">
                    <div class="span7">
                        <h4 id="estimateTitleLabel"></h4>
                        <div id="divEstimateTitle">
                            <input id="estimateTitleTextBox" type="text" class="input-xlarge" placeholder="Nome do orçamento... (opcional)" />
                            <a id="btnUpdateTitle" title="Salvar">
                                <img alt="Salvar" src="/images/eip_title_save.png" /></a>
                            <a id="btnUpdateCancelTitle" title="Cancelar">
                                <img alt="Salvar" src="/images/eip_title_cancel.png" /></a>
                        </div>
                        <div id="divSalesPerson"></div>
                    </div>
                    <div class="span4">
                        <div id="spanClientInfo"></div>
                        <div id="divClient">
                            <button id="btnClientReplace" class="btn btn-mini" data-bind="visible: authorized > 2">Substituir Cliente</button>&nbsp;
                        <button id="btnCancelClientReplace" class="btn btn-mini"><i class="fa fa-chevron-left"></i>&nbsp; Cancelar</button>
                            <div id="divClientSearch" class="padded">
                                <input id="clientSearch" style="width: 100%;" />
                                <div class="padded"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clearfix"></div>
                <div id="divSearchProducts" class="form-inline padded offset1">
                    <strong>Produtos:</strong>
                    <input id="productSearch" class="input-xxlarge" />&nbsp;
                    <input id="qTyTextBox" class="input-mini" />&nbsp;
                    <button id="btnAddSelectedProduct" class="btn btn-small" data-bind="attr: { 'disabled': estimateLocked() }"><i class="icon-plus"></i>&nbsp; Adicionar</button>
                </div>
                <div id="estimateItemsGrid"></div>
                <script id="tmplToolbar" type="text/x-kendo-template">
                    <ul id="estimateItemsToolbar" class="inline">
                        <li>
                            <h5>Items do Orçamento</h5>
                        </li>
                        <li>
                            <button id="btnExpandCollapse" class="btn btn-small" value="0"><i class="fa fa-plus-square"></i>&nbsp; Expandir Orçamento</button>
                        </li>
                        <li>
                            <button class="btn btn-small" onclick="my.syncEstimateItems(this); return false;"><i class="fa fa-retweet" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."></i>&nbsp; Sincronizar Selecionado(s)</button>
                        </li>
                        <li>
                            <button class="btn btn-small" onclick="my.removeEstimateItem(this); return false;" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Remover Selecionado(s)</button>
                        </li>
                    </ul>
                </script>            
                <script id="tmplProductDetail" type="text/x-kendo-template">
                    <table class="itemDetailsTable">
                        <tr><td></td><tr/>
                        <tr>
                            <td>
                                # if (ProductImageId > 0) { #
                                    <a class="photo" title="${ ProductName }" href="/databaseimages/${ ProductImageId }.${ Extension }?maxwidth=800&maxheight=600# if (watermark) { # &watermark=outglow&text=${ watermark } # } #" style="cursor: url(/desktopmodules/rildoinfo/webapi/content/images/zoomin.cur), pointer;">
                                        <img src="/databaseimages/${ ProductImageId }.${ Extension }?maxwidth=130&maxheight=130# if (watermark) { # &watermark=outglow&text=${ watermark } # } #" style="cursor: url(/desktopmodules/rildoinfo/webapi/content/images/zoomin.cur), pointer;" /></a>
                                # } else { #
                                    <a class="aspNetDisabled">
                                        <img src="/portals/0/Images/No-Image.jpg?w=130&h=130" />
                                    </a>
                                # } #
                                <div class="imagesListView"></div>
                            </td>
                            <td valign="top">
                                <h4>${ ProductName }</h4>
                                <small>${ Summary }</small>
                                <ul>
                                    <li>
                                        # if (Barcode) { #
                                            <strong>CB: </strong> ${ Barcode }
                                        # } else if (ProductRef) { #
                                            <strong>REF: </strong> ${ ProductRef }
                                        # } #
                                    </li>
                                    <li>
                                        # if (CategoriesNames) { #
                                            <strong>Categorias: </strong> ${ CategoriesNames }
                                        # } #
                                    </li>
                                </ul>
                            </td>
                        </tr>
                        <tr><td></td><tr/>
                    </table>
                </script>
                <script id="tmplProductImages" type="text/x-kendo-template">
                    <div class="pull-left padded">
                        <a class="photo" href="/databaseimages/${ ProductImageId }.${ Extension }?maxwidth=800&maxheight=600# if (watermark) { # &watermark=outglow&text=${ watermark } # } #" style="cursor: url(/desktopmodules/rildoinfo/webapi/content/images/zoomin.cur), pointer;">
                            <img src="/databaseimages/${ ProductImageId }.${ Extension }?maxwidth=30&maxheight=30# if (watermark) { # &watermark=outglow&text=${ watermark } # } #" style="cursor: url(/desktopmodules/rildoinfo/webapi/content/images/zoomin.cur), pointer;" /></a>
                    </div>
                </script>
                <div class="padded"></div>
                <div id="divPrintButtons" class="pull-left">
                    <ul class="inline">
                        <li>
                            <button class="btn btn-small btnReturn" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-chevron-left"></i>&nbsp; Retornar</button>
                        </li>
                        <li>
                            <button id="btnEmailEstimate" class="btn btn-small" data-bind="enable: clientEmail"><i class="fa fa-envelope"></i>&nbsp; Email</button>
                        </li>
                        <li>
                            <button id="btnPrintPdf" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon icon-print"></i>&nbsp;Baixar</button>
                        </li>
                        <li style="display: none;">
                            <button id="btnReloadEstimate" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon icon-refresh"></i>&nbsp;Recarregar</button>
                        </li>
                        <li>
                            <button id="btnNotifyClient" class="btn btn-small" data-bind="enable: clientEmail, visible: authorized > 1" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-bullhorn"></i>&nbsp; Notificar Cliente</button>
                        </li>
                        <li>
                            <button id="btnGenerateSale" class="btn btn-small" data-bind="visible: authorized > 1" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon icon-shopping-cart"></i>&nbsp;Gerar Venda</button>
                        </li>
                        <li>
                            <label class="checkbox padded">
                                <input id="chkBoxPrintConds" type="checkbox" />Imprimir condições de pagamento no PDF.
                            </label>
                        </li>
                    </ul>
                </div>
                <div class="pull-right">
                    <div class="text-right padded">
                        <button id="btnUpdateEstimateItems" class="btn btn-small btn-inverse" onclick="my.updateEstimateItems(this); return false;" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." data-bind="attr: { 'disabled': estimateLocked() }"><i class="fa fa-check"></i>&nbsp; Atualizar Orçamento</button>
                    </div>
                </div>
                <div class="clearfix"></div>
                <div class="pull-left padded">
                    <div id="divPayForms" data-bind="visible: ((!estimateLocked()) || (authorized !== 1))">
                        <label class="NormalBold">Formas de Pagamento:</label>
                        <input id="ddlPayForms" data-bind="kendoDropDownList: { data: payForms, value: selectedPayFormId, dataTextField: 'PayFormTitle', dataValueField: 'PayFormId', select: payFormSelected, index: 0, template: '# if (data.IsDeleted) { # <span class=lThrough> #= data.PayFormTitle # </span> # } else { # <span> #= data.PayFormTitle # </span> # } #' }" class="input-large" />&nbsp;
                        <span id="payCondMsg"></span>
                    </div>
                    <div class="padded"></div>
                    <div id="divPayFormCond">
                        <h5>Condições de Pagamento para <span data-bind="html: payFormTitle"></span>:</h5>
                        <div id="payCondGrid"></div>
                    </div>
                    <div id="divChosenPayCond">
                        <div class="thumbnail">
                            <div id="divPayIn">
                                <label class="NormalBold">Entrada Mínima:</label>
                                <input id="payIn" data-bind="kendoNumericTextBox: { value: conditionPayIn, min: payInMin, spinners: false }" />
                                <span id="spanPayInDays" data-bind="text: ' com ' + conditionPayInDays() + ' dia(s)'"></span>
                            </div>
                            <h5>A forma de pagamento escolhida é <span data-bind="html: selectedPayForm"></span>.</h5>
                            <table id="preCondition" class="table table-striped table-bordered table-responsive">
                                <thead>
                                    <tr>
                                        <th class="NormalBold">Número de Parcelas
                                        </th>
                                        <th class="NormalBold">Valor da Parc.
                                        </th>
                                        <th class="NormalBold">Total do Parcelado
                                        </th>
                                        <th class="NormalBold">Juros (%) a.m.
                                        </th>
                                        <th class="NormalBold">Intervalo (dias)
                                        </th>
                                        <th class="NormalBold">Total
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>
                                            <span id="NPayments" data-bind="text: conditionNPayments"></span>
                                        </td>
                                        <td>
                                            <span id="payment" data-bind="text: kendo.toString(conditionPayment(), 'n')"></span>
                                        </td>
                                        <td>
                                            <span id="payments" data-bind="text: kendo.toString(conditionPayments(), 'n')"></span>
                                        </td>
                                        <td>
                                            <span id="interest" data-bind="text: kendo.format('{0:p}', conditionInterest() / 100)"></span>
                                        </td>
                                        <td>
                                            <span id="interval" data-bind="text: conditionInterval()"></span>
                                        </td>
                                        <td>
                                            <span id="total" data-bind="text: kendo.toString(conditionTotalPay(), 'n')"></span>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <ul class="inline">
                                <li>
                                    <button id="btnSavePayFormCond" class="btn btn-small btn-info"><i class="fa fa-check"></i>&nbsp; Salvar alterações nas condições de Pagamento</button>
                                </li>
                                <li>
                                    <button id="btnCancelPayFormCond" class="btn btn-small"><i class="fa fa-chevron-left"></i>&nbsp; Redefinir Condição de Pagamento</button>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="pull-right well">
                    <dl id="estimateAmount" class="dl-horizontal" data-bind="visible: (authorized > 1 || viewPrice())">
                        <dt data-bind="visible: authorized > 2">Desconto Geral %:</dt>
                        <dd data-bind="visible: authorized > 2"><input id="estimateDiscount" data-bind="    kendoNumericTextBox: { value: 0, spinners: false }" class="input-mini" /></dd>
                        <dt id="dtOriginalAmount">Valor Original:</dt>
                        <dd id="ddOriginalAmount"></dd>
                        <dt id="dtProductDiscount">Desc. Produto:</dt>
                        <dd id="ddProductDiscountValue"></dd>
                        <dt id="dtEstimateDiscount">Desc. Orçamento:</dt>
                        <dd id="ddEstimateDiscountValue"></dd>
                        <dt id="dtDiscount">Desc. Total R$:</dt>
                        <dd id="ddTotalDiscountValue"></dd>
                        <dt id="dtTotalDiscount">Desc. Total %:</dt>
                        <dd id="ddTotalDiscountPerc"></dd>
                        <dt id="dtTotal">Valor Final:</dt>
                        <dd id="ddTotal"></dd>
                    </dl>
                    <dl id="adminAmount" class="dl-horizontal" data-bind="visible: authorized > 2">
                        <dt id="dtMarkupAmount">Markup R$:</dt>
                        <dd id="ddMarkupAmount"></dd>
                        <dt id="dtMarkupPerc">Markup %:</dt>
                        <dd id="ddMarkupPerc"></dd>
                        <dt id="dtProfitAmount">Lucro R$:</dt>
                        <dd id="ddProfitAmount"></dd>
                        <dt id="dtProfitPerc">Lucro %:</dt>
                        <dd id="ddProfitPerc"></dd>
                        <dt id="dtCommsAmount">Comissões R$:</dt>
                        <dd id="ddCommsAmount"></dd>
                        <dt id="dtCommsPerc">Comissões %:</dt>
                        <dd id="ddCommsPerc"></dd>
                    </dl>
                </div>
                <div class="clearfix"></div>
            </div>
            <div>
                <br />
                <div class="form-horizontal">
                    <div class="control-group">
                        <label class="control-label">
                            <strong>Mensagem:</strong><br />
                            <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                            <button class="btn btn-mini togglePreview" value="preview" data-provider="estimateMessageTextarea" data-toggle="tooltip" title="Clique aqui para pre-visualizar a mensagem.">
                                <i class="fa fa-eye"></i>&nbsp; Visualizar
                            </button>
                        </label>
                        <div class="controls">
                            <textarea id="estimateMessageTextarea" class="markdown-editor"></textarea><br />
                            <button id="btnAddMessage" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                        </div>
                    </div>
                </div>
                <div class="pull-left offset1 hidden">
                    <label class="checkbox padded" data-bind="visible: authorized > 1">
                        <input id="managerOnlyCheckbox" type="checkbox" checked="checked" />Mensagem entre gerentes e o vendedor.
                    </label>
                </div>
                <div class="clearfix padded"></div>
                <div class="accordion">
                    <div class="accordion-group">
                        <div class="accordion-heading">
                            <div class="pull-left">
                                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseMessages">
                                    <h4>Mensagens (<span data-bind="text: filteredMessages().length"></span>)</h4>
                                </a>
                            </div>
                            <div class="pull-right">
                                <br />
                                <strong>Filtrar: </strong>
                                <input type="text" data-bind="value: filterMessageTerm, valueUpdate: 'afterkeydown'" placeholder="Filtrar mensagens..." />
                                &nbsp;
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div id="collapseMessages" class="accordion-body collapse in">
                            <div class="accordion-inner">
                                <ul class="msgHolder" data-bind="foreach: filteredMessages">
                                    <li class="postHolder">
                                        <img data-bind="attr: { src: messageByAvatar }" />
                                        <p>
                                            <a data-bind="text: messageByName"></a>: <span class="timeago" data-bind="html: createdOnDate"></span><span data-bind="html: messageText"></span>
                                        </p>
                                        <div class="postFooter">
                                            <a class="linkComment" href="#" data-bind="click: toggleComment">Comentários (<span data-bind="    text: messageComments().length"></span>)</a>
                                            <div class="commentSection">
                                                <ul data-bind="foreach: messageComments">
                                                    <li class="commentHolder">
                                                        <img data-bind="attr: { src: commentedByAvatar }"><a data-bind="    text: commentedByName"></a>: <span class="timeago" data-bind="    html: createdOnDate"></span><span data-bind="    html: commentText"></span>
                                                    </li>
                                                </ul>
                                                <div class="publishComment">
                                                    <input class="commentTextArea" data-bind="value: newCommentMessage, jqAutoresize: {}" placeholder="digite seu comentário..." />
                                                    <input type="button" value="Comentar" class="btnComment btn btn-mini btn-primary" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." data-bind="click: addMessageComment.bind($data, $index())" />
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
            <div>
                <br />
                <div class="form-horizontal">
                    <div class="control-group">
                        <label class="control-label">
                            <strong>Adicionar ao Histórico:</strong><br />
                            <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                            <button class="btn btn-mini togglePreview" value="preview" data-provider="estimateHistoryTextarea" data-toggle="tooltip" title="Clique aqui para pre-visualizar a mensagem.">
                                <i class="fa fa-eye"></i>&nbsp; Visualizar
                            </button>
                        </label>
                        <div class="controls">
                            <textarea id="estimateHistoryTextarea" class="markdown-editor"></textarea><br />
                            <button id="btnAddHistory" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
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
                                <input type="text" data-bind="value: filterHistoryTerm, valueUpdate: 'afterkeydown'" placeholder="Filtrar histórico..." />
                                &nbsp;
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
            <div>
                <br />
                <div class="form-horizontal">
                    <div class="control-group">
                        <label class="control-label">
                            <strong>Termos deste orçamento:</strong><br />
                            <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                            <button class="btn btn-mini togglePreview" value="preview" data-provider="estimateTermTextarea" data-toggle="tooltip" title="Clique aqui para alternar em visualizar e editar o termo digitado">
                                <i class="fa fa-eye"></i>&nbsp; Visualizar
                            </button>
                        </label>
                        <div class="controls">
                            <textarea id="estimateTermTextarea" class="markdown-editor"></textarea>
                        </div>
                    </div>
                </div>
                <div class="padded">
                    <button id="btnUpdateTerm" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp;Atualizar</button>
                </div>
            </div>
            <div>
                <br />
                <div class="form-horizontal">
                    <div class="control-group" data-bind="visible: authorized > 2">
                        <label class="control-label" for="kddlSalesGroup"><strong>Vendedor:</strong></label>
                        <div class="controls">
                            <input id="kddlSalesGroup" />
                        </div>
                    </div>
                    <%--<div class="control-group hidden" data-bind="visible: authorized > 2">
                        <label class="control-label" for="estimateDiscount"><strong>Desconto %:</strong></label>
                        <div class="controls">
                            <input data-bind="kendoNumericTextBox: { value: 0, spinners: true }" class="input-small" />
                        </div>
                    </div>--%>
                    <div class="control-group">
                        <label class="control-label" for="kddlStatuses"><strong>Status:</strong></label>
                        <div class="controls">
                            <input id="kddlStatuses" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label" for="viewPriceCheckBox"><strong>Mostrar Preço:</strong></label>
                        <div class="controls">
                            <input id="viewPriceCheckBox" type="checkbox" class="normalCheckBox switch-small" data-on-label="SIM" data-off-label="NÃO" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label" for="lockCheckBox"><strong>Trancar Orçamento:</strong></label>
                        <div class="controls">
                            <input id="lockedCheckBox" type="checkbox" class="normalCheckBox switch-small" data-on-label="SIM" data-off-label="NÃO" />
                        </div>
                    </div>
                </div>
                <div class="padded">
                    <button id="btnUpdateConfig" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp;Atualizar</button>
                </div>
            </div>
            <div>
                <p class="alert alert-info">
                    Configurações de Email.<br />
                    Personalize o email com o link do orçamento. 
                    Nota*: Nos campos assunto e de mensagem do email existem palavras chaves chamadas "Token". 
                    Estes tokens são usados pelo sistema e substituidos por itens dinâmicos.<br />
                    [ID] = O número do orçamento.<br />
                    [WEBSITELINK] = O endereço do website.<br />
                    [CLIENTE] = O nome do cliente.<br />
                    [ORCAMENTOLINK] = O endereço para o orçamento.<br />
                </p>
                <fieldset>
                    <legend>Email</legend>
                    <div class="row-fluid">
                        <div class="form-horizontal">
                            <div class="control-group">
                                <label class="control-label required" for="subjectTextBox">
                                    <strong>Assunto:</strong>
                                    <i class="icon icon-exclamation-sign" title="Assunto" data-content="Texto que será enviado no campo de assunto do email para o cliente. Alguns tokens podem ser usados, como [ID] e [WEBSITELINK]. Estes tokens serão substituidos por seus respectivos dados."></i>
                                </label>
                                <div class="controls">
                                    <input id="preEmailSubjectTextBox" type="text" class="enterastab input-xxlarge" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label required" for="postOfficeMessageTextBox">
                                    <strong>Mensagem:</strong>
                                    <i class="icon icon-exclamation-sign" title="Mensagem" data-content="Texto enviado para o email do cliente. Alguns tokens podem ser usados, como [CLIENTE], [ID], [ORCAMENTOLINK] e [WEBSITELINK]. Estes tokens serão substituidos por seus respectivos dados."></i>
                                    <br />
                                    <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                                    <button class="btn btn-mini togglePreview" value="preview" data-provider="preEmailMessageBody" title="Pre-Visualização" data-content="Clique aqui para pre-visualizar a mensagem.">
                                        <i class="fa fa-eye"></i>&nbsp; Visualizar
                                    </button>
                                </label>
                                <div class="controls">
                                    <textarea id="preEmailMessageBody" class="markdown-editor"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <fieldset>
                    <legend>Notificação</legend>
                    <div class="row-fluid">
                        <div class="form-horizontal">
                            <div class="control-group">
                                <label class="control-label required" for="subjectTextBox">
                                    <strong>Assunto:</strong>
                                    <i class="icon icon-exclamation-sign" title="Assunto" data-content="Texto que será enviado no campo de assunto do email para o cliente. Alguns tokens podem ser usados, como [ID] e [WEBSITELINK]. Estes tokens serão substituidos por seus respectivos dados."></i>
                                </label>
                                <div class="controls">
                                    <input id="notifySubjectTextBox" type="text" class="enterastab input-xxlarge" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label required" for="postOfficeMessageTextBox">
                                    <strong>Mensagem:</strong>
                                    <i class="icon icon-exclamation-sign" title="Mensagem" data-content="Texto enviado para o email do cliente. Alguns tokens podem ser usados, como [CLIENTE], [ID], [ORCAMENTOLINK] e [WEBSITELINK]. Estes tokens serão substituidos por seus respectivos dados."></i>
                                    <br />
                                    <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                                    <button class="btn btn-mini togglePreview" value="preview" data-provider="notifyMessageBody" title="Pre-Visualização" data-content="Clique aqui para pre-visualizar a mensagem.">
                                        <i class="fa fa-eye"></i>&nbsp; Visualizar
                                    </button>
                                </label>
                                <div class="controls">
                                    <textarea id="notifyMessageBody" class="markdown-editor"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <div class="form-actions">
                    <button id="btnUpdateEmail" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><span class="fa fa-check"></span>&nbsp; Atualizar</button>
                </div>
            </div>
        </div>
        <div id="divDeleteDialog" class="confirmDialog">
            <p>
                Favor escolher a razão pela qual está excluindo este item deste orçamento. 
            Caso esteja excluindo dois ou mais itens, e a razão da exclusão entre eles for diferente, 
            clique em [Cancelar] abaixo e selecione cada item para excluir com sua devida razão.
            </p>
            <p>
                <select id="selectDeleteReason">
                    <option value="1">Desistência</option>
                    <option value="2">Não há em estoque</option>
                    <option value="3">Gerar novo Orçamento</option>
                </select>
            </p>
        </div>
        <div id="divEmail">
            <div class="form-horizontal">
                <div class="control-group">
                    <label class="control-label" for="expandCheckbox"><strong>Mostrar Detalhes:</strong></label>
                    <div class="controls">
                        <label class="checkbox">
                            Expandir orçamento e mostrar detalhes do(s) produto(s) no anexo.
                        <input id="expandCheckbox" type="checkbox" />
                        </label>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="toEmailTextBox"><strong>Email Destinatário:</strong></label>
                    <div class="controls">
                        <input id="toEmailTextBox" type="text" class="enterastab input-xlarge" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="emailSubjectTextBox"><strong>Assunto:</strong></label>
                    <div class="controls">
                        <input id="emailSubjectTextBox" type="text" class="enterastab input-xlarge" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="messageBody">
                        <strong>Mensagem:</strong><br />
                        <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                        <button class="btn btn-mini togglePreview" value="preview" data-provider="notifyMessageBody" title="Pre-Visualização" data-content="Clique aqui para pre-visualizar a mensagem.">
                            <i class="fa fa-eye"></i>&nbsp; Visualizar
                        </button>
                    </label>
                    <div class="controls">
                        <textarea id="messageBody" class="markdown-editor"></textarea>
                    </div>
                </div>
            </div>
            <ul class="inline">
                <li>
                    <button id="btnEmail" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-envelope"></i>&nbsp; Enviar</button>
                </li>
                <li>
                    <button id="btnCancelEmail" class="btn btn-small"><i class="fa fa-chevron-left"></i>&nbsp; Cancelar</button>
                </li>
            </ul>
        </div>
        <div id="dialog-message" title="Orçamento salvo com sucesso">
            <p>
                Caro(a) <span data-bind="text: clientDisplayName()"></span>, 
                caso deseja saber mais sobre como pode interagir no seu orçamento Assita este vídeo.
            </p>
            <label class="checkbox">
                <input type="checkbox" />
                Não mostrar mais esta mensagem.
            </label>
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

