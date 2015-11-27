<%@ Page Title="" Language="C#" MasterPageFile="~/Parent.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="com.fokatdeals.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div style="background:#fff; padding:20px;box-shadow:2px 2px 2px 2px #ececec;">
	<div class="row">
		<div class="col-md-12">
			<div class="tabbable" id="tabs-20043">
				<ul class="nav nav-tabs">
					<li class="active">
						<a href="#panel-home" data-toggle="tab">Home</a>
					</li>
					<li>
						<a href="#panel-earn" data-toggle="tab">Earning Report</a>
					</li>
                    <li>
						<a href="#panel-traffic" data-toggle="tab">Traffic Report</a>
					</li>
                    <li>
						<a href="#panel-recharge" data-toggle="tab">Recharge History</a>
					</li>
                     <li>
						<a href="#panel-ticket" data-toggle="tab">Issue Histroy</a>
					</li>
				</ul>
                <br />
				<div class="tab-content">
					<div class="tab-pane active" id="panel-home">
						<p>
							I'm in Section 1.
						</p>
					</div>
					<div class="tab-pane" id="panel-earn">
						<p>
							Howdy, I'm in Section 2.
						</p>
					</div>
                    <div class="tab-pane" id="panel-traffic">
						<p>
							Howdy, I'm in Section 2.
						</p>
					</div>
                    <div class="tab-pane" id="panel-recharge">
						<p>
							Howdy, I'm in Section 2.
						</p>
					</div>
                    <div class="tab-pane" id="panel-ticket">
						<p>
							Howdy, I'm in Section 2.
						</p>
					</div>
				</div>
			</div>
		</div>
	</div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScripts" runat="server">
    <script>
        var isScrollable = false;
        var isDisplayProduct = false;
        $(window).load(function () {
            updateHeader("Welcome Mayank,");
        });
    </script>
</asp:Content>
