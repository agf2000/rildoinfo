<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Manage_People.ascx.vb" Inherits="RIW.Modules.People.Views.ManagePeople" %>
<div id="divPeopleManager" class="row-fluid">
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
                            <li>
                                <strong>Desativados:</strong>
                                <i class="icon icon-exclamation-sign" title="Entidades Desativadas" data-content="Clique nesta opções para ver ou ocultar itens desativados da lista abaixo."></i>
                                <button id="btnIsDeleted" class="btn btn-small" data-toggle="" title="Clique para mostrar ou esconder entidades desativadas">Mostrar Desativados (<span data-bind="text: deleteds()"></span>)</button>
                                <input id="chkDeleted" type="checkbox" class="normalCheckBox" />
                                <div class="padded"></div>
                                <strong>Vendedores:</strong>
                                <i class="icon icon-exclamation-sign" title="Filtre por Vendedores" data-content="Escolha o vendedor e clique na lupa para aplicar o filtro."></i>
                                <input id="kddlSalesGroup" title="Filtrar lista por vendedor" />
                                <div class="padded"></div>
                                <strong>Status:</strong>
                                <i class="icon icon-exclamation-sign" title="Filtre por Status" data-content="Filtre por status. Escolha o status e clique na lupa para aplicar o filtro."></i>
                                <input id="ddlStatuses" title="Filtrar lista por status" />
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
                                <strong>Entidades:</strong>
                                <i class="icon icon-exclamation-sign" title="Tipos de Entidades" data-content="Filtre por tipos de entidades. Escolha a opção que deseja e clique na lupa para aplicar o filtro."></i>
                                <input id="kddlEntitiesRoles" title="Filtrar lista por tipos de entidade" />
                                <div class="padded"></div>
                                <strong>Procurar Por:</strong>
                                <i class="icon icon-exclamation-sign" title="Campo para busca" data-content="Escolha o campo que deseja executar seu filtro."></i>
                                <select id="kddlFields" data-bind="kendoDropDownList: {}" class="input-medium">
                                    <option value="PersonId">Código</option>
                                    <option value="FirstName"selected>Nome</option>
                                    <option value="LastName">Sobrenome</option>
                                    <option value="CompanyName">Empresa</option>
                                    <option value="Email">Email</option>
                                    <option value="Telephone">Telephone</option>
                                    <option value="CPF">CPF</option>
                                    <option value="Ident">Identidade</option>
                                    <option value="EIN">CNPJ</option>
                                    <option value="CreatedOnDate">Data Inserido</option>
                                    <option value="ModifiedOnDate">Data Alterado</option>
                                </select>
                                <div class="padded"></div>
                                <div class="form-inline">
                                    <strong>Palavra Chave:</strong>
                                    <i class="icon icon-exclamation-sign" title="Palavra Chave" data-content="Filtre por palavra chave. As opções são: Nome próprio, nome da empresa, telefone, email, cpf ou cnpj. Insira a palavra e clique na lupa para aplicar o filtro."></i>
                                    <input id="txtSearch" autocomplete="off" class="input-small" type="text" placeholder="Palavra Chave">
                                    <button id="btnSearch" class="btn btn-small"><i class="icon-search"></i></button>
                                    <button id="btnRemoveFilter" class="btn btn-small" title="Clique aqui para remover filtros"><span class="k-icon k-i-cancel"></span></button>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
        <div id="peopleGrid"></div>
        <div class="padd7">
            <button id="btnSyncPeople" class="btn btn-small" data-loading-text="Um momento...">Sincronizar</button>
            &nbsp;
            <span id="SyncMsg"></span>
        </div>
        <div id="imgWindow"></div>
        <div id="personWindow"></div>
        <script id="tmplToolbar" type="text/x-kendo-template">
            <ul id="ulToolbar">
                <li>
                    <button id="btnAddNewPerson" class="btn btn-small btn-inverse"><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                </li>
                <li>
                    <button id="btnEditSelected" class="btn btn-small" disabled="disabled"><i class="icon-edit"></i> Dados Básicos</button>
                </li>
                <li>
                    <button id="btnAssistSelected" class="btn btn-small" disabled="disabled"><i class="fa fa-phone-square"></i>&nbsp; Atender</button>
                </li>
                <li>
                    <button id="btnFinanSelected" class="btn btn-small" disabled="disabled"><i class="fa fa-usd"></i>&nbsp;  Dados Financeiros</button>
                </li>
                <li>
                    <button id="btnCommSelected" class="btn btn-small" disabled="disabled"><i class="icon-envelope"></i> Comunicar</button>
                </li>
                <li>
                    <button id="btnExportSelected" class="btn btn-small" disabled="disabled"><i class="icon-share"></i> Exportar</button>
                </li>
                <li>
                    <button id="btnHistorySelected" class="btn btn-small" disabled="disabled"><i class="icon-tasks"></i> Mais Ações</button>
                </li>
                <li>
                    <button id="btnDeleteSelected" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-ban-circle"></i> Desativar</button>
                </li>
                <li>
                    <button id="btnRemoveSelected" class="btn btn-small btn-danger" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i> Excluir</button>
                </li>
                <li>
                    <button id="btnRestoreSelected" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-repeat"></i> Restaurar</button>
                </li>
            </ul>
        </script>
        <script type="text/x-kendo-template" id="tmplPersonDetail">
            <div class="personDetails">
                <ul>
                    <li class="k-state-active">
                        Básico
                    </li>
                    <li>
                        Contatos
                    </li>
                    <li>
                        Endereços
                    </li>
                    <li>
                        Orçamentos
                    <li>
                        Compras
                    </li>
                </ul>
                <div>
                    <div class="dnnLeft" style="width: 49%;">
                        # if (PersonType) { #
                            (Pessoa Jurídica)
                        # } else { #
                            (Pessoa Física)
                        # } #
                        <h3>${ DisplayName }</h3>
                        <br />
                        # if (Telephone.length) { #
                            <strong>Fone:</strong> ${ Telephone }
                        # } #
                        <br />
                        # if (Cell.length) { #
                            <strong>Celular:</strong> ${ Cell }
                        # } #
                        <br />
                        # if (Zero800s.length) { #
                            <strong>0800:</strong> ${ Zero800s }
                        # } #
                        <br />
                        # if (Fax.length) { #
                            <strong>Fax:</strong> ${ Fax }
                        # } #
                        <div id="estimates_${ PersonId }"></div>
                        <div id="sales_${ PersonId }"></div>
                    </div>
                    <div class="dnnRight" style="width: 50%;">
                        <div class="activities">
                            <strong>Ramo(s):</strong>
                            ${ Activities }
                        </div>
                        <br />
                        <strong>Vendedor:</strong> ${ SalesRepName }
                    </div>
                    <div class="clearfix">
                    </div>
                </div>
                <div class="contacts"></div>
                <div class="addresses"></div>
                <div class="estimates"></div>
                <div class="orders"></div>
            </div>
        </script>
        <script type="text/x-kendo-template" id="tmplPersonContacts">
            <div class="inline">
                <div class="padded">
                    <address class="well">
                        <strong>Contato: </strong> ${ ContactName }
                        # if (Dept) { # <br /><strong>Dept.: </strong> ${ Dept } # } #
                        # if (ContactPhone1) { # <br /><strong>Telehone 1: </strong> ${ my.formatPhone(ContactPhone1) } # if (PhoneExt1) { # <strong>Ramal:</strong> ${ PhoneExt1 } # } # # } #
                        # if (ContactPhone2) { # <br /><strong>Telehone 2: </strong> ${ my.formatPhone(ContactPhone2) } # if (PhoneExt2) { # <strong>Ramal:</strong> ${ PhoneExt2 } # } # # } #
                        # if (ContactEmail1) { # <br /><strong>Email 1: </strong> ${ ContactEmail1 }  # } #
                        # if (ContactEmail2) { # <br /><strong>Email 2: </strong> ${ ContactEmail2 }  # } #
                        # if (DateBirth === 01/01/1900) { # <br /><strong>Aniversário: </strong> ${ kendo.toString(DateBirth, 'm') } # } #
                    </address>
                </div>
            </div>
        </script>
        <script type="text/x-kendo-template" id="tmplPersonAddresses">
            <div class="container">
                <div class="row-fluid">
                    <div class="span4 well">
                        <address>
                            <strong>${ AddressName }</strong>
                            # if (Street) { # <br /><strong>Endereço: </strong> ${ Street } # if (Unit) { # <strong>Nº:</strong> ${ Unit } # } # # } #
                            # if (Complement) { # <br /><strong>Complemento: </strong> ${ Complement }  # } #
                            # if (District) { # <br /><strong>Bairro: </strong> ${ District }  # } #
                            # if (City) { # <br /><strong>Cidade: </strong> ${ City } # if (Region) { # <strong>Estado:</strong> ${ Region } # } # # } #
                            # if (PostalCode) { # <br /><strong>CEP: </strong> ${ PostalCode } # if (Country) { # <strong>País:</strong> ${ Country } # } # # } #
                            # if (Telephone) { # <br /><strong>Telefone: </strong> ${ my.formatPhone(Telephone) }  # } #
                            # if (Cell) { # <br /><strong>Cell: </strong> ${ my.formatPhone(Cell) }  # } #
                            # if (Fax) { # <br /><strong>Fax: </strong> ${ my.formatPhone(Fax) }  # } #
                        </address>
                    </div>
                </div>
            </div>
        </script>
    </div>
</div>
