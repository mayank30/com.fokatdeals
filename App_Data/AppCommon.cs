using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Drawing;

namespace com.fokatdeals
{
    public class AppCommon
    {
        /// <summary>
        /// Validate Image from given url is valid or not
        /// </summary>
        /// <param name="url"></param>
        /// <returns value="true">image exist</returns>
        public static Boolean validateImageUrl(String url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }

        public static String randomColorCode()
        {
            Random random = new Random();
            Color c = Color.FromArgb(random.Next(200, 255), random.Next(150, 255), random.Next(150, 255));
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }
    }
}