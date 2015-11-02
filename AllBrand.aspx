<%@ Page Title="" Language="C#" MasterPageFile="~/Parent.Master" AutoEventWireup="true" CodeBehind="AllBrand.aspx.cs" Inherits="com.fokatdeals.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="server">
    <script>
        var url = "RandomProduct";

        function CallMe(url) {
            var $items = getBrandMenu(false);
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
    </script>
</asp:Content>
