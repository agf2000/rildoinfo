<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="View_Quick_Estimate.ascx.vb" Inherits="RIW.Modules.Quick_Estimate.Views.ViewQuickEstimate" %>
<div id="divQuickEstimate" class="container-fluid Normal thumbnail" style="background-color: white;">
    <div class="row-fluid">
        <div id="newORClientWindow"></div>
        <div id="docNumber" class="pull-left">
            <h4>N&#186;: <span data-bind="text: estimateId()"></span><span data-bind="    visible: ((salesRepName().length > 0) && (estimatedByUser() !== userID)), html: ' (<strong>Vendedor:</strong> ' + salesRepName() + ')'" style="font-size: 12px !important"></span></h4>
        </div>
        <div id="clientEdit" class="pull-right">
            <span class="font-size-large">Alt+C - <strong>Cliente: </strong>
            </span>
            <input id="clientSearch" class="input-xxlarge" />
            <span class="font-size-large" data-bind="text: ' (ID: ' + personId() + ')'"></span><span id="clientToggle" class="k-icon k-i-arrow-s"></span>
        </div>
        <div class="clearfix"></div>
        <div id="clientArea" class="padded">
            <div class="span9 thumbnail">
                <div id="spanClientInfo"></div>
            </div>
            <div class="span3">
                <ul>
                    <li style="padding: 7px 0;">
                        <button id="btnAddClient" class="btn btn-small btn-info"><em>Ctrl+B</em> - Adicionar Novo Cliente</button>
                    </li>
                    <li>
                        <button id="btnEditClient" class="btn btn-small"><i class="icon-edit"></i>&nbsp; Editar Cliente</button>
                    </li>
                </ul>
            </div>
            <div class="clearfix"></div>
        </div>
        <ul id="moduleHeader">
            <li id="liLeft">
                <img id="rildoLogo" alt="Logo" src="/desktopmodules/rildoinfo/webapi/content/images/spacer.gif" />
            </li>
            <li id="liCenter">
                <div id="divTotals">
                    <span class="totals" data-bind="text: displayTotal()"></span><span data-bind="    visible: couponDiscount() > 0 || payCondDisc() > 0" style="font-size: 14px; line-height: 14px; color: orange;">D</span>
                    <div class="totals" data-bind="text: prodTotal()"></div>
                </div>
                <div id="divItemTitle" data-bind="html: productName()" class="clearfix">
                </div>
            </li>
            <li id="liRight">
                <button name="btnMinMax" id="btnMinMax" class="btn" title="Visualizar o Menu"><em class="big">Ctrl+8 - </em>Max / Min</button><br />
                <button name="btnRestart" id="btnRestart" class="btn" title="Re-Iniciar a Tela"><em class="big">Ctrl+9 - </em>Novo Orc.</button><br />
                <button name="btnLogin" id="btnLogin" class="btn"><em class="big">Ctrl+0 - </em>Login</button>
            </li>
            <li class="clearfix"></li>
        </ul>
        <div class="clearfix">
            &nbsp;
        </div>
        <div id="divCoupon" class="pull-left">
            <div id="divCouponHeader">
                <span style="width: 105px">Cod</span><span>Descrição</span><br />
                <span style="width: 30px">Item</span> <span style="width: 45px">Quant.</span> <span style="width: 30px">Und.</span> <span style="width: 71px">Preço</span> <span style="width: 70px">Desc</span>Total
            </div>
            <%--<div id="couponGrid" data-bind="kendoGrid: items"></div>--%>
            <ul id="couponGrid" data-bind="foreach: items">
                <li class="selectable" data-bind="click: currentPlace">
                    <span style="width: 100px" data-bind="text: Barcode"></span>
                    <span data-bind="text: ProductName"></span>
                    <br />
                    <span style="width: 30px" data-bind="text: ($index() + 1).toString()"></span>
                    <span style="width: 49px" data-bind="text: parseFloat(ProductQty)"></span>
                    <span style="width: 30px" data-bind="text: UnitTypeTitle"></span>
                    <span style="width: 70px" data-bind="text: kendo.toString(UnitValue, 'n') "></span>
                    <span style="width: 75px" data-bind="text: kendo.toString(ProductDiscount, 'n')"></span>
                    <span data-bind="text: kendo.toString(ExtendedAmount, 'n')"></span>
                    <a data-bind="click: my.removeItem" class="deleteItem"><span class="fa fa-times" title="Excluir Item"></span></a>
                </li>
            </ul>
            <div class="clearfix">
            </div>
            <div class="itemsInstruction hidden">
                <span><i>Ctrl+L para selecionar a lista acima</i><br />
                    <i>Espaço para selcionar o item</i></span>
            </div>
        </div>
        <div class="span7">
            <div id="divImage" class="pull-left imgHover span8">
            </div>
            <div id="divButtons" class="pull-right">
                <button id="btnPrintTxt" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." data-bind="enable: estimateId() > 0 && authorized !== 2, attr: { 'title': estimateId() > 0 && authorized !== 2 ? 'Finalizar e Fechar' : 'Desativado' }"><em class="big">Ctrl+1 - </em>Imprimir</button><br />
                <button id="btnRegister" class="btn" data-bind="enable: ((estimateId() > 0 && authorized === 2 && estimatedByUser() === userID) || (estimateId() > 0 && authorized === 2 && kendo.parseInt(selectedStatusId()) === 5) || (authorized === 3 && estimateId() > 0)), attr: { 'title': (estimateId() > 0 && authorized === 2 && kendo.parseInt(selectedStatusId()) === 5 && estimatedByUser() === userID) || (estimateId() > 0 && kendo.parseInt(selectedStatusId()) === 5) ? 'Imprimir, Finalizar e Fechar' : 'Desativado' }"><em class="big">Ctrl+2 - </em>Receber</button><br />
                <button id="btnEmailEstimate" class="btn" data-bind="enable: ((email().length > 0) && (estimateId() > 0)), attr: { 'title': ((email().length > 0) && (estimateId() > 0)) ? 'Enviar link' : 'Desativado' }"><em class="big">Ctrl+3 - </em>Email</button><br />
                <button id="btnPlans" class="btn" data-bind="enable: estimateId() > 0, attr: { 'title': estimateId() > 0 ? 'Finalizar e Fechar' : 'Desativado' }"><em class="big">Ctrl+4 - </em>Planos</button><br />
                <button id="btnDiscount" class="btn" data-bind="enable: false && ((estimateId() > 0) && (kendo.parseInt(selectedStatusId()) !== 5 || kendo.parseInt(selectedStatusId()) !== 6)), attr: { 'title': estimateId() > 0 ? 'Finalizar e Fechar' : 'Desativado' }"><em class="big">Ctrl+5 - </em>Cupon</button><br />
                <input id="getEstimateTextBox" type="text" placeholder="Ctrl+6 ORC. ID" style="width: 125px;" />
            </div>
            <div class="clearfix"></div>
            <div id="productField">
                <ul>
                    <li id="liQty" class="pull-left">
                        <span class="NormalBold font-size-large">Qde: </span>
                        <input name="itemQtyBox" id="itemQtyBox" type="text" class="enterastab" data-bind="value: itemQty" placeholder="Ctrl+Q" />
                    </li>
                    <li class="pull-left">
                        <input name="itemsSearchBox" id="itemsSearchBox" placeholder="Ctrl+P, Insira Nome, Referência ou Código." />
                    </li>
                    <li class="clearfix"></li>
                </ul>
            </div>
            <div class="clearfix"></div>
            <div class="pull-left itemsInstruction">
                <span><i>&nbsp; &nbsp; Digite 3 caracteres ou mias para iniciar busca.</i><br />
                    <i>&nbsp; &nbsp; Busque itens por Referência usando um asterisco *.</i><br />
                    <i>&nbsp; &nbsp; Busque itens por Código de Barras usando um #.</i></span>
            </div>
            <div class="pull-right text-right padded">
                <a id="configLink" data-bind="visible: authorized === 3">Configurações</a><br />
                <span>Desenvolvido por: <a href="http://web.rildoinformatica.net/" target="_blank">Web.RildoInform&#225;tica.Net</a> &nbsp; (31) 3037-0551</span>
            </div>
            <div class="clearfix"></div>
        </div>
        <div class="clearfix"></div>
        <div id="payFormCondWindow">
            <div id="divPayForms">
                <label class="NormalBold">Formas de Pagamento:</label>
                <input id="ddlPayForms" data-bind="kendoDropDownList: { data: payForms, value: selectedPayFormId, optionsText: 'Dinheiro', dataTextField: 'PayFormTitle', dataValueField: 'PayFormId', select: payFormSelected }" class="input-large" />&nbsp;
                <span id="payCondMsg"></span>
            </div>
            <div id="divPayFormCond">
                <h5>Condições de Pagamento para <span data-bind="html: payFormTitle"></span>: <small><i>Ctrl+J para selecionar a lista abaixo. Dê um "Enter" para selecionar, mais um "Enter" para usar a condição selecionada.</i></small></h5>
                <div id="payCondGrid"></div>
            </div>
            <div id="divChosenPayCond">
                <div class="thumbnail">
                    <div id="divPayIn">
                        <label class="NormalBold">Entrada Mínima: <span class="normal">Ctrl + E</span></label>
                        <input id="payIn" data-bind="kendoNumericTextBox: { value: conditionPayIn, min: payInMin, spinners: false }" />
                        <span id="spanPayInDays" data-bind="text: ' com ' + conditionPayInDays() + ' dia(s)'"></span>
                    </div>
                    <h5>A forma de pagamento escolhida é <span data-bind="html: selectedPayForm"></span>.</h5>
                    <table id="preCondition" class="table table-striped table-bordered table-condensed">
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
                            <button id="btnSavePayFormCond" class="btn btn-small btn-info" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Salvar alterações nas condições de Pagamento</button>
                        </li>
                        <li>
                            <button id="btnCancelPayFormCond" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-chevron-left"></i>&nbsp; Redefinir a condição de pagamento</button>
                        </li>
                    </ul>
                </div>
            </div>
            <br />
            <label class="NormalBold">Status:</label>
            <input id="ddlStatuses" />
            <small><i>Defina o status do orçamento (Ctrl+A).</i></small>
            <div class="pull-right">
                <button id="btnClosePayFormCond" class="btn">ESC - Fechar</button>
            </div>
            <div class="clearfix"></div>
        </div>
        <div id="divDiscount">
            <div class="form-horizontal">
                <h4 data-bind="text: kendo.toString(itemPrice(), 'c') + ' - ' + productName() + (productCode().indexOf('00000') !== -1 ? '' : ' - (Código: ' + productCode() + ')')"></h4>
                <div class="control-group" data-bind="visible: authorized > 0">
                    <label class="control-label" for="couponDiscountTextBox"><strong>Desconto Geral %:</strong></label>
                    <div class="controls">
                        <input id="couponDiscountTextBox" class="input-mini" data-bind='kendoNumericTextBox: { min: 0, value: couponDiscount, spinners: false }' />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="itemQtyTextBox"><strong>Quantidade:</strong></label>
                    <div class="controls">
                        <input id="itemQtyTextBox" class="input-mini" data-bind="kendoNumericTextBox: { decimal: 1, min: 0, value: newItemQty, spinners: false }" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="itemDiscountTextBox"><strong>Desconto Unitário %:</strong></label>
                    <div class="controls">
                        <input id="itemDiscountTextBox" class="input-mini" data-bind='kendoNumericTextBox: { min: 0, max: estimateMaxDiscount, value: newItemDiscount, spinners: false }' />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="commentsTextArea">
                        <strong>Observações:</strong><br />
                        <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                        <button id="toggleCommentsTextAreaPreview" class="btn btn-mini" value="preview" data-toggle="tooltip" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                            <i class="fa fa-eye"></i>&nbsp; Visualizar
                        </button>
                    </label>
                    <div class="controls">
                        <textarea id="commentsTextArea"></textarea>
                    </div>
                </div>
            </div>
            <ul class="inline">
                <li>
                    <button id="btnApplyDiscount" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Salvar</button>
                </li>
                <li>
                    <button class="btn btn-small btnReturn"><i class="fa fa-chevron-left"></i>&nbsp; Retornar</button>
                </li>
                <li>
                    <i>Shift+D para excluir o item selecionado</i>
                </li>
            </ul>
        </div>
        <div id="divCouponWindow" class="hidden">
            <div class="form-horizontal">
                <div class="control-group hidden">
                    <label class="control-label" for="ddlCouponProducts"><strong>Produtos:</strong></label>
                    <div class="controls">
                        <input id="ddlCouponProducts" data-bind="kendoDropDownList: { data: items, autoBind: false, dataValueField: 'EstimateItemId', dataTextField: 'ProductName', template: kendo.template($('#ddlTmplProducts').html()) }" />
                    </div>
                </div>
            </div>
            <ul class="inline">
                <li>
                    <button class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Salvar</button>
                </li>
                <li>
                    <button class="btn btn-small"><i class="fa fa-chevron-left"></i>&nbsp; Retornar</button>
                </li>
            </ul>
        </div>
        <div id="divReceipt">
            <ul>
                <li>
                    <label for="cashInTextBox">VALOR A RECEBER EM DINHEIRO:</label>
                    <input id="cashInTextBox" name="cashInTextBox" type="text" class="input-large vertical" onkeypress="return my.isNumberKey(event)" data-bind="attr: { 'readonly': (estimatedByUser() !== userID && selectedPayForm() !== '' && authorized !== 3) }, commaDecimalFormatterCash: cashIn, valueUpdate: 'afterkeydown'" />
                </li>
                <li>
                    <label for="cardInTextBox">VALOR A RECEBER EM CARTÃO:</label>
                    <input id="cardInTextBox" name="cardInTextBox" type="text" class="input-large vertical" onkeypress="return my.isNumberKey(event)" data-bind="attr: { 'readonly': (estimatedByUser() !== userID && selectedPayForm().indexOf('Cart') === -1 && authorized !== 3) }, commaDecimalFormatterCard: cardIn, valueUpdate: 'afterkeydown'" />
                </li>
                <li>
                    <label for="debitInTextBox">VALOR A RECEBER EM DÉBITO:</label>
                    <input id="debitInTextBox" name="debitInTextBox" type="text" class="input-large vertical" onkeypress="return my.isNumberKey(event)" data-bind="attr: { 'readonly': (estimatedByUser() !== userID && selectedPayForm().indexOf('Debit') === -1 && authorized !== 3) }, commaDecimalFormatterCard: debitIn, valueUpdate: 'afterkeydown'" />
                </li>
                <li>
                    <label for="checkInTextBox">VALOR A RECEBER EM CHEQUE:</label>
                    <input id="checkInTextBox" name="checkInTextBox" type="text" class="input-large vertical" onkeypress="return my.isNumberKey(event)" data-bind="attr: { 'readonly': (estimatedByUser() !== userID && selectedPayForm() !== 'Cheque' && authorized !== 3) }, commaDecimalFormatterCheck: checkIn, valueUpdate: 'afterkeydown'" />
                </li>
                <li class="hidden">
                    <label for="bankInTextBox">VALOR A RECEBER EM BOLETO:</label>
                    <input id="bankInTextBox" name="bankInTextBox" type="text" class="input-large vertical" onkeypress="return my.isNumberKey(event)" data-bind="attr: { 'readonly': (estimatedByUser() !== userID && selectedPayForm() !== 'Boleto' && authorized !== 3) }, commaDecimalFormatterBank: bankIn, valueUpdate: 'afterkeydown'" />
                </li>
                <li>
                    <label for="totalAmountTextBox">VALOR TOTAL:</label>
                    <input id="totalAmountTextBox" type="text" class="input-large" readonly data-bind="value: kendo.format('{0:c}', grandTotal())" />
                </li>
                <li>
                    <label for="cashBackTextBox" data-bind="text: balance()"></label>
                    <input id="cashBackTextBox" type="text" class="input-large" readonly data-bind="value: kendo.format('{0:c}', cashBack())" />
                </li>
            </ul>
            <div class="form-actions">
                <button id="btnCard" class="btn btn-large hidden" title="Inserir Cartão" disabled="disabled"><em class="big">Ctrl+Y </em>Cartão</button>
                <button id="btnCheck" class="btn btn-large hidden" title="Inserir Cheque" disabled="disabled"><em class="big">Ctrl+Q </em>Cheque</button>
                <button id="btnBank" class="btn btn-large hidden" title="Gerar boleto(s)" disabled="disabled"><em class="big">Ctrl+B </em>Boleto</button>
                <button id="btnPrintReceipt" class="btn btn-large" title="Imprimir Recibo" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." data-bind="attr: { 'title': kendo.parseInt(cashBack()) <= 0 ? 'Imprimir, Finalizar e Fechar' : 'Desativado' }"><em class="big">Ctrl+I - </em>Imprimir</button>
                <button id="btnFinalize" class="btn btn-large" title="Finanlizar" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." data-bind="attr: { 'title': kendo.parseInt(cashBack()) <= 0 ? 'Imprimir, Finalizar e Fechar' : 'Desativado' }"><em class="big">Ctrl+F </em>Finalizar</button>
            </div>
        </div>
        <div id="divLogin">
            <div class="status"></div>
            <div class="form-horizontal">
                <div class="control-group">
                    <label class="control-label" for="emailSubjectTextBox"><strong>Login:</strong></label>
                    <div class="controls">
                        <input name="loginTextBox" id="loginTextBox" class="Required enterastab" type="text" oninvalid="this.setCustomValidity('Insira seu Login')" validationmessage=" " />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="emailSubjectTextBox"><strong>Senha:</strong></label>
                    <div class="controls">
                        <input name="passwordTextBox" id="passwordTextBox" class="Required enterastab" type="password" oninvalid="this.setCustomValidity('Insira sua Senha')" validationmessage=" " />
                    </div>
                </div>
            </div>
            <ul class="inline">
                <li>
                    <button id="btnDoLogin" class="btn btn-small btn-inverse"><em class="big">Ctrl+U - </em>Login</button>
                </li>
                <li>
                    <button id="btnLeave" class="btn btn-small"><em class="big">Ctrl+K - </em>Sair</button>
                </li>
            </ul>
        </div>
        <div id="divEmail">
            <div class="form-horizontal">
                <div class="control-group">
                    <label class="control-label" for="emailSubjectTextBox"><strong>Email Destinatário:</strong></label>
                    <div class="controls">
                        <input id="toEmailTextBox" type="text" class="enterastab input-xxlarge" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="emailSubjectTextBox"><strong>Assunto:</strong></label>
                    <div class="controls">
                        <input id="emailSubjectTextBox" type="text" class="enterastab input-xxlarge" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        <strong>Mensagem:</strong><br />
                        <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                        <button class="btn btn-mini togglePreview" value="preview" data-provider="messageBody" title="Clique aqui para pre-visualizar a mensagem.">
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
        <div id="compactPrint" style="width: 320px;">
            <span data-bind="html: currentDate()"></span>
            <span data-bind="html: '<br /><br />' + siteName"></span>
            <span data-bind="html: '<br />htt://' + siteURL"></span>
            <span data-bind="html: siteAddress"></span>
            <span data-bind="html: 'Telefone: ' + salesRepPhone()"></span>
            <span data-bind="html: '<br />Email: ' + salesRepEmail()"></span>
            <span data-bind="html: '<br /><br />ORCAMENTO n ' + estimateId() + '<br /><br />'"></span>
            <div id="printCouponHeader">
                <span style="width: 105px">Cod</span><span>Descrição</span><br />
                <span style="width: 35px">Item</span> <span style="width: 45px">Quant.</span> <span style="width: 40px">Und.</span> <span style="width: 81px">Preço</span> <span style="width: 65px">Acre/Desc</span>Total<br />
                -----------------------------------------------------<br />
            </div>
            <div id="divProduct" data-bind="foreach: items">
                <span style="width: 100px" data-bind="text: Barcode"></span><span data-bind="    text: ProductName"></span>
                <br />
                <span style="width: 35px" data-bind="text: $index() + 1"></span><span style="width: 50px" data-bind="    text: ProductQty"></span><span style="width: 40px" data-bind="    text: UnitTypeTitle"></span><span style="width: 85px" data-bind="    text: UnitValue.toFixed(2)"></span><span style="width: 65px" data-bind="    text: ProductDiscount.toFixed(2)"></span><span data-bind="    text: ExtendedAmount"></span>
                <br />
            </div>
            <br />
            <div id="totalsDiv">
                <label>
                    SUBTOTAL: <span data-bind="text: kendo.format('{0:c}', my.lineTotal)"></span>
                </label>
                <br />
                <label>
                    TOTAL: <span data-bind="text: kendo.format('{0:c}', grandTotal())"></span>
                </label>
            </div>
            <div class="clearfix"></div>
            <br />
            Condicoes de Pagamento:<br />
            <br />
            <div id="divCondPays">
                <span style="width: 85px">Forma</span><span style="width: 45px">Qde P.</span><span>Valor Parcela</span><br />
                <span style="width: 85px">Total</span><span style="width: 65px">Jur.AM</span><span style="width: 85px">Ent.</span><span>Inter. Dias</span><br />
                -----------------------------------------------------
        <div data-bind="foreach: payConds">
            <span style="width: 85px" data-bind="text: payCondTitle()"></span>
            <span style="width: 40px" data-bind="text: payCondN()"></span>
            <span data-bind="text: kendo.format('{0:c}', parcela())"></span>
            <br />
            <span style="width: 85px" data-bind="text: kendo.format('{0:c}', totalPayCond())"></span>
            <span style="width: 60px" data-bind="text: payCondPerc()"></span>
            <span style="width: 80px" data-bind="text: kendo.format('{0:c}', payIn())"></span>
            <span data-bind="text: interval()"></span>
            <br />
        </div>
            </div>
            <br />
            Observacoes Importantes:<br />
            Validade deste orçamento é de 3 dias. Todas as imagens são meramente ilustrativas. Estes valores podem ser alterados sem prévio aviso. Prazo de entrega(s) do(s) produtos e execução dos serviços, a combinar.
        </div>
        <%--        <asp:HiddenField ID="hfEId" runat="server" />
        <asp:HiddenField ID="hfCId" runat="server" />
        <asp:HiddenField ID="hfCashIn" runat="server" />
        <asp:HiddenField ID="hfCardIn" runat="server" />
        <asp:HiddenField ID="hfCheckIn" runat="server" />
        <asp:HiddenField ID="hfBankIn" runat="server" />
        <script type="text/javascript">

            var hfCashIn = <%= hfCashIn.ClientID%>,
                hfCardIn = <%= hfCardIn.ClientID%>,
            hfCheckIn = <%= hfCheckIn.ClientID%>,
            hfBankIn = <%= hfBankIn.ClientID%>;

        function loadURL() {
            var fragment = location.hash.replace(/^.*#/, '')
            var arg1 = fragment.indexOf('/')
            var arg2 = fragment.lastIndexOf('/')
            $('#<%= hfEId.ClientID%>').val(fragment.substring('estimateId/'.length, arg2).replace('/personId', ''));
            $('#<%= hfCId.ClientID%>').val(fragment.substring(arg2 + 1));
        }

        function pageLoad() {
            loadURL()
            setTimeout(function () {
                hfCashIn.value = cashIn();
                hfCardIn.value = cardIn();
                hfCheckIn.value = checkIn();
                hfBankIn.value = bankIn();
            }, 500);
        }

        // Print Buttons
        shortcut.add('Ctrl+1', function () {
            if (authorized !== 2) $('#<%= btnPrintCompact.ClientID%>').click();
        })

        shortcut.add('Ctrl+I', function () {
            $('#<%= btnPrint.ClientID%>').click();
        })

        </script>--%>
        <!-- Countdown Dialog HTML -->
        <div id="sm-countdown-dialog">
            <p>Esta sessão vai expirar em:</p>
            <p id="sm-countdown"></p>
            <p>Clique "Continuar" para continuar trabalhando, ou "Sair" para efetuar o logout.</p>
            <p>
                <button class="k-button confirm-continue">Continuar</button>
                &nbsp;
                <button class="k-Button confirm-exit">Sair</button>
            </p>
        </div>
        <script type="text/x-kendo-template" id="acEstimate">
            <strong>Orçamento: </strong><span>${ data.EstimateId }</span><br /><strong>Valor: </strong><span>${ kendo.toString(kendo.parseFloat(data.TotalAmount), "n") }</span>
        </script>
        <script type="textx-kendo-template" id="ddlTmplProducts">
            <strong>Produto: </strong><span>${ data.ProductName }</span><br />
            # if (Barcode) { #
                <strong>CB: </strong> ${ Barcode }
            # } else if (ProductRef) { #
                <strong>REF: </strong> ${ ProductRef }
            # } #
        </script>
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
<div id="davWindow">
    <h5 id="davInfo"></h5>
    <button id="btnSaveDav">Gerar DAV</button>
    &nbsp;
    Clique no botão [Gerar DAV] para a emissão de cupom fiscal.
</div>


