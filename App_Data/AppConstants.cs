using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.fokatdeals
{
    public class AppConstants
    {
        public static int PRODUCT_PAGE_SIZE = 30;

        public static String[] EMAIL_REQUIRED_BCC = new string[] { "mayankjhawar18@gmail.com" };
        public static String[] EMAIL_REQUIRED_CC = new string[] { "fokatd@gmail.com" };
        public static String HOME_PAGE = "WebForm2.aspx";

        public static String DOMAIN_NAME = "http://localhost:8080/";

        public static String META_AUTHOR = "Mr. Mayank Jhawar";
        public static String DEFAULT_TITLE = "Welcome to India's Largest Online Shopping Center.";
        public static String DEFAULT_META_KEYWORDS = "";
        public static String DEFAULT_META_DESCRIPTION = "Welcome to fokat deals, we are providing you best deals and connect with major e-commerce brand.";

        public static String YES = "Y";
        public static String NO = "N";

        public static String ACTIVATE = "A";
        public static String DEACTIVATE = "D";

        public static String SYS_DATE = DateTime.Now.ToShortTimeString();
        //public static String UNIQUE_ID = System.Guid.NewGuid().ToString();
        //public static String GENERATED_PASSWORD = UNIQUE_ID.Replace("-","").Substring(0, 8); 

        public static String SESSION_USER_ID = "IDUserID";
        public static String SESSION_USERNAME = "34hhjkhasd";
        public static String SESSION_UNIQUE_ID = "jkashdhk1334jk";
        public static String SESSION_EMAIL_ID = "asda@sdf.com";
        public static String SESSION_USER_TYPE = "g3jkd";

        public static String EMAIL_CHANGE_PASSWORD_SUBJECT = "Change password email";

    }

    enum UserType
    {
        Admin=1000,
        Guest=1001,
        Manager=1002
    }

    enum LeadSource
    {
        Direct,
        Facebook,
        Gmail,
        LinkedIn
    }

    enum Errors
    {
        LoginError=100,
        LoginSuccess=101,

        RegisterError=200,
        RegisterSuccess=201,

        ChangePasswordError=300,
        ChangePasswordSuccess=301,

        CategoryError = 400,
        CategorySuccess = 401
    }
}