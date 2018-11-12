<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Product_Detail.ascx.vb" Inherits="RIW.Modules.Store.Product_Detail" %>
<div id="productDetail">
    <a id="a_img" class="aspNetDisabled">
        <img id="imgProduct" src="/DesktopModules/rildoinfo/webapi/content/images/No-Image.jpg?w=100&amp;h=100" style="cursor: url(/desktopmodules/rildoinfo/ristoredataservices/content/images/zoomin.cur), pointer;" /></a>
    <label id="productTitle"></label>
    <div>
        #<label id="label_Code"></label>
    </div>
    <br />
    <div>
        <strong>Tipo de Unidade: </strong>
        <label id="unitTypeTitle"></label>
    </div>
    <div>
        <strong>Categorias: </strong>
        <span id="cat_links"></span>
    </div>
    <br />
    <div id="productIntro"></div>
    <div class="clearfix"></div>
    <span id="productDesc"></span>
</div>
