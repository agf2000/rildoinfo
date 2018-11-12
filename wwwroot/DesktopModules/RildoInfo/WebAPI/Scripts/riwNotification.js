
$(function () {

    // Sends a notification that expires after a timeout. If timeout = 0 it does not expire
    my.sendNotification = function (image, title, message, timeout, showOnFocus) {
        // Default values for optional params
        timeout = (typeof timeout !== 'undefined') ? timeout : 0;
        showOnFocus = (typeof showOnFocus !== 'undefined') ? showOnFocus : true;

        // Check if the browser window is focused
        var isWindowFocused = document.querySelector(":focus") === null ? false : true;

        // Check if we should send the notification based on the showOnFocus parameter
        var shouldNotify = !isWindowFocused || isWindowFocused && showOnFocus;

        if (window.webkitNotifications && shouldNotify) {
            // Create the notification object
            var notification = window.webkitNotifications.createNotification(
              image, title, message
            );

            notification.ondisplay = function () {
                playAudio();
            };

            // Display the notification
            notification.show();

            if (timeout > 0) {
                // Hide the notification after the timeout
                setTimeout(function () {
                    notification.cancel()
                }, timeout);
            }
        } else if (window.Notification && shouldNotify) {
            var ffNnotification = new Notification(title, { body: message });

            ffNnotification.ondisplay = function () {
                playAudio();
            };

            if (timeout > 0) {
                // Hide the notification after the timeout
                setTimeout(function () {
                    ffNnotification.cancel()
                }, timeout);
            }
        } else {
            var config = {
                nitification: {
                    ntitle: title,
                    nbody: message
                }
            };
            $.wnf(config);
        }
    };

    //// check for notifications support
    //// you can omit the 'window' keyword
    //if (window.webkitNotifications) {
    //    console.log("Notifications are supported!");
    //}
    //else {
    //    console.log("Notifications are not supported for this Browser/OS version yet.");
    //}

    //my.showNotification = function () {

    //    // Create the notification object
    //    var notification1 = null;

    //    if (window.webkitNotifications) {
    //        if (window.webkitNotifications.checkPermission() === 0) { // 0 is PERMISSION_ALLOWED
    //            // function defined in step 2

    //            var timeout = (typeof timeout !== 'undefined') ? timeout : 10000;
    //            var showOnFocus = (typeof showOnFocus !== 'undefined') ? showOnFocus : true;

    //            // Check if the browser window is focused
    //            var isWindowFocused = document.querySelector(":focus") === null ? false : true;

    //            // Check if we should send the notification based on the showOnFocus parameter
    //            var shouldNotify = !isWindowFocused || isWindowFocused && false;

    //            // Create the notification object
    //            notification1 = window.webkitNotifications.createNotification(
    //                '/desktopmodules/rildoinfo/webapi/content/images/ri-logo.png', _siteName, 'Necessitamos de sua atenção no website ' + _siteURL + '.'
    //                );

    //            notification1.ondisplay = function () {
    //                playAudio();
    //            };

    //            // Display the notification
    //            notification1.show();

    //            if (timeout > 0) {
    //                // Hide the notification after the timeout
    //                setTimeout(function () {
    //                    notification1.cancel()
    //                }, timeout);
    //            }
    //        } else {
    //            window.webkitNotifications.requestPermission();
    //        }
    //    } else {
    //        notifyMe();
    //    }
    //};

    //function notifyMe() {

    //    // Let's check if the browser supports notifications
    //    if (!("Notification" in window)) {
    //        alert("This browser does not support desktop notification");
    //    }

    //    // Let's check if the user is okay to get some notification
    //    else if (Notification.permission === "granted") {
    //        // If it's okay let's create a notification
    //        var notification2 = new Notification(_siteName, { body: 'Necessitamos de sua atenção no website ' + _siteURL + '.' });

    //        notification2.ondisplay = function () {
    //            playAudio();
    //        };
    //    }

    //    // Otherwise, we need to ask the user for permission
    //    // Note, Chrome does not implement the permission static property
    //    // So we have to check for NOT 'denied' instead of 'default'
    //    else if (Notification.permission !== 'denied') {
    //        Notification.requestPermission(function (permission) {

    //            // Whatever the user answers, we make sure Chrome stores the information
    //            if (!('permission' in Notification)) {
    //                Notification.permission = permission;
    //            }

    //            // If the user is okay, let's create a notification
    //            if (permission === "granted") {
    //                var notification3 = new Notification(_siteName, { body: 'Necessitamos de sua atenção no website ' + _siteURL + '.' });

    //                notification3.ondisplay = function () {
    //                    playAudio();
    //                };
    //            }
    //        });
    //    }

    //    // At last, if the user already denied any notification, and you 
    //    // want to be respectful there is no need to bother him any more.
    //}

    
    if (Modernizr.audio) {
        if (Modernizr.audio.wav) {
            $("#audiofile").val("/desktopmodules/rildoinfo/webapi/content/sounds/sample.wav");
        }
        if (Modernizr.audio.mp3) {
            $("#audiofile").val("/desktopmodules/rildoinfo/webapi/content/sounds/jad0008a.wav");
        }
    }
    else {
        $("#HTML5Audio").hide();
        $("#OldSound").html('<embed src="/desktopmodules/rildoinfo/webapi/content/sounds/sample.wav" autostart=false width=1 height=1 id="LegacySound" enablejavascript="true" >');
    }    

    var currentFile = "";
    function playAudio() {
        var oAudio = document.getElementById('myaudio');
        // See if we already loaded this audio file.
        if ($("#audiofile").val() !== currentFile) {
            oAudio.src = $("#audiofile").val();
            currentFile = $("#audiofile").val();
        }
        var test = $("#myaudio");
        test.src = $("#audiofile").val();
        oAudio.play();
    }

});
