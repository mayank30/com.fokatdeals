var isScrollable = true;
var isDisplayProduct = true;
$(window).load(function () {
    localStorage.setItem("gcount", 3);
    localStorage.setItem("pageIndex", 1);
    localStorage.setItem("notfound", "false");
    updateHeader();
    if (isDisplayProduct) {
        CallMe(url);
    }
    else {
        $('#loader').hide();
    }
    if (sessionId.val() != "") {

        myAccountMenu.show();
        signInMenu.hide();
    }
    else {
        myAccountMenu.hide();
        signInMenu.show();
    }
    
    $("#topBrands").html(getBrandMenu(true));
    if (localStorage.getItem("recent") != null) {
        $("#recent").html(localStorage.getItem("recent"));
    }
    $("#catMenu").html(getBaseCategory('Y'));
    OnLoadCall();
});
$(window).scroll(function () {
    debugger;
    var scount = parseInt(localStorage.getItem("gcount"));
    scount = scount + 1;
    localStorage.setItem("gcount", scount);
    if (isScrollable) {
        if ($(window).scrollTop() >= ($(document).height() - $(window).height())) {
            CallMe(url);
        }
    }
    else {
        return;
    }
});
