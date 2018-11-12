<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Manage.ascx.vb" Inherits="Manage" %>
<div id="editCategories" class="container-fluid">
    <div class="row-fluid">
        <div class="span4">
            <div id="treeViewCategories"></div>
        </div>
        <div class="span8">
            <ul class="inline">
                <li>
                    <button id="btnUpdateCategory" class="btn btn-inverse" data-bind="text: setButton, enable: categoryName().length"></button>
                </li>
                <li>
                    <button id="btnCancel" class="btn">Cancelar</button>
                </li>
                <li>
                    <button id="btnRemove" class="btn">Excluir</button>
                </li>
            </ul>
            <ul id="editCategoryMenu">
                <li id="menu_1" class="k-state-selected">Básico</li>
                <li id="menu_2">SEO</li>
                <li id="menu_3">Extra</li>
                <li id="menu_4">Produtos</li>
                <li id="menu_5">Permissões</li>
            </ul>
            <div id="basicEdit" class="form-horizontal">
                <div class="control-group">
                    <label class="control-label" for="nameTextBox"><strong>Nome:</strong> <span class="dnnFormRequired"></span></label>
                    <div class="controls">
                        <input id="nameTextBox" type="text" class="span8" data-bind="value: categoryName, valueUpdate: 'afterkeydown'" required="required" oninvalid="this.setCustomValidity('opção obrigatória')" validationmessage=" " />
                        <span data-bind="text: 'ID: ' + categoryId()"></span>
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
                        <input id="orderTextBox" type="text" data-bind="value: listOrder" />
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
                        <div class="make-switch" data-on-label="SIM" data-off-label="NÃO" data-bind="bootstrapSwitchOn: archived">
                            <input id="archivedCheckBox" type="checkbox" class="normalCheckBox" />
                        </div>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="hiddenCategory"><strong>Escondido:</strong></label>
                    <div class="controls">
                        <div class="make-switch" data-on-label="SIM" data-off-label="NÃO" data-bind="bootstrapSwitchOn: hidden">
                            <input id="hiddenCheckBox" type="checkbox" class="normalCheckBox" />
                        </div>
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
        </div>
    </div>
</div>
<script id="delete-confirmation" type="text/x-kendo-template">
    <p class="delete-message">Tem Certeza?</p>

    <button class="delete-confirm k-button">Sim</button>
    &nbsp;
    <a class="delete-cancel">Não</a>
</script>

