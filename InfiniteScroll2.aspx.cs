using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.fokatdeals.recharge;
using PayUMoney;
namespace com.fokatdeals
{
    public partial class InfiniteScroll2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //EmailUtil.CreateSupportTicket("mayank@kkk.com", "Sample Test Ticket", "Yes this is working ...");
            //com.fokatdeals.recharge.CommonDAL dal = new com.fokatdeals.recharge.CommonDAL();
            //Response.Write(dal.HttpGet(com.fokatdeals.recharge.AppConstant.GET_BALANCE_API + "?uid=9407393303&pwd=demo@123"));
            //Response.Write(com.fokatdeals.recharge.AppConstant.GET_ALL_OPERATORS());
            //Response.Write(dal.HttpGet(com.fokatdeals.recharge.AppConstant.GET_OPERATOR_API("8269")));
            PayUMoneyHandler p = new PayUMoneyHandler("PO00019391", "100", "Mayank", "mayankjhawar18@gmail.com", "2481355", "ProductInfo", "http://localhost:8080/paymentResponse.aspx", "http://localhost:8080/paymentResponseFail.aspx");
            Page.Controls.Add(new LiteralControl(PayUService.RequestToPayUMoney(p)));
        }
    }
}