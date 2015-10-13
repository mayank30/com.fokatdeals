using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.fokatdeals
{
    public class CommonModel
    {
        public int errorCode { get; set; }
        public String errorMessage { get; set; }
        public String sessionId { get; set; }

        public String UniqueId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}