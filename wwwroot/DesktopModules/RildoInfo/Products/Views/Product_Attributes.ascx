<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Product_Attributes.ascx.vb" Inherits="RIW.Modules.Products.Views.ProductAttributes" %>
<div id="editProductAttributes" class="row-fluid">
    <div class="span12">
        <ul class="inline">
            <li>
                <button class="btn btn-small btnReturn" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-chevron-left"></i>&nbsp; Fechar</button>
            </li>
            <li>
                <button id="btnUpdateAttributes" class="btn btn-small btn-inverse"><i class="fa fa-check"></i>&nbsp; Atualizar</button>
            </li>
        </ul>
        <ul id="productMenu">
            <li id="menu_1" class="hidden"><i class="icon-chevron-left"></i>&nbsp;Fechar</li>
            <li id="menu_2">Básico</li>
            <li id="menu_3">Imagens</li>
            <li id="menu_4">Descrição</li>
            <li id="menu_5">Vídeos</li>
            <li id="menu_6">Produtos Relacionados</li>
            <li id="menu_7" class="k-state-selected">Atributos</li>
            <li id="menu_8">Frete</li>
            <li id="menu_9">SEO</li>
            <li id="menu_11">Financeiro</li>
        </ul>
        <div class="form-horizontal">
            <div class="pull-left">
                <div class="row-fluid">
                    <div class="form-horizontal">
                        <div class="control-group">
                            <label class="control-label required" for="attributeTextBox">
                                <strong>Atributo:</strong>
                                <i class="icon-info-sign" title="Atributo" data-content="(ex. Tamanho)"></i>
                            </label>
                            <div class="controls">
                                <label class="inline">
                                    <input id="attributeTextBox" type="text" />
                                    <button id="btnAddAttr" class="btn btn-inverse btn-small">Adicionar</button>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="attributesGrid"></div>
            </div>
            <div class="pull-right">
                <div class="row-fluid">
                    <div  id="divAttrOptions">
                        <div class="control-group">
                            <label class="control-label" for="chosenAttribute"><strong>Atributo:</strong></label>
                            <div class="controls">
                                <label class="inline">
                                    <span id="chosenAttribute" class="font-size-large"></span>
                                </label>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label required" for="attributeOptionTextBox">
                                <strong>Opções:</strong>
                                <i class="icon-info-sign" title="Opções" data-content="(ex. Pequeno, Médio, Grande)"></i>
                            </label>
                            <div class="controls">
                                <label class="inline">
                                    <input id="attributeOptionTextBox" type="text" />
                                    <button id="btnAddAttrOption" class="btn btn-inverse btn-small">Adicionar</button>
                                </label>
                            </div>
                        </div>
                        <div id="attributeOptionsGrid"></div>
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
        </div>
    </div>
</div>