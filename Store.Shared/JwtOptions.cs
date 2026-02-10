using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Shared
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } 
        public string issuer { get; set; } 
        public string audience { get; set; } 
        public double DurationDays { get; set; }
    

    }
}
