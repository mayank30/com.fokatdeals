<%@ Page Title="" Language="C#" MasterPageFile="~/Parent.Master" AutoEventWireup="true" CodeBehind="WishList.aspx.cs" Inherits="com.fokatdeals.WishList" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScripts" runat="server">
    <script>
        var url = "GetUserWishList";
        function CallMe(url) {
            debugger;   
            isScrollable = false;
            $('#loader').show();
            localStorage.setItem("pageIndex", 1);
            var dataValue = '{'
                + '"index" : "' + localStorage.getItem("pageIndex") + '"'
             + '}';
            var $items = getProduct(url, dataValue);
            $items.hide();
            $container.append($items);
            $items.imagesLoaded().progress(function (imgLoad, image) {
                var $item = $(image.img).parents('.item');
                $item.show();
                $container.masonry('appended', $item);
                $('#loader').hide();
            });
            
        }
       

    </script>
</asp:Content>
