<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Product_Finance.ascx.vb" Inherits="RIW.Modules.Products.Views.ProductFinance" %>
<div id="editProductFinan" class="row-fluid mTop">
    <ul class="inline">
        <li>
            <button class="btn btn-small btnReturn"><i class="fa fa-chevron-left"></i>&nbsp; Fechar</button>
        </li>
        <li>
            <button id="btnCalc" class="btn btn-small btn-primary" data-toggle="tooltip" title="Clique aqui para calcular e ativar o botão de [Salvar]">Calcular</button>
        </li>
        <li>
            <button id="btnSaveFinan" class="btn btn-small btn-inverse" disabled="disabled" data-toggle="tooltip" title="Desativado"><i class="fa fa-check"></i>&nbsp; Atualizar</button>
        </li>
    </ul>
    <ul id="productMenu">
        <li id="menu_1" class="hidden"><i class="icon-chevron-left"></i>&nbsp;Fechar</li>
        <li id="menu_2">Básico</li>
        <li id="menu_3">Imagens</li>
        <li id="menu_4">Descrição</li>
        <li id="menu_5">Vídeos</li>
        <li id="menu_6">Produtos Relacionados</li>
        <li id="menu_7">Atributos</li>
        <li id="menu_8">Frete</li>
        <li id="menu_9">SEO</li>
        <li id="menu_11" class="k-state-selected">Financeiro</li>
    </ul>
    <div class="padded"></div>
    <div class="row-fluid">
        <div class="span6">
            <div class="form-horizontal">
                <div class="control-group">
                    <label class="control-label"><strong>Custo:</strong></label>
                    <div class="controls">
                        <label class="inline">
                            <span id="costDescLabel">0</span>
                        </label>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label"><strong>Custo Bruto:</strong></label>
                    <div class="controls">
                        <label class="inline">
                            <span id="netCostLabel">0</span>
                        </label>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label"><strong>Impostos:</strong></label>
                    <div class="controls">
                        <label class="inline">
                            <span id="taxesLabel">0</span>
                        </label>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label"><strong>Comissões:</strong></label>
                    <div class="controls">
                        <label class="inline">
                            <span id="commLabel">0</span>
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="span6">
            <div class="form-horizontal">
                <div class="control-group">
                    <label class="control-label" for="markupTextBox"><strong>Markup:</strong></label>
                    <div class="controls">
                        <input id="markupTextBox" data-bind="kendoNumericTextBox: { value: finan_MarkUp, min: 1, spinners: false }" class="input-small" />
                        <input id="markupCheckbox" type="checkbox" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="salePriceTextBox"><strong>Preço Venda:</strong></label>
                    <div class="controls">
                        <input id="salePriceTextBox" data-bind="kendoNumericTextBox: { value: finan_Sale_Price, format: 'n', spinners: false }" class="input-small" />
                        <input id="salePriceCheckbox" type="checkbox" checked="checked" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="specialPriceTextBox"><strong>Preço Oferta:</strong></label>
                    <div class="controls">
                        <input id="specialPriceTextBox" data-bind="kendoNumericTextBox: { value: finan_Special_Price, format: 'n', spinners: false }" class="input-small" />
                        <input id="specialPriceCheckbox" type="checkbox" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="dealerPriceTextBox"><strong>Preço Revenda:</strong></label>
                    <div class="controls">
                        <input id="dealerPriceTextBox" data-bind="kendoNumericTextBox: { value: finan_Dealer_Price, format: 'n', spinners: false }" class="input-small" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="markupLabel"><strong>Lucro:</strong></label>
                    <div class="controls">
                        <label class="inline">
                            <span id="markupLabel" data-bind="text: profitV">0</span> <span data-bind="text: profit"></span>
                        </label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="divFinanceTabs">
        <ul>
            <li class="k-state-active">Custos Primários</li>
            <li>Impostos</li>
            <li>Comissões</li>
            <li>Tributos</li>
        </ul>
        <div>
            <div id="editProductCosts" class="row-fluid">
                <div class="form-horizontal">
                    <div class="span6">
                        <div class="control-group">
                            <label class="control-label" for="costTextBox"><strong>Custo s/ Desc:</strong></label>
                            <div class="controls">
                                <input id="costTextBox" data-bind="kendoNumericTextBox: { value: finan_Paid, format: 'n', spinners: false }" class="input-small" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="discountTextBox"><strong>Desconto %:</strong></label>
                            <div class="controls">
                                <input id="discountTextBox" data-bind="kendoNumericTextBox: { value: finan_Paid_Discount, spinners: false }" class="input-small" />
                                <input id="discountCheckbox" type="checkbox" checked="checked" name="discount" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="freightTextBox"><strong>Frete %:</strong></label>
                            <div class="controls">
                                <input id="freightTextBox" data-bind="kendoNumericTextBox: { value: finan_Freight, spinners: false }" class="input-small" />
                                <input id="freightCheckbox" type="checkbox" checked="checked" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="icmsFreightTextBox"><strong>ICMS Frete %:</strong></label>
                            <div class="controls">
                                <input id="icmsFreightTextBox" data-bind="kendoNumericTextBox: { value: finan_ICMSFreight, spinners: false }" class="input-small" />
                                <input id="icmsFreightCheckbox" type="checkbox" checked="checked" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="otherExpensesTextBox"><strong>Dispesas %:</strong></label>
                            <div class="controls">
                                <input id="otherExpensesTextBox" data-bind="kendoNumericTextBox: { value: finan_OtherExpenses, spinners: false }" class="input-small" />
                                <input id="otherExpensesCheckbox" type="checkbox" checked="checked" />
                            </div>
                        </div>
                    </div>
                    <div class="span6">
                        <br /><br />
                        <div class="control-group">
                            <label class="control-label" for="vDiscountTextBox"><strong>Desconto:</strong></label>
                            <div class="controls">
                                <input id="vDiscountTextBox" data-bind="kendoNumericTextBox: { value: finan_PaidDiscountV, format: 'n', spinners: false }" class="input-small" />
                                <input id="vDiscountCheckbox" type="checkbox" name="discount" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="vFreightTextBox"><strong>Frete:</strong></label>
                            <div class="controls">
                                <input id="vFreightTextBox" data-bind="kendoNumericTextBox: { value: finan_FreightV, format: 'n', spinners: false }" class="input-small" />
                                <input id="vFreightCheckbox" type="checkbox" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="vICMSFreightTextBox"><strong>ICMS Frete:</strong></label>
                            <div class="controls">
                                <input id="vICMSFreightTextBox" data-bind="kendoNumericTextBox: { value: finan_ICMSFreightV, format: 'n', spinners: false }" class="input-small" />
                                <input id="vICMSFreightCheckbox" type="checkbox" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="vOtherExpensesTextBox"><strong>Despesas:</strong></label>
                            <div class="controls">
                                <input id="vOtherExpensesTextBox" data-bind="kendoNumericTextBox: { value: finan_OtherExpensesV, format: 'n', spinners: false }" class="input-small" />
                                <input id="vOtherExpensesCheckbox" type="checkbox" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div>
            <div id="editProductTaxes" class="row-fluid">
                <div class="form-horizontal">
                    <div class="span6">
                        <div class="control-group">
                            <label class="control-label" for="ipiTextBox"><strong>IPI:</strong></label>
                            <div class="controls">
                                <input id="ipiTextBox" data-bind="kendoNumericTextBox: { value: finan_IPI, spinners: false }" class="input-small" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="diffICMSTextBox"><strong>Dif. ICMS:</strong></label>
                            <div class="controls">
                                <input id="diffICMSTextBox" data-bind="kendoNumericTextBox: { value: finan_DiffICMS, spinners: false }" class="input-small" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="tribSubICMSTextBox"><strong>S. T. ICMS:</strong></label>
                            <div class="controls">
                                <input id="tribSubICMSTextBox" data-bind="kendoNumericTextBox: { value: finan_TribSubICMS, spinners: false }" class="input-small" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="issTextBox"><strong>ISS:</strong></label>
                            <div class="controls">
                                <input id="issTextBox" data-bind="kendoNumericTextBox: { value: finan_ISS, spinners: false }" class="input-small" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="otherTaxesTextBox"><strong>Outros:</strong></label>
                            <div class="controls">
                                <input id="otherTaxesTextBox" data-bind="kendoNumericTextBox: { value: finan_OtherTaxes, spinners: false }" class="input-small" />
                            </div>
                        </div>
                    </div>
                    <div class="span6">
                        <div class="control-group">
                            <label class="control-label"><strong>IPI:</strong></label>
                            <div class="controls">
                                <label class="inline">
                                    <span id="vIPI" data-bind="text: kendo.toString(finan_IPIV(), 'c')"></span>
                                </label>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label"><strong>Dif. ICMS:</strong></label>
                            <div class="controls">
                                <label class="inline">
                                    <span id="vDiffICMS" data-bind="text: kendo.toString(finan_DiffICMSV(), 'c')"></span>
                                </label>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label"><strong>S. T. ICMS:</strong></label>
                            <div class="controls">
                                <label class="inline">
                                    <span id="vTribSubICMS" data-bind="text: kendo.toString(finan_TribSubICMSV(), 'c')"></span>
                                </label>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label"><strong>ISS:</strong></label>
                            <div class="controls">
                                <label class="inline">
                                    <span id="vISS" data-bind="text: kendo.toString(finan_ISSV(), 'c')"></span>
                                </label>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label"><strong>Outros:</strong></label>
                            <div class="controls">
                                <label class="inline">
                                    <span id="vOtherTaxes" data-bind="text: kendo.toString(finan_OtherTaxesV(), 'c')"></span>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div>
            <div id="editProductCommission" class="row-fluid">
                <div class="form-horizontal">
                    <div class="span6">
                        <div class="control-group">
                            <label class="control-label" for="managerTextBox"><strong>Gerente %:</strong></label>
                            <div class="controls">
                                <input id="managerTextBox" data-bind="kendoNumericTextBox: { value: finan_Manager, spinners: false }" class="input-small" />
                                <input id="managerCheckbox" type="checkbox" checked="checked" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="sellerTextBox"><strong>Vendedor %:</strong></label>
                            <div class="controls">
                                <input id="sellerTextBox" data-bind="kendoNumericTextBox: { value: finan_SalesPerson, spinners: false }" class="input-small" />
                                <input id="sellerCheckbox" type="checkbox" checked="checked" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="repTextBox"><strong>Representante %:</strong></label>
                            <div class="controls">
                                <input id="repTextBox" data-bind="kendoNumericTextBox: { value: finan_Rep, spinners: false }" class="input-small" />
                                <input id="repCheckbox" type="checkbox" checked="checked" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="telemarketTextBox"><strong>Telemarket %:</strong></label>
                            <div class="controls">
                                <input id="telemarketTextBox" data-bind="kendoNumericTextBox: { value: finan_Telemarketing, spinners: false }" class="input-small" />
                                <input id="telemarketCheckbox" type="checkbox" checked="checked" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="techTextBox"><strong>Técnico %:</strong></label>
                            <div class="controls">
                                <input id="techTextBox" data-bind="kendoNumericTextBox: { value: finan_Tech, spinners: false }" class="input-small" />
                                <input id="techCheckbox" type="checkbox" checked="checked" />
                            </div>
                        </div>
                    </div>
                    <div class="span6">
                        <div class="control-group">
                            <label class="control-label" for="vManagerTextBox"><strong>Gerente:</strong></label>
                            <div class="controls">
                                <input id="vManagerTextBox" data-bind="kendoNumericTextBox: { value: managerCommV, format: 'n', spinners: false }" class="input-small" />
                                <input id="vManagerCheckbox" type="checkbox" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="vSellerTextBox"><strong>Vendedor:</strong></label>
                            <div class="controls">
                                <input id="vSellerTextBox" data-bind="kendoNumericTextBox: { value: sellerCommV, format: 'n', spinners: false }" class="input-small" />
                                <input id="vSellerCheckbox" type="checkbox" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="vRepTextBox"><strong>Representante:</strong></label>
                            <div class="controls">
                                <input id="vRepTextBox" data-bind="kendoNumericTextBox: { value: repCommV, format: 'n', spinners: false }" class="input-small" />
                                <input id="vRepCheckbox" type="checkbox" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="vTelemarketTextBox"><strong>Telemarketet:</strong></label>
                            <div class="controls">
                                <input id="vTelemarketTextBox" data-bind="kendoNumericTextBox: { value: teleCommV, format: 'n', spinners: false }" class="input-small" />
                                <input id="vTelemarketCheckbox" type="checkbox" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="vTechTextBox"><strong>Técnico:</strong></label>
                            <div class="controls">
                                <input id="vTechTextBox" data-bind="kendoNumericTextBox: { value: techCommV, format: 'n', spinners: false }" class="input-small" />
                                <input id="vTechCheckbox" type="checkbox" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div>
            <div id="editProductTributes" class="row-fluid">
                <div class="form-horizontal">
                    <div class="span4">
                        <div class="control-group">
                            <label class="control-label" for="defaultBarCode"><strong>Código de Barra:</strong></label>
                            <div class="controls">
                                <select id="defaultBarCode">
                                    <option value="">Selecionar</option>
                                    <option value="EAN13">EAN13</option>
                                    <option value="EAN8">EAN8</option>
                                    <option value="Peso">Peso</option>
                                    <option value="Balan">Balan</option>
                                    <option value="Outro">Outro</option>
                                    <option value="GTIN">GTIN</option>
                                </select>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="pisTribSit"><strong>Sit. Trib. PIS:</strong></label>
                            <div class="controls">
                                <select id="pisTribSit">
                                    <option value="01">01 Operação Tributável com Alíquota Básica</option>
                                    <option value="02">02 Operação Tributável com Alíquota Diferenciada</option>
                                    <option value="03">03 Operação Tributável com Alíquota por Unidade de Medida de Produto</option>
                                    <option value="04">04 Operação Tributável Monofásica - Revenda a Alíquota Zero</option>
                                    <option value="05">05 Operação Tributável por Substituição Tributária</option>
                                    <option value="06">06 Operação Tributável a Alíquota Zero</option>
                                    <option value="07">07 Operação Isenta da Contribuição</option>
                                    <option value="08">08 Operação sem Incidência da Contribuição</option>
                                    <option value="09">09 Operação com Suspensão da Contribuição</option>
                                    <option value="49">49 Outras Operações de Saída</option>
                                    <option value="50">50 Operação com Direito a Crédito - Vinculada Exclusivamente a Receita Tributada no Mercado Interno</option>
                                    <option value="51">51 Operação com Direito a Crédito – Vinculada Exclusivamente a Receita Não Tributada no Mercado Interno</option>
                                    <option value="52">52 Operação com Direito a Crédito - Vinculada Exclusivamente a Receita de Exportação</option>
                                    <option value="53">53 Operação com Direito a Crédito - Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno</option>
                                    <option value="54">54 Operação com Direito a Crédito - Vinculada a Receitas Tributadas no Mercado Interno e de Exportação</option>
                                    <option value="55">55 Operação com Direito a Crédito - Vinculada a Receitas Não-Tributadas no Mercado Interno e de Exportação</option>
                                    <option value="56">56 Operação com Direito a Crédito - Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno, e de Exportação</option>
                                    <option value="60">60 Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita Tributada no Mercado Interno</option>
                                    <option value="61">61 Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita Não-Tributada no Mercado Interno</option>
                                    <option value="62">62 Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita de Exportação</option>
                                    <option value="63">63 Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno</option>
                                    <option value="64">64 Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas no Mercado Interno e de Exportação</option>
                                    <option value="65">65 Crédito Presumido - Operação de Aquisição Vinculada a Receitas Não-Tributadas no Mercado Interno e de Exportação</option>
                                    <option value="66">66 Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno, e de Exportação</option>
                                    <option value="67">67 Crédito Presumido - Outras Operações','70 Operação de Aquisição sem Direito a Crédito','71 Operação de Aquisição com Isenção</option>
                                    <option value="70">70 Operação de Aquisição sem Direito a Crédito</option>
                                    <option value="71">71 Operação de Aquisição com Isenção</option>
                                    <option value="72">72 Operação de Aquisição com Suspensão</option>
                                    <option value="73">73 Operação de Aquisição a Alíquota Zero</option>
                                    <option value="74">74 Operação de Aquisição sem Incidência da Contribuição</option>
                                    <option value="75">75 Operação de Aquisição por Substituição Tributária</option>
                                    <option value="98">98 Outras Operações de Entrada</option>
                                    <option value="99">99 Outras Operações'</option>
                                </select>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="cofinsTribSit"><strong>Sit. Trib. COFINS:</strong></label>
                            <div class="controls">
                                <select id="cofinsTribSit">
                                    <option value="01">01 Operação Tributável com Alíquota Básica</option>
                                    <option value="02">02 Operação Tributável com Alíquota Diferenciada</option>
                                    <option value="03">03 Operação Tributável com Alíquota por Unidade de Medida de Produto</option>
                                    <option value="04">04 Operação Tributável Monofásica - Revenda a Alíquota Zero</option>
                                    <option value="05">05 Operação Tributável por Substituição Tributária</option>
                                    <option value="06">06 Operação Tributável a Alíquota Zero</option>
                                    <option value="07">07 Operação Isenta da Contribuição</option>
                                    <option value="08">08 Operação sem Incidência da Contribuição</option>
                                    <option value="09">09 Operação com Suspensão da Contribuição</option>
                                    <option value="49">49 Outras Operações de Saída</option>
                                    <option value="50">50 Operação com Direito a Crédito - Vinculada Exclusivamente a Receita Tributada no Mercado Interno</option>
                                    <option value="51">51 Operação com Direito a Crédito – Vinculada Exclusivamente a Receita Não Tributada no Mercado Interno</option>
                                    <option value="52">52 Operação com Direito a Crédito - Vinculada Exclusivamente a Receita de Exportação</option>
                                    <option value="53">53 Operação com Direito a Crédito - Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno</option>
                                    <option value="54">54 Operação com Direito a Crédito - Vinculada a Receitas Tributadas no Mercado Interno e de Exportação</option>
                                    <option value="55">55 Operação com Direito a Crédito - Vinculada a Receitas Não-Tributadas no Mercado Interno e de Exportação</option>
                                    <option value="56">56 Operação com Direito a Crédito - Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno, e de Exportação</option>
                                    <option value="60">60 Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita Tributada no Mercado Interno</option>
                                    <option value="61">61 Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita Não-Tributada no Mercado Interno</option>
                                    <option value="62">62 Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita de Exportação</option>
                                    <option value="63">63 Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno</option>
                                    <option value="64">64 Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas no Mercado Interno e de Exportação</option>
                                    <option value="65">65 Crédito Presumido - Operação de Aquisição Vinculada a Receitas Não-Tributadas no Mercado Interno e de Exportação</option>
                                    <option value="66">66 Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno, e de Exportação</option>
                                    <option value="67">67 Crédito Presumido - Outras Operações</option>
                                    <option value="70">70 Operação de Aquisição sem Direito a Crédito</option>
                                    <option value="71">71 Operação de Aquisição com Isenção</option>
                                    <option value="72">72 Operação de Aquisição com Suspensão</option>
                                    <option value="73">73 Operação de Aquisição a Alíquota Zero</option>
                                    <option value="74">74 Operação de Aquisição sem Incidência da Contribuição</option>
                                    <option value="75">75 Operação de Aquisição por Substituição Tributária</option>
                                    <option value="98">98 Outras Operações de Entrada</option>
                                    <option value="99">99 Outras Operações</option>
                                </select>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="ipiTribSit"><strong>IPI Sit. Trib.:</strong></label>
                            <div class="controls">
                                <select id="ipiTribSit">
                                    <option value="">Selecionar</option>
                                    <option value="01">01 - Entrada tributada com alíquota zero</option>
                                    <option value="02">02 - Entrada isenta</option>
                                    <option value="03">03 - Entrada não-tributada</option>
                                    <option value="04">04 - Entrada imune</option>
                                    <option value="05">05 - Entrada com suspensão</option>
                                    <option value="49">49 - Outras entradas</option>
                                    <option value="50">50 - Saída tributada</option>
                                    <option value="51">51 - Saída tributada com alíquota zero</option>
                                    <option value="52">52 - Saída isenta</option>
                                    <option value="53">53 - Saída não-tributada</option>
                                    <option value="54">54 - Saída imune</option>
                                    <option value="55">55 - Saída com suspensão</option>
                                    <option value="99">99 - Outras saídas</option>
                                </select>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="tribSitType"><strong>Tipo de Tributação:</strong></label>
                            <div class="controls">
                                <select id="tribSitType">
                                    <option value="">Selecionar</option>
                                    <option value="Normal">Normal</option>
                                    <option value="Substituição Tributária">Substituição Tributária</option>
                                    <option value="Isenção">Isenção</option>
                                    <option value="Não Incidência">Não Incidência</option>
                                </select>
                            </div>
                        </div>
                    </div>
                    <div class="span4">
                        <div class="control-group">
                            <label class="control-label" for="cstTextBox"><strong>CST:</strong></label>
                            <div class="controls">
                                <input id="cstTextBox" data-bind="kendoNumericTextBox: { value: 0, format: '000', max: 999, spinners: false }" class="input-small" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="pisBaseTextBox"><strong>Base Cálc. PIS:</strong></label>
                            <div class="controls">
                                <input id="pisBaseTextBox" data-bind="kendoNumericTextBox: { value: 0, spinners: false }" class="input-small" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="pisTextBox"><strong>PIS:</strong></label>
                            <div class="controls">
                                <input id="pisTextBox" data-bind="kendoNumericTextBox: { value: 0, spinners: false }" class="input-small" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="pisTribSubTextBox"><strong>PIS Sub. Trib.:</strong></label>
                            <div class="controls">
                                <input id="pisTribSubTextBox" data-bind="kendoNumericTextBox: { value: 0, spinners: false }" class="input-small" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="icmsTextBox"><strong>ICMS:</strong></label>
                            <div class="controls">
                                <input id="icmsTextBox" data-bind="kendoNumericTextBox: { value: 0, spinners: false }" class="input-small" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="pisTribSubBaseTextBox"><strong>Base Cálc. PIS Subs.:</strong></label>
                            <div class="controls">
                                <input id="pisTribSubBaseTextBox" data-bind="kendoNumericTextBox: { value: 0, spinners: false }" class="input-small" />
                            </div>
                        </div>
                    </div>
                    <div class="span4">
                        <div class="control-group">
                            <label class="control-label" for="ncmTextBox"><strong>NCM:</strong></label>
                            <div class="controls">
                                <input id="ncmTextBox" data-bind="kendoNumericTextBox: { value: 0, format: '00', max: 99999999, spinners: false }" class="input-small" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="cfopTextBox"><strong>CFOP:</strong></label>
                            <div class="controls">
                                <input id="cfopTextBox" data-bind="kendoNumericTextBox: { value: 0, format: '0000', max: 9999, spinners: false }" class="input-small" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="cofinsTribSubTextBox"><strong>COFINS Sub. Trib.:</strong></label>
                            <div class="controls">
                                <input id="cofinsTribSubTextBox" data-bind="kendoNumericTextBox: { value: 0, spinners: false }" class="input-small" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="cofinsBaseTextBox"><strong>BC COFINS:</strong></label>
                            <div class="controls">
                                <input id="cofinsBaseTextBox" data-bind="kendoNumericTextBox: { value: 0, spinners: false }" class="input-small" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="cofinsTextBox"><strong>COFINS:</strong></label>
                            <div class="controls">
                                <input id="cofinsTextBox" data-bind="kendoNumericTextBox: { value: 0, spinners: false }" class="input-small" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="cofinsTribSubBaseTextBox"><strong>BC COFINS Sub. Trib.:</strong></label>
                            <div class="controls">
                                <input id="cofinsTribSubBaseTextBox" data-bind="kendoNumericTextBox: { value: 0, spinners: false }" class="input-small" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="divSpecialPriceDates">
        <div class="padded"></div>
        <div class="form-horizontal">
            <div class="control-group">
                <label class="control-label" for="preSpecialPrice"><strong>Preço de Oferta:</strong></label>
                <div class="controls">
                    <input id="preSpecialPrice" data-bind="kendoNumericTextBox: { value: finan_Special_Price, format: 'n', spinners: false }" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="defaultBarCode"><strong>Data Incial:</strong></label>
                <div class="controls">
                    <input id="saleStartDate" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="defaultBarCode"><strong>Data Final:</strong></label>
                <div class="controls">
                    <input id="saleEndDate" />
                </div>
            </div>
        </div>
    </div>
</div>