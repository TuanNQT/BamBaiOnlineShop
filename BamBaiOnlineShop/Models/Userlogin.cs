using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BamBaiOnlineShop.Models
{
    [Serializable]
    public class Userlogin
    {
        public int UserID { set; get; }
        public string UserName { set; get; }
    }
}