<script type="text/javascript">
$(function () {

    $('#vitrineSlider').owlCarousel({
        autoPlay: 3000,
        stopOnHover: true,
        pagination: false,
        navigation: true,
        navigationText: ['anterior', 'próximo'],
        jsonPath: '/desktopmodules/riw/api/products/getProducts?featuredOnly=true',
        jsonSuccess: customDataSuccess,
        items: 2
    });

    function customDataSuccess(data) {
        var content = '';
        for (var i in data['data']) {

            var id = data['data'][i].ProductId;
            var img = data['data'][i].ProductImageId;
            var ext = data['data'][i].Extension;
            var alt = data['data'][i].ProductName;
            var intro = data['data'][i].Summary !== data['data'][i].ProductName ? data['data'][i].Summary : '';

            content += '<div class="item">'
            //content += "<h1>" + alt + "</h1><img src='/databaseimages/" + img + ".jpg' alt='" + alt + "' />"
            content += '<div id="divSale">';
            content += '<div class="title">' + alt + '</div>';
            content += '<div class="productImg">';
            content += '<a title="' + alt + '" class="photo" href="/databaseimages/' + img + '.jpg?maxwidth=800&maxheight=600&s.roundcorners=6&watermark=outglow&text=Cor e Tinta&404=myPreset" style="cursor:url(/DesktopModules/RildoInfo/WebAPI/Content/images/zoomin.cur), pointer;">';
            content += '<img src="/databaseimages/' + img + '.' + ext + '?width=160&height=160&s.roundcorners=6&watermark=outglow&text=Cor e Tinta" align="absmiddle" />';
            content += '</a>';
            content += '</div>';
            content += '<div class="intro">' + intro + '</div>';
            content += '<button onclick="javascript:openlink(' + id.toString() + ')">Saiba Mais</button>';
            content += "</div>"
            content += "</div>"
        }
        $("#vitrineSlider").html(content);
    };

    function openlink(id)
    {
        window.open("/produtos#item" + id, "_blank");
    }

});
</script>