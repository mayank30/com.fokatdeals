
var url = "RandomProduct";

function CallMe(url) {
    $('#loader').show();
    var $items = getRandomProducts(url);
    $items.hide();
    $container.append($items);
    $items.imagesLoaded().progress(function (imgLoad, image) {
        var $item = $(image.img).parents('.item');
        $item.show();
        $container.masonry('appended', $item);
        $('#loader').hide();
    });
}

