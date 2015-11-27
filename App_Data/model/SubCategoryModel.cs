using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.fokatdeals
{
    public class SubCategoryModel : CategoryModel
    {
        public string ParentCatId { get; set; }
    }
}