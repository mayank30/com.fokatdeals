using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PayUMoney;
namespace com.fokatdeals
{
    public partial class paymentResponseFail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //You will get handlers Object .. do whatever you want to do .. 
                Response.Write(PayUService.ResponseFromPayUMoney(Request).toString());
            }
            catch (Exception ex)
            {
                Response.Write("<span style='color:red'>" + ex.Message + "</span>");
            }
        }
    }
}