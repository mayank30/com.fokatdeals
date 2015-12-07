using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// Summary description for PayUMoneyHandler
/// </summary>
/// 

namespace PayUMoney
{

    public class PayUMoneyHandler
    {

        // these are required parameters
        public String AMOUNT { get; set; }
        public String FIRSTNAME { get; set; }
        public String EMAIL { get; set; }
        public String PHONE { get; set; }
        public String PRODUCT_INFO { get; set; }
        public String SUCCESS_URL { get; set; }
        public String FAILURE_URL { get; set; }
        public string SERVICE_PROVIDER { get; set; }
        public String ORDER_ID { get; set; }
        public String MERCHANT_KEY { get; set; }

        // These are optional parameter
        public string lastName { get; set; }
        public string cancleUrl { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string u1 { get; set; }
        public string u2 { get; set; }
        public string u3 { get; set; }
        public string u4 { get; set; }
        public string u5 { get; set; }
        public string pg { get; set; }

        public PayUMoneyHandler() { }
        public PayUMoneyHandler(String OrderId, String Amount, String FirstName, String Email, String Phone, String ProductInfo,String SuccessUrl, String FailUrl)
        {
            MERCHANT_KEY = ConfigurationManager.AppSettings["MERCHANT_KEY"];
            ORDER_ID = OrderId;
            AMOUNT = Amount;
            FIRSTNAME = FirstName;
            EMAIL = Email;
            PHONE = Phone;
            PRODUCT_INFO = ProductInfo;
            SUCCESS_URL = SuccessUrl;
            FAILURE_URL = FailUrl;
            SERVICE_PROVIDER = "payu_paisa";
            lastName = "";
            cancleUrl = FailUrl;
            address1 = "";
            address2 = "";
            city = "";
            country = "";
            zip = "";
            state = "";
            u1 = "";
            u2 = "";
            u3 = "";
            u4 = "";
            u5 = "";
            pg = "";
            u1 = "";
        }

        public String toString()
        {
            return AMOUNT + " | " + FIRSTNAME + " | " + EMAIL + " | " + PHONE + " | " + PRODUCT_INFO + " | " + ORDER_ID;
        }

    }

    public class PayUService
    {
        public static String RequestToPayUMoney(PayUMoneyHandler payU)
        {
            try
            {
                string[] hashVarsSeq;
                string hash_string = string.Empty;
                string action1 = string.Empty;
                string hash1 = string.Empty;

                hashVarsSeq = ConfigurationManager.AppSettings["hashSequence"].Split('|'); // spliting hash sequence from config
                hash_string = "";
                hash_string = hash_string + payU.MERCHANT_KEY + "|" + payU.ORDER_ID + "|" + payU.AMOUNT + "|" + payU.PRODUCT_INFO + "|" + payU.FIRSTNAME + "|" + payU.EMAIL + "|||||||||||";
                hash_string += ConfigurationManager.AppSettings["SALT"];// appending SALT

                hash1 = AppConstant.Generatehash512(hash_string).ToLower();         //generating hash
                action1 = ConfigurationManager.AppSettings["PAYU_BASE_URL"] + "/_payment";// setting URL

                if (!string.IsNullOrEmpty(hash1))
                {
                    System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
                    data.Add(AppConstant.HASH, hash1);
                    data.Add(AppConstant.ORDER_ID, payU.ORDER_ID);
                    data.Add(AppConstant.MERCHANT_KEY, payU.MERCHANT_KEY);
                    string AmountForm = Convert.ToDecimal(payU.AMOUNT).ToString("g29");// eliminating trailing zeros
                    data.Add(AppConstant.AMOUNT, AmountForm);
                    data.Add(AppConstant.FIRST_NAME, payU.FIRSTNAME);
                    data.Add(AppConstant.EMAIL, payU.EMAIL);
                    data.Add(AppConstant.PHONE, payU.PHONE);
                    data.Add(AppConstant.PRODUCT_INFO, payU.PRODUCT_INFO.Trim());
                    data.Add(AppConstant.SUCCESS_URL, payU.SUCCESS_URL.Trim());
                    data.Add(AppConstant.FAILURE_URL, payU.FAILURE_URL.Trim());
                    data.Add(AppConstant.LAST_NAME, payU.lastName.Trim());
                    data.Add(AppConstant.CANCLE_URL, "");
                    data.Add(AppConstant.ADDRESS1, "");
                    data.Add(AppConstant.ADDRESS2, "");
                    data.Add(AppConstant.CITY, "");
                    data.Add(AppConstant.STATE, "");
                    data.Add(AppConstant.COUNTRY, "");
                    data.Add(AppConstant.ZIPCODE, "");
                    data.Add(AppConstant.EXTRA1, "");
                    data.Add(AppConstant.EXTRA2, "");
                    data.Add(AppConstant.EXTRA3, "");
                    data.Add(AppConstant.EXTRA4, "");
                    data.Add(AppConstant.EXTRA5, "");
                    data.Add(AppConstant.EXTRA6, "");
                    data.Add(AppConstant.SERVICE_PROVIDER, payU.SERVICE_PROVIDER);


                    string strForm = PreparePOSTForm(action1, data);
                    return strForm;

                }

                else
                {
                    //no hash
                    return "Data is not valid to process transaction";
                }
            }

            catch (Exception ex)
            {
                return "<span style='color:red'>" + ex.Message + "</span>";
            }
        }

