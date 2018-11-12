<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Product_Shipping.ascx.vb" Inherits="RIW.Modules.Products.Views.ProductShipping" %>
<div id="editProductShipping" class="row-fluid">
    <div class="span12">
        <ul class="inline">
            <li>
                <button class="btn btn-small btnReturn"><i class="fa fa-chevron-left"></i>&nbsp; Fechar</button>
            </li>
            <li>
                <button id="btnUpdateShipping" class="btn btn-small btn-inverse"><i class="fa fa-check"></i>&nbsp; Atualizar</button>
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
            <li id="menu_8" class="k-state-selected">Frete</li>
            <li id="menu_9">SEO</li>
            <li id="menu_11">Financeiro</li>
        </ul>
        <div class="row-fluid">
            <div class="form-horizontal">
                <div class="pull-left">
                    <div class="control-group">
                        <label class="control-label"><strong>Peso:</strong></label>
                        <div class="controls">
                            <input id="weightTextBox" data-bind="kendoNumericTextBox: { value: 0, spinners: false, format: '' }" class="enterastab input-small" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label"><strong>Comprimento:</strong></label>
                        <div class="controls">
                            <input id="lengthTextBox" data-bind="kendoNumericTextBox: { value: 0, spinners: false }" class="enterastab input-small" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label"><strong>Altura:</strong></label>
                        <div class="controls">
                            <input id="heightTextBox" data-bind="kendoNumericTextBox: { value: 0, spinners: false }" class="enterastab input-small" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label"><strong>Largura:</strong></label>
                        <div class="controls">
                            <input id="widthTextBox" data-bind="kendoNumericTextBox: { value: 0, spinners: false }" class="enterastab input-small" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label"><strong>Diâmetro:</strong></label>
                        <div class="controls">
                            <input id="diameterTextBox" data-bind="kendoNumericTextBox: { value: 0, spinners: false }" class="enterastab input-small" />
                        </div>
                    </div>
                </div>
                <div class="pull-left">
                    <div class="control-group">
                        <label class="control-label"><strong>CEP de Origem:</strong></label>
                        <div class="controls">
                            <input id="zipOriginTextBox" type="text" class="input-large enterastab" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label"><strong>Cidade de Origem:</strong></label>
                        <div class="controls">
                            <input id="cityOriginTextBox" type="text" class="input-large enterastab" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>