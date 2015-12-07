using Newtonsoft.Json;
using PayUMoney;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace com.fokatdeals.recharge
{
    public partial class rechargeService : System.Web.UI.Page
    {
        static RechargeHandler dal = new RechargeHandler();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static String GetOperatorCode(String mn)
        {
            try
            {
                return dal.HttpGetXML(AppConstant.GET_OPERATOR_API(mn));
            }
            catch
            {
                return null;    
            }
            
        }

        [WebMethod]
        public static String GetBalance()
        {
            try
            {
                return dal.HttpGetXML(AppConstant.GET_BALANCE_API);
            }
            catch
            {
                return null;
            }

        }

        [WebMethod]
        public static String GetAllOperatorCode()
        {
            try
            {
                return AppConstant.GET_ALL_OPERATORS();
            }
            catch
            {
                return null;
            }

        }

        [WebMethod]
        public static String GetPrepaidOperators()
        {
            try
            {
                return JsonConvert.SerializeObject(AppConstant.GET_PREPAID_OPERATORS());
            }
            catch
            {
                return null;
            }

        }

        [WebMethod]
        public static String GetPostpaidOperators()
        {
            try
            {
                return JsonConvert.SerializeObject(AppConstant.GET_POSTPAID_OPERATORS());
            }
            catch
            {
                return null;
            }

        }

        [WebMethod]
        public static String DoRecharge(String mn,String op,String amt)
        {
            try
            {
                String orderid = dal.GenerateOrderId();
                HttpContext.Current.Session[AppConstant.SESSION_ORDER_ID] = orderid;
                //Send email
                return dal.HttpGetXML(AppConstant.RECHARGE_API(mn,op,amt,orderid,""));
            }
            catch
            {
                return null;
            }

        }

        [WebMethod]
        public static String RequestPayUMoney(String mn, String op, String amt)
        {
            RechargeDAL dal = new RechargeDAL();
            try
            {
                String orderid = "REC-"+System.DateTime.Now.Ticks.ToString();
                String productInfo = amt + "/- recharge on " + mn + " mobile number for operator code "+op+" on " + System.DateTime.Now;
                String userid = HttpContext.Current.Session[AppConstants.SESSION_USER_ID].ToString();
                String username = HttpContext.Current.Session[AppConstants.SESSION_USERNAME].ToString();
                String email = HttpContext.Current.Session[AppConstants.SESSION_EMAIL_ID].ToString();
                int i = dal.InsertRecharge(orderid, userid, mn, amt, productInfo, op);
                if (i == 0 || i == -2)
                {
                    HttpContext.Current.Session[RechargeDAL.SESSION_REC_ORDER] = orderid;
                    PayUMoneyHandler p = new PayUMoneyHandler(orderid, amt, username, email, mn, productInfo, AppConstants.RECHARGE_SUCCESS_URL, AppConstants.RECHARGE_FAIL_URL);
                    return PayUService.RequestToPayUMoney(p);
                }
                else {
                    return null;
                }
            }
            catch
            {
                HttpContext.Current.Session[RechargeDAL.SESSION_AMT] = amt;
                HttpContext.Current.Session[RechargeDAL.SESSION_MN] = mn;
                HttpContext.Current.Session[RechargeDAL.SESSION_OPCODE] = op;
                return null;
            }

        }

        [WebMethod]
        public static String GetRecharge(String value)
        {
            RechargeDAL dal = new RechargeDAL();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Recharge> listOfRecharge = new List<Recharge>();
            try
            {

                DataTable dt = dal.GetRecharge(value);
                if (dt != null || dt.Rows.Count <= 1)
                {
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        Recharge recharge =  new Recharge();
                        recharge.id = Int16.Parse(dtrow["id"].ToString());
                        recharge.orderId = dtrow["orderId"].ToString();
                        recharge.userId = dtrow["userid"].ToString();
                        recharge.email = dtrow["email"].ToString();
                        recharge.phone = dtrow["phone"].ToString();
                        recharge.amount = dtrow["amount"].ToString();
                        recharge.info = dtrow["info"].ToString();
                        recharge.opCode = dtrow["opCode"].ToString();
                        recharge.status = dtrow["status"].ToString();
                        recharge.dateOfRecharge = dtrow["dateOfRecharge"].ToString();
                        recharge.discount = dtrow["discount"].ToString();
                        recharge.couponCode = dtrow["counponCode"].ToString();
                        listOfRecharge.Add(recharge);
                       
                    }
                    return ser.Serialize(listOfRecharge);
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }


    }
}