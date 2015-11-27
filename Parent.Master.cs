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
                ipAddress.Value = new CommonDAL().GetIPAddress();
                offlineSession.Value = new CommonModel().UniqueId();
                if (Session[AppConstants.SESSION_UNIQUE_ID] != null)
                {
                    sessionId.Value = Session[AppConstants.SESSION_UNIQUE_ID].ToString() ;
                    offlineSession.Value = sessionId.Value;
                    sessionUser.Value = Session[AppConstants.SESSION_USERNAME].ToString();
                    sessionUserId.Value = Session[AppConstants.SESSION_USER_ID].ToString();
                    contactEmail.Value = Session[AppConstants.SESSION_EMAIL_ID].ToString();
                }
                else 
                {
                    sessionId.Value = "";
                    sessionUser.Value = "";
                    sessionUserId.Value = "";
                }
            }
            catch
            {
                Response.Redirect("sdfsdfsdf");
            }
        }
    }
}