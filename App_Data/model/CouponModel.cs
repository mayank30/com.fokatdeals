using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.fokatdeals
{
    public class CouponModel : CommonModel
    {
        public String VoucherCodeId { get; set; }
        public String Code { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String ActivationDate { get; set; }
        public String ExpiryDate { get; set; }
        public String TrackingUrl { get; set; }
        public String CategoryName { get; set; }
        public String Status { get; set; }
        public String Addedon { get; set; }
        public String Merchant { get; set; }
        public String Product { get; set; }
        public String Type { get; set; }
        public String Discount { get; set; }


        public String ToString()
        {
            return VoucherCodeId + ", " + Code + ", " + Status + ", " + Discount + ", " + ExpiryDate;
        }
    }
}