<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Product_Description.ascx.vb" Inherits="RIW.Modules.Products.Views.ProductDescription" %>
<div id="editProductDesc" class="row-fluid">
    <div class="span12">
        <ul class="inline">
            <li>
                <button class="btn btn-small btnReturn"><i class="fa fa-chevron-left"></i>&nbsp; Fechar</button>
            </li>
            <li>
                <button id="btnUpdateDescription" class="btn btn-small btn-inverse"><i class="fa fa-check"></i>&nbsp; Atualizar</button>
            </li>
        </ul>
        <ul id="productMenu">
            <li id="menu_1" class="hidden"><i class="icon-chevron-left"></i>&nbsp;Fechar</li>
            <li id="menu_2">Básico</li>
            <li id="menu_3">Imagens</li>
            <li id="menu_4" class="k-state-selected">Descrição</li>
            <li id="menu_5">Vídeos</li>
            <li id="menu_6">Produtos Relacionados</li>
            <li id="menu_7">Atributos</li>
            <li id="menu_8">Frete</li>
            <li id="menu_9">SEO</li>
            <li id="menu_11">Financeiro</li>
        </ul>
        <div class="row-fluid" style="display: none;">
            <div class="form-horizontal">
                <div class="control-group">
                    <label class="control-label"><strong>Editor:</strong></label>
                    <div class="controls">
                        <label class="radio inline">
                            <input type="radio" value="1" name="editors" checked="checked" />
                            Simples
                        </label>
                        <label class="radio inline">
                            <input type="radio" value="0" name="editors" />
                            Avançado
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="row-fluid">
            <div id="divProductDescriptionTextarea">
                <textarea id="productDescriptionTextarea" rows="10" cols="30" class="historyTextArea" style="width: 99%; height: 300px"></textarea>
            </div>
            <div id="divProductDescriptionEditor">
                <div id="productDescriptionEditor" class="markdown-input historyTextArea"></div>
            </div>
        </div>
    </div>
</div>