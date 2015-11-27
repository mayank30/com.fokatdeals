<%@ Page Title="" Language="C#" MasterPageFile="~/Parent.Master" AutoEventWireup="true" CodeBehind="BaseCategory.aspx.cs" Inherits="com.fokatdeals.Category" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <div id="sample" class="CategoryMenu" style="background-color:#fff; margin-bottom:30px;padding-top:20px;">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="server">
        <input type ="hidden" id="category"/>
        <input type ="hidden" id="subcat"/>
    <script>
        var url = "GetProductByPagination";
        function CallMe(url) {
            pathArray = location.href.split('/');
            var cat = pathArray[5];
            if ($("#category").val() == "") {
                $("#category").val(cat);
            }
            $('#sample').html(getCategoryMenu($("#category").val()));
            //alert($("#subcat").val());
            $('#loader').show();
            if (localStorage.getItem("pageIndex") == null) {
                localStorage.setItem("pageIndex", 1);
            }
            var dataValue = '{'
                + '"index" : "' + localStorage.getItem("pageIndex") + '",'
                + '"value" : "' + $('#subcat').val() + '"'
             + '}';
            debugger;
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
