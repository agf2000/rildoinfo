<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Estimate.ascx.vb" Inherits="RIW.Modules.Store.Views.Estimate" %>
<div id="divEstimate" class="row-fluid">
    <div class="span12">
        <a id="configLink" class="text-right"></a>
        <div class="divButtons">
            <div id="divCheckExpand">
                <label class="checkbox">
                    <input type="checkbox" />
                    <span>Expandir orçamento e mante-lo expandido.</span>
                </label>
            </div>
            <div id="divButtons">
                <button id="btnCancelEstimate" class="btn btn-small"><span class="fa fa-times"></span>&nbsp; Cancelar Orçamento</button>
                &nbsp;
                <button id="btnSaveEstimate" class="btn btn-small btn-info" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><span class="fa fa-sign-in"></span>&nbsp; Login ou Criar Conta</button>
            </div>
        </div>
        <div class="clearfix"></div>
        <table id="estimateItems" class="table table-striped table-bordered">
            <thead>
                <tr>
                    <th>Código
                    </th>
                    <th>Produto
                    </th>
                    <th>Quantidade
                    </th>
                    <th>Valor
                    </th>
                    <th></th>
                    <th>
                    </th>
                </tr>
            </thead>
            <tbody data-bind="template: { name: 'eachProductTmpl', foreach: selectedProducts }">
            </tbody>
            <tfoot class="tFooter">
                <tr>
                    <td colspan="4">
                        <div class="text-right">
                            <span style="font-weight: bold;">Valor Total: </span>
                            <span data-bind="html: incomplete() ? ' ******** ' : kendo.format('{0:c}', extendedPrice()), attr: { class: incomplete() ? 'noStock' : '', title: incomplete() ? 'Valor Total não disponível' : '', 'data-content': incomplete() ? 'O preço de um ou mais itens na lista de seu orçamento não está sendo exibido. O Valor Total estará disponível após a requisição do orçamento.' : '' }"></span>
                        </div>
                    </td>
                </tr>
            </tfoot>
        </table>
        <script type="text/html" id="eachProductTmpl">
            <tr>
                <td><span data-bind="html: productCode()"></span>
                </td>
                <td><span data-bind="text: productName(), attr: { class: qTyStockSet() <= 0 || itemType() === '1' ? '' : 'noStock', title: qTyStockSet() <= 0 || itemType() === '1' ? '' : 'Esgotado', 'data-content': qTyStockSet() <= 0 || itemType() === '1' ? '' : 'Neste momento, este produto não se encontra em estoque.' }"></span>
                </td>
                <td>
                    <input data-bind="kendoNumericTextBox: { value: qTy, min: 1, format: productUnit() === 1 ? '' : 'n' }" class="input-small" />
                </td>
                <td><span data-bind="text: (showPrice() || authorized > 1) ? kendo.toString(totalValue(), 'n') : '*********'"></span>
                </td>
                <td>
                    <button class="btn btn-small" title="Atualizar a quantidade" data-bind="click: updateItem"><span class="fa fa-check"></span>&nbsp; Atualizar</button>              
                </td>
                <td>
                    <button class="btn btn-small btn-link" data-bind="click: removeItem"><span class="fa fa-times" title="Remover este item"></span></button>
                </td>
            </tr>
        </script>

        <div id="dialog-message" title="Orçamento salvo com sucesso">
            <p>
                Caro(a) <span data-bind="text: displayName()"></span>, 
                Nossos orçamentos são administrados em tempo real. Em instantes estaremos analizando seu orçamento e será comunicado assim que o orçamento for atualizado.
            </p>
            <p>
                No momento, enviamos para sua caixa postal o link para acompanhar seu orçamento a qualquer hora em tempo real.<br />
                Uma vez que logado, acesse todos os seus orçamentos usando o link "Meus Orçamentos" localizado no menu do website no topo das páginas.<br />
                Por favor clique no "Ok" abaixo se deseja visualizar o orçamento que acabou de fazer.
            </p>
            <label class="checkbox">
                <input type="checkbox" />
                Não mostrar mais esta mensagem.
            </label>
        </div>

        <div id="divMini_Estimate">
            <span id="cartItems" data-bind="html: selectedProducts().length > 0 ? selectedProducts().length > 1 ? 'Há <strong>' + selectedProducts().length + '</strong> itens no seu carrinho de compras.' : 'Há <strong>' + selectedProducts().length + '</strong> item no seu carrinho de compras.' : 'Seu carrinho de compras está vazio.'"></span>
            <br />
            <img alt="Carrinho" src="/desktopmodules/rildoinfo/store/content/images/shopppingcart.png" /><br />
            <button id="btnExpandCart" class="btn" data-bind="attr: { 'disabled': selectedProducts().length > 0 ? false : true }"><span class="fa fa-chevron-down"></span>&nbsp; Visualizar Carrinho</button>
        </div>
    </div>
</div>