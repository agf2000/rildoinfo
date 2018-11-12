// A) Place this in a script block in your .ascx code - (See Footnote A)
$(function () {
    // TODO: Find a better name than "obj"
    // Pass JQuery and KnockoutJS to your constructor
    var obj = new riwScript($, ko);
    obj.init(_moduleId, 'something');
});

// B) Place this in a JavaScript file in your module (See Footnote B)
function riwScript($, ko) {
    // 1) Capture this, for namespacing purposes.
    var self = this;

    // 2) If you're using KnockoutJs, you could set the ViewModel here
    self.data = {
        // Setup ko.observable and ko.observableArray items:
        info: ko.observable()
    };

    self.init = function init(moduleId, elementId) {
        // 3) Instantiating the Services Framework for your module
        self.sf = $.ServicesFramework(moduleId);

        // 4) Scripting the Ajax call
        $.ajax({
            type: 'GET',
            url: self.sf.getServiceRoot('riw')
                    + 'RIStore' / 'Login',
            beforeSend: self.sf.setModuleHeaders,
            data: null
        }).done(function (serverData, textStatus) {
            if (textStatus === "success") {
                // Process your data from the server. E.g.:
                self.data.info(serverData);
                ko.applyBindings(self.data, document.getElementById($(elementId).attr("id")));
            } else {
                // Give appropriate feedback to the user; e.g.:
                displayMessage('#' + elementId, // placeholderSelector - such as '##ModuleName_ModuleID'
                                "Your error message here", // Server responded, but not with a success status
                                "dnnFormWarning");
            }
        }).fail(function (xhr, status) {
            // Give appropriate feedback to the user; e.g.:
            displayMessage('#' + elementId, // placeholderSelector - such as '##ModuleName_ModuleID'
                            "Your error message here", // server did not process request (service was not found)
                            "dnnFormWarning");
        });
    };

    /* Utilities:
        *  displayMessage - as taken from DotNetNuke: CoreMessaging.js, lines 17-27
        */
    function displayMessage(placeholderSelector, message, cssclass) {
        var messageNode = $("")
                .addClass('dnnFormMessage ' + cssclass)
                .text(message);

        $(placeholderSelector).prepend(messageNode);

        messageNode.fadeOut(3000, 'easeInExpo', function () {
            messageNode.remove();
        });
    };


    /* --------------
    * Notes:
    *  A) Note the inline code for a .ascx to get the ModuleID and the client-side .ascx ID
    *      obj.init(<%= ModuleId %>, ''<%=apiExplorerModule.ClientID %>'');
    *  B) The following items are specific to your script:
    *      riwScript => A namespace you could use for your JavaScript.
    *      'riw' => The name that you set up as the route name
    *          (first parameter in your call to MapHttpRoute(); 
    *           see your IServiceRouteMapper instance's RegsiterRoutes() method)
    *      'RIStore' => The name of your controller class
    *          (without the "Controller" part that appends your class name;
    *           it should also inherit from DnnApiController)
    *      'Login' => The name of the method on your controller class
    *      #ModuleName_ModuleID => A possible unique ID for an element on your module
    */
};