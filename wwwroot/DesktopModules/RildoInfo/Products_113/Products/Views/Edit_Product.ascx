<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Edit_Product.ascx.vb" Inherits="RIW.Modules.Products.Views.EditProduct" %>
<div id="divProductEdit" class="row-fluid">
    <ul class="inline">
        <li>
            <button class="btn btn-small btnReturn" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-chevron-left"></i>&nbsp; Fechar</button>
        </li>
        <li>
            <button id="btnUpdateProduct" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar Produto</button>
        </li>
        <li>    
            <button id="btnCopyProduct" class="btn btn-small btn-success" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-chevron-right"></i>&nbsp; Copiar Produto</button>
        </li>
        <li>
            <button id="btnDeleteProduct" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-ban-circle"></i>&nbsp;Desativar</button>
            <button id="btnRestoreProduct" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-repeat"></i>&nbsp;Restaurar</button>
        </li>
        <li>
            <button id="btnRemoveProduct" class="btn btn-small btn-danger" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
        </li>
    </ul>
    <ul id="productMenu">
        <li id="menu_1" class="hidden"><i class="icon-chevron-left"></i>&nbsp;Fechar</li>
        <li id="menu_2" class="k-state-selected">Básico</li>
        <li id="menu_3">Imagens</li>
        <li id="menu_4">Descrição</li>
        <li id="menu_5">Vídeos</li>
        <li id="menu_6">Produtos Relacionados</li>
        <li id="menu_7">Atributos</li>
        <li id="menu_8">Frete</li>
        <li id="menu_9">SEO</li>
        <li id="menu_11">Financeiro</li>
    </ul>
    <div class="pull-left">
        <div id="productForm" class="form-horizontal">
            <div class="control-group">
                <label class="control-label" for="productType"><strong>Tipo:</strong></label>
                <div class="controls">
                    <label class="radio inline">
                        <input type="radio" value="1" id="productType" name="itemType" data-bind="checked: itemType" />
                        Produto
                    </label>
                    <label class="radio inline">
                        <input type="radio" value="0" id="serviceType" name="itemType" data-bind="checked: itemType" />
                        Serviço
                    </label>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="divAvailCategories"><strong>Categoria:</strong></label>
                <div class="controls">
                    <div id="divAvailCategories">
                        <div class="select2-container select2-container-multi">
                            <ul id="selectedCats" class="select2-choices" data-toggle="tooltip" title="Selecionar categoria(s)">
                            </ul>
                            <div id="tvCategories" style="display: none;"></div>
                        </div>
                        <button id="btnAddCat" class="btn btn-small" title="Adicionar Categoria"><i class="icon-pencil"></i></button>
                    </div>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label required" for="prodNameTextBox"><strong>Nome:</strong></label>
                <div class="controls">
                    <input id="prodNameTextBox" type="text" required="required" class="enterastab" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="prodRefTextBox"><strong>Referência:</strong></label>
                <div class="controls">
                    <input id="prodRefTextBox" type="text" class=" enterastab" />
                </div>
            </div>
            <div id="divBarCode" class="control-group">
                <label class="control-label" for="prodBarCodeTextBox"><strong>Cód de Barra:</strong></label>
                <div class="controls">
                    <input id="prodBarCodeTextBox" type="text" class=" enterastab" />
                </div>
            </div>
            <div id="divUnitTypes" class="control-group">
                <label class="control-label" for="ddlUnitTypes"><strong>Unidade:</strong></label>
                <div class="controls">
                    <input id="ddlUnitTypes" class="input-large" data-bind="kendoDropDownList: { data: unitTypes, dataTextField: 'unitName', dataValueField: 'unitId', widget: unitTypeList, value: parseInt(amplify.store.sessionStorage('defaultUnit')) }" />
                </div>
            </div>
        </div>
    </div>
    <div class="pull-left">
        <div class="form-horizontal">
            <div id="divVendors" class="control-group" data-bind="visible: my.productId > 0">
                <label class="control-label" for="selectVendors"><strong>Fornecedores:</strong></label>
                <div class="controls">
                    <div class="pull-left">
                        <input id="selectVendors" />
                    </div>
                    <div class="pull-left">
                        &nbsp;<button id="btnAddProvider" class="btn btn-small" title="Adicionar Fornecedor"><i class="icon-plus"></i></button>
                    </div>
                    <div class="clearfix"></div>
                </div>
            </div>
            <div id="divBrand" class="control-group">
                <label class="control-label" for="ddlBrands"><strong>Marca:</strong></label>
                <div class="controls" style="white-space: nowrap;">
                    <input id="ddlBrands" />
                    <button id="btnAddBrand" class="btn btn-small" title="Clique aqui para adicionar nova Marca"><i class="icon-plus"></i></button>
                    <button id="btnUpdateBrand" class="btn btn-small" title="Clique aqui para atualizar Marca selecionada" data-bind="visible: selectedBrandId()"><i class="icon-ok"></i></button>
                    <button id="btnRemoveBrand" class="btn btn-small" name="remove" title="Clique aqui para remover Marca selecionada" data-bind="visible: selectedBrandId()"><i class="icon-remove"></i></button>
                </div>
            </div>
            <div id="divModels" class="control-group">
                <label class="control-label" for="ddlModels"><strong>Modêlo:</strong></label>
                <div class="controls" style="white-space: nowrap;">
                    <input id="ddlModels" />
                    <button id="btnAddModel" class="btn btn-small" title="Clique aqui para adicionar novo Modelo"><i class="icon-plus"></i></button>
                    <button id="btnUpdateModel" class="btn btn-small" title="Clique aqui para atualizar Modelo selecionado" data-bind="visible: selectedModelId()"><i class="icon-ok"></i></button>
                    <button id="btnRemoveModel" class="btn btn-small" name="remove" title="Clique aqui para remover Modelo selecionado" data-bind="visible: selectedModelId()"><i class="icon-remove"></i></button>
                </div>
            </div>
            <div id="divReorderPoint" class="control-group">
                <label class="control-label" for="reorderPointTextBox"><strong>Aviso de Estoque:</strong></label>
                <div class="controls">
                    <input id="reorderPointTextBox" class="enterastab input-small" data-bind="kendoNumericTextBox: { value: 0, format: '0', decimals: 0 }" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="showPriceCheckBox"><strong>Mostrar Preço:</strong></label>
                <div class="controls">
                    <input id="showPriceCheckBox" type="checkbox" class="normalCheckBox switch-small" data-on-label="SIM" data-off-label="NÃO" />
                </div>
            </div>
            <div id="divStockLabel" class="control-group">
                <label class="control-label" for="stockLabel"><strong>Estoque Atual:</strong></label>
                <div class="controls">
                    <label class="inline">
                        <span id="stockLabel"></span>
                    </label>
                </div>
            </div>
            <div id="divStockTextBox" class="control-group">
                <label class="control-label" for="stockTextBox"><strong>Estoque:</strong></label>
                <div class="controls">
                    <input id="stockTextBox" class="enterastab" data-bind="kendoNumericTextBox: { value: 0, min: 0, format: 'n' }" />
                </div>
            </div>
            <div class="control-group" data-bind="visible: my.productId == 0">
                <label class="control-label" for="reorderPointTextBox"><strong>ICMS:</strong></label>
                <div class="controls">
                    <input id="txtIcms" class="enterastab input-small" data-bind="kendoNumericTextBox: { value: 18, format: '0', decimals: 0 }" />
                </div>
            </div>
            <div class="control-group" data-bind="visible: my.productId == 0">
                <label class="control-label" for="reorderPointTextBox" style="padding-top: 4px;"><strong>CST:</strong></label>
                <div class="controls">
                    <input id="txtCst" class="enterastab input-mini" type="text" value="060" />
                </div>
            </div>
        </div>
    </div>
    <div class="clearfix"></div>
    <div id="check3boxes" class="row-fluid">
        <div class="span2 offset1">
            <label class="checkbox">
                <input id="featuredCheckbox" type="checkbox" />
                Destaque
            </label>
        </div>
        <div class="span2">
            <label class="checkbox">
                <input id="dealerOnlyCheckbox" type="checkbox" />
                Revenda
            </label>
        </div>
        <div class="span2">
            <label class="checkbox">
                <input id="scaleCheckbox" type="checkbox" />
                Balança
            </label>
        </div>
        <div class="span2">
            <label class="checkbox">
                <input id="hiddenCheckbox" type="checkbox" />
                Escondido
            </label>
        </div>
        <div class="span2">
            <label class="checkbox">
                <input id="archivedCheckbox" type="checkbox" />
                Descontinuado
            </label>
        </div>
    </div>
    <div class="clearfix padded"></div>
    <div class="form-horizontal">
        <div class="control-group">
            <label class="control-label">
                <strong>Introdução:</strong><br />
                <a href="http://pt.wikipedia.org/wiki/Markdown" target="_blank" title="Saiba como usar Markdown" class="font-size-small">Markdown Suporte</a><br />
                <button id="toggleSummaryPreview" class="btn btn-small" value="preview" data-toggle="tooltip" title="Clique aqui para alternar em visualizar e editar o campo de mensagem">
                    <i class="fa fa-eye"></i>&nbsp; Visualizar
                </button>
            </label>
            <div class="controls">
                <textarea id="summaryTextArea" class="markdown-editor"></textarea><br />
                <label id='counter'><span>500</span> &nbsp;caracteres disponíveis para a introdução.</label>
            </div>
        </div>
    </div>
</div>