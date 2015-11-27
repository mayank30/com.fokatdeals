using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace com.fokatdeals
{
    public class CouponDAL : CommonDAL
    {

        #region "Fetch Product Data "
        public DataTable GetCoupons(int pageIndx, int size, String value)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetCoupon", con);
                cmd.Parameters.AddWithValue("@indx", pageIndx);
                cmd.Parameters.AddWithValue("@size", size);
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
        #endregion

    }
}