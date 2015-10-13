using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.fokatdeals
{
    public class UserModel : CommonModel
    {
        public String id { get; set; }
        public String username { get; set; }
        public String email { get; set; }
        public String password { get; set; }
        public String phone { get; set; }
        public String isEmailVerfied { get; set; }
        public String isPhoneVerfied { get; set; }
        public String emailOTP { get; set; }
        public String phoneOTP { get; set; }
        public String name { get; set; }
        public String dob { get; set; }
        public String profile { get; set; }
        public String loggedSource { get; set; }
        public String userType { get; set; }
        public String status { get; set; }
        public String createdDate { get; set; }
    }
}