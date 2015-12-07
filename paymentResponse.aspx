<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="paymentResponse.aspx.cs" Inherits="com.fokatdeals.paymentResponse" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <meta http-equiv="refresh" content="5;url=/" />
    <title>Payment Received | Fokatdeals.com</title>
    <link rel="shortcut icon" href="/themes/ck-storepage-v2/favicon.ico" />
    <link rel="stylesheet" href='<%=ResolveUrl("bootstrap/css/bootstrap.min.css")%>' />
    <link rel="stylesheet" href="css/trackme.css" />
</head>
<body>
    <form runat="server">
    <section class="intermediary divider">
        <div class="block">
            <div class="block_inner">
	<div class="row">
		<div class="col-md-12">
			<div>
				<h1>
					<span>Thank you for shopping with fokatdeals.com</span>
				</h1>
                <img src="images/logo.png" />
				<h2>
					<span>Order Id: <asp:Label ID="orderId" runat="server" Text="Label"></asp:Label></span>
				</h2>
				<p>
					We will process your recharge. you will receive balance soon.
				</p><br />
				<p>
					<a class="btn btn-primary btn-large" href="#">Go to home ..</a>
				</p>
                <p>
					&nbsp;</p>
                <p>
					&nbsp;</p>
			</div>
		</div>
	</div>

            </div>
        </div>
    </section>
        </form>
    <script src='<%=ResolveUrl("js/modernizr.js")%>'></script>
    <script type="text/javascript" src='<%=ResolveUrl("js/jquery.min.js")%>'></script>
    <script type="text/javascript" src='<%=ResolveUrl("bootstrap/js/bootstrap.min.js")%>'></script>

</body>
</html>
