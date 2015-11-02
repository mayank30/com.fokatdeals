using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using com.flipkart;


namespace com.fokatdeals
{
    public partial class RestCallSample : System.Web.UI.Page
    {
        ApiListings flipkartApis = new ApiListings();
        MyCategory category = new MyCategory();
        List<MyCategory> categoryList = new List<MyCategory>();
        FlipkartDAL dal = new FlipkartDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        

        protected void Button1_Click(object sender, EventArgs e)
        {
            FlipkartAPIModel api = JsonConvert.DeserializeObject<FlipkartAPIModel>(dal.HttpGet(AffiliateConstants.FLIPKART_AFFILIATE_BASE_JSON_URL));
            //category.apiName = "bags_wallets_belts";
            categoryList = dal.getCategoryList(api.apiGroups.affiliate.apiListings);
            dal.updateProducts(dal.getCategory(categoryList, ddlApi.SelectedValue.ToString()));
        }
    }

}


