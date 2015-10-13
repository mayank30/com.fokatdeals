$(window).load(function () {
    myInit = true;
    $container = $('#basic');
    $container.imagesLoaded(function () {
        $container.masonry({
            itemSelector: '.item',
            isInitLayout: true,
            columnWidth: 10,
            isAnimated: true,
            animationOptions: {
                duration: 750,
                easing: 'linear',
                queue: false
            }
        });
    });
    getRandomProducts();
});
$(window).scroll(function () {
    if ($(window).scrollTop() + $(window).height() == $(document).height()) {
        $container = $('#basic');
        $container.imagesLoaded(function () {
            $container.masonry({
                itemSelector: '.item',
                isInitLayout: true,
                columnWidth: 10,
                isAnimated: true,
                animationOptions: {
                    duration: 750,
                    easing: 'linear',
                    queue: false
                }
            });
        });
        getRandomProducts();
    }
 
});

function getRandomProducts() {
    $('#loader').show();
    $.ajax(
        {
            type: "POST",
             url: resolveUrl("/service.aspx/SampleTest"),
            //data: returnDataValue(),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (msg) {
                    var items = JSON.parse(msg.d);
                    debugger;
                    for (var i in items) {
                        console.log(items[i].Name);
                        var product = items[i];
                        var htmlCode = MyHTML2(product);
                        if (myInit) {
                            myInit = false;
                            $container.append(htmlCode).imagesLoaded(function () {
                                $container.masonry('appended', htmlCode, true);
                            });
                        }
                        else {
                            $container.append(htmlCode).imagesLoaded(function () {
                                $container.masonry('reloadItems', htmlCode, true);
                            });
                        }
                        //$(htmlCode).appendTo(".masonry").delay(2000).hide().fadeIn(1000);
                        $(window).trigger('resize');
                    }
            },
            error: function (x, e) {
                alert("The call to the server side failed. " + x.responseText);
            }
        }
    );
}

function MyHTML(product)
{
    return $("<div class='item'><a href='" + product.PrdURL + "'><img class='lazy' src='" + product.Img + "' width="+product.Width+" height="+product.height+"/><h5>" + product.Name + "</h5></a></div>");
//<p><a href='#'>Category</a> by <a href='#'>Flipkart</a> at 100/-
// <a href='#'><img src='images/shop.png' width='25px' height='25px'/></a></p>
//</div>
}

function MyHTML2(product) {
    return $("<div class='item'><img src='" + product.Img + "' /><p>" + product.Name + "</p></div>");
}

function imgError(image) {
    image.onerror = "";
    var hostName = window.location.host;
    image.src = "http://" + hostName + "/no-product.png";
    return true;
}

function setImageBackground() {
    var element = document.getElementById("prdimg");
    var randomColor = Math.floor(Math.random() * 16777215).toString(16);
    element.style.background = randomColor;
}

function resolveUrl(serviceUrl) {
    var returnUrl = "http://" + window.location.host + serviceUrl;
    return returnUrl;
}

function returnDataValue() {
    var params = window.location.pathname.split('/').slice(1);
    var dataValue = "{value: ''}";
    if (params[0] === '') {
        //alert('no param' + params[0]);
        //Default Page Data
        dataValue = "{value: ''}";
    }
    else if (params[0] === '!') {
        // alert('brand : ' + params[1]);
        dataValue = '{value: "' + params[1] + '"}';
    }
    else {
        if (params[1] === undefined) {
            //   alert('caregory : ' + params[0]);
            dataValue = '{value: "' + params[0] + '"}';
        }
        else {
            // alert('caregory : ' + params[0] + " product name :-" + params[1]);
            //if (params[1] === '') {
            //  dataValue = '{value: "' + params[0] + '"}';
            //}
            //else {
            dataValue = '{value: "' + params[0] + '"}';
            //}
        }
    }
    return dataValue;
}