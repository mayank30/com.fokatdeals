using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PayUMoney;
using com.fokatdeals.recharge;
namespace com.fokatdeals
{
    public partial class paymentResponse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //You will get handlers Object .. do whatever you want to do .. 
                PayUMoneyHandler payU =  PayUService.ResponseFromPayUMoney(Request);
                if (Session[RechargeDAL.SESSION_AMT].ToString() == payU.AMOUNT.ToString() && Session[RechargeDAL.SESSION_REC_ORDER].ToString() == payU.ORDER_ID.ToString())
                {
                    orderId.Text = payU.ORDER_ID.ToString();
                    //Process recharge..
                }
                else 
                {
                    //Error message to user...
                }
            }
            catch (Exception ex)
            {
                Response.Write("<span style='color:red'>" + ex.Message + "</span>");
            }
        }
    }
}