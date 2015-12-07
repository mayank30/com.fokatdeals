
function rechargeIt() {

    $('#tbMobile').click(function () {

    });

    $('#tbDth').click(function () {
        $("#dthOperator").html(getAllOperator("DTH"));
    });

    $('#tbDataCard').click(function () {
        $("#dataCardOperator").html(getAllOperator("DATA_CARD"));
    });


    i = 0;
    var rechargeTypePanel = $("#rechargeType");
    $("#mobileOperators").html(getAllOperator("PREPAID"));

    $('#optRadio input:radio').click(function () {
        if ($(this).val() === 'pre') {
            rechargeTypePanel.show();
            //Prepaid Operators
            $("#mobileOperators").html(getAllOperator("PREPAID"));
            if ($("#mobileNumber").val() != "") {
                setOpertorBasedOnMobileNumber();
            }

        } else if ($(this).val() === 'post') {
            rechargeTypePanel.fadeOut("slow");
            //Postpaid Operators
            $("#mobileOperators").html(getAllOperator("POSTPAID"));
            if ($("#mobileNumber").val() != "") {
                setOpertorBasedOnMobileNumber();
            }
            //console.log($("#mobileOperators").text());
        }
    });


    $("#mobileNumber").focusout(function () {
        if ($("#mobileNumber").val().length > 4) {
            if ($("#mobileOperator").val() == -1) {
                setOpertorBasedOnMobileNumber();
            }
        }
    })
    $("#mobileOperator").change(function () {
        localStorage.setItem("opCode", $("#mobileOperator").val());
    });
    $('#rechargeType input:radio').click(function () {
        if ($(this).val() === 'TOP') {
            localStorage.setItem("opCode", $("#mobileOperator").val());

        } else if ($(this).val() === 'SPECIAL') {
            jk = getOperatorCodeForSpecial("PREPAID_SPECIAL", $("#mobileOperator option:selected").text());
            if (jk == '') {
                localStorage.setItem("opCode", $("#mobileOperator").val());
            }
            else {
                localStorage.setItem("opCode", jk);
            }
        }
        //console.log($(this).val());
    });

    $('#btnMobileRecharge').click(function () {
        //callBackPrdUrl.val("RECHARGE");
        if (ValidatedRechargePanel()) {
            //Check if user is not logged in .
            preProceesRecharge($("#mobileNumber").val(), localStorage.getItem("opCode"), $("#rechargeAmt").val());
        }
        else {
            //error on panel
        }
    });

    $('#btnDth').click(function () {
        if (ValidateDTHPanel()) {
            preProceesRecharge($("#dthUserId").val(), $("#dthOperator").val(), $("#dthAmt").val());
        }
    });

    $('#dcBtn').click(function () {
        if (ValidateDCPanel()) {
            preProceesRecharge($("#dcUserId").val(), $("#dataCardOperator").val(), $("#dcAmt").val());
        }
    });

}

function preProceesRecharge(mn, op, amt) {
    var obj = '{'
            + '"mn" : "' + mn + '",'
            + '"op" : "' + op + '",'
            + '"amt" : "' + amt + '"'
               + '}';

    strg = goToPaymentGateway(obj);
    $("#rechargeMe").val(obj);
    showPopup(null, "Please login / register to proceed with your easy Recharge.", "RECHARGE", obj);
}

function ValidatedRechargePanel() {
    opErr = $("#optRadioErr");
    mnErr = $("#mnErr");
    moErr = $("#moErr");
    amtErr = $("#amtErr");
    rtErr = $("#rtErr");
    err = $("#rechargeErr");
    if ($('input[name=optradio]:checked').length <= 0) {
        err.show();
        err.css(cssDisplay);
        err.delay(2000).fadeOut(500);
        err.text("Please select your mobile type either Prepaid  / Postpaid.");
        return false;
    }
    else if (!isNotEmpty($("#mobileNumber").val())) {
        mnErr.css(cssDisplay);
        mnErr.delay(2000).fadeOut(300);
        mnErr.text("Enter your mobile number");
        return false;
    }
    else if (!isNumber($("#mobileNumber").val())) {
        debugger;
        mnErr.css(cssDisplay);
        mnErr.delay(2000).fadeOut(300);
        mnErr.text("Only number are allowed");
        return false;
    }
    else if ($("#mobileOperator").val() == -1) {
        err.show();
        err.delay(2000).fadeOut(500);
        err.text("Select mobile operator");
        return false;
    }
    else if (!isNotEmpty($("#rechargeAmt").val())) {
        amtErr.css(cssDisplay);
        amtErr.delay(2000).fadeOut(300);
        amtErr.text("Enter valid amount");
        return false;
    }
    else if (!isNumber($("#rechargeAmt").val())) {
        amtErr.css(cssDisplay);
        amtErr.delay(2000).fadeOut(300);
        amtErr.text("Invalid amount.");
        return false;
    }
    else if ($('input[name=rechargeType]:checked').length <= 0) {
        if ($('input[name=optradio]:checked').val() == 'pre') {
            err.show();
            err.css(cssDisplay);
            err.delay(2000).fadeOut(500);
            err.text("Please select your recharge choice.");
            return false;
        }
        else {
            return true;
        }
    }
    else {
        return true;
    }

}

