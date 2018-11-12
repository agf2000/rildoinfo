
$(function () {

    //json data result = [{"id_img": 17,"img_filename": "ca0d8455-2702-4560-a444847429e36670.jpg"},{"id_img": 18,"img_filename": "eb0c6c77-fbd7-4f2c-bf22-10c874eefbf6.jpeg"},{"id_img": 19,"img_filename": "e7568c87-babb-4049-aed6-27b97866a817.png"}]
    var x = $.getJSON('/desktopmodules/riw/api/products/GetProducts', function (data) {
        $("#myCarousel").carousel("pause").removeData();
        var content_indi = "";
        var content_inner = "";
        $.each(data.data, function (i, obj) {
            var imgPath = ''
            var wMark = ''

            if (obj.watermark !== undefined || obj.watermark !== '') {
                wMark = obj.watermark
            } else {
                wMark = ''
            }
            if (obj.ProductImageId > 0) {
                imgPath = '/databaseimages/' + obj.ProductImageId + '.' + obj.Extension + '?maxwidth=800&maxheight=600&s.roundcorners=10&watermark=outglow&text=' + wMark;
            } else {
                imgPath = '/desktopmodules/rildoinfo/store/content/images/no-image.png'
            }
            
            content_indi += '<li data-target="#myCarousel" data-slide-to="' + i + '"></li>';
            content_inner += '<div class="item">' +
                            '<!-- Item image -->' +
                            '<div class="item-image">' +
                            '<a class="photo" title="' + obj.ProductName + '"' + (obj.ProductImageId > 0 ? ' href="' + imgPath + '" style="cursor: url(/desktopmodules/rildoinfo/webapi/content/images/zoomin.cur), pointer;">' : '>') +
                            '<img class="slimmage thumbnail" src="' + imgPath + '" />' +
                            '</a>' +
                            '</div>' +
                            '<!-- Item details -->' +
                            '<div class="item-details">' +
                            '<!-- Name -->' +
                            '<h5><a href="single-item.html">' + obj.ProductName + '</a></h5>' +
                            '<div class="clearfix"></div>' +
                            '<!-- Para. Note more than 2 lines. -->' +
                            '<p>' + obj.Summary + '</p>' +
                            '<hr>' +
                            '<!-- Price -->' +
                            '<div class="item-price pull-left">R$ ' + obj.UnitValue.toFixed(2) + '</div>' +
                            '<!-- Add to cart -->' +
                            (allowPurchase === 'true' ? 
                            '<div class="pull-right"><a href="#" class="btn btn-danger btn-sm">Comprar</a></div>'
                            :
                            '<div class="pull-right"><a href="#" class="btn btn-danger btn-sm">Orçar</a></div>') +
                            '<div class="clearfix"></div>' +
                            '</div>' +
                            '</div>';
        });
        $('#car_id').html(content_indi);
        $('#car_inner').html(content_inner);
        $('#car_inner .item').first().addClass('active');
        $('#car_indi > li').first().addClass('active');
        $('#myCarousel').carousel();
    });
});

