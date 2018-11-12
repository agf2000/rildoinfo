<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Product_SEO.ascx.vb" Inherits="RIW.Modules.Products.Views.ProductSeo" %>
<div id="editProductSEO" class="row-fluid">
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
            <li id="menu_4">Descrição</li>
            <li id="menu_5">Vídeos</li>
            <li id="menu_6">Produtos Relacionados</li>
            <li id="menu_7">Atributos</li>
            <li id="menu_8">Frete</li>
            <li id="menu_9" class="k-state-selected">SEO</li>
            <li id="menu_11">Financeiro</li>
        </ul>
        <div class="row-fluid">
            <div class="form-horizontal">
                <div class="control-group">
                    <label class="control-label">
                        <strong>Nome:</strong>
                        <i class="icon-info-sign" title="Nome" data-content="Nome do item que aparecerá no endereço da página"></i>
                    </label>
                    <div class="controls">
                        <input id="seoNameTextBox" type="text" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        <strong>Título da Página:</strong>
                        <i class="icon-info-sign" title="Título da Página" data-content="Nome do item que aparecerá como título da página"></i>
                    </label>
                    <div class="controls">
                        <input id="pageTitleTextBox" type="text" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        <strong>Descrição:</strong>
                        <i class="icon-info-sign" title="Descrição" data-content="Sumário a respeito do item lido por buscadores como o google"></i>
                    </label>
                    <div class="controls">
                        <textarea id="seoSummaryTextArea"></textarea>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        <strong>Palavras Chave:</strong>
                        <i class="icon-info-sign" title="Palavras Chave" data-content="Palavras chaves associadas ao item"></i></label>
                    <div class="controls">
                        <textarea id="keywordsTextarea"></textarea>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>