        public static PayUMoneyHandler ResponseFromPayUMoney(HttpRequest request)
        {
            string[] merc_hash_vars_seq;
            PayUMoneyHandler payU = new PayUMoneyHandler();
            if (request.Form["status"] == "success")
            {
                merc_hash_vars_seq = ConfigurationManager.AppSettings["hashSequence"].Split('|');
                Array.Reverse(merc_hash_vars_seq);
                foreach (string merc_hash_var in merc_hash_vars_seq)
                {
                    if (merc_hash_var == AppConstant.PHONE)
                    {
                        payU.PHONE = request.Form[merc_hash_var];
                    }
                    else if (merc_hash_var == AppConstant.EMAIL)
                    {
                        payU.EMAIL = request.Form[merc_hash_var];
                    }
                    else if (merc_hash_var.Equals(AppConstant.FIRST_NAME))
                    {
                        payU.FIRSTNAME = request.Form[merc_hash_var];
                    }
                    else if (merc_hash_var.Equals(AppConstant.PRODUCT_INFO))
                    {
                        payU.PRODUCT_INFO = request.Form[merc_hash_var];
                    }
                    else if (merc_hash_var.Equals(AppConstant.AMOUNT))
                    {
                        payU.AMOUNT = request.Form[merc_hash_var];
                    }
                    else if (merc_hash_var.Equals(AppConstant.ORDER_ID))
                    {
                        payU.ORDER_ID = request.Form[merc_hash_var];
                    }
                }
                return payU;
            }
            else
            {
                return null;
            }
            
        }
   
        private static string PreparePOSTForm(string url, System.Collections.Hashtable data)      // post form
        {
            //Set a name for the form
            string formID = "PostForm";
            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" +
                           formID + "\" action=\"" + url +
                           "\" method=\"POST\">");

            foreach (System.Collections.DictionaryEntry key in data)
            {

                strForm.Append("<input type=\"hidden\" name=\"" + key.Key +
                               "\" value=\"" + key.Value + "\">");
            }


            strForm.Append("</form>");
            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." +
                             formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");
            //Return the form and the script concatenated.
            //(The order is important, Form then JavaScript)
            return strForm.ToString() + strScript.ToString();
        }

    }


    public class AppConstant
    {
        public static string HASH = "hash";
        public static string ORDER_ID = "txnid";
        public static string MERCHANT_KEY = "key";
        public static string AMOUNT = "amount";
        public static string FIRST_NAME = "firstname";
        public static string EMAIL = "email";
        public static string PHONE = "phone";
        public static string PRODUCT_INFO = "productinfo";
        public static string SUCCESS_URL = "surl";
        public static string FAILURE_URL = "furl";
        public static string LAST_NAME = "lastname";
        public static string CANCLE_URL = "curl";
        public static string ADDRESS1 = "address1";
        public static string ADDRESS2 = "address2";
        public static string CITY = "city";
        public static string STATE = "state";
        public static string ZIPCODE = "zipcode";
        public static string COUNTRY = "country";
        public static string EXTRA1 = "udf1";
        public static string EXTRA2 = "udf2";
        public static string EXTRA3 = "udf3";
        public static string EXTRA4 = "udf4";
        public static string EXTRA5 = "udf5";
        public static string EXTRA6 = "pg";
        public static string SERVICE_PROVIDER = "service_provider";

        public static string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;

        }

    }

}

