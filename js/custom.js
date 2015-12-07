var $container = $('#container').masonry({
    itemSelector: '.item',
    columnWidth: 300,
    transitionDuration: '0.8s',
    isResizeBound: true
});
var cssDisplay = { 'display': 'inline-block' };
var cssNoDisplay = { 'display': 'none' };
var myAccountMenu = $('#myAccount'),
        signInMenu = $('#signIn'),
        sessionId = $('#sessionId'),
        sessionUser = $('#sessionUser'),
        sessionUserId = $('#sessionUserId'),
        sessionOffline = $('#offlineSession'),
        callBackPrdUrl = $('#callBackPrdUrl'),
        signInMessage = $('#signin-message'),
        signUpMessage = $('#signup-message'),
        email = $('#contactEmail'),
        form = $('.cd-form');
var $form_modal = $('#menuPopup'),
     $form_login = $form_modal.find('#cd-login'),
     $form_signup = $form_modal.find('#cd-signup'),
     $form_forgot_password = $form_modal.find('#cd-reset-password'),
     $form_modal_tab = $('.cd-switcher'),
     $tab_login = $form_modal_tab.children('li').eq(0).children('a'),
     $tab_signup = $form_modal_tab.children('li').eq(1).children('a'),
     $forgot_password_link = $form_login.find('.cd-form-bottom-message a'),
     $back_to_login_link = $form_forgot_password.find('.cd-form-bottom-message a'),
     $main_nav = $('#signIn');

function login_selected() {
    $form_login.addClass('is-selected');
    $form_signup.removeClass('is-selected');
    $form_forgot_password.removeClass('is-selected');
    $tab_login.addClass('selected');
    $tab_signup.removeClass('selected');
}

function signup_selected() {
    $form_login.removeClass('is-selected');
    $form_signup.addClass('is-selected');
    $form_forgot_password.removeClass('is-selected');
    $tab_login.removeClass('selected');
    $tab_signup.addClass('selected');
}

function forgot_password_selected() {
    $form_login.removeClass('is-selected');
    $form_signup.removeClass('is-selected');
    $form_forgot_password.addClass('is-selected');
}

function serverCallRemoveToWishList(dataValue) {
    $.ajax(
   {
       type: "POST",
       url: resolveUrl("/service.aspx/RemoveFromWishList"),
       data: dataValue,
       contentType: "application/json; charset=utf-8",
       dataType: "json",
       async: false,
       cache: false,
       success: function (msg) {
           var item = "done";

       },
       error: function (x, e) {
           alert("The call to the server side failed. " + x.responseText);
       }
   }
);
}
function serverCallAddToWishList(dataValue) {
    $.ajax(
   {
       type: "POST",
       url: resolveUrl("/service.aspx/AddToWishList"),
       data: dataValue,
       contentType: "application/json; charset=utf-8",
       dataType: "json",
       async: false,
       cache: false,
       success: function (msg) {
           var item = "done";
       },
       error: function (x, e) {
           alert("The call to the server side failed. " + x.responseText);
       }
   }
);
}

function serverCallLoginUser(dataValue) {
    $.ajax(
   {
       type: "POST",
       url: resolveUrl("/service.aspx/Login"),
       data: dataValue,
       contentType: "application/json; charset=utf-8",
       dataType: "json",
       async: false,
       cache: false,
       success: function (msg) {
           var items = JSON.parse(msg.d);
           if (items["errorCode"] == 101) {
               $('#loader').show();
               setTimeout(function () {
                   $('#loader').hide();
                   $form_modal.removeClass('is-visible');
               }, 600);
               myAccountMenu.show();
               signInMenu.hide();
               console.log(items["email"]);
               sessionId.val(items["sessionId"]);
               sessionUser.val(items["username"]);
               sessionOffline.val(items["sessionId"]);
               email.val(items["email"]);
               doProcessOnAuthentication();
           }
           else {
               myAccountMenu.hide();
               signInMenu.show();
               signInMessage.text("Invalid Login Details");
           }
       },
       error: function (x, e) {
           alert("The call to the server side failed. " + x.responseText);
       }
   }
);
}

