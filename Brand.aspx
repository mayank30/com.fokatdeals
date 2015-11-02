<%@ Page Title="" Language="C#" MasterPageFile="~/Parent.Master" AutoEventWireup="true" CodeBehind="Brand.aspx.cs" Inherits="com.fokatdeals.Brand" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="server">
      <input type ="hidden" id="brand"/>
    <script>
        var url = "GetProductByPagination";
        debugger;
        pathArray = location.href.split('/');
        function CallMe(url) {
            $("#brand").val(pathArray[3]);
            $('#loader').show();
            if (localStorage.getItem("pageIndex") == null) {
                localStorage.setItem("pageIndex", 1);
            }
            var dataValue = '{'
                + '"index" : "' + localStorage.getItem("pageIndex") + '",'
                + '"value" : "' + $('#brand').val() + '"'
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