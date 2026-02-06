using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Exceptions.NotFound
{
    public class BasketNotFoundException(string id ) :
        NotFoundExeception("Basket with {id} not found")
    {
       
    
    }
}
