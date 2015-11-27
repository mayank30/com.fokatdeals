<%@ Page Title="" Language="C#" MasterPageFile="~/Parent.Master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="com.fokatdeals.Product" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="first" class="item">
        <div class="row">
            <div class="col-lg-6 imageHolder">
                <img id="imageCall" src="" class="row prdImage" />
                 <div id="bottom" class="row">
                    <a id="fb" title="Share me on Facebook" target="_blank">
                        <img src='<%=ResolveUrl("images/social/Facebook.png")%>' /></a>
                    <a id="gplus" title="Share me on Google Plus" target="_blank">
                        <img src='<%=ResolveUrl("images/social/GooglePlus.png")%>' /></a>
                    <a id="linkin" title="Share me on you buisness network" target="_blank">
                        <img src='<%=ResolveUrl("images/social/LinkedIn.png")%>' /></a>
                    <a id="pin" title="Pin me on your board" target="_blank">
                        <img src='<%=ResolveUrl("images/social/Pin.png")%>' /></a>
                    <a id="tweet" title="Tweet me on twitter" target="_blank">
                        <img src='<%=ResolveUrl("images/social/Twitter.png")%>' /></a>
                    
                </div>
               
                

            </div>
            <div class="col-lg-6">
                 <div id="storeid" class="row clearfix">
                </div>
                <div id="name" class="row prdName clearfix">
                </div>
                <br />
                <div id="desc" class="row prdDesc clearfix">
                </div>
                <div id="price" class="row prdPrice clearfix">
                    <span id="oldPrice"></span>
                </div>
                <div class="row buyNow clearfix">
                </div>
                <br />
                <p>product saved in your recent search.</p>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="server">
    <input type="hidden" id="category" />
    <input type="hidden" id="prdid" />
    <script>
        var url = "GetProductByPagination";
        pathArray = location.href.split('/');
        p = pathArray[4].split('-');

        function getFullProduct(dataValue) {
            var htmlCode = '';
            var dataValue = '{'
               + '"value" : "' + p[p.length - 1] + '"'
            + '}';
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
                        debugger;
                        var items = JSON.parse(msg.d);
                        if (msg.d == "[]") {
                            $("#first").html('<div class="row"> <div class="col-lg-12"><img src=' + resolveUrl("/images/notfound.png") + '/></div></div>');
                            updateHeader("No such product found, but we have similar products for you :)");
                        }
                        else {
                            for (var i in items) {
                                var product = items[i];
                                debugger;
                                $("#imageCall").attr('src', product.Img);
                                $("#name").html(product.Name);
                                if (product.Status == "A") {
                                    product.Status = "In Stock";
                                }
                                else {
                                    product.Status = "Out Of Stock";
                                }
                                $("#desc").html(product.Status);
                                if (product.OfferPrice == product.RegularPrice) {
                                    $("#price").html(product.OfferPrice);

                                }
                                else {
                                    $("#price").prepend(product.OfferPrice);
                                    $("#oldPrice").append(product.RegularPrice);


                                }
                                var imageUrl = resolveUrl("/images/brand/" + product.Storeid + ".png");

                                $("#storeid").html("<img src='" + imageUrl + "' height=40px/>");
                                $(".buyNow").html("<a style='cursor:pointer' onclick='showPopup(\"" + product.UniqueId + "\");' ><img src=\"../images/shop Now.png\" style=\"max-height:40px;\" /></a>");
                                var url = window.location.href;
                                $('#fb').attr('href', 'http://www.facebook.com/sharer/sharer.php?u=' + url);
                                $('#gplus').attr('href', 'https://plus.google.com/share?url=' + url);
                                $('#linkin').attr('href', 'http://www.linkedin.com/shareArticle?url=' + url + '&title=' + product.Name);
                                $('#pin').attr('href', 'https://pinterest.com/pin/create/bookmarklet/?media=' + product.Img + '&url=' + url + '&is_video=false&description=' + product.Name);
                                $('#tweet').attr('href', 'https://twitter.com/share?url=' + url + '&text=' + product.Name + '&via=fokatdeals&hashtags=');

                            }
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

        function CallMe(url) {
            debugger;
            $("#category").val(pathArray[3]);
            if (localStorage.getItem("pageIndex") == null) {
                localStorage.setItem("pageIndex", 1);
            }
            var dataValue = '{'
                + '"index" : "' + localStorage.getItem("pageIndex") + '",'
                + '"value" : "' + $('#category').val() + '"'
             + '}';
            var $items = getProduct(url, dataValue);
            debugger;
            $items.hide();
            $container.append($items);
            $items.imagesLoaded().progress(function (imgLoad, image) {
                var $item = $(image.img).parents('.item');
                $item.show();
                $container.masonry('appended', $item);
                $('#loader').hide();
            });
        }

        $(window).load(function () {
            localStorage.setItem("pageIndex", 1);
            getFullProduct();
        });

    </script>
</asp:Content>
