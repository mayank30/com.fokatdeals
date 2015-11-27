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

        #region "Fetch Product Data "
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

        public DataTable GetProductByPagination(int pageIndx,int size,String value)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("USP_GetProductsByPagination", con);
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

        public DataTable SearchProductByPagination(int pageIndx, int size, String value)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("USP_SearchProductsByPagination", con);
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

        #region "WishList"
        public int InsertWishList(String prdid, String userid)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_InsertWishList", con);
                cmd.Parameters.AddWithValue("@prdid", prdid);
                cmd.Parameters.AddWithValue("@userid", userid);
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

        public int DeleteWishList(String prdid, String userid)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_DeleteWishList", con);
                cmd.Parameters.AddWithValue("@prdid", prdid);
                cmd.Parameters.AddWithValue("@userid", userid);
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

        public DataTable GetUserWishList(int pageIndx, int size, String value)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetWishList", con);
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


        #region "Product Category"

        public DataTable GetBaseCategory(String isDisplayAll, String status)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_GETCATEGORY", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@isAll", isDisplayAll);
                cmd.Parameters.AddWithValue("@status", status);
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

        public DataTable GetChildCategory(String category)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_GetSubCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@catid", category);
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

        public int InsertProducts(ProductModel prdModel)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_InsertProduct", con);
                cmd.Parameters.AddWithValue("@uniquId", prdModel.UniqueId);
                cmd.Parameters.AddWithValue("@prdid", prdModel.PrdId);
                cmd.Parameters.AddWithValue("@name", prdModel.Name);
                cmd.Parameters.AddWithValue("@desc", prdModel.Description);
                cmd.Parameters.AddWithValue("@prdUrl", prdModel.PrdUrl);
                cmd.Parameters.AddWithValue("@prdre", prdModel.PrdRedirectUrl);
                cmd.Parameters.AddWithValue("@img", prdModel.Img);
                cmd.Parameters.AddWithValue("@w", prdModel.Width);
                cmd.Parameters.AddWithValue("@h", prdModel.Height);
                cmd.Parameters.AddWithValue("@storeid", prdModel.Storeid);
                cmd.Parameters.AddWithValue("@reg", prdModel.RegularPrice);
                cmd.Parameters.AddWithValue("@sell", prdModel.OfferPrice);
                cmd.Parameters.AddWithValue("@catid", prdModel.SubCatId);
                cmd.Parameters.AddWithValue("@status", prdModel.Status);
                cmd.Parameters.AddWithValue("@c1", prdModel.Custom1);
                cmd.Parameters.AddWithValue("@c2", prdModel.Custom2);

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


    }
}