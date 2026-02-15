using Store.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Specifcations.Orders
{
    public class OrderSpecification : BaseSpecifications<Guid,Order>
    {
        public OrderSpecification(Guid id,string userEmail) : base(o => o.Id == id && o.UserEmail.ToLower()==userEmail.ToLower())
        {
            Includes.Add(o => o.Items);
            Includes.Add(o => o.DeliveryMethod);
        }

            public OrderSpecification(string userEmail) : base(o => o.UserEmail.ToLower() == userEmail.ToLower())
            {
                Includes.Add(o => o.Items);
                Includes.Add(o => o.DeliveryMethod);

                AddOrderByDescending(o => o.OrderDate);
        }
    }
}
