
$(function () {

    $('#moduleTitleSkinObject').html(moduleTitle);

    var buttons = '<ul class="inline">';
    buttons += '<li><a href="' + addressURL + '" class="btn btn-primary btn-medium" title="Endereço"><i class="fa fa-book fa-lg"></i></a></li>';
    buttons += '<li><a href="' + registrationURL + '" class="btn btn-primary btn-medium" title="Cadastro"><i class="fa fa-edit fa-lg"></i></a></li>';
    buttons += '<li><a href="' + payCondsURL + '" class="btn btn-primary btn-medium" title="Formas e Condições de Pagamento"><i class="fa fa-credit-card fa-lg"></i></a></li>';
    buttons += '<li><a href="' + syncURL + '" class="btn btn-primary btn-medium" title="Orçamento"><i class="fa fa-refresh fa-lg"></i></a></li>';
    buttons += '<li><a href="' + estimateURL + '" class="btn btn-primary btn-medium" title="Orçamento"><i class="fa fa-usd fa-lg"></i></a></li>';
    buttons += '<li><a href="' + smtpURL + '" class="btn btn-primary btn-medium" title="SMTP"><i class="fa fa-envelope-o fa-lg"></i></a></li>';
    buttons += '<li><a href="' + statusesManagerURL + '" class="btn btn-primary btn-medium" title="Status"><i class="fa fa-check-circle fa-lg"></i></a></li>';
    buttons += '<li><a href="' + websiteManagerURL + '" class="btn btn-primary btn-medium" title="Website"><i class="fa fa-bookmark fa-lg"></i></a></li>';
    //buttons += '<li><a href="' + templatesManagerURL + '" class="btn btn-primary btn-medium" title="Templates"><i class="fa fa-puzzle-piece fa-lg"></i></a></li>';
    buttons += '</ul>';
    $('#buttons').html(buttons);



});
