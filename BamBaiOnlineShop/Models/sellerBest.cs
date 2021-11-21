using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BamBaiOnlineShop.Models
{
    public class sellerBest
    {
        [Key]
        public int ProID { get; set; }
        public int Qty { get; set; }
        public int categr { get; set; }
        public bool status { get; set; }
        public float DoanhThu { get; set; }
        public string ProName { get; set; }
        public string Hinh { get; set; }
    }
}