function serverCallRegisterUser(dataValue) {
    $.ajax(
   {
       type: "POST",
       url: resolveUrl("/service.aspx/Register"),
       data: dataValue,
       contentType: "application/json; charset=utf-8",
       dataType: "json",
       async: false,
       cache: false,
       success: function (msg) {
           var items = JSON.parse(msg.d);
           if (items["errorCode"] == 201) {
               $('#loader').show();
               setTimeout(function () {
                   $('#loader').hide();
                   $form_modal.removeClass('is-visible');
               }, 600);
               myAccountMenu.show();
               signInMenu.hide();
               sessionId.val(items["sessionId"]);
               sessionUser.val(items["username"]);
               sessionOffline.val(items["sessionId"]);
               email.val(items["email"]);
               doProcessOnAuthentication();
           }
           else {
               myAccountMenu.hide();
               signInMenu.show();
               signUpMessage.text(items["errorMessage"]);
           }
       },
       error: function (x, e) {
           alert("The call to the server side failed. " + x.responseText);
       }
   }
);
}

function serverCallChangePassword(dataValue) {
    $.ajax(
   {
       type: "POST",
       url: resolveUrl("/service.aspx/ForgotPassword"),
       data: dataValue,
       contentType: "application/json; charset=utf-8",
       dataType: "json",
       async: false,
       cache: false,
       success: function (msg) {
           var items = JSON.parse(msg.d);
           debugger;
           if (items["errorCode"] == 301) {
               $('#changePassword').val(items["errorMessage"]);
               signInMessage.text(items["errorMessage"]);
               login_selected();
           }
           else {
               $('#changePassword').text(items["errorMessage"]);
           }
       },
       error: function (x, e) {
           alert("The call to the server side failed. " + x.responseText);
       }
   }
);
}

function randomInt(min, max) {
    return Math.floor(Math.random() * max + min);
}



function getItems() {
    var items = '';
    for (var i = 0; i < 12; i++) {
        items += getItem();
    }
    // return jQuery object
    return $(items);
}

function getRandomProducts(url, dataValue) {
    var htmlCode = '';
    $.ajax(
        {
            type: "POST",
            url: resolveUrl("/service.aspx/" + url),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: dataValue,
            async: false,
            cache: false,
            success: function (msg) {
                var items = JSON.parse(msg.d);
                for (var i in items) {
                    var product = items[i];
                    htmlCode += MyHTML2(product);
                }

            },
            error: function (x, e) {
                alert("The call to the server side failed. " + x.responseText);
            }
        }
    );
    if (parseInt(localStorage.getItem("gcount")) % 3 == 0) {
        //if ($(".menu").length == 1) {
        //    htmlCode = localStorage.getItem("everyThing");
        //}
        //else {
        htmlCode += getBaseCategory();
        //}
    }
    return $(htmlCode);
}

function getProduct(url, dataValue, isSearch) {
    var htmlCode = '';
    $.ajax(
        {
            type: "POST",
            url: resolveUrl("/service.aspx/" + url),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: dataValue,
            async: false,
            cache: false,
            success: function (msg) {
                var items = JSON.parse(msg.d);
                if (msg.d == "[]") {
                    //alert(localStorage.getItem("gcount"));
                    CallMe("RandomProduct");
                    updateHeader("No product found in this category, but still we have some thing more for you :)");
                    return;
                    //$('#loader').hide();
                    //$('#myModal').modal({
                    //    show: 'true',
                    //    backdrop: 'static'
                    //});
                    //$(".ui-dialog-titlebar-close", $(this).parent()).hide();
                }
                else {
                    if (isSearch) {
                        //saved recentSearch in localstorage
                        var rsCode = '<a href=' + window.location + '>' + getParameterByName("s") + '</a>';
                        if (localStorage.getItem("recent") != null) {
                            if (localStorage.getItem("recent").toString().indexOf(getParameterByName("s")) <= 0) {
                                rsCode += localStorage.getItem("recent");
                                localStorage.setItem("recent", rsCode);
                            }
                        }
                        else {
                            localStorage.setItem("recent", rsCode);
                        }

                    }

                    for (var i in items) {
                        var product = items[i];
                        localStorage.setItem("pageIndex", product.pageNext);
                        localStorage.setItem("pageSize", product.pageSize);
                        if (url == "GetUserWishList") {
                            updateHeader("Product that you loved :)");
                            htmlCode += MyHTML2(product, "WISH");
                        }
                        else {
                            htmlCode += MyHTML2(product);
                        }
                    }
                    if (parseInt(localStorage.getItem("gcount")) % 3 == 0) {
                        //if ($(".menu").length == 1) {
                        //    htmlCode = localStorage.getItem("everyThing");
                        //}
                        //else {
                        htmlCode += getBaseCategory();
                        //}
                    }
                }

            },
            error: function (x, e) {
                window.location.href = "http://localhost:8080";
            }
        }
    );

    return $(htmlCode);
}


