<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Profile.ascx.vb" Inherits="RIW.Modules.People.Views.Profile" %>
<div id="editProfile" class="row-fluid">
    <div class="span12">
		<div class="pull-left">
			<div id="personMenu">
				<ul>
					<li id="1">Cadastro</li>
					<li id="2">Login</li>
					<li id="3">Avatar (foto)</li>
				</ul>
			</div>
		</div>
		<div class="pull-left">
			<div id="personForm">
				<h3 id="personTitle"></h3>
				<div class="form-horizontal">
					<div class="pull-left">
						<div class="control-group">
							<label class="control-label required" for="firstNameTextBox"><strong>Nome:</strong></label>
							<div class="controls">
								<input id="firstNameTextBox" type="text" class="enterastab" title="Nome" data-content="Favor inserir o seu nome." data-role="validate" required />
							</div>
						</div>
						<div class="control-group">
							<label class="control-label" for="lastNameTextBox"><strong>Sobrenome:</strong></label>
							<div class="controls">
								<input id="lastNameTextBox" type="text" class="enterastab" title="Sobrenome" data-content="Favor inserir o seu sobrenome." />
							</div>
						</div>
						<div class="control-group">
							<label class="control-label" for="phoneTextBox"><strong>Telefone:</strong></label>
							<div class="controls">
								<input id="phoneTextBox" type="text" class="enterastab" title="Telefone" data-content="Favor inserir o telefone para contato." />
							</div>
						</div>
						<div class="control-group">
							<label class="control-label" for="cellTextBox"><strong>Celular:</strong></label>
							<div class="controls">
								<input id="cellTextBox" type="text" class="enterastab" />
							</div>
						</div>
						<div class="control-group">
							<label class="control-label" for="faxTextBox"><strong>Fax:</strong></label>
							<div class="controls">
								<input id="faxTextBox" type="text" class="enterastab" />
							</div>
						</div>
						<div class="control-group">
							<label class="control-label" for="bioTextArea"><strong>Biografia:</strong></label>
							<div class="controls">
								<textarea id="bioTextArea" placeholder="Um pouco a seu respeito..."></textarea>
							</div>
						</div>
					</div>
					<div class="pull-right">
						<div id="liAddress">
							<div class="control-group">
								<label class="control-label" for="postalCodeTextBox"><strong>Código Postal:</strong></label>
								<div class="controls">
									<input id="postalCodeTextBox" type="text" class="enterastab input-medium" title="CEP" data-content="Favor inserir o CEP referente ao endereço." />
								</div>
							</div>
							<div class="control-group">
								<label class="control-label" for="streetTextBox"><strong>Endereço:</strong></label>
								<div class="controls" style="white-space: nowrap;">
									<input id="streetTextBox" type="text" class="enterastab" title="Endereço" data-content="Favor inserir o nome da rua, avenida..." />
									<input id="unitTextBox" type="text" class="enterastab input-mini" placeholder="Nº" />
								</div>
							</div>
							<div class="control-group">
								<label class="control-label" for="complementTextBox"><strong>Complemento:</strong></label>
								<div class="controls">
									<input id="complementTextBox" type="text" class="enterastab" />
								</div>
							</div>
							<div class="control-group">
								<label class="control-label" for="districtTextBox"><strong>Bairro:</strong></label>
								<div class="controls">
									<input id="districtTextBox" type="text" class="enterastab" title="Bairro" data-content="Favor inserir o nome do bairro ou distrito referente ao endereço." />
								</div>
							</div>
							<div class="control-group">
								<label class="control-label" for="cityTextBox"><strong>Cidade:</strong></label>
								<div class="controls">
									<input id="cityTextBox" type="text" class="enterastab" title="Cidade" data-content="Favor inserir o nome da cidade referente ao endereço." />
								</div>
							</div>
							<div class="control-group">
								<label class="control-label" for="ddlRegions"><strong>Estado:</strong></label>
								<div class="controls">
									<div id="divRegionsDDL">
										<input id="ddlRegions" class="enterastab" />
									</div>
									<div id="divRegionTextBox">
										<input id="regionTextBox" type="text" title="Estado" data-content="Favor inserir o nome do estado ou região referente ao endereço." />
									</div>
								</div>
							</div>
							<div class="control-group">
								<label class="control-label" for="ddlCountries"><strong>País:</strong></label>
								<div class="controls">
									<input id="ddlCountries" />
								</div>
							</div>
						</div>
					</div>
					<div class="clearfix">
					</div>
                    <div class="form-actions">
                        <button class="btn btn-small btnReturn"><i class="fa fa-chevron-left"></i>&nbsp; Fechar</button>
		                <button id="btnUpdateUser" class="btn btn-small btn-inverse" data-loading-text="<i class='fa fa-spinner fa-spin'></i>&nbsp; Um momento..."><i class="fa fa-check"></i>&nbsp; Atualizar</button>           
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
										<div class="controls">
											<input id="loginTextBox" type="text" class="enterastab" />                            
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
		</div>
		<div class="status NormalRed"></div>
	</div>
</div>