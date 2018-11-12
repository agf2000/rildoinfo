$(function () {

    my.viewModel();

    my.vm.loadCountries();
    my.vm.loadRegions();

    $('#btnConfig').click(function (e) {
        e.preventDefault();

        document.location.href = _optionsURL;
    });

    $('#divMethod .fa-exclamation-circle').popover({
        placement: 'top',
        trigger: 'hover'
    });

    $('#listView').kendoListView({
        dataSource: my.docsData,
        template: kendo.template($('#tmplCatalogs').html()),
        dataBound: function (e) {
            var listview = this;
            if (listview.dataSource.data().length > 0) {
                if (_userID > 0) $('#divFiles').show();
                //$.each($('#listView label'), function (i, item) {
                //    $(item).popover({
                //        placement: 'top'
                //    });
                //});

                $('#listView label').each(function () {
                    var $this = $(this);

                    if ($this.find('.tooltipcontent').html().length > 0) {
                        $this.popover({
                            //delay: { hide: 60000 },
                            trigger: 'hover',
                            placement: 'top',
                            html: true,
                            content: $this.find('.tooltipcontent').html(),
                            title: $this.find('.title').html()
                        });
                    }
                });
            }

            //$('#listView label').each(function () {
            //    var $this = $(this);

            //    $this.popover({
            //        trigger: 'hover',
            //        //placement: 'top',
            //        html: true,
            //        content: $this.find('.tooltipcontent').html(),
            //        title: $this.find('.tooltipcontent').html()
            //    });
            //});
        }
    });

    //$('#listView').kendoTooltip({
    //    filter: '.catalogs .checkbox',
    //    position: 'top',
    //    content: function (e) {
    //        var container = e.target.parent();
    //        return container.find('.tooltipcontent').html();
    //    }
    //});

    if (_userID > 0) {
        $('#companyTextBox').val(_company);
        $('#displayNameTextBox').val(_displayName);
        $('#phoneTextBox').val(_telephone);
        $('#emailTextBox').val(_email);
        $('#websiteTextBox').val(_website);
        $('#postalCodeTextBox').val(_postalCode);
        $('#streetTextBox').val(_street);
        $('#unitTextBox').val(_unit);
        $('#complementTextBox').val(_complement);
        $('#districtTextBox').val(_district);
        $('#cityTextBox').val(_city);
        my.vm.selectedRegion(_region);
    }

    $('#divMethod input[type=checkbox]').click(function (e) {
        if ($(this).val() === '2') {
            if ($(this).is(':checked')) {
                reqAddress(true);
                $('#postalCodeTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                $('label[for="postalCodeTextBox"]').addClass('required');
                $('#streetTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                $('label[for="streetTextBox"]').addClass('required');
                $('#districtTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                $('label[for="districtTextBox"]').addClass('required');
                $('#cityTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                $('label[for="cityTextBox"]').addClass('required');
                $('#regionTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                $('label[for="ddlRegions"]').addClass('required');
                validator = new ValidationUtility();
            } else {
                if (!_reqAddress) {
                    reqAddress(false);
                }
                $('#postalCodeTextBox').removeAttr('required');
                $('#postalCodeTextBox').removeAttr('data-role');
                $('label[for="portalCodeTextBox"]').removeClass('required');
                $('#streetTextBox').removeAttr('required');
                $('#streetTextBox').removeAttr('data-role');
                $('label[for="streetTextBox"]').removeClass('required');
                $('#districtTextBox').removeAttr('required');
                $('#districtTextBox').removeAttr('data-role');
                $('label[for="districtTextBox"]').removeClass('required');
                $('#cityTextBox').removeAttr('required');
                $('#cityTextBox').removeAttr('data-role');
                $('label[for="cityTextBox"]').removeClass('required');
                $('#regionTextBox').removeAttr('required');
                $('#regionTextBox').removeAttr('data-role');
                $('label[for="ddlRegions"]').removeClass('required');
            }
        } else if ($(this).val() === '1') {
            if ($(this).is(':checked')) {
                $('#emailTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
                $('label[for="emailTextBox"]').addClass('required');
                validator = new ValidationUtility();
            } else {
                $('#emailTextBox').removeAttr('required');
                $('#emailTextBox').removeAttr('data-role');
                $('label[for="emailTextBox"]').removeClass('required');
            }
        }

        //if ($('#divMethod input[type=checkbox]:checked').length === 0) {
        //    emailMethodType(true);
        //}
    });

    $('#listView').bind('click', function (e) {
        if ($('#listView input[type=checkbox]:checked').length === 0) {
            $('#emailTextBox').removeAttr('required');
            $('#emailTextBox').removeAttr('data-role');
            $('label[for="btnCheckEmail"]').removeClass('required');
        } else {
            $('#emailTextBox').attr({ 'required': 'required', 'data-role': 'validate' });
            $('label[for="btnCheckEmail"]').addClass('required');
            if ($('#divMethod input[type=checkbox]:checked').length === 0) {
                emailMethodType(true);
            }
            validator = new ValidationUtility();
        }
    });

    $('#phoneTextBox').inputmask("(99) 9999-9999");
    $('#postalCodeTextBox').inputmask("99-999-999");

    $(':required').one('blur keydown', function () {
        $(this).addClass('touched');
    });

    $('#btnSubmit').click(function (e) {
        e.preventDefault();
        $('#contactForm').wrap('<form id="temp_form_id" />');
        if (!validator.validate('#temp_form_id')) {
            $.pnotify({
                title: 'Atenção!',
                text: 'Favor preenchar todos os campos obrigatórios.',
                type: 'warning',
                icon: 'fa fa-warning fa-lg',
                addclass: "stack-bottomright",
                stack: my.stack_bottomright
            });
            $('#contactForm').unwrap();
        } else {

            var $this = $(this);
            $this.button('loading');

            var catalogs = [];

            var listview = $('#listView').data('kendoListView').dataSource.view();
            var checkboxes = $('#listView').find('input:checkbox:checked');
            var selected = $.map(checkboxes, function (item) {
                catalogs.push(item.id);
            });

            var converter = new Showdown.converter();

            $.ajax({
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                url: '/desktopmodules/riwcf/api/contactform/sendCatalogs',
                data: JSON.stringify({
                    Catalogs: catalogs,
                    PortalId: _portalID,
                    EmailMethodType: my.vm.emailMethodType(),
                    POMethodType: my.vm.poMethodType(),
                    Company: $('#companyTextBox').val(),
                    DisplayName: $('#displayNameTextBox').val(),
                    Telephone: $('#phoneTextBox').val().replace(/\D/g, ''),
                    Email: $('#emailTextBox').val(),
                    Website: $('#websiteTextBox').val(),
                    Message: converter.makeHtml($('#commentsTextArea').val()),
                    PostalCode: $('#postalCodeTextBox').val(),
                    Address: $('#streetTextBox').val(),
                    Unit: $('#unitTextBox').val(),
                    Complement: $('#complementTextBox').val(),
                    District: $('#districtTextBox').val(),
                    City: $('#cityTextBox').val(),
                    State: (my.vm.selectedRegion() ? my.vm.selectedRegion() : $('#regionTextBox').val()),
                    Country: my.vm.selectedCountry(),
                    TabModuleId: _tModuleID
                })
            }).done(function (data) {
                if (data.Result.indexOf("success") !== -1) {
                    $.pnotify({
                        title: 'Sucesso!',
                        text: 'Sua mensagem foi enviada.<br />Redirecionando para página inicial...',
                        type: 'success',
                        icon: 'fa fa-check fa-lg',
                        addclass: "stack-bottomright",
                        stack: my.stack_bottomright
                    });
                    setTimeout(function () {
                        document.location.href = 'http://' + _siteURL;
                    }, 5000);
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
            }).always(function () {
                $this.button('reset');
                $('#contactForm').unwrap();
            });
        }
    });

    var validator = new ValidationUtility();

    $('.markdown-editor').css({ 'min-width': '90%', 'height': '80px', 'margin-bottom': '5px' }).attr({ 'cols': '30', 'rows': '2' });

    //$('.msgHolder').css({ 'max-height': kendo.parseInt($('#estimateTabs-1').height() / 1.5) });

    $('.markdown-editor').autogrow();
    $('.markdown-editor').css('overflow', 'hidden').autogrow();

});