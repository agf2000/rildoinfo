<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Manage_Users.ascx.vb" Inherits="RIW.Modules.People.Views.ManageUsers" %>
<div id="divPeopleManager" class="row-fluid">
    <div class="span12">
        <strong>Grupos:</strong>
        <i class="icon icon-exclamation-sign" title="Filtrar lista por departamentos" data-content="Filtre a lista de colaboradores por departamentos. Escolha o departamento para ativar o filtro."></i>
        <input id="kddlRoles" />
        <div id="peopleGrid"></div>
        <div id="imgWindow"></div>
        <div id="personWindow"></div>
        <script id="tmplToolbar" type="text/x-kendo-template">
            <ul class="ulToolbar">
                <li>
                    <button id="btnAddNewUser" class="btn btn-small btn-inverse"><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
                </li>
                <li>
                    <button id="btnEditSelected" class="btn btn-small" disabled="disabled"><i class="icon-edit"></i> Editar</button>
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
                        Cadastro
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
                        <div>
                            # if (Telephone.length) { #
                                <strong>Fone:</strong> ${ Telephone }
                            # } #
                        </div>
                        <div>
                            # if (Cell.length) { #
                                <strong>Celular:</strong> ${ Cell }
                            # } #
                        </div>
                        <div>
                            # if (Zero800s.length) { #
                                <strong>0800:</strong> ${ Zero800s }
                            # } #
                        </div>
                        <div>
                            # if (Fax.length) { #
                                <strong>Fax:</strong> ${ Fax }
                            # } #
                        </div>
                    </div>
                    <div class="dnnRight" style="width: 49%;">
                        <div class="activities">
                            <strong>Ramo(s):</strong>
                            ${ Activities }
                        </div>
                        <div>
                            <strong>Vendedor:</strong> ${ SalesRepName }
                        </div>
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
        <script id="delete-confirmation" type="text/x-kendo-template">
            <p class="delete-message">Tem Certeza?</p>

            <button class="delete-confirm btn">Sim</button>
            &nbsp;
            <a class="delete-cancel">Não</a>
        </script>
    </div>
</div>