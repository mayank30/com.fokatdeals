
$(window).load(function () {
    updateHeader();
    debugger;
    CallMe(url);
    if (sessionId.val() != "") {

        myAccountMenu.show();
        signInMenu.hide();
    }
    else {
        myAccountMenu.hide();
        signInMenu.show();
    }
    
    $("#topBrands").html(getBrandMenu(true));

});
$(window).scroll(function () {
    if ($(window).scrollTop() >= ($(document).height() - $(window).height())) {
        debugger;
        CallMe(url);
    }
});
