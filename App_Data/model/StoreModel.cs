using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.fokatdeals
{
    public class StoreModel : CommonModel
    {
        public string Storeid { get; set; }
        public string Storename { get; set; }
        public string SeoUrl { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string AffUrl { get; set; }
        public string AffCode { get; set; }
        public string Follower { get; set; }
        public String Rating { get; set; }
        public String Status { get; set; }
        public string Logo { get; set; }
    }
}