using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikeMgrWeb.Models
{
    public class Token
    {
        public string Value { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}