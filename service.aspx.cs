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
           // Register("m", "m", "m");
            //RunMe();
            //Response.Write(AppConstants.GENERATED_PASSWORD+"   <br/>"+AppConstants.UNIQUE_ID);
        }

        public void ImageDate(String imageUrl1,out String imageUrl,out string width,out String height)
        {
           
            try
            {
                WebRequest request = WebRequest.Create(imageUrl1);
                WebResponse response = request.GetResponse();
                Image image = Image.FromStream(response.GetResponseStream());
                imageUrl = imageUrl1;
                width = image.Width + "px";
                height = image.Height + "px";
            }
            catch
            {
                //Image Not Found Kindly Set Default Image in database.
                imageUrl = Properties.Settings.Default.Domain + "/images/no-product.png";
                width = "200px"; height = "300px";
            }
        }
        public void RunMe()
        {
            int count = 23546;
            int i = 2226;
            int next;
            DataTable dt = new DataTable();
            ProductDAL dal = new ProductDAL();
            //while (count > 0)
            //{
                dt = dal.GetProductByPagination(i, count, out next);
             //   count = count - next;
                foreach (DataRow dtrow in dt.Rows)
                {
                    Response.Write("Run " + dtrow["name"].ToString() + " <br>");
                    String width, height, imageurl;
                    ImageDate(dtrow["imageUrl"].ToString(), out imageurl, out width, out height);
                    dal.UpdateProductImageData(dtrow["uniqueid"].ToString(), imageurl, width, height);
                }
            //    i = next;
            //}

        }


        
        [WebMethod]
        public static String SampleTest()
        {
            String domain = Properties.Settings.Default.Domain;
            DataTable dt = new DataTable();
            ProductDAL dal = new ProductDAL();
            dt = dal.GetRandomProduct();
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
                    details.Add(obj);
                }

                JavaScriptSerializer ser = new JavaScriptSerializer();
                return ser.Serialize(details);
            }
            else {
                return "No Product Found";
            }
        }


        [WebMethod]
        public static String DisplayProduct(String value)
        {
            String domain = Properties.Settings.Default.Domain;
            String productString = "";
            DataTable dt = new DataTable();
            ProductDAL dal = new ProductDAL();
            dt = dal.GetProduct(value);
            if (dt.Rows.Count > 0)
            {
                int j = 50;
                if (dt.Rows.Count < j)
                {
                    j = dt.Rows.Count;
                }
                for (int i = 0; i < j; i++)
                {
                    string img = dt.Rows[i]["imageurl"].ToString();
                    productString = productString + "<div class='grid cs-style-3'>";
                    productString = productString + "<figure><div class='imgholder'><a href=" + domain + dt.Rows[i]["subcatid"] + "/" + dt.Rows[i]["seourl"] + " title = '" + dt.Rows[i]["name"] + " more info..'><img src='" + img + "' onerror=\"imgError(this);\"  alt= 'FokatDeals.com' /></a></div>";
                    productString = productString + "<strong><a href=" + domain + dt.Rows[i]["subcatid"] + "/" + dt.Rows[i]["seourl"] + ">" + dt.Rows[i]["name"] + "</a></strong>";
                    productString = productString + "<p><a href = " + domain + dt.Rows[i]["subcatid"] + ">in " + dt.Rows[i]["catName"] + "</a></p>";
                    productString = productString + "<div class='meta'>by <a href=" + domain + "!/" + dt.Rows[i]["storeid"] + ">" + dt.Rows[i]["storeid"] + "</a></div>";
                    productString = productString + "<figcaption><a href=" + dt.Rows[i]["prdRedirectUrl"] + " target = '_blank'>" + dt.Rows[i]["offer"] + "</a></figcaption></figure></div>";
                }
            }
            return productString;
        }


        [WebMethod]
        public string[] SearchData(string search)
        {
            List<string> result = new List<string>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand("Select distinct top(50) '<a href=/'+subCatid+'/'+seourl+'><div>'+substring(name,0,40)+'.. at '+offerprice+'</div><div> in <b>'+subcatid+'</b> by <i>'+storeid+'</i></div></a>' as html,name from tbl_Products where name like '%'+@SearchText+'%'", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@SearchText", search);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        result.Add(string.Format("{0}~{1}", dr["html"], dr["name"]));
                    }
                    return result.ToArray();
                }
            }
        }

        [WebMethod]
        public static string TopCategories()
        {
            return "<div class = 'grid' style='width:310px;height:310px; padding:0px; color:#ffffff;background:#A8A3A3; font-size:14px;'><b class='pull-left'>Top Categories</b><div class='col-md-12 column' id='cat'><div class='col-md-4 column'><div class='row'><a href='brands'>Brands</a></div><div class='row'><a href='flower-n-gifting-ideas'>Flower & Gifts</a></div><div class='row'><a href='books-and-staitionary'>Books & Staitionary</a></div><div class='row'><a href='health-and-beauty'>Health & Beauty</a></div><div class='row'><a href='mobile-and-accessories'>Mobile & Tablets</a></div><div class='row'><a href='kids-and-baby-store'>Kids & Baby</a></div></div><div class='col-md-4 column'><div class='row'><a href='sports-and-fitness'>Sports & Fitness</a></div><div class='row'><a href='clothings'>Clothing</a></div><div class='row'><a href='camera-and-accessories'>Cameras & Accessories</a></div><div class='row'><a href='household-appliance'>Household Appliance</a></div><div class='row'><a href='footwears-fashion-for-means-womens'>Footwears</a></div><div class='row'><a href='jewellery-gold-sliver-daimonds'>Jewellery</a></div></div><div class='col-md-4 column'><div class='row'><a href='computer-and-laptops'>Computers & Laptops</a></div><div class='row'><a href='home-kitchen-decor'>Home & Kitchen Decor</a></div><div class='row'><a href='fashion-accessories'>Fashion Accessories</a></div><div class='row'><a href='travels-and-hotels'>Travels & Hotels</a></div><div class='row'><a href='electronics'>Electronics</a></div><div class='row'><a href='food-products'>Food Products</a></div></div></div></div>";
        }


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
        public static String Register(String username, String password, String email,String data)
        {
            UserDAL dal = new UserDAL();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            UserModel user = new UserModel();
            try
            {
                user.username = username;
                user.password = password;
                user.email = email;
                user.loggedSource = LeadSource.Direct.ToString();
                user.userType = UserType.Guest.ToString();
                user.status = AppConstants.ACTIVATE;
                int i = dal.InsertAccountUser(user);
                if (i >= 1)
                {
                    user.errorCode = (int)Errors.RegisterSuccess;
                    user.errorMessage = "Welcome " + username;
                    setUserSession(user);
                }
                else {
                    user.errorCode = (int)Errors.RegisterError;
                    user.errorMessage = "Invalid form details";
                }
            }
            catch {
                user.errorCode = (int)Errors.RegisterError;
                user.errorMessage = "Data already Exist.";
            }
            return ser.Serialize(user);
        }


        [WebMethod]
        public static String ChangePassword(String email)
        {
            UserDAL dal = new UserDAL();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            UserModel user = new UserModel();
            try
            {
                String pass = CreatePassword(8);
                int i = dal.ChangePassword(email,pass);
                if (i >= 1)
                {
                    user.errorCode = (int)Errors.ChangePasswordSuccess;
                    user.errorMessage = "We have successfully change the password, Please check your mail for updated password.";
                    setUserSession(user);
                    Thread sendEmail = new Thread(delegate()
                    {
                        EmailUtil emailUser =  new EmailUtil();
                         String[] to = new string[] { email };
                        string[] cc = AppConstants.EMAIL_REQUIRED_CC;
                        string[] bcc = AppConstants.EMAIL_REQUIRED_BCC;
                        emailUser.SendEmail(to, cc, bcc, AppConstants.EMAIL_CHANGE_PASSWORD_SUBJECT, "Password : "+pass, "");
                    });
                    sendEmail.IsBackground = true;
                    sendEmail.Start();
                }
                else
                {
                    user.errorCode = (int)Errors.ChangePasswordError;
                    user.errorMessage = "Invalid form details";
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
            HttpContext.Current.Session[AppConstants.SESSION_USER_ID] = user.username;
            HttpContext.Current.Session[AppConstants.SESSION_EMAIL_ID] = user.email;
            HttpContext.Current.Session[AppConstants.SESSION_USER_TYPE] = user.userType;
            HttpContext.Current.Session[AppConstants.SESSION_UNIQUE_ID] = user.sessionId;
        }

    }

 
}