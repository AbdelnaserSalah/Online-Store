using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Orders
{
    // table
    public class Order :BaseEntity<Guid>
    {

        public Order()
        {
            
        }
       
        public Order(string userEmail, OrderAddress shipingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> orderItems, decimal subtotal)
        {
            UserEmail = userEmail;
            ShipingAddress = shipingAddress;
            DeliveryMethod = deliveryMethod;
            Items = orderItems;
            Subtotal = subtotal;
        }

        public string UserEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }=DateTimeOffset.Now;
        public OrderStatus status { get; set; }=OrderStatus.Pending;
        public OrderAddress ShipingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } // navigation property
        public int DeliveryMethodId { get; set; } // foreign key

        public ICollection<OrderItem> Items { get; set; } // navigation property

        public decimal Subtotal { get; set; }

        public decimal GetTotal() => Subtotal + DeliveryMethod.Price; // not mapped to database, calculated property

    }
}