function getFullProduct(dataValue) {
    var htmlCode = '';
    $.ajax(
        {
            type: "POST",
            url: resolveUrl("/service.aspx/SingleProduct"),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: dataValue,
            async: false,
            cache: false,
            success: function (msg) {
                var items = JSON.parse(msg.d);
                for (var i in items) {
                    var product = items[i];

                    htmlCode += MyHTML3(product, "N");

                }
            }

            ,
            error: function (x, e) {
                alert("The call to the server side failed. " + x.responseText);
            }
        }
    );
    return $(htmlCode);
}

function MyHTML3() {
    return '<div id="first" class="item"><div class="row"><div class="col-lg-6"><img src="http://img6a.flixcart.com/image/watch/z/e/f/a1012-01-giordano-400x400-imadxv7vdgjyfnxg.jpeg" class="prdImage"/></div><div class="col-lg-6"><div class="row prdName clearfix">Product Name</div><br /><div class="row prdName clearfix">Descripton</div><br /><div class="row prdPrice clearfix"> 100 /-</div><br /><div class="row clearfix"><img src="images/shop Now.png" style="max-height:50px;" /></div><div class="row"><p>Share this with your friends and family</p><img src="images/icons.png" /> <p>product saved in your recent search.</p></div></div></div></div>';
}

function doProcessOnAuthentication()
{
    debugger;
    console.log($("#rechargeMe").val());
   
    if (callBackPrdUrl.val() != '') {
        if (callBackPrdUrl.val() == "RECHARGE") {
            goToPaymentGateway($("#rechargeMe").val());
        }
        else {
            window.open(callBackPrdUrl.val(), '_blank');
        }
    }

}

function showPopup(dataValue, display, dontOpen,recharge) {
    var url = resolveUrl("/tracking.aspx?id=" + dataValue);
    callBackPrdUrl.val(url);
    if (dataValue == null) {
        callBackPrdUrl.val('');
    }
    if (recharge != null)
    {
        $("#rechargeMe").val(recharge);
        callBackPrdUrl.val(dontOpen)
    }
    if (sessionId.val() != "") {
        debugger;
        //redirect to tracking page.
        if (dontOpen == null) {
            window.open(url, '_blank');
        }
        else if (dontOpen == "RECHARGE") {
            goToPaymentGateway($("#rechargeMe").val());
        }
        return true;
    }
    else {
        $form_modal.addClass('is-visible');
        login_selected();

        var msg = "Please login so that we can track your cashback. or <b><a href=" + url + " target='_bank'>you can skip and move to merchant site</a></b>";
        if (display != null) {
            msg = display;
        }
        signInMessage.html(msg);
        signUpMessage.html(msg);
        return false;
    }
}
function wishList(dataValue, display, dontOpen) {
    //alert("#" + dataValue);
    $("#" + dataValue).html("<i class='fa fa-heart'></i>");
    var wish = dataValue;
    if (localStorage.getItem("wish") != null) {
        wish = wish + "," + localStorage.getItem("wish");
    }
    localStorage.setItem("wish", wish);
    if (sessionId.val() != "") {
        //Speprate Each value from scritng
        debugger;
        wisharr = localStorage.getItem("wish").toString().split(",");

        //Call database logic 
        for (mywsh in wisharr) {
            var obj = '{'
                 + '"prdid" : "' + wisharr[mywsh] + '"'
                + '}';
            serverCallAddToWishList(obj);
        }
        debugger;
        //add all value of wish to database and removeItem from localstorage
        localStorage.removeItem("wish");
        return true;
    }
}

