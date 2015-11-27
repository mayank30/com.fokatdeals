<%@ Page Title="" Language="C#" MasterPageFile="~/Parent.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="com.fokatdeals.Search" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScripts" runat="server">
    <script src='<%=ResolveUrl("js/hilitor.js")%>'></script>
    <script>
        var url = "SearchProductByPagination";
        debugger;
        
        function CallMe(url) {
            var s = getParameterByName("s");
            $('#loader').show();
            localStorage.setItem("pageIndex", 1);
            var dataValue = '{'
                + '"index" : "' + localStorage.getItem("pageIndex") + '",'
                + '"value" : "' + s + '"'
             + '}';
            var $items = getProduct(url, dataValue,true);
            $items.hide();
            $container.append($items);
            $items.imagesLoaded().progress(function (imgLoad, image) {
                var $item = $(image.img).parents('.item');
                $item.show();
                $container.masonry('appended', $item);
                $('#loader').hide();
            });
            var myHilitor = new Hilitor("content");
            myHilitor.apply(s);
        }
       

    </script>
</asp:Content>
