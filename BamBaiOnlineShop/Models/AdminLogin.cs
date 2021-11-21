using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BamBaiOnlineShop.Models
{
    [Serializable]
    public class AdminLogin
    {
        public string AdminID { set; get; }
        public string AdminName { set; get; }
    }

}