String.prototype.capitalizeFirstLetter = function () {
    return this.charAt(0).toUpperCase() + this.slice(1);
}

function MyHTML2(product, wishMe) {
    if (wishMe == null) {
        var str = "<div class='item'><a style='cursor:pointer' onclick='showPopup(\"" + product.UniqueId + "\");' >" +
            "<img src='" + product.Img + "' class='prd' onerror='imgError(this);'/></a><a href='" + resolveUrl("/" + product.SubCatId + "/" + product.SeoUrl) +
            "'><p>" + product.Name.substring(0, 35) + "..</p></a><p><span class ='pin'  onclick='showPopup(\"" + product.UniqueId +
            "\");'><i class='fa fa-opencart'></i></span><span class ='wish' id=" + product.UniqueId + " title='Add Product to wish list' onclick='wishList(\"" + product.UniqueId +
            "\");'><i class='fa fa-heart-o'></i></span><p><a href='" + resolveUrl("/!/" + product.SubCatId) + "'>" + product.SubCatId.capitalizeFirstLetter() +
            "</a> by <a href='" + resolveUrl("/" + product.Storeid) + "'>" + product.Storeid.capitalizeFirstLetter() + "</a> at " + product.OfferPrice +
            "</p></div>";
    }
    else {
        var str = WishList(product);
    }
    return str;
}

//<i class="fa fa-trash-o fa-fw"></i>
function DeleteWishList(dataValue) {
    $("#" + dataValue).html("Product Deleted :(");
    //$(".container").html("#" + dataValue);
    if (sessionId.val() != "") {

        var obj = '{'
             + '"prdid" : "' + dataValue + '"'
            + '}';
        serverCallRemoveToWishList(obj);
    }
}

function WishList(product) {
    var str = "<div  id=" + product.UniqueId + " class='item'><a style='cursor:pointer' onclick='showPopup(\"" + product.UniqueId + "\");' >" +
        "<img src='" + product.Img + "' class='prd' onerror='imgError(this);'/></a><a href='" + resolveUrl("/" + product.SubCatId + "/" + product.SeoUrl) +
        "'><p>" + product.Name.substring(0, 35) + "..</p></a><p><span class ='pin'  onclick='showPopup(\"" + product.UniqueId +
        "\");'><i class='fa fa-shopping-cart'></i></span><span class ='wish' id=" + product.UniqueId + " title='Add Product to wish list' onclick='DeleteWishList(\"" + product.UniqueId +
        "\");'><i class='fa fa-trash-o fa-fw'></i></span><p><a href='" + resolveUrl("/!/" + product.SubCatId) + "'>" + product.SubCatId.capitalizeFirstLetter() +
        "</a> by <a href='" + resolveUrl("/" + product.Storeid) + "'>" + product.Storeid.capitalizeFirstLetter() + "</a> at " + product.OfferPrice +
        "</p></div>";
    return str;
}

function imgError(image) {
    image.onerror = "";
    var hostName = window.location.host;
    image.src = resolveUrl("/no-product.png");
    return true;
}

function resolveUrl(serviceUrl) {
    var returnUrl = "http://" + window.location.host + serviceUrl;
    return returnUrl;
}

