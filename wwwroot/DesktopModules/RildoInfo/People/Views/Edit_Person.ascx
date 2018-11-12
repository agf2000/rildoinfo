<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Edit_Person.ascx.vb" Inherits="RIW.Modules.People.Views.EditPerson" %>
<div id="editPerson" class="row-fluid">
	<div class="span12">
		<div id="actionsMenu">
			<ul>
				<li id="12">
					<i class="fa fa-chevron-left"></i>&nbsp; Retornar
				</li>
				<li id="7">
					<i class="icon icon-edit"></i>&nbsp;Dados Básicos
				</li>
				<li id="8">
					<i class="fa fa-phone-square"></i>&nbsp; Atender
				</li>
				<li id="9">
					<i class="fa fa-usd"></i>&nbsp; Dados Financeiros
				</li>
				<li id="11">
					<i class="fa fa-envelope"></i>&nbsp; Comunicar
				</li>
				<li id="10">
					<i class="icon-tasks"></i>&nbsp;Mais Ações
				</li>
			</ul>
		</div>
		<div class="pull-left">
			<div id="personMenu">
				<ul>
					<li id="1">Cadastro</li>
					<li id="2">Endereços</li>
					<li id="3">Contatos</li>
					<li id="4">Documentos</li>
					<li id="5">Login</li>
					<li id="6">Avatar (foto)</li>
				</ul>
			</div>
		</div>
		<div class="pull-right">
			<div id="personForm">
				<h3 id="personTitle"></h3>
				<div class="form-horizontal">
					<div class="pull-left">                
						<div class="control-group" id="liPersonType" data-bind="visible: askCompany()">
							<label class="control-label" for="radioPerson"><strong>Natureza:</strong></label>
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
							<label class="control-label required" for="companyTextBox">
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
						<div class="control-group">
							<label class="control-label" for="displayNameTextBox"><strong>Apelido / Fantasia:</strong></label>
							<div class="controls">
								<input id="displayNameTextBox" type="text" class="enterastab" data-bind="value: displayName" title="Apelido / Fantasia" data-content="Favor inserir o apelido ou fantasia no caso de uma empresa. Nome e sobrenome ou razão social será usado se deixado em branco." />
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
						<div class="control-group" data-bind="visible: !personUserId() > 0" id="liEmail">
							<label class="control-label" for="btnCheckEmail"><strong>Email:</strong></label>
							<div class="controls" style="white-space: nowrap;">
								<input id="emailTextBox" class="enterastab" type="email" placeholder="ex. nome@yahoo.com" data-bind="value: email, css: { required: reqEmail() }, attr: { required: reqEmail() }" title="Email" data-content="Favor inserir o seu endereço de email. O email é usado para se autenticar no website." />
								<button id="btnCheckEmail" class="btn btn-small" title="Verificar" data-loading-text="<i class='fa fa-spinner fa-spin'></i>"><i class="fa fa-check"></i></button>
							</div>
						</div>
						<div class="control-group" id="liWebsite">
							<label class="control-label" for="websiteTextBox"><strong>Website:</strong></label>
							<div class="controls">
								<input id="websiteTextBox" type="text" class="enterastab" data-bind="value: website" title="Website" data-content="Favor inserir o endereço de seu website." />
							</div>
						</div>
						<div id="liLogin" class="control-group">
							<label class="control-label" for="enableLoginChk"><strong>Criar Login:</strong></label>
							<div class="controls">
								<label class="checkbox">
									<input id="enableLoginChk" type="checkbox" />
									Marque p/ criar login (email obrigatório)
								</label>
							</div>
						</div>
					</div>
					<div class="pull-right">
						<div id="registerTypes" class="control-group">
							<label class="control-label required" for="selectRegisterTypes"><strong>Grupo:</strong></label>
							<div class="controls" style="margin: 0; padding: 0;">
                                <input id="selectRegisterTypes" />
							</div>
						</div>
						<div class="control-group" id="liSalesReps" data-bind="visible: authorized >= 3 && my.vendor !== '1'">
							<label class="control-label" for="ddlSalesRep"><strong>Vendedor:</strong></label>
							<div class="controls">
								<input id="ddlSalesRep" data-bind="kendoDropDownList: { data: saleReps, value: selectedSalesRepId, dataTextField: 'DisplayName', dataValueField: 'MemberId' }" />
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
						<div id="liAddress" data-bind="visible: askAddress() && personId() === 0">
							<div class="control-group">
								<label class="control-label" for="postalCodeTextBox"><strong>Código Postal:</strong></label>
								<div class="controls">
									<input id="postalCodeTextBox" type="text" class="enterastab" data-bind="value: postalCode" title="CEP" data-content="Favor inserir o CEP referente ao endereço." />
								</div>
							</div>
							<div class="control-group">
								<label class="control-label" for="streetTextBox"><strong>Endereço:</strong></label>
								<div class="controls" style="white-space: nowrap;">
									<input id="streetTextBox" type="text" class="enterastab input-medium" data-bind="value: street" title="Endereço" data-content="Favor inserir o nome da rua, avenida..." />
									<input id="unitTextBox" type="text" class="enterastab input-mini" placeholder="Nº" data-bind="value: unit" />
								</div>
							</div>
							<div class="control-group">
								<label class="control-label" for="complementTextBox"><strong>Complemento:</strong></label>
								<div class="controls">
									<input id="complementTextBox" type="text" class="enterastab" data-bind="value: complement" />
								</div>
							</div>
							<div class="control-group">
								<label class="control-label" for="districtTextBox"><strong>Bairro:</strong></label>
								<div class="controls">
									<input id="districtTextBox" type="text" class="enterastab" data-bind="value: district" title="Bairro" data-content="Favor inserir o nome do bairro ou distrito referente ao endereço." />
								</div>
							</div>
							<div class="control-group">
								<label class="control-label" for="cityTextBox"><strong>Cidade:</strong></label>
								<div class="controls">
									<input id="cityTextBox" type="text" class="enterastab" data-bind="value: city" title="Cidade" data-content="Favor inserir o nome da cidade referente ao endereço." />
								</div>
							</div>
							<div class="control-group">
								<label class="control-label" for="ddlRegions"><strong>Estado:</strong></label>
								<div class="controls">
									<div data-bind="visible: hasRegions()">
										<input id="ddlRegions" class="enterastab" data-bind="kendoDropDownList: { data: regions, value: selectedRegion, dataTextField: 'RegionName', dataValueField: 'RegionCode', cascadeFrom: 'ddlCountries' }" />
									</div>
									<div data-bind="visible: !hasRegions()">
										<input id="regionTextBox" type="text" data-bind="value: selectedRegion" title="Estado" data-content="Favor inserir o nome do estado ou região referente ao endereço." />
									</div>
								</div>
							</div>
							<div class="control-group">
								<label class="control-label" for="ddlCountries"><strong>País:</strong></label>
								<div class="controls">
									<input id="ddlCountries" data-bind="kendoDropDownList: { data: countries, value: selectedCountry, dataTextField: 'CountryName', dataValueField: 'CountryCode' }" />
								</div>
							</div>
						</div>      
						<div class="control-group" id="liComments">
							<label class="control-label" for="commentsTextArea"><strong>Observações:</strong></label>
							<div class="controls">
								<textarea id="commentsTextArea" data-bind="value: comments"></textarea>
							</div>
						</div>  
                        <div class="control-group" data-bind="visible: my.personId > 0">
                            <label class="control-label" for="chkBoxBlocked"><strong>Bloqueado:</strong></label>
                            <div class="controls">
						        <input id="chkBoxBlocked" type="checkbox" class="normalCheckBox switch-small" data-on-label="SIM" data-off-label="NÃO" />
                            </div>
                        </div>
                        <div class="control-group" data-bind="visible: my.personId > 0">
                            <label class="control-label" for="txtBoxBlockedReason"><strong>Razão do bloqueio:</strong></label>
                            <div class="controls">
                                <textarea id="blockedReasonTextArea" data-bind="value: reasonBlocked"></textarea>
                            </div>
                        </div>        
					</div>
					<div class="clearfix">
					</div>
					<div class="form-actions">
                        <button class="btn btn-small btnReturn" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-chevron-left"></i>&nbsp; Retornar</button>
						<button id="btnUpdatePerson" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar</button>
						<button id="btnDeletePerson" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon icon-ban-circle"></i>&nbsp; Desativar</button>
						<button id="btnRemovePerson" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
						<button id="btnRestorePerson" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-refresh"></i>&nbsp; Restaurar</button>
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
								<button class="btn btn-link" onclick="my.editPersonAddress(${ PersonAddressId }); return false;" title="Editar Endereço"><i class="icon-edit"></i>Editar</button><br />
								# if (Street) { # <strong>Endereço: </strong> ${ Street } &nbsp; <strong>N°: </strong> ${ Unit }<br />  # } #
								# if (Complement) { # ${ Complement } <br />  # } #
								# if (District) { # <strong>Bairro: </strong> ${ District }<br />  # } #
								# if (City) { # <strong>Cidade: </strong> ${ City } # if (Region) { # <strong>Estado:</strong> ${ Region } &nbsp; ${ PostalCode }<br /> # } else { # <br /> # } # # } #
								# if (Telephone) { # <strong>Telefone: </strong> ${ my.formatPhone(Telephone) }<br />  # } #
								# if (Cell) { # <strong>Celular: </strong> ${ my.formatPhone(Cell) }<br />  # } #
								# if (Fax) { # <strong>Fax: </strong> ${ my.formatPhone(Fax) }<br />  # } #
								# if (Comment) { # <strong>Comentários: </strong>${ Comment } # } #
							</address>
						</div>
					</div>
				</script>
				<div class="clearfix"></div>
                <p>&nbsp;</p>
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
												<input id="addressStreetTextBox" type="text" class="enterastab" />
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
										<button id="btnDeletePersonAddress" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-ban"></i>&nbsp; Desativar</button>
										<button id="btnRestorePersonAddress" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-refresh"></i>&nbsp; Restaurar</button>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>                
                <div class="padd7">
                    <button id="btnSyncPersonAddress" data-loading-text="Um momento...">Sincronizar</button>
                </div>
			</div>
			<div id="personContacts">
				<div id="lvPersonContacts"></div>
				<script type="text/x-kendo-tmpl" id="tmplPersonContacts">
					<div class="pull-left">
						<div class="input-xlarge well">
							<address>
								<span class="NormalBold">${ ContactName }</span>
								<button id="edit_${ PersonContactId }" class="btn btn-link" onclick="my.editPersonContact(${ PersonContactId }); return false;" title="Editar Contato"><i class="icon-edit"></i>Editar</button><br />
								# if (ContactPhone1) { #
									<strong>Telefone: </strong>${ my.formatPhone(ContactPhone1) } # if (PhoneExt1) { # &nbsp; <strong>Ramal: </strong>${ PhoneExt1 }<br /> # } else { # <br /> # } #
								# } #
								# if (ContactPhone2) { #
									<strong>Telefone: </strong>${ ContactPhone2 } # if (PhoneExt2) { # &nbsp; <strong>Ramal: </strong>${ PhoneExt2 }<br /> # } else { # <br /> # } #
								# } #
								# if (ContactEmail1) { #
									<strong>Email: </strong>${ ContactEmail1 }<br />
								# } #
								# if (ContactEmail2) { #
									<strong>Email: </strong>${ ContactEmail2 }<br />
								# } #
								# if (Dept) { #
									<strong>Dept: </strong>${ Dept }<br />
								# } #
								# if (DateBirth > kendo.parseDate("1900-01-01 00:00:00")) { #
									<strong>Faz aniversário em: </strong>${ kendo.toString(DateBirth, "m") }<br />
								# } #
								# if (AddressName) { #
									<strong>Endereço: </strong>${ AddressName }<br />
								# } #
								# if (Comments) { #
									<strong>Comentários: </strong>${ Comments }
								# } #
							</address>
						</div>
					</div>
				</script>   
				<div class="clearfix"></div>         
				<div class="accordion">
					<div class="accordion-group">
						<div class="accordion-heading">
							<a class="accordion-toggle" data-toggle="collapse" href="#collapseContact">
								<h4>Adicionar / Editar Contato</h4>                           
							</a>
						</div>
						<div id="collapseContact" class="accordion-body collapse">
							<div class="accordion-inner">
								<div class="form-horizontal">
									<div class="pull-left">
										<div class="control-group">
											<label class="control-label required" for="contactNameTextBox"><strong>Contato:</strong></label>
											<div class="controls">
												<input id="contactNameTextBox" type="text" class="enterastab input-medium" title="Nome do Contato" data-content="Favor inserir o nome do contato." data-role="validate" required />
											</div>
										</div>
										<div class="control-group">
											<label class="control-label" for="email1NameTextBox"><strong>Email 1:</strong></label>
											<div class="controls">
												<input id="email1NameTextBox" type="email" class="enterastab" placeholder="ex. nome@yahoo.com" />
											</div>
										</div>
										<div class="control-group">
											<label class="control-label" for="email2NameTextBox"><strong>Email 2:</strong></label>
											<div class="controls">
												<input id="email2NameTextBox" type="email" class="enterastab" />
											</div>
										</div>
										<div id="liPhone1" class="control-group">
											<label class="control-label" for="phone1TextBox"><strong>Telefone:</strong></label>
											<div class="controls" style="white-space: nowrap;">
												<input id="phone1TextBox" type="text" class="enterastab input-medium" />
												<input id="phone1ExtTextBox" type="text" class="enterastab input-mini" placeholder="Ramal" />
											</div>
										</div>
										<div id="liPhone2" class="control-group">
											<label class="control-label" for="phone2TextBox"><strong>Telefone:</strong></label>
											<div class="controls" style="white-space: nowrap;">
												<input id="phone2TextBox" type="text" class="enterastab input-medium" />
												<input id="phone2ExtTextBox" type="text" class="enterastab input-mini" placeholder="Ramal" />
											</div>
										</div>
									</div>
									<div class="pull-right">
										<div class="control-group">
											<label class="control-label" for="deptTextBox"><strong>Depart.:</strong></label>
											<div class="controls">
												<input id="deptTextBox" type="text" class="enterastab input-medium" />
											</div>
										</div>
										<div class="control-group">
											<label class="control-label" for="bDayTextBox"><strong>Aniversário:</strong></label>
											<div class="controls">
												<input id="bDayTextBox" class="enterastab" />
											</div>
										</div>
										<div class="control-group">
											<label class="control-label" for="commentsContactTextArea"><strong>Observações:</strong></label>
											<div class="controls">
												<textarea id="commentsContactTextArea" class="enterastab input-medium"></textarea>
											</div>
										</div>
										<div class="control-group">
											<label class="control-label" for="ddlContactAddresses"><strong>Endereço</strong></label>
											<div class="controls">
												<input id="ddlContactAddresses" data-bind="kendoDropDownList: { data: addresses, value: selectedContactAddress, dataTextField: 'AddressName', dataValueField: 'PersonAddressId', optionLabel: 'Selecionar' }" />
											</div>
										</div>
									</div>
								</div>
								<div class="clearfix">
								</div>
								<div class="form-actions">
									<button id="btnCancelContact" class="btn btn-small"><i class="fa fa-level-up"></i>&nbsp; Cancelar</button>
									<button id="btnUpdatePersonContact" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="icon-plus icon-white"></i>&nbsp; Adicionar Contato</button>
									<button id="btnRemovePersonContact" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-times"></i>&nbsp; Excluir</button>
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
								<a href="/portals/0/docs/${ PersonId }/${ FileName }" title="${ DocDesc }" class="photo"><div class="nailthumb-container DNNAligncenter"><img atl="${ DocDesc }" src="/portals/0/docs/${ PersonId }/${ FileName }" /></div></a>
							# } else { #
								<a href="/portals/0/docs/${ PersonId }/${ FileName }" title="${ DocDesc }" onclick="window.open(this.href); return false;"><img atl="${ DocDesc }" src="/desktopmodules/rildoinfo/people/content/images/Docment.png" /></a>
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
										<label class="control-label required" for="docNameTextBox"><strong>Nome:</strong></label>
										<div class="controls">
											<input id="docNameTextBox" type="text" class="enterastab" />
										</div>
									</div>
									<div class="control-group">
										<label class="control-label required" for="docDescTextArea"><strong>Descrição</strong></label>
										<div class="controls">
											<textarea id="docDescTextArea" class="enterastab"></textarea>
										</div>
									</div>
									<div class="control-group">
										<label class="control-label required" for="files"><strong>Arquivo:</strong></label>
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
											<input id="loginTextBox" type="email" class="enterastab" />                            
										</div>
									</div>
									<div class="form-actions">
										<button id="btnCheckLogin" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Verificar</button>
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
									<div class="dnnFormItem Hidden">
										<label class="control-label required" for="currentPasswordTextBox">
											<i class="icon icon-exclamation-sign" title="Senha Atual" data-content="Senha deve ser de no mínimo 7 caracteres. Não deve conter espaços ou letras com acento."></i>
											<strong>Senha Atual:</strong>
										</label>
										<div class="controls">
											<input id="currentPasswordTextBox" type="password" class="enterastab input-medium" title="Senha Atual" data-content="É necessário inserir sua senha atual." />
											<span id="currentPass"></span>
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
										<button id="btnRandomPassword" class="btn btn-small" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento...">Gerar Senha Aleatória</button>
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
									<div class="control-group">
										<label class="control-label" for="photos"><strong>Foto:</strong></label>
										<div class="controls">
											<input id="photos" name="photos" type="file" />
											<a id="aImg"></a>
											<br />
											<button id="btnRemoveAvatar" class="btn btn-link" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."></button>
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