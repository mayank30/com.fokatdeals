using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace com.flipkart
{
    public class FlipkartProductModel
    {
        public string nextUrl { get; set; }
        public long validTill { get; set; }
        public List<ProductInfoList> productInfoList { get; set; }
        public string lastProductId { get; set; }
    }
    public class CategoryPaths
    {
        public IList<IList<Object>> categoryPath { get; set; }
    }

    public class ProductIdentifier
    {
        public string productId { get; set; }
        public CategoryPaths categoryPaths { get; set; }
    }

    public class ImageUrls
    {
        [JsonProperty("400x400")]
        public string _400x400 { get; set; }
        [JsonProperty("75x75")]
        public string _75x75 { get; set; }
        [JsonProperty("275x275")]
        public string _275x275 { get; set; }
        [JsonProperty("125x125")]
        public string _125x125 { get; set; }
        [JsonProperty("40x40")]
        public string _40x40 { get; set; }
        [JsonProperty("100x100")]
        public string _100x100 { get; set; }
        [JsonProperty("200x200")]
        public string _200x200 { get; set; }
        [JsonProperty("1100x1360")]
        public string _1100x1360 { get; set; }
        [JsonProperty("unknown")]
        public string unknown { get; set; }
        [JsonProperty("180x240")]
        public string _180x240 { get; set; }
        [JsonProperty("275x340")]
        public string _275x340 { get; set; }
        [JsonProperty("1000x1000")]
        public string _1000x1000 { get; set; }
        [JsonProperty("8x2")]
        public string _8x2 { get; set; }
    }

    public class MaximumRetailPrice
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }

    public class SellingPrice
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }

    public class ProductAttributes
    {
        public string title { get; set; }
        public string productDescription { get; set; }
        public ImageUrls imageUrls { get; set; }
        public MaximumRetailPrice maximumRetailPrice { get; set; }
        public SellingPrice sellingPrice { get; set; }
        public string productUrl { get; set; }
        public string productBrand { get; set; }
        public bool inStock { get; set; }
        public bool codAvailable { get; set; }
        public object emiAvailable { get; set; }
        public double discountPercentage { get; set; }
        public object cashBack { get; set; }
        public IList<object> offers { get; set; }
        public object size { get; set; }
        public string color { get; set; }
        public string sizeUnit { get; set; }
        public string sizeVariants { get; set; }
        public object colorVariants { get; set; }
        public string styleCode { get; set; }
    }

    public class ProductBaseInfo
    {
        public ProductIdentifier productIdentifier { get; set; }
        public ProductAttributes productAttributes { get; set; }
    }

    public class ProductShippingBaseInfo
    {
        public object shippingOptions { get; set; }
    }

    public class ProductInfoList
    {
        public ProductBaseInfo productBaseInfo { get; set; }
        public ProductShippingBaseInfo productShippingBaseInfo { get; set; }
        public object offset { get; set; }
    }

}