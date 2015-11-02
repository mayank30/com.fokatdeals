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

public class CommonDAL
{
    public string HttpGet(string url)
    {
        HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
        req.Headers[AffiliateConstants.FLIPKART_AFFILIATE_ID_STRING] = AffiliateConstants.FLIPKART_AFFILIATE_ID;
        req.Headers[AffiliateConstants.FLIPKART_AFFILIATE_TOKEN_STRING] = AffiliateConstants.FLIPKART_AFFILIATE_TOKEN;
        req.Headers[AffiliateConstants.SNAPDEAL_AFFILIATE_ID_STRING] = AffiliateConstants.SNAPDEAL_AFFILIATE_ID;
        req.Headers[AffiliateConstants.SNAPDEAL_AFFILIATE_TOKEN_STRING] = AffiliateConstants.SNAPDEAL_AFFILIATE_TOKEN;
        string result = null;
        using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
        {
            StreamReader reader = new StreamReader(resp.GetResponseStream());
            result = reader.ReadToEnd();
        }
        return result;
    }
}