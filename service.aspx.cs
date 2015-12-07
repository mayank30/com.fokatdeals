using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Data;
using System.Net;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Web;
using System.Threading;

namespace com.fokatdeals
{
    public partial class service : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }
        #region "Product Display Related"

        [WebMethod]
        public static String RandomProduct()
        {
            DataTable dt = new DataTable();
            ProductDAL dal = new ProductDAL();
            dt = dal.GetRandomProduct();
            return getProductJsonIncludePage(dt, 0);
        }

        [WebMethod]
        public static String SingleProduct(String value)
        {
            DataTable dt = new DataTable();
            ProductDAL dal = new ProductDAL();
            dt = dal.GetProduct(value);
            return getProductJsonIncludePage(dt,0);
        }


        [WebMethod]
        public static String GetProductByPagination(int index,String value)
        {
            DataTable dt = new DataTable();
            ProductDAL dal = new ProductDAL();
            dt = dal.GetProductByPagination(index,AppConstants.PRODUCT_PAGE_SIZE,value);
            return getProductJsonIncludePage(dt,index);
        }

        [WebMethod]
        public static String SearchProductByPagination(int index, String value)
        {
            DataTable dt = new DataTable();
            ProductDAL dal = new ProductDAL();
            dt = dal.SearchProductByPagination(index, AppConstants.PRODUCT_PAGE_SIZE, value);
            return getProductJsonIncludePage(dt, index);
        }

        private static String getProductJsonIncludePage(DataTable dt,int index)
        {
            if (dt != null)
            {
                List<ProductModel> details = new List<ProductModel>();
                foreach (DataRow dtrow in dt.Rows)
                {
                    ProductModel obj = new ProductModel();
                    obj.PrdRedirectUrl = dtrow["prdRedirectUrl"].ToString();
                    obj.Name = dtrow["name"].ToString();
                    obj.PrdUrl = dtrow["prdUrl"].ToString();
                    obj.Img = dtrow["imageUrl"].ToString();
                    obj.SeoUrl = dtrow["seourl"].ToString();
                    obj.SubCatId = dtrow["subCatid"].ToString();
                    obj.Storeid = dtrow["storeid"].ToString();
                    obj.RegularPrice = dtrow["regularprice"].ToString();
                    obj.OfferPrice = dtrow["offerprice"].ToString();
                    obj.UniqueId = dtrow["uniqueId"].ToString();
                    obj.PrdId = dtrow["prdid"].ToString();
                    obj.pageSize = AppConstants.PRODUCT_PAGE_SIZE.ToString();
                    obj.pageNext = (index + AppConstants.PRODUCT_PAGE_SIZE + 1).ToString();
                    details.Add(obj);
                }

                JavaScriptSerializer ser = new JavaScriptSerializer();
                return ser.Serialize(details);
            }
            else
            {
                return "No Product Found";
            }
        }

        #endregion

     
        #region "User Account Related"

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static String Login(String username,String password,String data)
        {
            UserDAL dal = new UserDAL();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            UserModel user = new UserModel();
            try
            {
                DataTable dt = dal.GetAccountUser(username, password);
                if (dt != null || dt.Rows.Count <= 1)
                {
                    user.id = dt.Rows[0]["id"].ToString();
                    user.username = dt.Rows[0]["username"].ToString();
                    user.password = dt.Rows[0]["password"].ToString();
                    user.email = dt.Rows[0]["email"].ToString();
                    user.phone = dt.Rows[0]["phone"].ToString();
                    user.isEmailVerfied = dt.Rows[0]["isEmailVerfied"].ToString();
                    user.isPhoneVerfied = dt.Rows[0]["isPhoneVerfied"].ToString();
                    user.emailOTP = dt.Rows[0]["emailOTP"].ToString();
                    user.phoneOTP = dt.Rows[0]["phoneOTP"].ToString();
                    user.name = dt.Rows[0]["name"].ToString();
                    user.dob = dt.Rows[0]["dob"].ToString();
                    user.profile = dt.Rows[0]["profile"].ToString();
                    user.loggedSource = dt.Rows[0]["loggedSource"].ToString();
                    user.userType = dt.Rows[0]["userType"].ToString();
                    user.status = dt.Rows[0]["status"].ToString();
                    user.createdDate = dt.Rows[0]["createdDate"].ToString();
                    user.errorCode = (int)Errors.LoginSuccess;
                    user.errorMessage = "Success Login";
                    user.sessionId = user.UniqueId();
                    //Set User Session 
                    setUserSession(user);
                }
                else
                {
                    user.errorCode = (int)Errors.LoginError;
                    user.errorMessage = "Invalid Login Details";
                }
            }
            catch
            {
                user.errorCode = (int)Errors.LoginError;
                user.errorMessage = "Invalid Login Details";
            }
            return ser.Serialize(user);
        }

        [WebMethod]
        public static String Register(String username, String email, String phone, String data)
        {
            UserDAL dal = new UserDAL();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            UserModel user = new UserModel();
            try
            {
                user.username = username;
                user.password = CreatePassword(6);
                user.email = email;
                user.phone = phone;
                user.sessionId = user.UniqueId();
                user.loggedSource = LeadSource.Direct.ToString();
                user.userType = UserType.Guest.ToString();
                user.status = AppConstants.ACTIVATE;
                int i = dal.InsertAccountUser(user);
                if (i >= 1)
                {
                    user.id = i+"";
                    user.errorCode = (int)Errors.RegisterSuccess;
                    user.errorMessage = "Welcome " + username;
                    setUserSession(user);
                    //Send Email and Send SMS
                    EmailUtil.RegisterEmail(user.email, user.username, user.password);
                }
                else {
                    user.errorCode = (int)Errors.RegisterError;
                    user.errorMessage = "User already exist, Please provide the unique details";
                }
            }
            catch {
                user.errorCode = (int)Errors.RegisterError;
                user.errorMessage = "Data already Exist.";
            }
            return ser.Serialize(user);
        }

        [WebMethod]
        public static String ForgotPassword(String email)
        {
            UserDAL dal = new UserDAL();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            UserModel user = new UserModel();
            try
            {
                String pass = CreatePassword(6);
                int i = dal.ChangePassword(email,pass);
                if (i >= 1)
                {
                    user.errorCode = (int)Errors.ChangePasswordSuccess;
                    user.errorMessage = "We have successfully change the password, Please check your mail for updated password.";
                    setUserSession(user);
                    EmailUtil.ForgotPasswordEmail(email, "", pass);
                }
                else
                {
                    user.errorCode = (int)Errors.ChangePasswordError;
                    user.errorMessage = "Invalid Email id or Phone #";
                }
            }
            catch
            {
                user.errorCode = (int)Errors.ChangePasswordError;
                user.errorMessage = "Data already Exist.";
            }
            return ser.Serialize(user);
        }

        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        private static void setUserSession(UserModel user)
        {
            HttpContext.Current.Session[AppConstants.SESSION_UNIQUE_ID] = user.sessionId;
            HttpContext.Current.Session[AppConstants.SESSION_USER_ID] = user.id;
            HttpContext.Current.Session[AppConstants.SESSION_USERNAME] = user.username;
            HttpContext.Current.Session[AppConstants.SESSION_EMAIL_ID] = user.email;
            HttpContext.Current.Session[AppConstants.SESSION_USER_TYPE] = user.userType;
        
        }
        #endregion

        #region "Wishlist Section"
        public class WishList : CommonModel
        {
            public String id { get; set; }
            public String userid { get; set; }
            public String prdid { get; set; }
            public String createdDate { get; set; }
            public String code { get; set; }
        }
     
        [WebMethod]
        public static String AddToWishList(String prdid)
        {
            WishList wl = new WishList();
            ProductDAL dal = new ProductDAL();
            int i = dal.InsertWishList(prdid, HttpContext.Current.Session[AppConstants.SESSION_USER_ID].ToString());
            return "";
        }

        [WebMethod]
        public static String RemoveFromWishList(String prdid)
        {
            WishList wl = new WishList();
            ProductDAL dal = new ProductDAL();
            int i = dal.DeleteWishList(prdid, HttpContext.Current.Session[AppConstants.SESSION_USER_ID].ToString());
            return "";
        }

        [WebMethod]
        public static String GetUserWishList(int index)
        {
            DataTable dt = new DataTable();
            ProductDAL dal = new ProductDAL();
            dt = dal.GetUserWishList(index, AppConstants.PRODUCT_PAGE_SIZE, HttpContext.Current.Session[AppConstants.SESSION_USER_ID].ToString());
            return getProductJsonIncludePage(dt, index);
        }
        #endregion


        [WebMethod]
        public static String GetBaseCategory(String isAll)
        {
            CategoryModel obj = new CategoryModel();
            DataTable dt = new DataTable();
            ProductDAL dal = new ProductDAL();
            List<CategoryModel> details = new List<CategoryModel>();
            dt = dal.GetBaseCategory(isAll,"A");
            if (dt != null)
            {
                foreach (DataRow dtrow in dt.Rows)
                {
                    obj = new CategoryModel();
                    obj.CatId = dtrow["catid"].ToString();
                    obj.CatName = dtrow["catname"].ToString();
                    obj.Status = dtrow["catstatus"].ToString();
                    obj.CatHotStatus = dtrow["cathotstatus"].ToString();
                    obj.CatImages = dtrow["catimage"].ToString();
                    obj.CatAlias = dtrow["categoryAlias"].ToString();
                    obj.CatDesc = dtrow["catdesc"].ToString();
                    obj.CatUrl = Properties.Settings.Default.Domain + "/!/top/" + dtrow["categoryAlias"].ToString();
                    obj.errorCode = (int) Errors.CategorySuccess;
                    obj.errorMessage = "Successfully Retrival";
                    details.Add(obj);
                }


            }
            else
            {
                obj.errorCode = (int)Errors.CategoryError;
                obj.errorMessage = "Error while Retrival";
            }
            JavaScriptSerializer ser = new JavaScriptSerializer();
            return ser.Serialize(details);
        }

        [WebMethod]
        public static String GetSubCategory(String baseCategory)
        {
            SubCategoryModel obj = new SubCategoryModel();
            DataTable dt = new DataTable();
            ProductDAL dal = new ProductDAL();
            List<SubCategoryModel> details = new List<SubCategoryModel>();
            dt = dal.GetChildCategory(baseCategory);
            if (dt != null)
            {
                foreach (DataRow dtrow in dt.Rows)
                {
                    obj = new SubCategoryModel();
                    obj.CatId = dtrow["subcatid"].ToString();
                    obj.CatName = dtrow["subcatname"].ToString();
                    obj.CatDesc = dtrow["subcatdesc"].ToString();
                    obj.CatImages = dtrow["subcatimage"].ToString();
                    obj.CatTagList = dtrow["subcattaglist"].ToString();
                    obj.Status = dtrow["subcatstatus"].ToString();
                    obj.CatHotStatus = dtrow["subcathotstatus"].ToString();
                    obj.CatAlias = dtrow["scatAlias"].ToString();
                    obj.ParentCatId = dtrow["catid"].ToString();
                    obj.CatUrl = Properties.Settings.Default.Domain + "/!/" + dtrow["scatAlias"].ToString();
                    obj.errorCode = (int)Errors.CategorySuccess;
                    obj.errorMessage = "Successfully Retrival";
                    details.Add(obj);
                }


            }
            else
            {
                obj.errorCode = (int)Errors.CategoryError;
                obj.errorMessage = "Error while Retrival";
            }
            JavaScriptSerializer ser = new JavaScriptSerializer();
            return ser.Serialize(details);
        }


        #region "Affiliated Store"
        [WebMethod]
        public static String GetAffiliatedStore()
        {
            DataTable dt = new DataTable();
            StoreDAL dal = new StoreDAL();
            dt = dal.GetStore();
            if (dt != null)
            {
                List<StoreModel> details = new List<StoreModel>();
                foreach (DataRow dtrow in dt.Rows)
                {
                    StoreModel obj = new StoreModel();
                    obj.AffCode = dtrow["affCode"].ToString();
                    obj.AffUrl = dtrow["affUrl"].ToString();
                    obj.Description = dtrow["description"].ToString();
                    obj.Logo = dtrow["logo"].ToString();
                    obj.Rating = dtrow["ratings"].ToString();
                    obj.SeoUrl = dtrow["seourl"].ToString();
                    obj.Source = dtrow["source"].ToString();
                    obj.Status = dtrow["status"].ToString();
                    obj.Storeid = dtrow["storeid"].ToString();
                    obj.Storename = dtrow["storename"].ToString();
                    obj.Website = dtrow["websiteLink"].ToString();
                    obj.errorCode = 9001;
                    obj.errorMessage = "Store Found.";
                    details.Add(obj);
                }

                JavaScriptSerializer ser = new JavaScriptSerializer();
                return ser.Serialize(details);
            }
            else
            {
                return "No Store Found";
            }
        }

        #endregion

        #region "Coupon Details"

        [WebMethod]
        public static String GetCoupon(int index,String value)
        {
            CouponModel obj = new CouponModel();
            DataTable dt = new DataTable();
            CouponDAL dal = new CouponDAL();
            List<CouponModel> details = new List<CouponModel>();
            dt = dal.GetCoupons(index, AppConstants.PRODUCT_PAGE_SIZE,value);
            if (dt != null)
            {
                foreach (DataRow dtrow in dt.Rows)
                {
                    obj = new CouponModel();
                    obj.VoucherCodeId = dtrow["VoucherCodeId"].ToString();
                    obj.Code = dtrow["Code"].ToString();
                    obj.Description = dtrow["Description"].ToString();
                    obj.Title = dtrow["Title"].ToString();
                    obj.Description = dtrow["Description"].ToString();
                    obj.ActivationDate = dtrow["ActivationDate"].ToString();
                    obj.ExpiryDate = dtrow["ExpiryDate"].ToString();
                    obj.TrackingUrl = dtrow["TrackingUrl"].ToString();
                    obj.CategoryName = dtrow["CategoryName"].ToString();
                    obj.Status = dtrow["Status"].ToString();
                    obj.Addedon = dtrow["Addedon"].ToString();
                    obj.Merchant = dtrow["Merchant"].ToString();
                    obj.Product = dtrow["Product"].ToString();
                    obj.errorCode = (int)Errors.CategorySuccess;
                    obj.errorMessage = "Successfully Retrival";
                    details.Add(obj);
                }


            }
            else
            {
                obj.errorCode = (int)Errors.CategoryError;
                obj.errorMessage = "Error while Retrival";
            }
            JavaScriptSerializer ser = new JavaScriptSerializer();
            return ser.Serialize(details);
        }


        #endregion 

        #region "Contact Us"
        [WebMethod]
        public static String CreateTicket(String rply,String sub,String msg)
        {
            CommonModel user = new CommonModel();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            try
            {
                EmailUtil.CreateSupportTicket(rply, sub, msg);
                user.errorCode = (int)Errors.RegisterError;
                user.errorMessage = "Thank you, Our Support team will get in touch with you shortly.";
            }
            catch
            {
                user.errorCode = (int)Errors.RegisterError;
                user.errorMessage = "Data already Exist.";
            }
            return ser.Serialize(user);
        }
        #endregion

    }

 
}