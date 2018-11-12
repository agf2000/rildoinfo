<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Edit_Invoice.ascx.vb" Inherits="RIW.Modules.Store_Manager.Views.EditInvoice" %>
<div id="editInvoice" class="row-fluid">
    <div class="span12">
        <div class="accordion">
            <div class="accordion-group">
                <div class="accordion-heading">
                    <a class="accordion-toggle" data-toggle="collapse" href="#collapseHeader">
                        <h4>Cabeçalho</h4>
                    </a>
                </div>
                <div id="collapseHeader" class="accordion-body collapse in">
                    <div class="accordion-inner">
                        <div class="form-horizontal">
                            <div class="span4">
                                <div class="control-group" style="display: none;">
                                    <label class="control-label" for="ntbPurchaseOrder"><strong>Orçamento:</strong></label>
                                    <div class="controls">
                                        <input id="ntbPurchaseOrder" value="0" class="enterastab input-small" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" for="ntbInvoiceNumber"><strong>Número Doc.:</strong></label>
                                    <div class="controls">
                                        <input id="ntbInvoiceNumber" class="enterastab input-small" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" for="emissionDate"><strong>Data Emissão:</strong></label>
                                    <div class="controls">
                                        <input id="emissionDate" class="enterastab" />
                                    </div>
                                </div>
                            </div>
                            <div class="span8">
                                <div id="divVendorTextBox" class="control-group">
                                    <label class="control-label" for="vendorSearchBox"><strong>Fornecedor:</strong></label>
                                    <div class="controls">
                                        <input id="vendorSearchBox" />
                                        <button id="btnAddVendor" class="btn btn-small" title="Clique aqui para adicionar novo fornecedor"><span class="icon icon-plus"></span></button>
                                    </div>
                                </div>
                                <div id="divVendorLabel" class="control-group">
                                    <label class="control-label" for="labelVendorName"><strong>Fornecedor:</strong></label>
                                    <div class="controls">
                                        <span id="labelVendorName"></span>
                                        <a onclick="my.editVendor(); return false;"><span class="fa fa-edit"></span></a>
                                    </div>
                                </div>
                                <div id="divClientTextBox" class="control-group">
                                    <label class="control-label" for="clientSearchBox"><strong>Cliente:</strong></label>
                                    <div class="controls">
                                        <input id="clientSearchBox" />
                                        <button id="btnAddClient" class="btn btn-small" title="Clique aqui para adicionar novo cliente"><span class="icon icon-plus"></span></button>
                                        <br />
                                    </div>
                                </div>
                                <div id="divClientLabel" class="control-group">
                                    <label class="control-label" for="labelClientName"><strong>Cliente:</strong></label>
                                    <div class="controls">
                                        <span id="labelClientName"></span>
                                        <a onclick="my.editClient(); return false;"><span class="fa fa-edit"></span></a>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div id="invoice">
                            <div class="clearfix"></div>
                            <div></div>
                            <div id="divInvoiceComment" class="form-inline">
                                <label>Comentário: </label>
                                <textarea id="invoiceCommentTextBox" class="input-xxlarge"></textarea>
                                &nbsp;
                                <button id="btnUpdateInvoice" class="btn btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." data-bind="enable: !locked() || my.invoiceId === 0, attr: { 'title': !locked() || my.invoiceId === 0 ? '' : 'Desativado' }"><span class="icon-plus icon-white"></span>&nbsp; Lançar</button>
                                &nbsp;
                                <label class="checkbox inline">
                                    <input id="chkBoxPurchase" type="checkbox" />Ordem de Compra
                                </label>
                                
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="accordion-group">
                <div class="accordion-heading">
                    <a class="accordion-toggle" data-toggle="collapse" href="#collapseItems">
                        <h4>Itens</h4>
                    </a>
                </div>
                <div id="collapseItems" class="accordion-body collapse in">
                    <div class="accordion-inner">
                        <table id="newInvoiceItem" class="table table-striped table-bordered table-condensed table-responsive">
                            <thead>
                                <tr>
                                    <th>Produto / Serviço</th>
                                    <th>Unidade</th>
                                    <th>Quantidade</th>
                                    <th>Preço</th>
                                    <th class="hidden">Preço 2</th>
                                    <th class="hidden">Desconto</th>
                                    <th class="hidden">Custo</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>
                                        <input id="itemSearchBox" style="width: 400px;" />
                                        <button id="btnAddNewProduct" class="btn btn-small" title="Clique aqui para adicionar novo produto ou serviço"><span class="icon icon-edit"></span></button>
                                    </td>
                                    <td>
                                        <div id="unitType" style="font-size: 1.2em; padding: 3px 0 0 3px; font-weight: bold;">UN</div>
                                    </td>
                                    <td>
                                        <input id="ntbQty" value="1" class="enterastab input-mini" />
                                    </td>
                                    <td>
                                        <input id="ntbValue1" value="0" class="enterastab input-small" />
                                    </td>
                                    <td class="hidden">
                                        <input id="ntbValue2" value="0" class="enterastab input-small" />
                                    </td>
                                    <td class="hidden">
                                        <input id="ntbDisc" value="0" class="enterastab input-small" />
                                    </td>
                                    <td class="hidden">
                                        <input id="costCheckBox" type="checkbox" class="enterastab" />
                                    </td>
                                    <td>
                                        <button id="btnAddItem" class="btn btn-small"><span class="icon-plus"></span>&nbsp; Adicionar</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div id="itemsGrid"></div>
                        <script id="tmplToolbar" type="text/x-kendo-template">
                            <ul>
                                <li class="pull-right">
                                    <button id="btnUpdateItems" class="btn btn-small btn-inverse"><i class="fa fa-check"></i>&nbsp; Atualizar</button>
                                </li>
                            </ul>
                        </script>
                        <div id="aggregate"></div>
                    </div>
                </div>
            </div>
            <div class="accordion-group">
                <div class="accordion-heading">
                    <a class="accordion-toggle" data-toggle="collapse" href="#collapsePayment">
                        <h4>Financeiro</h4>
                    </a>
                </div>
                <div id="collapsePayment" class="accordion-body collapse in">
                    <div class="accordion-inner">
                        <div class="form-horizontal">
                            <div class="span4">
                                <div class="control-group">
                                    <label class="control-label" for="payRadio"><strong>Tipo:</strong></label>
                                    <div class="controls">
                                        <label class="radio inline">
                                            <input type="radio" name="optionsRadios" id="creDebRadio1" value="1">
                                            Cr&#233;dito
                                        </label>
                                        <label class="radio inline">
                                            <input type="radio" name="optionsRadios" id="creDebRadio2" value="0" checked>
                                            D&#233;bito
                                        </label>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" for="dueDate"><strong>Vencimento:</strong></label>
                                    <div class="controls">
                                        <input id="dueDate" class="enterastab" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" for="ntbPayIn"><strong>Valor Entrada:</strong></label>
                                    <div class="controls">
                                        <input id="ntbPayIn" value="0" class="enterastab input-small" />
                                    </div>
                                </div>
                                <div id="divEstimateDDL" class="control-group">
                                    <label class="control-label" for="ddlEstimates"><strong>Orçamentos:</strong></label>
                                    <div class="controls">
                                        <input id="ddlEstimates" class="enterastab input-xlarge" />
                                    </div>
                                </div>
                                <div id="divEstimateTextBox" class="control-group">
                                    <label class="control-label" for="estimateTitleTextBox"><strong>Orçamento:</strong></label>
                                    <div class="controls">
                                        <input id="estimateTitleTextBox" class="k-textbox" placeholder="Nome do novo orçamento" />
                                    </div>
                                </div>
                            </div>
                            <div class="span4">
                                <div class="control-group">
                                    <label class="control-label" for="ntbPayQty"><strong>Nº Pagts.:</strong></label>
                                    <div class="controls">
                                        <input id="ntbPayQty" value="0" class="enterastab input-mini" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" for="ntbInterval"><strong>Intervalo:</strong></label>
                                    <div class="controls">
                                        <input id="ntbInterval" class="enterastab input-mini" placeholder="Mensal" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" for="ntbInterestRate"><strong>Juros:</strong></label>
                                    <div class="controls">
                                        <input id="ntbInterestRate" value="0" class="enterastab input-mini" />
                                    </div>
                                </div>
                            </div>
                            <div class="span4">
                                <div class="control-group">
                                    <label class="control-label" for="ntbInvoiceTotal"><strong>Total Doc.:</strong></label>
                                    <div class="controls">
                                        <input id="ntbInvoiceTotal" class="enterastab input-small" data-bind="kendoNumericTextBox: { value: invoiceTotal, min: 0, format: 'c', spinners: false }" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" for="ntbFreight"><strong>Frete:</strong></label>
                                    <div class="controls">
                                        <input id="ntbFreight" value="0" class="enterastab input-small" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" for="clientSearchBox"><strong>Total:</strong></label>
                                    <div class="controls">
                                        <label id="totalLabel"></label>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="form-actions">
                            <button id="btnUpdatePayment" class="btn btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." data-bind="enable: !locked() || my.invoiceId === 0, attr: { 'title': !locked() || my.invoiceId === 0 ? '' : 'Desativado' }"><span class="icon-plus icon-white"></span>&nbsp; Lançar Financeiro</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
<div id="invoiceWindow"></div>
