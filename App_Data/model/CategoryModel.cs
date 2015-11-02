using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.fokatdeals
{
    public class CategoryModel : ProductModel
    {
        public string CatId { get; set; }
        public string CatName { get; set; }
        public string CatDesc { get; set; }
        public string CatImages { get; set; }
        public string CatTagList { get; set; }
        public string CatStatus { get; set; }
        public string CatHotStatus { get; set; }
        public String CatAlias { get; set; }
        public string CatUrl { get; set; }
    }
}