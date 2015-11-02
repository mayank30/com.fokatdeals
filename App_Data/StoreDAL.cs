using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace com.fokatdeals
{
    public class StoreDAL : CommonDAL
    {
    
        public DataTable GetStore()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_GETAFFILIATEDSTORES", con);
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