function ValidateDTHPanel() {
    debugger;
    dthUserErr = $("#dthUserErr");
    dthAmtErr = $("#dthAmtErr");
    dthErr = $("#dthErr");
    if ($("#dthOperator").val() == -1) {
        dthErr.show();
        dthErr.delay(2000).fadeOut(500);
        dthErr.text("Select DTH Operator");
        return false;
    }
    else if (!isNotEmpty($("#dthUserId").val())) {
        dthUserErr.css(cssDisplay);
        dthUserErr.delay(2000).fadeOut(300);
        dthUserErr.text("Enter your consumer id");
        return false;
    }
    else if (!isNotEmpty($("#dthAmt").val())) {
        dthAmtErr.css(cssDisplay);
        dthAmtErr.delay(2000).fadeOut(300);
        dthAmtErr.text("Enter your bill amount");
        return false;
    }
    else if (!isNumber($("#dthAmt").val())) {
        dthAmtErr.css(cssDisplay);
        dthAmtErr.delay(2000).fadeOut(300);
        dthAmtErr.text("Only number are allowed");
        return false;
    }
    return true;
}

function ValidateDCPanel() {
    dcUserErr = $("#dcIdErr");
    dcAmtErr = $("#dcAmtErr");
    dcErr = $("#dcErr");
    if ($("#dataCardOperator").val() == -1) {
        dcErr.show();
        dcErr.delay(2000).fadeOut(500);
        dcErr.text("Select Data Card Operator");
        return false;
    }
    else if (!isNotEmpty($("#dcUserId").val())) {
        dcUserErr.css(cssDisplay);
        dcUserErr.delay(2000).fadeOut(300);
        dcUserErr.text("Enter your consumer id");
        return false;
    }
    else if (!isNotEmpty($("#dcAmt").val())) {
        dcAmtErr.css(cssDisplay);
        dcAmtErr.delay(2000).fadeOut(300);
        dcAmtErr.text("Enter your bill amount");
        return false;
    }
    else if (!isNumber($("#dcAmt").val())) {
        dcAmtErr.css(cssDisplay);
        dcAmtErr.delay(2000).fadeOut(300);
        dcAmtErr.text("Only number are allowed");
        return false;
    }
    return true;
}

function setOpertorBasedOnMobileNumber() {
    var obj = '{'
            + '"mn" : "' + $("#mobileNumber").val() + '"'
               + '}';
    value = getMobileOperator(obj);
    console.log($('#mobileOperator').find('option[value="' + value + '"]').length);
    if ($('#mobileOperator').find('option[value="' + value + '"]').length == 1) {
        $("#mobileOperator").val(value);
    }
    else {
        $("#mobileOperator").val(-1);
    }
}

function getMobileOperator(mn) {
    var str = '';
    $.ajax(
               {
                   type: "POST",
                   url: resolveUrl("/rechargeService.aspx/GetOperatorCode"),
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   data: mn,
                   async: false,
                   cache: false,
                   success: function (msg) {
                       str = msg.d;
                   },
                   error: function (x, e) {
                       console.log("The call to the server side failed. " + x.responseText);
                   }
               }
            );
    return str;

}

function getAllOperator(type) {
    widget = '<label class="image-replace cd-operator">Select Operators</label><select id="mobileOperator" class="full-width has-padding has-border"><option value=-1>--Select Operator--</option>';
    $.ajax(
    {
        type: "POST",
        url: resolveUrl("/rechargeService.aspx/GetAllOperatorCode"),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (msg) {
            var items = JSON.parse(msg.d);
            for (var i in items) {
                var ops = items[i];
                // alert(ops.name + " *** " + ops.code);
                if (type != "") {
                    if (ops.type == type) {
                        widget = widget + '<option value=' + ops.code + '>' + ops.name + '</option>';
                    }
                }
                else {
                    widget = widget + '<option value=' + ops.code + '>' + ops.name + '</option>';
                }
            }
            widget = widget + "</select>";
        },
        error: function (x, e) {
            alert("The call to the server side failed. " + x.responseText);
        }
    }
 );
    return widget;
}

function getOperatorCodeForSpecial(type, name) {
    opCode = '';
    $.ajax(
    {
        type: "POST",
        url: resolveUrl("/rechargeService.aspx/GetPrepaidOperators"),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (msg) {
            debugger;
            var items = JSON.parse(msg.d);
            for (var i in items) {
                var ops = items[i];
                if (ops.name == name) {
                    if (ops.type == type) {
                        opCode = ops.code;
                    }
                }
            }
        },
        error: function (x, e) {
            alert("The call to the server side failed. " + x.responseText);
        }
    }
 );
    return opCode;
}

