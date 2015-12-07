
jQuery(document).ready(function ($) {
    //localStorage.name = "WishList";
    //$(document).on('click', '.pin', function (event) {
    //    event.preventDefault();
    //    debugger;
    //    $msg = "<p>Please login if you would like to add this product to wishlist.</p>";
    //    if (showPopup(this.id,$msg,"no")) {
    //        if ($(this).find('.fa-heart').length == 0) {
    //            //already added to 
    //            $(this).html('<i class="fa fa-heart"></i>');
    //            localStorage.setItem(this.id, '<i class="fa fa-heart"></i>');
    //            updateWishList(this.id);
    //        }
    //        else {
    //            //need to add into user wishlist
    //            $(this).html('<i class="fa fa-heart-o"></i>');
    //            localStorage.removeItem(this.id);
    //            removeWishList(this.id);
    //        }
    //        return false;
    //    }
    //    else {

    //    }
    //    //alert($("#ipAddress").val());
    //});
	//open modal
	$main_nav.on('click', function(event){

		if( $(event.target).is($main_nav) ) {
			// on mobile open the submenu
			$(this).children('ul').toggleClass('is-visible');
		} else {
			// on mobile close submenu
			$main_nav.children('ul').removeClass('is-visible');
			//show modal layer
			$form_modal.addClass('is-visible');	
			//show the selected form
			( $(event.target).is('.cd-signup') ) ? signup_selected() : login_selected();
		}
		signInMessage.html("");
		signUpMessage.html("");

	});

	//close modal
	$('#menuPopup').on('click', function (event) {
	    if ($(event.target).is($form_modal) || $(event.target).is('.cd-close-form')) {
	        $form_modal.removeClass('is-visible');
	        
		}	
	});
	//close modal when clicking the esc keyboard button
	$(document).keyup(function(event){
    	if(event.which=='27'){
    		$form_modal.removeClass('is-visible');
	    }
    });

	//switch from a tab to another
	$form_modal_tab.on('click', function(event) {
		event.preventDefault();
		( $(event.target).is( $tab_login ) ) ? login_selected() : signup_selected();
	});

	//hide or show password
	$('.hide-password').on('click', function(){
		var $this= $(this),
			$password_field = $this.prev('input');
		
		( 'password' == $password_field.attr('type') ) ? $password_field.attr('type', 'text') : $password_field.attr('type', 'password');
		( 'Hide' == $this.text() ) ? $this.text('Show') : $this.text('Hide');
		//focus and move cursor to the end of input field
		$password_field.putCursorAtEnd();
	});

	//show forgot-password form 
	$forgot_password_link.on('click', function(event){
		event.preventDefault();
		forgot_password_selected();
	});

	//back to login from the forgot-password form
	$back_to_login_link.on('click', function(event){
		event.preventDefault();
		login_selected();
	});

	

	//REMOVE THIS - it's just to show error messages 
	$form_login.find('input[type="submit"]').on('click', function(event){
	    event.preventDefault();
	    var username = $('#signin-email').val();
	    var password = $('#signin-password').val();
	    if (validateLogin(username,password)) {
	        var obj = '{'
            + '"username" : "' + username + '",'
             + '"password"  : "' + password + '",'
            + '"data"  : ""'
            + '}';
	        serverCallLoginUser(obj);
	    }
	});

	$form_signup.find('input[type="submit"]').on('click', function(event){
	    event.preventDefault();
	    var username = $('#signup-username').val();
	    var phone = $('#signup-password').val();
        var email = $('#signup-email').val();
        if (validateRegister(username, email, phone)) {
	        var obj = '{'
           + '"username" : "' + username + '",'
           + '"email"  : "' + email + '",'
           + '"phone"  : "' + phone + '",'
            + '"data"  : ""'
           + '}';
	        serverCallRegisterUser(obj);
	    }
	});
	$form_forgot_password.find('input[type="submit"]').on('click', function (event) {
	    event.preventDefault();
	    if (validateForgotPassword($('#reset-email').val())) {
	        var obj = '{'
           + '"email" : "' + $('#reset-email').val() + '"'
           + '}';
	        serverCallChangePassword(obj);
	    }
	    //$form_signup.find('input[type="email"]').toggleClass('has-error').next('span').toggleClass('is-visible');
	});

	if(!Modernizr.input.placeholder){
		$('[placeholder]').focus(function() {
			var input = $(this);
			if (input.val() == input.attr('placeholder')) {
				input.val('');
		  	}
		}).blur(function() {
		 	var input = $(this);
		  	if (input.val() == '' || input.val() == input.attr('placeholder')) {
				input.val(input.attr('placeholder'));
		  	}
		}).blur();
		$('[placeholder]').parents('form').submit(function() {
		  	$(this).find('[placeholder]').each(function() {
				var input = $(this);
				if (input.val() == input.attr('placeholder')) {
			 		input.val('');
				}
		  	})
		});
	}

});

