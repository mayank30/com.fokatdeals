using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace com.fokatdeals
{
    public partial class Parent : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session[AppConstants.SESSION_UNIQUE_ID] != null)
                {
                    sessionId.Value = Session[AppConstants.SESSION_UNIQUE_ID].ToString() ;
                    sessionUser.Value = Session[AppConstants.SESSION_USER_ID].ToString();
                }
                else 
                {
                    sessionId.Value = "";
                    sessionUser.Value = "";
                }
            }
            catch
            {
 
            }
        }
    }
}