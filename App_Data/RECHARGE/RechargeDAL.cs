using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Xml;
namespace com.fokatdeals.recharge
{

    public class AppConstant
    {
        public static String SESSION_ORDER_ID = "jhasgd3jk";

        public static JavaScriptSerializer ser = new JavaScriptSerializer();

        private static String uid = "9407393303";
        private static String pwd = "demo@123";
        private static String API_BASE_URL = "http://api.freeonlinerechargeapi.com/apiservice.asmx/";
       
        private static String POST_ANTIVIRUS_API = API_BASE_URL + "Antivirus";

        public static String GET_BALANCE_API = API_BASE_URL + "GetBalance?uid=" + uid + "&pwd=" + pwd;

        public static String RECHARGE_API(String mobileNumber, String operatorCode, String amount, String fdOrderId, String operatorOrderId)
        {
            return API_BASE_URL + "Recharge?uid=" + uid + "&pwd=" + pwd + "&mn=" + mobileNumber + "&op=" + operatorCode + "&amt=" + amount + "&reqid=" + fdOrderId + "&Field1="+operatorOrderId;
        }

        public static String GET_OPERATOR_API(string mobileNumber)
        { 
            return API_BASE_URL + "GetOperator?mn="+mobileNumber;
        }
        public static String GET_RECHARGE_STATUS_API(string orderId)
        {
            return API_BASE_URL + "GetRechargeStatus?uid=" + uid + "&pwd=" + pwd+"&reqid="+orderId;
        }

        #region "Operator Code"

        public static List<OperatorCode> GET_PREPAID_OPERATORS()
        {
            List<OperatorCode> opCode = new List<OperatorCode>();
            opCode.Add(new OperatorCode(1,"Aircel","11","PREPAID"));
            opCode.Add(new OperatorCode(2, "Airtel", "1", "PREPAID"));
            opCode.Add(new OperatorCode(3, "BSNL", "2", "PREPAID"));
            opCode.Add(new OperatorCode(4, "Videocon", "66", "PREPAID"));
            opCode.Add(new OperatorCode(5, "Idea", "4", "PREPAID"));
            opCode.Add(new OperatorCode(6, "Loop Mobile", "7", "PREPAID"));
            opCode.Add(new OperatorCode(7, "MTNL", "8", "PREPAID"));
            opCode.Add(new OperatorCode(8, "MTS", "16", "PREPAID"));
            opCode.Add(new OperatorCode(9, "Reliance CDMA", "29", "PREPAID"));
            opCode.Add(new OperatorCode(10, "Reliance GSM", "3", "PREPAID"));
            opCode.Add(new OperatorCode(11, "T24", "90", "PREPAID"));
            opCode.Add(new OperatorCode(12, "Tata Docomo", "13", "PREPAID"));
            opCode.Add(new OperatorCode(13, "Tata Indicom", "63", "PREPAID"));
            opCode.Add(new OperatorCode(14, "Uninor", "12", "PREPAID"));
            opCode.Add(new OperatorCode(15, "Videocon", "14", "PREPAID_DEPRECATE"));
            opCode.Add(new OperatorCode(16, "Vodafone", "5", "PREPAID"));
            opCode.Add(new OperatorCode(17, "Virgin", "27", "PREPAID"));

            opCode.Add(new OperatorCode(18, "BSNL", "32", "PREPAID_SPECIAL"));
            opCode.Add(new OperatorCode(19, "Tata Docomo", "31", "PREPAID_SPECIAL"));
            opCode.Add(new OperatorCode(20, "Airtel", "1", "PREPAID_SPECIAL"));
            opCode.Add(new OperatorCode(21, "MTNL", "55", "PREPAID_SPECIAL"));
            opCode.Add(new OperatorCode(22, "T24", "96", "PREPAID_SPECIAL"));
            opCode.Add(new OperatorCode(23, "Uninor", "46", "PREPAID_SPECIAL"));
            opCode.Add(new OperatorCode(24, "Videocon", "30", "PREPAID_SPECIAL"));
            opCode.Add(new OperatorCode(25, "Virgin", "95", "PREPAID_SPECIAL"));

            return opCode;
        }

        public static List<OperatorCode> GET_POSTPAID_OPERATORS()
        {
            List<OperatorCode> opCode = new List<OperatorCode>();
            opCode.Add(new OperatorCode(26, "Aircel", "94", "POSTPAID"));
            opCode.Add(new OperatorCode(27, "Airtel", "35", "POSTPAID"));
            opCode.Add(new OperatorCode(28, "BSNL", "36", "POSTPAID"));
            opCode.Add(new OperatorCode(29, "Idea", "42", "POSTPAID"));
            opCode.Add(new OperatorCode(30, "MTS", "38", "POSTPAID"));
            opCode.Add(new OperatorCode(31, "Reliance CDMA", "26", "POSTPAID"));
            opCode.Add(new OperatorCode(32, "Reliance GSM", "34", "POSTPAID"));
            opCode.Add(new OperatorCode(33, "Tata Docomo", "43", "POSTPAID"));
            opCode.Add(new OperatorCode(34, "Tata Indicom", "37", "POSTPAID"));
            opCode.Add(new OperatorCode(35, "Vodafone", "33", "POSTPAID"));

            return opCode;
        }

