<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="PayForms.ascx.vb" Inherits="RIW.Modules.StoreAdmin.Views.PayForms" %>
<div id="payForms" class="container-fluid Normal">
    <div id="payFormTabs">
        <ul>
            <li id="tab_1" class="k-state-active">À Vista</li>
            <li id="tab_2">Cheque Pré</li>
            <li id="tab_3">Boleto</li>
            <li id="tab_4">Visa</li>
            <li id="tab_5">Master Card</li>
            <li id="tab_6">Diners Club</li>
            <li id="tab_7">Amex</li>
            <li id="tab_8">Cartão Débito</li>
        </ul>
        <div>
            <div class="form-horizontal">
                <div class="control-group">
                    <label class="control-label" for="cashDiscount">
                        <strong>Desconto:</strong>
                        <i class="icon icon-info-sign" title="Desconto" data-content="O valor do desconto (em porcentagem) para pagamentos à vista."></i>
                    </label>
                    <div class="controls">
                        <input id="cashDiscount" class="enterastab input-small" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="cashMessageTextArea">
                        <strong>Mensagem:</strong>
                        <i class="icon icon-info-sign" title="Mensagem" data-content="Uma breve descrição apresentando o desconto para pagamentos à vista."></i>
                        <br />
                        <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                        <button class="btn btn-mini togglePreview" value="preview" data-provider="cashMessageTextArea" data-toggle="tooltip" title="Clique aqui para alternar em visualizar e editar o termo digitado">
                            <i class="fa fa-eye"></i>&nbsp; Visualizar
                        </button>
                    </label>
                    <div class="controls">
                        <textarea id="cashMessageTextArea" class="markdown-editor"></textarea>
                    </div>
                </div>
                <div class="form-actions">
			        <button id="btnUpdatePreDiscount" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Atualizar</button>
                </div>
            </div>
        </div>
        <div>
            <div id="checkPreGrid"></div>
            <script id="tmplCheckPreToolbar" type="text/x-kendo-template">
                <ul class="inline">
                    <li>
                        <button class="btn btn-small btn-inverse" onclick="my.addPayCond(); return false;"><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                    </li>
                </ul>
            </script>
        </div>
        <div>
            <div id="bankPayGrid"></div>
            <script id="tmplBankPayToolbar" type="text/x-kendo-template">
                <ul class="inline">
                    <li>
                        <button class="btn btn-small btn-inverse" onclick="my.addPayCond(); return false;"><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                    </li>
                </ul>
            </script>
        </div>
        <div>
            <div id="visaGrid"></div>
            <script id="tmplVisaToolbar" type="text/x-kendo-template">
                <ul class="inline">
                    <li>
                        <button class="btn btn-small btn-inverse" onclick="my.addPayCond(); return false;"><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                    </li>
                </ul>
            </script>
        </div>
        <div>
            <div id="mcGrid"></div>
            <script id="tmplMCToolbar" type="text/x-kendo-template">
                <ul class="inline">
                    <li>
                        <button class="btn btn-small btn-inverse" onclick="my.addPayCond(); return false;"><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                    </li>
                </ul>
            </script>
        </div>
        <div>
            <div id="dinersGrid"></div>
            <script id="tmplDinersToolbar" type="text/x-kendo-template">
                <ul class="inline">
                    <li>
                        <button class="btn btn-small btn-inverse" onclick="my.addPayCond(); return false;"><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                    </li>
                </ul>
            </script>
        </div>
        <div>
            <div id="amexGrid"></div>
            <script id="tmplAmexToolbar" type="text/x-kendo-template">
                <ul class="inline">
                    <li>
                        <button class="btn btn-small btn-inverse" onclick="my.addPayCond(); return false;"><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                    </li>
                </ul>
            </script>
        </div>
        <div>
            <div id="debitGrid"></div>
            <script id="tmplDebitToolbar" type="text/x-kendo-template">
                <ul class="inline">
                    <li>
                        <button class="btn btn-small btn-inverse" onclick="my.addPayCond(); return false;"><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                    </li>
                </ul>
            </script>
        </div>
    </div>
    <div id="newPayCond">
        <div class="form-horizontal">
            <div class="control-group">
                <label class="control-label" for="startAt">
                    <strong>Apartir de:</strong>
                    <i class="icon icon-info-sign" title="Valor Inicial" data-content="O valor mínimo do orçamento para esta condição ser usada (min. 1)."></i>
                </label>
                <div class="controls">
                    <input id="startAt" data-bind="kendoNumericTextBox: { value: 1, min: 1, format: 'c' }" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="nPays">
                    <strong>Núm. parcelas:</strong>
                    <i class="icon icon-info-sign" title="Número de Parcelas" data-content="O número de parcelas adotada para esta condição (min. 1)."></i>
                </label>
                <div class="controls">
                    <input id="nPays" data-bind="kendoNumericTextBox: { value: 0, min: 0, format: '' }" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="interest">
                    <strong>Juros (% a.m.):</strong>
                    <i class="icon icon-info-sign" title="Juros (% a.m.)" data-content="Valor do juros (em porcentagem) acrescidos para esta condição de pagamento (min. 0)."></i>
                </label>
                <div class="controls">
                    <input id="interest" data-bind="kendoNumericTextBox: { value: 0, min: 0, format: 'n' }" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="payCondIn">
                    <strong>Entrada %:</strong>
                    <i class="icon icon-info-sign" title="Entrada %" data-content="O valor mínimo da entrada (em porcentagem) requerido nesta condição de pagamento."></i>
                </label>
                <div class="controls">
                    <input id="payCondIn" data-bind="kendoNumericTextBox: { value: 0, min: 0, format: '' }" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="payInDays">
                    <strong>Dias p/ Entrada:</strong>
                    <i class="icon icon-info-sign" title="Dias p/ Entrada" data-content="A quantidade de dias p/ a entrada  (0 = no ato da compra)."></i>
                </label>
                <div class="controls">
                    <input id="payInDays" data-bind="kendoNumericTextBox: { value: 0, min: 0, format: '' }" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="interval">
                    <strong>Intervalo:</strong>
                    <i class="icon icon-info-sign" title="Intervalo" data-content="O tempo de intervalo entre cada parcela (0 para 30 dias)."></i>
                </label>
                <div class="controls">
                    <input id="interval" data-bind="kendoNumericTextBox: { value: 0, min: 0, format: '' }" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="kddlDiscountGroups">
                    <strong>Grupo:</strong>
                    <i class="icon icon-info-sign" title="Grupo" data-content="Associe um grupo à esta condição. Somente clientes contidos neste grupo terão acesso a esta condição de pagamento"></i>
                </label>
                <div class="controls">
                    <input id="kddlDiscountGroups" />
                </div>
            </div>
            <div class="form-actions">
			    <button id="btnAddPayCond" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Adicionar</button>
                <button id="btnReturn" class="btn btn-small"><i class="fa fa-chevron-left"></i>&nbsp; Retornar</button>
            </div>
        </div>
    </div>
</div>
