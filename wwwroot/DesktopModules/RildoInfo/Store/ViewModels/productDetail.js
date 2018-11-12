
$(function () {

    my.productId = my.getParameterByName('pid');

    if (my.productId !== 0) {
        $.ajax({
            url: '/desktopmodules/riw/api/products/getProduct?productId=' + my.productId
        }).done(function (data) {
            if (data) {
                if (data.ProdImagesId > 0) {
                    $('#a_img').addClass('photo').css({ 'float': 'left' }).attr({ 'href': '/databaseimages/' + data.ProdImagesId + '.' + data.Extension + '?maxwidth=800&amp;maxheight=600' });
                    $('#imgProduct').css({ 'margin': '10px' }).attr({ 'alt': data.ProdName, 'src': '/databaseimages/' + data.ProdImagesId + '.' + data.Extension + '?maxwidth=200&maxheight=200' });
                    //$(".photo").colorbox();
                    $('#a_img').click(function (event) {
                        event.preventDefault(); // this just cancels the default link behavior.
                        parent.showColorBox($(this).attr("href")); //this makes the parent window load the showColorBox function, using the a.colorbox href value
                    });
                }
                $('#productTitle').text(data.ProdName);
                if (data.ProdBarCode.length > 0) {
                    $('#label_Code').text(data.ProdBarCode);
                } else {
                    $('#label_Code').text(data.ProdRef);
                }
                $('#unitTypeAbbv').text(data.UnitTypeAbbv);
                $('#productIntro').text(data.ProdIntro);
                $('#productDesc').html(data.ProdDesc);
                $('#cat_links').html(data.Name);
            }
        });
    }

    my.openCat = function (value) {
        parent.$(".k-window-content").each(function () {
            parent.$(this).data("kendoWindow").close();
        });

        window.top.location.hash = '#catId/' + value;
        //parent.my.vm.quick(2);
        parent.my.loadProducts(value);
    };

});
