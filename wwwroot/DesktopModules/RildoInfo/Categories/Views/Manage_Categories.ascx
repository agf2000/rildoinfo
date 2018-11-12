﻿<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Manage_Categories.ascx.vb" Inherits="RIW.Modules.Categories.Views.ManageCategories" %>
<div id="editCategories" class="container-fluid">
    <div class="row-fluid">
        <div class="span4">
            <div id="treeViewCategories"></div>
        </div>
        <div class="span8">
            <ul id="editCategoryMenu">
                <li id="menu_1" title="Retornar" data-bind="visible: my.returnUrl"><i class="icon-chevron-left"></i>&nbsp;Retornar</li>
                <li id="menu_2" class="k-state-selected" title="Dados Básicos">Básico</li>
                <li id="menu_3" title="Search Engine Optimization">SEO</li>
                <li id="menu_4" title="Dados Opcionais">Extra</li>
                <li id="menu_5" title="Produtos da Categoria">Produtos</li>
                <li id="menu_6" title="Configurar Permissões">Permissões</li>
            </ul>
            <div id="basicEdit" class="form-horizontal">
                <div class="control-group">
                    <label class="control-label required" for="nameTextBox"><strong>Nome:</strong></label>
                    <div class="controls">
                        <input id="nameTextBox" type="text" data-bind="value: categoryName, valueUpdate: 'afterkeydown'" required="required" oninvalid="this.setCustomValidity('opção obrigatória')" />
                        <span data-bind="text: 'ID: ' + categoryId(), visible: categoryId() > 0"></span>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="descTextArea"><strong>Descrição:</strong></label>
                    <div class="controls">
                        <textarea id="descTextArea" class="span8" data-bind="value: categoryDesc"></textarea>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="orderTextBox"><strong>Ordem:</strong></label>
                    <div class="controls">
                        <input id="orderTextBox" data-bind="value: listOrder" class="input-mini" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label"><strong>Categoria:</strong></label>
                    <div class="controls">
                        <div id="availCategoriesButton">
                            <div style="border: none;" id="mainCategories"></div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="seoEdit" class="form-horizontal">
                <div class="control-group">
                    <label class="control-label"><strong>(SEO) Nome:</strong></label>
                    <div class="controls">
                        <input id="seoNameTextBox" type="text" class="span8" data-bind="value: seoName" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label"><strong>Título da Página:</strong></label>
                    <div class="controls">
                        <input id="titlePageTextBox" type="text" class="span8" data-bind="value: seoPageTitle" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label"><strong>(Metatag) Descrição:</strong></label>
                    <div class="controls">
                        <textarea id="metaDescTextArea" class="span8" data-bind="text: metaDesc"></textarea>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="keywordsTextBox"><strong>(Metatag) Palavras Chave:</strong></label>
                    <div class="controls">
                        <textarea id="keywordsTextArea" class="span8" data-bind="text: metaKeywords"></textarea>
                    </div>
                </div>
            </div>
            <div id="extraEdit" class="form-horizontal">
                <div class="control-group">
                    <div class="padded"></div>
                    <label class="control-label" for="archivedCheckBox"><strong>Arquivado:</strong></label>
                    <div class="controls">
                        <input id="archivedCheckBox" type="checkbox" class="normalCheckBox switch-small" data-on-label="SIM" data-off-label="NÃO" data-bind="attr: { 'checked': archived }" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="hiddenCategory"><strong>Escondido:</strong></label>
                    <div class="controls">
                        <input id="hiddenCheckBox" type="checkbox" class="normalCheckBox switch-small" data-on-label="SIM" data-off-label="NÃO" data-bind="attr: { 'checked': hidden }" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="files"><strong>Imagem:</strong></label>
                    <div class="controls">
                        <div class="hidden">
                            <input name="files" id="files" type="file" />
                        </div>
                        <button id="btnUpload" class="dnnSecondaryAction">Anexar Imagem</button>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="editorTextArea"><strong>Mensagem:</strong></label>
                    <div class="controls">
                        <textarea id="editorTextArea" class="span6" data-bind="text: message"></textarea>
                    </div>
                </div>
            </div>
            <div id="productsEdit" class="form-horizontal">
                <div class="padded"></div>
                <ul class="inline">
                    <li>
                        <button id="btnRemoveProductsCategory" class="btn">Remover Produtos</button>
                    </li>
                </ul>
                <div class="control-group">
                    <input id="productSearch" class="span6" />
                    &nbsp;
                    <button id="btnAdddProductCategory" class="btn btn-inverse">Adicionar Produto</button>
                </div>
                <div id="categoryProductsGrid"></div>
            </div>
            <div id="permissionEdit" class="form-horizontal">
                <div class="padded"></div>
                <div class="control-group">
                    <div id="groupsGrid"></div>
                </div>
            </div>
            <ul class="inline">
                <li>
                    <button id="btnUpdateCategory" class="btn btn-small btn-info" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..." data-bind="html: setButton, enable: categoryName().length"></button>
                </li>
                <li>
                    <button id="btnCancel" class="btn btn-small"><i class="icon-chevron-left"></i>&nbsp;Cancelar</button>
                </li>
                <li>
                    <button id="btnRemove" class="btn btn-small btn-danger"><i class="fa fa-times"></i>&nbsp; Excluir</button>
                </li>
            </ul>
        </div>
    </div>
</div>
