<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Manage_Products.ascx.vb" Inherits="RIW.Modules.Products.Views.ManageProducts" %>
<div id="divProductsManager" class="row-fluid">
    <div class="span12">
        <div class="accordion">
            <div class="accordion-group">
                <div class="accordion-heading">
                    <a class="accordion-toggle" data-toggle="collapse" href="#collapseFilter">
                        <h4>Filtro</h4>
                    </a>
                </div>
                <div id="collapseFilter" class="accordion-body collapse">
                    <div class="accordion-inner">
                        <ul class="inline">
                            <li style="vertical-align: top;">
                                <strong>Desativados:</strong>
                                <i class="icon icon-exclamation-sign" title="Produtos e Serviços Desativados" data-content="Clique nesta opções para ver ou ocultar itens desativados da lista abaixo."></i>
                                <button id="btnIsDeleted" title="Clique para mostrar ou esconder produtos desativados">Mostrar Desativados (<span id="deletedCount" data-bind="text: deleteds"></span>)</button>
                                <input id="chkDeleted" type="checkbox" class="normalCheckBox hidden" />                            
                                <div class="padded"></div>
                                <strong>Fornecedores:</strong>
                                <i class="icon icon-exclamation-sign" title="Filtre por Vendedores" data-content="Escolha o vendedor e clique na lupa para aplicar o filtro."></i>
                                <input id="selectVendors" />
                            </li>
                            <li>
                                <strong>Filtar por Datas:</strong>
                                <i class="icon icon-exclamation-sign" title="Filtrar por Datas" data-content="Escolha entre data de inserção ou alteração. É necessário inserir a data incial e a data final. Clique em [Aplicar Filtar] patra ativar o filtro."></i>
                                <select id="ddlDates" data-bind="kendoDropDownList: {}" class="input-medium">
                                    <option value="ALL" selected>Selecionar</option>
                                    <option value="CreatedOnDate">Inserido Em</option>
                                    <option value="ModifiedOnDate">Alterado Em</option>
                                </select>
                                <div class="padded"></div>
                                <strong>Datas Inicial:</strong>
                                <i class="icon icon-exclamation-sign" title="Datas de Inserção" data-content="Filtre por data de inserção. Insira a data incial e final, e clique na lupa para aplicar o filtro."></i>
                                <input id="kdpStartDate" placeholder="Data Inicial" title="Data da última alteração" />
                                <div class="padded"></div>
                                <strong>Datas Final:</strong>
                                <i class="icon icon-exclamation-sign" title="Datas de Inserção" data-content="Filtre por data de inserção. Insira a data incial e final, e clique na lupa para aplicar o filtro."></i>
                                <input id="kdpEndDate" placeholder="Data Final" title="Data da última alteração" />
                            </li>
                            <li>
                                <div class="pull-left" style="padding: 5px 5px 0 0;"><strong>Categorias: </strong>
                                    <i class="icon icon-exclamation-sign" title="Filtrar por Categorias" data-content="Escolha a categoria desejada. Clique em [Aplicar Filtar] patra ativar o filtro."></i>
                                </div>
                                <div id="divAvailCategories" class="pull-left">
                                    <div class="select2-container select2-container-multi">
                                        <ul id="selectedCats" class="select2-choices" data-toggle="tooltip" title="Selecionar categoria(s)">
                                        </ul>
                                        <div id="tvCategories" style="display: none;"></div>
                                    </div>
                                </div>               
                                <div class="clearfix"></div>                 
                                <div class="padded"></div>
                                <strong>Procurar Por:</strong>
                                <i class="icon icon-exclamation-sign" title="Campo para busca" data-content="Escolha o campo que deseja executar seu filtro."></i>
                                <select id="kddlFields" data-bind="kendoDropDownList: {}" class="input-medium">
                                    <option value="ProductId">Código</option>
                                    <option value="ProductName" selected>Nome</option>
                                    <option value="ProductRef">Referência</option>
                                    <option value="Barcode">Cod. Barras</option>
                                </select>
                                <div class="padded"></div>
                                <strong>Palavra Chave:</strong>
                                <i class="icon icon-exclamation-sign" title="Palavra Chave" data-content="Filtre por palavra chave. As opções são: Nome próprio, nome da empresa, telefone, email, cpf ou cnpj. Insira a palavra e clique na lupa para aplicar o filtro."></i>
                                <div class="input-append">
                                    <input id="searchTerm" class="input-small" type="text" placeholder="Busca" title="Digite o termo e clique no botão ao lado" data-bind="value: filter" />
                                    <button id="btnSearch" class="btn" title="Clique aqui para fazer sua busca"><i class="icon-search"></i></button>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="dnnClear"></div>
    <div id="productsGrid"></div>
    <script id="tmplToolbar" type="text/x-kendo-template">
        <ul id="ulToolbar" class="inline">
            <li>
                <button id="btnAddNewProduct" class="btn btn-small btn-inverse"><i class="icon-plus icon-white"></i> Adicionar Novo Produto</button>
            </li>
            <li>
                <button id="btnEditSelected" class="btn btn-small"><i class="icon-edit"></i> Editar</button>
            </li>
            <li>
                <button id="btnExportProduct" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-external-link-square"></i>&nbsp; Exportar</button>
            </li>
            <li>
                <button id="btnUpdateProducts" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-ok"></i> Atualizar</button>
            </li>
            <li>
                <button id="btnRemoveSelected" class="btn btn-small btn-danger" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i> Excluir</button>
            </li>
            <li>
                <button id="btnDeleteSelected" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-ban-circle"></i> Desativar</button>
            </li>
            <li>
                <button id="btnRestoreSelected" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-repeat"></i> Restaurar</button>
            </li>
        </ul>
    </script>
    <script id="productLinkTMPL" type="text/x-kendo-template">
        <a onClick="my.showItem('#= uid #'); return false;">#= ProductId #</a>
    </script>
    <script id="delete-confirmation" type="text/x-kendo-template">
        <p class="delete-message">Tem Certeza?</p>

        <button class="delete-confirm btn">Sim</button>
        &nbsp;
        <a class="delete-cancel">Não</a>
    </script>
    <script id="tmplProductDetail" type="text/x-kendo-template">
        <div class="row-fluid">
            <div class="padded"></div>
            <div class="pull-left">
                # if (ProductImageId > 0) { #
                    <a class="photo" title="${ ProductName }" href="/databaseimages/${ ProductImageId }.${ Extension }?maxwidth=800&maxheight=600# if (watermark) { # &watermark=outglow&text=${ watermark } # } #" style="cursor: url(/desktopmodules/rildoinfo/webapi/content/images/zoomin.cur), pointer;">
                        <img src="/databaseimages/${ ProductImageId }.${ Extension }?maxwidth=130&maxheight=130# if (watermark) { # &watermark=outglow&text=${ watermark } # } #" /></a>
                # } else { #
                    <a class="aspNetDisabled">
                        <img src="http://a1.cambelt.co/icon/camera/130" class="img-square thumbnail" />
                    </a>
                # } #
                <div class="imagesListView"></div>
            </div>
            <div class="pull-left">
                <h4>${ ProductName }</h4>
                <small>#= my.htmlConverter.makeHtml(Summary) #</small>
                <div>
                    # if (Barcode) { #
                        <strong>CB: </strong> ${ Barcode }
                    # } else if (ProductRef) { #
                        <strong>REF: </strong> ${ ProductRef }
                    # } #
                </div>
                <div>
                    # if (CategoriesNames) { #
                        <strong>Categorias: </strong> #= my.htmlEncode(CategoriesNames) #
                    # } #
                </div>
            </div>
            <div class="clearfix padded"></div>
        </div>
    </script>
    <script id="tmplProductImages" type="text/x-kendo-template">
        <div class="pull-left padded">
            <a class="group_${ ProductId }" href="/databaseimages/${ ProductImageId }.${ Extension }?maxwidth=800&maxheight=600# if (watermark) { # &watermark=outglow&text=${ watermark } # } #" style="cursor: url(/desktopmodules/rildoinfo/webapi/content/images/zoomin.cur), pointer;">
                <img src="/databaseimages/${ ProductImageId }.${ Extension }?maxwidth=30&maxheight=30# if (watermark) { # &watermark=outglow&text=${ watermark } # } #" style="cursor: url(/desktopmodules/rildoinfo/webapi/content/images/zoomin.cur), pointer;" /></a>
        </div>
    </script>
    <div id="productWindow"></div>
</div>
<div class="padd7 left">
    <button id="btnSyncProducts" class="btn btn-small" data-loading-text="Um momento...">Sincronizar Produtos</button>
    &nbsp;
    <span id="SyncMsg"></span>
</div>
<div class="padd7 right">
    <button id="btnExportAll" data-loading-text="Um momento...">Exportar Produtos</button>
</div>
<div class="clearfix"></div>
<iframe name="tmpFrame" id="tmpFrame" width="1" height="1" style="visibility:hidden;position:absolute;display:none"></iframe>