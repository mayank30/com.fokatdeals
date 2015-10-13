using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace com.fokatdeals
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session[AppConstants.SESSION_UNIQUE_ID] = String.Empty;
            Session[AppConstants.SESSION_EMAIL_ID] = String.Empty;
            Session[AppConstants.SESSION_USER_TYPE] = String.Empty;
            Session[AppConstants.SESSION_USER_ID] = String.Empty;
            Session.RemoveAll();

            Response.Redirect(Properties.Settings.Default.Domain+"/");
        }
    }
}