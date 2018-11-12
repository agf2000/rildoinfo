<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Manage_Images.ascx.vb" Inherits="RIW.Modules.Products.Views.ManageImages" %>
<div id="divProductImages" class="row-fluid">
    <div class="span12">
        <ul class="inline">
            <li>
                <button class="btn btn-small btnReturn"><i class="fa fa-chevron-left"></i>&nbsp; Fechar</button>
            </li>
            <li>
                <input name="productImgs" id="productImgs" type="file" />
            </li>
        </ul>
        <ul id="productMenu">
            <li id="menu_1" class="hidden"><i class="icon-chevron-left"></i>&nbsp;Fechar</li>
            <li id="menu_2">Básico</li>
            <li id="menu_3" class="k-state-selected">Imagens</li>
            <li id="menu_4">Descrição</li>
            <li id="menu_5">Vídeos</li>
            <li id="menu_6">Produtos Relacionados</li>
            <li id="menu_7">Atributos</li>
            <li id="menu_8">Frete</li>
            <li id="menu_9">SEO</li>
            <li id="menu_11">Financeiro</li>
        </ul>
        <ul id="productImages" class="inline padded pagination-centered" data-bind="template: { name: 'tmplProductImages', foreach: prodImages }">
        </ul>
        <script type="text/html" id="tmplProductImages">
            <li class="padded" data-bind="attr: { id: ProductImageId() }">
                <div class="thumb">
                    <span></span>
                    <a data-bind="attr: { id: 'a_img_' + ProductImageId(), title: FileName(), href: '/databaseimages/' + ProductImageId() + '.' + Extension() + '?maxwidth=800&maxheight=600' }" style="cursor: url(/desktopmodules/rildoinfo/webapi/content/images/zoomin.cur), pointer;">
                        <img data-bind="attr: { src: '/databaseimages/' + ProductImageId() + '.' + Extension() + '?maxwidth=130&maxheight=130', alt: FileName() }" />
                    </a>
                </div>
                <div>
                    <button class="k-button" data-bind="attr: { id: ProductImageId(), title: 'Excluir ' + FileName() }" onclick="my.removeImage(this.id); return false;">&times;&nbsp;Excluir</button>
                </div>
            </li>
        </script>
    </div>
</div>