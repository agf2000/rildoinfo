<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Products.ascx.vb" Inherits="RIW.Modules.Store.Views.Products" %>
<div id="storeProducts" class="row-fluid">
    <div class="span12">
        <div id="searchArea">
            <select id="kddlOrder">
                <option value="ProductName ASC">Ordenar por Nome A-Z</option>
                <option value="ProductName DESC">Ordenar por Nome Z-A</option>
                <option value="UnitValue ASC">Ordenar por Menor Preço</option>
                <option value="UnitValue DESC">Ordenar por Maior Preço</option>
                <option value="CreatedOnDate ASC">Ordenar por Mais Novos no Site</option>
            </select>
            <div class="pull-right">
                <div class="input-append">
                    <input type="text" class="input-medium" data-bind="value: filterTerm" />
                    <button id="btnGetProducts" class="btn"><i class="fa fa-search"></i></button>
                </div>
            </div>
            <div class="clearfix">
            </div>
            <span id="status"></span>
            <button id="clearFilter" class="btn btn-link btn-small"><i class="fa fa-times"></i>&nbsp;remover filtro</button>
        </div>
        <div id="productDetail" class="Normal">
            <div class="row-fluid">
                <div class="span3" style="margin-bottom: 14px !important;">
                    <a class="photo" data-bind="attr: { 'title': productName(), 'href': productImageId() ? ('/databaseimages/' + productImageId() + '.' + productImageExtension() + '?maxwidth=800&maxheight=600' + (watermark.length ? ('&watermark=outglow&text=' + watermark) : '')) : '' }" style="cursor: url(/desktopmodules/rildoinfo/webapi/content/images/zoomin.cur), pointer;">
                        <img class="img-square thumbnail slimmage" data-bind="attr: { 'src': (productImageId() > 0 ? ('/databaseimages/' + productImageId() + '.' + productImageExtension() + '?w=140&mode=crop') : '/desktopmodules/RildoInfo/Store/Content/Images/no-image.png') }" />
                    </a>
                    <div class="mt-more-images" data-bind="visible: productImages().length">
                        <div class="more-views">
                            <div class="mt-more-views thumbnailscroller carousel">
                                <div class="thumbnail-viewport">
                                    <ul id="mt-thumbscroller">
                                    </ul>
                                </div>
                                <ul class="thumbnail-direction-nav" data-bind="visible: productImages().length > 4">
                                    <li><a class="thumbnail-prev" href="#">Previous</a></li>
                                    <li><a class="thumbnail-next" href="#">Next</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="span6">
                    <h3><a class="editItem" onclick="my.editDetailItem(); return false;" data-bind="visible: authorized > 2"><span class="fa fa-edit fa-lg"></span></a>&nbsp; <span data-bind="    text: productName"></span></h3>
                    <p data-bind="html: productCode()"></p>
                    <p data-bind="html: summary">
                    </p>
                    <div id="productOptions"></div>
                    <div class="clearfix"></div>
                    <div class="SpecialTag" data-bind="visible: !showPrice"></div>
                    <!-- pricing -->
                    Qde: <span data-bind="text: '(' + unitTypeAbbv + ')', visible: itemType === '1'"></span>
                    <br />
                    <input id="productQtyTextBox" class="input-mini" data-bind="kendoNumericTextBox: { value: productQty, min: 1, format: '', spinners: false }" />
                    <button id="btnProductDetailEstimate" class="btn btn-primary btn-small" data-bind="visible: noStockAllowed">Orçar</button>
                    <div class="padded"></div>
                    <button id="btnProductDetailBuy" class="btn btn-inverse btn-small" data-bind="visible: allowPurchase">Comprar</button>
                    <button id="btnDetailReturn" class="btn btn-small btn-info"><i class="fa fa-chevron-left"></i>&nbsp; Retornar</button>
                </div>
                <div class="span3">
                    <!-- availability -->
                    <div class="alert alert-info hidden" data-bind="visible: qTyStockSet() > 0 && itemType === '1'">Disponível</div>
                    <div class="alert alert-error" data-bind="visible: qTyStockSet() <= 0 && itemType === '1'">Esgotado</div>
                    <!-- availability -->
                    <!-- pricing -->
                    <div class="padded" data-bind="visible: showPrice">
                        <div data-bind="visible: finan_Special_Price() > 0">
                            <div>Era: <span style="text-decoration: line-through;" data-bind="html: kendo.toString(finan_Sale_Price(), 'c')"></span></div>
                            <div>Agora: <span class="NormalRed" data-bind="html: kendo.toString(finan_Special_Price(), 'c')"></span></div>
                        </div>
                        <div data-bind="visible: finan_Special_Price() === 0">
                            <span data-bind="html: 'Preço: <strong>' + kendo.toString(finan_Sale_Price(), 'c') + '</strong>'"></span>
                        </div>
                    </div>
                </div>
                <div class="clearfix"></div>
                <div data-bind="html: description"></div>
                <hr class="clearfix" data-bind="visible: productsRelated().length" />
                <h4 data-bind="visible: productsRelated().length">Recomendações</h4>
                <ul id="relatedProducts" class="inline"></ul>
            </div>
        </div>
        <div class="listView">
            <div id="listView"></div>
            <div class="clearfix"></div>
            <div id="pager" class="k-pager-wrap">
            </div>
        </div>
        <div id="productWindow"></div>
        <div id="dialog-alert" title="Sobre Listas Escolares">
            <p>Uma lista escolar já foi iniciada. Caso queira agrupar listas clique no "Ok". Ou clique em "Cancelar" e finalize a lista atual.</p>
        </div>
        <script type="text/x-kendo-tmpl" id="templateEstimate">
            <div class="well Normal">
                <!-- well -->
                <div class="ajax_block_product first_item item clearfix">
                    <!-- ajax class -->
                    <div class="row-fluid">
                        <div class="span2">
                            <div class="thumbnails">
                                # if (ProductImageId > 0) { #
                                    <a class="photo" title="${ ProductName }" href="/databaseimages/${ ProductImageId }.${ Extension }?maxwidth=800&maxheight=600&s.roundcorners=10# if (watermark) { # &watermark=outglow&text=${ watermark } # } #" style="cursor: url(/desktopmodules/rildoinfo/webapi/content/images/zoomin.cur), pointer;">
                                        <img class="slimmage" src="/databaseimages/${ ProductImageId }.${ Extension }?maxwidth=100&s.roundcorners=10# if (watermark) { # &watermark=outglow&text=${ watermark } # } #" />
                                    </a>
                                # } else { #
                                    <a class="aspNetDisabled">
                                        <img src="/desktopmodules/RildoInfo/Store/Content/Images/no-image.png" class="img-rounded slimmage" />
                                    </a>
                                # } #
                            </div>
                            # if (VideoSrc) { #
                                <a href="${ VideoSrc }" class="btn btn-small youtube"><i class="icon icon-facetime-video"></i> Vídeo</a>
                            # } #
                        </div>
                        <div class="span7 padded">
                            <h5>${ ProductName } <a class="editItem" href="/" onclick="my.editItem('${ uid }'); return false;"><span class="fa fa-edit fa-lg"></span></a></h5>
                            # if (Barcode.length > 0) { #
                                <strong>CB: </strong> ${ Barcode }
                            # } else if (ProductRef.length > 0) { #
                                <strong>REF: </strong> ${ ProductRef }
                            # } #
                            <p>#= my.converter.makeHtml(Summary) #</p>
                            # if (CategoriesNames) { #
                                <strong>Categorias: </strong> #= my.formatCategoryLink(CategoriesNames) #
                            # } #
                        </div>
                        <div class="span3">
                            <!--span 3-->
                            <!-- availability -->
                            # if (ItemType === '1') { #
                                # if (QtyStockSet > 0) { #
                                    <div class="alert alert-info hidden">Disponível</div>
                                # } else { #
                                    # if (Archived) { #
                                        <div class="alert alert-error">Discontinuado</div>
                                    # } else { #
                                        <div class="alert alert-error">Esgotado</div>
                                    # } #
                                # } #
                            # } else { #
                                # if (Archived) { #
                                    <div class="alert alert-error">Discontinuado</div>
                                # } #
                            # } #
                            <!-- availability -->
                            <!-- pricing -->
                            # if (ShowPrice) { #
                                # if (Finan_Special_Price > 0) { #
                                    <div class="padded">
                                        <div>Era: <span style="text-decoration: line-through;">#= kendo.toString(Finan_Sale_Price, 'c') #</span></div>
                                        <div>Agora: <span class="NormalRed">#= kendo.toString(Finan_Special_Price, 'c') #</span></div>
                                    </div>
                                # } else { #
                                    <div class="padded">
                                        <span>Preço: </span><strong>#= kendo.toString(Finan_Sale_Price, 'c') #</strong>
                                    </div>
                                # } #
                            # } else { #
                                # if (Finan_Special_Price > 0) { #
                                    <div class="SpecialTag"></div>
                                # } else { #
                                    <div class="padded">
                                        <span class="noStock" title="Preço disponível via Requisição de Orçamento" data-content="O preço deste item pode ser visualizado após a requisição de orçamento.">Preço: ********</span>
                                    </div>
                                # } #                        
                            # } #
                            <!-- pricing -->
                            <!-- buttons -->
			                <div id="divQuanText_${ ProductId }">
                                Qde: 
                                # if (ItemType === '1') { #
                                    (${ UnitTypeAbbv })<br />
                                # } #
			                    <input id="NumericTextBox_Qty_${ ProductId }" name="NumericTextBox_Qty_${ ProductId }" class="k-numerictextbox" value="1" type="text" />
                                <button id="btnEst_${ ProductId }" class="btn btn-primary btn-small" onclick="my.selectItem('${ uid }'); return false;">Orçar</button>
                                # if (allowPurchase === 'true') { #
                                    <div class="padded"><button id="btnBuy_${ ProductId }" class="btn btn-inverse btn-small purchase" onclick="my.buyItem('${ uid }'); return false;">Comprar</button></div>
                                # } #
                            </div>
                            <div style="padding: 5px 0px 5px 0px;">
                                # if ((ProductImagesCount > 1) || (ProductOptionsCount > 0) || (Description.length > 0)) { #
                                    # if (ProductOptionsCount > 0) { #
                                        <button id="btnDetail_${ ProductId }" class="btn btn-warning btn-small" onclick="my.detailItem('${ ProductId }'); return false;">Mais Opções</button>
                                    # } else { #
                                        <button id="btnDetail_${ ProductId }" class="btn btn-info btn-small" onclick="my.detailItem('${ ProductId }'); return false;">Mais Detalhes</button>
                                    # } #
                                # } #
                            </div>
                            <!-- buttons -->
                        </div>
                        <!--end span 3-->
                    </div>
                    <!-- end fluid row -->
                </div>
                <!--end ajax class -->
            </div>
        </script>
        <script type="text/x-kendo-tmpl" id="templateShowRoom">
        <div class="productShowRoom">
            <label title="${ ProductName }">#= my.Left(ProductName, 20) #</label>
            <div class="thumb">
                <span></span>
                # if (ProductImageId > 0) { #
                    <a class="photo" title="${ ProductName }" href="/databaseimages/${ ProductImageId }.${ Extension }?maxwidth=800&maxheight=600" style="cursor: url(/desktopmodules/rildoinfo/webapi/content/images/zoomin.cur), pointer;">
                        <img src="/databaseimages/${ProductImageId}.${Extension}?maxwidth=130&maxheight=130" style="cursor: url(/desktopmodules/rildoinfo/webapi/content/images/zoomin.cur), pointer;" /></a>
                # } else { #
                    <a class="aspNetDisabled">
                        <img src="/portals/0/Images/No-Image.jpg?w=130&h=130" /></a>
                # } #
            </div>
            <div class="ref">
                # if (Barcode.length > 0) { #
                    #= Barcode #
                # } else { #
                    #= ProductRef #
                # } # 
            </div>
            <button class="k-button" onclick="my.showItem('${ uid }'); return false;"><span class="k-icon k-i-tick"></span>Mais Detalhes</button>
        </div>
        </script>
    </div>
</div>
