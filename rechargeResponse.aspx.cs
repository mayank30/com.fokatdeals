using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace com.fokatdeals
{
    public partial class rechargeResponse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["reqid"] == null || Request.QueryString["status"] == null 
                    || Request.QueryString["remark"] == null
                    || Request.QueryString["mn"] == null
                    || Request.QueryString["field1"] == null 
                    || Request.QueryString["balance"] == null)
                {
                    Response.Write("");
                }
            }
            catch {
                Response.Redirect("");
            }
        }
    }
}