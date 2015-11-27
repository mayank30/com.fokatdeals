using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace com.fokatdeals
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session[AppConstants.SESSION_USER_ID] == null)
                {
                    Response.Redirect("/");
                }
                else {
                
                }
            }
            catch
            {
                Response.Redirect("/");
            }
        }
    }
}