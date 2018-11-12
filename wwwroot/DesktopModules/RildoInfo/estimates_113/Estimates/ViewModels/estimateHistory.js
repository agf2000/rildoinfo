
$(function () {
    
    // Declare a proxy to reference the hub. 
    var chat = $.connection.messageHub;
    // Create a function that the hub can call to broadcast messages.
    chat.client.broadcastMessage = function (name, message) {
        // Html encode display name and message. 
        var encodedName = $('<div />').text(name).html();
        var encodedMsg = $('<div />').text(message).html();
        // Add the message to the page. 
        $('#discussion').append('<li><strong>' + encodedName
            + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
    };
    // Get the user name and store it to prepend to messages.
    $('#displayname').val(prompt('Enter your name:', ''));
    // Set initial focus to message input box.  
    $('#message').focus();
    // Start the connection.
    $.connection.hub.start().done(function () {
        $('#sendmessage').click(function () {
            // Call the Send method on the hub. 
            chat.server.send($('#displayname').val(), $('#message').val());
            // Clear text box and reset focus for next comment. 
            $('#message').val('').focus();
        });
    });

    //my.personId = my.getParameterByName('personId');
    //my.estimateId = my.getParameterByName('estimateId');

    //$('#estimateTabs').kendoTabStrip({
    //    animation: {
    //        // fade-out current tab over 1000 milliseconds
    //        close: {
    //            effects: "fadeOut"
    //        },
    //        // fade-in new tab over 500 milliseconds
    //        open: {
    //            effects: "fadeIn"
    //        }
    //    },
    //    select: function (e) {

    //    }
    //});

    //$('#messageBody').markdown({
    //    autofocus: true,
    //    savable: false,
    //    disableButtons: 'image'
    //});
    //$('#messageBody').css({ 'min-width': '90% !important', 'height': '70px' }).attr({ 'cols': '10', 'rows': '3' });

    //my.loadHistory = function () {
        


    //};

    //my.loadHistory();

    //$('#togglePreview').click(function (e) {
    //    e.preventDefault();

    //    if (this.value === 'preview') {
    //        $('#togglePreview').html('<i class="fa fa-code"></i>&nbsp; Editar').attr('value', 'code');
    //        $('#messageBody').data('markdown').showPreview();
    //    } else {
    //        $('#togglePreview').html('<i class="fa fa-eye"></i>&nbsp; Visualizar').attr('value', 'preview');
    //        $('#messageBody').data('markdown').hidePreview();
    //    }
    //});

    //$('#messageBody').autogrow();
    //$('#messageBody').css('overflow', 'hidden').autogrow();

});
