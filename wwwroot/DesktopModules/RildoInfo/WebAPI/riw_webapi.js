
$(function () {

    //if (document.URL.toLowerCase().indexOf('mensagens') !== -1) {
    //    $('#moduleTitleSkinObject').html(_msgModuleTitle);
    //}
	
	$('#dnn_dnnUser_messageGroup, #dnn_dnnUser_notificationLink').hide();

    my.totalDeletedClients = 0;

    getTotals = function () {
        $.ajax({
            type: 'GET',
            url: '/desktopmodules/riw/api/messages/getTotals'
        }).done(function (data) {
            if (data) {
                $('#totalUnreadMessages').text(data.TotalUnreadMessages);
                $('#totalUnreadNotifications').text(data.TotalUnreadNotifications);
                $('#totalEstimatesOpened').text(data.TotalOpenedEstimates);
                $('#totalClients').text(data.TotalClients);
                $('#totalSales').text(data.TotalSales);
                my.totalDeletedClients = data.TotalDeletedClients;
            } else {
                $.pnotify({
                    title: 'Erro!',
                    text: data.Result,
                    type: 'error',
                    icon: 'fa fa-times-circle fa-lg',
                    addclass: "stack-bottomright",
                    stack: my.stack_bottomright
                });
            }
        }).fail(function (jqXHR, textStatus) {
            console.log(jqXHR.responseText);
        });
    };

    //if (document.URL.toLowerCase().indexOf('gerenciar') !== -1) {
    getTotals();
    //}

    // set local storage
    $.getJSON('/desktopmodules/riw/api/store/GetAppSettings?portalId=0&cultureCode=pt-BR', function (data) {
        if (data) {
            $.each(data, function (index, item) {
                // check for browser storage availability 
                if (my.storage) {

                    // add estimateVisibility setting to storage and set it to true
                    amplify.store.sessionStorage(item.SettingName, item.SettingValue);
                }
            });
            amplify.store.sessionStorage('customerLogo', customerLogo);
            amplify.store.sessionStorage('pdfFooter', pdfFooter);
            amplify.store.sessionStorage('footerText', footerText);
        }
    });

});