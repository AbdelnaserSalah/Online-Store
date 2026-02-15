using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Exceptions.NotFound
{
    public class OrderNotFoundException(Guid id):NotFoundExeception($"Order with id {id} was not found !")
    {
    }
}
