using Store.Shared.Dtos.Orders;

namespace Store.Services.Abstractions.Orders
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderAddressDto ShipingAddress { get; set; }
        public string DeliveryMethod { get; set; } 
        public ICollection<OrderItemDto> Items { get; set; }
       
        public decimal Subtotal { get; set; }

        public decimal Total { get; set; }

    }
}