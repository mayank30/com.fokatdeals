using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace com.fokatdeals
{
    public class UserDAL : CommonDAL
    {

        public DataTable GetAccountUser(String username, String password)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetAccountUser", con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
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

        public int InsertAccountUser(UserModel user)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_InsertAccountUser", con);
                cmd.Parameters.AddWithValue("@username", user.username);
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@phone", user.phone);
                cmd.Parameters.AddWithValue("@password", user.password);
                cmd.Parameters.AddWithValue("@loggedSource", user.loggedSource);
                cmd.Parameters.AddWithValue("@userType", user.userType);
                cmd.Parameters.AddWithValue("@status", user.status);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                int ret = cmd.ExecuteNonQuery();
                con.Close();
                return ret;
            }
            catch
            {
                return -99;
            }
            finally
            {
                con.Close();
            }
        }


        public int ChangePassword(String username, String password)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_ChangeAccountPassword", con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                int ret = cmd.ExecuteNonQuery();
                con.Close();
                return ret;
            }
            catch
            {
                return -99;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable GetProductByPagination(int pageIndx,int size,out int nextndx)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("USP_GetProductsByPagination", con);
                cmd.Parameters.AddWithValue("@indx", pageIndx);
                cmd.Parameters.AddWithValue("@size", size);
                nextndx = pageIndx + size + 1;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch
            {
                nextndx = 50;
                return null;
            }
        }

        #region "Tracking"
        public int InsertTracking(TrackingModel user)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("USP_INSERTTRACKING", con);
                cmd.Parameters.AddWithValue("@userid", user.userid);
                cmd.Parameters.AddWithValue("@prdid", user.prdid);
                cmd.Parameters.AddWithValue("@sessionid", user.sessionid);
                cmd.Parameters.AddWithValue("@brand", user.brand);
                cmd.Parameters.AddWithValue("@status", user.status);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                int ret = cmd.ExecuteNonQuery();
                con.Close();
                return ret;
            }
            catch
            {
                return -99;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion


        public int UpdateProductImageData(String prdid,String imageUrl,String width,String height)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_UpdateProductImageData", con);
                cmd.Parameters.AddWithValue("@prdid", prdid);
                cmd.Parameters.AddWithValue("@img", imageUrl);
                cmd.Parameters.AddWithValue("@w", width);
                cmd.Parameters.AddWithValue("@h", height);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                return i;
            }
            catch
            {
                return -99;
            }
        }

        public List<string> SearchData(string search)
        {
            List<string> result = new List<string>();
            SqlCommand cmd = new SqlCommand("Select distinct top(500) '<a href=/'+subCatid+'/'+seourl+'><div>'+substring(name,0,40)+'.. at '+offerprice+'</div><div> in <b>'+subcatid+'</b> by <i>'+storeid+'</i></div></a>' as name from tbl_Products where name like '%'+@SearchText+'%'", con);
            con.Open();
            cmd.Parameters.AddWithValue("@SearchText", search);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                result.Add(dr["name"].ToString());
            }
            return result;
        }

    }
}