function goToPaymentGateway(obj) {
    $.ajax(
            {
                type: "POST",
                url: resolveUrl("/rechargeService.aspx/RequestPayUMoney"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: obj,
                async: false,
                cache: false,
                success: function (msg) {
                    debugger;
                    $("body").append(msg.d);
                },
                error: function (x, e) {
                    console.log("The call to the server side failed. " + x.responseText);
                }
            });
   // return callPay;
}

function serverCallGetProductWishListMark(dataValue) {
    var code = '';
    debugger;
    $.ajax(
   {
       type: "POST",
       url: resolveUrl("/service.aspx/GetUserWishList"),
       data: dataValue,
       contentType: "application/json; charset=utf-8",
       dataType: "json",
       async: false,
       cache: false,
       success: function (msg) {
           debugger;
           var items = JSON.parse(msg.d);
           for (var i in items) {
               alert(items[i]);
               var wish = items[i];
               code = wish.code;
           }
       },
       error: function (x, e) {
           alert("The call to the server side failed. " + x.responseText);
       }
   }
);
    return code;
}

function getBaseCategory(isDisplayAll) {
    var htmlCode2 = "<div class='item menu'><h4>Everything</h4><div class='row'>";
    var htmlCode33 = "";
    var isMenuCall;
    if (isDisplayAll == undefined) {
        isDisplayAll = 'Y';
        isMenuCall = false;
    }
    else {
        isMenuCall = true;
    }
    var obj = '{'
       + '"isAll" : "' + isDisplayAll + '"'
       + '}';
    $.ajax(
        {
            type: "POST",
            url: resolveUrl("/service.aspx/GetBaseCategory"),
            contentType: "application/json; charset=utf-8",
            data: obj,
            dataType: "json",
            async: false,
            cache: false,
            success: function (msg) {
                var items = JSON.parse(msg.d);
                debugger;
                for (var i in items) {
                    var category = items[i];
                    if (isMenuCall) {
                        htmlCode33 += MyCategryMenu(category);
                    }
                    else {
                        htmlCode2 += MyCategoryHTML(category);

                    }
                }
                htmlCode2 += "<img src ='" + resolveUrl("/images/px.png") + "'/></div></div>";
            },
            error: function (x, e) {
                alert("The call to the server side failed. " + x.responseText);
            }
        }
    );
    if (isMenuCall) {
        htmlCode2 = htmlCode33;
    }
    localStorage.setItem("everyThing", htmlCode2);
    return htmlCode2;
    //return "<div class='item menu'><img src ='"+resolveUrl("/images/px.png")+"'/><h4>Everything</h4><div class='row'><div class='col-sm-6'><a href='http://localhost:8080/!/brands'>Brands</a></div><div class='col-sm-6'><a href='http://localhost:8080/!/flower-n-gifting-ideas'>Flower & Gifts</a></div><div class='col-sm-6'><a href='http://localhost:8080/!/books-and-staitionary'>Books & Staitionary</a></div><div class='col-sm-6'><a href='http://localhost:8080/!/health-and-beauty'>Health & Beauty</a></div><div class='col-sm-6'><a href='http://localhost:8080/!/mobile-and-accessories'>Mobile & Tablets</a></div><div class='col-sm-6'><a href='http://localhost:8080/!/kids-and-baby-store'>Kids & Baby</a></div><div class='col-sm-6'><a href='http://localhost:8080/!/sports-and-fitness'>Sports & Fitness</a></div><div class='col-sm-6'><a href='http://localhost:8080/!/clothings'>Clothing</a></div><div class='col-sm-6'><a href='http://localhost:8080/!/camera-and-accessories'>Cameras & Accessories</a></div><div class='col-sm-6'><a href='http://localhost:8080/!/household-appliance'>Household Appliance</a></div><div class='col-sm-6'><a href='http://localhost:8080/!/footwears-fashion-for-means-womens'>Footwears</a></div><div class='col-sm-6'><a href='http://localhost:8080/!/jewellery-gold-sliver-daimonds'>Jewellery</a></div><div class='col-sm-6'><a href='http://localhost:8080/!/computer-and-laptops'>Computers & Laptops</a></div><div class='col-sm-6'><a href='http://localhost:8080/!/home-kitchen-decor'>Home & Kitchen Decor</a></div><div class='col-sm-6'><a href='http://localhost:8080/!/fashion-accessories'>Fashion Accessories</a></div><div class='col-sm-6'><a href='http://localhost:8080/!/travels-and-hotels'>Travels & Hotels</a></div><div class='col-sm-6'><a href='http://localhost:8080/!/electronics'>Electronics</a></div><div class='col-sm-6'><a href='http://localhost:8080/!/food-products'>Food Products</a></div></div></div>";

}

function MyCategoryHTML(Category) {
    return "<div class='col-sm-6'><a href='" + Category.CatUrl + "'>" + Category.CatName + "</a></div>";
}

function AllBrand(Store) {
    return "<div class='item col-sm-2'><a style='cursor:pointer' onclick='showPopup(\"" + Store.Website + "\");' ><img src=" + Store.Logo + " /></a></div>";
}
function MyStoreMenu(Store) {
    return "<a href='" + resolveUrl("/" + Store.SeoUrl) + "' title=" + Store.Storename + "><img src=" + Store.Logo + " /></a>";
}
function getBrandMenu(isMenu) {
    var menuCode = '';
    $.ajax(
        {
            type: "POST",
            url: resolveUrl("/service.aspx/GetAffiliatedStore"),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (msg) {
                var items = JSON.parse(msg.d);
                for (var i in items) {
                    var store = items[i];
                    if (isMenu) {
                        menuCode += MyStoreMenu(store);

                    }
                    else {
                        //code for brand page.
                        debugger;
                        menuCode += AllBrand(store);
                    }

                }


            },
            error: function (x, e) {
                alert("The call to the server side failed. " + x.responseText);
            }
        }
    );
    if (isMenu) {
        menuCode += "<a href='" + resolveUrl("/all/top/brands") + "'>View more..</a>";
    }
    return $(menuCode);
}

function MyCategryMenu(Category) {
    var catImage = resolveUrl("/catIcons/" + Category.CatImages);
    //var url = resolveUrl("/!/" + Category.CatAlias);
    return "<a href=" + Category.CatUrl + "><img class='icon' src=" + catImage + " /><span>" + Category.CatName + "</span></a>";
}


function getCategoryMenu(baseCategory) {
    debugger;
    var menuCat = "";
    var catName = "";
    if (baseCategory == undefined) {
        baseCategory = '';
    }
    var obj = '{'
      + '"baseCategory" : "' + baseCategory + '"'
      + '}';
    $.ajax(
        {
            type: "POST",
            url: resolveUrl("/service.aspx/GetSubCategory"),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: obj,
            async: false,
            cache: false,
            success: function (msg) {
                var items = JSON.parse(msg.d);
                if (msg.d == "[]") {
                    $("#sample").hide();
                } else {
                    for (var i in items) {
                        var cat = items[i];
                        catName += "," + cat.CatAlias;
                        menuCat += MyCategryMenu(cat);
                    }
                }
            },
            error: function (x, e) {
                alert("The call to the server side failed. " + x.responseText);
            }
        }
    );
    $("#subcat").val(catName);
    return $(menuCat);
}



function getCoupon(dataValue) {
    var cpnHtml = '';
    $.ajax(
        {
            type: "POST",
            url: resolveUrl("/service.aspx/GetCoupon"),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: dataValue,
            async: false,
            cache: false,
            success: function (msg) {
                var items = JSON.parse(msg.d);
                if (msg.d == "[]") {
                    //$("#sample").hide();
                } else {
                    for (var i in items) {
                        var cpn = items[i];
                        cpnHtml += CouponHTMLCode(cpn);
                    }
                }
            },
            error: function (x, e) {
                alert("The call to the server side failed. " + x.responseText);
            }
        }
    );
    return $(cpnHtml);
}

function CouponHTMLCode(cpn) {
    var tempStr = '<div class="item coupon"><a href=' + cpn.TrackingUrl + '>' +
            '<img src='+cpn.Product+' class="brdCpn" onerror="imgError(this);"></a>' +
            '<div class="code">' +
             '<i class="fa fa-scissors"></i><span>' + cpn.Code + '</span></div>' +
            '<a class= "couponBtm" href=' + cpn.TrackingUrl + '>' +
            '<p>' + cpn.Title + '</p></a><br/>' +
            '<br/><a href=' + cpn.TrackingUrl + '>' +
            '<span class="couponPin" onclick="showPopup(&quot;TSODFZKZFTJZBGZJ&quot;);"><i class="fa fa-opencart"></i></span>' +
            '<p class="status">' + cpn.Status + '</p></a></div>';
    return tempStr;
}

/********  Contact Us  **************/
function validateContact()
{
    var cemail = $("#contactEmail");
    var csub = $("#contactSubject");
    var cmsg = $("#contactMessage");
    var cemailerr = $("#contactEmailError");
    var csuberr = $("#contactSubjectError");
    var cmsgerr = $("#contactMessageError");
   
    count = 0;
    if (cemail.val() == "")
    {
        cemail.focus();
        cemailerr.css(cssDisplay);
        cemailerr.delay(2000).fadeOut(300);
        cemailerr.text("Enter required email-id");
        return false;
    }
    else if (!validateEmail(cemail.val())) {
        cemail.focus();
        cemailerr.css(cssDisplay);
        cemailerr.delay(2000).fadeOut(300);
        cemailerr.text("Enter valid email-id");
        return false;
    }
    else  if (csub.val() == "")
    {
        csuberr.focus();
        csuberr.css(cssDisplay);
        csuberr.delay(2000).fadeOut(300);
        csuberr.text("Enter your query");
        return false;
    }
    else if (cmsg.val() == "") {
        cmsgerr.css(cssDisplay);
        cmsgerr.focus();
        cmsgerr.delay(2000).fadeOut(300);
        cmsgerr.text("Enter your message");
        return false;
    }
    else {
        cemailerr.css(cssNoDisplay);
        csuberr.css(cssNoDisplay);
        cmsgerr.css(cssNoDisplay);
        return true;}
}

function ContactMe() {
    $("#contactBtn").click(function () {
        
        if (validateContact()) {
       var dataValue = '{'
         + '"rply"  : "' + $("#contactEmail").val() + '",'
         + '"sub"  : "' + $("#contactSubject").val() + '",'
         + '"msg"  : "' + $("#contactMessage").val() + '"'
        + '}';
       $.ajax(
       {
           type: "POST",
           url: resolveUrl("/service.aspx/CreateTicket"),
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           data: dataValue,
           async: false,
           cache: false,
           success: function (msg) {
               var items = JSON.parse(msg.d);
               $("#cmsg").text(items["errorMessage"]);
               $("#contactEmail").val("");
               $("#contactSubject").val("");
               $("#contactMessage").val("");
               setTimeout("$('#modal-Contact-Us').modal('hide')", 1000);
           },
           error: function (x, e) {
               alert("The call to the server side failed. " + x.responseText);
           }
       }
   );
        }
        else {
            $("#cmsg").text("Please fill the form correctly.");
        }
    });
}


/*Change Password*/
function ChangePassword()
{
    $("#changePassBtn").click(function () {
        if (validateContact()) {
            var dataValue = '{'
              + '"old"  : "' + $("#contactEmail").val() + '",'
              + '"new"  : "' + $("#contactSubject").val() + '"'
             + '}';
            $.ajax(
            {
                type: "POST",
                url: resolveUrl("/service.aspx/CreateTicket"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: dataValue,
                async: false,
                cache: false,
                success: function (msg) {
                    var items = JSON.parse(msg.d);
                    $("#cmsg").text(items["errorMessage"]);
                    $("#contactEmail").val("");
                    $("#contactSubject").val("");
                    $("#contactMessage").val("");
                    setTimeout("$('#modal-Contact-Us').modal('hide')", 1000);
                },
                error: function (x, e) {
                    alert("The call to the server side failed. " + x.responseText);
                }
            }
        );
        }
        else {
            $("#cmsg").text("Please fill the form correctly.");
        }
    });
    
}


/**********  OnLoad Functions Need to call   *************/
function OnLoadCall()
{
    ContactMe();
}


/**********  Common Validation Methods. *************/
function validateEmail($email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test($email);
}
function isNotEmpty(str) {
    var pattern = /\S+/;
    return pattern.test(str);  // returns a boolean
}
function isNumber(str) {
    var pattern = /^\d+$/;
    return pattern.test(str);  // returns a boolean
}
