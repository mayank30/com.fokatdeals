﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Parent.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="com.fokatdeals.Category" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="server">
      <input type ="hidden" id="category"/>
    <script>
        var url = "GetProductByPagination";
        debugger;
        pathArray = location.href.split('/');
        function CallMe(url) {
            p = pathArray[4].split('-');
            $("#category").val(pathArray[4]);
            $('#loader').show();
            if (localStorage.getItem("pageIndex") == null) {
                localStorage.setItem("pageIndex", 1);
            }
            var dataValue = '{'
                + '"index" : "' + localStorage.getItem("pageIndex") + '",'
                + '"value" : "' + $('#category').val() + '"'
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
