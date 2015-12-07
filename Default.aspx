<%@ Page Title="" Language="C#" MasterPageFile="~/Parent.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="com.fokatdeals.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div class="item" id="first">
         <img src="images/signup_promo.jpg" />
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="server">
    <script src='<%=ResolveUrl("js/customCalls.js")%>'></script>
</asp:Content>
