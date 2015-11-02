using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace com.fokatdeals
{
    public partial class tracking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[AppConstants.SESSION_UNIQUE_ID] != null)
            {
                Response.Write(Session[AppConstants.SESSION_UNIQUE_ID].ToString()+"  *****  "+Session[AppConstants.SESSION_USERNAME].ToString());
            }
            else
            {
                
            }
        }
    }
}