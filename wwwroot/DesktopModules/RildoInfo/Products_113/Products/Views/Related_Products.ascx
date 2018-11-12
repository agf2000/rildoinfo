<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Related_Products.ascx.vb" Inherits="RIW.Modules.Products.Views.RelatedProducts" %>
<div id="editProductRelated" class="row-fluid">
    <div class="span12">
        <ul class="inline">
            <li>
                <button class="btn btn-small btnReturn"><i class="fa fa-chevron-left"></i>&nbsp; Fechar</button>
            </li>
            <li>
                <button id="btnAddRelatedProduct" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Adicionar</button>
            </li>
        </ul>
        <ul id="productMenu">
            <li id="menu_1" class="hidden"><i class="fa fa-chevron-left"></i>&nbsp; Fechar</li>
            <li id="menu_2">Básico</li>
            <li id="menu_3">Imagens</li>
            <li id="menu_4">Descrição</li>
            <li id="menu_5">Vídeos</li>
            <li id="menu_6" class="k-state-selected">Produtos Relacionados</li>
            <li id="menu_7">Atributos</li>
            <li id="menu_8">Frete</li>
            <li id="menu_9">SEO</li>
            <li id="menu_11">Financeiro</li>
        </ul>
        <div class="form-horizontal">
            <div class="control-group">
                <label class="control-label" for="productSearch"><strong>Produto:</strong></label>
                <div class="controls">
                    <input id="productSearch" class="input-xxlarge" placeholder="Insira Nome, Referência ou Código." />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label"><strong>Qde.:</strong></label>
                <div class="controls">
                    <input data-bind="kendoNumericTextBox: { value: productQty, placeholder: 'Qde', format: '', spinners: false }" class="input-small" />
                </div>
            </div>
            <div class="control-group hidden">
                <label class="control-label"><strong>Qde. Max:</strong></label>
                <div class="controls">
                    <input data-bind="kendoNumericTextBox: { value: maxQty, placeholder: 'Qde', format: '', spinners: false }" class="input-small" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="mustIncludeCheckbox"><strong>Promocional:</strong></label>
                <div class="controls">
                    <label class="checkbox">
                        <input id="mustIncludeCheckbox" type="checkbox" />
                        Item apenas recomendado.
                    </label>
                </div>
            </div>
        </div>
        <div id="divTotalAmount">
            <div class="pull-left">
                <%--<div data-bind="html: '<span>Número total de itens: </span>' + selectedProducts().length"></div>--%>
            </div>
            <div class="pull-right">
                <div data-bind="html: '<span>Valor Total: </span>' + kendo.format('{0:c}', total())"></div>
            </div>
            <div class="clearfix"></div>
        </div>
        <div id="relatedProductsGrid">
        </div>
        <script id="availProductsHeaderTemplate" type="text/x-kendo-tmpl">
            <h5>Itens Selecionados</h5>
            <div class="pull-right">
                <button id="btnRemoveSelectedItems" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Remover Item(ns)</button>
            </div>
            <div class="clearfix"></div>
        </script>
    </div>
</div>