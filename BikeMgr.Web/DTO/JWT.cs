using System;

namespace BikeMgrWeb.DTO
{
    public class JWT
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}