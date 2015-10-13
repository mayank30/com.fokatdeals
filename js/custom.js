var $container = $('#container').masonry({
    itemSelector: '.item',
    columnWidth: 300,
    transitionDuration: '0.8s',
    isResizeBound: true
});
var myAccountMenu = $('#myAccount'),
        signInMenu = $('#signIn'),
        sessionId = $('#sessionId'),
        sessionUser = $('#sessionUser'),
        signInMessage = $('#signin-message'),
        signUpMessage = $('#signup-message'),
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
           debugger;
           if (items["errorCode"] == 101) {
               $form_modal.removeClass('is-visible');
               myAccountMenu.show();
               signInMenu.hide();
               sessionId.val(items["sessionId"]);
               sessionUser.val(items["username"]);
               debugger;
               var obj = jQuery.parseJSON( dataValue );
               if (obj.data != "")
               {
                   window.location.href = 'www.w3schools.com';
               }
           }
           else {
               myAccountMenu.hide();
               signInMenu.show();
               form[0].reset();
               signInMessage.text(items["errorMessage"]);
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
           debugger;
           if (items["errorCode"] == 201) {
               $form_modal.removeClass('is-visible');
               myAccountMenu.show();
               signInMenu.hide();
               sessionId.val(items["sessionId"]);
               sessionUser.val(items["username"]);
           }
           else {
               myAccountMenu.hide();
               signInMenu.show();
               form[0].reset();
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
       url: resolveUrl("/service.aspx/ChangePassword"),
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
               form[0].reset();
               $('#changePassword').text(items["errorMessage"]);
           }
       },
       error: function (x, e) {
           alert("The call to the server side failed. " + x.responseText);
       }
   }
);
}


function CallMe(url) {
    $('#loader').show();
    var $items = getRandomProducts(url);
    // hide by default
    $items.hide();

    // append to container
    $container.append($items);
    $items.imagesLoaded().progress(function (imgLoad, image) {
        // get item
        // image is imagesLoaded class, not <img>
        // <img> is image.img
        var $item = $(image.img).parents('.item');
        // un-hide item
        $item.show();

        // masonry does its thing
        $container.masonry('appended', $item);
        $('#loader').hide();
    });
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

function getRandomProducts(url) {
    var htmlCode = '';
   
    $.ajax(
        {
            type: "POST",
            url: resolveUrl("/service.aspx/"+url),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (msg) {
                var items = JSON.parse(msg.d);
                debugger;
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
    return $(htmlCode);
}

function showPopup(dataValue) {
    var url = resolveUrl("/tracking.aspx?id=" + dataValue);
    if (sessionId.val() != "") {
        debugger;
        //redirect to tracking page.
        window.open(url,'_blank');
       // window.location.href = "tracking.aspx?id="+dataValue;
    }
    else {
        $form_modal.addClass('is-visible');
        login_selected();
        var msg = "Please login so that we can track your cashback. or <b><a href=" + url + " target='_bank'>you can skip and move to merchant site</a></b>";
        signInMessage.html(msg);
        signUpMessage.html(msg);
    }    
}

String.prototype.capitalizeFirstLetter = function () {
    return this.charAt(0).toUpperCase() + this.slice(1);
}

function MyHTML2(product) {
    debugger;

        //menu popup ...for login and saying please login before leave the page still want to move then skip...
    return "<div class='item'><a style='cursor:pointer' onclick='showPopup(\"" + product.UniqueId + "\");' ><img src='" + product.Img + "' class='prd'/></a><a href='"+resolveUrl("/"+ product.SubCatId + "/" + product.SeoUrl)+"'><p>" + product.Name.substring(0, 35) + "..</p></a><p><a href='"+resolveUrl("/!/"+product.SubCatId)+"'>" + product.SubCatId.capitalizeFirstLetter() + "</a> by <a href='"+resolveUrl("/"+product.Storeid)+"'>" + product.Storeid.capitalizeFirstLetter() + "</a> at " + product.OfferPrice + "</p></div>";
}
function resolveUrl(serviceUrl) {
    var returnUrl = "http://" + window.location.host + serviceUrl;
    return returnUrl;
}