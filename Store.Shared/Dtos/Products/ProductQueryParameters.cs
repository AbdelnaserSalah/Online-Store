using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Shared.Dtos.Products
{
    public class ProductQueryParameters
    {
        public int? brandid { get; set; }
        public int? typeid { get; set; }
        public string? sort { get; set; }
        public string? search { get; set; }
        public int pagenum { get; set; } = 1;
        public int pagesize { get; set; } = 5;
    }
}
