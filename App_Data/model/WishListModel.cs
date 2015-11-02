using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.fokatdeals
{
    public class WishListModel : ProductModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PrdId { get; set; }
        public string Code { get; set; }
        public string CreatedDate { get; set; }
    }
}