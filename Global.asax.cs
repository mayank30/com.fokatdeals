﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;

namespace com.fokatdeals
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            URLMapping(RouteTable.Routes);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        private void URLMapping(RouteCollection routes)
        {
            routes.MapPageRoute(
             "recharge",
             "all/user/free/recharge/now",
             "~/RechargeNow.aspx"
         );

            routes.MapPageRoute(
             "coupon",
             "all/brand/free/coupon",
             "~/Coupon.aspx"
         );
            routes.MapPageRoute(
               "dashboard",
               "all/user/{dashbaord}",
               "~/Dashboard.aspx"
           );
            routes.MapPageRoute(
              "WishList",
              "all/user/wishlist/product",
              "~/WishList.aspx"
          );
            routes.MapPageRoute(
               "all-brands",
               "all/top/brands/",
               "~/AllBrand.aspx"
           );
            routes.MapPageRoute(
                "Brand",
                "{brandName}/",
                "~/Brand.aspx"
            );
            routes.MapPageRoute(
                "Category",
                "!/{categoryName}/",
                "~/Category.aspx"
            );
            routes.MapPageRoute(
                "BaseCategory",
                "!/top/{categoryName}/",
                "~/BaseCategory.aspx"
            );
            routes.MapPageRoute(
                "Product",
                "{categoryName}/{productName}/",
                "~/Product.aspx"
            );
            routes.MapPageRoute(
                "index",
                "",
                "~/Default.aspx"
            );
           

        }
    }
}