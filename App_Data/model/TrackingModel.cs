using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.fokatdeals
{
    public class TrackingModel : CommonModel
    {
        public String id { get; set; }
        public String userid { get; set; }
        public String prdid { get; set; }
        public String brand { get; set; }
        public String sessionid { get; set; }
        public String status  { get; set; }
        public String trackon { get; set; }
    }
}