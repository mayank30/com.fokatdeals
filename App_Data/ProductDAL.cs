using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace com.fokatdeals
{
    public class ProductDAL : CommonDAL
    {
        
        public DataTable GetRandomProduct()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("USP_GetRandomProducts", con);
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

        public DataTable GetProduct(String value)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetProduct", con);
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