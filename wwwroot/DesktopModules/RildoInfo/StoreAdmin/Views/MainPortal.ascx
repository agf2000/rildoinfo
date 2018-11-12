<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="MainPortal.ascx.vb" Inherits="RIW.Modules.StoreAdmin.Views.MainPortal" %>
<div id="mainPortal" class="container-fluid Normal">
    <div class="row-fluid">
        <div class="form-horizontal">
            <div class="control-group">
                <label class="control-label" for="portalNameTextBox">
                    <strong>Título do Website:</strong>
                    <i class="icon icon-info-sign" title="Título do Website" data-content="Nome usado no website e impressões."></i>
                </label>
                <div class="controls">
                    <input id="portalNameTextBox" type="text" class="enterastab input-xlarge" data-bind="value: siteName" required placeholder="Título do Website" data-role="validate" title="Título do Website..." data-content="Favor inserir um nome para o website" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="portalDescriptionTextArea">
                    <strong>Descrição:</strong>
                    <i class="icon icon-info-sign" title="Descrição" data-content="Uma breve descrição sobre o website. Esta informação é usada por buscadores como, Google, Yahoo, etc."></i>
                </label>
                <div class="controls">
                    <textarea id="portalDescriptionTextArea" class="enterastab markdown-editor" data-bind="value: description"></textarea>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="portalKeywordsTextArea">
                    <strong>Palavras-Chaves:</strong>
                    <i class="icon icon-info-sign" title="Palavras-Chaves" data-content="Palavras que identifiquem a natureza do website. Esta informação é usada por buscadores como, Google, Yahoo, etc."></i>
                </label>
                <div class="controls">
                    <textarea id="portalKeywordsTextArea" class="enterastab markdown-editor" data-bind="value: keywords"></textarea>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="itemsPerPage">
                    <strong>Produtos por Página:</strong>
                    <i class="icon icon-info-sign" title="Itens por Página" data-content="Em uma página com vários produtos, insire o número de quantidade de itens que devem ser apresentados."></i>
                </label>
                <div class="controls">
                    <input id="itemsPerPage" class="enterastab input-small" data-bind="kendoNumericTextBox: { min: 5, value: pageSize, format: '' }" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="watermarkTextBox">
                    <strong>Marca D'Agua:</strong>
                    <i class="icon icon-info-sign" title="Marca D'Agua" data-content="Texto apresentado sob as imagens de produtos."></i>
                </label>
                <div class="controls">
                    <input id="watermarkTextBox" type="text" class="enterastab input-xlarge" data-bind="value: watermark" />
                </div>
            </div>
            <div class="accordion">
                <div class="accordion-group">
                    <div class="accordion-heading">
                        <a class="accordion-toggle" data-toggle="collapse" href="#collapseAddress">
                            <h4>Logos</h4>
                        </a>
                    </div>
                    <div id="collapseAddress" class="accordion-body collapse">
                        <div class="accordion-inner">
                            <fieldset>
                                <legend>Logo no Website</legend>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label class="control-label required" for="rootImages"><strong>Imagens Existente:</strong></label>
                                            <div class="controls">
                                                <input id="rootImages" class="enterastab" />
                                                <button class="btn btn-small fileManager"><i class="fa fa-folder-open"></i></button>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">
                                        <div class="text-center">
                                            <img id="logoImage" alt="Logo para Impressão" src="/desktopmodules/rildoinfo/webapi/content/images/spacer.gif" />
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </fieldset>
                            <fieldset>
                                <legend>Logo para Impressão</legend>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label class="control-label required" for="rootBWImages"><strong>Imagens Existente:</strong></label>
                                            <div class="controls">
                                                <input id="rootBWImages" class="enterastab" />
                                                <button class="btn btn-small fileManager"><i class="fa fa-folder-open"></i></button>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">
                                        <div class="text-center">
                                            <img id="logoBWImage" alt="Logo para Impressão" src="/desktopmodules/rildoinfo/webapi/content/images/spacer.gif" />
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-actions">
                <button id="btnUpdatePortal" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Atualizar</button>
            </div>
        </div>
    </div>
</div>
<div id="filesWindow"></div>
<div id="imgWindow"></div>
