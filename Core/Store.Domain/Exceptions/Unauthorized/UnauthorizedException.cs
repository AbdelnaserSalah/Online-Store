using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Exceptions.Unauthorized
{
    public class UnauthorizedException(): Exception("you are not authorized Access !")
    {
    }
}