        public static List<OperatorCode> GET_LANDLINE_OPERATORS()
        {
            List<OperatorCode> opCode = new List<OperatorCode>();
            opCode.Add(new OperatorCode(36, "Airtel", "94", "LANDLINE"));
            opCode.Add(new OperatorCode(37, "BSNL", "35", "LANDLINE"));
            opCode.Add(new OperatorCode(38, "Tata", "36", "LANDLINE"));
            opCode.Add(new OperatorCode(39, "RIM Net Connect", "42", "LANDLINE"));

            return opCode;
        }

        public static List<OperatorCode> GET_DTH_OPERATORS()
        {
            List<OperatorCode> opCode = new List<OperatorCode>();
            opCode.Add(new OperatorCode(40, "Airtel DTH", "22", "DTH"));
            opCode.Add(new OperatorCode(41, "Big TV", "18", "DTH"));
            opCode.Add(new OperatorCode(42, "Dish TV", "17", "DTH"));
            opCode.Add(new OperatorCode(43, "Sub Direct", "20", "DTH"));
            opCode.Add(new OperatorCode(44, "Tata Sky", "19", "DTH"));
            opCode.Add(new OperatorCode(45, "Videocon D2H", "21", "DTH"));
           
            return opCode;
        }

        public static List<OperatorCode> GET_DATA_CARD_OPERATORS()
        {
            List<OperatorCode> opCode = new List<OperatorCode>();
            opCode.Add(new OperatorCode(46, "Aircel Pocket Internet", "67", "DATA_CARD"));
            opCode.Add(new OperatorCode(47, "BSNL Data", "68", "DATA_CARD"));
            opCode.Add(new OperatorCode(48, "Idea Internet", "85", "DATA_CARD"));
            opCode.Add(new OperatorCode(49, "MTNL", "69", "DATA_CARD"));
            opCode.Add(new OperatorCode(50, "MTS MBlaze", "71", "DATA_CARD"));
            opCode.Add(new OperatorCode(51, "MTS MBrowse", "72", "DATA_CARD"));
            opCode.Add(new OperatorCode(52, "Reliance NetConnect+", "75", "DATA_CARD"));
            opCode.Add(new OperatorCode(53, "Tata Photon Whiz", "76", "DATA_CARD"));
            opCode.Add(new OperatorCode(54, "Tata Photon+", "77", "DATA_CARD"));
            opCode.Add(new OperatorCode(55, "Vodafone", "78", "DATA_CARD"));
            opCode.Add(new OperatorCode(56, "Vodafone Netcruise", "87", "DATA_CARD"));

            return opCode;
        }

        public static String GET_ALL_OPERATORS()
        {
            List<OperatorCode> opCode = new List<OperatorCode>();
            opCode.AddRange(GET_PREPAID_OPERATORS());
            opCode.AddRange(GET_POSTPAID_OPERATORS());
            opCode.AddRange(GET_DTH_OPERATORS());
            opCode.AddRange(GET_LANDLINE_OPERATORS());
            opCode.AddRange(GET_DATA_CARD_OPERATORS());

            return JsonConvert.SerializeObject(opCode);
        }

        #endregion
    }

    public class OperatorCode
    {
        public int id {get; set;}
        public String name { get; set; }
        public String code { get; set; }
        public String type { get; set; }

        public OperatorCode(int id,String name,String code,String type)
        {
            this.id = id;
            this.name = name;
            this.code = code;
            this.type = type;
        }
    }

    public class RechargeHandler
    {
        public string HttpGet(string url)
        {
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            string result = null;
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());
                result = reader.ReadToEnd();
            }
            return result;
        }

        public string HttpGetXML(string url)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(url);
            String ret= "";
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                if (node.LocalName.Equals("opid"))
                {
                    ret = node.InnerText; //or loop through its children as well
                }
            }
            return ret;
        }

        public string GenerateOrderId()
        {
            int length = 8;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }

    public class Recharge
    {
        public int id { get; set; }
        public String orderId { get; set; }
        public String userId { get; set; }
        public String email { get; set; }
        public String phone { get; set; }
        public String amount { get; set; }
        public String info { get; set; }
        public String opCode { get; set; }
        public String status { get; set; }
        public String dateOfRecharge { get; set; }
        public String discount { get; set; }
        public String couponCode { get; set; }
    }

    public class RechargeDAL : CommonDAL
    {
        public static string SESSION_MN = "234ddfds";
        public static string SESSION_AMT = "AMT8734ghjasd";
        public static string SESSION_OPCODE = "OPCODE364hkjdsfj";
        public static string SESSION_REC_ORDER = "RECORDER98324nd";

        public int InsertRecharge(String orderId, String userid,String phone,String amount,String info,String opCode)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_RechargeOrderInsert", con);
                cmd.Parameters.AddWithValue("@orderId", orderId);
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@info", info);
                cmd.Parameters.AddWithValue("@opCode", opCode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter retprm = new SqlParameter();
                retprm.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(retprm);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return Convert.ToInt32(retprm.Value); ;
            }
            catch
            {
                return -99;
            }
        }

        public DataTable GetRecharge(String value)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_RechargeOrderGet", con);
                cmd.Parameters.AddWithValue("@value", value);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
        }

    }

}