
$(function () {

    if (!ko.dataFor(document.getElementById('estimateMsg'))) {
        my.viewModel();
    }

    // Check for adding command in url string
    my.itemAdd = my.getStringParameterByName('add');

    // Allocate space in memory for the estimate id
    my.estimateId = null;

    // check for browser storage availability
    if (my.storage) {

        //// check for items in storage via amplify
        //if (amplify.store.sessionStorage(_siteURL + '_products')) {

        //    // convert item from stoage to json
        //    var products = ko.utils.parseJson(amplify.store.sessionStorage(_siteURL + '_products'));
        //    $.each(products, function (i, p) {
        //        my.vm.selectedProducts.push(new my.Product()
        //            .ProductId(p.ProductId)
        //            .productRef(p.productRef)
        //            .barcode(p.barcode)
        //            .productName(p.productName)
        //            .Summary(p.Summary)
        //            .unitValue(p.unitValue)
        //            .qTy(p.qTy)
        //            .totalValue(kendo.parseFloat(p.qTy) * p.unitValue));
        //    });
        //};

        // check if ko view model selectedProduct has anything
        if (amplify.store.sessionStorage(_siteURL + '_products')) {
            $('.divButtons').show().css('display', 'inline-block');
            $('#divCheckExpand').show();
        //} else {
        //    sessionStorage.clear();
        }

        // check estimate expanded checkbox in storage via emplify
        if (amplify.store.sessionStorage(_siteURL + '_expandEstimate')) {

            // set checkbox as checked if true
            $('#divCheckExpand input').attr({ 'checked': 'checked' });
            $('#divCheckExpand label span').html('Manter o orçamento expandido.');
            $('#estimateItems').slideDown();
            //$('#divCheckExpand').delay(400).show();
            //$('#btnExpandCart').html('<span class="fa fa-chevron-up"></span> Esconder Carrinho');
        } else {
            $('#divCheckExpand label span').html('Expandir e manter o orçamento expandido.');
        }
    }

    //// if cart is empty, set button to show cart's state to disabled
    //if (!my.vm.selectedProducts().length > 0) {
    //    $('#btnExpandCart').addClass('k-state-disabled');
    //};

    // if check checkbox for keeping estimated expanded is true, show estimate
    //if ($('#divCheckExpand input').is(':checked')) {

    //    // show estimate items
    //    $('#estimateItems').slideDown();

    //    //// show expand estimate checkbox area
    //    //$('#divCheckExpand').show();

    //    //// set expand estimate button text hide estimate
    //    //$('#btnExpandCart').html('<span class="fa fa-chevron-up"></span> Esconder Carrinho');
    //}

    //// expand / collapse estimate area
    //$('#btnExpandCart').click(function (e) {
    //    e.preventDefault();

    //    // check if estimate items are hidden
    //    if (!$('#estimateItems').is(':hidden')) {

    //        // if estimate items are not hidden, hide it
    //        $('#estimateItems').kendoAnimate({ effects: 'slide:up fade:out', hide: true });

    //        // if estimate items are hidden, set expand estimate items button's text to show cart
    //        $('#btnExpandCart').html('<span class="k-icon k-i-arrow-s"></span> Visualizar Carrinho');

    //        // if estimate items are not hidden, hide checkbox for keeping estimate items expanded
    //        $('#divCheckExpand').delay(400).hide();

    //        // if checkbox for keeping estimate items expanded is not true in storage, make sure to uncheck the checkbox
    //        $('#divCheckExpand input').removeAttr('checked');

    //        // set checkbox for keeping estimate items expanded to false in storage
    //        amplify.store.sessionStorage(_siteURL + '_expandEstimate', false);
    //    } else {

    //        // if estimate items are hidden, show it
    //        $('#estimateItems').kendoAnimate({ effects: 'slide:down fade:in', show: true });

    //        // if estimate items are not hidden, set expand estimate items button's text to hide cart
    //        $('#btnExpandCart').html('<span class="fa fa-chevron-up"></span> Esconder Carrinho');

    //        // if estimate items are hidden, show checkbox that keeps estimate items expanded
    //        $('#divCheckExpand').delay(400).show();
    //    }
    //});

    // on checkbox check event checked or unchecked to keep or not to keep estimate items expanded
    $('#divCheckExpand input').click(function () {

        // check if this checkbox check then
        if ($(this).is(':checked')) {

            // if estimate items are not hidden, hide it
            $('#estimateItems').slideDown();

            $('#divCheckExpand label span').html('Manter o orçamento expandido.');

            //// if estimate items are hidden, set expand estimate items button's text to show cart
            //$('#btnExpandCart').html('<span class="k-icon k-i-arrow-s"></span> Visualizar Carrinho');

            //// if estimate items are not hidden, hide checkbox for keeping estimate items expanded
            //$('#divCheckExpand').delay(400).hide();

            // if this checkbox is checked then add true to storage
            if (my.storage) amplify.store.sessionStorage(_siteURL + '_expandEstimate', true);
        } else {

            // if estimate items are hidden, show it
            $('#estimateItems').slideUp();

            //// if estimate items are not hidden, set expand estimate items button's text to hide cart
            //$('#btnExpandCart').html('<span class="fa fa-chevron-up"></span> Esconder Carrinho');

            //// if estimate items are hidden, show checkbox that keeps estimate items expanded
            //$('#divCheckExpand').delay(400).show();

            // if checkbox for keeping estimate items expanded is not true in storage, make sure to uncheck the checkbox
            $('#divCheckExpand input').removeAttr('checked');

            $('#divCheckExpand label span').html('Expandir e manter o orçamento expandido.');

            // else if this checkbox is not checked then add false to storage
            if (my.storage) amplify.store.sessionStorage(_siteURL + '_expandEstimate', false);
        }
    });

    // check if user is logged in
    if (_userID > 0) {

        // if yes, set estimate button text to send estimate
        $('#btnSaveEstimate').html('<span class="fa fa-check"></span>&nbsp; Requisitar Orçamento');
    //} else {

    //    // if not, set estimate button text to please login
    //    $('#btnSaveEstimate').html('<span class="fa fa-sign-in"></span>&nbsp; Login ou Criar Conta');
    }

    // send and save estimate button event
    $('#btnSaveEstimate').click(function (e) {
        if (e.clientX === 0) {
            return false;
        }
        e.preventDefault();

        // disabled button momentarilly
        var $this = $(this);
        $this.button('loading');
        //$('#btnSaveEstimate').addClass('k-state-disabled').html('<i class="fa fa-spinner fa-spin"></i>&nbsp; Um momento...').attr({ 'disabled': true });

        // make sure the user is still logged in
        if (_userID > 0) {

            // initiate progress effect
            kendo.ui.progress($("#estimateItems"), true);

            // fill estimate parameters
            var estimate = {
                PortalId: _portalID,
                EstimateId: 0,
                EstimateTitle: '',
                UserId: _userID,
                Guid: my.Right(my.generateUUID(), 8),
                SalesRep: _salesPerson,
                ViewPrice: _viewPrice,
                TotalAmount: my.vm.extendedPrice(),
                CreatedByUser: _userID,
                CreatedOnDate: moment().format(),
                ModifiedByUser: _userID,
                ModifiedOnDate: moment().format(),
            };

            // ajax call to server to save estimate
            $.ajax({
                async: false,
                type: 'POST',
                url: '/desktopmodules/riw/api/estimates/addEstimate',
                data: estimate
            }).done(function (data1) {
                if (data1.Result.indexOf("success") !== -1) {
                    my.estimateId = data1.EstimateId;

                    var estimateItems = [];
                    $.each(my.vm.selectedProducts(), function (i, item) {
                        estimateItems.push({
                            EstimateId: my.estimateId,
                            ProductId: item.productId(),
                            ProductQty: item.qTy(),
                            ProductName: item.productName(),
                            ProductEstimateOriginalPrice: item.unitValue(),
                            ProductEstimatePrice: item.unitValue(),
                            CreatedByUser: _userID,
                            CreatedOnDate: moment().format(),
                            ModifiedByUser: _userID,
                            ModifiedOnDate: moment().format()
                        });
                    });

                    $.ajax({
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        url: '/desktopmodules/riw/api/estimates/addEstimateItems',
                        data: JSON.stringify({
                            EstimateItems: estimateItems,
                            PortalId: _portalID,
                            EstimateId: my.estimateId,
                            TotalAmount: my.vm.extendedPrice(),
                            ModifiedByUser: _userID,
                            ModifiedOnDate: moment().format()
                        })
                    }).done(function (data2) {
                        if (data2.Result.indexOf("success") !== -1) {

                            //my.vm.subject(_estimateSubject.replace('[WEBSITE]', _siteName));

                            var estimateLink = '[link](' + _estimateURL + '#estimateId/' + my.estimateId + ')' // '[link] (' + _estimateURL + '?eid=' + my.eId + ' "Clique para acessar o orçamento")';
                            var _body = _estimateBody.replace('[ORCAMENTOLINK]', '<a href="' + estimateLink + '">link</a>');
                            _body = _body.replace('[CLIENTE]', my.vm.displayName());
                            _body = _body.replace('[ID]', my.estimateId);
                            //my.vm.message(_body);

                            var converter = new Showdown.converter();
                            var emailHtmlContent = converter.makeHtml(_body.trim());

                            // fill estimate email parameters
                            var params = {
                                PortalId: _portalID,
                                ToEmail: my.vm.emailTo(),
                                Subject: _estimateSubject.replace('[WEBSITELINK]', _siteName),
                                MesssageBody: emailHtmlContent,
                                SalesPersonId: _salesPerson,
                                UserId: _userID,
                                EstimateId: my.estimateId,
                                ModifiedOnDate: moment().format()
                                //ConnId: my.hub.connection.id,
                                //Watermark: _watermark
                            };

                            $.ajax({
                                type: 'POST',
                                url: '/desktopmodules/riw/api/estimates/sendEstimateNotification',
                                data: params
                            }).done(function (data3) {
                                //if (data3.Result.indexOf("success") !== -1) {
                                //}
                            }).fail(function (jqXHR, textStatus) {
                                console.log(jqXHR.responseText);
                            });

                            if (amplify.store(_siteURL + 'estimateMessage')) {
                                document.location.href = _estimateURL + '#estimateId/' + my.estimateId + '/new/1';
                            } else {
                                // prompt user for redirection to estimate page
                                var $dialog = $('#dialog-message')
                                    .dialog({
                                        autoOpen: false,
                                        modal: true,
                                        resizable: false,
                                        dialogClass: 'dnnFormPopup',
                                        title: 'Mensagem',
                                        width: 360,
                                        buttons: {
                                            'ok': {
                                                text: 'Continuar',
                                                //priority: 'primary',
                                                "class": 'btn btn-primary',
                                                click: function (e) {
                                                    // on ok, close dialog and do the redirection
                                                    document.location.href = _estimateURL + '#estimateId/' + my.estimateId + '/new/1';
                                                }
                                            }
                                        },
                                        close: function () {
                                            my.loadProducts(0);
                                        }
                                    });

                                $dialog.dialog('open');
                            }
                        } else {
                            //$().toastmessage('showErrorToast', data.Result);
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
                } else {
                    //$().toastmessage('showErrorToast', msg.Result);
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

                // initiate progress effect
                kendo.ui.progress($("#estimateItems"), false);
                
                $('#btnCancelEstimate').click();
            });
        } else {

            // if user is no longer logged in, do a redirection
            // set the return url
            var uri = "/login?returnurl=" + my.encodeSlash(window.location.pathname);

            // save estimate items expanded 
            if (my.storage) amplify.store.sessionStorage(_siteURL + '_expandEstimate', true);

            // do the redirection
            document.location.href = uri;
        }
    });

    // call authorization check function
    my.authorizationCheck();

    $('#dialog-message input').click(function (e) {
        //if (my.storage) amplify.store.sessionStorage(_siteURL + '_expandEstimate', true);
        if (my.storage) amplify.store(_siteURL + 'estimateMessage', true);
    });

    // reset and cancel on going estimate
    $('#btnCancelEstimate').click(function (e) {
        e.preventDefault();
        sessionStorage.clear();
        my.vm.selectedProducts([]);
        my.vm.estimateItems([]);
        $('.divButtons').fadeOut();
        my.loadProducts(0);
        $('#estimateItems').slideUp();
        //$('#btnExpandCart').html('<span class="k-icon k-i-arrow-s"></span> Visualizar Carrinho').addClass('k-state-disabled');
        //$('#divCheckExpand').delay(400).kendoAnimate({ effects: 'slide:up fade:out', hide: true });
        $('#divCheckExpand input').removeAttr('checked');
        history.pushState("", document.title, window.location.pathname);
    });

    $('#configLink').click(function (e) {
        e.preventDefault();

        document.location.href = _configURL;
    });

});