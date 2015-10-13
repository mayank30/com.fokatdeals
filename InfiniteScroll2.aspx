<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InfiniteScroll2.aspx.cs" Inherits="com.fokatdeals.InfiniteScroll2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    	<link rel="shortcut icon" type="image/png" href="images/favicon.png"/>
    <link rel="stylesheet" href="bootstrap/css/bootstrap.min.css"/>
    <link rel="stylesheet" href="font-awesome/css/font-awesome.min.css"/>
    <link rel="stylesheet" type="text/css" media="all" href="css/massonary.css"/>
    <link rel="stylesheet" type="text/css" media="all" href="css/footer-style.css"/>
    <link rel="stylesheet" type="text/css" media="all" href="css/menu.css"/>
    <link rel="stylesheet" type="text/css" media="all" href="css/header.css"/>
    <style>
 
/***** Loader *****/
        #first {
            width:590px;
            height:400px;
            background:#fff;
            padding:20px;
            color:#d2d2d2;
        }
            #first .prdImage {
            
                max-height:360px;
                margin-left:30px;
            }
     
        .prdName {
            font-size:14px;
        }
        .prdPrice {
            font-size:36px;
            padding:20px;
            color: #ef4444;
        }
    </style>
</head>
<body>
    <div id="loader">
		<div class="sk-spinner sk-spinner-pulse"></div>
	</div>
<div id="container">
    <div id="first" class="item">
        <div class="row">
            <div class="col-lg-6"><img src="http://img6a.flixcart.com/image/watch/z/e/f/a1012-01-giordano-400x400-imadxv7vdgjyfnxg.jpeg" class="prdImage"/></div>
            <div class="col-lg-6">
                <div class="row prdName clearfix">
                    DKNY NY8376 Analog Watch - For Wome Category 376 Analog Watch - For Wome Category
                </div>
                <br />
                <div class="row prdName clearfix">
                    DKNY NY8376 Analog Watch - For Wome Category 376 Analog Watch - For Wome Category
                </div>
                <br />
                <div class="row prdPrice clearfix">
                    100 /-
                </div>
                <br />
                <div class="row clearfix">
                    <img src="images/shop Now.png" style="max-height:50px;" />
                </div>
               
                <div class="row">
                     <p>Share this with your friends and family</p>
                   <img src="images/icons.png" />
                    <p>product saved in your recent search.</p>
                </div>
                

            </div>
        </div>
    </div>
</div>
    
<!-- java Script Section Start -->
	<script type="text/javascript" src="js/jquery.min.js"></script>
    <script type="text/javascript" src="bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="js/scrollTop.js"></script>
	<script type="text/javascript" src="js/webslidemenu.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/masonry/3.3.2/masonry.pkgd.js"></script>
    <script src="http://imagesloaded.desandro.com/imagesloaded.pkgd.js"></script>
<%--<script type="text/javascript" src="js/jquery.lazy.js"></script>--%>
<!-- java Script Section End -->
    <script>
            var $container = $('#container').masonry({
                itemSelector: '.item',
                columnWidth: 300,
                transitionDuration: '0.8s',
                isResizeBound: true
            });
            $('#first').hide();
            $(window).load(function () {
                
                CallMe();
            });
            $(window).scroll(function () {
                debugger;
               
                if ($(window).scrollTop() >= ($(document).height() - $(window).height()) * 0.9) {
                    CallMe();
                   
                }
            });

        function CallMe()
            {
            $('#loader').show();
            var $items = getRandomProducts();
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
             $('#first').fadeIn(3000);
                // masonry does its thing
                $container.masonry('appended', $item);
                $('#loader').hide();
            });
        }

        function randomInt(min, max) {
            return Math.floor(Math.random() * max + min);
        }

        function getItem() {
            var width = randomInt(150, 400);
            var height = randomInt(150, 250);
            var item = '<div class="item">' +
              '<img src="http://lorempixel.com/' +
              width + '/' + height + '/nature" /></div>';
            return item;
        }

        function getItems() {
            var items = '';
            for (var i = 0; i < 12; i++) {
                items += getItem();
            }
            // return jQuery object
            return $(items);
        }

        function getRandomProducts() {
            var htmlCode = '';
            $.ajax(
                {
                    type: "POST",
                    url: resolveUrl("/service.aspx/SampleTest"),
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

        function MyHTML2(product) {
            return "<div class='item'><a href='#redirectToClientPage'><img src='" + product.Img + "' class='prd'/></a><a href='#redirectToProductPage'><p>" + product.Name.substring(0, 35) + "..</p></a><p><a href='#'>Category</a> by <a href='#'>Flipkart</a> at 100/-</p></div>";
        }
        function resolveUrl(serviceUrl) {
            var returnUrl = "http://" + window.location.host + serviceUrl;
            return returnUrl;
        }
    </script>
</body>
</html>
