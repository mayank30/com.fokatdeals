using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.fokatdeals
{
    public class ProductModel : CommonModel
    {
        public string Id { get; set; }
        public string UniqueId { get; set; }
        public string PrdId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SeoUrl { get; set; }
        public string PrdUrl { get; set; }
        public string PrdRedirectUrl { get; set; }
        public string Img { get; set; }
        public String Width { get; set; }
        public String Height { get; set; }
        public string Storeid { get; set; }
        public string RegularPrice { get; set; }
        public string OfferPrice { get; set; }
        public string SubCatId { get; set; }
        public string CreatedOn { get; set; }
        public string ExpireOn { get; set; }
        public string Status { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
    }
}