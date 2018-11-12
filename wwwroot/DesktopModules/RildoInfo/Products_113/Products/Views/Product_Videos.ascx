<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Product_Videos.ascx.vb" Inherits="RIW.Modules.Products.Views.ProductVideos" %>
<div id="editProductVideos" class="row-fluid">
    <div class="span12">
        <ul class="inline">
            <li>
                <button class="btn btn-small btnReturn"><i class="fa fa-chevron-left"></i>&nbsp; Fechar</button>
            </li>
            <li>
                <button id="btnSaveVideo" class="btn btn-small btn-inverse"><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
            </li>
        </ul>
        <ul id="productMenu">
            <li id="menu_1" class="hidden"><i class="icon-chevron-left"></i>&nbsp;Fechar</li>
            <li id="menu_2">Básico</li>
            <li id="menu_3">Imagens</li>
            <li id="menu_4">Descrição</li>
            <li id="menu_5" class="k-state-selected">Vídeos</li>
            <li id="menu_6">Produtos Relacionados</li>
            <li id="menu_7">Atributos</li>
            <li id="menu_8">Frete</li>
            <li id="menu_9">SEO</li>
            <li id="menu_11">Financeiro</li>
        </ul>
        <div class="row-fluid">
            <div class="form-horizontal">
                <div class="control-group">
                    <label class="control-label required"><strong>Nome:</strong></label>
                    <div class="controls">
                        <input id="videoTitleTextBox" type="text" class="enterastab span8" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label required"><strong>Link:</strong></label>
                    <div class="controls">
                        <input id="videoURLTextBox" type="text" class="enterastab span8" placeholder="http://youtu.be/############, http://www.youtube.com/watch?############" />
                    </div>
                </div>
            </div>
        </div>
        <ul id="productVideos" data-bind="template: { name: 'tmplProductVideos', foreach: prodVideos }"></ul>
        <script type="text/html" id="tmplProductVideos">
            <li data-bind="attr: { id: VideoId() }" class="pagination-centered">
                <h5 data-bind="text: Alt()"></h5>
                <iframe width="560" height="315" class="yt" allowfullscreen data-bind="attr: { src: 'http://www.youtube.com/embed/' + my.Right(Src(), 11), frameborder: '0' }"></iframe>
                <div>
                    <button class="btn btn-small btn-danger" data-bind="attr: { id: VideoId(), title: 'Excluir ' + Alt() }" onclick="my.removeVideo(this.id); return false;"><i class="fa fa-times"></i>&nbsp; Excluir</button>
                </div>
            </li>
        </script>
        <div class="clearfix"></div>
    </div>
</div>