jQuery.fn.putCursorAtEnd = function() {
	return this.each(function() {
    	// If this function exists...
    	if (this.setSelectionRange) {
      		// ... then use it (Doesn't work in IE)
      		// Double the length because Opera is inconsistent about whether a carriage return is one character or two. Sigh.
      		var len = $(this).val().length * 2;
      		this.setSelectionRange(len, len);
    	} else {
    		// ... otherwise replace the contents with itself
    		// (Doesn't work in Google Chrome)
      		$(this).val($(this).val());
    	}
	});
};

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}


function updateWishList(prdid)
{
    var obj = '{'
           + '"prdid"  : "' + prdid + '"'
          + '}';
    //localStorage.getItem(localStorage.key(i));
    serverCallAddToWishList(obj);
}

function removeWishList(prdid) {
    var obj = '{'
           + '"prdid"  : "' + prdid + '"'
          + '}';
    //localStorage.getItem(localStorage.key(i));
    serverCallRemoveToWishList(obj);
}

function updateHeader(text) {
    if (text == undefined) {
        pathArray = location.href.split('/');
        i = pathArray.length - 1;
        if (i == 3) {
            if (pathArray[3].split("=")[0] == "Search.aspx?s") {
                $("#headerTitle").append(pathArray[3].split("=")[1].capitalizeFirstLetter().replace("%20"," ").replace("+"," "));
            }
            else {
                $("#headerTitle").append(pathArray[i].capitalizeFirstLetter());
            }
        }
        else if (i >= 4) {
            if (pathArray[3] == "!") {
                if (pathArray[4] == "top") {
                    var txt = pathArray[5].capitalizeFirstLetter();
                    $("#headerTitle").append(txt);
                }
                else {
                    $("#headerTitle").append(pathArray[4].capitalizeFirstLetter());
                }
            } else {
                catTitle = '<a href=' + resolveUrl("/!/" + pathArray[3]) + '>' + pathArray[3].capitalizeFirstLetter() + '</a>'
                $("#headerTitle").append(catTitle + " / " + pathArray[4].substring(0, 35).capitalizeFirstLetter() + "...");
            }
        }
        // alert(i);
    }
    else {
        debugger;
        $("#headerTitle").html('<a href=' + resolveUrl("") + '>' + text + '</a>');
    }
}


function validateLogin(user,pass)
{
    var luser = $("#loginEmailErr");
    var lpass = $("#loginPassErr");
    if (user == "")
    {
        luser.css(cssDisplay);
        luser.delay(2000).fadeOut(300);
        luser.text("Enter your username or email-id");
        return false;
    }
    else if (pass == "") {
        lpass.css(cssDisplay);
        lpass.delay(2000).fadeOut(300);
        lpass.text("Enter valid password");
        return false;
    }
    else {
        return true;
    }
}


function validateRegister(user, email, phone) {
    debugger;
    var suser = $("#suUserErr");
    var semail = $("#suEmailErr");
    var sphone = $("#suPhoneErr");
    var scheck = $("#suCheck");
    signUpMessage.text('');
    if (user == "") {
        suser.css(cssDisplay);
        suser.delay(2000).fadeOut(300);
        suser.text("Enter required username");
        return false;
    }
    else if (email == "") {
        semail.css(cssDisplay);
        semail.delay(2000).fadeOut(300);
        semail.text("Enter required email address");
        return false;
    } else if (!validateEmail(email)) {
        semail.css(cssDisplay);
        semail.delay(2000).fadeOut(300);
        semail.text("Enter valid email-id");
        return false;
    } else if (phone == "") {
        sphone.css(cssDisplay);
        sphone.delay(2000).fadeOut(300);
        sphone.text("Enter required phone");
        return false;
    }
    else {
        return true;
    }
}

function validateForgotPassword(email)
{
    var fpemail = $("#fpEmailErr");
    if (email == "") {
        fpemail.css(cssDisplay);
        fpemail.delay(2000).fadeOut(300);
        fpemail.text("Enter required email address");
        return false;
    }  else {
        return true;
    }
}