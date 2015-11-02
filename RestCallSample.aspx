<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RestCallSample.aspx.cs" Inherits="com.fokatdeals.RestCallSample" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>

<body>
    <form id="form1" runat="server">
    <div>
    <asp:dropdownlist ID="ddlApi" runat="server" DataSourceID="sdsCategory" DataTextField="subcatname" DataValueField="subcatdesc"></asp:dropdownlist>

        <asp:SqlDataSource ID="sdsCategory" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [subcatdesc], [subcatname] FROM [tbl_Subcategory]"></asp:SqlDataSource>
        <br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Get Data" />

    </div>
    </form>
</body>
</html>
