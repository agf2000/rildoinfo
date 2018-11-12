
$(function () {

    my.contentId = my.getParameterByName('pId');
        
    $.ajax({
        url: '/desktopmodules/rildoinfo/api/loadhtml/GetHtmlContent?portalId=' + portalID + '&contentId=' + my.contentId
    }).done(function (data) {
        if (data) {
            $('#htmlContent').html(data.Content);
        } else {
            $('#divMsg').freeow('Erro: ', data.Result, my.freewoWarning.opts);
        }
    }).fail(function (jqXHR, textStatus) {
        console.log(jqXHR.responseText);
    });

});