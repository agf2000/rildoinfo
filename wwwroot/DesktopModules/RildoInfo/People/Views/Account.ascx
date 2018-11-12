<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Account.ascx.vb" Inherits="RIW.Modules.People.Views.Account" %>
<div id="myAccount" class="row-fluid">
    <div class="span12">
		<div class="clearfix"></div>
		<div class="span2">
			<div id="personMenu">
				<ul>
					<li id="1">Dados Básicos</li>
					<li id="2">Endereços</li>
					<li id="3">Documentos</li>
					<li id="4">Login</li>
					<li id="5">Avatar (foto)</li>
					<li id="6">Dados Financeiros</li>
				</ul>
			</div>
		</div>
		<div class="span9">
			<div id="personForm">
				<h3 id="personTitle"></h3>
				<div class="form-horizontal">
					<div class="pull-left">
						<div class="control-group" id="liPersonType" data-bind="visible: false">
							<label class="control-label" for="radioPerson"><strong>Pessoa:</strong></label>
							<div class="controls">
								<label class="radio inline">
									<input type="radio" value="1" id="radioPerson" name="Person" data-bind="checked: personType" />
									Física
								</label>
								<label class="radio inline">
									<input type="radio" value="0" id="radioBusiness" name="Person" data-bind="checked: personType" />
									Jurídica
								</label>
							</div>
						</div>
						<div class="control-group" id="liCompanyName">
							<label class="control-label" for="companyTextBox">
                                <strong>Razão Social:</strong>
                            </label>
							<div class="controls">
								<input id="companyTextBox" type="text" class="enterastab" data-bind="value: companyName" title="Razão Social" data-content="Favor inserir o nome da emrpesa." />
							</div>
						</div>
						<div class="control-group">
							<label class="control-label required" for="firstNameTextBox"><strong>Nome:</strong></label>
							<div class="controls">
								<input id="firstNameTextBox" type="text" class="enterastab" data-bind="value: firstName" title="Nome" data-content="Favor inserir o seu nome." data-role="validate" required />
							</div>
						</div>
						<div class="control-group">
							<label class="control-label" for="lastNameTextBox"><strong>Sobrenome:</strong></label>
							<div class="controls">
								<input id="lastNameTextBox" type="text" class="enterastab" data-bind="value: lastName" title="Sobrenome" data-content="Favor inserir o seu sobrenome." />
							</div>
						</div>
						<div class="control-group" id="liPhone">
							<label class="control-label" for="phoneTextBox"><strong>Telefone:</strong></label>
							<div class="controls">
								<input id="phoneTextBox" type="text" class="enterastab" data-bind="value: telephone" title="Telefone" data-content="Favor inserir o telefone para contato." />
							</div>
						</div>
						<div class="control-group" id="liCell">
							<label class="control-label" for="cellTextBox"><strong>Celular:</strong></label>
							<div class="controls">
								<input id="cellTextBox" type="text" class="enterastab" data-bind="value: cell" />
							</div>
						</div>
						<div class="control-group" id="liFax">
							<label class="control-label" for="faxTextBox"><strong>Fax:</strong></label>
							<div class="controls">
								<input id="faxTextBox" type="text" class="enterastab" data-bind="value: fax" />
							</div>
						</div>
						<div class="control-group" id="liZero0800">
							<label class="control-label" for="0800TextBox"><strong>Tel. 0800:</strong></label>
							<div class="controls">
								<input id="0800TextBox" type="text" class="enterastab" data-bind="value: zero800" />
							</div>
						</div>
						<div class="control-group" id="liCPF">
							<label class="control-label" for="cpfTextBox"><strong>CPF:</strong></label>
							<div class="controls">
								<input id="cpfTextBox" type="text" class="enterastab" data-bind="value: cpf" title="CPF" data-content="Favor inserir o seu CPF." />
							</div>
						</div>
						<div class="control-group" id="liIdent">
							<label class="control-label" for="identTextBox"><strong>RG:</strong></label>
							<div class="controls">
								<input id="identTextBox" type="text" class="enterastab" data-bind="value: ident" title="Identidade" data-content="Favor inserir o seu RG." />
							</div>
						</div>
						<div class="control-group" id="liWebsite">
							<label class="control-label" for="websiteTextBox"><strong>Website:</strong></label>
							<div class="controls">
								<input id="websiteTextBox" type="text" class="enterastab" data-bind="value: website" title="Website" data-content="Favor inserir o endereço de seu website." />
							</div>
						</div>
					</div>
					<div class="pull-right">                
						<div id="registerTypes" class="control-group" data-bind="visible: false">
							<label class="control-label" for="selectRegisterTypes"><strong>Registro:</strong></label>
							<div class="controls" style="margin: 0; padding: 0;">
                                <input id="selectRegisterTypes" />
							</div>
						</div>
						<div class="control-group" id="liSalesReps" data-bind="visible: authorized >= 3">
							<label class="control-label" for="ddlSalesRep"><strong>Vendedor:</strong></label>
							<div class="controls">
								<input id="ddlSalesRep" data-bind="kendoDropDownList: { data: saleReps, dataTextField: 'DisplayName', dataValueField: 'UserID', value: selectedSalesRepId }" />
							</div>
						</div>
						<div class="control-group" id="liIndustries">
							<label class="control-label" for="ddlIndustries"><strong>Atividades:</strong></label>
							<div class="controls">
								<select id="ddlIndustries" multiple="multiple" data-bind="options: industries, optionsValue: 'IndustryId', optionsText: 'IndustryTitle', selectedOptions: selectedIndustries, formatSelection: formatSelection, formatResult: formatResult, select2: {}" style="width: 200px"></select>
							</div>
						</div>
						<div class="control-group" id="liEIN">
							<label class="control-label" for="einTextBox"><strong>CNPJ:</strong></label>
							<div class="controls">
								<input id="einTextBox" type="text" class="enterastab" data-bind="value: ein" title="CNPJ" data-content="Favor inserir o CNPJ da emrpesa." />
							</div>
						</div>
						<div class="control-group" id="liST">
							<label class="control-label" for="stateTaxTextBox"><strong>Insc. Est.:</strong></label>
							<div class="controls">
								<input id="stateTaxTextBox" type="text" class="enterastab" data-bind="value: stateTax" title="Incrição Estadual" data-content="Favor inserir a inscrição estadual da emrpesa." />
							</div>
						</div>
						<div class="control-group" id="liCT">
							<label class="control-label" for="cityTaxTextBox"><strong>Insc. Mun.:</strong></label>
							<div class="controls">
								<input id="cityTaxTextBox" type="text" class="enterastab" data-bind="value: cityTax" title="Inscrição Municipal" data-content="Favor inserir a inscrição municipal da emrpesa." />
							</div>
						</div>
						<div class="control-group" id="liBio" data-bind="visible: (authorized === 1) || (bio().length > 1)">
							<label class="control-label" for="bioTextArea"><strong>Biografia:</strong></label>
							<div class="controls">
								<textarea id="bioTextArea" data-bind="value: bio, visible: authorized === 1"></textarea>
								<span id="bioArea" data-bind="value: bio, visible: authorized > 1"></span>
							</div>
						</div>
					</div>
					<div class="clearfix">
					</div>
					<div class="form-actions">
						<button id="btnUpdatePerson" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Atualizar</button>
						<button id="btnDeletePerson" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
					</div>
				</div>
			</div>
			<div id="personAddresses">
				<div id="lvPersonAddresses"></div>
				<script type="text/x-kendo-tmpl" id="tmplPersonAddresses"> 
					<div class="pull-left">
						<div class="input-xlarge well">
							<address>
								# if (IsDeleted) { #
									<strong class="isDeleted">${ AddressName }</strong>
								# } else { #
									<strong>${ AddressName }</strong>
								# } #
								<button class="btn btn-link" onclick="my.editPersonAddress(${ PersonAddressId }); return false;" title="Editar Endereço"><i class="icon-edit"></i>&nbsp;Editar</button><br />
								# if (Street) { # ${ Street } &nbsp; <strong>N°: </strong> ${ Unit }<br />  # } #
								# if (Complement) { # ${ Complement } <br />  # } #
								# if (District) { # <strong>Bairro: </strong> ${ District }<br />  # } #
								# if (City) { # ${ City } # if (Region) { # , ${ Region } &nbsp; <strong>CEP:</strong> ${ PostalCode }<br /> # } else { # <br /> # } # # } #
								# if (Telephone) { # <strong>Telefone: </strong> ${ my.formatPhone(Telephone) }<br />  # } #
								# if (Cell) { # <strong>Celular: </strong> ${ my.formatPhone(Cell) }<br />  # } #
								# if (Fax) { # <strong>Fax: </strong> ${ my.formatPhone(Fax) }<br />  # } #
								# if (Comment) { # <strong>Comentários: </strong>${ Comment } # } #
							</address>
						</div>
					</div>
				</script>
				<div class="clearfix"></div>
				<div class="accordion">
					<div class="accordion-group">
						<div class="accordion-heading">
							<a class="accordion-toggle" data-toggle="collapse" href="#collapseAddress">
								<h4>Adicionar / Editar Endereço</h4>                                
							</a>
						</div>
						<div id="collapseAddress" class="accordion-body collapse">
							<div class="accordion-inner">
								<div class="form-horizontal">
									<div class="pull-left">
										<div class="control-group">
											<label class="control-label required" for="addressNameTextBox"><strong>Nome:</strong></label>
											<div class="controls">
												<input id="addressNameTextBox" type="text" class="enterastab" placeholder="Residência, Escritório, Cobrança, etc." title="Título" data-content="Dê um título ao endereço." data-role="validate" required />
											</div>
										</div>
										<div class="control-group">
											<label class="control-label" for="addressPhoneTextBox"><strong>Telefone:</strong></label>
											<div class="controls">
												<input id="addressPhoneTextBox" type="text" class="enterastab input-medium" />
											</div>
										</div>
										<div class="control-group">
											<label class="control-label" for="addressCellTextBox"><strong>Celular:</strong></label>
											<div class="controls">
												<input id="addressCellTextBox" type="text" class="enterastab input-medium" />
											</div>
										</div>
										<div class="control-group">
											<label class="control-label" for="addressFaxTextBox"><strong>Fax:</strong></label>
											<div class="controls">
												<input id="addressFaxTextBox" type="text" class="enterastab input-medium" />
											</div>
										</div>
										<div class="control-group">
											<label class="control-label" for="addressViewOrderTextBox"><strong>Ordem:</strong></label>
											<div class="controls">
												<input id="addressViewOrderTextBox" class="enterastab input-mini" data-bind="kendoNumericTextBox: { min: 1, value: 1, decimals: 0, format: '#' }" />
											</div>
										</div>
										<div class="control-group">
											<label class="control-label" for="addressCommentsTextArea"><strong>Observações:</strong></label>
											<div class="controls">
												<textarea id="addressCommentsTextArea"></textarea>
											</div>
										</div>
									</div>
									<div class="pull-right">
										<div class="control-group">
											<label class="control-label" for="addressPostalCodeTextBox"><strong>CEP:</strong></label>
											<div class="controls">
												<input id="addressPostalCodeTextBox" type="text" class="enterastab input-small" />
											</div>
										</div>
										<div class="control-group">
											<label class="control-label" for="addressUnitTextBox"><strong>Endereço:</strong></label>
											<div class="controls" style="white-space: nowrap;">
												<input id="addressStreetTextBox" type="text" class="enterastab input-medium" />
												<input id="addressUnitTextBox" type="text" class="enterastab input-mini" placeholder="Nº" />
											</div>
										</div>
										<div class="control-group">
											<label class="control-label" for="addressComplementTextBox"><strong>Complemento:</strong></label>
											<div class="controls">
												<input id="addressComplementTextBox" type="text" class="enterastab" />
											</div>
										</div>
										<div class="control-group">
											<label class="control-label" for="addressDistrictTextBox"><strong>Bairro:</strong></label>
											<div class="controls">
												<input id="addressDistrictTextBox" type="text" class="enterastab" />
											</div>
										</div>
										<div class="control-group">
											<label class="control-label" for="addressCityTextBox"><strong>Cidade:</strong></label>
											<div class="controls">
												<input id="addressCityTextBox" type="text" class="enterastab" />
											</div>
										</div>
										<div class="control-group hidden">
											<label class="control-label" for="radioPerson"><strong>País:</strong></label>
											<div class="controls">
												<input id="ddlAddressCountries" class="enterastab" data-bind="kendoDropDownList: { data: countries, value: selectedCountry, dataTextField: 'CountryName', dataValueField: 'CountryCode' }" />
											</div>
										</div>
										<div class="control-group">
											<label class="control-label" for="ddlAddressRegions"><strong>Estado:</strong></label>
											<div class="controls">
												<div data-bind="visible: hasRegions()">
													<input id="ddlAddressRegions" data-bind="kendoDropDownList: { data: regions, value: selectedRegion, dataTextField: 'RegionName', dataValueField: 'RegionCode' }" />
												</div>
												<div data-bind="visible: !hasRegions()">
													<input id="addressRegionTextBox" type="text" class="enterastab" />
												</div>
											</div>
										</div>
									</div>
									<div class="clearfix">
									</div>
									<div class="form-actions">
										<button id="btnCancelAddress" class="btn btn-small"><i class="fa fa-level-up"></i>&nbsp; Cancelar</button>
										<button id="btnUpdatePersonAddress" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar Endereço</button>
										<button id="btnDeletePersonAddress" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon icon-ban-circle"></i>&nbsp; Desativar</button>
										<button id="btnRestorePersonAddress" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-refresh"></i>&nbsp; Restaurar</button>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
			<div id="personDocuments">
				<div id="lvPersonDocs"></div>
				<script type="text/x-kendo-tmpl" id="tmplPersonDocs">
					<div class="pull-left">
						<div class="input-medium well pagination-centered">
							<div class="padded">${ my.Left(DocName, 20) }</div>
							# if (Extension === 'png' || Extension === 'jpg') { #
								<a href="/portals/0/docs/${ userID }/${ FileName }" title="${ DocDesc }" class="photo"><div class="nailthumb-container DNNAligncenter"><img atl="${ DocDesc }" src="/portals/0/docs/${ userID }/${ FileName }" /></div></a>
							# } else { #
								<a href="/portals/0/docs/${ userID }/${ FileName }" title="${ DocDesc }" onclick="window.open(this.href); return false;"><img atl="${ DocDesc }" src="/desktopmodules/rildoinfo/people/content/images/Docment.png" /></a>
							# } #
							<div class="padded"><button onclick="my.removeClientDoc(${ PersonDocId }); return false;" class="btn btn-link">&times; Excluir</button></div>
						</div>
					</div>
				</script>
				<div class="clearfix"></div>
				<div class="accordion">
					<div class="accordion-group">
						<div class="accordion-heading">
							<a class="accordion-toggle" data-toggle="collapse" href="#collapseDocument">
								<h4>Adicionar Documento</h4>
							</a>
						</div>
						<div id="collapseDocument" class="accordion-body collapse">
							<div class="accordion-inner">
								<div class="form-horizontal">
									<div class="control-group">
										<label class="control-label required" for="contactNameTextBox"><strong>Nome:</strong></label>
										<div class="controls">
											<input id="docNameTextBox" type="text" class="enterastab" />
										</div>
									</div>
									<div class="control-group">
										<label class="control-label required" for="contactNameTextBox"><strong>Descrição</strong></label>
										<div class="controls">
											<textarea id="docDescTextArea" class="enterastab"></textarea>
										</div>
									</div>
									<div class="control-group">
										<label class="control-label required" for="contactNameTextBox"><strong>Arquivo:</strong></label>
										<div class="controls">
											<input name="files" id="files" type="file" />
										</div>
									</div>
								</div>
						   </div>
						</div>
					</div>
				</div>
			</div>
			<div id="editLogin">
				<div class="accordion">
					<div class="accordion-group">
						<div class="accordion-heading">
							<a class="accordion-toggle" data-toggle="collapse" href="#collapseLogin">
								<h4>Login de Acesso</h4>
							</a>
						</div>
						<div id="collapseLogin" class="accordion-body collapse in">
							<div class="accordion-inner">
								<div class="form-horizontal">
									<div class="control-group">
										<label class="control-label required" for="loginTextBox"><strong>Login / Email:</strong></label>
										<div class="controls" style="white-space: nowrap;">
											<input id="loginTextBox" type="email" class="enterastab" placeholder="ex.: nome@yahoo.com" />                            
										</div>
									</div>
									<div class="form-actions">
										<button id="btnCheckLogin" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Verificar Disponibilidade</button>
										<button id="btnUpdateLogin" class="btn btn-small" disabled="disabled" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Alterar Login</button>
										<span id="usernameCheck"></span>
									</div>
								</div>
							</div>
						</div>
					</div>
					<div class="accordion-group">
						<div class="accordion-heading">
							<a class="accordion-toggle" data-toggle="collapse" href="#collapsePassword">
								<h4>Alterar Senha
								<span class="Normal"> - Senha Alterada em: <span id="lastPasswordChangedDate"></span></span></h4>
							</a>
						</div>
						<div id="collapsePassword" class="accordion-body collapse in">
							<div class="accordion-inner">
								<div class="form-horizontal">
									<div class="control-group">
										<label class="control-label required" for="currentPasswordTextBox">
											<i class="icon icon-exclamation-sign" title="Senha Atual" data-content="É necessário inserir sua senha atual."></i>
											<strong>Senha Atual:</strong>
										</label>
										<div class="controls">
											<input id="currentPasswordTextBox" type="password" class="enterastab input-medium" required="required" data-role="validate" title="Senha Atual" data-content="É necessário inserir sua senha atual." />
										</div>
									</div>
									<div class="control-group">
										<label class="control-label required" for="newPasswordTextBox">
											<i class="icon icon-exclamation-sign" title="Senha" data-content="Senha deve ser de no mínimo 7 caracteres. Não deve conter espaços ou letras com acento"></i>
											<strong>Senha:</strong>
										</label>
										<div class="controls" style="white-space: nowrap;">
											<input id="newPasswordTextBox" type="password" class="enterastab input-medium" required="required" data-role="validate" title="Senha" data-content="Senha deve ser de no mínimo 7 caracteres. Não deve conter espaços ou letras com acento." />
											<span id="passMetter"></span>
										</div>
									</div>
									<div class="control-group">
										<label class="control-label required" for="confirmPasswordTextBox">
											<i class="icon icon-exclamation-sign" title="Confirmar Senha" data-content="Favor confirmar sua senha para evitar erro de digitação."></i>
											<strong>Repetir Senha:</strong>
										</label>
										<div class="controls" style="white-space: nowrap;">
											<input id="confirmPasswordTextBox" type="password" class="enterastab input-medium" required="required" data-role="validate" title="Confirmar Senha" data-content="Favor confirmar sua senha para evitar erro de digitação." />
											<span id="passConfirm"></span>
										</div>
									</div>
									<div class="form-actions">
										<button id="btnUpdatePassword" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Alterar Senha</button>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
			<div id="userPhoto">
				<div class="accordion">
					<div class="accordion-group">
						<div class="accordion-heading">
							<a class="accordion-toggle" data-toggle="collapse" href="#collapseAvatar">
								<h4>Foto de perfil</h4>
							</a>
						</div>
						<div id="collapseAvatar" class="accordion-body collapse in">
							<div class="accordion-inner">
								<div class="form-horizontal">
									<div id="divPhotos" class="control-group">
										<label class="control-label" for="photos"><strong>Foto:</strong></label>
										<div class="controls">
											<input id="photos" name="photos" type="file" />
										</div>
									</div>
									<div id="divAvatar" class="control-group">
										<label class="control-label" for="aImg"><strong>Avatar:</strong></label>
										<div class="controls">
											<a id="aImg"></a>
											<br />
											<button id="btnRemoveAvatar" class="btn btn-link" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Remover</button>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
            <div id="personFinance">
                <div id="clientFinanceTabs">
                    <ul>
                        <li id="liIncomes">Fonte de Renda</li>
                        <li id="liPersonalRefs">Ref. Pessoal</li>
                        <li id="liBankRefs" class="k-state-active">Ref. Bancária</li>
                        <li id="liCommRefs">Ref. Comercial</li>
                        <li id="liPartners">Dados dos Sócios</li>
                        <li id="liPartnerBanks">Ref. Bancária dos Sócios</li>
                    </ul>
                    <div>
                        <div id="editIncomes">
                            <div id="lvIncomeSources"></div>
                            <script type="text/x-kendo-tmpl" id="tmplClientIncomeSources"> 
                                <div class="padded">
                                    <div class="well">
                                        <div class="span6">
                                            <address>
                                                <strong>${ ISName }</strong>
                                                <a href="/" onclick="my.editClientIncomeSource(${ ClientIncomeSourceId }); return false;" title="Editar Renda">
                                                    <i class="icon-edit"></i> Editar
                                                </a><br />
                                                # if (ISEIN) { #
                                                    <strong>CNPJ: </strong>${ ISEIN }<br />
                                                # } #
                                                # if (ISST) { #
                                                    <strong>Insc. Est.: </strong>${ ISST }<br />
                                                # } #
                                                # if (ISCT) { #
                                                    <strong>Insc. Mun.: </strong>${ ISCT }<br />
                                                # } #
                                                <strong>Renda Mensal: </strong>#= kendo.toString(ISIncome, 'c') #<br />
                                                # if (ISProof) { #
                                                    <strong>Renda pode ser comprovada? </strong>Sim<br />
                                                # } else { #
                                                    <strong>Renda pode ser comprovada? </strong>Não
                                                # } #
                                            </address>
                                        </div>
                                        <div class="span6">
                                            <address>
                                                # if (ISAddress) { #
                                                    <strong>Endereço: </strong>${ ISAddress} Nº: ${ ISAddressUnit }<br />
                                                # } #
                                                # if (ISComplement) { #
                                                    <strong>Complemento: </strong>${ ISComplement }<br />
                                                # } #
                                                # if (ISDistrict) { #
                                                    <strong>Bairro: </strong>${ ISDistrict }<br />
                                                # } #
                                                # if (ISCity) { #
                                                    <strong>Cidade: </strong>${ ISCity }
                                                # } #
                                                # if (ISPostalCode) { #
                                                    CEP: ${ my.formatPostalcode(ISPostalCode) }<br />
                                                # } #
                                                # if (ISRegion) { #
                                                    <strong>Estado: </strong>${ ISRegion }<br />
                                                # } #
                                                # if (ISPhone) { #
                                                    <strong>Telefone: </strong>${ ISPhone }<br />
                                                # } #
                                                # if (ISFax) { #
                                                    <strong>Fax: </strong>${ ISFax }<br />
                                                # } #
                                            </address>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                </div>
                            </script>
                            <div class="clearfix"></div>
                            <div id="collapseIncomeSource" class="accordion">
                                <div class="accordion-group">
                                    <div class="accordion-heading">
                                        <h4 class="panel-title">
                                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#collapseIncomeSource" href="#collapseIncomeSourceForm">Adicionar / Editar Fonte de Renda
                                            </a>
                                        </h4>
                                    </div>
                                    <div id="collapseIncomeSourceForm" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <div class="form-horizontal">
                                                <div class="span6">
                                                    <div class="control-group">
                                                        <label class="control-label required" for="iSNameTextBox"><strong>Nome:</strong></label>
                                                        <div class="controls">
                                                            <input id="iSNameTextBox" type="text" class="enterastab" placeholder="Nome da fonte de renda" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="iSPhoneTextBox"><strong>Telefone:</strong></label>
                                                        <div class="controls">
                                                            <input id="iSPhoneTextBox" type="text" class="enterastab" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="iSEINTextBox"><strong>CNPJ:</strong></label>
                                                        <div class="controls">
                                                            <input id="iSEINTextBox" type="text" class="enterastab" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="iSNameTextBox"><strong>Insc. Est.:</strong></label>
                                                        <div class="controls">
                                                            <input id="iSInsEstTextBox" type="text" class="enterastab" placeholder="Inscrição Estadual" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="iSInsMunTextBox"><strong>Insc. Mun.:</strong></label>
                                                        <div class="controls">
                                                            <input id="iSInsMunTextBox" type="text" class="enterastab" placeholder="Inscrição Municipal" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label required" for="iSInsMunTextBox"><strong>Renda:</strong></label>
                                                        <div class="controls">
                                                            <input id="iSIncomeNumericTextBox" class="enterastab" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="iSProofCheckbox"><strong>Renda Comprov.:</strong></label>
                                                        <div class="controls ">
                                                            <label class="checkbox">
                                                                <input id="iSProofCheckbox" type="checkbox" /><span id="iSProof">Marque se puder ser comprovado.</span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="span6">
                                                    <div class="control-group Hidden">
                                                        <label class="control-label" for="iSFaxTextBox"><strong>Fax:</strong></label>
                                                        <div class="controls">
                                                            <input id="iSFaxTextBox" type="text" class="enterastab" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="iSPostalCodeTextBox"><strong>CEP:</strong></label>
                                                        <div class="controls">
                                                            <input id="iSPostalCodeTextBox" type="text" class="enterastab" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="iSStreetTextBox"><strong>Endereço:</strong></label>
                                                        <div class="controls">
                                                            <input id="iSStreetTextBox" type="text" class="input-medium enterastab" />
                                                            <input id="iSUnitTextBox" type="text" class="input-mini enterastab" placeholder="Nº" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="iSComplementTextBox"><strong>Complemento:</strong></label>
                                                        <div class="controls" style="white-space: nowrap;">
                                                            <input id="iSComplementTextBox" type="text" class="enterastab" placeholder="Casa, Loja, etc" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="iSDistrictTextBox"><strong>Bairro:</strong></label>
                                                        <div class="controls">
                                                            <input id="iSDistrictTextBox" type="text" class="enterastab" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="iSCityTextBox"><strong>Cidade:</strong></label>
                                                        <div class="controls">
                                                            <input id="iSCityTextBox" type="text" class="enterastab" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="ddlISRegions"><strong>Estado:</strong></label>
                                                        <div class="controls">
                                                            <div data-bind="visible: hasRegions()">
                                                                <input id="ddlISRegions" data-bind="kendoDropDownList: { data: regions, value: selectedRegion, dataTextField: 'RegionName', dataValueField: 'RegionCode', cascadeFrom: 'ddlCountries' }" />
                                                            </div>
                                                            <div data-bind="visible: !hasRegions()">
                                                                <input id="iSRegionTextBox" type="text" class="enterastab" data-bind="value: selectedRegion" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="control-group Hidden">
                                                        <label class="control-label" for="ddlISCountries"><strong>País:</strong></label>
                                                        <div class="controls">
                                                            <input id="ddlISCountries" class="enterastab" data-bind="kendoDropDownList: { data: countries, value: selectedCountry, dataTextField: 'CountryName', dataValueField: 'CountryCode' }" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="clearfix">
                                                </div>
                                                <div class="form-actions">
                                                    <button id="btnCancelIncomeSource" class="btn btn-small"><i class="fa fa-level-up"></i>&nbsp; Cancelar</button>
                                                    <button id="btnUpdateIncomeSource" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar Renda</button>
                                                    <button id="btnRemoveIncomeSource" class="btn btn-small btn-danger" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
                                                    <button id="btnCopyIncomeSource" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-copy"></i>&nbsp; Copiar</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div>
                        <div id="editPersonalRefs">
                            <div id="lvPersonalRefs"></div>
                            <script type="text/x-kendo-tmpl" id="tmplClientPersonalRefs">
                                <div class="pull-left padded">
                                    <div class="well">
                                        <address>
                                            <strong>${ PRName }</strong>
                                            <a href="/" onclick="my.editClientPersonalRef(${ ClientPersonalRefId }); return false;" title="Editar Referência">
                                                <i class="icon-edit"></i> Editar</a><br />
                                            # if (PRPhone.length > 0) { #
                                                <strong>Telefone: </strong>${ my.formatPhone(PRPhone) }
                                            # } #<br />
                                            # if (PREmail.length > 0) { #
                                                <strong>Email: </strong>${ PREmail }
                                            # } #
                                        </address>
                                    </div>
                                </div>
                            </script>
                            <div class="clearfix"></div>
                            <div id="collapsePersonalRef" class="accordion">
                                <div class="accordion-group">
                                    <div class="accordion-heading">
                                        <h4 class="panel-title">
                                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#collapsePersonalRef" href="#collapsePersonalRefForm">Adicionar / Editar Referência Pessoal
                                            </a>
                                        </h4>
                                    </div>
                                    <div id="collapsePersonalRefForm" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <div class="form-horizontal">
                                                <div class="control-group">
                                                    <label class="control-label required" for="personalRefNameTextBox required"><strong>Nome:</strong></label>
                                                    <div class="controls">
                                                        <input id="personalRefNameTextBox" type="text" class="enterastab" placeholder="Nome completo" />
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label required" for="personalRefPhoneTextBox"><strong>Telefone:</strong></label>
                                                    <div class="controls">
                                                        <input id="personalRefPhoneTextBox" type="text" class="enterastab" />
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label" for="personalRefEmailTextBox"><strong>Email:</strong></label>
                                                    <div class="controls">
                                                        <input id="personalRefEmailTextBox" type="email" class="enterastab" />
                                                    </div>
                                                </div>
                                                <div class="form-actions">
                                                    <button id="btnCancelPersonalRef" class="btn btn-small"><i class="fa fa-level-up"></i>&nbsp; Cancelar</button>
                                                    <button id="btnUpdatePersonalRef" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar Referência</button>
                                                    <button id="btnRemovePersonalRef" class="btn btn-small btn-danger" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div>
                        <div id="editBankRefs">
                            <div id="lvBankRefs"></div>
                            <script type="text/x-kendo-tmpl" id="tmplClientBankRefs">
                                <div class="pull-left padded">
                                    <div class="well">
                                        <address>
                                            <strong>${ BankRef }</strong>
                                            <a href="/" onclick="my.editClientBankRef(${ ClientBankRefId }); return false;" title="Editar Referência">
                                                <i class="icon-edit"></i> Editar</a><br />
                                            # if (BankRefAgency) { #
                                                <strong>Agência: </strong>${ BankRefAgency }<br />
                                            # } #
                                            # if(BankRefAccount) { #
                                                <strong> Conta: </strong>${ BankRefAccount }<br />
                                            # } #
                                            # if (BankRefClientSince) { #
                                                <strong>Cliente desde: </strong> ${ kendo.toString(kendo.parseDate(BankRefClientSince), 'd') }<br />
                                            # } #
                                            # if (BankRefContact) { #
                                                <strong>Contato: </strong>${ BankRefContact }
                                            # } #
                                            # if(BankRefPhone) { #
                                                - ${ my.formatPhone(BankRefPhone) }<br />
                                            # } #
                                            # if (BankRefAccountType) { #
                                                <strong>Tipo de conta: </strong> ${ BankRefAccountType }<br />
                                            # } #
                                            # if (BankRefCredit > 0) { #
                                                <strong>Crédito: </strong>${ kendo.toString(BankRefCredit, 'c') }
                                            # } #
                                        </address>
                                    </div>
                                </div>
                            </script>
                            <div class="clearfix"></div>
                            <div id="collapseBankRef" class="accordion">
                                <div class="accordion-group">
                                    <div class="accordion-heading">
                                        <h4 class="panel-title">
                                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#collapseBankRef" href="#collapseBankRefForm">Adicionar / Editar Referência Bancária
                                            </a>
                                        </h4>
                                    </div>
                                    <div id="collapseBankRefForm" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <div class="form-horizontal">
                                                <div class="span6">
                                                    <div class="control-group">
                                                        <label class="control-label required" for="bankRefTextBox"><strong>Banco:</strong></label>
                                                        <div class="controls">
                                                            <input id="bankRefTextBox" type="text" class="enterastab" placeholder="Nome do banco" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label required" for="bankRefAgencyTextBox"><strong>Agência:</strong></label>
                                                        <div class="controls">
                                                            <input id="bankRefAgencyTextBox" type="text" class="enterastab" placeholder="Número da agência" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label required" for="bankRefAccountTextBox"><strong>Conta:</strong></label>
                                                        <div class="controls">
                                                            <input id="bankRefAccountTextBox" type="text" class="enterastab" placeholder="Número da conta" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label required" for="bankRefClientSinceTextBox"><strong>Cliente desde:</strong></label>
                                                        <div class="controls">
                                                            <input id="bankRefClientSinceTextBox" class="enterastab clientContact Combo" placeholder="ex: 05/12/2000" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="span6">
                                                    <div class="control-group">
                                                        <label class="control-label" for="bankRefContactNameTextBox"><strong>Contato:</strong></label>
                                                        <div class="controls">
                                                            <input id="bankRefContactNameTextBox" type="text" class="enterastab" placeholder="Nome do Contato" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="bankRefContactPhoneTextBox"><strong>Telefone:</strong></label>
                                                        <div class="controls">
                                                            <input id="bankRefContactPhoneTextBox" type="text" class="enterastab" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label required" for="bankRefAccountTypeTextBox"><strong>Tipo de Conta:</strong></label>
                                                        <div class="controls">
                                                            <input id="bankRefAccountTypeTextBox" type="text" class="enterastab" placeholder="Conta Corrente, Poupança, etc" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="bankRefCreditLimitTextBox"><strong>Limite de Crédito:</strong></label>
                                                        <div class="controls">
                                                            <input id="bankRefCreditLimitTextBox" class="enterastab" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="clearfix">
                                                </div>
                                                <div class="form-actions">
                                                    <button id="btnCancelBankRef" class="btn btn-small"><i class="fa fa-level-up"></i>&nbsp; Cancelar</button>
                                                    <button id="btnUpdateBankRef" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar Referência</button>
                                                    <button id="btnRemoveBankRef" class="btn btn-small btn-danger" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div>
                        <div id="editCommRefs">
                            <div id="lvCommRefs"></div>
                            <script type="text/x-kendo-tmpl" id="tmplClientCommRefs">
                                <div class="pull-left padded">
                                    <div class="well">
                                        <address>
                                            <strong>${ CommRefBusiness }</strong>
                                            <a href="/" onclick="my.editClientCommRef(${ ClientCommRefId }); return false;" title="Editar Referência">
                                                <i class="icon-edit"></i> Editar</a><br />
                                            # if (CommRefContact) { #
                                                <strong>Contato: </strong>${ CommRefContact }<br />
                                            # } #
                                            # if (CommRefPhone) { #
                                                <strong>Telefone:</strong> ${ my.formatPhone(CommRefPhone) }<br />
                                            # } #
                                            # if (CommRefLastActivity) { #
                                                <strong>Última atividade: </strong> ${ kendo.toString(kendo.parseDate(CommRefLastActivity), 'd') }<br />
                                            # } #
                                            # if (CommRefCredit > 0) { #
                                                <strong>Crédito: </strong>${ kendo.toString(CommRefCredit, 'c') }<br />
                                            # } #
                                        </address>
                                    </div>
                                </div>
                            </script>
                            <div class="clearfix"></div>
                            <div id="collapseCommRef" class="accordion">
                                <div class="accordion-group">
                                    <div class="accordion-heading">
                                        <h4 class="panel-title">
                                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#collapseCommRef" href="#collapseCommRefForm">Adicionar / Editar Referência Comercial
                                            </a>
                                        </h4>
                                    </div>
                                    <div id="collapseCommRefForm" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <div class="form-horizontal">
                                                <div class="span6">
                                                    <div class="control-group">
                                                        <label class="control-label required" for="commRefNameTextBox"><strong>Razão Social:</strong></label>
                                                        <div class="controls">
                                                            <input id="commRefNameTextBox" type="text" class="enterastab" placeholder="Nome da instituição comercial" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="commRefContactTextBox"><strong>Contato:</strong></label>
                                                        <div class="controls">
                                                            <input id="commRefContactTextBox" type="text" class="enterastab" placeholder="Nome do contato" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label required" for="commRefPhoneTextBox"><strong>Telefone:</strong></label>
                                                        <div class="controls">
                                                            <input id="commRefPhoneTextBox" type="text" class="enterastab" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="span6">
                                                    <div class="control-group">
                                                        <label class="control-label" for="commRefLastActivityTextBox"><strong>Última Compra:</strong></label>
                                                        <div class="controls">
                                                            <input id="commRefLastActivityTextBox" class="enterastab Combo" placeholder="Data última compra" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="commRefCreditTextBox"><strong>Crédito:</strong></label>
                                                        <div class="controls">
                                                            <input id="commRefCreditTextBox" class="enterastab" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="clearfix">
                                                </div>
                                                <div class="form-actions">
                                                    <button id="btnCancelCommRef" class="btn btn-small"><i class="fa fa-level-up"></i>&nbsp; Cancelar</button>
                                                    <button id="btnUpdateCommRef" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar Referência</button>
                                                    <button id="btnRemoveCommRef" class="btn btn-small btn-danger" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div>
                        <div id="editPartners">
                            <div id="lvClientPartners"></div>
                            <script type="text/x-kendo-tmpl" id="tmplClientPartners">
                                <div class="pull-left padded">
                                    <div class="well">
                                        <div class="pull-left">
                                            <address>
                                                <strong>${ PartnerName }</strong>
                                                <a href="/" onclick="my.editClientPartner(${ ClientPartnerId }); return false;" title="Editar Referência">
                                                    <i class="icon-edit"></i> Editar</a><br />
                                                # if (PartnerCPF) { #
                                                    <strong>CPF: </strong>${ PartnerCPF }<br />
                                                # } #
                                                # if (PartnerIdentity) { #
                                                    <strong>RG: </strong>${ PartnerIdentity }<br />
                                                # } #
                                                # if (PartnerPhone) { #
                                                    <strong>Telefone: </strong>${ my.formatPhone(PartnerPhone) }<br />
                                                # } #
                                                # if (PartnerCell) { #
                                                    <strong>Celular: </strong>${ my.formatPhone(PartnerCell) }<br />
                                                # } #
                                                # if (PartnerEmail) { #
                                                    <strong>Email: </strong>${ PartnerEmail }<br />
                                                # } #
                                                # if (PartnerQuota > 0) { #
                                                    <strong>Participação: </strong>${ PartnerQuota } %<br />
                                                # } #
                                            </address>
                                        </div>
                                        <div class="pull-left">
                                            <address>
                                                # if (PartnerAddress) { #
                                                    <strong>Endereço: </strong>${ PartnerAddress } <strong> Nº: </strong>${ PartnerAddressUnit }<br />
                                                # } #
                                                # if (PartnerComplement) { #
                                                    ${ PartnerComplement }<br />
                                                # } #
                                                # if (PartnerDistrict) { #
                                                    <strong>Bairro: </strong>${ PartnerDistrict }<br />
                                                # } #
                                                # if (PartnerCity) { #
                                                    <strong>Cidade: </strong>${ PartnerCity }
                                                # } #
                                                # if (PartnerPostalCode) { #
                                                    &nbsp; <strong>CEP: </strong>${ PartnerPostalCode }<br />
                                                # } #
                                                # if (PartnerRegion) { #
                                                    <strong>Estado: </strong>${ PartnerRegion }
                                                # } #
                                            </address>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                </div>
                            </script>
                            <div class="clearfix"></div>
                            <div id="collapsePartner" class="accordion">
                                <div class="accordion-group">
                                    <div class="accordion-heading">
                                        <h4 class="panel-title">
                                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#collapsePartner" href="#collapsePartnerForm">Adicionar / Editar Dados dos Sócios
                                            </a>
                                        </h4>
                                    </div>
                                    <div id="collapsePartnerForm" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <div class="form-horizontal">
                                                <div class="span6">
                                                    <div class="control-group">
                                                        <label class="control-label required" for="partnerNameTextBox"><strong>Nome:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerNameTextBox" type="text" class="enterastab" placeholder="Nome completo do sócio" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="partnerCPFTextBox"><strong>CPF:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerCPFTextBox" type="text" class="enterastab" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="partnerIdentTextBox"><strong>RG:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerIdentTextBox" type="text" class="enterastab" placeholder="Número da Identidade" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label required" for="partnerPhoneTextBox"><strong>Telefone:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerPhoneTextBox" type="text" class="enterastab" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="partnerCellTextBox"><strong>Celular:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerCellTextBox" type="text" class="enterastab" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="partnerEmailTextBox"><strong>Email:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerEmailTextBox" type="email" class="enterastab" placeholder="Endereço de email do sócio" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="partnerQuotaTextBox"><strong>Participação:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerQuotaTextBox" name="partnerQuotaTextBox" class="enterastab" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="span6">
                                                    <div class="control-group">
                                                        <label class="control-label" for="partnerPostalCodeTextBox"><strong>CEP:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerPostalCodeTextBox" type="text" class="enterastab" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="partnerStreetTextBox"><strong>Endereço:</strong></label>
                                                        <div class="controls" style="white-space: nowrap;">
                                                            <input id="partnerStreetTextBox" type="text" class="input-medium enterastab" />
                                                            <input id="partnerUnitTextBox" type="text" class="input-mini enterastab" placeholder="Nº" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="partnerComplementTextBox"><strong>Complemento:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerComplementTextBox" type="text" class="enterastab" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="partnerDistrictTextBox"><strong>Bairro:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerDistrictTextBox" type="text" class="enterastab" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="partnerCityTextBox"><strong>Cidade:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerCityTextBox" type="text" class="enterastab" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="ddlPartnerRegions"><strong>Estado:</strong></label>
                                                        <div class="controls">
                                                            <div data-bind="visible: hasRegions()">
                                                                <input id="ddlPartnerRegions" data-bind="kendoDropDownList: { data: regions, value: selectedRegion, dataTextField: 'RegionName', dataValueField: 'RegionCode', cascadeFrom: 'ddlCountries' }" />
                                                            </div>
                                                            <div data-bind="visible: !hasRegions()">
                                                                <input id="partnerRegionTextBox" type="text" class="enterastab" data-bind="value: selectedRegion" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="control-group Hidden">
                                                        <label class="control-label" for="ddlPartnerCountries"><strong>País:</strong></label>
                                                        <div class="controls">
                                                            <input id="ddlPartnerCountries" class="enterastab" data-bind="kendoDropDownList: { data: countries, value: selectedCountry, dataTextField: 'CountryName', dataValueField: 'CountryCode' }" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="clearfix">
                                                </div>
                                                <div class="form-actions">
                                                    <button id="btnCancelPartner" class="btn btn-small"><i class="fa fa-level-up"></i>&nbsp; Cancelar</button>
                                                    <button id="btnUpdatePartner" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar Sócio</button>
                                                    <button id="btnRemovePartner" class="btn btn-small btn-danger" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
                                                    <button id="btnCopyPartner" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-copy"></i>&nbsp; Copiar</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div>
                        <div id="editPartnerBankRefs">
                            <div id="lvPartnerBankRefs"></div>
                            <script type="text/x-kendo-tmpl" id="tmplClientPartnerBankRefs">
                                <div class="pull-left padded">
                                    <div class="well">
                                        <address>
                                            <strong>${ PartnerName }</strong>
                                            <a href="/" onclick="my.editClientPartnerBankRef(${ ClientPartnerBankRefId }); return false;" title="Editar Referência">
                                                <i class="icon-edit"></i> Editar</a><br />
                                            # if (BankRef.length > 0) { #
                                                <strong>Banco: </strong>${ BankRef }<br />
                                            # } #
                                            # if (BankRefAgency) { #
                                                <strong>Agência: </strong>${ BankRefAgency }<br />
                                            # } #
                                            # if(BankRefAccount) { #
                                                <strong>Conta: </strong>${ BankRefAccount }<br />
                                            # } #
                                            # if (BankRefClientSince) { #
                                                <strong>Cliente desde: </strong> ${ kendo.toString(kendo.parseDate(BankRefClientSince), 'd') }<br />
                                            # } #
                                            # if (BankRefContact) { #
                                                <strong>Contato: </strong>${ BankRefContact }
                                            # } #
                                            # if(BankRefPhone) { #
                                                <strong>Telefone: </strong>${ BankRefPhone }<br />
                                            # } #
                                            # if (BankRefAccountType) { #
                                                <strong>Tipo de conta: </strong> ${ BankRefAccountType }<br />
                                            # } #
                                            # if (BankRefCredit > 0) { #
                                                <strong>Crédito: </strong>${ kendo.toString(BankRefCredit, 'c') }
                                            # } #
                                        </address>
                                    </div>
                                </div>
                            </script>
                            <div class="clearfix"></div>
                            <div id="collapseBankPartnerRef" class="accordion">
                                <div class="accordion-group">
                                    <div class="accordion-heading">
                                        <h4 class="panel-title">
                                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#collapseBankPartnerRef" href="#collapseBankPartnerRefForm">Adicionar / Editar Referência Bancária dos Sócios
                                            </a>
                                        </h4>
                                    </div>
                                    <div id="collapseBankPartnerRefForm" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <div class="form-horizontal">
                                                <div class="span6">
                                                    <div class="control-group">
                                                        <label class="control-label required" for="partnerBankRefPartnerFullNameTextBox"><strong>Nome:</strong></label>
                                                        <div class="controls">
                                                            <input id="ddlClientPartners" class="enterastab" placeholder=" Selecionar Sócio " style="width: 220px;" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label required" for="partnerBankRefTextBox"><strong>Banco:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerBankRefTextBox" type="text" class="enterastab" placeholder="Nome do banco" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label required" for="partnerBankRefAgencyTextBox"><strong>Agência:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerBankRefAgencyTextBox" type="text" class="enterastab" placeholder="Número da agência" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label required" for="partnerBankRefAccountTextBox"><strong>Conta:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerBankRefAccountTextBox" type="text" class="enterastab" placeholder="Número da conta" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="partnerBankRefCreditLimitTextBox"><strong>Crédito:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerBankRefCreditLimitTextBox" class="enterastab" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="span6">
                                                    <div class="control-group">
                                                        <label class="control-label" for="partnerBankRefContactNameTextBox"><strong>Contato:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerBankRefContactNameTextBox" type="text" class="enterastab" placeholder="Nome do contato" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label" for="partnerBankRefContactPhoneTextBox"><strong>Telefone:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerBankRefContactPhoneTextBox" type="text" class="enterastab" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label required" for="partnerBankRefAccountTypeTextBox"><strong>Tipo de Conta:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerBankRefAccountTypeTextBox" type="text" class="enterastab" placeholder="Conta Corrente, Poupança, etc" />
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label required" for="partnerBankRefClientSinceTextBox"><strong>Cliente desde:</strong></label>
                                                        <div class="controls">
                                                            <input id="partnerBankRefClientSinceTextBox" class="enterastab Combo" placeholder="ex: 05/12/2000" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="clearfix">
                                                </div>
                                                <div class="form-actions">
                                                    <button id="btnCancelPartnerBankRef" class="btn btn-small"><i class="fa fa-level-up"></i>&nbsp; Cancelar</button>
                                                    <button id="btnUpdatePartnerBankRef" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar Referência</button>
                                                    <button id="btnRemovePartnerBankRef" class="btn btn-small btn-danger" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
		</div>
		<div class="status NormalRed"></div>
	</div>
</div>